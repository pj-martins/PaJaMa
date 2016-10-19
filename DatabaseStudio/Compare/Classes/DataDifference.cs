using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Classes
{
	public class DataDifference
	{
		public TableWorkspace TableWorkspace { get; set; }
		public int SourceOnly { get; set; }
		public int TargetOnly { get; set; }
		public int Differences { get; set; }
	}
}
