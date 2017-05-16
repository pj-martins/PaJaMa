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
	public partial class ucFromJoin : UserControl
	{
		private QueryBuilderHelper _queryBuilderHelper;
		private Dictionary<string, List<string>> _tablesColumns;

		public bool IsFrom
		{
			get { return lblFromJoin.Text == "From"; }
			set { lblFromJoin.Text = value ? "From" : "Join"; }
		}

		public ucFromJoin(QueryBuilderHelper queryBuilderHelper)
		{
			InitializeComponent();
			this.IsFrom = true;
			_queryBuilderHelper = queryBuilderHelper;
		}

		private void ucFromJoin_Load(object sender, EventArgs e)
		{
			DatabaseName.Items.Clear();
			DatabaseName.Items.AddRange(_queryBuilderHelper.GetDatabases().ToArray());
			DatabaseName.Text = _queryBuilderHelper.Connection.Database;
		}

		private void cboDatabase_SelectedIndexChanged(object sender, EventArgs e)
		{
			TableName.Items.Clear();
			_tablesColumns = _queryBuilderHelper.GetTablesColumns(DatabaseName.Text);
			TableName.Items.AddRange(_tablesColumns.Select(t => t.Key).ToArray());
		}

		private void btnAddColumn_Click(object sender, EventArgs e)
		{
			var ucCol = new ucColumn(_tablesColumns.ContainsKey(TableName.Text) ? _tablesColumns[TableName.Text] : null);
			ucCol.RemoveButton.Click += delegate (object sender2, EventArgs e2)
			{
				this.Height -= ucCol.Height;
				this.Controls.Remove(ucCol);
			};
			this.Height += ucCol.Height;
			ucCol.Dock = DockStyle.Bottom;
			this.Controls.Add(ucCol);
		}

		private void cboTable_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_tablesColumns.ContainsKey(TableName.Text))
			{
				foreach (var uc in this.Controls.OfType<ucColumn>())
				{
					uc.SetColumns(_tablesColumns[TableName.Text]);
				}
			}
		}
	}
}
