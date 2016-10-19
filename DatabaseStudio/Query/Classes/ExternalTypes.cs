using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Query.Classes
{
	public class ExternalTypes
	{
		public static Type[] GetExternalTypes()
		{
			string litePath = "System.Data.SQLite.dll";
			var types = new List<Type>();
			//if (!File.Exists(litePath))
			//	System.IO.File.WriteAllBytes(litePath, PaJaMa.DatabaseStudio.Properties.Resources.System_Data_SQLite);
			if (File.Exists(litePath))
			{
				FileInfo finf = new FileInfo(litePath);
				var asm = Assembly.LoadFile(finf.FullName);
				foreach (var t in asm.GetTypes().Where(t => t.GetInterface(typeof(System.Data.IDbConnection).Name) != null))
				{
					types.Add(t);
				}
			}
			return types.ToArray();
		}
	}
}
