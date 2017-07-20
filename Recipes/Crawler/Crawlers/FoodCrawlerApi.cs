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
	[RecipeSource("Food.com")]
	public class FoodCrawlerApi : FoodCrawler
	{
		protected override void crawl()
		{
			List<string> keywords;
			if (System.IO.File.Exists("newkeywords.txt"))
				keywords = System.IO.File.ReadAllLines("newkeywords.txt").ToList();
			else
				keywords = Properties.Resources.Keywords.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

			int forceStartPage = -1;
			var partial = keywords.ToList();
			if (System.IO.File.Exists("food.txt"))
			{
				var parts = System.IO.File.ReadAllText("food.txt").Split(new string[] { " page " }, StringSplitOptions.RemoveEmptyEntries);
				var i = keywords.ToList().IndexOf(parts[0]);
				partial = keywords.Skip(i).ToList();
				forceStartPage = Convert.ToInt32(parts[1]);
			}
			foreach (var kw in partial)
			{
				bool somethingFound = false;
				int startPage = 1;
				if (forceStartPage != -1)
				{
					startPage = forceStartPage;
					forceStartPage = -1;
				}
				int i = startPage;
				while (true)
				{
					int tries = 3;
					var json = string.Empty;
					while (tries > 0)
					{
						json = postHTML($"https://api.food.com/external/v1/nlp/search", @"{""contexts"":[""{\""degreesSeparation\"":\""0\"",\""paths\"":[\""/~asset\""],\""name\"":\""" + kw + @"\"",\""cleanedName\"":\""" + kw + @"\"",\""type\"":\""PRIMARY\"",\""searchType\"":\""NORMAL\"",\""tagged_content_count\"":\""13\"",\""tagLevel\"":\""2\"",\""userToken\"":\""true\"",\""searched\"":\""true\""}""],""searchTerm"":""" + kw + @""",""pn"":" + startPage + "}");
						if (!string.IsNullOrEmpty(json))
							break;
						tries--;
					}

					var searchResults = Newtonsoft.Json.JsonConvert.DeserializeObject<Api>(json);
					Console.WriteLine("Total results: " + searchResults.Response.TotalResultsCount.ToString() + " - " + kw);
					if (searchResults.Response.Results == null || searchResults.Response.Results.Count < 1)
						break;
					
					somethingFound = true;

					var urls = new Dictionary<string, string>();
					foreach (var result in searchResults.Response.Results)
					{
						if (result.record_type != "Recipe") continue;
						if (result.main_rating < 4) continue;
						if (string.IsNullOrEmpty(result.record_url)) continue;
						if (urls.ContainsKey(result.record_url)) continue;
						urls.Add(result.record_url, result.main_title);
					}

					crawlRecipeURLs(urls, startPage, searchResults.Response.TotalResultsCount / 10);
					startPage++;
					System.IO.File.WriteAllText("food.txt", kw + " page " + startPage.ToString());
				}

				if (!somethingFound)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Removing " + kw);
					Console.ResetColor();
					keywords.Remove(kw);
					System.IO.File.WriteAllText("newkeywords.txt", string.Join("\r\n", keywords.ToArray()));
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

	//public class YummlyResult
	//{
	//	public List<YummlyFeed> feed { get; set; }
	//	public int totalMatchCount { get; set; }
	//}

	//public class YummlyFeed
	//{
	//	public YummlyContent content { get; set; }
	//	public override string ToString()
	//	{
	//		return content.details.name;
	//	}
	//}

	//public class YummlyContent
	//{
	//	public List<string> preparationSteps { get; set; }
	//	public YummlyDetails details { get; set; }
	//	public List<YummlyIngredient> ingredientLines { get; set; }
	//}

	//public class YummlyAttribution
	//{
	//	public string url { get; set; }
	//}

	//public class YummlyDetails
	//{
	//	public string name { get; set; }
	//	public string directionsUrl { get; set; }
	//	public int? numberOfServings { get; set; }
	//	public float? rating { get; set; }
	//	public YummlyAttribution attribution { get; set; }
	//	public List<YummlyImage> images { get; set; }
	//}

	//public class YummlyIngredient
	//{
	//	public string wholeLine { get; set; }
	//	public string ingredient { get; set; }
	//	public float? quantity { get; set; }
	//	public string unit { get; set; }
	//	public string remainder { get; set; }
	//}

	//public class YummlyImage
	//{
	//	public string hostedLargeUrl { get; set; }
	//}
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
