using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaJaMa.Common
{
	public class Parse
	{
		public static bool ParseBool(object obj)
		{
			if (obj == null) return false;
			if (obj is bool) return (bool)obj;
			if (obj.ToString() == "1") return true;
            if (obj.ToString().ToLower() == "true") return true;
			return false;
		}
	}
}
