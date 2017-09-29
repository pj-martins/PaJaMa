// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.PlayerWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Controls;
using FreeYouTubeDownloader.License;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public sealed class PlayerWindow : Form
  {
    private const string UrlFormat = "https://youtubedownloader.com/wd/player/player.php?url={0}&mime={1}&thumb={2}&version={3}&isEncrypted=ZmFsc2U=";
    private readonly string _streamUrl;
    private readonly string _mimeType;
    private readonly string _thumbnailUrl;
    private int _oldWidth;
    private int _oldHeight;
    private IContainer components;
    private WebBrowserEx webBrowser;

    public static bool IsOpened { get; private set; }

    public PlayerWindow(string title, string streamUrl, string mimeType, string thumbnailUrl)
    {
      this.InitializeComponent();
      this.Text = title;
      this.Size = new Size(DpiHelper.LogicalToDeviceUnitsX(700), DpiHelper.LogicalToDeviceUnitsY(423));
      this._streamUrl = streamUrl;
      this._mimeType = mimeType;
      this._thumbnailUrl = thumbnailUrl;
    }

    public string Base64Encode(string plainText)
    {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
    }

    private void OnPlayerWindowLoad(object sender, EventArgs e)
    {
      this.webBrowser.Navigate(string.Format("https://youtubedownloader.com/wd/player/player.php?url={0}&mime={1}&thumb={2}&version={3}&isEncrypted=ZmFsc2U=", (object) Uri.EscapeDataString(this._streamUrl), (object) this.Base64Encode(this._mimeType), (object) this.Base64Encode(this._thumbnailUrl), (object) this.Base64Encode(LicenseHelper.IsGenuine ? "pro" : "free")));
    }

    private void PlayerWindow_ResizeEnd(object sender, EventArgs e)
    {
      this.ClientSize = new Size(this.ClientSize.Width, this.CalculateNewHeight(this.ClientSize.Width, this._oldWidth, this._oldHeight));
    }

    private void PlayerWindow_ResizeBegin(object sender, EventArgs e)
    {
      this._oldWidth = this.ClientSize.Width;
      this._oldHeight = this.ClientSize.Height;
    }

    private int CalculateNewHeight(int newWidth, int oldWidth, int oldHeight)
    {
      return oldHeight * newWidth / oldWidth;
    }

    protected override void OnShown(EventArgs e)
    {
      PlayerWindow.IsOpened = true;
      base.OnShown(e);
    }

    protected override void OnClosed(EventArgs e)
    {
      PlayerWindow.IsOpened = false;
      base.OnClosed(e);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.webBrowser = new WebBrowserEx();
      this.SuspendLayout();
      this.webBrowser.Dock = DockStyle.Fill;
      this.webBrowser.Location = new Point(0, 0);
      this.webBrowser.MinimumSize = new Size(20, 20);
      this.webBrowser.Name = "webBrowser";
      this.webBrowser.ScriptErrorsSuppressed = true;
      this.webBrowser.Size = new Size(698, 386);
      this.webBrowser.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(698, 386);
      this.Controls.Add((Control) this.webBrowser);
      this.Name = "PlayerWindow";
      this.ShowIcon = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "PlayerWindow";
      this.Load += new EventHandler(this.OnPlayerWindowLoad);
      this.ResizeBegin += new EventHandler(this.PlayerWindow_ResizeBegin);
      this.ResizeEnd += new EventHandler(this.PlayerWindow_ResizeEnd);
      this.ResumeLayout(false);
    }
  }
}
