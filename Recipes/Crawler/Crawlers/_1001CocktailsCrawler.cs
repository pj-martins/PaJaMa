//using PaJaMa.Common;

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
//    [RecipeSource("1001 Cocktails")]
//    public class _1001CocktailsCrawler : CrawlerBase
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
//                    MaxPageRegexPattern = "&start=(.*?)>.*?</a>",
//                    URLFormat = "&start={0}"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a href=\"(/recipes/.*?)\" title=\".*?\">.*?<b><u>(.*?)</u></b>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<p>(.*?)<b></b> <br>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<br>Rating:&nbsp;<span class=\"important2gras\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<TD CLASS=\"normal2\">(.*?)</td>"; }
//        }

//        protected override string ingredientRegexPattern
//        {
//            get { return "<a href=\".*?\" alt=\".*?\" title=\".*?\"><u>(.*?)</u></a>"; }
//        }

//        protected override string quantityRegexPattern
//        {
//            get { return "\\-\n(.*?) <a"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img src=\"(.*?)\" align=\"left\" alt=\""; }
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
//                keywordPages.Add(string.Format("http://www.1001cocktails.com/recipes/cocktails/rechercheCocktailsNom.php?mot={0}&x=9&y=19", c));
//            }

//            return keywordPages;
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return 5 * matchedRating / 20;
//        }

//        protected override string getImageURL(string matchedURL)
//        {
//            if (matchedURL == "http://www.1001cocktails.com/images/pas_de_photo.jpg")
//                return string.Empty;
//            return matchedURL;
//        }

//        #region OLD
//        /*
//        public static void Crawl(DataFactory df)
//        {
//            string src = "1001cocktails.com";
//            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;

//            List<string> recNames = df.GetDataTable("select RecipeName from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
//                .Select(dr => dr["RecipeName"].ToString()).ToList();

//            WebClient wc = new WebClient();
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
//                string url = string.Format("http://www.1001cocktails.com/recipes/cocktails/rechercheCocktailsNom.php?mot={0}&x=9&y=19", c);
//                string html = wc.DownloadString(url);

//                MatchCollection mc = Regex.Matches(html, "&start=(.*?)>.*?</a>");
//                int temp;
//                List<int> starts = (from m in mc.OfType<Match>()
//                                    where int.TryParse(m.Groups[1].Value, out temp)
//                                    select int.Parse(m.Groups[1].Value)).ToList();
//                starts.Insert(0, 0);
//                foreach (int start in starts)
//                {
//                    html = wc.DownloadString(url + "&start=" + start);
//                    mc = Regex.Matches(html, "<a href=\"(/recipes/.*?)\" title=\".*?\">.*?<b><u>(.*?)</u></b>", RegexOptions.Singleline);
//                    foreach (Match m in mc)
//                    {
//                        string recipeName = CrawlerHelper.ChildSafeName(m.Groups[2].Value);
//                        Console.WriteLine(c.ToString() + " - " + (starts.IndexOf(start) + 1).ToString() + " of " + starts.Count.ToString() + " - " + recipeName);

//                        if (recNames.Contains(recipeName))
//                            continue;

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

//                        Match m2 = Regex.Match(html, "<p>(.*?)<b></b> <br>", RegexOptions.Singleline);
//                        rec.Directions = Common.StripHTML(m2.Groups[1].Value);

//                        m2 = Regex.Match(html, "<br>Rating:&nbsp;<span class=\"important2gras\">(.*?)</span>");
//                        rec.Rating = 5 * Convert.ToSingle(m2.Groups[1].Value) / 20;

//                        MatchCollection mc2 = Regex.Matches(html, "<TD CLASS=\"normal2\">-" +
//                            "(.*?)<a href=\".*?\" alt=\".*?\" title=\".*?\"><u>(.*?)</u></a> </td>", RegexOptions.Singleline);
//                        foreach (Match m3 in mc2)
//                        {
//                            string ingredient = m3.Groups[2].Value;

//                            string[] parts = Common.StripHTML(m3.Groups[1].Value).Replace("100 proof", "").Replace("190 proof", "").Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//                            double tempQty = 0;
//                            int index = parts.Length - 1;
//                            while (index >= 0 && !Common.TryParseFraction(parts[index].Trim(), out tempQty))
//                            {
//                                index--;
//                            }

//                            string qtyString = "";
//                            for (int i = 0; i <= index; i++)
//                            {
//                                qtyString += parts[i].Trim() + " ";
//                            }
//                            qtyString = qtyString.Trim();

//                            string measurementName = "";
//                            bool firstIn = true;
//                            for (int i = index + 1; i < parts.Length; i++)
//                            {
//                                if (firstIn)
//                                    measurementName += parts[i].Trim() + " ";
//                                else
//                                    ingredient = parts[i].Trim() + " " + ingredient;
//                                firstIn = false;
//                            }
//                            measurementName = measurementName.Trim();

//                            if ((measurementName == "fill" || measurementName == "top") && ingredient.StartsWith("with "))
//                            {
//                                qtyString = "1";
//                                measurementName = string.Empty;
//                                ingredient = ingredient.Replace("with ", "");
//                            }

//                            Measurement measurement = null;
//                            if (!string.IsNullOrEmpty(measurementName))
//                            {
//                                MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//                                mf.MeasurementName = measurementName;
//                                measurement = mf.GetDataItems().FirstOrDefault();
//                                if (measurement == null)
//                                {
//                                    //measurement = df.CreateDataItem<Measurement>();
//                                    //measurement.MeasurementName = mf.MeasurementName;
//                                    //measurement.Save();

//                                    ingredient = measurementName + " " + ingredient;
//                                    measurementName = string.Empty;
//                                }
//                            }

//                            float qty = 0;
//                            if (!string.IsNullOrEmpty(qtyString))
//                            {
//                                if (qtyString.Contains("/"))
//                                {
//                                    if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
//                                        throw new NotImplementedException();
//                                }
//                                else if (qtyString.Contains("-") || qtyString.Contains("–"))
//                                {
//                                    parts = qtyString.Split(new string[] { "-", "–" }, StringSplitOptions.None);
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
//                                else
//                                    qty = Convert.ToSingle(qtyString.Replace("juice of ", ""));
//                            }

//                            CrawlerHelper.FillIngredient(df, rec, ingredient, measurement, qty);
//                        }


//                        if (!rec.RecipeRecipeIngredientMeasurements.Any())
//                            continue;

//                        m2 = Regex.Match(html, "<img src=\"(.*?)\" align=\"left\" alt=\"");
//                        if (m2.Groups[1].Value != "http://www.1001cocktails.com/images/pas_de_photo.jpg")
//                        {
//                            RecipeImage img = rec.RecipeRecipeImages.FirstOrDefault() ?? df.CreateDataItem<RecipeImage>();
//                            img.RecipeID = rec.RecipeID;
//                            img.ImageURL = m2.Groups[1].Value;
//                            if (!img.ImageURL.StartsWith("http://www.1001cocktails.com"))
//                                img.ImageURL = "http://www.1001cocktails.com" + img.ImageURL;
//                            img.LocalImagePath = null;
//                            rec.RecipeRecipeImages.Add(img);
//                        }

//                        CrawlerHelper.SaveRecipe(rec);
//                        recNames.Add(rec.RecipeName);
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
//         * */
//        #endregion


//        protected override string servingsRegexPattern
//        {
//            get { return "__NONE__"; }
//        }
//    }
//}
