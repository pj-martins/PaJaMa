﻿//using PaJaMa.Common;

//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Epicurious")]
//    public class EpicuriousCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.epicurious.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/recipesmenus/browse"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = " (\\d*) recipes found for"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a href=\"(/recipes/.*?)\" onclick=\"setBackToSearch.*?\" class=\"hed\">(.*?)</a>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<h2>Preparation</h2>(.*?)</div>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<img class=\"forks_img\" src=\"/images/recipes/recipe_detail/(.*?)_forks.gif"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li class=\"ingredient\"><span itemprop=\"ingredients\">(.*?)</span></li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img itemprop=\"image\" src=\"(.*?)\" class=\"photo\" alt=.*? />"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "itemprop=\"recipeYield\">(.*?)</span>"; }
//        }

//        protected override List<string> getKeywordPages(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "<div class=\"bItem\"><a href=\"(/tools/browseresults\\?type=browse&amp;att=.*?)\">.*?</a></div>");
//            return (from m in mc.OfType<Match>()
//                    select baseURL + m.Groups[1].Value.Replace("&amp;", "&")).ToList();
//        }

//        protected override string getPageURL(string keywordPage, int pageNum)
//        {
//            return keywordPage + "&pageNumber=" + pageNum.ToString() + "&pageSize=10&resultOffset=" + (pageNum == 1 ? "0" : (((pageNum - 1) * 10) + 1).ToString());
//        }

//        protected override float getRating(float matchedRating)
//        {
//            return (5 * matchedRating) / 4;
//        }

//        /*
//                public static void Crawl(DataFactory df)
//                {
//                    WebClient wc = new WebClient();
//                    string html = wc.DownloadString("http://www.epicurious.com/recipesmenus/browse");
//                    MatchCollection mc = Regex.Matches(html, "<div class=\"bItem\"><a href=\"(/tools/browseresults\\?type=browse&amp;att=.*?)\">.*?</a></div>");
//                    foreach (Match m in mc)
//                    {
//                        html = wc.DownloadString("http://www.epicurious.com/" + m.Groups[1].Value.Replace("&amp;", "&"));
//                        Match mmax = Regex.Match(html, " (\\d*) recipes found for");
//                        double maxPage = double.Parse(mmax.Groups[1].Value) / 10;
//                        for (int i = 1; i <= Math.Ceiling(maxPage); i++)
//                        {
//                            Console.WriteLine("Page: " + i.ToString() + " of " + Math.Ceiling(maxPage).ToString());
//                            html = wc.DownloadString("http://www.epicurious.com" + m.Groups[1].Value + "&pageNumber=" + i.ToString() +
//                                "&pageSize=10&resultOffset=" + (i == 1 ? "0" : (((i - 1) * 10) + 1).ToString()));

//                            MatchCollection mc2 = Regex.Matches(html, "<a href=\"(/recipes/.*?)\" onclick=\"setBackToSearch.*?\" class=\"hed\">(.*?)</a>");
//                            foreach (Match m2 in mc2)
//                            {
//                                string recipeName = m2.Groups[2].Value;
//                                string src = "epicurious.com";
//                                string recipeURL = "http://www.epicurious.com" + m2.Groups[1].Value;

//                                Recipe existingRec = CrawlerHelper.GetRecipe(df, recipeName, src);
//                                if (existingRec != null)
//                                {
//                                    Console.WriteLine(existingRec.RecipeSourceName + " - " + existingRec.RecipeName);
//                                    if (existingRec.RecipeURL == null)
//                                    {
//                                        existingRec.RecipeURL = recipeURL;
//                                        existingRec.Save();
//                                    }
//                                    continue;
//                                }

//                                html = wc.DownloadString("http://www.epicurious.com" + m2.Groups[1].Value);

//                                Match m3 = Regex.Match(html, "<img itemprop=\"image\" src=\"(.*?)\" class=\"photo\" alt=.*? />");
//                                // if (!m3.Success) throw new NotImplementedException(m.Groups[1].Value + " " + i.ToString());

//                                Recipe rec = df.CreateDataItem<Recipe>();
//                                rec.RecipeName = recipeName;
//                                rec.RecipeSource = CrawlerHelper.GetRecipeSource(df, src);
//                                rec.RecipeURL = recipeURL;
//                                if (!m3.Success)
//                                {

//                                }
//                                else
//                                    CrawlerHelper.AddImage(rec, "http://www.epicurious.com" + m3.Groups[1].Value);

//                                SetRating(rec, html);

//                                m3 = Regex.Match(html, "<h2>Preparation</h2>(.*?)</div>", RegexOptions.Singleline);
//                                rec.Directions = PaJaMa.Common.Common.StripHTML(m3.Groups[1].Value.Trim()).Trim();

//                                MatchCollection mc3 = Regex.Matches(html, "<li class=\"ingredient\"><span itemprop=\"ingredients\">(.*?)</span></li>");

//                                if (mc3.Count < 1)
//                                    continue;

//                                Console.WriteLine(rec.RecipeName);
						
//                                List<RecipeIngredientMeasurement> recIngs = new List<RecipeIngredientMeasurement>();
//                                foreach (Match m4 in mc3)
//                                {
//                                    string[] parts = Common.StripHTML(m4.Groups[1].Value).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//                                    double qty = 0;
//                                    double tempQty = 0;
//                                    int index = 0;
//                                    while (Common.TryParseFraction(parts[index], out tempQty))
//                                    {
//                                        qty += tempQty;
//                                        index++;
//                                    }

//                                    MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//                                    mf.MeasurementName = parts[index];
//                                    Measurement measurement = mf.GetDataItems().FirstOrDefault();
//                                    if (measurement == null)
//                                    {

//                                    }
//                                    else
//                                        index++;

//                                    string ingredient = string.Empty;

//                                    for (int i2 = index; i2 < parts.Length; i2++)
//                                    {
//                                        ingredient += parts[i2] + " ";
//                                    }

//                                    ingredient = ingredient.Trim();

//                                    if (ingredient.Length > 255)
//                                        continue;

//                                    CrawlerHelper.FillIngredient(df, rec, ingredient, measurement, qty);
//                                }

//                                CrawlerHelper.SaveRecipe(rec);

//                                System.Threading.Thread.Sleep(100);

//                            }
//                        }
//                    }
//                }

//                public static void SetRating(Recipe rec, string html)
//                {
//                    Match m3 = Regex.Match(html, "<img class=\"forks_img\" src=\"/images/recipes/recipe_detail/(.*?)_forks.gif");
//                    if (!m3.Success)
//                    {

//                    }
//                    else
//                    {
//                        string rating = m3.Groups[1].Value.Replace("_", ".");
//                        rec.Rating = (float?)(5 * double.Parse(rating) / 4);
//                    }
//                }
//         */
//    }
//}
