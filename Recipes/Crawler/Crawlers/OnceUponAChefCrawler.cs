//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Once Upon A Chef", IgnoreAuto = true)]
//    public class OnceUponAChefCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.onceuponachef.com"; }
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
//            get { return "<li class=\"instruction\">(.*?)</li>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li itemprop=\"ingredients\" class=\"ingredient\">(.*?)</li>"; }
//        }

//        protected override string quantityRegexPattern
//        {
//            get { return "<span class=\"amount\">(.*?)</span>"; }
//        }

//        protected override string ingredientRegexPattern
//        {
//            get { return "<span class=\"name\">(.*)"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<p style=\"text-align:center;\"><img .*? src=\"(.*?)\" class=\"attachment.*? /></p>"; }
//        }
//    }
//}
