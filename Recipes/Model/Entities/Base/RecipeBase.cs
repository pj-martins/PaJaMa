using PaJaMa.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PaJaMa.Recipes.Model.Entities.Base
{
	[Table("Recipe")]
	public abstract class RecipeBase : EntityBase
	{
		[NotMapped]
		public override int ID { get { return RecipeID; } set { RecipeID = value; } }

		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		[Column]
		public virtual System.Int32 RecipeID { get; set; }
		[Column]
		public virtual System.String RecipeName { get; set; }
		[Column]
		public virtual System.String Directions { get; set; }
		[Column]
		public virtual System.Int32? NumberOfServings { get; set; }
		[Column]
		public virtual System.Single? Rating { get; set; }
		[Column]
		public virtual System.Boolean Inactive { get; set; }
		[Column]
		public virtual System.String RecipeURL { get; set; }
		[Column]
		public virtual System.Int32? RecipeSourceID { get; set; }
		[Column]
		public virtual RecipeType RecipeType { get; set; }
		[NotMapped]
		public virtual string RecipeTypeString { get { return PaJaMa.Common.EnumHelper.GetEnumDisplay(RecipeType); } }

		public virtual RecipeSource RecipeSource { get; set; }


		public virtual System.Collections.Generic.ICollection<RecipeImage> RecipeImages { get; set; }

		public virtual System.Collections.Generic.ICollection<RecipeIngredientMeasurement> RecipeIngredientMeasurements { get; set; }

		public virtual System.Collections.Generic.ICollection<UserRecipe> UserRecipes { get; set; }

	}
}
