using Crawler.Crawlers;

using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
	public class Cleanup
	{
		public static void Keywords()
		{
			using (var dc = new RecipesContext())
			{
				dc.Database.CommandTimeout = 100000;
				var recNames = dc.Recipes.Select(r => r.RecipeName).Distinct().ToList();
				var parts = (from rn in recNames
							from p in rn.Split(' ')
							select p.ToLower()).Distinct().OrderBy(rn => rn).ToList();
				File.WriteAllText("Keywords.txt", string.Join("\r\n", parts.ToArray()));
			}
		}
		//		public static void DownloadMissingImages(DataFactory df)
		//		{
		//			WebClient wc = new WebClient();
		//			string imgPath = @"\\pjserver\e\HTTP\RecipesAPI\Images\Recipes";
		//			int tempInt = -1;
		//			int maxID = 0;
		//			try
		//			{
		//				maxID = (from dinf in new DirectoryInfo(imgPath).GetDirectories()
		//						 from finf in dinf.GetFiles()
		//						 let fn = finf.Name.Replace(finf.Extension, "")
		//						 where int.TryParse(fn, out tempInt)
		//						 select int.Parse(fn)).Max();
		//			}
		//			catch (InvalidOperationException)
		//			{

		//			}

		//			var recImageFilt = df.CreateDataFilter<RecipeImageFilter>();
		//			recImageFilt.Field_ImageURL.FilterNull = true;
		//			recImageFilt.Field_ImageURL.OperatorType = SingleComparison.OperatorType.NotEqualTo;
		//			recImageFilt.Field_LocalImagePath.FilterNull = true;
		//			recImageFilt.IncludeRecipe = true;

		//			var recImages = recImageFilt.GetDataItems();

		//			for (int i = 0; i < recImages.Count; i++)
		//			{
		//				var recImage = recImages[i];

		//				Console.WriteLine(i.ToString() + " of " + recImages.Count.ToString() + " " + recImage.Recipe.RecipeName);

		//				if (recImage.ImageURL == "http://images.media-allrecipes.com/images/44555.png")
		//					continue;


		//				string extension = Path.GetExtension(recImage.ImageURL);
		//				if (extension.Contains("&"))
		//				{
		//					int index = extension.IndexOf("&");
		//					extension = extension.Substring(0, index);
		//				}
		//				extension = extension.Replace("jpeg", "jpg");
		//				if (string.IsNullOrEmpty(extension))
		//					extension = ".jpg";
		//				string path = Path.Combine(imgPath, recImage.Recipe.RecipeSourceID.ToString(), recImage.RecipeID.ToString(),
		//					recImage.RecipeImageID.ToString() + extension);
		//				if (!Directory.Exists(Path.GetDirectoryName(path)))
		//					Directory.CreateDirectory(Path.GetDirectoryName(path));
		//				int tries = 3;
		//				while (tries > 0)
		//				{
		//					try
		//					{
		//						if (!File.Exists(path))
		//							wc.DownloadFile(recImage.ImageURL.Replace("Ã©", "é").Replace("Ã‰", "É").Replace("Ã“", "Ó").Replace("â€™", "’").Replace("Ã±", "ñ").Replace("Ã³", "ó"), path);
		//						recImage.LocalImagePath = Path.Combine(recImage.Recipe.RecipeSourceID.ToString(), recImage.RecipeID.ToString(),
		//							recImage.RecipeImageID.ToString() + extension);
		//						recImage.Save();
		//						break;
		//					}
		//					catch
		//					{
		//						System.Threading.Thread.Sleep(1000);
		//						tries--;
		//					}
		//				}
		//			}
		//		}

		public static void YummlyToOriginal(string domain, string recipeSourceName, CrawlerBase crawler)
		{
			using (var dc = new RecipesContext())
			{
				dc.Database.CommandTimeout = 10000;
				var yummlies = dc.Recipes.Where(r => r.RecipeSourceID == 65 && r.Directions.StartsWith(domain)).ToList();
				if (!yummlies.Any()) return;
				var origs = dc.Recipes.Where(r => r.RecipeSource.RecipeSourceName.Contains(recipeSourceName)).Select(r => r.RecipeURL).ToList();
				List<string> skipids = new List<string>();
				int i = 1;
				foreach (var yummly in yummlies)
				{
					Console.WriteLine((i++).ToString() + " of " + yummlies.Count.ToString());
					if (!origs.Any(f => f.ToLower() == yummly.Directions.ToLower()))
					{
						try
						{
							Console.ForegroundColor = ConsoleColor.Blue;
							Console.WriteLine(yummly.RecipeName + " - " + yummly.Directions);
							Console.ResetColor();
							var rec = crawler.CreateRecipe(yummly.Directions, yummly.RecipeName);
							if (rec == null)
								skipids.Add(yummly.ID.ToString());
						}
						catch
						{
							continue;
						}
					}
					else
					{
						Console.WriteLine(yummly.RecipeName + " - " + yummly.Directions);
					}
				}

				using (var cmd = dc.Database.Connection.CreateCommand())
				{
					cmd.CommandTimeout = 600000;
					cmd.CommandText = "delete from Recipe where Directions like '" + domain + "%'";
					if (skipids.Any())
						cmd.CommandText += " and RecipeID not in (" + string.Join(",", skipids) + ")";
					cmd.Connection.Open();
					cmd.ExecuteNonQuery();
					cmd.Connection.Close();
				}
			}
		}
	}
}
