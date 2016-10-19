using PaJaMa.DatabaseStudio.DatabaseObjects;
using PaJaMa.DatabaseStudio.DataGenerate.Classes;
using PaJaMa.DatabaseStudio.DataGenerate.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DataGenerate.Classes
{
	public class TableWorkspace
	{
		private GeneratorHelper _generatorHelper;
		public Table Table { get; set; }

		public List<ColumnWorkspace> ColumnWorkspaces { get; private set; }

		public int CurrentRowCount { get; private set; }

		private int _addRowCount;
		public int AddRowCount
		{
			get { return _addRowCount; }
			set
			{
				_addRowCount = value;
				//if (!_select)
				//	_removeAddKeys = false;
			}
		}

		private bool _delete;
		public bool Delete
		{
			get { return _delete; }
			set
			{
				_delete = value;
				if (_delete)
					_truncate = false;
			}
		}

		private bool _truncate;
		public bool Truncate
		{
			get { return _truncate; }
			set
			{
				_truncate = value;
				if (_truncate)
				{
					_delete = false;
					_removeAddKeys = true;
				}
			}
		}

		private bool _removeAddKeys;
		public bool RemoveAddKeys
		{
			get { return _removeAddKeys; }
			set
			{
				_removeAddKeys = value;
				//if (_removeAddKeys)
				//	SelectTableForData = true;
			}
		}

		public TableWorkspace(GeneratorHelper generatorHelper, Table table)
		{
			_generatorHelper = generatorHelper;
			Table = table;
			var tables = new List<Table>();
			using (var conn = new SqlConnection(generatorHelper.Database.ConnectionString))
			{
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = string.Format("select count(*) from [{0}].[{1}]", table.Schema.SchemaName, table.TableName);
					conn.Open();
					CurrentRowCount = (int)cmd.ExecuteScalar();
					conn.Close();
				}
			}
			ColumnWorkspaces = Table.Columns.OrderBy(c => c.OrdinalPosition).Select(c => new ColumnWorkspace(c)).ToList();
		}

		public override string ToString()
		{
			return Table.TableName;
		}
	}

	public class TableWorkspaceList
	{
		public List<TableWorkspace> Workspaces { get; private set; }

		public TableWorkspaceList()
		{
			Workspaces = new List<TableWorkspace>();
		}

		public static TableWorkspaceList GetTableWorkspaces(GeneratorHelper generatorHelper)
		{
			var lst = new TableWorkspaceList();

			var tbls = (from s in generatorHelper.Database.Schemas
						from t in s.Tables
						select t).ToList();

			foreach (var tbl in tbls)
			{
				lst.Workspaces.Add(new TableWorkspace(generatorHelper, tbl));
			}

			return lst;
		}
	}
}
