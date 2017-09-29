﻿// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.VideoStreamQualityDistinctComparer
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System.Collections.Generic;

namespace FreeYouTubeDownloader.Downloader
{
  internal class VideoStreamQualityDistinctComparer : IEqualityComparer<VideoLink>
  {
    public bool Equals(VideoLink x, VideoLink y)
    {
      return x.VideoStreamQuality == y.VideoStreamQuality;
    }

    public int GetHashCode(VideoLink obj)
    {
      return obj.VideoStreamQuality.GetHashCode();
    }
  }
}
