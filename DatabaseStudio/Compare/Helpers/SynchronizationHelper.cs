using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare.Helpers
{
	public class SynchronizationHelper
	{
		public static bool Synchronize(CompareHelper compareHelper, List<WorkspaceBase> workspaces, List<TableWorkspace> dataSpaces, 
			List<TableWorkspace> truncDelete, BackgroundWorker worker)
		{
			TransferHelper transferHelper = null;
			if (dataSpaces.Any())
				transferHelper = new TransferHelper(worker);
			using (var conn = new SqlConnection(compareHelper.ToDatabase.ConnectionString))
			{
				conn.Open();
				using (var trans = conn.BeginTransaction())
				{
					int i = 0;
					
					try
					{
						//if (!truncDelete.All(x => x.SelectTableForData && !x.Select))
						{
							using (var cmd = conn.CreateCommand())
							{
								cmd.Transaction = trans;

								foreach (var table in truncDelete)
								{
									table.TargetTable.ResetForeignKeys();
								}

								i = 0;
								foreach (var table in truncDelete)
								{
									if (table.RemoveAddKeys)
									{
										worker.ReportProgress(100 * i / truncDelete.Count, "Removing foreign keys for " + table.TargetTable.TableName);
										table.TargetTable.RemoveForeignKeys(cmd);
									}
								}

								i = 0;
								foreach (var table in truncDelete)
								{
									worker.ReportProgress(100 * i / truncDelete.Count, "Truncating/Deleting " + table.TargetTable.TableName);
									table.TargetTable.TruncateDelete(cmd, table.Truncate);
									i++;
								}

								i = 0;
								foreach (var table in truncDelete.Where(td => td.Select && !td.SelectTableForData))
								{
									i++;
									if (table.RemoveAddKeys)
									{
										worker.ReportProgress(100 * i / truncDelete.Count, "Adding foreign keys for " + table.TargetTable.TableName);
										table.TargetTable.AddForeignKeys(cmd);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						trans.Rollback();

						foreach (var table in truncDelete)
						{
							table.TargetTable.ResetForeignKeys();
						}

						MessageBox.Show("Failed to synchronize: " + ex.Message);
						return false;
					}

					if (workspaces.Any() && !compareHelper.Synchronize(worker, workspaces, trans))
					{
						trans.Rollback();
						return false;
					}

					if (dataSpaces.Any() && !transferHelper.Transfer(dataSpaces, trans, compareHelper.FromDatabase.ConnectionString))
					{
						trans.Rollback();
						return false;
					}

					var adds = truncDelete.Where(td => !td.Select);
					if (adds.Any())
					{
						try
						{
							using (var cmd = conn.CreateCommand())
							{
								cmd.Transaction = trans;

								i = 0;
								foreach (var table in adds)
								{
									i++;
									if (table.RemoveAddKeys)
									{
										worker.ReportProgress(100 * i / adds.Count(), "Adding foreign keys for " + table.TargetTable.TableName);
										table.TargetTable.AddForeignKeys(cmd);
									}
								}
							}
						}
						catch (Exception ex)
						{
							trans.Rollback();

							foreach (var table in truncDelete)
							{
								table.TargetTable.ResetForeignKeys();
							}

							MessageBox.Show("Failed to synchronize: " + ex.Message);
							return false;
						}
					}

					trans.Commit();
				}
				conn.Close();
				SqlConnection.ClearPool(conn);
			}

			return true;
		}
	}
}
