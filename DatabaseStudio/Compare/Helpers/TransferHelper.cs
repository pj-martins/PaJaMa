using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare.Helpers
{
	public class TransferHelper
	{
		private BackgroundWorker _worker;

		public TransferHelper(BackgroundWorker worker)
		{
			_worker = worker;
		}

		public bool Transfer(List<TableWorkspace> workspaces, DbTransaction trans, string fromConnectionString)
		{
			string tableName = string.Empty;
			try
			{
				using (var cmd = trans.Connection.CreateCommand())
				{
					cmd.Transaction = trans;

					var sort = !workspaces.All(ws => ws.RemoveAddKeys);

					int i = 0;
					var selected = workspaces.Where(t => t.SelectTableForData).ToList();
					var datas = sort ? getSortedWorkspaces(selected) : selected;
					var counts = datas.Count();
					foreach (var table in datas)
					{
						i++;
						tableName = table.TargetTable.ToString();

						if (table.RemoveAddIndexes)
						{
							_worker.ReportProgress(100 * i / counts, "Removing indexes for " + tableName);
							table.TargetTable.RemoveIndexes(cmd);
						}
					}

					i = 0;

					using (var conn = new SqlConnection(fromConnectionString))
					{
						using (var cmdSrc = conn.CreateCommand())
						{
							conn.Open();
							foreach (var table in datas)
							{
								i++;

								_worker.ReportProgress(100 * i / counts, string.Format("Copying: [{0}].[{1}]",
												table.SourceTable.Schema.SchemaName, table.SourceTable.TableName));

								int rowCount = 0;
								cmdSrc.CommandText = string.Format("select count(*) from [{0}].[{1}]", table.SourceTable.Schema.SchemaName, table.SourceTable.TableName);
								rowCount = (int)cmdSrc.ExecuteScalar();
								
								cmdSrc.CommandText = string.Format("select * from [{0}].[{1}]", table.SourceTable.Schema.SchemaName, table.SourceTable.TableName);
								using (var rdr = cmdSrc.ExecuteReader())
								{
									using (var copy = new SqlBulkCopy((SqlConnection)trans.Connection, 
										(table.KeepIdentity ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default) | SqlBulkCopyOptions.CheckConstraints,
										(SqlTransaction)trans))
									{
										foreach (var col in table.SourceTable.Columns)
										{
											if (!string.IsNullOrEmpty(col.Formula))
												continue;

											if (!table.TargetTable.Columns.Any(c => c.ColumnName == col.ColumnName))
												continue;

											copy.ColumnMappings.Add(col.ObjectName, col.ObjectName);
										}

										copy.BulkCopyTimeout = 600;
										copy.BatchSize = 5000;
										copy.NotifyAfter = 500;
										copy.SqlRowsCopied += delegate(object sender, SqlRowsCopiedEventArgs e)
										{
											_worker.ReportProgress(100 * i / counts, string.Format("Copying: [{0}].[{1}] {2} of {3}", 
												table.SourceTable.Schema.SchemaName, table.SourceTable.TableName, e.RowsCopied, rowCount));
										};
										copy.DestinationTableName = string.Format("[{0}].[{1}]", table.TargetTable.Schema.SchemaName, table.TargetTable.TableName);
										copy.WriteToServer(rdr);
									}
								}
							}
						}

						conn.Close();
						SqlConnection.ClearPool(conn);
					}

					i = 0;
					foreach (var table in datas)
					{
						i++;
						tableName = table.TargetTable.TableName;

						if (table.RemoveAddIndexes)
						{
							_worker.ReportProgress(100 * i / datas.Count(), "Adding indexes for " + tableName);
							table.TargetTable.AddIndexes(cmd);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to transfer " + tableName + ": " + ex.Message);
				return false;
			}

			return true;
		}

		private List<TableWorkspace> getSortedWorkspaces(List<TableWorkspace> workspaces)
		{
			List<TableWorkspace> sortedWorkspaces = new List<TableWorkspace>();
			List<TableWorkspace> currentWorkspaces = workspaces.ToList();
			while (currentWorkspaces.Count > 0)
			{
				foreach (var ws in currentWorkspaces)
				{
					bool goodToGo = true;
					foreach (var fk in ws.TargetTable.ForeignKeys)
					{
						if (currentWorkspaces.Any(w => w.TargetTable.TableName == fk.ParentTable.TableName))
						{
							goodToGo = false;
							break;
						}
					}
					if (goodToGo)
					{
						sortedWorkspaces.Add(ws);
						currentWorkspaces.Remove(ws);
						break;
					}
				}
			}

			return sortedWorkspaces;
		}
	}
}
