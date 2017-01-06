using PaJaMa.Common;
using PaJaMa.DatabaseStudio.Classes;
using PaJaMa.DatabaseStudio.Compare.Classes;
using PaJaMa.DatabaseStudio.DataGenerate.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio
{
	public partial class frmMain : Form
	{
		public frmMain()
		{
			InitializeComponent();
			this.Text += " - " + this.GetType().Assembly.AssemblyVersion();
		}
		
		private void frmMain_Load(object sender, EventArgs e)
		{
			var settings = Properties.Settings.Default;

			this.DesktopLocation = new Point(settings.Left, settings.Top);
			if (settings.Maximized)
				this.WindowState = FormWindowState.Maximized;
			else
			{
				//if (settings.Left > 0)
				//	this.Left = settings.Left;
				//if (settings.Top > 0)
				//	this.Top = settings.Top;
				if (settings.Height > 0)
					this.Height = settings.Height;
				if (settings.Width > 0)
					this.Width = settings.Width;
			}
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized) return;

			var settings = Properties.Settings.Default;
			settings.Left = this.DesktopLocation.X;
			settings.Top = this.DesktopLocation.Y;
			if (this.WindowState == FormWindowState.Maximized)
				settings.Maximized = true;
			else
			{
				settings.Maximized = false;
				settings.Width = this.Width;
				settings.Height = this.Height;
			}
			settings.Save();
		}

		private void uc_QueryDatabase(object sender, QueryEventArgs e)
		{
			ucQuery1.LoadFromIDatabase(e);
			tabMain.SelectedTab = tabQuery;
		}
	}
}
