using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Compare.Helpers;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare
{
	public partial class ucDifferences : UserControl
	{
		public CompareHelper CompareHelper { get; set; }

		private WorkspaceBase _workspace;
		public WorkspaceBase Workspace
		{
			get { return _workspace; }
			set
			{
				_workspace = value;
				refreshPage();
			}
		}

		public ucDifferences()
		{
			InitializeComponent();
		}

		private void frmStructureDetails_Load(object sender, EventArgs e)
		{
		}

		private void refreshPage()
		{
			if (_workspace == null)
			{
				gridDifferences.DataSource = null;
				txtAlterScript.Text = string.Empty;
				return;
			}
			var ds = _workspace.SynchronizationItems.Where(d => d.Scripts.Any(s => s.Value.Length > 0)).ToList();
			gridDifferences.AutoGenerateColumns = false;
			gridDifferences.DataSource = new BindingList<SynchronizationItem>(ds);
			if (ds.Count > 0)
			{
				refreshScriptText();
			}
			else
			{
				txtAlterScript.Text = string.Empty;
			}
		}

		private void refreshScriptText()
		{
			txtAlterScript.Text = string.Empty;
			var ds = gridDifferences.DataSource as BindingList<SynchronizationItem>;
			Dictionary<int, StringBuilder> scripts = new Dictionary<int, StringBuilder>();
			foreach (var d in ds)
			{
				if (d.Omit) continue;
				//if (Workspace.SourceObject is IDefinitionObject && Workspace.TargetObject != null)
				{
					//txtSourceScript.Text = (Workspace.SourceObject.GetRawCreateText());
					//txtTargetScript.Text = (Workspace.TargetObject.GetRawCreateText());
					//splitDifferenceText.Panel2Collapsed = false;
				}
				//else
				//{
				foreach (var script in d.Scripts.OrderBy(s => s.Key))
				{
					if (script.Value.Length > 0)
					{
						if (!scripts.ContainsKey(script.Key))
							scripts.Add(script.Key, new StringBuilder());
						scripts[script.Key].AppendLine(script.Value.ToString());
					}
				}
				//}
			}

			foreach (var script in scripts.OrderBy(s => s.Key))
			{
				if (script.Value.Length > 0)
					txtAlterScript.Text += script.Value.ToString() + "\r\n";
			}
		}

		private void gridDifferences_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			gridDifferences.EndEdit();
			refreshScriptText();
		}
	}
}
