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
	public partial class frmOverlayPictureBox : Form
	{
		private int _lastX = -1;
		private int _lastY = -1;

		private Graphics _graphics;

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

		public frmOverlayPictureBox()
		{
			InitializeComponent();

		}

		private void frmOverlay_Load(object sender, EventArgs e)
		{
			_graphics = this.CreateGraphics();
			_graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			Draw(Cursor.Position.X, Cursor.Position.Y, -1, -1);
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
			if (pictMain.Visible)
			{
				_graphics.Clear(Color.Empty);
				pictMain.Visible = false;
			}
		}

		public void Draw(int mouseX, int mouseY, int downX, int downY)
		{
			this.TopMost = true;

			var x = mouseX - ScreenX;
			var y = mouseY - ScreenY;
			var left = this.Left - ScreenX;
			var top = this.Top - ScreenY;

			if (x == _lastX && y == _lastY) return;

			_graphics.DrawLine(new Pen(new SolidBrush(Color.Yellow), 1), new Point(left, y), new Point(left + this.Width, y));
			_graphics.DrawLine(new Pen(new SolidBrush(Color.Yellow), 1), new Point(x, top), new Point(x, top + this.Height));

			if (_lastX != x)
				_graphics.DrawLine(new Pen(new SolidBrush(this.BackColor), 1), new Point(_lastX, top), new Point(_lastX, top + this.Height));
			if (_lastY != y)
				_graphics.DrawLine(new Pen(new SolidBrush(this.BackColor), 1), new Point(left, _lastY), new Point(left + this.Width, _lastY));

			bool drawImage = false;

			int pictLastX = pictMain.Location.X;
			int pictLastY = pictMain.Location.Y;
			int pictLastWidth = pictMain.Size.Width;
			int pictLastHeight = pictMain.Size.Height;

			if (downX >= 0 && downY >= 0)
			{
				pictMain.Visible = true;
				pictMain.Location = new Point(Math.Min(x, downX - ScreenX), Math.Min(y, downY - ScreenY));
				pictMain.Size = new System.Drawing.Size(Math.Abs(downX - mouseX), Math.Abs(downY - mouseY));
				drawImage = true;
			}
			else if (x > 0 && y > 0)
			{
				var window = WindowFromPoint(mouseX, mouseY);
				RECT rect;
				GetWindowRect(new HandleRef(pictMain, window), out rect);
				pictMain.Location = new Point(rect.Left - ScreenX, rect.Top - ScreenY);
				pictMain.Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
				pictMain.Visible = true;
				drawImage = true;
			}

			if (pictLastX == pictMain.Location.X
				&& pictLastY == pictMain.Location.Y
				&& pictLastWidth == pictMain.Size.Width
				&& pictLastHeight == pictMain.Size.Height)
				drawImage = false;

			if (drawImage)
			{
				if (pictMain.Image != null)
					pictMain.Image.Dispose();

				pictMain.Image = null;
				var bmp = new Bitmap(pictMain.Width, pictMain.Height);
				using (var graphics = Graphics.FromImage(bmp))
				{
					//this.Visible = false;
					graphics.CopyFromScreen(new Point(pictMain.Left + this.Left, pictMain.Top + this.Top), new Point(0, 0), pictMain.Size);
					//this.Visible = true;
				}
				pictMain.Image = bmp;
			}

			_lastX = x;
			_lastY = y;
		}
	}
}
