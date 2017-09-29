// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.VideoLink
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using Newtonsoft.Json;
using System;
using System.Reflection;

namespace FreeYouTubeDownloader.Downloader
{
    public class VideoLink : MediaLink
    {
        public VideoQuality VideoStreamQuality { get; set; }

        public VideoStreamType StreamType { get; set; }

        public bool IsDash { get; private set; }

        public AudioLink AudioLink { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool HasDimension
        {
            get
            {
                if (this.Width != 0)
                    return (uint)this.Height > 0U;
                return false;
            }
        }

        public string Dimension
        {
            get
            {
                return string.Format("{0}x{1}", (object)this.Width, (object)this.Height);
            }
        }

        public override bool IsAudioLink
        {
            get
            {
                return false;
            }
        }

        public override bool IsVideoLink
        {
            get
            {
                return true;
            }
        }

        public override string MediaFormat
        {
            get
            {
                switch (this.StreamType)
                {
                    case VideoStreamType.Flv:
                        return "flv";
                    case VideoStreamType.F4V:
                        return "f4v";
                    case VideoStreamType.M4V:
                        return "m4v";
                    case VideoStreamType.Mp4:
                        return "mp4";
                    case VideoStreamType.WebM:
                        return "webM";
                    case VideoStreamType.ThreeGp:
                        return "3gp";
                    default:
                        return Enum.GetName(typeof(VideoStreamType), (object)this.StreamType);
                }
            }
        }

        public override string MediaQuality
        {
            get
            {
                switch (this.VideoStreamQuality)
                {
                    case VideoQuality._4320p:
                        return "8K 4320p";
                    case VideoQuality._4320p60fps:
                        return "8K 4320p 60fps";
                    case VideoQuality._1440p:
                        return "HD 1440p";
                    case VideoQuality._1440p60fps:
                        return "HD 1440p 60fps";
                    case VideoQuality._2160p:
                        return "4K 2160p";
                    case VideoQuality._2160p60fps:
                        return "4K 2160p 60fps";
                    case VideoQuality._3072p:
                        return "4K 3072p";
                    case VideoQuality._1080p:
                        return "HD 1080p";
                    case VideoQuality._1080p60fps:
                        return "HD 1080p 60fps";
                    case VideoQuality._720p:
                        return "HD 720p";
                    case VideoQuality._720p60fps:
                        return "HD 720p 60fps";
                    case VideoQuality._270p:
                        return "270p";
                    case VideoQuality._360p:
                        return "360p";
                    case VideoQuality._360p60fps:
                        return "360p 60fps";
                    case VideoQuality._480p:
                        return "480p";
                    case VideoQuality._480p60fps:
                        return "480p 60fps";
                    case VideoQuality._240p:
                        return "240p";
                    case VideoQuality._240p60fps:
                        return "240p 60fps";
                    case VideoQuality._180p:
                        return "180p";
                    case VideoQuality._192p:
                        return "192p";
                    case VideoQuality._144p:
                        return "144p";
                    case VideoQuality._144p60fps:
                        return "144p 60fps";
                    default:
                        return "Unknown";
                }
            }
        }

        public VideoLink()
        {
        }

        public VideoLink(string url, VideoQuality videoStreamQuality, VideoStreamType videoStreamType, DateTime? expire = null, bool isDash = false, long length = 0)
          : base(url, expire, 0L)
        {
            this.VideoStreamQuality = videoStreamQuality;
            this.StreamType = videoStreamType;
            this.IsDash = isDash;
            this.Length = length;
        }

        public override VideoLink ToVideoLink()
        {
            return this;
        }

        public override AudioLink ToAudioLink()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("{0} {1}{2}", (object)this.MediaFormat, (object)this.MediaQuality, this.IsDash ? (object)" / DASH" : (object)string.Empty);
        }

        public override int GetHashCode()
        {
            return (int)this.VideoStreamQuality;
        }

        public override bool Equals(object obj)
        {
            VideoLink videoLink = obj as VideoLink;
            if (videoLink == null || this.VideoStreamQuality != videoLink.VideoStreamQuality)
                return false;
            return this.StreamType == videoLink.StreamType;
        }

        public override void ReadJson(JsonTextReader jsonTextReader)
        {
            while (jsonTextReader.Read() && jsonTextReader.TokenType != JsonToken.EndObject)
            {
                if (jsonTextReader.TokenType == JsonToken.PropertyName)
                {
                    string s = (string)jsonTextReader.Value;
                    // ISSUE: reference to a compiler-generated method
                    // uint stringHash = "\u003CPrivateImplementationDetails\u003E".ComputeStringHash(s);
                    int? nullable;
                    //if (stringHash <= 1839360066U)
                    //{
                    //    if (stringHash <= 536645421U)
                    //    {
                    //        if ((int)stringHash != 358315500)
                    //        {
                    //            if ((int)stringHash == 536645421 && s == "StreamType")
                    if (s == "StreamType")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.StreamType = (VideoStreamType)nullable.Value;
                    }
                    //}
                    else if (s == "Itag")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.Itag = nullable.Value;
                    }
                    //}
                    //else if ((int)stringHash != 575616685)
                    //{
                    //    if ((int)stringHash != 1507592515)
                    //    {
                    // if ((int)stringHash == 1839360066 && s == "Expire")
                    else if (s == "Expire")
                        this.Expire = jsonTextReader.ReadAsDateTime();
                    //}
                    else if (s == "JsPlayer")
                        this.JsPlayer = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "AudioLink")
                    {
                        jsonTextReader.Read();
                        jsonTextReader.Read();
                        this.AudioLink = (AudioLink)Activator.CreateInstance(Assembly.Load("FreeYouTubeDownloader.Downloader").GetType(jsonTextReader.ReadAsString()));
                        this.AudioLink.ReadJson(jsonTextReader);
                    }
                    //}
                    //else if (stringHash <= 3234472361U)
                    //{
                    //    if ((int)stringHash != -1793465410)
                    //    {
                    //        if ((int)stringHash != -1068956403)
                    //        {
                    //            if ((int)stringHash == -1060494935 && s == "SourceUrl")
                    else if (s == "SourceUrl")
                        this.SourceUrl = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Signature")
                        this.Signature = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "Url")
                        this.Url = jsonTextReader.ReadAsString();
                    //}
                    //else if ((int)stringHash != -880868651)
                    //{
                    //    if ((int)stringHash != -703851742)
                    //    {
                    //        if ((int)stringHash == -680800667 && s == "IsDash")
                    else if (s == "IsDash")
                    {
                        jsonTextReader.Read();
                        this.IsDash = (bool)jsonTextReader.Value;
                    }
                    //}
                    else if (s == "S")
                        this.S = jsonTextReader.ReadAsString();
                    //}
                    else if (s == "VideoStreamQuality")
                    {
                        nullable = jsonTextReader.ReadAsInt32();
                        this.VideoStreamQuality = (VideoQuality)nullable.Value;
                    }
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
            jsonTextWriter.WritePropertyName("VideoStreamQuality");
            jsonTextWriter.WriteValue((int)this.VideoStreamQuality);
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
            if (this.IsDash)
            {
                jsonTextWriter.WritePropertyName("IsDash");
                jsonTextWriter.WriteValue(this.IsDash);
                if (this.AudioLink != null)
                {
                    jsonTextWriter.WritePropertyName("AudioLink");
                    this.AudioLink.WriteJson(jsonTextWriter);
                }
            }
            jsonTextWriter.WriteEndObject();
        }
    }
}
