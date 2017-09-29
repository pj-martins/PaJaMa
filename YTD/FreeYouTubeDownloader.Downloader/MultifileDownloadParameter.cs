// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.MultifileDownloadParameter
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace FreeYouTubeDownloader.Downloader
{
  public class MultifileDownloadParameter : IJsonSerializable
  {
    internal MediaLink Link { get; private set; }

    public string FileName { get; private set; }

    public int ByteRange { get; private set; }

    public MultifileDownloadParameter()
    {
    }

    internal MultifileDownloadParameter(MediaLink link, string fileName, int byteRange = -1)
    {
      this.Link = link;
      this.FileName = fileName;
      this.ByteRange = byteRange;
    }

    public void ReadJson(JsonTextReader jsonTextReader)
    {
      while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
      {
        if (jsonTextReader.TokenType == JsonToken.PropertyName)
        {
          string str = (string) jsonTextReader.Value;
          if (!(str == "Link"))
          {
            if (!(str == "FileName"))
            {
              if (str == "ByteRange")
                this.ByteRange = jsonTextReader.ReadAsInt32().GetValueOrDefault(0);
            }
            else
              this.FileName = jsonTextReader.ReadAsString();
          }
          else
          {
            jsonTextReader.Read();
            jsonTextReader.Read();
            this.Link = (MediaLink) Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Downloader").GetType(jsonTextReader.ReadAsString()));
            this.Link.ReadJson(jsonTextReader);
          }
        }
      }
    }

    public void WriteJson(JsonTextWriter jsonTextWriter)
    {
      jsonTextWriter.WriteStartObject();
      jsonTextWriter.WritePropertyName("$type");
      jsonTextWriter.WriteValue(this.GetType().ToString());
      if (this.Link != null)
      {
        jsonTextWriter.WritePropertyName("Link");
        this.Link.WriteJson(jsonTextWriter);
      }
      jsonTextWriter.WritePropertyName("FileName");
      jsonTextWriter.WriteValue(this.FileName);
      jsonTextWriter.WritePropertyName("ByteRange");
      jsonTextWriter.WriteValue(this.ByteRange);
      jsonTextWriter.WriteEndObject();
    }
  }
}
