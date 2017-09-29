// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Tasks.DownloadTask
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FreeYouTubeDownloader.Downloader.Tasks
{
  public abstract class DownloadTask : IDownloadTask, IJsonSerializable
  {
    public abstract long BytesDownloaded { get; }

    internal MediaLink Link { get; set; }

    public string SourceUrl { get; protected set; }

    public string FileName { get; protected set; }

    public int Progress { get; protected set; }

    public long DownloadSpeed { get; protected set; }

    public long SecondsRemains { get; protected set; }

    public virtual long BytesTotal { get; protected set; }

    public bool IsUsed { get; set; }

    public virtual bool IsCompleted
    {
      get
      {
        if (this.BytesDownloaded >= this.BytesTotal && this.BytesTotal > 0L)
          return File.Exists(this.FileName);
        return false;
      }
    }

    public bool PausedByNetworkState { get; set; }

    public string Id { get; protected set; }

    public event DownloadTask.DownloadFinishedEventHandler DownloadFinished;

    public event DownloadTask.DownloadProgressEventHandler DownloadProgress;

    public event DownloadTask.DownloadErrorEventHandler DownloadError;

    public event DownloadTask.DownloadStateChangedEventHandler DownloadStateChanged;

    public abstract void Download(MediaLink link, string downloadedMediaFileLocation, long range = -1);

    public abstract void MultifileDownload(string downloadedMediaFileLocation, List<MultifileDownloadParameter> multifileDownloadParameters);

    public abstract void Abort();

    public abstract void Pause();

    public abstract void Resume();

    public abstract void ReadJson(JsonTextReader jsonTextReader);

    public abstract void WriteJson(JsonTextWriter jsonTextWriter);

    public virtual void UpdateAfterDeserialization()
    {
    }

    protected void NotifyDownloadProgress(long bytesDownloaded, long bytesTotal, long downloadSpeed, long secondsRemains)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.DownloadProgress == null)
        return;
      DownloadProgressEventArgs e = new DownloadProgressEventArgs(bytesDownloaded, bytesTotal, downloadSpeed, secondsRemains);
      this.Progress = e.ProgressInPercent;
      this.BytesTotal = e.BytesTotal;
      this.SecondsRemains = e.SecondsRemains;
      this.DownloadSpeed = e.DownloadSpeed;
      // ISSUE: reference to a compiler-generated field
      this.DownloadProgress((object) this, e);
    }

    protected void NotifyDownloadProgress(DownloadProgressEventArgs args)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.DownloadProgress == null)
        return;
      this.Progress = args.ProgressInPercent;
      this.BytesTotal = args.BytesTotal;
      this.SecondsRemains = args.SecondsRemains;
      this.DownloadSpeed = args.DownloadSpeed;
      // ISSUE: reference to a compiler-generated field
      this.DownloadProgress((object) this, args);
    }

    protected void NotifyDownloadStateChanged(DownloadState e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.DownloadStateChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.DownloadStateChanged((object) this, e);
    }

    protected void NotifyDownloadError(DownloadErrorEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.DownloadError == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.DownloadError((object) this, e);
    }

    protected void NotifyDownloadFinished(DownloadFinishedEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.DownloadFinished == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.DownloadFinished((object) this, e);
    }

    protected void Cleanup(params IDisposable[] disposables)
    {
      foreach (IDisposable disposable in disposables)
      {
        try
        {
          disposable.Dispose();
        }
        catch (ObjectDisposedException ex)
        {
        }
      }
    }

    public delegate void DownloadFinishedEventHandler(object sender, DownloadFinishedEventArgs e);

    public delegate void DownloadProgressEventHandler(object sender, DownloadProgressEventArgs e);

    public delegate void DownloadErrorEventHandler(object sender, DownloadErrorEventArgs e);

    public delegate void DownloadStateChangedEventHandler(object sender, DownloadState state);
  }
}
