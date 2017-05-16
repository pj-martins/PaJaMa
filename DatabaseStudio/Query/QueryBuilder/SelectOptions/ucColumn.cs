using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Query.QueryBuilder.SelectOptions
{
	public partial class ucColumn : UserControl
	{
		public ucColumn(List<string> columns)
		{
			InitializeComponent();
			SetColumns(columns);
		}

		public void SetColumns(List<string> columns)
		{
			cboColumn.Items.Clear();
			cboColumn.Items.Add("<All>");
			if (columns != null)
				cboColumn.Items.AddRange(columns.ToArray());
		}
	}
}
