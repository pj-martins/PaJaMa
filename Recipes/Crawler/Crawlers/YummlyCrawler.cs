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

//namespace Crawler.Crawlers
//{
//    [RecipeSource("Yummly", StartPage = 0)]
//    public class YummlyCrawler : CrawlerBase
//    {
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
