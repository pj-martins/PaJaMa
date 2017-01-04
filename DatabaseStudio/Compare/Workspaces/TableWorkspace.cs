using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Workspaces
{
	public class TableWorkspace : WorkspaceWithSourceBase
	{
		public const int DEFAULT_BATCH_SIZE = 5000;
		private CompareHelper _compareHelper;

		public Table SourceTable { get { return SourceObject as Table; } }
		public Table TargetTable { get { return TargetObject as Table; } }

		public override DatabaseObjectBase SourceObject
		{
			get { return base.SourceObject; }
			set
			{
				base.SourceObject = value;
				_delete = _truncate = _selectTableForData = _keepIdentity = _removeAddKeys = false;
			}
		}

		private bool _selectTableForData;
		public bool SelectTableForData
		{
			get { return _selectTableForData; }
			set
			{
				_selectTableForData = value;
				if (!_selectTableForData)
					_keepIdentity = _removeAddKeys = false;
				else if (TransferBatchSize == 0)
					TransferBatchSize = DEFAULT_BATCH_SIZE;
				if (TargetObject == null)
					Select = true;
			}
		}

		private bool _delete;
		public bool Delete
		{
			get { return _delete; }
			set
			{
				if (TargetObject == null)
					value = false;
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
				if (TargetObject == null)
					value = false;
				_truncate = value;
				if (_truncate)
				{
					_delete = false;
					_removeAddKeys = true;
				}
			}
		}

		private bool _keepIdentity;
		public bool KeepIdentity
		{
			get { return _keepIdentity; }
			set
			{
				if (TargetObject == null)
					value = false;
				_keepIdentity = value;
				if (_keepIdentity)
					SelectTableForData = true;
			}
		}

		private bool _removeAddKeys;
		public bool RemoveAddKeys
		{
			get { return _removeAddKeys; }
			set
			{
				if (TargetObject == null)
					value = false;
				_removeAddKeys = value;
				//if (_removeAddKeys)
				//	SelectTableForData = true;
			}
		}

		public bool RemoveAddIndexes { get; set; }
		public int TransferBatchSize { get; set; }

		public DataTableWithSchema ComparedData { get; set; }

		public TableWorkspace(CompareHelper compareHelper, Table sourceTable, Table targetTable)
			: base(sourceTable, targetTable)
		{
			_compareHelper = compareHelper;
		}

		public override string ToString()
		{
			return SourceObject.ObjectName;
		}
	}

	public class SerializableTableWorkspace
	{
		public string SourceSchemaTableName { get; set; }
		public string TargetSchemaTableName { get; set; }
		public bool SelectTableForStructure { get; set; }
		public bool SelectTableForData { get; set; }
		public bool Delete { get; set; }
		public bool Truncate { get; set; }
		public bool KeepIdentity { get; set; }
		public bool RemoveAddKeys { get; set; }
		public bool RemoveAddIndexes { get; set; }
		public int TransferBatchSize { get; set; }
		public List<SerializableSynchronizationItem> SynchronizationItems { get; set; }

		public static SerializableTableWorkspace GetFromTableWorkspace(TableWorkspace ws)
		{
			return new SerializableTableWorkspace()
			{
				SourceSchemaTableName = ws.SourceTable.ToString(),
				TargetSchemaTableName = ws.TargetTable == null ? string.Empty : ws.TargetTable.ToString(),
				SelectTableForStructure = ws.Select,
				SelectTableForData = ws.SelectTableForData,
				Delete = ws.Delete,
				Truncate = ws.Truncate,
				KeepIdentity = ws.KeepIdentity,
				RemoveAddKeys = ws.RemoveAddKeys,
				RemoveAddIndexes = ws.RemoveAddIndexes,
				TransferBatchSize = ws.TransferBatchSize,
				SynchronizationItems = ws.SynchronizationItems.Select(si =>
					new SerializableSynchronizationItem()
					{
						ObjectName = si.ObjectName,
						Omit = si.Omit
					}).ToList()
			};
		}
	}

	public class TableWorkspaceList
	{
		public List<TableWorkspace> Workspaces { get; private set; }
		public List<DropWorkspace> DropWorkspaces { get; private set; }

		public TableWorkspaceList()
		{
			Workspaces = new List<TableWorkspace>();
			DropWorkspaces = new List<DropWorkspace>();
		}

		public static TableWorkspaceList GetTableWorkspaces(CompareHelper compareHelper)
		{
			var lst = new TableWorkspaceList();

			var fromTbls = (from s in compareHelper.FromDatabase.Schemas
							from t in s.Tables
							select t).ToList();

			var toTbls = (from s in compareHelper.ToDatabase.Schemas
						  from t in s.Tables
						  select t).ToList();

			foreach (var tbl in fromTbls)
			{
				Table sourceTable = tbl;
				Table targetTable = toTbls.FirstOrDefault(t => t.TableName == tbl.TableName && t.Schema.SchemaName == tbl.Schema.SchemaName);
				lst.Workspaces.Add(new TableWorkspace(compareHelper, sourceTable, targetTable));
			}

			foreach (var table in toTbls
				.Where(x => !fromTbls.Any(t => t.TableName == x.TableName && t.Schema.SchemaName == x.Schema.SchemaName)))
			{
				lst.DropWorkspaces.Add(new DropWorkspace(table));
			}

			return lst;
		}
	}
}
