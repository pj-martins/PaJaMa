using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.Data
{
	public class OperationResult
	{
		public bool Failed { get; set; }
		public Exception Exception { get; set; }
		public IEntity Entity { get; set; }

		public string GetExceptionText()
		{
			return Exception == null ? string.Empty : Exception.GetFullExceptionText();
		}
	}
}
