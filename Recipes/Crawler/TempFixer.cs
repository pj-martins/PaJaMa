//
//using PaJaMa.Recipes.Model;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Crawler
//{
//	public class TempFixer
//	{
//		public static void Fix(DataFactory df)
//		{
//			//var recFilt = df.CreateDataFilter<RecipeFilter>();
//			//recFilt.ImageURL = "http://foodnetwork";
//			//recFilt.Field_ImageURL.OperatorType = SingleComparison.OperatorType.BeginsWith;
//			//recFilt.Field_RecipeURL.FilterNull = true;
//			//recFilt.Field_RecipeSourceID.FilterNull = true;
//			//recFilt.Field_RecipeSourceID.OperatorType = SingleComparison.OperatorType.NotEqualTo;
//			//var recs = recFilt.GetDataItems();
//			//foreach (var rec in recs)
//			//{
//			//	Console.WriteLine(rec.RecipeName + " - " + rec.RecipeSourceName);
//			//	string searchUrl = string.Format("http://www.bing.com/search?q={0}",
//			//		rec.RecipeName.Replace(" ", "+") + "+" + rec.RecipeSourceName.Replace(" ", "+"));
//			//	WebClient wc = new WebClient();
//			//	string html = wc.DownloadString(searchUrl);
//			//	Match m = Regex.Match(html, "href=\"(http://www\\.foodnetwork\\.com.*?)\"target=\"_blank\"");
//			//	if (!m.Success)
//			//		continue;
//			//	rec.RecipeURL = m.Groups[1].Value;
//			//	if (rec.RecipeURL.IndexOf("?") != -1)
//			//		rec.RecipeURL = rec.RecipeURL.Substring(0, rec.RecipeURL.IndexOf("?"));
//			//	if (rec.RecipeURL.EndsWith("#!"))
//			//		rec.RecipeURL = rec.RecipeURL.Substring(0, rec.RecipeURL.Length - 2);
//			//	Console.WriteLine(rec.RecipeURL);
//			//	rec.Save();
//			//	System.Threading.Thread.Sleep(100);
//			//}
//		}
//	}
//}
