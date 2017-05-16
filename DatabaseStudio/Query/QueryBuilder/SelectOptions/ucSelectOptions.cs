using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Query.QueryBuilder.SelectOptions
{
	public partial class ucSelectOptions : UserControl, IOptions
	{
		private QueryBuilderHelper _queryBuilderHelper;
		public ucSelectOptions(QueryBuilderHelper queryBuilderHelper)
		{
			InitializeComponent();
			_queryBuilderHelper = queryBuilderHelper;
			addFromJoin(true);
		}

		private void btnAddJoin_Click(object sender, EventArgs e)
		{
			addFromJoin(false);
		}

		private void addFromJoin(bool isFrom)
		{
			ucFromJoin ucJoin = new ucFromJoin(_queryBuilderHelper);
			ucJoin.IsFrom = isFrom;
			ucJoin.Dock = DockStyle.Top;
			this.pnlFromJoins.Controls.Add(ucJoin);
			if (isFrom)
				this.pnlFromJoins.Height = ucJoin.Height;
			else
				this.pnlFromJoins.Height += ucJoin.Height;
		}

		public string GetQuery()
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine("SELECT");
			foreach (var ucFromJoin in this.pnlFromJoins.Controls.OfType<ucFromJoin>())
			{
				sb.AppendLine((ucFromJoin.IsFrom ? "FROM " : "LEFT JOIN ") +
					string.Format("[{0}].[{1}]{2}", ucFromJoin.DatabaseName.Text, ucFromJoin.TableName.Text,
						string.IsNullOrEmpty(ucFromJoin.Alias.Text) ? "" : " as " + ucFromJoin.Alias.Text));
			}
			return sb.ToString();
		}
	}
}
