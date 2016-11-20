using PaJaMa.Recipes.Dto.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Dto.Entities
{
	public class IngredientMeasurementDto : IngredientMeasurementDtoBase
	{
		public virtual IngredientDto Ingredient { get; set; }
		public virtual MeasurementDto Measurement { get; set; }
	}
}
