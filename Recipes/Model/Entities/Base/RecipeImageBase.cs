using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("RecipeImage")]
	public abstract class RecipeImageBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return RecipeImageID; } set { RecipeImageID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 RecipeImageID { get; set; }
		[Column]
		public virtual System.Int32 RecipeID { get; set; }
		[Column]
		public virtual System.String ImageURL { get; set; }
		[Column]
		public virtual System.String LocalImagePath { get; set; }
		[Column]
		public virtual System.Int32 Sequence { get; set; }

		public virtual Recipe Recipe { get; set; }

	}
}
