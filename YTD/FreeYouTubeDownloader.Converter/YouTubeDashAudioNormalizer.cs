// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.YouTubeDashAudioNormalizer
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using System;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
    public class YouTubeDashAudioNormalizer : ConversionProfile
    {
        private const string FfmpegCommandArgsPattern = "-y -i \"{0}\" -f {2} -acodec copy -vcodec copy \"{1}\"";

        public override string FormatName
        {
            get
            {
                return "MP4";
            }
            set
            {
            }
        }

        public override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
        {
            return string.Format("-y -i \"{0}\" -f {2} -acodec copy -vcodec copy \"{1}\"", (object)this.InputFileName, (object)this.OutputFileName, (object)this.FormatName);
        }
    }
}
