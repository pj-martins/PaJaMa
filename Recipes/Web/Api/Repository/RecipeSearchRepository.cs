using AutoMapper.QueryableExtensions;
using PaJaMa.Data;
using PaJaMa.Web;
using PaJaMa.Recipes.Model.Dto;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Text;
using System.Data.SqlClient;
using PaJaMa.Recipes.Model;

namespace PaJaMa.Recipes.Web.Api.Repository
{
	public class RecipeSearchRepository : Repository<RecipesContext, Recipe, RecipeCoverDto>
	{
		private string foodImagePath
		{
			get { return string.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Authority, "/Images/food.png"); }
		}

		private Tuple<string, object[]> getQueryParameters(string includes, string excludes, float? rating,
			bool bookmarked, int? recipeSourceID, bool picturesOnly)
		{
			int? userId = null;

			// Database has a full text catalog so we can search by ingredients and recipe name, however,
			// EF does not support full text search so we need to create our sql query manually
			StringBuilder sbSQL = new StringBuilder();
			List<SqlParameter> paramz = new List<SqlParameter>();

			sbSQL.AppendLine("select top 1000 r.* from Recipe r with (nolock)");
			sbSQL.AppendLine("join RecipeSearch rs with (nolock) on rs.RecipeID = r.RecipeID");
			if (userId != null)
			{
				sbSQL.AppendLine("	left join UserRecipe ur with (nolock) on ur.RecipeID = r.RecipeID and ur.UserID = @UserID");
				paramz.Add(new SqlParameter("@UserID", userId.Value));
			}
			sbSQL.AppendLine("where 1 = 1");

			if (rating != null)
			{
				if (userId != null)
					sbSQL.AppendLine("and isnull(ur.Rating, r.Rating) >= @Rating");
				else
					sbSQL.AppendLine("and Rating >= @Rating");
				paramz.Add(new SqlParameter("@Rating", rating.Value));
			}

			if (bookmarked)
				sbSQL.AppendLine("and IsBookmarked = 1");

			if (recipeSourceID != null)
			{
				sbSQL.AppendLine("and RecipeSourceID = @RecipeSourceID");
				paramz.Add(new SqlParameter("@RecipeSourceID", recipeSourceID.Value));
			}

			if (picturesOnly)
				sbSQL.AppendLine("and exists (select 1 from RecipeImage where RecipeID = r.RecipeID)");

			if (!string.IsNullOrEmpty(includes))
			{
				string[] parts = includes.Split(';');
				includes = "\"" + includes.Replace(";", " ").Replace(" ", "*\" AND \"") + "*\"";
				sbSQL.AppendLine("and ((contains(rs.Ingredients, @Includes) or contains(rs.RecipeName, @Includes))");
				if (parts.Length > 1)
				{
					sbSQL.AppendLine("or (contains(rs.RecipeName, @RecipeInclude) and contains(rs.Ingredients, @RestIncludes))");
					paramz.Add(new SqlParameter("@RecipeInclude", "\"" + parts[0].Replace(" ", "*\" AND \"") + "*\""));
					string rest = string.Empty;
					for (int i = 1; i < parts.Length; i++)
					{
						rest += parts[i] + " ";
					}
					rest = rest.Trim();
					paramz.Add(new SqlParameter("@RestIncludes", "\"" + rest.Replace(" ", "*\" AND \"") + "*\""));
				}
				sbSQL.AppendLine(")");
				paramz.Add(new SqlParameter("@Includes", includes));
			}

			if (!string.IsNullOrEmpty(excludes))
			{
				excludes = "\"" + excludes.Replace(";", " ").Replace(" ", "*\" AND \"") + "*\"";
				sbSQL.AppendLine("and (not contains(rs.Ingredients, @Excludes) and not contains(rs.RecipeName, @Excludes))");
				paramz.Add(new SqlParameter("@Excludes", excludes));
			}

			sbSQL.AppendLine("order by rs.RecipeName");

			return new Tuple<string, object[]>(sbSQL.ToString(), paramz.ToArray());
		}

		public IQueryable<RecipeCoverDto> SearchRecipes(string includes, string excludes, float? rating,
			bool bookmarked, int? recipeSourceID, bool picturesOnly, int page, int pageSize, out int count)
		{
			// we're doing two database hits, one to get the result count, a second to get the paged results. Since parameters have to be
			// unique within each database hit, we need to retrieve a new set of parameters for each hit

			var qryParams = getQueryParameters(includes, excludes, rating, bookmarked, recipeSourceID, picturesOnly);
			var qry = context.Recipes.SqlQuery(qryParams.Item1, qryParams.Item2);
			count = qry.Count();

			qryParams = getQueryParameters(includes, excludes, rating, bookmarked, recipeSourceID, picturesOnly);
			qry = context.Recipes.SqlQuery(qryParams.Item1, qryParams.Item2);
			return qry.Skip((page - 1) * pageSize).Take(pageSize).AsQueryable().ProjectTo<RecipeCoverDto>(context.MapperConfig);
		}

		public IQueryable<RecipeCoverDto> GetRandomRecipes(int numberOfRecipes)
		{
			int[] ids = context.Recipes
				.Where(r => r.Rating > 4 && r.RecipeImages.Any())
				.OrderBy(r => Guid.NewGuid())
				.Take(numberOfRecipes)
				.Select(r => r.RecipeID).ToArray();

			return context.Recipes.Where(r => ids.Contains(r.RecipeID)).ProjectTo<RecipeCoverDto>(context.MapperConfig);
		}
	}
}