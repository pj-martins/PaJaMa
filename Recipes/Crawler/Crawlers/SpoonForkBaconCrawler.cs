//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Spoon Fork Bacon", IgnoreAuto = true)]
//    public class SpoonForkBaconCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.spoonforkbacon.com"; }
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
//            get { return "property=\"og:image\" content=\"(.*?)\" />"; }
//        }
//    }
//}
