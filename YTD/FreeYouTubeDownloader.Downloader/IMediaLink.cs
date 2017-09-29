// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.IMediaLink
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;

namespace FreeYouTubeDownloader.Downloader
{
  public interface IMediaLink
  {
    string Url { get; }

    string MediaFormat { get; }

    string MediaQuality { get; }

    bool IsVideoLink { get; }

    bool IsAudioLink { get; }

    DateTime? Expire { get; }

    int Itag { get; }

    string S { get; set; }

    string Signature { get; set; }

    string JsPlayer { get; set; }

    VideoLink ToVideoLink();

    AudioLink ToAudioLink();

    void UpdateLink();
  }
}
