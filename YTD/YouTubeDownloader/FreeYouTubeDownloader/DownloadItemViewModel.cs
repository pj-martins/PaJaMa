// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.DownloadItemViewModel
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using BrightIdeasSoftware;
using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Converter;
using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.Downloader;
using FreeYouTubeDownloader.Downloader.Tasks;
using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Model;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  internal class DownloadItemViewModel : IPreservable
  {
    private readonly object _locker = new object();
    private ObservableCollection<DownloadItem> _downloads;
    private readonly ObjectListView _objectListView;

    internal static DownloadItemViewModel Instance { get; private set; }

    internal ObservableCollection<DownloadItem> Downloads
    {
      get
      {
        return this._downloads ?? (this._downloads = new ObservableCollection<DownloadItem>());
      }
    }

    internal ObjectListView ObjectListView
    {
      get
      {
        return this._objectListView;
      }
    }

    internal IEnumerable<DownloadItem> QueuedDownloads
    {
      get
      {
        return this.Downloads.Where<DownloadItem>((Func<DownloadItem, bool>) (download => download.State == DownloadState.InQueue));
      }
    }

    public bool NeedToSerialize
    {
      get
      {
        return true;
      }
      set
      {
        throw new System.NotSupportedException();
      }
    }

    internal DownloadItemViewModel(ObjectListView objectListView)
    {
      this._objectListView = objectListView;
      this.AddToStateManager();
    }

    internal DownloadItem GetStubDownloadItem(DownloadItem downloadItem)
    {
      DownloadItem downloadItem1 = (DownloadItem) null;
      if (this.Downloads.Count > 0)
        downloadItem1 = this.Downloads.FirstOrDefault<DownloadItem>((Func<DownloadItem, bool>) (item =>
        {
          if (item.Title.Equals(downloadItem.Title) && item.State == DownloadState.AnalizeSource && (item.SourceUrl.Equals(downloadItem.SourceUrl) && item.Size == 0L) && item.FileNameFormat.Equals(downloadItem.FileNameFormat))
            return item.VideoQualityInfo == null;
          return false;
        }));
      return downloadItem1;
    }

    internal DownloadItem GetStubDownloadItem(string title, string sourceUrl)
    {
      DownloadItem downloadItem = (DownloadItem) null;
      if (this.Downloads.Count > 0)
        downloadItem = this.Downloads.FirstOrDefault<DownloadItem>((Func<DownloadItem, bool>) (item =>
        {
          if (item.Title.Equals(title) && item.State == DownloadState.AnalizeSource && (item.SourceUrl.Equals(sourceUrl) && item.Size == 0L))
            return item.VideoQualityInfo == null;
          return false;
        }));
      return downloadItem;
    }

    private void OnDownloadsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
      if (e.Action == NotifyCollectionChangedAction.Remove)
      {
        foreach (DownloadItem oldItem in (IEnumerable) e.OldItems)
          this._objectListView.RemoveObject((object) oldItem);
        if (this.Downloads.Count == 0 && Program.NetworkMonitor.IsActive)
          Program.NetworkMonitor.StopMonitorInternetConnection();
        this.UpdateQueue();
      }
      else if (e.Action == NotifyCollectionChangedAction.Add)
      {
        foreach (DownloadItem newItem in (IEnumerable) e.NewItems)
          this._objectListView.AddObject((object) newItem);
      }
      this._objectListView.BuildGroups(this._objectListView.AllColumns.First<OLVColumn>(), SortOrder.None);
    }

    internal static void Initialize(object data, ObjectListView objectListView)
    {
      DownloadItemViewModel.Instance = new DownloadItemViewModel(objectListView);
      DownloadItemViewModel.Instance.RestoreState(data);
    }

    internal bool PutInQueue(Guid downloadItemId)
    {
      Log.Trace("CALL DownloadItemViewModel.PutInQueue(Guid)", (Exception) null);
      int num = this.Downloads.Count<DownloadItem>((Func<DownloadItem, bool>) (download =>
      {
        if (!download.IsStubDownloadItem)
          return download.IsBusy;
        return false;
      }));
      return num != 0 && Settings.Instance.AllowSimultaneousDownloads && num >= Settings.Instance.MaxSimultaneousDownloads;
    }

    internal void Add(MainWindow window, string title, string fileName, double duration, MediaInfo mediaInfo, VideoQualityInfo videoQualityInfo, ConversionProfile conversionProfile, string mediaLinkUrl = null, bool isStubDownloadItem = false)
    {
      lock (this._locker)
      {
        DownloadItem downloadItem1;
        if (isStubDownloadItem)
        {
          DownloadItem downloadItem2 = new DownloadItem();
          downloadItem2.Name = title;
          downloadItem2.Title = title;
          downloadItem2.FileName = fileName;
          string sourceUrl = mediaInfo.SourceUrl;
          downloadItem2.SourceUrl = sourceUrl;
          double num1 = duration;
          downloadItem2.Duration = num1;
          int num2 = 10;
          downloadItem2.State = (DownloadState) num2;
          downloadItem1 = downloadItem2;
        }
        else
        {
          try
          {
            downloadItem1 = new DownloadItem(title, fileName, duration, mediaInfo, videoQualityInfo, conversionProfile);
            downloadItem1.RaiseSetStatus += new EventHandler(window.SetStatus);
          }
          catch (DirectoryNotFoundException ex)
          {
            if (MessageBox.Show((IWin32Window) Program.MainWindow, Strings.DownloadFolderIsUnavailable, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
              new SettingsWindow(1).Show((IWin32Window) Program.MainWindow);
            this.Downloads.Remove(this.GetStubDownloadItem(title, mediaInfo.SourceUrl));
            Program.MainWindow.SetStatusStrip(string.Empty);
            return;
          }
        }
        DownloadItem stubDownloadItem = this.GetStubDownloadItem(downloadItem1);
        if (stubDownloadItem == null)
        {
          this.Downloads.Add(downloadItem1);
        }
        else
        {
          if (isStubDownloadItem)
            return;
          this.Downloads.Remove(stubDownloadItem);
          this.Downloads.Add(downloadItem1);
        }
      }
    }

    internal void AddConversion(MainWindow window, string inputFileName, string outputFileName, double duration)
    {
      DownloadItem downloadItem = new DownloadItem(inputFileName, outputFileName, duration);
      downloadItem.RaiseSetStatus += new EventHandler(window.SetStatus);
      this.Downloads.Add(downloadItem);
    }

    internal void Remove(DownloadItem downloadItem)
    {
      this.Downloads.Remove(downloadItem);
      if (downloadItem.State != DownloadState.Downloading && downloadItem.State != DownloadState.Paused)
        return;
      try
      {
        downloadItem.DownloadTask.Abort();
        if (downloadItem.DownloadTask is MultifileDownloadTask)
        {
          foreach (MultifileDownloadParameter downloadParameter in (downloadItem.DownloadTask as MultifileDownloadTask).MultifileDownloadParameters)
            FileSystemUtil.SafeDeleteFile(downloadParameter.FileName);
        }
        else
          FileSystemUtil.SafeDeleteFile(downloadItem.FileName);
      }
      catch (Exception ex)
      {
        Log.Error("DownloadItemViewModel.Remove(DownloadItem) => " + ex.Message, (Exception) null);
      }
    }

    internal void Delete(DownloadItem downloadItem)
    {
      this.Downloads.Remove(downloadItem);
      FileSystemUtil.SafeDeleteFile(downloadItem.FileName);
    }

    internal void CancelConversion(DownloadItem downloadItem)
    {
      if (downloadItem.State != DownloadState.Converting || downloadItem.ConversionTask == null)
        return;
      this.Downloads.Remove(downloadItem);
      downloadItem.ConversionTask.Cancel();
    }

    internal void UpdateQueue()
    {
      foreach (DownloadItem queuedDownload in this.QueuedDownloads)
        queuedDownload.TriggerDownloadingSinceQueueReleased();
    }

    public void AddToStateManager()
    {
      ApplicationStateManager.Instance.AddObject((IPreservable) this);
    }

    public void RestoreState()
    {
    }

    public void RestoreState(object data)
    {
      this.Downloads.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnDownloadsCollectionChanged);
      if (!File.Exists(SharedConstants.SerializedDownloadsFileName))
        return;
      try
      {
        using (FileStream fileStream = new FileStream(SharedConstants.SerializedDownloadsFileName, FileMode.Open, FileAccess.Read))
        {
          using (StreamReader streamReader = new StreamReader((Stream) fileStream))
          {
            using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader) streamReader))
            {
label_12:
              while (jsonTextReader.Read())
              {
                if (jsonTextReader.TokenType == JsonToken.PropertyName && (string) jsonTextReader.Value == "Downloads")
                {
                  while (true)
                  {
                    DownloadItem instance;
                    do
                    {
                      do
                      {
                        if (!jsonTextReader.Read() || jsonTextReader.TokenType == JsonToken.EndArray)
                          goto label_12;
                      }
                      while (jsonTextReader.TokenType != JsonToken.PropertyName || !((string) jsonTextReader.Value == "$type"));
                      instance = (DownloadItem) Activator.CreateInstance(Type.GetType(jsonTextReader.ReadAsString()));
                      MainWindow mainWindow = data as MainWindow;
                      if (mainWindow != null)
                        instance.RaiseSetStatus += new EventHandler(mainWindow.SetStatus);
                      instance.ReadJson(jsonTextReader);
                    }
                    while (!instance.UpdateAfterDeserialization());
                    this.Downloads.Add(instance);
                  }
                }
              }
            }
          }
        }
      }
      catch
      {
      }
    }

    public void SaveState()
    {
      try
      {
        using (FileStream fileStream = new FileStream(SharedConstants.SerializedDownloadsFileName, FileMode.Create, FileAccess.Write))
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fileStream))
          {
            using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter) streamWriter))
            {
              jsonTextWriter.WriteStartObject();
              jsonTextWriter.WritePropertyName("Downloads");
              jsonTextWriter.WriteStartArray();
              lock (this._locker)
              {
                foreach (DownloadItem downloadItem in this.Downloads.Where<DownloadItem>((Func<DownloadItem, bool>) (d =>
                {
                  if (Settings.Instance.RemoveAllFinishedFiles)
                    return d.State != DownloadState.Completed;
                  return true;
                })))
                {
                  if (downloadItem.State != DownloadState.AnalizeSource && downloadItem.State != DownloadState.Error)
                    downloadItem.WriteJson(jsonTextWriter);
                }
              }
              jsonTextWriter.WriteEndArray();
              jsonTextWriter.WriteEndObject();
            }
          }
        }
      }
      catch
      {
      }
    }
  }
}
