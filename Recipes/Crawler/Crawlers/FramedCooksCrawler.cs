//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Framed Cooks", IgnoreAuto = true)]
//    public class FramedCooksCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.framedcooks.com"; }
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
//            get { return "__NONE__"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</div>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "itemprop=\"image\" src=\"(.*?)\""; }
//        }
//    }
//}
