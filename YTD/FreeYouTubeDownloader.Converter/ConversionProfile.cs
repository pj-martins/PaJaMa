// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.ConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace FreeYouTubeDownloader.Converter
{
    public abstract class ConversionProfile : IJsonSerializable
    {
        public static List<ConversionProfile> ConversionProfiles { get; private set; }

        public abstract string FormatName { get; set; }

        public abstract IEnumerable<AudioStreamType> PreferredAudioStreamTypes { get; }

        public virtual string OutputFileName { get; set; }

        public string InputFileName { get; set; }

        public bool DeleteInputFile { get; set; }

        public string AudioBitRate { get; set; }

        public string AudioBitRateCommandArg
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.AudioBitRate))
                    return string.Empty;
                return string.Format(" -b:a {0} ", (object)this.AudioBitRate);
            }
        }

        public string VideoScale { get; set; }

        public string VideoScaleCommandArg
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.VideoScale))
                    return string.Empty;
                return string.Format(" -vf scale=-1:{0} ", (object)this.VideoScale);
            }
        }

        static ConversionProfile()
        {
            ConversionProfile.ConversionProfiles = new List<ConversionProfile>()
      {
        (ConversionProfile) new Mp3ConversionProfile(),
        (ConversionProfile) new AacConversionProfile(),
        (ConversionProfile) new AviConversionProfile(),
        (ConversionProfile) new Mp4ConversionProfile(),
        (ConversionProfile) new FlvConversionProfile(),
        (ConversionProfile) new VorbisConversionProfile(),
        (ConversionProfile) new WebmConversionProfile()
      };
        }

        public ConversionProfile()
        {
            this.DeleteInputFile = true;
        }

        protected virtual string ChangeExtension(string fileName, string formatName)
        {
            return Path.ChangeExtension(fileName, formatName);
        }

        public abstract string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo);

        public override string ToString()
        {
            return this.FormatName;
        }

        public override bool Equals(object obj)
        {
            string b = obj as string;
            if (b == null)
                return base.Equals(obj);
            return string.Equals(this.FormatName, b, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string CreateCommandArg(string pattern, string value)
        {
            string str = string.Empty;
            if (!string.IsNullOrWhiteSpace(pattern) && !string.IsNullOrWhiteSpace(value))
                str = string.Format(pattern, (object)value);
            return str;
        }

        public virtual void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string str = (string)jsonTextReader.Value;
                    if (!(str == "OutputFileName"))
                    {
                        if (str == "DeleteInputFile")
                        {
                            jsonTextReader.Read();
                            this.DeleteInputFile = (bool)jsonTextReader.Value;
                        }
                    }
                    else
                        this.OutputFileName = jsonTextReader.ReadAsString();
                }
            }
        }

        public virtual void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("OutputFileName");
            jsonTextWriter.WriteValue(this.OutputFileName);
            jsonTextWriter.WritePropertyName("DeleteInputFile");
            jsonTextWriter.WriteValue(this.DeleteInputFile);
            jsonTextWriter.WriteEndObject();
        }
    }
}
