using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class KeyConstraint : DatabaseObjectBase
	{
		public Table Table { get; set; }
		public string ConstraintName { get; set; }

		public override string ObjectName
		{
			get { return ConstraintName; }
		}

		[Ignore]
		public List<IndexColumn> Columns { get; set; }

		public string ClusteredNonClustered { get; set; }
		public bool IsPrimaryKey { get; set; }

		public KeyConstraint()
		{
			Columns = new List<IndexColumn>();
		}

		public static void PopulateKeys(Database database, DbConnection connection)
		{
			var constraints = new List<KeyConstraint>();

			string qry = database.Is2000OrLess ? @"
select ku.CONSTRAINT_NAME as ConstraintName, COLUMN_NAME as ColumnName, ORDINAL_POSITION as Ordinal, 
	ku.TABLE_NAME as TableName, tc.TABLE_SCHEMA as SchemaName, 'CLUSTERED' as ClusteredNonClustered, convert(bit, 1) as IsPrimaryKey, convert(bit, 0) as Descending
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS tc
INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS ku
ON tc.CONSTRAINT_TYPE = 'PRIMARY KEY' 
AND tc.CONSTRAINT_NAME = ku.CONSTRAINT_NAME
and ku.CONSTRAINT_SCHEMA = tc.CONSTRAINT_SCHEMA
"
				:
				@"
select kc.name as ConstraintName, c.name as ColumnName, key_ordinal as Ordinal, t.name as TableName, s.name as SchemaName,
	i.type_desc as ClusteredNonClustered, i.is_primary_key as IsPrimaryKey, ic.is_descending_key as Descending
from sys.key_constraints kc
join sys.tables t on t.object_id = kc.parent_object_id
join sys.schemas s on s.schema_id = t.schema_id
join sys.index_columns ic on ic.object_id = t.object_id
	and ic.index_id = kc.unique_index_id
join sys.columns c on c.column_id = ic.column_id and c.object_id = ic.object_id
join sys.indexes i on i.index_id = ic.index_id and i.object_id = ic.object_id
";

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							string constraintName = rdr["ConstraintName"].ToString();
							string tableName = rdr["TableName"].ToString();
							var schema = database.Schemas.First(s => s.SchemaName == rdr["SchemaName"].ToString());
							var constraint = constraints.FirstOrDefault(c => c.ConstraintName == constraintName && c.Table.TableName == tableName
								&& c.Table.Schema.SchemaName == schema.SchemaName);
							if (constraint == null)
							{
								constraint = rdr.ToObject<KeyConstraint>();
								constraint.Table = schema.Tables.First(t => t.TableName == rdr["TableName"].ToString());
								constraint.Table.KeyConstraints.Add(constraint);
								constraints.Add(constraint);
							}

							var col = rdr.ToObject<IndexColumn>();
							constraint.Columns.Add(col);

						}
					}
					rdr.Close();
				}
			}
		}
	}
}
