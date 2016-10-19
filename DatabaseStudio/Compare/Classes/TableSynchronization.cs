using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Classes
{
	public class TableSynchronization : DatabaseObjectSynchronizationBase<Table>
	{
		private List<string> _createdColumns;
		private List<string> _alteredColumns;
		
		public TableSynchronization(Table table) : base(table)
		{
		}

		private StringBuilder getColumnCreates()
		{
			_createdColumns = new List<string>();

			var sb = new StringBuilder();
			bool firstIn = true;
			foreach (var col in databaseObject.Columns.OrderBy(c => c.OrdinalPosition))
			{
				_createdColumns.Add(col.ColumnName);
				if (!string.IsNullOrEmpty(col.Formula))
				{
					sb.AppendLine("\t" + (firstIn ? string.Empty : ",") + string.Format("[{0}] AS {1}",
						col.ColumnName,
						col.Formula));

					firstIn = false;
					continue;
				}

				string part2 = new ColumnSynchronization(col).GetPostScript();
				string def = new ColumnSynchronization(col).GetDefaultScript();

				sb.AppendLine("\t" + (firstIn ? string.Empty : ",") + string.Format("[{0}] [{1}]{2} {3} {4}",
					col.ColumnName,
					col.DataType,
					part2,
					col.IsNullable ? "NULL" : "NOT NULL",
					def));
				firstIn = false;
			}
			return sb;
		}

		private string getCreateScript()
		{
			var sb = new StringBuilder();
			sb.AppendLineFormat("CREATE TABLE [{0}].[{1}](", databaseObject.Schema.SchemaName, databaseObject.TableName);
			sb.AppendLine(getColumnCreates().ToString());
			foreach (var kc in databaseObject.KeyConstraints)
			{
				sb.AppendLine(", " + new KeyConstraintSynchronization(kc).GetInnerCreateText());
			}
			sb.AppendLine(")");
			return sb.ToString();
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			var items = new List<SynchronizationItem>();
			var item = new SynchronizationItem(databaseObject);
			items.Add(item);
			item.Differences.Add(new Difference() { PropertyName = Difference.CREATE });
			item.AddScript(2, getCreateScript());

			foreach (var fk in databaseObject.ForeignKeys)
			{
				items.AddRange(new ForeignKeySynchronization(fk).GetCreateItems());
			}

			items.AddRange(getIndexCreateUpdateItems(null));
			items.AddRange(getDefaultConstraintCreateUpdateItems(null));
			items.AddRange(getTriggerUpdateItems(null));
			foreach (var column in databaseObject.Columns)
			{
				items.AddRange(ExtendedPropertySynchronization.GetExtendedProperties(column, null));
			}

			return items;
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var targetTable = target as Table;

			var items = new List<SynchronizationItem>();


			bool recreate = false;

			foreach (var fc in databaseObject.Columns)
			{
				var tc = targetTable.Columns.FirstOrDefault(c => c.ColumnName.ToLower() == fc.ColumnName.ToLower());
				if (tc != null && tc.IsIdentity != fc.IsIdentity)
				{
					var item = getRecreateTableItem(targetTable);
					item.Differences.Add(new Difference()
					{
						PropertyName = "IsIdentity",
						SourceValue = fc.IsIdentity.ToString(),
						TargetValue = tc.IsIdentity.ToString()
					});
					items.Add(item);
					recreate = true;
					targetTable = null;
					break;
				}

				if (tc != null && tc.DataType == "timestamp")
				{
					var diff = new ColumnSynchronization(fc).GetPropertyDifferences(tc);
					if (diff.Any())
					{
						var item = getRecreateTableItem(targetTable);
						item.Differences.AddRange(diff);
						items.Add(item);
						recreate = true;
						targetTable = null;
						break;
					}
				}
			}

			if (!recreate)
			{
				foreach (var tk in targetTable.ForeignKeys)
				{
					if (!databaseObject.ForeignKeys.Any(k => k.ForeignKeyName.ToLower() == tk.ForeignKeyName.ToLower()))
					{
						items.AddRange(new ForeignKeySynchronization(tk).GetDropItems());
					}
				}

				foreach (var fk in databaseObject.ForeignKeys)
				{
					var tk = targetTable.ForeignKeys.FirstOrDefault(x => x.ObjectName == fk.ObjectName);
					if (tk != null)
					{
						items.AddRange(new ForeignKeySynchronization(fk).GetSynchronizationItems(tk));
					}
					else
					{
						items.AddRange(new ForeignKeySynchronization(fk).GetCreateItems());
					}
				}

				foreach (var tc in targetTable.Columns)
				{
					if (!databaseObject.Columns.Any(c => c.ColumnName.ToLower() == tc.ColumnName.ToLower()))
					{
						items.AddRange(new ColumnSynchronization(tc).GetDropItems());
					}
				}

				_createdColumns = new List<string>();
				_alteredColumns = new List<string>();

				foreach (var fc in databaseObject.Columns)
				{
					var tc = targetTable.Columns.FirstOrDefault(c => c.ColumnName.ToLower() == fc.ColumnName.ToLower());
					if (tc == null)
					{
						items.AddRange(new ColumnSynchronization(fc).GetAddAlterItems(null));
						_createdColumns.Add(fc.ColumnName);
					}
					else
					{
						var alteredItems = new ColumnSynchronization(fc).GetSynchronizationItems(tc);
						if (alteredItems.Any())
						{
							items.AddRange(alteredItems);
							_alteredColumns.Add(fc.ColumnName);
						}
					}
				}
			}

			if (!recreate)
				items.AddRange(getKeyConstraintUpdateItems(targetTable));

			items.AddRange(getIndexCreateUpdateItems(targetTable));
			items.AddRange(getDefaultConstraintCreateUpdateItems(targetTable));
			items.AddRange(getTriggerUpdateItems(targetTable));
			foreach (var column in databaseObject.Columns)
			{
				items.AddRange(ExtendedPropertySynchronization.GetExtendedProperties(column, targetTable == null ? null : targetTable.Columns.FirstOrDefault(c => c.ColumnName == column.ColumnName)));
			}
			return items;
		}

		//public override List<Difference> GetDifferences(DatabaseObjectBase target)
		//{
		//	return (from si in GetSynchronizationItems(target)
		//			from d in si.Differences
		//			select d).ToList();
		//}

		//public override List<Difference> GetDifferences(DatabaseObjectBase target)
		//{
		//	var diffs = base.GetDifferences(target);
		//	foreach (var col in databaseObject.Columns)
		//	{
		//		diffs.AddRange(new ColumnSynchronization(col).GetDifferences(target == null ? null : (target as Table).Columns.FirstOrDefault(x => x.ColumnName == col.ColumnName)));
		//	}

		//	foreach (var i in databaseObject.Indexes)
		//	{
		//		diffs.AddRange(new IndexSynchronization(i).GetDifferences(target == null ? null : (target as Table).Indexes.FirstOrDefault(x => x.IndexName == i.IndexName)));
		//	}

		//	foreach (var kc in databaseObject.KeyConstraints)
		//	{
		//		diffs.AddRange(new KeyConstraintSynchronization(kc).GetDifferences(target == null ? null : (target as Table).KeyConstraints.FirstOrDefault(x => x.ConstraintName == kc.ConstraintName)));
		//	}

		//	foreach (var fk in databaseObject.ForeignKeys)
		//	{
		//		diffs.AddRange(new ForeignKeySynchronization(fk).GetDifferences(target == null ? null : (target as Table).ForeignKeys.FirstOrDefault(x => x.ForeignKeyName == fk.ForeignKeyName)));
		//	}

		//	foreach (var dc in databaseObject.DefaultConstraints)
		//	{
		//		diffs.AddRange(new DefaultConstraintSynchronization(dc).GetDifferences(target == null ? null : (target as Table).DefaultConstraints.FirstOrDefault(x => x.ConstraintName == dc.ConstraintName)));
		//	}

		//	foreach (var t in databaseObject.Triggers)
		//	{
		//		diffs.AddRange(new TriggerSynchronization(t).GetDifferences(target == null ? null : (target as Table).Triggers.FirstOrDefault(x => x.TriggerName == t.TriggerName)));
		//	}
		//	return diffs;
		//}

		private SynchronizationItem getRecreateTableItem(Table targetTable)
		{
			var item = new SynchronizationItem(databaseObject);
			var tmpTable = "#tmp" + Guid.NewGuid().ToString().Replace("-", "_");
			item.AddScript(0, string.Format("SELECT * INTO {0} FROM [{1}].[{2}]",
				tmpTable,
				databaseObject.Schema.SchemaName,
				databaseObject.TableName
			));

			var foreignKeys = from s in targetTable.Schema.Database.Schemas
							  from t in s.Tables
							  from fk in t.ForeignKeys
							  where fk.ParentTable.ObjectName == databaseObject.ObjectName
							  || fk.ChildTable.ObjectName == databaseObject.ObjectName
							  select fk;

			foreach (var fk in foreignKeys)
			{
				item.AddScript(4, new ForeignKeySynchronization(fk).GetDropItems().ToString());
			}

			item.AddScript(4, string.Format("DROP TABLE [{0}].[{1}]", databaseObject.Schema.SchemaName,
				databaseObject.TableName));
			item.AddScript(5, getCreateScript());

			if (databaseObject.Columns.Any(c => c.IsIdentity))
				item.AddScript(6, string.Format("SET IDENTITY_INSERT [{0}].[{1}] ON", databaseObject.Schema.SchemaName, databaseObject.TableName));

			item.AddScript(6, string.Format("INSERT INTO [{0}].[{1}] ({2}) SELECT {2} FROM [{3}]",
				databaseObject.Schema.SchemaName,
				databaseObject.TableName,
				string.Join(",", databaseObject.Columns.Where(c => c.DataType != "timestamp").Select(c => "[" + c.ColumnName + "]").ToArray()),
				tmpTable));

			if (databaseObject.Columns.Any(c => c.IsIdentity))
				item.AddScript(6, string.Format("SET IDENTITY_INSERT [{0}].[{1}] OFF", databaseObject.Schema.SchemaName, databaseObject.TableName));

			foreach (var fk in foreignKeys)
			{
				foreach (var i in new ForeignKeySynchronization(fk).GetCreateItems())
				{
					foreach (var kvp in i.Scripts.Where(s => s.Value.Length > 0))
					{
						item.AddScript(7, kvp.Value.ToString());
					}
				}
			}

			item.AddScript(7, string.Format("DROP TABLE [{0}]", tmpTable));


			return item;
		}

		private List<SynchronizationItem> getKeyConstraintUpdateItems(Table targetTable)
		{
			var items = new List<SynchronizationItem>();

			var skips = new List<string>();

			if (targetTable != null)
			{
				foreach (var toConstraint in targetTable.KeyConstraints)
				{
					bool drop = false;
					Difference diff = null;
					var fromConstraint = databaseObject.KeyConstraints.FirstOrDefault(c => c.ConstraintName == toConstraint.ConstraintName);
					if (fromConstraint == null)
						drop = true;
					else if (fromConstraint.Columns.Any(c => !toConstraint.Columns.Any(t => t.ColumnName == c.ColumnName
						&& t.Ordinal == c.Ordinal && t.Descending == c.Descending)) ||
							toConstraint.Columns.Any(c => !fromConstraint.Columns.Any(t => t.ColumnName == c.ColumnName
						&& t.Ordinal == c.Ordinal && t.Descending == c.Descending)))
						diff = new Difference()
						{
							PropertyName = "Columns",
							SourceValue = string.Join(", ", fromConstraint.Columns.OrderBy(ic => ic.Ordinal).Select(ic => ic.ColumnName + " (" + (ic.Descending ? "DESC" : "ASC") + ")").ToArray()),
							TargetValue = string.Join(", ", toConstraint.Columns.OrderBy(ic => ic.Ordinal).Select(ic => ic.ColumnName + " (" + (ic.Descending ? "DESC" : "ASC") + ")").ToArray())
						};
					else if (toConstraint.Columns.Any(t => _alteredColumns.Contains(t.ColumnName)))
						drop = true;

					if (diff != null && toConstraint.IsPrimaryKey)
					{
						var item = new SynchronizationItem(toConstraint);
						items.Add(item);
						item.Differences.Add(diff);
						foreach (var constraintItem in new KeyConstraintSynchronization(fromConstraint).GetAlterItems(toConstraint))
						{
							foreach (var script in constraintItem.Scripts)
							{
								item.AddScript(script.Key, script.Value.ToString());
							}
						}
						//item.AddScript(1, toConstraint.GetRawDropText());
						//item.AddScript(7, fromConstraint.GetRawCreateText());
					}

					if (drop)
						items.AddRange(new KeyConstraintSynchronization(toConstraint).GetDropItems());
					else
						skips.Add(toConstraint.ConstraintName);
				}
			}

			var target = targetTable == null ? databaseObject : targetTable;
			foreach (var fromConstraint in databaseObject.KeyConstraints)
			{
				if (skips.Contains(fromConstraint.ConstraintName))
					continue;


				var item = new SynchronizationItem(fromConstraint);
				item.Differences.Add(new Difference() { PropertyName = Difference.CREATE });
				item.AddScript(7, new KeyConstraintSynchronization(fromConstraint).GetRawCreateText());
				items.Add(item);
			}

			return items;
		}

		private List<SynchronizationItem> getIndexCreateUpdateItems(Table targetTable)
		{
			var items = new List<SynchronizationItem>();

			var skips = new List<string>();

			if (targetTable != null)
			{
				foreach (var toIndex in targetTable.Indexes)
				{
					bool drop = false;
					Difference diff = null;
					var fromIndex = databaseObject.Indexes.FirstOrDefault(c => c.IndexName == toIndex.IndexName);
					if (fromIndex == null)
						drop = true;
					else if (fromIndex.IndexColumns.Any(c => !toIndex.IndexColumns.Any(t => t.ColumnName == c.ColumnName
						&& t.Ordinal == c.Ordinal && t.Descending == c.Descending)) ||
							toIndex.IndexColumns.Any(c => !fromIndex.IndexColumns.Any(t => t.ColumnName == c.ColumnName
						&& t.Ordinal == c.Ordinal && t.Descending == c.Descending)))
						diff = new Difference()
						{
							PropertyName = "Columns",
							SourceValue = string.Join(", ", fromIndex.IndexColumns.OrderBy(ic => ic.Ordinal).Select(ic => ic.ColumnName).ToArray()),
							TargetValue = string.Join(", ", toIndex.IndexColumns.OrderBy(ic => ic.Ordinal).Select(ic => ic.ColumnName).ToArray())
						};
					else if (toIndex.IndexColumns.Any(t => _alteredColumns.Contains(t.ColumnName)))
						drop = true;

					if (diff != null)
					{
						var item = new SynchronizationItem(toIndex);
						items.Add(item);
						item.Differences.Add(diff);
						item.AddScript(1, new IndexSynchronization(toIndex).GetRawDropText());
						item.AddScript(7, new IndexSynchronization(fromIndex).GetCreateScript(targetTable != null).ToString());
					}

					if (drop)
						items.AddRange(new IndexSynchronization(toIndex).GetDropItems());
					else
						skips.Add(toIndex.IndexName);
				}
			}

			var target = targetTable == null ? databaseObject : targetTable;
			foreach (var fromIndex in databaseObject.Indexes)
			{
				if (skips.Contains(fromIndex.IndexName))
					continue;


				var item = new SynchronizationItem(fromIndex);
				item.Differences.Add(new Difference() { PropertyName = Difference.CREATE });
				item.AddScript(7, new IndexSynchronization(fromIndex).GetCreateScript(targetTable != null).ToString());
				items.Add(item);
			}

			return items;
		}

		private List<SynchronizationItem> getDefaultConstraintCreateUpdateItems(Table targetTable)
		{
			var skips = new List<string>();
			var items = new List<SynchronizationItem>();

			if (targetTable != null)
			{
				foreach (var toConstraint in targetTable.DefaultConstraints)
				{
					Difference diff = null;
					bool drop = false;
					var fromConstraint = databaseObject.DefaultConstraints.FirstOrDefault(c => c.Table.TableName == databaseObject.TableName &&
						c.ConstraintName == toConstraint.ConstraintName);
					if (fromConstraint == null)
						drop = true;
					else if (fromConstraint.Column.ColumnName != toConstraint.Column.ColumnName ||
							fromConstraint.ColumnDefault.Replace("((", "(").Replace("))", ")")
								!= toConstraint.ColumnDefault.Replace("((", "(").Replace("))", ")"))
					{
						diff = new Difference()
						{
							PropertyName = "ColumnDefault",
							SourceValue = fromConstraint.ColumnDefault,
							TargetValue = toConstraint.ColumnDefault
						};

						var creates = new DefaultConstraintSynchronization(fromConstraint).GetCreateItems();
						var item = creates.First();
						item.Differences.Add(diff);

						// handled on column add
						if (_createdColumns.Contains(fromConstraint.Column.ColumnName))
							item.Scripts.Clear();

						item.AddScript(0, new DefaultConstraintSynchronization(toConstraint).GetRawDropText());
						items.Add(item);
					}

					if (drop)
					{
						// table was dropped
						items.AddRange(new DefaultConstraintSynchronization(toConstraint).GetDropItems());
					}
					else
						skips.Add(toConstraint.ConstraintName);
				}
			}

			foreach (var fromConstraint in databaseObject.DefaultConstraints)
			{
				if (skips.Contains(fromConstraint.ConstraintName))
					continue;

				if (_createdColumns.Contains(fromConstraint.Column.ObjectName))
					continue;

				items.AddRange(new DefaultConstraintSynchronization(fromConstraint).GetCreateItems());
			}

			return items;
		}

		private List<SynchronizationItem> getTriggerUpdateItems(Table targetTable)
		{
			var skips = new List<string>();
			var items = new List<SynchronizationItem>();

			if (targetTable != null)
			{
				foreach (var toTrigger in targetTable.Triggers)
				{
					bool drop = false;
					var fromTrigger = databaseObject.Triggers.FirstOrDefault(c => c.TriggerName == toTrigger.TriggerName);
					if (fromTrigger == null)
						drop = true;
					else
						items.AddRange(new TriggerSynchronization(fromTrigger).GetSynchronizationItems(toTrigger));

					if (drop)
						items.AddRange(new TriggerSynchronization(toTrigger).GetDropItems());
					else
						skips.Add(toTrigger.TriggerName);
				}
			}

			foreach (var fromTrigger in databaseObject.Triggers)
			{
				if (skips.Contains(fromTrigger.TriggerName))
					continue;

				items.AddRange(new TriggerSynchronization(fromTrigger).GetCreateItems());
			}

			return items;
		}


		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("DROP TABLE [{0}].[{1}]", databaseObject.Schema.SchemaName, databaseObject.TableName));
		}

		public override List<DatabaseObjectBase> GetMissingDependencies(List<DatabaseObjectBase> existingTargetObjects, List<SynchronizationItem> selectedItems,
			bool isForDrop)
		{
			if (isForDrop)
				return base.GetMissingDependencies(existingTargetObjects, selectedItems, isForDrop);

			var toTbls = existingTargetObjects.OfType<Table>();

			List<DatabaseObjectBase> missing = new List<DatabaseObjectBase>();
			foreach (var fk in databaseObject.ForeignKeys)
			{
				var toTbl = toTbls.FirstOrDefault(t => t.TableName == fk.ParentTable.TableName);
				if (toTbl != null)
				{
					if (fk.Columns.Any(k => !toTbl.KeyConstraints.Any(kc => kc.Columns.Any(c => c.ColumnName == k.ParentColumn.ColumnName))))
					{
						if (!selectedItems.Select(si => si.DatabaseObject).OfType<KeyConstraint>().Any(kc => kc.Table.TableName == fk.ParentTable.ObjectName))
							missing.Add(fk.ParentTable);
					}
					continue;
				}

				var item = (from si in selectedItems
							where si.ObjectName == fk.ParentTable.ObjectName
								&& si.DatabaseObject is Table
							select si).FirstOrDefault();

				if (item != null && !item.Omit)
					continue;

				missing.Add(fk.ParentTable);
			}

			if (!existingTargetObjects.OfType<Schema>().Any(s => s.SchemaName == databaseObject.Schema.SchemaName))
			{
				var item = (from si in selectedItems
							where si.DatabaseObject is Schema
							where si.ObjectName == databaseObject.Schema.SchemaName
							select si).FirstOrDefault();

				if (item == null || item.Omit)
					missing.Add(databaseObject.Schema);
			}

			foreach (var trig in databaseObject.Triggers)
			{
				var toTrig = from tt in toTbls
							 where tt.TableName != databaseObject.TableName && tt.Schema.SchemaName == databaseObject.Schema.SchemaName
							 from tr in tt.Triggers
							 where tr.TriggerName == trig.TriggerName
							 select tr;

				foreach (var tr in toTrig)
				{
					var selectedItem = (from item in selectedItems
										where !item.Omit
										&& item.DatabaseObject.Equals(tr)
										select item).FirstOrDefault();
					if (selectedItem == null)
						missing.Add(tr.Table);
				}
			}

			return missing;
		}
	}
}
