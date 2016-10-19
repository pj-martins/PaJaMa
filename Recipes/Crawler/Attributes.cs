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
		public int StartPage { get; set; }
		public string StartKeyword { get; set; }
        public bool IgnoreAuto { get; set; }
		public bool UniqueRecipeName { get; set; }

		public RecipeSourceAttribute(string recipeSourceName)
		{
			StartPage = 1;
			RecipeSourceName = recipeSourceName;
		}
	}
}
