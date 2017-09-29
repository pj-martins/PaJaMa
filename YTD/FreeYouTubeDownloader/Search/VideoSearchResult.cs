// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.VideoSearchResult
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Search
{
  public class VideoSearchResult
  {
    public bool HasHD { get; set; }

    public DateTime Uploaded { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Uploader { get; set; }

    public string Url { get; set; }

    public string Id { get; set; }

    public TimeSpan Duration { get; set; }

    public IList<Thumbnail> Thumbnails { get; set; }

    public override string ToString()
    {
      return this.Title;
    }
  }
}
