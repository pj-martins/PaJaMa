// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Youtube.YoutubeHelper
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.Downloader.Providers;
using FreeYouTubeDownloader.Search;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FreeYouTubeDownloader.Youtube
{
  public static class YoutubeHelper
  {
    private static readonly string VideoDataUrlTemplateByFeedType = "https://www.googleapis.com/youtube/v3/videos?part=contentDetails%2Csnippet&fields=items(contentDetails%2Cid%2Csnippet)&id={0}&key={1}";
    private static readonly string ThumbnailUrlTemplate = "https://img.youtube.com/vi/{0}/default.jpg";
    internal static List<string> YouTubeAuthKeys = new List<string>(20)
    {
      "AIzaSyCCXQQobRhNJNKHzvjujAJnCEf0SxQP1eE",
      "AIzaSyAbV63tQo85k2M-oydwTLHOxKy-r26krA4",
      "AIzaSyANcBj-Q2VmcK4Wl7JB5iq40goDOdUdcSw",
      "AIzaSyDl5N4vfPlsOGUKbfCKn5aYZO2RqdlZf38",
      "AIzaSyAWNoo_LLHf3HEfcVlRrdfimnSJdR4YINU",
      "AIzaSyBcANZAgUzZeX0GCOgPVI_lLfhGDUiHanE",
      "AIzaSyDnAt-Hwbc1WB11Q1WjjqPCTtpP_aw5IKo",
      "AIzaSyCxoSLtXuniLMAad-QqMLJvkEyHaHdInGM",
      "AIzaSyBo3UEEKn4Sz0cRehXW2g8SswzkBlcn5gw",
      "AIzaSyCZhoKYMeviwDHIjMBvbZCaSiqu8gQZTIU",
      "AIzaSyA2bf-lRfss9Gg5kvXegmx6eJOea7a0SPQ",
      "AIzaSyB21lWI0jBja8nHA4QvQkyGPew1F27h01M",
      "AIzaSyB3YWMLO8ZTXO7J3lP1GCC_2k74Y4yBlrg",
      "AIzaSyDjPDwlF8Hea9CGxWuP_MjdblrAiPbYEZQ",
      "AIzaSyB9l7XKzHqGLxP5fp3904FnXTdWbuuTL98",
      "AIzaSyCrClonkqcIBQSxlA6iWQWDgVpnY528SyA",
      "AIzaSyB1B0Jra8XSXvP9lIMziW92KZtQ-CHQRz4"
    };
    internal static YouTubeVideoSearch YoutubeSearch = new YouTubeVideoSearch(YoutubeHelper.YouTubeAuthKeys.ElementAt<string>(new Random().Next(0, YoutubeHelper.YouTubeAuthKeys.Count)));
    private const string AuthKeysUrl = "https://youtubedownloader.com/wd/apikeys/apikeys.json";
    private const string AuthKeysUrlFallback = "https://getyoutubedownloader.com/wd/apikeys/apikeys.json";

    static YoutubeHelper()
    {
      Task.Run((Func<Task>) (async () =>
      {
        string keysJson = (string) null;
        string keysUrl = "https://youtubedownloader.com/wd/apikeys/apikeys.json";
        while (true)
        {
          try
          {
            keysJson = await HttpClientEx.GetStringAsync(keysUrl);
            break;
          }
          catch (WebException ex)
          {
            if (keysUrl != "https://getyoutubedownloader.com/wd/apikeys/apikeys.json")
              keysUrl = "https://getyoutubedownloader.com/wd/apikeys/apikeys.json";
            else
              break;
          }
        }
        if (keysJson == null)
          return;
        try
        {
          JObject jobject = JObject.Parse(keysJson);
          YoutubeHelper.YouTubeAuthKeys = new List<string>(20);
          string index = "keys";
          foreach (string str in jobject[index].Values<string>())
            YoutubeHelper.YouTubeAuthKeys.Add(str);
        }
        catch (Exception ex)
        {
          Log.Error("Cannot obtain auth keys", ex);
        }
        YoutubeHelper.YoutubeSearch.ApiKey = YoutubeHelper.YouTubeAuthKeys.ElementAt<string>(new Random().Next(0, YoutubeHelper.YouTubeAuthKeys.Count));
      }));
    }

    public static List<YoutubeVideoRecord> GetYoutubeVideoData(string keyword, uint startIndex, uint countItems = 25)
    {
      List<YoutubeVideoRecord> youtubeVideoRecordList = new List<YoutubeVideoRecord>();
      if (string.IsNullOrWhiteSpace(keyword))
        return youtubeVideoRecordList;
      List<VideoSearchResult> source = YoutubeHelper.PerformYouTubeSearch(keyword, countItems);
      youtubeVideoRecordList.AddRange(source.Select<VideoSearchResult, YoutubeVideoRecord>((Func<VideoSearchResult, YoutubeVideoRecord>) (searchResult => new YoutubeVideoRecord(searchResult.Id, searchResult.Url, searchResult.Title, searchResult.Description.Replace("\n", " "), searchResult.Duration.TotalMilliseconds, searchResult.Thumbnails.First<Thumbnail>().Url, YoutubeHelper.GetVideoThumbnailByThumbnailUrl(searchResult.Thumbnails.First<Thumbnail>().Url)))));
      return youtubeVideoRecordList;
    }

    public static List<VideoSearchResult> PerformYouTubeSearch(string keyword, uint maxResults)
    {
      YoutubeHelper.YoutubeSearch.Query = keyword;
      YoutubeHelper.YoutubeSearch.MaxResults = maxResults;
      return YoutubeHelper.YoutubeSearch.FetchResultSet();
    }

    public static YoutubeVideoRecord GetYoutubeVideoDataByUrl(string videoUrl, YoutubeHelper.FeedType youtubeVideoDataFormat)
    {
      YoutubeVideoRecord youtubeVideoRecord = (YoutubeVideoRecord) null;
      if (youtubeVideoDataFormat != YoutubeHelper.FeedType.Json)
      {
        if (youtubeVideoDataFormat == YoutubeHelper.FeedType.Rss)
          youtubeVideoRecord = YoutubeHelper.GetYoutubeVideoRecordUrlTypeRss(videoUrl);
      }
      else
        youtubeVideoRecord = YoutubeHelper.GetYoutubeVideoRecordUrlTypeJson(videoUrl);
      return youtubeVideoRecord;
    }

    public static string GetYoutubeVideoIdByUrl(string url)
    {
      return YouTubeDownloadProvider.GetVideoIdStatic(url);
    }

    private static string GetUrlByYoutubeVideoFeedType(string videoId, YoutubeHelper.FeedType youtubeVideoDataFormat)
    {
      string str = (string) null;
      switch (youtubeVideoDataFormat)
      {
        case YoutubeHelper.FeedType.Atom:
          str = string.Format(YoutubeHelper.VideoDataUrlTemplateByFeedType, (object) videoId, (object) "atom");
          break;
        case YoutubeHelper.FeedType.Json:
          str = string.Format(YoutubeHelper.VideoDataUrlTemplateByFeedType, (object) videoId, (object) "json");
          break;
        case YoutubeHelper.FeedType.JsonInScript:
          str = string.Format(YoutubeHelper.VideoDataUrlTemplateByFeedType, (object) videoId, (object) "json-in-script");
          break;
        case YoutubeHelper.FeedType.Rss:
          str = string.Format(YoutubeHelper.VideoDataUrlTemplateByFeedType, (object) videoId, (object) "rss");
          break;
      }
      return str;
    }

    private static YoutubeVideoRecord GetYoutubeVideoRecordUrlTypeJson(string youtubeVideoUrl)
    {
      string id = !youtubeVideoUrl.Contains("/shared?") ? YoutubeHelper.GetYoutubeVideoIdByUrl(youtubeVideoUrl) : Regex.Match(HttpClientEx.GetString(youtubeVideoUrl), "\"video_id\":\"([A-Za-z0-9_-]+)\"", RegexOptions.Singleline).Groups[1].Value;
      string thumbnailUrl = string.Format(YoutubeHelper.ThumbnailUrlTemplate, (object) id);
      WebClient webClient = new WebClient()
      {
        Encoding = Encoding.UTF8
      };
      YoutubeHelper.YoutubeEntry youtubeEntry = (YoutubeHelper.YoutubeEntry) null;
      while (true)
      {
        string address = string.Format(YoutubeHelper.VideoDataUrlTemplateByFeedType, (object) id, (object) YoutubeHelper.YoutubeSearch.ApiKey);
        try
        {
          string json = webClient.DownloadString(address);
          if (!string.IsNullOrWhiteSpace(json))
          {
            youtubeEntry = new YoutubeHelper.YoutubeEntry(json);
            break;
          }
          break;
        }
        catch (Exception ex)
        {
          WebException webException = ex as WebException;
          if (webException != null)
          {
            if (((HttpWebResponse) webException.Response).StatusCode == HttpStatusCode.Forbidden)
            {
              Log.Info("Bad auth key. Setting up new one", (Exception) null);
              YoutubeHelper.YouTubeAuthKeys.Remove(YoutubeHelper.YoutubeSearch.ApiKey);
              if (YoutubeHelper.YouTubeAuthKeys.Count > 0)
                YoutubeHelper.YoutubeSearch.ApiKey = YoutubeHelper.YouTubeAuthKeys.ElementAt<string>(new Random().Next(0, YoutubeHelper.YouTubeAuthKeys.Count));
              else
                break;
            }
            else
              break;
          }
          else
            break;
        }
      }
      if (youtubeEntry == null)
        return new YoutubeVideoRecord(id, youtubeVideoUrl, (string) null, (string) null, 0.0, thumbnailUrl, YoutubeHelper.GetVideoThumbnailByThumbnailUrl(thumbnailUrl));
      return new YoutubeVideoRecord(id, youtubeVideoUrl, youtubeEntry.Title, youtubeEntry.Description, youtubeEntry.Duration * 1000.0, thumbnailUrl, YoutubeHelper.GetVideoThumbnailByThumbnailUrl(thumbnailUrl));
    }

    private static YoutubeVideoRecord GetYoutubeVideoRecordUrlTypeRss(string youtubeVideoUrl)
    {
      string videoId = YoutubeHelper.GetYoutubeVideoIdByUrl(youtubeVideoUrl);
      string youtubeVideoFeedType = YoutubeHelper.GetUrlByYoutubeVideoFeedType(videoId, YoutubeHelper.FeedType.Rss);
      XNamespace media = (XNamespace) "http://search.yahoo.com/mrss/";
      XNamespace yt = (XNamespace) "http://gdata.youtube.com/schemas/2007";
      return XDocument.Load(youtubeVideoFeedType).Descendants(media + "group").Select<XElement, YoutubeVideoRecord>((Func<XElement, YoutubeVideoRecord>) (i => new YoutubeVideoRecord(videoId, youtubeVideoUrl, i.Element(media + "title").Value, i.Element(media + "description").Value, YoutubeHelper.GetMicroseconds(i.Element(yt + "duration").Attribute((XName) "seconds").Value), i.Elements(media + "thumbnail").Select<XElement, string>((Func<XElement, string>) (e => (string) e.Attribute((XName) "url"))).First<string>(), YoutubeHelper.GetVideoThumbnailByThumbnailUrl(i.Elements(media + "thumbnail").Select<XElement, string>((Func<XElement, string>) (e => (string) e.Attribute((XName) "url"))).First<string>())))).FirstOrDefault<YoutubeVideoRecord>();
    }

    private static double GetMicroseconds(string time)
    {
      double result;
      if (double.TryParse(time, out result))
        result *= 1000.0;
      return result;
    }

    private static Image GetVideoThumbnailByThumbnailUrl(string thumbnailUrl)
    {
      Image image = (Image) null;
      if (string.IsNullOrWhiteSpace(thumbnailUrl))
        return (Image) null;
      try
      {
        image = Image.FromStream((Stream) new MemoryStream(new WebClient().DownloadData(thumbnailUrl)));
      }
      catch
      {
      }
      return image;
    }

    public enum FeedType
    {
      Atom,
      Json,
      JsonInScript,
      Rss,
    }

    private class YoutubeEntry
    {
      public string Description { get; private set; }

      public string Title { get; private set; }

      public double Duration { get; private set; }

      public YoutubeEntry(string json)
      {
        JObject jobject = JObject.Parse(json);
        JToken jtoken = jobject["items"].First[(object) "snippet"];
        this.Title = jtoken[(object) "title"].ToString();
        this.Description = jtoken[(object) "description"].ToString();
        this.Duration = XmlConvert.ToTimeSpan(jobject["items"].First[(object) "contentDetails"][(object) "duration"].ToString()).TotalSeconds;
      }
    }
  }
}
