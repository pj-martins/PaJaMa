﻿// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.AacConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
  internal sealed class AacConversionProfile : ConversionProfile
  {
    public static readonly IEnumerable<AudioStreamType> PreferredAudioStreamTypesStatic = (IEnumerable<AudioStreamType>) new AudioStreamType[2]
    {
      AudioStreamType.Mp4,
      AudioStreamType.M4A
    };
    private const string FfmpegCommandArgsPattern = "-y -i \"{0}\" {1} \"{2}\"";

    internal override string FormatName
    {
      get
      {
        return "AAC";
      }
      set
      {
      }
    }

    internal override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
    {
      get
      {
        return AacConversionProfile.PreferredAudioStreamTypesStatic;
      }
    }

    internal override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
    {
      return string.Format("-y -i \"{0}\" {1} \"{2}\"", (object) this.InputFileName, (object) this.AudioBitRateCommandArg, (object) this.OutputFileName);
    }
  }
}
