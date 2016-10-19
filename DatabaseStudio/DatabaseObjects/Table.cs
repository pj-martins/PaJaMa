using PaJaMa.Common;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class Table : DatabaseObjectWithExtendedProperties
	{
		//private DataTable _removedKeys;

		public Schema Schema { get; set; }
		public string TableName { get; set; }

		[Ignore]
		public List<Column> Columns { get; private set; }

		[Ignore]
		public List<Index> Indexes { get; private set; }

		[Ignore]
		public List<KeyConstraint> KeyConstraints { get; private set; }

		[Ignore]
		public List<ForeignKey> ForeignKeys { get; private set; }

		[Ignore]
		public List<DefaultConstraint> DefaultConstraints { get; private set; }

		[Ignore]
		public List<Trigger> Triggers { get; private set; }

		public override string ObjectName
		{
			get { return TableName; }
		}

		public Table()
		{
			Columns = new List<Column>();
			Indexes = new List<Index>();
			KeyConstraints = new List<KeyConstraint>();
			ForeignKeys = new List<ForeignKey>();
			DefaultConstraints = new List<DefaultConstraint>();
			Triggers = new List<Trigger>();
		}

		public static void PopulateTables(Database database, DbConnection connection, List<ExtendedProperty> allExtendedProperties,
			BackgroundWorker worker)
		{
			if (worker != null) worker.ReportProgress(0, "Populating tables for " + connection.Database + "...");
			string qry = @"select TABLE_NAME, TABLE_SCHEMA from INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE'";
			var tables = new List<Table>();
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							string tableName = rdr["TABLE_NAME"].ToString();
							var table = new Table();
							table.TableName = tableName;
							table.Schema = database.Schemas.First(s => s.SchemaName == rdr["TABLE_SCHEMA"].ToString());
							table.ExtendedProperties = allExtendedProperties.Where(ep => ep.Level1Object == table.TableName && ep.ObjectSchema == table.Schema.SchemaName &&
								string.IsNullOrEmpty(ep.Level2Object)).ToList();
							table.Schema.Tables.Add(table);
						}
					}
					rdr.Close();
				}
			}


			if (worker != null) worker.ReportProgress(0, "Populating columns for " + connection.Database + "...");
			Column.PopulateColumnsForTable(database, connection, allExtendedProperties);

			if (worker != null) worker.ReportProgress(0, "Populating foreign keys for " + connection.Database + "...");
			ForeignKey.PopulateKeys(database, connection);

			if (worker != null) worker.ReportProgress(0, "Populating primary keys for " + connection.Database + "...");
			KeyConstraint.PopulateKeys(database, connection);

			if (worker != null) worker.ReportProgress(0, "Populating indexes for " + connection.Database + "...");
			Index.PopulateIndexes(database, connection);

			if (worker != null) worker.ReportProgress(0, "Populating constraints for " + connection.Database + "...");
			DefaultConstraint.PopulateConstraints(database, connection);

			if (worker != null) worker.ReportProgress(0, "Populating triggers for " + connection.Database + "...");
			Trigger.PopulateTriggers(database, connection);
		}

		private List<ForeignKey> getForeignKeys()
		{
			var fks = this.ForeignKeys.ToList();
			fks.AddRange(from s in this.Schema.Database.Schemas
						  from t in s.Tables
						  where t.TableName != this.TableName
						  from fk in t.ForeignKeys
						  where fk.ParentTable.TableName == this.TableName
						  select fk);

			return fks;
		}

		public void ResetIndexes()
		{
			foreach (var ix in Indexes)
			{
				ix.HasBeenDropped = false;
			}
		}

		public void RemoveIndexes(IDbCommand cmd)
		{
			foreach (var ix in Indexes)
			{
				cmd.CommandText = new Compare.Classes.IndexSynchronization(ix).GetRawDropText();
				cmd.ExecuteNonQuery();
				ix.HasBeenDropped = true;
			}
		}

		public void AddIndexes(IDbCommand cmd)
		{
			foreach (var ix in Indexes)
			{
				if (!ix.HasBeenDropped)
					continue;

				var items = new Compare.Classes.IndexSynchronization(ix).GetCreateItems();
				foreach (var item in items)
				{
					foreach (var script in item.Scripts.Where(s => s.Value.Length > 0).OrderBy(s => (int)s.Key))
					{
						cmd.CommandText = script.Value.ToString();
						cmd.CommandTimeout = 1200;
						cmd.ExecuteNonQuery();
					}
				}
				ix.HasBeenDropped = false;
			}
		}

		public void ResetForeignKeys()
		{
			foreach (var fk in getForeignKeys())
			{
				fk.HasBeenDropped = false;
			}
		}

		public void RemoveForeignKeys(IDbCommand cmd)
		{
			foreach (var fk in getForeignKeys())
			{
				if (fk.HasBeenDropped)
					continue;

				cmd.CommandText = new Compare.Classes.ForeignKeySynchronization(fk).GetRawDropText();
				cmd.ExecuteNonQuery();
				fk.HasBeenDropped = true;
			}
		}

		public void AddForeignKeys(IDbCommand cmd)
		{
			foreach (var fk in getForeignKeys())
			{
				if (!fk.HasBeenDropped)
					continue;

				var items = new Compare.Classes.ForeignKeySynchronization(fk).GetCreateItems();
				foreach (var item in items)
				{
					foreach (var script in item.Scripts.Where(s => s.Value.Length > 0).OrderBy(s => (int)s.Key))
					{
						cmd.CommandText = script.Value.ToString();
						cmd.CommandTimeout = 1200;
						cmd.ExecuteNonQuery();
					}
				}
				fk.HasBeenDropped = false;
			}
		}

		public void TruncateDelete(IDbCommand cmd, bool truncate)
		{
			if (truncate)
			{
				cmd.CommandText = string.Format("truncate table [{0}].[{1}]", Schema, TableName);
				cmd.ExecuteNonQuery();
			}
			else
			{
				cmd.CommandText = string.Format("delete from [{0}].[{1}]", Schema, TableName);
				cmd.ExecuteNonQuery();
			}
		}

		public override string ToString()
		{
			return Schema == null ? TableName : Schema.SchemaName + "." + TableName;
		}
	}
}
