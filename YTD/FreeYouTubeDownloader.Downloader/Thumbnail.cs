﻿// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Thumbnail
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

namespace FreeYouTubeDownloader.Downloader
{
  public class Thumbnail
  {
    public string Url;
    public int Width;
    public int Height;

    public Thumbnail(string url, int width, int height)
    {
      this.Url = url;
      this.Width = width;
      this.Height = height;
    }
  }
}
