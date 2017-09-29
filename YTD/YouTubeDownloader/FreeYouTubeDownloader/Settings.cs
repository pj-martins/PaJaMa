// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Settings
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.Downloader;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
    internal class Settings : IPreservable, IJsonSerializable
    {
        private bool _allowSimultaneousDownloads = true;
        private bool _removeAllFinishedFiles = true;
        private bool _checkForUpdates = true;
        private bool _notifyUrlInClipboard = true;
        private string _preferedMediaFormat = "mp4";
        public DesiredAction DesiredAudioDownloadAction = DesiredAction.ExtractMP3;
        public VideoQuality DesiredDownloadVideoQuality = VideoQuality._1080p;
        public AudioQuality DesiredDownloadAudioQuality = AudioQuality._128kbps;
        public const string HiddenStartArgument = "-h";
        public const string InitializeAutoStartupArgument = "-ias";
        public const string RemoveAutoStartupArgument = "-ras";
        private int _maxSimultaneousDownloads;
        private bool _alwaysOnTop;
        private bool _allowSounds;
        private bool _allowBalloons;
        private bool _needToSerialize;
        private bool _rememberLastQualityUsed;
        private bool _runSearchOnEnterKey;
        private string _languageCode;
        private string _defaultVideosDownloadFolder;
        private string _defaultAudiosDownloadFolder;
        private int _windowWidth;
        private int _windowHeight;
        public DesiredAction? DesiredDownloadAction;
        public DesiredAction DesiredVideoDownloadAction;
        public DesiredAction? DesiredConversionAction;
        public FileLocationOption FileLocationOption;
        private CloseAppActions _closeAppAction;
        private int _nameColumnWidth;
        private bool _automaticallyDownloadVideo;
        private bool _automaticallyDownloadAudio;
        private static Settings _instance;

        internal int MaxSimultaneousDownloads
        {
            get
            {
                return this._maxSimultaneousDownloads;
            }
            set
            {
                this._maxSimultaneousDownloads = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool AllowSimultaneousDownloads
        {
            get
            {
                return this._allowSimultaneousDownloads;
            }
            set
            {
                this._allowSimultaneousDownloads = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool AlwaysOnTop
        {
            get
            {
                return this._alwaysOnTop;
            }
            set
            {
                this._alwaysOnTop = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool AllowSounds
        {
            get
            {
                return this._allowSounds;
            }
            set
            {
                this._allowSounds = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool AllowBalloons
        {
            get
            {
                return this._allowBalloons;
            }
            set
            {
                this._allowBalloons = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool RemoveAllFinishedFiles
        {
            get
            {
                return this._removeAllFinishedFiles;
            }
            set
            {
                this._removeAllFinishedFiles = value;
                this.NeedToSerialize = true;
            }
        }

        internal string LanguageCode
        {
            get
            {
                if (!string.IsNullOrEmpty(this._languageCode))
                    return this._languageCode;
                return "en";
            }
            set
            {
                if (this._languageCode == value)
                    return;
                this._languageCode = value;
                this.NeedToSerialize = true;
            }
        }

        internal int WindowWidth
        {
            get
            {
                return this._windowWidth;
            }
            set
            {
                this._windowWidth = value;
                this.NeedToSerialize = true;
            }
        }

        internal int WindowHeight
        {
            get
            {
                return this._windowHeight;
            }
            set
            {
                this._windowHeight = value;
                this.NeedToSerialize = true;
            }
        }

        internal string PreferedMediaFormat
        {
            get
            {
                return this._preferedMediaFormat;
            }
            set
            {
                if (this._preferedMediaFormat == value)
                    return;
                this._preferedMediaFormat = value;
                this.NeedToSerialize = true;
            }
        }

        internal string DefaultVideosDownloadFolder
        {
            get
            {
                return this._defaultVideosDownloadFolder ?? (this._defaultVideosDownloadFolder = this.GetDefaultDownloadDirectory(true));
            }
            set
            {
                if (this._defaultVideosDownloadFolder == value)
                    return;
                this._defaultVideosDownloadFolder = value;
                if (!Directory.Exists(this._defaultVideosDownloadFolder))
                    Directory.CreateDirectory(this._defaultVideosDownloadFolder);
                this.NeedToSerialize = true;
            }
        }

        internal string DefaultAudiosDownloadFolder
        {
            get
            {
                return this._defaultAudiosDownloadFolder ?? (this._defaultAudiosDownloadFolder = this.GetDefaultDownloadDirectory(false));
            }
            set
            {
                if (this._defaultAudiosDownloadFolder == value)
                    return;
                this._defaultAudiosDownloadFolder = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool RememberLastQualityUsed
        {
            get
            {
                return this._rememberLastQualityUsed;
            }
            set
            {
                if (this._rememberLastQualityUsed == value)
                    return;
                this._rememberLastQualityUsed = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool RunSearchOnEnterKey
        {
            get
            {
                return this._runSearchOnEnterKey;
            }
            set
            {
                if (this._runSearchOnEnterKey == value)
                    return;
                this._runSearchOnEnterKey = value;
                this.NeedToSerialize = true;
            }
        }

        public string LogFileName
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Vitzo", "Free YouTube Downloader", "log.txt");
            }
        }

        public bool CheckForUpdates
        {
            get
            {
                return this._checkForUpdates;
            }
            set
            {
                if (this._checkForUpdates == value)
                    return;
                this._checkForUpdates = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool StartWithWindows
        {
            get
            {
                return RegistryManager.IsValue(Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", Application.ProductName);
            }
            set
            {
                Settings.MakeStartWithWindows(value);
            }
        }

        internal bool NotifyUrlInClipboard
        {
            get
            {
                return this._notifyUrlInClipboard;
            }
            set
            {
                if (this._notifyUrlInClipboard == value)
                    return;
                this._notifyUrlInClipboard = value;
                this.NeedToSerialize = true;
            }
        }

        public CloseAppActions CloseAppAction
        {
            get
            {
                return this._closeAppAction;
            }
            set
            {
                if (this._closeAppAction == value)
                    return;
                this._closeAppAction = value;
                this.NeedToSerialize = true;
            }
        }

        public int NameColumnWidth
        {
            get
            {
                return this._nameColumnWidth;
            }
            set
            {
                if (this._nameColumnWidth == value)
                    return;
                this._nameColumnWidth = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool AutomaticallyDownloadVideo
        {
            get
            {
                return this._automaticallyDownloadVideo;
            }
            set
            {
                if (this._automaticallyDownloadVideo == value)
                    return;
                this._automaticallyDownloadVideo = value;
                this.NeedToSerialize = true;
            }
        }

        internal bool AutomaticallyDownloadAudio
        {
            get
            {
                return this._automaticallyDownloadAudio;
            }
            set
            {
                if (this._automaticallyDownloadAudio == value)
                    return;
                this._automaticallyDownloadAudio = value;
                this.NeedToSerialize = true;
            }
        }

        internal static Settings Instance
        {
            get
            {
                return Settings._instance ?? (Settings._instance = new Settings());
            }
        }

        public bool NeedToSerialize
        {
            get
            {
                return this._needToSerialize;
            }
            set
            {
                if (this._needToSerialize)
                    return;
                this._needToSerialize = value;
            }
        }

        public event Settings.SettingsChangedEventHandler SettingsChanged;

        internal Settings()
        {
            this.MaxSimultaneousDownloads = 3;
            this.NotifyUrlInClipboard = true;
            this.AddToStateManager();
            this.RestoreState();
        }

        internal void RaiseSettingsChanged()
        {
            // ISSUE: reference to a compiler-generated field
            Settings.SettingsChangedEventHandler settingsChanged = this.SettingsChanged;
            if (settingsChanged == null)
                return;
            EventArgs e = new EventArgs();
            settingsChanged((object)this, e);
        }

        private string GetDefaultDownloadDirectory(bool isVideo)
        {
            string path = Environment.OSVersion.Version.Major <= 5 ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), isVideo ? "Free YouTube Downloader\\Video" : "Free YouTube Downloader\\Audio") : Path.Combine(isVideo ? Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) : Environment.GetFolderPath(Environment.SpecialFolder.MyMusic), "Free YouTube Downloader");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        public static void MakeStartWithWindows(bool start)
        {
            if (start)
                RegistryManager.SetValue(Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", Application.ProductName, (object)string.Format("\"{0}\" {1}", (object)Application.ExecutablePath, (object)"-h"), RegistryValueKind.String);
            else
                RegistryManager.DeleteValue(Registry.CurrentUser, "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\", Application.ProductName);
        }

        public void AddToStateManager()
        {
            ApplicationStateManager.Instance.AddObject((IPreservable)this);
        }

        public void RestoreState()
        {
            Log.Trace("CALL Settings.RestoreState()", (Exception)null);
            if (!File.Exists(SharedConstants.SettingsFileName))
                return;
            try
            {
                using (FileStream fileStream = new FileStream(SharedConstants.SettingsFileName, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)fileStream))
                    {
                        using (JsonTextReader jsonTextReader = new JsonTextReader((TextReader)streamReader))
                            this.ReadJson(jsonTextReader);
                    }
                }
            }
            catch
            {
            }
        }

        public void SaveState()
        {
            Log.Trace("CALL Settings.SaveState()", (Exception)null);
            try
            {
                using (FileStream fileStream = new FileStream(SharedConstants.SettingsFileName, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)fileStream))
                    {
                        using (JsonTextWriter jsonTextWriter = new JsonTextWriter((TextWriter)streamWriter))
                            this.WriteJson(jsonTextWriter);
                    }
                }
            }
            catch
            {
            }
        }

        public void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string s = (string)jsonTextReader.Value;
                    int? nullable;
                    if (s == "AutomaticallyDownloadAudio")
                    {
                        jsonTextReader.Read();
                        this._automaticallyDownloadAudio = (bool)jsonTextReader.Value;
                    }
                    else if (s == "AllowSimultaneousDownloads")
                    {
                        jsonTextReader.Read();
                        this.AllowSimultaneousDownloads = (bool)jsonTextReader.Value;
                    }
                    else if (s == "AllowBalloons")
                    {
                        jsonTextReader.Read();
                        this.AllowBalloons = (bool)jsonTextReader.Value;
                    }
                    else if (s == "AutomaticallyDownloadVideo") // if ((int) stringHash == 1159863881 && s == "AutomaticallyDownloadVideo")
                    {
                        jsonTextReader.Read();
                        this._automaticallyDownloadVideo = (bool)jsonTextReader.Value;
                    }
                    else if (s == "FileLocationOption")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.FileLocationOption = (FileLocationOption)nullable.GetValueOrDefault(0);
                    }
                    else if (s == "RemoveAllFinishedFiles")
                    {
                        jsonTextReader.Read();
                        this.RemoveAllFinishedFiles = (bool)jsonTextReader.Value;
                    }
                    else if (true && s == "AlwaysOnTop") // if ((int) stringHash == 1921169572 && s == "AlwaysOnTop")
                    {
                        jsonTextReader.Read();
                        this.AlwaysOnTop = (bool)jsonTextReader.Value;
                    }
                    else if (s == "LanguageCode")
                        this.LanguageCode = jsonTextReader.ReadAsString();
                    else if (s == "MaxSimultaneousDownloads")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.MaxSimultaneousDownloads = nullable.GetValueOrDefault(3);
                    }
                    else if (s == "PreferedMediaFormat") // if ((int) stringHash == -1905862315 && s == "PreferedMediaFormat")
                        this._preferedMediaFormat = jsonTextReader.ReadAsString();
                    else if (s == "NotifyUrlInClipboard")
                    {
                        jsonTextReader.Read();
                        this._notifyUrlInClipboard = (bool)jsonTextReader.Value;
                    }
                    else if (s == "DesiredAudioDownloadAction")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.DesiredAudioDownloadAction = (DesiredAction)nullable.Value;
                    }
                    else if (true && s == "CheckForUpdates") // if ((int) stringHash == -1437496568 && s == "CheckForUpdates")
                    {
                        jsonTextReader.Read();
                        this.CheckForUpdates = (bool)jsonTextReader.Value;
                    }
                    else if (s == "RememberLastQualityUsed")
                    {
                        jsonTextReader.Read();
                        this.RememberLastQualityUsed = (bool)jsonTextReader.Value;
                    }
                    else if (s == "DefaultVideosDownloadFolder")
                        this.DefaultVideosDownloadFolder = jsonTextReader.ReadAsString();
                    if (true && s == "CloseAppAction") // if ((int) stringHash == -677240356 && s == "CloseAppAction")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this._closeAppAction = (CloseAppActions)nullable.Value;
                    }
                    else if (s == "WindowHeight")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this._windowHeight = nullable.GetValueOrDefault(0);
                    }
                    else if (s == "DefaultAudiosDownloadFolder")
                        this.DefaultAudiosDownloadFolder = jsonTextReader.ReadAsString();
                    if (true && s == "NameColumnWidth") // if ((int) stringHash == -537897656 && s == "NameColumnWidth")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this._nameColumnWidth = nullable.Value;
                    }
                    else if (s == "WindowWidth")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this._windowWidth = nullable.GetValueOrDefault(0);
                    }
                    else if (s == "DesiredDownloadVideoQuality")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.DesiredDownloadVideoQuality = (VideoQuality)nullable.Value;
                    }
                    if (true && s == "AllowSounds") // if ((int) stringHash == -193317986 && s == "AllowSounds")
                    {
                        jsonTextReader.Read();
                        this.AllowSounds = (bool)jsonTextReader.Value;
                    }
                    else if (s == "DesiredDownloadAudioQuality")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.DesiredDownloadAudioQuality = (AudioQuality)nullable.Value;
                    }
                    if (true && s == "RunSearchOnEnterKey") // if ((int) stringHash == -126984490 && s == "RunSearchOnEnterKey")
                    {
                        jsonTextReader.Read();
                        this.RunSearchOnEnterKey = (bool)jsonTextReader.Value;
                    }
                    else if (s == "DesiredVideoDownloadAction")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.DesiredVideoDownloadAction = (DesiredAction)nullable.Value;
                    }
                }
            }
        }

        public void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("MaxSimultaneousDownloads");
            jsonTextWriter.WriteValue(this.MaxSimultaneousDownloads);
            jsonTextWriter.WritePropertyName("LanguageCode");
            jsonTextWriter.WriteValue(this.LanguageCode);
            if (this.WindowWidth > 0)
            {
                jsonTextWriter.WritePropertyName("WindowWidth");
                jsonTextWriter.WriteValue(this.WindowWidth);
            }
            if (this.WindowHeight > 0)
            {
                jsonTextWriter.WritePropertyName("WindowHeight");
                jsonTextWriter.WriteValue(this.WindowHeight);
            }
            jsonTextWriter.WritePropertyName("PreferedMediaFormat");
            jsonTextWriter.WriteValue(this.PreferedMediaFormat);
            jsonTextWriter.WritePropertyName("AllowSimultaneousDownloads");
            jsonTextWriter.WriteValue(this.AllowSimultaneousDownloads);
            jsonTextWriter.WritePropertyName("AlwaysOnTop");
            jsonTextWriter.WriteValue(this.AlwaysOnTop);
            jsonTextWriter.WritePropertyName("DefaultVideosDownloadFolder");
            jsonTextWriter.WriteValue(this.DefaultVideosDownloadFolder);
            jsonTextWriter.WritePropertyName("DefaultAudiosDownloadFolder");
            jsonTextWriter.WriteValue(this.DefaultAudiosDownloadFolder);
            jsonTextWriter.WritePropertyName("FileLocationOption");
            jsonTextWriter.WriteValue((int)this.FileLocationOption);
            jsonTextWriter.WritePropertyName("AllowSounds");
            jsonTextWriter.WriteValue(this.AllowSounds);
            jsonTextWriter.WritePropertyName("AllowBalloons");
            jsonTextWriter.WriteValue(this.AllowBalloons);
            jsonTextWriter.WritePropertyName("RemoveAllFinishedFiles");
            jsonTextWriter.WriteValue(this.RemoveAllFinishedFiles);
            jsonTextWriter.WritePropertyName("DesiredDownloadVideoQuality");
            jsonTextWriter.WriteValue((int)this.DesiredDownloadVideoQuality);
            jsonTextWriter.WritePropertyName("DesiredDownloadAudioQuality");
            jsonTextWriter.WriteValue((int)this.DesiredDownloadAudioQuality);
            jsonTextWriter.WritePropertyName("DesiredVideoDownloadAction");
            jsonTextWriter.WriteValue((int)this.DesiredVideoDownloadAction);
            jsonTextWriter.WritePropertyName("DesiredAudioDownloadAction");
            jsonTextWriter.WriteValue((int)this.DesiredAudioDownloadAction);
            jsonTextWriter.WritePropertyName("RememberLastQualityUsed");
            jsonTextWriter.WriteValue(this.RememberLastQualityUsed);
            jsonTextWriter.WritePropertyName("RunSearchOnEnterKey");
            jsonTextWriter.WriteValue(this.RunSearchOnEnterKey);
            jsonTextWriter.WritePropertyName("CheckForUpdates");
            jsonTextWriter.WriteValue(this.CheckForUpdates);
            jsonTextWriter.WritePropertyName("NotifyUrlInClipboard");
            jsonTextWriter.WriteValue(this.NotifyUrlInClipboard);
            if (this.CloseAppAction != CloseAppActions.Prompt)
            {
                jsonTextWriter.WritePropertyName("CloseAppAction");
                jsonTextWriter.WriteValue((int)this.CloseAppAction);
            }
            jsonTextWriter.WritePropertyName("NameColumnWidth");
            jsonTextWriter.WriteValue(this.NameColumnWidth);
            if (this.AutomaticallyDownloadVideo)
            {
                jsonTextWriter.WritePropertyName("AutomaticallyDownloadVideo");
                jsonTextWriter.WriteValue(this.AutomaticallyDownloadVideo);
            }
            if (this.AutomaticallyDownloadAudio)
            {
                jsonTextWriter.WritePropertyName("AutomaticallyDownloadAudio");
                jsonTextWriter.WriteValue(this.AutomaticallyDownloadAudio);
            }
            jsonTextWriter.WriteEndObject();
        }

        public delegate void SettingsChangedEventHandler(object sender, EventArgs e);
    }
}
