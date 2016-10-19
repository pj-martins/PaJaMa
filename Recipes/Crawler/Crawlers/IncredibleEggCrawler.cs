//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Incredible Egg", IgnoreAuto = true)]
//    public class IncredibleEggCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.incredibleegg.org"; }
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
//            get { return "<td class=\"direction\">(.*?)</td>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<meta itemprop=\"ratingValue\" content=\"(.*?)\" />"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span itemprop=\"recipeYield\">(.*?)</span>"; }
//        }

//        protected override string ingredientSectionRegexPattern
//        {
//            get { return "<table class=\"ingredients recipe_details-block\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">(.*?)</table>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<tr>(.*?)</tr>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img src=\"(.*?)\" alt=\".*?\" itemprop=\"image\""; }
//        }
//    }
//}
