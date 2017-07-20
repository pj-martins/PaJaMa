using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
	public class RecipeSourceAttribute : Attribute
	{
		public string RecipeSourceName { get; set; }
		public bool StartsAt0 { get; set; }
        public bool IncludeNoRating { get; set; }
		public bool UniqueRecipeName { get; set; }

		public RecipeSourceAttribute(string recipeSourceName)
		{
			RecipeSourceName = recipeSourceName;
		}
	}
}
