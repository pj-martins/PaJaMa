// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.FlvConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
    public sealed class FlvConversionProfile : ConversionProfile
    {
        public static readonly IEnumerable<AudioStreamType> PreferredAudioStreamTypesStatic = (IEnumerable<AudioStreamType>)new AudioStreamType[2]
        {
      AudioStreamType.Mp4,
      AudioStreamType.M4A
        };
        private const string FfmpegConvertCommandArgsPatternConvert = "-y -i \"{0}\" -qscale 0 {1} \"{2}\"";

        public override string FormatName
        {
            get
            {
                return "FLV";
            }
            set
            {
            }
        }

        public override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
        {
            get
            {
                return FlvConversionProfile.PreferredAudioStreamTypesStatic;
            }
        }

        public override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
        {
            return string.Format("-y -i \"{0}\" -qscale 0 {1} \"{2}\"", (object)this.InputFileName, (object)this.VideoScaleCommandArg, (object)this.OutputFileName);
        }
    }
}
