using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("UserRecipe")]
	public abstract class UserRecipeBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return UserRecipeID; } set { UserRecipeID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 UserRecipeID { get; set; }
		[Column]
		public virtual System.Int32 UserID { get; set; }
		[Column]
		public virtual System.Int32 RecipeID { get; set; }
		[Column]
		public virtual System.Boolean IsBookmarked { get; set; }
		[Column]
		public virtual System.Single? Rating { get; set; }
		[Column]
		public virtual System.String Notes { get; set; }
		[Column]
		public virtual System.Boolean AllowEdit { get; set; }

		public virtual Recipe Recipe { get; set; }

		public virtual User User { get; set; }

	}
}
