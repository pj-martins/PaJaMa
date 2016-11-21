using PaJaMa.Data;
using PaJaMa.Web;
using PaJaMa.Recipes.Dto.Entities;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Text;
using System.Data.SqlClient;
using PaJaMa.Recipes.Model;
using PaJaMa.Recipes.Dto;

namespace PaJaMa.Recipes.Web.Api.Repository
{
	public class RecipeRepository : Repository<RecipesDtoMapper, Recipe, RecipeDto>
	{
        // automapper generally does the includes automatically but not for sqlite
        protected override Recipe getEntity(int id)
        {
			var context = mapper.GetDbContext();
            return (context as RecipesContext).Recipes
                .Include("RecipeImages")
                .Include("RecipeSource")
                .Include("RecipeIngredientMeasurements.IngredientMeasurement.Ingredient")
                .Include("RecipeIngredientMeasurements.IngredientMeasurement.Measurement")
                .Where(r => r.RecipeID == id).First();
        }
    }
}