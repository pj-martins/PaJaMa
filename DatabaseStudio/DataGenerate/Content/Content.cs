using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using PaJaMa.DatabaseStudio.DataGenerate.Classes;
using PaJaMa.DatabaseStudio.DataGenerate.Content.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.DataGenerate.Content
{
	public class ContentControlAttribute : Attribute
	{
		public Type ContentControlType { get; private set; }
		public ContentControlAttribute(Type contentControlType)
		{
			ContentControlType = contentControlType;
		}
	}

	public abstract class ContentBase
	{
		public const string NONE = "None";

		public SqlDbType DbType { get; private set; }
		protected static Random random = new Random();

		public ContentBase(SqlDbType dbType)
		{
			DbType = dbType;
		}

		public void ShowPropertiesControl()
		{
			var attr = this.GetType().GetCustomAttributes(typeof(ContentControlAttribute), true).FirstOrDefault() as ContentControlAttribute;
			if (attr != null)
			{
				using (var frm = Activator.CreateInstance(attr.ContentControlType) as frmContentBase)
				{
					frm.Content = this;
					frm.ShowDialog();
				}
			}
		}

		public abstract object GetContent(DbTransaction trans);
	}

	public class NoContent : ContentBase
	{
		public NoContent(SqlDbType dbType) : base(dbType) { }

		public override string ToString()
		{
			return ContentBase.NONE;
		}

		public override object GetContent(DbTransaction trans)
		{
			return null;
		}
	}

	[ContentControl(typeof(frmStringContent))]
	public class StringContent : ContentBase
	{
		public StringContentType ContentType { get; set; }

		private Dictionary<StringContentType, string[]> _content = new Dictionary<StringContentType, string[]>();

		public StringContent(SqlDbType dbType)
			: base(dbType)
		{
			ContentType = StringContentType.LastFirstName;
		}

		public override string ToString()
		{
			return ContentType.ToString().CamelCaseToSpaced();
		}

		private string getContent(StringContentType contentType)
		{
			if (contentType == StringContentType.None) return null;

			if (!_content.ContainsKey(contentType))
			{
				var rn = this.GetType().Assembly.GetManifestResourceNames().First(x => x.EndsWith("." + contentType.ToString() + ".txt"));
				string allContent = this.GetType().Assembly.GetManifestResourceStream(rn).GetString();
				//switch (contentType)
				//{
				//	case StringContentType.Country:
				//		allContent = Properties.Resources.Countries;
				//		break;
				//	case StringContentType.LastName:
				//		allContent = Properties.Resources.LastNames;
				//		break;
				//	case StringContentType.FirstName:
				//		allContent = Properties.Resources.FirstNames;
				//		break;
				//}
				_content.Add(contentType, allContent.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

			}
			return _content[contentType][random.Next(0, _content[contentType].Length - 1)];
		}

		public override object GetContent(DbTransaction trans)
		{
			string content = string.Empty;
			if (ContentType == StringContentType.LastFirstName)
				content = string.Format("{0}, {1}", getContent(StringContentType.LastName), getContent(StringContentType.FirstName));
			else if (ContentType == StringContentType.Address)
				content = string.Format("{0} {1} St, {2}, {3}, {4}", random.Next(1000, 3000), getContent(StringContentType.Street),
					getContent(StringContentType.City), getContent(StringContentType.State), random.Next(10000, 99999));
			else
				content = getContent(ContentType);

			if (DbType == SqlDbType.VarBinary || DbType == SqlDbType.Binary || DbType == SqlDbType.Image)
				return System.Text.Encoding.ASCII.GetBytes(content);

			return content;
		}
	}

	public enum StringContentType
	{
		None,
		LastFirstName,
		LastName,
		FirstName,
		Address,
		Street,
		City,
		State
	}

	[ContentControl(typeof(frmNumericContent))]
	public class NumericContent : ContentBase
	{
		public decimal Min { get; set; }
		public decimal Max { get; set; }
		public bool IsDecimal { get; private set; }

		public NumericContent(SqlDbType dbType)
			: base(dbType)
		{
			IsDecimal = DbType == SqlDbType.Decimal || DbType == SqlDbType.Float || DbType == SqlDbType.Money || DbType == SqlDbType.Real;

			Max = 100;
		}

		public override string ToString()
		{
			return Min == Max ? Min.ToString() : Min.ToString() + " - " + Max.ToString();
		}

		public override object GetContent(DbTransaction trans)
		{
			if (Max <= Min)
				return Min;

			if (IsDecimal)
			{
				var next = random.NextDouble();
				return (double)Min + (next * (double)(Max - Min));
			}

			return random.Next((int)Min, (int)Max);
		}
	}

	public class KeyContent : ContentBase
	{
		private int? _max = null;
		public KeyConstraint KeyConstraint { get; private set; }
		public KeyContent(SqlDbType dbType) : base(dbType) { }
		public KeyContent(KeyConstraint key, SqlDbType dbType)
			: base(dbType)
		{
			KeyConstraint = key;
		}



		public override string ToString()
		{
			return "Auto";
		}

		public override object GetContent(DbTransaction trans)
		{
			// TODO: multiple
			if (_max == null)
			{
				var conn = trans.Connection;
				using (var cmd = conn.CreateCommand())
				{
					// TODO: multi cols
					cmd.CommandText = string.Format("select max([{0}]) from [{1}].[{2}]", KeyConstraint.Columns[0].ColumnName, KeyConstraint.Table.Schema.SchemaName,
						KeyConstraint.Table.TableName);
					cmd.Transaction = trans;
					var obj = cmd.ExecuteScalar();
					_max = obj.Equals(DBNull.Value) ? 1 : (int)obj;
				}
			}

			int next = _max.Value + 1;
			_max = next;
			return next;
		}
	}

	public class ForeignKeyContent : ContentBase
	{
		private DataTable _foreignKeyValues = null;

		public ForeignKey ForeignKey { get; private set; }
		public ForeignKeyContent(SqlDbType dbType) : base(dbType) { }
		public ForeignKeyContent(ForeignKey foreignKey, SqlDbType dbType)
			: base(dbType)
		{
			ForeignKey = foreignKey;
		}

		public override string ToString()
		{
			return ForeignKey.ForeignKeyName;
		}

		public override object GetContent(DbTransaction trans)
		{
			if (_foreignKeyValues == null)
			{
				_foreignKeyValues = new DataTable();
				using (var cmd = trans.Connection.CreateCommand())
				{
					// TODO: multi cols
					cmd.CommandText = string.Format("select [{0}] from [{1}].[{2}]", ForeignKey.Columns[0].ParentColumn, ForeignKey.ParentTable.Schema.SchemaName,
						ForeignKey.ParentTable.TableName);
					cmd.Transaction = trans;

					using (var rdr = cmd.ExecuteReader())
					{
						_foreignKeyValues.Load(rdr);
					}
				}
			}

			if (_foreignKeyValues.Rows.Count < 1)
				return null;

			int rand = random.Next(0, _foreignKeyValues.Rows.Count - 1);
			return _foreignKeyValues.Rows[rand][0];
		}
	}

	[ContentControl(typeof(frmDateTimeContent))]
	public class DateTimeContent : ContentBase
	{
		public DateTime Min { get; set; }
		public DateTime Max { get; set; }

		public bool ShowDate { get; set; }
		public bool ShowTime { get; set; }

		public DateTimeContent(SqlDbType dbType)
			: base(dbType)
		{
			Min = Max = DateTime.Now;
			ShowDate = ShowTime = true;
			if (DbType == SqlDbType.Time)
				ShowDate = false;
			if (DbType == SqlDbType.Date)
				ShowTime = false;
		}

		public override string ToString()
		{
			return Min == Max ? formatDate(Max) : formatDate(Min) + " - " + formatDate(Max);
		}

		private string formatDate(DateTime dt)
		{
			if (!ShowTime)
				return dt.ToShortDateString();
			if (!ShowDate)
				return dt.ToShortTimeString();
			return dt.ToString();
		}

		public override object GetContent(DbTransaction trans)
		{
			var dt = DateTime.Now;
			if (Max <= Min)
				dt = Min;
			else
			{
				TimeSpan timeSpan = Max - Min;
				TimeSpan newSpan = new TimeSpan(0, random.Next(0, (int)timeSpan.TotalMinutes), 0);
				dt = Min + newSpan;
			}

			if (DbType == SqlDbType.Time)
				return dt.TimeOfDay;

			if (DbType == SqlDbType.DateTimeOffset)
			{
				DateTimeOffset dto = dt;
				return dto;
			}


			return dt;
		}
	}

	public class GuidContent : ContentBase
	{
		public GuidContent(SqlDbType dbType)
			: base(dbType)
		{
		}

		public override string ToString()
		{
			return "newid()";
		}

		public override object GetContent(DbTransaction trans)
		{
			return Guid.NewGuid();
		}
	}

	public class BoolContent : ContentBase
	{
		public BoolContent(SqlDbType dbType)
			: base(dbType)
		{
		}

		public override string ToString()
		{
			return "true/false";
		}

		public override object GetContent(DbTransaction trans)
		{
			return random.Next(2) == 0;
		}
	}
}
