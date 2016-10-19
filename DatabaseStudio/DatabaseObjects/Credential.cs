using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public class Credential : DatabaseObjectBase
	{
		public string CredentialName { get; set; }
		public string CredentialIdentity { get; set; }

		public override string ObjectName
		{
			get { return CredentialName; }
		}

		public static void PopulateCredentials(Database database, DbConnection connection)
		{
			// TODO: 2000 or less
			if (database.Is2000OrLess)
				return;

			string qry = @"select name as CredentialName, credential_identity as CredentialIdentity from sys.credentials";

			using (var cmd = connection.CreateCommand())
			{
				cmd.CommandText = qry;
				using (var rdr = cmd.ExecuteReader())
				{
					if (rdr.HasRows)
					{
						while (rdr.Read())
						{
							database.Credentials.Add(rdr.ToObject<Credential>());
						}
					}
					rdr.Close();
				}
			}
		}
	}
}
