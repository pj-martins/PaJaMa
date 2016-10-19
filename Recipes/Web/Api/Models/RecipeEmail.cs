using PaJaMa.Recipes.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaJaMa.Recipes.API.Models
{
	public class RecipeEmail
	{
		public RecipeDto Recipe { get; set; }
		public string To { get; set; }
		public string Subject { get; set; }
		public string Message { get; set; }
		public string ActiveImageURL { get; set; }
	}
}