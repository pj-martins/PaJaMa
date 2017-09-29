// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.StringExtensions
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Common
{
  public static class StringExtensions
  {
    public static bool IsValidFileName(this string expression)
    {
      return File.Exists(expression);
    }

    public static bool IsFileAudioFormat(this string expression)
    {
      string lower = Path.GetExtension(expression).ToLower().ToLower();
      return lower == ".mp3" || lower == ".aac" || lower == ".ogg";
    }

    public static int GetIntegerValueFromText(this string text)
    {
      int num = -1;
      if (!string.IsNullOrWhiteSpace(text))
      {
        try
        {
          num = int.Parse(Regex.Replace(text, "[^0-9]+", string.Empty));
        }
        catch
        {
        }
      }
      return num;
    }

    public static string Truncate(this string source, int limitLenght)
    {
      if (!string.IsNullOrWhiteSpace(source) && limitLenght > 0)
      {
        source = source.Trim();
        if (source.Length > limitLenght)
          return source.Substring(0, limitLenght) + "...";
      }
      return source;
    }

    public static int ToInt32(this string str)
    {
      return Convert.ToInt32(str);
    }

    public static long ToInt64(this string str)
    {
      return Convert.ToInt64(str);
    }

    public static string Remove(this string source, string strToRemove)
    {
      return source.Replace(strToRemove, string.Empty);
    }

    public static string RemoveInvalidFileNameCharacters(this string givenString)
    {
      char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
      StringBuilder stringBuilder = new StringBuilder(givenString.Length);
      foreach (char ch in givenString.Where<char>((Func<char, bool>) (symbol => !((IEnumerable<char>) invalidFileNameChars).Contains<char>(symbol))))
        stringBuilder.Append(ch);
      return stringBuilder.ToString();
    }
  }
}
