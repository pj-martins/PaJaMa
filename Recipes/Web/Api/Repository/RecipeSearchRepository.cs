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

namespace PaJaMa.Recipes.Web.Api.Repository
{
    public class RecipeSearchRepository : Repository<RecipesContext, Recipe>
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
            List<SQLiteParameter> paramz = new List<SQLiteParameter>();

            sbSQL.AppendLine(string.Format("select {0} r.RecipeID from Recipe r", context.Database.Connection is SQLiteConnection ? "" : "top 1000"));
            if (userId != null)
            {
                sbSQL.AppendLine("	left join UserRecipe ur on ur.RecipeID = r.RecipeID and ur.UserID = @UserID");
                paramz.Add(new SQLiteParameter("@UserID", userId.Value));
            }
            sbSQL.AppendLine("where 1 = 1");

            if (rating != null)
            {
                if (userId != null)
                    sbSQL.AppendLine("and isnull(ur.Rating, r.Rating) >= @Rating");
                else
                    sbSQL.AppendLine("and Rating >= @Rating");
                paramz.Add(new SQLiteParameter("@Rating", rating.Value));
            }

            if (bookmarked)
                sbSQL.AppendLine("and IsBookmarked = 1");

            if (recipeSourceID != null)
            {
                sbSQL.AppendLine("and RecipeSourceID = @RecipeSourceID");
                paramz.Add(new SQLiteParameter("@RecipeSourceID", recipeSourceID.Value));
            }

            if (picturesOnly)
                sbSQL.AppendLine("and exists (select 1 from RecipeImage where RecipeID = r.RecipeID)");

            if (!string.IsNullOrEmpty(includes))
            {
                string[] parts = includes.Split(';');
                sbSQL.AppendLine("and r.RecipeID in (");

                for (int i = 0; i < parts.Length; i++)
                {
                    sbSQL.AppendLine((i > 0 ? "INTERSECT\r\n" : "") + @"
	select rim.RecipeID from Ingredient i
	join IngredientMeasurement im on im.IngredientID = i.IngredientID
	join RecipeIngredientMeasurement rim on rim.IngredientMeasurementID = im.IngredientMeasurementID
	where IngredientName like '%" + parts[i].Replace("'", "''") + @"%'
");
                }

                //if (parts.Length > 1)
                //{
                //	sbSQL.AppendLine("or (contains(rs.RecipeName, @RecipeInclude) and contains(rs.Ingredients, @RestIncludes))");
                //	paramz.Add(new SqlParameter("@RecipeInclude", "\"" + parts[0].Replace(" ", "*\" AND \"") + "*\""));
                //	string rest = string.Empty;
                //	for (int i = 1; i < parts.Length; i++)
                //	{
                //		rest += parts[i] + " ";
                //	}
                //	rest = rest.Trim();
                //	paramz.Add(new SqlParameter("@RestIncludes", "\"" + rest.Replace(" ", "*\" AND \"") + "*\""));
                //}

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

            if (context.Database.Connection is SQLiteConnection)
                sbSQL.AppendLine("limit 1000");

            return new Tuple<string, object[]>(sbSQL.ToString(), paramz.ToArray());
        }

        public IQueryable<Recipe> SearchRecipes(string includes, string excludes, float? rating,
            bool bookmarked, int? recipeSourceID, bool picturesOnly, int page, int pageSize, out int count)
        {
            // we're doing two database hits, one to get the result count, a second to get the paged results. Since parameters have to be
            // unique within each database hit, we need to retrieve a new set of parameters for each hit

            var qryParams = getQueryParameters(includes, excludes, rating, bookmarked, recipeSourceID, picturesOnly);
            List<long> ids = new List<long>();
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
                            ids.Add(Convert.ToInt64(rdr[0]));
                        }
                    }
                }
                context.Database.Connection.Close();
            }
            count = ids.Count();

            return context.Recipes
                    .Include("RecipeImages")
                    // .Include("RecipeIngredientMeasurements.IngredientMeasurement.Ingredient")
                    .Where(r => ids.Contains(r.RecipeID))
                    .OrderBy(r => r.RecipeName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize);
        }

        public IQueryable<Recipe> GetRandomRecipes(int numberOfRecipes)
        {
            int[] ids = context.Recipes
                .Where(r => r.Rating > 4 && r.RecipeImages.Any())
                .OrderBy(r => Guid.NewGuid())
                .Take(numberOfRecipes)
                .Select(r => r.RecipeID).ToArray();

            return context.Recipes
                .Include("RecipeImages")
                .Where(r => ids.Contains(r.RecipeID));
        }
    }
}