// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Analyzer.VideoQualityInfo
// Assembly: FreeYouTubeDownloader.Analyzer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A8CA31B-B610-409A-8D33-C65C37E89DC8
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Analyzer.dll

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Downloader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FreeYouTubeDownloader.Analyzer
{
  internal class VideoQualityInfo : IJsonSerializable
  {
    internal string FormatName { get; set; }

    internal double Duration { get; set; }

    internal long Size { get; set; }

    internal MediaLink File { get; set; }

    public void ReadJson(JsonTextReader jsonTextReader)
    {
      List<StreamInfo> streamInfoList = new List<StreamInfo>();
      while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
      {
        if (jsonTextReader.TokenType == JsonToken.PropertyName)
        {
          string str = (string) jsonTextReader.Value;
          if (!(str == "FormatName"))
          {
            if (!(str == "Duration"))
            {
              if (!(str == "Size"))
              {
                if (str == "File")
                {
                  jsonTextReader.Read();
                  jsonTextReader.Read();
                  this.File = (MediaLink) Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Downloader").GetType(jsonTextReader.ReadAsString()));
                  this.File.ReadJson(jsonTextReader);
                }
              }
              else
              {
                jsonTextReader.Read();
                this.Size = (long) jsonTextReader.Value;
              }
            }
            else
            {
              jsonTextReader.Read();
              this.Duration = (double) jsonTextReader.Value;
            }
          }
          else
            this.FormatName = jsonTextReader.ReadAsString();
        }
      }
    }

    public void WriteJson(JsonTextWriter jsonTextWriter)
    {
      jsonTextWriter.WriteStartObject();
      jsonTextWriter.WritePropertyName("$type");
      jsonTextWriter.WriteValue(this.GetType().ToString());
      jsonTextWriter.WritePropertyName("FormatName");
      jsonTextWriter.WriteValue(this.FormatName);
      jsonTextWriter.WritePropertyName("Duration");
      jsonTextWriter.WriteValue(this.Duration);
      jsonTextWriter.WritePropertyName("Size");
      jsonTextWriter.WriteValue(this.Size);
      if (this.File != null)
      {
        jsonTextWriter.WritePropertyName("File");
        this.File.WriteJson(jsonTextWriter);
      }
      jsonTextWriter.WriteEndObject();
    }
  }
}
