//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Martha Stewart", StartPage = 0)]
//    public class MarthaStewartCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.marthastewart.com"; }
//        }

//        protected override string allURL
//        {
//            get { return string.Empty; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return @"<h4 class=""topic-info-genre"">recipe</h4>.*?<h2 class=""topic-info-title"" itemprop=""headline""><a href=""(.*?)"" itemprop=""url"""; }
//        }

//        protected override string recipeNameRegexPattern
//        {
//            get { return "<meta property=\"og:title\" content=\"(.*?)\" />"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<p class=\"recipe-step\">(.*?)</p>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return @"<span itemprop=""ratingValue"" style=""display:none"">.*?<!-- esi -->(.*?)<!-- /esi -->"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return @"<li class=""credit"">.*?<span class=""credit-label"">.*?</span>(.*?)</li>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img src=\"(.*?)\" alt=\".*?\" title=\".*?\" class=\"feat-primary-img\" itemprop=\"image\">"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "\\?page=(\\d+).*?\"",
//                    URLFormat = "?page={0}"
//                };
//            }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            html = webClient.DownloadString("http://www.marthastewart.com/1067188/key-ingredients");
//            int maxPage = (from pm in Regex.Matches(html, "\\?page=(\\d*)").OfType<Match>()
//                           select int.Parse(pm.Groups[1].Value)).Max();

//            List<string> keywordPages = new List<string>();
//            for (int i = 0; i <= maxPage; i++)
//            {
//                html = webClient.DownloadString("http://www.marthastewart.com/1067188/key-ingredients?page=" + i.ToString());
//                MatchCollection mc = Regex.Matches(html, "<a href=\"(.*?)\" data-linktitle-override=\"(.*?)\"", RegexOptions.IgnoreCase);
//                foreach (Match m in mc)
//                {
//                    string url = "http://www.marthastewart.com" + m.Groups[1].Value;
//                    if (url.Contains("target=\""))
//                        continue;
//                    keywordPages.Add(url);
//                }
//            }

//            return keywordPages;
//        }
//    }
//}
