using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class ForeignKey : DatabaseObjectBase
	{
		public override string ObjectName
		{
			get { return ForeignKeyName; }
		}

		public string ForeignKeyName { get; set; }
		public Table ChildTable { get; set; }
		public Table ParentTable { get; set; }

		[Ignore]
		public List<ForeignKeyColumn> Columns { get; set; }
		public string UpdateRule { get; set; }
		public string DeleteRule { get; set; }
		public string WithCheck { get; set; }

		[Ignore]
		public bool HasBeenDropped { get; set; }

		public ForeignKey()
		{
			Columns = new List<ForeignKeyColumn>();
		}

		public static void PopulateKeys(Database database, DbConnection connection)
		{
			var foreignKeys = new List<ForeignKey>();

			string foreignKeyQuery = database.Is2000OrLess ? @"
select fk.CONSTRAINT_NAME as ForeignKeyName, fk.TABLE_NAME as ChildTableName, cc.COLUMN_NAME as ChildColumnName, 
	tc.TABLE_NAME as ParentTableName, tcc.COLUMN_NAME as ParentColumnName, UPDATE_RULE as UpdateRule, DELETE_RULE as DeleteRule,
	WithCheck = case when OBJECTPROPERTY(OBJECT_ID(QUOTENAME(c.CONSTRAINT_SCHEMA) + '.' + QUOTENAME(c.CONSTRAINT_NAME)), 'CnstIsNotTrusted') = 1 then 'NO' else '' end,
	tc.CONSTRAINT_SCHEMA as ParentTableSchema, fk.CONSTRAINT_SCHEMA as ChildTableSchema
from INFORMATION_SCHEMA.TABLE_CONSTRAINTS fk
join INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS c on c.CONSTRAINT_NAME = fk.CONSTRAINT_NAME
	and c.CONSTRAINT_SCHEMA = fk.CONSTRAINT_SCHEMA
join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE cc on cc.CONSTRAINT_NAME = c.CONSTRAINT_NAME
	and cc.CONSTRAINT_SCHEMA = fk.CONSTRAINT_SCHEMA
join INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc on tc.CONSTRAINT_NAME = c.UNIQUE_CONSTRAINT_NAME
join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE tcc on tcc.CONSTRAINT_NAME = tc.CONSTRAINT_NAME
	and tcc.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
WHERE fk.CONSTRAINT_TYPE = 'FOREIGN KEY'
order by ForeignKeyName, ChildTableName
" : @"
select fk.name as ForeignKeyName, ct.name as ChildTableName, cc.name as ChildColumnName, pt.name as ParentTableName, 
	pc.name as ParentColumnName, 
	replace(update_referential_action_desc, '_', ' ') as UpdateRule,
	replace(delete_referential_action_desc, '_', ' ') as DeleteRule,
	case when is_not_trusted = 1 then 'NO' else '' end as WithCheck,
	ps.name as ParentTableSchema,
	cs.name as ChildTableSchema
from sys.foreign_keys fk
join sys.tables ct on ct.object_id = fk.parent_object_id
join sys.tables pt on pt.object_id = fk.referenced_object_id
join sys.foreign_key_columns fkc on fkc.constraint_object_id = fk.object_id
join sys.all_columns cc on cc.object_id = fkc.parent_object_id
	and cc.column_id = fkc.parent_column_id
join sys.all_columns pc on pc.object_id = fkc.referenced_object_id
	and pc.column_id = fkc.referenced_column_id
join sys.schemas cs on cs.schema_id = ct.schema_id
join sys.schemas ps on ps.schema_id = pt.schema_id";

			using (var fromCmd = connection.CreateCommand())
			{
				fromCmd.CommandText = foreignKeyQuery;

				using (var rdr = fromCmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							var foreignKeyName = rdr["ForeignKeyName"].ToString();
							var childTableName = rdr["ChildTableName"].ToString();
							var parentSchema = database.Schemas.First(s => s.SchemaName == rdr["ParentTableSchema"].ToString());
							var childSchema = database.Schemas.First(s => s.SchemaName == rdr["ChildTableSchema"].ToString());
							var foreignKey = foreignKeys.FirstOrDefault(f => f.ForeignKeyName == foreignKeyName && f.ChildTable.TableName == childTableName
								&& f.ChildTable.Schema.SchemaName == childSchema.SchemaName);

							if (foreignKey == null)
							{
								foreignKey = rdr.ToObject<ForeignKey>();
								foreignKey.ParentTable = parentSchema.Tables.First(t => t.TableName == rdr["ParentTableName"].ToString());
								foreignKey.ChildTable = childSchema.Tables.First(t => t.TableName == rdr["ChildTableName"].ToString());
								foreignKey.ChildTable.ForeignKeys.Add(foreignKey);
								foreignKeys.Add(foreignKey);
							}

							foreignKey.Columns.Add(new ForeignKeyColumn()
								{
									ParentColumn = foreignKey.ParentTable.Columns.First(c => c.ColumnName == rdr["ParentColumnName"].ToString()),
									ChildColumn = foreignKey.ChildTable.Columns.First(c => c.ColumnName == rdr["ChildColumnName"].ToString())
								});
						}
					}
					rdr.Close();
				}
			}
		}
	}

	public class ForeignKeyColumn
	{
		public Column ChildColumn { get; set; }
		public Column ParentColumn { get; set; }
	}
}
