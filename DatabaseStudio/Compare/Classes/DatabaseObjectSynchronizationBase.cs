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
	public abstract class DatabaseObjectSynchronizationBase
	{
		protected DatabaseObjectBase databaseObject { get; private set; }

		public DatabaseObjectSynchronizationBase(DatabaseObjectBase obj)
		{
			databaseObject = obj;
		}

		public virtual List<SynchronizationItem> GetSynchronizationItems(DatabaseObjectBase target)
		{
			if (target == null)
				return GetCreateItems();

			return GetAlterItems(target);
		}

		public virtual List<SynchronizationItem> GetDropItems()
		{
			return getStandardDropItems(string.Format("DROP {0} [{1}]", databaseObject.ObjectType.ToString(), databaseObject.ObjectName));
		}

		public abstract List<SynchronizationItem> GetCreateItems();
		public virtual List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var items = GetCreateItems();
			var dropItem = items.FirstOrDefault();
			if (dropItem == null)
			{
				dropItem = new SynchronizationItem(databaseObject);
				items.Insert(0, dropItem);
			}

			var diff = dropItem.Differences.FirstOrDefault();
			if (diff != null && diff.PropertyName == Difference.CREATE)
				dropItem.Differences.Remove(diff);

			dropItem.Differences.AddRange(GetPropertyDifferences(target));
			dropItem.AddScript(0, GetRawDropText());
			return items;
		}

		public List<Difference> GetPropertyDifferences(DatabaseObjectBase target)
		{
			if (target == null)
				return new List<Difference>() { new Difference() { PropertyName = Difference.CREATE } };

			var diff = new List<Difference>();
			foreach (var propInf in databaseObject.GetType().GetProperties())
			{
				if (propInf.Name == "ObjectName" || propInf.Name == "Description" || propInf.Name == "ObjectType") continue;

				var type = propInf.PropertyType;

				// nullable
				if (type.GetGenericArguments().Any())
					type = type.GetGenericArguments().First();

				if (!type.IsPrimitive && !type.Equals(typeof(string)) && !type.IsSubclassOf(typeof(DatabaseObjectBase)))
					continue;

				if (propInf.HasAttribute<IgnoreAttribute>())
					continue;

				var targetVal = propInf.GetValue(target);
				var sourceVal = propInf.GetValue(databaseObject);

				if (targetVal is DatabaseObjectBase)
					targetVal = (targetVal as DatabaseObjectBase).ObjectName;

				if (sourceVal is DatabaseObjectBase)
					sourceVal = (sourceVal as DatabaseObjectBase).ObjectName;

				if (targetVal == null && sourceVal == null)
					continue;

				if (targetVal != null && sourceVal != null)
				{
					if (propInf.HasAttribute<IgnoreCaseAttribute>())
					{
						sourceVal = sourceVal.ToString().ToLower().Trim();
						targetVal = targetVal.ToString().ToLower().Trim();
					}

					if (targetVal.Equals(sourceVal))
						continue;
				}

				diff.Add(new Difference()
				{
					PropertyName = propInf.Name,
					SourceValue = sourceVal == null ? string.Empty : sourceVal.ToString(),
					TargetValue = targetVal == null ? string.Empty : targetVal.ToString()
				});
			}

			return diff;
		}

		protected List<SynchronizationItem> getStandardDropItems(string script, int level = 0, string propertyName = Difference.DROP)
		{
			return getStandardItems(script, level, propertyName);
		}

		protected List<SynchronizationItem> getStandardItems(string script, int level = 4, string propertyName = Difference.CREATE)
		{
			var item = new SynchronizationItem(databaseObject);
			item.Differences.Add(new Difference() { PropertyName = propertyName });
			item.AddScript(level, script);
			return new List<SynchronizationItem>() { item };
		}

		public virtual List<DatabaseObjectBase> GetMissingDependencies(List<DatabaseObjectBase> existingTargetObjects, List<SynchronizationItem> selectedItems,
			bool isForDrop)
		{
			return new List<DatabaseObjectBase>();
		}

		public string GetRawCreateText(bool insertGo = false)
		{
			string rawText = string.Join((insertGo ? "\r\nGO\r\n\r\n" : "\r\n"), from i in GetCreateItems()
																				 from kvp in i.Scripts
																				 where kvp.Value.Length > 0
																				 orderby (int)kvp.Key
																				 select kvp.Value);

			if (databaseObject is DatabaseObjectWithExtendedProperties)
			{
				foreach (var ep in (databaseObject as DatabaseObjectWithExtendedProperties).ExtendedProperties)
				{
					rawText += (insertGo ? "\r\nGO\r\n\r\n" : "\r\n") + new ExtendedPropertySynchronization(ep).GetRawCreateText();
				}
			}

			return rawText.Trim();
		}

		public virtual string GetRawDropText()
		{
			string rawText = string.Join("\r\n", from i in GetDropItems()
												 from kvp in i.Scripts
												 where kvp.Value.Length > 0
												 orderby (int)kvp.Key
												 select kvp.Value);

			return rawText.Trim();
		}

		public static DatabaseObjectSynchronizationBase GetSynchronization(object forObject)
		{
			var genericType = typeof(DatabaseObjectSynchronizationBase<>).MakeGenericType(forObject.GetType());
			var type = (from t in typeof(DatabaseObjectSynchronizationBase).Assembly.GetTypes()
						where t.IsSubclassOf(genericType)
						select t).First();

			return Activator.CreateInstance(type, forObject) as DatabaseObjectSynchronizationBase;
		}
	}

	public abstract class DatabaseObjectSynchronizationBase<TDatabaseObject> : DatabaseObjectSynchronizationBase
		where TDatabaseObject : DatabaseObjectBase
	{
		protected new TDatabaseObject databaseObject { get; private set; }

		public DatabaseObjectSynchronizationBase(TDatabaseObject obj)
			: base(obj)
		{
			databaseObject = obj;
		}
	}
}
