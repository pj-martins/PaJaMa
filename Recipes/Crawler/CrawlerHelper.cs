using PaJaMa.Common;
using PaJaMa.Recipes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using PaJaMa.Recipes.Model.Entities;

namespace Crawler
{
	public class CrawlerHelper
	{
		private static object _lockRecipeSource = new object();
		public static object LockObject = new object();

		private static List<RecipeSource> _recipeSources;
		private static List<Measurement> _measurements;

		public static Measurement GetMeasurement(RecipesContext db, string measurementName, bool create)
		{
			if (_measurements == null)
				_measurements = db.Measurements.ToList();

			var measurement = _measurements.FirstOrDefault(m => m.MeasurementName.ToLower().Trim() == measurementName.ToLower().Trim());
			if (create && measurement == null)
			{
				measurement = new Measurement();
				measurement.MeasurementName = measurementName;
				db.Measurements.Add(measurement);
				db.SaveChanges();
				_measurements.Add(measurement);
			}
			return measurement;
		}

		public static RecipeSource GetRecipeSource(RecipesContext db, string sourceName)
		{
			if (_recipeSources == null)
			{
				lock (_lockRecipeSource)
					_recipeSources = db.RecipeSources.ToList();
			}

			RecipeSource rs = _recipeSources.FirstOrDefault(x => x.RecipeSourceName == sourceName);
			if (rs == null)
			{
				rs = new RecipeSource();
				rs.RecipeSourceName = sourceName;
				db.RecipeSources.Add(rs);
				db.SaveChanges();
				_recipeSources.Add(rs);
			}
			return rs;
		}

		public static RecipeIngredientMeasurement GetIngredient(RecipesContext db, string ingredient, Measurement measurement, double qty)
		{
			if (string.IsNullOrEmpty(ingredient)) throw new NotImplementedException();
			if (ingredient.Length > 255)
				ingredient = ingredient.Substring(0, 255);

			Ingredient ing = db.Ingredients.FirstOrDefault(i => i.IngredientName == ingredient);
			if (ing == null)
			{
				ing = new Ingredient();
				ing.IngredientName = ingredient;
				db.Ingredients.Add(ing);
				db.SaveChanges();
			}

			int? measurementID = (measurement == null ? (int?)null : measurement.MeasurementID);

			var im = db.IngredientMeasurements.FirstOrDefault(x => x.IngredientID == ing.IngredientID
				&& x.MeasurementID == measurementID);

			if (im == null)
			{
				im = new IngredientMeasurement();
				im.IngredientID = ing.IngredientID;
				if (measurement != null) im.MeasurementID = measurement.MeasurementID;
				db.IngredientMeasurements.Add(im);
				db.SaveChanges();
			}


			var recIngrMeasurement = new RecipeIngredientMeasurement();
			recIngrMeasurement.IngredientMeasurementID = im.IngredientMeasurementID;
			if (qty != 0)
				recIngrMeasurement.Quantity = (float)qty;

			return recIngrMeasurement;
		}


		public static string ChildSafeName(string input)
		{
			return input.Replace("fucker", "f***er").Replace("fuck", "f***").Replace("Fucker", "F***er").Replace("Fuck", "F***");
		}

		public static Tuple<string, Measurement, float> GetIngredientQuantity(RecipesContext db, string line, bool createMeasurement, bool ingredientIncluded = false)
		{
			string[] tos = new string[] { "-", "–" };

			string ingredient = string.Empty;

			List<string> parts = Common.StripHTML(line.Replace(" to ", "-").Replace("\n", " ").Replace("-", " - ")).Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).ToList();

			string measurementName = "";
			string qtyString = "";

			int index = 0;
			double tempQty = 0;
			while (index < parts.Count && (Common.TryParseFraction(parts[index], out tempQty) || tos.Contains(parts[index]) || parts[index] == "."))
			{
				index++;
			}

			if (index > 0 && parts[index - 1] == ".")
				index--;

			for (int i = 0; i < index; i++)
			{
				qtyString += parts[i].Trim() + " ";
			}

			float qty = 0;
			if (!string.IsNullOrEmpty(qtyString))
			{
				qtyString = qtyString.Trim();
				if (qtyString.Contains("/") || qtyString.Contains("¼") || qtyString.Contains("½") || qtyString.Contains("¾") || qtyString.Contains("⅓"))
				{
					if (!PaJaMa.Common.Common.TryParseFraction(qtyString, out qty))
						throw new NotImplementedException();
				}
				else if (tos.Any(t => qtyString.Contains(t)))
				{
					string[] parts2 = qtyString.Split(new string[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
					if (parts2.Length != 2)
						index = 0;
					else
					{
						float first = 0;
						float second = 0;

						if (!string.IsNullOrEmpty(parts2[0].Trim()) && !PaJaMa.Common.Common.TryParseFraction(parts2[0], out first))
							throw new NotImplementedException();

						if (!string.IsNullOrEmpty(parts2[1].Trim()) && !PaJaMa.Common.Common.TryParseFraction(parts2[1], out second))
							throw new NotImplementedException();

						qty = (first + second) / 2;
					}
				}
				else if (qtyString.Contains(" "))
				{
					string[] parts2 = qtyString.Split(' ');
					if (parts2[0] == ".")
						qty = Convert.ToSingle(qtyString.Replace(" ", ""));
					else
					{
						qty = Convert.ToSingle(parts2[0]);
						index -= parts2.Length - 1;
					}
				}
				else
					qty = Convert.ToSingle(qtyString);
			}

			bool firstIn = true;
			for (int i = index; i < parts.Count; i++)
			{
				if (firstIn)
					measurementName += parts[i].Trim() + " ";
				else
					ingredient = ingredient + " " + parts[i].Trim();
				firstIn = false;
			}

			ingredient = ingredient.Trim();
			measurementName = measurementName.Trim();

			Measurement measurement = null;
			if (!string.IsNullOrEmpty(measurementName))
			{
				measurement = GetMeasurement(db, measurementName, createMeasurement);
				if (measurement == null)
				{
					ingredient = measurementName + " " + ingredient;
					measurementName = string.Empty;
				}
			}

			if (string.IsNullOrEmpty(ingredient) && measurement != null && ingredientIncluded)
			{
				ingredient = measurement.MeasurementName;
				measurement = null;
			}

			return new Tuple<string, Measurement, float>(ingredient, measurement, qty);
		}

		public static string[] GetKeywords(string startKeyword)
		{
			string[] keywords = Properties.Resources.Keywords.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			if (!string.IsNullOrEmpty(startKeyword))
			{
				int kindex = keywords.ToList().IndexOf(startKeyword);
				if (kindex > 0)
					keywords = keywords.Skip(kindex).ToArray();
			}
			return keywords;
		}
	}
}
