// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.FFMpegException
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using System;

namespace FreeYouTubeDownloader.Converter
{
  public class FFMpegException : Exception
  {
    public int ErrorCode { get; private set; }

    public FFMpegException(int errCode, string message)
      : base(message)
    {
      this.ErrorCode = errCode;
    }

    public FFMpegException(int errCode, string message, Exception innerException)
      : base(message, innerException)
    {
      this.ErrorCode = errCode;
    }
  }
}
