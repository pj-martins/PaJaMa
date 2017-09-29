// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Tasks.DownloadProgressEventArgs
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;

namespace FreeYouTubeDownloader.Downloader.Tasks
{
  public class DownloadProgressEventArgs : EventArgs
  {
    private readonly long _bytesDownloaded;
    private readonly long _bytesTotal;
    private readonly long _downloadSpeed;
    private readonly long _secondsRemains;

    public long BytesDownloaded
    {
      get
      {
        return this._bytesDownloaded;
      }
    }

    public long BytesTotal
    {
      get
      {
        return this._bytesTotal;
      }
    }

    public long DownloadSpeed
    {
      get
      {
        return this._downloadSpeed;
      }
    }

    public long SecondsRemains
    {
      get
      {
        return this._secondsRemains;
      }
    }

    public int ProgressInPercent { get; private set; }

    public DownloadProgressEventArgs(long bytesDownloaded, long bytesTotal, long downloadSpeed, long secondsRemains)
    {
      this._bytesDownloaded = bytesDownloaded;
      this._bytesTotal = bytesTotal;
      this._downloadSpeed = downloadSpeed;
      this._secondsRemains = secondsRemains;
      if (bytesTotal <= 100L)
        return;
      this.ProgressInPercent = (int) (bytesDownloaded / (bytesTotal / 100L));
    }

    public DownloadProgressEventArgs(int progressInPercent, long downloadSpeed, long secondsRemains)
    {
      this.ProgressInPercent = progressInPercent;
      this._downloadSpeed = downloadSpeed;
      this._secondsRemains = secondsRemains;
    }
  }
}
