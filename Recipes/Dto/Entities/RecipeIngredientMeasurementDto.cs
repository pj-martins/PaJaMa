using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using PaJaMa.Recipes.Dto.Entities.Base;

namespace PaJaMa.Recipes.Dto.Entities
{
	public class RecipeIngredientMeasurementDto : RecipeIngredientMeasurementDtoBase
    {
		public virtual IngredientMeasurementDto IngredientMeasurement { get; set; }
		public ICollection<IngredientMeasurementAlternateDto> Alternates { get; set; }
    }
}
