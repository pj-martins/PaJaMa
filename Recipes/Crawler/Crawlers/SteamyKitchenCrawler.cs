//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Steamy Kitchen", IgnoreAuto = true)]
//    public class SteamyKitchenCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://steamykitchen.com"; }
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
//            get { return "<span itemprop=\"recipeInstructions\">(.*?)</span>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span itemprop=\"recipeYield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<span itemprop=\"ingredients\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<div class=\"entry-thumb\"><img .*? src=\"(.*?)\""; }
//        }
//    }
//}
