// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.MediaLinksExtension
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeYouTubeDownloader.Downloader
{
  public static class MediaLinksExtension
  {
    internal static VideoLink GetVideoLink(this IMediaLink[] links, VideoStreamType streamType, VideoQuality videoQuality)
    {
      return ((IEnumerable<IMediaLink>) links).Where<IMediaLink>((Func<IMediaLink, bool>) (link => link.IsVideoLink)).Cast<VideoLink>().OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>) (link => link.VideoStreamQuality)).FirstOrDefault<VideoLink>((Func<VideoLink, bool>) (link =>
      {
        if (link.StreamType == streamType)
          return link.VideoStreamQuality <= videoQuality;
        return false;
      }));
    }

    internal static VideoLink GetVideoLink(this IMediaLink[] links, VideoQuality videoQuality)
    {
      return ((IEnumerable<IMediaLink>) links).Where<IMediaLink>((Func<IMediaLink, bool>) (link => link.IsVideoLink)).Cast<VideoLink>().OrderByDescending<VideoLink, VideoQuality>((Func<VideoLink, VideoQuality>) (link => link.VideoStreamQuality)).FirstOrDefault<VideoLink>((Func<VideoLink, bool>) (link => link.VideoStreamQuality <= videoQuality));
    }

    internal static AudioLink GetAudioLink(this IMediaLink[] links, AudioQuality preferredQuality, IEnumerable<AudioStreamType> preferredStreamTypes)
    {
      IOrderedEnumerable<AudioLink> source = ((IEnumerable<IMediaLink>) links).Where<IMediaLink>((Func<IMediaLink, bool>) (l => l.IsAudioLink)).Cast<AudioLink>().OrderByDescending<AudioLink, AudioQuality>((Func<AudioLink, AudioQuality>) (l => l.AudioStreamQuality));
      foreach (AudioStreamType preferredStreamType1 in preferredStreamTypes)
      {
        AudioStreamType preferredStreamType = preferredStreamType1;
        AudioLink audioLink = source.FirstOrDefault<AudioLink>((Func<AudioLink, bool>) (l => l.StreamType == preferredStreamType));
        if (audioLink != null)
          return audioLink;
      }
      throw new InvalidOperationException(string.Format("Could not found one of the following {{{0}}} in the collection", (object) string.Join<AudioStreamType>(",", preferredStreamTypes)));
    }

    internal static AudioLink GetAudioLink(this IMediaLink[] links, AudioQuality preferredQuality)
    {
      AudioLink audioLink = ((IEnumerable<IMediaLink>) links).Where<IMediaLink>((Func<IMediaLink, bool>) (link => link.IsAudioLink)).Cast<AudioLink>().OrderByDescending<AudioLink, AudioQuality>((Func<AudioLink, AudioQuality>) (link => link.AudioStreamQuality)).FirstOrDefault<AudioLink>((Func<AudioLink, bool>) (link => link.AudioStreamQuality <= preferredQuality));
      if (audioLink != null)
        return audioLink;
      throw new InvalidOperationException("Could not find a proper stream to download. Please report with the video link you tried to download.");
    }
  }
}
