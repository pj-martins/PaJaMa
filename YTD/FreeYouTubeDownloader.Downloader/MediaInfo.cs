// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.MediaInfo
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Downloader
{
    public class MediaInfo : IJsonSerializable
    {
        public string Title;
        public string SourceUrl;
        public IMediaLink[] Links;
        public Exception Exception;

        public MediaInfo()
        {
            this.Title = Strings.PreparingForDownloading;
            this.Links = new IMediaLink[0];
        }

        public virtual void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string str = (string)jsonTextReader.Value;
                    if (!(str == "Title"))
                    {
                        if (!(str == "SourceUrl"))
                        {
                            if (!(str == "Links"))
                            {
                                if (str == "Exception")
                                    this.Exception = new Exception(jsonTextReader.ReadAsString());
                            }
                            else
                            {
                                List<MediaLink> mediaLinkList = new List<MediaLink>();
                                while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndArray)
                                {
                                    if (jsonTextReader.TokenType == JsonToken.PropertyName && object.Equals(jsonTextReader.Value, (object)"$type"))
                                    {
                                        MediaLink instance = (MediaLink)Activator.CreateInstance(Type.GetType(jsonTextReader.ReadAsString()));
                                        instance.ReadJson(jsonTextReader);
                                        mediaLinkList.Add(instance);
                                    }
                                }
                                this.Links = (IMediaLink[])mediaLinkList.ToArray();
                            }
                        }
                        else
                            this.SourceUrl = jsonTextReader.ReadAsString();
                    }
                    else
                        this.Title = jsonTextReader.ReadAsString();
                }
            }
        }

        public virtual void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("Title");
            jsonTextWriter.WriteValue(this.Title);
            jsonTextWriter.WritePropertyName("SourceUrl");
            jsonTextWriter.WriteValue(this.SourceUrl);
            jsonTextWriter.WritePropertyName("Links");
            jsonTextWriter.WriteStartArray();
            foreach (MediaLink link in this.Links)
                link.WriteJson(jsonTextWriter);
            jsonTextWriter.WriteEndArray();
            if (this.Exception != null)
            {
                jsonTextWriter.WritePropertyName("Exception");
                jsonTextWriter.WriteValue(this.Exception.Message);
            }
            jsonTextWriter.WriteEndObject();
        }
    }
}
