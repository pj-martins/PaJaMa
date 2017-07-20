using PaJaMa.Recipes.Model.Entities;
using PaJaMa.Data;

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace PaJaMa.Recipes.Model
{
	public abstract class RecipesContextBase : DbContextBase
	{
		public RecipesContextBase() : base("name=RecipesContext")
		{
			Database.SetInitializer<RecipesContext>(null);
		}
		
		public DbSet<Entities.Recipe> Recipes { get; set; }
		public DbSet<Entities.RecipeImage> RecipeImages { get; set; }
		public DbSet<Entities.RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }
		public DbSet<Entities.IngredientMeasurement> IngredientMeasurements { get; set; }
		public DbSet<Entities.IngredientMeasurementAlternate> IngredientMeasurementAlternates { get; set; }
		public DbSet<Entities.Ingredient> Ingredients { get; set; }
		public DbSet<Entities.Measurement> Measurements { get; set; }
		public DbSet<Entities.RecipeSource> RecipeSources { get; set; }
		public DbSet<Entities.User> Users { get; set; }
		public DbSet<Entities.UserRecipe> UserRecipes { get; set; }

		protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Model.Entities.IngredientMeasurementAlternate>()
					.HasOptional(x => x.FromIngredientMeasurement)
					.WithMany(x => x.IngredientMeasurementAlternates)
					.HasForeignKey(x => x.FromIngredientMeasurementID);
					
			modelBuilder.Entity<Model.Entities.IngredientMeasurementAlternate>()
					.HasOptional(x => x.ToIngredientMeasurement)
					.WithMany(x => x.IngredientMeasurementAlternates)
					.HasForeignKey(x => x.ToIngredientMeasurementID);
					
		}
	}
}
