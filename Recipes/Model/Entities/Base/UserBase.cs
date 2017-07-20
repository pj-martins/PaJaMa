using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("User")]
	public abstract class UserBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return UserID; } set { UserID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 UserID { get; set; }
		[Column]
		public virtual System.String UserName { get; set; }
		[Column]
		public virtual System.String Password { get; set; }

		public virtual System.Collections.Generic.ICollection<UserRecipe> UserRecipes { get; set; }

	}
}
