//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Campbell")]
//    public class CampbellCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.campbellskitchen.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/SearchResults.aspx?q="; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "s.prop50=\"(\\d*)\"",
//                    URLFormat = "&page={0}"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<p class=\"recipeTitle\">.*?<a href=\"(/recipes/.*?)\".*?title=\"(.*?)\""; }
//        }

//        protected override string directionsSectionRegexPattern
//        {
//            get { return "<div class=\"instructions\">(.*?)</div>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<p>(.*?)</p>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<span class=\"average\">.*?<span class=\"value-title\" title=\"(.*?)\" />"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "class=\"ingredient\">(.*?)<br />"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "class=\"photo\" src=\"(.*?)\""; }
//        }

//        protected override float getRating(float matchedRating)
//        {
//            if (matchedRating == 3) return 0;
//            return matchedRating;
//        }
//    }
//}
