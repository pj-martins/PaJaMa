using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("RecipeIngredientMeasurement")]
	public abstract class RecipeIngredientMeasurementBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return RecipeIngredientMeasurementID; } set { RecipeIngredientMeasurementID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 RecipeIngredientMeasurementID { get; set; }
		[Column]
		public virtual System.Int32 RecipeID { get; set; }
		[Column]
		public virtual System.Int32 IngredientMeasurementID { get; set; }
		[Column]
		public virtual System.Single? Quantity { get; set; }
		[Column]
		public virtual System.Boolean IsOptional { get; set; }

		public virtual IngredientMeasurement IngredientMeasurement { get; set; }

		public virtual Recipe Recipe { get; set; }

	}
}
