// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.VideoSearch
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeYouTubeDownloader.Search
{
  public abstract class VideoSearch
  {
    public virtual uint MaxResults { get; set; }

    public virtual string Query { get; set; }

    public SearchResultsOrder Order { get; set; }

    public SearchVideoDefinitions Definition { get; set; }

    protected VideoSearch()
    {
      this.Order = SearchResultsOrder.Relevance;
      this.Definition = SearchVideoDefinitions.Any;
      this.MaxResults = 10U;
    }

    public abstract List<VideoSearchResult> FetchResultSet();

    public abstract Task<List<VideoSearchResult>> FetchResultSetAsync();
  }
}
