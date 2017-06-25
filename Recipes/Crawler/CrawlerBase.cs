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

		public RecipesContext DbContext { get; set; }
		protected WebClient webClient { get; private set; }
		protected RecipeSource recipeSource { get; private set; }
		protected List<string> existingRecipes { get; private set; }

		protected abstract string baseURL { get; }
		protected virtual string allURL { get { return string.Empty; } }

		protected virtual string recipesXPath { get { return string.Empty; } }
		protected virtual Tuple<string, string> getRecipeURL(HtmlNode node)
		{
			return new Tuple<string, string>(node.Attributes["href"].Value, node.InnerText.Trim());
		}

		protected virtual string servingsXPath
		{
			get { return "//*[@itemprop='yield']"; }
		}

		protected virtual string servings2XPath
		{
			get { return "//*[@itemprop='recipeYield']"; }
		}

		protected virtual string recipeNameXPath
		{
			get { return "//*[@itemprop='name']"; }
		}

		protected virtual string directionsXPath
		{
			get { return "//*[@itemprop='instructions']"; }
		}

		protected virtual string directions2XPath
		{
			get { return "//*[@itemprop='recipeInstructions']"; }
		}

		protected virtual string ingredientsXPath
		{
			get { return "//*[@itemprop='ingredients']"; }
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
			get { return "//*[@itemprop='photo']"; }
		}

		protected virtual string images2XPath
		{
			get { return "//*[@itemprop='image']"; }
		}

		protected virtual string ratingXPath
		{
			get { return "//*[@itemprop='rating']"; }
		}

		protected virtual string rating2XPath
		{
			get { return "//*[@itemprop='ratingValue']"; }
		}

		protected virtual PageNumbers pageNumberURLRegex { get { return null; } }

		protected virtual Dictionary<string, string> getRecipeURLs(HtmlDocument doc)
		{
			var dict = new Dictionary<string, string>();

			var children = doc.DocumentNode.SelectNodes(recipesXPath);
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
			var doc = new HtmlDocument();
			doc.LoadHtml(node.InnerHtml);
			var qty = doc.DocumentNode.SelectSingleNode(ingredientAmountXPath);
			if (qty != null)
			{
				var ingr = doc.DocumentNode.SelectSingleNode(ingredientNameXPath);
				var meas = CrawlerHelper.GetIngredientQuantity(DbContext, qty.InnerText, true, false);
				var ingrText = PaJaMa.Common.Common.StripHTML(ingr.InnerText).Trim();
				if (meas.Item2 == null && string.IsNullOrEmpty(ingrText))
					return null;
				return new Tuple<string, Measurement, float>(ingrText, meas.Item2, meas.Item3);
			}
			var innerText = Common.StripHTML(node.InnerHtml).Trim();
			if (string.IsNullOrEmpty(innerText)) return null;
			return CrawlerHelper.GetIngredientQuantity(DbContext, innerText, false, true);
		}

		public CrawlerBase()
		{
			webClient = new WebClient();
        }

		[DebuggerNonUserCode()]
		protected virtual string getHTML(string url)
		{
			string html = string.Empty;
			int tries = 3;
			while (tries > 0)
			{
				try
				{
                    webClient.Headers.Add("User-Agent: Other");
                    html = webClient.DownloadString(url);
					break;
				}
				catch
				{
					tries--;
					System.Threading.Thread.Sleep(2500);
				}
			}
			return html;
		}

		public void Crawl(RecipesContext db)
		{
			DbContext = db;
			lock (CrawlerHelper.LockObject)
			{
				recipeSource = CrawlerHelper.GetRecipeSource(DbContext, myAttribute.RecipeSourceName);
				string sql = string.Format("select {0} from Recipe where RecipeSourceID = {1}", myAttribute.UniqueRecipeName ? "RecipeName" : "RecipeURL", recipeSource.ID);
				//existingRecipes = myAttribute.UniqueRecipeName ? recipeSource.Recipes.Select(r => r.RecipeName).ToList()
				//	: recipeSource.Recipes.Select(r => r.RecipeURL).ToList();
				var dt = new DataTable();
				using (var cmd = db.Database.Connection.CreateCommand())
				{
					cmd.CommandTimeout = 300;
					cmd.CommandText = sql;
					db.Database.Connection.Open();
					using (var dr = cmd.ExecuteReader())
						dt.Load(dr);
					db.Database.Connection.Close();
				}
				existingRecipes = dt.Rows.OfType<DataRow>().Select(dr => dr[0].ToString()).ToList();
			}
			crawl(myAttribute.StartPage);
		}

		protected virtual void crawl(int startPage)
		{
			string html = getHTML(baseURL + allURL);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);


			List<string> keywordPages = getKeywordPages(doc);

			foreach (string keywordPage in keywordPages)
			{
				Console.WriteLine(myAttribute.RecipeSourceName + " - Keyword " + keywordPage);

				int tempInt = 1;
                if (pageNumberURLRegex == null || string.IsNullOrEmpty(pageNumberURLRegex.MaxPageRegexPattern))
                {
                    for (int i = 0; i < tempInt; i++)
                    {
                        crawlPage(getPageURL(keywordPage, i), i, ref tempInt);
                    }
                }
                else
                {
                    html = getHTML(keywordPage);
                    MatchCollection mc2 = Regex.Matches(html, pageNumberURLRegex.MaxPageRegexPattern, RegexOptions.Singleline);
                    if (mc2.Count < 1)
                    {
                        tempInt = 1;
                        crawlPage(keywordPage, 1, ref tempInt);
                    }
                    else
                    {
                        float temp = 0;
                        int maxPage = getMaxPage((from m2 in mc2.OfType<Match>()
                                                  where float.TryParse(m2.Groups[1].Value.Replace(",", ""), out temp)
                                                  select float.Parse(m2.Groups[1].Value.Replace(",", ""))).Max());

                        for (int i = startPage; i <= maxPage; i++)
                        {
                            Console.WriteLine(myAttribute.RecipeSourceName + " - Page " + i + " of " + maxPage);
                            crawlPage(getPageURL(keywordPage, i), i, ref maxPage);
                        }
                    }
                }

			}
		}

		protected virtual void updateMaxPage(HtmlDocument doc, ref int maxPage)
		{
		}

		protected virtual string getPageURL(string keywordPage, int pageNum)
		{
			return keywordPage + (pageNumberURLRegex == null ? string.Empty : string.Format(pageNumberURLRegex.URLFormat, pageNum));
		}

		protected virtual void crawlPage(string url, int pageNum, ref int maxPage)
		{
            var doc = new HtmlDocument();
            doc.LoadHtml(getHTML(url));
			var urls = getRecipeURLs(doc);
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

					if (existingRecipes.Contains(myAttribute.UniqueRecipeName ? recipeName : recipeURL))
						continue;

					string display = myAttribute.RecipeSourceName + " - Page " + pageNum + " of " + maxPage + " - " + (string.IsNullOrEmpty(recipeName) ? recipeURL : recipeName);
					Console.WriteLine("* " + display);

					lock (CrawlerHelper.LockObject)
						CreateRecipe(recipeURL, recipeName, recipeSource.RecipeSourceID);

					existingRecipes.Add(myAttribute.UniqueRecipeName ? recipeName : recipeURL);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.GetFullExceptionTextWithStackTrace());
					System.IO.File.AppendAllText("errors.txt", ex.GetFullExceptionTextWithStackTrace());
				}
			}

			updateMaxPage(doc, ref maxPage);
		}

		public virtual Recipe CreateRecipe(string recipeURL, string recipeName, int recipeSourceID)
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
			rec.RecipeSourceID = recipeSourceID;
			rec.RecipeName = recipeName;
			rec.RecipeURL = recipeURL;

			var directions = doc.DocumentNode.SelectNodes(directionsXPath);
			if (directions == null || !directions.Any())
				directions = doc.DocumentNode.SelectNodes(directions2XPath);
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



            // FOR SMALLER DBS:
            if (rec.Rating != null && rec.Rating.Value < 4)
                return null;

			var servingsNode = doc.DocumentNode.SelectSingleNode(servingsXPath);
			if (servingsNode == null) servingsNode = doc.DocumentNode.SelectSingleNode(servings2XPath);
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
				rec.NumberOfServings = tempInt;
			}

			var ingredients = getIngredients(doc.DocumentNode);
			if (ingredients == null)
				return null;

			foreach (var ing in ingredients)
				rec.RecipeIngredientMeasurements.Add(ing);

			foreach (var img in getRecipeImages(doc.DocumentNode))
				rec.RecipeImages.Add(img);

			DbContext.Recipes.Add(rec);
			DbContext.SaveChanges();
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
				recIngrs.Add(CrawlerHelper.GetIngredient(DbContext, ing.Item1, ing.Item2, ing.Item3));
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
				if (imageNodes == null) imageNodes = pageNode.SelectNodes(images2XPath);
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

		protected virtual float? getRating(HtmlNode node)
		{
			var ratingNode = node.SelectSingleNode(ratingXPath);
			if (ratingNode == null) ratingNode = node.SelectSingleNode(rating2XPath);
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
	}
}
