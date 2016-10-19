//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Girl Carnivore")]
//    public class GirlCarnivoreCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://girlcarnivore.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/recipes/"; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<h2><a href=\"(.*?)\">(.*?)</a></h2>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "itemprop=\"recipeInstructions\">(.*?)</li>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "_NONE_"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "_NONE_"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "itemprop=\"image\" src=\"(.*?)\""; }
//        }
//    }
//}
