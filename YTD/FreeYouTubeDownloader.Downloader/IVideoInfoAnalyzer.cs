// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.IVideoInfoAnalyzer
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;

namespace FreeYouTubeDownloader.Downloader
{
  internal interface IVideoInfoAnalyzer
  {
    VideoInfo ParseVideoInfo(string videoInfoContent);

    void VideoInfoContentReceived(string videoInfoContent, string pageData, Action<MediaInfo> callback);

    void VideoInfoContentReceived(string videoInfoContent, string pageData, Action<MediaInfo> callback, object userState);

    void ParseVideoInfo(Uri videoInfoUrl, string pageData, Action<MediaInfo> callback);

    void ParseVideoInfo(Uri videoInfoUrl, string pageData, Action<MediaInfo> callback, object userState);
  }
}
