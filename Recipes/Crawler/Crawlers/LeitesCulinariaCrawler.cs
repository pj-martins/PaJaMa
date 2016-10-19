//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Leite's Culinaria", IgnoreAuto = true)]
//    public class LeitesCulinariaCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://leitesculinaria.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/category/recipes"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "/page/(.*?)\"",
//                    URLFormat = "/page/{0}"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "entry\"><a href=\"(.*?)\" title=\"(.*?)\""; }
//        }

//        protected override string directionsSectionRegexPattern
//        {
//            get { return "<ul class=\"directions-list\">(.*?)</ul>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<li>(.*?)</li>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "itemprop=\"recipeYield\" class=\".*?\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li class=\"ingredient\" itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img itemprop=\"image\" .*? src=\"(.*?)\""; }
//        }
//    }
//}
