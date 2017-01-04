using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.Reminder
{
	public partial class frmMain : Form
	{
		private bool _closing;
		private TimeSpan _lastReminded;

		public frmMain()
		{
			InitializeComponent();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			var reminders = new List<Reminder>();
			if (!string.IsNullOrEmpty(Properties.Settings.Default.Reminders))
			{
				try
				{
					reminders = PaJaMa.Common.XmlSerialize.DeserializeObject<List<Reminder>>(Properties.Settings.Default.Reminders);
				}
				catch { }
			}

			gridMain.AutoGenerateColumns = false;
			gridMain.DataSource = new BindingList<Reminder>(reminders);

			this.WindowState = FormWindowState.Minimized;

			timMain_Tick(sender, e);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			var reminders = (gridMain.DataSource as BindingList<Reminder>).ToList();
			var settings = Properties.Settings.Default;
			settings.Reminders = PaJaMa.Common.XmlSerialize.SerializeObject<List<Reminder>>(reminders);
			settings.Save();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_closing) return;

			this.WindowState = FormWindowState.Minimized;
			this.Visible = false;
			this.ShowInTaskbar = false;
			e.Cancel = true;
		}

		private void notifyMain_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			showMe();
		}

		private void configureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			showMe();
		}

		private void showMe()
		{
			this.ShowInTaskbar = true;
			this.Visible = true;
			this.WindowState = FormWindowState.Normal;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_closing = true;
			this.Close();
		}

		private void timMain_Tick(object sender, EventArgs e)
		{
			if (gridMain.DataSource == null) return;

			var reminders = (gridMain.DataSource as BindingList<Reminder>).ToList();
			if (reminders == null) return;

			var latest = reminders
				.Where(r => r.Time != null && r.Time.TimeOfDay <= DateTime.Now.TimeOfDay)
				.OrderByDescending(r => r.Time.TimeOfDay)
				.FirstOrDefault();

			if (latest != null && latest.Time.TimeOfDay != _lastReminded)
			{
				_lastReminded = latest.Time.TimeOfDay;
				using (var frm = new frmTimedDialog())
				{
					frm.lblMain.Text = latest.Time.ToShortTimeString() + " - " + latest.Description;
					frm.ShowDialog();
				}
			}
		}
	}
}
