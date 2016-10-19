namespace PaJaMa.ScreenCapture
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
			this.mnuNotification = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.captureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.captureFullScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openWithToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lstMain = new System.Windows.Forms.ListView();
			this.ilMain = new System.Windows.Forms.ImageList(this.components);
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.ucImageEditor = new PaJaMa.ScreenCapture.ImageEditor.ucImageEditor();
			this.panel1 = new System.Windows.Forms.Panel();
			this.mnuNotification.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// notifyIcon1
			// 
			this.notifyIcon1.ContextMenuStrip = this.mnuNotification;
			this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			this.notifyIcon1.Text = "Capture";
			this.notifyIcon1.Visible = true;
			this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
			// 
			// mnuNotification
			// 
			this.mnuNotification.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureToolStripMenuItem,
            this.captureFullScreenToolStripMenuItem,
            this.exitToolStripMenuItem});
			this.mnuNotification.Name = "contextMenuStrip2";
			this.mnuNotification.Size = new System.Drawing.Size(177, 70);
			// 
			// captureToolStripMenuItem
			// 
			this.captureToolStripMenuItem.Name = "captureToolStripMenuItem";
			this.captureToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.captureToolStripMenuItem.Text = "Capture";
			this.captureToolStripMenuItem.Click += new System.EventHandler(this.captureToolStripMenuItem_Click);
			// 
			// captureFullScreenToolStripMenuItem
			// 
			this.captureFullScreenToolStripMenuItem.Name = "captureFullScreenToolStripMenuItem";
			this.captureFullScreenToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.captureFullScreenToolStripMenuItem.Text = "Capture Full Screen";
			this.captureFullScreenToolStripMenuItem.Click += new System.EventHandler(this.captureFullScreenToolStripMenuItem_Click);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.openWithToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.showInExplorerToolStripMenuItem,
            this.deleteToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(162, 114);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// openWithToolStripMenuItem
			// 
			this.openWithToolStripMenuItem.Name = "openWithToolStripMenuItem";
			this.openWithToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.openWithToolStripMenuItem.Text = "Open &With";
			// 
			// copyToolStripMenuItem
			// 
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
			// 
			// showInExplorerToolStripMenuItem
			// 
			this.showInExplorerToolStripMenuItem.Name = "showInExplorerToolStripMenuItem";
			this.showInExplorerToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.showInExplorerToolStripMenuItem.Text = "&Show In Explorer";
			this.showInExplorerToolStripMenuItem.Click += new System.EventHandler(this.showInExplorerToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.deleteToolStripMenuItem.Text = "&Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// lstMain
			// 
			this.lstMain.Alignment = System.Windows.Forms.ListViewAlignment.Left;
			this.lstMain.BackColor = System.Drawing.SystemColors.Control;
			this.lstMain.ContextMenuStrip = this.contextMenuStrip1;
			this.lstMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lstMain.HideSelection = false;
			this.lstMain.LargeImageList = this.ilMain;
			this.lstMain.Location = new System.Drawing.Point(0, 0);
			this.lstMain.Name = "lstMain";
			this.lstMain.Size = new System.Drawing.Size(785, 127);
			this.lstMain.TabIndex = 3;
			this.lstMain.UseCompatibleStateImageBehavior = false;
			this.lstMain.SelectedIndexChanged += new System.EventHandler(this.lstMain_SelectedIndexChanged);
			this.lstMain.DoubleClick += new System.EventHandler(this.lstMain_DoubleClick);
			this.lstMain.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstMain_KeyDown);
			// 
			// ilMain
			// 
			this.ilMain.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.ilMain.ImageSize = new System.Drawing.Size(128, 128);
			this.ilMain.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.AutoScroll = true;
			this.splitMain.Panel1.Controls.Add(this.panel1);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.lstMain);
			this.splitMain.Size = new System.Drawing.Size(785, 431);
			this.splitMain.SplitterDistance = 300;
			this.splitMain.TabIndex = 5;
			// 
			// ucImageEditor
			// 
			this.ucImageEditor.CurrentColor = System.Drawing.Color.Blue;
			this.ucImageEditor.CurrentFont = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.ucImageEditor.CurrentRadius = 8;
			this.ucImageEditor.CurrentWidth = 3F;
			this.ucImageEditor.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucImageEditor.DrawColor = System.Drawing.Color.Empty;
			this.ucImageEditor.DrawWidth = 3F;
			this.ucImageEditor.EraseColor = System.Drawing.Color.Empty;
			this.ucImageEditor.EraseWidth = 0F;
			this.ucImageEditor.FileName = null;
			this.ucImageEditor.HighlightColor = System.Drawing.Color.Empty;
			this.ucImageEditor.HighlightWidth = 0F;
			this.ucImageEditor.Location = new System.Drawing.Point(0, 0);
			this.ucImageEditor.Name = "ucImageEditor";
			this.ucImageEditor.Size = new System.Drawing.Size(785, 300);
			this.ucImageEditor.TabIndex = 4;
			this.ucImageEditor.ImageDoubleClick += new System.EventHandler(this.ucImageEditor_ImageDoubleClick);
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.ucImageEditor);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(785, 300);
			this.panel1.TabIndex = 5;
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(785, 431);
			this.Controls.Add(this.splitMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Capture";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.Resize += new System.EventHandler(this.frmMain_Resize);
			this.mnuNotification.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NotifyIcon notifyIcon1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip mnuNotification;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem captureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem captureFullScreenToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showInExplorerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openWithToolStripMenuItem;
		private System.Windows.Forms.ListView lstMain;
		private System.Windows.Forms.ImageList ilMain;
		private ImageEditor.ucImageEditor ucImageEditor;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.Panel panel1;
	}
}

