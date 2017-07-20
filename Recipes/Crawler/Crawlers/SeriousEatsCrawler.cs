using PaJaMa.Common;
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
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace Crawler.Crawlers
{
	[RecipeSource("Serious Eats")]
	public class SeriousEatsCrawler : CrawlerBase
	{
		protected override string baseURL
		{
			get { return "http://www.seriouseats.com"; }
		}

		protected override void crawl()
		{
			var eats = JsonConvert.DeserializeObject<SeriousEats>(System.IO.File.ReadAllText("eats.json"));
			// var eats = JsonConvert.DeserializeObject<SeriousEats>(getHTML(baseURL + "/topics/search?index=recipe&count=1"));
			//var html = getHTML(baseURL + "/topics/search?index=recipe&count=" + eats.total_entries.ToString());
			//System.IO.File.WriteAllText("seriouseats.json", html);
			//eats = JsonConvert.DeserializeObject<SeriousEats>(html);
			int i = 1;
			foreach (var eat in eats.entries)
			{
				string display = $"{i++} of {eats.entries.Count} - Serious Eats - {eat.title}";

				Console.WriteLine(display);

				if (existingRecipes.Contains(eat.permalink)) continue;

				if (eat.ingredients.Count < 1) continue;

				Recipe rec = new Recipe();
				rec.RecipeSourceID = recipeSource.RecipeSourceID;
				rec.RecipeName = eat.title;
				rec.RecipeURL = eat.permalink;

				foreach (var proc in eat.procedures)
				{
					if (proc.text != null)
						rec.Directions += Common.StripHTML(proc.text) + "\r\n";
				}

				if (rec.Directions != null)
					rec.Directions = rec.Directions.Trim();

				if (eat.average_rating != null)
					rec.Rating = Regex.Matches(eat.average_rating, "se\\-rating\\-on\\.png").Count;

				if (eat.number_serves != null)
				{
					int tempInt = -1;
					if (!int.TryParse(eat.number_serves, out tempInt))
					{
						Match servingsMatch2 = Regex.Match(eat.number_serves, "(\\d+)");
						if (!int.TryParse(servingsMatch2.Groups[1].Value, out tempInt))
						{

						}
					}
					if (tempInt > 0)
						rec.NumberOfServings = tempInt;
				}

				foreach (var ingredient in eat.ingredients)
				{
					var ing = CrawlerHelper.GetIngredientQuantity(dbContext, ingredient, false, true);
					if (ing == null || string.IsNullOrEmpty(ing.Item1))
						continue;
					rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(dbContext, ing.Item1, ing.Item2, ing.Item3));
				}

				if (eat.thumbnail_750 != null)
				{
					RecipeImage img = new RecipeImage();
					img.ImageURL = eat.thumbnail_750;
					img.LocalImagePath = null;
					rec.RecipeImages.Add(img);
				}

				dbContext.Recipes.Add(rec);
				dbContext.SaveChanges();

				existingRecipes.Add(eat.permalink);
			}
		}

		public class SeriousEats
		{
			public int total_entries { get; set; }
			public int listed_entries { get; set; }
			public List<SeriousEatsEntry> entries { get; set; }
		}

		public class SeriousEatsEntry
		{
			public int id { get; set; }
			public string number_serves { get; set; }
			public string permalink { get; set; }
			public List<string> ingredients { get; set; }
			public List<SeriousEatsProcedure> procedures { get; set; }
			public string average_rating { get; set; }
			public string title { get; set; }
			public string thumbnail_750 { get; set; }

			public override string ToString()
			{
				return id.ToString() + " - " + title + " - " + permalink;
			}
		}

		public class SeriousEatsProcedure
		{
			public string text { get; set; }
			public string thumbnail { get; set; }
			public string image { get; set; }
		}
		#region OLD
		//        protected override void crawl(int startPage)
		//        {
		//            SeriousEats eats = null;

		//            string html = new WebClient().DownloadString("http://www.seriouseats.com/recipes");
		//            Match m = Regex.Match(html, "id=\"browse-categories\">(.*?)id=\"browse-columns\">(.*?)</div>", RegexOptions.Singleline);
		//            List<string> keywords = new List<string>();
		//            MatchCollection mc2 = Regex.Matches(m.Groups[1].Value, "href=\".*/(.*?)\"");
		//            foreach (Match m2 in mc2)
		//            {
		//                keywords.Add(m2.Groups[1].Value.Replace("_", ""));
		//            }

		//            mc2 = Regex.Matches(m.Groups[2].Value, "href=\".*/(.*?)/\"");
		//            foreach (Match m2 in mc2)
		//            {
		//                keywords.Add(m2.Groups[1].Value.Replace("_", ""));
		//            }

		//            foreach (string keyword in keywords)
		//            {
		//                int pageSize = 100;
		//                int current = 1;

		//                string url = "http://www.seriouseats.com/topics/search?index=recipe&count=1&term=c|" + keyword;

		//                int tries = 3;
		//                while (tries > 0)
		//                {
		//                    try
		//                    {
		//                        eats = Newtonsoft.Json.JsonConvert.DeserializeObject<SeriousEats>(new WebClient().DownloadString(url));
		//                        break;
		//                    }
		//                    catch
		//                    {
		//                        System.Threading.Thread.Sleep(1000);
		//                        tries--;
		//                    }
		//                }

		//                if (tries <= 0)
		//                    throw new NotImplementedException();

		//                pageSize = eats.total_entries;

		//                url = "http://www.seriouseats.com/topics/search?index=recipe&count=" + pageSize.ToString() + "&term=c|" + keyword;

		//                tries = 3;
		//                while (tries > 0)
		//                {
		//                    try
		//                    {
		//                        eats = Newtonsoft.Json.JsonConvert.DeserializeObject<SeriousEats>(new WebClient().DownloadString(url));
		//                        break;
		//                    }
		//                    catch
		//                    {
		//                        System.Threading.Thread.Sleep(1000);
		//                        tries--;
		//                    }
		//                }

		//                if (tries <= 0)
		//                    throw new NotImplementedException();

		//                foreach (SeriousEatsEntry entry in eats.entries)
		//                {
		//                    string display = keyword + " - " + (current++).ToString() + " of " + eats.total_entries + " - " + entry.title;
		//                    if (existingRecipes.Contains(entry.permalink))
		//                        continue;

		//                    Console.WriteLine("* " + display);

		//                    lock (CrawlerHelper.LockObject)
		//                    {
		//                        var rec = new Recipe();
		//                        rec.RecipeSourceID = recipeSource.RecipeSourceID;
		//                        rec.RecipeURL = entry.permalink;
		//                        rec.RecipeName = entry.title;
		//                        rec.Directions = string.Join("\r\n", entry.procedures.Select(p => Common.StripHTML(p.text).Trim()).ToArray());
		//                        if (!string.IsNullOrEmpty(entry.average_rating))
		//                        {
		//                            MatchCollection mc = Regex.Matches(entry.average_rating, "se-rating-on.png");
		//                            rec.Rating = mc.Count;
		//                        }

		//                        if (!string.IsNullOrEmpty(entry.number_serves))
		//                        {
		//                            int tempInt = -1;
		//                            Match servingsMatch = Regex.Match(entry.number_serves, "(\\d+)");
		//                            if (!int.TryParse(servingsMatch.Groups[1].Value, out tempInt))
		//                            {

		//                            }
		//                            else
		//                                rec.NumberOfServings = tempInt;
		//                        }

		//                        foreach (string line in entry.ingredients)
		//                        {
		//                            string line2 = Common.StripHTML(line);
		//                            if (string.IsNullOrEmpty(line2))
		//                                continue;
		//                            var ingrLine = CrawlerHelper.GetIngredientQuantity(DbContext, line2, false, true);
		//                            rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(DbContext, ingrLine.Item1, ingrLine.Item2, ingrLine.Item3));
		//                        }

		//                        if (!string.IsNullOrEmpty(entry.thumbnail_625))
		//                        {
		//                            RecipeImage img = new RecipeImage();
		//                            img.ImageURL = entry.thumbnail_625;
		//                            img.LocalImagePath = null;
		//                            rec.RecipeImages.Add(img);
		//                        }

		//                        DbContext.Recipes.Add(rec);
		//                        DbContext.SaveChanges();
		//                        existingRecipes.Add(entry.permalink);
		//                    }
		//                }
		//            }
		//        }

		//        protected override string baseURL
		//        {
		//            get { return "http://www.seriouseats.com"; }
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
		//            get { return "<div class=\"procedure-text\">(.*?)</div>"; }
		//        }

		//        protected override string ratingRegexPattern
		//        {
		//            get { return "_NONE_"; }
		//        }

		//        protected override string ingredientsRegexPattern
		//        {
		//            get { return "<span class=\"ingredient\">(.*?)</span>"; }
		//        }

		//        protected override string imageRegexPattern
		//        {
		//            get { return "<section role=\"main\".*?<img src=\"(.*?)\" class=\"photo\""; }
		//        }

		//        protected override string servingsRegexPattern
		//        {
		//            get { return "<span class=\"info yield\">(.*?)</span>"; }
		//        }

		//        protected override float? getRating(string html)
		//        {
		//            Match m = Regex.Match(html, "<tr><td class=\"rater\">(.*?)</tr>");
		//            if (m.Success)
		//            {
		//                MatchCollection mc = Regex.Matches(m.Groups[1].Value, "http://www.seriouseats.com/imagesV2/se-rating-on.png");
		//                return mc.Count;
		//            }
		//            return null;
		//        }
		//    }
		#endregion

	}
}
