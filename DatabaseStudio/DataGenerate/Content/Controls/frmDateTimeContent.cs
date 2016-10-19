using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.DataGenerate.Content.Controls
{
	public partial class frmDateTimeContent : frmContentBase
	{
		public frmDateTimeContent()
		{
			InitializeComponent();
		}

		protected override void btnOK_Click(object sender, EventArgs e)
		{
			var numContent = Content as DateTimeContent;
			numContent.Min = numMin.Value;
			numContent.Max = numMax.Value;
			base.btnOK_Click(sender, e);
		}

		private void frmNumericContent_Load(object sender, EventArgs e)
		{
		}
	}
}
