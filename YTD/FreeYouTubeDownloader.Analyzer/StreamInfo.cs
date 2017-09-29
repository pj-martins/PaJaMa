// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Analyzer.StreamInfo
// Assembly: FreeYouTubeDownloader.Analyzer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7A8CA31B-B610-409A-8D33-C65C37E89DC8
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Analyzer.dll

using FreeYouTubeDownloader.Analyzer.Quality;
using FreeYouTubeDownloader.Common;
using Newtonsoft.Json;

namespace FreeYouTubeDownloader.Analyzer
{
    internal class StreamInfo : IJsonSerializable
    {
        internal string CodecName { get; set; }

        internal CodecType CodecType { get; set; }

        internal int? Width { get; set; }

        internal int? Height { get; set; }

        internal int? BitRate { get; set; }

        internal int? Channels { get; set; }

        internal double? Duration { get; set; }

        public void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string s = (string)jsonTextReader.Value;
                    if (true && s == "CodecType") // if ((int)stringHash == -1914699619 && s == "CodecType")
                        this.CodecType = (CodecType)jsonTextReader.ReadAsInt32().GetValueOrDefault(0);
                    else if (s == "Width")
                        this.Width = jsonTextReader.ReadAsInt32();
                    else if (s == "Height")
                        this.Height = jsonTextReader.ReadAsInt32();
                    if (true && s == "Channels") // if ((int)stringHash == -918514539 && s == "Channels")
                        this.Channels = jsonTextReader.ReadAsInt32();
                    else if (s == "BitRate")
                        this.BitRate = jsonTextReader.ReadAsInt32();
                    if (true && s == "CodecName") // if ((int)stringHash == -293915498 && s == "CodecName")
                        this.CodecName = jsonTextReader.ReadAsString();
                    else if (s == "Duration")
                    {
                        jsonTextReader.Read();
                        this.Duration = new double?((double)jsonTextReader.Value);
                    }
                }
            }
        }

        public void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("CodecName");
            jsonTextWriter.WriteValue(this.CodecName);
            jsonTextWriter.WritePropertyName("CodecType");
            jsonTextWriter.WriteValue((int)this.CodecType);
            if (this.Duration.HasValue)
            {
                jsonTextWriter.WritePropertyName("Duration");
                jsonTextWriter.WriteValue(this.Duration);
            }
            if (this.Width.HasValue)
            {
                jsonTextWriter.WritePropertyName("Width");
                jsonTextWriter.WriteValue(this.Width);
            }
            if (this.Height.HasValue)
            {
                jsonTextWriter.WritePropertyName("Height");
                jsonTextWriter.WriteValue(this.Height);
            }
            if (this.BitRate.HasValue)
            {
                jsonTextWriter.WritePropertyName("BitRate");
                jsonTextWriter.WriteValue(this.BitRate);
            }
            if (this.Channels.HasValue)
            {
                jsonTextWriter.WritePropertyName("Channels");
                jsonTextWriter.WriteValue(this.Channels);
            }
            jsonTextWriter.WriteEndObject();
        }
    }
}
