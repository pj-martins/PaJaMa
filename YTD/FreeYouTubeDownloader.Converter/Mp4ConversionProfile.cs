// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.Mp4ConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
  internal sealed class Mp4ConversionProfile : ConversionProfile
  {
    public static readonly IEnumerable<AudioStreamType> PreferredAudioStreamTypesStatic = (IEnumerable<AudioStreamType>) new AudioStreamType[2]
    {
      AudioStreamType.Mp4,
      AudioStreamType.M4A
    };
    private const string FfmpegConvertCommandArgsPatternConvert = "-y -i \"{0}\" -qscale 0 {1} \"{2}\"";

    internal override string FormatName
    {
      get
      {
        return "MP4";
      }
      set
      {
      }
    }

    internal override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
    {
      get
      {
        return Mp4ConversionProfile.PreferredAudioStreamTypesStatic;
      }
    }

    internal override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
    {
      return string.Format("-y -i \"{0}\" -qscale 0 {1} \"{2}\"", (object) this.InputFileName, (object) this.VideoScaleCommandArg, (object) this.OutputFileName);
    }
  }
}
