using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Query.QueryBuilder
{
	public partial class frmQueryBuilder : Form
	{
		private QueryBuilderHelper _queryBuilderHelper;
		private Control _options;

		public frmQueryBuilder(QueryBuilderHelper queryBuilderHelper)
		{
			InitializeComponent();
			_queryBuilderHelper = queryBuilderHelper;
		}

		private void frmQueryBuilder_Load(object sender, EventArgs e)
		{

		}

		private void wizOptions_ShowFromNext(object sender, EventArgs e)
		{
			if (rdbSelect.Checked)
			{
				_options = new SelectOptions.ucSelectOptions(_queryBuilderHelper);
			}

			_options.Dock = DockStyle.Fill;
			this.pageOptions.Controls.Clear();
			this.pageOptions.Controls.Add(_options);
			_options.BringToFront();
		}

		private void pageStart_CloseFromNext(object sender, WinControls.Wizard.PageEventArgs e)
		{
		}

		private void pageStart_ShowFromBack(object sender, EventArgs e)
		{
		}

		public string GetQuery()
		{
			return (_options as IOptions).GetQuery();
		}
	}
}
