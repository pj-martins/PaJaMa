using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class Permission : DatabaseObjectWithExtendedProperties
	{
		public string SchemaName { get; set; }
		public string PermissionSchemaName { get; set; }
		public string PermissionName { get; set; }
		public List<PermissionPrincipal> PermissionPrincipals { get; set; }

		public Database Database { get; set; }

		public override string ObjectName
		{
			get { return PermissionName; }
		}

		public Permission()
		{
			PermissionPrincipals = new List<PermissionPrincipal>();
		}

		public static void PopulatePermissions(Database database, DbConnection connection, List<ExtendedProperty> extendedProperties)
		{
			// TODO: 2000 or less
			if (database.Is2000OrLess)
				return; 
			
			string qry = @"select s.name as SchemaName, s2.name as PermissionSchemaName,
					coalesce(s2.name, o.name) as PermissionName, state_desc as GrantType, 
					permission_name as PermissionType, class_desc as PermissionType, pr.Name as PrincipalName
				from sys.database_permissions p
				join sys.database_principals pr on pr.principal_id = p.grantee_principal_id
				join sys.objects o on o.object_id = p.major_id
				join sys.schemas s on s.schema_id = o.schema_id
				left join sys.schemas s2 on s2.schema_id = p.major_id
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
							var tempPermission = rdr.ToObject<Permission>();
							var permission = database.Permissions.FirstOrDefault(p => p.SchemaName == tempPermission.SchemaName
								&& p.PermissionSchemaName == tempPermission.PermissionSchemaName && p.PermissionName == tempPermission.PermissionName);
							if (permission == null)
							{
								permission = tempPermission;
								permission.Database = database;
								database.Permissions.Add(permission);
							}

							var permissionPrincipal = rdr.ToObject<PermissionPrincipal>();
							permissionPrincipal.DatbasePrincipal = database.Principals.First(p => p.PrincipalName == rdr["PrincipalName"].ToString());
							permissionPrincipal.Permission = permission;
							permission.PermissionPrincipals.Add(permissionPrincipal);
						}
					}
				}
			}
		}
	}

	public class PermissionPrincipal
	{
		public string PermissionType { get; set; }
		public PermissionDescription PermissionDescription { get; set; }
		public DatabasePrincipal DatbasePrincipal { get; set; }
		public GrantType GrantType { get; set; }
		public Permission Permission { get; set; }

		public string GetCreateRemoveScript(bool create)
		{
			return string.Format(@"{0} {1} ON {2} {5} [{3}] {4}",
							create ?
							(GrantType == GrantType.GRANT_WITH_GRANT_OPTION ? GrantType.GRANT.ToString() : GrantType.ToString())
							: "REVOKE",
							PermissionType,
							PermissionDescription == PermissionDescription.SCHEMA ?
								string.Format("SCHEMA::[{0}]", Permission.PermissionSchemaName) :
								string.Format("[{0}].[{1}]", Permission.SchemaName, Permission.PermissionName),
							DatbasePrincipal.PrincipalName,
							create ? (GrantType == GrantType.GRANT_WITH_GRANT_OPTION ? "WITH GRANT OPTION" : string.Empty)
							: "CASCADE",
							create ? "TO" : "FROM");
		}

		public bool IsEqual(PermissionPrincipal permissionPrincipal)
		{
			if (PermissionType != permissionPrincipal.PermissionType)
				return false;

			if (PermissionDescription != permissionPrincipal.PermissionDescription)
				return false;

			if (DatbasePrincipal.PrincipalName != permissionPrincipal.DatbasePrincipal.PrincipalName)
				return false;

			if (GrantType != permissionPrincipal.GrantType)
				return false;

			return true;
		}
	}

	public enum PermissionDescription
	{
		DATABASE,
		OBJECT_OR_COLUMN,
		SCHEMA,
		DATABASE_PRINCIPAL,
		ASSEMBLY,
		TYPE,
		XML_SCHEMA_COLLECTION,
		MESSAGE_TYPE,
		SERVICE_CONTRACT,
		SERVICE,
		REMOTE_SERVICE_BINDING,
		ROUTE,
		FULLTEXT_CATALOG,
		SYMMETRIC_KEY,
		CERTIFICATE,
		ASYMMETRIC_KEY
	}

	public enum GrantType
	{
		DENY,
		REVOKE,
		GRANT,
		GRANT_WITH_GRANT_OPTION
	}
}
