//using PaJaMa.Recipes.Model;
//using PaJaMa.Recipes.Model.Entities;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Crawler.Crawlers
//{
//    // place holder
//    [RecipeSource("Food Network", IgnoreAuto = true)]
//    public class RawTextCrawler : CrawlerBase
//    {
//        protected override void crawl(int startPage)
//        {
//            string allText = File.ReadAllText("d:\\recipestxt.txt");
//            string[] recipes = allText.Split(new string[] { "_*" }, StringSplitOptions.RemoveEmptyEntries);
//            foreach (string recipe in recipes)
//            {
//                var rec = new Recipe();
//                string[] recipeParts = recipe.Split(new string[] { "_-" }, StringSplitOptions.RemoveEmptyEntries);
//                if (recipeParts.Length != 6) throw new NotImplementedException();
//                rec.RecipeName = recipeParts[0].Trim();
//                rec.RecipeSource = CrawlerHelper.GetRecipeSource(DbContext, recipeParts[1].Trim());
//                rec.NumberOfServings = Convert.ToInt16(recipeParts[2].Trim());
//                if (recipeParts[3].Trim() != "0")
//                    rec.Rating = Convert.ToSingle(recipeParts[3].Trim());
//                foreach (var ingr in getIngredients(recipeParts[4].Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList()))
//                    rec.RecipeIngredientMeasurements.Add(ingr);

//                // cache it
//                rec.Directions = recipeParts[5].Trim();
//                DbContext.Recipes.Add(rec);
//                DbContext.SaveChanges();
//            }
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
//}
