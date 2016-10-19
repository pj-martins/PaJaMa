//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("EveryDay with Rachael Ray", IgnoreAuto = true)]
//    public class EveryDayRachaelRayCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.rachaelraymag.com"; }
//        }

//        protected override string allURL
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string directionsSectionRegexPattern
//        {
//            get { return "<ol class=\"instructions\">(.*?)</ol>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<li>(.*?)</li>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li class=\"ingredient\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<meta name=\"mdp:contentThumb\" content=\"(.*?)\">"; }
//        }

//        protected override float? getRating(string html)
//        {
//            Match m = Regex.Match(html, "<ul class=\"rating (.*?)\">");
//            switch (m.Groups[1].Value)
//            {
//                case "zero":
//                    return null;
//                case "half":
//                    return 0.5F;
//                case "one":
//                    return 1;
//                case "onehalf":
//                    return 1.5F;
//                case "two":
//                    return 2;
//                case "twohalf":
//                    return 2.5F;
//                case "three":
//                    return 3;
//                case "threehalf":
//                    return 3.5F;
//                case "four":
//                    return 4;
//                case "fourhalf":
//                    return 4.5F;
//                case "five":
//                    return 5;
//            }
//            throw new NotImplementedException();
//        }
//    }
//}
