// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.DownloadState
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

namespace FreeYouTubeDownloader.Downloader
{
  public enum DownloadState
  {
    Unknown,
    Preparing,
    Waiting,
    Downloading,
    Completed,
    Error,
    InQueue,
    Paused,
    Converting,
    Canceled,
    AnalizeSource,
    Merging,
  }
}
