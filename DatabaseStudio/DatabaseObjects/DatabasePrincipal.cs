using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class DatabasePrincipal : DatabaseObjectWithExtendedProperties
	{
		public override string ObjectName
		{
			get { return PrincipalName; }
		}

		[Ignore]
		public int PrincipalID { get; set; }
		public string PrincipalName { get; set; }
		public PrincipalType PrincipalType { get; set; }
		public string DefaultSchema { get; set; }
		public string LoginName { get; set; }
		public bool IsFixedRole { get; set; }

		[Ignore]
		public List<DatabasePrincipal> ChildMembers { get; set; }

		[Ignore]
		public List<DatabaseObjectBase> Ownings { get; set; }
		public DatabasePrincipal Owner { get; set; }
		public AuthenticationType AuthenticationType { get; set; }

		public Database Database { get; set; }

		public string OwnerName
		{
			get { return Owner == null ? string.Empty : Owner.ObjectName; }
		}

		public override string ObjectType
		{
			get { return PrincipalType.ToString(); }
		}

		public DatabasePrincipal()
		{
			ChildMembers = new List<DatabasePrincipal>();
			Ownings = new List<DatabaseObjectBase>();
		}

		public static void PopulatePrincipals(Database database, DbConnection connection, List<ExtendedProperty> extendedProperties)
		{
			string qry = database.Is2000OrLess ? @"
select uid as PrincipalID, altuid as OwningPrincipalID, name as PrincipalName, 
	PrincipalType = case when issqlrole = 1 then 'DATABASEROLE', convert(bit, 0) as IsFixedRole
	when isntuser = 1 then 'WINDOWSUSER'
	else 'SQLUSER'
	end
from sysusers u

" : @"
select dp.principal_id as PrincipalID, dp.owning_principal_id as OwningPrincipalID, dp.name as PrincipalName, replace(dp.type_desc, '_', '') as PrincipalType, 
	dp.default_schema_name as DefaultSchema, sp.name as LoginName, dp.is_fixed_role as IsFixedRole
from sys.database_principals dp
left join sys.server_principals sp on sp.sid = dp.sid
-- where dp.name not in ('INFORMATION_SCHEMA', 'sys', 'guest', 'public')
";

			Dictionary<DatabasePrincipal, int> owners = new Dictionary<DatabasePrincipal, int>();
			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							var princ = rdr.ToObject<DatabasePrincipal>();
							princ.ExtendedProperties = extendedProperties.Where(ep => ep.Level1Object == princ.PrincipalName &&
								ep.Level1Type == "USER").ToList();
							if (rdr["OwningPrincipalID"] != DBNull.Value)
								owners.Add(princ, Convert.ToInt16(rdr["OwningPrincipalID"]));

							if (string.IsNullOrEmpty(princ.LoginName))
								princ.AuthenticationType = AuthenticationType.NONE;
							else
							{
								switch (princ.PrincipalType)
								{
									case PrincipalType.SQLUser:
										princ.AuthenticationType = AuthenticationType.INSTANCE;
										break;
									case PrincipalType.WindowsUser:
										princ.AuthenticationType = AuthenticationType.WINDOWS;
										break;
								}
							}

							princ.Database = database;
							if (string.IsNullOrEmpty(princ.DefaultSchema))
								princ.DefaultSchema = "dbo";
							database.Principals.Add(princ);
						}
					}
					rdr.Close();
				}
			}

			foreach (var owner in owners)
			{
				owner.Key.Owner = database.Principals.First(p => p.PrincipalID == owner.Value);
				owner.Key.Owner.Ownings.Add(owner.Key);
			}

			if (database.Is2000OrLess)
			{
				using (var cmd = connection.CreateCommand())
				{
					foreach (var role in database.Principals.Where(dp => dp.PrincipalType == PrincipalType.DatabaseRole))
					{
						cmd.CommandText = string.Format("exec sp_helprolemember [{0}]", role.PrincipalName);

						using (var rdr = cmd.ExecuteReader())
						{
							if (rdr.HasRows)
							{
								while (rdr.Read())
								{
									var child = database.Principals.FirstOrDefault(p => p.PrincipalName == rdr["MemberName"].ToString());
									if (child == null) continue;
									role.ChildMembers.Add(child);
								}
							}
							rdr.Close();
						}
					}
				}
			}
			else
			{
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = @"
select drb.role_principal_id as RolePrincipalID, drb.member_principal_id as MemberPrincipalID
from sys.database_role_members drb
"; ;
					using (var rdr = cmd.ExecuteReader())
					{
						if (rdr.HasRows)
						{
							while (rdr.Read())
							{
								var parent = database.Principals.FirstOrDefault(p => p.PrincipalID == (int)rdr["RolePrincipalID"]);
								var child = database.Principals.FirstOrDefault(p => p.PrincipalID == (int)rdr["MemberPrincipalID"]);
								if (child == null || parent == null) continue;
								parent.ChildMembers.Add(child);
							}
						}
						rdr.Close();
					}
				}
			}
		}
	}

	public enum PrincipalType
	{
		SQLUser,
		WindowsUser,
		DatabaseRole,
		WindowsGroup
	}

	public enum AuthenticationType
	{
		NONE,
		WINDOWS,
		INSTANCE
	}
}
