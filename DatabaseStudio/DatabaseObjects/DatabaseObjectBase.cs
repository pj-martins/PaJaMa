using PaJaMa.Common;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.DatabaseObjects
{
	public abstract class DatabaseObjectBase
	{
		public virtual bool Synchronized { get; set; }

		public abstract string ObjectName { get; }

		public string Description
		{
			get { return ToString() + " (" + ObjectType + ")"; }
		}

		public virtual string ObjectType
		{
			get { return this.GetType().Name; }
		}

		public static void PopulateObjects(Database database, DbConnection connection, List<ExtendedProperty> extendedProperties, bool condensed, BackgroundWorker worker)
		{
			if (worker != null) worker.ReportProgress(0, "Populating procedures, synonyms, permissions for " + connection.Database + "...");
			RoutineSynonym.PopulateRoutinesSynonyms(database, connection, extendedProperties);

			if (worker != null) worker.ReportProgress(0, "Populating views for " + connection.Database + "...");
			View.PopulateViews(database, connection, extendedProperties);

			if (!condensed)
			{
				try
				{
					if (worker != null) worker.ReportProgress(0, "Populating logins for " + connection.Database + "...");
					ServerLogin.PopulateServerLogins(database, connection, extendedProperties);
				}
				catch
				{
					// TODO: still want to compare other stuff so ignore failures here
				}

				try
				{
					if (worker != null) worker.ReportProgress(0, "Populating principals for " + connection.Database + "...");
					DatabasePrincipal.PopulatePrincipals(database, connection, extendedProperties);
				}
				catch
				{
					// TODO: still want to compare other stuff so ignore failures here
				}

				try
				{
					if (worker != null) worker.ReportProgress(0, "Populating permissions for " + connection.Database + "...");
					Permission.PopulatePermissions(database, connection, extendedProperties);
				}
				catch
				{
					// TODO: still want to compare other stuff so ignore failures here
				}

				try
				{
					if (worker != null) worker.ReportProgress(0, "Populating credentials for " + connection.Database + "...");
					Credential.PopulateCredentials(database, connection);
				}
				catch
				{
					// TODO: still want to compare other stuff so ignore failures here
				}
			}
		}

		public override string ToString()
		{
			return ObjectName;
		}
	}

	public abstract class DatabaseObjectWithExtendedProperties : DatabaseObjectBase
	{
		[Ignore]
		public List<ExtendedProperty> ExtendedProperties { get; set; }

		public DatabaseObjectWithExtendedProperties()
		{
			ExtendedProperties = new List<ExtendedProperty>();
		}
	}

	public interface IObjectWithExtendedProperty
	{
		List<ExtendedProperty> ExtendedProperties { get; set; }
	}
}
