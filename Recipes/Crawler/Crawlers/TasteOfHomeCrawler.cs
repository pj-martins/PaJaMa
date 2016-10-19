//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Taste Of Home")]
//    public class TasteOfHomeCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.tasteofhome.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/recipes/ingredients"; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a class=\"\" href=\"(/recipes/.*?)\"  title=\"(.*?)\""; }
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

//        protected override RegexOptions recipeMatchRegexOptions
//        {
//            get { return RegexOptions.Multiline; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<span class=\"rd_name\">(.*?)</span>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<li id=\"top_avg_rating\" class=\"rd_ratedstar\" style=\"width:(.*?)%;\">"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span itemprop=\"recipeyield\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<span class=\"rd_name\" itemprop=\"ingredients\">(.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img class=\"photo\" alt=\".*?\" title=\".*?\" itemprop=\"image\" src=\"(.*?)\"/>"; }
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return 5 * matchedRating / 100;
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            List<string> keywordPages = new List<string>();
//            MatchCollection mc = Regex.Matches(html, "href=\"(http://www.tasteofhome.com/recipes/ingredients/.*?)\"");
//            List<string> parents = new List<string>();
//            foreach (Match m in mc)
//            {
//                if (parents.Contains(m.Groups[1].Value))
//                    continue;
//                parents.Add(m.Groups[1].Value);
//                string html2 = getHTML(m.Groups[1].Value);
//                MatchCollection mc2 = Regex.Matches(html2, "<a href=\"(" + m.Groups[1].Value + "/.*?)\"");
//                foreach (Match m2 in mc2)
//                {
//                    keywordPages.Add(m2.Groups[1].Value);
//                }
//            }

//            return keywordPages;
//        }

//        protected override void updateMaxPage(string html, ref int maxPage)
//        {
//            MatchCollection mc = Regex.Matches(html, "\\?page=(\\d*)");
//            int tempInt = 0;
//            var pageNums = from m in mc.OfType<Match>()
//                           where int.TryParse(m.Groups[1].Value, out tempInt)
//                           select int.Parse(m.Groups[1].Value);
//            if (pageNums.Any())
//                maxPage = pageNums.Max();
//        }

//        /*
//        public static void Crawl(DataFactory df)
//        {
//            WebClient wc = new WebClient();
//            string html = wc.DownloadString("http://www.tasteofhome.com/recipes/ingredients");

//            string src = "Taste Of Home";
//            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;
//            List<string> recURLs = df.GetDataTable("select RecipeURL from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
//                .Select(r => r["RecipeURL"].ToString()).ToList();

//            MatchCollection mc = Regex.Matches(html, "<a href=\"(http://www.tasteofhome.com/recipes/ingredients/.*?)\"", RegexOptions.IgnoreCase);
//            foreach (Match m in mc)
//            {
//                html = wc.DownloadString(m.Groups[1].Value);
//                MatchCollection mc2 = Regex.Matches(html, "<a href=\"(" + m.Groups[1].Value + "/.*?)\"");
//                foreach (Match m2 in mc2)
//                {
//                    string baseURL = m2.Groups[1].Value;
//                    html = wc.DownloadString(baseURL);
//                    crawlPage(df, html, wc, recURLs, srcID, 1);

//                    int page = 2;
//                    while (true)
//                    {
//                        try
//                        {
//                            html = wc.DownloadString(baseURL + "?page=" + (page++).ToString());
//                        }
//                        catch
//                        {
//                            break;
//                        }
//                        if (!crawlPage(df, html, wc, recURLs, srcID, page - 1))
//                            break;
//                    }
//                }
//            }


//        }

//        private static bool crawlPage(DataFactory df, string html, WebClient wc, List<string> recURLs, int srcID, int pageNum)
//        {
//            string pattern = "href=\"(/recipes/.*?)\"  title=\"(.*?)\"";

//            MatchCollection mc = Regex.Matches(html, pattern);

//            if (mc.Count < 1)
//                return false;

//            foreach (Match m in mc)
//            {
//                string url = "http://www.tasteofhome.com" + m.Groups[1].Value;
//                string display = "Page " + pageNum + " - " + m.Groups[2].Value;
//                if (recURLs.Contains(url))
//                {
//                    Console.WriteLine(display);
//                    continue;
//                }

//                Console.WriteLine("* " + display);

//                html = wc.DownloadString(url.Replace("&#233;", "é"));

//                Match m2 = Regex.Match(html, "<meta Property=\"og:title\" content=\"(.*?)\" />");

//                Recipe rec = df.CreateDataItem<Recipe>();
//                rec.RecipeSourceID = srcID;
//                rec.RecipeName = m2.Groups[1].Value;
//                rec.RecipeURL = url;

//                m2 = Regex.Match(html, "<ol class=\"rd_directions\">(.*?)</ol>", RegexOptions.Singleline);
//                if (!m2.Success)
//                    continue;
				
//                MatchCollection mc2 = Regex.Matches(m2.Groups[1].Value, "<span class=\"rd_name\">(.*?)</span>");
//                if (mc2.Count < 1)
//                    throw new NotImplementedException();

//                rec.Directions = "";
//                foreach (Match m3 in mc2)
//                {
//                    rec.Directions += m3.Groups[1].Value.Trim() + "\r\n";
//                }
//                rec.Directions = rec.Directions.Trim();

//                m2 = Regex.Match(html, "<li id=\"top_avg_rating\" class=\"rd_ratedstar\" style=\"width:(.*?)%;\">", RegexOptions.Singleline);
//                if (!m2.Success)
//                    throw new NotImplementedException();

//                if (!string.IsNullOrEmpty(m2.Groups[1].Value))
//                    rec.Rating = 5 * float.Parse(m2.Groups[1].Value.Trim()) / 100;

//                m2 = Regex.Match(html, @">MAKES:</span><span>(.*?) servings</span>", RegexOptions.Singleline);
//                if (m2.Success)
//                {
//                    if (m2.Groups[1].Value.Contains('-'))
//                        rec.NumberOfServings = int.Parse(m2.Groups[1].Value.Substring(0, m2.Groups[1].Value.IndexOf("-")).Trim());
//                    else
//                        rec.NumberOfServings = int.Parse(m2.Groups[1].Value.Trim());
//                }
//                else
//                {
//                    m2 = Regex.Match(html, @"<li class=""credit"">.*?<span class=""credit-label"">Yield:</span>(.*?)</li>", RegexOptions.Singleline);
//                    if (m2.Success)
//                    {
//                        Match ms = Regex.Match(m2.Groups[1].Value, "(\\d+)");
//                        if (ms.Success)
//                            rec.NumberOfServings = int.Parse(ms.Groups[1].Value.Trim());
//                    }
//                }


//                m2 = Regex.Match(html, "<ul class=\"rd_ingredients\">(.*?)</ul>", RegexOptions.Singleline);
//                if (!m2.Success)
//                    throw new NotImplementedException();
//                mc2 = Regex.Matches(m2.Groups[1].Value, "<span class=\"rd_name\" itemprop=\"ingredients\">(.*?)</span>");
				
//                foreach (Match m3 in mc2)
//                {
//                    Tuple<string, Measurement, float> ingr = CrawlerHelper.GetIngredientQuantity(df, m3.Groups[1].Value, false, true);

//                    CrawlerHelper.FillIngredient(rec, ingr.Item1, ingr.Item2, ingr.Item3);
//                }

//                m2 = Regex.Match(html, "<img class=\"photo\" alt=\".*?\" title=\".*?\" itemprop=\"image\" src=\"(.*?)\"/>");
//                if (m2.Success)
//                {
//                    RecipeImage img = df.CreateDataItem<RecipeImage>();
//                    img.ImageURL = m2.Groups[1].Value;
//                    if (img.ImageURL.StartsWith("//"))
//                        img.ImageURL = "http:" + img.ImageURL;
//                    img.LocalImagePath = null;
//                    rec.RecipeRecipeImages.Add(img);
//                }

//                CrawlerHelper.SaveRecipe(rec);

//                recURLs.Add(rec.RecipeURL);
//            }

//            return true;
//        }
//         */
//    }
//}
