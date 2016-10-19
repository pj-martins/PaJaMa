using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Classes
{
	public class ServerLoginSynchronization : DatabaseObjectSynchronizationBase<ServerLogin>
	{
		public ServerLoginSynchronization(ServerLogin login)
			: base(login)
		{
		}

		public override List<SynchronizationItem> GetDropItems()
		{
			if (databaseObject.LoginType == LoginType.SQLLogin)
			{
				return getStandardDropItems(string.Format("DROP LOGIN [{0}]", databaseObject.ObjectName));
			}
			return getStandardDropItems(string.Format("DROP USER [{0}]", databaseObject.ObjectName));
		}

		private string getLoginScript(bool create)
		{
			string script = (create ? "CREATE " : "ALTER ")
				+ string.Format("LOGIN [{0}]{1} WITH PASSWORD=N'p@ssw0rd', DEFAULT_DATABASE=[{2}], DEFAULT_LANGUAGE=[{3}]",
				databaseObject.LoginName, databaseObject.LoginType == LoginType.WindowsLogin ? " FROM WINDOWS" : "", databaseObject.DefaultDatabaseName, databaseObject.DefaultLanguageName);

			script += string.Format(", CHECK_EXPIRATION={0}", databaseObject.IsExpirationChecked ? "ON" : "OFF");
			script += string.Format(", CHECK_POLICY={0}", databaseObject.IsPolicyChecked ? "ON" : "OFF");

			return script;
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			var items = getStandardItems(getLoginScript(true));
			if (databaseObject.IsDisabled)
			{
				var item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference() { PropertyName = "Disabled" });
				item.AddScript(7, string.Format("\r\nALTER LOGIN [{0}] DISABLE", databaseObject.LoginName));
				items.Add(item);
			}
			return items;
		}

		public override List<SynchronizationItem> GetAlterItems(DatabaseObjectBase target)
		{
			var diff = GetPropertyDifferences(target);
			if (diff.Count == 1 && diff[0].PropertyName == "IsDisabled")
			{
				var item = new SynchronizationItem(databaseObject);
				item.Differences.Add(new Difference() { PropertyName = "Disabled" });
				item.AddScript(7, string.Format("\r\nALTER LOGIN [{0}] {1}", databaseObject.LoginName, databaseObject.IsDisabled ? "DISABLE" : "ENABLE"));
				return new List<SynchronizationItem>() { item };
			}
			return new List<SynchronizationItem>();
		}
	}
}
