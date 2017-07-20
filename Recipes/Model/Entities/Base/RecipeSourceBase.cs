using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("RecipeSource")]
	public abstract class RecipeSourceBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return RecipeSourceID; } set { RecipeSourceID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 RecipeSourceID { get; set; }
		[Column]
		public virtual System.String RecipeSourceName { get; set; }

		public virtual System.Collections.Generic.ICollection<Recipe> Recipes { get; set; }

	}
}
