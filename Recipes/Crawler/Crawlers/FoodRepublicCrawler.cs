//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Food Republic", IgnoreAuto = true)]
//    public class FoodRepublicCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://foodrepublic.com"; }
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
//            get { return "Directions:&nbsp;</div>.*?<ol>(.*?)</ol>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<div class=\"ingredient recipe-ingredient\">(.*?)</div>"; }
//        }

//        protected override string quantityRegexPattern
//        {
//            get { return "<span class=\"amount\">(.*?)</span>"; }
//        }

//        protected override string ingredientRegexPattern
//        {
//            get { return "<span class=\"name\">(.*)"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<div class=\"image\">.*?<img src=\"(.*?)\" .*?</div>"; }
//        }
//    }
//}
