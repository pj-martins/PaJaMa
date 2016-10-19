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

namespace PaJaMa.DatabaseStudio.Search.Classes
{
	public class TableWorkspace
	{
		private SearchHelper _serachHelper;
		public Table Table { get; set; }

		public List<ColumnWorkspace> ColumnWorkspaces { get; private set; }

		public bool? SelectAll
		{
			get
			{
				//if (!ColumnWorkspaces.Any(c => c.Select))
				//	return false;
				//if (ColumnWorkspaces.All(c => c.Select))
				//	return true;
				//return null;

				return ColumnWorkspaces.Any(c => c.Select);
			}
			set
			{
				foreach (var col in ColumnWorkspaces)
				{
					col.Select = true;
				}
			}
		}

		public TableWorkspace(SearchHelper searchHelper, Table table)
		{
			_serachHelper = searchHelper;
			Table = table;
			var tables = new List<Table>();
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

		public static TableWorkspaceList GetTableWorkspaces(SearchHelper searchHelper)
		{
			var lst = new TableWorkspaceList();

			var tbls = (from s in searchHelper.Database.Schemas
						from t in s.Tables
						select t).ToList();

			foreach (var tbl in tbls)
			{
				lst.Workspaces.Add(new TableWorkspace(searchHelper, tbl));
			}

			return lst;
		}
	}
}
