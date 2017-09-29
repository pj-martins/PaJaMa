// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Model.DownloadItem
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Common.Reporting.Freshdesk;
using FreeYouTubeDownloader.Converter;
using FreeYouTubeDownloader.Downloader;
using FreeYouTubeDownloader.Downloader.Tasks;
using FreeYouTubeDownloader.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.Model
{
    public class DownloadItem : IJsonSerializable, INotifyPropertyChanged
    {
        private Guid _id = Guid.Empty;
        private readonly object _locker = new object();
        private string _name;
        private string _title;
        private int _progress;
        private long _size;
        private double _duration;
        private long _speed;
        private long _secondsRemains;
        private string _frameSize;
        private string _fileName;
        private DownloadState _state;

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public string Title
        {
            get
            {
                return this._title;
            }
            set
            {
                this._title = value;
                this.RaisePropertyChanged("Title");
            }
        }

        public int Progress
        {
            get
            {
                return this._progress;
            }
            private set
            {
                this._progress = value;
                this.RaisePropertyChanged("Progress");
            }
        }

        public long Size
        {
            get
            {
                return this._size;
            }
            private set
            {
                this._size = value;
                this.RaisePropertyChanged("Size");
            }
        }

        public double Duration
        {
            get
            {
                return this._duration;
            }
            set
            {
                this._duration = value;
                this.RaisePropertyChanged("Duration");
            }
        }

        public long Speed
        {
            get
            {
                return this._speed;
            }
            private set
            {
                this._speed = value;
                this.RaisePropertyChanged("Speed");
            }
        }

        public long SecondsRemains
        {
            get
            {
                return this._secondsRemains;
            }
            private set
            {
                this._secondsRemains = value;
                this.RaisePropertyChanged("SecondsRemains");
            }
        }

        public string FrameSize
        {
            get
            {
                return this._frameSize;
            }
            private set
            {
                this._frameSize = value;
                this.RaisePropertyChanged("FrameSize");
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
                this.RaisePropertyChanged("FileName");
            }
        }

        public string SourceUrl { get; set; }

        internal DownloadState State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                this.RaisePropertyChanged("State");
            }
        }

        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if (this._id != Guid.Empty || this._id == value)
                    return;
                this._id = value;
            }
        }

        internal DownloadTask DownloadTask { get; private set; }

        internal ConversionProfile ConversionProfile { get; private set; }

        internal ConversionTask ConversionTask { get; private set; }

        internal VideoQualityInfo VideoQualityInfo { get; private set; }

        internal DateTime TimeStamp { get; private set; }

        public bool IsBusy
        {
            get
            {
                if (this.State != DownloadState.AnalizeSource && this.State != DownloadState.Converting && (this.State != DownloadState.Downloading && this.State != DownloadState.Merging) && this.State != DownloadState.Paused)
                    return this.State == DownloadState.Preparing;
                return true;
            }
        }

        public bool IsStubDownloadItem
        {
            get
            {
                return this.Id == Guid.Empty;
            }
        }

        public bool AudioFile { get; set; }

        public bool FileExists
        {
            get
            {
                return System.IO.File.Exists(this.FileName);
            }
        }

        public string FileNameFormat
        {
            get
            {
                if (this.ConversionProfile != null)
                    return this.ConversionProfile.FormatName;
                return Path.GetExtension(this.FileName).TrimStart('.').ToUpperInvariant();
            }
        }

        public event EventHandler RaiseSetStatus;

        public event PropertyChangedEventHandler PropertyChanged;

        public DownloadItem()
        {
        }

        internal DownloadItem(string title, string targetFileName, double duration, MediaInfo mediaInfo, VideoQualityInfo videoQualityInfo, ConversionProfile conversionProfile)
          : this()
        {
            this.VideoQualityInfo = videoQualityInfo;
            this.Id = Guid.NewGuid();
            this.State = DownloadState.Preparing;
            this.SourceUrl = mediaInfo.SourceUrl;
            this.Title = title;
            this.Size = videoQualityInfo.Size;
            this.Duration = duration;
            lock (this._locker)
                targetFileName = this.SetFileName(targetFileName);
            if (videoQualityInfo.File is AudioLink && conversionProfile == null)
                conversionProfile = (ConversionProfile)new YouTubeDashAudioNormalizer();
            if (conversionProfile != null)
            {
                this.ConversionProfile = conversionProfile;
                this.ConversionProfile.OutputFileName = targetFileName;
            }
            this.FileName = this.ConversionProfile == null ? targetFileName : FileSystemUtil.GetTempFileName(videoQualityInfo.FormatName.ToLowerInvariant(), (string)null);
            VideoLink file = videoQualityInfo.File as VideoLink;
            if (file != null && file.HasDimension)
                this.FrameSize = file.Dimension;
            if (file != null && file.IsDash)
            {
                this.DownloadTask = (DownloadTask)new MultifileDownloadTask();
                file.AudioLink = mediaInfo.Links.GetAudioLink(Settings.Instance.DesiredDownloadAudioQuality, file.StreamType.GetPreferredAudioStreamTypes());
            }
            else
                this.DownloadTask = (DownloadTask)new SinglefileDownloadTask();
            this.DownloadTask.DownloadProgress += new DownloadTask.DownloadProgressEventHandler(this.OnDownloadProgress);
            this.DownloadTask.DownloadFinished += new DownloadTask.DownloadFinishedEventHandler(this.OnDownloadFinished);
            this.DownloadTask.DownloadError += new DownloadTask.DownloadErrorEventHandler(this.OnDownloadError);
            this.DownloadTask.DownloadStateChanged += new DownloadTask.DownloadStateChangedEventHandler(this.OnDownloadStateChanged);
            if (DownloadItemViewModel.Instance.PutInQueue(this.Id))
                this.State = DownloadState.InQueue;
            else if (this.DownloadTask is SinglefileDownloadTask)
                this.DownloadTask.Download(videoQualityInfo.File, this.FileName, -1L);
            else
                this.DownloadTask.MultifileDownload(this.FileName, new List<MultifileDownloadParameter>()
        {
          new MultifileDownloadParameter((MediaLink) file, FileSystemUtil.GetTempFileName((string) null, Settings.Instance.DefaultVideosDownloadFolder), -1),
          new MultifileDownloadParameter((MediaLink) file.AudioLink, FileSystemUtil.GetTempFileName((string) null, Settings.Instance.DefaultVideosDownloadFolder), -1)
        });
            this.TimeStamp = DateTime.Now;
        }

        internal DownloadItem(string inputFileName, string outputFileName, double duration)
          : this()
        {
            this.Title = Path.GetFileNameWithoutExtension(inputFileName);
            this.Size = new FileInfo(inputFileName).Length;
            this.Duration = duration;
            lock (this._locker)
                this.FileName = this.SetFileName(outputFileName);
            this.ConversionProfile = ConversionProfile.ConversionProfiles.Single<ConversionProfile>((Func<ConversionProfile, bool>)(profile => string.Equals(profile.FormatName.ToLowerInvariant(), Path.GetExtension(this.FileName).TrimStart('.'))));
            this.ConversionProfile.DeleteInputFile = false;
            this.ConversionProfile.OutputFileName = this.FileName;
            this.AudioFile = this.ConversionProfile.OutputFileName.IsFileAudioFormat();
            if (this.AudioFile)
            {
                this.ConversionProfile.VideoScale = (string)null;
                this.ConversionProfile.AudioBitRate = string.Format("{0}k", (object)Settings.Instance.DesiredDownloadAudioQuality);
            }
            else
            {
                this.ConversionProfile.AudioBitRate = (string)null;
                this.ConversionProfile.VideoScale = Settings.Instance.DesiredDownloadVideoQuality.GetFrameHeight().ToString((IFormatProvider)CultureInfo.InvariantCulture);
            }
            this.TimeStamp = DateTime.Now;
            this.ConversionTask = (ConversionTask)new FfmpegWrapper();
            this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
            this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
            this.ConversionTask.Convert(inputFileName, this.ConversionProfile, (VideoQualityInfo)null);
            this.State = DownloadState.Converting;
        }

        public void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string s = (string)jsonTextReader.Value;
                    int? nullable;
                    if (true && s == "Name")// if ((int) stringHash == 266367750 && s == "Name")
                        this.Name = jsonTextReader.ReadAsString();
                    else if (s == "Speed")
                    {
                        jsonTextReader.Read();
                        this.Speed = (long)jsonTextReader.Value;
                    }
                    if (true && s == "Progress")// if ((int) stringHash == 439787878 && s == "Progress")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.Progress = nullable.GetValueOrDefault();
                    }
                    else if (s == "SecondsRemains")
                    {
                        jsonTextReader.Read();
                        this.SecondsRemains = (long)jsonTextReader.Value;
                    }
                    if (true && s == "Title")// if ((int) stringHash == 617902505 && s == "Title")
                        this.Title = jsonTextReader.ReadAsString();
                    else if (s == "State")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.State = (DownloadState)nullable.GetValueOrDefault();
                    }
                    if (true && s == "FileName")// if ((int) stringHash == 1610471560 && s == "FileName")
                        this.FileName = jsonTextReader.ReadAsString();
                    else if (s == "Id")
                        this._id = Guid.Parse(jsonTextReader.ReadAsString());
                    if (true && s == "DownloadTask")// if ((int) stringHash == -2027211912 && s == "DownloadTask")
                    {
                        jsonTextReader.Read();
                        jsonTextReader.Read();
                        this.DownloadTask = (DownloadTask)Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Downloader").GetType(jsonTextReader.ReadAsString()));
                        this.DownloadTask.ReadJson(jsonTextReader);
                    }
                    else if (s == "FrameSize")
                    {
                        jsonTextReader.Read();
                        this.FrameSize = (string)jsonTextReader.Value;
                    }
                    if (true && s == "ConversionProfile")// if ((int) stringHash == -1510499410 && s == "ConversionProfile")
                    {
                        jsonTextReader.Read();
                        jsonTextReader.Read();
                        this.ConversionProfile = (ConversionProfile)Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Converter").GetType(jsonTextReader.ReadAsString()));
                        this.ConversionProfile.ReadJson(jsonTextReader);
                    }
                    else if (s == "VideoQualityInfo")
                    {
                        jsonTextReader.Read();
                        jsonTextReader.Read();
                        this.VideoQualityInfo = (VideoQualityInfo)Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Analyzer").GetType(jsonTextReader.ReadAsString()));
                        this.VideoQualityInfo.ReadJson(jsonTextReader);
                    }
                    if (true && s == "TimeStamp")// if ((int) stringHash == -1239535261 && s == "TimeStamp")
                    {
                        jsonTextReader.Read();
                        this.TimeStamp = new DateTime((long)jsonTextReader.Value);
                    }
                    else if (s == "Size")
                    {
                        jsonTextReader.Read();
                        this.Size = (long)jsonTextReader.Value;
                    }
                    if (true && s == "Duration")// if ((int) stringHash == -725137107 && s == "Duration")
                    {
                        jsonTextReader.Read();
                        this.Duration = (double)(long)jsonTextReader.Value;
                    }
                    else if (s == "SourceUrl")
                        this.SourceUrl = jsonTextReader.ReadAsString();
                }
            }
        }

        public void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("Name");
            jsonTextWriter.WriteValue(this.Name);
            jsonTextWriter.WritePropertyName("Title");
            jsonTextWriter.WriteValue(this.Title);
            jsonTextWriter.WritePropertyName("Progress");
            jsonTextWriter.WriteValue(this.Progress);
            jsonTextWriter.WritePropertyName("Size");
            jsonTextWriter.WriteValue(this.Size);
            jsonTextWriter.WritePropertyName("Duration");
            jsonTextWriter.WriteValue(this.Duration);
            jsonTextWriter.WritePropertyName("Speed");
            jsonTextWriter.WriteValue(this.Speed);
            jsonTextWriter.WritePropertyName("SecondsRemains");
            jsonTextWriter.WriteValue(this.SecondsRemains);
            jsonTextWriter.WritePropertyName("FrameSize");
            jsonTextWriter.WriteValue(this.FrameSize);
            jsonTextWriter.WritePropertyName("State");
            jsonTextWriter.WriteValue((int)this.State);
            jsonTextWriter.WritePropertyName("FileName");
            jsonTextWriter.WriteValue(this.FileName);
            if (this.DownloadTask != null)
            {
                jsonTextWriter.WritePropertyName("DownloadTask");
                this.DownloadTask.WriteJson(jsonTextWriter);
            }
            jsonTextWriter.WritePropertyName("SourceUrl");
            jsonTextWriter.WriteValue(this.SourceUrl);
            if (this.ConversionProfile != null)
            {
                jsonTextWriter.WritePropertyName("ConversionProfile");
                this.ConversionProfile.WriteJson(jsonTextWriter);
            }
            if (this.VideoQualityInfo != null)
            {
                jsonTextWriter.WritePropertyName("VideoQualityInfo");
                this.VideoQualityInfo.WriteJson(jsonTextWriter);
            }
            jsonTextWriter.WritePropertyName("TimeStamp");
            jsonTextWriter.WriteValue(this.TimeStamp.Ticks);
            if (this.Id != Guid.Empty)
            {
                jsonTextWriter.WritePropertyName("Id");
                jsonTextWriter.WriteValue(this._id.ToString());
            }
            jsonTextWriter.WriteEndObject();
        }

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            try
            {
                // ISSUE: reference to a compiler-generated field
                if (this.PropertyChanged == null)
                    return;
                // ISSUE: reference to a compiler-generated field
                this.PropertyChanged((object)this, new PropertyChangedEventArgs(propertyName));
            }
            catch (ObjectDisposedException ex)
            {
            }
        }

        public bool UpdateAfterDeserialization()
        {
            if (this.State == DownloadState.Converting)
            {
                MergeProfile conversionProfile = this.ConversionProfile as MergeProfile;
                if (conversionProfile != null && (!System.IO.File.Exists(conversionProfile.MergeParameters.Input[0]) || !System.IO.File.Exists(conversionProfile.MergeParameters.Input[1])))
                    return false;
                this.ConversionTask = (ConversionTask)new FfmpegWrapper();
                this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
                this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
                this.ConversionTask.Convert(this.FileName, this.ConversionProfile, this.VideoQualityInfo);
                return true;
            }
            if (this.DownloadTask == null)
                return true;
            this.DownloadTask.DownloadProgress += new DownloadTask.DownloadProgressEventHandler(this.OnDownloadProgress);
            this.DownloadTask.DownloadFinished += new DownloadTask.DownloadFinishedEventHandler(this.OnDownloadFinished);
            this.DownloadTask.DownloadError += new DownloadTask.DownloadErrorEventHandler(this.OnDownloadError);
            this.DownloadTask.DownloadStateChanged += new DownloadTask.DownloadStateChangedEventHandler(this.OnDownloadStateChanged);
            if (this.DownloadTask.IsCompleted && !this.FileExists)
            {
                this.State = DownloadState.Error;
                return true;
            }
            if (this.Progress >= 100)
                return true;
            switch (this.State)
            {
                case DownloadState.Preparing:
                case DownloadState.Waiting:
                    if (this.DownloadTask is SinglefileDownloadTask)
                    {
                        this.DownloadTask.Download(this.VideoQualityInfo.File, this.FileName, -1L);
                        break;
                    }
                    this.DownloadTask.MultifileDownload(this.FileName, new List<MultifileDownloadParameter>()
          {
            new MultifileDownloadParameter(this.VideoQualityInfo.File, FileSystemUtil.GetTempFileName((string) null, Settings.Instance.DefaultVideosDownloadFolder), -1),
            new MultifileDownloadParameter((MediaLink) ((VideoLink) this.VideoQualityInfo.File).AudioLink, FileSystemUtil.GetTempFileName((string) null, Settings.Instance.DefaultVideosDownloadFolder), -1)
          });
                    break;
                case DownloadState.Downloading:
                    this.DownloadTask.UpdateAfterDeserialization();
                    break;
            }
            return true;
        }

        public void TriggerDownloadingSinceQueueReleased()
        {
            if (DownloadItemViewModel.Instance.PutInQueue(this.Id))
                return;
            this.State = DownloadState.Preparing;
            if (this.DownloadTask is SinglefileDownloadTask)
            {
                this.DownloadTask.Download(this.VideoQualityInfo.File, this.FileName, this.DownloadTask.BytesDownloaded);
            }
            else
            {
                MultifileDownloadTask downloadTask = this.DownloadTask as MultifileDownloadTask;
                if (downloadTask.SinglefileDownloads != null && downloadTask.SinglefileDownloads.Count > 0)
                    downloadTask.Resume();
                else
                    downloadTask.MultifileDownload(this.FileName, new List<MultifileDownloadParameter>()
          {
            new MultifileDownloadParameter(this.VideoQualityInfo.File, FileSystemUtil.GetTempFileName((string) null, Settings.Instance.DefaultVideosDownloadFolder), -1),
            new MultifileDownloadParameter((MediaLink) ((VideoLink) this.VideoQualityInfo.File).AudioLink, FileSystemUtil.GetTempFileName((string) null, Settings.Instance.DefaultVideosDownloadFolder), -1)
          });
            }
        }

        internal string GetDownloadStatus()
        {
            switch (this.State)
            {
                case DownloadState.Preparing:
                    return Strings.Preparing;
                case DownloadState.Downloading:
                    return Strings.Downloading;
                case DownloadState.Completed:
                    if (!this.FileExists)
                        return Strings.FileMissingOnComputer;
                    return Strings.Completed;
                case DownloadState.Error:
                    if (!this.FileExists)
                        return Strings.FileMissingOnComputer;
                    return Strings.Error;
                case DownloadState.InQueue:
                    return Strings.InQueue;
                case DownloadState.Paused:
                    return Strings.Paused;
                case DownloadState.Converting:
                    return Strings.Converting;
                case DownloadState.AnalizeSource:
                    return Strings.AnalyzingStreamsQuality;
                default:
                    return this.State.ToString();
            }
        }

        private void OnDownloadProgress(object sender, DownloadProgressEventArgs args)
        {
            this.Progress = args.ProgressInPercent;
            this.Speed = args.DownloadSpeed;
            this.SecondsRemains = args.SecondsRemains;
            this.State = DownloadState.Downloading;
            // ISSUE: reference to a compiler-generated field
            EventHandler raiseSetStatus = this.RaiseSetStatus;
            if (raiseSetStatus == null)
                return;
            // ISSUE: variable of the null type
            EventArgs local = null;
            raiseSetStatus((object)this, (EventArgs)local);
        }

        private void OnDownloadFinished(object sender, DownloadFinishedEventArgs args)
        {
            MultifileDownloadTask multifileDownloadTask = sender as MultifileDownloadTask;
            if (multifileDownloadTask != null)
            {
                List<string> list = multifileDownloadTask.MultifileDownloadParameters.Select<MultifileDownloadParameter, string>((Func<MultifileDownloadParameter, string>)(parameter => parameter.FileName)).ToList<string>();
                this.ConversionTask = (ConversionTask)new FfmpegWrapper();
                this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
                this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
                FileMergeTaskParameters mergeTaskParameters = new FileMergeTaskParameters()
                {
                    Input = list.ToArray(),
                    Output = multifileDownloadTask.FileName
                };
                ConversionProfile conversionProfile1 = this.ConversionProfile;
                MergeProfile mergeProfile = new MergeProfile();
                mergeProfile.MergeParameters = mergeTaskParameters;
                int num = 0;
                mergeProfile.DeleteInputFile = num != 0;
                ConversionProfile conversionProfile2 = conversionProfile1;
                mergeProfile.OriginalProfile = conversionProfile2;
                this.ConversionProfile = (ConversionProfile)mergeProfile;
                this.ConversionTask.Convert(multifileDownloadTask.FileName, this.ConversionProfile, this.VideoQualityInfo);
                this.State = DownloadState.Merging;
            }
            else
            {
                this.Progress = 100;
                this.Speed = 0L;
                this.SecondsRemains = 0L;
                this.State = DownloadState.Completed;
                this.TimeStamp = DateTime.Now;
                this.Size = new FileInfo(this.FileName).Length;
                if (this.ConversionProfile != null)
                {
                    if (!this.ConversionProfile.Equals((object)Path.GetExtension(this.FileName).TrimStart('.')))
                    {
                        this.ConversionTask = (ConversionTask)new FfmpegWrapper();
                        this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
                        this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
                        this.ConversionTask.Convert(this.DownloadTask.FileName, this.ConversionProfile, this.VideoQualityInfo);
                        this.State = DownloadState.Converting;
                        goto label_7;
                    }
                }
                if (this.VideoQualityInfo.File is AudioLink && this.ConversionProfile.GetType() == typeof(YouTubeDashAudioNormalizer))
                {
                    this.ConversionTask = (ConversionTask)new FfmpegWrapper();
                    this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
                    this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
                    this.ConversionTask.Convert(this.DownloadTask.FileName, this.ConversionProfile, this.VideoQualityInfo);
                    this.State = DownloadState.Converting;
                }
                label_7:
                DownloadItemViewModel.Instance.ObjectListView.BuildList();
                DownloadItemViewModel.Instance.UpdateQueue();
                // ISSUE: reference to a compiler-generated field
                EventHandler raiseSetStatus = this.RaiseSetStatus;
                if (raiseSetStatus != null)
                {
                    DownloadStateEventArgs downloadStateEventArgs = new DownloadStateEventArgs(DownloadState.Downloading);
                    raiseSetStatus((object)this, (EventArgs)downloadStateEventArgs);
                }
            }
        }

        private void OnDownloadError(object sender, DownloadErrorEventArgs args)
        {
            if (!Program.NetworkMonitor.Connected)
            {
                DownloadTask downloadTask = (DownloadTask)sender;
                downloadTask.Pause();
                int num = 1;
                downloadTask.PausedByNetworkState = num != 0;
            }
            else
            {
                this.State = DownloadState.Error;
                this.TimeStamp = DateTime.Now;
                DownloadItemViewModel.Instance.UpdateQueue();
                // ISSUE: reference to a compiler-generated field
                EventHandler raiseSetStatus = this.RaiseSetStatus;
                if (raiseSetStatus != null)
                {
                    // ISSUE: variable of the null type
                    EventArgs local = null;
                    raiseSetStatus((object)this, (EventArgs)local);
                }
                StringBuilder stringBuilder = new StringBuilder();
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                string str1 = string.Empty;
                stringBuilder.AppendLine(string.Format(Strings.VideoCouldNotBeDownloaded, (object)this.Title));
                Exception exception = args.Exception;
                if (!string.IsNullOrEmpty(exception != null ? exception.Message : (string)null))
                {
                    str1 = Strings.FollowingErrorOccurred + args.Exception.Message;
                    stringBuilder.AppendLine(str1);
                }
                if (!(args.Exception is WebException))
                {
                    stringBuilder.AppendLine(Strings.WouldYouLikeSubmitErrorReport);
                    buttons = MessageBoxButtons.YesNo;
                }
                if (MessageBox.Show(stringBuilder.ToString(), Strings.DownloadErrors, buttons, MessageBoxIcon.Hand) != DialogResult.Yes)
                    return;
            }
        }

        private void OnDownloadStateChanged(object sender, DownloadState state)
        {
            this.State = state;
            switch (this.State)
            {
                case DownloadState.Downloading:
                    if (!Program.NetworkMonitor.IsActive)
                    {
                        Program.NetworkMonitor.StartMonitorInternetConnection();
                        break;
                    }
                    break;
                case DownloadState.Completed:
                case DownloadState.Error:
                case DownloadState.Paused:
                case DownloadState.Canceled:
                    if (Program.NetworkMonitor.IsActive)
                    {
                        ObservableCollection<DownloadItem> downloads = DownloadItemViewModel.Instance.Downloads;
                        Func<DownloadItem, bool> func = (Func<DownloadItem, bool>)(d => d.State != DownloadState.Downloading);
                        Func<DownloadItem, bool> predicate = null;
                        if (downloads.All<DownloadItem>(predicate))
                        {
                            Program.NetworkMonitor.StopMonitorInternetConnection();
                            break;
                        }
                        break;
                    }
                    break;
            }
            // ISSUE: reference to a compiler-generated field
            EventHandler raiseSetStatus = this.RaiseSetStatus;
            if (raiseSetStatus == null)
                return;
            // ISSUE: variable of the null type
            EventArgs local = null;
            raiseSetStatus((object)this, (EventArgs)local);
        }

        private void OnConversionProgress(object sender, FreeYouTubeDownloader.Converter.ProgressChangedEventArgs args)
        {
            this.State = DownloadState.Converting;
            this.Progress = args.ProgressInPercent;
            this.FrameSize = ((FfmpegWrapper)this.ConversionTask).VideoSize;
            // ISSUE: reference to a compiler-generated field
            EventHandler raiseSetStatus = this.RaiseSetStatus;
            if (raiseSetStatus == null)
                return;
            // ISSUE: variable of the null type
            EventArgs local = null;
            raiseSetStatus((object)this, (EventArgs)local);
        }

        private void OnConversionCompleted(object sender, ConversionCompletedEventArgs args)
        {
            this.ConversionProfile = ((FfmpegWrapper)sender).ConversionProfile;
            if (args.Succeeded)
            {
                ConversionProfile conversionProfile1 = (ConversionProfile)null;
                if (this.ConversionProfile is MergeProfile)
                {
                    MergeProfile conversionProfile2 = this.ConversionProfile as MergeProfile;
                    conversionProfile1 = conversionProfile2.OriginalProfile;
                    foreach (string path in conversionProfile2.MergeParameters.Input)
                    {
                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch
                        {
                        }
                    }
                }
                if (conversionProfile1 != null)
                {
                    if (!conversionProfile1.Equals((object)Path.GetExtension(this.FileName).TrimStart('.')))
                    {
                        this.ConversionTask = (ConversionTask)new FfmpegWrapper();
                        this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
                        this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
                        this.ConversionTask.Convert(this.DownloadTask.FileName, conversionProfile1, this.VideoQualityInfo);
                        this.State = DownloadState.Converting;
                    }
                    else if (this.VideoQualityInfo.File is AudioLink && conversionProfile1.GetType() == typeof(YouTubeDashAudioNormalizer))
                    {
                        this.ConversionTask = (ConversionTask)new FfmpegWrapper();
                        this.ConversionTask.ProgressChanged += new ConversionTask.ProgressChangedEvent(this.OnConversionProgress);
                        this.ConversionTask.ConversionCompleted += new ConversionTask.ConversionCompletedEvent(this.OnConversionCompleted);
                        this.ConversionTask.Convert(this.DownloadTask.FileName, conversionProfile1, this.VideoQualityInfo);
                        this.State = DownloadState.Converting;
                    }
                }
                else
                {
                    this.State = DownloadState.Completed;
                    if (this.ConversionProfile.DeleteInputFile)
                        FileSystemUtil.SafeDeleteFile(this.ConversionProfile.InputFileName);
                    if (this.VideoQualityInfo != null || !string.IsNullOrWhiteSpace(this.SourceUrl))
                        this.FileName = this.ConversionProfile.OutputFileName;
                    try
                    {
                        this.Size = new FileInfo(this.FileName).Length;
                    }
                    catch
                    {
                        this.Size = 0L;
                    }
                    this.Progress = 100;
                    this.TimeStamp = DateTime.Now;
                }
            }
            else if (args.IsCanceled)
            {
                this.State = DownloadState.Canceled;
                if (this.ConversionProfile is MergeProfile)
                {
                    Thread.Sleep(1000);
                    foreach (string path in ((MergeProfile)this.ConversionProfile).MergeParameters.Input)
                        FileSystemUtil.SafeDeleteFile(path);
                    FileSystemUtil.SafeDeleteFile(this.ConversionProfile.OutputFileName);
                }
                else
                {
                    try
                    {
                        if (this.ConversionProfile.DeleteInputFile)
                            FileSystemUtil.SafeDeleteFile(this.FileName);
                    }
                    catch
                    {
                    }
                }
            }
            else if (args.Error != null)
            {
                this.State = DownloadState.Error;
                this.TimeStamp = DateTime.Now;
                int num = (int)MessageBox.Show(args.Error.Message, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (string.IsNullOrWhiteSpace(this.FrameSize))
                this.FrameSize = ((FfmpegWrapper)this.ConversionTask).VideoSize;
            DownloadItemViewModel.Instance.ObjectListView.BuildList();
            DownloadItemViewModel.Instance.UpdateQueue();
            // ISSUE: reference to a compiler-generated field
            if (this.RaiseSetStatus != null && args.Error == null)
            {
                // ISSUE: reference to a compiler-generated field
                this.RaiseSetStatus((object)this, (EventArgs)new DownloadStateEventArgs(DownloadState.Converting));
            }
            if (!Program.NetworkMonitor.IsActive)
                return;
            ObservableCollection<DownloadItem> downloads = DownloadItemViewModel.Instance.Downloads;
            Func<DownloadItem, bool> func = (Func<DownloadItem, bool>)(d => d.State != DownloadState.Downloading);
            if (!downloads.All<DownloadItem>(di => di.State == DownloadState.Completed))
                return;
            Program.NetworkMonitor.StopMonitorInternetConnection();
        }

        private string SetFileName(string targetFileName)
        {
            string directoryName = Path.GetDirectoryName(targetFileName);
            string fileName = Path.GetFileNameWithoutExtension(targetFileName);
            string lower = Path.GetExtension(targetFileName).ToLower();
            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName);
            fileName = Regex.Replace(fileName, "\\s\\(\\d+\\)$", string.Empty);
            List<string> list1 = ((IEnumerable<string>)Directory.GetFiles(directoryName, string.Format("*{0}", (object)lower), SearchOption.TopDirectoryOnly)).ToList<string>().Select<string, string>(new Func<string, string>(Path.GetFileNameWithoutExtension)).ToList<string>().Where<string>((Func<string, bool>)(filename => fileName.Equals(Regex.Replace(filename, "\\s\\(\\d+\\)$", string.Empty)))).ToList<string>();
            if (DownloadItemViewModel.Instance.Downloads.Count > 0)
            {
                List<DownloadItem> list2 = DownloadItemViewModel.Instance.Downloads.Where<DownloadItem>((Func<DownloadItem, bool>)(item => item.State != DownloadState.AnalizeSource)).ToList<DownloadItem>();
                string fileNameFormat = lower.Replace(".", string.Empty);
                Func<DownloadItem, bool> predicate = (Func<DownloadItem, bool>)(downloadItem => fileNameFormat.Equals(downloadItem.FileNameFormat.ToLower()));
                List<string> list3 = list2.Where<DownloadItem>(predicate).ToList<DownloadItem>().Where<DownloadItem>((Func<DownloadItem, bool>)(downloadItem => fileName.Equals(Regex.Replace(downloadItem.Name, "\\s\\(\\d+\\)$", string.Empty)))).ToList<DownloadItem>().Select<DownloadItem, string>((Func<DownloadItem, string>)(downloadItem => downloadItem.Name)).ToList<string>();
                list1.AddRange((IEnumerable<string>)list3);
            }
            List<string> list4 = list1.Distinct<string>().OrderBy<string, string>((Func<string, string>)(f => f)).ToList<string>();
            if (list4.Count > 0)
            {
                if (!list4.Any<string>((Func<string, bool>)(filename => filename.Equals(fileName))))
                {
                    this.Name = fileName;
                    targetFileName = Path.Combine(directoryName, string.Format("{0}{1}", (object)this.Name, (object)lower));
                }
                else
                {
                    int index = 1;
                    while (list4.Any<string>((Func<string, bool>)(filename => filename.Equals(string.Format("{0} ({1})", (object)fileName, (object)index)))))
                        ++index;
                    this.Name = string.Format("{0} ({1})", (object)fileName, (object)index);
                    targetFileName = Path.Combine(directoryName, string.Format("{0}{1}", (object)this.Name, (object)lower));
                }
            }
            else
            {
                this.Name = fileName;
                targetFileName = Path.Combine(directoryName, string.Format("{0}{1}", (object)this.Name, (object)lower));
            }
            return targetFileName;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
