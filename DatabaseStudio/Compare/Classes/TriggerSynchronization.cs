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
	public class TriggerSynchronization : DatabaseObjectSynchronizationBase<Trigger>
	{
		public TriggerSynchronization(Trigger trigger)
			: base(trigger)
		{
		}

		private int getIndex()
		{
			return databaseObject.Table.Triggers.IndexOf(databaseObject) + 1;
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("DROP TRIGGER [{0}].[{1}]", databaseObject.Table.Schema.SchemaName, databaseObject.TriggerName), level: 11 * getIndex());
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			var items = getStandardItems(databaseObject.Definition.Trim(), level: 10 * getIndex());
			if (databaseObject.Disabled)
			{
				var item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference() { PropertyName = "Disabled" });
				item.AddScript(11 * getIndex(), string.Format("DISABLE TRIGGER [{0}] ON [{1}]", databaseObject.TriggerName, databaseObject.Table.TableName));
				items.Add(item);
			}
			return items;
		}

		public override List<SynchronizationItem> GetSynchronizationItems(DatabaseObjectBase target)
		{
			if (target == null)
				return base.GetSynchronizationItems(target);

			if (GetRawCreateText().ToLower() == new TriggerSynchronization(target as Trigger).GetRawCreateText().ToLower()) return new List<SynchronizationItem>();

			var targetTrigger = target as Trigger;

			if (targetTrigger.Table.TableName != databaseObject.Table.TableName)
				return base.GetSynchronizationItems(target);

			var items = new List<SynchronizationItem>();

			var item = new SynchronizationItem(databaseObject);
			item.Differences.Add(new Difference() { PropertyName = target == null ? Difference.CREATE : Difference.ALTER });

			var createAlter = databaseObject.Definition.Trim();
			if (target != null)
			{
				var targetDef = targetTrigger.Definition.Trim();

				// it was only disabled
				if (targetDef.ToLower() == createAlter.ToLower())
					createAlter = string.Empty;
				else
					createAlter = Regex.Replace(createAlter, "CREATE TRIGGER", "ALTER TRIGGER", RegexOptions.IgnoreCase);
			}

			if (!string.IsNullOrEmpty(createAlter))
			{
				item.AddScript(10 * getIndex(), createAlter);
				items.Add(item);
			}

			if (databaseObject.Disabled)
			{
				item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference() { PropertyName = "Disabled", SourceValue = "false", TargetValue = "true" });
				item.AddScript(11 * getIndex(), string.Format("DISABLE TRIGGER [{0}] ON [{1}]", databaseObject.TriggerName, databaseObject.Table.TableName));
				items.Add(item);
			}
			else if (target != null && databaseObject.Disabled != (target as Trigger).Disabled)
			{
				item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference()
				{
					PropertyName = "Disabled",
					SourceValue = databaseObject.Disabled.ToString(),
					TargetValue = (target as Trigger).Disabled.ToString()
				});
				item.AddScript(11 * getIndex(), string.Format("{2} TRIGGER [{0}] ON [{1}]", databaseObject.TriggerName, databaseObject.Table.TableName, databaseObject.Disabled ? "DISABLE" : "ENABLE"));
				items.Add(item);
			}

			return items;
		}

	}
}
