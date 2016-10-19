using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PaJaMa.Common
{
	public class EnumDisplayAttribute : Attribute
	{
		public string DisplayString { get; set; }
		public EnumDisplayAttribute(string displayString)
		{
			DisplayString = displayString;
		}
	}

	public class EnumHelper
	{
		public static List<TEnum> GetEnumValues<TEnum>()
		{
			return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToList();
		}

		public static string GetEnumDisplay<TEnum>(TEnum enumVal)
		{
			FieldInfo fi = enumVal.GetType().GetField(enumVal.ToString());
			EnumDisplayAttribute attr = fi.GetCustomAttributes(typeof(EnumDisplayAttribute), false).FirstOrDefault() as EnumDisplayAttribute;
			if (attr != null)
				return attr.DisplayString;
			return enumVal.ToString();
		}
	}
}
