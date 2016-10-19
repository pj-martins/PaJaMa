using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using PaJaMa.Recipes.Model.Dto.Base;

namespace PaJaMa.Recipes.Model.Dto
{
	public class RecipeIngredientMeasurementDto : RecipeIngredientMeasurementDtoBase
    {
		public virtual IngredientMeasurementDto IngredientMeasurement { get; set; }
		public ICollection<IngredientMeasurementAlternateDto> Alternates { get; set; }
    }
}
