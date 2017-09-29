// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Controls.WebBrowserExNewWindow3EventArgs
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;

namespace FreeYouTubeDownloader.Controls
{
  public class WebBrowserExNewWindow3EventArgs : EventArgs
  {
    public bool Cancel { get; set; }

    public uint Flags { get; set; }

    public string Url { get; set; }

    public string UrlContext { get; set; }

    public WebBrowserExNewWindow3EventArgs(string url, uint flags, string urlContext, bool cancel)
    {
      this.Url = url;
      this.Flags = flags;
      this.UrlContext = urlContext;
      this.Cancel = cancel;
    }
  }
}
