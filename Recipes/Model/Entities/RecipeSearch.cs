using PaJaMa.Recipes.Model.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Model.Entities
{
	public class RecipeSearch : RecipeSearchBase
	{
		// expose additional attributes
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public override int RecipeID
		{
			get { return base.RecipeID; }
			set { base.RecipeID = value; }
		}
	}
}
