using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class View : DatabaseObjectWithExtendedProperties
	{
		public override string ObjectName
		{
			get { return ViewName; }
		}

		public Schema Schema { get; set; }
		public string ViewName { get; set; }

		[Ignore]
		public List<Column> Columns { get; set; }

		[IgnoreCase]
		public string Definition { get; set; }

		public override string ToString()
		{
			return Schema.SchemaName + "." + ViewName;
		}

		public View()
		{
			Columns = new List<Column>();
		}

		public static void PopulateViews(Database database, DbConnection connection, List<ExtendedProperty> extendedProperties)
		{
			string qry = database.Is2000OrLess ?
				@"select 
	VIEW_SCHEMA as ObjectSchema,
	vcu.VIEW_NAME as ViewName,
	vcu.COLUMN_NAME as ColumnName,
	convert(bit, 0) as IsIdentity,
	c.DATA_TYPE as DataType,
	c.CHARACTER_MAXIMUM_LENGTH as CharacterMaximumLength,
	convert(bit, case when c.IS_NULLABLE = 'YES' then 1 else 0 end) as IsNullable,
	VIEW_DEFINITION as Definition
from INFORMATION_SCHEMA.VIEW_COLUMN_USAGE vcu
join INFORMATION_SCHEMA.COLUMNS c on c.TABLE_SCHEMA = vcu.VIEW_SCHEMA
	and c.TABLE_NAME = c.TABLE_NAME and c.COLUMN_NAME = vcu.COLUMN_NAME
join INFORMATION_SCHEMA.VIEWS v on v.TABLE_NAME = vcu.VIEW_NAME and v.TABLE_SCHEMA = vcu.VIEW_SCHEMA
" : @"select
	s.name as ObjectSchema,
	v.name as ViewName,
	vc.name as ColumnName,
	is_identity as IsIdentity,
	t.name as DataType,
	columnproperty(vc.object_id, vc.name, 'charmaxlen') as CharacterMaximumLength,
	vc.is_nullable as IsNullable,
	OBJECT_DEFINITION(OBJECT_ID(s.name + '.' + v.name)) as Definition
from sys.views v
join sys.columns vc on vc.object_id = v.object_id
join sys.types t on t.user_type_id = vc.system_type_id
join sys.schemas s on s.schema_id = v.schema_id";
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							var schema = database.Schemas.First(s => s.SchemaName == rdr["ObjectSchema"].ToString());
							var viewName = rdr["ViewName"].ToString();
							var currView = schema.Views.FirstOrDefault(v => v.ViewName == viewName && v.Schema.SchemaName == schema.SchemaName);
							if (currView == null)
							{
								currView = rdr.ToObject<View>();
								currView.Schema = schema;
								currView.ExtendedProperties = extendedProperties.Where(ep => ep.Level1Object == currView.ViewName && ep.ObjectSchema == currView.Schema.SchemaName &&
									string.IsNullOrEmpty(ep.Level2Object)).ToList();
								schema.Views.Add(currView);
							}

							var col = rdr.ToObject<Column>();
							currView.Columns.Add(col);
						}
					}
				}
			}
		}
	}
}
