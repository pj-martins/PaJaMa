using PaJaMa.Data;
using PaJaMa.Web;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Text;
using System.Data.SQLite;
using PaJaMa.Recipes.Model;
using System.Data.Common;
using System.Data.SqlClient;

namespace PaJaMa.Recipes.Web.Api.Repository
{
	public class RecipeSearchRepository : Repository<RecipesContext, Recipe>
	{
		private string foodImagePath
		{
			get { return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, "/Images/food.png"); }
		}

		private Tuple<string, object[]> getQueryParameters(string recipeName, string includes, string excludes, float? rating,
			bool bookmarked, int? recipeSourceID, bool picturesOnly)
		{
			int? userId = null;

			// Database has a full text catalog so we can search by ingredients and recipe name, however,
			// EF does not support full text search so we need to create our sql query manually
			StringBuilder sbSQL = new StringBuilder();
			List<DbParameter> paramz = new List<DbParameter>();
			bool isSqlite = context.Database.Connection is SQLiteConnection;


			sbSQL.AppendLine(string.Format("select {0} r.RecipeID from Recipe r", isSqlite ? "" : "top 1000"));
			if (userId != null)
			{
				sbSQL.AppendLine("	left join UserRecipe ur on ur.RecipeID = r.RecipeID and ur.UserID = @UserID");
				if (isSqlite)
					paramz.Add(new SQLiteParameter("@UserID", userId.Value));
				else
					paramz.Add(new SqlParameter("@UserID", userId.Value));
			}
			sbSQL.AppendLine("where 1 = 1");

			if (rating != null)
			{
				if (userId != null)
					sbSQL.AppendLine("and isnull(ur.Rating, r.Rating) >= @Rating");
				else
					sbSQL.AppendLine("and Rating >= @Rating");
				if (isSqlite)
					paramz.Add(new SQLiteParameter("@Rating", rating.Value));
				else
					paramz.Add(new SqlParameter("@Rating", rating.Value));
			}

			// TODO: user
			if (bookmarked)
				sbSQL.AppendLine("and exists(select 1 from UserRecipe ur join [User] u on u.UserID = ur.UserID where IsBookmarked = 1 and ur.RecipeID = r.RecipeID and u.UserName = 'PJ')");

			if (recipeSourceID != null)
			{
				sbSQL.AppendLine("and RecipeSourceID = @RecipeSourceID");
				if (isSqlite)
					paramz.Add(new SQLiteParameter("@RecipeSourceID", recipeSourceID.Value));
				else
					paramz.Add(new SqlParameter("@RecipeSourceID", recipeSourceID.Value));
			}

			if (picturesOnly)
				sbSQL.AppendLine("and exists (select 1 from RecipeImage where RecipeID = r.RecipeID)");

			if (!string.IsNullOrEmpty(recipeName))
				sbSQL.AppendLine("and r.RecipeName like '%" + recipeName.Replace("'", "''") + "%'");

			if (!string.IsNullOrEmpty(includes))
			{
				string[] parts = includes.Split(';');
				sbSQL.AppendLine("and r.RecipeID in (");
				sbSQL.AppendLine("select RecipeID from RecipeSearch where " +
					string.Join(" and ", parts.Select(p => "IngredientString like '%" + p.Replace("'", "''") + "%'").ToArray())
				);
				sbSQL.AppendLine(")");

			}

			if (!string.IsNullOrEmpty(excludes))
			{
				string[] parts = excludes.Split(';');
				sbSQL.AppendLine("and r.RecipeID not in (");

				for (int i = 0; i < parts.Length; i++)
				{
					sbSQL.AppendLine((i > 0 ? "UNION ALL\r\n" : "") + @"
	select rim.RecipeID from Ingredient i
	join IngredientMeasurement im on im.IngredientID = i.IngredientID
	join RecipeIngredientMeasurement rim on rim.IngredientMeasurementID = im.IngredientMeasurementID
	where IngredientName like '%" + parts[i].Replace("'", "''") + @"%'
");
				}

				sbSQL.AppendLine(")");
			}

			if (isSqlite)
				sbSQL.AppendLine("limit 1000");

			return new Tuple<string, object[]>(sbSQL.ToString(), paramz.ToArray());
		}

		private IQueryable<object> getRecipes(List<int> ids)
		{
			return context.Recipes
					//.Include("RecipeImages")
					//.Include("RecipeIngredientMeasurements.IngredientMeasurement.Ingredient")
					.Select(r => new
					{
						ID = r.RecipeID,
						r.RecipeID,
						r.RecipeName,
						r.Rating,
						ImageURL = r.RecipeImages.Select(ri => ri.ImageURL).FirstOrDefault(),
						Ingredients = r.RecipeIngredientMeasurements
							.Select(rim => rim.IngredientMeasurement.Ingredient.IngredientName)
					})
					.Where(r => ids.Contains(r.RecipeID))
					.OrderBy(r => r.RecipeName);
		}

		public IQueryable<object> SearchRecipes(string recipeName, string includes, string excludes, float? rating,
			bool bookmarked, int? recipeSourceID, bool picturesOnly, int page, int pageSize, out int count)
		{
			// we're doing two database hits, one to get the result count, a second to get the paged results. Since parameters have to be
			// unique within each database hit, we need to retrieve a new set of parameters for each hit

			var qryParams = getQueryParameters(recipeName, includes, excludes, rating, bookmarked, recipeSourceID, picturesOnly);
			List<int> ids = new List<int>();
			using (var cmd = context.Database.Connection.CreateCommand())
			{
				cmd.CommandText = qryParams.Item1;
				cmd.Parameters.AddRange(qryParams.Item2);
				context.Database.Connection.Open();
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							ids.Add(Convert.ToInt32(rdr[0]));
						}
					}
				}
				context.Database.Connection.Close();
			}
			count = ids.Count();

			return getRecipes(ids)
				.Skip((page - 1) * pageSize)
					.Take(pageSize);
		}

		public IQueryable<object> GetRandomRecipes(int numberOfRecipes)
		{
			var ids = context.Recipes
				.Where(r => r.Rating > 4 && r.RecipeImages.Any())
				.OrderBy(r => Guid.NewGuid())
				.Take(numberOfRecipes)
				.Select(r => r.RecipeID).ToList();

			return getRecipes(ids);
		}
	}
}