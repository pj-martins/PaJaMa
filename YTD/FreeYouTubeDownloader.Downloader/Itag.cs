// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Itag
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

namespace FreeYouTubeDownloader.Downloader
{
  public abstract class Itag
  {
    internal int Width;
    internal int Height;
    internal string Codec;

    internal abstract bool IsVideoTag { get; }

    internal abstract VideoItag AsVideoTag { get; }

    internal abstract AudioItag AsAudioTag { get; }
  }
}
