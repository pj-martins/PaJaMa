using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare.Workspaces
{
	// excludes tables
	public class ObjectWorkspace : WorkspaceWithSourceBase
	{
		public string Type
		{
			get { return SourceObject.ObjectType; }
		}

		public string Source { get { return SourceObject.ToString(); } }
		public string Target { get { return TargetObject == null ? string.Empty : TargetObject.ToString(); } }

		public ObjectWorkspace(CompareHelper compareHelper, DatabaseObjectBase sourceObject, DatabaseObjectBase targetObject) : base(sourceObject, targetObject)
		{
		}

		public override string ToString()
		{
			return Source;
		}
	}

	public class SerializableObjectWorkspace
	{
		public string SourceObjectName { get; set; }
		public string TargetObjectName { get; set; }
		public string ObjectType { get; set; }

		public static SerializableObjectWorkspace GetFromObjectWorkspace(ObjectWorkspace ws)
		{
			return new SerializableObjectWorkspace()
			{
				SourceObjectName = ws.Source,
				TargetObjectName = ws.Target,
				ObjectType = ws.Type
			};
		}
	}

	public class ObjectWorkspaceList
	{
		public List<ObjectWorkspace> Workspaces { get; private set; }
		public List<DropWorkspace> DropWorkspaces { get; private set; }

		public ObjectWorkspaceList()
		{
			Workspaces = new List<ObjectWorkspace>();
			DropWorkspaces = new List<DropWorkspace>();
		}

		public static ObjectWorkspaceList GetObjectWorkspaces(CompareHelper compareHelper)
		{
			var lst = new ObjectWorkspaceList();

			var fromObjs = compareHelper.FromDatabase.GetDatabaseObjects(true);
			var toObjs = compareHelper.ToDatabase.GetDatabaseObjects(true);

			foreach (var def in fromObjs)
			{
				DatabaseObjectBase sourceDef = def;
				DatabaseObjectBase targetDef = toObjs.FirstOrDefault(t =>
					t.ToString() == def.ToString() && t.ObjectType == def.ObjectType);
				lst.Workspaces.Add(new ObjectWorkspace(compareHelper, sourceDef, targetDef));
			}


			foreach (var def in toObjs
				.Where(x => !fromObjs.Any(d => d.ToString() == x.ToString() && d.ObjectType == x.ObjectType)))
			{
				lst.DropWorkspaces.Add(new DropWorkspace(def));
			}

			return lst;
		}
	}
}
