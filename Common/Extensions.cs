using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace PaJaMa.Common
{
	public static class Extensions
	{
		public static string GetFullExceptionText(this Exception ex)
		{
			StringBuilder sb = new StringBuilder();
			do
			{
				sb.AppendLine(ex.Message);
				ex = ex.InnerException;
			}
			while (ex != null);
			return sb.ToString();
		}

		public static string GetFullExceptionTextWithStackTrace(this Exception ex)
		{
			StringBuilder sb = new StringBuilder();
			do
			{
				sb.AppendLine(ex.Message);
				sb.AppendLine(ex.StackTrace);
				ex = ex.InnerException;
			}
			while (ex != null);
			return sb.ToString();
		}

		#region STRING
		public static string FileSafeName(this string s)
		{
			foreach (char c in System.IO.Path.GetInvalidFileNameChars())
			{
				s = s.Replace(c.ToString(), "");
			}
			return s;
		}

		public static string CamelCaseToSpaced(this string input)
		{
			return Regex.Replace(input, "(\\B[A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}

		public static string UpperToWords(this string input)
		{
			var matches = Regex.Matches(input, "([A-Z])([A-Z]*)?");
			if (matches.Count > 0)
			{
				foreach (Match m in matches)
				{
					input = input.Replace(m.Groups[0].Value, m.Groups[1].Value + m.Groups[2].Value.ToLower());
				}
			}
			return input;
		}

		public static void AppendLineFormat(this StringBuilder sb, string format, params object[] args)
		{
			sb.AppendLine(string.Format(format, args));
		}
		#endregion

		#region A S S E M B L Y
		public static string AssemblyTitle(this Assembly a)
		{
			return getAssemblyAttribute<AssemblyTitleAttribute>(a).Title;
		}

		public static string AssemblyDescription(this Assembly a)
		{
			return getAssemblyAttribute<AssemblyDescriptionAttribute>(a).Description;
		}

		public static string AssemblyConfiguration(this Assembly a)
		{
			return getAssemblyAttribute<AssemblyConfigurationAttribute>(a).Configuration;
		}

		public static string AssemblyCopyright(this Assembly a)
		{
			return getAssemblyAttribute<AssemblyCopyrightAttribute>(a).Copyright;
		}

		public static string AssemblyCompany(this Assembly a)
		{
			return getAssemblyAttribute<AssemblyCompanyAttribute>(a).Company;
		}

		public static string AssemblyProduct(this Assembly a)
		{
			return getAssemblyAttribute<AssemblyProductAttribute>(a).Product;
		}

		public static string AssemblyVersion(this Assembly a)
		{
			return a.GetName().Version.ToString();
		}

		private static T getAssemblyAttribute<T>(Assembly a) where T : Attribute
		{
			return (T)Attribute.GetCustomAttribute(a, typeof(T));
		}

		#endregion

		public static bool IsInt(this string s)
		{
			int tempInt = -1;
			return int.TryParse(s, out tempInt);
		}

		//public static bool IsPrimitive(this Type type)
		//{
		//	var types = new[]
		//				  {
		//					  typeof (Enum),
		//					  typeof (String),
		//					  typeof (Char),
		//					  typeof (Guid),

		//					  typeof (Boolean),
		//					  typeof (Byte),
		//					  typeof (Int16),
		//					  typeof (Int32),
		//					  typeof (Int64),
		//					  typeof (Single),
		//					  typeof (Double),
		//					  typeof (Decimal),

		//					  typeof (SByte),
		//					  typeof (UInt16),
		//					  typeof (UInt32),
		//					  typeof (UInt64),

		//					  typeof (DateTime),
		//					  typeof (DateTimeOffset),
		//					  typeof (TimeSpan),
		//				  };


		//	var nullTypes = from t in types
		//					where t.IsValueType
		//					select typeof(Nullable<>).MakeGenericType(t);

		//	var allTypes = types.Concat(nullTypes).ToArray();
		//	return allTypes.Contains(type);
		//}

		public static bool IsNumericType(this Type type)
		{
			if (type == null)
			{
				return false;
			}

			switch (Type.GetTypeCode(type))
			{
				case TypeCode.Byte:
				case TypeCode.Decimal:
				case TypeCode.Double:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				case TypeCode.SByte:
				case TypeCode.Single:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
					return true;
				case TypeCode.Object:
					if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
					{
						return IsNumericType(Nullable.GetUnderlyingType(type));
					}
					return false;
			}
			return false;
		}

		public static string GetString(this Stream stream, Encoding encoding = null)
		{
			if (encoding == null) encoding = System.Text.Encoding.UTF8;
			var bytes = new byte[stream.Length];
			stream.Read(bytes, 0, bytes.Length);
			return encoding.GetString(bytes);
		}
	}
}
