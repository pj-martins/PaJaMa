using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaJaMa.DatabaseStudio.DatabaseObjects;

namespace PaJaMa.DatabaseStudio.Compare.Workspaces
{
	public class DatabaseWorkspace : WorkspaceWithSourceBase
	{
		public DatabaseWorkspace(DatabaseObjectBase sourceObject, DatabaseObjectBase targetObject) : base(sourceObject, targetObject)
		{
		}
	}
}
