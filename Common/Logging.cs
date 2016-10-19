using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PaJaMa.Common
{
	public class Logging
	{
		public static void LogException(Exception ex)
		{
			try
			{
				FileInfo fInf = new FileInfo(Folders.AppDataPath + "\\Server Stuff\\error.log");
				if (!fInf.Directory.Exists) fInf.Directory.Create();
				File.AppendAllText(fInf.FullName, "\r\n\r\n" + System.Reflection.Assembly.GetCallingAssembly().FullName + "\r\n" + DateTime.Now.ToString() + ":" + ex.Message + "\r\n" + ex.StackTrace);
			}
			catch
			{
				throw ex;
			}
		}
	}
}
