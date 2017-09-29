// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.WebmConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
  public class WebmConversionProfile : ConversionProfile
  {
    public static readonly IEnumerable<AudioStreamType> PreferredAudioStreamTypesStatic = (IEnumerable<AudioStreamType>) new AudioStreamType[1]
    {
      AudioStreamType.WebM
    };
    private const string FfmpegConvertCommandArgsPatternConvert = "-y -i \"{0}\" -c:v libvpx -b:v 1900K -crf 10 -f webm {1} \"{2}\"";

    public override string FormatName
    {
      get
      {
        return "WEBM";
      }
      set
      {
      }
    }

    public override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
    {
      get
      {
        return WebmConversionProfile.PreferredAudioStreamTypesStatic;
      }
    }

    public override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
    {
      return string.Format("-y -i \"{0}\" -c:v libvpx -b:v 1900K -crf 10 -f webm {1} \"{2}\"", (object) this.InputFileName, (object) this.VideoScaleCommandArg, (object) this.OutputFileName);
    }
  }
}
