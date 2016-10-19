using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Model.Dto
{
	public class RecipeCoverDto : PaJaMa.Data.EntityDtoBase
	{
		public string RecipeName { get; set; }
		public float? Rating { get; set; }
		public ICollection<string> Ingredients { get; set; }
		public string ImageURL { get; set; }
		public string RecipeURL { get; set; }
		public int? RecipeSourceID { get; set; }
	}
}
