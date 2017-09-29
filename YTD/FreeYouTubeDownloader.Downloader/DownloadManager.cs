// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.DownloadManager
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Downloader.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Downloader
{
  internal static class DownloadManager
  {
    private static readonly List<DownloadProvider> Providers = new List<DownloadProvider>()
    {
      (DownloadProvider) new YouTubeDownloadProvider()
    };

    internal static DownloadProvider GetProviderForUrl(string url)
    {
      return DownloadManager.Providers.SingleOrDefault<DownloadProvider>((Func<DownloadProvider, bool>) (provider => Regex.IsMatch(url, provider.UrlPattern)));
    }

    internal static DownloadProvider GetProviderByProviderType(Type providerType)
    {
      return DownloadManager.Providers.SingleOrDefault<DownloadProvider>((Func<DownloadProvider, bool>) (provider => provider.GetType() == providerType));
    }

    internal static bool HasProviderForUrl(string url)
    {
      if (!string.IsNullOrWhiteSpace(url))
        return DownloadManager.Providers.Any<DownloadProvider>((Func<DownloadProvider, bool>) (provider => Regex.IsMatch(url, provider.UrlPattern)));
      return false;
    }
  }
}
