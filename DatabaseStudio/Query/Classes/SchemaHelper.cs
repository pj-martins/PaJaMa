using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Query.Classes
{
	public class NonSqlServerSchemaHelper
	{
		public static List<Table> GetTables(DbConnection connection, string database = null)
		{
			List<Table> tables = new List<Table>();
			using (var conn = (DbConnection)Activator.CreateInstance(connection.GetType()))
			{
				conn.ConnectionString = connection.ConnectionString;
				conn.Open();
				if (!string.IsNullOrEmpty(database))
					conn.ChangeDatabase(database);
				tables = GetTablesExistingConnection(conn);
				conn.Close();
			}
			return tables.OrderBy(t => t.TableName).ToList();
		}

		public static List<Table> GetTablesExistingConnection(DbConnection connection)
		{
			List<Table> tables = new List<Table>();
			var dt = connection.GetSchema("Columns");
			foreach (var dr in dt.Rows.OfType<DataRow>())
			{
				var tableName = dr["TABLE_NAME"].ToString();
				if (dt.Columns.Contains("TABLE_SCHEM"))
					tableName = dr["TABLE_SCHEM"].ToString() + "." + tableName;
				var table = tables.FirstOrDefault(t => t.TableName == tableName);
				if (table == null)
				{
					table = new Table();
					table.TableName = tableName;
					tables.Add(table);
				}

				var col = new Column();
				col.ColumnName = dr["COLUMN_NAME"].ToString();
				col.DataType = dr["DATA_TYPE"].ToString();
				col.IsNullable = "yes|true".Contains(dr["IS_NULLABLE"].ToString().ToLower());
				table.Columns.Add(col);
			}
			return tables.OrderBy(t => t.TableName).ToList();
		}
	}
}
