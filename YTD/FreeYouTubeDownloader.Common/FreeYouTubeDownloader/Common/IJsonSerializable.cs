﻿// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.IJsonSerializable
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using Newtonsoft.Json;

namespace FreeYouTubeDownloader.Common
{
  public interface IJsonSerializable
  {
    void ReadJson(JsonTextReader jsonTextReader);

    void WriteJson(JsonTextWriter jsonTextWriter);
  }
}
