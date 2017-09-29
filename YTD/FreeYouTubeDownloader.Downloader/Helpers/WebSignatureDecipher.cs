// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Downloader.Helpers.WebSignatureDecipher
// Assembly: FreeYouTubeDownloader.Downloader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9C3DA7DE-7248-4390-96DB-845A18A8472A
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Downloader.dll

using System;
using System.IO;
using System.Net;

namespace FreeYouTubeDownloader.Downloader.Helpers
{
  public class WebSignatureDecipher
  {
    private const string BaseDecipherRequestUrl = "http://www.freemake.com/SignatureDecoder/decoder.php?text={0}";
    private const int DefaultRequestTimeout = 10000;

    public static string Decipher(string signature)
    {
      try
      {
        return WebSignatureDecipher.TryDecipherFewTimes(WebSignatureDecipher.GenerateUri(signature));
      }
      catch (Exception ex)
      {
        return YouTubeSignatureDecipher.Decrypt(signature, "");
      }
    }

    private static string TryDecipherFewTimes(string uri)
    {
      for (int index = 0; index < 5; ++index)
      {
        HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
        string str1 = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
        httpWebRequest.Accept = str1;
        httpWebRequest.Headers[HttpRequestHeader.AcceptLanguage] = "en-US;q=0.6,en;q=0.4";
        string str2 = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/32.0.1700.107 Safari/537.36";
        httpWebRequest.UserAgent = str2;
        int num1 = 10000;
        httpWebRequest.Timeout = num1;
        int num2 = 10000;
        httpWebRequest.ReadWriteTimeout = num2;
        using (WebResponse response = httpWebRequest.GetResponse())
        {
          using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
          {
            string end = streamReader.ReadToEnd();
            if (!string.IsNullOrEmpty(end))
              return end;
          }
        }
      }
      return string.Empty;
    }

    private static string GenerateUri(string signature)
    {
      return string.Format("http://www.freemake.com/SignatureDecoder/decoder.php?text={0}", (object) signature);
    }

    public string Decode(string signature)
    {
      return WebSignatureDecipher.Decipher(signature);
    }
  }
}
