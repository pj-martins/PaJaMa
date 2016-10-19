using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Data
{
	public class ValidationError
	{
		public string Error { get; private set; }
		public IEntity Entity { get; private set; }

		public ValidationError(string error, IEntity entity)
		{
			Error = error;
			Entity = entity;
		}
	}
}
