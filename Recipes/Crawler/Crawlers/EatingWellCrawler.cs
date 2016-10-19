//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Eating Well", StartPage = 0)]
//    public class EatingWellCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.eatingwell.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/recipes/browse_all_recipes"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "browse_all_recipes\\?page=(\\d*)",
//                    URLFormat = "?page={0}"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<div class=\"views-field-title\">.*?<span class=\"field-content\"><a href=\"(.*?)\" "; }
//        }

//        protected override string recipeNameRegexPattern
//        {
//            get { return "<meta name=\"page_name\" content=\"recipes:view:(.*?)\">"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<ol itemprop=\"recipeinstructions\">(.*?)</ol>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<input type=\"hidden\" name=\"vote_average\" id=\"edit-vote-average\" value=\"(.*?)\"  />"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span itemprop=\"recipeyield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "div id=\"recipeImage\">.*?<img src=\"(.*?)\""; }
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return matchedRating / 20;
//        }
//    }
//}
