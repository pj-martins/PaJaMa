// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.YouTubeSearchException
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using Newtonsoft.Json.Linq;

namespace FreeYouTubeDownloader.Search
{
  public class YouTubeSearchException : SearchException
  {
    public JObject JsonResponse { get; private set; }

    public YouTubeSearchException(string message, string url = null, string responseContent = null, JObject jsonResponse = null)
      : base(message, url, responseContent)
    {
      this.JsonResponse = jsonResponse;
    }
  }
}
