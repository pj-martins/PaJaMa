// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.ProviderException
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;
using System.Collections;
using System.Collections.Generic;

namespace FreeYouTubeDownloader.Downloader
{
  public class ProviderException : Exception
  {
    private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>(1);

    public override IDictionary Data
    {
      get
      {
        return (IDictionary) this._dictionary;
      }
    }

    public ProviderException(string message)
      : base(message)
    {
    }

    public ProviderException(string message, string siteUrl)
      : base(message)
    {
      this._dictionary.Add("Url", siteUrl);
    }
  }
}
