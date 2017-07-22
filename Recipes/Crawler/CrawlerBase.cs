using HtmlAgilityPack;
using PaJaMa.Common;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Crawler
{
	public abstract class CrawlerBase
	{
		private RecipeSourceAttribute _myAttribute;
		protected RecipeSourceAttribute myAttribute
		{
			get
			{
				if (_myAttribute == null)
				{
					_myAttribute = this.GetType().GetCustomAttributes(typeof(RecipeSourceAttribute), true).FirstOrDefault() as RecipeSourceAttribute;
					if (_myAttribute == null) throw new NotImplementedException();
				}
				return _myAttribute;
			}
		}

		protected RecipesContext dbContext { get; private set; }
		protected WebClient webClient { get; private set; }
		protected RecipeSource recipeSource { get; private set; }
		protected List<string> existingRecipes { get; private set; }

		protected abstract string baseURL { get; }
		protected virtual string allURL { get { return string.Empty; } }

		protected abstract string recipesXPath { get; }
		protected virtual Tuple<string, string> getRecipeURL(HtmlNode node)
		{
			return new Tuple<string, string>(node.Attributes["href"].Value, node.InnerText.Trim());
		}

		protected virtual string servingsXPath
		{
			get { return "//*[@itemprop='yield' or @itemprop='recipeYield' or @itemprop='recipeyield']"; }
		}

		protected virtual string recipeNameXPath
		{
			get { return "//*[@itemprop='name']"; }
		}

		protected virtual string directionsXPath
		{
			get { return "//*[@itemprop='instructions' or @itemprop='recipeInstructions']"; }
		}

		protected virtual string ingredientsXPath
		{
			get { return "//*[@itemprop='ingredients' or @itemprop='recipeIngredient']"; }
		}

		protected virtual string ingredientAmountXPath
		{
			get { return "//*[@itemprop='amount']"; }
		}

		protected virtual string ingredientNameXPath
		{
			get { return "//*[@itemprop='name']"; }
		}

		protected virtual string imagesXPath
		{
			get { return "//*[@itemprop='photo' or @itemprop='image']"; }
		}

		protected virtual string ratingXPath
		{
			get { return "//*[@itemprop='rating' or @itemprop='ratingValue']"; }
		}

		protected virtual PageNumbers pageNumberURLRegex { get { return null; } }

		protected virtual Dictionary<string, string> getRecipeURLs(HtmlDocument doc)
		{
			var dict = new Dictionary<string, string>();

			var children = doc.DocumentNode.SelectNodes(recipesXPath);
			if (children == null)
				return new Dictionary<string, string>();
			foreach (HtmlNode child in children)
			{
				var rec = getRecipeURL(child);
				if (rec != null && !dict.ContainsKey(rec.Item1))
					dict.Add(rec.Item1, rec.Item2);
			}

			return dict;
		}

		protected Tuple<string, Measurement, float> getIngredient(HtmlNode node)
		{
			var innerHtml = node.InnerHtml.Replace("&frasl;", "/");
			var doc = new HtmlDocument();
			doc.LoadHtml(innerHtml);
			var qty = doc.DocumentNode.SelectSingleNode(ingredientAmountXPath);
			if (qty != null)
			{
				var ingr = doc.DocumentNode.SelectSingleNode(ingredientNameXPath);
				var meas = CrawlerHelper.GetIngredientQuantity(dbContext, qty.InnerText, true, false);
				var ingrText = PaJaMa.Common.Common.StripHTML(ingr.InnerText).Trim();
				if (meas.Item2 == null && string.IsNullOrEmpty(ingrText))
					return null;
				return new Tuple<string, Measurement, float>(ingrText, meas.Item2, meas.Item3);
			}
			var innerText = Common.StripHTML(innerHtml).Trim();
			if (string.IsNullOrEmpty(innerText)) return null;
			return CrawlerHelper.GetIngredientQuantity(dbContext, innerText, false, true);
		}

		public CrawlerBase()
		{
			webClient = new WebClient();
			dbContext = new RecipesContext();
			dbContext.Configuration.AutoDetectChangesEnabled = false;
			dbContext.Database.CommandTimeout = 120;
			recipeSource = CrawlerHelper.GetRecipeSource(dbContext, myAttribute.RecipeSourceName);
		}

		[DebuggerNonUserCode()]
		protected virtual string getHTML(string url, Dictionary<string, string> headers = null)
		{
			string html = string.Empty;
			int tries = 3;
			while (tries > 0)
			{
				try
				{
					if (headers != null)
						webClient = new WebClient();

					webClient.Headers.Add("User-Agent: Other");
					if (headers != null)
					{
						foreach (var kvp in headers)
						{
							webClient.Headers.Add(kvp.Key, kvp.Value);
						}
					}
					html = webClient.DownloadString(url);
					break;
				}
				catch
				{
					tries--;
					if (tries == 0) throw;
					System.Threading.Thread.Sleep(2500);
				}
			}
			return html;
		}

		protected virtual string postHTML(string url, string data)
		{
			string html = string.Empty;
			int tries = 3;
			while (tries > 0)
			{
				try
				{
					webClient.Headers.Add("User-Agent: Other");
					html = webClient.UploadString(url, data);
					break;
				}
				catch
				{
					tries--;
					if (tries == 0) throw;
					System.Threading.Thread.Sleep(2500);
				}
			}
			return html;
		}

		public void Crawl()
		{
			lock (CrawlerHelper.LockObject)
			{
				string sql = string.Format("select {0} from Recipe where RecipeSourceID = {1}", myAttribute.UniqueRecipeName ? "RecipeName" : "RecipeURL", recipeSource.ID);
				//existingRecipes = myAttribute.UniqueRecipeName ? recipeSource.Recipes.Select(r => r.RecipeName).ToList()
				//	: recipeSource.Recipes.Select(r => r.RecipeURL).ToList();
				var dt = new DataTable();
				using (var cmd = dbContext.Database.Connection.CreateCommand())
				{
					cmd.CommandTimeout = 300;
					cmd.CommandText = sql;
					dbContext.Database.Connection.Open();
					using (var dr = cmd.ExecuteReader())
						dt.Load(dr);
					dbContext.Database.Connection.Close();
				}
				existingRecipes = dt.Rows.OfType<DataRow>().Select(dr => dr[0].ToString()).ToList();
			}
			crawl();
		}

		protected virtual void crawl()
		{
			string html = getHTML(baseURL + allURL);
			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			var fn = System.IO.Path.Combine("..\\..\\Progress", this.GetType().Name + ".txt");

			List<string> keywordPages = getKeywordPages(doc);
			int forceStartPage = -1;
			if (System.IO.File.Exists(fn))
			{
				var parts = System.IO.File.ReadAllText(fn).Split(new string[] { " page " }, StringSplitOptions.RemoveEmptyEntries);
				var i = keywordPages.ToList().IndexOf(parts[0]);
				keywordPages = keywordPages.Skip(i).ToList();
				forceStartPage = Convert.ToInt16(parts[1]);
			}

			foreach (string keywordPage in keywordPages)
			{
				Console.WriteLine(myAttribute.RecipeSourceName + " - Keyword " + keywordPage);

				int tempInt = 1;
				int startPage = myAttribute.StartsAt0 ? 0 : 1;
				if (pageNumberURLRegex == null || string.IsNullOrEmpty(pageNumberURLRegex.MaxPageRegexPattern))
				{
					if (forceStartPage != -1)
					{
						startPage = forceStartPage;
						tempInt = startPage + 1;
						forceStartPage = -1;
					}

					for (int i = startPage; i < tempInt; i++)
					{
						crawlPage(getPageURL(keywordPage, i), keywordPage, i, ref tempInt);
						System.IO.File.WriteAllText(fn, keywordPage + " page " + i.ToString());
					}
				}
				else
				{
					html = getHTML(keywordPage);
					MatchCollection mc2 = Regex.Matches(html, pageNumberURLRegex.MaxPageRegexPattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
					if (mc2.Count < 1)
					{
						tempInt = 1;
						if (forceStartPage != -1)
						{
							startPage = forceStartPage;
							tempInt = startPage + 1;
							forceStartPage = -1;
						}

						crawlPage(keywordPage, keywordPage, startPage, ref tempInt);
						System.IO.File.WriteAllText(fn, keywordPage + " page " + tempInt.ToString());
					}
					else
					{
						float temp = 0;
						int maxPage = getMaxPage((from m2 in mc2.OfType<Match>()
												  where float.TryParse(m2.Groups[1].Value.Replace(",", ""), out temp)
												  select float.Parse(m2.Groups[1].Value.Replace(",", ""))).Max());
						if (forceStartPage != -1)
						{
							startPage = forceStartPage;
							forceStartPage = -1;
						}

						for (int i = startPage; i <= maxPage; i++)
						{
							Console.WriteLine(myAttribute.RecipeSourceName + " - Page " + i + " of " + maxPage + " - " + keywordPage);
							crawlPage(getPageURL(keywordPage, i), keywordPage, i, ref maxPage);
							System.IO.File.WriteAllText(fn, keywordPage + " page " + i.ToString());
						}
					}
				}

			}
		}

		protected virtual void updateMaxPage(HtmlDocument doc, ref int maxPage)
		{
			if (pageNumberURLRegex != null && pageNumberURLRegex.UnknownTotalPages)
				maxPage++;
		}

		protected virtual string getPageURL(string keywordPage, int pageNum)
		{
			return keywordPage + (keywordPage.EndsWith(".com") ? "/" : "") + (pageNumberURLRegex == null || pageNum == 0 ? string.Empty : string.Format(pageNumberURLRegex.URLFormat, pageNum));
		}

		protected virtual void crawlPage(string url, string keyword, int pageNum, ref int maxPage)
		{
			Console.WriteLine("Page " + pageNum.ToString() + " of " + maxPage.ToString() + " - " + keyword);
			var doc = new HtmlDocument();
			try
			{
				doc.LoadHtml(getHTML(url));
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("404") && pageNumberURLRegex != null && pageNumberURLRegex.UnknownTotalPages)
				{
					maxPage = 0;
					return;
				}
				else
					throw new Exception(ex.GetFullExceptionTextWithStackTrace());
			}
			crawlRecipeURLs(getRecipeURLs(doc), pageNum, maxPage);
			updateMaxPage(doc, ref maxPage);
		}

		protected void crawlRecipeURLs(Dictionary<string, string> urls, int pageNum, int maxPage)
		{
			foreach (var kvp in urls)
			{
				try
				{
					string recipeURL = kvp.Key;
					if (string.IsNullOrEmpty(recipeURL)) throw new NotImplementedException();

					if (!recipeURL.StartsWith(baseURL) && !recipeURL.StartsWith("http:") && !recipeURL.StartsWith("https:"))
						recipeURL = baseURL + recipeURL;

					recipeURL = recipeURL.Replace("\t", "");

					string recipeName = CrawlerHelper.ChildSafeName(Common.StripHTML(HttpUtility.HtmlDecode(kvp.Value))).Trim();

					string display = myAttribute.RecipeSourceName + " - Page " + pageNum + " of " + maxPage + " - " + (string.IsNullOrEmpty(recipeName) ? recipeURL : recipeName);

					if (existingRecipes.Contains(myAttribute.UniqueRecipeName ? recipeName : recipeURL))
					{
						Console.WriteLine(display);
						continue;
					}

					lock (CrawlerHelper.LockObject)
					{
						var rec = CreateRecipe(recipeURL, recipeName);
						if (rec != null)
							Console.ForegroundColor = ConsoleColor.Blue;
						Console.WriteLine(display);
						Console.ResetColor();
					}
					existingRecipes.Add(myAttribute.UniqueRecipeName ? recipeName : recipeURL);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.GetFullExceptionTextWithStackTrace());
					System.IO.File.AppendAllText("errors.txt", ex.GetFullExceptionTextWithStackTrace());
				}
			}
		}

		public virtual Recipe CreateRecipe(string recipeURL, string recipeName)
		{
			string html = getHTML(recipeURL);

			if (string.IsNullOrEmpty(html))
				return null;

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			if (string.IsNullOrEmpty(recipeName))
			{
				var recipeNameNode = doc.DocumentNode.SelectSingleNode(recipeNameXPath);
				if (recipeNameNode == null) throw new NotImplementedException();
				recipeName = CrawlerHelper.ChildSafeName(HttpUtility.HtmlDecode(Common.StripHTML(recipeNameNode.InnerText))).Trim();
			}

			Recipe rec = new Recipe();
			rec.RecipeSourceID = recipeSource.RecipeSourceID;
			rec.RecipeName = recipeName;
			rec.RecipeURL = recipeURL;

			var directions = doc.DocumentNode.SelectNodes(directionsXPath);
			rec.Directions = string.Empty;
			if (directions != null)
			{
				foreach (HtmlNode d in directions)
				{
					rec.Directions += Common.StripHTML(d.InnerText) + "\r\n";
				}
				rec.Directions = rec.Directions.Trim();
			}
			rec.Rating = getRating(doc.DocumentNode);

			// if (rec.Rating.GetValueOrDefault() > 0 && rec.Rating.Value < 4)
			if (rec.Rating.GetValueOrDefault() < 4 && (!myAttribute.IncludeNoRating || rec.Rating != null))
				return null;

			rec.NumberOfServings = getServings(doc.DocumentNode);

			var ingredients = getIngredients(doc.DocumentNode);
			if (ingredients == null)
				return null;

			rec.RecipeIngredientMeasurements = new List<RecipeIngredientMeasurement>();
			foreach (var ing in ingredients)
				rec.RecipeIngredientMeasurements.Add(ing);

			rec.RecipeImages = new List<RecipeImage>();
			foreach (var img in getRecipeImages(doc.DocumentNode))
				rec.RecipeImages.Add(img);

			dbContext.Recipes.Add(rec);
			dbContext.SaveChanges();
			return rec;
		}

		protected List<RecipeIngredientMeasurement> getIngredients(HtmlNode node)
		{
			var ingredients = node.SelectNodes(ingredientsXPath);
			if (ingredients == null) return null;
			var recIngrs = new List<RecipeIngredientMeasurement>();
			foreach (var ingredient in ingredients)
			{
				var ing = getIngredient(ingredient);
				if (ing == null || string.IsNullOrEmpty(ing.Item1))
					continue;
				recIngrs.Add(CrawlerHelper.GetIngredient(dbContext, ing.Item1, ing.Item2, ing.Item3));
			}
			return recIngrs;
		}

		protected virtual List<HtmlNode> getRecipeImagesPageNodes(HtmlNode node)
		{
			return new List<HtmlNode>() { node };
		}

		protected virtual List<RecipeImage> getRecipeImages(HtmlNode node)
		{
			var pageNodes = getRecipeImagesPageNodes(node);
			List<RecipeImage> images = new List<RecipeImage>();
			foreach (var pageNode in pageNodes)
			{
				List<string> imageURLs = new List<string>();
				var imageNodes = pageNode.SelectNodes(imagesXPath);
				if (imageNodes == null) continue;
				foreach (var imgNode in imageNodes)
				{
					var imageURL = getImageURL(imgNode);
					if (imageURL == null) continue;
					if (!imageURL.StartsWith(baseURL) && !imageURL.StartsWith("http://") && !imageURL.StartsWith("https://") && !imageURL.StartsWith("//"))
						imageURL = baseURL + imageURL;

					if (imageURLs.Contains(imageURL)) continue;
					imageURLs.Add(imageURL);

					RecipeImage img = new RecipeImage();
					img.ImageURL = imageURL;
					img.LocalImagePath = null;
					images.Add(img);
				}
			}

			return images;
		}

		protected virtual int? getServings(HtmlNode node)
		{
			var servingsNode = node.SelectSingleNode(servingsXPath);
			if (servingsNode != null)
			{
				int tempInt = -1;
				if (!int.TryParse(Common.StripHTML(servingsNode.InnerText), out tempInt))
				{
					Match servingsMatch2 = Regex.Match(Common.StripHTML(servingsNode.InnerText), "(\\d+)");
					if (!int.TryParse(servingsMatch2.Groups[1].Value, out tempInt))
					{

					}
				}
				return tempInt;
			}
			return null;
		}

		protected virtual float? getRating(HtmlNode node)
		{
			var ratingNode = node.SelectSingleNode(ratingXPath);
			if (ratingNode != null)
			{
				float rating = 0;
				if (!float.TryParse(Common.StripHTML(ratingNode.InnerText), out rating))
				{

				}
				return rating;
			}
			return null;
		}

		protected virtual int getMaxPage(float maxPageMatch)
		{
			return (int)maxPageMatch;
		}

		protected virtual string getImageURL(HtmlNode node)
		{
			var attr = node.Attributes["content"];
			if (attr != null) return attr.Value;
			return (node.Attributes["src"] ?? node.Attributes["data-src"]).Value;
		}

		protected virtual List<string> getKeywordPages(HtmlDocument document)
		{
			List<string> keywordPages = new List<string>();
			keywordPages.Add(baseURL + allURL);
			return keywordPages;
		}
	}

	public class PageNumbers
	{
		public string MaxPageRegexPattern { get; set; }
		public string URLFormat { get; set; }
		public bool UnknownTotalPages { get; set; }
	}
}
