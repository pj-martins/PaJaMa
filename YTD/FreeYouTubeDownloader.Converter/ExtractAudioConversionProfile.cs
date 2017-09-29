// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.ExtractAudioConversionProfile
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Converter
{
  internal sealed class ExtractAudioConversionProfile : ConversionProfile
  {
    private const string FfmpegCommandArgsPattern = "-y -i \"{0}\" -vn -acodec copy \"{1}\"";

    internal override string FormatName { get; set; }

    internal override IEnumerable<AudioStreamType> PreferredAudioStreamTypes
    {
      get
      {
        throw new NotImplementedException();
      }
    }

    internal override string GetFfmpegCommandArgs(VideoQualityInfo inputVideoQualityInfo)
    {
      return string.Format("-y -i \"{0}\" -vn -acodec copy \"{1}\"", (object) this.InputFileName, (object) this.OutputFileName);
    }

    public override void ReadJson(JsonTextReader jsonTextReader)
    {
      while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
      {
        if (jsonTextReader.TokenType == JsonToken.PropertyName)
        {
          string str = (string) jsonTextReader.Value;
          if (!(str == "OutputFileName"))
          {
            if (!(str == "FormatName"))
            {
              if (str == "DeleteInputFile")
              {
                jsonTextReader.Read();
                this.DeleteInputFile = (bool) jsonTextReader.Value;
              }
            }
            else
              this.FormatName = jsonTextReader.ReadAsString();
          }
          else
            this.OutputFileName = jsonTextReader.ReadAsString();
        }
      }
    }

    public override void WriteJson(JsonTextWriter jsonTextWriter)
    {
      jsonTextWriter.WriteStartObject();
      jsonTextWriter.WritePropertyName("$type");
      jsonTextWriter.WriteValue(this.GetType().ToString());
      jsonTextWriter.WritePropertyName("OutputFileName");
      jsonTextWriter.WriteValue(this.OutputFileName);
      jsonTextWriter.WritePropertyName("FormatName");
      jsonTextWriter.WriteValue(this.FormatName);
      jsonTextWriter.WritePropertyName("DeleteInputFile");
      jsonTextWriter.WriteValue(this.DeleteInputFile);
      jsonTextWriter.WriteEndObject();
    }
  }
}
