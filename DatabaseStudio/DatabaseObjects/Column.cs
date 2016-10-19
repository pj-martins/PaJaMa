using PaJaMa.Common;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class Column : DatabaseObjectWithExtendedProperties
	{
		public override string ObjectName
		{
			get { return ColumnName; }
		}

		[Ignore]
		public Table Table { get; set; }

		public string ColumnName { get; set; }

		[Ignore]
		public int OrdinalPosition { get; set; }

		public bool IsIdentity { get; set; }
		public string DataType { get; set; }
		public int? CharacterMaximumLength { get; set; }
		public bool IsNullable { get; set; }
		public string Formula { get; set; }
		public string ColumnDefault { get; set; }
		public byte? NumericPrecision { get; set; }
		public int? NumericScale { get; set; }
		public decimal? Increment { get; set; }

		[Ignore]
		public string ConstraintName { get; set; }

		public static void PopulateColumnsForTable(Database database, DbConnection connection, List<ExtendedProperty> allExtendedProperties)
		{
			string lessQry = @"select co.TABLE_NAME as TableName, COLUMN_NAME as ColumnName, ORDINAL_POSITION as OrdinalPosition, 
	CHARACTER_MAXIMUM_LENGTH as CharacterMaximumLength, DATA_TYPE as DataType,
    IsNullable = convert(bit, case when UPPER(ltrim(rtrim(co.IS_NULLABLE))) = 'YES' then 1 else 0 end), convert(bit, COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity')) as IsIdentity, d.name as ConstraintName,
	COLUMN_DEFAULT as ColumnDefault, null as Formula, NUMERIC_PRECISION as NumericPrecision, NUMERIC_SCALE as NumericScale,
	SchemaName = co.TABLE_SCHEMA, IDENT_INCR(co.TABLE_SCHEMA + '.' + TABLE_NAME) AS Increment
from INFORMATION_SCHEMA.COLUMNS co
join syscolumns c on c.name = co.column_name
join sysobjects t on t.id = c.id
	and t.name = co.TABLE_NAME
left join
(
	select dc.colid, d.name, d.parent_obj
	from sysconstraints dc
	join sysobjects d on d.id = dc.constid
) d
on d.colid = c.colid and d.parent_obj = t.id
where t.xtype = 'U'";

			string moreQry = @"select TABLE_NAME as TableName, COLUMN_NAME as ColumnName, ORDINAL_POSITION as OrdinalPosition, 
	CHARACTER_MAXIMUM_LENGTH as CharacterMaximumLength, DATA_TYPE as DataType,
    IsNullable = convert(bit, case when UPPER(ltrim(rtrim(co.IS_NULLABLE))) = 'YES' then 1 else 0 end), convert(bit, COLUMNPROPERTY(object_id(co.TABLE_SCHEMA + '.' + TABLE_NAME), COLUMN_NAME, 'IsIdentity')) as IsIdentity, d.name as ConstraintName,
	isnull(d.definition, COLUMN_DEFAULT) as ColumnDefault, cm.definition as Formula, NUMERIC_PRECISION as NumericPrecision, NUMERIC_SCALE as NumericScale,
	SchemaName = co.TABLE_SCHEMA, IDENT_INCR(co.TABLE_SCHEMA + '.' + TABLE_NAME) AS Increment
from INFORMATION_SCHEMA.COLUMNS co
join sys.all_columns c on c.name = co.column_name
join sys.tables t on t.object_id = c.object_id
	and t.name = co.TABLE_NAME
join sys.schemas sc on sc.schema_id = t.schema_id
	and sc.name = co.TABLE_SCHEMA
left join sys.default_constraints d on d.object_id = c.default_object_id
left join sys.computed_columns cm on cm.name = co.column_name and c.is_computed = 1 and cm.object_id = t.object_id";

			var qry = database.Is2000OrLess ? lessQry : moreQry;

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							var col = rdr.ToObject<Column>();
							var schema = database.Schemas.First(s => s.SchemaName == rdr["SchemaName"].ToString());
							col.Table = schema.Tables.First(t => t.TableName == rdr["TableName"].ToString());
							col.Table.Columns.Add(col);
							col.ExtendedProperties = allExtendedProperties.Where(ep => ep.Level1Object == col.Table.ObjectName && ep.ObjectSchema == col.Table.Schema.SchemaName &&
								ep.Level2Object == col.ColumnName).ToList();
						}
					}
					rdr.Close();
				}
			}
		}
	}
}
