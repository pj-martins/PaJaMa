using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Data.SqlClient;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System.Threading;
using PaJaMa.DatabaseStudio.Query.Classes;

namespace PaJaMa.DatabaseStudio.Query
{
	public partial class ucQueryOutput : UserControl
	{
		private List<SplitContainer> _splitContainers = new List<SplitContainer>();
		private bool _lock = false;
		private bool _stopRequested = false;
		private DateTime _start;
		public DbConnection _currentConnection;
		private DbCommand _currentCommand;
		private string _query;
		private bool _somethingPrinted;

		public ucQueryOutput()
		{
			InitializeComponent();
		}

		private void btnGo_Click(object sender, EventArgs e)
		{
			string query = txtQuery.Text;
			if (txtQuery.SelectionLength > 0)
				query = txtQuery.SelectedText;

			// rich text replaces newlines
			query = query.Replace("\n", "\r\n");

			lblResults.Visible = false;
			_stopRequested = false;
			_currentCommand = _currentConnection.CreateCommand();
			_query = query;
			btnGo.Visible = false;
			btnStop.Visible = true;
			btnStop.Enabled = true;
			cboDatabases.Enabled = false;
			_start = DateTime.Now;
			lblTime.Text = string.Empty;
			lblTime.Visible = true;
			timDuration.Enabled = true;
			progMain.Visible = true;
			txtQuery.ReadOnly = true;

			var currControls = tabResults.Controls.OfType<Control>();
			foreach (var ctrl in currControls)
			{
				tabResults.Controls.Remove(ctrl);
				ctrl.Dispose();
			}
			txtMessages.Text = string.Empty;
			tabControl1.SelectedTab = tabResults;
			currControls = null;
			_splitContainers = new List<SplitContainer>();

			this.Parent.Text += " (Executing)";

			if (_currentConnection.GetType().Equals(typeof(System.Data.OleDb.OleDbConnection)))
				execute();
			else
				new Thread(new ThreadStart(execute)).Start();
		}

		public void Connect(DbConnection connection, Type serverType, string initialDatabase, bool useDummyDA)
		{
			if (useDummyDA)
				_currentConnection = connection;
			else
			{
				_currentConnection = Activator.CreateInstance(connection.GetType()) as DbConnection;

				_currentConnection.ConnectionString = connection.ConnectionString;
				if (_currentConnection is SqlConnection)
					(_currentConnection as SqlConnection).InfoMessage += ucQueryOutput_InfoMessage;

				_currentConnection.Open();
			}

			pnlButtons.Enabled = splitQuery.Enabled = true;

			lblDatabase.Visible = cboDatabases.Visible = serverType.Equals(typeof(System.Data.SqlClient.SqlConnection));


			if (serverType.Equals(typeof(System.Data.SqlClient.SqlConnection)))
			{
				var dt = new DataTable();
				using (var da = new System.Data.SqlClient.SqlDataAdapter("select [name] from sys.databases order by [name]", (System.Data.SqlClient.SqlConnection)_currentConnection))
				{
					da.Fill(dt);
					cboDatabases.Items.Clear();
					foreach (var dr in dt.Rows.OfType<DataRow>())
					{
						var connStringBuilder = new SqlConnectionStringBuilder(_currentConnection.ConnectionString);
						connStringBuilder.InitialCatalog = dr[0].ToString();
						var db = new Database(connStringBuilder.ConnectionString);
						cboDatabases.Items.Add(db);
					}

					if (!string.IsNullOrEmpty(initialDatabase) && initialDatabase != _currentConnection.Database)
						_currentConnection.ChangeDatabase(initialDatabase);

					_lock = true;
					cboDatabases.Text = _currentConnection.Database;
					_lock = false;
				}
			}
			//else
			//{
			//	foreach (var table in NonSqlServerSchemaHelper.GetTables(_currentConnection))
			//	{
			//		var node = treeTables.Nodes.Add(table.ToString());
			//		foreach (var column in table.Columns)
			//		{
			//			var node2 = node.Nodes.Add(column.ColumnName + " (" + column.DataType + ", "
			//								+ (column.IsNullable ? "null" : "not null") + ")");
			//			node2.Tag = column;
			//		}
			//		node.Tag = table;
			//	}
			//}


		}

		private void ucQueryOutput_InfoMessage(object sender, SqlInfoMessageEventArgs e)
		{
			_somethingPrinted = true;
			this.Invoke(new Action(() =>
				{
					txtMessages.Text += e.Message + "\r\n\r\n";
				}));
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
		}

		private void setDatabaseText()
		{
			if (cboDatabases.Items.Count > 0 && cboDatabases.Visible)
			{
				_lock = true;
				cboDatabases.Text = _currentConnection.Database;
				_lock = false;
			}
		}


		private void execute()
		{
			int totalResults = 0;
			int recordsAffected = 0;
			string errorMessage = string.Empty;
			try
			{
				var parts = _query.Split(new string[] { "\r\ngo\r\n", "\r\nGO\r\n", "\r\nGo\r\n", "\r\ngO\r\n",
					"\ngo\n", "\nGO\n", "\nGo\n", "\ngO\n"
				}, StringSplitOptions.RemoveEmptyEntries);

				bool ignorePrompt = false;

				foreach (var part in parts)
				{
					_currentCommand.CommandText = part;
					_currentCommand.CommandTimeout = 600000;
					try
					{
						using (var dr = _currentCommand.ExecuteReader())
						{
							this.Invoke(new Action(() =>
							{
								//if (!_stopRequested && !dr.HasRows)
								//{
								//	lblResults.Text = "Complete.";
								//	lblResults.Visible = true;
								//	setDatabaseText();

								//	return;
								//}
								bool hasNext = true;
								while (!_stopRequested)
								{
									DataTable dt = new DataTable();
									var schema = dr.GetSchemaTable();
									if (schema == null || !hasNext)
									{
										if (dr.RecordsAffected > 0)
											recordsAffected += dr.RecordsAffected;
										break;
									}

									var grid = new DataGridView();
									foreach (var row in schema.Rows.OfType<DataRow>())
									{
										// int existingCount = dt.Columns.OfType<DataColumn>().Count(c => c.ColumnName == row["ColumnName"].ToString());
										var colType = Type.GetType(row["DataType"].ToString());
										if (colType.Equals(typeof(byte[])))
											colType = typeof(string);
										string colName = row["ColumnName"].ToString();
										int curr = 1;
										while (dt.Columns.OfType<DataColumn>().Any(c => c.ColumnName == colName))
										{
											colName = row["ColumnName"].ToString() + curr.ToString();
											curr++;
										}
										dt.Columns.Add(colName, colType);
										grid.Columns.Add(colName, row["ColumnName"].ToString());
									}

									var lastSplit = _splitContainers.LastOrDefault();
									if (lastSplit != null)
										lastSplit.Panel2Collapsed = false;
									var splitContainer = new SplitContainer();
									splitContainer.Dock = DockStyle.Fill;
									splitContainer.Orientation = Orientation.Horizontal;
									splitContainer.Panel2Collapsed = true;
									splitContainer.Panel2MinSize = 0;
									grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
									grid.Dock = DockStyle.Fill;
									grid.AllowUserToAddRows = grid.AllowUserToDeleteRows = false;
									grid.ReadOnly = true;
									grid.VirtualMode = true;
									grid.RowCount = 0;
									grid.CellValueNeeded += grid_CellValueNeeded;
									grid.CellFormatting += grid_CellFormatting;
									splitContainer.Panel1.Controls.Add(grid);
									if (lastSplit != null)
										lastSplit.Panel2.Controls.Add(splitContainer);
									else
										tabResults.Controls.Add(splitContainer);
									_splitContainers.Add(splitContainer);
									foreach (var split in _splitContainers)
									{
										split.SplitterDistance = splitQuery.Panel2.Height / _splitContainers.Count;
									}
									//grid.DataSource = dt;
									grid.AutoGenerateColumns = true;
									grid.Tag = new List<DataTable>() { dt };

									foreach (DataGridViewColumn col in grid.Columns)
									{
										var dtCol = dt.Columns[col.Name];
										col.ToolTipText = dtCol.DataType.Name;
									}

									int i = 0;
									var lastRefresh = DateTime.Now;
									dt.BeginLoadData();
									while (dr.Read())
									{
										i++;
										if (_stopRequested) break;
										var row = dt.NewRow();
										var cols = dt.Columns.OfType<DataColumn>();
										for (int j = 0; j < cols.Count(); j++)
										{
											row[j] = dr[j] is byte[] ? Convert.ToBase64String(dr[j] as byte[]) : dr[j];
										}

										dt.Rows.Add(row);

										// if (i % 1000 == 0)
										if ((DateTime.Now - lastRefresh).TotalSeconds > 3)
										{
											lastRefresh = DateTime.Now;
											dt.EndLoadData();
											grid.RowCount += dt.Rows.Count;
											dt = dt.Clone();
											(grid.Tag as List<DataTable>).Add(dt);
											dt.BeginLoadData();
											//dt.EndLoadData();
											//dt.BeginLoadData();
											//Application.DoEvents();

											Application.DoEvents();
										}

										if (i % 100 == 0)
											Application.DoEvents();
									}
									dt.EndLoadData();

									var sum = (grid.Tag as List<DataTable>).Sum(t => t.Rows.Count);
									grid.RowCount = sum;
									totalResults += sum;

									//grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

									if (!_stopRequested)
										hasNext = dr.NextResult();
								}
							}));
						}
					}
					catch (Exception ex)
					{
						if (!ignorePrompt)
						{
							var dlgResult = PaJaMa.WinControls.YesNoMessageDialog.Show("Error: " + ex.Message + ". Would you like to continue?", "Error!",
								showNoToAll: false, showCancel: false);
							switch (dlgResult)
							{
								case WinControls.YesNoMessageDialogResult.YesToAll:
									ignorePrompt = true;
									continue;
								case WinControls.YesNoMessageDialogResult.Yes:
									continue;
							}
						}
						else
							continue;

						throw ex;
					}
				}
			}
			catch (Exception ex)
			{
				if (!ex.Message.Contains("cancelled by user"))
					errorMessage = ex.Message;
			}
			finally
			{
				_currentCommand.Dispose();
				_currentCommand = null;
				this.Invoke(new Action(() =>
				{
					lblResults.Text = totalResults == 0 ? (recordsAffected < 0 ? "Complete." : recordsAffected.ToString() + " record(s) affected") : (totalResults.ToString() + " Records.");
					lblResults.Visible = true;
					if (totalResults == 0 || !string.IsNullOrEmpty(errorMessage))
					{
						txtMessages.Text += string.IsNullOrEmpty(errorMessage) ? lblResults.Text : errorMessage;
						tabControl1.SelectedTab = tabMessages;
					}

					setDatabaseText();

					timDuration.Enabled = false;
					progMain.Visible = false;
					btnGo.Visible = true;
					cboDatabases.Enabled = true;
					btnStop.Visible = false;
					txtQuery.ReadOnly = false;
					txtQuery.Focus();
					this.Parent.Text = this.Parent.Text.Replace(" (Executing)", "");

				}));
			}
		}

		private object getDataTableObject(DataGridView grid, int rowIndex, int colIndex)
		{
			var dts = grid.Tag as List<DataTable>;
			if (dts.Count <= 0) return null;

			int dtRowIndex = rowIndex;
			int dtTableIndex = 0;
			var dt = dts[dtTableIndex];

			while (dtRowIndex >= dt.Rows.Count)
			{
				dtRowIndex -= dt.Rows.Count;
				dtTableIndex++;
				dt = dts[dtTableIndex];
			}

			return dt.Rows[dtRowIndex][colIndex];
		}

		private void grid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			var val = getDataTableObject(sender as DataGridView, e.RowIndex, e.ColumnIndex);

			if (val == DBNull.Value)
			{
				e.CellStyle.BackColor = Color.LightYellow;
				e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Italic);
			}
		}

		private void grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
		{
			e.Value = getDataTableObject(sender as DataGridView, e.RowIndex, e.ColumnIndex);

			if (e.Value == DBNull.Value)
				e.Value = "NULL";

			(sender as DataGridView).Rows[e.RowIndex].HeaderCell.Value = (e.RowIndex + 1).ToString();

			//int currPage = 0;
			//int currRow = 0;
			//var currDt = dts[currPage];
			//while (e.RowIndex > currDt.Rows.Count + currRow)
			//{
			//	currPage++;
			//	currRow += currDt.Rows.Count;
			//	currDt = dts[currPage];
			//}
		}

		public void SelectTopN(int? topN, TreeNode selectedNode)
		{
			if (!string.IsNullOrEmpty(txtQuery.Text))
			{
				var dlgResult = MessageBox.Show("Yes to overwrite, No to append.", "Append", MessageBoxButtons.YesNoCancel);
				switch (dlgResult)
				{
					case DialogResult.Cancel:
						return;
					case DialogResult.Yes:
						txtQuery.Text = string.Empty;
						break;
					case DialogResult.No:
						txtQuery.AppendText(";\r\n");
						break;
				}
			}

			if (selectedNode.Parent != null && _currentConnection.Database != selectedNode.Parent.Parent.Text)
				_currentConnection.ChangeDatabase(selectedNode.Parent.Parent.Text);

			string objName = string.Empty;
			string schemaName = string.Empty;
			string columns = string.Empty;
			string join = ",\r\n\t";
			if (_currentConnection.GetType().Equals(typeof(SqlConnection)))
				join = "],\r\n\t[";
			if (selectedNode.Tag is PaJaMa.DatabaseStudio.DatabaseObjects.View)
			{
				var view = selectedNode.Tag as PaJaMa.DatabaseStudio.DatabaseObjects.View;
				objName = view.ViewName;
				if (view.Schema != null)
					schemaName = view.Schema.SchemaName;
				columns = (join.StartsWith("]") ? "[" : "") +
					string.Join(join, view.Columns.Select(c => c.ColumnName).ToArray()) +
					 (join.StartsWith("]") ? "]" : "");
			}
			else
			{
				var tbl = selectedNode.Tag as Table;
				objName = tbl.TableName;
				if (tbl.Schema != null)
					schemaName = tbl.Schema.SchemaName;
				columns = (join.StartsWith("]") ? "[" : "") +
					string.Join(join, tbl.Columns.Select(c => c.ColumnName).ToArray()) +
					 (join.StartsWith("]") ? "]" : "");
			}

			if (_currentConnection.GetType().Name.ToLower().Contains("sqlite"))
				txtQuery.AppendText(string.Format("select * from [" + selectedNode.Text + "]{0}", topN > 0 ? " limit " + topN.ToString() : ""));
			else if (_currentConnection.GetType().Equals(typeof(SqlConnection)))
			{
				txtQuery.AppendText(string.Format("select{0}\r\n\t{4}\r\nfrom [{1}].[{2}].[{3}]", topN > 0 ? " top " + topN.ToString() : "",
					_currentConnection.Database, schemaName, objName, columns));
			}
			else
				txtQuery.AppendText(string.Format("select{0}\r\n\t{1}\r\nfrom " + selectedNode.Text, topN > 0 ? " top " + topN.ToString() : "", columns));
			// btnGo_Click(this, new EventArgs());
		}

		public void PopulateScript(string script, TreeNode selectedNode)
		{
			if (selectedNode.Parent != null && _currentConnection.Database != selectedNode.Parent.Parent.Text)
				_currentConnection.ChangeDatabase(selectedNode.Parent.Parent.Text);

			if (_currentConnection.GetType().Equals(typeof(SqlConnection)))
				txtQuery.Text = script;
		}

		private void btnStop_Click(object sender, EventArgs e)
		{
			btnStop.Enabled = false;
			_stopRequested = true;
			_currentCommand.Cancel();
		}

		private void timDuration_Tick(object sender, EventArgs e)
		{
			lblTime.Text = (DateTime.Now - _start).TotalHours.ToString("00") + ":" + (DateTime.Now - _start).Minutes.ToString("00") + ":" +
				(DateTime.Now - _start).Seconds.ToString("00");
		}

		private void cboDatabases_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lock) return;
			try
			{
				_currentConnection.ChangeDatabase(cboDatabases.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
				_lock = true;
				cboDatabases.Text = _currentConnection.Database;
				_lock = false;
			}
		}

		private void txtQuery_KeyDown(object sender, KeyEventArgs e)
		{
			if ((e.KeyCode == Keys.E && e.Modifiers == Keys.Control) || e.KeyCode == Keys.F5)
			{
				btnGo_Click(sender, e);
				e.Handled = true;
			}
			//else if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
			//	(sender as TextBox).SelectAll();
		}
	}
}
