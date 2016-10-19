using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Crawler.Crawlers
{
    [RecipeSource("Food.com")]
    public class FoodCrawler : CrawlerBase
    {
        protected override float? getRating(HtmlAgilityPack.HtmlNode node)
        {
            var ratingNode = node.SelectSingleNode(rating2XPath);
            if (ratingNode != null)
            {
                var attr = ratingNode.Attributes["content"];
                return Convert.ToSingle(attr.Value);
            }
            return null;
        }

        protected override void crawl(int startPage)
        {
            startPage = 4340;
            int totalPages = 1000000;
            for (int i = startPage; i < totalPages; i++)
            {
				Console.WriteLine("FoodCrawler: Page " + i.ToString() + " of " + totalPages.ToString());

				string json = getHTML("http://www.food.com/services/mobile/fdc/search/all" + (i == 0 ? string.Empty : "?pn=" + i));
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<FoodJSON>(json);
                if (totalPages == 1000000)
                    totalPages = (int)Math.Ceiling(obj.response.totalResultsCount / 10);
                foreach (var rec in obj.response.results)
                {
                    if (rec.record_type == "Recipe")
                    {
						if (existingRecipes.Contains(myAttribute.UniqueRecipeName ? rec.main_title : rec.record_url))
							continue;

						Console.WriteLine("* FoodCrawler: Page " + i.ToString() + " of " + totalPages.ToString() + " - " + rec.main_title);

						lock (CrawlerHelper.LockObject)
                            CreateRecipe(rec.record_url, rec.main_title, recipeSource.RecipeSourceID);

                        existingRecipes.Add(myAttribute.UniqueRecipeName ? rec.main_title : rec.record_url);
                    }
                }
            }
        }

        protected override string baseURL
        {
            get { return "http://food.com"; }
        }

        protected override string allURL
        {
            get { throw new NotImplementedException(); }
        }

        protected override string recipesXPath
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class FoodJSON
    {
        public foodResponse response { get; set; }
    }

    public class foodResponse
    {
        public float totalResultsCount { get; set; }
        public foodRecipe[] results { get; set; }
    }

    public class foodRecipe
    {
        public string record_type { get; set; }
        public string main_title { get; set; }
        public string recipe_photo_url { get; set; }
        public string record_url { get; set; }
        public float main_rating { get; set; }
    }
}
