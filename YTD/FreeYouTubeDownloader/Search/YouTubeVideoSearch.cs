// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.YouTubeVideoSearch
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Youtube;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FreeYouTubeDownloader.Search
{
    public sealed class YouTubeVideoSearch : VideoSearch
    {
        private const string SearchQueryBaseUrl = "https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&fields=items%2Fid%2CnextPageToken";
        private const string ListVideosBaseUrl = "https://www.googleapis.com/youtube/v3/videos?part=contentDetails%2Csnippet&fields=items(contentDetails%2Cid%2Csnippet)";
        private string _apiKey;
        private string _pageToken;
        private uint _maxResults;
        private string _query;

        public override uint MaxResults
        {
            get
            {
                return this._maxResults;
            }
            set
            {
                if ((int)this._maxResults == (int)value)
                    return;
                if (value > 50U)
                    throw new ArgumentOutOfRangeException("MaxResults", "Should be in between 0 and 50");
                this._maxResults = value;
            }
        }

        public override string Query
        {
            get
            {
                return this._query;
            }
            set
            {
                if (this._query == value)
                    return;
                this._query = value;
                this._pageToken = (string)null;
            }
        }

        public string ApiKey
        {
            get
            {
                return this._apiKey;
            }
            set
            {
                if (this._apiKey == value)
                    return;
                this._apiKey = value;
            }
        }

        internal YouTubeVideoSearch()
        {
        }

        internal YouTubeVideoSearch(string apiKey)
          : this()
        {
            this._apiKey = apiKey;
        }

        public override List<VideoSearchResult> FetchResultSet()
        {
            return this.FetchResultSetInternal();
        }

        public override Task<List<VideoSearchResult>> FetchResultSetAsync()
        {
            return Task.Factory.StartNew<List<VideoSearchResult>>((Func<List<VideoSearchResult>>)(() => this.FetchResultSetInternal()));
        }

        private string BuildSearchQueryUrl()
        {
            StringBuilder stringBuilder = new StringBuilder("https://www.googleapis.com/youtube/v3/search?part=snippet&type=video&fields=items%2Fid%2CnextPageToken");
            stringBuilder.Append("&key=" + this._apiKey);
            stringBuilder.Append("&maxResults=" + (object)this.MaxResults);
            stringBuilder.Append("&q=" + this.Query);
            stringBuilder.Append("&order=" + (object)this.Order);
            stringBuilder.Append("&videoDefinition=" + (object)this.Definition);
            if (!string.IsNullOrEmpty(this._pageToken))
                stringBuilder.Append("&pageToken=" + this._pageToken);
            return stringBuilder.ToString();
        }

        private string BuildListVideosUrl(IEnumerable<string> videoIds)
        {
            StringBuilder stringBuilder = new StringBuilder("https://www.googleapis.com/youtube/v3/videos?part=contentDetails%2Csnippet&fields=items(contentDetails%2Cid%2Csnippet)");
            string str1 = "&key=" + this._apiKey;
            stringBuilder.Append(str1);
            string str2 = "&id=" + string.Join("%2C", videoIds);
            stringBuilder.Append(str2);
            return stringBuilder.ToString();
        }

        private void AnalyzeWebExceptionAndRethrow(WebException ex, string url)
        {
            if (ex.Response == null)
                return;
            Stream responseStream = ex.Response.GetResponseStream();
            if (responseStream == null)
                return;
            using (StreamReader streamReader = new StreamReader(responseStream))
            {
                string end = streamReader.ReadToEnd();
                if (string.IsNullOrEmpty(end))
                    return;
                JObject jsonResponse = JObject.Parse(end);
                JToken jtoken = jsonResponse["error"];
                if (jtoken != null)
                    throw new YouTubeSearchException(string.Format("Error {0} ({1}.{2}){3}{4}", (object)jtoken[(object)"code"].ToObject<int>(), (object)jtoken[(object)"errors"][(object)0][(object)"domain"].ToString(), (object)jtoken[(object)"errors"][(object)0][(object)"reason"].ToString(), (object)Environment.NewLine, (object)jtoken[(object)"errors"][(object)0][(object)"message"].ToString()), url, end, jsonResponse);
            }
        }

        private JObject GetJsonFromUrl(string url)
        {
            string str = (string)null;
            try
            {
                str = HttpClientEx.GetString(url);
                return JObject.Parse(str);
            }
            catch (WebException ex)
            {
                this.AnalyzeWebExceptionAndRethrow(ex, url);
                throw new SearchException(Strings.CantExecuteSearchQuery, (Exception)ex, url, (string)null);
            }
            catch (JsonException ex)
            {
                throw new SearchException("Error while reading reply from search", (Exception)ex, url, str);
            }
        }

        private List<VideoSearchResult> FetchResultSetInternal()
        {
            string url;
            JObject jobject;
            List<VideoSearchResult> videoSearchResultList;
            JArray source;
            while (true)
            {
                url = this.BuildSearchQueryUrl();
                try
                {
                    jobject = this.GetJsonFromUrl(url);
                }
                catch (YouTubeSearchException ex)
                {
                    jobject = ex.JsonResponse;
                }
                videoSearchResultList = new List<VideoSearchResult>();
                source = jobject["items"] as JArray;
                if (jobject["error"] != null && jobject["error"][(object)"code"].Value<int>() == 403)
                {
                    YoutubeHelper.YouTubeAuthKeys.Remove(this._apiKey);
                    if (YoutubeHelper.YouTubeAuthKeys.Count > 0)
                        this._apiKey = YoutubeHelper.YouTubeAuthKeys.ElementAt<string>(new Random().Next(0, YoutubeHelper.YouTubeAuthKeys.Count));
                    else
                        break;
                }
                else
                    goto label_7;
            }
            throw new SearchException("Search failed due to YouTube issues. Please report.", (Exception)new SearchException(jobject["error"][(object)"message"].ToString(), url, jobject.ToString()), (string)null, (string)null);
            label_7:
            if (source != null)
            {
                JToken jtoken1 = jobject["nextPageToken"];
                if (jtoken1 != null)
                    this._pageToken = jtoken1.ToString();
                videoSearchResultList.Capacity = source.Count;
                JArray jarray = this.GetJsonFromUrl(this.BuildListVideosUrl(source.Select<JToken, string>((Func<JToken, string>)(item => item[(object)"id"][(object)"videoId"].ToString()))))["items"] as JArray;
                if (jarray != null)
                {
                    foreach (JToken jtoken2 in jarray)
                    {
                        VideoSearchResult videoSearchResult = new VideoSearchResult();
                        videoSearchResult.Id = jtoken2[(object)"id"].ToString();
                        videoSearchResult.Url = "https://www.youtube.com/watch?v=" + videoSearchResult.Id;
                        videoSearchResult.Uploaded = jtoken2[(object)"snippet"][(object)"publishedAt"].ToObject<DateTime>();
                        videoSearchResult.Title = jtoken2[(object)"snippet"][(object)"title"].ToString();
                        videoSearchResult.Description = jtoken2[(object)"snippet"][(object)"description"].ToString();
                        videoSearchResult.Uploader = jtoken2[(object)"snippet"][(object)"channelTitle"].ToString();
                        videoSearchResult.HasHD = jtoken2[(object)"contentDetails"][(object)"definition"].ToString().ToLowerInvariant() == "hd";
                        videoSearchResult.Duration = XmlConvert.ToTimeSpan(jtoken2[(object)"contentDetails"][(object)"duration"].ToObject<string>());
                        IJEnumerable<JToken> jenumerable = jtoken2[(object)"snippet"][(object)"thumbnails"].Values();
                        videoSearchResult.Thumbnails = (IList<Thumbnail>)new List<Thumbnail>();
                        foreach (JToken jtoken3 in (IEnumerable<JToken>)jenumerable)
                            videoSearchResult.Thumbnails.Add(new Thumbnail()
                            {
                                Url = jtoken3[(object)"url"].ToString(),
                                Width = jtoken3[(object)"width"].ToObject<int>(),
                                Height = jtoken3[(object)"height"].ToObject<int>()
                            });
                        videoSearchResultList.Add(videoSearchResult);
                    }
                }
            }
            return videoSearchResultList;
        }
    }
}
