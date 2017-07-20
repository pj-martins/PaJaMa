using PaJaMa.Data;
using PaJaMa.Web;
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
	public class RecipeRepository : Repository<RecipesContext, Recipe>
	{
		public override Recipe GetEntity(int id)
		{
			return (context as RecipesContext).Recipes
			.Include("RecipeImages")
			.Include("RecipeSource")
			.Include("RecipeIngredientMeasurements.IngredientMeasurement.Ingredient")
			.Include("RecipeIngredientMeasurements.IngredientMeasurement.Measurement")
			.Where(r => r.RecipeID == id).First();
		}

		private UserRecipe getUserRecipe(int recipeId)
		{
			// TODO: actual user
			var userId = context.Users.First(u => u.UserName == "PJ").UserID;

			var ur = context.UserRecipes.FirstOrDefault(x => x.UserID == userId && x.RecipeID == recipeId);
			if (ur == null)
			{
				ur = new UserRecipe();
				ur.UserID = userId;
				ur.RecipeID = recipeId;
				context.UserRecipes.Add(ur);
			}
			else
				context.Entry(ur).State = EntityState.Modified;
			return ur;
		}

		public void BookmarkRecipe(int recipeId)
		{
			var ur = getUserRecipe(recipeId);
			ur.IsBookmarked = !ur.IsBookmarked;
			context.SaveChanges();
		}
	}
}