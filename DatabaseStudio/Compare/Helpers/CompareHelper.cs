using PaJaMa.Common;
using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
using PaJaMa.DatabaseStudio.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare.Helpers
{
	public class CompareHelper
	{
		public bool IsFrom2000OrLess { get; private set; }
		public bool IsTo2000OrLess { get; private set; }
		public List<TableWorkspace> TablesToSync { get; private set; }

		public Database FromDatabase { get; private set; }
		public Database ToDatabase { get; private set; }

		public CompareHelper(string fromConnectionString, string toConnectionString, BackgroundWorker worker)
		{
			FromDatabase = new Database(fromConnectionString);
			ToDatabase = new Database(toConnectionString);
			FromDatabase.PopulateChildren(false, worker);
			ToDatabase.PopulateChildren(false, worker);
		}

		public void Init(BackgroundWorker worker)
		{
			FromDatabase = new Database(FromDatabase.ConnectionString);
			ToDatabase = new Database(ToDatabase.ConnectionString);
			FromDatabase.PopulateChildren(false, worker);
			ToDatabase.PopulateChildren(false, worker);
		}

		public bool Synchronize(BackgroundWorker worker, List<WorkspaceBase> workspaces, DbTransaction trans)
		{
			Dictionary<WorkspaceBase, List<string>> scriptStrings = new Dictionary<WorkspaceBase, List<string>>();

			int totalProgress = 0;
			bool ignorePrompt = false;
			foreach (var ws in getSortedWorkspaces(workspaces))
			{
				if (!ws.SynchronizationItems.Any()) continue;

				scriptStrings.Add(ws, new List<string>());
				var allScripts = new Dictionary<int, StringBuilder>();
				foreach (var item in ws.SynchronizationItems)
				{
					if (item.Omit)
						continue;

					// TODO: need others?
					if (ws is DropWorkspace)
					{
						if (item.DatabaseObject.Synchronized)
							continue;

						item.DatabaseObject.Synchronized = true;
					}

					foreach (var kvp in item.Scripts)
					{
						if (kvp.Value.Length <= 0) continue;
						if (!allScripts.ContainsKey(kvp.Key)) allScripts.Add(kvp.Key, new StringBuilder());
						allScripts[kvp.Key].AppendLine(kvp.Value.ToString());
					}
				}
				foreach (var kvp in allScripts.OrderBy(s => s.Key))
				{
					if (kvp.Value.Length <= 0) continue;
					totalProgress++;
					scriptStrings[ws].Add(kvp.Value.ToString());
				}
			}

			try
			{
				using (var cmd = trans.Connection.CreateCommand())
				{
					cmd.Transaction = trans;

					int i = 0;
					foreach (var kvp in scriptStrings)
					{
						foreach (var script in kvp.Value)
						{
							i++;
							worker.ReportProgress(100 * i / totalProgress, string.Format("Synchronizing {0} {1} of {2}", kvp.Key.ToString(), i.ToString(), totalProgress.ToString()));
							cmd.CommandText = script;
							cmd.CommandTimeout = 600;
							try
							{
								cmd.ExecuteNonQuery();
							}
							catch (Exception ex)
							{
								if (!ignorePrompt)
								{
									var dlgResult = PaJaMa.WinControls.YesNoMessageDialog.Show("Failed to synchronize \"" + (kvp.Key is WorkspaceWithSourceBase && kvp.Key.TargetObject == null ? (kvp.Key as WorkspaceWithSourceBase).SourceObject.ObjectName : kvp.Key.TargetObject.ObjectName)
										+ "\": " + ex.Message + ". Would you like to continue?", "Error!", showNoToAll: false, showCancel: false);
									switch (dlgResult)
									{
										case WinControls.YesNoMessageDialogResult.No:
											return false;
										case WinControls.YesNoMessageDialogResult.YesToAll:
											ignorePrompt = true;
											break;
									}

									//var dlgResult = MessageBox.Show("Failed to synchronize \"" + (kvp.Key is WorkspaceWithSourceBase && kvp.Key.TargetObject == null ? (kvp.Key as WorkspaceWithSourceBase).SourceObject.ObjectName : kvp.Key.TargetObject.ObjectName)
									//	+ "\": " + ex.Message + ". Would you like to continue?", "Error!", MessageBoxButtons.YesNo);
									//if (dlgResult != DialogResult.Yes)
									//	return false;
								}
							}
						}
					}
				}

				//foreach (var ws in workspaces.OfType<TableWorkspace>())
				//{
				//	ws.TargetTable
				//}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Failed to synchronize: " + ex.Message);
				return false;
			}
			return true;
		}

		private List<WorkspaceBase> getSortedWorkspaces(List<WorkspaceBase> workspaces)
		{
			var sortedWorkspaces = new List<WorkspaceBase>();

			// make copy
			var currentWorkspaces = workspaces.ToList();

			while (currentWorkspaces.Count > 0)
			{
				foreach (var ws in currentWorkspaces)
				{
					var missing = new Dictionary<DatabaseObjectBase, List<DatabaseObjectBase>>();
					var tempSelected = sortedWorkspaces.ToList();
					tempSelected.Add(ws);
					populateMissingDependencies(tempSelected, missing);
					if (!missing.Any())
					{
						sortedWorkspaces.Add(ws);
						currentWorkspaces.Remove(ws);
						break;
					}
				}
			}

			return sortedWorkspaces;
		}

		//private List<DropWorkspace> getSortedDrops(List<DropWorkspace> objects)
		//{
		//	//var sortedObjects = new List<DropWorkspace>();

		//	//// make copy
		//	//var currentObjects = objects.ToList();

		//	//while (currentObjects.Count > 0)
		//	//{
		//	//	foreach (var obj in currentObjects)
		//	//	{
		//	//		var missing = new Dictionary<DatabaseObjectBase, List<DatabaseObjectBase>>();
		//	//		var tempSelected = sortedObjects.ToList();
		//	//		tempSelected.Add(obj);
		//	//		populateMissingObjectDependencies(tempSelected.Select(s => (DatabaseObjectBase)s.DatabaseObject).ToList(), missing, true);
		//	//		if (!missing.Any())
		//	//		{
		//	//			sortedObjects.Add(obj);
		//	//			currentObjects.Remove(obj);
		//	//			break;
		//	//		}
		//	//	}
		//	//}

		//	//return sortedObjects;
		//	return objects;
		//}

		//public bool DropObjects(List<DropWorkspace> objects, DbTransaction trans)
		//{
		//	using (var cmd = trans.Connection.CreateCommand())
		//	{
		//		cmd.Transaction = trans;

		//		int i = 0;
		//		foreach (var obj in getSortedDrops(objects.Where(t => t.Drop).ToList()))
		//		{
		//			i++;
		//			cmd.CommandText = obj.DatabaseObject.GetDropScript();

		//			// object was dropped by previous script
		//			if (string.IsNullOrEmpty(cmd.CommandText))
		//				continue;

		//			try
		//			{
		//				cmd.ExecuteNonQuery();
		//			}
		//			catch (Exception ex)
		//			{
		//				//if (!prompted)
		//				var dlgResult = MessageBox.Show("Failed to drop \"" + obj.DatabaseObject.Description
		//									+ "\": " + ex.Message + ". Would you like to continue?", "Error!", MessageBoxButtons.YesNo);
		//				if (dlgResult != DialogResult.Yes)
		//					return false;
		//			}
		//		}
		//	}

		//	return true;
		//}

		public Dictionary<DatabaseObjectBase, List<DatabaseObjectBase>> GetMissingDependencies(List<WorkspaceBase> selectedWorkspaces)
		{
			var missing = new Dictionary<DatabaseObjectBase, List<DatabaseObjectBase>>();
			populateMissingDependencies(selectedWorkspaces, missing);
			return missing;
		}

		private void populateMissingDependencies(List<WorkspaceBase> selectedWorkspaces, Dictionary<DatabaseObjectBase, List<DatabaseObjectBase>> missing)
		{
			foreach (var ws in selectedWorkspaces)
			{
				recursivelyPopulateMissingDependencies(ws is WorkspaceWithSourceBase ? (ws as WorkspaceWithSourceBase).SourceObject : ws.TargetObject,
					selectedWorkspaces, missing, new List<DatabaseObjectBase>(), ws is DropWorkspace);
			}
		}

		private void recursivelyPopulateMissingDependencies(DatabaseObjectBase parent, List<WorkspaceBase> selectedWorkspaces, Dictionary<DatabaseObjectBase, List<DatabaseObjectBase>> missing,
			List<DatabaseObjectBase> checkedObjects, bool isForDrop)
		{
			if (checkedObjects.Contains(parent)) return;

			checkedObjects.Add(parent);

			var toObjects = ToDatabase.GetDatabaseObjects(false).ConvertAll(s => (DatabaseObjectBase)s);
			toObjects.AddRange(from s in ToDatabase.Schemas
							   from t in s.Tables
							   select t);

			var selectedItems = from ws in selectedWorkspaces
								from si in ws.SynchronizationItems
								where !si.Omit
								select si;

			var sync = DatabaseObjectSynchronizationBase.GetSynchronization(parent);

			var missingDendencies = sync.GetMissingDependencies(toObjects, selectedItems.ToList(), isForDrop);
			foreach (var child in missingDendencies)
			{
				if (checkedObjects.Contains(child))
					continue;

				checkedObjects.Add(child);

				if (!missing.ContainsKey(parent))
					missing.Add(parent, new List<DatabaseObjectBase>());

				if (!missing[parent].Contains(child))
					missing[parent].Add(child);

				recursivelyPopulateMissingDependencies(child, selectedWorkspaces, missing, checkedObjects, isForDrop);
			}
		}
	}
}