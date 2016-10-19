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
	public class DatabasePrincipalSynchronization : DatabaseObjectSynchronizationBase<DatabasePrincipal>
	{
		public DatabasePrincipalSynchronization(DatabasePrincipal principal)
			: base(principal)
		{
		}


		public override List<SynchronizationItem> GetDropItems()
		{
			if (databaseObject.PrincipalType == PrincipalType.DatabaseRole)
			{
				var items = new List<SynchronizationItem>();
				SynchronizationItem item;
				StringBuilder sb = new StringBuilder();
				foreach (var child in databaseObject.ChildMembers)
				{
					item = new SynchronizationItem(child);
					item.Differences.Add(new Difference() { PropertyName = "Drop - " + child.ObjectName });
					item.AddScript(1, string.Format("ALTER ROLE [{0}] DROP MEMBER [{1}]", databaseObject.ObjectName, child.ObjectName));
					items.Add(item);
				}
				item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference() { PropertyName = "Drop - " + databaseObject.ObjectName });
				item.AddScript(2, string.Format("DROP ROLE [{0}]", databaseObject.ObjectName));
				items.Add(item);
				return items;
			}
			return getStandardDropItems(string.Format("DROP USER [{0}]", databaseObject.ObjectName));
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			if (databaseObject.PrincipalType == PrincipalType.DatabaseRole)
			{
				var sb = new StringBuilder();
				sb.AppendLineFormat(@"CREATE ROLE [{0}] {1}", databaseObject.PrincipalName, databaseObject.Owner == null ? string.Empty :
					string.Format("AUTHORIZATION [{0}]", databaseObject.Owner.ObjectName));
				foreach (var dp in databaseObject.ChildMembers)
				{
					sb.AppendLineFormat("ALTER ROLE [{0}] ADD MEMBER [{1}]", databaseObject.ObjectName, dp.ObjectName);
				}

				return getStandardItems(sb.ToString());
			}

			return getStandardItems(getLoginScript(true));
		}

		private string getLoginScript(bool create)
		{
			return create ?
				string.Format(
						(databaseObject.AuthenticationType == AuthenticationType.NONE ?
						@"CREATE USER [{0}] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[{2}]" :
						 @"CREATE USER [{0}] FOR LOGIN [{1}] WITH DEFAULT_SCHEMA=[{2}]")
						, databaseObject.PrincipalName, databaseObject.LoginName, databaseObject.DefaultSchema) :
						string.Format(@"ALTER USER [{0}] WITH LOGIN = [{1}], DEFAULT_SCHEMA=[{2}]", databaseObject.PrincipalName,
						databaseObject.LoginName, databaseObject.DefaultSchema);
		}

		public override List<SynchronizationItem> GetSynchronizationItems(DatabaseObjectBase target)
		{
			var items = new List<SynchronizationItem>();

			if (databaseObject.PrincipalType != PrincipalType.DatabaseRole)
			{
				if (target == null)
					return base.GetSynchronizationItems(target);

				var diffs = GetPropertyDifferences(target);
				if (diffs.Any())
				{
					if (target != null && databaseObject.PrincipalName == "dbo")
					{
						var item = new SynchronizationItem(databaseObject);
						item.Differences.AddRange(diffs);
						item.AddScript(2, string.Format("sp_changedbowner '{0}'", databaseObject.LoginName));
						return new List<SynchronizationItem>() { item };
					}
					else
					{
						var item = new SynchronizationItem(databaseObject);
						item.Differences.AddRange(diffs);
						item.AddScript(2, string.Format("ALTER USER [{0}] WITH DEFAULT_SCHEMA = [{1}]{2}", databaseObject.PrincipalName,
							databaseObject.DefaultSchema, string.IsNullOrEmpty(databaseObject.LoginName) ?
							string.Empty : string.Format(", LOGIN = [{0}]", databaseObject.LoginName)));
						return new List<SynchronizationItem>() { item };
					}
				}

				return new List<SynchronizationItem>();
			}

			var dp = target as DatabasePrincipal;
			if (dp == null)
			{
				var item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference() { PropertyName = Difference.CREATE });

				if (!item.Scripts.ContainsKey(7))
					item.Scripts.Add(7, new StringBuilder());

				item.AddScript(7, string.Format(@"CREATE ROLE [{0}] {1}", databaseObject.PrincipalName, databaseObject.Owner == null ? string.Empty :
						string.Format("AUTHORIZATION [{0}]", databaseObject.Owner.ObjectName)));

				items.Add(item);
			}
			else
			{
				if (databaseObject.Owner.PrincipalName != dp.Owner.PrincipalName)
				{
					var item = new SynchronizationItem(databaseObject);

					item.Differences.Add(new Difference()
					{
						PropertyName = "Owner",
						SourceValue = databaseObject.Owner.PrincipalName,
						TargetValue = dp.Owner.PrincipalName
					});

					if (!item.Scripts.ContainsKey(7))
						item.Scripts.Add(7, new StringBuilder());

					item.AddScript(7, string.Format("ALTER AUTHORIZATION ON ROLE::[{0}] TO [{1}]", databaseObject.ObjectName, databaseObject.Owner.PrincipalName));
					items.Add(item);
				}

				var drops = dp.ChildMembers.Where(m => !databaseObject.ChildMembers.Any(x => x.ObjectName == m.ObjectName));
				foreach (var drop in drops)
				{
					var item = new SynchronizationItem(databaseObject);

					item.Differences.Add(new Difference()
					{
						PropertyName = "Member",
						SourceValue = "Drop",
						TargetValue = drop.ObjectName
					});

					if (!item.Scripts.ContainsKey(7))
						item.Scripts.Add(7, new StringBuilder());

					item.AddScript(7, string.Format("ALTER ROLE [{0}] DROP MEMBER [{1}]", databaseObject.ObjectName, drop.ObjectName));
					items.Add(item);
				}
			}

			var creates = databaseObject.ChildMembers.Where(m => target == null || !dp.ChildMembers.Any(x => x.ObjectName == m.ObjectName));
			foreach (var create in creates)
			{
				var item = new SynchronizationItem(databaseObject);

				item.Differences.Add(new Difference()
				{
					PropertyName = "Member",
					SourceValue = "Create",
					TargetValue = create.ObjectName
				});

				if (!item.Scripts.ContainsKey(7))
					item.Scripts.Add(7, new StringBuilder());

				item.AddScript(7, string.Format("ALTER ROLE [{0}] ADD MEMBER [{1}]", databaseObject.ObjectName, create.ObjectName));
				items.Add(item);
			}

			return items;
		}

		public override List<DatabaseObjectBase> GetMissingDependencies(List<DatabaseObjectBase> existingTargetObjects, List<SynchronizationItem> selectedItems,
			bool isForDrop)
		{
			var missing = new List<DatabaseObjectBase>();
			var checks = new List<DatabaseObjectBase>();
			//if (isForDrop)
			//{
			//	//if (this.ObjectType == Classes.ObjectType.DatabaseRole)
			//	//{
			//	//	if (this.Owner != null)
			//	//		checks.Add(this.Owner);
			//	//}
			//	//else
			//	{
			//		checks.AddRange(Ownings.ToList());
			//	}
			//}
			//else
			{
				checks = databaseObject.ChildMembers.OfType<DatabaseObjectBase>().ToList();
				if (databaseObject.Owner != null)
					checks.Add(databaseObject.Owner);

				if (!string.IsNullOrEmpty(databaseObject.LoginName))
				{
					var slogin = databaseObject.Database.ServerLogins.FirstOrDefault(l => l.LoginName == databaseObject.LoginName);
					if (slogin != null)
						checks.Add(slogin);
				}
			}

			foreach (var child in checks)
			{
				//if ("|dbo|".Contains(child.ObjectName))
				//	continue;

				if (!existingTargetObjects.OfType<DatabaseObjectBase>().Any(o => o.ObjectType == child.ObjectType && o.ObjectName == child.ObjectName)
					&& !selectedItems.Select(i => i.DatabaseObject).OfType<DatabaseObjectBase>().Any(o => o.ObjectType == child.ObjectType && o.ObjectName == child.ObjectName))
				{
					//if (child is DatabasePrincipal && (child as DatabasePrincipal).SynchronizationItem.Omit)
					//	continue;

					missing.Add(child);
				}
			}
			return missing;
		}
	}
}
