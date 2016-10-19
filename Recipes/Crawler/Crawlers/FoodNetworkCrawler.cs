
//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using PaJaMa.Common;
//using System.IO;
//using PaJaMa.Recipes.Model.Entities;

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Food Network")]
//    public class FoodNetworkCrawler : CrawlerBase
//    {
//        private Dictionary<int, List<string>> _existingRecipes = new Dictionary<int, List<string>>();

//        protected override void crawl(int startPage)
//        {
//            string html = getHTML("http://www.foodnetwork.com/chefs/a-z.html");
//            MatchCollection mc = Regex.Matches(html, "<a href=\"(/chefs/.*?)\"");
//            var chefMcs = (from m in mc.OfType<Match>()
//                           select m.Groups[1].Value).Distinct().ToList();
//            foreach (var m in chefMcs)
//            {
//                System.Threading.Thread.Sleep(1000);

//                Console.WriteLine(m);
//                html = getHTML("http://foodnetwork.com" + m);

//                MatchCollection mcs = Regex.Matches(html, "<a href=\"(.*?)\".*?>Recipes<(.*?)</a>", RegexOptions.RightToLeft);

//                string defRecPage = m.Replace(".html", "/recipes.html");
//                bool defaultFound = false;
//                foreach (Match m2 in mcs)
//                {
//                    if (m2.Groups[1].Value.StartsWith("/recipes.html"))
//                        continue;

//                    string url = m2.Groups[1].Value;
//                    if (url.IndexOf("\" ") != -1)
//                        url = url.Substring(0, url.IndexOf("\" "));

//                    html = getHTML("http://foodnetwork.com" + url);
//                    parseHTML(html, m);

//                    if (url == defRecPage)
//                        defaultFound = true;
//                }

//                if (!defaultFound)
//                {
//                    try
//                    {
//                        html = getHTML("http://foodnetwork.com" + defRecPage);
//                        parseHTML(html, m);
//                    }
//                    catch
//                    {

//                    }

//                }
//            }
//        }

//        public void parseHTML(string html, string chefUrl)
//        {
//            WebClient wc = new WebClient();

//            Match mchef = Regex.Match(html, "<h2>(.*?)</h2>");
//            if (!mchef.Success)
//                return;

//            string chef = mchef.Groups[1].Value;

//            MatchCollection mc2 = Regex.Matches(html, "<a href=\"(.*?)\\.page-\\d*\\.html\">(\\d*)</a>");
//            var pageNums = from m2 in mc2.OfType<Match>()
//                           let pageNum = int.Parse(m2.Groups[2].Value)
//                           select new { PageNumber = pageNum, Prefix = m2.Groups[1].Value };
//            int maxPage = pageNums.Any() ? pageNums.Max(p => p.PageNumber) : 1;
//            string preFix = pageNums.Any() ? pageNums.First().Prefix : string.Empty;
//            for (int i = 1; i <= maxPage; i++)
//            {
//                Console.WriteLine(myAttribute.RecipeSourceName + " - Page " + i + " of " + maxPage);

//                string url = string.IsNullOrEmpty(preFix) ? chefUrl.Replace(".html", string.Format("/recipes.mostpopulare.page-{0}.html", i))
//                    : preFix + string.Format(".page-{0}.html", i);
//                try
//                {
//                    html = wc.DownloadString("http://foodnetwork.com" + url);
//                }
//                catch
//                {
//                    continue;
//                }

//                mc2 = Regex.Matches(html, "<li itemscope itemtype=\"http://schema.org/Recipe\">.*?" +
//                    "<h6><a href=\"(.*?)\" itemprop=\"url\"><span itemprop=\"name\">(.*?)</span></a>(.*?)</li>", RegexOptions.Singleline);
//                foreach (Match m2 in mc2)
//                {
//                    string recipeURL = "http://foodnetwork.com" + m2.Groups[1].Value;
//                    string recipeName = m2.Groups[2].Value;

//                    RecipeSource src = CrawlerHelper.GetRecipeSource(DbContext, chef);

//                    if (!_existingRecipes.ContainsKey(src.RecipeSourceID))
//                        _existingRecipes.Add(src.RecipeSourceID, src.Recipes.Select(r => r.RecipeURL).ToList());

//                    if (_existingRecipes[src.RecipeSourceID].Contains(recipeURL))
//                        continue;

//                    lock (CrawlerHelper.LockObject)
//                        CreateRecipe(recipeURL, recipeName, src.RecipeSourceID);

//                    _existingRecipes[src.RecipeSourceID].Add(recipeURL);

//                    System.Threading.Thread.Sleep(100);

//                }
//            }
//        }

//        protected override string baseURL
//        {
//            get { return "http://www.foodnetwork.com"; }
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
//            get { return "<h6>Directions</h6>(.*?)<hr>"; }
//        }

//        protected override string ratingRegexPattern
//        {
//            get { throw new NotImplementedException(); }
//        }

//        protected override string servingsRegexPattern
//        {
//            get { return "itemprop=\"recipeYield\">(.*?)<"; }
//        }

//        protected override string ingredientsRegexPattern
//        {
//            get { return "<li itemprop=\"ingredients\">(.*?)</li>"; }
//        }

//        protected override string imageRegexPattern
//        {
//            get { return "<img title=\".*?\" itemprop=\"image\" .*? src=\"(.*?)\""; }
//        }

//        protected override float? getRating(string html)
//        {
//            MatchCollection mc = Regex.Matches(html, "\"ss-star\"");
//            return mc.Count;
//        }

//        /*
//        public static void Crawl(DataFactory df)
//        {
//            WebClient wc = new WebClient();
//            string html = wc.DownloadString("http://www.foodnetwork.com/chefs/a-z.html");
//            MatchCollection mc = Regex.Matches(html, "<a href=\"(/chefs/.*?)\"");
//            var chefMcs = (from m in mc.OfType<Match>()
//                           select m.Groups[1].Value).Distinct().ToList();
//            foreach (var m in chefMcs)
//            {
//                System.Threading.Thread.Sleep(1000);

//                Console.WriteLine(m);
//                int tries = 3;
//                while (tries > 0)
//                {
//                    try
//                    {
//                        html = wc.DownloadString("http://foodnetwork.com" + m);
//                        break;
//                    }
//                    catch
//                    {
//                        System.Threading.Thread.Sleep(1000);
//                        tries--;
//                    }
//                }

//                MatchCollection mcs = Regex.Matches(html, "<a href=\"(.*?)\".*?>Recipes<(.*?)</a>", RegexOptions.RightToLeft);

//                string defRecPage = m.Replace(".html", "/recipes.html");
//                bool defaultFound = false;
//                foreach (Match m2 in mcs)
//                {
//                    if (m2.Groups[1].Value.StartsWith("/recipes.html"))
//                        continue;

//                    string url = m2.Groups[1].Value;
//                    if (url.IndexOf("\" ") != -1)
//                        url = url.Substring(0, url.IndexOf("\" "));

//                    html = wc.DownloadString("http://foodnetwork.com" + url);
//                    parseHTML(df, html, m);

//                    if (url == defRecPage)
//                        defaultFound = true;
//                }

//                if (!defaultFound)
//                {
//                    try
//                    {
//                        html = wc.DownloadString("http://foodnetwork.com" + defRecPage);
//                        parseHTML(df, html, m);
//                    }
//                    catch
//                    {

//                    }
					
//                }
//            }
//        }

//        public static void parseHTML(DataFactory df, string html, string chefUrl)
//        {
//            WebClient wc = new WebClient();
//            //try
//            //{
//            //	html = wc.DownloadString("http://foodnetwork.com" + m.Replace(".html", "/recipes.html"));
//            //}
//            //catch
//            //{
//            //	return;
//            //}

//            Match mchef = Regex.Match(html, "<h2>(.*?)</h2>");
//            if (!mchef.Success)
//                return;
			
//            string chef = mchef.Groups[1].Value;

//            // TODO: temporarily
////			string sql = @"
////select convert(bit, case when exists (select 1 from Recipes..Recipe
////where Source = @Source
////) then 1 else 0 end)";
////			ParameterizedQuery qry = new ParameterizedQuery(sql);
////			qry.Parameters.Add(df.GetParameter("@Source", chef));
////			bool exists = (bool)df.ExecuteScalar(qry);
////			if (exists)
////				return;

//            MatchCollection mc2 = Regex.Matches(html, "<a href=\"(.*?)\\.page-\\d*\\.html\">(\\d*)</a>");
//            var pageNums = from m2 in mc2.OfType<Match>()
//                           let pageNum = int.Parse(m2.Groups[2].Value)
//                           select new { PageNumber = pageNum, Prefix = m2.Groups[1].Value };
//            int maxPage = pageNums.Any() ? pageNums.Max(p => p.PageNumber) : 1;
//            string preFix = pageNums.Any() ? pageNums.First().Prefix : string.Empty;
//            for (int i = 1; i <= maxPage; i++)
//            {
//                string url = string.IsNullOrEmpty(preFix) ? chefUrl.Replace(".html", string.Format("/recipes.mostpopulare.page-{0}.html", i))
//                    : preFix + string.Format(".page-{0}.html", i);
//                html = wc.DownloadString("http://foodnetwork.com" + url);

//                mc2 = Regex.Matches(html, "<li itemscope itemtype=\"http://schema.org/Recipe\">.*?" +
//                    "<h6><a href=\"(.*?)\" itemprop=\"url\"><span itemprop=\"name\">(.*?)</span></a>(.*?)</li>", RegexOptions.Singleline);
//                foreach (Match m2 in mc2)
//                {
//                    string recipeName = m2.Groups[2].Value;
//                    string src = chef;

//                    var recSrc = CrawlerHelper.GetRecipeSource(df, src);

//                    // TEMP //
//                    Recipe existingRec = CrawlerHelper.GetRecipe(df, recipeName, src);
//                    if (existingRec != null)
//                    {
//                        Console.WriteLine(existingRec.RecipeSourceName + " - " + existingRec.RecipeName);
//                        if (existingRec.RecipeURL == null)
//                        {
//                            existingRec.RecipeURL = "http://foodnetwork.com" + m2.Groups[1].Value;
//                            existingRec.Save();
//                        }
//                        continue;
//                    }
//                    // END TEMP //
						
//                    if (CrawlerHelper.RecipeExists(df, recipeName, recSrc.RecipeSourceID))
//                        continue;
					
//                    int tries = 3;
//                    while (tries > 0)
//                    {
//                        try
//                        {
//                            html = wc.DownloadString("http://foodnetwork.com" + m2.Groups[1].Value);
//                            break;
//                        }
//                        catch
//                        {
//                            System.Threading.Thread.Sleep(1000);
//                            tries--;
//                        }
//                    }

//                    Match m3 = Regex.Match(html, "<img title=\"" + m2.Groups[2].Value.Replace("(", "\\(").Replace(")", "\\)") + "\" itemprop=\"image\" .*? src=\"(.*?)\"", RegexOptions.Singleline);
//                    //if (!m3.Success) throw new NotImplementedException(m.Groups[1].Value + " " + i.ToString());

//                    Recipe rec = df.CreateDataItem<Recipe>();
//                    rec.RecipeName = recipeName;
//                    rec.RecipeSource = recSrc;
//                    rec.RecipeURL = "http://foodnetwork.com" + m2.Groups[1].Value;
//                    Console.WriteLine(chef + " Page: " + i.ToString() + " of " + maxPage.ToString() + " Recipe: " + rec.RecipeName);
//                    if (m3.Success)
//                        CrawlerHelper.AddImage(rec, m3.Groups[1].Value);

//                    MatchCollection mc3 = Regex.Matches(m2.Groups[3].Value, "\"ss-star\"");
//                    rec.Rating = mc3.Count;

//                    m3 = Regex.Match(html, "<h6>Directions</h6>(.*?)<hr>", RegexOptions.Singleline);
//                    rec.Directions = PaJaMa.Common.Common.StripHTML(m3.Groups[1].Value.Trim());

//                    mc3 = Regex.Matches(html, "<li itemprop=\"ingredients\">(.*?)</li>");

//                    List<RecipeIngredientMeasurement> recIngs = new List<RecipeIngredientMeasurement>();
//                    foreach (Match m4 in mc3)
//                    {
//                        string[] parts = Common.StripHTML(m4.Groups[1].Value).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
//                        double qty = 0;
//                        double tempQty = 0;
//                        int index = 0;
//                        while (Common.TryParseFraction(parts[index], out tempQty))
//                        {
//                            qty += tempQty;
//                            index++;
//                        }

//                        MeasurementFilter mf = df.CreateDataFilter<MeasurementFilter>();
//                        mf.MeasurementName = parts[index];
//                        Measurement measurement = mf.GetDataItems().FirstOrDefault();
//                        if (measurement == null)
//                        {

//                        }
//                        else
//                            index++;

//                        string ingredient = string.Empty;

//                        for (int i2 = index; i2 < parts.Length; i2++)
//                        {
//                            ingredient += parts[i2] + " ";
//                        }

//                        ingredient = ingredient.Trim();

//                        if (ingredient.Length > 255)
//                            continue;

//                        rec.RecipeRecipeIngredientMeasurements.Add(CrawlerHelper.GetIngredient(df, ingredient, measurement, qty));
//                    }

//                    CrawlerHelper.SaveRecipe(rec);

//                    //if (!string.IsNullOrEmpty(rec.ImageURL))
//                    //{
//                    //	string imgPath = @"\\pjserver\e\HTTP\RecipesAPI\Images\Recipes";
//                    //	string extension = Path.GetExtension(rec.ImageURL);
//                    //	extension = extension.Replace("jpeg", "jpg");
//                    //	string path = Path.Combine(imgPath, rec.RecipeID.ToString() + extension);
//                    //	if (!File.Exists(path))
//                    //		wc.DownloadFile(rec.ImageURL, path);
//                    //}

//                    System.Threading.Thread.Sleep(100);

//                }
//            }
//        }
//         * 
//         * */

//    }
//}