using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class ServerLogin : DatabaseObjectWithExtendedProperties
	{
		public override string ObjectName
		{
			get { return LoginName; }
		}

		public override string ObjectType
		{
			get { return LoginType.ToString(); }
		}

		public Database Database { get; set; }

		public string LoginName { get; set; }
		public LoginType LoginType { get; set; }
		public bool IsDisabled { get; set; }
		public string DefaultDatabaseName { get; set; }
		public string DefaultLanguageName { get; set; }
		public bool IsExpirationChecked { get; set; }
		public bool IsPolicyChecked { get; set; }

		public static void PopulateServerLogins(Database database, DbConnection connection, List<ExtendedProperty> extendedProperties)
		{
			// TODO: 2000 or less
			if (database.Is2000OrLess)
				return;

			string qry = @"
select p.name as LoginName, 
	p.default_database_name as DefaultDatabaseName,
	p.default_language_name as DefaultLanguageName,
	replace(p.type_desc, '_', '') as LoginType,
	isnull(l.is_expiration_checked, 0) as IsExpirationChecked, 
	isnull(l.is_disabled, 0) as IsDisabled, 
	isnull(l.is_policy_checked, 0) as IsPolicyChecked
from sys.server_principals p
left join sys.sql_logins l on l.principal_id = p.principal_id
where p.type in ('U', 'S') and p.name not in ('INFORMATION_SCHEMA', 'sys', 'guest', 'public', 'dbo')
-- and p.sid in (select sid from sys.database_principals)
";
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							var login = rdr.ToObject<ServerLogin>();
							login.ExtendedProperties = extendedProperties.Where(ep => ep.ObjectType == LoginType.SQLLogin.ToString() &&
								ep.Level1Object == login.LoginName).ToList();
							login.Database = database;
							database.ServerLogins.Add(login);
						}
					}
					rdr.Close();
				}
			}
		}
	}

	public enum LoginType
	{
		SQLLogin,
		WindowsLogin
	}
}
