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
	public class CredentialSynchronization : DatabaseObjectSynchronizationBase<Credential>
	{
		public CredentialSynchronization(Credential credential) : base(credential)
		{
		}

		public override List<SynchronizationItem> GetCreateItems()
		{
			return getStandardItems(string.Format(@"CREATE CREDENTIAL [{0}] WITH IDENTITY = '{1}'", databaseObject.CredentialName, databaseObject.CredentialIdentity));
		}

		public override List<SynchronizationItem> GetSynchronizationItems(DatabaseObjectBase target)
		{
			if (target == null)
				return base.GetSynchronizationItems(target);

			var targetCredential = target as Credential;
			if (databaseObject.CredentialIdentity != targetCredential.CredentialIdentity)
			{
				var item = new SynchronizationItem(databaseObject);
				item.Differences.AddRange(GetPropertyDifferences(target));
				item.AddScript(7, string.Format(@"ALTER CREDENTIAL [{0}] WITH IDENTITY = '{1}'", databaseObject.CredentialName, databaseObject.CredentialIdentity));

				return new List<SynchronizationItem>() { item };
			}

			return new List<SynchronizationItem>();
		}
	}
}
