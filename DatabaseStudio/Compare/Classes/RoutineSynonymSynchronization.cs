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
	public class RoutineSynonymSynchronization : DatabaseObjectSynchronizationBase<RoutineSynonym>
	{
		public RoutineSynonymSynchronization(RoutineSynonym routineSynonym)
			: base(routineSynonym)
		{
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var items = new List<SynchronizationItem>();
			var propDifferences = GetPropertyDifferences(target);
			if (propDifferences.Any())
			{
				var createAlter = databaseObject.Definition;
				if (databaseObject.Type == PaJaMa.DatabaseStudio.DatabaseObjects.RoutineSynonym.RoutineSynonymType.Synonym)
				{
					createAlter = createAlter.Insert(0, new RoutineSynonymSynchronization(target as RoutineSynonym).GetRawDropText() + "\r\n");
				}
				else
				{
					createAlter = Regex.Replace(createAlter, "CREATE PROCEDURE", "ALTER PROCEDURE", RegexOptions.IgnoreCase);
					createAlter = Regex.Replace(createAlter, "CREATE FUNCTION", "ALTER FUNCTION", RegexOptions.IgnoreCase);
					createAlter = Regex.Replace(createAlter, "CREATE VIEW", "ALTER VIEW", RegexOptions.IgnoreCase);
				}
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
			return databaseObject.Schema.SchemaName + "." + databaseObject.Name;
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("DROP {0} [{1}].[{2}]", databaseObject.ObjectType.ToString().ToUpper(), 
				databaseObject.Schema.SchemaName, databaseObject.ObjectName));
		}
	}
}
