using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PaJaMa.Common
{
	public class IgnoreAttribute : Attribute { }

	public static class AttributeExtensions
	{
		public static TAttribute GetAttribute<TAttribute>(this PropertyInfo pinf)
			where TAttribute : Attribute
		{
			return (TAttribute)pinf.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault();
		}

		public static bool HasAttribute<TAttribute>(this PropertyInfo pinf)
			where TAttribute : Attribute
		{
			return GetAttribute<TAttribute>(pinf) != null;
		}
	}
}
