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
	public class DatabaseSynchronization
	{
		private Database _database;
		public DatabaseSynchronization(Database database)
		{
			_database = database;
			_database.PopulateChildren(false, null);
		}

		private List<Table> getSortedTables()
		{
			var tbls = (from s in _database.Schemas
						from t in s.Tables
						select t).ToList();

			var sorted = new List<Table>();
			while (tbls.Count > 0)
			{
				for (int i = tbls.Count - 1; i >= 0; i--)
				{
					var tbl = tbls[i];
					//if (!tbls.Any(t => t.ForeignKeys.Any(fk => fk.ParentTable.TableName == tbl.TableName && fk.ParentTable.Schema.SchemaName == tbl.Schema.SchemaName)))
					//{
					//	sorted.Add(tbl);
					//	tbls.RemoveAt(i);
					//	break;
					//}

					if (!tbl.ForeignKeys.Any(fk => !sorted.Any(s => s.TableName == fk.ParentTable.TableName && s.Schema.SchemaName == fk.ParentTable.Schema.SchemaName)))
					{
						sorted.Add(tbl);
						tbls.RemoveAt(i);
						break;
					}
				}
			}

			return sorted;
		}

		public string GetCreateScript(string targetDatabaseName)
		{
			if (string.IsNullOrEmpty(targetDatabaseName))
				targetDatabaseName = _database.DatabaseName;

			var sb = new StringBuilder();
			sb.AppendLineFormat("USE master\r\nGO\r\n\r\n", targetDatabaseName);

			sb.AppendLineFormat("IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '{0}')\r\nDROP DATABASE [{0}]\r\nGO\r\n\r\n", targetDatabaseName);

			sb.AppendLineFormat("CREATE DATABASE [{0}]\r\nGO\r\n\r\nUSE [{0}]\r\nGO\r\n\r\n", targetDatabaseName);

			var objs = _database.GetDatabaseObjects(true);
			var sorted = new List<DatabaseObjectBase>();
			//var schemas = objs.OfType<Schema>().Where(s =>
			//	s.Tables.Any() || s.RoutinesSynonyms.Any() || s.Views.Any());

			var schemas = objs.OfType<Schema>().Where(s =>
				!objs.OfType<DatabasePrincipal>().Any(dp => dp.PrincipalName == s.SchemaName && dp.IsFixedRole) && !"|guest|sys|".Contains("|" + s.SchemaName + "|"));

			sorted.AddRange(objs.OfType<DatabasePrincipal>().Where(dp => !dp.IsFixedRole && !"|INFORMATION_SCHEMA|dbo|".Contains("|" + dp.PrincipalName + "|")));
			sorted.AddRange(schemas);

			var tbls = getSortedTables();
			sorted.AddRange(tbls);
			//foreach (var t in tbls)
			//{
			//	sorted.AddRange(t.Triggers);
			//}
			sorted.AddRange(objs.OfType<RoutineSynonym>());
			sorted.AddRange(objs.OfType<View>());

			sorted.AddRange(objs.OfType<Permission>());


			foreach (var obj in sorted)
			{
				var sync = DatabaseObjectSynchronizationBase.GetSynchronization(obj);
				var rawText = sync.GetRawCreateText(true);
				sb.AppendLine(rawText);
				sb.AppendLine("GO\r\n\r\n");
			}

			return sb.ToString();
		}
	}
}
