// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.SearchException
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;

namespace FreeYouTubeDownloader.Search
{
  public class SearchException : ApplicationException
  {
    public string Url { get; private set; }

    public string ResponseContent { get; private set; }

    public SearchException(string message, string url = null, string responseContent = null)
      : base(message)
    {
      this.Url = url;
      this.ResponseContent = responseContent;
    }

    public SearchException(string message, Exception innerException = null, string url = null, string responseContent = null)
      : base(message, innerException)
    {
      this.Url = url;
      this.ResponseContent = responseContent;
    }
  }
}
