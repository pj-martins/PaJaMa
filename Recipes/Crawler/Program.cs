using PaJaMa.Common;

using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crawler.Crawlers;
using System.Threading;
using System.Data;
using PaJaMa.Recipes.Model.Entities;

namespace Crawler
{
	class Program
	{
		static void Main(string[] args)
		{
			//List<Task> runningTasks = new List<Task>();

			//Queue<CrawlerBase> crawlers = new Queue<CrawlerBase>();

			//var crawlerTypes = from t in typeof(CrawlerHelper).Assembly.GetTypes()
			//                   where !t.IsAbstract && isCrawlerBase(t)
			//                   let rsa = t.GetCustomAttributes(typeof(RecipeSourceAttribute), true).FirstOrDefault() as RecipeSourceAttribute
			//                   where rsa != null && !rsa.IgnoreAuto
			//                   orderby t.Name
			//                   select t;

			//foreach (var t in crawlerTypes)
			//{
			//    // DEBUG
			//    if (true
			//                    //&& !t.Name.Contains("FoodNetworkCrawler")
			//                    //&& !t.Name.Contains("BigOven")
			//                    //  && !t.Name.Contains("AllRecipesCrawler")
			//                    //&& !t.Name.Contains("MyRecipes")
			//                    && !t.Name.Contains("EpicuriousCrawler")
			//        //&& !t.Name.Contains("BodyBuilding")

			//        ) continue;

			//    //crawlers.Enqueue(Activator.CreateInstance(t) as CrawlerBase);
			//}

#if Thread1
			// new ChowCrawler().Crawl(getDbContext());
			// new FoodNetworkCrawler().Crawl(getDbContext());
			new TasteOfHomeCrawler().Crawl();
#endif
#if Thread2
			// new TheKitchnCrawler().Crawl(getDbContext());
			//Cleanup.YummlyToOriginal("http://www.foodnetwork.com/", "Food Network", new FoodNetworkCrawler());
			// Cleanup.YummlyToOriginal("http://www.epicurious.com/", "Epicurious", new EpicuriousCrawler());
			//Cleanup.YummlyToOriginal("http://www.seriouseats.com/", "Serious Eats", new SeriousEatsCrawler());
			// Cleanup.YummlyToOriginal("http://www.food.com/", "Food.com", new FoodCrawler());
			// Cleanup.YummlyToOriginal("http://www.tasteofhome.com/", "Taste Of Home", new TasteOfHomeCrawler());
			// Cleanup.Keywords();
			// new AllRecipesCrawler().Crawl();
			// new CooksCrawler().Crawl();
			new AllRecipesCrawler().Crawl();

#endif
#if Thread3
			new EpicuriousCrawlerApi().Crawl();
			// new SeriousEatsCrawler().Crawl(getDbContext());
			// new YummlyCrawler().Crawl();
#endif
#if Thread4
			new FoodCrawlerApi().Crawl();

#endif

			//while (true)
			//{
			//    while (runningTasks.Count < 5 && crawlers.Any())
			//    {
			//        var crawler = crawlers.Dequeue();
			//        var task = Task.Factory.StartNew(() => crawler.Crawl(getDbContext()));
			//        runningTasks.Add(task);
			//    }

			//    for (int i = runningTasks.Count - 1; i >= 0; i--)
			//    {
			//        if (runningTasks[i].Status == TaskStatus.RanToCompletion)
			//        {
			//            runningTasks[i].Dispose();
			//            runningTasks.RemoveAt(i);
			//        }
			//    }

			//    if (runningTasks.Count < 1 && !crawlers.Any())
			//        break;

			//    Thread.Sleep(5000);
			//}


			//Task.WaitAll(runningTasks.ToArray());

			//Cleanup.DownloadMissingImages(df);
		}

		static bool isCrawlerBase(Type type)
		{
			while (type != null)
			{
				if (type.Equals(typeof(CrawlerBase)))
					return true;

				type = type.BaseType;
			}

			return false;
		}

		static RecipesContext getDbContext()
		{
			var context = new RecipesContext();
			context.Configuration.AutoDetectChangesEnabled = false;
			context.Database.CommandTimeout = 120;
			return context;
		}

		//		static DataFactory getDataFactory()
		//		{
		//#if DEBUG
		//			string connString = "server=petjak.com;database=Recipes;trusted_connection=yes";
		//#else
		//			string connString = "server=localhost;database=Recipes;trusted_connection=yes";
		//#endif
		//			var asm = typeof(Recipe).Assembly;

		//			return new DataFactory(connString, asm) { TimeoutSeconds = 90 };
		//		}
	}
}
