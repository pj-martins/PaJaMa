// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.DateTimeUtil
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;

namespace FreeYouTubeDownloader.Common
{
  public static class DateTimeUtil
  {
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimeStamp).ToLocalTime();
    }

    public static DateTime UnixTimeStampToDateTime(string unixTimeStamp)
    {
      if (!string.IsNullOrEmpty(unixTimeStamp))
        return DateTimeUtil.UnixTimeStampToDateTime(Convert.ToDouble(unixTimeStamp));
      return new DateTime();
    }
  }
}
