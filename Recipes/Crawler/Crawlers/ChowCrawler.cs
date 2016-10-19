
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
//    [RecipeSource("Chow")]
//    public class ChowCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://www.chow.com"; }
//        }

//        protected override string allURL
//        {
//            get { return "/recipes"; }
//        }

//        protected override PageNumbers pageNumberURLRegex
//        {
//            get
//            {
//                return new PageNumbers()
//                {
//                    MaxPageRegexPattern = "/recipes\\?page=(\\d*)",
//                    URLFormat = "?page={0}"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<a href=\"(http://www.chow.com/recipes/.*?)\" .*?<img alt=\"(.*?)\" class=\"image_link_image\""; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<li class=\"d.*?\">(.*?)</li>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return " id=\"average_rating\" style=\".*?\">(.*?)</span>"; }
//        }

//        protected override string ingredientSectionRegexPattern
//        {
//            get { return "<div id=\"ingredients\".*?>(.*?)</div>"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li>(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<meta property=\"og:image\" content=\"(.*?)\">"; }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "<span itemprop=\"yield\">(.*?)</span>"; }
//        }

//        //		public static void Crawl(DataFactory df, int startPage = 1)
//        //		{
//        //			WebClient wc = new WebClient();
//        //			string html = wc.DownloadString("http://www.chow.com/recipes");

//        //			MatchCollection mc = Regex.Matches(html, "/recipes\\?page=(\\d*)");
//        //			int maxPage = (from m in mc.OfType<Match>()
//        //							  int double.Parse(m.Groups[1].Value)).Max();

//        //			string src = "chow.com";
//        //			int srcID = CrawlerHelper.GetRecipeSource(df, src).RecipeSourceID;

//        //			List<string> recURLs = df.GetDataTable("select RecipeURL from Recipe where RecipeSourceID = " + srcID.ToString()).Rows.OfType<DataRow>()
//        //				.Select(r => r["RecipeURL"].ToString()).ToList();

//        //			for (int i = startPage; i <= maxPage; i++)
//        //			{
//        //				html = wc.DownloadString("http://www.chow.com/recipes?page=" + i.ToString());
//        //				crawlPage(df, html, wc, recURLs, srcID, i, maxPage);
//        //			}
//        //		}

//        //		private static void crawlPage(DataFactory df, string html, WebClient wc, List<string> recURLs, int srcID, int pageNum, int maxPage)
//        //		{
//        //			MatchCollection mc = Regex.Matches(html, "<a href=\"(/recipes/.*)\" class=\"recipe_item");
//        //			foreach (Match m in mc.OfType<Match>())
//        //			{
//        //				string recipeURL = "http://www.chow.com" + m.Groups[1].Value;
//        //				if (recURLs.Contains(m.Groups[1].Value))
//        //				{
//        //					Console.WriteLine("Page: " + pageNum.ToString() + " of " + maxPage.ToString() + " Recipe: " + m.Groups[1].Value);
//        //					continue;
//        //				}

//        //				Console.WriteLine("* Page: " + pageNum.ToString() + " of " + maxPage.ToString() + " Recipe: " + m.Groups[1].Value);
//        //				int tries = 3;
//        //				while (tries > 0)
//        //				{
//        //					try
//        //					{
//        //						html = wc.DownloadString(recipeURL);
//        //						break;
//        //					}
//        //					catch
//        //					{
//        //						System.Threading.Thread.Sleep(1000);
//        //						tries--;
//        //					}
//        //				}

//        //				if (tries == 0)
//        //					continue;

//        //				html = wc.DownloadString(recipeURL);

//        //				Match m2 = Regex.Match(html, "<h1 .*?>(.*?)</h1>");

//        //				Recipe rec = df.CreateDataItem<Recipe>();
//        //				rec.RecipeName = m2.Groups[1].Value;
//        //				rec.RecipeSourceID = srcID;
//        //				rec.RecipeURL = recipeURL;

//        //				MatchCollection mc2 = Regex.Matches(html, "<li itemprop=\"recipeInstructions\">(.*?)</li>", RegexOptions.Singleline);
//        //				if (mc2.Count > 0)
//        //				{
//        //					rec.Directions = "";
//        //					foreach (Match m3 in mc2)
//        //					{
//        //						rec.Directions += m3.Groups[1].Value.Trim() + "\r\n";
//        //					}
//        //					rec.Directions = rec.Directions.Trim();
//        //				}

//        //				m2 = Regex.Match(html, "saveCount\">(.*?)</span>");
//        //				if (!m2.Success)
//        //					throw new NotImplementedException();

//        //				int faves = int.Parse(m2.Groups[1].Value.Replace(",", ""));

//        //				if (faves < 100)
//        //					rec.Rating = 3;
//        //				else if (faves < 500)
//        //					rec.Rating = 3.5F;
//        //				else if (faves < 1000)
//        //					rec.Rating = 4;
//        //				else if (faves < 5000)
//        //					rec.Rating = 4.5F;
//        //				else
//        //					rec.Rating = 5;


//        //				m2 = Regex.Match(html, "<p itemprop=\"recipeYield\"><strong><em>Serves&nbsp;(.*?)</em></strong></p>", RegexOptions.Singleline);
//        //				if (m2.Success)
//        //				{
//        //					Match m3 = Regex.Match(m2.Groups[1].Value, "(\\d+)");
//        //					int tempInt = -1;
//        //					if (int.TryParse(m3.Groups[1].Value, out tempInt))
//        //						rec.NumberOfServings = tempInt;
//        //					else
//        //					{

//        //					}
//        //				}
//        //				//else
//        //				//{
//        //				//	throw new NotImplementedException();
//        //				//}


//        //				mc2 = Regex.Matches(html, "<li class=\".*?\" itemprop=\"ingredients\">(.*?)</li>", RegexOptions.Singleline);
//        //				if (mc2.Count < 1)
//        //					continue;

//        //				foreach (Match m3 in mc2)
//        //				{
//        //					string ingredient = string.Empty;

//        //					float qty = 0;
//        //					Match m4 = Regex.Match(m3.Groups[1].Value, "<span class=\"quantity\">(.*?)</span>");
//        //					if (m4.Success)
//        //					{
//        //						string qtyString = m4.Groups[1].Value.Trim().ToLower()
//        //							.Replace("one", "1")
//        //							.Replace("two", "2")
//        //							.Replace("three", "3")
//        //							.Replace("four", "4")
//        //							.Replace("five", "5")
//        //							.Replace("six", "6")
//        //							.Replace("seven", "7")
//        //							.Replace("eight", "8")
//        //							.Replace("nine", "9")
//        //							.Replace("ten", "10")
//        //							.Replace("â½", "1/2")
//        //							.Replace("â¼", "1/4")
//        //							.Replace("~", "");
//        //						if (qtyString.Contains("to") || qtyString.Contains("-"))
//        //						{
//        //							string[] parts2 = qtyString.Split(new string[] { "to", "-" }, StringSplitOptions.RemoveEmptyEntries);
//        //							if (parts2.Length < 2)
//        //								ingredient = qtyString;
//        //							else
//        //							{
//        //								float first = 0;
//        //								float second = 0;

//        //								if (!PaJaMa.Common.Common.TryParseFraction(parts2[0].Trim(), out first))
//        //								{
//        //									ingredient = parts2[0];
//        //									qty = 0;
//        //								}

//        //								if (!PaJaMa.Common.Common.TryParseFraction(parts2[1].Trim(), out second))
//        //								{
//        //									ingredient += " " + parts2[1];
//        //									qty = first;
//        //								}
//        //								else
//        //									qty = (first + second) / 2;
//        //							}
//        //						}
//        //						else if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
//        //							ingredient = qtyString;
//        //					}

//        //					m4 = Regex.Match(m3.Groups[1].Value, "<span class=\"item-name\">(.*?)</span>");
//        //					if (!m4.Success)
//        //						throw new NotImplementedException();

//        //					ingredient += " " + m4.Groups[1].Value;
//        //					ingredient = ingredient.Trim();

//        //					if (string.IsNullOrEmpty(ingredient))
//        //						continue;

//        //					Tuple<string, Measurement, float> ingr = CrawlerHelper.GetIngredientQuantity(df, ingredient);
//        //					CrawlerHelper.FillIngredient(df, rec, ingr.Item1, ingr.Item2, qty);
//        //				}

//        //				m2 = Regex.Match(html, "<div id=\"recipe-gallery-frame\">(.*?)</div>", RegexOptions.Singleline);
//        //				if (m2.Success)
//        //				{
//        //					mc2 = Regex.Matches(m2.Groups[1].Value, "<img .*?src=\"(.*?)\"");
//        //					foreach (Match m3 in mc2)
//        //					{
//        //						RecipeImage img = df.CreateDataItem<RecipeImage>();
//        //						img.ImageURL = m3.Groups[1].Value;
//        //						img.LocalImagePath = null;
//        //						rec.RecipeRecipeImages.Add(img);
//        //					}
//        //				}
//        //				else
//        //				{

//        //				}

//        //				CrawlerHelper.SaveRecipe(rec);

//        //				recURLs.Add(rec.RecipeURL);
//        //			}
//        //		}
//    }
//}
