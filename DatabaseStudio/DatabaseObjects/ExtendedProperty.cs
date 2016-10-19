using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class ExtendedProperty : DatabaseObjectBase
	{
		//const string SP_ADD = "EXEC sp_addextendedproperty N'{0}', N'{1}', 'SCHEMA', N'{2}', '{3}', N'{4}', {5}, {6}";
		//const string SP_REMOVE = "EXEC sp_dropextendedproperty N'{0}', 'SCHEMA', N'{1}', '{2}', N'{3}', {4}, {5}";

		public string PropName { get; set; }

		[Ignore]
		public object PropValue { get; set; }

		public string ObjectSchema { get; set; }
		public string Level1Type { get; set; }
		public string Level1Object { get; set; }
		public string Level2Type { get; set; }
		public string Level2Object { get; set; }
		public bool IgnoreSchema { get; set; }

		public override string ObjectName
		{
			get { return (string.IsNullOrEmpty(Level2Object) ? string.Empty : Level2Object + ".") + PropName; }
		}

		//public override string ObjectType
		//{
		//	get { return "Exten; }
		//}

		public static List<ExtendedProperty> GetExtendedProperties(DbConnection connection, bool is2000orLess)
		{
			string qry = is2000orLess ? @"
select name as PropName, value as PropValue, objtype as Level1Type, objname as Level1Object, null as Level2Type,
	null as Level2Object, 'dbo' as ObjectSchema, convert(bit, 0) as IgnoreSchema
from ::fn_listextendedproperty 
(NULL, 'user', 'dbo', 'function', NULL, NULL, NULL)
union all
select name, value, objtype, objname, null, null, 'dbo', convert(bit, 0) from ::fn_listextendedproperty 
(NULL, 'user', 'dbo', 'table', NULL, NULL, NULL)
union all
select name, value, objtype, objname, null, null, 'dbo', convert(bit, 0) from ::fn_listextendedproperty 
(NULL, 'user', 'dbo', 'view', NULL, NULL, NULL)
union all
select name, value, objtype, objname, null, null, 'dbo', convert(bit, 0) from ::fn_listextendedproperty 
(NULL, 'user', 'dbo', 'synonym', NULL, NULL, NULL)
"
				: @"
select ep.name as PropName, ep.value as PropValue, 'PROCEDURE' as Level1Type, p.name as Level1Object, null as Level2Type, null as Level2Object, sc.name as ObjectSchema, IgnoreSchema = convert(bit, 0)
FROM sys.extended_properties AS ep
JOIN sys.procedures p on p.object_id = ep.major_id
join sys.schemas sc on sc.schema_id = p.schema_id
union all
select ep.name as PropName, ep.value as PropValue, 'VIEW' as Level1Type, v.name as Level1Object, null as Level2Type, null as Level2Object, s.name as ObjectSchema, IgnoreSchema = convert(bit, 0)
FROM sys.extended_properties AS ep
join sys.views v on v.object_id = ep.major_id
join sys.schemas s on s.schema_id = v.schema_id
union all
select ep.name as PropName, ep.value as PropValue, 'FUNCTION' as Level1Type, p.name as Level1Object, null as Level2Type, null as Level2Object, sc.name as ObjectSchema, IgnoreSchema = convert(bit, 0)
FROM sys.extended_properties AS ep
JOIN sys.objects p on p.object_id = ep.major_id
 and type in ('FN', 'IF', 'TF')
join sys.schemas sc on sc.schema_id = p.schema_id
union all
select ep.name as PropName, ep.value as PropValue, 'SYNONYM' as Level1Type, sy.name as Level1Object, null as Level2Type, null as Level2Object, s.name as ObjectSchema, IgnoreSchema = convert(bit, 0)
FROM sys.extended_properties AS ep
JOIN sys.synonyms sy on sy.object_id = ep.major_id
join sys.schemas s on s.schema_id = sy.schema_id
union all
select ep.name as PropName, ep.value as PropValue, 'TABLE' as Level1Type, t.name as Level1Object, 'COLUMN' as Level2Type, c.name as Level2Object, s.name as ObjectSchema, IgnoreSchema = convert(bit, 0)
FROM sys.extended_properties AS ep
JOIN sys.tables AS t ON ep.major_id = t.object_id 
join sys.schemas s on s.schema_id = t.schema_id
left JOIN sys.columns AS c ON ep.major_id = c.object_id AND ep.minor_id = c.column_id
union all
select ep.name as PropName, ep.value as PropValue, 'USER' as Level1Type, p.name as Level1Object, null as Level2Type, null as Level2Object, p.default_schema_name as ObjectSchema, IgnoreSchema = convert(bit, 1)
FROM sys.extended_properties AS ep
join sys.database_principals p on p.principal_id = ep.major_id
where ep.class = 4
union all
select ep.name as PropName, ep.value as PropValue, 'SCHEMA' as Level1Type, s.name as Level1Object, null as Level2Type, null as Level2Object, s.name as ObjectSchema, IgnoreSchema = convert(bit, 1)
FROM sys.extended_properties AS ep
join sys.schemas s on s.schema_id = ep.major_id
where ep.class_desc = 'SCHEMA'
";
			var extendedProperties = new List<ExtendedProperty>();
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;

				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							extendedProperties.Add(rdr.ToObject<ExtendedProperty>());
						}
					}
					rdr.Close();
				}

				if (is2000orLess)
				{
					StringBuilder sb = new StringBuilder();

					cmd.CommandText = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES";
					bool firstIn = true;
					using (var rdr = cmd.ExecuteReader())
					{
						if (rdr.HasRows)
						{
							while (rdr.Read())
							{
								if (!firstIn)
									sb.AppendLine("union all");
								sb.AppendLineFormat(@"select name as PropName, value as PropValue, objtype as Level2Type, objname as Level2Object, 'TABLE' as Level1Type,
	'{0}' as Level1Object, 'dbo' as ObjectSchema, convert(bit, 0) as IgnoreSchema
from ::fn_listextendedproperty 
(NULL, 'user', 'dbo', 'table', '{0}', 'column', NULL)", rdr["TABLE_NAME"].ToString());
								firstIn = false;
							}
						}
						rdr.Close();
					}

					cmd.CommandText = sb.ToString();
					using (var rdr = cmd.ExecuteReader())
					{
						if (rdr.HasRows)
						{
							while (rdr.Read())
							{
								var ep = rdr.ToObject<ExtendedProperty>();
								extendedProperties.Add(ep);
							}
						}
						rdr.Close();
					}
				}
			}

			return extendedProperties;
		}
	}
}
