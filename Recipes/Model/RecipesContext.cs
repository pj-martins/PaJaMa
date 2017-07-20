using PaJaMa.Common;
using PaJaMa.Data;
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

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<IngredientMeasurementAlternate>()
				.HasRequired(ima => ima.FromIngredientMeasurement)
				.WithMany(im => im.IngredientMeasurementAlternates)
				.HasForeignKey(ima => ima.FromIngredientMeasurementID);

			modelBuilder.Entity<IngredientMeasurementAlternate>()
				.HasRequired(ima => ima.ToIngredientMeasurement)
				.WithMany(im => im.IngredientMeasurementAlternates1)
				.HasForeignKey(ima => ima.ToIngredientMeasurementID);
		}
	}
}
