//using PaJaMa.Common;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Knorr", IgnoreAuto = true)]
//    public class KnorrCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://knorr.com/"; }
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
//            get { return "<ul class=\"directions\" itemprop=\"recipeInstructions\">(.*?)</ul>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<span class=\"rating\" itemprop=\"ratingValue\">(.*?)</span>"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<meta property=\"og:image\" content=\"(.*?)\" />"; }
//        }
//    }
//}
