using PaJaMa.DatabaseStudio.Classes;
using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using PaJaMa.DatabaseStudio.Query.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Query
{
	public partial class ucWorkspace : UserControl
	{
		private DbConnection _currentConnection;

		private string _initialConnString;
		private Type _initialDbType;

		private QueryEventArgs _queryEventArgs;

		public ucWorkspace()
		{
			InitializeComponent();
		}

		private void ucWorkspace_Load(object sender, EventArgs e)
		{
			List<Type> types = new List<Type>() {
				typeof(System.Data.SqlClient.SqlConnection), 
				typeof(System.Data.OleDb.OleDbConnection),
				typeof(System.Data.Odbc.OdbcConnection)
			};

			types.AddRange(ExternalTypes.GetExternalTypes());

			cboServer.DataSource = types.ToArray();

			var settings = Properties.Settings.Default;

			if (settings.ConnectionStrings == null)
				settings.ConnectionStrings = string.Empty;

			refreshConnStrings();

			this.ParentForm.FormClosing += ucWorkspace_FormClosing;

			if (!string.IsNullOrEmpty(_initialConnString))
			{
				txtConnectionString.Text = _initialConnString;
				cboServer.SelectedItem = _initialDbType;
				btnConnect_Click(sender, e);
				//if (cboDatabases.Items.Count > 0)
				//	cboDatabases.SelectedItem = copiedFrom.cboDatabases.SelectedItem;

				//var treeNode = treeTables.Nodes.OfType<TreeNode>().First(n => n.Text == copiedFrom.cboDatabases.SelectedItem.ToString());
				//treeNode.Expand();

				//copiedFrom = null;
			}
			else if (_queryEventArgs != null)
			{
				txtConnectionString.Text = _queryEventArgs.Database.ConnectionString;
				cboServer.SelectedItem = typeof(SqlConnection);
				btnConnect_Click(this, new EventArgs());
				//if (cboDatabases.Items.Count > 0)
				//	cboDatabases.SelectedItem = _queryEventArgs.Database.DatabaseName;

				var treeNode = treeTables.Nodes.OfType<TreeNode>().First(n => n.Text == _queryEventArgs.Database.DatabaseName);
				treeNode.Expand();

				if (_queryEventArgs.InitialTable != null)
				{
					var childNode = (from n in treeNode.Nodes.OfType<TreeNode>()
									 from n2 in n.Nodes.OfType<TreeNode>()
									 let tbl = n2.Tag as Table
									 where tbl != null && tbl.TableName == _queryEventArgs.InitialTable
										&& tbl.Schema.SchemaName == _queryEventArgs.InitialSchema
									 select n2).First();

					treeTables.SelectedNode = childNode;

					if (_queryEventArgs.InitialTopN != null)
						select(_queryEventArgs.InitialTopN.Value);
				}

				_queryEventArgs = null;
			}
			else if (!string.IsNullOrEmpty(Properties.Settings.Default.LastQueryConnectionString))
			{
				txtConnectionString.Text = Properties.Settings.Default.LastQueryConnectionString;
				cboServer.SelectedItem = Type.GetType(Properties.Settings.Default.LastQueryServerType);
				chkUseDummyDA.Checked = Properties.Settings.Default.LastQueryUseDummyDA;
			}
		}

		public void Disconnect()
		{
			if (_currentConnection != null)
			{
				if (_currentConnection.State == ConnectionState.Open)
					_currentConnection.Close();
				_currentConnection.Dispose();
				_currentConnection = null;
			}
			//lblResults.Text = "";

			pnlControls.Visible = false;
			pnlConnect.Visible = true;

			foreach (TabPage page in tabOutputs.TabPages)
			{
				var uc = page.Controls[0] as ucQueryOutput;
				uc.Disconnect();
			}

			tabOutputs.TabPages.Clear();
			splitMain.Panel1Collapsed = true;
		}

		private void btnConnect_Click(object sender, EventArgs e)
		{
			Disconnect();

			if (cboServer.SelectedItem == null)
			{
				MessageBox.Show("Select server type.");
				return;
			}

			Type serverType = cboServer.SelectedItem as Type;
			_currentConnection = Activator.CreateInstance(serverType) as DbConnection;

			try
			{
				_currentConnection.ConnectionString = txtConnectionString.Text;
				if (chkUseDummyDA.Checked)
				{
					DbDataAdapter dummy;
					if (_currentConnection.GetType().Equals(typeof(System.Data.Odbc.OdbcConnection)))
						dummy = new System.Data.Odbc.OdbcDataAdapter("dummy", (System.Data.Odbc.OdbcConnection)_currentConnection);
					else
						throw new NotImplementedException();
				}
				_currentConnection.Open();
			}
			catch (Exception ex)
			{
				Disconnect();
				MessageBox.Show(ex.Message);
				return;
			}

			List<string> connStrings = Properties.Settings.Default.ConnectionStrings.Split('|').ToList();
			if (!connStrings.Any(s => s == txtConnectionString.Text))
				connStrings.Add(txtConnectionString.Text);
			Properties.Settings.Default.ConnectionStrings = string.Join("|", connStrings.ToArray());
			Properties.Settings.Default.LastQueryConnectionString = txtConnectionString.Text;
			Properties.Settings.Default.LastQueryServerType = (cboServer.SelectedItem as Type).AssemblyQualifiedName;
			Properties.Settings.Default.LastQueryUseDummyDA = chkUseDummyDA.Checked;
			Properties.Settings.Default.Save();

			var uc = new ucQueryOutput();
			uc.Dock = DockStyle.Fill;
			uc.Connect(_currentConnection, serverType, _currentConnection.Database, chkUseDummyDA.Checked);
			var tabPage = new TabPage();
			tabPage.Text = "Query " + (tabOutputs.TabPages.Count + 1).ToString();
			tabPage.Controls.Add(uc);
			tabOutputs.TabPages.Add(tabPage);
			tabOutputs.SelectedTab = tabPage;

			lblConnString.Text = txtConnectionString.Text;
			pnlConnect.Visible = false;
			treeTables.Nodes.Clear();

			if (!serverType.Equals(typeof(System.Data.SqlClient.SqlConnection)))
			{
				var tables = new List<Table>();
				if (chkUseDummyDA.Checked)
					tables = NonSqlServerSchemaHelper.GetTablesExistingConnection(_currentConnection);
				else
					tables = NonSqlServerSchemaHelper.GetTables(_currentConnection);
				foreach (var table in tables)
				{
					var node = treeTables.Nodes.Add(table.ToString());
					foreach (var column in table.Columns)
					{
						var node2 = node.Nodes.Add(column.ColumnName + " (" + column.DataType + ", "
											+ (column.IsNullable ? "null" : "not null") + ")");
						node2.Tag = column;
					}
					node.Tag = table;
				}
			}

			pnlControls.Visible = true;

			splitMain.Panel1Collapsed = false;

			if (serverType.Equals(typeof(System.Data.SqlClient.SqlConnection)))
			{
				var dt = new DataTable();
				using (var da = new System.Data.SqlClient.SqlDataAdapter("select [name] from sys.databases order by [name]", (System.Data.SqlClient.SqlConnection)_currentConnection))
				{
					da.Fill(dt);
					foreach (var dr in dt.Rows.OfType<DataRow>())
					{
						var connStringBuilder = new SqlConnectionStringBuilder(_currentConnection.ConnectionString);
						connStringBuilder.InitialCatalog = dr[0].ToString();
						var db = new Database(connStringBuilder.ConnectionString);
						var node = treeTables.Nodes.Add(db.ToString());
						node.Nodes.Add("__NONE__");
						node.Tag = db;
					}
				}

				if (!string.IsNullOrEmpty(_currentConnection.Database))
				{
					var treeNode = treeTables.Nodes.OfType<TreeNode>().First(n => n.Text == _currentConnection.Database);
					treeNode.Expand();
				}
			}
			else
			{

			}
		}

		private ucQueryOutput addQueryOutput(string initialDatabase)
		{
			var uc = new ucQueryOutput();
			uc.Dock = DockStyle.Fill;
			uc.Connect(_currentConnection, cboServer.SelectedItem as Type, initialDatabase, chkUseDummyDA.Checked);
			var tabPage = new TabPage();
			tabPage.Text = "Query " + (tabOutputs.TabPages.Count + 1).ToString();
			tabPage.Controls.Add(uc);
			tabOutputs.TabPages.Add(tabPage);
			tabOutputs.SelectedTab = tabPage;
			return uc;
		}

		private void btnDisconnect_Click(object sender, EventArgs e)
		{
			Disconnect();
		}

		//private DataTable FixBinaryColumnsForDisplay(DataTable t)
		//{
		//	List<string> binaryColumnNames = t.Columns.Cast<DataColumn>().Where(col => col.DataType.Equals(typeof(byte[]))).Select(col => col.ColumnName).ToList();
		//	foreach (string binaryColumnName in binaryColumnNames)
		//	{
		//		// Create temporary column to copy over data
		//		string tempColumnName = "C" + Guid.NewGuid().ToString();
		//		t.Columns.Add(new DataColumn(tempColumnName, typeof(string)));
		//		t.Columns[tempColumnName].SetOrdinal(t.Columns[binaryColumnName].Ordinal);

		//		// Replace values in every row
		//		StringBuilder hexBuilder = new StringBuilder(8000 * 2 + 2);
		//		foreach (DataRow r in t.Rows)
		//		{
		//			r[tempColumnName] = BinaryDataColumnToString(hexBuilder, r[binaryColumnName]);
		//		}

		//		t.Columns.Remove(binaryColumnName);
		//		t.Columns[tempColumnName].ColumnName = binaryColumnName;
		//	}
		//	return t;
		//}

		//private string BinaryDataColumnToString(StringBuilder hexBuilder, object columnValue)
		//{
		//	const string hexChars = "0123456789ABCDEF";
		//	if (columnValue == DBNull.Value)
		//	{
		//		// Return special "(null)" value here for null column values
		//		return "(null)";
		//	}
		//	else
		//	{
		//		// Otherwise return hex representation
		//		byte[] byteArray = (byte[])columnValue;
		//		int displayLength = (byteArray.Length > maxBinaryDisplayString) ? maxBinaryDisplayString : byteArray.Length;
		//		hexBuilder.Length = 0;
		//		hexBuilder.Append("0x");
		//		for (int i = 0; i < displayLength; i++)
		//		{
		//			hexBuilder.Append(hexChars[(int)byteArray[i] >> 4]);
		//			hexBuilder.Append(hexChars[(int)byteArray[i] % 0x10]);
		//		}
		//		return hexBuilder.ToString();
		//	}
		//}

		private void refreshConnStrings()
		{
			var conns = Properties.Settings.Default.ConnectionStrings.Split('|');
			txtConnectionString.Items.Clear();
			txtConnectionString.Items.AddRange(conns);
		}

		private void ucWorkspace_FormClosing(object sender, FormClosingEventArgs e)
		{
			Disconnect();
		}

		private void cboServer_Format(object sender, ListControlConvertEventArgs e)
		{
			Type type = e.ListItem as Type;
			e.Value = type.Name;
		}


		private void btnShowHideTables_Click(object sender, EventArgs e)
		{
			splitMain.Panel1Collapsed = !splitMain.Panel1Collapsed;
		}

		private void treeTables_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			var node = e.Node;
			if (node.Tag is Database)
			{
				var db = node.Tag as Database;
				db.PopulateChildren(true, null);
				node.Nodes.Clear();
				var node2 = node.Nodes.Add("Table");
				foreach (var table in from s in db.Schemas
									  from t in s.Tables
									  orderby t.TableName
									  orderby t.Schema.SchemaName
									  select t)
				{
					var node3 = node2.Nodes.Add(table.ToString());
					foreach (var column in table.Columns)
					{
						node3.Nodes.Add(column.ToString());
					}
					node3.Tag = table;
				}

				node2 = node.Nodes.Add("Views");
				foreach (var view in from s in db.Schemas
									 from v in s.Views
									 orderby v.ViewName
									 orderby v.Schema.SchemaName
									 select v)
				{
					var node3 = node2.Nodes.Add(view.ToString());
					foreach (var column in view.Columns)
					{
						node3.Nodes.Add(column.ToString());
					}
					node3.Tag = view;
				}
			}
		}

		private void mnuTree_Opening(object sender, CancelEventArgs e)
		{
			var selectedNode = treeTables.SelectedNode;
			selectTop1000ToolStripMenuItem.Enabled = selectToolStripMenuItem.Enabled = selectedNode != null &&
				selectedNode.Tag != null && (selectedNode.Tag is Table || selectedNode.Tag is PaJaMa.DatabaseStudio.DatabaseObjects.View);
		}


		private void selectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			selectToNew(0);
		}

		private void selectTop1000ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			selectToNew(1000);
		}

		private void select(int topN)
		{
			var uc = tabOutputs.SelectedTab.Controls[0] as ucQueryOutput;
			uc.SelectTopN(topN, treeTables.SelectedNode);
		}


		private void selectToNew(int topN)
		{
			var uc = addQueryOutput(string.Empty);
			uc.SelectTopN(topN, treeTables.SelectedNode);
		}

		private void btnCopyWorkspace_Click(object sender, EventArgs e)
		{
			copyWorkspace(true);
		}

		private ucWorkspace copyWorkspace(bool andText, string initialConnString = null)
		{
			var uc = new ucWorkspace();
			if (string.IsNullOrEmpty(initialConnString))
				initialConnString = txtConnectionString.Text;

			uc._initialConnString = initialConnString;
			uc._initialDbType = cboServer.SelectedItem as Type;
			//if (andText)
			//	uc.txtQuery.Text = txtQuery.Text;
			var tabMain = this.Parent.Parent as TabControl;
			var tab = new TabPage("Workspace " + (tabMain.TabPages.Count + 1).ToString());
			uc.Dock = DockStyle.Fill;
			tab.Controls.Add(uc);
			tabMain.TabPages.Add(tab);
			tabMain.SelectedTab = tab;
			return uc;
		}

		private void treeTables_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (e.Button == MouseButtons.Right) treeTables.SelectedNode = e.Node;
		}

		public Workspace GetWorkspace()
		{
			var ws = new Workspace()
			{
				ConnectionString = txtConnectionString.Text,
				ConnectionType = (cboServer.SelectedItem as Type).FullName,
				//Database = cboDatabases.Text
			};

			foreach (TabPage page in tabOutputs.TabPages)
			{
				var uc = page.Controls[0] as ucQueryOutput;
				ws.Queries.Add(new QueryOutput() { Query = uc.txtQuery.Text, Database = uc.cboDatabases.Text });
			}

			return ws;
		}

		public void LoadWorkspace(Workspace workspace)
		{
			txtConnectionString.Text = workspace.ConnectionString;
			cboServer.SelectedItem = cboServer.Items.OfType<Type>().First(t => t.FullName == workspace.ConnectionType);
			btnConnect_Click(this, new EventArgs());

			tabOutputs.TabPages.Clear();
			foreach (var qry in workspace.Queries)
			{
				var uc = addQueryOutput(qry.Database);
				uc.txtQuery.Text = qry.Query;
			}

		}

		public void LoadFromIDatabase(QueryEventArgs args)
		{
			_queryEventArgs = args;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (tabOutputs.TabPages.Count < 1) return;
			var uc = tabOutputs.SelectedTab.Controls[0] as ucQueryOutput;
			uc.Disconnect();
			tabOutputs.TabPages.Remove(tabOutputs.SelectedTab);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			addQueryOutput(_currentConnection.Database);
		}

		private void scriptCreateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var connString = txtConnectionString.Text;
			var ws = this;
			var inputBox = PaJaMa.WinControls.InputBox.Show("Enter Target Connection String", "Target", connString);
			if (inputBox.Result == DialogResult.OK && connString != inputBox.Text)
			{
				connString = inputBox.Text;
				ws = copyWorkspace(false, connString);
			}
			else if (inputBox.Result == DialogResult.Cancel)
				return;

			var uc = ws.addQueryOutput(string.Empty);

			if (treeTables.SelectedNode.Tag is Database)
			{
				var db = treeTables.SelectedNode.Tag as Database;
				inputBox = PaJaMa.WinControls.InputBox.Show("Enter Target Database", "Target", db.DatabaseName);
				if (inputBox.Result == DialogResult.OK)
				{
					uc.PopulateScript(new DatabaseSynchronization(db).GetCreateScript(inputBox.Text), treeTables.SelectedNode);
				}
			}
			else
			{
				var obj = treeTables.SelectedNode.Tag as DatabaseObjectBase;
				if (obj != null)
				{
					uc.PopulateScript(DatabaseObjectSynchronizationBase.GetSynchronization(obj).GetRawCreateText(), treeTables.SelectedNode);
				}
			}
		}

	}
}
