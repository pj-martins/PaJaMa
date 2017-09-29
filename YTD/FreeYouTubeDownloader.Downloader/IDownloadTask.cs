﻿// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.IDownloadTask
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System.Collections.Generic;

namespace FreeYouTubeDownloader.Downloader
{
  internal interface IDownloadTask
  {
    string Id { get; }

    void Download(MediaLink link, string downloadedMediaFileLocation, long range = -1);

    void MultifileDownload(string downloadedMediaFileLocation, List<MultifileDownloadParameter> multifileDownloadParameters);

    void Abort();

    void Pause();

    void Resume();
  }
}