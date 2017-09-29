// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Model.VideoStreamTypeExtensions
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Converter;
using FreeYouTubeDownloader.Downloader;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Model
{
  public static class VideoStreamTypeExtensions
  {
    public static IEnumerable<AudioStreamType> GetPreferredAudioStreamTypes(this VideoStreamType videoStreamType)
    {
      switch (videoStreamType)
      {
        case VideoStreamType.Flv:
          return FlvConversionProfile.PreferredAudioStreamTypesStatic;
        case VideoStreamType.Mp4:
          return Mp4ConversionProfile.PreferredAudioStreamTypesStatic;
        case VideoStreamType.WebM:
          return WebmConversionProfile.PreferredAudioStreamTypesStatic;
        case VideoStreamType.ThreeGp:
          return AacConversionProfile.PreferredAudioStreamTypesStatic;
        default:
          throw new System.NotSupportedException("Selected video stream does not have predefined audio data");
      }
    }
  }
}
