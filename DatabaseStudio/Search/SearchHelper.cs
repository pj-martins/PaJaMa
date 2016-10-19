using PaJaMa.DatabaseStudio.DatabaseObjects;
using PaJaMa.DatabaseStudio.Search.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Search
{
	public class SearchHelper
	{
		public Database Database { get; set; }
		public SearchHelper(string connectionString, BackgroundWorker worker)
		{
			Database = new Database(connectionString);
			Database.PopulateChildren(true, worker);
		}

		public void Init(BackgroundWorker worker)
		{
			Database = new Database(Database.ConnectionString);
			Database.PopulateChildren(true, worker);
		}

		public void Search(string searchFor, List<ColumnWorkspace> columns, BindingList<DataTable> outputTables)
		{
			if (string.IsNullOrEmpty(searchFor)) return;
			if (!columns.Any()) return;

			var tblsToSearch = from c in columns
							   group c by c.Column.Table into g
							   select g;

			using (var conn = new SqlConnection(Database.ConnectionString))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					foreach (var tbl in tblsToSearch)
					{
						cmd.Parameters.Clear();

						var dt = new DataTable(tbl.Key.TableName);
						var sb = new StringBuilder(string.Format("select * from [{0}].[{1}]", tbl.Key.Schema.SchemaName, tbl.Key.TableName));
						var firstIn = true;
						foreach (var col in tbl)
						{
							sb.AppendLine(firstIn ? "where " : "and ");
							sb.AppendLine(string.Format("[{0}] = @{0}", col.Column.ColumnName));
							cmd.Parameters.AddWithValue(string.Format("@{0}", col.Column.ColumnName), searchFor);

							firstIn = false;
						}
						cmd.CommandText = sb.ToString();
						using (var rdr = cmd.ExecuteReader())
						{
							dt.Load(rdr);
						}
						outputTables.Add(dt);
					}
				}
				conn.Close();
				SqlConnection.ClearPool(conn);
			}
		}
	}
}
