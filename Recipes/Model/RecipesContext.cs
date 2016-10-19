using AutoMapper;
using PaJaMa.Common;
using PaJaMa.Data;
using PaJaMa.Recipes.Model.Dto;
using PaJaMa.Recipes.Model.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Model
{
	public class RecipesContext : RecipesContextBase
	{
		public RecipesContext()
			: base()
		{
		}

        protected override void createMaps(IMapperConfiguration cfg)
        {
            base.createMaps(cfg);
            cfg.CreateMap<Recipe, RecipeCoverDto>()
                .ForMember(x => x.ID, y => y.MapFrom(r => r.RecipeID))
                .ForMember(x => x.ImageURL, y => y.MapFrom(r => r.RecipeImages.OrderByDescending(ri => ri.Sequence).Select(i => i.ImageURL).FirstOrDefault()))
                .ForMember(x => x.Ingredients, y => y.MapFrom(r => r.RecipeIngredientMeasurements.Select(rim =>
                    rim.IngredientMeasurement.Ingredient.IngredientName)))
                ;

            var mapping = Mappings[typeof(RecipeIngredientMeasurement)] as IMappingExpression<RecipeIngredientMeasurement, RecipeIngredientMeasurementDto>;
            mapping.ForMember(ri => ri.Alternates, c => c.MapFrom(ri => from ria in ri.IngredientMeasurement.FromIngredientMeasurementAlternates
                                                                        select ria));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IngredientMeasurementAlternate>()
				.HasRequired(ima => ima.FromIngredientMeasurement)
				.WithMany(im => im.FromIngredientMeasurementAlternates)
				.HasForeignKey(ima => ima.FromIngredientMeasurementID);

			modelBuilder.Entity<IngredientMeasurementAlternate>()
				.HasRequired(ima => ima.ToIngredientMeasurement)
				.WithMany(im => im.ToIngredientMeasurementAlternates)
				.HasForeignKey(ima => ima.ToIngredientMeasurementID);
		}
	}
}
