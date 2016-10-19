//using PaJaMa.Common;
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
//    [RecipeSource("Liquor")]
//    public class LiquorCrawler : CrawlerBase
//    {
//        protected override string baseURL
//        {
//            get { return "http://liquor.com"; }
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
//                    MaxPageRegexPattern = "<a href=\"http://liquor.com/recipes/page/\\d*/\">(\\d*)</a>",
//                    URLFormat = "/recipes/page/{0}/"
//                };
//            }
//        }

//        protected override string recipesRegexPattern
//        {
//            get { return "<h2 class=\"entry-title\"><a href=\"(.*?)\" title=\".*?\" rel=\"bookmark\">(.*?)</a></h2>"; }
//        }

//        protected override string directionsRegexPattern
//        {
//            get { return "<div\nclass=\"preparation\" itemprop=\"recipeInstructions\"><h2>How to make .*?<span\nclass=\"highlight\">.*?</span> Cocktail</h2>(.*?)<div"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { return "<div class=\"user-ratings-star active\" data-rating=\"(.*?)\">"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li\nitemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<noscript><img\nwidth=\".*?\" height=\".*?\" src=\"(.*?)\" class=\"featured-image"; }
//        }

//        #region OLD
//        //		public static void Crawl(DataFactory df)
//        //		{
//        //			WebClient wc = new WebClient();
//        //			string html = wc.DownloadString("http://liquor.com/recipes");
//        //			MatchCollection mc = Regex.Matches(html, "<a href=\"http://liquor.com/recipes/page/\\d*/\">(\\d*)</a>");
//        //			var pageNums = from m2 in mc.OfType<Match>()
//        //						   let pageNum = int.Parse(m2.Groups[1].Value)
//        //						   select pageNum;
//        //			int maxPage = pageNums.Any() ? pageNums.Max() : 200;
//        //			for (int i = 1; i <= maxPage; i++)
//        //			{
//        //				html = wc.DownloadString(string.Format("http://liquor.com/recipes/page/{0}/", i));
//        //				mc = Regex.Matches(html, "<h2 class=\"entry-title\"><a href=\"(.*?)\" title=\".*?\" rel=\"bookmark\">(.*?)</a></h2>");
//        //				foreach (Match m in mc)
//        //				{
//        //					string recipeName = m.Groups[2].Value;
//        //					string src = "liquor.com";
//        //					Recipe existingRec = CrawlerHelper.GetRecipe(df, recipeName, src);
//        //					if (existingRec != null)
//        //					{
//        //						Console.WriteLine(existingRec.RecipeSourceName + " - " + existingRec.RecipeName);
//        //						if (existingRec.RecipeURL == null)
//        //						{
//        //							existingRec.RecipeURL = m.Groups[1].Value;
//        //							existingRec.Save();
//        //						}
//        //						continue;
//        //					}

//        //					html = wc.DownloadString(m.Groups[1].Value).Replace("<a\n", "<a ");
//        //					Match m2 = Regex.Match(html, "<noscript><img\nwidth=\".*?\" height=\".*?\" src=\"(.*?)\" class=\"featured-image", RegexOptions.Singleline);
//        //					if (!m2.Success)
//        //						throw new NotImplementedException(i.ToString());


//        //					Recipe rec = df.CreateDataItem<Recipe>();
//        //					rec.RecipeName = m.Groups[2].Value;
//        //					Console.WriteLine("Page: " + i.ToString() + " Recipe: " + rec.RecipeName);
//        //					CrawlerHelper.AddImage(rec, m2.Groups[1].Value);
//        //					rec.RecipeURL = m.Groups[1].Value;
//        //					rec.RecipeSource = CrawlerHelper.GetRecipeSource(df, src);

//        //					int? rating = null;
//        //					MatchCollection mc2 = Regex.Matches(html, "<div class=\"user-ratings-star active\" data-rating=\"(.*?)\">");
//        //					foreach (Match m3 in mc2)
//        //					{
//        //						int tempInt = -1;
//        //						if (int.TryParse(m3.Groups[1].Value, out tempInt) && tempInt > rating.GetValueOrDefault())
//        //							rating = tempInt;
//        //					}

//        //					rec.Rating = rating;

//        //					List<Tuple<string, string>> ingrs = new List<Tuple<string, string>>();
//        //					mc2 = Regex.Matches(html, "<li\nitemprop=\"ingredients\">(.*?)</li>", RegexOptions.Singleline);
//        //					if (mc2.Count < 1)
//        //					{
//        //						mc2 = Regex.Matches(html, "(<span\nitemprop=\"amount\">(.*?)</span>)?.*?<span\nitemprop=\"name\">(.*?)</span>");
//        //						if (mc2.Count < 1)
//        //							throw new NotImplementedException(i.ToString());

//        //						foreach (Match m3 in mc2)
//        //						{
//        //							ingrs.Add(new Tuple<string, string>(Common.StripHTML(m3.Groups[3].Value),
//        //								string.IsNullOrEmpty(m3.Groups[2].Value) ? "1" : Common.StripHTML(m3.Groups[2].Value)));
//        //						}
//        //					}
//        //					else
//        //					{
//        //						foreach (Match m3 in mc2)
//        //						{
//        //							string amount = "";
//        //							string ingredient = "";
//        //							Match m4 = Regex.Match(m3.Groups[1].Value, "itemprop=\"amount\">(.*?)<");
//        //							if (m4.Success)
//        //								amount = m4.Groups[1].Value;
//        //							else
//        //							{
//        //								m4 = Regex.Match(m3.Groups[1].Value, "<span>(.*?)</span>");
//        //								if (m4.Success)
//        //									amount = m4.Groups[1].Value;
//        //							}

//        //							m4 = Regex.Match(m3.Groups[1].Value, "itemprop=\"name\">(.*?)</span>");
//        //							if (m4.Success)
//        //								ingredient = m4.Groups[1].Value;
//        //							else
//        //								ingredient = m3.Groups[1].Value;

//        //							if (string.IsNullOrEmpty(amount))
//        //							{
//        //								string[] possibles = new string[] { " oz", " cup", " dashes" };
//        //								foreach (var possible in possibles)
//        //								{
//        //									int index = ingredient.ToLower().IndexOf(possible);
//        //									if (index >= 0)
//        //									{
//        //										amount = ingredient.Substring(0, index + possible.Length - 1);
//        //										ingredient = ingredient.Substring(index + possible.Length);
//        //										break;
//        //									}
//        //								}
//        //							}

//        //							ingrs.Add(new Tuple<string, string>(Common.StripHTML(ingredient), string.IsNullOrEmpty(amount) ? "1" : Common.StripHTML(amount)));
//        //						}
//        //					}

//        //					foreach (var ingr in ingrs)
//        //					{
//        //						string[] possibles = new string[] { " oz", " cup", " dashes" };

//        //						string measurementString = string.Empty;
//        //						string amount = ingr.Item2;
//        //						string ingredient = ingr.Item1;

//        //						foreach (string possible in possibles)
//        //						{
//        //							int index = amount.ToLower().IndexOf(possible);
//        //							if (index > 0)
//        //							{
//        //								measurementString = amount.Substring(index + 1).Trim();
//        //								amount = amount.Substring(0, index);
//        //								break;
//        //							}
//        //						}

//        //						double qty = 0;
//        //						if (!PaJaMa.Common.Common.TryParseFraction(amount, out qty))
//        //						{

//        //						}

//        //						Measurement measurement = null;
//        //						if (!string.IsNullOrEmpty(measurementString))
//        //						{
//        //							MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//        //							mf.MeasurementName = measurementString;
//        //							measurement = mf.GetDataItems().FirstOrDefault();
//        //							if (measurement == null)
//        //							{

//        //							}
//        //						}

//        //						CrawlerHelper.FillIngredient(df, rec, ingredient, measurement, qty);
//        //					}


//        //					m2 = Regex.Match(html, "<div\nclass=\"preparation\" itemprop=\"recipeInstructions\"><h2>How to make .*?<span\n" +
//        //"class=\"highlight\">.*?</span> Cocktail</h2>(.*?)<div", RegexOptions.Singleline);
//        //					if (m2.Success)
//        //						rec.Directions = Common.StripHTML(m2.Groups[1].Value);


//        //					CrawlerHelper.SaveRecipe(rec);

//        //					System.Threading.Thread.Sleep(100);
//        //				}
//        //			}
//        //		}
//        #endregion

//        protected override string servingsRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }
//    }
//}
