// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Controls.WebBrowserExBeforeNavigate2EventArgs
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;

namespace FreeYouTubeDownloader.Controls
{
  public class WebBrowserExBeforeNavigate2EventArgs : EventArgs
  {
    public bool Cancel { get; set; }

    public string Url { get; set; }

    public string TargetFrameName { get; set; }

    public string PostData { get; set; }

    public object Flags { get; set; }

    public object Headers { get; set; }

    public WebBrowserExBeforeNavigate2EventArgs(string url, object flags, string targetFrameName, string postData, object headers, bool cancel)
    {
      this.Url = url;
      this.Flags = flags;
      this.TargetFrameName = targetFrameName;
      this.PostData = postData;
      this.Headers = headers;
      this.Cancel = cancel;
    }
  }
}
