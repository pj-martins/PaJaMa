using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMaData
{
	public class EntityAttribute : Attribute
	{
		public string KeyFieldName { get; set; }
		public EntityAttribute()
		{
		}
	}
}
