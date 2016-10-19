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
//    [RecipeSource("1001 Cocktails (French)", StartPage = 0)]
//    public class _1001CocktailsFrenchCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.1001cocktails.com"; }
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
//                    MaxPageRegexPattern = "<a href=\"\\d*\\-liste\\-cocktails.*?\">(\\d*)</a>"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<strong><a href=\"(/cocktails/.*?)\" title=\"Cocktail .*?\">(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<span itemprop=\"recipeInstructions\".*?><br />&bull;(.*?)<br />&bull;(.*?)<br"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<span id=\"rabidRating\\-.*?\\-((?:\\d*\\.)?\\d+)_5\" class=\"rabidRating\""; }
//        }

//        protected override string ingredientSectionRegexPattern
//        {
//            get { return "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">(.*)<td valign=\".*?\">"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<td>(.*?)</td>"; }
//        }

//        protected override string ingredientRegexPattern
//        {
//            get { return ".*?itemprop=\"ingredients\">(.*?)</a>"; }
//        }

//        protected override string quantityRegexPattern
//        {
//            get { return "</a>(.*? <a href=\"http://www.1001cocktails.com/unites-mesure.php\".*?>.*?)</a>.*?itemprop=\"ingredients\">"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img src=\"(.*?)\".*?itemprop=\"image\""; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            List<string> keywordPages = new List<string>();

//            List<char> chars = new List<char>();

//            for (int i = 0; i < 10; i++)
//            {
//                chars.Add(i.ToString()[0]);
//            }

//            for (int i = 97; i <= 122; i++)
//            {
//                chars.Add((char)i);
//            }

//            foreach (char c in chars)
//            {
//                keywordPages.Add(string.Format("http://www.1001cocktails.com/cocktails/liste-cocktails-commencant-par-{0}.html", c));
//            }

//            return keywordPages;
//        }

//        protected override string getPageURL(string keywordPage, int pageNum)
//        {
//            return string.Format(keywordPage.Replace("cocktails/liste", "cocktails/{0}liste"), (pageNum == 0 ? "" : (pageNum * 50).ToString() + "-"));
//        }

//        #region OLD
//        /*
//        public static void Crawl(DataFactory df)
//        {
//            string src = "1001cocktails.com (french)";
//            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;

//            List<string> recNames = df.GetDataTable("select RecipeName from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
//                .Select(dr => dr["RecipeName"].ToString()).ToList();

//            WebClient wc = new WebClient();
//            for (int i = (int)'t'; i <= 122; i++)
//            {
//                string url = i == 96 ? "http://www.1001cocktails.com/cocktails/lister_cocktails.php3"
//                    : string.Format("http://www.1001cocktails.com/cocktails/liste-cocktails-commencant-par-{0}.html", (char)i);
//                string html = wc.DownloadString(url);

//                MatchCollection mc = Regex.Matches(html, "<a href=\"(\\d*)\\-liste\\-cocktails");
//                int temp;
//                List<int> starts = (from m in mc.OfType<Match>()
//                                    where int.TryParse(m.Groups[1].Value, out temp)
//                                    select int.Parse(m.Groups[1].Value)).Distinct().ToList();
//                starts.Insert(0, 0);
//                foreach (int start in starts)
//                {
//                    html = wc.DownloadString(string.Format(url.Replace("cocktails/liste", "cocktails/{0}liste"), (start == 0 ? "" : start.ToString() + "-")));
//                    mc = Regex.Matches(html, "<strong><a href=\"(/cocktails/.*?)\" title=\"Cocktail .*?\">(.*?)</a>", RegexOptions.Singleline);
//                    foreach (Match m in mc)
//                    {
//                        string recipeName = CrawlerHelper.ChildSafeName(m.Groups[2].Value);
//                        Console.WriteLine(((char)i).ToString() + " - " + (starts.IndexOf(start) + 1).ToString() + " of " + starts.Count.ToString() + " - " + recipeName);

//                        if (recNames.Contains(recipeName))
//                            continue;
//                        //if (CrawlerHelper.RecipeExists(df, recipeName, srcID))
//                        //	continue;

//                        Recipe rec = df.CreateDataItem<Recipe>();
//                        rec.RecipeSourceID = srcID;
//                        rec.RecipeName = recipeName;
//                        rec.RecipeURL = "http://www.1001cocktails.com" + m.Groups[1].Value;

//                        int tries = 3;
//                        while (tries > 0)
//                        {
//                            try
//                            {
//                                html = wc.DownloadString(rec.RecipeURL);
//                                break;
//                            }
//                            catch
//                            {
//                                System.Threading.Thread.Sleep(1000);
//                                tries--;
//                            }
//                        }

//                        if (tries == 0)
//                            throw new Exception("ERROR");

//                        Match m2 = Regex.Match(html, "<span itemprop=\"recipeInstructions\".*?><br />&bull;(.*?)<br />&bull;(.*?)<br", RegexOptions.Singleline);
//                        rec.Directions = PaJaMa.Common.Common.StripHTML(m2.Groups[1].Value + "\r\n" + m2.Groups[2].Value);

//                        if (string.IsNullOrEmpty(rec.Directions))
//                            throw new NotImplementedException();

//                        m2 = Regex.Match(html, "<span id=\"rabidRating\\-.*?\\-((?:\\d*\\.)?\\d+)_5\" class=\"rabidRating\"");
//                        if (!string.IsNullOrEmpty(m2.Groups[1].Value))
//                            rec.Rating = Convert.ToSingle(m2.Groups[1].Value);

//                        m2 = Regex.Match(html, "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">(.*)<td valign=\".*?\">");
//                        MatchCollection mc2 = Regex.Matches(m2.Groups[1].Value, "</a>(.*?) <a href=\"http://www.1001cocktails.com/unites-mesure.php\"" +
//                            ".*?>(.*?)</a>.*?itemprop=\"ingredients\">(.*?)</a>");

//                        foreach (Match m3 in mc2)
//                        {
//                            Measurement measurement = null;
//                            MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//                            mf.MeasurementName = m3.Groups[2].Value.Trim().Replace(" de", "");
//                            if (!string.IsNullOrEmpty(mf.MeasurementName) && mf.MeasurementName != "de")
//                            {
//                                measurement = mf.GetDataItems().FirstOrDefault();
//                                if (measurement == null)
//                                {
//                                    measurement = df.CreateDataItem<Measurement>();
//                                    measurement.MeasurementName = mf.MeasurementName;
//                                    measurement.Save();
//                                }
//                            }

//                            float qty = 0;
//                            string qtyString = PaJaMa.Common.Common.StripHTML(m3.Groups[1].Value.Replace(",", "."));
//                            if (string.IsNullOrEmpty(qtyString))
//                            {

//                            }
//                            else
//                            {
//                                if (qtyString.Contains("/"))
//                                {
//                                    if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
//                                        throw new NotImplementedException();
//                                }
//                                else if (qtyString.Contains("Â"))
//                                {
//                                    if (!PaJaMa.Common.Common.TryParseFraction(qtyString.Replace("Â", " ").Replace("½", "1/2"), out qty))
//                                        throw new NotImplementedException();
//                                }
//                                else if (qtyString.Contains("-") || qtyString.Contains("=>") || qtyString.Contains("ou") || qtyString.Contains("Ã"))
//                                {
//                                    string[] parts = qtyString.Split(new string[] { "-", "=>", "ou", "Ã" }, StringSplitOptions.None);
//                                    if (parts.Length != 2)
//                                        throw new NotImplementedException();
//                                    float first = 0;
//                                    float second = 0;

//                                    if (!PaJaMa.Common.Common.TryParseFraction(parts[0], out first))
//                                        throw new NotImplementedException();

//                                    if (!PaJaMa.Common.Common.TryParseFraction(parts[1], out second))
//                                        throw new NotImplementedException();

//                                    qty = (first + second) / 2;
//                                }
//                                else if (qtyString.EndsWith("%"))
//                                    qty = Convert.ToSingle(qtyString.Replace("%", "")) / 100;
//                                else if (qtyString.ToLower() == "quelques" || qtyString == "reste" || qtyString == "quelq")
//                                    qty = 0;
//                                else if (qtyString == "un" || qtyString == "n")
//                                    qty = 1;
//                                else
//                                    qty = Convert.ToSingle(qtyString);
//                            }
//                            CrawlerHelper.FillIngredient(df, rec, m3.Groups[3].Value, measurement, qty);
//                        }

//                        if (!rec.RecipeRecipeIngredientMeasurements.Any())
//                            continue;

//                        m2 = Regex.Match(html, "<img src=\"(.*?)\".*?itemprop=\"image\"");
//                        RecipeImage img = df.CreateDataItem<RecipeImage>();
//                        img.ImageURL = m2.Groups[1].Value;
//                        if (!img.ImageURL.StartsWith("http://www.1001cocktails.com"))
//                            img.ImageURL = "http://www.1001cocktails.com" + img.ImageURL;
//                        img.LocalImagePath = null;
//                        rec.RecipeRecipeImages.Add(img);

//                        if (!rec.RecipeRecipeImages.Any())
//                            throw new NotImplementedException();

//                        CrawlerHelper.SaveRecipe(rec);
//                    }
//                }
//            }
//        }
		
//        private static bool roughMatch(string directions1, string directions2)
//        {
//            directions1 = directions1.Trim().ToLower().Replace(".", "").Replace("<br />", " ");
//            directions2 = directions2.Trim().ToLower().Replace(".", "").Replace("<br />", " ");
//            string[] parts1 = directions1.Split(' ');
//            string[] parts2 = directions2.Split(' ');
//            for (int i = 0; i < 3; i++)
//            {
//                if (i >= parts1.Length || i >= parts2.Length)
//                    return i > 0;

//                if (parts1[i] != parts1[i])
//                    return false;
//            }

//            return true;
//        }
//         */
//        #endregion

//        protected override string servingsRegexPattern
//        {
//            get { return "__NONE__"; }
//        }
//    }
//}
