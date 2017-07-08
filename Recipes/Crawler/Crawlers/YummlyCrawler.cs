using Newtonsoft.Json;
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
	[RecipeSource("Yummly", StartPage = 0)]
	public class YummlyCrawler : CrawlerBase
	{
		protected override string baseURL
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		protected override void crawl(int startPage)
		{
			var allRecipesURLS = DbContext.Recipes.Where(r => r.RecipeURL.Contains("allrecipes"))
				.Select(r => r.RecipeURL).ToList();

			var keywords = Properties.Resources.Keywords
				.ToLower()
				.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
				.Union(
					 Properties.Resources.Keywords
					.ToLower()
					.Split(new string[] { "\r\n", " " }, StringSplitOptions.RemoveEmptyEntries)
				)
				.Distinct();
			foreach (var kw in keywords)
			{
				startPage = 1;
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
					}
					var yummlyResult = JsonConvert.DeserializeObject<YummlyResult>(json);
					if (yummlyResult.feed.Count < 1)
						break;

					foreach (var feed in yummlyResult.feed)
					{
						string display = $"{i++} of {yummlyResult.totalMatchCount} - Yummly - {feed.content.details.name}";

						Console.WriteLine(display);
						if (existingRecipes.Contains(feed.content.details.attribution.url) ||
							allRecipesURLS.Contains(feed.content.details.attribution.url))
						{
							continue;
						}

						if (feed.content.ingredientLines.Count < 1) continue;

						Recipe rec = new Recipe();
						rec.RecipeSourceID = recipeSource.RecipeSourceID;
						rec.RecipeName = feed.content.details.name;
						rec.RecipeURL = feed.content.details.attribution.url;

						rec.Directions = feed.content.preparationSteps == null ? feed.content.details.directionsUrl : string.Join("\r\n", feed.content.preparationSteps);
						rec.Rating = feed.content.details.rating;

						if (rec.Rating.GetValueOrDefault() > 0 && rec.Rating.GetValueOrDefault() < 4)
							continue;

						Console.WriteLine("****");

						rec.NumberOfServings = feed.content.details.numberOfServings;

						foreach (var ingredient in feed.content.ingredientLines)
						{
							if (ingredient.ingredient == null && string.IsNullOrEmpty(ingredient.wholeLine)) continue;

							if (ingredient.ingredient == null)
							{
								var ing = CrawlerHelper.GetIngredientQuantity(DbContext, ingredient.wholeLine, false, false);
								rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(DbContext, ing.Item1, ing.Item2, ing.Item3));
							}
							else
								rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(DbContext, ingredient.ingredient,
									ingredient.unit == null ? null : CrawlerHelper.GetMeasurement(DbContext, ingredient.unit, true),
									(double)ingredient.quantity.GetValueOrDefault()));
						}

						foreach (var im in feed.content.details.images)
						{
							RecipeImage img = new RecipeImage();
							img.ImageURL = im.hostedLargeUrl;
							img.LocalImagePath = null;
							rec.RecipeImages.Add(img);
						}

						DbContext.Recipes.Add(rec);
						DbContext.SaveChanges();

						existingRecipes.Add(rec.RecipeURL);
					}

					startPage += pageSize;
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
