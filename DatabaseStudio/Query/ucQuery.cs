using PaJaMa.DatabaseStudio.Classes;
using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.Query.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Query
{
	public partial class ucQuery : UserControl
	{
		public ucQuery()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			if (tabMain.TabPages.Count < 1)
				newWorkspaceToolStripMenuItem_Click(sender, e);
		}

		private void newWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var uc = new ucWorkspace();
			var tab = new TabPage("Workspace " + (tabMain.TabPages.Count + 1).ToString());
			uc.Dock = DockStyle.Fill;
			tab.Controls.Add(uc);
			tabMain.TabPages.Add(tab);
			tabMain.SelectedTab = tab;
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			if (tabMain.SelectedTab != null)
			{
				(tabMain.SelectedTab.Controls[0] as ucWorkspace).Disconnect();
				tabMain.TabPages.Remove(tabMain.SelectedTab);
			}
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				if (tabMain.SelectedTab != null)
				{
					var workSpace = (tabMain.SelectedTab.Controls[0] as ucWorkspace).GetWorkspace();
					if (string.IsNullOrEmpty(workSpace.ConnectionString) || workSpace.ConnectionType == null)
						MessageBox.Show("Incomplete workspace.");
					else
					{
						using (var dlg = new SaveFileDialog())
						{
							dlg.Filter = "DatabaseStudio files (*.dbs)|*.dbs";
							dlg.Title = "Workspace";
							if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
							{
								tabMain.SelectedTab.Text = new FileInfo(dlg.FileName).Name;
								PaJaMa.Common.XmlSerialize.SerializeObjectToFile<Workspace>(workSpace, dlg.FileName);
								MessageBox.Show("Workspace saved.");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void loadToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				using (var dlg = new OpenFileDialog())
				{
					dlg.Filter = "DatabaseStudio files (*.dbs)|*.dbs";
					dlg.Title = "Workspace";
					if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						var workspace = PaJaMa.Common.XmlSerialize.DeserializeObjectFromFile<Workspace>(dlg.FileName);
						newWorkspaceToolStripMenuItem_Click(sender, e);

						tabMain.SelectedTab.Text = new FileInfo(dlg.FileName).Name;
						(tabMain.SelectedTab.Controls[0] as ucWorkspace).LoadWorkspace(workspace);
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public void LoadFromIDatabase(QueryEventArgs args)
		{
			var uc = new ucWorkspace();
			var tab = new TabPage("Workspace " + (tabMain.TabPages.Count + 1).ToString());
			uc.Dock = DockStyle.Fill;
			tab.Controls.Add(uc);
			tabMain.TabPages.Add(tab);
			tabMain.SelectedTab = tab;
			uc.LoadFromIDatabase(args);
		}
	}
}
