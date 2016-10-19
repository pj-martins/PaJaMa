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
	public class ViewSynchronization : DatabaseObjectSynchronizationBase<View>
	{
		public ViewSynchronization(View view)
			: base(view)
		{
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var items = new List<SynchronizationItem>();
			var diffs = GetPropertyDifferences(target);
			if (diffs.Any())
			{
				var createAlter = databaseObject.Definition;
				createAlter = Regex.Replace(createAlter, "CREATE VIEW", "ALTER VIEW", RegexOptions.IgnoreCase);
				items.AddRange(getStandardItems(createAlter, propertyName: Difference.ALTER));
			}

			return items;
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(databaseObject.Definition);
		}

		public override string ToString()
		{
			return databaseObject.Schema.SchemaName + "." + databaseObject.ViewName;
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("DROP {0} [{1}].[{2}]", databaseObject.ObjectType.ToString().ToUpper(),
				databaseObject.Schema.SchemaName, databaseObject.ObjectName));
		}
	}
}
