﻿using System;
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

		public void Generate()
		{
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
					File.Copy(file, file.Replace(srcDir, targetDir));
					continue;
				}

				var targetFileWithoutExtension = targetFile.FullName.Substring(0, targetFile.FullName.Length - 3);
				File.Copy(fileWithoutExtension + ".js", targetFileWithoutExtension + ".js", true);
				File.Copy(fileWithoutExtension + ".js.map", targetFileWithoutExtension + ".js.map", true);


				string tsInputText = File.ReadAllText(file);

				var sb = new StringBuilder();

				var importMatches = Regex.Matches(tsInputText, "import {.*?} from .*", RegexOptions.Multiline);
				foreach (Match import in importMatches)
				{
					sb.AppendLine(import.Groups[0].Value.Trim());
				}

				var exportClasses = Regex.Matches(tsInputText, "export(.*?) (class|interface) (.*?) (.*?){", RegexOptions.Multiline);

				foreach (Match cls in exportClasses)
				{
					var classNameWith = cls.Groups[3].Value.Trim() + " " + cls.Groups[4].Value.Trim();

					sb.AppendLine();
					sb.AppendLine($"export declare{cls.Groups[1].Value} {cls.Groups[2].Value} {classNameWith} {{");

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
					File.WriteAllText(targetFileWithoutExtension + ".d.ts", sb.ToString());
				}
			}

			if (_sbUnsupportedTypes.Length > 0)
			{
				Console.Write("\r\nUnsupported Types:\r\n" + _sbUnsupportedTypes.ToString());
			}

			Console.WriteLine("publish?");
			string rtv = Console.ReadLine();
			if (rtv.ToLower().StartsWith("y"))
			{
				var fullPath = new FileInfo("NPMPublish.bat").FullName;
				Directory.SetCurrentDirectory(new DirectoryInfo(targetDir).Parent.FullName);
				Process.Start(fullPath).WaitForExit();
			}
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
				if (curr.Contains("="))
					curr = curr.Substring(0, curr.IndexOf("="));
				paramString += curr.Trim();
				if (constructorFields != null && !a.StartsWith("private"))
					constructorFields.Add(curr.Trim());
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
