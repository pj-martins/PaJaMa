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
	public class SchemaSynchronization : DatabaseObjectSynchronizationBase<Schema>
	{
		public SchemaSynchronization(Schema schema)
			: base(schema)
		{
		}


		public override List<SynchronizationItem> GetSynchronizationItems(DatabaseObjectBase target)
		{
			if (target == null)
				return base.GetSynchronizationItems(target);

			var targetSchema = target as Schema;
			if (databaseObject.SchemaOwner != targetSchema.SchemaOwner)
			{
				var item = new SynchronizationItem(databaseObject);
				item.Differences.AddRange(GetPropertyDifferences(target));
				item.AddScript(7, string.Format(@"ALTER AUTHORIZATION ON SCHEMA::[{0}] TO [{1}]", databaseObject.SchemaName, databaseObject.SchemaOwner));

				return new List<SynchronizationItem>() { item };
			}

			return new List<SynchronizationItem>();
		}

		public override List<DatabaseObjectBase> GetMissingDependencies(List<DatabaseObjectBase> existingTargetObjects, List<SynchronizationItem> selectedItems, bool isForDrop)
		{
			if (!isForDrop)
			{
				var princ = databaseObject.Database.Principals.FirstOrDefault(p => p.PrincipalName == databaseObject.SchemaOwner);
				if (princ != null)
				{
					var targetPrinc = existingTargetObjects.OfType<DatabasePrincipal>().FirstOrDefault(p => p.PrincipalName == princ.PrincipalName);
					if (targetPrinc == null)
					{
						var selectedItem = selectedItems.FirstOrDefault(i => i.DatabaseObject is DatabasePrincipal
							&& (i.DatabaseObject as DatabasePrincipal).PrincipalName == princ.PrincipalName);
						if (selectedItem == null)
							return new List<DatabaseObjectBase>() { princ };
					}
				}
				return new List<DatabaseObjectBase>();
			}

			var missing = new List<DatabaseObjectBase>();
			var checks = databaseObject.Tables.ConvertAll(t => t as DatabaseObjectBase).ToList();
			checks.AddRange(databaseObject.RoutinesSynonyms);
			foreach (var c in checks)
			{
				if (!selectedItems.Any(i => i.DatabaseObject.Equals(c)))
					missing.Add(c);
			}
			return missing;
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(string.Format(@"CREATE SCHEMA [{0}] AUTHORIZATION [{1}]", databaseObject.SchemaName, databaseObject.SchemaOwner));
		}
	}
}
