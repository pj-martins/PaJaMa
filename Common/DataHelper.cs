using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PaJaMa.Common
{
	public static class DataHelper
	{
		public static List<T> ToObjects<T>(this DataTable dt) where T : class
		{
			List<T> objs = new List<T>();
			foreach (DataRow dr in dt.Rows)
			{
				objs.Add(dr.ToObject<T>());
			}
			return objs;
		}

		public static T ToObject<T>(this DataRow dr) where T : class
		{
			var obj = Activator.CreateInstance<T>();
			foreach (DataColumn dc in dr.Table.Columns)
			{
				var propInf = typeof(T).GetProperty(dc.ColumnName, BindingFlags.Public | BindingFlags.Instance);
				if (propInf != null)
					propInf.SetValue(obj, dr[dc] == DBNull.Value ? null : dr[dc], null);
			}
			return obj;
		}

		public static T ToObject<T>(this DbDataReader reader) where T : class
		{
			var columns = new List<string>();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				columns.Add(reader.GetName(i));
			}

			var obj = Activator.CreateInstance<T>();
			foreach (string column in columns)
			{
				var propInf = typeof(T).GetProperty(column, BindingFlags.Public | BindingFlags.Instance);
				if (propInf != null)
				{
					if (reader[column] == DBNull.Value)
						propInf.SetValue(obj, null, null);
					else if (propInf.PropertyType.IsEnum && reader[column] is string)
					{
						var val = Enum.Parse(propInf.PropertyType, reader[column].ToString(), true);
						propInf.SetValue(obj, val, null);
					}
					else
						propInf.SetValue(obj, reader[column], null);
				}
			}
			return obj;
		}

		public static Type GetClrType(SqlDbType sqlType)
		{
			switch (sqlType)
			{
				case SqlDbType.BigInt:
					return typeof(long?);

				case SqlDbType.Binary:
				case SqlDbType.Image:
				case SqlDbType.Timestamp:
				case SqlDbType.VarBinary:
					return typeof(byte[]);

				case SqlDbType.Bit:
					return typeof(bool?);

				case SqlDbType.Char:
				case SqlDbType.NChar:
				case SqlDbType.NText:
				case SqlDbType.NVarChar:
				case SqlDbType.Text:
				case SqlDbType.VarChar:
				case SqlDbType.Xml:
					return typeof(string);

				case SqlDbType.DateTime:
				case SqlDbType.SmallDateTime:
				case SqlDbType.Date:
				case SqlDbType.Time:
				case SqlDbType.DateTime2:
					return typeof(DateTime?);

				case SqlDbType.Decimal:
				case SqlDbType.Money:
				case SqlDbType.SmallMoney:
					return typeof(decimal?);

				case SqlDbType.Float:
					return typeof(double?);

				case SqlDbType.Int:
					return typeof(int?);

				case SqlDbType.Real:
					return typeof(float?);

				case SqlDbType.UniqueIdentifier:
					return typeof(Guid?);

				case SqlDbType.SmallInt:
					return typeof(short?);

				case SqlDbType.TinyInt:
					return typeof(byte?);

				case SqlDbType.Variant:
				case SqlDbType.Udt:
					return typeof(object);

				case SqlDbType.Structured:
					return typeof(DataTable);

				case SqlDbType.DateTimeOffset:
					return typeof(DateTimeOffset?);

				default:
					throw new ArgumentOutOfRangeException("sqlType");
			}
		}
	}
}
