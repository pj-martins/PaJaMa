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
    }

	public class RecipeSearch
	{
		public int ID { get; set; }
		public int RecipeID { get; set; }
		public string RecipeName { get; set; }
		public string IngredientString { get; set; }
		public float? Rating { get; set; }
		public float? UserRating { get; set; }
		public string CoverImageURL { get; set; }
	}
}
