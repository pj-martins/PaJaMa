using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("Ingredient")]
	public abstract class IngredientBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return IngredientID; } set { IngredientID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 IngredientID { get; set; }
		[Column]
		public virtual System.String IngredientName { get; set; }

		public virtual System.Collections.Generic.ICollection<IngredientMeasurement> IngredientMeasurements { get; set; }

	}
}
