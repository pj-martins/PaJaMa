using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.DatabaseStudio.Classes
{
	public class GridHelper
	{
		private DataGridViewSelectedRowCollection _selectedRows;
		
		public void DecorateGrid(DataGridView grid)
		{
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grid.CurrentCellDirtyStateChanged += grid_CurrentCellDirtyStateChanged;
			grid.MouseUp += grid_MouseUp;
			grid.KeyUp += grid_KeyUp;
			grid.CellMouseDown += grid_CellMouseDown;
		}

		private void grid_MouseUp(object sender, MouseEventArgs e)
		{
			var grid = sender as DataGridView;
			_selectedRows = grid.SelectedRows;
		}

		private void grid_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || (e.Control && e.KeyCode == Keys.A))
			{
				var grid = sender as DataGridView;
				_selectedRows = grid.SelectedRows;
			}
		}

		private void grid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			var grid = sender as DataGridView;
			if (e.RowIndex < 0) return;

			if (!grid.Rows[e.RowIndex].Selected)
				_selectedRows = null;
		}
		

		private void grid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			var grid = sender as DataGridView;
			if (grid.IsCurrentCellDirty)
			{
				grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
				grid.CancelEdit();
			}

			if (grid.CurrentCell != null && _selectedRows != null && grid.CurrentCell.OwningColumn is DataGridViewCheckBoxColumn)
			{
				var selectedValue = (bool)grid.CurrentCell.Value;
				foreach (var row in grid.Rows.OfType<DataGridViewRow>())
				{
					if (_selectedRows.Contains(row))
					{
						if (row.Index == grid.CurrentCell.RowIndex)
							continue;

						row.Cells[grid.CurrentCell.OwningColumn.Name].Value = selectedValue;
						row.Selected = true;
					}
					else
						row.Selected = false;
				}
			}
			_selectedRows = null;
		}
	}
}
