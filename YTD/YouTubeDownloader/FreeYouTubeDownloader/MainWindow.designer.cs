using BrightIdeasSoftware;
using FreeYouTubeDownloader.Properties;
using FreeYouTubeDownloader.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wyDay.Controls;

namespace FreeYouTubeDownloader
{
    public partial class MainWindow
    {
        private IContainer components;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.olvDownloads = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDuration = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnProgress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.barRenderer = new BrightIdeasSoftware.BarRenderer();
            this.olvColumnDownloadSpeed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnEta = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFormat = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnFrameSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tableLayoutPanelActionButtons = new System.Windows.Forms.TableLayoutPanel();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.tableLayoutPanelActionButtonsContainer = new System.Windows.Forms.TableLayoutPanel();
            this.panelButtonActionVideo = new System.Windows.Forms.Panel();
            this.picBoxVideo = new FreeYouTubeDownloader.UI.TransparentPictureBox();
            this.splitButtonActionVideo = new wyDay.Controls.SplitButton();
            this.contextMenuStripDownloadVideo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelButtonActionAudio = new System.Windows.Forms.Panel();
            this.picBoxAudio = new FreeYouTubeDownloader.UI.TransparentPictureBox();
            this.splitButtonActionAudio = new wyDay.Controls.SplitButton();
            this.contextMenuStripDownloadAudio = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxSettings = new System.Windows.Forms.PictureBox();
            this.lnkLabelSettings = new System.Windows.Forms.LinkLabel();
            this.btnVideoFiles = new System.Windows.Forms.Button();
            this.btnAudioFiles = new System.Windows.Forms.Button();
            this.panelSearchResult = new System.Windows.Forms.Panel();
            this.pictureBoxCopyPath = new System.Windows.Forms.PictureBox();
            this.pictureBoxOpenFile = new System.Windows.Forms.PictureBox();
            this.lblSearchResultTime = new System.Windows.Forms.Label();
            this.lblSearchResultDescription = new System.Windows.Forms.Label();
            this.lblSearchResultTitle = new System.Windows.Forms.Label();
            this.PanelContainerAutoComplete = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.windowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysOnTopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.minimizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizeToNotificationAreaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.askFileNameAndFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compatibleURLNotificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoDownloadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.videoAutoDownloadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.audioAutoDownloadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.versionNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.PanelWrapperContainerAutoComplete = new System.Windows.Forms.Panel();
            this.progressBarLoadingData = new System.Windows.Forms.ProgressBar();
            this.toolTipPictureBoxOpenFile = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDownloads)).BeginInit();
            this.tableLayoutPanelActionButtons.SuspendLayout();
            this.tableLayoutPanelActionButtonsContainer.SuspendLayout();
            this.panelButtonActionVideo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxVideo)).BeginInit();
            this.panelButtonActionAudio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAudio)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).BeginInit();
            this.panelSearchResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCopyPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpenFile)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.PanelWrapperContainerAutoComplete.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxUrl.Location = new System.Drawing.Point(10, 7);
            this.textBoxUrl.Margin = new System.Windows.Forms.Padding(10, 7, 0, 8);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(629, 21);
            this.textBoxUrl.TabIndex = 6;
            this.textBoxUrl.Text = "Paste URL here";
            this.textBoxUrl.Click += new System.EventHandler(this.OnTextBoxUrlClick);
            this.textBoxUrl.TextChanged += new System.EventHandler(this.OnTextBoxUrlTextChanged);
            this.textBoxUrl.Enter += new System.EventHandler(this.OnTextBoxUrlEnter);
            this.textBoxUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnTextBoxUrlKeyDown);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textBoxUrl, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.statusStrip, 0, 6);
            this.tableLayoutPanel.Controls.Add(this.olvDownloads, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelActionButtons, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanelActionButtonsContainer, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.tableLayoutPanel2, 0, 5);
            this.tableLayoutPanel.Controls.Add(this.panelSearchResult, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 7;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 47F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(674, 466);
            this.tableLayoutPanel.TabIndex = 8;
            // 
            // statusStrip
            // 
            this.statusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.tableLayoutPanel.SetColumnSpan(this.statusStrip, 2);
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 444);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(674, 22);
            this.statusStrip.TabIndex = 9;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // olvDownloads
            // 
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
            this.olvDownloads.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnStatus,
            this.olvColumnName,
            this.olvColumnDuration,
            this.olvColumnProgress,
            this.olvColumnDownloadSpeed,
            this.olvColumnEta,
            this.olvColumnFormat,
            this.olvColumnSize,
            this.olvColumnFrameSize});
            this.tableLayoutPanel.SetColumnSpan(this.olvDownloads, 2);
            this.olvDownloads.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvDownloads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvDownloads.FullRowSelect = true;
            this.olvDownloads.Location = new System.Drawing.Point(10, 149);
            this.olvDownloads.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.olvDownloads.Name = "olvDownloads";
            this.olvDownloads.Size = new System.Drawing.Size(654, 257);
            this.olvDownloads.SortGroupItemsByPrimaryColumn = false;
            this.olvDownloads.TabIndex = 10;
            this.olvDownloads.UseCompatibleStateImageBehavior = false;
            this.olvDownloads.UseNotifyPropertyChanged = true;
            this.olvDownloads.View = System.Windows.Forms.View.Details;
            this.olvDownloads.CellRightClick += new System.EventHandler<BrightIdeasSoftware.CellRightClickEventArgs>(this.OnDownloadsListCellRightClick);
            this.olvDownloads.SelectionChanged += new System.EventHandler(this.OnDownloadsSelectionChanged);
            this.olvDownloads.DoubleClick += new System.EventHandler(this.OnDownloadsListDoubleClick);
            this.olvDownloads.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OnDownloadsListKeyUp);
            // 
            // olvColumnStatus
            // 
            this.olvColumnStatus.IsEditable = false;
            this.olvColumnStatus.MaximumWidth = 18;
            this.olvColumnStatus.MinimumWidth = 18;
            this.olvColumnStatus.Text = "";
            this.olvColumnStatus.Width = 18;
            // 
            // olvColumnName
            // 
            this.olvColumnName.AspectName = "Name";
            this.olvColumnName.IsEditable = false;
            this.olvColumnName.MinimumWidth = 170;
            this.olvColumnName.Text = "Name";
            this.olvColumnName.Width = 170;
            // 
            // olvColumnDuration
            // 
            this.olvColumnDuration.AspectName = "Duration";
            this.olvColumnDuration.MinimumWidth = 65;
            this.olvColumnDuration.Text = "Duration";
            this.olvColumnDuration.Width = 65;
            // 
            // olvColumnProgress
            // 
            this.olvColumnProgress.AspectName = "Progress";
            this.olvColumnProgress.IsEditable = false;
            this.olvColumnProgress.MinimumWidth = 105;
            this.olvColumnProgress.Renderer = this.barRenderer;
            this.olvColumnProgress.Text = "Progress";
            this.olvColumnProgress.Width = 105;
            // 
            // olvColumnDownloadSpeed
            // 
            this.olvColumnDownloadSpeed.AspectName = "Speed";
            this.olvColumnDownloadSpeed.IsEditable = false;
            this.olvColumnDownloadSpeed.MinimumWidth = 70;
            this.olvColumnDownloadSpeed.Text = "Speed";
            this.olvColumnDownloadSpeed.Width = 70;
            // 
            // olvColumnEta
            // 
            this.olvColumnEta.AspectName = "SecondsRemains";
            this.olvColumnEta.MinimumWidth = 60;
            this.olvColumnEta.Text = "ETA";
            // 
            // olvColumnFormat
            // 
            this.olvColumnFormat.AspectName = "FileNameFormat";
            this.olvColumnFormat.IsEditable = false;
            this.olvColumnFormat.MinimumWidth = 45;
            this.olvColumnFormat.Text = "Format";
            this.olvColumnFormat.Width = 45;
            // 
            // olvColumnSize
            // 
            this.olvColumnSize.AspectName = "Size";
            this.olvColumnSize.IsEditable = false;
            this.olvColumnSize.MinimumWidth = 65;
            this.olvColumnSize.Text = "Size";
            this.olvColumnSize.Width = 65;
            // 
            // olvColumnFrameSize
            // 
            this.olvColumnFrameSize.AspectName = "FrameSize";
            this.olvColumnFrameSize.MinimumWidth = 65;
            this.olvColumnFrameSize.Text = "Dimension";
            this.olvColumnFrameSize.Width = 65;
            // 
            // tableLayoutPanelActionButtons
            // 
            this.tableLayoutPanelActionButtons.ColumnCount = 1;
            this.tableLayoutPanelActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelActionButtons.Controls.Add(this.buttonConvert, 0, 0);
            this.tableLayoutPanelActionButtons.Location = new System.Drawing.Point(642, 3);
            this.tableLayoutPanelActionButtons.Name = "tableLayoutPanelActionButtons";
            this.tableLayoutPanelActionButtons.RowCount = 1;
            this.tableLayoutPanelActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelActionButtons.Size = new System.Drawing.Size(29, 30);
            this.tableLayoutPanelActionButtons.TabIndex = 11;
            // 
            // buttonConvert
            // 
            this.buttonConvert.Cursor = System.Windows.Forms.Cursors.Default;
            this.buttonConvert.Location = new System.Drawing.Point(1, 3);
            this.buttonConvert.Margin = new System.Windows.Forms.Padding(1, 3, 3, 3);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(25, 23);
            this.buttonConvert.TabIndex = 0;
            this.buttonConvert.Text = "...";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.OnConvertButtonClick);
            // 
            // tableLayoutPanelActionButtonsContainer
            // 
            this.tableLayoutPanelActionButtonsContainer.ColumnCount = 8;
            this.tableLayoutPanel.SetColumnSpan(this.tableLayoutPanelActionButtonsContainer, 2);
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 282F));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 282F));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelActionButtonsContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 4F));
            this.tableLayoutPanelActionButtonsContainer.Controls.Add(this.panelButtonActionVideo, 1, 0);
            this.tableLayoutPanelActionButtonsContainer.Controls.Add(this.panelButtonActionAudio, 3, 0);
            this.tableLayoutPanelActionButtonsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelActionButtonsContainer.Location = new System.Drawing.Point(3, 105);
            this.tableLayoutPanelActionButtonsContainer.Name = "tableLayoutPanelActionButtonsContainer";
            this.tableLayoutPanelActionButtonsContainer.RowCount = 1;
            this.tableLayoutPanelActionButtonsContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelActionButtonsContainer.Size = new System.Drawing.Size(668, 41);
            this.tableLayoutPanelActionButtonsContainer.TabIndex = 12;
            // 
            // panelButtonActionVideo
            // 
            this.panelButtonActionVideo.Controls.Add(this.picBoxVideo);
            this.panelButtonActionVideo.Controls.Add(this.splitButtonActionVideo);
            this.panelButtonActionVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtonActionVideo.Location = new System.Drawing.Point(4, 3);
            this.panelButtonActionVideo.Name = "panelButtonActionVideo";
            this.panelButtonActionVideo.Size = new System.Drawing.Size(276, 35);
            this.panelButtonActionVideo.TabIndex = 6;
            // 
            // picBoxVideo
            // 
            this.picBoxVideo.BackColor = System.Drawing.Color.Transparent;
            this.picBoxVideo.Image = global::FreeYouTubeDownloader.Properties.Resources.movie;
            this.picBoxVideo.Location = new System.Drawing.Point(3, 3);
            this.picBoxVideo.Name = "picBoxVideo";
            this.picBoxVideo.Size = new System.Drawing.Size(26, 28);
            this.picBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxVideo.TabIndex = 0;
            this.picBoxVideo.TabStop = false;
            // 
            // splitButtonActionVideo
            // 
            this.splitButtonActionVideo.AutoSize = true;
            this.splitButtonActionVideo.ContextMenuStrip = this.contextMenuStripDownloadVideo;
            this.splitButtonActionVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.splitButtonActionVideo.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.splitButtonActionVideo.Location = new System.Drawing.Point(32, 0);
            this.splitButtonActionVideo.Margin = new System.Windows.Forms.Padding(0);
            this.splitButtonActionVideo.Name = "splitButtonActionVideo";
            this.splitButtonActionVideo.Size = new System.Drawing.Size(244, 29);
            this.splitButtonActionVideo.SplitMenuStrip = this.contextMenuStripDownloadVideo;
            this.splitButtonActionVideo.TabIndex = 1;
            this.splitButtonActionVideo.Text = "Download as Video";
            this.splitButtonActionVideo.UseVisualStyleBackColor = true;
            this.splitButtonActionVideo.Click += new System.EventHandler(this.OnActionButtonClick);
            this.splitButtonActionVideo.Paint += new System.Windows.Forms.PaintEventHandler(this.splitButtonActionVideo_Paint);
            // 
            // contextMenuStripDownloadVideo
            // 
            this.contextMenuStripDownloadVideo.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDownloadVideo.Name = "contextMenuStripDownloadVideo";
            this.contextMenuStripDownloadVideo.ShowImageMargin = false;
            this.contextMenuStripDownloadVideo.Size = new System.Drawing.Size(36, 4);
            // 
            // panelButtonActionAudio
            // 
            this.panelButtonActionAudio.Controls.Add(this.picBoxAudio);
            this.panelButtonActionAudio.Controls.Add(this.splitButtonActionAudio);
            this.panelButtonActionAudio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtonActionAudio.Location = new System.Drawing.Point(286, 3);
            this.panelButtonActionAudio.Name = "panelButtonActionAudio";
            this.panelButtonActionAudio.Size = new System.Drawing.Size(276, 35);
            this.panelButtonActionAudio.TabIndex = 7;
            // 
            // picBoxAudio
            // 
            this.picBoxAudio.BackColor = System.Drawing.Color.Transparent;
            this.picBoxAudio.Image = global::FreeYouTubeDownloader.Properties.Resources.music;
            this.picBoxAudio.Location = new System.Drawing.Point(4, 3);
            this.picBoxAudio.Name = "picBoxAudio";
            this.picBoxAudio.Size = new System.Drawing.Size(26, 28);
            this.picBoxAudio.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBoxAudio.TabIndex = 3;
            this.picBoxAudio.TabStop = false;
            // 
            // splitButtonActionAudio
            // 
            this.splitButtonActionAudio.AutoSize = true;
            this.splitButtonActionAudio.ContextMenuStrip = this.contextMenuStripDownloadAudio;
            this.splitButtonActionAudio.Cursor = System.Windows.Forms.Cursors.Hand;
            this.splitButtonActionAudio.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.splitButtonActionAudio.Location = new System.Drawing.Point(36, 0);
            this.splitButtonActionAudio.Name = "splitButtonActionAudio";
            this.splitButtonActionAudio.Size = new System.Drawing.Size(240, 29);
            this.splitButtonActionAudio.SplitMenuStrip = this.contextMenuStripDownloadAudio;
            this.splitButtonActionAudio.TabIndex = 2;
            this.splitButtonActionAudio.Text = "Download as Audio";
            this.splitButtonActionAudio.UseVisualStyleBackColor = true;
            this.splitButtonActionAudio.Click += new System.EventHandler(this.OnActionButtonClick);
            this.splitButtonActionAudio.Paint += new System.Windows.Forms.PaintEventHandler(this.splitButtonActionAudio_Paint);
            // 
            // contextMenuStripDownloadAudio
            // 
            this.contextMenuStripDownloadAudio.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDownloadAudio.Name = "contextMenuStripDownloadAudio";
            this.contextMenuStripDownloadAudio.ShowImageMargin = false;
            this.contextMenuStripDownloadAudio.Size = new System.Drawing.Size(36, 4);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 9;
            this.tableLayoutPanel.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBoxSettings, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.lnkLabelSettings, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnVideoFiles, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAudioFiles, 8, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(8, 409);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(8, 3, 6, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(660, 32);
            this.tableLayoutPanel2.TabIndex = 13;
            // 
            // pictureBoxSettings
            // 
            this.pictureBoxSettings.Image = global::FreeYouTubeDownloader.Properties.Resources.gears_preferences;
            this.pictureBoxSettings.Location = new System.Drawing.Point(200, 3);
            this.pictureBoxSettings.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.pictureBoxSettings.Name = "pictureBoxSettings";
            this.pictureBoxSettings.Size = new System.Drawing.Size(30, 25);
            this.pictureBoxSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxSettings.TabIndex = 4;
            this.pictureBoxSettings.TabStop = false;
            // 
            // lnkLabelSettings
            // 
            this.lnkLabelSettings.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lnkLabelSettings.AutoSize = true;
            this.lnkLabelSettings.Location = new System.Drawing.Point(230, 9);
            this.lnkLabelSettings.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lnkLabelSettings.Name = "lnkLabelSettings";
            this.lnkLabelSettings.Size = new System.Drawing.Size(64, 13);
            this.lnkLabelSettings.TabIndex = 6;
            this.lnkLabelSettings.TabStop = true;
            this.lnkLabelSettings.Text = "Preferences";
            this.lnkLabelSettings.Click += new System.EventHandler(this.OnPreferencesMenuItemClick);
            // 
            // btnVideoFiles
            // 
            this.btnVideoFiles.BackColor = System.Drawing.SystemColors.Control;
            this.btnVideoFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVideoFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVideoFiles.Location = new System.Drawing.Point(403, 3);
            this.btnVideoFiles.Name = "btnVideoFiles";
            this.btnVideoFiles.Size = new System.Drawing.Size(124, 26);
            this.btnVideoFiles.TabIndex = 7;
            this.btnVideoFiles.Text = "My video files";
            this.btnVideoFiles.UseVisualStyleBackColor = false;
            this.btnVideoFiles.Click += new System.EventHandler(this.OnButtonVideoFilesClick);
            // 
            // btnAudioFiles
            // 
            this.btnAudioFiles.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAudioFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAudioFiles.Location = new System.Drawing.Point(533, 3);
            this.btnAudioFiles.Name = "btnAudioFiles";
            this.btnAudioFiles.Size = new System.Drawing.Size(124, 26);
            this.btnAudioFiles.TabIndex = 8;
            this.btnAudioFiles.Text = "My audio files";
            this.btnAudioFiles.UseVisualStyleBackColor = true;
            this.btnAudioFiles.Click += new System.EventHandler(this.OnButtonAudioFilesClick);
            // 
            // panelSearchResult
            // 
            this.panelSearchResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearchResult.BackColor = System.Drawing.Color.White;
            this.panelSearchResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel.SetColumnSpan(this.panelSearchResult, 2);
            this.panelSearchResult.Controls.Add(this.pictureBoxCopyPath);
            this.panelSearchResult.Controls.Add(this.pictureBoxOpenFile);
            this.panelSearchResult.Controls.Add(this.lblSearchResultTime);
            this.panelSearchResult.Controls.Add(this.lblSearchResultDescription);
            this.panelSearchResult.Controls.Add(this.lblSearchResultTitle);
            this.panelSearchResult.Location = new System.Drawing.Point(10, 39);
            this.panelSearchResult.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.panelSearchResult.Name = "panelSearchResult";
            this.panelSearchResult.Size = new System.Drawing.Size(654, 60);
            this.panelSearchResult.TabIndex = 14;
            // 
            // pictureBoxCopyPath
            // 
            this.pictureBoxCopyPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxCopyPath.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxCopyPath.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxCopyPath.Image = global::FreeYouTubeDownloader.Properties.Resources.link_icon;
            this.pictureBoxCopyPath.Location = new System.Drawing.Point(628, 31);
            this.pictureBoxCopyPath.Name = "pictureBoxCopyPath";
            this.pictureBoxCopyPath.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxCopyPath.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCopyPath.TabIndex = 5;
            this.pictureBoxCopyPath.TabStop = false;
            this.pictureBoxCopyPath.Click += new System.EventHandler(this.OnPictureBoxCopyPathClick);
            // 
            // pictureBoxOpenFile
            // 
            this.pictureBoxOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxOpenFile.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxOpenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxOpenFile.Location = new System.Drawing.Point(628, 6);
            this.pictureBoxOpenFile.Name = "pictureBoxOpenFile";
            this.pictureBoxOpenFile.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxOpenFile.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOpenFile.TabIndex = 4;
            this.pictureBoxOpenFile.TabStop = false;
            this.pictureBoxOpenFile.Click += new System.EventHandler(this.OnPictureBoxOpenFileClick);
            // 
            // lblSearchResultTime
            // 
            this.lblSearchResultTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearchResultTime.Enabled = false;
            this.lblSearchResultTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchResultTime.Location = new System.Drawing.Point(547, 8);
            this.lblSearchResultTime.Name = "lblSearchResultTime";
            this.lblSearchResultTime.Size = new System.Drawing.Size(80, 17);
            this.lblSearchResultTime.TabIndex = 3;
            this.lblSearchResultTime.Text = "00:00:00";
            this.lblSearchResultTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSearchResultDescription
            // 
            this.lblSearchResultDescription.BackColor = System.Drawing.Color.White;
            this.lblSearchResultDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchResultDescription.Location = new System.Drawing.Point(99, 32);
            this.lblSearchResultDescription.Name = "lblSearchResultDescription";
            this.lblSearchResultDescription.Size = new System.Drawing.Size(481, 17);
            this.lblSearchResultDescription.TabIndex = 2;
            this.lblSearchResultDescription.Text = "Description";
            // 
            // lblSearchResultTitle
            // 
            this.lblSearchResultTitle.BackColor = System.Drawing.Color.White;
            this.lblSearchResultTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchResultTitle.Location = new System.Drawing.Point(99, 8);
            this.lblSearchResultTitle.Name = "lblSearchResultTitle";
            this.lblSearchResultTitle.Size = new System.Drawing.Size(399, 17);
            this.lblSearchResultTitle.TabIndex = 1;
            this.lblSearchResultTitle.Text = "Title";
            // 
            // PanelContainerAutoComplete
            // 
            this.PanelContainerAutoComplete.BackColor = System.Drawing.Color.White;
            this.PanelContainerAutoComplete.Location = new System.Drawing.Point(0, 0);
            this.PanelContainerAutoComplete.Name = "PanelContainerAutoComplete";
            this.PanelContainerAutoComplete.Size = new System.Drawing.Size(149, 12);
            this.PanelContainerAutoComplete.TabIndex = 15;
            this.PanelContainerAutoComplete.Scroll += new System.Windows.Forms.ScrollEventHandler(this.OnPanelContainerAutoCompleteScroll);
            this.PanelContainerAutoComplete.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OnPanelContainerAutoCompletePreviewKeyDown);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.windowToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.versionNumberToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(674, 24);
            this.menuStrip.TabIndex = 9;
            this.menuStrip.Text = "menuStrip1";
            // 
            // windowToolStripMenuItem
            // 
            this.windowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysOnTopMenuItem,
            this.toolStripSeparator1,
            this.minimizeToolStripMenuItem,
            this.minimizeToNotificationAreaToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
            this.windowToolStripMenuItem.Size = new System.Drawing.Size(63, 20);
            this.windowToolStripMenuItem.Text = "Window";
            // 
            // alwaysOnTopMenuItem
            // 
            this.alwaysOnTopMenuItem.Name = "alwaysOnTopMenuItem";
            this.alwaysOnTopMenuItem.Size = new System.Drawing.Size(226, 22);
            this.alwaysOnTopMenuItem.Text = "Always on top";
            this.alwaysOnTopMenuItem.Click += new System.EventHandler(this.OnAlwaysOnTopMenuItemClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // minimizeToolStripMenuItem
            // 
            this.minimizeToolStripMenuItem.Image = global::FreeYouTubeDownloader.Properties.Resources.minimize_bar;
            this.minimizeToolStripMenuItem.Name = "minimizeToolStripMenuItem";
            this.minimizeToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.minimizeToolStripMenuItem.Text = "Minimize";
            this.minimizeToolStripMenuItem.Click += new System.EventHandler(this.OnMinimizeMenuItemClick);
            // 
            // minimizeToNotificationAreaToolStripMenuItem
            // 
            this.minimizeToNotificationAreaToolStripMenuItem.Image = global::FreeYouTubeDownloader.Properties.Resources.minimize_tray;
            this.minimizeToNotificationAreaToolStripMenuItem.Name = "minimizeToNotificationAreaToolStripMenuItem";
            this.minimizeToNotificationAreaToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.minimizeToNotificationAreaToolStripMenuItem.Text = "Minimize to notification area";
            this.minimizeToNotificationAreaToolStripMenuItem.Click += new System.EventHandler(this.OnMinimizeToTrayMenuItemClick);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::FreeYouTubeDownloader.Properties.Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.OnExitMenuItemClick);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.preferencesToolStripMenuItem,
            this.toolStripSeparator2,
            this.askFileNameAndFolderToolStripMenuItem,
            this.compatibleURLNotificationToolStripMenuItem,
            this.autoDownloadMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Image = global::FreeYouTubeDownloader.Properties.Resources.gears_preferences;
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.OnPreferencesMenuItemClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(221, 6);
            // 
            // askFileNameAndFolderToolStripMenuItem
            // 
            this.askFileNameAndFolderToolStripMenuItem.Name = "askFileNameAndFolderToolStripMenuItem";
            this.askFileNameAndFolderToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.askFileNameAndFolderToolStripMenuItem.Text = "Ask file name and folder";
            this.askFileNameAndFolderToolStripMenuItem.Click += new System.EventHandler(this.OnAskFileNameAndFolderpMenuItemClick);
            // 
            // compatibleURLNotificationToolStripMenuItem
            // 
            this.compatibleURLNotificationToolStripMenuItem.Checked = true;
            this.compatibleURLNotificationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.compatibleURLNotificationToolStripMenuItem.Name = "compatibleURLNotificationToolStripMenuItem";
            this.compatibleURLNotificationToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.compatibleURLNotificationToolStripMenuItem.Text = "Compatible URL notification";
            this.compatibleURLNotificationToolStripMenuItem.Click += new System.EventHandler(this.OnCompatibleURLNotificationCheckedChanged);
            // 
            // autoDownloadMenuItem
            // 
            this.autoDownloadMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.videoAutoDownloadMenuItem,
            this.audioAutoDownloadMenuItem});
            this.autoDownloadMenuItem.Name = "autoDownloadMenuItem";
            this.autoDownloadMenuItem.Size = new System.Drawing.Size(224, 22);
            this.autoDownloadMenuItem.Text = "Auto-download videos";
            // 
            // videoAutoDownloadMenuItem
            // 
            this.videoAutoDownloadMenuItem.CheckOnClick = true;
            this.videoAutoDownloadMenuItem.Name = "videoAutoDownloadMenuItem";
            this.videoAutoDownloadMenuItem.Size = new System.Drawing.Size(106, 22);
            this.videoAutoDownloadMenuItem.Text = "Video";
            this.videoAutoDownloadMenuItem.Click += new System.EventHandler(this.OnAutoDownloadVideoMenuItemClick);
            // 
            // audioAutoDownloadMenuItem
            // 
            this.audioAutoDownloadMenuItem.CheckOnClick = true;
            this.audioAutoDownloadMenuItem.Name = "audioAutoDownloadMenuItem";
            this.audioAutoDownloadMenuItem.Size = new System.Drawing.Size(106, 22);
            this.audioAutoDownloadMenuItem.Text = "Audio";
            this.audioAutoDownloadMenuItem.Click += new System.EventHandler(this.OnAutoDownloadAudioMenuItemClick);
            // 
            // versionNumberToolStripMenuItem
            // 
            this.versionNumberToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.versionNumberToolStripMenuItem.Name = "versionNumberToolStripMenuItem";
            this.versionNumberToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.versionNumberToolStripMenuItem.Text = "4.0.0.0";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(207, 6);
            // 
            // PanelWrapperContainerAutoComplete
            // 
            this.PanelWrapperContainerAutoComplete.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PanelWrapperContainerAutoComplete.Controls.Add(this.PanelContainerAutoComplete);
            this.PanelWrapperContainerAutoComplete.Controls.Add(this.progressBarLoadingData);
            this.PanelWrapperContainerAutoComplete.Location = new System.Drawing.Point(10, 53);
            this.PanelWrapperContainerAutoComplete.Name = "PanelWrapperContainerAutoComplete";
            this.PanelWrapperContainerAutoComplete.Size = new System.Drawing.Size(149, 12);
            this.PanelWrapperContainerAutoComplete.TabIndex = 0;
            // 
            // progressBarLoadingData
            // 
            this.progressBarLoadingData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarLoadingData.Location = new System.Drawing.Point(0, 12);
            this.progressBarLoadingData.MarqueeAnimationSpeed = 10;
            this.progressBarLoadingData.Name = "progressBarLoadingData";
            this.progressBarLoadingData.Size = new System.Drawing.Size(149, 3);
            this.progressBarLoadingData.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarLoadingData.TabIndex = 0;
            this.progressBarLoadingData.Visible = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(254)))), ((int)(((byte)(254)))), ((int)(((byte)(254)))));
            this.ClientSize = new System.Drawing.Size(674, 490);
            this.Controls.Add(this.PanelWrapperContainerAutoComplete);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(604, 393);
            this.Name = "MainWindow";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Free YouTube Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnMainWindowLoad);
            this.Resize += new System.EventHandler(this.OnMainWindowResize);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDownloads)).EndInit();
            this.tableLayoutPanelActionButtons.ResumeLayout(false);
            this.tableLayoutPanelActionButtonsContainer.ResumeLayout(false);
            this.panelButtonActionVideo.ResumeLayout(false);
            this.panelButtonActionVideo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxVideo)).EndInit();
            this.panelButtonActionAudio.ResumeLayout(false);
            this.panelButtonActionAudio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAudio)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).EndInit();
            this.panelSearchResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCopyPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOpenFile)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.PanelWrapperContainerAutoComplete.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
