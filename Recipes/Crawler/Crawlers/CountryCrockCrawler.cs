//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Country Crock", IgnoreAuto = true)]
//    public class CountryCrockCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://countrycrock.com"; }
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
//            get { return "itemprop=\"recipeInstructions\">(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span class=\"serving-size\">.*?<span class=\"value\">(.*?)</span></span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img src=\"(.*?)\" width=\".*?\" alt=\".*?\" title=\".*?\" itemprop=\"image\" /> "; }
//        }
//    }
//}
