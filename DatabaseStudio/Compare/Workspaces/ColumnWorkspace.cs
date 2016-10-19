using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.DatabaseStudio.Compare.Workspaces
{
	public class ColumnWorkspace
	{
		public string SourceColumn { get; set; }

		private string _targetColumn;
		public string TargetColumn
		{
			get { return _targetColumn; }
			set
			{
				_targetColumn = value;
				if (string.IsNullOrEmpty(_targetColumn))
					SelectColumn = false;
			}
		}

		public bool SelectColumn { get; set; }

		public void OnColumnCreated()
		{
			_targetColumn = SourceColumn;
		}
	}
}
