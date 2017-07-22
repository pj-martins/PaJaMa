using PaJaMa.Common;

using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using PaJaMa.Recipes.Model.Entities;

namespace Crawler.Crawlers
{
	[RecipeSource("Epicurious")]
	public class EpicuriousCrawlerApi : EpicuriousCrawler
	{
		protected override void crawl()
		{
			List<string> keywords = Properties.Resources.Keywords.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
			int forceStartPage = -1;
			var partial = keywords.ToList();
			var fn = Path.Combine("..\\..\\Progress", this.GetType().Name + ".txt");
			if (System.IO.File.Exists(fn))
			{
				var parts = System.IO.File.ReadAllText(fn).Split(new string[] { " page " }, StringSplitOptions.RemoveEmptyEntries);
				var i = keywords.ToList().IndexOf(parts[0]);
				partial = keywords.Skip(i).ToList();
				forceStartPage = Convert.ToInt32(parts[1]);
			}
			foreach (var kw in partial)
			{
				int startPage = 1;
				if (forceStartPage != -1)
				{
					startPage = forceStartPage;
					forceStartPage = -1;
				}
				var headers = new Dictionary<string, string>();
				headers.Add("x-requested-with", "XMLHttpRequest");
				int totalPages = 0;
				do
				{
					System.IO.File.WriteAllText(fn, kw + " page " + startPage.ToString());

					string json = string.Empty;
					int tries = 3;
					while (tries-- >= 0)
					{
						try
						{
							json = getHTML($"http://www.epicurious.com/search/{kw}?page={startPage}&xhr=true", headers);
						}
						catch
						{
							System.Threading.Thread.Sleep(100);
							tries--;
						}
					}
					if (tries <= 0 && string.IsNullOrEmpty(json)) break;
					var searchResults = Newtonsoft.Json.JsonConvert.DeserializeObject<Api>(json);
					totalPages = searchResults.page.count;
					Console.WriteLine($"Page {startPage} of {totalPages} - {kw}");
					if (totalPages < 1)
						break;

					foreach (var item in searchResults.items)
					{
						if (item.type != "recipe") continue;
						var url = baseURL + item.url;
						if (existingRecipes.Contains(url)) continue;
						string display = $"{recipeSource.RecipeSourceName} - Page {startPage} of {totalPages} - {item.hed}";
						if (item.aggregateRating < 3) continue;
						if (item.ingredients == null || item.ingredients.Count < 1) continue;
						Console.ForegroundColor = ConsoleColor.Blue;
						Console.WriteLine(display);
						Recipe rec = new Recipe();
						rec.RecipeSourceID = recipeSource.RecipeSourceID;
						rec.RecipeName = item.hed;
						rec.RecipeURL = url;
						rec.Directions = string.Join("\r\n", item.prepSteps.ToArray());
						rec.Rating = 5 * item.aggregateRating / 4;

						foreach (var ingredient in item.ingredients)
						{
							if (string.IsNullOrEmpty(ingredient)) continue;
							var ing = CrawlerHelper.GetIngredientQuantity(dbContext, ingredient, false, true);
							if (string.IsNullOrEmpty(ing.Item1)) continue;
							rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(dbContext, ing.Item1, ing.Item2, ing.Item3));
						}

						if (item.photoData != null)
						{
							RecipeImage img = new RecipeImage();
							img.ImageURL = $"http://assets.epicurious.com/photos/{item.photoData.id}/6:4/w_620%2Ch_413/{item.photoData.filename}";
							img.LocalImagePath = null;
							rec.RecipeImages.Add(img);
						}

						dbContext.Recipes.Add(rec);
						dbContext.SaveChanges();
						existingRecipes.Add(url);
						Console.ResetColor();
					}

					startPage++;
				}
				while (startPage <= totalPages);
			}
		}
	}

	public class Api
	{
		public List<ApiItem> items { get; set; }
		public ApiPage page { get; set; }
	}

	public class ApiItem
	{
		public string type { get; set; }
		public string url { get; set; }
		public string hed { get; set; }
		public float aggregateRating { get; set; }
		public List<string> ingredients { get; set; }
		public List<string> prepSteps { get; set; }
		public ApiPhoto photoData { get; set; }
	}

	public class ApiPage
	{
		public int count { get; set; }
	}

	public class ApiPhoto
	{
		public string id { get; set; }
		public string filename { get; set; }
	}
}
