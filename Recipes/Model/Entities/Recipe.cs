//------------------------------------------------------------------------------
// <onetime-generated>
// code is only generated if not exists, do custom stuff in here
// </onetime-generated>
//------------------------------------------------------------------------------

namespace PaJaMa.Recipes.Model.Entities
{
    using PaJaMa.Data;
    using PaJaMa.Recipes.Model.Entities.Base;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Recipe : RecipeBase
    {
        public string RecipeSourceName
        {
            get { return RecipeSource == null ? string.Empty : RecipeSource.RecipeSourceName; }
        }

		public override void OnEntitySaved(DbContextBase context)
		{
            base.OnEntitySaved(context);

            var db = context as RecipesContext;
            var recSearch = db.RecipeSearches.FirstOrDefault(rs => rs.RecipeID == RecipeID);
            if (recSearch == null)
            {
                recSearch = new RecipeSearch();
                recSearch.RecipeID = RecipeID;
                db.RecipeSearches.Add(recSearch);
            }
            recSearch.RecipeName = RecipeName;
            recSearch.Ingredients = string.Join(" ", RecipeIngredientMeasurements.Select(ri =>
                ri.IngredientMeasurement.Ingredient.IngredientName).ToArray());
            if (recSearch.Ingredients.Length > 5000)
                recSearch.Ingredients = recSearch.Ingredients.Substring(0, 5000);
            db.SaveChanges();
        }
    }
}
