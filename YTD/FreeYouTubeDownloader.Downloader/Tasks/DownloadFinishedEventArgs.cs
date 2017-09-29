// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Tasks.DownloadFinishedEventArgs
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;

namespace FreeYouTubeDownloader.Downloader.Tasks
{
  public class DownloadFinishedEventArgs : EventArgs
  {
    public DownloadState DownloadState { get; private set; }

    public bool IsDownloadCompleted
    {
      get
      {
        return this.DownloadState == DownloadState.Completed;
      }
    }

    public DownloadFinishedEventArgs()
    {
    }

    public DownloadFinishedEventArgs(DownloadState downloadState)
      : this()
    {
      this.DownloadState = downloadState;
    }
  }
}
