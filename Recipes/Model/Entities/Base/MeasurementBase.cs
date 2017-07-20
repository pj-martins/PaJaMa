using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("Measurement")]
	public abstract class MeasurementBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return MeasurementID; } set { MeasurementID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 MeasurementID { get; set; }
		[Column]
		public virtual System.String MeasurementName { get; set; }

		public virtual System.Collections.Generic.ICollection<IngredientMeasurement> IngredientMeasurements { get; set; }

	}
}
