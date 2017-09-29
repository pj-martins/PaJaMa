// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.SettingsWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
    public class SettingsWindow : Form, ILocalizableForm
    {
        private readonly Color _panelContentTabDefaultBackgroundColor = Color.FromArgb(238, 238, 238);
        private readonly Color _panelContentTabFocusedtBackgroundColor = Color.White;
        private Color _panelContentTabSelectedBackgroundColor;
        private Label _prevSelectedLabel;
        private Panel _currentTab;
        private IContainer components;
        private ComboBox comboBoxLanguages;
        private NumericUpDown numericMaxDownloads;
        private Label labelMaxDownloads;
        private CheckBox checkBoxAllowSimDownloads;
        private Button buttonChooseDefaultVideosDownloadFolder;
        private TextBox textBoxDefaultVideosDownloadFolder;
        private TextBox textBoxDefaultAudioDownloadsFolder;
        private Button buttonChooseDefaultAudioDownloadsFolder;
        private CheckBox checkBoxIncludeSoundsForNotifications;
        private CheckBox checkBoxIncludeTooltipsForNotifications;
        private CheckBox checkBoxRemoveFinishedFiles;
        private Button buttonOk;
        private Button buttonCancel;
        private ToolTip toolTip;
        private CheckBox checkBoxRunSearchOnEnterKey;
        private GroupBox groupBoxDefaultVideoDownloadsFolder;
        private GroupBox groupBoxDefaultAudioDownloadsFolder;
        private GroupBox groupBoxBehavioural;
        private GroupBox groupBoxSimultaneousDownloads;
        private GroupBox groupBoxPreferredLanguage;
        private Panel panelMainContainer;
        private Panel panelMenuContainer;
        private Panel panelContainerTab2;
        private Panel panelTabDownloads;
        private PictureBox picBoxTabDownloads;
        private Label labelTabDownloads;
        private Panel panelContainerTab1;
        private Panel panelTabGeneral;
        private PictureBox picBoxTabGeneral;
        private Label labelTabGeneral;
        private Panel panelContentContainer;
        private Panel panelContentTab1Container;
        private Panel panelContentTitle;
        private PictureBox picBoxMainContent;
        private Label labelMainContent;
        private Panel panelContentTab2Container;
        private GroupBox groupBoxCrossButtonAction;
        private CheckBox checkBoxRememberLastQualityUsed;
        private ComboBox comboBoxCrossButtonAction;

        public SettingsWindow()
        {
            this.InitializeComponent();
            this.BuildLanguagesComboBox();
            FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler)((sender, args) => this.ApplyCurrentLocalization());
            this.ApplyCurrentLocalization();
            this._panelContentTabSelectedBackgroundColor = WindowHelper.GetWindowColorizationColor(true);
            this.numericMaxDownloads.Value = (Decimal)Settings.Instance.MaxSimultaneousDownloads;
            this.checkBoxAllowSimDownloads.Checked = Settings.Instance.AllowSimultaneousDownloads;
            this.labelMaxDownloads.Enabled = this.checkBoxAllowSimDownloads.Checked;
            this.numericMaxDownloads.Enabled = this.checkBoxAllowSimDownloads.Checked;
            this.textBoxDefaultVideosDownloadFolder.Text = Settings.Instance.DefaultVideosDownloadFolder;
            this.textBoxDefaultAudioDownloadsFolder.Text = Settings.Instance.DefaultAudiosDownloadFolder;
            this.checkBoxIncludeSoundsForNotifications.Checked = Settings.Instance.AllowSounds;
            this.checkBoxIncludeTooltipsForNotifications.Checked = Settings.Instance.AllowBalloons;
            this.checkBoxRemoveFinishedFiles.Checked = Settings.Instance.RemoveAllFinishedFiles;
            this.checkBoxRememberLastQualityUsed.Checked = Settings.Instance.RememberLastQualityUsed;
            this.checkBoxRunSearchOnEnterKey.Checked = Settings.Instance.RunSearchOnEnterKey;
            this.comboBoxCrossButtonAction.SelectedIndex = (int)Settings.Instance.CloseAppAction;
            this._prevSelectedLabel = this.labelTabGeneral;
            this.InitializeSelectedTag(this.panelTabGeneral, this.picBoxTabGeneral, this.labelTabGeneral, this.panelContentTab1Container);
        }

        public SettingsWindow(int tabIndex)
          : this()
        {
            if (tabIndex >= this.panelMenuContainer.Controls.Count)
                return;
            if (tabIndex != 0)
            {
                if (tabIndex != 1)
                    return;
                this.InitializeSelectedTag(this.panelTabDownloads, this.picBoxTabDownloads, this.labelTabDownloads, this.panelContentTab2Container);
            }
            else
                this.InitializeSelectedTag(this.panelTabGeneral, this.picBoxTabGeneral, this.labelTabGeneral, this.panelContentTab1Container);
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            bool flag = false;
            if ((Decimal)Settings.Instance.MaxSimultaneousDownloads != this.numericMaxDownloads.Value)
            {
                Settings.Instance.MaxSimultaneousDownloads = Convert.ToInt32(this.numericMaxDownloads.Value);
                flag = true;
            }
            if (Settings.Instance.LanguageCode != ((LanguageItem)this.comboBoxLanguages.SelectedItem).CountryCode)
            {
                Settings.Instance.LanguageCode = ((LanguageItem)this.comboBoxLanguages.SelectedItem).CountryCode;
                FreeYouTubeDownloader.Localization.Localization.Instance.SetLanguage(Settings.Instance.LanguageCode);
                flag = true;
            }
            if (Settings.Instance.AllowSimultaneousDownloads != this.checkBoxAllowSimDownloads.Checked)
            {
                Settings.Instance.AllowSimultaneousDownloads = this.checkBoxAllowSimDownloads.Checked;
                flag = true;
            }
            if (Settings.Instance.DefaultVideosDownloadFolder != this.textBoxDefaultVideosDownloadFolder.Text)
            {
                Settings.Instance.DefaultVideosDownloadFolder = this.textBoxDefaultVideosDownloadFolder.Text;
                flag = true;
            }
            if (Settings.Instance.DefaultAudiosDownloadFolder != this.textBoxDefaultAudioDownloadsFolder.Text)
            {
                Settings.Instance.DefaultAudiosDownloadFolder = this.textBoxDefaultAudioDownloadsFolder.Text;
                flag = true;
            }
            if (flag)
                Settings.Instance.RaiseSettingsChanged();
            Settings.Instance.AllowSounds = this.checkBoxIncludeSoundsForNotifications.Checked;
            Settings.Instance.AllowBalloons = this.checkBoxIncludeTooltipsForNotifications.Checked;
            Settings.Instance.RemoveAllFinishedFiles = this.checkBoxRemoveFinishedFiles.Checked;
            Settings.Instance.RememberLastQualityUsed = this.checkBoxRememberLastQualityUsed.Checked;
            Settings.Instance.RunSearchOnEnterKey = this.checkBoxRunSearchOnEnterKey.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void checkBoxAllowSimDownloads_CheckedChanged(object sender, EventArgs e)
        {
            this.labelMaxDownloads.Enabled = this.checkBoxAllowSimDownloads.Checked;
            this.numericMaxDownloads.Enabled = this.checkBoxAllowSimDownloads.Checked;
        }

        private void buttonChooseDefaultDownloadFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = Settings.Instance.DefaultVideosDownloadFolder,
                Description = Strings.ChooseDefaultDownloadFolder
            };
            if (folderBrowserDialog.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            Settings.Instance.DefaultVideosDownloadFolder = folderBrowserDialog.SelectedPath;
            this.textBoxDefaultVideosDownloadFolder.Text = Settings.Instance.DefaultVideosDownloadFolder;
        }

        private void buttonChooseDefaultConversionFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                SelectedPath = Settings.Instance.DefaultAudiosDownloadFolder,
                Description = Strings.ChooseDefaultDownloadFolder
            };
            if (folderBrowserDialog.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            Settings.Instance.DefaultAudiosDownloadFolder = folderBrowserDialog.SelectedPath;
            this.textBoxDefaultAudioDownloadsFolder.Text = Settings.Instance.DefaultAudiosDownloadFolder;
        }

        private void comboBoxLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            LanguageItem selectedItem = (LanguageItem)((ComboBox)sender).SelectedItem;
            if (!(Settings.Instance.LanguageCode != selectedItem.CountryCode))
                return;
            Settings.Instance.LanguageCode = selectedItem.CountryCode;
            FreeYouTubeDownloader.Localization.Localization.Instance.SetLanguage(selectedItem.CountryCode);
            this.ApplyCurrentLocalization();
        }

        public void ApplyCurrentLocalization()
        {
            this.Text = Strings.Preferences;
            this.buttonCancel.Text = Strings.Cancel;
            this.labelMaxDownloads.Text = Strings.MaxSimultaneousDownloads;
            this.groupBoxDefaultVideoDownloadsFolder.Text = Strings.DefaultFolderForVideoDownloads;
            this.groupBoxDefaultAudioDownloadsFolder.Text = Strings.DefaultFolderForAudioDownloads;
            this.groupBoxBehavioural.Text = Strings.Behavioural;
            this.groupBoxSimultaneousDownloads.Text = Strings.SimultaneousDownloads;
            this.groupBoxPreferredLanguage.Text = Strings.PreferredLanguage;
            this.checkBoxIncludeSoundsForNotifications.Text = Strings.IncludeSoundsForNotifications;
            this.checkBoxIncludeTooltipsForNotifications.Text = Strings.IncludeTooltipsForNotifications;
            this.checkBoxRemoveFinishedFiles.Text = Strings.RemoveFinishedFiles;
            this.checkBoxRememberLastQualityUsed.Text = Strings.RememberLastQualityUsed;
            this.checkBoxRunSearchOnEnterKey.Text = Strings.RunSearchOnEnterKey;
            this.toolTip.SetToolTip((Control)this.checkBoxRemoveFinishedFiles, Strings.RemoveFinishedFiles);
            this.toolTip.SetToolTip((Control)this.checkBoxRememberLastQualityUsed, Strings.RememberLastQualityUsed);
            this.toolTip.SetToolTip((Control)this.checkBoxRunSearchOnEnterKey, Strings.RunSearchOnEnterKey);
            this.labelTabGeneral.Text = Strings.General;
            this.labelTabDownloads.Text = Strings.Downloads;
            this.groupBoxCrossButtonAction.Text = Strings.ExitPromptSettingsText;
            this.comboBoxCrossButtonAction.Items.Add((object)Strings.CloseAppActionPrompt);
            this.comboBoxCrossButtonAction.Items.Add((object)Strings.CloseAppActionExitApplication);
            this.comboBoxCrossButtonAction.Items.Add((object)Strings.MinimizeToNotificationArea);
        }

        private void BuildLanguagesComboBox()
        {
            List<LanguageItem> source = new List<LanguageItem>()
      {
        new LanguageItem("en"),
        new LanguageItem("uk"),
        new LanguageItem("es"),
        new LanguageItem("pt"),
        new LanguageItem("ru"),
        new LanguageItem("zh-CHS")
      };
            this.comboBoxLanguages.Items.AddRange((object[])source.ToArray());
            this.comboBoxLanguages.SelectedItem = (object)source.SingleOrDefault<LanguageItem>((Func<LanguageItem, bool>)(l => l.CountryCode == Settings.Instance.LanguageCode));
        }

        private void SetBackgroundColorPanel(Panel panel, Color color)
        {
            if (panel.Tag != null)
                return;
            panel.BackColor = color;
        }

        private void InitializeSelectedTag(Panel panel, PictureBox pictureBox, Label label, Panel contentPanel)
        {
            this.panelTabGeneral.Tag = (object)null;
            this.panelTabGeneral.BackColor = this._panelContentTabDefaultBackgroundColor;
            this.panelContentTab1Container.Visible = false;
            this.panelTabDownloads.Tag = (object)null;
            this.panelTabDownloads.BackColor = this._panelContentTabDefaultBackgroundColor;
            this.panelContentTab2Container.Visible = false;
            panel.Tag = (object)"selected";
            panel.BackColor = this._panelContentTabSelectedBackgroundColor;
            this._currentTab = panel;
            this.picBoxMainContent.Image = pictureBox.Image;
            this.labelMainContent.Text = label.Text;
            contentPanel.Visible = true;
            label.ForeColor = WindowHelper.GetContrastColor(this._panelContentTabSelectedBackgroundColor);
            if (this._prevSelectedLabel == label)
                return;
            this._prevSelectedLabel.ForeColor = SystemColors.ControlText;
            this._prevSelectedLabel = label;
        }

        private void panelContentTab1_MouseEnter(object sender, EventArgs e)
        {
            this.SetBackgroundColorPanel((Panel)sender, this._panelContentTabFocusedtBackgroundColor);
        }

        private void panelContentTab1_MouseLeave(object sender, EventArgs e)
        {
            this.SetBackgroundColorPanel((Panel)sender, this._panelContentTabDefaultBackgroundColor);
        }

        private void panelContentTab1_MouseClick(object sender, MouseEventArgs e)
        {
            this.InitializeSelectedTag((Panel)sender, this.picBoxTabGeneral, this.labelTabGeneral, this.panelContentTab1Container);
        }

        private void panelContentTab2_MouseEnter(object sender, EventArgs e)
        {
            this.SetBackgroundColorPanel((Panel)sender, this._panelContentTabFocusedtBackgroundColor);
        }

        private void panelContentTab2_MouseLeave(object sender, EventArgs e)
        {
            this.SetBackgroundColorPanel((Panel)sender, this._panelContentTabDefaultBackgroundColor);
        }

        private void panelContentTab2_MouseClick(object sender, MouseEventArgs e)
        {
            this.InitializeSelectedTag((Panel)sender, this.picBoxTabDownloads, this.labelTabDownloads, this.panelContentTab2Container);
        }

        private void OnCrossButtonActionSelectedIndexChanged(object sender, EventArgs e)
        {
            Settings.Instance.CloseAppAction = (CloseAppActions)this.comboBoxCrossButtonAction.SelectedIndex;
        }

        private void labelTabGeneral_Click(object sender, EventArgs e)
        {
            this.InitializeSelectedTag(this.panelTabGeneral, this.picBoxTabGeneral, this.labelTabGeneral, this.panelContentTab1Container);
        }

        private void labelTabDownloads_Click(object sender, EventArgs e)
        {
            this.InitializeSelectedTag(this.panelTabDownloads, this.picBoxTabDownloads, this.labelTabDownloads, this.panelContentTab2Container);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg != 800)
                return;
            this._panelContentTabSelectedBackgroundColor = WindowHelper.GetWindowColorizationColor(true);
            this._prevSelectedLabel.ForeColor = WindowHelper.GetContrastColor(this._panelContentTabSelectedBackgroundColor);
            this._currentTab.BackColor = this._panelContentTabSelectedBackgroundColor;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = (IContainer)new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SettingsWindow));
            this.comboBoxLanguages = new ComboBox();
            this.numericMaxDownloads = new NumericUpDown();
            this.labelMaxDownloads = new Label();
            this.checkBoxAllowSimDownloads = new CheckBox();
            this.checkBoxIncludeSoundsForNotifications = new CheckBox();
            this.checkBoxIncludeTooltipsForNotifications = new CheckBox();
            this.checkBoxRemoveFinishedFiles = new CheckBox();
            this.buttonCancel = new Button();
            this.buttonOk = new Button();
            this.checkBoxRunSearchOnEnterKey = new CheckBox();
            this.buttonChooseDefaultVideosDownloadFolder = new Button();
            this.textBoxDefaultVideosDownloadFolder = new TextBox();
            this.textBoxDefaultAudioDownloadsFolder = new TextBox();
            this.buttonChooseDefaultAudioDownloadsFolder = new Button();
            this.toolTip = new ToolTip(this.components);
            this.groupBoxDefaultVideoDownloadsFolder = new GroupBox();
            this.groupBoxDefaultAudioDownloadsFolder = new GroupBox();
            this.groupBoxBehavioural = new GroupBox();
            this.groupBoxSimultaneousDownloads = new GroupBox();
            this.groupBoxPreferredLanguage = new GroupBox();
            this.panelMainContainer = new Panel();
            this.panelMenuContainer = new Panel();
            this.panelContainerTab2 = new Panel();
            this.panelTabDownloads = new Panel();
            this.picBoxTabDownloads = new PictureBox();
            this.labelTabDownloads = new Label();
            this.panelContainerTab1 = new Panel();
            this.panelTabGeneral = new Panel();
            this.picBoxTabGeneral = new PictureBox();
            this.labelTabGeneral = new Label();
            this.panelContentContainer = new Panel();
            this.panelContentTitle = new Panel();
            this.picBoxMainContent = new PictureBox();
            this.labelMainContent = new Label();
            this.panelContentTab1Container = new Panel();
            this.groupBoxCrossButtonAction = new GroupBox();
            this.comboBoxCrossButtonAction = new ComboBox();
            this.panelContentTab2Container = new Panel();
            this.checkBoxRememberLastQualityUsed = new CheckBox();
            this.numericMaxDownloads.BeginInit();
            this.groupBoxDefaultVideoDownloadsFolder.SuspendLayout();
            this.groupBoxDefaultAudioDownloadsFolder.SuspendLayout();
            this.groupBoxBehavioural.SuspendLayout();
            this.groupBoxSimultaneousDownloads.SuspendLayout();
            this.groupBoxPreferredLanguage.SuspendLayout();
            this.panelMainContainer.SuspendLayout();
            this.panelMenuContainer.SuspendLayout();
            this.panelContainerTab2.SuspendLayout();
            this.panelTabDownloads.SuspendLayout();
            ((ISupportInitialize)this.picBoxTabDownloads).BeginInit();
            this.panelContainerTab1.SuspendLayout();
            this.panelTabGeneral.SuspendLayout();
            ((ISupportInitialize)this.picBoxTabGeneral).BeginInit();
            this.panelContentContainer.SuspendLayout();
            this.panelContentTitle.SuspendLayout();
            ((ISupportInitialize)this.picBoxMainContent).BeginInit();
            this.panelContentTab1Container.SuspendLayout();
            this.groupBoxCrossButtonAction.SuspendLayout();
            this.panelContentTab2Container.SuspendLayout();
            this.SuspendLayout();
            this.comboBoxLanguages.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxLanguages.FormattingEnabled = true;
            this.comboBoxLanguages.Location = new Point(7, 24);
            this.comboBoxLanguages.Margin = new Padding(8, 5, 8, 2);
            this.comboBoxLanguages.Name = "comboBoxLanguages";
            this.comboBoxLanguages.Size = new Size(479, 24);
            this.comboBoxLanguages.TabIndex = 3;
            this.comboBoxLanguages.SelectedIndexChanged += new EventHandler(this.comboBoxLanguages_SelectedIndexChanged);
            this.numericMaxDownloads.Location = new Point(385, 31);
            this.numericMaxDownloads.Margin = new Padding(4, 6, 3, 2);
            this.numericMaxDownloads.Name = "numericMaxDownloads";
            this.numericMaxDownloads.Size = new Size(107, 22);
            this.numericMaxDownloads.TabIndex = 1;
            this.labelMaxDownloads.AutoSize = true;
            this.labelMaxDownloads.Location = new Point(36, 34);
            this.labelMaxDownloads.Margin = new Padding(0, 10, 0, 0);
            this.labelMaxDownloads.Name = "labelMaxDownloads";
            this.labelMaxDownloads.Size = new Size(301, 17);
            this.labelMaxDownloads.TabIndex = 0;
            this.labelMaxDownloads.Text = "Limit simultaneous downloads to a maximum of";
            this.checkBoxAllowSimDownloads.AutoSize = true;
            this.checkBoxAllowSimDownloads.Location = new Point(12, 34);
            this.checkBoxAllowSimDownloads.Margin = new Padding(4, 10, 4, 4);
            this.checkBoxAllowSimDownloads.Name = "checkBoxAllowSimDownloads";
            this.checkBoxAllowSimDownloads.Size = new Size(18, 17);
            this.checkBoxAllowSimDownloads.TabIndex = 2;
            this.checkBoxAllowSimDownloads.UseVisualStyleBackColor = true;
            this.checkBoxAllowSimDownloads.Click += new EventHandler(this.checkBoxAllowSimDownloads_CheckedChanged);
            this.checkBoxIncludeSoundsForNotifications.AutoSize = true;
            this.checkBoxIncludeSoundsForNotifications.Location = new Point(12, 26);
            this.checkBoxIncludeSoundsForNotifications.Margin = new Padding(4);
            this.checkBoxIncludeSoundsForNotifications.Name = "checkBoxIncludeSoundsForNotifications";
            this.checkBoxIncludeSoundsForNotifications.Size = new Size(225, 21);
            this.checkBoxIncludeSoundsForNotifications.TabIndex = 2;
            this.checkBoxIncludeSoundsForNotifications.Text = "Include sounds for notifications";
            this.checkBoxIncludeSoundsForNotifications.UseVisualStyleBackColor = true;
            this.checkBoxIncludeTooltipsForNotifications.AutoSize = true;
            this.checkBoxIncludeTooltipsForNotifications.Location = new Point(12, 58);
            this.checkBoxIncludeTooltipsForNotifications.Margin = new Padding(4);
            this.checkBoxIncludeTooltipsForNotifications.Name = "checkBoxIncludeTooltipsForNotifications";
            this.checkBoxIncludeTooltipsForNotifications.Size = new Size(224, 21);
            this.checkBoxIncludeTooltipsForNotifications.TabIndex = 3;
            this.checkBoxIncludeTooltipsForNotifications.Text = "Include tooltips for notifications";
            this.checkBoxIncludeTooltipsForNotifications.UseVisualStyleBackColor = true;
            this.checkBoxRemoveFinishedFiles.AutoSize = true;
            this.checkBoxRemoveFinishedFiles.Location = new Point(12, 268);
            this.checkBoxRemoveFinishedFiles.Margin = new Padding(4);
            this.checkBoxRemoveFinishedFiles.Name = "checkBoxRemoveFinishedFiles";
            this.checkBoxRemoveFinishedFiles.Size = new Size(182, 21);
            this.checkBoxRemoveFinishedFiles.TabIndex = 4;
            this.checkBoxRemoveFinishedFiles.Text = "Remove all finished files";
            this.checkBoxRemoveFinishedFiles.UseVisualStyleBackColor = true;
            this.buttonCancel.Location = new Point(478, 405);
            this.buttonCancel.Margin = new Padding(3, 2, 3, 2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new Size(133, 32);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
            this.buttonOk.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.buttonOk.Location = new Point(620, 405);
            this.buttonOk.Margin = new Padding(3, 2, 3, 2);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new Size(133, 32);
            this.buttonOk.TabIndex = 0;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
            this.checkBoxRunSearchOnEnterKey.AutoSize = true;
            this.checkBoxRunSearchOnEnterKey.Location = new Point(12, 90);
            this.checkBoxRunSearchOnEnterKey.Margin = new Padding(4);
            this.checkBoxRunSearchOnEnterKey.Name = "checkBoxRunSearchOnEnterKey";
            this.checkBoxRunSearchOnEnterKey.Size = new Size(187, 21);
            this.checkBoxRunSearchOnEnterKey.TabIndex = 6;
            this.checkBoxRunSearchOnEnterKey.Text = "Run search on Enter key";
            this.checkBoxRunSearchOnEnterKey.UseVisualStyleBackColor = true;
            this.buttonChooseDefaultVideosDownloadFolder.Location = new Point(424, 30);
            this.buttonChooseDefaultVideosDownloadFolder.Margin = new Padding(4);
            this.buttonChooseDefaultVideosDownloadFolder.Name = "buttonChooseDefaultVideosDownloadFolder";
            this.buttonChooseDefaultVideosDownloadFolder.Size = new Size(69, 26);
            this.buttonChooseDefaultVideosDownloadFolder.TabIndex = 0;
            this.buttonChooseDefaultVideosDownloadFolder.Text = "...";
            this.buttonChooseDefaultVideosDownloadFolder.UseVisualStyleBackColor = true;
            this.buttonChooseDefaultVideosDownloadFolder.Click += new EventHandler(this.buttonChooseDefaultDownloadFolder_Click);
            this.textBoxDefaultVideosDownloadFolder.Location = new Point(12, 30);
            this.textBoxDefaultVideosDownloadFolder.Margin = new Padding(4);
            this.textBoxDefaultVideosDownloadFolder.Name = "textBoxDefaultVideosDownloadFolder";
            this.textBoxDefaultVideosDownloadFolder.ReadOnly = true;
            this.textBoxDefaultVideosDownloadFolder.Size = new Size(405, 22);
            this.textBoxDefaultVideosDownloadFolder.TabIndex = 1;
            this.textBoxDefaultAudioDownloadsFolder.Location = new Point(12, 30);
            this.textBoxDefaultAudioDownloadsFolder.Margin = new Padding(4);
            this.textBoxDefaultAudioDownloadsFolder.Name = "textBoxDefaultAudioDownloadsFolder";
            this.textBoxDefaultAudioDownloadsFolder.ReadOnly = true;
            this.textBoxDefaultAudioDownloadsFolder.Size = new Size(405, 22);
            this.textBoxDefaultAudioDownloadsFolder.TabIndex = 0;
            this.buttonChooseDefaultAudioDownloadsFolder.Location = new Point(424, 30);
            this.buttonChooseDefaultAudioDownloadsFolder.Margin = new Padding(4);
            this.buttonChooseDefaultAudioDownloadsFolder.Name = "buttonChooseDefaultAudioDownloadsFolder";
            this.buttonChooseDefaultAudioDownloadsFolder.Size = new Size(69, 26);
            this.buttonChooseDefaultAudioDownloadsFolder.TabIndex = 1;
            this.buttonChooseDefaultAudioDownloadsFolder.Text = "...";
            this.buttonChooseDefaultAudioDownloadsFolder.UseVisualStyleBackColor = true;
            this.buttonChooseDefaultAudioDownloadsFolder.Click += new EventHandler(this.buttonChooseDefaultConversionFolder_Click);
            this.groupBoxDefaultVideoDownloadsFolder.Controls.Add((Control)this.buttonChooseDefaultVideosDownloadFolder);
            this.groupBoxDefaultVideoDownloadsFolder.Controls.Add((Control)this.textBoxDefaultVideosDownloadFolder);
            this.groupBoxDefaultVideoDownloadsFolder.Location = new Point(0, 6);
            this.groupBoxDefaultVideoDownloadsFolder.Margin = new Padding(4);
            this.groupBoxDefaultVideoDownloadsFolder.Name = "groupBoxDefaultVideoDownloadsFolder";
            this.groupBoxDefaultVideoDownloadsFolder.Padding = new Padding(4);
            this.groupBoxDefaultVideoDownloadsFolder.Size = new Size(504, 76);
            this.groupBoxDefaultVideoDownloadsFolder.TabIndex = 3;
            this.groupBoxDefaultVideoDownloadsFolder.TabStop = false;
            this.groupBoxDefaultVideoDownloadsFolder.Text = "Default folder for video downloads";
            this.groupBoxDefaultAudioDownloadsFolder.Controls.Add((Control)this.buttonChooseDefaultAudioDownloadsFolder);
            this.groupBoxDefaultAudioDownloadsFolder.Controls.Add((Control)this.textBoxDefaultAudioDownloadsFolder);
            this.groupBoxDefaultAudioDownloadsFolder.Location = new Point(0, 89);
            this.groupBoxDefaultAudioDownloadsFolder.Margin = new Padding(4);
            this.groupBoxDefaultAudioDownloadsFolder.Name = "groupBoxDefaultAudioDownloadsFolder";
            this.groupBoxDefaultAudioDownloadsFolder.Padding = new Padding(4);
            this.groupBoxDefaultAudioDownloadsFolder.Size = new Size(504, 76);
            this.groupBoxDefaultAudioDownloadsFolder.TabIndex = 4;
            this.groupBoxDefaultAudioDownloadsFolder.TabStop = false;
            this.groupBoxDefaultAudioDownloadsFolder.Text = "Default folder for audio downloads";
            this.groupBoxBehavioural.Controls.Add((Control)this.checkBoxIncludeSoundsForNotifications);
            this.groupBoxBehavioural.Controls.Add((Control)this.checkBoxIncludeTooltipsForNotifications);
            this.groupBoxBehavioural.Controls.Add((Control)this.checkBoxRunSearchOnEnterKey);
            this.groupBoxBehavioural.Location = new Point(0, 151);
            this.groupBoxBehavioural.Margin = new Padding(4);
            this.groupBoxBehavioural.Name = "groupBoxBehavioural";
            this.groupBoxBehavioural.Padding = new Padding(4);
            this.groupBoxBehavioural.Size = new Size(504, 177);
            this.groupBoxBehavioural.TabIndex = 5;
            this.groupBoxBehavioural.TabStop = false;
            this.groupBoxBehavioural.Text = "Behavioural";
            this.groupBoxSimultaneousDownloads.Controls.Add((Control)this.numericMaxDownloads);
            this.groupBoxSimultaneousDownloads.Controls.Add((Control)this.checkBoxAllowSimDownloads);
            this.groupBoxSimultaneousDownloads.Controls.Add((Control)this.labelMaxDownloads);
            this.groupBoxSimultaneousDownloads.Location = new Point(0, 173);
            this.groupBoxSimultaneousDownloads.Margin = new Padding(4);
            this.groupBoxSimultaneousDownloads.Name = "groupBoxSimultaneousDownloads";
            this.groupBoxSimultaneousDownloads.Padding = new Padding(4);
            this.groupBoxSimultaneousDownloads.Size = new Size(504, 76);
            this.groupBoxSimultaneousDownloads.TabIndex = 6;
            this.groupBoxSimultaneousDownloads.TabStop = false;
            this.groupBoxSimultaneousDownloads.Text = "Simultaneous downloads";
            this.groupBoxPreferredLanguage.Controls.Add((Control)this.comboBoxLanguages);
            this.groupBoxPreferredLanguage.Location = new Point(0, 6);
            this.groupBoxPreferredLanguage.Margin = new Padding(4);
            this.groupBoxPreferredLanguage.Name = "groupBoxPreferredLanguage";
            this.groupBoxPreferredLanguage.Padding = new Padding(4);
            this.groupBoxPreferredLanguage.Size = new Size(504, 68);
            this.groupBoxPreferredLanguage.TabIndex = 7;
            this.groupBoxPreferredLanguage.TabStop = false;
            this.groupBoxPreferredLanguage.Text = "Preferred language";
            this.panelMainContainer.BackColor = Color.White;
            this.panelMainContainer.Controls.Add((Control)this.buttonOk);
            this.panelMainContainer.Controls.Add((Control)this.buttonCancel);
            this.panelMainContainer.Controls.Add((Control)this.panelMenuContainer);
            this.panelMainContainer.Controls.Add((Control)this.panelContentContainer);
            this.panelMainContainer.Location = new Point(0, 0);
            this.panelMainContainer.Margin = new Padding(4);
            this.panelMainContainer.Name = "panelMainContainer";
            this.panelMainContainer.Size = new Size(777, 455);
            this.panelMainContainer.TabIndex = 8;
            this.panelMenuContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            this.panelMenuContainer.BackColor = Color.Transparent;
            this.panelMenuContainer.Controls.Add((Control)this.panelContainerTab2);
            this.panelMenuContainer.Controls.Add((Control)this.panelContainerTab1);
            this.panelMenuContainer.Location = new Point(8, 7);
            this.panelMenuContainer.Margin = new Padding(4);
            this.panelMenuContainer.Name = "panelMenuContainer";
            this.panelMenuContainer.Size = new Size(232, 441);
            this.panelMenuContainer.TabIndex = 0;
            this.panelContainerTab2.BackColor = Color.FromArgb(221, 221, 221);
            this.panelContainerTab2.Controls.Add((Control)this.panelTabDownloads);
            this.panelContainerTab2.Location = new Point(0, 57);
            this.panelContainerTab2.Margin = new Padding(4);
            this.panelContainerTab2.Name = "panelContainerTab2";
            this.panelContainerTab2.Size = new Size(232, 49);
            this.panelContainerTab2.TabIndex = 1;
            this.panelTabDownloads.BackColor = Color.FromArgb(238, 238, 238);
            this.panelTabDownloads.Controls.Add((Control)this.picBoxTabDownloads);
            this.panelTabDownloads.Controls.Add((Control)this.labelTabDownloads);
            this.panelTabDownloads.Location = new Point(1, 1);
            this.panelTabDownloads.Margin = new Padding(4);
            this.panelTabDownloads.Name = "panelTabDownloads";
            this.panelTabDownloads.Size = new Size(229, 47);
            this.panelTabDownloads.TabIndex = 0;
            this.panelTabDownloads.MouseClick += new MouseEventHandler(this.panelContentTab2_MouseClick);
            this.panelTabDownloads.MouseEnter += new EventHandler(this.panelContentTab2_MouseEnter);
            this.panelTabDownloads.MouseLeave += new EventHandler(this.panelContentTab2_MouseLeave);
            this.picBoxTabDownloads.BackColor = Color.Transparent;
            this.picBoxTabDownloads.Enabled = false;
            this.picBoxTabDownloads.Image = (Image)componentResourceManager.GetObject("picBoxTabDownloads.Image");
            this.picBoxTabDownloads.Location = new Point(15, 14);
            this.picBoxTabDownloads.Margin = new Padding(4);
            this.picBoxTabDownloads.Name = "picBoxTabDownloads";
            this.picBoxTabDownloads.Size = new Size(21, 20);
            this.picBoxTabDownloads.TabIndex = 3;
            this.picBoxTabDownloads.TabStop = false;
            this.labelTabDownloads.AutoSize = true;
            this.labelTabDownloads.Location = new Point(51, 15);
            this.labelTabDownloads.Margin = new Padding(4, 0, 4, 0);
            this.labelTabDownloads.Name = "labelTabDownloads";
            this.labelTabDownloads.Size = new Size(77, 17);
            this.labelTabDownloads.TabIndex = 2;
            this.labelTabDownloads.Text = "Downloads";
            this.labelTabDownloads.Click += new EventHandler(this.labelTabDownloads_Click);
            this.panelContainerTab1.BackColor = Color.FromArgb(221, 221, 221);
            this.panelContainerTab1.Controls.Add((Control)this.panelTabGeneral);
            this.panelContainerTab1.Location = new Point(0, 0);
            this.panelContainerTab1.Margin = new Padding(4);
            this.panelContainerTab1.Name = "panelContainerTab1";
            this.panelContainerTab1.Size = new Size(232, 49);
            this.panelContainerTab1.TabIndex = 0;
            this.panelTabGeneral.BackColor = Color.FromArgb(238, 238, 238);
            this.panelTabGeneral.Controls.Add((Control)this.picBoxTabGeneral);
            this.panelTabGeneral.Controls.Add((Control)this.labelTabGeneral);
            this.panelTabGeneral.Location = new Point(1, 1);
            this.panelTabGeneral.Margin = new Padding(4);
            this.panelTabGeneral.Name = "panelTabGeneral";
            this.panelTabGeneral.Size = new Size(229, 47);
            this.panelTabGeneral.TabIndex = 0;
            this.panelTabGeneral.MouseClick += new MouseEventHandler(this.panelContentTab1_MouseClick);
            this.panelTabGeneral.MouseEnter += new EventHandler(this.panelContentTab1_MouseEnter);
            this.panelTabGeneral.MouseLeave += new EventHandler(this.panelContentTab1_MouseLeave);
            this.picBoxTabGeneral.BackColor = Color.Transparent;
            this.picBoxTabGeneral.Enabled = false;
            this.picBoxTabGeneral.Image = (Image)componentResourceManager.GetObject("picBoxTabGeneral.Image");
            this.picBoxTabGeneral.Location = new Point(15, 14);
            this.picBoxTabGeneral.Margin = new Padding(4);
            this.picBoxTabGeneral.Name = "picBoxTabGeneral";
            this.picBoxTabGeneral.Size = new Size(21, 20);
            this.picBoxTabGeneral.TabIndex = 1;
            this.picBoxTabGeneral.TabStop = false;
            this.labelTabGeneral.AutoSize = true;
            this.labelTabGeneral.Location = new Point(51, 15);
            this.labelTabGeneral.Margin = new Padding(4, 0, 4, 0);
            this.labelTabGeneral.Name = "labelTabGeneral";
            this.labelTabGeneral.Size = new Size(59, 17);
            this.labelTabGeneral.TabIndex = 0;
            this.labelTabGeneral.Text = "General";
            this.labelTabGeneral.Click += new EventHandler(this.labelTabGeneral_Click);
            this.panelContentContainer.BackColor = Color.Transparent;
            this.panelContentContainer.Controls.Add((Control)this.panelContentTitle);
            this.panelContentContainer.Controls.Add((Control)this.panelContentTab1Container);
            this.panelContentContainer.Controls.Add((Control)this.panelContentTab2Container);
            this.panelContentContainer.Location = new Point(249, 7);
            this.panelContentContainer.Margin = new Padding(4);
            this.panelContentContainer.Name = "panelContentContainer";
            this.panelContentContainer.Size = new Size(520, 382);
            this.panelContentContainer.TabIndex = 1;
            this.panelContentTitle.BackColor = Color.FromArgb(238, 238, 238);
            this.panelContentTitle.Controls.Add((Control)this.picBoxMainContent);
            this.panelContentTitle.Controls.Add((Control)this.labelMainContent);
            this.panelContentTitle.Location = new Point(0, 0);
            this.panelContentTitle.Margin = new Padding(4);
            this.panelContentTitle.Name = "panelContentTitle";
            this.panelContentTitle.Size = new Size(520, 48);
            this.panelContentTitle.TabIndex = 0;
            this.picBoxMainContent.BackColor = Color.Transparent;
            this.picBoxMainContent.Enabled = false;
            this.picBoxMainContent.Image = (Image)componentResourceManager.GetObject("picBoxMainContent.Image");
            this.picBoxMainContent.Location = new Point(15, 14);
            this.picBoxMainContent.Margin = new Padding(4);
            this.picBoxMainContent.Name = "picBoxMainContent";
            this.picBoxMainContent.Size = new Size(21, 20);
            this.picBoxMainContent.TabIndex = 3;
            this.picBoxMainContent.TabStop = false;
            this.labelMainContent.Enabled = false;
            this.labelMainContent.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.labelMainContent.Location = new Point(51, 11);
            this.labelMainContent.Margin = new Padding(4, 0, 4, 0);
            this.labelMainContent.Name = "labelMainContent";
            this.labelMainContent.Size = new Size(431, 25);
            this.labelMainContent.TabIndex = 2;
            this.labelMainContent.Text = "General";
            this.labelMainContent.TextAlign = ContentAlignment.MiddleCenter;
            this.panelContentTab1Container.BackColor = Color.Transparent;
            this.panelContentTab1Container.Controls.Add((Control)this.groupBoxBehavioural);
            this.panelContentTab1Container.Controls.Add((Control)this.groupBoxCrossButtonAction);
            this.panelContentTab1Container.Controls.Add((Control)this.groupBoxPreferredLanguage);
            this.panelContentTab1Container.Location = new Point(0, 48);
            this.panelContentTab1Container.Margin = new Padding(4);
            this.panelContentTab1Container.Name = "panelContentTab1Container";
            this.panelContentTab1Container.Size = new Size(520, 332);
            this.panelContentTab1Container.TabIndex = 1;
            this.groupBoxCrossButtonAction.Controls.Add((Control)this.comboBoxCrossButtonAction);
            this.groupBoxCrossButtonAction.Location = new Point(0, 80);
            this.groupBoxCrossButtonAction.Name = "groupBoxCrossButtonAction";
            this.groupBoxCrossButtonAction.Size = new Size(505, 64);
            this.groupBoxCrossButtonAction.TabIndex = 8;
            this.groupBoxCrossButtonAction.TabStop = false;
            this.groupBoxCrossButtonAction.Text = "Action when red X-button is clicked";
            this.comboBoxCrossButtonAction.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBoxCrossButtonAction.FormattingEnabled = true;
            this.comboBoxCrossButtonAction.Location = new Point(8, 21);
            this.comboBoxCrossButtonAction.Name = "comboBoxCrossButtonAction";
            this.comboBoxCrossButtonAction.Size = new Size(483, 24);
            this.comboBoxCrossButtonAction.TabIndex = 0;
            this.comboBoxCrossButtonAction.SelectedIndexChanged += new EventHandler(this.OnCrossButtonActionSelectedIndexChanged);
            this.panelContentTab2Container.BackColor = Color.Transparent;
            this.panelContentTab2Container.Controls.Add((Control)this.checkBoxRememberLastQualityUsed);
            this.panelContentTab2Container.Controls.Add((Control)this.checkBoxRemoveFinishedFiles);
            this.panelContentTab2Container.Controls.Add((Control)this.groupBoxDefaultVideoDownloadsFolder);
            this.panelContentTab2Container.Controls.Add((Control)this.groupBoxDefaultAudioDownloadsFolder);
            this.panelContentTab2Container.Controls.Add((Control)this.groupBoxSimultaneousDownloads);
            this.panelContentTab2Container.Location = new Point(0, 48);
            this.panelContentTab2Container.Margin = new Padding(4);
            this.panelContentTab2Container.Name = "panelContentTab2Container";
            this.panelContentTab2Container.Size = new Size(520, 332);
            this.panelContentTab2Container.TabIndex = 2;
            this.checkBoxRememberLastQualityUsed.AutoSize = true;
            this.checkBoxRememberLastQualityUsed.Location = new Point(12, 297);
            this.checkBoxRememberLastQualityUsed.Margin = new Padding(4);
            this.checkBoxRememberLastQualityUsed.Name = "checkBoxRememberLastQualityUsed";
            this.checkBoxRememberLastQualityUsed.Size = new Size(205, 21);
            this.checkBoxRememberLastQualityUsed.TabIndex = 7;
            this.checkBoxRememberLastQualityUsed.Text = "Remember last quality used";
            this.checkBoxRememberLastQualityUsed.UseVisualStyleBackColor = true;
            this.AutoScaleDimensions = new SizeF(8f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.ActiveCaptionText;
            this.ClientSize = new Size(777, 455);
            this.Controls.Add((Control)this.panelMainContainer);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Margin = new Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.numericMaxDownloads.EndInit();
            this.groupBoxDefaultVideoDownloadsFolder.ResumeLayout(false);
            this.groupBoxDefaultVideoDownloadsFolder.PerformLayout();
            this.groupBoxDefaultAudioDownloadsFolder.ResumeLayout(false);
            this.groupBoxDefaultAudioDownloadsFolder.PerformLayout();
            this.groupBoxBehavioural.ResumeLayout(false);
            this.groupBoxBehavioural.PerformLayout();
            this.groupBoxSimultaneousDownloads.ResumeLayout(false);
            this.groupBoxSimultaneousDownloads.PerformLayout();
            this.groupBoxPreferredLanguage.ResumeLayout(false);
            this.panelMainContainer.ResumeLayout(false);
            this.panelMenuContainer.ResumeLayout(false);
            this.panelContainerTab2.ResumeLayout(false);
            this.panelTabDownloads.ResumeLayout(false);
            this.panelTabDownloads.PerformLayout();
            ((ISupportInitialize)this.picBoxTabDownloads).EndInit();
            this.panelContainerTab1.ResumeLayout(false);
            this.panelTabGeneral.ResumeLayout(false);
            this.panelTabGeneral.PerformLayout();
            ((ISupportInitialize)this.picBoxTabGeneral).EndInit();
            this.panelContentContainer.ResumeLayout(false);
            this.panelContentTitle.ResumeLayout(false);
            ((ISupportInitialize)this.picBoxMainContent).EndInit();
            this.panelContentTab1Container.ResumeLayout(false);
            this.groupBoxCrossButtonAction.ResumeLayout(false);
            this.panelContentTab2Container.ResumeLayout(false);
            this.panelContentTab2Container.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
