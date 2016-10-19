//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Simply Recipes")]
//    public class SimplyRecipesCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.simplyrecipes.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/index"; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<li id=\"post.*?<a href=\"(.*?)\".*?</li>"; }
//        }

//        protected override string recipeNameRegexPattern
//        {
//            get { return "class=\"entry-title\".*?>(.*?)</"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<div itemprop=\"recipeInstructions\">(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "NONE"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span class=\"yield\" itemprop=\"recipeYield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li class=\"ingredient\" itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<div class=\"featured-image\"><img   src=\"(.*?)\" class=\"photo"; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            Match m = Regex.Match(html, "<div id=\"recipe-index-list\">(.*?)</div>");
//            MatchCollection mc = Regex.Matches(m.Groups[1].Value, "href=\"(http://www.simplyrecipes.com/recipes.*?)\">.*?</a>");
//            return mc.OfType<Match>().Select(m2 => m2.Groups[1].Value).ToList();
//        }

//        protected override string getHTML(string url)
//        {
//            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)");
//            return base.getHTML(url);
//        }
//    }
//}
