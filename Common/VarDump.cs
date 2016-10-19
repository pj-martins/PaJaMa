using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PaJaMa.Common
{
	public class VarDump
	{
		public static Dictionary<string, object> Dump(object input)
		{
			return dump(input, string.Empty, 0);
		}

		private static Dictionary<string, object> dump(object input, string prefix, int level)
		{
			if (level >= 7) return new Dictionary<string, object>();
			if (input == null) return new Dictionary<string, object>();
			var dict = new Dictionary<string, object>();
			var objType = input.GetType();
			var propInfs = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (var prop in propInfs)
			{
				var objVal = prop.GetValue(input, null);
				dict.Add(prefix + prop.Name, objVal);
				if (objVal != null && !objVal.GetType().IsValueType)
				{
					foreach (var kvp in dump(objVal, prop.Name + ".", level + 1))
					{
						dict.Add(kvp.Key, kvp.Value);
					}
				}
			}

			return dict;
		}
	}
}
