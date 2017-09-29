// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.ConversionCompletedEventArgs
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using System;

namespace FreeYouTubeDownloader.Converter
{
  public class ConversionCompletedEventArgs
  {
    public bool IsCanceled { get; private set; }

    public bool Succeeded { get; private set; }

    public Exception Error { get; private set; }

    public ConversionCompletedEventArgs(bool succeeded, bool isCanceled = false, Exception error = null)
    {
      this.Succeeded = succeeded;
      this.IsCanceled = isCanceled;
      this.Error = error;
    }
  }
}
