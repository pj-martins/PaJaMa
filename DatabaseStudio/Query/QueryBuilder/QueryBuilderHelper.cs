using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Query.QueryBuilder
{
	public class QueryBuilderHelper
	{
		public SqlConnection Connection { get; private set; }

		public QueryBuilderHelper(string connectionString, string currentDatabase)
		{
			Connection = new SqlConnection(connectionString);
			if (!string.IsNullOrEmpty(currentDatabase))
			{
				Connection.Open();
				Connection.ChangeDatabase(currentDatabase);
				Connection.Close();
			}
		}

		public List<string> GetDatabases()
		{
			List<string> databases = new List<string>();
			using (var cmd = Connection.CreateCommand())
			{
				cmd.CommandText = "select name from sys.databases order by name";
				Connection.Open();
				using (var rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						databases.Add(rdr["name"].ToString());
					}
				}
				Connection.Close();
			}
			return databases;
		}

		public Dictionary<string, List<string>> GetTablesColumns(string databaseName)
		{
			Dictionary<string, List<string>> tables = new Dictionary<string, List<string>>();
			using (var cmd = Connection.CreateCommand())
			{
				cmd.CommandText = "select TABLE_NAME, COLUMN_NAME from " + databaseName + ".INFORMATION_SCHEMA.COLUMNS order by TABLE_NAME";
				Connection.Open();
				using (var rdr = cmd.ExecuteReader())
				{
					while (rdr.Read())
					{
						string tableName = rdr["TABLE_NAME"].ToString();
						if (!tables.ContainsKey(tableName))
							tables.Add(tableName, new List<string>());
						tables[tableName].Add(rdr["COLUMN_NAME"].ToString());
					}
				}
				Connection.Close();
			}
			return tables;
		}
	}
}
