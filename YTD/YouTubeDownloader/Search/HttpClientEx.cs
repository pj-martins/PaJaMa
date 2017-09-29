// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.HttpClientEx
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FreeYouTubeDownloader.Search
{
  public class HttpClientEx
  {
    static HttpClientEx()
    {
      ServicePointManager.DefaultConnectionLimit = 100;
      ServicePointManager.Expect100Continue = false;
      ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback) ((param0, param1, param2, param3) => true);
    }

    public static async Task<string> GetStringAsync(string url)
    {
      HttpWebRequest request = HttpClientEx.CreateRequest(url);
      string str1 = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
      request.UserAgent = str1;
      string str2 = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
      request.Accept = str2;
      request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
      string end;
      using (HttpWebResponse httpWebResponse = (HttpWebResponse) await request.GetResponseAsync().ConfigureAwait(false))
      {
        Encoding encoding = Encoding.UTF8;
        System.Text.RegularExpressions.Match match = Regex.Match(httpWebResponse.ContentType, "charset=([a-zA-Z0-9-]+)", RegexOptions.IgnoreCase);
        if (match.Success)
          encoding = Encoding.GetEncoding(match.Groups[1].ToString());
        Stream stream = httpWebResponse.GetResponseStream();
        if (httpWebResponse.ContentEncoding.ToLower().Contains("gzip"))
          stream = (Stream) new GZipStream(stream, CompressionMode.Decompress);
        else if (httpWebResponse.ContentEncoding.ToLower().Contains("deflate"))
          stream = (Stream) new DeflateStream(stream, CompressionMode.Decompress);
        using (StreamReader streamReader = new StreamReader(stream, encoding))
          end = streamReader.ReadToEnd();
      }
      return end;
    }

    public static string GetString(string url)
    {
      HttpWebRequest request = HttpClientEx.CreateRequest(url);
      string str1 = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
      request.UserAgent = str1;
      string str2 = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
      request.Accept = str2;
      request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
      int num = 3;
      request.AutomaticDecompression = (DecompressionMethods) num;
      using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
      {
        Encoding encoding = Encoding.UTF8;
        System.Text.RegularExpressions.Match match = Regex.Match(response.ContentType, "charset=([a-zA-Z0-9-]+)", RegexOptions.IgnoreCase);
        if (match.Success)
          encoding = Encoding.GetEncoding(match.Groups[1].ToString());
        using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), encoding))
          return streamReader.ReadToEnd();
      }
    }

    private static HttpWebRequest CreateRequest(string url)
    {
      HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
      HttpClientEx.InitProxy(request);
      return request;
    }

    private static void InitProxy(HttpWebRequest request)
    {
      request.Proxy = (IWebProxy) null;
    }
  }
}
