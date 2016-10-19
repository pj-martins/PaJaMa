//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Daisy Brand Sour Cream", IgnoreAuto = true)]
//    public class DaisyBrandCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.daisybrand.com"; }
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
//            get { return "<p itemprop=\"recipeInstructions\">(.*?)</p>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<div class=\"star\\-rating rating\\-(.*?)\"></div>"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span itemprop=\"recipeYield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "property=\"og:image\" content=\"(.*?)\" />"; }
//        }
//    }
//}
