using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	public class SelectablePictureBox : PictureBox
	{
		public SelectablePictureBox()
		{
			this.SetStyle(ControlStyles.Selectable, true);
			this.TabStop = true;
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			this.Focus();
			base.OnMouseDown(e);
		}
		protected override bool IsInputKey(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.Right:
				case Keys.Left:
				case Keys.Up:
				case Keys.Down:
					return true;
				case Keys.Shift | Keys.Right:
				case Keys.Shift | Keys.Left:
				case Keys.Shift | Keys.Up:
				case Keys.Shift | Keys.Down:
					return true;
			}
			return base.IsInputKey(keyData);
		}
		protected override void OnEnter(EventArgs e)
		{
			this.Invalidate();
			base.OnEnter(e);
		}
		protected override void OnLeave(EventArgs e)
		{
			this.Invalidate();
			base.OnLeave(e);
		}
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			if (this.Focused)
			{
				var rc = this.ClientRectangle;
				rc.Inflate(-2, -2);
				ControlPaint.DrawFocusRectangle(pe.Graphics, rc);
			}
		}
	}
}
