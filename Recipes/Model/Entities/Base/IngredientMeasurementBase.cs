using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("IngredientMeasurement")]
	public abstract class IngredientMeasurementBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return IngredientMeasurementID; } set { IngredientMeasurementID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 IngredientMeasurementID { get; set; }
		[Column]
		public virtual System.Int32 IngredientID { get; set; }
		[Column]
		public virtual System.Int32? MeasurementID { get; set; }
		[Column]
		public virtual System.Single? CaloriesPer { get; set; }
		[Column]
		public virtual System.Single? CarbohydratesPer { get; set; }
		[Column]
		public virtual System.Single? FatPer { get; set; }
		[Column]
		public virtual System.Single? SaturatedFatPer { get; set; }
		[Column]
		public virtual System.Single? SugarsPer { get; set; }
		[Column]
		public virtual System.Single? ProteinPer { get; set; }

		public virtual Ingredient Ingredient { get; set; }

		public virtual Measurement Measurement { get; set; }


		public virtual System.Collections.Generic.ICollection<IngredientMeasurementAlternate> IngredientMeasurementAlternates { get; set; }

		public virtual System.Collections.Generic.ICollection<IngredientMeasurementAlternate> IngredientMeasurementAlternates1 { get; set; }

		public virtual System.Collections.Generic.ICollection<RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }

	}
}
