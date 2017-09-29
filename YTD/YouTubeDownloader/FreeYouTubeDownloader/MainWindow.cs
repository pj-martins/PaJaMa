// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.MainWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using BrightIdeasSoftware;
using FreeYouTubeDownloader.Ads;
using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Common.Types;
using FreeYouTubeDownloader.Controls;
using FreeYouTubeDownloader.Converter;
using FreeYouTubeDownloader.Downloader;
using FreeYouTubeDownloader.Downloader.Providers;
using FreeYouTubeDownloader.Downloader.Tasks;
using FreeYouTubeDownloader.Extensions;
using FreeYouTubeDownloader.License;
using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Model;
using FreeYouTubeDownloader.Properties;
using FreeYouTubeDownloader.Search;
using FreeYouTubeDownloader.UI;
using FreeYouTubeDownloader.Youtube;
using Google.Measurement;
using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Shell;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using wyDay.Controls;
using wyDay.TurboActivate;

namespace FreeYouTubeDownloader
{
    public sealed class MainWindow : Form, ILocalizableForm, IAdsHost
    {
        private FormWindowState _lastWindowState = FormWindowState.Minimized;
        private bool _connected = true;
        private readonly object _locker = new object();
        private int _additionalWidth = 28;
        private const string LatestVersionInfoUrl = "http://youtubedownloader.com/files/version.json";
        private const string LatestVersionInfoUrlFallback = "http://wd.getyoutubedownloader.com/version/version.json";
        private const int RowHeight = 70;
        private SynchronizationContext _syncContext;
        private bool _actionConvert;
        private System.Windows.Forms.Button _clearInputButton;
        private MediaInfo _receivedMediaInfo;
        private Thread _threadNotifyIconClose;
        private int _countBalloons;
        private int _maxLengthFileNameInBalloonTip;
        private Thread _searchVideoDataThread;
        private TimerEx _timerSearch;
        private float _tableLayoutPanelSearchResultRowHeight;
        private bool _hiddenStart;
        private bool _bottomAdShown;
        private bool _rightAdShown;
        private WebBrowserEx _adRightHost;
        private WebBrowserEx _adBottomHost;
        private System.Windows.Forms.ToolTip _toolTip;
        private uint _lastClipboardSequenceNumber;
        private System.Drawing.Image _thumbnailOverlay;
        private bool _ignoreAutoDownloadFlag;
        private TaskbarIcon _notifyIcon;
        private IContainer components;
        private System.Windows.Forms.TextBox textBoxUrl;
        private TableLayoutPanel tableLayoutPanel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ObjectListView olvDownloads;
        private OLVColumn olvColumnProgress;
        private OLVColumn olvColumnDownloadSpeed;
        private OLVColumn olvColumnEta;
        private OLVColumn olvColumnName;
        private BarRenderer barRenderer;
        private OLVColumn olvColumnSize;
        private OLVColumn olvColumnDuration;
        private MenuStrip menuStrip;
        private ToolStripMenuItem windowToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private OLVColumn olvColumnStatus;
        private ToolStripMenuItem versionToolStripMenuItem;
        private ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private OLVColumn olvColumnFormat;
        private ToolStripMenuItem minimizeToolStripMenuItem;
        private ToolStripMenuItem contributeToolStripMenuItem;
        private ToolStripMenuItem suggestToolStripMenuItem;
        private ToolStripMenuItem versionNumberToolStripMenuItem;
        private ToolStripMenuItem minimizeToNotificationAreaToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem downloadLatestVersionToolStripMenuItem;
        private ToolStripMenuItem becomeAFanToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem visitOurWebsiteToolStripMenuItem;
        private ToolStripMenuItem askAQuestionToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanelActionButtons;
        private System.Windows.Forms.Button buttonConvert;
        private ToolStripMenuItem alwaysOnTopMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private TableLayoutPanel tableLayoutPanelActionButtonsContainer;
        private TransparentPictureBox picBoxVideo;
        private SplitButton splitButtonActionVideo;
        private SplitButton splitButtonActionAudio;
        private TransparentPictureBox picBoxAudio;
        private TableLayoutPanel tableLayoutPanel2;
        private PictureBox picBoxHelp;
        private LinkLabel lnkLabelHelp;
        private PictureBox picBoxSupport;
        private PictureBox pictureBoxSettings;
        private LinkLabel lnkLabelSupport;
        private LinkLabel lnkLabelSettings;
        private System.Windows.Forms.Button btnVideoFiles;
        private System.Windows.Forms.Button btnAudioFiles;
        private ContextMenuStrip contextMenuStripDownloadVideo;
        private ContextMenuStrip contextMenuStripDownloadAudio;
        private ToolStripMenuItem askFileNameAndFolderToolStripMenuItem;
        private OLVColumn olvColumnFrameSize;
        private ToolStripMenuItem readDocumentationToolStripMenuItem;
        private PictureBox picBoxFacebook;
        private PictureBox picBoxTwitter;
        private System.Windows.Forms.Panel panelButtonActionVideo;
        private System.Windows.Forms.Panel panelButtonActionAudio;
        private System.Windows.Forms.Panel PanelContainerAutoComplete;
        private System.Windows.Forms.Panel PanelWrapperContainerAutoComplete;
        private System.Windows.Forms.ProgressBar progressBarLoadingData;
        private System.Windows.Forms.Panel panelSearchResult;
        private PictureBoxEx pictureBoxSearchResult;
        private System.Windows.Forms.Label lblSearchResultTitle;
        private System.Windows.Forms.Label lblSearchResultTime;
        private System.Windows.Forms.Label lblSearchResultDescription;
        private PictureBox pictureBoxOpenFile;
        private System.Windows.Forms.ToolTip toolTipPictureBoxOpenFile;
        private PictureBox pictureBoxCopyPath;
        private ToolStripMenuItem reportAProblemToolStripMenuItem;
        private ToolStripMenuItem signupForUpdatesToolStripMenuItem;
        private System.Windows.Forms.Panel panelAdBottom;
        private System.Windows.Forms.Panel panelAdRight;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem compatibleURLNotificationToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem upgradeToProVersionToolStripMenuItem;
        private ToolStripMenuItem deactivateFYDProToolStripMenuItem;
        private PictureBox picBoxAppNotify;
        private ToolStripMenuItem autoDownloadMenuItem;
        private ToolStripMenuItem videoAutoDownloadMenuItem;
        private ToolStripMenuItem audioAutoDownloadMenuItem;

        private string Status
        {
            set
            {
                this.SetStatusStrip(value);
            }
        }

        private static bool AllowSound
        {
            get
            {
                return Settings.Instance.AllowSounds;
            }
        }

        private static bool AllowBalloon
        {
            get
            {
                return Settings.Instance.AllowBalloons;
            }
        }

        private string TextBoxUrl
        {
            get
            {
                return (string)this.textBoxUrl.Tag;
            }
            set
            {
                this.textBoxUrl.Tag = (object)value;
                if (string.IsNullOrEmpty(value))
                {
                    this.picBoxFacebook.Enabled = false;
                    this.picBoxFacebook.Image = (System.Drawing.Image)Resources.facebook_image_dis;
                    this.picBoxTwitter.Enabled = false;
                    this.picBoxTwitter.Image = (System.Drawing.Image)Resources.twitter_image_dis;
                    YoutubeHelper.YoutubeSearch.Query = value;
                }
                else
                {
                    if (YoutubeHelper.GetYoutubeVideoIdByUrl(value) == null)
                        return;
                    this.picBoxFacebook.Enabled = true;
                    this.picBoxFacebook.Image = (System.Drawing.Image)Resources.facebook_image;
                    this.picBoxTwitter.Enabled = true;
                    this.picBoxTwitter.Image = (System.Drawing.Image)Resources.twitter_image;
                }
            }
        }

        public bool Connected
        {
            get
            {
                return this._connected;
            }
            set
            {
                if (this._connected == value)
                    return;
                this._connected = value;
                this.HandleInternetConnectionChanged();
            }
        }

        public bool HiddenStart
        {
            set
            {
                this._hiddenStart = value;
            }
        }

        public MainWindow()
        {
            this.InitializeComponent();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            this.InitializeVariablesAndControls();
            this.InitSearchPanelContainer();
            FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler)((sender, args) => this.ApplyCurrentLocalization());
            this.ApplyCurrentLocalization();
            this.ConfigObjectListView();
            DownloadItemViewModel.Initialize((object)this, this.olvDownloads);
            this.HandleCommandArgs();
            this.CheckForSupportedUrlInClipboard(false);
            if (Settings.Instance.CheckForUpdates)
                ThreadPool.QueueUserWorkItem((WaitCallback)(foo => this.CheckForNewVersion(false, false)));
            this.olvDownloads.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.OnColumnWidthChanged);
            AdsHelper.Init((IAdsHost)this);
            this.VerifyAndInitializeLicense();
            this.InitializeFromSettings();
            if (WBEmulator.IsBrowserEmulationSet())
                return;
            WBEmulator.SetBrowserEmulationVersion();
        }

        private void OnActionButtonClick(object sender, EventArgs e)
        {
            System.Windows.Forms.Control control = (System.Windows.Forms.Control)sender;
            if (control.Tag == null)
                return;
            ButtonActionData tag = (ButtonActionData)control.Tag;
            bool flag = !this._actionConvert;
            int num1 = flag ? 1 : 0;
            if (!MainWindow.SaveCustomQuality(tag, num1 != 0))
            {
                int num2 = (int)System.Windows.Forms.MessageBox.Show(Strings.MessagePleaseSelectAction);
            }
            else
            {
                try
                {
                    if (flag)
                    {
                        this.Status = string.Format("{0} ...", (object)Strings.ReceivingDownloadLinks);
                        this.ProcessDownloadAccordingToDesiredAction(Settings.Instance.DesiredDownloadAction.Value, this._receivedMediaInfo, true);
                        System.Windows.Forms.Application.DoEvents();
                    }
                    else
                    {
                        string textBoxUrl = this.TextBoxUrl;
                        if (!System.IO.File.Exists(textBoxUrl))
                            throw new FileNotFoundException(Strings.InputFileNameIsInvalid, textBoxUrl);
                        this.Status = string.Format("{0} ...", (object)Strings.ReceivingConvertLinks);
                        this.ProcessConversionAccordingToDesiredAction(Settings.Instance.DesiredConversionAction.Value, textBoxUrl);
                    }
                }
                catch (Exception ex)
                {
                    this.Status = Strings.Error;
                    int num3 = (int)System.Windows.Forms.MessageBox.Show(ex.Message, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void OnExitMenuItemClick(object sender, EventArgs e)
        {
            this._notifyIcon.Visibility = Visibility.Collapsed;
            ApplicationStateManager.Instance.Save();
            Environment.Exit(0);
        }

        private void OnDownloadsListDoubleClick(object sender, EventArgs e)
        {
            ObjectListView objectListView = sender as ObjectListView;
            if (objectListView == null || objectListView.SelectedObject == null)
                return;
            DownloadItem selectedObject = objectListView.SelectedObject as DownloadItem;
            if (selectedObject == null || selectedObject.State != DownloadState.Completed)
                return;
            MainWindow.RunProcessStart(selectedObject.FileExists, selectedObject.FileName, true);
        }

        private void OnDownloadsListCellRightClick(object sender, CellRightClickEventArgs e)
        {
            if (e.Model == null)
                return;
            e.MenuStrip = this.CreateContextMenuForDownload(e.Model as DownloadItem);
        }

        private void OnRetryDownloadClick(object sender, EventArgs eventArgs)
        {
            ((DownloadItem)((ToolStripItem)sender).Tag).TriggerDownloadingSinceQueueReleased();
        }

        private void OnRemoveDownloadClick(object sender, EventArgs eventArgs)
        {
            foreach (DownloadItem selectedObject in (IEnumerable)this.olvDownloads.SelectedObjects)
            {
                selectedObject.RaiseSetStatus -= new EventHandler(this.SetStatus);
                if (selectedObject.State == DownloadState.Converting)
                {
                    DownloadItemViewModel.Instance.CancelConversion(selectedObject);
                }
                else
                {
                    DownloadItemViewModel.Instance.Remove(selectedObject);
                    if (selectedObject.State == DownloadState.Error)
                        DownloadItemViewModel.Instance.Delete(selectedObject);
                }
            }
            this.Status = this.GetLocalizedStatusText(DownloadState.Canceled);
        }

        private void OnDeleteDownloadClick(object sender, EventArgs eventArgs)
        {
            if (this.olvDownloads.SelectedObjects.Count > 1)
            {
                if (System.Windows.Forms.MessageBox.Show(Strings.DeleteSelectedDownloads, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                foreach (DownloadItem selectedObject in (IEnumerable)this.olvDownloads.SelectedObjects)
                    DownloadItemViewModel.Instance.Delete(selectedObject);
            }
            else
            {
                DownloadItem selectedObject = (DownloadItem)this.olvDownloads.SelectedObjects[0];
                if (System.Windows.Forms.MessageBox.Show(Strings.DeleteThisDownload, string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                try
                {
                    DownloadItemViewModel.Instance.Delete(selectedObject);
                }
                catch
                {
                }
            }
        }

        private void OnRemoveAllFinishedDownloadsClick(object sender, EventArgs eventArgs)
        {
            List<DownloadItem> tag = ((ToolStripItem)sender).Tag as List<DownloadItem>;
            if (tag == null || !tag.Any<DownloadItem>() || System.Windows.Forms.MessageBox.Show(string.Format(Strings.DeleteFinishedFiles, (object)tag.Count), string.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            foreach (DownloadItem downloadItem in tag)
                DownloadItemViewModel.Instance.Remove(downloadItem);
            this.Status = Strings.Done;
        }

        private void OnPauseDownloadClick(object sender, EventArgs eventArgs)
        {
            DownloadItem tag = (DownloadItem)((ToolStripItem)sender).Tag;
            this.Status = this.GetLocalizedStatusText(DownloadState.Paused);
            tag.DownloadTask.Pause();
        }

        private void OnResumeDownloadClick(object sender, EventArgs eventArgs)
        {
            DownloadItem tag = (DownloadItem)((ToolStripItem)sender).Tag;
            this.Status = this.GetLocalizedStatusText(DownloadState.Downloading);
            tag.DownloadTask.Resume();
        }

        private void OnPreferencesMenuItemClick(object sender, EventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Owner = (Form)this;
            int num = (int)settingsWindow.ShowDialog((IWin32Window)this);
        }

        private void OnApplicationSettingsChanged(object sender, EventArgs e)
        {
            DownloadItemViewModel.Instance.UpdateQueue();
        }

        private void OnCheckForUpdateMenuItemClick(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((WaitCallback)(foo => this.CheckForNewVersion(true, false)));
        }

        private void OnMainWindowResize(object sender, EventArgs e)
        {
            this.ResizePanelSearchResultControls(false);
            Settings.Instance.WindowWidth = this.Width;
            Settings.Instance.WindowHeight = this.Height;
            if (this.PanelWrapperContainerAutoComplete.Visible)
                this.SearchDropDownPanelShow(false);
            if (this.WindowState == this._lastWindowState)
                return;
            this._lastWindowState = this.WindowState;
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (LicenseHelper.IsGenuine)
                    return;
                this.ShowRightAd(true);
                this.HideBottomAd(true);
            }
            if (this.WindowState != FormWindowState.Normal)
                return;
            if (LicenseHelper.IsGenuine)
                return;
            this.HideRightAd(true);
            DateTime startupTime = Process.GetCurrentProcess().StartTime;
            this.ShowBottomAd(!this._bottomAdShown & (DownloadItemViewModel.Instance != null && DownloadItemViewModel.Instance.Downloads.Any<DownloadItem>((Func<DownloadItem, bool>)(d => d.TimeStamp > startupTime))));
        }

        private void OnSuggestMenuItemClick(object sender, EventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("https://youtubedownloader.com/suggest-a-feature/", UriKind.Absolute), "https://youtubedownloader.com/suggest-a-feature/", false);
        }

        private void OnPlayDownloadClick(object sender, EventArgs eventArgs)
        {
            DownloadItem tag = (DownloadItem)((ToolStripItem)sender).Tag;
            if (tag.State != DownloadState.Completed)
                return;
            MainWindow.RunProcessStart(tag.FileExists, tag.FileName, true);
        }

        private void OnOpenFileLocationDownloadClick(object sender, EventArgs eventArgs)
        {
            DownloadItem tag = (DownloadItem)((ToolStripItem)sender).Tag;
            if (tag.FileExists)
            {
                string directoryName = Path.GetDirectoryName(tag.FileName);
                if (string.IsNullOrWhiteSpace(directoryName))
                    return;
                MainWindow.RunProcessStart(Directory.Exists(directoryName), directoryName, true);
            }
            else
                MainWindow.RunProcessStart(false, tag.FileName, true);
        }

        private void OnCopyVideoUrlDownloadClick(object sender, EventArgs eventArgs)
        {
            System.Windows.Forms.Clipboard.SetText(((DownloadItem)((ToolStripItem)sender).Tag).SourceUrl);
            this.Status = Strings.VideoURLCopiedToClipboard;
        }

        private void OnShareVideoOnFacebookDownloadClick(object sender, EventArgs eventArgs)
        {
            string str = string.Format("https://www.facebook.com/sharer/sharer.php?u={0}", (object)Uri.EscapeDataString(string.Format("https://savemedia.com/watch?v={0}", (object)new YouTubeDownloadProvider().GetVideoId(((DownloadItem)((ToolStripItem)sender).Tag).SourceUrl))));
            MainWindow.RunProcessStart(MainWindow.VerifyUrl(str, UriKind.Absolute), str, false);
        }

        private void OnMinimizeMenuItemClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void OnTextBoxUrlTextChanged(object sender, EventArgs e)
        {
            this.RowSearchResultHide();
            this.SearchDropDownPanelHide(true);
            this.SetEnableDownloadButtons(false, true);
            this.SetTooltipToFacebookAndTwitterButtons(false);
            if (string.IsNullOrWhiteSpace(this.textBoxUrl.Text) || this.textBoxUrl.Text.Length <= 2 || Strings.PasteURLHere.Equals(this.textBoxUrl.Text))
                return;
            string str = this.textBoxUrl.Text.Trim();
            if (DownloadManager.HasProviderForUrl(str) && !this._timerSearch.Enabled)
            {
                this.TextBoxUrl = (string)null;
                this.GetMediaInfo(str, true, this._ignoreAutoDownloadFlag);
                if (!this._ignoreAutoDownloadFlag)
                    return;
                this._ignoreAutoDownloadFlag = false;
            }
            else if (str.IsValidFileName())
            {
                this._actionConvert = true;
                this.Status = string.Format("{0} ...", (object)Strings.ReceivingConvertLinks);
                MainWindow.SetControlVisible((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, false);
                this.SetEnableDownloadButtons(true, false);
                this.InitDropDownMenuDownload(true, str.IsFileAudioFormat(), str);
                MainWindow.SetControlVisible((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, true);
            }
            else
            {
                if (str.Length <= 2)
                    return;
                this._timerSearch.Stop();
                this.SearchDropDownPanelHide(true);
                this._timerSearch.IsIgnored = Settings.Instance.RunSearchOnEnterKey;
                this._timerSearch.Start(str);
            }
        }

        private void OnTimerSearchElapsed(object sender, ElapsedEventArgs e)
        {
            this.SearchDataByKeyword(((TimerEx)sender).Keyword);
        }

        private void OnPanelContainerAutoCompleteScroll(object sender, ScrollEventArgs e)
        {
            System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)sender;
            if (e.OldValue == e.NewValue || panel.VerticalScroll.Maximum - panel.Height >= panel.VerticalScroll.Value)
                return;
            this.CallInitVideoDataList();
        }

        private void OnPanelContainerAutoCompleteMouseWheel(object sender, MouseEventArgs e)
        {
            System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)sender;
            if (panel.VerticalScroll.Value <= 0 || panel.VerticalScroll.Maximum - panel.Height >= panel.VerticalScroll.Value)
                return;
            this.CallInitVideoDataList();
        }

        private void OnPanelRowAutoCompleteClick(object sender, EventArgs e)
        {
            this.RowAutoCompleteSelect(sender);
        }

        private void OnPanelRowAutoCompleteMouseMove(object sender, EventArgs e)
        {
            this.PanelContainerAutoComplete.Focus();
            System.Windows.Forms.Panel panel = sender as System.Windows.Forms.Panel;
            if (panel == null)
                return;
            foreach (System.Windows.Forms.Control control in (ArrangedElementCollection)this.PanelContainerAutoComplete.Controls)
                control.BackColor = System.Drawing.Color.White;
            panel.Cursor = Cursors.Hand;
            panel.BackColor = System.Drawing.Color.LightSkyBlue;
        }

        private void OnPanelRowAutoCompleteMouseLeave(object sender, EventArgs e)
        {
            System.Windows.Forms.Panel panel = sender as System.Windows.Forms.Panel;
            if (panel == null)
                return;
            panel.BackColor = System.Drawing.Color.White;
        }

        private void OnContextMenuDownloadVideoSubMenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            this.splitButtonActionVideo.Text = string.Format("{0} ({1})", (object)toolStripMenuItem.OwnerItem.Text, (object)toolStripMenuItem.Text);
            VideoStreamType tag = (VideoStreamType)toolStripMenuItem.OwnerItem.Tag;
            VideoQuality videoQuality = (VideoQuality)toolStripMenuItem.Tag;
            if (!LicenseHelper.IsGenuine)
            {
                switch (videoQuality)
                {
                    case VideoQuality._4320p:
                    case VideoQuality._4320p60fps:
                    case VideoQuality._1440p:
                    case VideoQuality._1440p60fps:
                    case VideoQuality._2160p:
                    case VideoQuality._2160p60fps:
                    case VideoQuality._3072p:
                        if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.UltraHDProSubscriptionMessageText, Strings.SubscribeToFYDPro, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                            MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
                        VideoLink videoLink = this._receivedMediaInfo.Links.GetVideoLink(tag, VideoQuality._1080p60fps);
                        videoQuality = videoLink.VideoStreamQuality;
                        this.splitButtonActionVideo.Text = string.Format("{0} ({1})", (object)toolStripMenuItem.OwnerItem.Text, (object)videoLink.MediaQuality);
                        break;
                }
            }
            switch (tag)
            {
                case VideoStreamType.Flv:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.DownloadFLV, (object)videoQuality);
                    break;
                case VideoStreamType.Mp4:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.DownloadMP4, (object)videoQuality);
                    break;
                case VideoStreamType.WebM:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.DownloadWebM, (object)videoQuality);
                    break;
                case VideoStreamType.ThreeGp:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.DownloadThreeGP, (object)videoQuality);
                    break;
            }
        }

        private void OnContextMenuConvertToAviVideoSubMenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            this.splitButtonActionVideo.Text = string.Format("{0} ({1})", (object)toolStripMenuItem.OwnerItem.Text, (object)toolStripMenuItem.Text);
            VideoQuality videoQuality = (VideoQuality)toolStripMenuItem.Tag;
            if (!LicenseHelper.IsGenuine)
            {
                switch (videoQuality)
                {
                    case VideoQuality._4320p:
                    case VideoQuality._4320p60fps:
                    case VideoQuality._1440p:
                    case VideoQuality._1440p60fps:
                    case VideoQuality._2160p:
                    case VideoQuality._2160p60fps:
                    case VideoQuality._3072p:
                        if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.UltraHDProSubscriptionMessageText, Strings.SubscribeToFYDPro, MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == DialogResult.OK)
                            MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
                        VideoLink videoLink = this._receivedMediaInfo.Links.GetVideoLink(VideoQuality._1080p60fps);
                        videoQuality = videoLink.VideoStreamQuality;
                        this.splitButtonActionVideo.Text = string.Format("{0} ({1})", (object)toolStripMenuItem.OwnerItem.Text, (object)videoLink.MediaQuality);
                        break;
                }
            }
            this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.DownloadAndConvertToAVI, (object)videoQuality);
        }

        private void OnContextMenuConvertVideoSubMenuItemClick(object sender, EventArgs e)
        {
            ToolStripMenuItem toolStripMenuItem = (ToolStripMenuItem)sender;
            this.splitButtonActionVideo.Text = string.Format("{0} ({1})", (object)toolStripMenuItem.OwnerItem.Text, (object)toolStripMenuItem.Text);
            VideoStreamType tag1 = (VideoStreamType)toolStripMenuItem.OwnerItem.Tag;
            VideoQuality tag2 = (VideoQuality)toolStripMenuItem.Tag;
            switch (tag1)
            {
                case VideoStreamType.Flv:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.ConvertToFLV, (object)tag2);
                    break;
                case VideoStreamType.Mp4:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.ConvertToMP4, (object)tag2);
                    break;
                case VideoStreamType.WebM:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.ConvertToWebM, (object)tag2);
                    break;
                case VideoStreamType.Avi:
                    this.splitButtonActionVideo.Tag = (object)new ButtonActionData(DesiredAction.ConvertToAVI, (object)tag2);
                    break;
            }
        }

        private void OnConvertButtonClick(object sender, EventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            string str1 = "Supported video formats|*.avi;*.mp4;*.flv;*.mkv;*.mpeg;*.webm|Supported audio formats|*.mp3;*.aac;*.flac;*.ogg;";
            openFileDialog1.Filter = str1;
            string str2 = "Choose file to convert";
            openFileDialog1.Title = str2;
            System.Windows.Forms.OpenFileDialog openFileDialog2 = openFileDialog1;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                this.TextBoxUrl = openFileDialog2.FileName;
                this.textBoxUrl.Text = openFileDialog2.FileName;
                this._actionConvert = true;
            }
            else
                this.Status = Strings.Canceled;
        }

        private void OnTextBoxUrlKeyDown(object sender, KeyEventArgs e)
        {
            this.OnKeyDown(e);
            switch (e.KeyData)
            {
                case Keys.Return:
                    if (this._timerSearch.IsIgnored)
                    {
                        this._timerSearch.IsIgnored = false;
                        string keyword = ((System.Windows.Forms.Control)sender).Text.Trim();
                        ThreadPool.QueueUserWorkItem((WaitCallback)(_ => this.SearchDataByKeyword(keyword)));
                        return;
                    }
                    break;
                case Keys.Escape:
                    this.SearchDropDownPanelHide(false);
                    break;
            }
            if (!e.Control)
            {
                if (e.KeyValue != 40 || !this.PanelContainerAutoComplete.Visible || this.PanelContainerAutoComplete.Controls.Count <= 0)
                    return;
                this.PanelContainerAutoComplete.Focus();
                System.Windows.Forms.Control.ControlCollection controls = this.PanelContainerAutoComplete.Controls;
                Func<System.Windows.Forms.Control, bool> func = (Func<System.Windows.Forms.Control, bool>)(c => c.BackColor == System.Drawing.Color.LightSkyBlue);
                int num = 0;
                Func<System.Windows.Forms.Control, bool> condition = null;
                if (!controls.Where(condition, num != 0).Any<System.Windows.Forms.Control>())
                    return;
                this.PanelContainerAutoComplete.Controls[0].BackColor = System.Drawing.Color.LightSkyBlue;
            }
            else
            {
                if (e.KeyCode != Keys.A)
                    return;
                this.textBoxUrl.Select(0, this.textBoxUrl.Text.Length);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void OnVisitOurWebsiteMenuItemClick(object sender, EventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("http://youtubedownloader.com/", UriKind.Absolute), "http://youtubedownloader.com/", false);
        }

        private void OnBecomeAFanMenuItemClick(object sender, EventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("https://www.facebook.com/FreeYouTubeDownloader", UriKind.Absolute), "https://www.facebook.com/FreeYouTubeDownloader", false);
        }

        private void OnDownloadLatestVersionMenuItemClick(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem((WaitCallback)(foo => this.CheckForNewVersion(true, true)));
        }

        private void OnMinimizeToTrayMenuItemClick(object sender, EventArgs e)
        {
            this.MinimizeToSystemTray();
        }

        private void OnNotifyIconClick(object sender, EventArgs e)
        {
            MouseEventArgs mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null && mouseEventArgs.Button != MouseButtons.Left)
                return;
            this.Show();
            this.Activate();
            this._notifyIcon.Visibility = Visibility.Collapsed;
        }

        private void OnAskQuestionMenuItemClick(object sender, EventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("http://youtubedownloader.com/support", UriKind.Absolute), "http://youtubedownloader.com/support", false);
        }

        private void OnMainWindowLoad(object sender, EventArgs e)
        {
            int height = this.textBoxUrl.ClientSize.Height - 2;
            System.Windows.Forms.Button button = new System.Windows.Forms.Button();
            var size = new System.Drawing.Size(18, height);
            button.Size = size;
            System.Drawing.Color transparent = System.Drawing.Color.Transparent;
            button.BackColor = transparent;
            this._clearInputButton = button;
            this._clearInputButton.Click += (EventHandler)((o, args) =>
           {
               this.SearchDropDownPanelHide(true);
               this.TextBoxUrl = (string)null;
               this.textBoxUrl.Clear();
           });
            this._clearInputButton.Location = new System.Drawing.Point(this.textBoxUrl.ClientSize.Width - this._clearInputButton.Width, -1);
            this._clearInputButton.Cursor = Cursors.Default;
            this._clearInputButton.Image = (System.Drawing.Image)Resources.Clear;
            this._clearInputButton.Dock = DockStyle.Right;
            this.textBoxUrl.Controls.Add((System.Windows.Forms.Control)this._clearInputButton);
            this._clearInputButton.Width = this._clearInputButton.Height;
            FreeYouTubeDownloader.Common.NativeMethods.User32.SendMessage(this.textBoxUrl.Handle, 211, (IntPtr)2, (IntPtr)(this._clearInputButton.Width << 16));
            this.buttonConvert.Width = this.buttonConvert.Height = this.textBoxUrl.Height + 2;
            this.buttonConvert.Parent.Width = this.buttonConvert.Width + this.buttonConvert.Location.X + this.buttonConvert.Parent.Margin.Left + this.buttonConvert.Parent.Margin.Right;
            this._additionalWidth = this.buttonConvert.FindForm().PointToClient(this.buttonConvert.Parent.PointToScreen(this.buttonConvert.Location)).X + this.buttonConvert.Width - (this.textBoxUrl.Location.X + this.textBoxUrl.Width) - 1;
            Program.NetworkMonitor.InternetAvailabilityChanged += new NetworkMonitor.InternetAvailabilityChangedEventHandler(this.OnInternetAvailabilityChanged);
            ApplicationManager.MeasurementClient.EventAsync("app", "launch", System.Windows.Forms.Application.ProductName, "Main", Program.Version);
        }

        private void OnInternetAvailabilityChanged(InternetAvailabilityEventArgs args)
        {
            FreeYouTubeDownloader.Debug.Log.Trace("EVENT MainWindow.OnInternetAvailabilityChanged", (Exception)null);
            FreeYouTubeDownloader.Debug.Log.Info(string.Format("Internet is {0}", args.HasInternetConnection ? (object)"connected" : (object)"disconnected"), (Exception)null);
            this.Connected = args.HasInternetConnection;
        }

        private void OnExtractMp3MenuItemClick(object sender, EventArgs e)
        {
            AudioQuality audioQuality = (AudioQuality)((ToolStripItem)sender).Tag;
            if (!LicenseHelper.IsGenuine && audioQuality > AudioQuality._128kbps)
            {
                if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.HighBitrateProSubscriptionMessageText, Strings.SubscribeToFYDPro, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
                audioQuality = AudioQuality._128kbps;
            }
            this.splitButtonActionAudio.Text = string.Format("{0} MP3 ({1} kbps)", (object)Strings.Download, (object)((int)audioQuality).ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ExtractMP3, (object)audioQuality);
        }

        private void OnExtractAacMenuItemClick(object sender, EventArgs e)
        {
            AudioQuality audioQuality = (AudioQuality)((ToolStripItem)sender).Tag;
            if (!LicenseHelper.IsGenuine && audioQuality > AudioQuality._128kbps)
            {
                if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.HighBitrateProSubscriptionMessageText, Strings.SubscribeToFYDPro, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
                audioQuality = AudioQuality._128kbps;
            }
            this.splitButtonActionAudio.Text = string.Format("{0} AAC ({1} kbps)", (object)Strings.Download, (object)((int)audioQuality).ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ExtractAAC, (object)audioQuality);
        }

        private void OnExtractVorbisMenuItemClick(object sender, EventArgs e)
        {
            AudioQuality audioQuality = (AudioQuality)((ToolStripItem)sender).Tag;
            if (!LicenseHelper.IsGenuine && audioQuality > AudioQuality._128kbps)
            {
                if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.HighBitrateProSubscriptionMessageText, Strings.SubscribeToFYDPro, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
                audioQuality = AudioQuality._128kbps;
            }
            this.splitButtonActionAudio.Text = string.Format("{0} Vorbis ({1} kbps)", (object)Strings.Download, (object)((int)audioQuality).ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ExtractVorbis, (object)audioQuality);
        }

        private void OnConvertToMp3MenuItemClick(object sender, EventArgs e)
        {
            this.splitButtonActionAudio.Text = string.Format("{0} ({1} kbps)", (object)Strings.ConvertToMP3, (object)128.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ConvertToMP3, (object)AudioQuality._128kbps);
        }

        private void OnConvertToAacMenuItemClick(object sender, EventArgs e)
        {
            this.splitButtonActionAudio.Text = string.Format("{0} ({1} kbps)", (object)Strings.ConvertToAAC, (object)128.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ConvertToAAC, (object)AudioQuality._128kbps);
        }

        private void OnConvertToVorbisMenuItemClick(object sender, EventArgs e)
        {
            this.splitButtonActionAudio.Text = string.Format("{0} ({1} kbps)", (object)Strings.ConvertToVorbis, (object)128.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ConvertToVorbis, (object)AudioQuality._128kbps);
        }

        private void OnAlwaysOnTopMenuItemClick(object sender, EventArgs e)
        {
            Settings.Instance.AlwaysOnTop = !Settings.Instance.AlwaysOnTop;
            this.TopMost = Settings.Instance.AlwaysOnTop;
            this.alwaysOnTopMenuItem.Checked = Settings.Instance.AlwaysOnTop;
        }

        private void OnNotifyIconBalloonTipShown(object sender, EventArgs e)
        {
            this._countBalloons = this._countBalloons + 1;
        }

        private void OnNotifyIconBalloonTipClosed(object sender, EventArgs e)
        {
            this._countBalloons = this._countBalloons - 1;
            if (this._threadNotifyIconClose != null && this._threadNotifyIconClose.ThreadState != System.Threading.ThreadState.Stopped)
                return;
            this._threadNotifyIconClose = new Thread(new ThreadStart(this.NotifyIconClose));
            this._threadNotifyIconClose.Start();
        }

        private void OnDownloadsListKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData != Keys.Delete)
                return;
            foreach (DownloadItem selectedObject in (IEnumerable)this.olvDownloads.SelectedObjects)
            {
                selectedObject.RaiseSetStatus -= new EventHandler(this.SetStatus);
                if (selectedObject.State == DownloadState.Converting)
                    DownloadItemViewModel.Instance.CancelConversion(selectedObject);
                else
                    DownloadItemViewModel.Instance.Remove(selectedObject);
            }
            this.Status = this.GetLocalizedStatusText(DownloadState.Canceled);
        }

        private void OnDownloadsSelectionChanged(object sender, EventArgs e)
        {
            List<DownloadItem> list = this.olvDownloads.SelectedObjects.Cast<DownloadItem>().Where<DownloadItem>((Func<DownloadItem, bool>)(item => item.State == DownloadState.Completed)).ToList<DownloadItem>();
            if (list.Count > 0)
            {
                double milliseconds = list.Sum<DownloadItem>((Func<DownloadItem, double>)(i => i.Duration));
                long bytes = list.Sum<DownloadItem>((Func<DownloadItem, long>)(i => i.Size));
                if (milliseconds <= 0.0 && bytes <= 0L)
                    return;
                string str = string.Format(Strings.SelectedFiles, (object)list.Count);
                if (bytes > 0L)
                {
                    str = str + ": " + bytes.ToFileSize(true);
                    if (milliseconds > 0.0)
                        str = str + " - " + milliseconds.MillisecondsToHumanFriendlyRepresentation(false, false, true);
                }
                else if (milliseconds > 0.0)
                    str = str + ": " + milliseconds.MillisecondsToHumanFriendlyRepresentation(false, false, true);
                this.Status = str;
            }
            else
                this.Status = Strings.Done;
        }

        private void OnButtonVideoFilesClick(object sender, EventArgs e)
        {
            string videosDownloadFolder = Settings.Instance.DefaultVideosDownloadFolder;
            MainWindow.RunProcessStart(Directory.Exists(videosDownloadFolder), videosDownloadFolder, true);
        }

        private void OnButtonAudioFilesClick(object sender, EventArgs e)
        {
            string audiosDownloadFolder = Settings.Instance.DefaultAudiosDownloadFolder;
            MainWindow.RunProcessStart(Directory.Exists(audiosDownloadFolder), audiosDownloadFolder, true);
        }

        private void OnAskFileNameAndFolderpMenuItemClick(object sender, EventArgs e)
        {
            this.askFileNameAndFolderToolStripMenuItem.Checked = !this.askFileNameAndFolderToolStripMenuItem.Checked;
            Settings.Instance.FileLocationOption = this.askFileNameAndFolderToolStripMenuItem.Checked ? FileLocationOption.AskMeForNameAndFolder : FileLocationOption.OriginalNameAndDefaultFolder;
        }

        private void OnLabelHelpClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("http://youtubedownloader.com/en/how-to-download-youtube-videos", UriKind.Absolute), "http://youtubedownloader.com/en/how-to-download-youtube-videos", false);
        }

        private void OnReadDocumentationMenuItemClick(object sender, EventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("http://youtubedownloader.com/en/how-to-download-youtube-videos", UriKind.Absolute), "http://youtubedownloader.com/en/how-to-download-youtube-videos", false);
        }

        private void OnButtonFacebookClick(object sender, EventArgs e)
        {
            string youtubeVideoIdByUrl = YoutubeHelper.GetYoutubeVideoIdByUrl(this.TextBoxUrl);
            string str = string.IsNullOrWhiteSpace(youtubeVideoIdByUrl) ? "https://www.facebook.com/sharer/sharer.php?u=https%3A//youtubedownloader.com/" : string.Format("https://www.facebook.com/sharer/sharer.php?u=https%3A//savemedia.com/watch?v={0}", (object)youtubeVideoIdByUrl);
            MainWindow.RunProcessStart(MainWindow.VerifyUrl(str, UriKind.Absolute), str, false);
        }

        private void OnShareVideoOnTwitterDownloadClick(object sender, EventArgs e)
        {
            string youtubeVideoIdByUrl = YoutubeHelper.GetYoutubeVideoIdByUrl(this.TextBoxUrl);
            string str = string.IsNullOrWhiteSpace(youtubeVideoIdByUrl) ? "https://twitter.com/intent/tweet?related=FreeYouTubeDL&text=YouTube%20Downloader%20-%20Download%20and%20Convert%20Videos%20for%20Free&url=https%3A//youtubedownloader.com/" : string.Format("https://twitter.com/intent/tweet?related=FreeYouTubeDL&text=YouTube%20Downloader%20-%20Download%20and%20Convert%20Videos%20for%20Free&url=https%3A//savemedia.com/watch?v={0}", (object)youtubeVideoIdByUrl);
            MainWindow.RunProcessStart(MainWindow.VerifyUrl(str, UriKind.Absolute), str, false);
        }

        private void OnTextBoxUrlClick(object sender, EventArgs e)
        {
            if (this.PanelWrapperContainerAutoComplete.Visible || !this.textBoxUrl.Text.Equals(Strings.PasteURLHere) && !this.textBoxUrl.Text.IsValidFileName())
                return;
            this.textBoxUrl.Clear();
        }

        private void OnPictureBoxOpenFileClick(object sender, EventArgs e)
        {
            this.PreparePath(this.TextBoxUrl, true);
        }

        private void OnPictureBoxCopyPathClick(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(this.TextBoxUrl);
            this.Status = Strings.VideoURLCopiedToClipboard;
        }

        private void OnPanelContainerAutoCompletePreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyValue)
            {
                case 38:
                    this.PanelContainerAutoCompleteNavigation(false);
                    break;
                case 40:
                    this.PanelContainerAutoCompleteNavigation(true);
                    break;
                case 13:
                    this.RowAutoCompleteSelect((object)this.PanelContainerAutoComplete.Controls.ToList(false).FirstOrDefault<System.Windows.Forms.Control>((Func<System.Windows.Forms.Control, bool>)(c => c.BackColor == System.Drawing.Color.LightSkyBlue)));
                    break;
                case 27:
                    this.SearchDropDownPanelHide(false);
                    break;
            }
        }

        private void OnReportProblemMenuItemClick(object sender, EventArgs e)
        {
            new ProblemReporterWindow().Show((IWin32Window)this);
        }

        private void OnTextBoxUrlEnter(object sender, EventArgs e)
        {
            if (this.PanelWrapperContainerAutoComplete.Visible)
                return;
            this.SearchDropDownPanelShow(true);
        }

        private void OnSignupForUpdatesClick(object sender, EventArgs e)
        {
            Thread thread = new Thread((ThreadStart)(() =>
           {
               IFrameWindow iframeWindow = new IFrameWindow("https://youtubedownloader.com/signup");
               iframeWindow.Icon = this.Icon;
               int num = (int)iframeWindow.ShowDialog();
               iframeWindow.Dispose();
           }));
            int num1 = 0;
            thread.SetApartmentState((ApartmentState)num1);
            thread.Start();
        }

        private void OnCompatibleURLNotificationCheckedChanged(object sender, EventArgs e)
        {
            this.compatibleURLNotificationToolStripMenuItem.Checked = Settings.Instance.NotifyUrlInClipboard = !this.compatibleURLNotificationToolStripMenuItem.Checked;
        }

        private void OnNotifyIconBalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            this.Activate();
            this._notifyIcon.Visibility = Visibility.Collapsed;
        }

        private void OnAboutClick(object sender, EventArgs e)
        {
            new AboutWindow().Show((IWin32Window)this);
        }

        private void OnUpgradeToProVersionClick(object sender, EventArgs e)
        {
            if (new ActivationWindow().ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            this.upgradeToProVersionToolStripMenuItem.Visible = false;
            this.deactivateFYDProToolStripMenuItem.Visible = true;
            this.Text = LicenseHelper.GetAppTitle(true);
            this.OnTextBoxUrlTextChanged((object)this.textBoxUrl, new EventArgs());
        }

        private void OnDeactivateFydProClick(object sender, EventArgs e)
        {
            if (new DeactivationWindow().ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            this.upgradeToProVersionToolStripMenuItem.Visible = true;
            this.deactivateFYDProToolStripMenuItem.Visible = false;
            this.Text = "Free YouTube Downloader";
            this.OnTextBoxUrlTextChanged((object)this.textBoxUrl, new EventArgs());
            this.autoDownloadMenuItem.Checked = this.videoAutoDownloadMenuItem.Checked = Settings.Instance.AutomaticallyDownloadVideo = false;
            this.autoDownloadMenuItem.Checked = this.audioAutoDownloadMenuItem.Checked = Settings.Instance.AutomaticallyDownloadAudio = false;
            this.splitButtonActionVideo.Image = (System.Drawing.Image)null;
            this.splitButtonActionAudio.Image = (System.Drawing.Image)null;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            switch (e.CloseReason)
            {
                case CloseReason.WindowsShutDown:
                case CloseReason.TaskManagerClosing:
                case CloseReason.ApplicationExitCall:
                    Program.Exit();
                    break;
                case CloseReason.UserClosing:
                    switch (Settings.Instance.CloseAppAction)
                    {
                        case CloseAppActions.Prompt:
                            switch (new ExitPromptWindow().ShowDialog((IWin32Window)this))
                            {
                                case DialogResult.Cancel:
                                    e.Cancel = true;
                                    return;
                                case DialogResult.Yes:
                                    Program.Exit();
                                    return;
                                case DialogResult.No:
                                    this.MinimizeToSystemTray();
                                    e.Cancel = true;
                                    return;
                                default:
                                    return;
                            }
                        case CloseAppActions.ExitApplication:
                            Program.Exit();
                            return;
                        case CloseAppActions.MinimizeToTray:
                            this.MinimizeToSystemTray();
                            e.Cancel = true;
                            return;
                        default:
                            return;
                    }
            }
        }

        private void OnColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (e.ColumnIndex != 1)
                return;
            Settings.Instance.NameColumnWidth = this.olvColumnName.Width;
        }

        private void OnPictureBoxSearchResultMouseEnter(object sender, EventArgs e)
        {
            if (this.pictureBoxSearchResult.Image != null)
                return;
            this.pictureBoxSearchResult.Image = this._thumbnailOverlay;
        }

        private void OnPictureBoxSearchResultMouseLeave(object sender, EventArgs e)
        {
            this.pictureBoxSearchResult.Image = (System.Drawing.Image)null;
        }

        private void OnPictureBoxSearchResultClick(object sender, EventArgs e)
        {
            this.PlayYouTubeUrl();
        }

        private void OnAutoDownloadVideoMenuItemClick(object sender, EventArgs e)
        {
            if (!LicenseHelper.IsGenuine)
            {
                this.videoAutoDownloadMenuItem.Checked = false;
                if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.OnlyInProVersion, Strings.SubscribeToFYDPro, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                    return;
                MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
            }
            else
            {
                Settings.Instance.AutomaticallyDownloadVideo = this.videoAutoDownloadMenuItem.Checked;
                if (!Settings.Instance.AutomaticallyDownloadVideo && !Settings.Instance.AutomaticallyDownloadAudio)
                {
                    this.autoDownloadMenuItem.Checked = false;
                }
                else
                {
                    if (!Settings.Instance.AutomaticallyDownloadVideo)
                        return;
                    this.autoDownloadMenuItem.Checked = true;
                }
            }
        }

        private void OnAutoDownloadAudioMenuItemClick(object sender, EventArgs e)
        {
            if (!LicenseHelper.IsGenuine)
            {
                this.audioAutoDownloadMenuItem.Checked = false;
                if (System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.OnlyInProVersion, Strings.SubscribeToFYDPro, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                    return;
                MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
            }
            else
            {
                Settings.Instance.AutomaticallyDownloadAudio = this.audioAutoDownloadMenuItem.Checked;
                if (!Settings.Instance.AutomaticallyDownloadVideo && !Settings.Instance.AutomaticallyDownloadAudio)
                {
                    this.autoDownloadMenuItem.Checked = false;
                }
                else
                {
                    if (!Settings.Instance.AutomaticallyDownloadAudio)
                        return;
                    this.autoDownloadMenuItem.Checked = true;
                }
            }
        }

        private void CheckForSupportedUrlInClipboard(bool fromWndProc = false)
        {
            if (!MainWindow.ClipboardContainsText())
                return;
            string text = System.Windows.Forms.Clipboard.GetText();
            if (!DownloadManager.HasProviderForUrl(text) || !(text != this.textBoxUrl.Text) || object.Equals((object)this.TextBoxUrl, (object)text))
                return;
            if (!fromWndProc && !this._ignoreAutoDownloadFlag)
                this._ignoreAutoDownloadFlag = true;
            this.RowSearchResultHide();
            this.SetEnableDownloadButtons(false, true);
            this.TextBoxUrl = text;
            this.textBoxUrl.Text = text;
            if (!fromWndProc)
                return;
            MeasurementClient measurementClient = ApplicationManager.MeasurementClient;
            AppEventData appEventData = new AppEventData();
            appEventData.ApplicationName = System.Windows.Forms.Application.ProductName;
            appEventData.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
            string str1 = "URL pasted";
            appEventData.EventAction = str1;
            string str2 = "app";
            appEventData.EventCategory = str2;
            string str3 = "Main";
            appEventData.ScreenName = str3;
            measurementClient.EventAsync((EventData)appEventData);
            if (!Settings.Instance.NotifyUrlInClipboard || this.autoDownloadMenuItem.Checked)
                return;
            this.ShowBalloon(System.Windows.Forms.Application.ProductName, Strings.CompatibleVideoURLDetected, BalloonIcon.Info);
        }

        private void CheckForNewVersion(bool showMessageAlreadyHaveLatestVersion = false, bool autoDownload = false)
        {
            if (!showMessageAlreadyHaveLatestVersion && !autoDownload)
                Thread.Sleep(5000);
            try
            {
                string json;
                try
                {
                    json = new WebClient().DownloadString("http://youtubedownloader.com/files/version.json");
                }
                catch (WebException ex)
                {
                    json = new WebClient().DownloadString("http://wd.getyoutubedownloader.com/version/version.json");
                }
                JObject jobject = JObject.Parse(json);
                string version = (string)jobject["Version"];
                if (Assembly.GetExecutingAssembly().GetName().Version.CompareTo(new Version(version)) < 0)
                {
                    if (!(System.Windows.Forms.MessageBox.Show(string.Format(Strings.NewVersionIsAvailableMessage, (object)version), Strings.NewVersionIsAvailableTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes | autoDownload))
                        return;
                    string fileName = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), ".exe");
                    this.DownloadLatestInstaller(LicenseHelper.IsGenuine ? (string)jobject["UrlPro"] : (string)jobject["Url"], fileName);
                }
                else
                {
                    if (!showMessageAlreadyHaveLatestVersion)
                        return;
                    int num = (int)System.Windows.Forms.MessageBox.Show(FreeYouTubeDownloader.Localization.Localization.Instance.GetString("AlreadyHaveTheLatestVersion", Settings.Instance.LanguageCode), "Free YouTube Downloader", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (!autoDownload)
                        return;
                    string fileName = Path.ChangeExtension(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()), ".exe");
                    this.DownloadLatestInstaller((string)jobject["Url"], fileName);
                }
            }
            catch (Exception ex)
            {
                this.ExecuteOnUIThread((Action)(() => this.Status = FreeYouTubeDownloader.Localization.Localization.Instance.GetString("UnableToCheckForUpdates", Settings.Instance.LanguageCode)));
            }
        }

        private void DownloadLatestInstaller(string uri, string fileName)
        {
            WebClient webClient = new WebClient();
            DownloadProgressChangedEventHandler changedEventHandler = (DownloadProgressChangedEventHandler)((sender, args) => this._syncContext.Post((SendOrPostCallback)(state => this.Status = string.Format(Strings.DownloadingUpdate, (object)args.ProgressPercentage)), (object)null));
            webClient.DownloadProgressChanged += changedEventHandler;
            AsyncCompletedEventHandler completedEventHandler = (AsyncCompletedEventHandler)((sender, args) =>
           {
               this._syncContext.Post((SendOrPostCallback)(state => this.Status = Strings.Done), (object)null);
               if (args.Error != null || args.Cancelled)
               {
                   if (!System.IO.File.Exists(fileName))
                       return;
                   FileSystemUtil.SafeDeleteFile(fileName);
               }
               else
                   MainWindow.RunDefaultApp((string)args.UserState, true, true);
           });
            webClient.DownloadFileCompleted += completedEventHandler;
            Uri address = new Uri(uri, UriKind.Absolute);
            string fileName1 = fileName;
            string str = fileName;
            webClient.DownloadFileAsync(address, fileName1, (object)str);
        }

        private bool GetFileNameAccordingToPreferences(FileLocationOption fileLocationOption, bool isAudioFile, string title, string fileExtension, out string fileName)
        {
            fileName = string.Empty;
            string path1 = isAudioFile ? Settings.Instance.DefaultAudiosDownloadFolder : Settings.Instance.DefaultVideosDownloadFolder;
            if (fileLocationOption != FileLocationOption.OriginalNameAndDefaultFolder)
            {
                if (fileLocationOption != FileLocationOption.AskMeForNameAndFolder)
                    return false;
                System.Windows.Forms.SaveFileDialog saveFileDialog1;
                if (!this._actionConvert)
                {
                    System.Windows.Forms.SaveFileDialog saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
                    string str1 = string.Format("{0}.{1}", (object)title, (object)fileExtension).RemoveInvalidFileNameCharacters();
                    saveFileDialog2.FileName = str1;
                    string str2 = string.Format("{0}|*.{0}", (object)fileExtension);
                    saveFileDialog2.Filter = str2;
                    string str3 = path1;
                    saveFileDialog2.InitialDirectory = str3;
                    saveFileDialog1 = saveFileDialog2;
                }
                else
                {
                    System.Windows.Forms.SaveFileDialog saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
                    string str1 = fileExtension;
                    saveFileDialog2.Filter = str1;
                    string str2 = "Select output format and file name";
                    saveFileDialog2.Title = str2;
                    string withoutExtension = Path.GetFileNameWithoutExtension(title);
                    saveFileDialog2.FileName = withoutExtension;
                    string str3 = path1;
                    saveFileDialog2.InitialDirectory = str3;
                    saveFileDialog1 = saveFileDialog2;
                }
                if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                    return false;
                fileName = saveFileDialog1.FileName;
                return true;
            }
            fileName = !this._actionConvert ? Path.Combine(path1, string.Format("{0}.{1}", (object)title, (object)fileExtension).RemoveInvalidFileNameCharacters()) : Path.Combine(path1, Path.ChangeExtension(Path.GetFileName(title), fileExtension));
            return true;
        }

        private object[] GetVideoQualityInfo(object[] parameters)
        {
            MediaInfo parameter1 = (MediaInfo)parameters[1];
            if (parameter1.Exception != null)
                throw parameter1.Exception;
            MediaLink parameter2 = (MediaLink)parameters[2];
            try
            {
                parameter2.UpdateLink();
                if (!parameter2.HasLength)
                {
                    HttpWebResponse response = (HttpWebResponse)WebRequest.Create(parameter2.Url).GetResponse();
                    parameter2.Length = response.ContentLength;
                    response.Close();
                }
                VideoQualityInfo videoQualityInfo1 = new VideoQualityInfo();
                videoQualityInfo1.File = parameter2;
                string mediaFormat = parameter2.MediaFormat;
                videoQualityInfo1.FormatName = mediaFormat;
                long length = parameter2.Length;
                videoQualityInfo1.Size = length;
                VideoQualityInfo videoQualityInfo2 = videoQualityInfo1;
                parameters[2] = (object)videoQualityInfo2;
                this.ExecuteOnUIThread((Action)(() => this.Cursor = Cursors.Default));
            }
            catch
            {
                if ((bool)parameters[3])
                    parameters[2] = (object)null;
                else
                    throw;
            }
            return parameters;
        }

        private double GetDurationOfSelectedVideo()
        {
            if (string.IsNullOrWhiteSpace(this.lblSearchResultTime.Text) || this.lblSearchResultTime.Tag == null)
                return 0.0;
            return (double)this.lblSearchResultTime.Tag;
        }

        private void VideoQualityInfoResult(IAsyncResult result)
        {
            if (!result.IsCompleted)
                return;
            MainWindow.VideoQualityInfoManager asyncDelegate = (MainWindow.VideoQualityInfoManager)((AsyncResult)result).AsyncDelegate;
            object[] objArray;
            try
            {
                objArray = asyncDelegate.EndInvoke(result);
            }
            catch (Exception ex)
            {
                int num = (int)System.Windows.Forms.MessageBox.Show(ex.Message, Strings.DownloadingError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            if (objArray[2] != null)
                this.ProcessDownloadAccordingToDesiredAction((DesiredAction)objArray[0], (MediaInfo)objArray[1], (VideoQualityInfo)objArray[2], (bool)objArray[3], (bool)objArray[4], (string)objArray[5], (ConversionProfile)objArray[6]);
            else
                this.ProcessDownloadAccordingToDesiredAction((DesiredAction)objArray[0], (MediaInfo)objArray[1], (VideoQualityInfo)objArray[2], false, (bool)objArray[4], (string)objArray[5], (ConversionProfile)objArray[6]);
        }

        private void ProcessDownloadAccordingToDesiredAction(DesiredAction action, MediaInfo mediaInfo, bool fromWebSide = false)
        {
            bool flag = false;
            string str1 = string.Empty;
            ConversionProfile conversionProfile1 = (ConversionProfile)null;
            MediaLink mediaLink = (MediaLink)null;
            this.Status = string.Format("{0} ...", (object)Strings.AnalyzingStreamsQuality);
            this.Cursor = Cursors.WaitCursor;
            MainWindow.VideoQualityInfoManager qualityInfoManager = new MainWindow.VideoQualityInfoManager(this.GetVideoQualityInfo);
            switch (action)
            {
                case DesiredAction.DownloadMP4:
                    mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.Mp4, Settings.Instance.DesiredDownloadVideoQuality);
                    str1 = "mp4";
                    MeasurementClient measurementClient1 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData1 = new AppEventData();
                    appEventData1.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData1.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData1.EventAction = "mp4";
                    appEventData1.EventCategory = "download";
                    appEventData1.EventLabel = Settings.Instance.DesiredDownloadVideoQuality.ToString().TrimStart('_');
                    appEventData1.ScreenName = "Main";
                    AppEventData appEventData2 = appEventData1;
                    measurementClient1.EventAsync((EventData)appEventData2);
                    break;
                case DesiredAction.DownloadFLV:
                    mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.Flv, Settings.Instance.DesiredDownloadVideoQuality);
                    str1 = "flv";
                    MeasurementClient measurementClient2 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData3 = new AppEventData();
                    appEventData3.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData3.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData3.EventAction = "flv";
                    appEventData3.EventCategory = "download";
                    appEventData3.EventLabel = Settings.Instance.DesiredDownloadVideoQuality.ToString().TrimStart('_');
                    appEventData3.ScreenName = "Main";
                    AppEventData appEventData4 = appEventData3;
                    measurementClient2.EventAsync((EventData)appEventData4);
                    break;
                case DesiredAction.DownloadWebM:
                    mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.WebM, Settings.Instance.DesiredDownloadVideoQuality);
                    str1 = "webm";
                    MeasurementClient measurementClient3 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData5 = new AppEventData();
                    appEventData5.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData5.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData5.EventAction = "webm";
                    appEventData5.EventCategory = "download";
                    appEventData5.EventLabel = Settings.Instance.DesiredDownloadVideoQuality.ToString().TrimStart('_');
                    appEventData5.ScreenName = "Main";
                    AppEventData appEventData6 = appEventData5;
                    measurementClient3.EventAsync((EventData)appEventData6);
                    break;
                case DesiredAction.DownloadThreeGP:
                    mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.ThreeGp, Settings.Instance.DesiredDownloadVideoQuality);
                    str1 = "3gp";
                    MeasurementClient measurementClient4 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData7 = new AppEventData();
                    appEventData7.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData7.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData7.EventAction = "3gp";
                    appEventData7.EventCategory = "download";
                    appEventData7.EventLabel = Settings.Instance.DesiredDownloadVideoQuality.ToString().TrimStart('_');
                    appEventData7.ScreenName = "Main";
                    AppEventData appEventData8 = appEventData7;
                    measurementClient4.EventAsync((EventData)appEventData8);
                    break;
                case DesiredAction.ExtractMP3:
                    flag = true;
                    str1 = "mp3";
                    Mp3ConversionProfile conversionProfile2 = new Mp3ConversionProfile();
                    string str2 = string.Format("{0}k", (int)Settings.Instance.DesiredDownloadAudioQuality);
                    conversionProfile2.AudioBitRate = str2;
                    conversionProfile1 = (ConversionProfile)conversionProfile2;
                    try
                    {
                        mediaLink = (MediaLink)mediaInfo.Links.GetAudioLink(Settings.Instance.DesiredDownloadAudioQuality);
                    }
                    catch (InvalidOperationException ex)
                    {
                        mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.Mp4, VideoQuality._480p);
                    }
                    MeasurementClient measurementClient5 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData9 = new AppEventData();
                    appEventData9.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData9.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData9.EventAction = "mp3";
                    appEventData9.EventCategory = "download";
                    appEventData9.EventLabel = Settings.Instance.DesiredDownloadAudioQuality.ToString().TrimStart('_');
                    appEventData9.ScreenName = "Main";
                    AppEventData appEventData10 = appEventData9;
                    measurementClient5.EventAsync((EventData)appEventData10);
                    break;
                case DesiredAction.ExtractAAC:
                    flag = true;
                    str1 = "aac";
                    AacConversionProfile conversionProfile3 = new AacConversionProfile();
                    string str3 = string.Format("{0}k", (int)Settings.Instance.DesiredDownloadAudioQuality);
                    conversionProfile3.AudioBitRate = str3;
                    conversionProfile1 = (ConversionProfile)conversionProfile3;
                    try
                    {
                        mediaLink = (MediaLink)mediaInfo.Links.GetAudioLink(Settings.Instance.DesiredDownloadAudioQuality);
                    }
                    catch (InvalidOperationException ex)
                    {
                        mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.Mp4, VideoQuality._480p);
                    }
                    MeasurementClient measurementClient6 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData11 = new AppEventData();
                    appEventData11.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData11.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData11.EventAction = "aac";
                    appEventData11.EventCategory = "download";
                    appEventData11.EventLabel = Settings.Instance.DesiredDownloadAudioQuality.ToString().TrimStart('_');
                    appEventData11.ScreenName = "Main";
                    AppEventData appEventData12 = appEventData11;
                    measurementClient6.EventAsync((EventData)appEventData12);
                    break;
                case DesiredAction.ExtractVorbis:
                    flag = true;
                    str1 = "ogg";
                    VorbisConversionProfile conversionProfile4 = new VorbisConversionProfile();
                    string str4 = string.Format("{0}k", (int)Settings.Instance.DesiredDownloadAudioQuality);
                    conversionProfile4.AudioBitRate = str4;
                    conversionProfile1 = (ConversionProfile)conversionProfile4;
                    try
                    {
                        mediaLink = (MediaLink)mediaInfo.Links.GetAudioLink(Settings.Instance.DesiredDownloadAudioQuality);
                    }
                    catch (InvalidOperationException ex)
                    {
                        mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(VideoStreamType.Mp4, VideoQuality._480p);
                    }
                    MeasurementClient measurementClient7 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData13 = new AppEventData();
                    appEventData13.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData13.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData13.EventAction = "ogg";
                    appEventData13.EventCategory = "download";
                    appEventData13.EventLabel = Settings.Instance.DesiredDownloadAudioQuality.ToString().TrimStart('_');
                    appEventData13.ScreenName = "Main";
                    AppEventData appEventData14 = appEventData13;
                    measurementClient7.EventAsync((EventData)appEventData14);
                    break;
                case DesiredAction.DownloadAndConvertToAVI:
                    mediaLink = (MediaLink)mediaInfo.Links.GetVideoLink(Settings.Instance.DesiredDownloadVideoQuality);
                    int frameHeight = Settings.Instance.DesiredDownloadVideoQuality.GetFrameHeight();
                    str1 = "avi";
                    AviConversionProfile conversionProfile5 = new AviConversionProfile();
                    string str5 = frameHeight.ToString((IFormatProvider)CultureInfo.InvariantCulture);
                    conversionProfile5.VideoScale = str5;
                    conversionProfile1 = (ConversionProfile)conversionProfile5;
                    MeasurementClient measurementClient8 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData15 = new AppEventData();
                    appEventData15.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData15.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    appEventData15.EventAction = "avi";
                    appEventData15.EventCategory = "download";
                    appEventData15.EventLabel = Settings.Instance.DesiredDownloadVideoQuality.ToString().TrimStart('_');
                    appEventData15.ScreenName = "Main";
                    AppEventData appEventData16 = appEventData15;
                    measurementClient8.EventAsync((EventData)appEventData16);
                    break;
            }
            string fileName = string.Format("{0}.{1}", (object)mediaInfo.Title.RemoveInvalidFileNameCharacters(), (object)str1);
            double durationOfSelectedVideo = this.GetDurationOfSelectedVideo();
            DownloadItemViewModel.Instance.Add((MainWindow)null, mediaInfo.Title, fileName, durationOfSelectedVideo, mediaInfo, (VideoQualityInfo)null, conversionProfile1, mediaLink != null ? mediaLink.Url : (string)null, true);
            qualityInfoManager.BeginInvoke(new object[7]
            {
        (object) action,
        (object) mediaInfo,
        (object) mediaLink,
        (object) fromWebSide,
        (object) flag,
        (object) str1,
        (object) conversionProfile1
            }, new AsyncCallback(this.VideoQualityInfoResult), (object)"finish");
        }

        private void ProcessDownloadAccordingToDesiredAction(DesiredAction action, MediaInfo mediaInfo, VideoQualityInfo videoQualityInfo, bool fromWebSide, bool isAudioFile, string fileExtension, ConversionProfile profile)
        {
            if (this.InvokeRequired)
                this.Invoke((Delegate)new Action<DesiredAction, MediaInfo, VideoQualityInfo, bool, bool, string, ConversionProfile>(this.ProcessDownloadAccordingToDesiredAction), (object)action, (object)mediaInfo, (object)videoQualityInfo, (object)fromWebSide, (object)isAudioFile, (object)fileExtension, (object)profile);
            else if (videoQualityInfo == null)
            {
                if (!fromWebSide)
                {
                    DownloadProvider providerFromDomain = DownloadProvider.GetDownloadProviderFromDomain(mediaInfo.SourceUrl);
                    if (providerFromDomain == null)
                        return;
                    string sourceUrl = mediaInfo.SourceUrl;
                    Action<MediaInfo> callback = (Action<MediaInfo>)(info => this.ProcessDownloadAccordingToDesiredAction(action, info, false));
                    providerFromDomain.ReceiveMediaInfoFromServerSide(sourceUrl, callback);
                }
                else
                {
                    DownloadItem downloadItem = this.olvDownloads.Objects.Cast<DownloadItem>().FirstOrDefault<DownloadItem>((Func<DownloadItem, bool>)(item =>
                   {
                       if (item.State == DownloadState.AnalizeSource && item.Title.Equals(mediaInfo.Title))
                           return item.SourceUrl.Equals(mediaInfo.SourceUrl);
                       return false;
                   }));
                    if (downloadItem != null)
                        this.olvDownloads.RemoveObject((object)downloadItem);
                    this.Status = Strings.AnalyzingFailed;
                    FreeYouTubeDownloader.Debug.Log.Warning("AnalyzingFailed => " + mediaInfo.SourceUrl, (Exception)null);
                    this.Cursor = Cursors.Default;
                }
            }
            else
            {
                string fileName;
                if (this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, isAudioFile, mediaInfo.Title, fileExtension, out fileName))
                {
                    double durationOfSelectedVideo = this.GetDurationOfSelectedVideo();
                    DownloadItemViewModel.Instance.Add(this, mediaInfo.Title, fileName, durationOfSelectedVideo, mediaInfo, videoQualityInfo, profile, (string)null, false);
                }
                else
                {
                    this.Status = Strings.Canceled;
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void ProcessConversionAccordingToDesiredAction(DesiredAction action, string fileName)
        {
            bool flag = false;
            string fileName1 = string.Empty;
            switch (action)
            {
                case DesiredAction.ConvertToMP4:
                    MeasurementClient measurementClient1 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData1 = new AppEventData();
                    appEventData1.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData1.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str1 = "mp4";
                    appEventData1.EventAction = str1;
                    string str2 = "convert";
                    appEventData1.EventCategory = str2;
                    string str3 = "Main";
                    appEventData1.ScreenName = str3;
                    measurementClient1.EventAsync((EventData)appEventData1);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, false, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "MP4|*.mp4" : "mp4", out fileName1);
                    break;
                case DesiredAction.ConvertToAVI:
                    MeasurementClient measurementClient2 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData2 = new AppEventData();
                    appEventData2.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData2.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str4 = "avi";
                    appEventData2.EventAction = str4;
                    string str5 = "convert";
                    appEventData2.EventCategory = str5;
                    string str6 = "Main";
                    appEventData2.ScreenName = str6;
                    measurementClient2.EventAsync((EventData)appEventData2);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, false, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "AVI|*.avi" : "avi", out fileName1);
                    break;
                case DesiredAction.ConvertToFLV:
                    MeasurementClient measurementClient3 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData3 = new AppEventData();
                    appEventData3.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData3.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str7 = "flv";
                    appEventData3.EventAction = str7;
                    string str8 = "convert";
                    appEventData3.EventCategory = str8;
                    string str9 = "Main";
                    appEventData3.ScreenName = str9;
                    measurementClient3.EventAsync((EventData)appEventData3);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, false, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "FLV|*.flv" : "flv", out fileName1);
                    break;
                case DesiredAction.ConvertToWebM:
                    MeasurementClient measurementClient4 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData4 = new AppEventData();
                    appEventData4.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData4.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str10 = "webm";
                    appEventData4.EventAction = str10;
                    string str11 = "convert";
                    appEventData4.EventCategory = str11;
                    string str12 = "Main";
                    appEventData4.ScreenName = str12;
                    measurementClient4.EventAsync((EventData)appEventData4);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, false, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "WebM|*.webm" : "webm", out fileName1);
                    break;
                case DesiredAction.ConvertToMP3:
                    MeasurementClient measurementClient5 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData5 = new AppEventData();
                    appEventData5.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData5.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str13 = "mp3";
                    appEventData5.EventAction = str13;
                    string str14 = "convert";
                    appEventData5.EventCategory = str14;
                    string str15 = "Main";
                    appEventData5.ScreenName = str15;
                    measurementClient5.EventAsync((EventData)appEventData5);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, true, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "MP3|*.mp3" : "mp3", out fileName1);
                    break;
                case DesiredAction.ConvertToAAC:
                    MeasurementClient measurementClient6 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData6 = new AppEventData();
                    appEventData6.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData6.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str16 = "aac";
                    appEventData6.EventAction = str16;
                    string str17 = "convert";
                    appEventData6.EventCategory = str17;
                    string str18 = "Main";
                    appEventData6.ScreenName = str18;
                    measurementClient6.EventAsync((EventData)appEventData6);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, true, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "AAC|*.aac" : "aac", out fileName1);
                    break;
                case DesiredAction.ConvertToVorbis:
                    MeasurementClient measurementClient7 = ApplicationManager.MeasurementClient;
                    AppEventData appEventData7 = new AppEventData();
                    appEventData7.ApplicationName = System.Windows.Forms.Application.ProductName;
                    appEventData7.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
                    string str19 = "ogg";
                    appEventData7.EventAction = str19;
                    string str20 = "convert";
                    appEventData7.EventCategory = str20;
                    string str21 = "Main";
                    appEventData7.ScreenName = str21;
                    measurementClient7.EventAsync((EventData)appEventData7);
                    flag = this.GetFileNameAccordingToPreferences(Settings.Instance.FileLocationOption, true, fileName, Settings.Instance.FileLocationOption == FileLocationOption.AskMeForNameAndFolder ? "OGG|*.ogg" : "ogg", out fileName1);
                    break;
            }
            if (flag)
            {
                double durationOfSelectedVideo = this.GetDurationOfSelectedVideo();
                DownloadItemViewModel.Instance.AddConversion(this, fileName, fileName1, durationOfSelectedVideo);
            }
            else
                this.Status = Strings.Canceled;
        }

        private void GetMediaInfo(string pathToSource, bool initPanelSearchResult, bool ignoreAutoDownload)
        {
            try
            {
                this._actionConvert = false;
                this.Status = string.Format("{0} ...", (object)Strings.ReceivingDownloadLinks);
                this.Cursor = Cursors.WaitCursor;
                DownloadManager.GetProviderForUrl(pathToSource).ReceiveMediaInfo(pathToSource, (Action<MediaInfo>)(info =>
               {
                   this._receivedMediaInfo = info;
                   bool flag = HttpUtil.Instance.LastOperationStatus == WebExceptionStatus.Success;
                   if (initPanelSearchResult && info.Exception == null)
                       flag = this.InitPanelSearchResultByYoutubeUrl(pathToSource);
                   if (flag && ((IEnumerable<IMediaLink>)this._receivedMediaInfo.Links).Any<IMediaLink>())
                   {
                       MainWindow.SetControlEnable((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, false);
                       this.SetEnableDownloadButtons(true, false);
                       this.InitDropDownMenuDownload(false, false, (string)null);
                       this.ExecuteOnUIThread((Action)(() =>
               {
                       this.TextBoxUrl = pathToSource;
                       this.SetTooltipToFacebookAndTwitterButtons(true);
                   }));
                       MainWindow.SetControlEnable((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, true);
                       if (ignoreAutoDownload)
                           return;
                       this.ExecuteOnUIThread((Action)(() =>
               {
                       if (this.videoAutoDownloadMenuItem.Checked)
                           this.OnActionButtonClick((object)this.splitButtonActionVideo, EventArgs.Empty);
                       if (this.audioAutoDownloadMenuItem.Checked)
                           this.OnActionButtonClick((object)this.splitButtonActionAudio, EventArgs.Empty);
                       if (!this.autoDownloadMenuItem.Checked)
                           return;
                       this.ShowBalloon(System.Windows.Forms.Application.ProductName, Strings.AutoDownloadStarted, BalloonIcon.Info);
                   }));
                   }
                   else
                   {
                       if (info.Exception != null)
                       {
                           this.Status = info.Exception.Message;
                           int num;
                           this.Invoke(new Action(() => num = (int)System.Windows.Forms.MessageBox.Show((IWin32Window)this, info.Exception.Message, Strings.Information, MessageBoxButtons.OK, MessageBoxIcon.Asterisk)));
                       }
                       else
                           this.Status = string.Format(Strings.FoundResults, (object)0);
                       this.SetEnableDownloadButtons(false, true);
                   }
               }), (MediaInfo)null);
            }
            catch (Exception ex)
            {
                this.SetEnableDownloadButtons(false, true);
                MainWindow.SetControlEnable((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, true);
                int num = (int)System.Windows.Forms.MessageBox.Show(MainWindow.CreateMultiLineMessage(new string[2]
                {
          ex.Message,
          pathToSource
                }), Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private static bool ClipboardContainsText()
        {
            bool flag = false;
            try
            {
                flag = System.Windows.Forms.Clipboard.ContainsText();
            }
            catch
            {
            }
            return flag;
        }

        private static void ExecutePath(string path, bool play, bool localFile)
        {
            if (play)
                MainWindow.RunDefaultApp(path, localFile, false);
            else
                System.Windows.Forms.Clipboard.SetText(path);
        }

        internal static void RunDefaultApp(string path, bool localFile, bool installation = false)
        {
            try
            {
                ProcessStartInfo startInfo;
                if (localFile)
                {
                    startInfo = new ProcessStartInfo(path);
                    if (installation)
                        startInfo.UseShellExecute = true;
                }
                else
                    startInfo = new ProcessStartInfo(path);
                Process.Start(startInfo);
            }
            catch (FileNotFoundException ex)
            {
                int num = (int)System.Windows.Forms.MessageBox.Show(MainWindow.CreateMultiLineMessage(new string[2]
                {
          Strings.ResourceNotFound,
          path
                }), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Win32Exception ex)
            {
                int num = (int)System.Windows.Forms.MessageBox.Show(MainWindow.CreateMultiLineMessage(new string[2]
                {
          ex.Message,
          path
                }), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                int num = (int)System.Windows.Forms.MessageBox.Show(MainWindow.CreateMultiLineMessage(new string[2]
                {
          ex.Message,
          path
                }), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void HandleInternetConnectionChanged()
        {
            if (!this.Connected)
                return;
            foreach (DownloadItem download in (Collection<DownloadItem>)DownloadItemViewModel.Instance.Downloads)
            {
                if (download.DownloadTask.PausedByNetworkState)
                    download.DownloadTask.Resume();
            }
        }

        public void SetStatus(object sender, EventArgs e)
        {
            MainWindow.SetCultureInfoForThread(Settings.Instance.LanguageCode);
            DownloadItem currentItem = (DownloadItem)sender;
            DownloadState downloadState = currentItem.State;
            this.Status = this.GetLocalizedStatusText(downloadState);
            this.ExecuteOnUIThread((Action)(() =>
           {
               switch (downloadState)
               {
                   case DownloadState.Completed:
                       if (!(e is DownloadStateEventArgs))
                       {
                           this.ReproduceSoundByDownloadState(downloadState, (string)null, DownloadState.Unknown);
                           break;
                       }
                       this.ReproduceSoundByDownloadState(downloadState, Path.GetFileName(currentItem.FileName), string.IsNullOrWhiteSpace(currentItem.SourceUrl) ? DownloadState.Converting : DownloadState.Downloading);
                       break;
                   case DownloadState.Error:
                       this.ReproduceSoundByDownloadState(downloadState, currentItem.Title, string.IsNullOrWhiteSpace(currentItem.SourceUrl) ? DownloadState.Converting : DownloadState.Downloading);
                       break;
               }
           }));
        }

        private static void ReproduceSound(DownloadState downloadState)
        {
            try
            {
                if (downloadState != DownloadState.Completed)
                {
                    if (downloadState != DownloadState.Error)
                        return;
                    using (SoundPlayer soundPlayer = new SoundPlayer((Stream)Resources.windows_critical_stop))
                        soundPlayer.Play();
                }
                else
                {
                    using (SoundPlayer soundPlayer = new SoundPlayer((Stream)Resources.windows_notify))
                        soundPlayer.Play();
                }
            }
            catch
            {
            }
        }

        internal static void RunProcessStart(bool resourceExists, string fileName, bool localFile = false)
        {
            if (resourceExists)
            {
                MainWindow.RunDefaultApp(fileName, localFile, false);
            }
            else
            {
                int num = (int)System.Windows.Forms.MessageBox.Show(MainWindow.CreateMultiLineMessage(new string[2]
                {
          Strings.ResourceNotFound,
          fileName
                }), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        internal static bool VerifyUrl(string url, UriKind uriKind)
        {
            string pattern = string.Empty;
            switch (uriKind)
            {
                case UriKind.Absolute:
                    pattern = "^(http|https|ftp)\\://[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\?\\,\\'/\\\\\\+&%\\$#\\=~])*[^\\.\\,\\)\\(\\s]$";
                    break;
            }
            return new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled).IsMatch(url);
        }

        private static List<VideoQuality> GetVideoQualities(string fileName, out int height)
        {
            height = 0;
            List<VideoQuality> videoQualityList = (List<VideoQuality>)null;
            MediaMetadata mediaMetadata = new FfmpegWrapper().ObtainMediaMetadata(fileName);
            if (mediaMetadata != null)
            {
                MediaStream mediaStream = mediaMetadata.Streams.FirstOrDefault<MediaStream>((Func<MediaStream, bool>)(s => s.IsVideo));
                if (mediaStream != null && mediaStream.Height.HasValue)
                {
                    height = mediaStream.Height.Value;
                    List<VideoQuality> source = new List<VideoQuality>();
                    foreach (VideoQuality videoQuality in System.Enum.GetValues(typeof(VideoQuality)))
                    {
                        int frameHeight = videoQuality.GetFrameHeight();
                        if (videoQuality <= VideoQuality._720p60fps)
                        {
                            if (videoQuality == VideoQuality._72p || videoQuality == VideoQuality._720p60fps)
                                continue;
                        }
                        else if (videoQuality == VideoQuality._1080p60fps || videoQuality == VideoQuality._1440p60fps || videoQuality == VideoQuality._2160p60fps)
                            continue;
                        if (frameHeight <= height)
                            source.Add(videoQuality);
                        else
                            break;
                    }
                    videoQualityList = source.OrderByDescending<VideoQuality, VideoQuality>((Func<VideoQuality, VideoQuality>)(v => v)).ToList<VideoQuality>();
                }
            }
            return videoQualityList;
        }

        private static bool SaveCustomQuality(ButtonActionData buttonActionData, bool isActionDownload)
        {
            bool flag = false;
            if (buttonActionData.DataQuality is System.Enum)
            {
                if (buttonActionData.DataQuality is VideoQuality)
                {
                    Settings.Instance.DesiredDownloadVideoQuality = (VideoQuality)buttonActionData.DataQuality;
                    flag = true;
                }
                else if (buttonActionData.DataQuality is AudioQuality)
                {
                    Settings.Instance.DesiredDownloadAudioQuality = (AudioQuality)buttonActionData.DataQuality;
                    flag = true;
                }
            }
            if (flag)
            {
                if (isActionDownload)
                {
                    Settings.Instance.DesiredDownloadAction = buttonActionData.DesiredAction;
                    if (Settings.Instance.DesiredDownloadAction.HasValue)
                    {
                        DesiredAction? desiredDownloadAction = Settings.Instance.DesiredDownloadAction;
                        DesiredAction desiredAction1 = DesiredAction.ExtractAAC;
                        if ((desiredDownloadAction.GetValueOrDefault() == desiredAction1 ? (desiredDownloadAction.HasValue ? 1 : 0) : 0) == 0)
                        {
                            desiredDownloadAction = Settings.Instance.DesiredDownloadAction;
                            DesiredAction desiredAction2 = DesiredAction.ExtractMP3;
                            if ((desiredDownloadAction.GetValueOrDefault() == desiredAction2 ? (desiredDownloadAction.HasValue ? 1 : 0) : 0) == 0)
                            {
                                desiredDownloadAction = Settings.Instance.DesiredDownloadAction;
                                DesiredAction desiredAction3 = DesiredAction.ExtractVorbis;
                                if ((desiredDownloadAction.GetValueOrDefault() == desiredAction3 ? (desiredDownloadAction.HasValue ? 1 : 0) : 0) == 0)
                                {
                                    Settings.Instance.DesiredVideoDownloadAction = Settings.Instance.DesiredDownloadAction.Value;
                                    goto label_14;
                                }
                            }
                        }
                        Settings.Instance.DesiredAudioDownloadAction = Settings.Instance.DesiredDownloadAction.Value;
                    }
                }
                else
                    Settings.Instance.DesiredConversionAction = buttonActionData.DesiredAction;
            }
            label_14:
            return flag;
        }

        private void PreparePath(string path, bool play)
        {
            bool flag = false;
            if (!string.IsNullOrWhiteSpace(path))
            {
                string str = path.Trim();
                if (str.IsValidFileName())
                {
                    flag = true;
                    MainWindow.ExecutePath(str, play, true);
                }
                else if (DownloadManager.HasProviderForUrl(str))
                {
                    flag = true;
                    this.PlayYouTubeUrl();
                }
            }
            if (flag)
                return;
            int num = (int)System.Windows.Forms.MessageBox.Show(MainWindow.CreateMultiLineMessage(new string[2]
            {
        Strings.FilePathIsInvalid,
        path
            }), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void PlayYouTubeUrl()
        {
            if (PlayerWindow.IsOpened)
                return;
            IMediaLink mediaLink = ((IEnumerable<IMediaLink>)this._receivedMediaInfo.Links).First<IMediaLink>((Func<IMediaLink, bool>)(l =>
          {
              if (l.IsVideoLink && !l.ToVideoLink().IsDash)
                  return l.ToVideoLink().StreamType == VideoStreamType.Mp4;
              return false;
          }));
            mediaLink.UpdateLink();
            int maxWidth = ((VideoInfo)this._receivedMediaInfo).Thumbnails.Max<FreeYouTubeDownloader.Downloader.Thumbnail>((Func<FreeYouTubeDownloader.Downloader.Thumbnail, int>)(t => t.Width));
            string thumbnailUrl = ((VideoInfo)this._receivedMediaInfo).Thumbnails.Single<FreeYouTubeDownloader.Downloader.Thumbnail>((Func<FreeYouTubeDownloader.Downloader.Thumbnail, bool>)(t => t.Width == maxWidth)).Url;
            if (thumbnailUrl.IndexOf('?') != -1)
                thumbnailUrl = thumbnailUrl.Substring(0, thumbnailUrl.IndexOf('?'));
            Uri uri = new Uri(mediaLink.Url);
            new PlayerWindow(this._receivedMediaInfo.Title, uri.Scheme + "://redirector.googlevideo.com" + uri.PathAndQuery, "video/mp4", thumbnailUrl).Show((IWin32Window)this);
        }

        private List<ToolStripMenuItem> GetContextMenuAudioItemsForLocalFile(bool isLocalFile, string filePath, AudioQuality? highestAudioQuality)
        {
            List<ToolStripMenuItem> toolStripMenuItemList = new List<ToolStripMenuItem>();
            string title = !isLocalFile ? Strings.Download : Strings.ConvertTo;
            string str = isLocalFile ? Path.GetExtension(filePath) : (string)null;
            if (highestAudioQuality.HasValue)
            {
                AudioQuality? nullable = highestAudioQuality;
                AudioQuality audioQuality = AudioQuality._128kbps;
                if ((nullable.GetValueOrDefault() > audioQuality ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                {
                    this.Invoke(new Action(() => this.picBoxAppNotify.Visible = !LicenseHelper.IsGenuine));
                    ToolStripMenuItem audioSubmenuItem1 = this.CreateAudioSubmenuItem("MP3", title, isLocalFile, highestAudioQuality.Value, !isLocalFile || !str.ToLower().Contains("mp3"), new EventHandler(this.OnExtractMp3MenuItemClick), new EventHandler(this.OnConvertToMp3MenuItemClick));
                    toolStripMenuItemList.Add(audioSubmenuItem1);
                    ToolStripMenuItem audioSubmenuItem2 = this.CreateAudioSubmenuItem("AAC", title, isLocalFile, highestAudioQuality.Value, !isLocalFile || !str.ToLower().Contains("aac"), new EventHandler(this.OnExtractAacMenuItemClick), new EventHandler(this.OnConvertToAacMenuItemClick));
                    toolStripMenuItemList.Add(audioSubmenuItem2);
                    ToolStripMenuItem audioSubmenuItem3 = this.CreateAudioSubmenuItem("OGG", title, isLocalFile, highestAudioQuality.Value, !isLocalFile || !str.ToLower().Contains("ogg"), new EventHandler(this.OnExtractVorbisMenuItemClick), new EventHandler(this.OnConvertToVorbisMenuItemClick));
                    audioSubmenuItem3.Text = string.Format("{0} {1} ({2} kbps)", (object)title, (object)"Vorbis", (object)((int)highestAudioQuality.Value).ToString((IFormatProvider)CultureInfo.InvariantCulture));
                    toolStripMenuItemList.Add(audioSubmenuItem3);
                    goto label_4;
                }
            }
            this.Invoke(new Action(() => this.picBoxAppNotify.Visible = false));
            label_4:
            ToolStripMenuItem audioSubmenuItem4 = this.CreateAudioSubmenuItem("MP3", title, isLocalFile, AudioQuality._128kbps, !isLocalFile || !str.ToLower().Contains("mp3"), new EventHandler(this.OnExtractMp3MenuItemClick), new EventHandler(this.OnConvertToMp3MenuItemClick));
            toolStripMenuItemList.Add(audioSubmenuItem4);
            ToolStripMenuItem audioSubmenuItem5 = this.CreateAudioSubmenuItem("AAC", title, isLocalFile, AudioQuality._128kbps, !isLocalFile || !str.ToLower().Contains("aac"), new EventHandler(this.OnExtractAacMenuItemClick), new EventHandler(this.OnConvertToAacMenuItemClick));
            toolStripMenuItemList.Add(audioSubmenuItem5);
            ToolStripMenuItem audioSubmenuItem6 = this.CreateAudioSubmenuItem("OGG", title, isLocalFile, AudioQuality._128kbps, !isLocalFile || !str.ToLower().Contains("ogg"), new EventHandler(this.OnExtractVorbisMenuItemClick), new EventHandler(this.OnConvertToVorbisMenuItemClick));
            audioSubmenuItem6.Text = string.Format("{0} {1} ({2} kbps)", (object)title, (object)"Vorbis", (object)128.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            toolStripMenuItemList.Add(audioSubmenuItem6);
            return toolStripMenuItemList;
        }

        private ToolStripMenuItem CreateAudioSubmenuItem(string format, string title, bool isLocalFile, AudioQuality audioQuality, bool isMenuItemEnable, EventHandler onExtractHandler, EventHandler onConvertHandler)
        {
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(string.Format("{0} {1} ({2} kbps)", (object)title, (object)format, (object)((int)audioQuality).ToString((IFormatProvider)CultureInfo.InvariantCulture)));
            int num = isMenuItemEnable ? 1 : 0;
            toolStripMenuItem1.Enabled = num != 0;
            // ISSUE: variable of a boxed type
            AudioQuality local = audioQuality;
            toolStripMenuItem1.Tag = (object)local;
            ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
            if (toolStripMenuItem2.Enabled)
            {
                if (isLocalFile)
                    toolStripMenuItem2.Click += onConvertHandler;
                else
                    toolStripMenuItem2.Click += onExtractHandler;
            }
            return toolStripMenuItem2;
        }

        private void InitDropDownMenuDownload(bool isLocalFile, bool isAudioFile = false, string filePath = null)
        {
            MainWindow.SetCultureInfoForThread(Settings.Instance.LanguageCode);
            if (!isLocalFile)
            {
                this.SetDefaultTextDownloadButtons(false);
                if (this._receivedMediaInfo != null)
                {
                    List<MediaLink> list1 = this._receivedMediaInfo.Links.Cast<MediaLink>().Distinct<MediaLink>((IEqualityComparer<MediaLink>)new MediaLinksComparer()).ToList<MediaLink>();
                    List<VideoLink> list2 = list1.Where<MediaLink>((Func<MediaLink, bool>)(link => link.IsVideoLink)).Cast<VideoLink>().ToList<VideoLink>();
                    List<ToolStripMenuItem> videoMenuItems = new List<ToolStripMenuItem>();
                    List<VideoLink> source1 = list2;
                    Func<VideoLink, bool> func1 = (Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.Mp4);
                    if (source1.Any<VideoLink>())
                    {
                        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.DownloadMP4);
                        // ISSUE: variable of a boxed type
                        VideoStreamType local = VideoStreamType.Mp4;
                        toolStripMenuItem1.Tag = (object)local;
                        ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                        foreach (VideoLink videoLink in (IEnumerable<VideoLink>)list2.Where<VideoLink>((Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.Mp4)).OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>)(link => link.VideoStreamQuality)))
                        {
                            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(videoLink.MediaQuality);
                            // ISSUE: variable of a boxed type
                            VideoQuality videoStreamQuality = videoLink.VideoStreamQuality;
                            toolStripMenuItem3.Tag = (object)videoStreamQuality;
                            ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                            toolStripMenuItem4.Click += new EventHandler(this.OnContextMenuDownloadVideoSubMenuItemClick);
                            toolStripMenuItem2.DropDownItems.Add((ToolStripItem)toolStripMenuItem4);
                        }
                        videoMenuItems.Add(toolStripMenuItem2);
                    }
                    List<VideoLink> source2 = list2;
                    Func<VideoLink, bool> func2 = (Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.Flv);
                    if (source2.Any<VideoLink>())
                    {
                        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.DownloadFLV);
                        // ISSUE: variable of a boxed type
                        VideoStreamType local = VideoStreamType.Flv;
                        toolStripMenuItem1.Tag = (object)local;
                        ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                        foreach (VideoLink videoLink in (IEnumerable<VideoLink>)list2.Where<VideoLink>((Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.Flv)).OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>)(link => link.VideoStreamQuality)))
                        {
                            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(videoLink.MediaQuality);
                            // ISSUE: variable of a boxed type
                            VideoQuality videoStreamQuality = videoLink.VideoStreamQuality;
                            toolStripMenuItem3.Tag = (object)videoStreamQuality;
                            ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                            toolStripMenuItem4.Click += new EventHandler(this.OnContextMenuDownloadVideoSubMenuItemClick);
                            toolStripMenuItem2.DropDownItems.Add((ToolStripItem)toolStripMenuItem4);
                        }
                        videoMenuItems.Add(toolStripMenuItem2);
                    }
                    List<VideoLink> source3 = list2;
                    Func<VideoLink, bool> func3 = (Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.WebM);
                    if (source3.Any<VideoLink>())
                    {
                        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.DownloadWebM);
                        // ISSUE: variable of a boxed type
                        VideoStreamType local = VideoStreamType.WebM;
                        toolStripMenuItem1.Tag = (object)local;
                        ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                        foreach (VideoLink videoLink in (IEnumerable<VideoLink>)list2.Where<VideoLink>((Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.WebM)).OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>)(link => link.VideoStreamQuality)))
                        {
                            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(videoLink.MediaQuality);
                            // ISSUE: variable of a boxed type
                            VideoQuality videoStreamQuality = videoLink.VideoStreamQuality;
                            toolStripMenuItem3.Tag = (object)videoStreamQuality;
                            ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                            toolStripMenuItem4.Click += new EventHandler(this.OnContextMenuDownloadVideoSubMenuItemClick);
                            toolStripMenuItem2.DropDownItems.Add((ToolStripItem)toolStripMenuItem4);
                        }
                        videoMenuItems.Add(toolStripMenuItem2);
                    }
                    List<VideoLink> source4 = list2;
                    Func<VideoLink, bool> func4 = (Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.ThreeGp);
                    if (source4.Any<VideoLink>())
                    {
                        ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(string.Format("{0} {1}", (object)Strings.Download, (object)"3GP"));
                        // ISSUE: variable of a boxed type
                        VideoStreamType local = VideoStreamType.ThreeGp;
                        toolStripMenuItem1.Tag = (object)local;
                        ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                        foreach (VideoLink videoLink in (IEnumerable<VideoLink>)list2.Where<VideoLink>((Func<VideoLink, bool>)(link => link.StreamType == VideoStreamType.ThreeGp)).OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>)(link => link.VideoStreamQuality)))
                        {
                            ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(videoLink.MediaQuality);
                            // ISSUE: variable of a boxed type
                            VideoQuality videoStreamQuality = videoLink.VideoStreamQuality;
                            toolStripMenuItem3.Tag = (object)videoStreamQuality;
                            ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                            toolStripMenuItem4.Click += new EventHandler(this.OnContextMenuDownloadVideoSubMenuItemClick);
                            toolStripMenuItem2.DropDownItems.Add((ToolStripItem)toolStripMenuItem4);
                        }
                        videoMenuItems.Add(toolStripMenuItem2);
                    }
                    ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(Strings.ConvertToAVI);
                    foreach (ToolStripMenuItem toolStripMenuItem1 in list2.Distinct<VideoLink>((IEqualityComparer<VideoLink>)new VideoStreamQualityDistinctComparer()).ToList<VideoLink>().OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>)(l => l.VideoStreamQuality)).Select<VideoLink, ToolStripMenuItem>((Func<VideoLink, ToolStripMenuItem>)(link =>
                 {
                     return new ToolStripMenuItem(link.MediaQuality)
                     {
                         Tag = (object)link.VideoStreamQuality
                     };
                 })).ToList<ToolStripMenuItem>())
                    {
                        toolStripMenuItem1.Click += new EventHandler(this.OnContextMenuConvertToAviVideoSubMenuItemClick);
                        toolStripMenuItem5.DropDownItems.Add((ToolStripItem)toolStripMenuItem1);
                    }
                    videoMenuItems.Add(toolStripMenuItem5);
                    AudioQuality? highestAudioQuality = new AudioQuality?();
                    try
                    {
                        highestAudioQuality = new AudioQuality?(list1.Where<MediaLink>((Func<MediaLink, bool>)(l => l.IsAudioLink)).Cast<AudioLink>().Max<AudioLink, AudioQuality>((Func<AudioLink, AudioQuality>)(l => l.AudioStreamQuality)));
                    }
                    catch
                    {
                    }
                    AudioQuality? nullable = highestAudioQuality;
                    AudioQuality audioQuality = AudioQuality._128kbps;
                    if ((nullable.GetValueOrDefault() > audioQuality ? (nullable.HasValue ? 1 : 0) : 0) != 0)
                        highestAudioQuality = new AudioQuality?(AudioQuality._192kbps);
                    List<ToolStripMenuItem> audioMenuItems = this.GetContextMenuAudioItemsForLocalFile(false, filePath, highestAudioQuality);
                    this.ExecuteOnUIThread((Action)(() =>
                   {
                       this.InitContextMenuStrip(this.contextMenuStripDownloadVideo, videoMenuItems);
                       this.InitContextMenuStrip(this.contextMenuStripDownloadAudio, audioMenuItems);
                   }));
                }
                this.Status = Strings.Done;
            }
            else
            {
                this.ProgressBarLoadingDataShow();
                this.Status = string.Format("{0} ...", (object)Strings.ReceivingConvertLinks);
                this.SetDefaultTextDownloadButtons(true);
                new Thread((ParameterizedThreadStart)(i =>
               {
                   List<ToolStripMenuItem> videoMenuItems = (List<ToolStripMenuItem>)null;
                   List<ToolStripMenuItem> audioMenuItems = (List<ToolStripMenuItem>)null;
                   if (isAudioFile)
                       MainWindow.SetControlEnable((System.Windows.Forms.Control)this.splitButtonActionVideo, false, Strings.DownloadAsVideo);
                   else
                       videoMenuItems = this.GetContextMenuVideoItemsForLocalFile(filePath);
                   if (isAudioFile || videoMenuItems != null)
                   {
                       this.InitPanelSearchResultFromLocalFile(filePath);
                       audioMenuItems = this.GetContextMenuAudioItemsForLocalFile(true, filePath, new AudioQuality?());
                       if (audioMenuItems.Count > 0 || videoMenuItems != null)
                           this.ExecuteOnUIThread((Action)(() =>
                   {
                         if (audioMenuItems.Count > 0)
                             this.InitContextMenuStrip(this.contextMenuStripDownloadAudio, audioMenuItems);
                         if (videoMenuItems != null)
                             this.InitContextMenuStrip(this.contextMenuStripDownloadVideo, videoMenuItems);
                         this.PanelWrapperContainerAutoComplete.Visible = false;
                         this.Status = Strings.Done;
                     }));
                       else
                           this.ExecuteOnUIThread((Action)(() =>
                   {
                         this.PanelWrapperContainerAutoComplete.Visible = false;
                         this.Status = Strings.Done;
                     }));
                   }
                   else
                   {
                       this.SetEnableDownloadButtons(false, false);
                       this.ExecuteOnUIThread((Action)(() =>
               {
                       this.PanelWrapperContainerAutoComplete.Visible = false;
                       this.Status = Strings.Done;
                       int num = (int)System.Windows.Forms.MessageBox.Show(string.Format(Strings.FileWithoutLinksWarningMessage, (object)filePath), Strings.Warning, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                   }));
                   }
               }))
                {
                    IsBackground = true
                }.Start();
            }
        }

        private void InitPanelContainerAutoComplete(List<YoutubeVideoRecord> videoDataList, int pointY, string keyword)
        {
            if (videoDataList.Count > 0)
            {
                int countVisibleRows = (this.olvDownloads.Height + 100 + this.GetRowSearchResultHeight()) / 70;
                int num1 = this.PanelWrapperContainerAutoComplete.Width - (this.PanelContainerAutoComplete.VerticalScroll.Maximum - this.PanelContainerAutoComplete.Height < this.PanelContainerAutoComplete.VerticalScroll.Value ? 20 : 0);
                List<System.Windows.Forms.Control> panelRowAutoCompleteList = new List<System.Windows.Forms.Control>();
                int num2 = pointY == 0 ? pointY : pointY + 70;
                for (int index = 0; index < videoDataList.Count; ++index)
                {
                    System.Windows.Forms.Panel panel1 = new System.Windows.Forms.Panel();
                    int num3 = 70;
                    panel1.Height = num3;
                    int num4 = num1;
                    panel1.Width = num4;
                    System.Drawing.Point point1 = new System.Drawing.Point(1, index * 70 + num2);
                    panel1.Location = point1;
                    string url = videoDataList[index].Url;
                    panel1.Tag = (object)url;
                    int num5 = 13;
                    panel1.Anchor = (AnchorStyles)num5;
                    System.Windows.Forms.Panel panel2 = panel1;
                    panel2.Click += new EventHandler(this.OnPanelRowAutoCompleteClick);
                    panel2.MouseMove += new MouseEventHandler(this.OnPanelRowAutoCompleteMouseMove);
                    panel2.MouseLeave += new EventHandler(this.OnPanelRowAutoCompleteMouseLeave);
                    PictureBox pictureBox1 = new PictureBox();
                    string str1 = "Image";
                    pictureBox1.Name = str1;
                    int num6 = 120;
                    pictureBox1.Width = num6;
                    int num7 = 68;
                    pictureBox1.Height = num7;
                    System.Drawing.Image thumbnail = videoDataList[index].Thumbnail;
                    pictureBox1.Image = thumbnail;
                    int num8 = 3;
                    pictureBox1.SizeMode = (PictureBoxSizeMode)num8;
                    System.Drawing.Point point2 = new System.Drawing.Point(1, 1);
                    pictureBox1.Location = point2;
                    int num9 = 0;
                    pictureBox1.Enabled = num9 != 0;
                    int num10 = 5;
                    pictureBox1.Anchor = (AnchorStyles)num10;
                    PictureBox pictureBox2 = pictureBox1;
                    panel2.Controls.Add((System.Windows.Forms.Control)pictureBox2);
                    System.Windows.Forms.Label label1 = new System.Windows.Forms.Label();
                    string str2 = "Title";
                    label1.Name = str2;
                    Font font1 = new Font(this.lblSearchResultTitle.Font.FontFamily, this.lblSearchResultTitle.Font.Size, this.lblSearchResultTitle.Font.Style);
                    label1.Font = font1;
                    string title = videoDataList[index].Title;
                    label1.Text = title;
                    int num11 = num1 - 228 - 4;
                    label1.Width = num11;
                    System.Drawing.Point point3 = new System.Drawing.Point(pictureBox2.Location.X + pictureBox2.Width + 4, 8);
                    label1.Location = point3;
                    int num12 = 0;
                    label1.Enabled = num12 != 0;
                    int num13 = 13;
                    label1.Anchor = (AnchorStyles)num13;
                    System.Windows.Forms.Label label2 = label1;
                    panel2.Controls.Add((System.Windows.Forms.Control)label2);
                    System.Windows.Forms.Label label3 = new System.Windows.Forms.Label();
                    string str3 = "Time";
                    label3.Name = str3;
                    Font font2 = new Font(this.lblSearchResultTime.Font.FontFamily, this.lblSearchResultTime.Font.Size, this.lblSearchResultTime.Font.Style);
                    label3.Font = font2;
                    // ISSUE: variable of a boxed type
                    double duration = videoDataList[index].Duration;
                    label3.Tag = (object)duration;
                    string friendlyRepresentation = videoDataList[index].Duration.MillisecondsToHumanFriendlyRepresentation(false, false, false);
                    label3.Text = friendlyRepresentation;
                    int num14 = 100;
                    label3.Width = num14;
                    System.Drawing.Point point4 = new System.Drawing.Point(label2.Location.X + label2.Width + 4, 8);
                    label3.Location = point4;
                    int num15 = 0;
                    label3.Enabled = num15 != 0;
                    int num16 = 9;
                    label3.Anchor = (AnchorStyles)num16;
                    System.Windows.Forms.Label label4 = label3;
                    panel2.Controls.Add((System.Windows.Forms.Control)label4);
                    System.Windows.Forms.Label label5 = new System.Windows.Forms.Label();
                    string str4 = "Description";
                    label5.Name = str4;
                    Font font3 = new Font(this.lblSearchResultDescription.Font.FontFamily, this.lblSearchResultDescription.Font.Size, this.lblSearchResultDescription.Font.Style);
                    label5.Font = font3;
                    string description = videoDataList[index].Description;
                    label5.Text = description;
                    System.Drawing.Point location = label4.Location;
                    int num17 = location.X + label4.Width;
                    location = label2.Location;
                    int x1 = location.X;
                    int num18 = num17 - x1;
                    label5.Width = num18;
                    location = label2.Location;
                    int x2 = location.X;
                    location = label2.Location;
                    int y = location.Y + label2.Height + 6;
                    System.Drawing.Point point5 = new System.Drawing.Point(x2, y);
                    label5.Location = point5;
                    int num19 = 0;
                    label5.Enabled = num19 != 0;
                    int num20 = 13;
                    label5.Anchor = (AnchorStyles)num20;
                    System.Windows.Forms.Label label6 = label5;
                    panel2.Controls.Add((System.Windows.Forms.Control)label6);
                    panelRowAutoCompleteList.Add((System.Windows.Forms.Control)panel2);
                }
                this.ExecuteOnUIThread((Action)(() =>
               {
                   if (!keyword.Equals(this.textBoxUrl.Text.Trim()))
                       return;
                   lock (this._locker)
                   {
                       this.progressBarLoadingData.Visible = false;
                       this.PanelContainerAutoComplete.SuspendLayout();
                       this.PanelContainerAutoComplete.Controls.AddRange(panelRowAutoCompleteList.ToArray());
                       this.PanelContainerAutoComplete.ResumeLayout();
                       this.PanelContainerAutoComplete.Height = panelRowAutoCompleteList.Count < countVisibleRows ? panelRowAutoCompleteList.Count * 70 : countVisibleRows * 70;
                       this.progressBarLoadingData.Location = new System.Drawing.Point(0, this.PanelContainerAutoComplete.Height);
                       this.PanelWrapperContainerAutoComplete.Height = this.PanelContainerAutoComplete.Height + this.progressBarLoadingData.Height + 2;
                       this.PanelWrapperContainerAutoComplete.BorderStyle = BorderStyle.FixedSingle;
                       this.PanelWrapperContainerAutoComplete.Visible = true;
                       this.Status = string.Format(Strings.FoundResults, (object)this.PanelContainerAutoComplete.Controls.Count);
                   }
               }));
            }
            else
                this.ExecuteOnUIThread((Action)(() =>
               {
                   if (!keyword.Equals(this.textBoxUrl.Text))
                       return;
                   this.Status = string.Format(Strings.FoundResults, (object)0);
                   this.SearchDropDownPanelHide(true);
               }));
        }

        private void InitVideoDataList(string keyword, uint startIndex = 1, int pointY = 0)
        {
            try
            {
                this.InitPanelContainerAutoComplete(YoutubeHelper.GetYoutubeVideoData(keyword, startIndex, 25U), pointY, keyword);
            }
            catch (Exception ex)
            {
                this.ExecuteOnUIThread((Action)(() =>
               {
                   this.Status = ex.Message;
                   this.SearchDropDownPanelHide(false);
               }));
                int num = (int)System.Windows.Forms.MessageBox.Show(ex.Message, Strings.SearchError, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void PanelContainerAutoCompleteNavigation(bool down)
        {
            bool flag = false;
            if (down)
            {
                for (int index1 = 0; index1 < this.PanelContainerAutoComplete.Controls.Count; ++index1)
                {
                    if (this.PanelContainerAutoComplete.Controls[index1].BackColor == System.Drawing.Color.LightSkyBlue)
                    {
                        int index2 = index1;
                        if (this.PanelContainerAutoComplete.Controls[index2].Location.Y + 70 + 5 > this.PanelContainerAutoComplete.ClientSize.Height)
                        {
                            lock (this._locker)
                                this.PanelContainerAutoComplete.ScrollDown(69);
                            if (index2 + 2 == this.PanelContainerAutoComplete.Controls.Count)
                                this.CallInitVideoDataList();
                        }
                        if (index2 + 1 != this.PanelContainerAutoComplete.Controls.Count)
                        {
                            this.PanelContainerAutoComplete.Controls[index2].BackColor = System.Drawing.Color.White;
                            this.PanelContainerAutoComplete.Controls[index2].Cursor = Cursors.Default;
                            this.PanelContainerAutoComplete.Controls[index2 + 1].BackColor = System.Drawing.Color.LightSkyBlue;
                            this.PanelContainerAutoComplete.Controls[index2 + 1].Cursor = Cursors.Hand;
                        }
                        flag = true;
                        break;
                    }
                }
            }
            else
            {
                for (int index1 = this.PanelContainerAutoComplete.Controls.Count - 1; index1 >= 0; --index1)
                {
                    if (this.PanelContainerAutoComplete.Controls[index1].BackColor == System.Drawing.Color.LightSkyBlue)
                    {
                        int index2 = index1;
                        if (index2 != 0)
                        {
                            if (this.PanelContainerAutoComplete.Controls[index2].Location.Y < 10)
                            {
                                lock (this._locker)
                                    this.PanelContainerAutoComplete.ScrollUp(-70);
                            }
                            this.PanelContainerAutoComplete.Controls[index2].BackColor = System.Drawing.Color.White;
                            this.PanelContainerAutoComplete.Controls[index2].Cursor = Cursors.Default;
                            this.PanelContainerAutoComplete.Controls[index2 - 1].BackColor = System.Drawing.Color.LightSkyBlue;
                            this.PanelContainerAutoComplete.Controls[index2 - 1].Cursor = Cursors.Hand;
                        }
                        flag = true;
                        break;
                    }
                }
            }
            if (flag)
                return;
            System.Windows.Forms.Control.ControlCollection controls = this.PanelContainerAutoComplete.Controls;
            int num = 0;
            System.Windows.Forms.Control control = controls.Where((Func<System.Windows.Forms.Control, bool>)(row => row.Location.Y == 0), num != 0).FirstOrDefault<System.Windows.Forms.Control>();
            if (control == null)
                return;
            control.BackColor = System.Drawing.Color.LightSkyBlue;
            control.Cursor = Cursors.Hand;
        }

        private ToolStripMenuItem GetLastUsedMenuItem(ToolStripMenuItem item)
        {
            if (item.Enabled && !item.HasDropDownItems)
            {
                if ((DesiredAction)item.Tag != Settings.Instance.DesiredAudioDownloadAction)
                    return (ToolStripMenuItem)null;
                return item;
            }
            if (!LicenseHelper.IsGenuine && Settings.Instance.DesiredDownloadVideoQuality > VideoQuality._1080p60fps)
                Settings.Instance.DesiredDownloadVideoQuality = VideoQuality._1080p60fps;
            if (item.Enabled && item.HasDropDownItems)
            {
                switch (Settings.Instance.DesiredVideoDownloadAction)
                {
                    case DesiredAction.DownloadMP4:
                        if ((VideoStreamType)item.Tag == VideoStreamType.Mp4)
                            return item.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault<ToolStripMenuItem>((Func<ToolStripMenuItem, bool>)(subItem => (VideoQuality)subItem.Tag <= Settings.Instance.DesiredDownloadVideoQuality));
                        break;
                    case DesiredAction.DownloadFLV:
                        if ((VideoStreamType)item.Tag == VideoStreamType.Flv)
                            return item.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault<ToolStripMenuItem>((Func<ToolStripMenuItem, bool>)(subItem => (VideoQuality)subItem.Tag <= Settings.Instance.DesiredDownloadVideoQuality));
                        break;
                    case DesiredAction.DownloadWebM:
                        if ((VideoStreamType)item.Tag == VideoStreamType.WebM)
                            return item.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault<ToolStripMenuItem>((Func<ToolStripMenuItem, bool>)(subItem => (VideoQuality)subItem.Tag <= Settings.Instance.DesiredDownloadVideoQuality));
                        break;
                    case DesiredAction.DownloadThreeGP:
                        if ((VideoStreamType)item.Tag == VideoStreamType.ThreeGp)
                            return item.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault<ToolStripMenuItem>((Func<ToolStripMenuItem, bool>)(subItem => (VideoQuality)subItem.Tag <= Settings.Instance.DesiredDownloadVideoQuality));
                        break;
                    case DesiredAction.DownloadAndConvertToAVI:
                        return item.DropDownItems.Cast<ToolStripMenuItem>().FirstOrDefault<ToolStripMenuItem>((Func<ToolStripMenuItem, bool>)(subItem => (VideoQuality)subItem.Tag <= Settings.Instance.DesiredDownloadVideoQuality));
                }
            }
            return (ToolStripMenuItem)null;
        }

        private void ReproduceSoundByDownloadState(DownloadState downloadState, string fileName, DownloadState originalState)
        {
            if (MainWindow.AllowSound)
                MainWindow.ReproduceSound(downloadState);
            if (!MainWindow.AllowBalloon)
                return;
            this.ShowBalloonTip(downloadState, fileName, originalState);
        }

        private void ShowBalloonTip(DownloadState downloadState, string fileName, DownloadState originalState)
        {
            if (downloadState != DownloadState.Completed)
            {
                if (downloadState != DownloadState.Error)
                    return;
                this.ShowBalloon(Strings.Error, string.Format(originalState == DownloadState.Downloading ? Strings.DownloadIsFailed : Strings.ConvertIsFailed, (object)fileName.Truncate(this._maxLengthFileNameInBalloonTip)), BalloonIcon.Error);
            }
            else if (originalState == DownloadState.Downloading)
            {
                this.ShowBalloon(Strings.DownloadingCompleted, string.Format(Strings.DownloadIsCompleted, (object)fileName.Truncate(this._maxLengthFileNameInBalloonTip)), BalloonIcon.Info);
            }
            else
            {
                if (originalState != DownloadState.Converting)
                    return;
                this.ShowBalloon(Strings.ConvertingCompleted, string.Format(Strings.ConvertIsCompleted, (object)fileName.Truncate(this._maxLengthFileNameInBalloonTip)), BalloonIcon.Info);
            }
        }

        private ToolStripMenuItem InitHighestQuality(ToolStripMenuItem item)
        {
            if (item.Enabled && !item.HasDropDownItems)
            {
                if ((AudioQuality)item.Tag > AudioQuality._128kbps && !LicenseHelper.IsGenuine)
                    return (ToolStripMenuItem)null;
                return item;
            }
            if (item.Enabled && item.HasDropDownItems)
            {
                for (int index = 0; index < item.DropDownItems.Count; ++index)
                {
                    ToolStripMenuItem dropDownItem = (ToolStripMenuItem)item.DropDownItems[index];
                    if (dropDownItem.Enabled)
                    {
                        if (dropDownItem.Tag.GetType() != typeof(VideoQuality) || LicenseHelper.IsGenuine)
                            return dropDownItem;
                        switch ((VideoQuality)dropDownItem.Tag)
                        {
                            case VideoQuality._4320p:
                            case VideoQuality._4320p60fps:
                            case VideoQuality._1440p:
                            case VideoQuality._1440p60fps:
                            case VideoQuality._2160p:
                            case VideoQuality._2160p60fps:
                            case VideoQuality._3072p:
                                continue;
                            default:
                                return dropDownItem;
                        }
                    }
                }
            }
            return (ToolStripMenuItem)null;
        }

        private void ShowBalloon(string title, string text, BalloonIcon tipIcon)
        {
            if (this._notifyIcon.Visibility == Visibility.Visible)
                this._notifyIcon.Visibility = Visibility.Collapsed;
            this._notifyIcon.Visibility = Visibility.Visible;
            this._notifyIcon.ShowBalloonTip(title, text, tipIcon);
        }

        private void InitContextMenuStrip(ContextMenuStrip contextMenu, List<ToolStripMenuItem> menuItems)
        {
            contextMenu.Items.Clear();
            ToolStripMenuItem toolStripMenuItem1 = (ToolStripMenuItem)null;
            foreach (ToolStripMenuItem toolStripMenuItem2 in menuItems.ToList<ToolStripMenuItem>())
            {
                if (toolStripMenuItem1 == null)
                    toolStripMenuItem1 = Settings.Instance.RememberLastQualityUsed ? this.GetLastUsedMenuItem(toolStripMenuItem2) : this.InitHighestQuality(toolStripMenuItem2);
                ((ToolStripDropDownMenu)toolStripMenuItem2.DropDown).ShowImageMargin = false;
                contextMenu.Items.Add((ToolStripItem)toolStripMenuItem2);
            }
            if (toolStripMenuItem1 == null)
                return;
            if (this.InvokeRequired)
                this.Invoke((Delegate)new MethodInvoker(((ToolStripItem)toolStripMenuItem1).PerformClick));
            else
                toolStripMenuItem1.PerformClick();
        }

        private List<ToolStripMenuItem> GetContextMenuVideoItemsForLocalFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            int height;
            List<VideoQuality> videoQualities = MainWindow.GetVideoQualities(filePath, out height);
            if (height == 0 || videoQualities == null)
                return (List<ToolStripMenuItem>)null;
            return new List<ToolStripMenuItem>()
      {
        this.InitVideoMenuItem(Strings.ConvertToMP4, VideoStreamType.Mp4, extension, height, (IEnumerable<VideoQuality>) videoQualities),
        this.InitVideoMenuItem(Strings.ConvertToFLV, VideoStreamType.Flv, extension, height, (IEnumerable<VideoQuality>) videoQualities),
        this.InitVideoMenuItem(Strings.ConvertToWebM, VideoStreamType.WebM, extension, height, (IEnumerable<VideoQuality>) videoQualities),
        this.InitVideoMenuItem(Strings.ConvertToAVI, VideoStreamType.Avi, extension, height, (IEnumerable<VideoQuality>) videoQualities)
      };
        }

        private ToolStripMenuItem InitVideoMenuItem(string titleMenuItem, VideoStreamType videoStreamType, string fileExtension, int height, IEnumerable<VideoQuality> videoQualities)
        {
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(titleMenuItem);
            // ISSUE: variable of a boxed type
            VideoStreamType local1 = videoStreamType;
            toolStripMenuItem1.Tag = (object)local1;
            ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
            foreach (VideoQuality videoQuality in videoQualities)
            {
                string str = videoQuality.ToString();
                int frameHeight = videoQuality.GetFrameHeight();
                string text = str.ToLower().Contains("HD".ToLower()) ? string.Format("HD {0}p", (object)frameHeight) : string.Format("{0}p", (object)frameHeight);
                bool flag = !fileExtension.Contains(videoStreamType.ToString().ToLower()) || frameHeight != height;
                ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(text);
                // ISSUE: variable of a boxed type
                VideoQuality local2 = videoQuality;
                toolStripMenuItem3.Tag = (object)local2;
                int num = flag ? 1 : 0;
                toolStripMenuItem3.Enabled = num != 0;
                ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                toolStripMenuItem4.Click += new EventHandler(this.OnContextMenuConvertVideoSubMenuItemClick);
                toolStripMenuItem2.DropDownItems.Add((ToolStripItem)toolStripMenuItem4);
            }
            return toolStripMenuItem2;
        }

        private void NotifyIconClose()
        {
            while (this._countBalloons > 0)
                Thread.Sleep(1000);
            this.ExecuteOnUIThread((Action)(() => this._notifyIcon.Visibility = Visibility.Collapsed));
        }

        private void SearchDataByKeyword(string keyword)
        {
            this.ExecuteOnUIThread((Action)(() =>
           {
               this.ProgressBarLoadingDataShow();
               this.Status = Strings.SearchingForVideos;
               this.statusStrip.Refresh();
           }));
            this.InitVideoDataList(keyword, 1U, 0);
            MeasurementClient measurementClient = ApplicationManager.MeasurementClient;
            AppEventData appEventData = new AppEventData();
            appEventData.ApplicationName = System.Windows.Forms.Application.ProductName;
            appEventData.ApplicationVersion = System.Windows.Forms.Application.ProductVersion;
            string str1 = "search";
            appEventData.EventAction = str1;
            string str2 = "app";
            appEventData.EventCategory = str2;
            string str3 = keyword;
            appEventData.EventLabel = str3;
            string str4 = "Main";
            appEventData.ScreenName = str4;
            measurementClient.EventAsync((EventData)appEventData);
        }

        private void MinimizeToSystemTray()
        {
            this._notifyIcon.Visibility = Visibility.Visible;
            this.Hide();
        }

        private void VerifyAndInitializeLicense()
        {
            if (!LicenseHelper.IsGenuine && LicenseHelper.IsProductKeyRevoked())
            {
                FreeYouTubeDownloader.Debug.Log.Warning("Failed to verify the subscription", (Exception)null);
                int num1 = RegistryManager.GetValue<int>(Registry.CurrentUser, "Software\\Vitzo\\FreeYouTubeDownloader\\", "LicenseWarningShown");
                if (num1 < 5)
                {
                    if (num1 == 0 && System.Windows.Forms.MessageBox.Show((IWin32Window)this, Strings.ProSubscriptionIsReverted2, System.Windows.Forms.Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                        MainWindow.RunProcessStart(MainWindow.VerifyUrl("https://youtubedownloader.com/support", UriKind.Absolute), "https://youtubedownloader.com/support", false);
                    int num2;
                    RegistryManager.SetValue(Registry.CurrentUser, "Software\\Vitzo\\FreeYouTubeDownloader\\", "LicenseWarningShown", (object)(num2 = num1 + 1), RegistryValueKind.DWord);
                    this.SetStatusStrip(Strings.ProSubscriptionIsReverted);
                }
            }
            if (!LicenseHelper.IsGenuine)
            {
                if (LicenseHelper.IsExpired)
                {
                    try
                    {
                        //LicenseHelper.TurboActivate.Deactivate(true);
                    }
                    catch (TurboActivateException ex)
                    {
                    }
                    string path;
                    switch (new ConfirmationWindow(System.Windows.Forms.Application.ProductName, Strings.KeyExpiredMessage, (string)null).ShowDialog())
                    {
                        case DialogResult.Ignore:
                            goto label_13;
                        case DialogResult.Yes:
                            path = "https://youtubedownloader.com/subscribe/";
                            break;
                        default:
                            path = "http://youtubedownloader.com/support";
                            break;
                    }
                    int num1 = 0;
                    int num2 = 0;
                    MainWindow.RunDefaultApp(path, num1 != 0, num2 != 0);
                }
            }
            label_13:
            this.Text = LicenseHelper.GetAppTitle(true);
            if (LicenseHelper.IsGenuine)
            {
                this.upgradeToProVersionToolStripMenuItem.Visible = false;
                this.deactivateFYDProToolStripMenuItem.Visible = true;
            }
            else
            {
                this.upgradeToProVersionToolStripMenuItem.Visible = true;
                this.deactivateFYDProToolStripMenuItem.Visible = false;
            }
        }

        private void HandleCommandArgs()
        {
            if (!Environment.CommandLine.Contains("-cc"))
                return;
            foreach (DownloadItem downloadItem in DownloadItemViewModel.Instance.Downloads.Where<DownloadItem>((Func<DownloadItem, bool>)(dItem =>
           {
               if (dItem.DownloadTask != null)
                   return dItem.DownloadTask.IsCompleted;
               return false;
           })))
            {
                MultifileDownloadTask downloadTask = downloadItem.DownloadTask as MultifileDownloadTask;
                if (downloadTask != null)
                {
                    foreach (MultifileDownloadParameter downloadParameter in downloadTask.MultifileDownloadParameters)
                    {
                        try
                        {
                            System.IO.File.Delete(downloadParameter.FileName);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    try
                    {
                        System.IO.File.Delete(downloadItem.DownloadTask.FileName);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void InitializeVariablesAndControls()
        {
            this.MinimumSize = new System.Drawing.Size(DpiHelper.LogicalToDeviceUnitsX(800), DpiHelper.LogicalToDeviceUnitsY(475));
            TimerEx timerEx = new TimerEx(2000.0);
            int num1 = 0;
            timerEx.AutoReset = num1 != 0;
            this._timerSearch = timerEx;
            this._timerSearch.Elapsed += new ElapsedEventHandler(this.OnTimerSearchElapsed);
            this.statusStrip.Padding = new Padding(8, 0, 0, 0);
            this._tableLayoutPanelSearchResultRowHeight = (float)this.GetRowSearchResultHeight();
            this._syncContext = SynchronizationContext.Current;
            this._maxLengthFileNameInBalloonTip = 30;
            this._toolTip = new System.Windows.Forms.ToolTip();
            this._thumbnailOverlay = PictureBoxEx.SetImageOpacity((System.Drawing.Image)Resources.play, 0.5f);
            this.toolTipPictureBoxOpenFile.SetToolTip((System.Windows.Forms.Control)this.pictureBoxOpenFile, "Undefined");
            this.versionNumberToolStripMenuItem.Text = Program.Version;
            TaskbarIcon taskbarIcon = new TaskbarIcon();
            taskbarIcon.Icon = Resources.icon;
            taskbarIcon.ToolTipText = "Free YouTube Downloader";
            int num2 = 2;
            taskbarIcon.Visibility = (Visibility)num2;
            this._notifyIcon = taskbarIcon;
            System.Windows.Controls.ContextMenu contextMenu = new System.Windows.Controls.ContextMenu();
            System.Windows.Controls.MenuItem menuItem1 = new System.Windows.Controls.MenuItem();
            string openApplication = Strings.OpenApplication;
            menuItem1.Header = (object)openApplication;
            System.Windows.Controls.MenuItem openAppMenuItem = menuItem1;
            openAppMenuItem.Click += (RoutedEventHandler)((sender, args) => this.OnNotifyIconClick((object)openAppMenuItem, (EventArgs)new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
            contextMenu.Items.Add((object)openAppMenuItem);
            System.Windows.Controls.MenuItem menuItem2 = new System.Windows.Controls.MenuItem();
            string exit = Strings.Exit;
            menuItem2.Header = (object)exit;
            System.Windows.Controls.MenuItem exitAppMenuItem = menuItem2;
            exitAppMenuItem.Click += (RoutedEventHandler)((sender, args) => this.OnExitMenuItemClick((object)exitAppMenuItem, EventArgs.Empty));
            contextMenu.Items.Add((object)exitAppMenuItem);
            this._notifyIcon.ContextMenu = contextMenu;
            this._notifyIcon.TrayLeftMouseUp += (RoutedEventHandler)((sender, args) => this.OnNotifyIconClick((object)openAppMenuItem, (EventArgs)new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0)));
        }

        private void InitializeFromSettings()
        {
            if (Settings.Instance.WindowWidth > 0 && Settings.Instance.WindowHeight > 0)
                this.Size = new System.Drawing.Size(Settings.Instance.WindowWidth, Settings.Instance.WindowHeight);
            this.alwaysOnTopMenuItem.Checked = Settings.Instance.AlwaysOnTop;
            this.TopMost = Settings.Instance.AlwaysOnTop;
            this.compatibleURLNotificationToolStripMenuItem.Checked = Settings.Instance.NotifyUrlInClipboard;
            switch (Settings.Instance.FileLocationOption)
            {
                case FileLocationOption.OriginalNameAndDefaultFolder:
                    this.askFileNameAndFolderToolStripMenuItem.Checked = false;
                    break;
                case FileLocationOption.AskMeForNameAndFolder:
                    this.askFileNameAndFolderToolStripMenuItem.Checked = true;
                    break;
            }
            this.olvColumnName.Width = Settings.Instance.NameColumnWidth;
            if (LicenseHelper.IsGenuine)
            {
                this.autoDownloadMenuItem.Checked = this.videoAutoDownloadMenuItem.Checked = Settings.Instance.AutomaticallyDownloadVideo;
                this.autoDownloadMenuItem.Checked = this.audioAutoDownloadMenuItem.Checked = Settings.Instance.AutomaticallyDownloadAudio;
            }
            else
                Settings.Instance.AutomaticallyDownloadVideo = Settings.Instance.AutomaticallyDownloadAudio = false;
            Settings.Instance.SettingsChanged += new Settings.SettingsChangedEventHandler(this.OnApplicationSettingsChanged);
        }

        private void splitButtonActionVideo_Paint(object sender, PaintEventArgs e)
        {
            this.RefreshControl(sender as System.Windows.Forms.Control, "picBoxVideo");
        }

        private void splitButtonActionAudio_Paint(object sender, PaintEventArgs e)
        {
            this.RefreshControl(sender as System.Windows.Forms.Control, "picBoxAudio");
        }

        private void InitSearchPanelContainer()
        {
            this.PanelWrapperContainerAutoComplete.Width = this.PanelContainerAutoComplete.Width = this.textBoxUrl.Width + this._additionalWidth;
            this.PanelContainerAutoComplete.AutoScroll = true;
            this.PanelContainerAutoComplete.MouseWheel += new MouseEventHandler(this.OnPanelContainerAutoCompleteMouseWheel);
            this.PanelWrapperContainerAutoComplete.Visible = false;
        }

        private void ProgressBarLoadingDataShow()
        {
            this.PanelWrapperContainerAutoComplete.Width = this.PanelContainerAutoComplete.Width = this.textBoxUrl.Width + this._additionalWidth;
            this.PanelContainerAutoComplete.Height = 0;
            this.progressBarLoadingData.Location = new System.Drawing.Point(0, 0);
            this.PanelWrapperContainerAutoComplete.Height = this.progressBarLoadingData.Height + 2;
            this.PanelWrapperContainerAutoComplete.BorderStyle = BorderStyle.None;
            this.PanelWrapperContainerAutoComplete.Visible = this.progressBarLoadingData.Visible = true;
        }

        private void ResizePanelSearchResultControls(bool forseResize = false)
        {
            if (!forseResize && this.GetRowSearchResultHeight() == 0)
                return;
            System.Windows.Forms.Label searchResultTitle = this.lblSearchResultTitle;
            System.Drawing.Point location1 = this.lblSearchResultTime.Location;
            int x1 = location1.X;
            location1 = this.lblSearchResultTitle.Location;
            int x2 = location1.X;
            int num1 = x1 - x2 - 2;
            searchResultTitle.Width = num1;
            System.Windows.Forms.Label resultDescription = this.lblSearchResultDescription;
            System.Drawing.Point location2 = this.pictureBoxOpenFile.Location;
            int num2 = location2.X - 7;
            location2 = this.lblSearchResultDescription.Location;
            int x3 = location2.X;
            int num3 = num2 - x3;
            resultDescription.Width = num3;
        }

        private void ConfigObjectListView()
        {
            this.olvColumnStatus.Renderer = (IRenderer)new ImageRenderer();
            this.olvColumnStatus.AspectGetter += (AspectGetterDelegate)(rowObject =>
           {
               switch (((DownloadItem)rowObject).State)
               {
                   case DownloadState.Preparing:
                       this.Status = string.Format("{0} ...", (object)Strings.Preparing);
                       goto case DownloadState.Waiting;
                   case DownloadState.Waiting:
                   case DownloadState.InQueue:
                       return (object)Resources.Waiting;
                   case DownloadState.Downloading:
                       return (object)Resources.Downloading;
                   case DownloadState.Completed:
                       return (object)Resources.Completed;
                   case DownloadState.Error:
                       return (object)Resources.Error;
                   case DownloadState.Paused:
                       return (object)Resources.Pause;
                   case DownloadState.Converting:
                       return (object)Resources.Converting;
                   case DownloadState.AnalizeSource:
                       return (object)Resources.AnalizeSource;
                   case DownloadState.Merging:
                       return (object)Strings.Merging;
                   default:
                       return (object)null;
               }
           });
            this.olvColumnStatus.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnName.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnSize.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnProgress.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnDownloadSpeed.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnEta.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnFrameSize.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvColumnDuration.GroupKeyGetter += new GroupKeyGetterDelegate(this.GetGroupName);
            this.olvDownloads.CellToolTipShowing += (EventHandler<ToolTipShowingEventArgs>)((sender, args) =>
           {
               if (args.ColumnIndex != 0)
                   return;
               DownloadItem model = (DownloadItem)args.Model;
               args.Text = model.GetDownloadStatus();
           });
            this.olvColumnSize.AspectToStringConverter += (AspectToStringConverterDelegate)(value =>
           {
               if (!(value is long))
                   return value.ToString();
               return ((long)value).ToFileSize(true);
           });
            this.olvColumnDownloadSpeed.AspectToStringConverter += (AspectToStringConverterDelegate)(value =>
           {
               if (!(value is long))
                   return value.ToString();
               return ((long)value).ToInternetSpeed();
           });
            this.olvColumnEta.AspectToStringConverter += (AspectToStringConverterDelegate)(value =>
           {
               if (!(value is long))
                   return value.ToString();
               return ((long)value).FormatSeconds();
           });
            this.olvColumnDuration.AspectToStringConverter += (AspectToStringConverterDelegate)(value =>
           {
               if (!(value is double))
                   return "-";
               return ((double)value).MillisecondsToHumanFriendlyRepresentation(false, false, false);
           });
            this.olvDownloads.BeforeCreatingGroups += (EventHandler<CreateGroupsEventArgs>)((sender, args) =>
           {
               if (args.Parameters.GroupByColumn != this.olvColumnStatus)
                   return;
               if (args.Parameters.PrimarySortOrder == SortOrder.Ascending)
                   args.Parameters.ItemComparer = (IComparer<OLVListItem>)new DownloadItemComparerAscending();
               else
                   args.Parameters.ItemComparer = (IComparer<OLVListItem>)new DownloadItemComparerDescending();
           });
        }

        private ContextMenuStrip CreateContextMenuForDownload(DownloadItem download)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.SuspendLayout();
            contextMenuStrip.Size = new System.Drawing.Size(153, 70);
            if (download.State == DownloadState.Downloading || download.State == DownloadState.Paused)
            {
                if (download.State == DownloadState.Downloading)
                {
                    ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.Pause, (System.Drawing.Image)null, new EventHandler(this.OnPauseDownloadClick));
                    DownloadItem downloadItem = download;
                    toolStripMenuItem1.Tag = (object)downloadItem;
                    Bitmap pause = Resources.Pause;
                    toolStripMenuItem1.Image = (System.Drawing.Image)pause;
                    ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                    contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
                }
                else
                {
                    ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.Resume, (System.Drawing.Image)null, new EventHandler(this.OnResumeDownloadClick));
                    DownloadItem downloadItem = download;
                    toolStripMenuItem1.Tag = (object)downloadItem;
                    Bitmap actionResume = Resources.ActionResume;
                    toolStripMenuItem1.Image = (System.Drawing.Image)actionResume;
                    ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                    contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
                }
                contextMenuStrip.Items.Add((ToolStripItem)new ToolStripSeparator());
            }
            if (download.State != DownloadState.Completed && download.State != DownloadState.Converting)
            {
                ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.CopyVideoURLToClipboard, (System.Drawing.Image)null, new EventHandler(this.OnCopyVideoUrlDownloadClick));
                DownloadItem downloadItem1 = download;
                toolStripMenuItem1.Tag = (object)downloadItem1;
                Bitmap copy = Resources.copy;
                toolStripMenuItem1.Image = (System.Drawing.Image)copy;
                ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
                ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(Strings.ShareVideoOnFacebook, (System.Drawing.Image)null, new EventHandler(this.OnShareVideoOnFacebookDownloadClick));
                DownloadItem downloadItem2 = download;
                toolStripMenuItem3.Tag = (object)downloadItem2;
                Bitmap facebook = Resources.facebook;
                toolStripMenuItem3.Image = (System.Drawing.Image)facebook;
                ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem4);
                ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(Strings.ShareThisVideoOnTwitter, (System.Drawing.Image)null, new EventHandler(this.OnShareVideoOnTwitterDownloadClick));
                DownloadItem downloadItem3 = download;
                toolStripMenuItem5.Tag = (object)downloadItem3;
                Bitmap twitterImage = Resources.twitter_image;
                toolStripMenuItem5.Image = (System.Drawing.Image)twitterImage;
                ToolStripMenuItem toolStripMenuItem6 = toolStripMenuItem5;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem6);
                contextMenuStrip.Items.Add((ToolStripItem)new ToolStripSeparator());
            }
            if (download.State == DownloadState.Error)
            {
                ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.Retry, (System.Drawing.Image)null, new EventHandler(this.OnRetryDownloadClick));
                DownloadItem downloadItem = download;
                toolStripMenuItem1.Tag = (object)downloadItem;
                Bitmap refresh = Resources.Refresh;
                toolStripMenuItem1.Image = (System.Drawing.Image)refresh;
                ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
            }
            if (download.State == DownloadState.Completed && download.FileExists && this.olvDownloads.SelectedItems.Count == 1)
            {
                ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.OpenFileInDefaultPlayer, (System.Drawing.Image)null, new EventHandler(this.OnPlayDownloadClick));
                DownloadItem downloadItem1 = download;
                toolStripMenuItem1.Tag = (object)downloadItem1;
                Bitmap mediaPlay = Resources.MediaPlay;
                toolStripMenuItem1.Image = (System.Drawing.Image)mediaPlay;
                ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
                ToolStripMenuItem toolStripMenuItem3 = new ToolStripMenuItem(Strings.OpenFileLocation, (System.Drawing.Image)null, new EventHandler(this.OnOpenFileLocationDownloadClick));
                DownloadItem downloadItem2 = download;
                toolStripMenuItem3.Tag = (object)downloadItem2;
                Bitmap folder = Resources.Folder;
                toolStripMenuItem3.Image = (System.Drawing.Image)folder;
                ToolStripMenuItem toolStripMenuItem4 = toolStripMenuItem3;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem4);
                contextMenuStrip.Items.Add((ToolStripItem)new ToolStripSeparator());
                if (!string.IsNullOrEmpty(download.SourceUrl))
                {
                    ToolStripMenuItem toolStripMenuItem5 = new ToolStripMenuItem(Strings.CopyVideoURLToClipboard, (System.Drawing.Image)null, new EventHandler(this.OnCopyVideoUrlDownloadClick));
                    DownloadItem downloadItem3 = download;
                    toolStripMenuItem5.Tag = (object)downloadItem3;
                    Bitmap copy = Resources.copy;
                    toolStripMenuItem5.Image = (System.Drawing.Image)copy;
                    ToolStripMenuItem toolStripMenuItem6 = toolStripMenuItem5;
                    contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem6);
                    ToolStripMenuItem toolStripMenuItem7 = new ToolStripMenuItem(Strings.ShareVideoOnFacebook, (System.Drawing.Image)null, new EventHandler(this.OnShareVideoOnFacebookDownloadClick));
                    DownloadItem downloadItem4 = download;
                    toolStripMenuItem7.Tag = (object)downloadItem4;
                    Bitmap facebook = Resources.facebook;
                    toolStripMenuItem7.Image = (System.Drawing.Image)facebook;
                    ToolStripMenuItem toolStripMenuItem8 = toolStripMenuItem7;
                    contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem8);
                    ToolStripMenuItem toolStripMenuItem9 = new ToolStripMenuItem(Strings.ShareThisVideoOnTwitter, (System.Drawing.Image)null, new EventHandler(this.OnShareVideoOnTwitterDownloadClick));
                    DownloadItem downloadItem5 = download;
                    toolStripMenuItem9.Tag = (object)downloadItem5;
                    Bitmap twitterImage = Resources.twitter_image;
                    toolStripMenuItem9.Image = (System.Drawing.Image)twitterImage;
                    ToolStripMenuItem toolStripMenuItem10 = toolStripMenuItem9;
                    contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem10);
                    contextMenuStrip.Items.Add((ToolStripItem)new ToolStripSeparator());
                }
            }
            ToolStripMenuItem toolStripMenuItem11 = new ToolStripMenuItem(Strings.Remove, (System.Drawing.Image)null, new EventHandler(this.OnRemoveDownloadClick));
            DownloadItem downloadItem6 = download;
            toolStripMenuItem11.Tag = (object)downloadItem6;
            Bitmap delete2_1 = Resources.delete2;
            toolStripMenuItem11.Image = (System.Drawing.Image)delete2_1;
            ToolStripMenuItem toolStripMenuItem12 = toolStripMenuItem11;
            contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem12);
            List<DownloadItem> list = this.olvDownloads.Objects.Cast<DownloadItem>().Where<DownloadItem>((Func<DownloadItem, bool>)(item => item.State == DownloadState.Completed)).ToList<DownloadItem>();
            if (list.Any<DownloadItem>())
            {
                ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.RemoveAllFinishedFiles, (System.Drawing.Image)null, new EventHandler(this.OnRemoveAllFinishedDownloadsClick));
                List<DownloadItem> downloadItemList = list;
                toolStripMenuItem1.Tag = (object)downloadItemList;
                Bitmap delete2_2 = Resources.delete2;
                toolStripMenuItem1.Image = (System.Drawing.Image)delete2_2;
                ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
            }
            if (download.State == DownloadState.Completed && download.FileExists)
            {
                ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(Strings.Delete, (System.Drawing.Image)null, new EventHandler(this.OnDeleteDownloadClick));
                DownloadItem downloadItem1 = download;
                toolStripMenuItem1.Tag = (object)downloadItem1;
                Bitmap delete = Resources.Delete;
                toolStripMenuItem1.Image = (System.Drawing.Image)delete;
                ToolStripMenuItem toolStripMenuItem2 = toolStripMenuItem1;
                contextMenuStrip.Items.Add((ToolStripItem)toolStripMenuItem2);
            }
            contextMenuStrip.ResumeLayout(false);
            contextMenuStrip.PerformLayout();
            return contextMenuStrip;
        }

        private string GetGroupName(object rowObject)
        {
            DownloadItem downloadItem = (DownloadItem)rowObject;
            if (downloadItem.State == DownloadState.Completed || downloadItem.State == DownloadState.Error)
                return string.Format(Strings.CompletedStringPattern, (object)DownloadItemViewModel.Instance.Downloads.Count<DownloadItem>((Func<DownloadItem, bool>)(di =>
              {
                  if (di.State != DownloadState.Completed)
                      return di.State == DownloadState.Error;
                  return true;
              })));
            if (downloadItem.State == DownloadState.Converting)
                return string.Format(Strings.ConvertingStringPattern, (object)DownloadItemViewModel.Instance.Downloads.Count<DownloadItem>((Func<DownloadItem, bool>)(di => di.State == DownloadState.Converting)));
            return string.Format(Strings.ActiveStringPattern, (object)DownloadItemViewModel.Instance.Downloads.Count<DownloadItem>((Func<DownloadItem, bool>)(di =>
          {
              if (di.State != DownloadState.Completed)
                  return di.State != DownloadState.Error;
              return false;
          })));
        }

        internal void SetStatusStrip(string value)
        {
            try
            {
                if (this.statusStrip.InvokeRequired)
                {
                    this.statusStrip.Invoke((Delegate)new Action<string>(this.SetStatusStrip), (object)value);
                }
                else
                {
                    if (this.statusStrip.Items.Count <= 0)
                        return;
                    this.statusStrip.Items[0].Text = value;
                }
            }
            catch (ObjectDisposedException ex)
            {
            }
        }

        private void SetEnableDownloadButtons(bool enable, bool isTitle)
        {
            if (MainWindow.GetControlEnable((System.Windows.Forms.Control)this.splitButtonActionVideo) != enable)
                MainWindow.SetControlEnable((System.Windows.Forms.Control)this.splitButtonActionVideo, enable, isTitle ? Strings.DownloadAsVideo : (string)null);
            if (MainWindow.GetControlEnable((System.Windows.Forms.Control)this.splitButtonActionAudio) == enable)
                return;
            MainWindow.SetControlEnable((System.Windows.Forms.Control)this.splitButtonActionAudio, enable, isTitle ? Strings.DownloadAsAudio : (string)null);
        }

        private void SetDefaultTextDownloadButtons(bool isLocalFile)
        {
            if (!isLocalFile)
            {
                MainWindow.SetControlText((System.Windows.Forms.Control)this.splitButtonActionVideo, Strings.DownloadAsVideo);
                MainWindow.SetControlText((System.Windows.Forms.Control)this.splitButtonActionAudio, Strings.DownloadAsAudio);
            }
            else
            {
                MainWindow.SetControlText((System.Windows.Forms.Control)this.splitButtonActionVideo, Strings.ConvertToVideo);
                MainWindow.SetControlText((System.Windows.Forms.Control)this.splitButtonActionAudio, Strings.ConvertToAudio);
            }
        }

        private void SearchDropDownPanelHide(bool clearOldResults)
        {
            if (clearOldResults && this.PanelContainerAutoComplete.Controls.Count > 0)
                this.PanelContainerAutoComplete.Controls.Clear();
            if (!this.PanelWrapperContainerAutoComplete.Visible)
                return;
            this.PanelWrapperContainerAutoComplete.Visible = false;
        }

        private void SearchDropDownPanelShow(bool initDropDownRows)
        {
            if (this.PanelContainerAutoComplete.Controls.Count <= 0)
                return;
            int num1 = (this.olvDownloads.Height + 100 + this.GetRowSearchResultHeight()) / 70;
            int num2 = this.textBoxUrl.Width + this._additionalWidth;
            int num3 = this.PanelContainerAutoComplete.Controls.Count < num1 ? this.PanelContainerAutoComplete.Controls.Count * 70 : num1 * 70;
            if (this.PanelWrapperContainerAutoComplete.Width != num2)
                this.PanelWrapperContainerAutoComplete.Width = this.PanelContainerAutoComplete.Width = this.textBoxUrl.Width + this._additionalWidth;
            if (this.PanelContainerAutoComplete.Height != num3)
            {
                this.PanelContainerAutoComplete.Height = num3;
                this.PanelWrapperContainerAutoComplete.Height = this.PanelContainerAutoComplete.Height + this.progressBarLoadingData.Height + 2;
            }
            this.PanelWrapperContainerAutoComplete.Visible = true;
            this.PanelContainerAutoComplete.Focus();
            if (!initDropDownRows)
                return;
            for (int index = 0; index < this.PanelContainerAutoComplete.Controls.Count; ++index)
            {
                if (this.PanelContainerAutoComplete.Controls[index].Tag.ToString().Equals(this.TextBoxUrl))
                {
                    this.PanelContainerAutoComplete.Controls[index].BackColor = System.Drawing.Color.LightSkyBlue;
                    break;
                }
            }
        }

        private void CallInitVideoDataList()
        {
            if (this._searchVideoDataThread != null && this._searchVideoDataThread.IsAlive)
                return;
            this._searchVideoDataThread = (Thread)null;
            this.progressBarLoadingData.Value = 0;
            this.progressBarLoadingData.Visible = true;
            uint startIndex = (uint)(this.PanelContainerAutoComplete.Controls.Count + 1);
            int lastRowLocationY = this.PanelContainerAutoComplete.Controls.Count > 0 ? this.PanelContainerAutoComplete.Controls[this.PanelContainerAutoComplete.Controls.Count - 1].Location.Y : 0;
            this._searchVideoDataThread = new Thread((ParameterizedThreadStart)(i => this.InitVideoDataList(this.textBoxUrl.Text, startIndex, lastRowLocationY)))
            {
                IsBackground = true
            };
            this._searchVideoDataThread.Start();
        }

        private void RowAutoCompleteSelect(object sender)
        {
            System.Windows.Forms.Panel panel = (System.Windows.Forms.Panel)sender;
            if (panel == null)
                return;
            this.textBoxUrl.Tag = panel.Tag;
            this.SearchDropDownPanelHide(false);
            this.InitPanelSearchResultFromYoutube(panel);
            string correctUrl = MainWindow.GetCorrectUrl(panel.Tag.ToString());
            this.TextBoxUrl = correctUrl;
            this.SetTooltipToFacebookAndTwitterButtons(true);
            this.GetMediaInfo(correctUrl, false, false);
        }

        private void InitPanelSearchResultFromYoutube(System.Windows.Forms.Panel panel)
        {
            System.Windows.Forms.Control[] controlArray1 = panel.Controls.Find("Image", false);
            if (controlArray1.Length != 0)
                this.pictureBoxSearchResult.BackgroundImage = ((PictureBox)controlArray1[0]).Image;
            System.Windows.Forms.Control[] controlArray2 = panel.Controls.Find("Title", false);
            if (controlArray2.Length != 0)
                this.lblSearchResultTitle.Text = controlArray2[0].Text;
            System.Windows.Forms.Control[] controlArray3 = panel.Controls.Find("Description", false);
            if (controlArray3.Length != 0)
            {
                this.lblSearchResultDescription.Text = controlArray3[0].Text;
                this._toolTip.SetToolTip((System.Windows.Forms.Control)this.lblSearchResultDescription, this.lblSearchResultDescription.Text);
            }
            System.Windows.Forms.Control[] controlArray4 = panel.Controls.Find("Time", false);
            if (controlArray4.Length != 0)
            {
                this.lblSearchResultTime.Tag = controlArray4[0].Tag;
                this.lblSearchResultTime.Text = controlArray4[0].Text;
            }
            this.InitPictureBoxOpenFile(false);
            this.ResizePanelSearchResultControls(true);
            this.RowSearchResultShow();
            this.panelSearchResult.Focus();
        }

        private int GetRowSearchResultHeight()
        {
            return (int)this.tableLayoutPanel.RowStyles[1].Height;
        }

        private void RowSearchResultShow()
        {
            this.panelSearchResult.Visible = false;
            this.tableLayoutPanel.RowStyles[1].Height = this._tableLayoutPanelSearchResultRowHeight;
            this.panelSearchResult.Visible = true;
            this.panelSearchResult.Refresh();
        }

        private void RowSearchResultHide()
        {
            if ((double)this.tableLayoutPanel.RowStyles[1].Height <= 0.0)
                return;
            this.tableLayoutPanel.RowStyles[1].Height = 0.0f;
        }

        private bool InitPanelSearchResultByYoutubeUrl(string url)
        {
            YoutubeVideoRecord videoData = YoutubeHelper.GetYoutubeVideoDataByUrl(url, YoutubeHelper.FeedType.Json);
            int num = videoData != null ? 1 : 0;
            if (num == 0)
                return num != 0;
            this.ExecuteOnUIThread((Action)(() =>
           {
               this.TextBoxUrl = url;
               this.pictureBoxSearchResult.BackgroundImage = videoData.Thumbnail;
               this.lblSearchResultTitle.Text = videoData.Title;
               this.lblSearchResultDescription.Text = videoData.Description;
               this._toolTip.SetToolTip((System.Windows.Forms.Control)this.lblSearchResultDescription, videoData.Description);
               this.lblSearchResultTime.Tag = (object)videoData.Duration;
               this.lblSearchResultTime.Text = videoData.Duration.MillisecondsToHumanFriendlyRepresentation(false, false, false);
               this.InitPictureBoxOpenFile(false);
               this.ResizePanelSearchResultControls(false);
               this.RowSearchResultShow();
           }));
            return num != 0;
        }

        private void InitPanelSearchResultFromLocalFile(string fileName)
        {
            using (ShellFile shellFile = ShellFile.FromFilePath(fileName))
            {
                try
                {
                    double result;
                    double.TryParse(shellFile.Properties.System.Media.Duration.Value.ToString(), out result);
                    double milliseconds = result * 0.0001;
                    this.lblSearchResultTime.Tag = (object)milliseconds;
                    this.lblSearchResultTime.Text = milliseconds.MillisecondsToHumanFriendlyRepresentation(false, false, false);
                    this.lblSearchResultTitle.Text = Path.GetFileNameWithoutExtension(shellFile.Name);
                    this.pictureBoxSearchResult.BackgroundImage = (System.Drawing.Image)shellFile.Thumbnail.MediumBitmap;
                    if (!string.IsNullOrWhiteSpace(shellFile.Properties.System.FileExtension.Value))
                        this.lblSearchResultDescription.Text = string.Format("{0}: {1}", (object)Strings.Format.ToLower(), (object)shellFile.Properties.System.FileExtension.Value.Substring(1).ToUpper());
                    ulong? nullable = shellFile.Properties.System.Size.Value;
                    if (nullable.HasValue)
                    {
                        System.Windows.Forms.Label resultDescription = this.lblSearchResultDescription;
                        string text = resultDescription.Text;
                        string format = ", {0}: {1}";
                        string lower = Strings.Size.ToLower();
                        nullable = shellFile.Properties.System.Size.Value;
                        string fileSize = nullable.Value.ToFileSize(true);
                        string str1 = string.Format(format, (object)lower, (object)fileSize);
                        string str2 = text + str1;
                        resultDescription.Text = str2;
                    }
                    if (shellFile.Properties.System.Video.FrameWidth.Value.HasValue && shellFile.Properties.System.Video.FrameHeight.Value.HasValue)
                        this.lblSearchResultDescription.Text += string.Format(", {0}: {1}", (object)Strings.Resolution.ToLower(), (object)(((int)shellFile.Properties.System.Video.FrameWidth.Value.Value).ToString() + " x " + (object)shellFile.Properties.System.Video.FrameHeight.Value.Value + " px"));
                    if (shellFile.Properties.System.Audio.ChannelCount.Value.HasValue)
                        this.lblSearchResultDescription.Text += string.Format(", {0}: {1}", (object)"channel(s)", (object)shellFile.Properties.System.Audio.ChannelCount.Value.Value);
                    if (shellFile.Properties.System.Audio.EncodingBitrate.Value.HasValue)
                        this.lblSearchResultDescription.Text += string.Format(", {0}: {1}", (object)"bit rate", (object)(((int)(shellFile.Properties.System.Audio.EncodingBitrate.Value.Value / 1000U)).ToString() + " kbps"));
                    if (shellFile.Properties.System.Audio.SampleRate.Value.HasValue)
                        this.lblSearchResultDescription.Text += string.Format(", {0}: {1}", (object)"sampling rate", (object)shellFile.Properties.System.Audio.SampleRate.Value.Value.HzToHumanFriendlyRepresentation(true));
                    this.InitPictureBoxOpenFile(true);
                    this.ResizePanelSearchResultControls(false);
                    this.RowSearchResultShow();
                }
                catch
                {
                }
            }
        }

        private void InitPictureBoxOpenFile(bool localFile)
        {
            if (localFile)
            {
                this.pictureBoxOpenFile.Image = (System.Drawing.Image)Resources.player_icon;
                this.toolTipPictureBoxOpenFile.SetToolTip((System.Windows.Forms.Control)this.pictureBoxOpenFile, Strings.PlayMediaPlayer);
            }
            else
            {
                this.pictureBoxOpenFile.Image = (System.Drawing.Image)Resources.youtube_icon;
                this.toolTipPictureBoxOpenFile.SetToolTip((System.Windows.Forms.Control)this.pictureBoxOpenFile, Strings.PlayYoutubePlayer);
            }
        }

        private void RefreshControl(System.Windows.Forms.Control sender, string key)
        {
            if (sender == null || string.IsNullOrWhiteSpace(key))
                return;
            System.Windows.Forms.Control[] controlArray = sender.Parent.Controls.Find(key, false);
            if (controlArray.Length == 0)
                return;
            controlArray[0].Refresh();
        }

        private static void SetControlText(System.Windows.Forms.Control control, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Delegate)new Action<System.Windows.Forms.Control, string>(MainWindow.SetControlText), (object)control, (object)text);
            }
            else
            {
                control.Text = text;
                control.Tag = (object)null;
            }
        }

        private static void SetControlEnable(System.Windows.Forms.Control control, bool enable, string text)
        {
            if (control.InvokeRequired)
            {
                control.Invoke((Delegate)new Action<System.Windows.Forms.Control, bool, string>(MainWindow.SetControlEnable), (object)control, (object)enable, (object)text);
            }
            else
            {
                if (!control.Enabled && !enable)
                    return;
                if (!string.IsNullOrWhiteSpace(text))
                    control.Text = text;
                control.Enabled = enable;
                if (control.Enabled)
                    return;
                control.Tag = (object)null;
                if (control.ContextMenuStrip == null || control.ContextMenuStrip.Items.Count <= 0)
                    return;
                control.ContextMenuStrip.Items.Clear();
            }
        }

        private static void SetControlVisible(System.Windows.Forms.Control control, bool visible)
        {
            if (control.InvokeRequired)
                control.Invoke((Delegate)new Action<System.Windows.Forms.Control, bool>(MainWindow.SetControlVisible), (object)control, (object)visible);
            else
                control.Visible = visible;
        }

        private static void SetControlEnable(System.Windows.Forms.Control control, bool enable)
        {
            if (control.InvokeRequired)
                control.Invoke((Delegate)new Action<System.Windows.Forms.Control, bool>(MainWindow.SetControlEnable), (object)control, (object)enable);
            else
                control.Enabled = enable;
        }

        private static bool GetControlEnable(System.Windows.Forms.Control control)
        {
            if (!control.InvokeRequired)
                return control.Enabled;
            return (bool)control.Invoke((Delegate)new Func<System.Windows.Forms.Control, bool>(MainWindow.GetControlEnable), (object)control);
        }

        private static string GetCorrectUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException();
            if (url.Contains<char>('&'))
            {
                string[] strArray = url.Split('&');
                if (strArray.Length != 0 && !string.IsNullOrWhiteSpace(strArray[0]))
                    url = strArray[0];
            }
            return url;
        }

        private static void SetCultureInfoForThread(string languageCode)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals(languageCode))
                return;
            CultureInfo cultureInfo = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private static string CreateMultiLineMessage(string[] lines)
        {
            string seed = string.Empty;
            if (lines != null && lines.Length != 0)
                seed = ((IEnumerable<string>)lines).Where<string>((Func<string, bool>)(t => !string.IsNullOrWhiteSpace(t))).Aggregate<string, string>(seed, (Func<string, string, string>)((current, t) => current + (string.IsNullOrWhiteSpace(current) ? t : string.Format("{0}{1}", (object)Environment.NewLine, (object)t))));
            return seed;
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case 1:
                    if (FreeYouTubeDownloader.Common.NativeMethods.User32.AddClipboardFormatListener(this.Handle))
                        break;
                    FreeYouTubeDownloader.Debug.Log.Error("The window hadn't been added to the system-maintained clipboard format listener list", (Exception)null);
                    break;
                case 2:
                    if (FreeYouTubeDownloader.Common.NativeMethods.User32.RemoveClipboardFormatListener(this.Handle))
                        break;
                    FreeYouTubeDownloader.Debug.Log.Error(string.Format("The window hadn't been removed from the system-maintained clipboard format listener list. Last Win32 error code - {0}", (object)Marshal.GetLastWin32Error()), (Exception)null);
                    break;
                case 797:
                    uint clipboardSequenceNumber = FreeYouTubeDownloader.Common.NativeMethods.User32.GetClipboardSequenceNumber();
                    if ((int)this._lastClipboardSequenceNumber == (int)clipboardSequenceNumber)
                        break;
                    try
                    {
                        this.CheckForSupportedUrlInClipboard(true);
                        break;
                    }
                    finally
                    {
                        this._lastClipboardSequenceNumber = clipboardSequenceNumber;
                    }
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                IContainer components = this.components;
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void SetVisibleCore(bool value)
        {
            if (this._hiddenStart)
            {
                this._hiddenStart = false;
                base.SetVisibleCore(false);
                this._notifyIcon.Visibility = Visibility.Visible;
            }
            else
                base.SetVisibleCore(value);
        }

        public void ApplyCurrentLocalization()
        {
            this.textBoxUrl.Text = Strings.PasteURLHere;
            this.TextBoxUrl = (string)null;
            this.exitToolStripMenuItem.Text = Strings.Exit;
            this.minimizeToolStripMenuItem.Text = Strings.Minimize;
            this.minimizeToNotificationAreaToolStripMenuItem.Text = Strings.MinimizeToNotificationArea;
            this.contributeToolStripMenuItem.Text = Strings.Contribute;
            this.becomeAFanToolStripMenuItem.Text = Strings.BecomeAFan;
            this.settingsToolStripMenuItem.Text = Strings.Settings;
            this.preferencesToolStripMenuItem.Text = Strings.Preferences;
            this.windowToolStripMenuItem.Text = Strings.Window;
            this.versionToolStripMenuItem.Text = Strings.Version;
            this.downloadLatestVersionToolStripMenuItem.Text = Strings.DownloadLatestVersion;
            this.checkForUpdateToolStripMenuItem.Text = Strings.CheckForUpdate;
            this.olvColumnName.Text = Strings.Name;
            this.olvColumnFormat.Text = Strings.Format;
            this.olvColumnSize.Text = Strings.Size;
            this.olvColumnDownloadSpeed.Text = Strings.Speed;
            this.olvColumnEta.Text = Strings.Eta;
            this.olvColumnFrameSize.Text = Strings.Resolution;
            this.olvColumnDuration.Text = Strings.Duration;
            this.olvColumnProgress.Text = Strings.Progress;
            this.suggestToolStripMenuItem.Text = Strings.SuggestFeature;
            this.helpToolStripMenuItem.Text = Strings.Help;
            this.visitOurWebsiteToolStripMenuItem.Text = Strings.VisitOurWebsite;
            this.askAQuestionToolStripMenuItem.Text = Strings.AskAQuestion;
            this.askFileNameAndFolderToolStripMenuItem.Text = Strings.AskFileNameAndFolder;
            this.alwaysOnTopMenuItem.Text = Strings.AlwaysOnTop;
            this.toolTip.SetToolTip((System.Windows.Forms.Control)this.buttonConvert, Strings.SelectFileForConversionButtonHint);
            this.lnkLabelHelp.Text = Strings.Help;
            this.lnkLabelSupport.Text = Strings.Support;
            this.lnkLabelSettings.Text = Strings.Preferences;
            this.btnVideoFiles.Text = Strings.OpenVideoFiles;
            this.btnAudioFiles.Text = Strings.OpenAudioFiles;
            this.readDocumentationToolStripMenuItem.Text = Strings.ReadDocumentation;
            this.toolTip.SetToolTip((System.Windows.Forms.Control)this.pictureBoxCopyPath, Strings.CopyPath);
            this.reportAProblemToolStripMenuItem.Text = Strings.ReportAProblem;
            this.signupForUpdatesToolStripMenuItem.Text = Strings.SignUpForUpdates;
            this.compatibleURLNotificationToolStripMenuItem.Text = Strings.CompatibleURLNotification;
            this.aboutToolStripMenuItem.Text = Strings.About + "...";
            this.upgradeToProVersionToolStripMenuItem.Text = Strings.UpgradeToProVersion;
            ((HeaderedItemsControl)this._notifyIcon.ContextMenu.Items[0]).Header = (object)Strings.OpenApplication;
            ((HeaderedItemsControl)this._notifyIcon.ContextMenu.Items[1]).Header = (object)Strings.Exit;
            this.toolTip.SetToolTip((System.Windows.Forms.Control)this.picBoxAppNotify, Strings.HigherAudioQualitiesTooltip);
            this.autoDownloadMenuItem.Text = Strings.AutoDownloadVideos;
            this.videoAutoDownloadMenuItem.Text = Strings.Video;
            this.audioAutoDownloadMenuItem.Text = Strings.Audio;
            this.SetDefaultTextDownloadButtons(false);
        }

        private void SetTooltipToFacebookAndTwitterButtons(bool urlVideoAvailable)
        {
            if (urlVideoAvailable)
            {
                this.toolTip.SetToolTip((System.Windows.Forms.Control)this.picBoxFacebook, Strings.ShareThisVideoOnFacebook);
                this.toolTip.SetToolTip((System.Windows.Forms.Control)this.picBoxTwitter, Strings.ShareThisVideoOnTwitter);
            }
            else
            {
                this.toolTip.SetToolTip((System.Windows.Forms.Control)this.picBoxFacebook, Strings.ShareVideoOnFacebook);
                this.toolTip.SetToolTip((System.Windows.Forms.Control)this.picBoxTwitter, Strings.ShareVideoOnTwitter);
            }
        }

        private string GetLocalizedStatusText(DownloadState downloadState)
        {
            string str;
            switch (downloadState)
            {
                case DownloadState.Downloading:
                    str = string.Format("{0} ...", (object)Strings.Downloading);
                    break;
                case DownloadState.Error:
                    str = Strings.Error;
                    break;
                case DownloadState.Paused:
                    str = Strings.Paused;
                    break;
                case DownloadState.Converting:
                    str = string.Format("{0} ...", (object)Strings.Converting);
                    break;
                case DownloadState.Canceled:
                    str = Strings.Canceled;
                    break;
                case DownloadState.Merging:
                    str = Strings.Merging;
                    break;
                default:
                    str = Strings.Done;
                    break;
            }
            return str;
        }

        public void ShowBottomAd(bool createNew = true)
        {
            FreeYouTubeDownloader.Debug.Log.Trace(string.Format("CALL MainWindow.ShowBottomAd(createNew:{0})", (object)createNew), (Exception)null);
            if (LicenseHelper.IsGenuine || this.WindowState == FormWindowState.Maximized)
                return;
            if (this.InvokeRequired)
                this.Invoke(new Action(() => this.ShowBottomAd(createNew)));
            else if (!createNew)
            {
                if (this._adBottomHost == null)
                    return;
                this.OnBotomAdFinishLoading((object)this._adBottomHost, new WebBrowserDocumentCompletedEventArgs(new Uri("https://youtubedownloader.com/wd/ads/728x90.html")));
            }
            else
            {
                if (this._bottomAdShown)
                    return;
                if (this._adBottomHost == null)
                {
                    FreeYouTubeDownloader.Debug.Log.Trace("Creating bottom ad", (Exception)null);
                    WebBrowserEx webBrowserEx = new WebBrowserEx();
                    int num1 = 1;
                    webBrowserEx.ScriptErrorsSuppressed = num1 != 0;
                    int num2 = 0;
                    webBrowserEx.ScrollBarsEnabled = num2 != 0;
                    this._adBottomHost = webBrowserEx;
                    this._adBottomHost.NavigateError += (WebBrowserExNavigateErrorEventHandler)((sender, args) => this.HideBottomAd(true));
                    this._adBottomHost.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.OnBotomAdFinishLoading);
                    this._adBottomHost.NewWindow3 += (WebBrowserExNewWindow3EventHandler)((sender, args) =>
                   {
                       this.HideBottomAd(false);
                       args.Cancel = true;
                       MainWindow.RunDefaultApp(args.Url, false, false);
                   });
                    this._adBottomHost.Navigate("https://youtubedownloader.com/wd/ads/728x90.html");
                    this.panelAdBottom.Controls.Add((System.Windows.Forms.Control)this._adBottomHost);
                    PictureBox closeButton = AdsHelper.CreateCloseButton(712, 0);
                    closeButton.Click += (EventHandler)((sender, args) =>
                   {
                       this.PromptForPro();
                       this.HideBottomAd(false);
                   });
                    this.panelAdBottom.Controls.Add((System.Windows.Forms.Control)closeButton);
                    this.panelAdBottom.Controls.SetChildIndex((System.Windows.Forms.Control)closeButton, 1);
                    closeButton.BringToFront();
                    System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer()
                    {
                        Interval = 1000
                    };
                    timer1.Tick += (EventHandler)((sender, args) =>
                   {
                       ((System.Windows.Forms.Timer)sender).Stop();
                       this.panelAdBottom.Controls[0].Visible = true;
                   });
                    this.panelAdBottom.Tag = (object)timer1;
                    System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer()
                    {
                        Interval = 604800000
                    };
                    timer2.Tick += (EventHandler)((sender, args) =>
                   {
                       WebBrowserEx adBottomHost = this._adBottomHost;
                       if (adBottomHost == null)
                           return;
                       string urlString = "https://youtubedownloader.com/wd/ads/728x90.html";
                       adBottomHost.Navigate(urlString);
                   });
                    this._adBottomHost.Tag = (object)timer2;
                }
                else
                    this._adBottomHost.Navigate("https://youtubedownloader.com/wd/ads/728x90.html");
            }
        }

        public void OnBotomAdFinishLoading(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            FreeYouTubeDownloader.Debug.Log.Trace("EVENT MainWindow.OnBotomAdFinishLoading", (Exception)null);
            if (args.Url.ToString() != "https://youtubedownloader.com/wd/ads/728x90.html" || this.WindowState == FormWindowState.Maximized || LicenseHelper.IsGenuine)
                return;
            this.Invoke(new Action(() =>
            {
                this.tableLayoutPanel.RowStyles[4].Height = 100f;
                this._adBottomHost.Size = this._adBottomHost.Document.Window.Size;
                this._adBottomHost.Visible = true;
                System.Windows.Forms.Timer tag1 = (System.Windows.Forms.Timer)this.panelAdBottom.Tag;
                if (!tag1.Enabled)
                    tag1.Start();
                System.Windows.Forms.Timer tag2 = (System.Windows.Forms.Timer)this._adBottomHost.Tag;
                if (tag2.Enabled)
                    return;
                tag2.Start();
            }));
        }

        public void HideBottomAd(bool onlyHide = true)
        {
            FreeYouTubeDownloader.Debug.Log.Trace(string.Format("CALL MainWindow.HideBottomAd(onlyHide:{0})", (object)onlyHide), (Exception)null);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.HideBottomAd(onlyHide)));
            }
            else
            {
                this.tableLayoutPanel.RowStyles[4].Height = 0.0f;
                if (this.panelAdBottom.Controls.Count > 0)
                {
                    this.panelAdBottom.Controls[0].Visible = false;
                    if (this._adBottomHost != null)
                    {
                        this.olvDownloads.Focus();
                        this._adBottomHost.Visible = false;
                    }
                }
                if (onlyHide)
                    return;
                ((System.Windows.Forms.Timer)this._adBottomHost.Tag).Stop();
                this._adBottomHost = (WebBrowserEx)null;
                this._bottomAdShown = true;
            }
        }

        public void ShowRightAd(bool createNew = true)
        {
            FreeYouTubeDownloader.Debug.Log.Trace(string.Format("CALL MainWindow.ShowRightAd(createNew:{0})", (object)createNew), (Exception)null);
            if (LicenseHelper.IsGenuine || this.WindowState != FormWindowState.Maximized)
                return;
            if (this.InvokeRequired)
                this.Invoke(new Action(() => this.ShowRightAd(createNew)));
            else if (!createNew)
            {
                if (this._adRightHost == null)
                    return;
                this.OnRightAdFinishLoading((object)this._adRightHost, new WebBrowserDocumentCompletedEventArgs(new Uri("https://youtubedownloader.com/wd/ads/160x600.html")));
            }
            else
            {
                if (this._rightAdShown)
                    return;
                if (this._adRightHost == null)
                {
                    FreeYouTubeDownloader.Debug.Log.Trace("Creating right ad", (Exception)null);
                    WebBrowserEx webBrowserEx = new WebBrowserEx();
                    int num1 = 1;
                    webBrowserEx.ScriptErrorsSuppressed = num1 != 0;
                    int num2 = 0;
                    webBrowserEx.ScrollBarsEnabled = num2 != 0;
                    this._adRightHost = webBrowserEx;
                    this._adRightHost.NavigateError += (WebBrowserExNavigateErrorEventHandler)((sender, args) => this.HideRightAd(true));
                    this._adRightHost.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.OnRightAdFinishLoading);
                    this._adRightHost.NewWindow3 += (WebBrowserExNewWindow3EventHandler)((sender, args) =>
                   {
                       this.HideRightAd(false);
                       args.Cancel = true;
                       MainWindow.RunDefaultApp(args.Url, false, false);
                   });
                    this._adRightHost.Navigate("https://youtubedownloader.com/wd/ads/160x600.html");
                    this.panelAdRight.Controls.Add((System.Windows.Forms.Control)this._adRightHost);
                    PictureBox closeButton = AdsHelper.CreateCloseButton(144, 584);
                    closeButton.Click += (EventHandler)((sender, args) =>
                   {
                       this.PromptForPro();
                       this.HideRightAd(false);
                   });
                    this.panelAdRight.Controls.Add((System.Windows.Forms.Control)closeButton);
                    this.panelAdRight.Controls.SetChildIndex((System.Windows.Forms.Control)closeButton, 1);
                    closeButton.BringToFront();
                    System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer()
                    {
                        Interval = 1000
                    };
                    timer1.Tick += (EventHandler)((sender, args) =>
                   {
                       ((System.Windows.Forms.Timer)sender).Stop();
                       this.panelAdRight.Controls[0].Visible = true;
                   });
                    this.panelAdRight.Tag = (object)timer1;
                    System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer()
                    {
                        Interval = 604800000
                    };
                    timer2.Tick += (EventHandler)((sender, args) =>
                   {
                       WebBrowserEx adRightHost = this._adRightHost;
                       if (adRightHost == null)
                           return;
                       string urlString = "https://youtubedownloader.com/wd/ads/160x600.html";
                       adRightHost.Navigate(urlString);
                   });
                    this._adRightHost.Tag = (object)timer2;
                }
                else
                    this._adRightHost.Navigate("https://youtubedownloader.com/wd/ads/160x600.html");
            }
        }

        public void OnRightAdFinishLoading(object sender, WebBrowserDocumentCompletedEventArgs args)
        {
            FreeYouTubeDownloader.Debug.Log.Trace("EVENT MainWindow.OnRightAdFinishLoading", (Exception)null);
            if (args.Url.ToString() != "https://youtubedownloader.com/wd/ads/160x600.html" || this.WindowState != FormWindowState.Maximized || LicenseHelper.IsGenuine)
                return;
            this.Invoke(new Action(() =>
            {
                this.tableLayoutPanel.ColumnStyles[2].Width = 167f;
                this._adRightHost.Size = this._adRightHost.Document.Window.Size;
                this._adRightHost.Visible = true;
                System.Windows.Forms.Timer tag1 = (System.Windows.Forms.Timer)this.panelAdRight.Tag;
                if (!tag1.Enabled)
                    tag1.Start();
                System.Windows.Forms.Timer tag2 = (System.Windows.Forms.Timer)this._adRightHost.Tag;
                if (tag2.Enabled)
                    return;
                tag2.Start();
            }));
        }

        public void HideRightAd(bool onlyHide = true)
        {
            FreeYouTubeDownloader.Debug.Log.Trace(string.Format("CALL MainWindow.HideRightAd(onlyHide:{0})", (object)onlyHide), (Exception)null);
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.HideRightAd(onlyHide)));
            }
            else
            {
                this.tableLayoutPanel.ColumnStyles[2].Width = 0.0f;
                if (this.panelAdRight.Controls.Count > 0)
                {
                    this.panelAdRight.Controls[0].Visible = false;
                    if (this._adRightHost != null)
                    {
                        this.olvDownloads.Focus();
                        this._adRightHost.Visible = false;
                    }
                }
                if (onlyHide)
                    return;
                ((System.Windows.Forms.Timer)this._adRightHost.Tag).Stop();
                this._adRightHost = (WebBrowserEx)null;
                this._rightAdShown = true;
            }
        }

        private void PromptForPro()
        {
            if (System.Windows.Forms.MessageBox.Show(new string[3]
            {
        Strings.AdCloseText,
        Strings.AdCloseText2,
        Strings.AdCloseText3
            }[new Random().Next(3)], Strings.AdCloseCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) != DialogResult.Yes)
                return;
            MainWindow.RunDefaultApp("https://youtubedownloader.com/subscribe-to-pro-youtube-downloader/", false, false);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainWindow));
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.statusStrip = new StatusStrip();
            this.toolStripStatusLabel = new ToolStripStatusLabel();
            this.olvDownloads = new ObjectListView();
            this.olvColumnStatus = new OLVColumn();
            this.olvColumnName = new OLVColumn();
            this.olvColumnFormat = new OLVColumn();
            this.olvColumnSize = new OLVColumn();
            this.olvColumnDuration = new OLVColumn();
            this.olvColumnProgress = new OLVColumn();
            this.barRenderer = new BarRenderer();
            this.olvColumnDownloadSpeed = new OLVColumn();
            this.olvColumnEta = new OLVColumn();
            this.olvColumnFrameSize = new OLVColumn();
            this.tableLayoutPanelActionButtons = new TableLayoutPanel();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.tableLayoutPanelActionButtonsContainer = new TableLayoutPanel();
            this.picBoxFacebook = new PictureBox();
            this.picBoxTwitter = new PictureBox();
            this.panelButtonActionVideo = new System.Windows.Forms.Panel();
            this.picBoxVideo = new TransparentPictureBox();
            this.splitButtonActionVideo = new SplitButton();
            this.contextMenuStripDownloadVideo = new ContextMenuStrip(this.components);
            this.panelButtonActionAudio = new System.Windows.Forms.Panel();
            this.picBoxAudio = new TransparentPictureBox();
            this.splitButtonActionAudio = new SplitButton();
            this.contextMenuStripDownloadAudio = new ContextMenuStrip(this.components);
            this.picBoxAppNotify = new PictureBox();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.picBoxHelp = new PictureBox();
            this.lnkLabelHelp = new LinkLabel();
            this.picBoxSupport = new PictureBox();
            this.pictureBoxSettings = new PictureBox();
            this.lnkLabelSupport = new LinkLabel();
            this.lnkLabelSettings = new LinkLabel();
            this.btnVideoFiles = new System.Windows.Forms.Button();
            this.btnAudioFiles = new System.Windows.Forms.Button();
            this.panelSearchResult = new System.Windows.Forms.Panel();
            this.pictureBoxCopyPath = new PictureBox();
            this.pictureBoxOpenFile = new PictureBox();
            this.lblSearchResultTime = new System.Windows.Forms.Label();
            this.lblSearchResultDescription = new System.Windows.Forms.Label();
            this.lblSearchResultTitle = new System.Windows.Forms.Label();
            this.pictureBoxSearchResult = new PictureBoxEx();
            this.panelAdBottom = new System.Windows.Forms.Panel();
            this.panelAdRight = new System.Windows.Forms.Panel();
            this.PanelContainerAutoComplete = new System.Windows.Forms.Panel();
            this.menuStrip = new MenuStrip();
            this.windowToolStripMenuItem = new ToolStripMenuItem();
            this.alwaysOnTopMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator1 = new ToolStripSeparator();
            this.minimizeToolStripMenuItem = new ToolStripMenuItem();
            this.minimizeToNotificationAreaToolStripMenuItem = new ToolStripMenuItem();
            this.exitToolStripMenuItem = new ToolStripMenuItem();
            this.settingsToolStripMenuItem = new ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator2 = new ToolStripSeparator();
            this.askFileNameAndFolderToolStripMenuItem = new ToolStripMenuItem();
            this.compatibleURLNotificationToolStripMenuItem = new ToolStripMenuItem();
            this.autoDownloadMenuItem = new ToolStripMenuItem();
            this.videoAutoDownloadMenuItem = new ToolStripMenuItem();
            this.audioAutoDownloadMenuItem = new ToolStripMenuItem();
            this.versionToolStripMenuItem = new ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new ToolStripMenuItem();
            this.downloadLatestVersionToolStripMenuItem = new ToolStripMenuItem();
            this.signupForUpdatesToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator4 = new ToolStripSeparator();
            this.upgradeToProVersionToolStripMenuItem = new ToolStripMenuItem();
            this.deactivateFYDProToolStripMenuItem = new ToolStripMenuItem();
            this.contributeToolStripMenuItem = new ToolStripMenuItem();
            this.reportAProblemToolStripMenuItem = new ToolStripMenuItem();
            this.suggestToolStripMenuItem = new ToolStripMenuItem();
            this.becomeAFanToolStripMenuItem = new ToolStripMenuItem();
            this.versionNumberToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.visitOurWebsiteToolStripMenuItem = new ToolStripMenuItem();
            this.askAQuestionToolStripMenuItem = new ToolStripMenuItem();
            this.readDocumentationToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripSeparator3 = new ToolStripSeparator();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PanelWrapperContainerAutoComplete = new System.Windows.Forms.Panel();
            this.progressBarLoadingData = new System.Windows.Forms.ProgressBar();
            this.toolTipPictureBoxOpenFile = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((ISupportInitialize)this.olvDownloads).BeginInit();
            this.tableLayoutPanelActionButtons.SuspendLayout();
            this.tableLayoutPanelActionButtonsContainer.SuspendLayout();
            ((ISupportInitialize)this.picBoxFacebook).BeginInit();
            ((ISupportInitialize)this.picBoxTwitter).BeginInit();
            this.panelButtonActionVideo.SuspendLayout();
            this.panelButtonActionAudio.SuspendLayout();
            ((ISupportInitialize)this.picBoxAppNotify).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((ISupportInitialize)this.picBoxHelp).BeginInit();
            ((ISupportInitialize)this.picBoxSupport).BeginInit();
            ((ISupportInitialize)this.pictureBoxSettings).BeginInit();
            this.panelSearchResult.SuspendLayout();
            ((ISupportInitialize)this.pictureBoxCopyPath).BeginInit();
            ((ISupportInitialize)this.pictureBoxOpenFile).BeginInit();
            this.menuStrip.SuspendLayout();
            this.PanelWrapperContainerAutoComplete.SuspendLayout();
            this.SuspendLayout();
            this.textBoxUrl.Dock = DockStyle.Fill;
            this.textBoxUrl.Font = new Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.textBoxUrl.Location = new System.Drawing.Point(13, 9);
            this.textBoxUrl.Margin = new Padding(13, 9, 0, 10);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(839, 24);
            this.textBoxUrl.TabIndex = 6;
            this.textBoxUrl.Text = "Paste URL here";
            this.textBoxUrl.Click += new EventHandler(this.OnTextBoxUrlClick);
            this.textBoxUrl.TextChanged += new EventHandler(this.OnTextBoxUrlTextChanged);
            this.textBoxUrl.Enter += new EventHandler(this.OnTextBoxUrlEnter);
            this.textBoxUrl.KeyDown += new KeyEventHandler(this.OnTextBoxUrlKeyDown);
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f));
            this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.textBoxUrl, 0, 0);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.statusStrip, 0, 6);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.olvDownloads, 0, 3);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.tableLayoutPanelActionButtons, 1, 0);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, 0, 2);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.tableLayoutPanel2, 0, 5);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.panelSearchResult, 0, 1);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.panelAdBottom, 0, 4);
            this.tableLayoutPanel.Controls.Add((System.Windows.Forms.Control)this.panelAdRight, 2, 0);
            this.tableLayoutPanel.Dock = DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel.Margin = new Padding(4);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 81f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 58f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 0.0f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 47f));
            this.tableLayoutPanel.RowStyles.Add(new RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(899, 575);
            this.tableLayoutPanel.TabIndex = 8;
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
            this.tableLayoutPanel.SetColumnSpan((System.Windows.Forms.Control)this.statusStrip, 2);
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new ToolStripItem[1]
            {
        (ToolStripItem) this.toolStripStatusLabel
            });
            this.statusStrip.Location = new System.Drawing.Point(0, 553);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Padding = new Padding(1, 0, 19, 0);
            this.statusStrip.Size = new System.Drawing.Size(899, 22);
            this.statusStrip.TabIndex = 9;
            this.statusStrip.Text = "statusStrip1";
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            this.olvDownloads.AllColumns.Add(this.olvColumnStatus);
            this.olvDownloads.AllColumns.Add(this.olvColumnName);
            this.olvDownloads.AllColumns.Add(this.olvColumnDuration);
            this.olvDownloads.AllColumns.Add(this.olvColumnProgress);
            this.olvDownloads.AllColumns.Add(this.olvColumnDownloadSpeed);
            this.olvDownloads.AllColumns.Add(this.olvColumnEta);
            this.olvDownloads.AllColumns.Add(this.olvColumnFormat);
            this.olvDownloads.AllColumns.Add(this.olvColumnSize);
            this.olvDownloads.AllColumns.Add(this.olvColumnFrameSize);
            this.olvDownloads.CellEditUseWholeCell = false;
            this.olvDownloads.Columns.AddRange(new ColumnHeader[9]
            {
        (ColumnHeader) this.olvColumnStatus,
        (ColumnHeader) this.olvColumnName,
        (ColumnHeader) this.olvColumnDuration,
        (ColumnHeader) this.olvColumnProgress,
        (ColumnHeader) this.olvColumnDownloadSpeed,
        (ColumnHeader) this.olvColumnEta,
        (ColumnHeader) this.olvColumnFormat,
        (ColumnHeader) this.olvColumnSize,
        (ColumnHeader) this.olvColumnFrameSize
            });
            this.tableLayoutPanel.SetColumnSpan((System.Windows.Forms.Control)this.olvDownloads, 2);
            this.olvDownloads.Cursor = Cursors.Default;
            this.olvDownloads.Dock = DockStyle.Fill;
            this.olvDownloads.FullRowSelect = true;
            this.olvDownloads.Location = new System.Drawing.Point(13, 184);
            this.olvDownloads.Margin = new Padding(13, 0, 13, 0);
            this.olvDownloads.Name = "olvDownloads";
            this.olvDownloads.Size = new System.Drawing.Size(873, 322);
            this.olvDownloads.SortGroupItemsByPrimaryColumn = false;
            this.olvDownloads.TabIndex = 10;
            this.olvDownloads.UseCompatibleStateImageBehavior = false;
            this.olvDownloads.UseNotifyPropertyChanged = true;
            this.olvDownloads.View = View.Details;
            this.olvDownloads.CellRightClick += new EventHandler<CellRightClickEventArgs>(this.OnDownloadsListCellRightClick);
            this.olvDownloads.DoubleClick += new EventHandler(this.OnDownloadsListDoubleClick);
            this.olvDownloads.KeyUp += new KeyEventHandler(this.OnDownloadsListKeyUp);
            this.olvDownloads.SelectionChanged += new EventHandler(this.OnDownloadsSelectionChanged);
            this.olvColumnStatus.IsEditable = false;
            this.olvColumnStatus.MaximumWidth = 18;
            this.olvColumnStatus.MinimumWidth = 18;
            this.olvColumnStatus.Text = "";
            this.olvColumnStatus.Width = 18;
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.IsEditable = false;
            this.olvColumnName.MinimumWidth = 170;
            this.olvColumnName.Text = "Name";
            this.olvColumnName.Width = 170;
            this.olvColumnFormat.AspectName = "FileNameFormat";
            this.olvColumnFormat.IsEditable = false;
            this.olvColumnFormat.MinimumWidth = 45;
            this.olvColumnFormat.Text = "Format";
            this.olvColumnFormat.Width = 45;
            this.olvColumnSize.AspectName = "Size";
            this.olvColumnSize.IsEditable = false;
            this.olvColumnSize.MinimumWidth = 65;
            this.olvColumnSize.Text = "Size";
            this.olvColumnSize.Width = 65;
            this.olvColumnProgress.AspectName = "Progress";
            this.olvColumnProgress.IsEditable = false;
            this.olvColumnProgress.MinimumWidth = 105;
            this.olvColumnProgress.Renderer = (IRenderer)this.barRenderer;
            this.olvColumnProgress.Text = "Progress";
            this.olvColumnProgress.Width = 105;
            this.olvColumnDownloadSpeed.AspectName = "Speed";
            this.olvColumnDownloadSpeed.IsEditable = false;
            this.olvColumnDownloadSpeed.MinimumWidth = 70;
            this.olvColumnDownloadSpeed.Text = "Speed";
            this.olvColumnDownloadSpeed.Width = 70;
            this.olvColumnEta.AspectName = "SecondsRemains";
            this.olvColumnEta.MinimumWidth = 60;
            this.olvColumnEta.Text = "ETA";
            this.olvColumnFrameSize.AspectName = "FrameSize";
            this.olvColumnFrameSize.MinimumWidth = 65;
            this.olvColumnFrameSize.Text = "Dimension";
            this.olvColumnFrameSize.Width = 65;
            this.olvColumnDuration.AspectName = "Duration";
            this.olvColumnSize.IsEditable = false;
            this.olvColumnDuration.MinimumWidth = 65;
            this.olvColumnDuration.Text = "Duration";
            this.olvColumnDuration.Width = 65;
            this.tableLayoutPanelActionButtons.ColumnCount = 1;
            this.tableLayoutPanelActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
            this.tableLayoutPanelActionButtons.Controls.Add((System.Windows.Forms.Control)this.buttonConvert, 0, 0);
            this.tableLayoutPanelActionButtons.Location = new System.Drawing.Point(856, 4);
            this.tableLayoutPanelActionButtons.Margin = new Padding(4);
            this.tableLayoutPanelActionButtons.Name = "tableLayoutPanelActionButtons";
            this.tableLayoutPanelActionButtons.RowCount = 1;
            this.tableLayoutPanelActionButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanelActionButtons.Size = new System.Drawing.Size(39, 37);
            this.tableLayoutPanelActionButtons.TabIndex = 11;
            this.buttonConvert.Cursor = Cursors.Default;
            this.buttonConvert.Location = new System.Drawing.Point(1, 4);
            this.buttonConvert.Margin = new Padding(1, 4, 4, 4);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(33, 28);
            this.buttonConvert.TabIndex = 0;
            this.buttonConvert.Text = "...";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new EventHandler(this.OnConvertButtonClick);
            this.tableLayoutPanelActionButtonsContainer.ColumnCount = 8;
            this.tableLayoutPanel.SetColumnSpan((System.Windows.Forms.Control)this.tableLayoutPanelActionButtonsContainer, 2);
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 1f));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 376f));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0.0f));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 376f));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 5f));
            this.tableLayoutPanelActionButtonsContainer.Controls.Add((System.Windows.Forms.Control)this.picBoxFacebook, 5, 0);
            this.tableLayoutPanelActionButtonsContainer.Controls.Add((System.Windows.Forms.Control)this.picBoxTwitter, 6, 0);
            this.tableLayoutPanelActionButtonsContainer.Controls.Add((System.Windows.Forms.Control)this.panelButtonActionVideo, 1, 0);
            this.tableLayoutPanelActionButtonsContainer.Controls.Add((System.Windows.Forms.Control)this.panelButtonActionAudio, 3, 0);
            this.tableLayoutPanelActionButtonsContainer.Controls.Add((System.Windows.Forms.Control)this.picBoxAppNotify, 4, 0);
            this.tableLayoutPanelActionButtonsContainer.Dock = DockStyle.Fill;
            this.tableLayoutPanelActionButtonsContainer.Location = new System.Drawing.Point(4, 130);
            this.tableLayoutPanelActionButtonsContainer.Margin = new Padding(4);
            this.tableLayoutPanelActionButtonsContainer.Name = "tableLayoutPanelActionButtonsContainer";
            this.tableLayoutPanelActionButtonsContainer.RowCount = 1;
            this.tableLayoutPanelActionButtonsContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanelActionButtonsContainer.Size = new System.Drawing.Size(891, 50);
            this.tableLayoutPanelActionButtonsContainer.TabIndex = 12;
            this.picBoxFacebook.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.picBoxFacebook.BackColor = System.Drawing.Color.Transparent;
            this.picBoxFacebook.Cursor = Cursors.Hand;
            this.picBoxFacebook.Enabled = false;
            this.picBoxFacebook.Image = (System.Drawing.Image)Resources.facebook_image_dis;
            this.picBoxFacebook.Location = new System.Drawing.Point(782, 5);
            this.picBoxFacebook.Margin = new Padding(4, 5, 4, 4);
            this.picBoxFacebook.Name = "picBoxFacebook";
            this.picBoxFacebook.Size = new System.Drawing.Size(45, 36);
            this.picBoxFacebook.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBoxFacebook.TabIndex = 4;
            this.picBoxFacebook.TabStop = false;
            this.picBoxFacebook.Click += new EventHandler(this.OnButtonFacebookClick);
            this.picBoxTwitter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.picBoxTwitter.BackColor = System.Drawing.Color.Transparent;
            this.picBoxTwitter.Cursor = Cursors.Hand;
            this.picBoxTwitter.Enabled = false;
            this.picBoxTwitter.Image = (System.Drawing.Image)Resources.twitter_image_dis;
            this.picBoxTwitter.Location = new System.Drawing.Point(835, 5);
            this.picBoxTwitter.Margin = new Padding(4, 5, 6, 4);
            this.picBoxTwitter.Name = "picBoxTwitter";
            this.picBoxTwitter.Size = new System.Drawing.Size(45, 35);
            this.picBoxTwitter.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBoxTwitter.TabIndex = 5;
            this.picBoxTwitter.TabStop = false;
            this.picBoxTwitter.Click += new EventHandler(this.OnShareVideoOnTwitterDownloadClick);
            this.panelButtonActionVideo.Controls.Add((System.Windows.Forms.Control)this.picBoxVideo);
            this.panelButtonActionVideo.Controls.Add((System.Windows.Forms.Control)this.splitButtonActionVideo);
            this.panelButtonActionVideo.Dock = DockStyle.Fill;
            this.panelButtonActionVideo.Location = new System.Drawing.Point(5, 4);
            this.panelButtonActionVideo.Margin = new Padding(4);
            this.panelButtonActionVideo.Name = "panelButtonActionVideo";
            this.panelButtonActionVideo.Size = new System.Drawing.Size(368, 42);
            this.panelButtonActionVideo.TabIndex = 6;
            this.picBoxVideo.BackColor = System.Drawing.Color.Transparent;
            this.picBoxVideo.Image = (System.Drawing.Image)Resources.movie;
            this.picBoxVideo.Location = new System.Drawing.Point(4, 4);
            this.picBoxVideo.Margin = new Padding(4);
            this.picBoxVideo.Name = "picBoxVideo";
            this.picBoxVideo.Size = new System.Drawing.Size(34, 34);
            this.picBoxVideo.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBoxVideo.TabIndex = 0;
            this.picBoxVideo.TabStop = false;
            this.splitButtonActionVideo.AutoSize = true;
            this.splitButtonActionVideo.ContextMenuStrip = this.contextMenuStripDownloadVideo;
            this.splitButtonActionVideo.Cursor = Cursors.Hand;
            this.splitButtonActionVideo.ImageAlign = ContentAlignment.MiddleRight;
            this.splitButtonActionVideo.Location = new System.Drawing.Point(28, 0);
            this.splitButtonActionVideo.Margin = new Padding(0);
            this.splitButtonActionVideo.Name = "splitButtonActionVideo";
            this.splitButtonActionVideo.Size = new System.Drawing.Size(340, 36);
            this.splitButtonActionVideo.SplitMenuStrip = this.contextMenuStripDownloadVideo;
            this.splitButtonActionVideo.TabIndex = 1;
            this.splitButtonActionVideo.Text = "Download as Video";
            this.splitButtonActionVideo.UseVisualStyleBackColor = true;
            this.splitButtonActionVideo.Click += new EventHandler(this.OnActionButtonClick);
            this.splitButtonActionVideo.Paint += new PaintEventHandler(this.splitButtonActionVideo_Paint);
            this.contextMenuStripDownloadVideo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDownloadVideo.Name = "contextMenuStripDownloadVideo";
            this.contextMenuStripDownloadVideo.ShowImageMargin = false;
            this.contextMenuStripDownloadVideo.Size = new System.Drawing.Size(36, 4);
            this.panelButtonActionAudio.Controls.Add((System.Windows.Forms.Control)this.picBoxAudio);
            this.panelButtonActionAudio.Controls.Add((System.Windows.Forms.Control)this.splitButtonActionAudio);
            this.panelButtonActionAudio.Dock = DockStyle.Fill;
            this.panelButtonActionAudio.Location = new System.Drawing.Point(381, 4);
            this.panelButtonActionAudio.Margin = new Padding(4);
            this.panelButtonActionAudio.Name = "panelButtonActionAudio";
            this.panelButtonActionAudio.Size = new System.Drawing.Size(368, 42);
            this.panelButtonActionAudio.TabIndex = 7;
            this.picBoxAudio.BackColor = System.Drawing.Color.Transparent;
            this.picBoxAudio.Image = (System.Drawing.Image)Resources.music;
            this.picBoxAudio.Location = new System.Drawing.Point(6, 4);
            this.picBoxAudio.Margin = new Padding(4);
            this.picBoxAudio.Name = "picBoxAudio";
            this.picBoxAudio.Size = new System.Drawing.Size(34, 34);
            this.picBoxAudio.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBoxAudio.TabIndex = 3;
            this.picBoxAudio.TabStop = false;
            this.splitButtonActionAudio.AutoSize = true;
            this.splitButtonActionAudio.ContextMenuStrip = this.contextMenuStripDownloadAudio;
            this.splitButtonActionAudio.Cursor = Cursors.Hand;
            this.splitButtonActionAudio.ImageAlign = ContentAlignment.MiddleRight;
            this.splitButtonActionAudio.Location = new System.Drawing.Point(29, 0);
            this.splitButtonActionAudio.Margin = new Padding(4);
            this.splitButtonActionAudio.Name = "splitButtonActionAudio";
            this.splitButtonActionAudio.Size = new System.Drawing.Size(339, 36);
            this.splitButtonActionAudio.SplitMenuStrip = this.contextMenuStripDownloadAudio;
            this.splitButtonActionAudio.TabIndex = 2;
            this.splitButtonActionAudio.Text = "Download as Audio";
            this.splitButtonActionAudio.UseVisualStyleBackColor = true;
            this.splitButtonActionAudio.Click += new EventHandler(this.OnActionButtonClick);
            this.splitButtonActionAudio.Paint += new PaintEventHandler(this.splitButtonActionAudio_Paint);
            this.contextMenuStripDownloadAudio.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDownloadAudio.Name = "contextMenuStripDownloadAudio";
            this.contextMenuStripDownloadAudio.ShowImageMargin = false;
            this.contextMenuStripDownloadAudio.Size = new System.Drawing.Size(36, 4);
            this.picBoxAppNotify.Anchor = AnchorStyles.Left;
            this.picBoxAppNotify.Image = (System.Drawing.Image)Resources.InfoRed;
            this.picBoxAppNotify.Location = new System.Drawing.Point(756, 12);
            this.picBoxAppNotify.Margin = new Padding(3, 1, 3, 3);
            this.picBoxAppNotify.Name = "picBoxAppNotify";
            this.picBoxAppNotify.Size = new System.Drawing.Size(19, 24);
            this.picBoxAppNotify.SizeMode = PictureBoxSizeMode.StretchImage;
            this.picBoxAppNotify.TabIndex = 8;
            this.picBoxAppNotify.TabStop = false;
            this.picBoxAppNotify.Visible = false;
            this.tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.tableLayoutPanel2.ColumnCount = 9;
            this.tableLayoutPanel.SetColumnSpan((System.Windows.Forms.Control)this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 87f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 113f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 173f));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 173f));
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.picBoxHelp, 0, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.lnkLabelHelp, 1, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.picBoxSupport, 2, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.pictureBoxSettings, 4, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.lnkLabelSupport, 3, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.lnkLabelSettings, 5, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.btnVideoFiles, 7, 0);
            this.tableLayoutPanel2.Controls.Add((System.Windows.Forms.Control)this.btnAudioFiles, 8, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(11, 510);
            this.tableLayoutPanel2.Margin = new Padding(11, 4, 8, 4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(880, 39);
            this.tableLayoutPanel2.TabIndex = 13;
            this.picBoxHelp.Image = (System.Drawing.Image)componentResourceManager.GetObject("picBoxHelp.Image");
            this.picBoxHelp.Location = new System.Drawing.Point(0, 4);
            this.picBoxHelp.Margin = new Padding(0, 4, 0, 4);
            this.picBoxHelp.Name = "picBoxHelp";
            this.picBoxHelp.Size = new System.Drawing.Size(40, 31);
            this.picBoxHelp.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBoxHelp.TabIndex = 1;
            this.picBoxHelp.TabStop = false;
            this.lnkLabelHelp.Anchor = AnchorStyles.Left;
            this.lnkLabelHelp.AutoSize = true;
            this.lnkLabelHelp.Location = new System.Drawing.Point(40, 11);
            this.lnkLabelHelp.Margin = new Padding(0, 0, 4, 0);
            this.lnkLabelHelp.Name = "lnkLabelHelp";
            this.lnkLabelHelp.Size = new System.Drawing.Size(37, 17);
            this.lnkLabelHelp.TabIndex = 2;
            this.lnkLabelHelp.TabStop = true;
            this.lnkLabelHelp.Text = "Help";
            this.lnkLabelHelp.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnLabelHelpClicked);
            this.picBoxSupport.Image = (System.Drawing.Image)Resources.user_headset;
            this.picBoxSupport.Location = new System.Drawing.Point((int)sbyte.MaxValue, 4);
            this.picBoxSupport.Margin = new Padding(0, 4, 0, 4);
            this.picBoxSupport.Name = "picBoxSupport";
            this.picBoxSupport.Size = new System.Drawing.Size(40, 31);
            this.picBoxSupport.SizeMode = PictureBoxSizeMode.Zoom;
            this.picBoxSupport.TabIndex = 3;
            this.picBoxSupport.TabStop = false;
            this.pictureBoxSettings.Image = (System.Drawing.Image)Resources.gears_preferences;
            this.pictureBoxSettings.Location = new System.Drawing.Point(267, 4);
            this.pictureBoxSettings.Margin = new Padding(0, 4, 0, 4);
            this.pictureBoxSettings.Name = "pictureBoxSettings";
            this.pictureBoxSettings.Size = new System.Drawing.Size(40, 31);
            this.pictureBoxSettings.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxSettings.TabIndex = 4;
            this.pictureBoxSettings.TabStop = false;
            this.lnkLabelSupport.Anchor = AnchorStyles.Left;
            this.lnkLabelSupport.AutoSize = true;
            this.lnkLabelSupport.Location = new System.Drawing.Point(167, 11);
            this.lnkLabelSupport.Margin = new Padding(0, 0, 4, 0);
            this.lnkLabelSupport.Name = "lnkLabelSupport";
            this.lnkLabelSupport.Size = new System.Drawing.Size(58, 17);
            this.lnkLabelSupport.TabIndex = 5;
            this.lnkLabelSupport.TabStop = true;
            this.lnkLabelSupport.Text = "Support";
            this.lnkLabelSupport.Click += new EventHandler(this.OnAskQuestionMenuItemClick);
            this.lnkLabelSettings.Anchor = AnchorStyles.Left;
            this.lnkLabelSettings.AutoSize = true;
            this.lnkLabelSettings.Location = new System.Drawing.Point(307, 11);
            this.lnkLabelSettings.Margin = new Padding(0, 0, 4, 0);
            this.lnkLabelSettings.Name = "lnkLabelSettings";
            this.lnkLabelSettings.Size = new System.Drawing.Size(85, 17);
            this.lnkLabelSettings.TabIndex = 6;
            this.lnkLabelSettings.TabStop = true;
            this.lnkLabelSettings.Text = "Preferences";
            this.lnkLabelSettings.Click += new EventHandler(this.OnPreferencesMenuItemClick);
            this.btnVideoFiles.BackColor = System.Drawing.SystemColors.Control;
            this.btnVideoFiles.Cursor = Cursors.Hand;
            this.btnVideoFiles.Dock = DockStyle.Fill;
            this.btnVideoFiles.Location = new System.Drawing.Point(538, 4);
            this.btnVideoFiles.Margin = new Padding(4);
            this.btnVideoFiles.Name = "btnVideoFiles";
            this.btnVideoFiles.Size = new System.Drawing.Size(165, 31);
            this.btnVideoFiles.TabIndex = 7;
            this.btnVideoFiles.Text = "My video files";
            this.btnVideoFiles.UseVisualStyleBackColor = false;
            this.btnVideoFiles.Click += new EventHandler(this.OnButtonVideoFilesClick);
            this.btnAudioFiles.Cursor = Cursors.Hand;
            this.btnAudioFiles.Dock = DockStyle.Fill;
            this.btnAudioFiles.Location = new System.Drawing.Point(711, 4);
            this.btnAudioFiles.Margin = new Padding(4);
            this.btnAudioFiles.Name = "btnAudioFiles";
            this.btnAudioFiles.Size = new System.Drawing.Size(165, 31);
            this.btnAudioFiles.TabIndex = 8;
            this.btnAudioFiles.Text = "My audio files";
            this.btnAudioFiles.UseVisualStyleBackColor = true;
            this.btnAudioFiles.Click += new EventHandler(this.OnButtonAudioFilesClick);
            this.panelSearchResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.panelSearchResult.BackColor = System.Drawing.Color.White;
            this.panelSearchResult.BorderStyle = BorderStyle.FixedSingle;
            this.tableLayoutPanel.SetColumnSpan((System.Windows.Forms.Control)this.panelSearchResult, 2);
            this.panelSearchResult.Controls.Add((System.Windows.Forms.Control)this.pictureBoxCopyPath);
            this.panelSearchResult.Controls.Add((System.Windows.Forms.Control)this.pictureBoxOpenFile);
            this.panelSearchResult.Controls.Add((System.Windows.Forms.Control)this.lblSearchResultTime);
            this.panelSearchResult.Controls.Add((System.Windows.Forms.Control)this.lblSearchResultDescription);
            this.panelSearchResult.Controls.Add((System.Windows.Forms.Control)this.lblSearchResultTitle);
            this.panelSearchResult.Controls.Add((System.Windows.Forms.Control)this.pictureBoxSearchResult);
            this.panelSearchResult.Location = new System.Drawing.Point(13, 49);
            this.panelSearchResult.Margin = new Padding(13, 4, 13, 4);
            this.panelSearchResult.Name = "panelSearchResult";
            this.panelSearchResult.Size = new System.Drawing.Size(873, 73);
            this.panelSearchResult.TabIndex = 14;
            this.pictureBoxCopyPath.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.pictureBoxCopyPath.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxCopyPath.Cursor = Cursors.Hand;
            this.pictureBoxCopyPath.Image = (System.Drawing.Image)Resources.link_icon;
            this.pictureBoxCopyPath.Location = new System.Drawing.Point(839, 38);
            this.pictureBoxCopyPath.Margin = new Padding(4);
            this.pictureBoxCopyPath.Name = "pictureBoxCopyPath";
            this.pictureBoxCopyPath.Size = new System.Drawing.Size(27, 25);
            this.pictureBoxCopyPath.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxCopyPath.TabIndex = 5;
            this.pictureBoxCopyPath.TabStop = false;
            this.pictureBoxCopyPath.Click += new EventHandler(this.OnPictureBoxCopyPathClick);
            this.pictureBoxOpenFile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.pictureBoxOpenFile.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxOpenFile.Cursor = Cursors.Hand;
            this.pictureBoxOpenFile.Location = new System.Drawing.Point(839, 8);
            this.pictureBoxOpenFile.Margin = new Padding(4);
            this.pictureBoxOpenFile.Name = "pictureBoxOpenFile";
            this.pictureBoxOpenFile.Size = new System.Drawing.Size(27, 25);
            this.pictureBoxOpenFile.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxOpenFile.TabIndex = 4;
            this.pictureBoxOpenFile.TabStop = false;
            this.pictureBoxOpenFile.Click += new EventHandler(this.OnPictureBoxOpenFileClick);
            this.lblSearchResultTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.lblSearchResultTime.Enabled = false;
            this.lblSearchResultTime.Font = new Font("Microsoft Sans Serif", 9.7f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lblSearchResultTime.Location = new System.Drawing.Point(731, 10);
            this.lblSearchResultTime.Margin = new Padding(4, 0, 4, 0);
            this.lblSearchResultTime.Name = "lblSearchResultTime";
            this.lblSearchResultTime.Size = new System.Drawing.Size(107, 21);
            this.lblSearchResultTime.TabIndex = 3;
            this.lblSearchResultTime.Text = "00:00:00";
            this.lblSearchResultTime.TextAlign = ContentAlignment.TopRight;
            this.lblSearchResultDescription.BackColor = System.Drawing.Color.White;
            this.lblSearchResultDescription.Font = new Font("Microsoft Sans Serif", 9.7f, System.Drawing.FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.lblSearchResultDescription.Location = new System.Drawing.Point(132, 39);
            this.lblSearchResultDescription.Margin = new Padding(4, 0, 4, 0);
            this.lblSearchResultDescription.Name = "lblSearchResultDescription";
            this.lblSearchResultDescription.Size = new System.Drawing.Size(641, 21);
            this.lblSearchResultDescription.TabIndex = 2;
            this.lblSearchResultDescription.Text = "Description";
            this.lblSearchResultTitle.BackColor = System.Drawing.Color.White;
            this.lblSearchResultTitle.Font = new Font("Microsoft Sans Serif", 9.7f, System.Drawing.FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblSearchResultTitle.Location = new System.Drawing.Point(132, 10);
            this.lblSearchResultTitle.Margin = new Padding(4, 0, 4, 0);
            this.lblSearchResultTitle.Name = "lblSearchResultTitle";
            this.lblSearchResultTitle.Size = new System.Drawing.Size(532, 21);
            this.lblSearchResultTitle.TabIndex = 1;
            this.lblSearchResultTitle.Text = "Title";
            this.pictureBoxSearchResult.BackColor = System.Drawing.Color.White;
            this.pictureBoxSearchResult.Cursor = Cursors.Hand;
            this.pictureBoxSearchResult.Location = new System.Drawing.Point(2, 2);
            this.pictureBoxSearchResult.Margin = new Padding(0);
            this.pictureBoxSearchResult.Name = "pictureBoxSearchResult";
            this.pictureBoxSearchResult.Size = new System.Drawing.Size(118, 67);
            this.pictureBoxSearchResult.SizeMode = PictureBoxSizeMode.CenterImage;
            this.pictureBoxSearchResult.TabIndex = 0;
            this.pictureBoxSearchResult.TabStop = false;
            this.pictureBoxSearchResult.Click += new EventHandler(this.OnPictureBoxSearchResultClick);
            this.pictureBoxSearchResult.MouseEnter += new EventHandler(this.OnPictureBoxSearchResultMouseEnter);
            this.pictureBoxSearchResult.MouseLeave += new EventHandler(this.OnPictureBoxSearchResultMouseLeave);
            this.tableLayoutPanel.SetColumnSpan((System.Windows.Forms.Control)this.panelAdBottom, 2);
            this.panelAdBottom.Dock = DockStyle.Fill;
            this.panelAdBottom.Location = new System.Drawing.Point(12, 509);
            this.panelAdBottom.Margin = new Padding(12, 3, 12, 3);
            this.panelAdBottom.Name = "panelAdBottom";
            this.panelAdBottom.Size = new System.Drawing.Size(875, 1);
            this.panelAdBottom.TabIndex = 15;
            this.panelAdRight.Dock = DockStyle.Fill;
            this.panelAdRight.Location = new System.Drawing.Point(902, 3);
            this.panelAdRight.Name = "panelAdRight";
            this.tableLayoutPanel.SetRowSpan((System.Windows.Forms.Control)this.panelAdRight, 6);
            this.panelAdRight.Size = new System.Drawing.Size(1, 547);
            this.panelAdRight.TabIndex = 16;
            this.PanelContainerAutoComplete.BackColor = System.Drawing.Color.White;
            this.PanelContainerAutoComplete.Location = new System.Drawing.Point(0, 0);
            this.PanelContainerAutoComplete.Margin = new Padding(4);
            this.PanelContainerAutoComplete.Name = "PanelContainerAutoComplete";
            this.PanelContainerAutoComplete.Size = new System.Drawing.Size(199, 15);
            this.PanelContainerAutoComplete.TabIndex = 15;
            this.PanelContainerAutoComplete.Scroll += new ScrollEventHandler(this.OnPanelContainerAutoCompleteScroll);
            this.PanelContainerAutoComplete.PreviewKeyDown += new PreviewKeyDownEventHandler(this.OnPanelContainerAutoCompletePreviewKeyDown);
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new ToolStripItem[6]
            {
        (ToolStripItem) this.windowToolStripMenuItem,
        (ToolStripItem) this.settingsToolStripMenuItem,
        (ToolStripItem) this.versionToolStripMenuItem,
        (ToolStripItem) this.contributeToolStripMenuItem,
        (ToolStripItem) this.versionNumberToolStripMenuItem,
        (ToolStripItem) this.helpToolStripMenuItem
            });
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new Padding(8, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(899, 28);
            this.menuStrip.TabIndex = 9;
            this.menuStrip.Text = "menuStrip1";
            this.windowToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) this.alwaysOnTopMenuItem,
        (ToolStripItem) this.toolStripSeparator1,
        (ToolStripItem) this.minimizeToolStripMenuItem,
        (ToolStripItem) this.minimizeToNotificationAreaToolStripMenuItem,
        (ToolStripItem) this.exitToolStripMenuItem
            });
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(76, 24);
            this.windowToolStripMenuItem.Text = "Window";
            this.alwaysOnTopMenuItem.Name = "alwaysOnTopMenuItem";
            this.alwaysOnTopMenuItem.Size = new System.Drawing.Size(276, 26);
            this.alwaysOnTopMenuItem.Text = "Always on top";
            this.alwaysOnTopMenuItem.Click += new EventHandler(this.OnAlwaysOnTopMenuItemClick);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(273, 6);
            this.minimizeToolStripMenuItem.Image = (System.Drawing.Image)Resources.minimize_bar;
            this.minimizeToolStripMenuItem.Name = "minimizeToolStripMenuItem";
            this.minimizeToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.minimizeToolStripMenuItem.Text = "Minimize";
            this.minimizeToolStripMenuItem.Click += new EventHandler(this.OnMinimizeMenuItemClick);
            this.minimizeToNotificationAreaToolStripMenuItem.Image = (System.Drawing.Image)Resources.minimize_tray;
            this.minimizeToNotificationAreaToolStripMenuItem.Name = "minimizeToNotificationAreaToolStripMenuItem";
            this.minimizeToNotificationAreaToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.minimizeToNotificationAreaToolStripMenuItem.Text = "Minimize to notification area";
            this.minimizeToNotificationAreaToolStripMenuItem.Click += new EventHandler(this.OnMinimizeToTrayMenuItemClick);
            this.exitToolStripMenuItem.Image = (System.Drawing.Image)Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = Keys.F4 | Keys.Alt;
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(276, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new EventHandler(this.OnExitMenuItemClick);
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) this.preferencesToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator2,
        (ToolStripItem) this.askFileNameAndFolderToolStripMenuItem,
        (ToolStripItem) this.compatibleURLNotificationToolStripMenuItem,
        (ToolStripItem) this.autoDownloadMenuItem
            });
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.preferencesToolStripMenuItem.Image = (System.Drawing.Image)Resources.gears_preferences;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new EventHandler(this.OnPreferencesMenuItemClick);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(269, 6);
            this.askFileNameAndFolderToolStripMenuItem.Name = "askFileNameAndFolderToolStripMenuItem";
            this.askFileNameAndFolderToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
            this.askFileNameAndFolderToolStripMenuItem.Text = "Ask file name and folder";
            this.askFileNameAndFolderToolStripMenuItem.Click += new EventHandler(this.OnAskFileNameAndFolderpMenuItemClick);
            this.compatibleURLNotificationToolStripMenuItem.Checked = true;
            this.compatibleURLNotificationToolStripMenuItem.CheckState = CheckState.Checked;
            this.compatibleURLNotificationToolStripMenuItem.Name = "compatibleURLNotificationToolStripMenuItem";
            this.compatibleURLNotificationToolStripMenuItem.Size = new System.Drawing.Size(272, 26);
            this.compatibleURLNotificationToolStripMenuItem.Text = "Compatible URL notification";
            this.compatibleURLNotificationToolStripMenuItem.Click += new EventHandler(this.OnCompatibleURLNotificationCheckedChanged);
            this.autoDownloadMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
            {
        (ToolStripItem) this.videoAutoDownloadMenuItem,
        (ToolStripItem) this.audioAutoDownloadMenuItem
            });
            this.autoDownloadMenuItem.Name = "autoDownloadMenuItem";
            this.autoDownloadMenuItem.Size = new System.Drawing.Size(272, 26);
            this.autoDownloadMenuItem.Text = "Auto-download videos";
            this.videoAutoDownloadMenuItem.CheckOnClick = true;
            this.videoAutoDownloadMenuItem.Name = "videoAutoDownloadMenuItem";
            this.videoAutoDownloadMenuItem.Size = new System.Drawing.Size(124, 26);
            this.videoAutoDownloadMenuItem.Text = "Video";
            this.videoAutoDownloadMenuItem.Click += new EventHandler(this.OnAutoDownloadVideoMenuItemClick);
            this.audioAutoDownloadMenuItem.CheckOnClick = true;
            this.audioAutoDownloadMenuItem.Name = "audioAutoDownloadMenuItem";
            this.audioAutoDownloadMenuItem.Size = new System.Drawing.Size(124, 26);
            this.audioAutoDownloadMenuItem.Text = "Audio";
            this.audioAutoDownloadMenuItem.Click += new EventHandler(this.OnAutoDownloadAudioMenuItemClick);
            this.versionToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[6]
            {
        (ToolStripItem) this.checkForUpdateToolStripMenuItem,
        (ToolStripItem) this.downloadLatestVersionToolStripMenuItem,
        (ToolStripItem) this.signupForUpdatesToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator4,
        (ToolStripItem) this.upgradeToProVersionToolStripMenuItem,
        (ToolStripItem) this.deactivateFYDProToolStripMenuItem
            });
            this.versionToolStripMenuItem.Name = "versionToolStripMenuItem";
            this.versionToolStripMenuItem.Size = new System.Drawing.Size(69, 24);
            this.versionToolStripMenuItem.Text = "Version";
            this.checkForUpdateToolStripMenuItem.Image = (System.Drawing.Image)Resources.updates_find;
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            this.checkForUpdateToolStripMenuItem.Size = new System.Drawing.Size(296, 26);
            this.checkForUpdateToolStripMenuItem.Text = "Check for update";
            this.checkForUpdateToolStripMenuItem.Click += new EventHandler(this.OnCheckForUpdateMenuItemClick);
            this.downloadLatestVersionToolStripMenuItem.Image = (System.Drawing.Image)Resources.download;
            this.downloadLatestVersionToolStripMenuItem.Name = "downloadLatestVersionToolStripMenuItem";
            this.downloadLatestVersionToolStripMenuItem.Size = new System.Drawing.Size(296, 26);
            this.downloadLatestVersionToolStripMenuItem.Text = "Download latest version";
            this.downloadLatestVersionToolStripMenuItem.Click += new EventHandler(this.OnDownloadLatestVersionMenuItemClick);
            this.signupForUpdatesToolStripMenuItem.Image = (System.Drawing.Image)Resources.subscribe;
            this.signupForUpdatesToolStripMenuItem.Name = "signupForUpdatesToolStripMenuItem";
            this.signupForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(296, 26);
            this.signupForUpdatesToolStripMenuItem.Text = "Sign-up for updates notification";
            this.signupForUpdatesToolStripMenuItem.Click += new EventHandler(this.OnSignupForUpdatesClick);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(293, 6);
            this.upgradeToProVersionToolStripMenuItem.Name = "upgradeToProVersionToolStripMenuItem";
            this.upgradeToProVersionToolStripMenuItem.Size = new System.Drawing.Size(296, 26);
            this.upgradeToProVersionToolStripMenuItem.Text = "Upgrade to Pro version";
            this.upgradeToProVersionToolStripMenuItem.Click += new EventHandler(this.OnUpgradeToProVersionClick);
            this.deactivateFYDProToolStripMenuItem.Name = "deactivateFYDProToolStripMenuItem";
            this.deactivateFYDProToolStripMenuItem.Size = new System.Drawing.Size(296, 26);
            this.deactivateFYDProToolStripMenuItem.Text = "Deactivate FYD Pro";
            this.deactivateFYDProToolStripMenuItem.Click += new EventHandler(this.OnDeactivateFydProClick);
            this.contributeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[3]
            {
        (ToolStripItem) this.reportAProblemToolStripMenuItem,
        (ToolStripItem) this.suggestToolStripMenuItem,
        (ToolStripItem) this.becomeAFanToolStripMenuItem
            });
            this.contributeToolStripMenuItem.Name = "contributeToolStripMenuItem";
            this.contributeToolStripMenuItem.Size = new System.Drawing.Size(91, 24);
            this.contributeToolStripMenuItem.Text = "Contribute";
            this.reportAProblemToolStripMenuItem.Image = (System.Drawing.Image)Resources.flag_red;
            this.reportAProblemToolStripMenuItem.Name = "reportAProblemToolStripMenuItem";
            this.reportAProblemToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            this.reportAProblemToolStripMenuItem.Text = "Report a problem";
            this.reportAProblemToolStripMenuItem.Click += new EventHandler(this.OnReportProblemMenuItemClick);
            this.suggestToolStripMenuItem.Image = (System.Drawing.Image)Resources.suggest_feature;
            this.suggestToolStripMenuItem.Name = "suggestToolStripMenuItem";
            this.suggestToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            this.suggestToolStripMenuItem.Text = "Suggest feature";
            this.suggestToolStripMenuItem.Click += new EventHandler(this.OnSuggestMenuItemClick);
            this.becomeAFanToolStripMenuItem.Image = (System.Drawing.Image)Resources.facebook;
            this.becomeAFanToolStripMenuItem.Name = "becomeAFanToolStripMenuItem";
            this.becomeAFanToolStripMenuItem.Size = new System.Drawing.Size(202, 26);
            this.becomeAFanToolStripMenuItem.Text = "Become a fan";
            this.becomeAFanToolStripMenuItem.Click += new EventHandler(this.OnBecomeAFanMenuItemClick);
            this.versionNumberToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            this.versionNumberToolStripMenuItem.Name = "versionNumberToolStripMenuItem";
            this.versionNumberToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.versionNumberToolStripMenuItem.Text = "4.0.0.0";
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
            {
        (ToolStripItem) this.visitOurWebsiteToolStripMenuItem,
        (ToolStripItem) this.askAQuestionToolStripMenuItem,
        (ToolStripItem) this.readDocumentationToolStripMenuItem,
        (ToolStripItem) this.toolStripSeparator3,
        (ToolStripItem) this.aboutToolStripMenuItem
            });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            this.visitOurWebsiteToolStripMenuItem.Image = (System.Drawing.Image)Resources.browser_view;
            this.visitOurWebsiteToolStripMenuItem.Name = "visitOurWebsiteToolStripMenuItem";
            this.visitOurWebsiteToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.visitOurWebsiteToolStripMenuItem.Text = "Visit our website";
            this.visitOurWebsiteToolStripMenuItem.Click += new EventHandler(this.OnVisitOurWebsiteMenuItemClick);
            this.askAQuestionToolStripMenuItem.Image = (System.Drawing.Image)Resources.support_public;
            this.askAQuestionToolStripMenuItem.Name = "askAQuestionToolStripMenuItem";
            this.askAQuestionToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.askAQuestionToolStripMenuItem.Text = "Ask a question";
            this.askAQuestionToolStripMenuItem.Click += new EventHandler(this.OnAskQuestionMenuItemClick);
            this.readDocumentationToolStripMenuItem.Image = (System.Drawing.Image)Resources.help;
            this.readDocumentationToolStripMenuItem.Name = "readDocumentationToolStripMenuItem";
            this.readDocumentationToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.readDocumentationToolStripMenuItem.Text = "Read documention";
            this.readDocumentationToolStripMenuItem.Click += new EventHandler(this.OnReadDocumentationMenuItemClick);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(207, 6);
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(210, 26);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.OnAboutClick);
            this.PanelWrapperContainerAutoComplete.BorderStyle = BorderStyle.FixedSingle;
            this.PanelWrapperContainerAutoComplete.Controls.Add((System.Windows.Forms.Control)this.PanelContainerAutoComplete);
            this.PanelWrapperContainerAutoComplete.Controls.Add((System.Windows.Forms.Control)this.progressBarLoadingData);
            this.PanelWrapperContainerAutoComplete.Location = new System.Drawing.Point(13, 65);
            this.PanelWrapperContainerAutoComplete.Margin = new Padding(4);
            this.PanelWrapperContainerAutoComplete.Name = "PanelWrapperContainerAutoComplete";
            this.PanelWrapperContainerAutoComplete.Size = new System.Drawing.Size(198, 14);
            this.PanelWrapperContainerAutoComplete.TabIndex = 0;
            this.progressBarLoadingData.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.progressBarLoadingData.Location = new System.Drawing.Point(0, 15);
            this.progressBarLoadingData.Margin = new Padding(4);
            this.progressBarLoadingData.MarqueeAnimationSpeed = 10;
            this.progressBarLoadingData.Name = "progressBarLoadingData";
            this.progressBarLoadingData.Size = new System.Drawing.Size(199, 4);
            this.progressBarLoadingData.Style = ProgressBarStyle.Marquee;
            this.progressBarLoadingData.TabIndex = 0;
            this.progressBarLoadingData.Visible = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(254, 254, 254);
            this.ClientSize = new System.Drawing.Size(899, 603);
            this.Controls.Add((System.Windows.Forms.Control)this.PanelWrapperContainerAutoComplete);
            this.Controls.Add((System.Windows.Forms.Control)this.tableLayoutPanel);
            this.Controls.Add((System.Windows.Forms.Control)this.menuStrip);
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new Padding(4);
            this.MinimumSize = new System.Drawing.Size(800, 475);
            this.Name = "MainWindow";
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Free YouTube Downloader";
            this.FormClosing += new FormClosingEventHandler(this.OnFormClosing);
            this.Load += new EventHandler(this.OnMainWindowLoad);
            this.Resize += new EventHandler(this.OnMainWindowResize);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((ISupportInitialize)this.olvDownloads).EndInit();
            this.tableLayoutPanelActionButtons.ResumeLayout(false);
            this.tableLayoutPanelActionButtonsContainer.ResumeLayout(false);
            ((ISupportInitialize)this.picBoxFacebook).EndInit();
            ((ISupportInitialize)this.picBoxTwitter).EndInit();
            this.panelButtonActionVideo.ResumeLayout(false);
            this.panelButtonActionVideo.PerformLayout();
            this.panelButtonActionAudio.ResumeLayout(false);
            this.panelButtonActionAudio.PerformLayout();
            ((ISupportInitialize)this.picBoxAppNotify).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((ISupportInitialize)this.picBoxHelp).EndInit();
            ((ISupportInitialize)this.picBoxSupport).EndInit();
            ((ISupportInitialize)this.pictureBoxSettings).EndInit();
            this.panelSearchResult.ResumeLayout(false);
            ((ISupportInitialize)this.pictureBoxCopyPath).EndInit();
            ((ISupportInitialize)this.pictureBoxOpenFile).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.PanelWrapperContainerAutoComplete.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private delegate object[] VideoQualityInfoManager(object[] parameters);
    }
}
