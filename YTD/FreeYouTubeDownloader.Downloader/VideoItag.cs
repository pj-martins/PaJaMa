// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.VideoItag
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

namespace FreeYouTubeDownloader.Downloader
{
  public class VideoItag : Itag
  {
    internal VideoQuality Quality;
    internal VideoStreamType StreamType;
    internal bool IsDash;
    internal bool IsLive;
    internal bool Is3D;

    internal override bool IsVideoTag
    {
      get
      {
        return true;
      }
    }

    internal override VideoItag AsVideoTag
    {
      get
      {
        return this;
      }
    }

    internal override AudioItag AsAudioTag
    {
      get
      {
        return (AudioItag) null;
      }
    }
  }
}
