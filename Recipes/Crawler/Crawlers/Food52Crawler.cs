
using HtmlAgilityPack;
using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Crawlers
{
	[RecipeSource("Food52")] //, IncludeNoRating = true)]
	public class Food52Crawler : CrawlerBase
	{
		protected override string baseURL
		{
			get { return "http://food52.com"; }
		}

		protected override string allURL
		{
			get { return "/recipes"; }
		}

		protected override PageNumbers pageNumberURLRegex
		{
			get
			{
				return new PageNumbers()
				{
					MaxPageRegexPattern = "\\?page=(.*?)\">",
					URLFormat = "?page={0}"
				};
			}
		}

		protected override string recipesXPath
		{
			get { return "//a[contains(@href, '/recipes') and @class='photo']"; }
		}

		protected override List<string> getKeywordPages(HtmlDocument document)
		{
			return document.DocumentNode.SelectNodes("//ul[contains(@class, 'filters')]//li//a")
				.Select(n => baseURL + n.Attributes["href"].Value).ToList();
		}

		//protected override string recipesRegexPattern
		//{
		//	get { return "<div class=\"photo.*?>.*?<a href=\"(/recipes/.*?)\" class=\"photo\" title=\"(.*?)\">(.*?)</a>"; }
		//}

		//protected override string directionsRegexPattern
		//{
		//	get { return "<li itemprop=\"recipeInstructions\">(.*?)</li>"; }
		//}

		//protected override string ratingRegexPattern
		//{
		//	get { return "saveCount\">(.*?)</span>"; }
		//}

		//protected override string servingsRegexPattern
		//{
		//	get { return "<p itemprop=\"recipeYield\"><strong><em>(.*?)</em></strong></p>"; }
		//}

		//protected override string ingredientsRegexPattern
		//{
		//	get { return "<li class=\".*?\" itemprop=\"ingredients\">(.*?)</li>"; }
		//}

		//protected override string imageRegexPattern
		//{
		//	get { return "<img alt=\".*?\" itemprop=\"image\" src=\"(.*?)\" />"; }
		//}

		//protected override float getRating(float matchedRating)
		//{
		//	if (matchedRating < 100)
		//		return 3;
		//	else if (matchedRating < 500)
		//		return 3.5F;
		//	else if (matchedRating < 1000)
		//		return 4;
		//	else if (matchedRating < 5000)
		//		return 4.5F;
		//	return 5;
		//}

		/*
        public static void Crawl(DataFactory df)
        {
            WebClient wc = new WebClient();
            string html = wc.DownloadString("http://food52.com/recipes");

            string src = "food52.com";
            int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;
            List<string> recURLs = df.GetDataTable("select RecipeURL from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
                .Select(r => r["RecipeURL"].ToString()).ToList();

            MatchCollection mc = Regex.Matches(html, "<a href=\"(/recipes/.*?)\" title=\"Add filter\">");
            foreach (Match m in mc)
            {
                string baseURL = "http://food52.com" + m.Groups[1].Value;
                html = wc.DownloadString(baseURL);

                MatchCollection mc2 = Regex.Matches(html, "\\?page=(.*?)\">");
                int maxPage = (from m2 in mc2.OfType<Match>()
                               select int.Parse(m2.Groups[1].Value)).Max();
                crawlPage(df, html, wc, recURLs, srcID, 1, maxPage);

                for (int i = 2; i <= maxPage; i++)
                {
                    html = wc.DownloadString(baseURL + "?page=" + i.ToString());
                    crawlPage(df, html, wc, recURLs, srcID, i, maxPage);
                }
            }
        }

        private static void crawlPage(DataFactory df, string html, WebClient wc, List<string> recURLs, int srcID, int pageNum, int maxPages)
        {
            string pattern = "<a href=\"(/recipes/.*?)\" title=\"(.*?)\">(.*?)</a>";

            MatchCollection mc = Regex.Matches(html, pattern);

            foreach (Match m in mc)
            {
                if (m.Groups[2].Value != m.Groups[3].Value) continue;
                string url = "http://food52.com" + m.Groups[1].Value;
                string display = "Page " + pageNum + " of " + maxPages + " - " + m.Groups[2].Value;
                if (recURLs.Contains(url))
                {
                    Console.WriteLine(display);
                    continue;
                }

                Console.WriteLine("* " + display);

                html = wc.DownloadString(url);

                Match m2 = Regex.Match(html, "<meta name=\"twitter:title\" content=\"(.*?)\">");

                Recipe rec = df.CreateDataItem<Recipe>();
                rec.RecipeSourceID = srcID;
                rec.RecipeName = m2.Groups[1].Value;
                rec.RecipeURL = url;

                MatchCollection mc2 = Regex.Matches(html, "<li itemprop=\"recipeInstructions\">(.*?)</li>", RegexOptions.Singleline);
                if (mc2.Count > 0)
                {
                    rec.Directions = "";
                    foreach (Match m3 in mc2)
                    {
                        rec.Directions += m3.Groups[1].Value.Trim() + "\r\n";
                    }
                    rec.Directions = rec.Directions.Trim();
                }

                m2 = Regex.Match(html, "saveCount\">(.*?)</span>");
                if (!m2.Success)
                    throw new NotImplementedException();

                int faves = int.Parse(m2.Groups[1].Value.Replace(",", ""));

                if (faves < 100)
                    rec.Rating = 3;
                else if (faves < 500)
                    rec.Rating = 3.5F;
                else if (faves < 1000)
                    rec.Rating = 4;
                else if (faves < 5000)
                    rec.Rating = 4.5F;
                else
                    rec.Rating = 5;


                m2 = Regex.Match(html, "<p itemprop=\"recipeYield\"><strong><em>Serves&nbsp;(.*?)</em></strong></p>", RegexOptions.Singleline);
                if (m2.Success)
                {
                    Match m3 = Regex.Match(m2.Groups[1].Value, "(\\d+)");
                    int tempInt = -1;
                    if (int.TryParse(m3.Groups[1].Value, out tempInt))
                        rec.NumberOfServings = tempInt;
                    else
                    {

                    }
                }
                //else
                //{
                //	throw new NotImplementedException();
                //}


                mc2 = Regex.Matches(html, "<li class=\".*?\" itemprop=\"ingredients\">(.*?)</li>", RegexOptions.Singleline);
                if (mc2.Count < 1)
                    continue;

                foreach (Match m3 in mc2)
                {
                    string ingredient = string.Empty;

                    float qty = 0;
                    Match m4 = Regex.Match(m3.Groups[1].Value, "<span class=\"quantity\">(.*?)</span>");
                    if (m4.Success)
                    {
                        string qtyString = m4.Groups[1].Value.Trim().ToLower()
                            .Replace("one", "1")
                            .Replace("two", "2")
                            .Replace("three", "3")
                            .Replace("four", "4")
                            .Replace("five", "5")
                            .Replace("six", "6")
                            .Replace("seven", "7")
                            .Replace("eight", "8")
                            .Replace("nine", "9")
                            .Replace("ten", "10")
                            .Replace("â½", "1/2")
                            .Replace("â¼", "1/4")
                            .Replace("~", "");
                        if (qtyString.Contains("to") || qtyString.Contains("-"))
                        {
                            string[] parts2 = qtyString.Split(new string[] { "to", "-" }, StringSplitOptions.RemoveEmptyEntries);
                            if (parts2.Length < 2)
                                ingredient = qtyString;
                            else
                            {
                                float first = 0;
                                float second = 0;

                                if (!PaJaMa.Common.Common.TryParseFraction(parts2[0].Trim(), out first))
                                {
                                    ingredient = parts2[0];
                                    qty = 0;
                                }

                                if (!PaJaMa.Common.Common.TryParseFraction(parts2[1].Trim(), out second))
                                {
                                    ingredient += " " + parts2[1];
                                    qty = first;
                                }
                                else
                                    qty = (first + second) / 2;
                            }
                        }
                        else if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
                            ingredient = qtyString;
                    }

                    m4 = Regex.Match(m3.Groups[1].Value, "<span class=\"item-name\">(.*?)</span>");
                    if (!m4.Success)
                        throw new NotImplementedException();

                    ingredient += " " + m4.Groups[1].Value;
                    ingredient = ingredient.Trim();

                    if (string.IsNullOrEmpty(ingredient))
                        continue;

                    Tuple<string, Measurement, float> ingr = CrawlerHelper.GetIngredientQuantity(df, ingredient, false);
                    CrawlerHelper.FillIngredient(rec, ingr.Item1, ingr.Item2, qty);
                }

                m2 = Regex.Match(html, "<div id=\"recipe-gallery-frame\">(.*?)</div>", RegexOptions.Singleline);
                if (m2.Success)
                {
                    mc2 = Regex.Matches(m2.Groups[1].Value, "<img .*?src=\"(.*?)\"");
                    foreach (Match m3 in mc2)
                    {
                        RecipeImage img = df.CreateDataItem<RecipeImage>();
                        img.ImageURL = m3.Groups[1].Value;
                        img.LocalImagePath = null;
                        rec.RecipeRecipeImages.Add(img);
                    }
                }
                else
                {

                }

                CrawlerHelper.SaveRecipe(rec);

                recURLs.Add(rec.RecipeURL);
            }
        }
        */

	}
}
