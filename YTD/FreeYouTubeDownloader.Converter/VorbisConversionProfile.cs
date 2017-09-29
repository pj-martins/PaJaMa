// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.VorbisConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
  public class VorbisConversionProfile : ConversionProfile
  {
    private const string FfmpegCommandArgsPattern = "-i \"{0}\" -acodec libvorbis -vn {1} \"{2}\"";

    internal override string FormatName
    {
      get
      {
        return "OGG";
      }
      set
      {
      }
    }

    internal override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
    {
      get
      {
        return (IEnumerable<AudioStreamType>) new AudioStreamType[1]
        {
          AudioStreamType.WebM
        };
      }
    }

    internal override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
    {
      return string.Format("-i \"{0}\" -acodec libvorbis -vn {1} \"{2}\"", (object) this.InputFileName, (object) this.AudioBitRateCommandArg, (object) this.OutputFileName);
    }
  }
}
