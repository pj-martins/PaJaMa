using PaJaMa.Recipes.Model.Dto.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Model.Dto
{
	public class RecipeDto : RecipeDtoBase
	{
		public virtual RecipeSourceDto RecipeSource { get; set; }
		public virtual ICollection<RecipeImageDto> RecipeImages { get; set; }
		public virtual ICollection<RecipeIngredientMeasurementDto> RecipeIngredientMeasurements { get; set; }
		//public float? UserRating { get; set; }
		//[DataMember]
		//public bool IsBookmarked { get; set; }
		//[DataMember]
		//public string Notes { get; set; }
		//[DataMember]
		//public bool IsDirty { get; set; }

		//[DataMember]
		//public float TotalCalories { get; set; }
		//[DataMember]
		//public float TotalCarbohydrates { get; set; }
		//[DataMember]
		//public float TotalFat { get; set; }
		//[DataMember]
		//public float TotalSaturatedFat { get; set; }
		//[DataMember]
		//public float TotalProtein { get; set; }
		//[DataMember]
		//public float TotalSugars { get; set; }
	}
}
