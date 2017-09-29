// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Model.DownloadStateEventArgs
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Downloader;
using System;

namespace FreeYouTubeDownloader.Model
{
  public class DownloadStateEventArgs : EventArgs
  {
    public DownloadState State { get; set; }

    public DownloadStateEventArgs(DownloadState state)
    {
      this.State = state;
    }
  }
}
