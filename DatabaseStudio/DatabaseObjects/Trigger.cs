using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class Trigger : DatabaseObjectBase
	{
		public string TriggerName { get; set; }
		public Table Table { get; set; }

		[Ignore]
		public string Definition { get; set; }

		public override string ObjectName
		{
			get { return TriggerName; }
		}

		public bool Disabled { get; set; }

		public static void PopulateTriggers(Database database, DbConnection connection)
		{
			string qry = database.Is2000OrLess ?
				@"select Definition = c.text, o.name as TriggerName,  convert(bit, OBJECTPROPERTY(o.id, 'ExecIsTriggerDisabled')) AS Disabled, p.name as TableName, u.name as SchemaName
from sysobjects o
join syscomments c on c.id = o.id
join sysobjects p on p.id = o.parent_obj
join sysusers u on u.uid = p.uid
where o.type = 'TR'
"
				:
				@"select Definition = OBJECT_DEFINITION(t.object_id), t.name as TriggerName, Disabled = is_disabled, o.name as TableName, SchemaName = sc.name
From sys.triggers t
join sys.tables o on o.object_id = t.parent_id
join sys.schemas sc on sc.schema_id = o.schema_id
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
							var schema = database.Schemas.First(s => s.SchemaName == rdr["SchemaName"].ToString());

							var trig = rdr.ToObject<Trigger>();
							trig.Table = (from t in schema.Tables
										  where t.TableName == rdr["TableName"].ToString()
										  select t).First();

							trig.Table.Triggers.Add(trig);
						}
					}
				}
			}
		}

		public override string ToString()
		{
			return Table.Schema.SchemaName.ToString() + "." + TriggerName;
		}
	}

	public enum TriggerEvent
	{
		INSERT,
		UPDATE,
		DELETE
	}
}
