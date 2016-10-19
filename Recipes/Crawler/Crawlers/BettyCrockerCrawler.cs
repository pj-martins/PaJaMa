//using PaJaMa.Common;
//using PaJaMa.Recipes.Model;
//using PaJaMa.Recipes.Model.Entities;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Web;
//using System.Xml;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Betty Crocker", UniqueRecipeName = true)]
//    public class BettyCrockerCrawler : CrawlerBase
//    {
//        protected override void crawl(int startPage)
//        {
//            string html = getHTML("http://www.bettycrocker.com/recipes/main-ingredient");

//            MatchCollection mc = Regex.Matches(html, "<a href=\"(/recipes/main-ingredient/.*?)\">(.*?)<a", RegexOptions.Singleline);

//            List<string> keywords = new List<string>();
//            foreach (Match m in mc)
//            {
//                crawl(keywords, Common.StripHTML(m.Groups[2].Value));
//                html = getHTML("http://www.bettycrocker.com" + m.Groups[1].Value);
//                MatchCollection mc2 = Regex.Matches(html, "href=\"" + m.Groups[1].Value + "/.*?\">(.*?)<");
//                foreach (Match m2 in mc2)
//                {
//                    if (string.IsNullOrEmpty(m2.Groups[1].Value))
//                        continue;
//                    crawl(keywords, m2.Groups[1].Value);
//                }
//            }
//        }

//        private void crawl(List<string> keywords, string searchString)
//        {
//            string[] parts = searchString.Split(' ');
//            foreach (string part in parts)
//            {
//                if (keywords.Contains(part) || string.IsNullOrEmpty(part.Trim()))
//                    continue;

//                System.IO.File.AppendAllText(@"\\pjserver\e\temp\keywords.txt", part.Trim() + "\r\n");
//                string url = "http://services.bettycrocker.com/v2/search/recipes/true/{0}/200/" + part.Trim().ToLower() + ".xml";
//                int searchResults = parseSearchResults(url, 1);
//                int pages = (int)Math.Ceiling((decimal)searchResults / 200);
//                for (int i = 2; i <= pages; i++)
//                {
//                    Console.WriteLine(myAttribute.RecipeSourceName + " - Page " + i + " of " + pages);
//                    parseSearchResults(url, i);
//                }

//                keywords.Add(part);
//                if (part.EndsWith("s"))
//                    crawl(keywords, part.Substring(0, part.Length - 1));
//            }
//        }

//        private int parseSearchResults(string url, int pageNum)
//        {
//            string xml = string.Empty;
//            try
//            {
//                xml = getHTML(string.Format(url, pageNum));
//            }
//            catch
//            {
//                return 0;
//            }

//            if (string.IsNullOrEmpty(xml)) return 0;

//            SearchServiceResponse search = PaJaMa.Common.XmlSerialize.DeserializeObject<SearchServiceResponse>(xml);
//            if (search == null) return 0;
//            for (int j = 0; j < search.searchResults.recipeList.Length; j++)
//            {
//                var recSummary = search.searchResults.recipeList[j];
//                string display = "BettyCroker - Page " + pageNum + " of " + ((int)Math.Ceiling((decimal)search.searchResults.numberOfResults / 200)).ToString() + " - " +
//                        (j + 1).ToString() + " of " + search.searchResults.recipeList.Length.ToString() + " - " + recSummary.title;
//                if (existingRecipes.Contains(recSummary.title))
//                    continue;

//                Console.WriteLine("* " + display);

//                lock (CrawlerHelper.LockObject)
//                {
//                    xml = getHTML("http://services.bettycrocker.com/v2/recipes/" + recSummary.id.ToString().ToLower() + ".xml");
//                    RecipeServiceResponse response = PaJaMa.Common.XmlSerialize.DeserializeObject<RecipeServiceResponse>(xml);

//                    Recipe rec = new Recipe();
//                    rec.RecipeSourceID = recipeSource.RecipeSourceID;
//                    rec.RecipeName = response.recipe.title;
//                    rec.RecipeURL = recSummary.bettyCrockerWebsiteURL;

//                    rec.Directions = string.Join("\r\n", response.recipe.methods.OrderBy(m => m.stepNumber).Select(m => m.description).ToArray());
//                    var rat = response.recipe.ratings.FirstOrDefault(r => r.averageRating != 0);
//                    if (rat != null)
//                        rec.Rating = rat.averageRating;

//                    if (!string.IsNullOrEmpty(response.recipe.servings))
//                    {
//                        int index = 0;
//                        int tempInt = -1;
//                        while (index < response.recipe.servings.Length && int.TryParse(response.recipe.servings[index].ToString(), out tempInt))
//                        {
//                            index++;
//                        }

//                        if (index > 0)
//                            rec.NumberOfServings = int.Parse(response.recipe.servings.Substring(0, index));
//                    }

//                    foreach (var ing in response.recipe.ingredients)
//                    {
//                        double qty = 0;
//                        if (!string.IsNullOrEmpty(ing.quantity) && !Common.TryParseFraction(ing.quantity, out qty))
//                            throw new NotImplementedException();

//                        string ingredient = string.Empty;
//                        string measurementName = string.Empty;

//                        string[] parts = ing.name.Split(' ');
//                        if (parts.Length > 1)
//                        {
//                            bool firstIn = true;
//                            for (int i = 0; i < parts.Length; i++)
//                            {
//                                if (firstIn)
//                                    measurementName += parts[i].Trim() + " ";
//                                else
//                                    ingredient = ingredient + " " + parts[i].Trim();
//                                firstIn = false;
//                            }
//                        }
//                        else
//                            ingredient = ing.name;

//                        ingredient = ingredient.Trim();
//                        measurementName = measurementName.Trim();

//                        Measurement measurement = null;
//                        if (!string.IsNullOrEmpty(measurementName))
//                        {
//                            measurement = CrawlerHelper.GetMeasurement(DbContext, measurementName.ToLower(), false);
//                            if (measurement == null)
//                            {
//                                ingredient = measurementName + " " + ingredient;
//                                measurementName = string.Empty;
//                            }
//                        }

//                        if (!string.IsNullOrEmpty(ingredient))
//                            rec.RecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(DbContext, ingredient, measurement, qty));
//                    }

//                    foreach (var p in response.recipe.photos)
//                    {
//                        RecipeImage img = rec.RecipeImages.FirstOrDefault();
//                        if (img == null)
//                        {
//                            img = new RecipeImage();
//                            rec.RecipeImages.Add(img);
//                        }
//                        img.ImageURL = p.Url;
//                        img.LocalImagePath = null;
//                        rec.RecipeImages.Add(img);
//                    }

//                    DbContext.Recipes.Add(rec);
//                    DbContext.SaveChanges();

//                    existingRecipes.Add(rec.RecipeName);
//                }
//            }

//            return search.searchResults.numberOfResults;
//        }

//        protected override string baseURL
//        {
//            get { throw new NotImplementedException(); }
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
//            get { throw new NotImplementedException(); }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string imageRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }
//    }

//    [System.Xml.Serialization.XmlRoot("serviceResponse")]
//    public class SearchServiceResponse { public SearchResults searchResults { get; set; } }

//    public class SearchResults
//    {
//        public int numberOfResults { get; set; }
//        public recipeSummary[] recipeList { get; set; }
//    }


//    public class recipeSummary
//    {
//        public Guid id { get; set; }
//        public string title { get; set; }
//        public string bettyCrockerWebsiteURL { get; set; }
//    }

//    [System.Xml.Serialization.XmlRoot("serviceResponse")]
//    public class RecipeServiceResponse { public BettyRecipe recipe { get; set; } }

//    public class BettyRecipe
//    {
//        public Guid id { get; set; }
//        public string title { get; set; }
//        public string servings { get; set; }
//        public List<rating> ratings { get; set; }
//        public List<ingredient> ingredients { get; set; }
//        public List<method> methods { get; set; }
//        public List<photo> photos { get; set; }
//    }

//    public class rating
//    {
//        public float averageRating { get; set; }
//    }

//    public class ingredient
//    {
//        public string name { get; set; }
//        public string quantity { get; set; }
//    }

//    public class method
//    {
//        public string description { get; set; }
//        public int stepNumber { get; set; }
//    }

//    public class photo
//    {
//        public string Url { get; set; }
//    }
//}
