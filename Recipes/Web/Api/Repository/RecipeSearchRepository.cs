using PaJaMa.Common;
using PaJaMa.Web;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Text;
using PaJaMa.Recipes.Model;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace PaJaMa.Recipes.Web.Api.Repository
{
	public class RecipeSearchRepository : Repository<RecipesContext, Recipe>
	{
		public List<RecipeSearch> SearchRecipes(string recipeName, string includes, string excludes, float? rating,
			bool bookmarked, bool rated, int? recipeSourceID, bool picturesOnly, int page, int pageSize, out int count)
		{
			StringBuilder sbSQL = new StringBuilder();
			try
			{
				// TODO:
				string userName = "PJ";
				Dictionary<string, object> paramz = new Dictionary<string, object>();

				// should be RecipeName in order but this is faster
				sbSQL.AppendLine("select top 1000 r.RecipeID as ID, r.*, RowNum = row_number() over (order by r.RecipeID)");
				if (!string.IsNullOrEmpty(userName))
					sbSQL.AppendLine(", UserRating = ur.Rating");
				if ((bookmarked || rated) && !string.IsNullOrEmpty(userName))
				{
					sbSQL.AppendLine($@"from UserRecipe ur 
join [User] u on u.UserID = ur.UserID and u.UserName = @UserName
	{(bookmarked ? " and ur.IsBookmarked = 1" : "")}
	{(rated ? " and isnull(ur.Rating, 0) > 0" : "")}
join RecipeSearch r on r.RecipeID = ur.RecipeID");
					paramz.Add("@UserName", userName);
				}
				else
					sbSQL.AppendLine("from RecipeSearch r");

				if (!string.IsNullOrEmpty(userName) && !bookmarked && !rated)
				{
					sbSQL.AppendLine(@"	left join UserRecipe ur on ur.RecipeID = r.RecipeID 
left join [User] u on u.UserID = ur.UserID
and u.UserName = @UserName");
					paramz.Add("@UserName", userName);
				}
				sbSQL.AppendLine("where 1 = 1");

				if (rating != null)
				{
					if (!string.IsNullOrEmpty(userName))
						sbSQL.AppendLine("and isnull(ur.Rating, r.Rating) >= @Rating");
					else
						sbSQL.AppendLine("and Rating >= @Rating");
					paramz.Add("@Rating", rating.Value);
				}

				if (recipeSourceID != null)
				{
					sbSQL.AppendLine("and RecipeSourceID = @RecipeSourceID");
					paramz.Add("@RecipeSourceID", recipeSourceID.Value);
				}

				if (picturesOnly)
					sbSQL.AppendLine("and CoverImageURL is not null");

				if (!string.IsNullOrEmpty(recipeName))
				{
					sbSQL.AppendLine("and Contains(RecipeName, '\"*" + recipeName.Replace("'", "''").Replace("\"", "\"\"") + "*\"')");
				}

				if (!string.IsNullOrEmpty(includes))
				{
					string[] parts = includes.Split(';');
					sbSQL.AppendLine("and Contains(IngredientString, '" +
						string.Join(" AND ", parts.Select(p => "\"*" + p.Replace("'", "''").Replace("\"", "\"\"") + "*\"").ToArray())
					);
					sbSQL.AppendLine("')");
				}

				if (!string.IsNullOrEmpty(excludes))
				{
					string[] parts = excludes.Split(';');
					sbSQL.AppendLine("and not Contains(IngredientString, '" +
						string.Join(" OR ", parts.Select(p => "\"*" + p.Replace("'", "''").Replace("\"", "\"\"") + "*\"").ToArray())
					);
					sbSQL.AppendLine("')");
				}

				DataTable dt = new DataTable();
				using (var cmd = context.Database.Connection.CreateCommand() as SqlCommand)
				{
					context.Database.Connection.Open();
					cmd.CommandText = $"select isnull(count(*), 0) from ({sbSQL.ToString()}) z";
					foreach (var kvp in paramz)
						cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
					count = (int)cmd.ExecuteScalar();
					context.Database.Connection.Close();
				}

				using (var cmd = context.Database.Connection.CreateCommand() as SqlCommand)
				{
					context.Database.Connection.Open();
					int start = 1 + ((page - 1) * pageSize);
					cmd.CommandText = $"select * from ({sbSQL.ToString()}) z where RowNum between {start} and {start + pageSize - 1}";
					foreach (var kvp in paramz)
						cmd.Parameters.AddWithValue(kvp.Key, kvp.Value);
					using (var rdr = cmd.ExecuteReader())
					{
						if (rdr.HasRows)
						{
							dt.Load(rdr);
						}
					}
					context.Database.Connection.Close();
				}
				return dt.ToObjects<RecipeSearch>();
			}
			catch (Exception ex)
			{
				throw new Exception(sbSQL.ToString(), ex);
			}
		}

		public List<RecipeSearch> GetRandomRecipes(int numberOfRecipes)
		{
			DataTable dt = new DataTable();
			using (var cmd = context.Database.Connection.CreateCommand())
			{
				cmd.CommandText = $@"select rs.RecipeID as ID, rs.* from
(
select top {numberOfRecipes} RecipeID from Recipe r
where Rating > 4.6 order by newid()
) z
join RecipeSearch rs on rs.RecipeID = z.RecipeID";
				context.Database.Connection.Open();
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						dt.Load(rdr);
					}
				}
				context.Database.Connection.Close();
			}
			return dt.ToObjects<RecipeSearch>();
		}
	}
}