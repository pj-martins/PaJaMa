// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.MainWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using BrightIdeasSoftware;
using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Common.Types;
using FreeYouTubeDownloader.Controls;
using FreeYouTubeDownloader.Converter;
using FreeYouTubeDownloader.Downloader;
using FreeYouTubeDownloader.Downloader.Providers;
using FreeYouTubeDownloader.Downloader.Tasks;
using FreeYouTubeDownloader.Extensions;
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

namespace FreeYouTubeDownloader
{
    public partial class MainWindow : Form, ILocalizableForm
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
        private System.Windows.Forms.ToolTip _toolTip;
        private uint _lastClipboardSequenceNumber;
        private System.Drawing.Image _thumbnailOverlay;
        private bool _ignoreAutoDownloadFlag;
        private TaskbarIcon _notifyIcon;
        
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
                    YoutubeHelper.YoutubeSearch.Query = value;
                }
                else
                {
                    if (YoutubeHelper.GetYoutubeVideoIdByUrl(value) == null)
                        return;
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
            this.olvDownloads.ColumnWidthChanged += new ColumnWidthChangedEventHandler(this.OnColumnWidthChanged);
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
            if (this.WindowState != FormWindowState.Normal)
                return;
            DateTime startupTime = Process.GetCurrentProcess().StartTime;
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

        private void OnMinimizeMenuItemClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void OnTextBoxUrlTextChanged(object sender, EventArgs e)
        {
            this.RowSearchResultHide();
            this.SearchDropDownPanelHide(true);
            this.SetEnableDownloadButtons(false, true);
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
            this._clearInputButton.Image = (System.Drawing.Image)Properties.Resources.Clear;
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
            this.Connected = args.HasInternetConnection;
        }

        private void OnExtractMp3MenuItemClick(object sender, EventArgs e)
        {
            AudioQuality audioQuality = (AudioQuality)((ToolStripItem)sender).Tag;
            this.splitButtonActionAudio.Text = string.Format("{0} MP3 ({1} kbps)", (object)Strings.Download, (object)((int)audioQuality).ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ExtractMP3, (object)audioQuality);
        }

        private void OnExtractAacMenuItemClick(object sender, EventArgs e)
        {
            AudioQuality audioQuality = (AudioQuality)((ToolStripItem)sender).Tag;
            this.splitButtonActionAudio.Text = string.Format("{0} AAC ({1} kbps)", (object)Strings.Download, (object)((int)audioQuality).ToString((IFormatProvider)CultureInfo.InvariantCulture));
            this.splitButtonActionAudio.Tag = (object)new ButtonActionData(DesiredAction.ExtractAAC, (object)audioQuality);
        }

        private void OnExtractVorbisMenuItemClick(object sender, EventArgs e)
        {
            AudioQuality audioQuality = (AudioQuality)((ToolStripItem)sender).Tag;
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

        private void OnAutoDownloadVideoMenuItemClick(object sender, EventArgs e)
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

        private void OnAutoDownloadAudioMenuItemClick(object sender, EventArgs e)
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
                    using (SoundPlayer soundPlayer = new SoundPlayer(new MemoryStream(Resources.windows_critical_stop)))
                        soundPlayer.Play();
                }
                else
                {
                    using (SoundPlayer soundPlayer = new SoundPlayer(new MemoryStream(Resources.windows_notify)))
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
                return item;
            }
            if (item.Enabled && item.HasDropDownItems)
            {
                for (int index = 0; index < item.DropDownItems.Count; ++index)
                {
                    ToolStripMenuItem dropDownItem = (ToolStripMenuItem)item.DropDownItems[index];
                    if (dropDownItem.Enabled)
                    {
                        if (dropDownItem.Tag.GetType() != typeof(VideoQuality))
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
            this.autoDownloadMenuItem.Checked = this.videoAutoDownloadMenuItem.Checked = Settings.Instance.AutomaticallyDownloadVideo;
            this.autoDownloadMenuItem.Checked = this.audioAutoDownloadMenuItem.Checked = Settings.Instance.AutomaticallyDownloadAudio;
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
                       return (object)Properties.Resources.Waiting;
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
            this.GetMediaInfo(correctUrl, false, false);
        }

        private void InitPanelSearchResultFromYoutube(System.Windows.Forms.Panel panel)
        {
            System.Windows.Forms.Control[] controlArray1 = panel.Controls.Find("Image", false);
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
                    break;
                case 2:
                    if (FreeYouTubeDownloader.Common.NativeMethods.User32.RemoveClipboardFormatListener(this.Handle))
                        break;
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
            this.settingsToolStripMenuItem.Text = Strings.Settings;
            this.preferencesToolStripMenuItem.Text = Strings.Preferences;
            this.windowToolStripMenuItem.Text = Strings.Window;
            this.olvColumnName.Text = Strings.Name;
            this.olvColumnFormat.Text = Strings.Format;
            this.olvColumnSize.Text = Strings.Size;
            this.olvColumnDownloadSpeed.Text = Strings.Speed;
            this.olvColumnEta.Text = Strings.Eta;
            this.olvColumnFrameSize.Text = Strings.Resolution;
            this.olvColumnDuration.Text = Strings.Duration;
            this.olvColumnProgress.Text = Strings.Progress;
            this.askFileNameAndFolderToolStripMenuItem.Text = Strings.AskFileNameAndFolder;
            this.alwaysOnTopMenuItem.Text = Strings.AlwaysOnTop;
            this.toolTip.SetToolTip((System.Windows.Forms.Control)this.buttonConvert, Strings.SelectFileForConversionButtonHint);
            this.lnkLabelSettings.Text = Strings.Preferences;
            this.btnVideoFiles.Text = Strings.OpenVideoFiles;
            this.btnAudioFiles.Text = Strings.OpenAudioFiles;
            this.toolTip.SetToolTip((System.Windows.Forms.Control)this.pictureBoxCopyPath, Strings.CopyPath);
            this.compatibleURLNotificationToolStripMenuItem.Text = Strings.CompatibleURLNotification;
            ((HeaderedItemsControl)this._notifyIcon.ContextMenu.Items[0]).Header = (object)Strings.OpenApplication;
            ((HeaderedItemsControl)this._notifyIcon.ContextMenu.Items[1]).Header = (object)Strings.Exit;
            this.autoDownloadMenuItem.Text = Strings.AutoDownloadVideos;
            this.videoAutoDownloadMenuItem.Text = Strings.Video;
            this.audioAutoDownloadMenuItem.Text = Strings.Audio;
            this.SetDefaultTextDownloadButtons(false);
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

        private delegate object[] VideoQualityInfoManager(object[] parameters);
    }
}
