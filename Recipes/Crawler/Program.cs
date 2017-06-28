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
            new FoodNetworkCrawler().Crawl(getDbContext());
#endif
#if Thread2
			// new AllRecipesCrawler().Crawl(getDbContext());
			new TheKitchnCrawler().Crawl(getDbContext());
#endif
#if Thread3
            // new EpicuriousCrawler().Crawl(getDbContext());
            new SeriousEatsCrawler().Crawl(getDbContext());
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
