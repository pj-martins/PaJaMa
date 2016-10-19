//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Drinks Mixer")]
//    public class DrinksMixerCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.drinksmixer.com"; }
//        }

//        protected override string allURL
//        {
//            get { return string.Empty; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "<a href=\"/cat/\\d*/(\\d*)/\">",
//                    URLFormat = "{0}/"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a href=\"(/drink.*?\\.html)\">(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "instructions\">(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<div class=\"ratingsBox rating\">rating<div><div .*?>(.*?)</div>"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return string.Empty; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<span class=\"ingredient\">(.*?)<br>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "_NONE_"; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "<a href=\"(/cat/.*?/)\">.*?</a>");
//            return mc.OfType<Match>().Select(m => baseURL + m.Groups[1].Value).ToList();
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return matchedRating / 2;
//        }
//    }
//}
