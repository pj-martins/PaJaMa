//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Data.Common;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace PaJaMa.DatabaseStudio.Compare
//{
//	public class ProgrammibilityWorkspace
//	{
//		public string ObjectName { get; private set; }
//		public string ObjectType { get; private set; }
//		public string TargetObject { get; private set; }
//		public bool Select { get; set; }

//		public string UpdateText { get; private set; }

//		public void OnCreated()
//		{
//			TargetObject = ObjectName;
//		}

//		public static List<ProgrammibilityWorkspace> GetWorkspaces(DbConnection fromConnection, DbConnection toConnection)
//		{
//			DataTable fromFunctions = new DataTable();
//			DataTable toFunctions = new DataTable();
//			string cmdText = "select ROUTINE_NAME, ROUTINE_TYPE, ROUTINE_DEFINITION = OBJECT_DEFINITION(OBJECT_ID(ROUTINE_NAME)) from INFORMATION_SCHEMA.ROUTINES order by ROUTINE_NAME";
//			using (var fromCmd = fromConnection.CreateCommand())
//			{
//				fromCmd.CommandText = cmdText;
//				using (var rdr = fromCmd.ExecuteReader())
//				{
//					fromFunctions.Load(rdr);
//				}
//			}

//			using (var toCmd = toConnection.CreateCommand())
//			{
//				toCmd.CommandText = cmdText;
//				using (var rdr = toCmd.ExecuteReader())
//				{
//					toFunctions.Load(rdr);
//				}
//			}

//			List<ProgrammibilityWorkspace> ws = new List<ProgrammibilityWorkspace>();
//			foreach (DataRow dr in fromFunctions.Rows)
//			{
//				var toDr = toFunctions.Rows.OfType<DataRow>().FirstOrDefault(f => f["ROUTINE_NAME"].ToString() == dr["ROUTINE_NAME"].ToString());
//				if (toDr == null)
//				{
//					ws.Add(new ProgrammibilityWorkspace()
//					{
//						ObjectName = dr["ROUTINE_NAME"].ToString(),
//						ObjectType = dr["ROUTINE_TYPE"].ToString(),
//						UpdateText = dr["ROUTINE_DEFINITION"].ToString()
//					});
//				}
//				else if (toDr["ROUTINE_DEFINITION"].ToString().ToLower() != dr["ROUTINE_DEFINITION"].ToString().ToLower())
//				{
//					string updateDefinition = dr["ROUTINE_DEFINITION"].ToString();
//					// updateDefinition = "ALTER " + updateDefinition.Trim().Substring("CREATE ".Length);
//					updateDefinition = Regex.Replace(updateDefinition, "CREATE PROCEDURE", "ALTER PROCEDURE", RegexOptions.IgnoreCase);
//					ws.Add(new ProgrammibilityWorkspace()
//					{
//						ObjectName = dr["ROUTINE_NAME"].ToString(),
//						ObjectType = dr["ROUTINE_TYPE"].ToString(),
//						TargetObject = dr["ROUTINE_TYPE"].ToString(),
//						UpdateText = updateDefinition
//					});
//				}
//			}
//			return ws;
//		}

//		public static bool Sync(BackgroundWorker worker, List<ProgrammibilityWorkspace> workspaces, DbConnection toConnection, DbTransaction trans)
//		{
//			try
//			{
//				using (var cmd = toConnection.CreateCommand())
//				{
//					cmd.Transaction = trans;
//					foreach (var ws in workspaces)
//					{
//						var scripts = new Dictionary<ScriptType, StringBuilder>();
//						scripts.Add(2, new StringBuilder(ws.UpdateText));

//						List<string> scriptStrings = new List<string>();
//						foreach (var kvp in scripts.OrderBy(s => (int)s.Key))
//						{
//							if (kvp.Value.Length <= 0) continue;
//							scriptStrings.Add(kvp.Value.ToString());
//						}

//						foreach (var script in scriptStrings)
//						{
//							cmd.CommandText = script;
//							cmd.CommandTimeout = 600;
//							cmd.ExecuteNonQuery();
//						}

//						ws.OnCreated();
//					}
//				}
//			}
//			catch (Exception ex)
//			{
//				MessageBox.Show("Failed to synchronize: " + ex.Message);
//				return false;
//			}
//			return true;
//		}

//		//private string getScripts()
//		//{

//		//}
//	}
//}
