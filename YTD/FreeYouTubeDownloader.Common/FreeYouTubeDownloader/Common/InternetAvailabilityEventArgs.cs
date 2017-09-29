// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.InternetAvailabilityEventArgs
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;

namespace FreeYouTubeDownloader.Common
{
  public class InternetAvailabilityEventArgs : EventArgs
  {
    public bool HasInternetConnection { get; private set; }

    public InternetAvailabilityEventArgs(bool hasInternetConnection)
    {
      this.HasInternetConnection = hasInternetConnection;
    }
  }
}
