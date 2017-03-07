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
	public class ColumnSynchronization : DatabaseObjectSynchronizationBase<Column>
	{
		public ColumnSynchronization(Column column)
			: base(column)
		{
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			return getStandardItems(string.Format("ALTER TABLE [{0}].[{1}] DROP COLUMN [{2}]",
						databaseObject.Table.Schema.SchemaName,
						databaseObject.Table.TableName,
						databaseObject.ColumnName), level: 1, propertyName: Difference.DROP);
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return GetAddAlterItems(null);
		}


		public string GetPostScript()
		{
			string part2 = string.Empty;
			if ((databaseObject.DataType == "decimal" || databaseObject.DataType == "numeric") && databaseObject.NumericPrecision != null && databaseObject.NumericScale != null)
			{
				part2 = "(" + databaseObject.NumericPrecision.ToString() + ", " + databaseObject.NumericScale.ToString() + ")";
			}
			else if (databaseObject.CharacterMaximumLength != null && databaseObject.DataType != "text" && databaseObject.DataType != "image"
				&& databaseObject.DataType != "ntext" && databaseObject.DataType != "xml")
			{
				string max = databaseObject.CharacterMaximumLength.ToString();
				if (max == "-1")
					max = "max";
				part2 = "(" + max + ")";
			}

			if (databaseObject.IsIdentity)
				part2 = string.Format(" IDENTITY(1,{0})", databaseObject.Increment.GetValueOrDefault(1));
			return part2;
		}

		public string GetDefaultScript()
		{
			string def = string.Empty;
			//if (fromCol["ColumnDefault"] != DBNull.Value && fromCol["ConstraintName"] != DBNull.Value)
			//	def = "CONSTRAINT [" + fromCol["ConstraintName"].ToString() + "] DEFAULT(" + fromCol["ColumnDefault"] + ")";
			//else if (fromCol["COLUMN_DEFAULT"] != DBNull.Value)
			//	def = "DEFAULT(" + fromCol["COLUMN_DEFAULT"] + ")";

			string colDef = databaseObject.ColumnDefault;
			if (!string.IsNullOrEmpty(colDef) && colDef.StartsWith("((") && colDef.EndsWith("))"))
				colDef = colDef.Substring(1, colDef.Length - 2);

			if (!string.IsNullOrEmpty(databaseObject.ColumnDefault) && !string.IsNullOrEmpty(databaseObject.ConstraintName))
				def = "CONSTRAINT [" + databaseObject.ConstraintName + "] DEFAULT(" + colDef + ")";
			else if (!string.IsNullOrEmpty(databaseObject.ColumnDefault))
				def = "DEFAULT(" + colDef + ")";
			return def;
		}

		public List<SynchronizationItem> GetAddAlterItems(Column targetColumn)
		{
			var items = new List<SynchronizationItem>();

			SynchronizationItem item = null;

			var sb = new StringBuilder();

			if (!string.IsNullOrEmpty(databaseObject.Formula))
			{
				if (targetColumn == null || databaseObject.Formula != targetColumn.Formula)
				{
					item = new SynchronizationItem(databaseObject);
					item.Differences.Add(new Difference() { PropertyName = "Formula", SourceValue = databaseObject.Formula, TargetValue = targetColumn == null ? string.Empty : targetColumn.Formula });
					if (targetColumn != null)
						item.AddScript(1, string.Format("ALTER TABLE [{0}].[{1}] DROP COLUMN [{2}]", databaseObject.Table.Schema.SchemaName,
							databaseObject.Table.TableName, databaseObject.ColumnName));

					item.AddScript(3, string.Format("ALTER TABLE [{0}].[{1}] ADD [{2}] AS {3}",
						databaseObject.Table.Schema.SchemaName,
						databaseObject.Table.TableName,
						databaseObject.ColumnName,
						databaseObject.Formula));

					items.Add(item);

					return items;
				}
			}

			var differences = targetColumn == null ? new List<Difference>() { new Difference() { PropertyName = Difference.CREATE } }
				: base.GetPropertyDifferences(targetColumn);

			// case mismatch
			if (targetColumn != null && targetColumn.ColumnName != databaseObject.ColumnName)
			{
				item = new SynchronizationItem(databaseObject);
				item.AddScript(2, string.Format("EXEC sp_rename '{0}.{1}.{2}', '{3}', 'COLUMN'",
							targetColumn.Table.Schema.SchemaName,
							targetColumn.Table.TableName,
							targetColumn.ColumnName,
							databaseObject.ColumnName));
				item.Differences.Add(new Difference()
				{
					PropertyName = "ColumnName",
					SourceValue = databaseObject.ColumnName,
					TargetValue = targetColumn.ColumnName
				});
				items.Add(item);
				var diff = differences.First(d => d.PropertyName == "ColumnName");
				differences.Remove(diff);
			}

			if (!differences.Any())
				return items;

			string part2 = GetPostScript();

			string def = string.Empty;

			string tempConstraint = null;

			// default constraints for existing cols need to be created after the fact
			if (targetColumn == null)
			{
				def = GetDefaultScript();

				if (!databaseObject.IsNullable && !databaseObject.IsIdentity && string.IsNullOrEmpty(def) && databaseObject.DataType != "timestamp")
				{
					// added columns to existing tables must have default so we must add a temporary one for now
					var sqlDbType = (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), 
						databaseObject.DataType == "numeric" ? "decimal" : databaseObject.DataType, true);
					var clrType = Common.DataHelper.GetClrType(sqlDbType);

					clrType = clrType.GetGenericArguments().FirstOrDefault() ?? clrType;

					tempConstraint = "constraint_" + Guid.NewGuid().ToString().Replace("-", "_");

					def = "CONSTRAINT [" + tempConstraint + "] DEFAULT({0})";

					if (clrType.Equals(typeof(string)))
						def = string.Format(def, "''");
					else if (clrType.Equals(typeof(DateTime)) || clrType.Equals(typeof(DateTimeOffset)))
						def = string.Format(def, "'1/1/1900'");
					else if (clrType.IsNumericType())
						def = string.Format(def, 0);
					else if (clrType.Equals(typeof(byte[])))
						def = string.Format(def, "0x");
					else if (clrType.Equals(typeof(bool)))
						def = string.Format(def, "0");
					else if (clrType.Equals(typeof(Guid)))
						def = string.Format(def, "'" + Guid.Empty.ToString() + "'");
					else
						throw new NotImplementedException();
				}
			}

			sb.AppendLineFormat("ALTER TABLE [{0}].[{1}] {7} [{2}] [{3}]{4} {5} {6}",
				databaseObject.Table.Schema.SchemaName,
				databaseObject.Table.TableName,
				databaseObject.ColumnName,
				databaseObject.DataType,
				part2,
				databaseObject.IsNullable ? "NULL" : "NOT NULL",
				def,
				targetColumn == null ? "ADD" : "ALTER COLUMN");

			if (!string.IsNullOrEmpty(tempConstraint))
			{
				sb.AppendLineFormat("ALTER TABLE [{0}].[{1}] {7} [{2}] [{3}]{4} {5} {6}",
				databaseObject.Table.Schema.SchemaName,
				databaseObject.Table.TableName,
				databaseObject.ColumnName,
				databaseObject.DataType,
				part2,
				databaseObject.IsNullable ? "NULL" : "NOT NULL",
				string.Empty,
				"ALTER COLUMN");

				sb.AppendLineFormat("ALTER TABLE [{0}].[{1}] DROP CONSTRAINT [{2}]", databaseObject.Table.Schema.SchemaName, databaseObject.Table.TableName, tempConstraint);
			}

			item = new SynchronizationItem(databaseObject);
			item.AddScript(2, sb.ToString());
			item.Differences.AddRange(differences);
			items.Add(item);

			var kcs = databaseObject.Table.KeyConstraints.Where(k => !k.IsPrimaryKey && k.Columns.Any(ic => ic.ColumnName == databaseObject.ColumnName));
			foreach (var kc in kcs)
			{
				var syncItem = new KeyConstraintSynchronization(kc);
				item.AddScript(0, syncItem.GetRawDropText());
				item.AddScript(10, syncItem.GetRawCreateText());
			}

			if (targetColumn != null)
			{
				var dcs = databaseObject.Table.DefaultConstraints.Where(dc => dc.Column.ColumnName == databaseObject.ColumnName);
				foreach (var dc in dcs)
				{
					var syncItem = new DefaultConstraintSynchronization(dc);
					item.AddScript(0, syncItem.GetRawDropText());
					item.AddScript(10, syncItem.GetRawCreateText());
				}
			}

			return items;
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			return GetAddAlterItems(target as Column);
		}
	}
}
