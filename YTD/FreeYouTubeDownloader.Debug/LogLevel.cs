// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Debug.LogLevel
// Assembly: FreeYouTubeDownloader.Debug, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0983BC95-B55A-44DE-AED0-739C8DD882F6
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Debug.dll

using System;

namespace FreeYouTubeDownloader.Debug
{
  [Flags]
  public enum LogLevel : byte
  {
    Info = 1,
    Trace = 2,
    Debug = 4,
    Warning = 8,
    Error = 16,
    Fatal = 32,
  }
}
