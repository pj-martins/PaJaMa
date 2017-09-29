// Decompiled with JetBrains decompiler
// Type: Google.Measurement.StringExtensions
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public static class StringExtensions
  {
    internal static byte[] GetBytes(this string givenString)
    {
      byte[] numArray = new byte[givenString.Length * 2];
      Buffer.BlockCopy((Array) givenString.ToCharArray(), 0, (Array) numArray, 0, numArray.Length);
      return numArray;
    }

    internal static byte[] GetBytes(this string givenString, Encoding encoding)
    {
      return encoding.GetBytes(givenString);
    }

    internal static string UrlEncode(this string givenString)
    {
      return Uri.EscapeDataString(givenString);
    }

    internal static bool IsNullOrEmpty(this string givenString)
    {
      return string.IsNullOrEmpty(givenString);
    }

    internal static bool NotNullOrEmpty(this string givenString)
    {
      return !string.IsNullOrEmpty(givenString);
    }

    internal static int GetSafeLength(this string givenString)
    {
      if (!givenString.NotNullOrEmpty())
        return 0;
      return givenString.Length;
    }
  }
}
