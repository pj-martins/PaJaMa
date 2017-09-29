// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.StringExtension
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Downloader
{
  public static class StringExtension
  {
    private static readonly Regex HtmlToTextExp = new Regex("<[^>]*>");

    public static string JustBefore(this string str, string seq)
    {
      string str1 = str;
      try
      {
        str = str.ToLower();
        seq = seq.ToLower();
        return str1.Substring(0, str.Length - (str.Length - str.IndexOf(seq, StringComparison.Ordinal)));
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    public static string JustAfter(this string str, string seq, string seqEnd)
    {
      string str1 = str;
      try
      {
        int num1 = str.IndexOf(seq, StringComparison.Ordinal);
        if (num1 < 0)
          return (string) null;
        int startIndex = num1 + seq.Length;
        int num2 = str.IndexOf(seqEnd, startIndex, StringComparison.Ordinal);
        int length = num2 <= 0 ? str.Length - startIndex : num2 - startIndex;
        return str1.Substring(startIndex, length);
      }
      catch (Exception ex)
      {
        return string.Empty;
      }
    }

    public static string JustAfter(this string str, string seq)
    {
      string str1 = str;
      try
      {
        str = str.ToLower();
        seq = seq.ToLower();
        int num = str.IndexOf(seq, StringComparison.Ordinal);
        if (num < 0)
          return (string) null;
        int startIndex = num + seq.Length;
        return str1.Substring(startIndex, str.Length - startIndex);
      }
      catch (Exception ex)
      {
        return "";
      }
    }

    public static string HtmlToText(this string htmlString)
    {
      return StringExtension.HtmlToTextExp.Replace(htmlString, string.Empty);
    }

    public static string EscapeInvalidCharacters(this string givenString)
    {
      char[] chArray = new char[41]
      {
        '"',
        '<',
        '>',
        '|',
        char.MinValue,
        '\x0001',
        '\x0002',
        '\x0003',
        '\x0004',
        '\x0005',
        '\x0006',
        '\a',
        '\b',
        '\t',
        '\n',
        '\v',
        '\f',
        '\r',
        '\x000E',
        '\x000F',
        '\x0010',
        '\x0011',
        '\x0012',
        '\x0013',
        '\x0014',
        '\x0015',
        '\x0016',
        '\x0017',
        '\x0018',
        '\x0019',
        '\x001A',
        '\x001B',
        '\x001C',
        '\x001D',
        '\x001E',
        '\x001F',
        ':',
        '*',
        '?',
        '\\',
        '/'
      };
      StringBuilder stringBuilder = new StringBuilder(givenString.Length);
      foreach (char ch in givenString)
      {
        if (((IEnumerable<char>) chArray).Contains<char>(ch))
          stringBuilder.Append("%" + Convert.ToUInt16(ch).ToString("x2"));
        else
          stringBuilder.Append(ch);
      }
      return stringBuilder.ToString();
    }

    public static string ToBase64(this string givenString)
    {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(givenString));
    }
  }
}
