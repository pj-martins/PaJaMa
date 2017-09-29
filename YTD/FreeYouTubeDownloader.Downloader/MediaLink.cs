// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.MediaLink
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.Downloader.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FreeYouTubeDownloader.Downloader
{
  public abstract class MediaLink : IMediaLink, IJsonSerializable
  {
    public string SourceUrl { get; set; }

    public long Length { get; set; }

    public bool HasLength
    {
      get
      {
        return this.Length > 0L;
      }
    }

    public string S { get; set; }

    public string JsPlayer { get; set; }

    public string Signature { get; set; }

    public string Url { get; set; }

    public DateTime? Expire { get; set; }

    public int Itag { get; set; }

    public abstract string MediaFormat { get; }

    public abstract string MediaQuality { get; }

    public abstract bool IsVideoLink { get; }

    public abstract bool IsAudioLink { get; }

    protected MediaLink()
    {
    }

    protected MediaLink(string url, DateTime? expire = null, long length = 0)
    {
      this.Url = url;
      this.Expire = expire;
      this.Length = length;
    }

    public static string Decode(string data)
    {
      if (string.IsNullOrEmpty(data))
        return data;
      return Uri.UnescapeDataString(data);
    }

    public abstract VideoLink ToVideoLink();

    public abstract AudioLink ToAudioLink();

    public void UpdateLink()
    {
      Log.Trace("CALL MediaLink.UpdateLink", (Exception) null);
      if (this.Expire.HasValue && this.Expire.Value <= DateTime.Now)
      {
        ManualResetEvent waitHandle = new ManualResetEvent(false);
        new YouTubeDownloadProvider().ReceiveMediaInfo(this.SourceUrl, (Action<MediaInfo>) (info =>
        {
          IMediaLink mediaLink = ((IEnumerable<IMediaLink>) info.Links).First<IMediaLink>((Func<IMediaLink, bool>) (link => link.Itag == this.Itag));
          this.Url = mediaLink.Url;
          this.Expire = mediaLink.Expire;
          this.S = mediaLink.S;
          this.JsPlayer = mediaLink.JsPlayer;
          if (!string.IsNullOrEmpty(this.S) && string.IsNullOrEmpty(mediaLink.Signature))
          {
            this.Signature = YouTubeSignatureDecipher.DecryptFromWebSide(this.S, this.JsPlayer, this.SourceUrl);
            this.Url = this.Url + "&signature=" + this.Signature;
          }
          waitHandle.Set();
        }), (MediaInfo) null);
        waitHandle.WaitOne(TimeSpan.FromSeconds(30.0));
      }
      else
      {
        if (string.IsNullOrEmpty(this.S) || !string.IsNullOrEmpty(this.Signature))
          return;
        this.Signature = YouTubeSignatureDecipher.DecryptFromWebSide(this.S, this.JsPlayer, this.SourceUrl);
        this.Url = this.Url + "&signature=" + this.Signature;
      }
    }

    public abstract void ReadJson(JsonTextReader jsonTextReader);

    public abstract void WriteJson(JsonTextWriter jsonTextWriter);
  }
}
