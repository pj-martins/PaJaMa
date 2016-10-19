using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PaJaMa.WinControls;

namespace PaJaMa.ProcessMonitor
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			refreshGrid();
		}

		private void refreshGrid()
		{
			var processes = Process.GetProcesses();
			gridMain.DataSource = new SortableBindingList<Process>(processes.OrderBy(p => p.ProcessName).ToList());

			//foreach (var col in gridMain.Columns.OfType<DataGridViewColumn>())
			//{
			//	gridMain.Columns[col.Name].SortMode = DataGridViewColumnSortMode.Automatic;
			//}
		}

		private void gridMain_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{
			gridMain.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = e.Exception.Message;
		}

		private void btnKill_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to kill selected process?", "Kill", MessageBoxButtons.YesNo)
				!= System.Windows.Forms.DialogResult.Yes)
				return;

			var bw = new BackgroundWorker();
			bw.DoWork += delegate(object sender2, DoWorkEventArgs e2)
			{
				int i = 1;
				foreach (DataGridViewRow row in gridMain.SelectedRows)
				{
					var process = row.DataBoundItem as Process;
					bw.ReportProgress(100 * i / gridMain.SelectedRows.Count, "Killing " + process.ProcessName);
					process.Kill();
				}
			};

			WinProgressBox.ShowProgress(bw);
			refreshGrid();
		}
	}
}
