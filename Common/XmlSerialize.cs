using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace PaJaMa.Common
{
	public class XmlSerialize
	{
		public static String SerializeObject<T>(T obj)
		{
			try
			{
				String XmlizedString = null;
				MemoryStream memoryStream = new MemoryStream();
				XmlSerializer xs = new XmlSerializer(typeof(T));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Default);
				xs.Serialize(xmlTextWriter, obj);
				memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
				XmlizedString = Encoding.Default.GetString(memoryStream.ToArray());
				return XmlizedString;
			}
			catch (Exception e)
			{
				throw new Exception(e.GetFullExceptionText());
			}
		}

		public static void SerializeObjectToFile<T>(T obj, string path)
		{
			FileInfo inf = new FileInfo(path);
			if (!inf.Directory.Exists) inf.Directory.Create();
			File.WriteAllText(path, SerializeObject<T>(obj));
		}

		public static T DeserializeObjectFromFile<T>(string path)
		{
			FileInfo inf = new FileInfo(path);
			if (!inf.Exists) return default(T);
			return DeserializeObject<T>(File.ReadAllText(path));
		}

		public static T DeserializeObject<T>(String xml)
		{
			try
			{
				XmlSerializer xs = new XmlSerializer(typeof(T));
				MemoryStream memoryStream = new MemoryStream(Encoding.Default.GetBytes(xml));
				XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Default);
				return (T)xs.Deserialize(memoryStream);
			}
			catch (Exception e)
			{
				throw new Exception(e.GetFullExceptionText());
			}
		}

	}
}
