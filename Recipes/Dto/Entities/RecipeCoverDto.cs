using PaJaMa.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Dto.Entities
{
    // flatter version for smaller json payload
	public class RecipeCoverDto : EntityDtoBase
	{
		public string RecipeName { get; set; }
		public float? Rating { get; set; }
		public ICollection<string> Ingredients { get; set; }
		public ICollection<string> ImageURLs { get; set; }
		public string RecipeURL { get; set; }
		public int? RecipeSourceID { get; set; }
	}
}
