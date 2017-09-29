// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.ConversionProfileExtension
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using System;

namespace FreeYouTubeDownloader.Converter
{
  public static class ConversionProfileExtension
  {
    public static bool IsVideoProfile(this ConversionProfile profile)
    {
      Type type = profile.GetType();
      return !(type == typeof (AacConversionProfile)) && !(type == typeof (ExtractAudioConversionProfile)) && (!(type == typeof (Mp3ConversionProfile)) && !(type == typeof (VorbisConversionProfile)));
    }

    public static bool IsAudioProfile(this ConversionProfile profile)
    {
      Type type = profile.GetType();
      return type == typeof (AacConversionProfile) || type == typeof (ExtractAudioConversionProfile) || (type == typeof (Mp3ConversionProfile) || type == typeof (VorbisConversionProfile));
    }
  }
}
