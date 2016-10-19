//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Comfort Food", StartPage = 0)]
//    public class ComfortFoodCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.guideposts.org"; }
//        }

//        protected override string allURL
//        {
//            get { return "/inspiration/recipes"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "\\?page=(\\d*)",
//                    URLFormat = "?page={0}"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "views-field-title\">.*?<span class=\"field-content\"><a href=\"(.*?)\">(.*?)</a>"; }
//        }

//        protected override string directionsSectionRegexPattern
//        {
//            get { return "Preparation</h3>(.*?)</div>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<p>(.*?)</p>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<strong>Serves (.*?)</strong>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "property=\"og:image\" content=\"(.*?)\""; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "views-field-name\">.*?<span class=\"field-content\"><a href=\"(.*?)\">", RegexOptions.Singleline);
//            return mc.OfType<Match>().Select(m => baseURL + m.Groups[1].Value).ToList();
//        }

//        protected override List<string> getIngredientLines(string html)
//        {
//            MatchCollection mc0 = Regex.Matches(html, "Ingredients</h3>(.*?)<h3", RegexOptions.Singleline);
//            if (mc0.Count < 1)
//                return null;

//            List<string> lines = new List<string>();

//            foreach (Match m0 in mc0)
//            {
//                string html2 = m0.Groups[1].Value;
//                var mc = Regex.Matches(html2, "<td.*?>(.*?)</td>", RegexOptions.Singleline).OfType<Match>().ToList();
//                mc.AddRange(Regex.Matches(html2, "<p>(.*?)</p>", RegexOptions.Singleline).OfType<Match>());

//                if (mc.Count < 1)
//                    mc = Regex.Matches(html2, "(.*?)<br />", RegexOptions.Multiline).OfType<Match>().ToList();


//                lines.AddRange(from m in mc.OfType<Match>()
//                               from p in m.Groups[1].Value.Split(new string[] { "<br>", "<br />", "\n" }, StringSplitOptions.RemoveEmptyEntries)
//                               select p);
//            }

//            return lines
//                .Where(x => !x.ToLower().Contains("<strong>") && x.ToLower() != "&nbsp;")
//                .Select(x => PaJaMa.Common.Common.StripHTML(x))
//                .Select(x => x
//                    .Replace("&frac12;", " 1/2")
//                    .Replace("&frac14;", " 1/4")
//                    .Replace("&frac34;", " 3/4")
//                    .Replace("â…“", "1/3")
//                    .Trim()
//                )
//                .Where(x => !string.IsNullOrEmpty(x)).ToList();
//        }
//    }
//}
