using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using PaJaMa.DatabaseStudio.DataGenerate.Content;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Search.Classes
{
	public class ColumnWorkspace
	{
		public Column Column { get; private set; }
		public bool Select { get; set; }

		public ColumnWorkspace(Column column)
		{
			Column = column;
		}
	}
}
