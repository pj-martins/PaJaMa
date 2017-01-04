using PaJaMa.DatabaseStudio.Classes;
using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
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

namespace PaJaMa.DatabaseStudio.Compare
{
	public partial class ucCompare : UserControl
	{
		private CompareHelper _compareHelper;

		private List<TabPage> _differencedTabs = new List<TabPage>();

		private bool _lockDbChange = false;
		private WorkspaceBase _activeWorkspace;

		public event QueryEventHandler QueryDatabase;

		public ucCompare()
		{
			InitializeComponent();

			if (Properties.Settings.Default.ConnectionStrings == null)
				Properties.Settings.Default.ConnectionStrings = string.Empty;

			refreshConnStrings();

			if (!string.IsNullOrEmpty(Properties.Settings.Default.LastCompareSourceConnString))
				cboSource.Text = Properties.Settings.Default.LastCompareSourceConnString;

			if (!string.IsNullOrEmpty(Properties.Settings.Default.LastCompareTargetConnString))
				cboTarget.Text = Properties.Settings.Default.LastCompareTargetConnString;

			new GridHelper().DecorateGrid(gridTables);
			new GridHelper().DecorateGrid(gridObjects);
			new GridHelper().DecorateGrid(gridDropObjects);
		}

		private void refreshConnStrings()
		{
			var conns = Properties.Settings.Default.ConnectionStrings.Split('|');
			cboSource.Items.Clear();
			cboTarget.Items.Clear();
			cboSource.Items.AddRange(conns.OrderBy(c => c).ToArray());
			cboTarget.Items.AddRange(conns.OrderBy(c => c).ToArray());
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{

			string fromConnString = cboSource.Text;
			string toConnString = cboTarget.Text;

			Exception exception = null;

			DataTable fromSchema = null;
			DataTable toSchema = null;
			string fromDatabase = string.Empty;
			string toDatabase = string.Empty;

			var worker = new BackgroundWorker();
			worker.DoWork += delegate (object sender2, DoWorkEventArgs e2)
				{
					_differencedTabs = new List<TabPage>();
					try
					{
						using (var fromConnection = new SqlConnection(fromConnString))
						{
							fromConnection.Open();
							fromSchema = fromConnection.GetSchema("Databases");
							fromDatabase = fromConnection.Database;
							fromConnection.Close();
							SqlConnection.ClearPool(fromConnection);
						}
					}
					catch (Exception ex)
					{
						exception = new Exception("Error opening source connection: " + ex.Message);
						return;
					}

					try
					{
						using (var toConnection = new SqlConnection(toConnString))
						{
							toConnection.Open();
							toSchema = toConnection.GetSchema("Databases");
							toDatabase = toConnection.Database;
							toConnection.Close();
							SqlConnection.ClearPool(toConnection);
						}
					}
					catch (Exception ex)
					{
						exception = new Exception("Error opening target connection: " + ex.Message);
						return;
					}

					_compareHelper = new CompareHelper(fromConnString, toConnString, worker);
				};

			WinControls.WinProgressBox.ShowProgress(worker, progressBarStyle: ProgressBarStyle.Marquee);
			if (exception != null)
				MessageBox.Show(exception.Message);
			else
			{
				refreshPage(false);

				List<string> connStrings = Properties.Settings.Default.ConnectionStrings.Split('|').ToList();
				if (!connStrings.Any(s => s == cboSource.Text))
					connStrings.Add(cboSource.Text);
				if (!connStrings.Any(s => s == cboTarget.Text))
					connStrings.Add(cboTarget.Text);

				Properties.Settings.Default.ConnectionStrings = string.Join("|", connStrings.ToArray());
				Properties.Settings.Default.LastCompareSourceConnString = cboSource.Text;
				Properties.Settings.Default.LastCompareTargetConnString = cboTarget.Text;
				Properties.Settings.Default.Save();

				_lockDbChange = true;
				cboSourceDatabase.Items.Clear();
				foreach (var dr in fromSchema.Rows.OfType<DataRow>().OrderBy(x => x["database_name"].ToString()))
				{
					cboSourceDatabase.Items.Add(dr["database_name"].ToString());
				}
				cboSourceDatabase.Text = fromDatabase;

				cboTargetDatabase.Items.Clear();
				foreach (var dr in toSchema.Rows.OfType<DataRow>().OrderBy(x => x["database_name"].ToString()))
				{
					cboTargetDatabase.Items.Add(dr["database_name"].ToString());
				}
				cboTargetDatabase.Text = toDatabase;
				_lockDbChange = false;

				btnConnect.Visible = btnRemoveSourceConnString.Visible = btnRemoveTargetConnString.Visible = false;
				btnDisconnect.Visible = true;
				cboSource.SelectionLength = 0;
				cboTarget.SelectionLength = 0;
				cboTarget.Enabled = cboSource.Enabled = false;
				cboSourceDatabase.Visible = cboTargetDatabase.Visible = true;
				btnSourceQuery.Enabled = btnTargetQuery.Enabled = true;
				btnGo.Enabled = btnRefresh.Enabled = btnViewMissingDependencies.Enabled = btnSelectAll.Enabled = true;
			}
		}

		private void refreshPage(bool reinit)
		{
			if (reinit)
			{
				var worker = new BackgroundWorker();
				worker.DoWork += delegate (object sender2, DoWorkEventArgs e2)
				{
					_compareHelper.Init(worker);
				};
				WinControls.WinProgressBox.ShowProgress(worker, progressBarStyle: ProgressBarStyle.Marquee);
			}

			var dropWorkspaces = new List<DropWorkspace>();
			refreshTables(dropWorkspaces);
			refreshObjects(dropWorkspaces);

			gridDropObjects.AutoGenerateColumns = false;
			gridDropObjects.DataSource = new BindingList<DropWorkspace>(dropWorkspaces.OrderBy(w => w.TargetObject.ToString()).ToList());

			_differencedTabs = new List<TabPage>();
			identifyDifferences();
			setTabText();
		}

		private void refreshTables(List<DropWorkspace> dropWorkspaces)
		{
			var lst = TableWorkspaceList.GetTableWorkspaces(_compareHelper);

			foreach (var dw in lst.DropWorkspaces)
			{
				dropWorkspaces.Add(dw);
			}

			TargetTable.Items.Clear();
			TargetTable.Items.Add(string.Empty);
			var toTbls = from s in _compareHelper.ToDatabase.Schemas
						 from t in s.Tables
						 select t;
			TargetTable.Items.AddRange(toTbls.OrderBy(t => t.TableName).ToArray());


			gridTables.AutoGenerateColumns = false;
			gridTables.DataSource = new BindingList<TableWorkspace>(lst.Workspaces.OrderBy(w => w.SourceTable.ToString()).ToList());
		}

		private void refreshObjects(List<DropWorkspace> dropWorkspaces)
		{
			var lst = ObjectWorkspaceList.GetObjectWorkspaces(_compareHelper);
			gridObjects.AutoGenerateColumns = false;
			gridObjects.DataSource = new BindingList<ObjectWorkspace>(lst.Workspaces.OrderBy(w => w.SourceObject.ToString()).ToList());
			foreach (var dw in lst.DropWorkspaces)
			{
				dropWorkspaces.Add(dw);
			}
		}

		private StringBuilder getMissingDependencyString()
		{
			var sb = new StringBuilder();
			var workspaces = gridTables.Rows.OfType<DataGridViewRow>().Select(r => (r.DataBoundItem as TableWorkspace))
				.Where(tw => tw.Select)
				.OfType<WorkspaceBase>()
				.ToList();

			workspaces.AddRange(gridObjects.Rows.OfType<DataGridViewRow>().Select(r => (r.DataBoundItem as ObjectWorkspace))
				.Where(ow => ow.Select));

			workspaces.AddRange(gridDropObjects.Rows.OfType<DataGridViewRow>().Select(r => (r.DataBoundItem as DropWorkspace))
				.Where(ow => ow.Select));

			var missingDependencies = _compareHelper.GetMissingDependencies(workspaces);
			if (missingDependencies.Any())
			{
				foreach (var kvp in missingDependencies)
				{
					sb.AppendLine(kvp.Key.Description + " is dependent on: " + string.Join(", ", kvp.Value.Select(v => v.Description).ToArray()));
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


			var dropSpaces = (gridDropObjects.DataSource as BindingList<DropWorkspace>)
				.Where(t => t.Select).ToList();

			var structureSpaces = (gridTables.DataSource as BindingList<TableWorkspace>)
				.Where(t => t.Select).ToList();

			var dataSpaces = (gridTables.DataSource as BindingList<TableWorkspace>)
				.Where(t => t.SelectTableForData).ToList();

			var truncDelete = (gridTables.DataSource as BindingList<TableWorkspace>)
				.Where(t => t.Truncate || t.Delete).ToList();

			var objSpaces = gridObjects.DataSource == null ? new List<ObjectWorkspace>() : (gridObjects.DataSource as BindingList<ObjectWorkspace>)
				.Where(p => p.Select).ToList();

			var changes = dropSpaces.Select(t => t.TargetObject.Description + " - Drop").Union(
						structureSpaces.Where(t => t.TargetObject != null).Select(t => t.TargetObject + " - Alter")
						).Union(
						structureSpaces.Where(t => t.TargetObject == null).Select(t => t.SourceObject + " - Create")
						).Union(
						dataSpaces.Select(t => (t.TargetObject == null ? t.SourceObject : t.TargetObject) + " - Data")
						).Union(
						truncDelete.Select(t => (t.TargetObject == null ? t.SourceObject : t.TargetObject) + " - Truncate/Delete")
						).Union(
						objSpaces.Select(t => t.SourceObject + " - " + t.SourceObject.ObjectType)
					).ToList();

			if (changes.Count() > 15)
			{
				changes = changes.Take(15).ToList();
				changes.Add("...");
			}

			if (MessageBox.Show(string.Format("{0} - {1} will be changed:\r\n\r\n{2}\r\n\r\nContinue?", _compareHelper.ToDatabase.DataSource,
					_compareHelper.ToDatabase.DatabaseName,
				string.Join("\r\n", changes.ToArray())), "Proceed", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;


			if (!dropSpaces.Any() && !structureSpaces.Any() && !dataSpaces.Any() && !objSpaces.Any() && !truncDelete.Any())
			{
				MessageBox.Show("Nothing to synchronize.");
				return;
			}

			List<WorkspaceBase> workspaces = new List<WorkspaceBase>();
			if (structureSpaces.Any())
				workspaces.AddRange(structureSpaces);

			if (objSpaces.Any())
				workspaces.AddRange(objSpaces);

			if (dropSpaces.Any())
				workspaces.AddRange(dropSpaces);

			bool success = false;
			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.WorkerSupportsCancellation = true;
			worker.DoWork += delegate (object sender2, DoWorkEventArgs e2)
			{
				success = SynchronizationHelper.Synchronize(_compareHelper, workspaces, dataSpaces, truncDelete, worker);
			};

			PaJaMa.WinControls.WinProgressBox.ShowProgress(worker, allowCancel: true, progressBarStyle: ProgressBarStyle.Marquee);

			if (success)
			{
				MessageBox.Show("Done");
				refreshPage(true);
			}
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			gridTables.DataSource = null;
			gridDropObjects.DataSource = null;
			gridObjects.DataSource = null;
			btnDisconnect.Visible = false;
			btnConnect.Visible = true;
			cboSource.Enabled = cboTarget.Enabled = true;
			btnRemoveSourceConnString.Visible = btnRemoveTargetConnString.Visible = true;
			cboSourceDatabase.Visible = cboTargetDatabase.Visible = false;
			btnSourceQuery.Enabled = btnTargetQuery.Enabled = false;
			btnGo.Enabled = btnViewMissingDependencies.Enabled = btnRefresh.Enabled = btnSelectAll.Enabled = false;
			tabTables.Text = "Tables";
			tabObjects.Text = "Objects";
			tabDrop.Text = "Drop";
		}

		private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if ((sender as DataGridView).CurrentCell != null && (sender as DataGridView).CurrentCell.OwningColumn == TransferBatchSize)
				return;

			setTabText();
		}

		private void gridTables_SelectionChanged(object sender, EventArgs e)
		{
			refreshDifferencesControl();
		}

		private void cboSourceDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lockDbChange) return;
			_compareHelper.FromDatabase.ChangeDatabase(cboSourceDatabase.Text);
			refreshPage(true);
		}

		private void cboTargetDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lockDbChange) return;
			_compareHelper.ToDatabase.ChangeDatabase(cboTargetDatabase.Text);
			refreshPage(true);
		}

		private void gridTables_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (gridTables.Columns[e.ColumnIndex] == TransferBatchSize)
				return;

			if (gridTables.Columns[e.ColumnIndex] == TargetTable)
				gridTables_SelectionChanged(sender, e);

			TransferBatchSize.Visible = gridTables.DataSource != null && (gridTables.DataSource as BindingList<TableWorkspace>).Any(tw => tw.SelectTableForData);
		}

		private void btnRemoveSourceConnString_Click(object sender, EventArgs e)
		{
			removeConnString(cboSource.Text, true);
		}

		private void btnRemoveTargetConnString_Click(object sender, EventArgs e)
		{
			removeConnString(cboTarget.Text, false);
		}

		private void removeConnString(string connString, bool source)
		{
			List<string> connStrings = Properties.Settings.Default.ConnectionStrings.Split('|').ToList();
			connStrings.Remove(connString);
			Properties.Settings.Default.ConnectionStrings = string.Join("|", connStrings.ToArray());
			if (source)
				Properties.Settings.Default.LastCompareSourceConnString = string.Empty;
			else
				Properties.Settings.Default.LastCompareTargetConnString = string.Empty;
			Properties.Settings.Default.Save();
			refreshConnStrings();
			if (source)
				cboSource.Text = string.Empty;
			else
				cboTarget.Text = string.Empty;
		}

		private void cboConnString_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnRemoveSourceConnString.Enabled = !string.IsNullOrEmpty(cboSource.Text);
			btnRemoveTargetConnString.Enabled = !string.IsNullOrEmpty(cboTarget.Text);
		}

		private void gridTables_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			var grid = sender as DataGridView;
			if (e.RowIndex >= 0)
			{
				if (grid.Columns[e.ColumnIndex] == DataDetails)
				{
					var workspace = grid.Rows[e.RowIndex].DataBoundItem as TableWorkspace;

					if (workspace.TargetTable == null)
					{
						MessageBox.Show("No target specified.");
						return;
					}

					using (var frm = new frmDataDetails())
					{
						frm.SelectedWorkspace = workspace;
						frm.ShowDialog();
					}
				}
			}
		}

		private void identifyDifferences()
		{
			if (_differencedTabs.Contains(tabMain.SelectedTab)) return;
			_differencedTabs.Add(tabMain.SelectedTab);

			List<DataGridViewRow> rows = new List<DataGridViewRow>();
			if (tabMain.SelectedTab == tabTables)
				rows = gridTables.Rows.OfType<DataGridViewRow>().ToList();
			else if (tabMain.SelectedTab == tabObjects)
				rows = gridObjects.Rows.OfType<DataGridViewRow>().ToList();

			foreach (DataGridViewRow row in rows)
			{
				var ws = row.DataBoundItem as WorkspaceBase;

				row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor = Color.Empty;
				row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor = Color.Empty;
				if (ws.TargetObject == null)
				{
					row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor = Color.LimeGreen;
					row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor = Color.White;
				}
				else
				{
					var differences = ws.SynchronizationItems;
					if (differences.Any(d => d.Scripts.Any(s => s.Value.Length > 0)))
					{
						row.DefaultCellStyle.SelectionBackColor = row.DefaultCellStyle.BackColor = Color.Red;
						row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor = Color.White;
					}
				}

				if (tabMain.SelectedTab == tabTables)
				{
					row.Cells[DataDetails.Name].Value = "Data";
				}
			}
		}

		private void setTabText()
		{
			tabTables.Text = "Tables";
			tabObjects.Text = "Objects";
			tabDrop.Text = "Drop";

			if (gridDropObjects.Rows.Count > 0)
			{
				var selected = (gridDropObjects.DataSource as BindingList<DropWorkspace>).Count(d => d.Select);
				tabDrop.Text += " (" + gridDropObjects.Rows.Count.ToString() + (selected > 0 ? ", " + selected.ToString() + " selected" : "") + ")";
			}

			var dict = new Dictionary<DataGridView, TabPage>();
			dict.Add(gridTables, tabTables);
			dict.Add(gridObjects, tabObjects);

			foreach (var kvp in dict)
			{
				int diff = 0;
				int nw = 0;
				int selected = 0;
				foreach (DataGridViewRow row in kvp.Key.Rows.OfType<DataGridViewRow>())
				{
					var ws = row.DataBoundItem as WorkspaceBase;

					if (ws.Select)
						selected++;

					if (ws.TargetObject == null)
						nw++;
					else
					{
						var differences = ws.SynchronizationItems;
						if (differences.Any(d => d.Scripts.Any(s => s.Value.Length > 0)))
							diff++;
					}
				}

				string inner = string.Empty;
				if (nw > 0)
					inner += ", " + nw.ToString() + " new";

				if (diff > 0)
					inner += ", " + diff.ToString() + " diff";

				if (selected > 0)
					inner += ", " + selected.ToString() + " selected";

				if (!string.IsNullOrEmpty(inner))
				{
					kvp.Value.Text += " (" + inner.Substring(2) + ")";
				}
			}
		}

		private void gridObjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			//var grid = sender as DataGridView;
			//if (grid.Columns[e.ColumnIndex] == colProgDetails && e.RowIndex >= 0)
			//{
			//	//using (var frm = new frmObjectStructureDetails())
			//	//{
			//	//	var workspace = grid.Rows[e.RowIndex].DataBoundItem as ObjectWorkspace;
			//	//	frm.Workspace = workspace;
			//	//	frm.ShowDialog();
			//	//}
			//}
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

		private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabMain.SelectedTab == tabDrop)
			{
				refreshDifferencesControl();
				btnViewCreates.Enabled = false;
			}
			else
			{
				refreshDifferencesControl();
				identifyDifferences();
			}
		}

		private void _frmDifferences_Synchronized(object sender, EventArgs e)
		{
			refreshPage(true);
		}

		private void refreshDifferencesControl()
		{
			btnViewCreates.Enabled = false;

			DataGridView grid = null;
			ucDifferences activeDifferences = null;
			if (tabMain.SelectedTab == tabTables)
			{
				grid = gridTables;
				activeDifferences = diffTables;
			}
			else if (tabMain.SelectedTab == tabObjects)
			{
				grid = gridObjects;
				activeDifferences = diffObjects;
			}
			else
			{
				grid = gridDropObjects;
				activeDifferences = diffDrops;
			}

			if (grid == null)
				return;

			activeDifferences.CompareHelper = _compareHelper;

			if (grid.SelectedRows.Count != 1)
				activeDifferences.Workspace = null;
			else
			{
				activeDifferences.Workspace = grid.SelectedRows[0].DataBoundItem as WorkspaceBase;
				_activeWorkspace = activeDifferences.Workspace;
				btnViewCreates.Enabled = _activeWorkspace is WorkspaceWithSourceBase && _activeWorkspace.TargetObject != null && _activeWorkspace.SynchronizationItems.Any();

			}
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			refreshPage(true);
		}

		private void gridObjects_SelectionChanged(object sender, EventArgs e)
		{
			refreshDifferencesControl();
		}

		private void btnViewMissingDependencies_Click(object sender, EventArgs e)
		{
			var sb = getMissingDependencyString();

			if (sb.Length > 0)
				MessageBox.Show(sb.ToString());
			else
				MessageBox.Show("No missing dependencies!");
		}

		private void btnSwitch_Click(object sender, EventArgs e)
		{
			bool connected = btnDisconnect.Visible;
			if (connected)
				btnDisconnect_Click(sender, e);

			string sourceText = cboSource.Text;
			string targetText = cboTarget.Text;

			cboSource.Text = targetText;
			cboTarget.Text = sourceText;

			if (connected)
				btnConnect_Click(sender, e);
		}

		private void gridDropObjects_SelectionChanged(object sender, EventArgs e)
		{
			refreshDifferencesControl();
		}

		private void btnViewCreates_Click(object sender, EventArgs e)
		{
			if (_activeWorkspace is WorkspaceWithSourceBase)
			{
				frmCreates frm = new frmCreates();
				frm.Workspace = _activeWorkspace as WorkspaceWithSourceBase;
				frm.Show();
			}
		}

		private void btnSelectAll_Click(object sender, EventArgs e)
		{
			foreach (var ws in (gridDropObjects.DataSource as BindingList<DropWorkspace>))
			{
				ws.Select = true;
			}
			gridDropObjects.Invalidate();

			foreach (var ws in (gridObjects.DataSource as BindingList<ObjectWorkspace>))
			{
				ws.Select = true;
			}
			gridObjects.Invalidate();

			foreach (var ws in (gridTables.DataSource as BindingList<TableWorkspace>))
			{
				ws.Select = true;
			}
			gridTables.Invalidate();

			setTabText();
		}

		private void btnSourceQuery_Click(object sender, EventArgs e)
		{
			query(_compareHelper.FromDatabase);
		}

		private void btnTargetQuery_Click(object sender, EventArgs e)
		{
			query(_compareHelper.ToDatabase);
		}

		private void selectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			query(_compareHelper.FromDatabase, 0);
		}

		private void selectTop1000ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			query(_compareHelper.FromDatabase, 1000);
		}

		private void query(IDatabase database, int? topN = null)
		{
			if (database != null)
			{
				var args = new QueryEventArgs();
				args.Database = database;
				if (topN != null)
				{
					var selectedItem = gridTables.SelectedRows[0].DataBoundItem as TableWorkspace;
					args.InitialTopN = topN;
					args.InitialTable = selectedItem.SourceTable.TableName;
					args.InitialSchema = selectedItem.SourceTable.Schema.SchemaName;
				}
				QueryDatabase(this, args);
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (var dlg = new SaveFileDialog())
				{
					dlg.Filter = "DatabaseStudio compare files (*.dbc)|*.dbc";
					dlg.Title = "Workspaces";
					if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						var ws = new CompareWorkspace();
						ws.FromConnectionString = cboSource.Text;
						ws.ToConnectionString = cboTarget.Text;
						ws.FromDatabase = cboSourceDatabase.Text;
						ws.ToDatabase = cboTargetDatabase.Text;

						ws.SelectedDropWorkspaces = (gridDropObjects.DataSource as BindingList<DropWorkspace>)
							.Where(t => t.Select)
							.Select(dw => SerializableDropWorkspace.GetFromDropWorkspace(dw))
							.ToList();
						ws.SelectedTableWorkspaces = (gridTables.DataSource as BindingList<TableWorkspace>)
							.Where(t => t.Select || t.SelectTableForData || t.Truncate || t.Delete)
							.Select(tw => SerializableTableWorkspace.GetFromTableWorkspace(tw)).ToList();
						ws.SelectedObjectWorkspaces = gridObjects.DataSource == null ? new List<SerializableObjectWorkspace>() : (gridObjects.DataSource as BindingList<ObjectWorkspace>)
							.Where(p => p.Select)
							.Select(o => SerializableObjectWorkspace.GetFromObjectWorkspace(o))
							.ToList();


						PaJaMa.Common.XmlSerialize.SerializeObjectToFile<CompareWorkspace>(ws, dlg.FileName);
						MessageBox.Show("Workspaces saved.");
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (var dlg = new OpenFileDialog())
				{
					dlg.Filter = "DatabaseStudio compare files (*.dbc)|*.dbc";
					dlg.Title = "Workspaces";
					if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						var workspaces = PaJaMa.Common.XmlSerialize.DeserializeObjectFromFile<CompareWorkspace>(dlg.FileName);
						if (btnDisconnect.Visible)
							btnDisconnect_Click(sender, e);

						cboSource.Text = workspaces.FromConnectionString;
						cboTarget.Text = workspaces.ToConnectionString;
						cboSourceDatabase.Text = workspaces.FromDatabase;
						cboTargetDatabase.Text = workspaces.ToDatabase;
						btnConnect_Click(sender, e);

						var tws = gridTables.DataSource as BindingList<TableWorkspace>;
						foreach (var stw in workspaces.SelectedTableWorkspaces)
						{
							var tw = tws.FirstOrDefault(w => w.SourceTable.ToString() == stw.SourceSchemaTableName);
							if (tw != null)
							{
								if (!string.IsNullOrEmpty(stw.TargetSchemaTableName))
								{
									tw.TargetObject = (from s in _compareHelper.ToDatabase.Schemas
													   from t in s.Tables
													   where t.ToString() == stw.TargetSchemaTableName
													   select t).FirstOrDefault();
								}
								tw.SelectTableForData = stw.SelectTableForData;
								tw.Select = stw.SelectTableForStructure;
								tw.Delete = stw.Delete;
								tw.Truncate = stw.Truncate;
								tw.KeepIdentity = stw.KeepIdentity;
								tw.RemoveAddKeys = stw.RemoveAddKeys;
								tw.RemoveAddIndexes = stw.RemoveAddIndexes;
								tw.TransferBatchSize = stw.TransferBatchSize;
							}
						}

						var ows = gridObjects.DataSource as BindingList<ObjectWorkspace>;
						foreach (var sow in workspaces.SelectedObjectWorkspaces)
						{
							var ow = ows.FirstOrDefault(w => w.SourceObject.ToString() == sow.SourceObjectName && w.SourceObject.ObjectType == sow.ObjectType);
							if (ow != null)
							{
								//if (!string.IsNullOrEmpty(sow.TargetObjectName))
								//{
								//	ow.TargetObject = _compareHelper.ToDatabase.Obj
								//}
								ow.Select = true;
							}
						}

						var dws = gridDropObjects.DataSource as BindingList<DropWorkspace>;
						foreach (var sdw in workspaces.SelectedDropWorkspaces)
						{
							var dw = dws.FirstOrDefault(w => w.TargetObject.ToString() == sdw.Name && w.TargetObject.ObjectType == sdw.Type);
							if (dw != null)
								dw.Select = true;
						}

						TransferBatchSize.Visible = workspaces.SelectedTableWorkspaces.Any(tw => tw.SelectTableForData);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void btnDataDifferences_Click(object sender, EventArgs e)
		{
			var bw = new BackgroundWorker();
			var tws = (gridTables.DataSource as BindingList<TableWorkspace>).Where(tw => tw.SelectTableForData);
			var differences = new List<DataDifference>();
			var prog = new PaJaMa.WinControls.WinProgressBox();
			var dataHelper = new DataHelper();
			prog.Cancel += delegate (object sender2, EventArgs e2)
			{
				dataHelper.Cancel();
			};
			bw.DoWork += delegate (object sender2, DoWorkEventArgs e2)
			{
				foreach (var tw in tws)
				{
					if (bw.CancellationPending)
						return;
					var diffs = dataHelper.GetDataDifferences(tw, bw);
					if (diffs != null)
						differences.Add(diffs);
				}
			};

			prog.Show(bw, allowCancel: true);

			foreach (var diff in differences)
			{
				var row = gridTables.Rows.OfType<DataGridViewRow>().First(r => r.DataBoundItem.Equals(diff.TableWorkspace));
				row.Cells[DataDetails.Name].Value = string.Format("{0}/{1}/{2}", diff.Differences, diff.SourceOnly, diff.TargetOnly);
			}
		}

		private void gridTables_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
		{
			if (gridTables.CurrentCell.OwningColumn != TransferBatchSize) return;

			var tws = gridTables.Rows[gridTables.CurrentCell.RowIndex].DataBoundItem as TableWorkspace;
			e.Control.Visible = tws.SelectTableForData;
		}

		private void mnuMain_Opening(object sender, CancelEventArgs e)
		{
			var allRowsDataSelected = gridTables.SelectedRows.OfType<DataGridViewRow>().Any()
				&& gridTables.SelectedRows.OfType<DataGridViewRow>().All(dgv => (dgv.DataBoundItem as TableWorkspace).SelectTableForData);

			setBatchSizeToolStripMenuItem.Visible = allRowsDataSelected;
		}

		private void setBatchSizeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var result = PaJaMa.WinControls.NumericInputBox.Show("Enter batch size", "Batch Size", TableWorkspace.DEFAULT_BATCH_SIZE);
			if (result.Result == DialogResult.OK)
			{
				foreach (DataGridViewRow row in gridTables.SelectedRows)
				{
					var tw = row.DataBoundItem as TableWorkspace;
					tw.TransferBatchSize = (int)result.Value;
				}
				gridTables.Refresh();
			}
		}
	}
}
