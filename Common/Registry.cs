using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaJaMa.Common
{
	public class Registry
	{
		public static Dictionary<string, string> GetOpenWithList(string extension)
		{
			if (!extension.StartsWith("."))
				extension = "." + extension;

			string baseKey = @"Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\" + extension;

			Dictionary<string, string> progs = new Dictionary<string, string>();

			using (RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(baseKey + @"\OpenWithList"))
			{
				if (rk != null)
				{
					string mruList = (string)rk.GetValue("MRUList");
					if (mruList != null)
					{
						foreach (char c in mruList.ToString())
						{
							string str = rk.GetValue(c.ToString()).ToString();
							if (str.ToLower().Contains("dllhost")) continue;
							var appPath = GetRegisteredApplication(str);
							if (string.IsNullOrEmpty(appPath)) continue;
							var appName = appPath;
							var match = Regex.Match(appPath, "\"(.*?)\"");
							if (match.Success)
								appName = GetMuiCacheApplicationName(match.Groups[1].Value);
							else
								appName = GetMuiCacheApplicationName(appPath);
							
							if (!progs.ContainsKey(appPath))
							{
								progs.Add(appPath, appName);
							}
						}
					}
				}
			}

			return progs;
		}

		public static string GetRegisteredApplication(string app)
		{
			// 
			//  Return registered application by file's extension
			// 
			RegistryKey oHKCR;
			RegistryKey oOpenCmd;
			string command;

			if (Environment.Is64BitOperatingSystem == true)
			{
				oHKCR = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, RegistryView.Registry64);
			}
			else
			{
				oHKCR = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, RegistryView.Registry32);
			}
			try
			{


				oOpenCmd = oHKCR.OpenSubKey(app + "\\shell\\open\\command");
				if (oOpenCmd == null)
				{
					oOpenCmd = oHKCR.OpenSubKey("\\Applications\\" + app + "\\shell\\open\\command");
				}
				if (oOpenCmd == null)
				{
					oOpenCmd = oHKCR.OpenSubKey("Applications\\" + app + "\\shell\\open\\command");
				}
				if (oOpenCmd == null)
				{
					oOpenCmd = oHKCR.OpenSubKey("\\Applications\\" + app + "\\shell\\edit\\command");
				}
				if (oOpenCmd == null)
				{
					oOpenCmd = oHKCR.OpenSubKey("Applications\\" + app + "\\shell\\edit\\command");
				}
				if (oOpenCmd != null)
				{
					command = oOpenCmd.GetValue(null).ToString();
					oOpenCmd.Close();
				}
				else
				{
					return null;
				}
			}
			catch
			{
				return null;
			}
			return command;
		}

		public static string GetMuiCacheApplicationName(string appPath)
		{
			RegistryKey oHKCR;
			string appName = string.Empty;

			if (Environment.Is64BitOperatingSystem == true)
			{
				oHKCR = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, RegistryView.Registry64);
			}
			else
			{
				oHKCR = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.ClassesRoot, RegistryView.Registry32);
			}
			try
			{


				var appSubKey = oHKCR.OpenSubKey("Local Settings\\Software\\Microsoft\\Windows\\Shell\\MuiCache\\");
				var valKey = appSubKey.GetValueNames().FirstOrDefault(vn => vn == appPath);
				if (valKey != null)
					appName = appSubKey.GetValue(valKey).ToString();
			}
			catch
			{
				return null;
			}
			return appName;
		}
	}
}
