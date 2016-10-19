//using Newtonsoft.Json;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Linq;
//using System.Net;
//using System.Web;

//namespace PaJaMa.Recipes.API.Models
//{
//	public class RecipeHelper
//	{
//		public static object GetRecipeForID(long id)
//		{
//			object recipe = null;
//			using (WebClient client = new WebClient())
//			{
//				string json = client.DownloadString(ConfigurationManager.AppSettings["RecipeAPIURL"] + "/api/recipe/" + id.ToString());
//				recipe = JsonConvert.DeserializeObject<Recipes.Model.Dto.RecipeDto>(json);
//			}
//			return recipe;
//		}
//	}
//}