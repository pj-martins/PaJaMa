// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Controls.WebBrowserExNavigateErrorEventArgs
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;

namespace FreeYouTubeDownloader.Controls
{
  public class WebBrowserExNavigateErrorEventArgs : EventArgs
  {
    public int StatusCode { get; set; }

    public bool Cancel { get; set; }

    public string Url { get; set; }

    public string Frame { get; set; }

    public WebBrowserExNavigateErrorEventArgs(string url, string frame, int statusCode, bool cancel)
    {
      this.Url = url;
      this.Frame = frame;
      this.StatusCode = statusCode;
      this.Cancel = cancel;
    }
  }
}
