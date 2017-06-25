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
using HtmlAgilityPack;

namespace Crawler.Crawlers
{
    [RecipeSource("Food.com")]
    public class FoodCrawler : CrawlerBase
    {
        private bool _hasMore;

        protected override string baseURL => "http://www.food.com/topics";
        protected override string imagesXPath => "//img[@class='recipe-main-img']";
        protected override string directionsXPath => "//div[@data-module='recipeDirections']//li";
        protected override string servingsXPath => "//li[@id='yield-servings']//span";
        protected override string ingredientsXPath => "//li[@data-ingredient]";

        protected override PageNumbers pageNumberURLRegex => new PageNumbers() { URLFormat = "/?pn={0}" };

        protected override List<string> getKeywordPages(HtmlDocument document)
        {
            List<HtmlNode> childNodes = new List<HtmlNode>();
            childNodes.Add(document.DocumentNode);
            var nodes = document.DocumentNode.SelectNodes("//section[@class='letter-index']//a");
            foreach (var node in nodes)
            {
                string html = getHTML(node.Attributes["href"].Value);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                childNodes.Add(doc.DocumentNode);
            }

            List<string> keywordPages = new List<string>();
            foreach (var childNode in childNodes)
            {
                var keynodes = childNode.SelectNodes("//section[@class='topic-index-items']//a");
                if (keynodes == null) continue;
                keywordPages.AddRange(keynodes.Select(n => n.Attributes["href"].Value));

            }


            // TEMP TEMP TEMP
            keywordPages = keywordPages.OrderBy(kp => kp.ToLower().Contains("indian") ? 0 : 1).ToList();

            return keywordPages;
        }

        protected override void updateMaxPage(HtmlDocument doc, ref int maxPage)
        {
            base.updateMaxPage(doc, ref maxPage);
            if (_hasMore)
                maxPage++;
        }

        protected override Dictionary<string, string> getRecipeURLs(HtmlDocument doc)
        {
            var match = Regex.Match(doc.DocumentNode.InnerHtml, "var searchResults = (.*);");
            var searchResults = Newtonsoft.Json.JsonConvert.DeserializeObject<Api>(match.Groups[1].Value);
            _hasMore = searchResults.HasMore;

            var urls = new Dictionary<string, string>();
            foreach (var result in searchResults.Response.Results)
            {
                if (result.record_type != "Recipe") continue;
                if (result.main_rating > 0 && result.main_rating < 4) continue;
                urls.Add(result.record_url, result.main_title);
            }
            return urls;
        }



        public class Api
        {
            public int Version { get; set; }
            public bool HasMore { get; set; }
            public ApiResponse Response { get; set; }
        }

        public class ApiResponse
        {
            public int TotalResultsCount { get; set; }
            public List<ApiRecord> Results { get; set; }
        }

        public class ApiRecord
        {
            public string record_type { get; set; }
            public string record_url { get; set; }
            public string main_title { get; set; }
            public float main_rating { get; set; }
        }

        #region OLD2
        /*
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
        */
        #endregion

    }
}