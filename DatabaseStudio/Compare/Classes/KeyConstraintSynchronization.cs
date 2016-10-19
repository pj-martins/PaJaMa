using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Classes
{
	public class KeyConstraintSynchronization : DatabaseObjectSynchronizationBase<KeyConstraint>
	{
		public KeyConstraintSynchronization(KeyConstraint constraint)
			: base(constraint)
		{
		}


		public string GetInnerCreateText()
		{
			return string.Format(@"CONSTRAINT [{0}]
{1}
{2}
({3})", databaseObject.ConstraintName, databaseObject.IsPrimaryKey ? "PRIMARY KEY" : "UNIQUE", databaseObject.ClusteredNonClustered, string.Join(", ",
	  databaseObject.Columns.OrderBy(c => c.Ordinal).Select(c => string.Format("[{0}] {1}", c.ColumnName, c.Descending ? "DESC" : "ASC"))));
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(string.Format("ALTER TABLE [{0}].[{1}] ADD {2}", databaseObject.Table.Schema.SchemaName, databaseObject.Table.TableName, GetInnerCreateText()));
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("ALTER TABLE [{0}].[{1}] DROP CONSTRAINT [{2}]", databaseObject.Table.Schema.SchemaName,
				databaseObject.Table.TableName, databaseObject.ConstraintName));
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var items = base.GetAlterItems(target);
			if (target != null)
			{
				var targetKey = target as KeyConstraint;
				var childKeys = from t in databaseObject.Table.Schema.Tables
								from fk in t.ForeignKeys
								where fk.ParentTable.TableName == targetKey.Table.TableName
								select fk;
				foreach (var childKey in childKeys)
				{
					var childSync = new ForeignKeySynchronization(childKey);
					var item = new SynchronizationItem(childKey);
					foreach (var dropItem in childSync.GetDropItems())
					{
						item.Differences.AddRange(dropItem.Differences);
						foreach (var script in dropItem.Scripts)
						{
							item.AddScript(-1, script.Value.ToString());
						}
					}
					foreach (var createItem in childSync.GetCreateItems())
					{
						item.Differences.AddRange(createItem.Differences);
						foreach (var script in createItem.Scripts)
						{
							item.AddScript(100, script.Value.ToString());
						}
					}
					items.Add(item);
				}
			}
			return items;
		}

		//public override List<Difference> GetDifferences(DatabaseObjectBase target)
		//{
		//	var diffs = base.GetDifferences(target);
		//	if (target == null)
		//		return diffs;

		//	var targetConstraint = target as KeyConstraint;
		//	foreach (var col in databaseObject.Columns)
		//	{
		//		var targetCol = targetConstraint.Columns.FirstOrDefault(c => c.ColumnName == col.ColumnName);
		//		if (targetCol == null)
		//			diffs.Add(new Difference() { PropertyName = Difference.CREATE });
		//		else 
		//		{if (targetCol.Ordinal != col.Ordinal)
		//			diffs.Add(new Difference)
		//		}
		//	}

		//	return diffs;
		//}
	}
}
