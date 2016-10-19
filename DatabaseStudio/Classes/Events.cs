using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Classes
{
	public class QueryEventArgs : EventArgs
	{
		public IDatabase Database { get; set; }
		public string InitialTable { get; set; }
		public string InitialSchema { get; set; }
		public int? InitialTopN { get; set; }
	}

	public delegate void QueryEventHandler(object sender, QueryEventArgs e);
}
