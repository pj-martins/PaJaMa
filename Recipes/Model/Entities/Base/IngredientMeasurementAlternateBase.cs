using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("IngredientMeasurementAlternate")]
	public abstract class IngredientMeasurementAlternateBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return IngredientMeasurementAlternateID; } set { IngredientMeasurementAlternateID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 IngredientMeasurementAlternateID { get; set; }
		[Column]
		public virtual System.Int32 FromIngredientMeasurementID { get; set; }
		[Column]
		public virtual System.Int32 ToIngredientMeasurementID { get; set; }
		[Column]
		public virtual System.Single? Multiplier { get; set; }

		public virtual IngredientMeasurement FromIngredientMeasurement { get; set; }

		public virtual IngredientMeasurement ToIngredientMeasurement { get; set; }

	}
}
