// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.AudioLink
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using Newtonsoft.Json;
using System;

namespace FreeYouTubeDownloader.Downloader
{
    public class AudioLink : MediaLink
    {
        public AudioQuality AudioStreamQuality { get; set; }

        public AudioStreamType StreamType { get; set; }

        public override bool IsAudioLink
        {
            get
            {
                return true;
            }
        }

        public override bool IsVideoLink
        {
            get
            {
                return false;
            }
        }

        public override string MediaFormat
        {
            get
            {
                switch (this.StreamType)
                {
                    case AudioStreamType.Mp3:
                        return "mp3";
                    case AudioStreamType.Mp4:
                        return "mp4";
                    case AudioStreamType.WebM:
                        return "webM";
                    case AudioStreamType.M4A:
                        return "m4a";
                    case AudioStreamType.Aac:
                        return "aac";
                    case AudioStreamType.ThreeGP:
                        return "3gp";
                    default:
                        return Enum.GetName(typeof(AudioStreamType), (object)this.StreamType);
                }
            }
        }

        public override string MediaQuality
        {
            get
            {
                switch (this.AudioStreamQuality)
                {
                    case AudioQuality._224kbps:
                        return "224 kbps";
                    case AudioQuality._256kbps:
                        return "256 kbps";
                    case AudioQuality._320kbps:
                        return "320 kbps";
                    case AudioQuality._160kbps:
                        return "160 kbps";
                    case AudioQuality._192kbps:
                        return "192 kbps";
                    case AudioQuality._112kbps:
                        return "112 kbps";
                    case AudioQuality._128kbps:
                        return "128 kbps";
                    case AudioQuality._144kbps:
                        return "144 kbps";
                    case AudioQuality._80kbps:
                        return "80 kbps";
                    case AudioQuality._96kbps:
                        return "96 kbps";
                    case AudioQuality._56kbps:
                        return "56 kbps";
                    case AudioQuality._64kbps:
                        return "64 kbps";
                    case AudioQuality._70kbps:
                        return "70 kbps";
                    case AudioQuality._48kbps:
                        return "48 kbps";
                    case AudioQuality._50kbps:
                        return "50 kbps";
                    case AudioQuality._24kbps:
                        return "24 kbps";
                    case AudioQuality._32kbps:
                        return "32 kbps";
                    case AudioQuality._8kbps:
                        return "8 kbps";
                    case AudioQuality._16kbps:
                        return "16 kbps";
                    default:
                        return string.Empty;
                }
            }
        }

        public AudioLink()
        {
        }

        public AudioLink(string url, AudioStreamType audioStreamType, DateTime? expire = null)
          : base(url, expire, 0L)
        {
            this.AudioStreamQuality = AudioQuality.Unknown;
            this.StreamType = audioStreamType;
        }

        public AudioLink(string url, AudioQuality audioStreamQuality, AudioStreamType audioStreamType, DateTime? expire = null, long length = 0)
          : base(url, expire, length)
        {
            this.AudioStreamQuality = audioStreamQuality;
            this.StreamType = audioStreamType;
        }

        public override VideoLink ToVideoLink()
        {
            throw new NotImplementedException();
        }

        public override AudioLink ToAudioLink()
        {
            return this;
        }

        public override void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string s = (string)jsonTextReader.Value;
                    // ISSUE: reference to a compiler-generated method
                    //uint stringHash = <PrivateImplementationDetails>.ComputeStringHash(s);
                    int? nullable;
                    //if (stringHash <= 1507592515U)
                    //{
                    //  if (stringHash <= 536645421U)
                    //  {
                    //    if ((int) stringHash != 358315500)
                    //    {
                    //if ((int) stringHash == 536645421 && s == "StreamType")
                    if (s == "StreamType")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.StreamType = (AudioStreamType)nullable.Value;
                    }
                    //}
                    else if (s == "Itag")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.Itag = nullable.Value;
                    }
                    //}
                    //else if ((int)stringHash != 537132402)
                    //{
                    //if ((int)stringHash == 1507592515 && s == "JsPlayer")
                    else if (s == "JsPlayer")
                        this.JsPlayer = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "AudioStreamQuality")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.AudioStreamQuality = (AudioQuality)nullable.Value;
                    }
                    //}
                    //else if (stringHash <= 2501501886U)
                    //{
                    //    if ((int)stringHash != 1839360066)
                    //    {
                    //        if ((int)stringHash == -1793465410 && s == "Url")
                    else if (s == "Url")
                        this.Url = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Expire")
                        this.Expire = jsonTextReader.ReadAsDateTime();
                    //}
                    //else if ((int)stringHash != -1068956403)
                    //{
                    //    if ((int)stringHash != -1060494935)
                    //    {
                    //        if ((int)stringHash == -703851742 && s == "S")
                    else if (s == "S")
                        this.S = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "SourceUrl")
                        this.SourceUrl = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Signature")
                        this.Signature = jsonTextReader.ReadAsString();
                }
            }
        }

        public override void WriteJson(JsonTextWriter jsonTextWriter)
        {
            jsonTextWriter.WriteStartObject();
            jsonTextWriter.WritePropertyName("$type");
            jsonTextWriter.WriteValue(this.GetType().ToString());
            jsonTextWriter.WritePropertyName("Url");
            jsonTextWriter.WriteValue(this.Url);
            jsonTextWriter.WritePropertyName("StreamType");
            jsonTextWriter.WriteValue((int)this.StreamType);
            jsonTextWriter.WritePropertyName("AudioStreamQuality");
            jsonTextWriter.WriteValue((int)this.AudioStreamQuality);
            if (this.Expire.HasValue)
            {
                jsonTextWriter.WritePropertyName("Expire");
                jsonTextWriter.WriteValue(this.Expire);
            }
            jsonTextWriter.WritePropertyName("SourceUrl");
            jsonTextWriter.WriteValue(this.SourceUrl);
            jsonTextWriter.WritePropertyName("Itag");
            jsonTextWriter.WriteValue(this.Itag);
            if (!string.IsNullOrEmpty(this.S))
            {
                jsonTextWriter.WritePropertyName("S");
                jsonTextWriter.WriteValue(this.S);
            }
            if (!string.IsNullOrEmpty(this.Signature))
            {
                jsonTextWriter.WritePropertyName("Signature");
                jsonTextWriter.WriteValue(this.Signature);
            }
            if (!string.IsNullOrEmpty(this.JsPlayer))
            {
                jsonTextWriter.WritePropertyName("JsPlayer");
                jsonTextWriter.WriteValue(this.JsPlayer);
            }
            jsonTextWriter.WriteEndObject();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", (object)this.MediaFormat, (object)this.MediaQuality);
        }

        public override int GetHashCode()
        {
            return (int)this.AudioStreamQuality;
        }

        public override bool Equals(object obj)
        {
            AudioLink audioLink = obj as AudioLink;
            if (audioLink == null || this.AudioStreamQuality != audioLink.AudioStreamQuality)
                return false;
            return this.StreamType == audioLink.StreamType;
        }
    }
}
