using HtmlAgilityPack;
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
using System.Xml;
using System.Xml.Serialization;

namespace Crawler.Crawlers
{
    [RecipeSource("Big Oven")]
    public class BigOvenCrawler : CrawlerBase
    {
        protected override string baseURL
        {
            get { return "http://www.bigoven.com"; }
        }

        protected override string allURL
        {
            get { return "/glossary"; }
        }

        protected override string recipesXPath
        {
            get { return "//*[@class='recipe-tile-full']//a"; }
        }

        protected override PageNumbers pageNumberURLRegex
        {
            get
            {
                return new PageNumbers()
                {
                    MaxPageRegexPattern = "href=\"/recipes/.*?/best/page/(\\d*)\"",
                    URLFormat = "/page/{0}"
                };
            }
        }

        protected override Tuple<string, string> getRecipeURL(HtmlNode node)
        {
            if (node != null && node.Attributes["href"] != null && node.Attributes["href"].Value.StartsWith("http://www.bigoven.com/recipe")
                        && !string.IsNullOrEmpty(node.InnerText))
            {
                return new Tuple<string, string>(node.Attributes["href"].Value, node.InnerText.Trim());
            }
            return null;
        }

        protected override List<string> getKeywordPages(HtmlDocument doc)
        {
            //MatchCollection mc = Regex.Matches(html, "<a href=\"/article/.*?\">(.*?)</a>");
            //return (from m in mc.OfType<Match>()
            //        select (baseURL + "/recipes/" + m.Groups[1].Value.ToLower().Replace(" ", "-") + "/best")
            //        ).ToList();
            throw new NotImplementedException();
        }

        protected override string getImageURL(HtmlNode node)
        {
            string matchedURL = base.getImageURL(node);

            if (matchedURL == "http://mda.bigoven.com/pics/recipe-no-image.jpg")
                return string.Empty;

            if (matchedURL == "http://mda.bigoven.com/pics/rs/256/recipe-no-image.jpg")
                return string.Empty;

            return matchedURL;
        }

        /*
        protected override void crawl(PaJaMaFramework.DataFactory df, int startPage = 1)
        {
            BigOvenSearchResults searchResults = null;
            int srcID = CrawlerHelper.GetRecipeSource(df, "Big Oven").RecipeSourceID;

            do
            {
                Console.WriteLine("Big Oven - Page " + startPage);
				
                string url = string.Format("http://api.bigoven.com/recipes?api_key=dvxHPB3Gxit7cJpS3LJ164GEPPJYSSS7&pg={0}&rpp=50", startPage);
                webClient.Headers.Add("Content-Type", "application/json");
                string json = webClient.DownloadString(url);
                searchResults = Newtonsoft.Json.JsonConvert.DeserializeObject<BigOvenSearchResults>(json);
                foreach (var result in searchResults.Results)
                {
					
                    if (existing.Contains(result.WebURL))
                        continue;

                    string display = "Big Oven - " + startPage.ToString() + " - " + result.Title;
                    Console.WriteLine("* " + display);

                    var rec = df.CreateDataItem<Recipe>();
                    rec.RecipeName = result.Title;
                    rec.Rating = result.StarRating;
                    rec.NumberOfServings = (int)result.YieldNumber;
                    rec.RecipeURL = result.WebURL.Replace("\t", "");
                    rec.RecipeSourceID = srcID;

                    var recImg = df.CreateDataItem<RecipeImage>();
                    recImg.ImageURL = result.ImageURL;
                    rec.RecipeRecipeImages.Add(recImg);

                    string html = getHTML(rec.RecipeURL);
                    rec.Directions = getDirections(html);
                    var ingrs = getIngredients(df, html);
                    if (ingrs == null) continue;
                    rec.RecipeRecipeIngredientMeasurements.AddRange(ingrs);
                    CrawlerHelper.SaveRecipe(rec);
                }
                startPage++;
            }
            while (searchResults.Results.Any());
        }
         */
    }

    public class BigOvenSearchResults
    {
        public int ResultCount { get; set; }
        public List<BigOvenRecipeInfo> Results { get; set; }
    }

    public class BigOvenRecipeInfo
    {
        public int RecipeID { get; set; }
        public string Title { get; set; }
        public float StarRating { get; set; }
        public string WebURL { get; set; }
        public string ImageURL { get; set; }
        public float YieldNumber { get; set; }
    }
}
