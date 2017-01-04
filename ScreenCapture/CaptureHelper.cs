using Gma.UserActivityMonitor;
using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture
{
	public class CaptureHelper : IDisposable
	{
		private List<frmOverlay> _overlays = new List<frmOverlay>();

		private int? _downX = null;
		private int? _downY = null;

		private GlobalHooks _hooks;

		public event CaptureEventHandler PictureCaptured;

		public KeyPressEventHandler KeyPress;

		public CaptureHelper()
		{
			_hooks = new GlobalHooks();
			_hooks.KeyDown += _hooks_KeyDown;
			_hooks.KeyPress += _hooks_KeyPress;
		}

		private void _hooks_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (KeyPress != null)
			{
				KeyPress(this, e);
			}
		}

		private void _hooks_MouseMove(object sender, MouseEventArgs e)
		{
			if (_overlays.Any())
			{
				bool mouseDown = _downX >= 0 && _downY >= 0;
				foreach (var overlay in _overlays)
				{
					if (overlay.IsInControl(e.X, e.Y) || mouseDown)
					{
						overlay.Draw(e.X, e.Y, _downX, _downY, mouseDown);
					}
					else
						overlay.ClearGraphics();
				}
			}
		}

		private void _hooks_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.PrintScreen)
			{
				if (e.Control)
				{
					var activeScreen = Screen.FromPoint(Cursor.Position);
					CaptureScreen(activeScreen);
				}
				else
					StartCapture();
			}
			else if (e.KeyCode == Keys.Escape && _overlays.Any())
			{
				closeAll();

				_downX = null;
				_downY = null;
			}
		}

		private void _hooks_LeftMouseUp(object sender, MouseEventArgs e)
		{
			if (_overlays.Any())
			{
				foreach (var overlay in _overlays)
				{
					if (overlay.IsInControl(e.X, e.Y))
					{
						using (var image = overlay.GetImage())
						{
							PictureCaptured(this, new CaptureEventArgs() { Image = image });
							break;
						}
					}
				}

				closeAll();
			}

			_downX = null;
			_downY = null;
		}

		private void _hooks_LeftMouseDown(object sender, MouseEventArgs e)
		{
			_downX = e.X;
			_downY = e.Y;
			var args = (CancelMouseEventArgs)e;
			args.Cancel = true;
		}

		private void closeAll()
		{
			foreach (var overlay in _overlays)
			{
				overlay.Close();
				overlay.Dispose();
			}

			_overlays.Clear();
			_hooks.MouseMove -= _hooks_MouseMove;
			_hooks.LeftMouseDown -= _hooks_LeftMouseDown;
			_hooks.LeftMouseUp -= _hooks_LeftMouseUp;
			_hooks.DeatchMouseHook();

			Cursor.Show();
		}

		public void StartCapture()
		{
			Cursor.Hide();
			_hooks.AttachMouseHook();
			_hooks.MouseMove += _hooks_MouseMove;
			_hooks.LeftMouseDown += _hooks_LeftMouseDown;
			_hooks.LeftMouseUp += _hooks_LeftMouseUp;

			foreach (var screen in Screen.AllScreens)
			{
				var overlay = new frmOverlay();
				overlay.SetBounds(screen.Bounds.X, screen.Bounds.Y, screen.Bounds.Width, screen.Bounds.Height);
				overlay.ScreenX = screen.Bounds.X;
				overlay.ScreenY = screen.Bounds.Y;
				overlay.StartPosition = FormStartPosition.Manual;
				overlay.Show();
				_overlays.Add(overlay);
			}
		}

		public void CaptureFullScreen()
		{
			int xStop = 0;
			int yStop = 0;
			foreach (var screen in Screen.AllScreens)
			{
				if (screen.Bounds.Right > xStop)
					xStop = screen.Bounds.Right;

				if (screen.Bounds.Bottom > yStop)
					yStop = screen.Bounds.Bottom;
			}

			using (var bmp = new Bitmap(xStop, yStop))
			{
				using (var graphics = Graphics.FromImage(bmp))
				{
					graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(xStop, yStop));
				}

				PictureCaptured(this, new CaptureEventArgs() { Image = bmp });
			}
		}

		public void CaptureScreen(Screen screen)
		{
			using (var bmp = new Bitmap(screen.Bounds.Width, screen.Bounds.Height))
			{
				using (var graphics = Graphics.FromImage(bmp))
				{
					graphics.CopyFromScreen(screen.Bounds.Location, new Point(0, 0), screen.Bounds.Size);
				}

				PictureCaptured(this, new CaptureEventArgs() { Image = bmp });
			}
		}

		public void Dispose()
		{
			_hooks.Dispose();
			_hooks = null;
		}
	}

	public delegate void CaptureEventHandler(object sender, CaptureEventArgs e);
	public class CaptureEventArgs : EventArgs
	{
		public Image Image { get; set; }
	}
}
