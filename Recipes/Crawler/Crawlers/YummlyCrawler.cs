﻿using Newtonsoft.Json;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Model.Entities;
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
	[RecipeSource("Yummly", StartsAt0 = true)]
	public class YummlyCrawler : CrawlerBase
	{
		protected override string baseURL
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		protected override string recipesXPath
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		private Dictionary<string, Type> _externals = new Dictionary<string, Type>()
		{
			{ "http://allrecipes.com", typeof(AllRecipesCrawler) },
			{ "http://www.epicurious.com/", typeof(EpicuriousCrawler) },
			{ "http://www.foodnetwork.com/", typeof(FoodNetworkCrawler) },
			{ "http://www.food.com/", typeof(FoodCrawler) },
			{ "http://www.seriouseats.com/", typeof(SeriousEatsCrawler) },
			{ "http://www.tasteofhome.com/", typeof(TasteOfHomeCrawler) },
			{ "http://www.myrecipes.com/", typeof(MyRecipesCrawler) },
			{ "https://food52.com/", typeof(Food52Crawler) },
			{ "http://www.food52.com/", typeof(Food52Crawler) },
		};

		protected override void crawl()
		{
			var keywords = Properties.Resources.Keywords.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			int forceStartPage = -1;

			var fn = System.IO.Path.Combine("..\\..\\Progress", this.GetType().Name + ".txt");

			if (System.IO.File.Exists(fn))
			{
				var parts = System.IO.File.ReadAllText(fn).Split(new string[] { " page " }, StringSplitOptions.RemoveEmptyEntries);
				var i = keywords.ToList().IndexOf(parts[0]);
				keywords = keywords.Skip(i).ToArray();
				forceStartPage = Convert.ToInt32(parts[1]);
			}
			foreach (var kw in keywords)
			{
				int startPage = 1;
				if (forceStartPage != -1)
				{
					startPage = forceStartPage;
					forceStartPage = -1;
				}
				int pageSize = 36;
				int i = startPage;
				while (true)
				{
					int tries = 3;
					var json = string.Empty;
					while (tries > 0)
					{
						json = getHTML($"https://mapi.yummly.com/mapi/v15/content/search?q={kw}&start={startPage}&maxResult={pageSize}&fetchUserCollections=false&allowedContent[]=single_recipe&allowedContent[]=suggested_source&allowedContent[]=suggested_search&allowedContent[]=related_search&allowedContent[]=article&allowedContent[]=video&facetField[]=diet&facetField[]=holiday&facetField[]=technique&facetField[]=cuisine&facetField[]=course&facetField[]=source&facetField[]=brand&facetField[]=difficulty&facetField[]=dish&facetField[]=adtag&guided-search=true&solr.view_type=search_internal");
						if (!string.IsNullOrEmpty(json))
							break;
						tries--;
						System.Threading.Thread.Sleep(1000);
					}
					if (string.IsNullOrEmpty(json))
						break;
					var yummlyResult = JsonConvert.DeserializeObject<YummlyResult>(json);
					if (yummlyResult.feed.Count < 1)
						break;

					foreach (var feed in yummlyResult.feed)
					{
						string display = $"{i++} of {yummlyResult.totalMatchCount} - Yummly - {feed.content.details.name}";

						if (feed.content.details.attribution.url.Length > 255)
							feed.content.details.attribution.url = feed.content.details.attribution.url.Substring(0, 255);

						if (existingRecipes.Contains(feed.content.details.attribution.url))
						{
							Console.WriteLine(display);
							continue;
						}

						var external = _externals.Keys.FirstOrDefault(k => feed.content.details.directionsUrl.StartsWith(k));
						if (external != null)
						{
							if (dbContext.Recipes.Any(r => r.RecipeURL.ToLower() == feed.content.details.directionsUrl.ToLower()
								|| r.RecipeURL.ToLower().Replace("https://", "http://") == feed.content.details.directionsUrl.ToLower().Replace("https://", "http://")))
							{
								Console.WriteLine(display);
								continue;
							}
							try
							{
								CrawlerBase crawler = Activator.CreateInstance(_externals[external]) as CrawlerBase;
								var arec = crawler.CreateRecipe(feed.content.details.directionsUrl, feed.content.details.name);
								if (arec != null)
								{
									Console.ForegroundColor = ConsoleColor.Yellow;
									Console.WriteLine("GOT FROM " + external);
									Console.ResetColor();
									continue;
								}
							}
							catch { }
						}

						if (feed.content.ingredientLines.Count < 1) continue;

						Recipe rec = new Recipe();
						rec.RecipeSourceID = recipeSource.RecipeSourceID;
						rec.RecipeName = feed.content.details.name;
						rec.RecipeURL = feed.content.details.attribution.url;

						rec.Directions = feed.content.preparationSteps == null ? feed.content.details.directionsUrl : string.Join("\r\n", feed.content.preparationSteps);
						rec.Rating = feed.content.details.rating;

						if (rec.Rating.GetValueOrDefault() < 4)
							continue;

						Console.ForegroundColor = ConsoleColor.Blue;
						Console.WriteLine(display);
						Console.ResetColor();

						rec.NumberOfServings = feed.content.details.numberOfServings;

						rec.RecipeIngredientMeasurements = new List<RecipeIngredientMeasurement>();
						foreach (var ingredient in feed.content.ingredientLines)
						{
							if (ingredient.ingredient == null && string.IsNullOrEmpty(ingredient.wholeLine)) continue;

							if (ingredient.ingredient == null)
							{
								var ing = CrawlerHelper.GetIngredientQuantity(dbContext, ingredient.wholeLine, false, false);
								if (string.IsNullOrEmpty(ing.Item1)) continue;
								rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(dbContext, ing.Item1, ing.Item2, ing.Item3));
							}
							else
								rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(dbContext, ingredient.ingredient,
									ingredient.unit == null ? null : CrawlerHelper.GetMeasurement(dbContext, ingredient.unit, true),
									(double)ingredient.quantity.GetValueOrDefault()));
						}

						rec.RecipeImages = new List<RecipeImage>();
						foreach (var im in feed.content.details.images)
						{
							RecipeImage img = new RecipeImage();
							img.ImageURL = im.hostedLargeUrl;
							img.LocalImagePath = null;
							rec.RecipeImages.Add(img);
						}

						dbContext.Recipes.Add(rec);
						dbContext.SaveChanges();

						existingRecipes.Add(rec.RecipeURL);
					}

					startPage += pageSize;
					System.IO.File.WriteAllText(fn, kw + " page " + startPage.ToString());
				}
			}
		}

		#region OLD
		//        private Dictionary<int, List<string>> _existingRecipes = new Dictionary<int, List<string>>();

		//        protected override void crawl(int startPage)
		//        {
		//            WebClient wc = new WebClient();
		//            int curr = startPage;

		//            while (true)
		//            {
		//                Console.WriteLine("Yummly - Page " + curr.ToString());

		//                string html = getHTML("http://www.yummly.com/search/more/" + curr.ToString() + "?sortBy=popular");
		//                MatchCollection mc = Regex.Matches(html, @"<a class=""y-full"" href=""(.*?)"">.*?<h3>(.*?)</h3>.*?<span class=""y-source tiny"">.*?<a .*?/>(.*?)</a>.*?</span>", RegexOptions.Singleline);

		//                if (mc.Count < 1)
		//                    break;

		//                foreach (Match m in mc)
		//                {
		//                    string sourceName = m.Groups[3].Value.Replace("&#x27;", "'");
		//                    string recipeURL = "http://www.yummly.com" + m.Groups[1].Value;

		//                    var matchingCrawlerType = (from t in this.GetType().Assembly.GetTypes()
		//                                               let attr = t.GetCustomAttributes(typeof(RecipeSourceAttribute), true).FirstOrDefault() as RecipeSourceAttribute
		//                                               where attr != null && attr.RecipeSourceName.ToLower() == sourceName.ToLower()
		//                                               select new { Type = t, Attr = attr }).FirstOrDefault();

		//                    string recipeName = m.Groups[2].Value;

		//                    RecipeSource src = null;
		//                    if (matchingCrawlerType == null)
		//                        src = CrawlerHelper.GetRecipeSource(DbContext, sourceName);
		//                    else
		//                        src = CrawlerHelper.GetRecipeSource(DbContext, matchingCrawlerType.Attr.RecipeSourceName);

		//                    if (src.RecipeSourceID == 333)
		//                        continue;

		//                    if (!_existingRecipes.ContainsKey(src.RecipeSourceID))
		//                        _existingRecipes.Add(src.RecipeSourceID, src.Recipes.Select(r => r.RecipeURL).ToList());

		//                    if (_existingRecipes[src.RecipeSourceID].Contains(recipeURL))
		//                        continue;

		//                    string display = sourceName + " - " + curr.ToString() + " - " + recipeName;

		//                    Console.WriteLine("* " + display);

		//                    string html2 = string.Empty;
		//                    int tries = 3;
		//                    while (tries > 0)
		//                    {
		//                        try
		//                        {
		//                            html2 = wc.DownloadString("http://www.yummly.com" + m.Groups[1].Value);
		//                            break;
		//                        }
		//                        catch
		//                        {
		//                            tries--;
		//                            System.Threading.Thread.Sleep(100);
		//                            continue;
		//                        }
		//                    }

		//                    if (tries <= 0)
		//                        continue;

		//                    string destURL = string.Empty;

		//                    Match m2 = Regex.Match(html2, "<button class=\"open-window btn-tertiary mixpanel-track\" id=\"source-full-directions\" link=\"(.*?)\"");
		//                    if (!m2.Success)
		//                    {
		//                        matchingCrawlerType = null;
		//                    }
		//                    else
		//                    {
		//                        destURL = m2.Groups[1].Value;
		//                        if (!destURL.StartsWith("http://"))
		//                        {
		//                            html2 = getHTML("http://www.yummly.com" + m2.Groups[1].Value);
		//                            m2 = Regex.Match(html2, "<iframe .*? src=\"(.*?)\"");

		//                            if (!m2.Success)
		//                                throw new NotImplementedException();

		//                            destURL = m2.Groups[1].Value;
		//                        }
		//                    }

		//                    Recipe rec = null;
		//                    if (matchingCrawlerType != null)
		//                    {
		//                        CrawlerBase crawler = Activator.CreateInstance(matchingCrawlerType.Type) as CrawlerBase;
		//                        crawler.DbContext = DbContext;
		//                        try
		//                        {
		//                            lock (CrawlerHelper.LockObject)
		//                                rec = crawler.CreateRecipe(m2.Groups[1].Value, recipeName, src.RecipeSourceID);
		//                        }
		//                        catch
		//                        {
		//                            rec = null;
		//                        }
		//                    }

		//                    if (rec == null)
		//                    {
		//                        lock (CrawlerHelper.LockObject)
		//                            rec = CreateRecipe(recipeURL, recipeName, src.RecipeSourceID);
		//                        if (string.IsNullOrEmpty(rec.Directions))
		//                        {
		//                            rec.Directions = destURL;
		//                            DbContext.SaveChanges();
		//                        }
		//                    }

		//                    _existingRecipes[src.RecipeSourceID].Add(recipeURL);
		//                }

		//                curr += 30;
		//            }
		//        }

		//        protected override string baseURL
		//        {
		//            get { return "http://www.yummly.com"; }
		//        }

		//        protected override string allURL
		//        {
		//            get { throw new NotImplementedException(); }
		//        }

		//        protected override string recipesRegexPattern
		//        {
		//            get { throw new NotImplementedException(); }
		//        }

		//        protected override string directionsRegexPattern
		//        {
		//            get { return "<ol itemprop=\"recipeInstructions\">(.*?)</ol>"; }
		//        }

		//        protected override string ratingRegexPattern
		//        {
		//            get { return "__NONE__"; }
		//        }

		//        protected override string servingsRegexPattern
		//        {
		//            get { return "itemprop=\"recipeYield\">(.*?)</p>"; }
		//        }

		//        protected override string ingredientsRegexPattern
		//        {
		//            get { return "<li itemprop=\"ingredients\" class=\"ingredient\">(.*?)</li>"; }
		//        }

		//        protected override string imageRegexPattern
		//        {
		//            get { return "<img itemprop=\"image\" src=\"(.*?)\""; }
		//        }
		//    }
		//}
		#endregion
	}

	public class YummlyResult
	{
		public List<YummlyFeed> feed { get; set; }
		public int totalMatchCount { get; set; }
	}

	public class YummlyFeed
	{
		public YummlyContent content { get; set; }
		public override string ToString()
		{
			return content.details.name;
		}
	}

	public class YummlyContent
	{
		public List<string> preparationSteps { get; set; }
		public YummlyDetails details { get; set; }
		public List<YummlyIngredient> ingredientLines { get; set; }
	}

	public class YummlyAttribution
	{
		public string url { get; set; }
	}

	public class YummlyDetails
	{
		public string name { get; set; }
		public string directionsUrl { get; set; }
		public int? numberOfServings { get; set; }
		public float? rating { get; set; }
		public YummlyAttribution attribution { get; set; }
		public List<YummlyImage> images { get; set; }
	}

	public class YummlyIngredient
	{
		public string wholeLine { get; set; }
		public string ingredient { get; set; }
		public float? quantity { get; set; }
		public string unit { get; set; }
		public string remainder { get; set; }
	}

	public class YummlyImage
	{
		public string hostedLargeUrl { get; set; }
	}
	/*
	 * ingredientLines: [
{
wholeLine: "12 ounces ziti (or any pasta shape)",
ingredient: "ziti",
quantity: 12,
unit: "ounce",
amount: {
metric: {
unit: {
name: "gram",
plural: "grams",
decimal: true
},
quantity: 340
},
imperial: {
unit: {
name: "ounce",
plural: "ounces",
decimal: false
},
quantity: 12
}
},
category: "Pasta",
relatedRecipeSearchTerm: [
{
allowedIngredient: "ziti"
}
],
remainder: "or any pasta shape"
},
*/
}
