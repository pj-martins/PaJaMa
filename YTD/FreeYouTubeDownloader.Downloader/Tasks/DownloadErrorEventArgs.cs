// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Tasks.DownloadErrorEventArgs
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;

namespace FreeYouTubeDownloader.Downloader.Tasks
{
  public class DownloadErrorEventArgs : EventArgs
  {
    public DownloadError DownloadError { get; private set; }

    public Exception Exception { get; private set; }

    public DownloadErrorEventArgs(DownloadError downloadError, Exception exception = null)
    {
      this.DownloadError = downloadError;
      this.Exception = exception;
    }
  }
}
