// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.ConvertExtension
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Globalization;

namespace FreeYouTubeDownloader.Common
{
  public static class ConvertExtension
  {
    public static string BytesToKilobytes(this int bytes)
    {
      return string.Format("{0} kB/s", (object) (bytes / 1000));
    }

    public static string BytesToKilobytes(this long bytes)
    {
      return string.Format("{0} kB/s", (object) (bytes / 1000L));
    }

    public static string ToInternetSpeed(this long bytes)
    {
      double num = (double) bytes / 1000.0;
      if ((long) (num / 1000.0) != 0L)
        return string.Format("{0:F} MB/s", (object) (num / 1000.0));
      return string.Format("{0} kB/s", (object) Convert.ToInt32(num));
    }

    public static string ToFileSize(this long bytes, bool includeSymbol = true)
    {
      if (bytes > 1000000000L)
        return string.Format(includeSymbol ? "{0:F} GB" : "{0:F}", (object) ((double) bytes / 1000.0 / 1000.0 / 1000.0));
      if (bytes > 1000000L)
        return string.Format(includeSymbol ? "{0:F} MB" : "{0:F}", (object) ((double) bytes / 1000.0 / 1000.0));
      if (bytes > 1000L)
        return string.Format(includeSymbol ? "{0:F} KB" : "{0:F}", (object) ((double) bytes / 1000.0));
      return bytes.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string ToFileSize(this ulong bytes, bool includeSymbol = true)
    {
      if (bytes > 1000000000UL)
        return string.Format(includeSymbol ? "{0:F} GB" : "{0:F}", (object) ((double) bytes / 1000.0 / 1000.0 / 1000.0));
      if (bytes > 1000000UL)
        return string.Format(includeSymbol ? "{0:F} MB" : "{0:F}", (object) ((double) bytes / 1000.0 / 1000.0));
      if (bytes > 1000UL)
        return string.Format(includeSymbol ? "{0:F} KB" : "{0:F}", (object) ((double) bytes / 1000.0));
      return bytes.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string HzToHumanFriendlyRepresentation(this uint hz, bool includeSymbol = true)
    {
      return hz < 1000000000U ? (hz < 1000000U ? (hz < 1000U ? string.Format(includeSymbol ? "{0:F} Hz" : "{0:F}", (object) hz) : string.Format(includeSymbol ? "{0:F} kHz" : "{0:F}", (object) (hz / 1000U))) : string.Format(includeSymbol ? "{0:F} MHz" : "{0:F}", (object) (hz / 1000U / 1000U))) : string.Format(includeSymbol ? "{0:F} GHz" : "{0:F}", (object) (hz / 1000U / 1000U / 1000U));
    }

    public static string MillisecondsToHumanFriendlyRepresentation(this double milliseconds, bool includeMilliseconds, bool fullFormat, bool includeSymbol = true)
    {
      TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
      string str;
      if (includeSymbol)
      {
        if (includeMilliseconds)
        {
          if (fullFormat)
            str = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
          else if (timeSpan.Hours > 0)
            str = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
          else
            str = timeSpan.Minutes <= 0 ? string.Format("{0:D2}s:{1:D3}ms", (object) timeSpan.Seconds, (object) timeSpan.Milliseconds) : string.Format("{0:D2}m:{1:D2}s:{2:D3}ms", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
        }
        else
          str = !fullFormat ? (timeSpan.Hours > 0 ? string.Format("{0:D2}h:{1:D2}m:{2:D2}s", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds) : string.Format("{0:D2}m:{1:D2}s", (object) timeSpan.Minutes, (object) timeSpan.Seconds)) : string.Format("{0:D2}h:{1:D2}m:{2:D2}s", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
      }
      else if (includeMilliseconds)
      {
        if (fullFormat)
          str = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
        else if (timeSpan.Hours > 0)
          str = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
        else
          str = timeSpan.Minutes <= 0 ? string.Format("{0:D2}:{1:D3}", (object) timeSpan.Seconds, (object) timeSpan.Milliseconds) : string.Format("{0:D2}:{1:D2}:{2:D3}", (object) timeSpan.Minutes, (object) timeSpan.Seconds, (object) timeSpan.Milliseconds);
      }
      else
        str = !fullFormat ? (timeSpan.Hours > 0 ? string.Format("{0:D2}:{1:D2}:{2:D2}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds) : string.Format("{0:D2}:{1:D2}", (object) timeSpan.Minutes, (object) timeSpan.Seconds)) : string.Format("{0:D2}:{1:D2}:{2:D2}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
      return str;
    }

    public static string ToFileSize(this uint bytes, bool includeSymbol = true)
    {
      return bytes.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    }

    public static string FormatSeconds(this double seconds)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
      if (timeSpan.Hours != 0)
        return string.Format("{0:d2}:{1:d2}:{2:d2}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
      return string.Format("{0:d2}:{1:d2}", (object) timeSpan.Minutes, (object) timeSpan.Seconds);
    }

    public static string FormatSeconds(this long seconds)
    {
      TimeSpan timeSpan = TimeSpan.FromSeconds((double) seconds);
      if (timeSpan.Hours != 0)
        return string.Format("{0:d2}:{1:d2}:{2:d2}", (object) timeSpan.Hours, (object) timeSpan.Minutes, (object) timeSpan.Seconds);
      return string.Format("{0:d2}:{1:d2}", (object) timeSpan.Minutes, (object) timeSpan.Seconds);
    }
  }
}
