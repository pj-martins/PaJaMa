//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("101 Cookbooks")]
//    public class _101CookbooksCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.101cookbooks.com"; }
//        }

//        protected override string allURL
//        {
//            get { return string.Empty; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<div class=\"cat-blurb\"><a href=\"(.*?)\">(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "</blockquote>(.*?)<p class=\"recipeend\">"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "NONE"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "((>Serves .*?<)|(>Makes .*?<))"; }
//        }

//        protected override string ingredientSectionRegexPattern
//        {
//            get { return "<blockquote>(.*?)</blockquote>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return string.Empty; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<meta property=\"og:image\" content=\"(.*?)\"/>"; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            List<string> keywordPages = new List<string>();
//            MatchCollection mc = Regex.Matches(html, "<ul class=\"navitems\">(.*?)</ul>", RegexOptions.Singleline);
//            foreach (Match m in mc)
//            {
//                MatchCollection mc2 = Regex.Matches(m.Groups[1].Value, "<a href=\"(.*?)\">.*?</a>", RegexOptions.Singleline);
//                foreach (Match m2 in mc2)
//                {
//                    string keywordPage = m2.Groups[1].Value;
//                    if (keywordPage.Contains("id=\"toggle"))
//                        continue;
//                    if (!keywordPage.StartsWith(baseURL))
//                    {
//                        if (keywordPage.StartsWith("http://"))
//                            continue;
//                        keywordPage = baseURL + keywordPage;
//                    }
//                    keywordPages.Add(keywordPage);
//                }
//            }

//            return keywordPages;
//        }

//        protected override List<string> getIngredientLines(string html)
//        {
//            return html.Split(new string[] { "<br />" }, StringSplitOptions.RemoveEmptyEntries).Select(l => PaJaMa.Common.Common.StripHTML(l)).ToList();
//        }
//    }
//}
