// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.YouTubeSignatureDecipher
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Downloader.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;

namespace FreeYouTubeDownloader.Downloader
{
  internal class YouTubeSignatureDecipher
  {
    private const string DecipherEndpoint = "https://api.videoplex.com/sign?sig={0}&js={1}&token={2}";

    private static char[] Swap(char[] arg1, int arg2)
    {
      char ch1 = arg1[0];
      char ch2 = arg1[arg2 % arg1.Length];
      arg1[0] = ch2;
      arg1[arg2] = ch1;
      return arg1;
    }

    private static char[] Reverse(char[] arg)
    {
      arg = ((IEnumerable<char>) arg).Reverse<char>().ToArray<char>();
      return arg;
    }

    private static char[] Clone(char[] arg1, int arg2)
    {
      return arg1.Slice<char>(arg2);
    }

    public static string Decrypt(string s, string swfPlayer = "")
    {
      FreeYouTubeDownloader.Debug.Log.Debug("YouTubeSignatureDecipher.Decrypt(string, string) => swfPlayer is " + (string.IsNullOrEmpty(swfPlayer) ? "(empty)" : swfPlayer), (Exception) null);
      try
      {
        char[] charArray = s.ToCharArray();
        int num = swfPlayer == "vfllJ43Wt" ? 1 : 0;
        return new string(YouTubeSignatureDecipher.Clone(YouTubeSignatureDecipher.Reverse(YouTubeSignatureDecipher.Clone(YouTubeSignatureDecipher.Swap(YouTubeSignatureDecipher.Reverse(YouTubeSignatureDecipher.Clone(YouTubeSignatureDecipher.Swap(charArray, 63), 1)), 46), 2)), 3));
      }
      catch (Exception ex)
      {
        throw new YouTubeSignatureException("Invalid YouTube signature");
      }
    }

    public static string DecryptFromWebSide(string s, string jsPlayer, string baseUrl = "")
    {
      FreeYouTubeDownloader.Debug.Log.Debug("YouTubeSignatureDecipher.DecryptFromWebSide(string, string) => JS player is " + (string.IsNullOrEmpty(jsPlayer) ? "(empty)" : jsPlayer), (Exception) null);
      if (string.IsNullOrEmpty(jsPlayer))
        throw new ArgumentNullException((string) null, "Js player cannot be empty. Please report");
      Stopwatch stopwatch = new Stopwatch();
      stopwatch.Start();
      jsPlayer = jsPlayer.Replace("\\/", "/");
      jsPlayer = jsPlayer.StartsWith("//") ? "http:" + jsPlayer : "http://www.youtube.com" + jsPlayer;
      string dataSync = HttpUtil.Instance.GetDataSync(string.Format("https://api.videoplex.com/sign?sig={0}&js={1}&token={2}", (object) s, (object) jsPlayer, (object) Authorization.GenerateToken()), "GET", (Dictionary<string, object>) null);
      stopwatch.Stop();
      if (dataSync.Contains("\"error\": \"Not authorized\""))
      {
        string empty = string.Empty;
        throw new AuthenticationException("Signature authorization failure");
      }
      if (dataSync == string.Empty || dataSync == s && !string.IsNullOrEmpty(baseUrl))
        dataSync = HttpUtil.Instance.GetDataSync(string.Format("https://api.videoplex.com/sign?sig={0}&js={1}&token={2}", (object) s, (object) baseUrl, (object) Authorization.GenerateToken()), "GET", (Dictionary<string, object>) null);
      FreeYouTubeDownloader.Debug.Log.Debug(string.Format("PERF DecryptFromWebSide elapsed in {0}ms", (object) stopwatch.ElapsedMilliseconds), (Exception) null);
      return dataSync;
    }

    public static string DecryptAgeProtected(string s)
    {
      if (s.Length == 86)
        return s.Substring(2, 61) + s[82].ToString() + s.Substring(64, 18) + s[63].ToString();
      return YouTubeSignatureDecipher.Decrypt(s, "");
    }

    public static string DecryptAgeProtectedFromWebSide(string s)
    {
      return YouTubeSignatureDecipher.DecryptAgeProtected(s);
    }
  }
}
