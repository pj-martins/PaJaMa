using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Monitor
{
	public partial class frmQueryText : Form
	{
		public frmQueryText()
		{
			InitializeComponent();
		}

		private void txtQuery_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A)
				txtQuery.SelectAll();
		}
	}
}
