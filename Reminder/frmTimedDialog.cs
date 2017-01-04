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
	public partial class frmTimedDialog : Form
	{
		const int SECONDS_TO_WAIT = 10;

		private DateTime _start;

		public frmTimedDialog()
		{
			InitializeComponent();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void timMain_Tick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;
			btnOK.Text = "OK (" + Math.Round((SECONDS_TO_WAIT - (now - _start).TotalSeconds), 0).ToString() + ")";
			if ((now - _start).TotalSeconds >= SECONDS_TO_WAIT)
				this.Close();
		}

		private void frmTimedDialog_Load(object sender, EventArgs e)
		{
			_start = DateTime.Now;
			timMain_Tick(sender, e);
		}
	}
}
