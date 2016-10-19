using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Recipes.Model.Dto
{
	public class Base64ImageDto
	{
		public string ImageURL { get; set; }
		public string Base64String { get; set; }
		public string ContentType { get; set; }
	}
}
