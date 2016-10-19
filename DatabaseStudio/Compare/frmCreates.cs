using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Compare.Workspaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Compare
{
	public partial class frmCreates : Form
	{
		public WorkspaceWithSourceBase Workspace { get; set; }

		public frmCreates()
		{
			InitializeComponent();
		}

		private void frmStructureDetails_Load(object sender, EventArgs e)
		{
			var sync = DatabaseObjectSynchronizationBase.GetSynchronization(Workspace.SourceObject);

			txtFromScript.Text = string.Join("\r\n\r\n", sync.GetRawCreateText());
			if (Workspace.TargetObject == null)
				txtToScript.Text = string.Empty;
			else
			{
				sync = DatabaseObjectSynchronizationBase.GetSynchronization(Workspace.TargetObject);
				txtToScript.Text = sync.GetRawCreateText();
			}
			splitMain.Panel2Collapsed = Workspace.TargetObject == null;
		}

		private void txt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A)
			{
				var txt = sender as TextBox;
				txt.SelectAll();
			}
		}
	}
}
