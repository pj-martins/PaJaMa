using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.Angular2.Deploy
{
	public class NPMPackage
	{
		private StringBuilder _sbUnsupportedTypes = new StringBuilder();

		private void recursivelyCopy(DirectoryInfo srcDir, DirectoryInfo destDir)
		{
			if (!destDir.Exists) destDir.Create();
			foreach (var finf in srcDir.GetFiles("*.ts").Union(srcDir.GetFiles("*.css")))
			{
				string srcFileText = File.ReadAllText(finf.FullName);
				srcFileText = srcFileText.Replace("../../pajama", "pajama").Replace("../pajama", "pajama");
				File.WriteAllText(finf.FullName.Replace(srcDir.FullName, destDir.FullName), srcFileText);
			}

			foreach (var dinf in srcDir.GetDirectories())
			{
				recursivelyCopy(dinf, new DirectoryInfo(Path.Combine(destDir.FullName, dinf.Name)));
			}
		}

		public void Generate()
		{
			int minorVersion = 0;

			var sbUnsupportedTypes = new StringBuilder();

			var srcDir = Path.GetFullPath(@"..\..\..\src\pajama\");
			var files = getFiles(new DirectoryInfo(srcDir));
			var targetDir = Path.Combine(Path.GetTempPath(), "pajamaangular", "pajama") + "\\";
			if (Directory.Exists(targetDir))
				Directory.Delete(targetDir, true);
			Directory.CreateDirectory(targetDir);
			foreach (var file in files)
			{
				Console.WriteLine(file);
				var fileWithoutExtension = file.Substring(0, file.Length - 3);
				var targetFile = new FileInfo(file.Replace(srcDir, targetDir));
				if (!targetFile.Directory.Exists)
					Directory.CreateDirectory(targetFile.Directory.FullName);

				if (file.EndsWith(".css") || file.EndsWith("package.json"))
				{
					if (file.EndsWith("package.json"))
					{
						string json = File.ReadAllText(file);
						Match m = Regex.Match(json, "\"version\": \"(.*?)\\.(.*?)\\.(.*?)\"");
						minorVersion = Convert.ToInt16(m.Groups[3].Value) + 1;
						json = json.Replace(m.Groups[0].Value, m.Groups[0].Value.Replace($"{m.Groups[1].Value}.{m.Groups[2].Value}.{m.Groups[3].Value}",
							$"{m.Groups[1].Value}.{m.Groups[2].Value}.{minorVersion}"));
						File.WriteAllText(file, json);
					}
					File.Copy(file, file.Replace(srcDir, targetDir));
					continue;
				}

				var targetFileWithoutExtension = targetFile.FullName.Substring(0, targetFile.FullName.Length - 3);
				File.Copy(fileWithoutExtension + ".js", targetFileWithoutExtension + ".js", true);
				File.Copy(fileWithoutExtension + ".js.map", targetFileWithoutExtension + ".js.map", true);

				// TEMP
				File.Copy(fileWithoutExtension + ".ts", targetFileWithoutExtension + ".ts", true);


				string tsInputText = File.ReadAllText(file);

				var sb = new StringBuilder();

				var importMatches = Regex.Matches(tsInputText, "import {.*?} from .*", RegexOptions.Multiline);
				foreach (Match import in importMatches)
				{
					sb.AppendLine(import.Groups[0].Value.Trim());
				}

				var exportMatches = Regex.Matches(tsInputText, "export {.*?} from .*", RegexOptions.Multiline);
				foreach (Match export in exportMatches)
				{
					sb.AppendLine(export.Groups[0].Value.Trim());
				}

				var exportClasses = Regex.Matches(tsInputText, "(export)?(.*?) (class|interface) (.*?) (.*?){", RegexOptions.Multiline);

				foreach (Match cls in exportClasses)
				{
					var classNameWith = cls.Groups[4].Value.Trim() + " " + cls.Groups[5].Value.Trim();

					sb.AppendLine();
					var prepend = cls.Groups[2].Value;
					if (!string.IsNullOrEmpty(prepend) && !prepend.StartsWith(" "))
						prepend = " " + prepend;
					var preDeclare = cls.Groups[1].Value.Trim();
					if (!string.IsNullOrEmpty(preDeclare))
						preDeclare += " ";
					sb.AppendLine($"{preDeclare}declare{prepend} {cls.Groups[3].Value} {classNameWith} {{");

					var body = Regex.Match(tsInputText, $"{cls.Groups[0].Value}(.*?)\r\n}}", RegexOptions.Singleline).Groups[1].Value;
					body = Regex.Replace(Regex.Replace(body, "@Input\\(\\) ?", ""), "@Output\\(\\) ?", "");
					createBody(body, sb);

					sb.AppendLine("}");
					sb.AppendLine();
				}

				var exportEnums = Regex.Matches(tsInputText, "export enum (.*?) (.*?){(.*?)\r\n}", RegexOptions.Singleline);

				foreach (Match en in exportEnums)
				{
					sb.AppendLine(en.Groups[0].Value);
				}

				if (exportClasses.Count > 0 || exportEnums.Count > 0)
				{
					// File.WriteAllText(targetFileWithoutExtension + ".d.ts", sb.ToString());
				}
			}

			if (_sbUnsupportedTypes.Length > 0)
			{
				Console.Write("\r\nUnsupported Types:\r\n" + _sbUnsupportedTypes.ToString());
			}

			Directory.SetCurrentDirectory(new DirectoryInfo(targetDir).Parent.FullName);
			Process.Start(new ProcessStartInfo("npm", "publish pajama")).WaitForExit();

			Directory.SetCurrentDirectory(new DirectoryInfo(srcDir).Parent.FullName);
			var distFile = Path.GetFullPath(@"..\dist\package.json");
			string distJson = File.ReadAllText(distFile);
			Match mDist = Regex.Match(distJson, "\"pajama\": \"(.*?)\\.(.*?)\\.(.*?)\"");

			distJson = distJson.Replace(mDist.Groups[0].Value, mDist.Groups[0].Value.Replace($"{mDist.Groups[1].Value}.{mDist.Groups[2].Value}.{mDist.Groups[3].Value}",
							$"{mDist.Groups[1].Value}.{mDist.Groups[2].Value}.{minorVersion}"));
			File.WriteAllText(distFile, distJson);

			Directory.SetCurrentDirectory(new FileInfo(distFile).Directory.FullName);
			string nodeModulesDirectory = "node_modules\\pajama";
			if (Directory.Exists(nodeModulesDirectory))
				Directory.Delete(nodeModulesDirectory, true);
			Process.Start(new ProcessStartInfo("npm", "install")).WaitForExit();

			recursivelyCopy(new DirectoryInfo(Path.GetFullPath("..\\src\\app")), new DirectoryInfo(Path.GetFullPath("..\\dist\\app")));

		}

		private void createBody(string bodyText, StringBuilder sb)
		{
			createFields(bodyText, sb);
			createProps(bodyText, sb);
			createFunctions(bodyText, sb);
		}

		private void createFields(string bodyText, StringBuilder sb)
		{
			var matches = Regex.Matches(bodyText, "^(\t)([A-Za-z].*?)[:|=](.*);", RegexOptions.Multiline);

			var constrMatch = Regex.Match(bodyText, "[\t| ]constructor\\((.*?)\\)");
			List<string> constructorFields = new List<string>();
			if (constrMatch.Success)
			{
				sb.AppendLine($"\tconstructor({getParamString(constrMatch.Groups[1].Value, constructorFields)});");
			}

			if (matches.Count > 0 || constructorFields.Any())
				sb.AppendLine("\r\n\t// fields");

			foreach (var c in constructorFields)
			{
				sb.AppendLine($"\t{c};");
			}

			foreach (Match match in matches)
			{
				var matchText = match.Groups[0].Value.Trim();
				generateField(matchText, sb);

			}


		}

		private string getParamString(string currParams, List<string> constructorFields)
		{
			var paramString = "";
			var all = currParams.Split(',');
			foreach (var a in all)
			{
				if (!string.IsNullOrEmpty(paramString))
					paramString += ", ";
				var curr = Regex.Replace(a, "(public|private|protected) ", "");
				string field = a;
				bool hasDefault = false;
				if (curr.Contains("="))
				{
					curr = curr.Substring(0, curr.IndexOf("="));
					field = a.Substring(0, a.IndexOf("="));
					hasDefault = true;
				}

				if (!hasDefault || curr.Contains("?:"))
					paramString += curr.Trim();
				else if (hasDefault)
					paramString += curr.Replace(":", "?:").Trim();
				if (constructorFields != null && !field.Trim().StartsWith("private"))
					constructorFields.Add(field.Replace("public ", "").Trim());
			}
			return paramString;
		}

		private void generateField(string matchText, StringBuilder sb)
		{
			if (matchText.Trim().StartsWith("private "))
				return;

			if (matchText.StartsWith("public "))
				matchText = matchText.Substring(7);

			Match partsMatch = null;
			if (matchText.Contains(":") && !matchText.Contains("= "))
				sb.AppendLine("\t" + matchText.Trim());
			else if ((partsMatch = Regex.Match(matchText, "(.*?):(.*?)=.*?;")).Success)
				sb.AppendLine($"\t{partsMatch.Groups[1].Value.Trim()}: {partsMatch.Groups[2].Value.Trim()};");
			else if ((partsMatch = Regex.Match(matchText, "(.*?)=(.*?);")).Success)
			{
				var variableName = partsMatch.Groups[1].Value.Trim();
				var typeVal = partsMatch.Groups[2].Value.Trim();
				var newMatch = Regex.Match(typeVal, "new (.*?)\\(\\)");
				if (newMatch.Success)
				{
					sb.AppendLine($"\t{variableName}: {newMatch.Groups[1].Value.Trim()};");
				}
				else
				{
					switch (typeVal)
					{
						case "true":
						case "false":
							typeVal = "boolean";
							break;
						case "[]":
							typeVal = "Array<any>";
							break;
						default:
							// TODO: others
							int tempInt = -1;
							if (int.TryParse(typeVal, out tempInt))
								typeVal = "number";
							else
							{
								_sbUnsupportedTypes.AppendLine(typeVal);
								typeVal = "any";
							}
							break;
					}
					sb.AppendLine($"\t{variableName}: {typeVal.Trim()};");
				}
			}
			else
			{
				throw new NotImplementedException(matchText);
			}
		}

		private void createProps(string bodyText, StringBuilder sb)
		{
			var matches = Regex.Matches(bodyText, "^(\t)(.*?)get(.*?)\\(\\)(.*?){", RegexOptions.Multiline);

			bool firstIn = true;
			foreach (Match match in matches)
			{
				if (match.Groups[2].Value == "private ")
					continue;

				var propName = match.Groups[3].Value.Trim();

				var setMatch = Regex.Match(bodyText, $"^(\t)(.*?)set {propName}\\(", RegexOptions.Multiline);
				if (!setMatch.Success || setMatch.Groups[3].Value.StartsWith("private"))
					continue;

				var matchText = (propName + match.Groups[4].Value).Trim();
				if (!matchText.Contains(":"))
					matchText += ": any;";
				else
					matchText += ";";

				if (firstIn)
					sb.AppendLine("\r\n\t// props");

				sb.AppendLine("\t" + matchText.Trim());

				firstIn = false;
			}
		}

		private void createFunctions(string bodyText, StringBuilder sb)
		{
			var matches = Regex.Matches(bodyText, "^(\t)([A-Za-z_].*?)\\((.*?)\\)(.*){", RegexOptions.Multiline);

			bool firstIn = true;
			foreach (Match match in matches)
			{
				var funcName = match.Groups[2].Value;
				if (funcName.StartsWith("private ") ||
					funcName == "constructor" ||
					funcName.StartsWith("get ") ||
					funcName.StartsWith("set ") ||
					funcName.StartsWith("protected get ") ||
					funcName.StartsWith("protected set ") ||
					funcName.StartsWith("\t"))
					continue;

				if (firstIn)
					sb.AppendLine("\r\n\t// functions");

				var returnVal = match.Groups[4].Value.Trim();

				// yuck!
				if (funcName.Trim().EndsWith("="))
				{
					funcName = funcName.Replace("=", "").Trim() + ":";
					returnVal = Regex.Replace(returnVal, ":(.*?)=>", " =>$1");
				}

				returnVal = (string.IsNullOrEmpty(returnVal) ? ": void" : returnVal);
				returnVal = returnVal.Trim() + ";";

				sb.AppendLine($"\t{funcName}({getParamString(match.Groups[3].Value, null)})" + returnVal);

				firstIn = false;
			}
		}

		private List<string> getFiles(DirectoryInfo dinf)
		{
			var lst = new List<string>();
			lst.AddRange(dinf.GetFiles("*.ts").Select(f => f.FullName));
			lst.AddRange(dinf.GetFiles("*.css").Select(f => f.FullName));
			lst.AddRange(dinf.GetFiles("package.json").Select(f => f.FullName));
			lst.AddRange(dinf.GetDirectories().SelectMany(d => getFiles(d)));
			return lst;
		}
	}
}
