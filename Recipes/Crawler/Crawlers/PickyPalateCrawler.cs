//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Picky Palate", IgnoreAuto = true)]
//    public class PickyPalateCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://picky-palate.com"; }
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
//            get { return "<span class=\"yield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li class=\"ingredient\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img class=\"aligncenter size-full wp-image-.*?\" alt=\".*?\" src=\"(.*?)\" .*?>"; }
//        }
//    }
//}
