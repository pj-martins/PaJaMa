using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare
{
	public class DataTableWithSchema : DataTable
	{
		public List<string> PrimaryKeyFields { get; set; }
	}
}
