//using PaJaMa.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("I Adore Food!", IgnoreAuto = true)]
//    public class IAdoreFoodCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://steamykitchen"; }
//        }

//        protected override string allURL
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "class=\"instructions\"><ol>(.*?)</ol>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li><span.*?itemprop=\"ingredient\".*?>(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "class=\"align size- wp-image- photo\" title=\".*?\" src=\"(.*?)\""; }
//        }

//        protected override string getHTML(string url)
//        {
//            return new GZipWebClient().DownloadString(url);
//        }
//    }
//}
