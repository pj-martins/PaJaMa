using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture
{
	public partial class frmOverlay : Form
	{
		private int _lastX = -1;
		private int _lastY = -1;

		public int ScreenX { get; set; }
		public int ScreenY { get; set; }

		[DllImport("user32.dll")]
		static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetWindowRect(HandleRef hWnd, out RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left;        // x position of upper-left corner
			public int Top;         // y position of upper-left corner
			public int Right;       // x position of lower-right corner
			public int Bottom;      // y position of lower-right corner
		}

		public frmOverlay()
		{
			InitializeComponent();

		}

		private void frmOverlay_Load(object sender, EventArgs e)
		{
			Draw(Cursor.Position.X, Cursor.Position.Y, -1, -1, false);
		}

		public bool IsInControl(int x, int y)
		{
			if (x < this.Left || y < this.Top) return false;
			if (x > this.Left + this.Width) return false;
			if (y > this.Top + this.Height) return false;

			return true;
		}

		public void ClearGraphics()
		{
			if (pnlSelection.Visible)
			{
				var graphics = this.CreateGraphics();
				graphics.Clear(Color.Empty);
				pnlSelection.Visible = false;
			}
		}

		public void Draw(int mouseX, int mouseY, int? downX, int? downY, bool mouseDown)
		{
			var x = mouseX - ScreenX;
			var y = mouseY - ScreenY;
			if (x == _lastX && y == _lastY) return;

			draw(this, x, y);

			var lastLocation = pnlSelection.Location;
			if (downX != null && downY != null)
			{
				pnlSelection.Visible = true;
				pnlSelection.Location = new Point(Math.Min(x, downX.Value - ScreenX), Math.Min(y, downY.Value - ScreenY)); // new Point(Math.Min(x, downX - ScreenX), Math.Min(y, downY - ScreenY));
				pnlSelection.Size = new System.Drawing.Size(Math.Abs(downX.Value - mouseX), Math.Abs(downY.Value - mouseY));
			}
			else if (x > 0 && y > 0)
			{
				var window = WindowFromPoint(mouseX, mouseY);
				RECT rect;
				GetWindowRect(new HandleRef(pnlSelection, window), out rect);
				var left = rect.Left;
				var right = rect.Right;
				var top = rect.Top;
				var bottom = rect.Bottom;
				//if (left < 0 || top < 0)
				//{
				//	// TODO: need to check for more vertical monitors

				//	var subtract = left < 0 ? Math.Abs(left) : Math.Abs(top);
				//	left += subtract;
				//	right -= subtract;
				//	top += subtract;
				//	bottom -= subtract;
				//}
				pnlSelection.Location = new Point(left - ScreenX, top - ScreenY);
				pnlSelection.Size = new Size(right - left, bottom - top);
				pnlSelection.Visible = true;
			}

			if (pnlSelection.Visible)
			{
				if (lastLocation != pnlSelection.Location)
				{
					using (var graphics = pnlSelection.CreateGraphics())
						graphics.Clear(pnlSelection.BackColor);
				}
				draw(pnlSelection, x, y);
			}

			_lastX = x;
			_lastY = y;
		}

		public Image GetImage()
		{
			using (var pnlGraphics = pnlSelection.CreateGraphics())
			{
				pnlGraphics.Clear(pnlSelection.BackColor);
			}
			var bmp = new Bitmap(pnlSelection.Width, pnlSelection.Height);
			using (var graphics = Graphics.FromImage(bmp))
			{
				graphics.CopyFromScreen(new Point(pnlSelection.Left + this.Left, pnlSelection.Top + this.Top), new Point(0, 0), pnlSelection.Size);
			}
			return bmp;
		}

		private void draw(Control ctrl, int x, int y)
		{
			using (var graphics = ctrl.CreateGraphics())
			{
				int xstart = ctrl.Left - ((ctrl is Form) ? ScreenX : 0);
				int ystart = ctrl.Top - ((ctrl is Form) ? ScreenY : 0);

				int lineWidth = 3;
				int textDistance = 4;

				var resolutionText = pnlSelection.Width.ToString() + ", " + pnlSelection.Height.ToString();
				var resolutionFont = new Font(FontFamily.GenericSerif, 18, FontStyle.Bold);
				var resolutionTextSize = graphics.MeasureString(resolutionText, resolutionFont);

				if (_lastX != x || _lastY != y)
				{
					graphics.FillRectangle(new SolidBrush(ctrl.BackColor), _lastX - xstart, _lastY - ystart - resolutionTextSize.Height - lineWidth, 200, 100);

					if (_lastX != x)
						graphics.DrawLine(new Pen(new SolidBrush(ctrl.BackColor), lineWidth), new Point(_lastX - xstart, 0), new Point(_lastX - xstart, ctrl.Height));
					if (_lastY != y)
						graphics.DrawLine(new Pen(new SolidBrush(ctrl.BackColor), lineWidth), new Point(0, _lastY - ystart), new Point(ctrl.Width, _lastY - ystart));
				}

				graphics.DrawLine(new Pen(new SolidBrush(Color.Red), lineWidth), new Point(0, y - ystart), new Point(ctrl.Width, y - ystart));
				graphics.DrawLine(new Pen(new SolidBrush(Color.Red), lineWidth), new Point(x - xstart, 0), new Point(x - xstart, ctrl.Height));

				graphics.FillRectangle(new SolidBrush(Color.Black), x - xstart + (textDistance / 2), y - ystart + (textDistance / 2) - resolutionTextSize.Height - lineWidth, resolutionTextSize.Width, resolutionTextSize.Height);
				graphics.DrawString(resolutionText, resolutionFont, new SolidBrush(Color.Red), new Point(x - xstart + textDistance, y - ystart + textDistance - (int)resolutionTextSize.Height - lineWidth));
			}
		}
	}
}