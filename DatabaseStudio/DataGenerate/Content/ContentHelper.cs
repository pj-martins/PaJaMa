using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DataGenerate.Content
{
	public class ContentHelper
	{
		const string NAMESPACE = "PaJaMa.DatabaseStudio.DataGenerate.Content";

		public static List<string> GetStrings()
		{
			var strings = new List<string>();
			foreach (var rn in typeof(ContentHelper).Assembly.GetManifestResourceNames())
			{
				Match m = Regex.Match(rn, NAMESPACE.Replace(".", "\\.") + "\\.(.*?)\\.txt");
				if (m.Success)
				{
					strings.Add(m.Groups[1].Value.CamelCaseToSpaced());
				}
			}
			return strings;
		}
	}
}
