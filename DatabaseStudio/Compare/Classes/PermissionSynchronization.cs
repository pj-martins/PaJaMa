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
	public class PermissionSynchronization : DatabaseObjectSynchronizationBase<Permission>
	{
		public PermissionSynchronization(Permission permission)
			: base(permission)
		{
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var princ in databaseObject.PermissionPrincipals)
			{
				sb.AppendLine(princ.GetCreateRemoveScript(false));
			}
			return getStandardDropItems(sb.ToString());
		}

		private string getCreateScript()
		{
			StringBuilder sb = new StringBuilder();
			foreach (var princ in databaseObject.PermissionPrincipals)
			{
				sb.AppendLine(princ.GetCreateRemoveScript(true));
			}
			return sb.ToString();
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(getCreateScript());
		}

		public override List<DatabaseObjectBase> GetMissingDependencies(List<DatabaseObjectBase> existingTargetObjects, List<SynchronizationItem> selectedItems,
			bool isForDrop)
		{
			var match = Regex.Match(GetRawCreateText(), @"ON SCHEMA::\[(.*?)\]");
			if (match.Success)
			{
				var target = existingTargetObjects.OfType<Schema>().FirstOrDefault(t => t.ObjectName == match.Groups[1].Value);

				if (target == null)
					target = selectedItems.Select(i => i.DatabaseObject).OfType<Schema>().FirstOrDefault(t => t.ObjectName == match.Groups[1].Value);

				if (target == null)
					return new List<DatabaseObjectBase>() { databaseObject.Database.Schemas.First(d => d.ObjectName == match.Groups[1].Value) };
			}
			else
			{
				var missingPrincipals = new List<DatabaseObjectBase>();

				foreach (var pp in databaseObject.PermissionPrincipals)
				{
					if (!existingTargetObjects.OfType<DatabasePrincipal>().Any(dp => dp.PrincipalName == pp.DatbasePrincipal.PrincipalName)
						&& !selectedItems.Select(i => i.DatabaseObject).OfType<DatabasePrincipal>().Any(dp => dp.PrincipalName == pp.DatbasePrincipal.PrincipalName))
						missingPrincipals.Add(pp.DatbasePrincipal);
				}

				if (missingPrincipals.Any())
					return missingPrincipals;
			}

			return base.GetMissingDependencies(existingTargetObjects, selectedItems, isForDrop);
		}

		public override List<SynchronizationItem> GetSynchronizationItems(DatabaseObjectBase target)
		{
			if (target == null)
				return base.GetSynchronizationItems(target);

			var items = new List<SynchronizationItem>();
			var targetPermission = target as Permission;
			List<PermissionPrincipal> skips = new List<PermissionPrincipal>();
			foreach (var pp in databaseObject.PermissionPrincipals)
			{
				var tpp = targetPermission.PermissionPrincipals.FirstOrDefault(p => pp.IsEqual(p));
				if (tpp == null)
				{
					var item = new SynchronizationItem(databaseObject);
					item.Differences.Add(new Difference() { PropertyName = Difference.CREATE });
					item.AddScript(2, pp.GetCreateRemoveScript(true));
					items.Add(item);
					skips.Add(pp);
				}
			}

			foreach (var pp in targetPermission.PermissionPrincipals)
			{
				if (skips.Any(s => s.PermissionType == pp.PermissionType && s.DatbasePrincipal.PrincipalName == pp.DatbasePrincipal.PrincipalName))
					continue;

				var tpp = databaseObject.PermissionPrincipals.FirstOrDefault(p => pp.IsEqual(p));
				if (tpp == null)
				{
					var item = new SynchronizationItem(databaseObject);
					item.Differences.Add(new Difference() { PropertyName = Difference.DROP });
					item.AddScript(2, pp.GetCreateRemoveScript(false));
					items.Add(item);
				}
			}

			return items;
		}
	}
}
