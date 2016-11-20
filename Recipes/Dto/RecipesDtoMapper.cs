using AutoMapper;
using PaJaMa.Dto;
using PaJaMa.Recipes.Dto.Entities;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Dto
{
    public class RecipesDtoMapper : RecipesDtoMapperBase
    {
		public override PaJaMa.Data.DbContextBase GetDbContext()
		{
			return new Model.RecipesContext();
		}

		protected override void createMaps(IMapperConfigurationExpression cfg)
		{
			base.createMaps(cfg);
			cfg.CreateMap<Recipe, RecipeCoverDto>()
				.ForMember(x => x.ID, y => y.MapFrom(r => r.RecipeID))
				.ForMember(x => x.ImageURLs, y => y.MapFrom(r => r.RecipeImages.OrderByDescending(ri => ri.Sequence).Select(i => i.ImageURL)))
				.ForMember(x => x.Ingredients, y => y.MapFrom(r => r.RecipeIngredientMeasurements.Select(rim =>
					rim.IngredientMeasurement.Ingredient.IngredientName)))
				;

			var mapping = Mappings[typeof(RecipeIngredientMeasurement)] as IMappingExpression<RecipeIngredientMeasurement, RecipeIngredientMeasurementDto>;
			mapping.ForMember(ri => ri.Alternates, c => c.MapFrom(ri => from ria in ri.IngredientMeasurement.FromIngredientMeasurementAlternates
																		select ria));
		}
	}
}
