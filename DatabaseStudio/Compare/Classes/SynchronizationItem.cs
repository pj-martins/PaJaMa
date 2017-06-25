using PaJaMa.DatabaseStudio.Compare.Workspaces;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Classes
{
	public class SynchronizationItem
	{
		public bool Omit { get; set; }
		public DatabaseObjectBase DatabaseObject { get; private set; }
		public List<Difference> Differences { get; private set; }
		public Dictionary<int, StringBuilder> Scripts { get; private set; }

		public string ObjectName
		{
			get { return DatabaseObject.ObjectName; }
		}

		public string ObjectType
		{
			get { return DatabaseObject.ObjectType; }
		}

		public string DifferenceText
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				foreach (var diff in Differences)
				{
					string line = diff.PropertyName;
					if (!string.IsNullOrEmpty(diff.SourceValue) || !string.IsNullOrEmpty(diff.TargetValue))
					{
						line += ": " + (string.IsNullOrEmpty(diff.SourceValue) ? "<NONE>" : diff.SourceValue) + " - " +
							 (string.IsNullOrEmpty(diff.TargetValue) ? "<NONE>" : diff.TargetValue);
					}
					sb.AppendLine(line);
				}
				return sb.ToString().Trim();
			}
		}

		public SynchronizationItem(DatabaseObjectBase obj)
		{
			DatabaseObject = obj;
			Differences = new List<Difference>();
			Scripts = new Dictionary<int, StringBuilder>();
		}

		public void AddScript(int level, string script)
		{
			if (!Scripts.ContainsKey(level))
				Scripts.Add(level, new StringBuilder());
			Scripts[level].AppendLine(script);
		}

        public override string ToString()
        {
            return ObjectName;
        }
    }

	public class SerializableSynchronizationItem
	{
		public bool Omit { get; set; }
		public string ObjectName { get; set; }
	}

	public class Difference
	{
		public const string CREATE = "CREATE";
		public const string ALTER = "ALTER";
		public const string DROP = "DROP";

		public string PropertyName { get; set; }
		public string SourceValue { get; set; }
		public string TargetValue { get; set; }
	}
}
