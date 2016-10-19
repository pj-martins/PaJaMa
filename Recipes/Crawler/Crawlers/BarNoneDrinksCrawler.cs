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
//    [RecipeSource("Bar None Drinks", IgnoreAuto = true)]
//    public class BarNoneDrinksCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.barnonedrinks.com"; }
//        }

//        protected override string allURL
//        {
//            get { return string.Empty; }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a href=\"(/drinks/([A-Za-z]|\\-)/.*?)\" title=\".*?\">(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "property=\"v:instructions\">(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "class=\"important2gras\">(.*?)</span>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "typeof=\"v:Ingredient\">(<span property=\"v:amount\">.*?</span><span property=\"v:name\">.*?)</span>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "_NONE_"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "__NONE__"; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            List<string> keywords = new List<string>();
//            List<char> chars = new List<char>();
//            chars.Add('-');

//            for (int i = 97; i <= 122; i++)
//            {
//                chars.Add((char)i);
//            }

//            string html2 = getHTML(baseURL + "/drinks/by_category/");
//            MatchCollection mc = Regex.Matches(html2, "<a href=\"(/drinks/by_category/.*?/)\" title=\".*?\">.*?</a>");

//            foreach (Match m in mc)
//            {
//                foreach (char c in chars)
//                {
//                    keywords.Add(baseURL + string.Format(m.Groups[1].Value + "{0}.html", c));
//                }
//            }

//            foreach (char c in chars)
//            {
//                html2 = getHTML(baseURL + string.Format("/drinks/by_ingredient/{0}/", c));
//                mc = Regex.Matches(html2, "<a href=\"(/drinks/by_ingredient/.*?/.*?.html)\" title=\".*?\">.*?</a>");
//                foreach (Match m in mc)
//                {
//                    keywords.Add(baseURL + m.Groups[1].Value);
//                }
//            }

//            return keywords;
//        }

//        /*
//        public static void Crawl(DataFactory df)
//        {
//            string src = "barnonedrinks.com";
//            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;

//            List<string> recNames = df.GetDataTable("select RecipeName from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
//                .Select(dr => dr["RecipeName"].ToString()).ToList();

//            WebClient wc = new WebClient();
//            string html = wc.DownloadString("http://www.barnonedrinks.com/drinks/by_category/");

//            MatchCollection mc = Regex.Matches(html, "<a href=\"(/drinks/by_category/.*?/)\" title=\".*?\">.*?</a>");
//            foreach (Match m in mc)
//            {
//                crawlPage(df, srcID, "http://www.barnonedrinks.com" + m.Groups[1].Value, wc, recNames);
//            }
//        }

//        private static void crawlPage(DataFactory df, int srcID, string baseURL, WebClient wc, List<string> recNames)
//        {
//            List<char> chars = new List<char>();

//            chars.Add('-');

//            for (int i = 97; i <= 122; i++)
//            {
//                chars.Add((char)i);
//            }

//            foreach (char c in chars)
//            {
//                string url = baseURL + string.Format("{0}.html", c);
//                string html = string.Empty;
//                int tries = 3;
//                while (tries > 0)
//                {
//                    try
//                    {
//                        html = wc.DownloadString(url);
//                        break;
//                    }
//                    catch
//                    {
//                        System.Threading.Thread.Sleep(1000);
//                        tries--;
//                    }
//                }

//                if (tries == 0) continue;

//                MatchCollection mc = Regex.Matches(html, "<a href=\"(/drinks/" + c + "/.*?)\" title=\".*?\">(.*?)</a>", RegexOptions.Singleline);
//                for (int j = 0; j < mc.Count; j++)
//                {
//                    Match m = mc[j];
//                    string recipeName = CrawlerHelper.ChildSafeName(m.Groups[2].Value);
//                    Console.WriteLine(c.ToString() + " - " + (j + 1).ToString() + " of " + mc.Count.ToString() + " - " + recipeName);

//                    if (recNames.Contains(recipeName))
//                        continue;

//                    Recipe rec = df.CreateDataItem<Recipe>();
//                    rec.RecipeSourceID = srcID;
//                    rec.RecipeName = recipeName;
//                    rec.RecipeURL = "http://www.barnonedrinks.com" + m.Groups[1].Value;

//                    tries = 3;
//                    while (tries > 0)
//                    {
//                        try
//                        {
//                            html = wc.DownloadString(rec.RecipeURL);
//                            break;
//                        }
//                        catch
//                        {
//                            System.Threading.Thread.Sleep(1000);
//                            tries--;
//                        }
//                    }

//                    if (tries == 0)
//                        throw new Exception("ERROR");

//                    Match m2 = Regex.Match(html, "<div class=\"bnd-c-text-sect\" property=\"v:instructions\">(.*?)</div>", RegexOptions.Singleline);
//                    if (!m2.Success)
//                        throw new NotImplementedException();

//                    rec.Directions = Common.StripHTML(m2.Groups[1].Value).Trim();

//                    //m2 = Regex.Match(html, "<br>Rating:&nbsp;<span class=\"important2gras\">(.*?)</span>");
//                    //rec.Rating = 5 * Convert.ToSingle(m2.Groups[1].Value) / 20;

//                    MatchCollection mc2 = Regex.Matches(html, "<span typeof=\"v:Ingredient\"><span property=\"v:amount\">(.*?)</span><span property=\"v:name\">(.*?)</span>", RegexOptions.Singleline);
//                    foreach (Match m3 in mc2)
//                    {
//                        string ingredient = Common.StripHTML(m3.Groups[2].Value).Trim();

//                        string[] parts = Common.StripHTML(m3.Groups[1].Value.Trim()).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//                        double tempQty = 0;
//                        int index = parts.Length - 1;
//                        while (index >= 0 && !Common.TryParseFraction(parts[index].Trim(), out tempQty))
//                        {
//                            index--;
//                        }

//                        string qtyString = "";
//                        for (int i = 0; i <= index; i++)
//                        {
//                            qtyString += parts[i].Trim() + " ";
//                        }
//                        qtyString = qtyString.Trim();

//                        string measurementName = "";
//                        for (int i = index + 1; i < parts.Length; i++)
//                        {
//                            measurementName += parts[i].Trim() + " ";
//                        }
//                        measurementName = measurementName.Trim();

//                        Measurement measurement = null;
//                        if (!string.IsNullOrEmpty(measurementName))
//                        {
//                            MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//                            mf.MeasurementName = measurementName;
//                            measurement = mf.GetDataItems().FirstOrDefault();
//                            if (measurement == null)
//                            {
//                                measurement = df.CreateDataItem<Measurement>();
//                                measurement.MeasurementName = mf.MeasurementName;
//                                measurement.Save();
//                            }
//                        }

//                        float qty = 0;
//                        if (!string.IsNullOrEmpty(qtyString))
//                        {
//                            if (qtyString.Contains("/"))
//                            {
//                                if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
//                                    throw new NotImplementedException();
//                            }
//                            else if (qtyString.Contains("-") || qtyString.Contains("–"))
//                            {
//                                parts = qtyString.Split(new string[] { "-", "–" }, StringSplitOptions.None);
//                                if (parts.Length != 2)
//                                    throw new NotImplementedException();
//                                float first = 0;
//                                float second = 0;

//                                if (!PaJaMa.Common.Common.TryParseFraction(parts[0], out first))
//                                    throw new NotImplementedException();

//                                if (!PaJaMa.Common.Common.TryParseFraction(parts[1], out second))
//                                    throw new NotImplementedException();

//                                qty = (first + second) / 2;
//                            }
//                            else
//                                qty = Convert.ToSingle(qtyString);
//                        }

//                        CrawlerHelper.FillIngredient(df, rec, ingredient, measurement, qty);
//                    }


//                    if (!rec.RecipeRecipeIngredientMeasurements.Any())
//                        continue;

//                    m2 = Regex.Match(html, "<img src=\"(.*?)\" .*? rel=\"v:photo\"");
//                    if (m2.Success)
//                    {
//                        RecipeImage img = rec.RecipeRecipeImages.FirstOrDefault() ?? df.CreateDataItem<RecipeImage>();
//                        img.RecipeID = rec.RecipeID;
//                        img.ImageURL = m2.Groups[1].Value;
//                        if (!img.ImageURL.StartsWith("http://www.barnonedrinks.com"))
//                            img.ImageURL = "http://www.barnonedrinks.com" + img.ImageURL;
//                        img.LocalImagePath = null;
//                        rec.RecipeRecipeImages.Add(img);
//                    }

//                    CrawlerHelper.SaveRecipe(rec);
//                    recNames.Add(rec.RecipeName);
//                }
//            }
//        }
//        */
//    }
//}
