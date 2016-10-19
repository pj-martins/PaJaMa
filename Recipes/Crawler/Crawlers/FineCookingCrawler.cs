//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Fine Cooking")]
//    public class FineCookingCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.finecooking.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/browseall.aspx"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "<span class=pag>.*? of (.*?)</span>",
//                    URLFormat = "?cp={0}"
//                };
//            }
//        }

//        protected override RegexOptions recipeMatchRegexOptions
//        {
//            get { return RegexOptions.Multiline; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a href=\"(/recipes/.*?)\\?nterms=.*?\">(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "itemprop=\"recipeInstructions\">(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "property=\"og:rating\" content=\"(.*?)\""; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "itemprop=\"recipeYield\">(.*?)</p>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "image-primary\">.*?<img src=\"(.*?)\" alt=\".*?\" itemprop=\"image\""; }
//        }

//        protected override int getMaxPage(float maxPageMatch)
//        {
//            return (int)Math.Ceiling(maxPageMatch / 18);
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "<li><a href=\"(/recipes/.*?)\">.*?</a></li>");
//            var keywords = mc.OfType<Match>().Select(m => baseURL + m.Groups[1].Value).ToList();
//            keywords.Remove("http://www.finecooking.com/recipes/");
//            return keywords;
//        }

//        [DebuggerNonUserCode()]
//        protected override string getHTML(string url)
//        {
//            string html = string.Empty;
//            int tries = 3;
//            while (tries > 0)
//            {
//                try
//                {
//                    WebClient objWebClient = new WebClient();
//                    objWebClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//                    html = objWebClient.DownloadString(url);
//                    break;
//                }
//                catch
//                {
//                    tries--;
//                    System.Threading.Thread.Sleep(100);
//                }
//            }
//            return html;
//        }
//    }
//}
