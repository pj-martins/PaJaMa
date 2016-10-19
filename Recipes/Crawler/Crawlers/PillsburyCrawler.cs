//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Pillsbury", IgnoreAuto = true)]
//    public class PillsburyCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.pillsbury.com"; }
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
//            get { return "<div class=\"recipesteppedlistcontainer\">(.*?)</ul>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "itemprop=\"recipeInstructions\">(.*?)</li>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "\"AverageRating\":\"(.*?)\""; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "\"NumberOfServings\":\"(.*?)\""; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "itemprop=\"ingredients\">(.*?)</dl>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "property=\"og:image\" content=\"(.*?)\" />"; }
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return matchedRating * 5;
//        }

//        protected override List<string> getIngredientLines(string html)
//        {
//            var lines = base.getIngredientLines(html);
//            var newLines = new List<string>();
//            foreach (string line in lines)
//            {
//                newLines.Add(PaJaMa.Common.Common.StripHTML(line).Trim().Replace("\r\n                        \r\n                    \r\n                    \r\n                        \r\n                            \r\n                                \r\n                            \r\n                            ", " "));
//            }
//            return newLines;
//        }

//        protected override string getDirections(string html)
//        {
//            return base.getDirections(html).Replace("                               \r\n\r\n                                   ", "").Replace("\r\n                                   ", ". ");
//        }
//    }
//}
