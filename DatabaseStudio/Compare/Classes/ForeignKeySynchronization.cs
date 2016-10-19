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
	public class ForeignKeySynchronization : DatabaseObjectSynchronizationBase<ForeignKey>
	{
		public ForeignKeySynchronization(ForeignKey foreignKey)
			: base(foreignKey)
		{
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(string.Format(@"
ALTER TABLE [{0}].[{1}] WITH {9}CHECK ADD CONSTRAINT [{2}] FOREIGN KEY({3})
REFERENCES [{4}].[{5}] ({6})
ON DELETE {7}
ON UPDATE {8}

ALTER TABLE [{0}].[{1}] CHECK CONSTRAINT [{2}]
", databaseObject.ChildTable.Schema.SchemaName, databaseObject.ChildTable.TableName, databaseObject.ForeignKeyName,
		string.Join(",", databaseObject.Columns.Select(c => "[" + c.ChildColumn.ColumnName + "]").ToArray()),
		databaseObject.ParentTable.Schema.SchemaName, databaseObject.ParentTable.TableName,
		string.Join(",", databaseObject.Columns.Select(c => "[" + c.ParentColumn.ColumnName + "]").ToArray()),
	databaseObject.DeleteRule, databaseObject.UpdateRule, databaseObject.WithCheck), 7);
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format(@"
ALTER TABLE [{0}].[{1}] DROP CONSTRAINT [{2}]
", databaseObject.ChildTable.Schema.SchemaName, databaseObject.ChildTable.TableName, databaseObject.ForeignKeyName));
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var diffs = GetPropertyDifferences(target);
			var diff = getColumnDifference(target);
			if (diff != null)
				diffs.Add(diff);

			if (diffs.Any())
			{
				var syncItem = new SynchronizationItem(databaseObject);
				syncItem.Differences.AddRange(diffs);
				syncItem.AddScript(0, GetRawDropText());
				syncItem.AddScript(1, GetRawCreateText());
				return new List<SynchronizationItem>() { syncItem };
			}

			return new List<SynchronizationItem>();
		}

		private Difference getColumnDifference(DatabaseObjectBase target)
		{
			var targetFk = target as ForeignKey;
			if (targetFk.Columns.Any(f => !databaseObject.Columns.Any(c => c.ParentColumn.ColumnName == f.ParentColumn.ColumnName
				&& c.ChildColumn.ColumnName == f.ChildColumn.ColumnName))
				|| databaseObject.Columns.Any(f => !targetFk.Columns.Any(c => c.ParentColumn.ColumnName == f.ParentColumn.ColumnName
				&& c.ChildColumn.ColumnName == f.ChildColumn.ColumnName)))
				return new Difference()
				{
					PropertyName = "Columns",
					SourceValue = string.Join("\r\n", databaseObject.Columns.Select(c => c.ParentColumn.ColumnName + " - " + c.ChildColumn.ColumnName).ToArray()),
					TargetValue = string.Join("\r\n", targetFk.Columns.Select(c => c.ParentColumn.ColumnName + " - " + c.ChildColumn.ColumnName).ToArray()),
				};
			return null;
		}
	}
}
