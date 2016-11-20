using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaJaMa.Common;
using PaJaMa.Data;

namespace PaJaMa.Dto
{
	public class OperationResult
	{
		public bool Failed { get; set; }
		public Exception Exception { get; set; }
		public IEntity Entity { get; set; }
		public IEntityDto EntityDto { get; set; }

		public string GetExceptionText()
		{
			return Exception == null ? string.Empty : Exception.GetFullExceptionText();
		}
	}
}
