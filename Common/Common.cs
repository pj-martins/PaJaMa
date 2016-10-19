using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Net;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading;

namespace PaJaMa.Common
{
	public partial class Common
	{
		public static string Md5Hash(string input)
		{
			// First we need to convert the string into bytes, which
			// means using a text encoder.
			Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

			// Create a buffer large enough to hold the string
			byte[] unicodeText = new byte[input.Length * 2];
			enc.GetBytes(input.ToCharArray(), 0, input.Length, unicodeText, 0, true);

			// Now that we have a byte array we can ask the CSP to hash it
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			// Build the final string by converting each byte
			// into hex and appending it to a StringBuilder
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < result.Length; i++)
			{
				sb.Append(result[i].ToString("X2"));
			}

			// And return it
			return sb.ToString();

		}

		public static Guid Md5HashGuid(string input)
		{
			return new Guid(Md5Hash(input));
		}

		#region GetHTML
		public static string GetHTML(string url, Dictionary<string, string> postParameters)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			Stream requestStream;
			if (postParameters != null && postParameters.Count > 0)
			{
				string data = "";
				foreach (string key in postParameters.Keys)
				{
					data += (data.Length > 0 ? "&" : "") + key + "=" + postParameters[key];
				}
				byte[] buffer = Encoding.ASCII.GetBytes(data);
				request.Method = "POST";
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = buffer.Length;
				requestStream = request.GetRequestStream();
				requestStream.Write(buffer, 0, buffer.Length);
				requestStream.Close();
			}
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			requestStream = response.GetResponseStream();
			StreamReader sr = new StreamReader(requestStream);
			return sr.ReadToEnd();
		}

		public static string GetHTML(string url)
		{
			return GetHTML(url, null);
		}
		#endregion
		#region StringBytes
		public static string GetStringFromStream(Stream stream)
		{
			//return GetStringFromStream(stream, (int)stream.Length);
			//string rtv = string.Empty;
			//StreamReader reader = new StreamReader(stream);
			//return reader.ReadToEnd();
			MemoryStream ms = new MemoryStream();
			byte[] chunk = new byte[2048];
			int bytesRead;
			while ((bytesRead = stream.Read(chunk, 0, chunk.Length)) > 0)
			{
				ms.Write(chunk, 0, bytesRead);
				//if (bytesRead < 2048) break;
			}
			return GetStringFromBytes(ms.GetBuffer());
		}

		public static string GetStringFromStream(Stream stream, int length)
		{
			byte[] bytes = new byte[length];
			int count = stream.Read(bytes, 0, (int)bytes.Length);
			String data = GetStringFromBytes(bytes);
			char[] unused = { (char)data[count] };
			return data.TrimEnd(unused);
		}

		public static string GetStringFromBytes(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		public static byte[] GetBytesFromString(string input)
		{
			return Encoding.UTF8.GetBytes(input);
		}

		public static MemoryStream GetMemoryStreamFromString(string input)
		{
			return new MemoryStream(GetBytesFromString(input));
		}

		public static String UTF8ByteArrayToString(Byte[] bytes)
		{

			UTF8Encoding encoding = new UTF8Encoding();
			String constructedString = encoding.GetString(bytes);
			return (constructedString);
		}

		public static Byte[] StringToUTF8ByteArray(String xml)
		{
			UTF8Encoding encoding = new UTF8Encoding();
			Byte[] byteArray = encoding.GetBytes(xml);
			return byteArray;
		}

		public static string SpaceCameledCase(string input)
		{
			return System.Text.RegularExpressions.Regex.Replace(input, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
		}

		public static byte[] StreamToBytes(Stream input, int length)
		{
			byte[] buffer = new byte[length];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		#endregion
		#region SurroundingString
		public static string SurroundingString(string full, string middle, int returnLength)
		{
			return SurroundingString(full, full.IndexOf(middle), middle.Length);
		}

		public static string SurroundingString(string full, int index, int length)
		{
			return SurroundingString(full, index, length, 50);
		}

		public static string SurroundingString(string full, string middle, int returnLength, int startIndex)
		{
			return SurroundingString(full, full.IndexOf(middle, startIndex), middle.Length);
		}

		public static string SurroundingString(string full, string middle)
		{
			return SurroundingString(full, middle, 50);
		}

		public static string SurroundingString(string full, int index, int length, int returnLength)
		{
			int tempLength = returnLength;
			int start = Math.Max(index - (Math.Max(returnLength - length, 0) / 2), 0);
			if (start + tempLength > full.Length)
			{
				start = full.Length - tempLength;
				if (start < 0)
				{
					start = 0;
					tempLength = full.Length;
				}
			}
			return full.Substring(start, tempLength);
		}
		#endregion
		#region CamelCaseToSpaced
		public static string EnumCamelCaseToSpaced<T>(object enumValue)
		{
			return Enum.GetName(typeof(T), enumValue).CamelCaseToSpaced();
		}
		#endregion
		#region MinMax
		public static T Max<T>(params T[] values) where T : IComparable
		{
			if (values.Length < 1) return default(T);
			T rtv = values[0];
			foreach (T item in values)
			{
				if (item.CompareTo(rtv) > 0)
					rtv = item;
			}
			return rtv;
		}

		public static T Min<T>(params T[] values) where T : IComparable
		{
			if (values.Length < 1) return default(T);
			T rtv = values[0];
			foreach (T item in values)
			{
				if (item.CompareTo(rtv) < 0)
					rtv = item;
			}
			return rtv;
		}
		#endregion
		#region Shutdown
		[DllImport("user32.dll")]
		private static extern void LockWorkStation();

		public enum ShutdownType
		{
			LogOff = 0,
			Shutdown = 1,
			Reboot = 2,
			ForcedLogOff = 4,
			ForcedShutdown = 5,
			ForcedReboot = 6,
			PowerOff = 8,
			ForcedPowerOff = 12,
			Lock = 13,
			Standby = 14,
			Hibernate = 15
		}

		public static void Shutdown(ShutdownType type)
		{
			switch (type)
			{
				case ShutdownType.Lock:
					LockWorkStation();
					break;
				case ShutdownType.Standby:
					Application.SetSuspendState(PowerState.Suspend, true, true);
					break;
				case ShutdownType.Hibernate:
					Application.SetSuspendState(PowerState.Hibernate, true, true);
					break;
				default:
					ManagementBaseObject mbo = null;
					ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
					mcWin32.Get();

					mcWin32.Scope.Options.EnablePrivileges = true;
					ManagementBaseObject mboParams =
						mcWin32.GetMethodParameters("Win32Shutdown");

					mboParams["Flags"] = ((int)type).ToString();
					mboParams["Reserved"] = "0";
					foreach (ManagementObject manObj in mcWin32.GetInstances())
					{
						mbo = manObj.InvokeMethod("Win32Shutdown",
							mboParams, null);
					}
					break;
			}
		}
		#endregion
		public static bool IsEmpty(object input)
		{
			if (input == null)
				return true;
			if (input == DBNull.Value)
				return true;
			if (string.IsNullOrEmpty(input.ToString()))
				return true;
			if (input is Guid && input.Equals(Guid.Empty))
				return true;
			return false;
		}
		#region ChangeType
		public static T ChangeType<T>(object value)
		{
			TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
			if (typeof(T).Equals(typeof(decimal)) || typeof(T).Equals(typeof(decimal?)))
				return (T)tc.ConvertFrom(Convert.ToDecimal(value));
			if (typeof(T).Equals(typeof(int)) || typeof(T).Equals(typeof(int?)))
				return (T)tc.ConvertFrom(Convert.ToInt32(value));
			return (T)tc.ConvertFrom(value);
		}
		#endregion

		public static void RunInThread(Action action)
		{
			new System.Threading.Thread(new System.Threading.ThreadStart(action)).Start();
		}

		#region GetFilesRecursively
		public static string[] GetFilesRecursively(string directoryName) { return GetFilesRecursively(directoryName, true); }
		public static string[] GetFilesRecursively(string directoryName, bool includeDirectories)
		{
			List<string> rtv = new List<string>();
			DirectoryInfo dinf = new DirectoryInfo(directoryName);
			if (dinf.Exists)
			{
				foreach (FileInfo finf in dinf.GetFiles())
					rtv.Add(finf.FullName);
				foreach (DirectoryInfo dinf2 in dinf.GetDirectories())
				{
					if (includeDirectories)
						rtv.Add(dinf2.FullName);
					rtv.AddRange(GetFilesRecursively(dinf2.FullName, includeDirectories));
				}
			}
			return rtv.ToArray();
		}
		public static List<FileInfo> GetFilesRecursively(DirectoryInfo dinf)
		{
			List<FileInfo> rtv = new List<FileInfo>();
			foreach (FileInfo finf in dinf.GetFiles())
				rtv.Add(finf);
			foreach (DirectoryInfo dinf2 in dinf.GetDirectories())
			{
				rtv.AddRange(GetFilesRecursively(dinf2));
			}
			return rtv;
		}

		#endregion
		#region Recycle
		public static void Recycle(string fileName) { Recycle(fileName, true, true); }
		public static void Recycle(string fileName, bool prompt) { Recycle(fileName, prompt, true); }
		public static void Recycle(string fileName, bool prompt, bool showErrorUI)
		{
			Win32Api.InteropSHFileOperation fo = new Win32Api.InteropSHFileOperation();
			fo.wFunc = Win32Api.InteropSHFileOperation.FO_Func.FO_DELETE;
			fo.fFlags.FOF_ALLOWUNDO = true;
			fo.fFlags.FOF_NOCONFIRMATION = !prompt;
			fo.fFlags.FOF_NOERRORUI = !showErrorUI;
			fo.pFrom = fileName;
			int exec = fo.Execute();
			if (exec == 0 || showErrorUI)
				return;
			switch (exec)
			{
				case 5:
					throw new Exception("Could not delete '" + fileName + "', access denied.");
				case 32:
					throw new Exception("Could not delete '" + fileName + "', file is in use.");
				default:
					throw new Exception(exec.ToString());
			}
		}
		#endregion
		#region ExecuteFile
		public static string ExecuteFile(string fileName) { return ExecuteFile(fileName, string.Empty, true); }
		public static string ExecuteFile(string fileName, string arguments) { return ExecuteFile(fileName, arguments, true); }
		public static string ExecuteFile(string fileName, string arguments, bool waitForExit)
		{
			if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName)) return string.Empty;
			FileInfo fInf = new FileInfo(fileName);
			ProcessStartInfo psi = new ProcessStartInfo(fInf.FullName, arguments);
			psi.UseShellExecute = false;
			psi.ErrorDialog = false;
			psi.CreateNoWindow = true;
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			Process p = Process.Start(psi);
			StreamReader sr = p.StandardError;
			string errOutput = sr.ReadToEnd();
			sr.Close();
			sr = p.StandardOutput;
			string output = sr.ReadToEnd();
			sr.Close();
			if (waitForExit)
				p.WaitForExit();
			if (!string.IsNullOrEmpty(errOutput)) throw new Exception(errOutput);
			return output;
		}
		#endregion
		#region Reflection
		public static void ReflectionSetValue(object field, string propertyName, object value)
		{
			System.Reflection.FieldInfo fi = field.GetType().GetField(propertyName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			fi.SetValue(field, value);
		}
		public static T CreateType<T>(string assemblyName, string typeName) { return CreateType<T>(assemblyName, Assembly.GetExecutingAssembly().Location, typeName, null); }
		public static T CreateType<T>(string assemblyName, string typeName, object[] args) { return CreateType<T>(assemblyName, Assembly.GetExecutingAssembly().Location, typeName, args); }
		public static T CreateType<T>(string assemblyName, string assemblyFileName, string typeName) { return CreateType<T>(assemblyName, assemblyFileName, typeName, null); }
		public static T CreateType<T>(string assemblyName, string assemblyFileName, string typeName, object[] args)
		{
			try
			{
				Assembly asm = null;
				T instance = default(T);
				try
				{
					asm = Assembly.Load(assemblyName);
				}
				catch
				{
					asm = Assembly.LoadFile(assemblyFileName);
				}
				var type = (from t in asm.GetTypes()
							where t.FullName == typeName
							select t).FirstOrDefault();
				if (type == null)
					throw new Exception("Type could not be found in assembly.");

				instance = (T)Activator.CreateInstance(type, args);
				return instance;
			}
			catch (Exception ex)
			{
				throw new Exception(assemblyFileName + " is invalid or could not be found or " + typeName + " does not exist. Check inner exception for more details!", ex);
			}
		}
		#endregion
		#region GetType
		public static Type GetType(string typeName) { return GetType(typeName, false, false); }
		public static Type GetType(string typeName, bool throwOnError) { return GetType(typeName, throwOnError, false); }
		public static Type GetType(string typeName, bool throwOnError, bool ignoreCase)
		{
			Type tp = Type.GetType(typeName, false, ignoreCase);
			if (tp != null) return tp;
			List<Type> types = new List<Type>();
			foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				tp = asm.GetType(typeName, false, ignoreCase);
				if (tp == null)
				{
					types = new List<Type>();
					types.AddRange(asm.GetTypes());
					if (ignoreCase)
						tp = types.Find(t => t.Name.ToLower() == typeName.ToLower() || t.FullName.ToLower() == typeName.ToLower());
					else
						tp = types.Find(t => t.Name == typeName || t.FullName == typeName);
					if (tp != null)
						break;
				}
			}
			if (tp == null && throwOnError)
				tp = GetType(typeName, true);
			return tp;
		}

		public static string ParseStringFromWebPage(string url, string regexPattern) { return ParseStringFromWebPage(url, regexPattern, RegexOptions.None); }
		public static string ParseStringFromWebPage(string url, string regexPattern, RegexOptions opts)
		{
			string match = string.Empty;
			try
			{
				string html = new WebClient().DownloadString(new Uri(url));
				Match m = Regex.Match(html, regexPattern, opts);
				if (m.Success)
				{
					match = m.Groups[1].Value;
				}
				return match;
			}
			catch
			{
				return string.Empty;
			}
		}

		#endregion

		[DebuggerNonUserCode()]
		public static bool ParseGUID(string input, out Guid outGuid)
		{
			outGuid = Guid.Empty;
			if (!Regex.Match(input, @"^\{?[0-9A-Za-z]{8}-[0-9A-Za-z]{4}-[0-9A-Za-z]{4}-[0-9A-Za-z]{4}-[0-9A-Za-z]{12}\}?$").Success)
				return false;
			try
			{
				outGuid = new Guid(input);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static bool IsValidGUID(string input)
		{
			Guid outGuid = Guid.Empty;
			return ParseGUID(input, out outGuid);
		}

		public static TValue GetDictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key)
		{
			if (!dictionary.ContainsKey(key))
				return default(TValue);
			return dictionary[key];
		}

		public static void SetDictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
		{
			if (!dictionary.ContainsKey(key))
				dictionary.Add(key, value);
			else
				dictionary[key] = value;
		}

		public static string Substring(string str, int startIndex, int length)
		{
			if (string.IsNullOrEmpty(str))
				return string.Empty;
			if (str.Length <= startIndex)
				return string.Empty;
			if (str.Length <= length + startIndex)
				return str.Substring(startIndex, str.Length - startIndex);
			return str.Substring(startIndex, length);
		}

		public static string GetContentType(string fileName)
		{
			string contentType = "application/octetstream";
			string ext = System.IO.Path.GetExtension(fileName).ToLower();
			Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
			if (registryKey != null && registryKey.GetValue("Content Type") != null)
				contentType = registryKey.GetValue("Content Type").ToString();
			return contentType;

		}

		public static string NullString(string str, string defaultValue)
		{
			return string.IsNullOrEmpty(str) ? defaultValue : str;
		}

		public static DateTime MinDate(DateTime dt1, DateTime dt2)
		{
			return dt1 < dt2 ? dt1 : dt2;
		}

		public static DateTime MaxDate(DateTime dt1, DateTime dt2)
		{
			return dt1 > dt2 ? dt1 : dt2;
		}

		public static string StripHTML(string input)
		{
			if (string.IsNullOrEmpty(input))
				return string.Empty;

			return Regex.Replace(input, @"<[^>]*>", String.Empty);
		}

		public static string PathSafeName(string input)
		{
			if (string.IsNullOrEmpty(input))
				return input;

			foreach (char c in Path.GetInvalidPathChars())
				input = input.Replace(c.ToString(), "");
			return input;
		}

		public static string FileSafeName(string input)
		{
			foreach (char c in Path.GetInvalidFileNameChars())
				input = input.Replace(c.ToString(), "");
			return input;
		}

		public static bool TryParseFraction(string input, out float fraction)
		{
			double tempDbl = -1;
			fraction = 0;
			if (!TryParseFraction(input, out tempDbl))
				return false;
			fraction = (float)tempDbl;
			return true;
		}
		public static bool TryParseFraction(string input, out double fraction)
		{
			if (input.Contains("¼"))
				input = input.Replace("¼", " 1/4");
			if (input.Contains("½"))
				input = input.Replace("½", " 1/2");
			if (input.Contains("¾"))
				input = input.Replace("¾", " 3/4");
			if (input.Contains("⅓"))
				input = input.Replace("⅓", " 1/3");

			double? tempFraction = null;
			fraction = 0;
			string[] parts = input.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string part in parts)
			{
				double tmpDbl = -1;
				if (double.TryParse(part, out tmpDbl))
				{
					if (tempFraction == null) tempFraction = 0;
					tempFraction += tmpDbl;
					continue;
				}

				if (part.Contains("/"))
				{
					string[] parts2 = part.Split('/');
					if (parts2.Length != 2) return false;
					double numerator = -1;
					double denominator = -1;
					if (!double.TryParse(parts2[0], out numerator) || !double.TryParse(parts2[1], out denominator))
						return false;
					if (tempFraction == null) tempFraction = 0;
					tempFraction += numerator / denominator;
				}
			}
			fraction = tempFraction.GetValueOrDefault();
			return tempFraction != null;
		}
	}
}
