// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Youtube.YoutubeVideoRecord
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Drawing;

namespace FreeYouTubeDownloader.Youtube
{
  public class YoutubeVideoRecord
  {
    public string Id { get; set; }

    public string Url { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public double Duration { get; set; }

    public string ThumbnailUrl { get; set; }

    public Image Thumbnail { get; set; }

    public YoutubeVideoRecord(string id, string url, string title, string description, double duration, string thumbnailUrl, Image thumbnail)
    {
      this.Id = id;
      this.Url = url;
      this.Title = title;
      this.Description = description;
      this.Duration = duration;
      this.ThumbnailUrl = thumbnailUrl;
      this.Thumbnail = thumbnail;
    }
  }
}
