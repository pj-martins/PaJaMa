// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.IFrameWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public class IFrameWindow : Form
  {
    private bool _panelCaptionMousePressed;
    private int _windowLocationX;
    private int _windowLocationY;
    private IContainer components;
    private PictureBox pictureBoxCloseWindow;
    private PictureBox pictureBoxMinimizeWindow;
    private WebBrowser webBrowser;
    private Label lblPleaseWait;
    private Panel panelCaption;

    public string Url { get; private set; }

    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams createParams = base.CreateParams;
        int num = createParams.ClassStyle | 131072;
        createParams.ClassStyle = num;
        return createParams;
      }
    }

    public IFrameWindow()
    {
      this.InitializeComponent();
      this.InitializeWindow();
      this.InitializeControls();
    }

    public IFrameWindow(string url)
      : this()
    {
      this.Url = url;
    }

    private void InitializeWindow()
    {
      this.BackColor = Color.FromArgb(248, 248, 248);
    }

    private void InitializeControls()
    {
      this.Text = Strings.SubscribeForUpdates;
      this.webBrowser.Visible = false;
      this.lblPleaseWait.Text = Strings.PleaseWait;
      this.lblPleaseWait.Location = new Point((this.Width - this.lblPleaseWait.Width) / 2 + 3, this.lblPleaseWait.Location.Y);
    }

    private void OnMinimizeWindowClick(object sender, EventArgs e)
    {
      this.WindowState = FormWindowState.Minimized;
    }

    private void OnCloseWindowClick(object sender, EventArgs e)
    {
      this.Close();
    }

    private void OnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
    {
      this.lblPleaseWait.Visible = false;
      this.webBrowser.Visible = true;
    }

    private void OnCaptionMouseDown(object sender, MouseEventArgs e)
    {
      this._panelCaptionMousePressed = true;
      Point screen = this.PointToScreen(e.Location);
      this._windowLocationX = screen.X;
      this._windowLocationY = screen.Y;
    }

    private void OnCaptionMouseUp(object sender, MouseEventArgs e)
    {
      this._panelCaptionMousePressed = false;
    }

    private void OnCaptionMouseMove(object sender, MouseEventArgs e)
    {
      if (!this._panelCaptionMousePressed || this.WindowState == FormWindowState.Maximized)
        return;
      Point screen = this.PointToScreen(e.Location);
      this.Location = new Point(this.Location.X + (screen.X - this._windowLocationX), this.Location.Y + (screen.Y - this._windowLocationY));
      this._windowLocationX = screen.X;
      this._windowLocationY = screen.Y;
    }

    private void OnComboBoxFirstWindowMouseEnter(object sender, EventArgs e)
    {
      ((Control) sender).BackColor = Color.FromArgb(50, Color.Gray);
    }

    private void OnIFrameWindowLoad(object sender, EventArgs e)
    {
      this.webBrowser.Navigate(this.Url);
    }

    private void OnComboBoxFirstWindowMouseLeave(object sender, EventArgs e)
    {
      ((Control) sender).BackColor = Color.Transparent;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pictureBoxCloseWindow = new PictureBox();
      this.pictureBoxMinimizeWindow = new PictureBox();
      this.webBrowser = new WebBrowser();
      this.lblPleaseWait = new Label();
      this.panelCaption = new Panel();
      ((ISupportInitialize) this.pictureBoxCloseWindow).BeginInit();
      ((ISupportInitialize) this.pictureBoxMinimizeWindow).BeginInit();
      this.SuspendLayout();
      this.pictureBoxCloseWindow.BackColor = Color.Transparent;
      this.pictureBoxCloseWindow.Image = (Image) Resources.firstwindow_close;
      this.pictureBoxCloseWindow.Location = new Point(565, 2);
      this.pictureBoxCloseWindow.Margin = new Padding(2);
      this.pictureBoxCloseWindow.Name = "pictureBoxCloseWindow";
      this.pictureBoxCloseWindow.Size = new Size(23, 23);
      this.pictureBoxCloseWindow.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBoxCloseWindow.TabIndex = 0;
      this.pictureBoxCloseWindow.TabStop = false;
      this.pictureBoxCloseWindow.Click += new EventHandler(this.OnCloseWindowClick);
      this.pictureBoxCloseWindow.MouseEnter += new EventHandler(this.OnComboBoxFirstWindowMouseEnter);
      this.pictureBoxCloseWindow.MouseLeave += new EventHandler(this.OnComboBoxFirstWindowMouseLeave);
      this.pictureBoxMinimizeWindow.BackColor = Color.Transparent;
      this.pictureBoxMinimizeWindow.Image = (Image) Resources.firstwindow_minimize;
      this.pictureBoxMinimizeWindow.Location = new Point(538, 2);
      this.pictureBoxMinimizeWindow.Margin = new Padding(2);
      this.pictureBoxMinimizeWindow.Name = "pictureBoxMinimizeWindow";
      this.pictureBoxMinimizeWindow.Size = new Size(23, 23);
      this.pictureBoxMinimizeWindow.SizeMode = PictureBoxSizeMode.CenterImage;
      this.pictureBoxMinimizeWindow.TabIndex = 1;
      this.pictureBoxMinimizeWindow.TabStop = false;
      this.pictureBoxMinimizeWindow.Click += new EventHandler(this.OnMinimizeWindowClick);
      this.pictureBoxMinimizeWindow.MouseEnter += new EventHandler(this.OnComboBoxFirstWindowMouseEnter);
      this.pictureBoxMinimizeWindow.MouseLeave += new EventHandler(this.OnComboBoxFirstWindowMouseLeave);
      this.webBrowser.Location = new Point(8, 27);
      this.webBrowser.Margin = new Padding(2);
      this.webBrowser.MinimumSize = new Size(15, 16);
      this.webBrowser.Name = "webBrowser";
      this.webBrowser.ScrollBarsEnabled = false;
      this.webBrowser.Size = new Size(579, 304);
      this.webBrowser.TabIndex = 2;
      this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.OnDocumentCompleted);
      this.lblPleaseWait.AutoSize = true;
      this.lblPleaseWait.Font = new Font("Arial", 35f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.lblPleaseWait.ForeColor = Color.FromArgb(151, 151, 151);
      this.lblPleaseWait.Location = new Point(85, 130);
      this.lblPleaseWait.Name = "lblPleaseWait";
      this.lblPleaseWait.Size = new Size(366, 53);
      this.lblPleaseWait.TabIndex = 3;
      this.lblPleaseWait.Text = "PLEASE WAIT...";
      this.panelCaption.Location = new Point(1, 2);
      this.panelCaption.Name = "panelCaption";
      this.panelCaption.Size = new Size(533, 23);
      this.panelCaption.TabIndex = 4;
      this.panelCaption.MouseDown += new MouseEventHandler(this.OnCaptionMouseDown);
      this.panelCaption.MouseMove += new MouseEventHandler(this.OnCaptionMouseMove);
      this.panelCaption.MouseUp += new MouseEventHandler(this.OnCaptionMouseUp);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Window;
      this.BackgroundImageLayout = ImageLayout.Stretch;
      this.ClientSize = new Size(595, 339);
      this.Controls.Add((Control) this.panelCaption);
      this.Controls.Add((Control) this.webBrowser);
      this.Controls.Add((Control) this.lblPleaseWait);
      this.Controls.Add((Control) this.pictureBoxMinimizeWindow);
      this.Controls.Add((Control) this.pictureBoxCloseWindow);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Margin = new Padding(2);
      this.MaximizeBox = false;
      this.Name = "IFrameWindow";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Welcome to ClipClip";
      this.Load += new EventHandler(this.OnIFrameWindowLoad);
      ((ISupportInitialize) this.pictureBoxCloseWindow).EndInit();
      ((ISupportInitialize) this.pictureBoxMinimizeWindow).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
