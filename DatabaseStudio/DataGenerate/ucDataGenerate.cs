using PaJaMa.Common;
using PaJaMa.DatabaseStudio.Classes;
using PaJaMa.DatabaseStudio.DataGenerate.Classes;
using PaJaMa.DatabaseStudio.DataGenerate.Content;
using PaJaMa.DatabaseStudio.DataGenerate.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.DataGenerate
{
	public partial class ucDataGenerate : UserControl
	{
		private GeneratorHelper _generatorHelper;

		private bool _lockDbChange = false;

		public event QueryEventHandler QueryDatabase;

		public ucDataGenerate()
		{
			InitializeComponent();

			if (Properties.Settings.Default.ConnectionStrings == null)
				Properties.Settings.Default.ConnectionStrings = string.Empty;

			refreshConnStrings();

			if (!string.IsNullOrEmpty(Properties.Settings.Default.LastQueryConnectionString))
				cboConnectionString.Text = Properties.Settings.Default.LastQueryConnectionString;

			new GridHelper().DecorateGrid(gridTables);
		}

		private void refreshConnStrings()
		{
			var conns = Properties.Settings.Default.ConnectionStrings.Split('|');
			cboConnectionString.Items.Clear();
			cboConnectionString.Items.AddRange(conns.OrderBy(c => c).ToArray());
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{

			string connString = cboConnectionString.Text;

			Exception exception = null;

			DataTable schema = null;
			string database = string.Empty;

			var worker = new BackgroundWorker();
			worker.DoWork += delegate(object sender2, DoWorkEventArgs e2)
			{
				try
				{
					using (var conn = new SqlConnection(connString))
					{
						conn.Open();
						schema = conn.GetSchema("Databases");
						database = conn.Database;
						conn.Close();
						SqlConnection.ClearPool(conn);
					}
				}
				catch (Exception ex)
				{
					exception = new Exception("Error opening source connection: " + ex.Message);
					return;
				}

				_generatorHelper = new GeneratorHelper(connString, worker);
			};

			WinControls.WinProgressBox.ShowProgress(worker, progressBarStyle: ProgressBarStyle.Marquee);
			if (exception != null)
				MessageBox.Show(exception.Message);
			else
			{
				refreshPage(false);

				List<string> connStrings = Properties.Settings.Default.ConnectionStrings.Split('|').ToList();
				if (!connStrings.Any(s => s == cboConnectionString.Text))
					connStrings.Add(cboConnectionString.Text);

				Properties.Settings.Default.ConnectionStrings = string.Join("|", connStrings.ToArray());
				Properties.Settings.Default.LastQueryConnectionString = cboConnectionString.Text;
				Properties.Settings.Default.Save();

				_lockDbChange = true;
				cboDatabase.Items.Clear();
				foreach (var dr in schema.Rows.OfType<DataRow>())
				{
					cboDatabase.Items.Add(dr["database_name"].ToString());
				}
				cboDatabase.Text = database;

				_lockDbChange = false;

				btnConnect.Visible = btnRemoveConnString.Visible = false;
				btnDisconnect.Visible = true;
				cboConnectionString.SelectionLength = 0;
				cboConnectionString.Enabled = false;
				cboDatabase.Visible = true;
				btnGo.Enabled = btnRefresh.Enabled = btnViewMissingDependencies.Enabled = btnAdd10.Enabled = btnAddNRows.Enabled = btnQuery.Enabled = true;
			}
		}

		private void refreshPage(bool reinit)
		{
			if (reinit)
			{
				var worker = new BackgroundWorker();
				worker.DoWork += delegate(object sender2, DoWorkEventArgs e2)
				{
					_generatorHelper.Init(worker);
				};
				WinControls.WinProgressBox.ShowProgress(worker, progressBarStyle: ProgressBarStyle.Marquee);
			}

			refreshTables();
		}

		private void refreshTables()
		{
			var lst = TableWorkspaceList.GetTableWorkspaces(_generatorHelper);
			gridTables.AutoGenerateColumns = false;
			gridTables.DataSource = new BindingList<TableWorkspace>(lst.Workspaces.OrderBy(w => w.Table.ToString()).ToList());
		}

		private StringBuilder getMissingDependencyString()
		{
			var sb = new StringBuilder();
			var workspaces = gridTables.Rows.OfType<DataGridViewRow>().Select(r => (r.DataBoundItem as TableWorkspace))
				.Where(tw => tw.AddRowCount > 0 || tw.CurrentRowCount > 0)
				.ToList();

			var missingDependencies = _generatorHelper.GetMissingDependencies(workspaces);
			if (missingDependencies.Any())
			{
				foreach (var kvp in missingDependencies)
				{
					sb.AppendLine(kvp.Key.TableName + " is dependent on: " + string.Join(", ", kvp.Value.Select(v => v.TableName).ToArray()));
				}
			}

			return sb;
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			var sb = getMissingDependencyString();

			if (sb.Length > 0)
			{
				MessageBox.Show(sb.ToString(), "Dependencies");
				return;
			}


			var workspaces = getSortedWorkspaces((gridTables.DataSource as BindingList<TableWorkspace>)
				.Where(t => t.AddRowCount > 0 || t.Truncate || t.Delete).ToList());

			var changes = workspaces.Select(t => t.Table.TableName).ToList();

			if (changes.Count() > 15)
			{
				changes = changes.Take(15).ToList();
				changes.Add("...");
			}

			if (MessageBox.Show(string.Format("{0} - {1} will be populated:\r\n\r\n{2}\r\n\r\nContinue?", _generatorHelper.Database.DataSource,
					_generatorHelper.Database.DatabaseName,
				string.Join("\r\n", changes.ToArray())), "Proceed", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;


			if (!workspaces.Any())
			{
				MessageBox.Show("Nothing to populate.");
				return;
			}

			bool success = false;
			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += delegate(object sender2, DoWorkEventArgs e2)
			{
				if (workspaces.Any() && !_generatorHelper.Generate(worker, workspaces))
					return;

				success = true;
			};

			PaJaMa.WinControls.WinProgressBox.ShowProgress(worker, allowCancel: true);

			if (success)
			{
				MessageBox.Show("Done");
				refreshPage(true);
			}
		}

		private List<TableWorkspace> getSortedWorkspaces(List<TableWorkspace> workspaces)
		{
			var sortedWorkspaces = new List<TableWorkspace>();

			// make copy
			var currentWorkspaces = workspaces.ToList();

			while (currentWorkspaces.Count > 0)
			{
				foreach (var ws in currentWorkspaces)
				{
					var tempSelected = sortedWorkspaces.ToList();
					tempSelected.Add(ws);

					var missing = _generatorHelper.GetMissingDependencies(tempSelected);
					if (!missing.Any(n => n.Value.Any(v => currentWorkspaces.Any(cw => cw.Table.TableName == v.TableName))))
					{
						sortedWorkspaces.Add(ws);
						currentWorkspaces.Remove(ws);
						break;
					}
				}
			}

			return sortedWorkspaces;
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			gridTables.DataSource = null;
			btnDisconnect.Visible = false;
			btnConnect.Visible = true;
			cboConnectionString.Enabled = true;
			btnRemoveConnString.Visible = true;
			cboDatabase.Visible = false;
			btnGo.Enabled = btnViewMissingDependencies.Enabled = btnRefresh.Enabled = btnAdd10.Enabled = btnAddNRows.Enabled = btnQuery.Enabled = false;
		}

		private void cboDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lockDbChange) return;
			_generatorHelper.Database.ChangeDatabase(cboDatabase.Text);
			refreshPage(true);
		}

		private void btnRemoveConnString_Click(object sender, EventArgs e)
		{
			removeConnString(cboConnectionString.Text, true);
		}

		private void removeConnString(string connString, bool source)
		{
			List<string> connStrings = Properties.Settings.Default.ConnectionStrings.Split('|').ToList();
			connStrings.Remove(connString);
			Properties.Settings.Default.ConnectionStrings = string.Join("|", connStrings.ToArray());
			Properties.Settings.Default.LastQueryConnectionString = string.Empty;
			Properties.Settings.Default.Save();
			refreshConnStrings();
			cboConnectionString.Text = string.Empty;
		}

		private void cboConnString_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnRemoveConnString.Enabled = !string.IsNullOrEmpty(cboConnectionString.Text);
		}

		private void gridTables_MouseClick(object sender, MouseEventArgs e)
		{
			//if (e.Button == System.Windows.Forms.MouseButtons.Right)
			//	_tablesMenu.Show(gridTables, new Point(e.X, e.Y));
		}

		private void gridObjects_MouseClick(object sender, MouseEventArgs e)
		{
			//if (e.Button == System.Windows.Forms.MouseButtons.Right)
			//	_objectsMenu.Show(gridObjects, new Point(e.X, e.Y));
		}

		private void gridTables_MouseDown(object sender, MouseEventArgs e)
		{

		}

		private void gridTables_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{

		}

		private void gridDataColumns_DataError(object sender, DataGridViewDataErrorEventArgs e)
		{

		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			refreshPage(true);
		}

		private void btnViewMissingDependencies_Click(object sender, EventArgs e)
		{
			var sb = getMissingDependencyString();

			if (sb.Length > 0)
				MessageBox.Show(sb.ToString());
			else
				MessageBox.Show("No missing dependencies!");
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			foreach (var row in gridTables.SelectedRows.OfType<DataGridViewRow>())
			{
				var tableWorkspace = row.DataBoundItem as TableWorkspace;
				tableWorkspace.AddRowCount += 10;
			}
			gridTables.Invalidate();
		}

		private void gridTables_SelectionChanged(object sender, EventArgs e)
		{
			gridColumns.DataSource = null;
			if (gridTables.SelectedRows.Count != 1) return;

			var tableWorkspace = gridTables.SelectedRows[0].DataBoundItem as TableWorkspace;
			gridColumns.AutoGenerateColumns = false;
			gridColumns.DataSource = tableWorkspace.ColumnWorkspaces;
		}

		private void gridColumns_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == Content.Index)
			{
				var row = gridColumns.Rows[e.RowIndex].DataBoundItem as ColumnWorkspace;
				if (row.Content == null) return;
				row.Content.ShowPropertiesControl();
			}
		}

		private void btnQuery_Click(object sender, EventArgs e)
		{
			query(null);
		}

		private void selectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			query(0);
		}

		private void selectTop1000ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			query(1000);
		}

		private void query(int? topN = null)
		{
			if (_generatorHelper.Database != null)
			{
				var args = new QueryEventArgs();
				args.Database = _generatorHelper.Database;
				if (topN != null)
				{
					var selectedItem = gridTables.SelectedRows[0].DataBoundItem as TableWorkspace;
					args.InitialTopN = topN;
					args.InitialTable = selectedItem.Table.TableName;
					args.InitialSchema = selectedItem.Table.Schema.SchemaName;
				}
				QueryDatabase(this, args);
			}
		}

		private void btnAddNRows_Click(object sender, EventArgs e)
		{
			using (var dlg = new dlgAddNRows())
			{
				if (dlg.ShowDialog() == DialogResult.OK)
				{
					foreach (var row in gridTables.SelectedRows.OfType<DataGridViewRow>())
					{
						var tableWorkspace = row.DataBoundItem as TableWorkspace;
						tableWorkspace.AddRowCount += (int)dlg.NumberRows.Value;
					}
					gridTables.Invalidate();
				}
			}
		}
	}
}