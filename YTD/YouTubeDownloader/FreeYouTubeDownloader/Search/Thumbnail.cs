// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.Thumbnail
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

namespace FreeYouTubeDownloader.Search
{
  public class Thumbnail
  {
    public string Url { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public override string ToString()
    {
      return string.Format("{0}x{1}", (object) this.Width, (object) this.Height);
    }
  }
}
