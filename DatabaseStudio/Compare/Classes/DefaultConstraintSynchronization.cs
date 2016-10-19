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
	public class DefaultConstraintSynchronization : DatabaseObjectSynchronizationBase<DefaultConstraint>
	{
		public DefaultConstraintSynchronization(DefaultConstraint constraint)
			: base(constraint)
		{
		}


		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("ALTER TABLE [{0}].[{1}] DROP CONSTRAINT [{2}]", databaseObject.Table.Schema.SchemaName,
				databaseObject.Table.TableName, databaseObject.ConstraintName));
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			string def = databaseObject.ColumnDefault;
			if (!string.IsNullOrEmpty(def) && def.StartsWith("((") && def.EndsWith("))"))
				def = def.Substring(1, def.Length - 2);

			return getStandardItems(string.Format(@"ALTER TABLE [{0}].[{1}] ADD  CONSTRAINT [{2}]  DEFAULT {3} FOR [{4}]",
				databaseObject.Table.Schema.SchemaName, databaseObject.Table.TableName, databaseObject.ConstraintName, def, databaseObject.Column.ColumnName), 7);
		}

		//public override SynchronizationItem GetSynchronizationItem(DatabaseObjectBase target)
		//{
		//	var item = base.GetSynchronizationItem(target);
		//	if (ColumnDefault != (target as Constraint).ColumnDefault)
		//	{
		//		if (item == null)
		//			item = new SynchronizationItem(this);

		//		item.Differences.Add(new Difference()
		//			{
		//				PropertyName = "ColumnDefault",
		//				SourceValue = ColumnDefault,
		//				TargetValue = (target as Constraint).ColumnDefault
		//			});
		//	}
		//	return item;
		//}
	}
}
