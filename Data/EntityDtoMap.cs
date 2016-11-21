using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Data
{
	public class EntityDtoMap
	{
		public Type EntityType { get; set; }
		public Type DtoType { get; set; }
		public object MappingExpression { get; set; }

		public EntityDtoMap(Type entityType, Type dtoType, object mappingExpression)
		{
			this.EntityType = entityType;
			this.DtoType = dtoType;
			this.MappingExpression = mappingExpression;
		}
	}
}
