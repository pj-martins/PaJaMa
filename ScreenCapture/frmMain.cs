using PaJaMa.Common;
using PaJaMa.ScreenCapture.ImageEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture
{
	public partial class frmMain : Form
	{
		private UserSettings _userSettings;
		private DirectoryInfo _picturesDirectory { get; set; }
		//private Dictionary<string, Image> _images = new Dictionary<string, Image>();
		private int[] _lastSelectedIndices;

		private bool _lock = false;
		private bool _lockWatcher = false;

		private CaptureHelper _captureHelper;

		//private bool _drawingArrow = false;
		//private Point _mouseDownLocation;
		//private Graphics _currGraphics;

		private string userSettingsConfigFile
		{
			get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PJCapture", "UserSettings.xml"); }
		}

		public frmMain()
		{
			InitializeComponent();
		}

		private void lstMain_MouseWheel(object sender, MouseEventArgs e)
		{
			if (ModifierKeys == Keys.Control)
			{
				int increase = (int)Math.Ceiling((double)e.Delta / 3.75);
				var thumbSize = ilMain.ImageSize;
				var newSize = new Size(thumbSize.Width + increase, thumbSize.Height + increase);

				if (newSize.Width >= 16 && newSize.Height >= 16 && newSize.Width <= 256 && newSize.Height <= 256)
				{
					ilMain.ImageSize = newSize;
					refreshImages();
				}
				((HandledMouseEventArgs)e).Handled = true;
			}
		}

		private void _captureHelper_PictureCaptured(object sender, CaptureEventArgs e)
		{
			captureImage(e.Image);
		}

		private string captureImage(Image image)
		{
			_lockWatcher = true;
			string imagePath = Path.Combine(_picturesDirectory.FullName, DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".png");
			image.Save(imagePath, System.Drawing.Imaging.ImageFormat.Png);
			Clipboard.SetImage(image);

			var item = new ListViewItem(new FileInfo(imagePath).Name);
			item.Tag = imagePath;
			lstMain.Items.Insert(0, item);
			using (var memStream = new MemoryStream(File.ReadAllBytes(imagePath)))
			{
				using (var thumb = Imaging.ResizeImage(Image.FromStream(memStream),
					new Size(256, 256)))
				{
					ilMain.Images.Add(thumb);
				}
			}

			item.ImageIndex = ilMain.Images.Count - 1;

			_lock = true;
			foreach (ListViewItem item2 in lstMain.Items)
			{
				item2.Selected = false;
			}
			_lock = false;

			item.Selected = true;

			this.Visible = true;
			this.ShowInTaskbar = true;
			if (this.WindowState == FormWindowState.Minimized)
				this.WindowState = _userSettings.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
			Win32Api.SetForegroundWindow(this.Handle);

			_lockWatcher = false;
			
			return imagePath;
		}

		private void refreshList()
		{
			lstMain.Items.Clear();
			ilMain.Images.Clear();
			if (_picturesDirectory == null)
			{
				_picturesDirectory = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "PJCapture", "Images"));
				if (!_picturesDirectory.Exists)
					_picturesDirectory.Create();

				var watcher = new FileSystemWatcher(_picturesDirectory.FullName);
				watcher.Changed += watcher_Changed;
				watcher.Deleted += watcher_Deleted;
				watcher.EnableRaisingEvents = true;

			}

			foreach (var file in _picturesDirectory.GetFiles().OrderByDescending(fi => fi.Name))
			{
				var item = new ListViewItem(file.Name);
				item.Tag = file.FullName;
				lstMain.Items.Add(item);
				//using (var memStream = new MemoryStream(File.ReadAllBytes(file.FullName)))
				//{
				//	_images.Add(file.FullName, Imaging.ResizeImage(Image.FromStream(memStream),
				//		new Size(256, 256)));
				//}
			}

			refreshImages();
			if (lstMain.Items.Count > 0)
				lstMain.Items[0].Selected = true;
		}

		private void refreshImages()
		{
			foreach (ListViewItem item in lstMain.Items)
			{
				// var image = _images[item.Tag.ToString()];
				using (var image = Image.FromFile(item.Tag.ToString()))
				{
					using (var thumb = Imaging.ResizeImage(image, new Size(256, 256)))
					{
						ilMain.Images.Add(thumb);
					}
				}
				item.ImageIndex = ilMain.Images.Count - 1;
			}
		}

		private void frmMain_Load(object sender, EventArgs e)
		{

			_lock = true;
			if (File.Exists(userSettingsConfigFile))
			{
				try
				{
					_userSettings = XmlSerialize.DeserializeObjectFromFile<UserSettings>(userSettingsConfigFile);
					this.Height = _userSettings.FormHeight;
					this.Width = _userSettings.FormWidth;
					if (_userSettings.Maximized)
						this.WindowState = FormWindowState.Maximized;
					splitMain.SplitterDistance = _userSettings.SplitterDistance;
					ilMain.ImageSize = new Size(_userSettings.ThumbnailSize, _userSettings.ThumbnailSize);

					if (_userSettings.DrawColorArgb != 0)
						ucImageEditor.DrawColor = Color.FromArgb(_userSettings.DrawColorArgb);
					else
						ucImageEditor.DrawColor = Color.Blue;

					if (_userSettings.DrawWidth > 0)
						ucImageEditor.DrawWidth = _userSettings.DrawWidth;
					else
						ucImageEditor.DrawWidth = 1;

					if (_userSettings.EraseColorArgb != 0)
						ucImageEditor.EraseColor = Color.FromArgb(_userSettings.EraseColorArgb);
					else
						ucImageEditor.EraseColor = Color.White;

					if (_userSettings.EraseWidth > 0)
						ucImageEditor.EraseWidth = _userSettings.EraseWidth;
					else
						ucImageEditor.EraseWidth = 10;

					if (_userSettings.HighlightColorArgb != 0)
						ucImageEditor.HighlightColor = Color.FromArgb(_userSettings.HighlightColorArgb);
					else
						ucImageEditor.HighlightColor = Color.FromArgb(100, Color.Yellow);

					if (_userSettings.HighlightWidth > 0)
						ucImageEditor.HighlightWidth = _userSettings.HighlightWidth;
					else
						ucImageEditor.HighlightWidth = 10;

					if (!string.IsNullOrEmpty(_userSettings.Font))
						ucImageEditor.CurrentFont = (Font)new FontConverter().ConvertFromString(_userSettings.Font);
					else
						ucImageEditor.CurrentFont = new Font(FontFamily.GenericSansSerif, 12);

					if (_userSettings.Radius > 0)
						ucImageEditor.CurrentRadius = _userSettings.Radius;
					else
						ucImageEditor.CurrentRadius = 8;
				}
				catch
				{
					_userSettings = new UserSettings();
				}

			}
			else
			{
				_userSettings = new UserSettings();
				ucImageEditor.CurrentColor = Color.Blue;
				ucImageEditor.CurrentWidth = 1;
				ucImageEditor.EraseColor = Color.White;
				ucImageEditor.EraseWidth = 10;
				ucImageEditor.CurrentFont = new Font(FontFamily.GenericSansSerif, 12);
			}

			lstMain.MouseWheel += lstMain_MouseWheel;

			int i = 1;
			foreach (var screen in Screen.AllScreens)
			{
				var mnu = new ToolStripMenuItem(screen.DeviceName + " - " + (i++).ToString(), null, captureScreenMenuItem_Click) { Tag = screen };
				mnuNotification.Items.Insert(mnuNotification.Items.IndexOf(exitToolStripMenuItem), mnu);
			}

			_lock = false;

			refreshList();
			_captureHelper = new CaptureHelper();
			_captureHelper.PictureCaptured += _captureHelper_PictureCaptured;
			_captureHelper.KeyPress += captureHelper_KeyPress;

			openWithToolStripMenuItem.DropDownItems.Clear();
			var openWith = Registry.GetOpenWithList(".png");
			foreach (var kvp in openWith)
			{
				openWithToolStripMenuItem.DropDownItems.Add(kvp.Value, null, openWithToolClick).Tag = kvp.Key;
			}
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.Visible)
			{
				_userSettings.FormHeight = this.Height;
				_userSettings.FormWidth = this.Width;
				_userSettings.Maximized = this.WindowState == FormWindowState.Maximized;
				_userSettings.ThumbnailSize = ilMain.ImageSize.Width;
				_userSettings.SplitterDistance = splitMain.SplitterDistance;
				_userSettings.DrawColorArgb = ucImageEditor.DrawColor.ToArgb();
				_userSettings.DrawWidth = ucImageEditor.DrawWidth;
				_userSettings.HighlightColorArgb = ucImageEditor.HighlightColor.ToArgb();
				_userSettings.HighlightWidth = ucImageEditor.HighlightWidth;
				_userSettings.EraseColorArgb = ucImageEditor.EraseColor.ToArgb();
				_userSettings.EraseWidth = ucImageEditor.EraseWidth;
				_userSettings.Font = new FontConverter().ConvertToString(ucImageEditor.CurrentFont);
				_userSettings.Radius = ucImageEditor.CurrentRadius;
				XmlSerialize.SerializeObjectToFile<UserSettings>(_userSettings, userSettingsConfigFile);
			}

			if (_lock) return;

			this.WindowState = FormWindowState.Minimized;
			this.Visible = false;
			this.ShowInTaskbar = false;
			e.Cancel = true;
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			// ShowInTaskbar = true; // WindowState != FormWindowState.Minimized;
		}

		private void captureHelper_KeyPress(object sender, KeyPressEventArgs e)
		{
			ucImageEditor.ProcessKey(e);
		}

		private void openWithToolClick(object sender, EventArgs e)
		{
			var toolstripItem = sender as ToolStripItem;
			var exe = toolstripItem.Tag.ToString();
			openImages(exe);
		}

		private void lstMain_DoubleClick(object sender, EventArgs e)
		{
			openImages();
		}

		private void openImages(string exe = null)
		{
			//var selectedItems = new List<ImageListViewItem>();
			//foreach (ImageListViewItem img in lstMain.SelectedItems)
			//{
			//	selectedItems.Add(img);
			//}

			//if (selectedItems.Count < 1 && lstMain.View == Manina.Windows.Forms.View.Gallery
			//	&& lstMain.LastDrawnGalleryItem != null)
			//	selectedItems.Add(lstMain.LastDrawnGalleryItem);

			//if (selectedItems.Count < 1)
			//	return;

			//foreach (ImageListViewItem img in selectedItems)
			foreach (ListViewItem item in lstMain.SelectedItems)
			{
				openImage(item.Tag.ToString(), exe);
			}
		}

		private void openImage(string fileName, string exe = null)
		{
			// ucImageEditor.PromptSave();

			if (string.IsNullOrEmpty(exe))
				exe = "mspaint.exe";

			if (exe.Contains(" \"%1\""))
				exe = exe.Replace(" \"%1\"", string.Empty);
			Process.Start(exe, fileName);
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ucImageEditor.CopyImage(true);
		}

		private void showInExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var img = lstMain.SelectedItems[0];

			//if (img == null && lstMain.View == Manina.Windows.Forms.View.Gallery)
			//	img = lstMain.LastDrawnGalleryItem;

			if (img == null)
				return;

			Process.Start("explorer.exe", "/select, " + img.Tag.ToString());
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (ucImageEditor.CurrentDrawObject != null)
			{
				ucImageEditor.DeleteCurrentDrawObject();
				return;
			}

			if (MessageBox.Show("Are you sure you want to delete selected images?", "Delete", MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
				return;

			var faileds = new List<string>();
			var items = lstMain.SelectedItems.OfType<ListViewItem>().ToList();
			foreach (var item in items)
			{
				string file = item.Tag.ToString();
				try
				{
					File.Delete(file);

					string doFile = Path.Combine(Path.GetDirectoryName(file), "drawObjects", Path.GetFileName(file) + ".xml");
					if (File.Exists(doFile))
						File.Delete(doFile);

				}
				catch (Exception ex)
				{
					faileds.Add("Failed to delete " + file + ": " + ex.Message);
					continue;
				}

				lstMain.Items.Remove(item);
			}

			lstMain_SelectedIndexChanged(sender, e);

			if (faileds.Any())
			{
				MessageBox.Show(string.Join("\r\n", faileds.ToArray()));
			}
		}

		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (lstMain.SelectedItems.Count < 1) e.Cancel = true;
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			lstMain_DoubleClick(sender, e);
		}

		private void lstMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				deleteToolStripMenuItem_Click(sender, e);
			}
			else if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
			{
				foreach (ListViewItem item in lstMain.Items)
				{
					item.Selected = true;
				}
			}
			else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
			{
				copyToolStripMenuItem_Click(sender, e);
			}
		}

		private void lstMain_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (_lock) return;

			//var cancelEventArgs = new CancelEventArgs();
			//ucImageEditor.PromptClose(cancelEventArgs);
			//if (cancelEventArgs.Cancel)
			//{
			//	_lock = true;
			//	//if (lstMain.SelectedItems.Count > 0)
			//	//	lstMain.SelectedItems[0].Selected = false;
			//	//lstMain.Items.OfType<ListViewItem>().First(i => i.Tag.ToString() == ucImageEditor.FileName).Selected = true;
			//	//lstMain.SelectedIndices.Clear();
			//	//foreach (var ind in _lastSelectedIndices)
			//	//{
			//	//	lstMain.SelectedIndices.Add(ind);
			//	//}
			//	_lock = false;
			//	return;
			//}

			_lastSelectedIndices = lstMain.SelectedIndices.OfType<int>().ToArray();
			if (lstMain.SelectedItems.Count > 0)
			{
				ucImageEditor.FileName = lstMain.SelectedItems[0].Tag.ToString();
				ucImageEditor.LoadImage();
			}
			else
			{
				ucImageEditor.FileName = string.Empty;
				ucImageEditor.LoadImage();
			}
			_lock = false;
		}

		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			this.ShowInTaskbar = true;
			this.Visible = true;
			this.WindowState = _userSettings.Maximized ? FormWindowState.Maximized : FormWindowState.Normal;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			exit();
		}

		private void exit()
		{
			_captureHelper.Dispose();
			_lock = true;
			this.Close();
		}

		private void captureToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_captureHelper.StartCapture();
		}

		private void captureFullScreenToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//int xStop = 0;
			//int yStop = 0;
			//foreach (var screen in Screen.AllScreens)
			//{
			//	if (screen.Bounds.Right > xStop)
			//		xStop = screen.Bounds.Right;

			//	if (screen.Bounds.Bottom > yStop)
			//		yStop = screen.Bounds.Bottom;
			//}

			//var bmp = new Bitmap(xStop, yStop);
			//using (var graphics = Graphics.FromImage(bmp))
			//{
			//	graphics.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(xStop, yStop));
			//}

			//captureImage(bmp);
			_captureHelper.CaptureFullScreen();
		}

		private void captureScreenMenuItem_Click(object sender, EventArgs e)
		{
			var screen = (sender as ToolStripMenuItem).Tag as Screen;
			_captureHelper.CaptureScreen(screen);
		}

		private void watcher_Deleted(object sender, FileSystemEventArgs e)
		{
			this.Invoke(new Action(() =>
			{
				var item = lstMain.Items.OfType<ListViewItem>().FirstOrDefault(i => i.Tag.ToString() == e.FullPath);
				if (item == null) return;
				lstMain.Items.Remove(item);
			}));
		}

		private void watcher_Changed(object sender, FileSystemEventArgs e)
		{
			if (_lockWatcher) 
				return;
			this.Invoke(new Action(() =>
			{
				if (!e.FullPath.EndsWith(".png")) return;

				var item = lstMain.Items.OfType<ListViewItem>().First(i => i.Tag.ToString() == e.FullPath);

				ilMain.Images[item.ImageIndex].Dispose();

				using (var memStream = new MemoryStream(File.ReadAllBytes(e.FullPath)))
				{
					using (var img = Imaging.ResizeImage(Image.FromStream(memStream),
						new Size(256, 256)))
					{
						ilMain.Images[item.ImageIndex] = img;
					}
				}

				refreshImages();

				if (item.Selected)
					ucImageEditor.LoadImage();
			}));
		}

		private void ucImageEditor_ImageDoubleClick(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(ucImageEditor.FileName)) return;

			openImage(ucImageEditor.FileName);
		}


	}
}
