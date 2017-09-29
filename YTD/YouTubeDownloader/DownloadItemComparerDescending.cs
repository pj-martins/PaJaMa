// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.DownloadItemComparerDescending
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using BrightIdeasSoftware;
using FreeYouTubeDownloader.Model;
using System.Collections.Generic;

namespace FreeYouTubeDownloader
{
  public class DownloadItemComparerDescending : IComparer<OLVListItem>
  {
    public int Compare(OLVListItem x, OLVListItem y)
    {
      if (((DownloadItem) x.RowObject).TimeStamp > ((DownloadItem) y.RowObject).TimeStamp)
        return -1;
      return ((DownloadItem) x.RowObject).TimeStamp < ((DownloadItem) y.RowObject).TimeStamp ? 1 : 0;
    }
  }
}
