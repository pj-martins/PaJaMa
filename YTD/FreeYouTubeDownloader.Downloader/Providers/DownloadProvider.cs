// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Providers.DownloadProvider
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using System;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Downloader.Providers
{
    public abstract class DownloadProvider
    {
        public abstract string UrlPattern { get; }

        public abstract void ReceiveMediaInfo(string url, string pageData, Action<MediaInfo> callback);

        public virtual void ReceiveMediaInfo(string url, Action<MediaInfo> callback, MediaInfo mediaInfo = null)
        {
            HttpUtil.Instance.GetData(new UriBuilder(url).Uri, (Action<string>)(content => this.ReceiveMediaInfo(url, content, callback)), "GET", (Dictionary<string, object>)null);
        }

        public abstract void ReceiveMediaInfoFromServerSide(string url, Action<MediaInfo> callback);

        protected virtual string GetMediaTitle(string pageData)
        {
            if (string.IsNullOrEmpty(pageData))
                throw new ArgumentNullException("pageData");
            return pageData.JustAfter("<title>", "</title>");
        }

        public static DownloadProvider GetDownloadProviderFromDomain(string url)
        {
            if (url.Contains("youtube.com/") || url.Contains("youtu.be/"))
                return (DownloadProvider)new YouTubeDownloadProvider();
            return (DownloadProvider)null;
        }
    }
}
