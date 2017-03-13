using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.Angular2.Deploy
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			var jsText = File.ReadAllText("..\\..\\..\\src\\data.js");

			var firstNames = File.ReadAllLines(@"C:\Projects\GIT\PaJaMa\DatabaseStudio\DataGenerate\Content\FirstName.txt");
			var lastNames = File.ReadAllLines(@"C:\Projects\GIT\PaJaMa\DatabaseStudio\DataGenerate\Content\LastName.txt");

			var rand = new Random();
			foreach (var fld in new string[] { "coordinator", "requestedBy" })
			{
				var nameMatches = Regex.Matches(jsText, $"\"{fld}\": \"(.*?)\"");
				foreach (Match nameMatch in nameMatches)
				{
					var randomFirstName = firstNames[rand.Next(0, firstNames.Length - 1)];
					var randomLastName = lastNames[rand.Next(0, lastNames.Length - 1)];
					jsText = jsText.Replace(nameMatch.Groups[1].Value, $"{randomFirstName} {randomLastName}");
				}
			}

			var phoneMatches = Regex.Matches(jsText, $"\"phoneNumber\": \"\\(800\\) 555\\-(.*?)\"");
			foreach (Match phoneMatch in phoneMatches)
			{
				var randNum = rand.Next(1000, 9999);
				jsText = jsText.Replace(phoneMatch.Groups[1].Value, randNum.ToString());
			}
			*/
			new NPMPackage().Generate();
		}
	}
}

