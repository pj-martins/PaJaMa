//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("McCormick")]
//    public class McCormickCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.mccormick.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/Recipes"; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<div class=\"info\"><a href=\"(/Recipes/.*?)\" ><h3>(.*?)</h3>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "itemprop=\"recipeInstructions\">(.*?)</span>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "itemprop=\"recipeYield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "itemprop=\"image\" src=\"(.*?)\""; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "<a href=\"(/Recipes/.*?)\" ><h3>.*?</h3>", RegexOptions.RightToLeft);
//            return (from m in mc.OfType<Match>()
//                    select baseURL + m.Groups[1].Value).ToList();
//        }

//        protected override float? getRating(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "icon icon-star on");
//            return mc.Count;
//        }
//    }
//}
