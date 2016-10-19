using PaJaMa.DatabaseStudio.Classes;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare
{
	public partial class frmDataDetails : Form
	{
		public TableWorkspace SelectedWorkspace { get; set; }

		private int _currentPage = 1;
		private int _pageSize = 100;
		private bool _lock = false;

		public frmDataDetails()
		{
			InitializeComponent();
		}

		private void frmDataDetails_Load(object sender, EventArgs e)
		{
			new GridHelper().DecorateGrid(gridMain);

			var settings = Properties.Settings.Default;

			this.DesktopLocation = new Point(settings.DataDetailsLeft, settings.DataDetailsTop);
			if (settings.DataDetailsMaximized)
				this.WindowState = FormWindowState.Maximized;
			else
			{
				//if (settings.DataDetailsLeft > 0)
				//	this.Left = settings.DataDetailsLeft;
				//if (settings.DataDetailsTop > 0)
				//	this.Top = settings.DataDetailsTop;
				if (settings.DataDetailsHeight > 0)
					this.Height = settings.DataDetailsHeight;
				if (settings.DataDetailsWidth > 0)
					this.Width = settings.DataDetailsWidth;
			}

			this.Text = SelectedWorkspace.SourceTable.TableName + ": " + SelectedWorkspace.SourceTable.Schema.Database.DataSource + " - " +
					SelectedWorkspace.SourceTable.Schema.Database.DatabaseName + " <> " +
					SelectedWorkspace.TargetTable.Schema.Database.DataSource + " - " +
					SelectedWorkspace.TargetTable.Schema.Database.DatabaseName;

			cboOverrideKeyField.Items.Add("");
			cboOverrideKeyField.Items.AddRange(SelectedWorkspace.SourceTable.Columns.Select(c => c.ColumnName).ToArray());

			refreshGrid(false);
		}

		private void refreshGrid(bool resetData)
		{
			if (resetData)
				SelectedWorkspace.ComparedData = null;

			gridMain.Columns.Clear();

			string overrideKeyField = null;
			if (!string.IsNullOrEmpty(cboOverrideKeyField.Text))
				overrideKeyField = cboOverrideKeyField.Text;

			var dataHelper = new DataHelper();
			var bw = new BackgroundWorker();
			bw.DoWork += delegate(object sender2, DoWorkEventArgs e2)
			{
				dataHelper.CompareData(SelectedWorkspace, bw, true, overrideKeyField);
			};

			var prog = new PaJaMa.WinControls.WinProgressBox();
			prog.Cancel += delegate(object sender2, EventArgs e2)
			{
				dataHelper.Cancel();
			};

			prog.Show(bw, "Populating data...", allowCancel: true, progressBarStyle: ProgressBarStyle.Marquee);

			if (SelectedWorkspace.ComparedData == null)
				return;

			setData();

			//gridMain.DataSource = SelectedWorkspace.ComparedData;

			foreach (DataGridViewColumn col in gridMain.Columns)
			{
				col.ReadOnly = true;
			}

			var selectCol = new DataGridViewCheckBoxColumn();
			selectCol.Name = "Select";
			gridMain.Columns.Insert(0, selectCol);
		}

		private void setData()
		{
			if (_currentPage < 1) _currentPage = 1;
			int totalPages = (SelectedWorkspace.ComparedData.Rows.Count / _pageSize) + 1;
			if (_currentPage > totalPages) _currentPage = totalPages;

			_lock = true;
			numPage.Value = _currentPage;
			_lock = false;
			lblTotalPages.Text = totalPages.ToString();

			var dt = SelectedWorkspace.ComparedData.Clone() as DataTableWithSchema;
			dt.PrimaryKeyFields = SelectedWorkspace.ComparedData.PrimaryKeyFields;

			var sortedProp = gridMain.SortedColumn == null ? string.Empty : gridMain.SortedColumn.DataPropertyName;
			var sortOrder = gridMain.SortOrder;

			var rows = SelectedWorkspace.ComparedData.Rows.OfType<DataRow>();
			if (!chkAll.Checked)
				rows = rows.Where(dr =>
					(chkLeft.Checked && dr[DataHelper.FROM_PREFIX + dt.PrimaryKeyFields[0]].Equals(DBNull.Value)) ||
					(chkRight.Checked && dr[DataHelper.TO_PREFIX + dt.PrimaryKeyFields[0]].Equals(DBNull.Value)) ||
					(chkDifferent.Checked && !dr[DataHelper.FROM_PREFIX + dt.PrimaryKeyFields[0]].Equals(DBNull.Value) &&
						!dr[DataHelper.TO_PREFIX + dt.PrimaryKeyFields[0]].Equals(DBNull.Value) &&
						dr.Table.Columns.OfType<DataColumn>().Any(dc => !
						dr[DataHelper.FROM_PREFIX + dc.ColumnName.Substring(2)].Equals(dr[DataHelper.TO_PREFIX + dc.ColumnName.Substring(2)])))
					);

			var take = rows
				.OrderBy(dr => gridMain.SortedColumn == null ? 0 : dr[gridMain.SortedColumn.DataPropertyName])
				.Skip((_currentPage - 1) * _pageSize)
				.Take(_pageSize);

			foreach (var dr in take)
			{
				dt.ImportRow(dr);
			}

			gridMain.DataSource = dt;

			_lock = true;
			if (!string.IsNullOrEmpty(sortedProp))
				gridMain.Sort(gridMain.Columns.OfType<DataGridViewColumn>().First(c => c.DataPropertyName == sortedProp), sortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);
			_lock = false;
		}

		private void gridMain_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			var dt = gridMain.DataSource as DataTableWithSchema;
			var row = gridMain.Rows[e.RowIndex];

			if (row.Cells[DataHelper.FROM_PREFIX + dt.PrimaryKeyFields[0]].Value.Equals(DBNull.Value))
			{
				e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.LightSalmon;
				return;
			}

			if (row.Cells[DataHelper.TO_PREFIX + dt.PrimaryKeyFields[0]].Value.Equals(DBNull.Value))
			{
				e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.LightSkyBlue;
				return;
			}

			var dr = row.DataBoundItem as DataRowView;
			var col = gridMain.Columns[e.ColumnIndex];
			if (string.IsNullOrEmpty(col.DataPropertyName)) return;

			var fldName = col.DataPropertyName.Substring(2);
			if (!dr[DataHelper.FROM_PREFIX + fldName].Equals(dr[DataHelper.TO_PREFIX + fldName]))
			{
				row.DefaultCellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.LightGoldenrodYellow;
				e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.LightGreen;
				return;
			}
			else if (e.Value == DBNull.Value)
			{
				e.CellStyle.BackColor = Color.LightYellow;
				e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);
				e.Value = "NULL";
			}
			//else
			//{
			//	bool hasDiff = false;
			//	foreach (DataGridViewColumn c in gridMain.Columns)
			//	{
			//		if (string.IsNullOrEmpty(c.DataPropertyName)) continue;

			//		if (!dr[DataHelper.FROM_PREFIX + c.DataPropertyName.Substring(2)].Equals(dr[DataHelper.TO_PREFIX + c.DataPropertyName.Substring(2)]))
			//		{
			//			hasDiff = true;
			//			break;
			//		}
			//	}

			//	if (hasDiff)
			//	{
			//		e.CellStyle.BackColor = e.CellStyle.SelectionBackColor = Color.LightGoldenrodYellow;
			//	}
			//}
		}

		private void frmDataDetails_FormClosing(object sender, FormClosingEventArgs e)
		{
			var settings = Properties.Settings.Default;
			settings.DataDetailsLeft = this.DesktopLocation.X;
			settings.DataDetailsTop = this.DesktopLocation.Y;
			if (this.WindowState == FormWindowState.Maximized)
				settings.DataDetailsMaximized = true;
			else
			{
				settings.DataDetailsMaximized = false;
				settings.DataDetailsWidth = this.Width;
				settings.DataDetailsHeight = this.Height;
			}
			settings.Save();
		}

		private void btnSync_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(string.Format("{0} - {1} will be changed:\r\n\r\nContinue?", SelectedWorkspace.TargetTable.Schema.Database.DataSource,
					SelectedWorkspace.TargetTable.Schema.Database.DatabaseName), "Proceed", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;

			List<DataRow> drs = new List<DataRow>();
			foreach (DataGridViewRow row in gridMain.Rows)
			{
				if (row.Cells["Select"].Value != null && (bool)row.Cells["Select"].Value)
					drs.Add((row.DataBoundItem as DataRowView).Row);
			}
			try
			{
				new DataHelper().SynchronizeRows(SelectedWorkspace, drs.ToArray());
			}
			catch (Exception ex)
			{
				MessageBox.Show("ERROR! " + ex.Message);
				return;
			}

			MessageBox.Show("Done!");
			refreshGrid(true);
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			refreshGrid(true);
		}

		private void btnPrevious_Click(object sender, EventArgs e)
		{
			_currentPage--;
			setData();
		}

		private void btnFirst_Click(object sender, EventArgs e)
		{
			_currentPage = 1;
			setData();
		}

		private void btnNext_Click(object sender, EventArgs e)
		{
			_currentPage++;
			setData();
		}

		private void btnLast_Click(object sender, EventArgs e)
		{
			_currentPage = (SelectedWorkspace.ComparedData.Rows.Count / _pageSize) + 1;
			setData();
		}

		private void gridMain_Sorted(object sender, EventArgs e)
		{
			if (_lock) return;
			setData();
		}

		private void numPage_ValueChanged(object sender, EventArgs e)
		{
			if (_lock) return;
			_currentPage = (int)numPage.Value;
			setData();
		}

		private void chk_CheckedChanged(object sender, EventArgs e)
		{
			if (_lock) return;
			_lock = true;
			if (chkLeft.Checked || chkRight.Checked || chkDifferent.Checked)
				chkAll.Checked = false;
			else if (!(chkLeft.Checked || chkRight.Checked || chkDifferent.Checked))
				chkAll.Checked = true;
			_lock = false;
			setData();
		}

		private void chkAll_CheckedChanged(object sender, EventArgs e)
		{
			if (_lock) return;
			_lock = true;
			if (!chkAll.Checked)
			{
				chkAll.Checked = true;
				_lock = false;
				return;
			}
			else
				chkLeft.Checked = chkRight.Checked = chkDifferent.Checked = false;
			
			_lock = false;
			setData();
		}

		private void cboOverrideKeyField_SelectedIndexChanged(object sender, EventArgs e)
		{
			refreshGrid(true);
		}
	}
}
