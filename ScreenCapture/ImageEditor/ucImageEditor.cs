using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using PaJaMa.Common;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	public partial class ucImageEditor : UserControl
	{
		private Type _drawObjectType;
		private Point _mouseDownLocation = Point.Empty;
		private Rectangle _imageRectangle;
		private Rectangle _cropRectangle;
		private KeyValuePair<Rectangle, Cursor> _resizeRectangle;
		private Graphics _currGraphics;
		private List<DrawObject> _drawObjects;
		private Bitmap _bmp;
		private Bitmap _originalImage;
		private Size _originalImageSize;
		private MemoryStream _imageMemoryStream;
		private bool _lock;
		private float _zoomMultiplier = 1;

		private Stack<UndoAction> _undos = new Stack<UndoAction>();
		private Stack<UndoAction> _redos = new Stack<UndoAction>();
		private List<CheckBox> _tools = new List<CheckBox>();
		private CheckBox _currentTool;

		public DrawObject CurrentDrawObject { get; private set; }

		public Color DrawColor { get; set; }
		public Color EraseColor { get; set; }
		public Color HighlightColor { get; set; }

		private Color _color;
		public Color CurrentColor
		{
			get { return _color; }
			set
			{
				_color = value;
				pnlCurrentColor.BackColor = value;
				numAlpha.Value = value.A;
				if (CurrentDrawObject != null)
				{
					addUndoAction(UndoActionType.StyleChange);
					CurrentDrawObject.UpdateColor(value);
					serializeDrawObjects();
					redraw(true);
				}
			}
		}

		public float DrawWidth { get; set; }
		public float EraseWidth { get; set; }
		public float HighlightWidth { get; set; }

		private float _currentLineWidth;
		public float CurrentWidth
		{
			get { return _currentLineWidth; }
			set
			{
				_currentLineWidth = value;
				numWidth.Value = (decimal)value;
				if (CurrentDrawObject != null)
				{
					addUndoAction(UndoActionType.StyleChange);
					CurrentDrawObject.UpdateWidth(value);
					serializeDrawObjects();
					redraw(true);
				}
			}
		}

		private int _currentRadius;
		public int CurrentRadius
		{
			get { return _currentRadius; }
			set
			{
				_currentRadius = value;
				numRadius.Value = (decimal)value;
				if (CurrentDrawObject is IRadiusDrawObject)
				{
					(CurrentDrawObject as IRadiusDrawObject).Radius = (int)numRadius.Value;
					serializeDrawObjects();
					redraw(true);
				}
			}
		}

		private Font _currentFont;
		public Font CurrentFont
		{
			get { return _currentFont; }
			set
			{
				_currentFont = value;
				if (CurrentDrawObject != null && (CurrentDrawObject is ITextDrawObject))
				{
					addUndoAction(UndoActionType.StyleChange);
					(CurrentDrawObject as ITextDrawObject).Font = value;
					serializeDrawObjects();
					redraw(true);
				}
			}
		}

		public event EventHandler ImageDoubleClick;

		public string FileName { get; set; }

		public ucImageEditor()
		{
			InitializeComponent();
			populateTools();
			CurrentColor = Color.Blue;
			CurrentWidth = 3;
			CurrentRadius = 8;
			CurrentFont = new Font(FontFamily.GenericSansSerif, 12);
            numAlpha.Value = 255;
			pictMain.KeyDown += pictMain_KeyDown;
			pictMain.MouseWheel += pictMain_MouseWheel;
		}

		private void ucImageEditor_Load(object sender, EventArgs e)
		{
			LoadImage();

			if (this.ParentForm != null)
				this.ParentForm.FormClosing += ParentForm_FormClosing;

			pnlCurrentColor.Visible = true;
		}

		private void ParentForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// PromptClose(e);
		}

		private void populateTools()
		{
			var toolTypes = this.GetType().Assembly.GetTypes()
				.Where(t => t.IsSubclassOf(typeof(DrawObject)) && !t.IsAbstract)
				.OrderBy(t => ((DrawObjectAttribute)(t.GetCustomAttributes(typeof(DrawObjectAttribute), false).First())).Sequence);
			foreach (var tt in toolTypes.Reverse())
			{
				var chk = new CheckBox();
				chk.Dock = DockStyle.Top;
				chk.Appearance = Appearance.Button;
				chk.Tag = tt;
				chk.Image = (Image)Properties.Resources.ResourceManager.GetObject(tt.Name.Replace("DrawObject", "").ToLower());
				chk.CheckedChanged += chkTool_CheckChanged;
				pnlTools.Controls.Add(chk);
				chk.SendToBack();
				_tools.Add(chk);
			}
		}

		private void addUndoAction(UndoActionType type)
		{
			if (CurrentDrawObject == null) return;
			_undos.Push(new UndoAction()
			{
				UndoActionType = type,
				DrawObject = CurrentDrawObject.GetSerializable()
			});
		}

		private void serializeDrawObjects()
		{
			if (string.IsNullOrEmpty(FileName)) return;

			string path = Path.Combine(Path.GetDirectoryName(FileName), "drawObjects");
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			File.WriteAllText(Path.Combine(path, Path.GetFileName(FileName) + ".xml"),
				SerializableDrawObject.GetXMLForDrawObjects(_drawObjects));
		}

		private void deserializeDrawObjects()
		{
			_drawObjects = new List<DrawObject>();

			if (string.IsNullOrEmpty(FileName)) return;

			string path = Path.Combine(Path.GetDirectoryName(FileName), "drawObjects");
			if (!Directory.Exists(path))
				return;

			string fileName = Path.Combine(path, Path.GetFileName(FileName) + ".xml");
			if (!File.Exists(fileName))
				return;

			_drawObjects = SerializableDrawObject.GetDrawObjectsFromXML(_currGraphics, File.ReadAllText(fileName), _zoomMultiplier);

			if (_drawObjects == null)
				_drawObjects = new List<DrawObject>();

			if (_drawObjects.Any())
				redraw(true);
		}

		//public void PromptSave()
		//{
		//	if (_drawObjects != null && _drawObjects.Any())
		//	{
		//		var dlgResult = MessageBox.Show("Would you like to save changes?", "Save Changes?",
		//			MessageBoxButtons.YesNoCancel);
		//		switch (dlgResult)
		//		{
		//			case DialogResult.Cancel:
		//				e.Cancel = true;
		//				break;
		//			case DialogResult.No:
		//				break;
		//			case DialogResult.Yes:
		//				btnSave_Click(this, e);
		//				break;
		//		}
		//	}
		//}

		public void LoadImage(bool retain = false)
		{
			if (string.IsNullOrEmpty(FileName))
			{
				_drawObjects = new List<DrawObject>();
				if (_originalImage != null)
					_originalImage.Dispose();
				_originalImage = null;
				if (_bmp != null)
					_bmp.Dispose();
				_bmp = null;
				pictMain.Image = null;
				return;
			}

			if (!retain)
			{
				_drawObjects = new List<DrawObject>();
				if (_imageMemoryStream != null)
				{
					_imageMemoryStream.Dispose();
					_imageMemoryStream = null;
				}
				_cropRectangle = Rectangle.Empty;
			}

			if (_imageMemoryStream == null)
				_imageMemoryStream = new MemoryStream(File.ReadAllBytes(FileName));

			Size currSize = _bmp == null ? Size.Empty : _bmp.Size;

			_originalImage = new Bitmap(_imageMemoryStream);
			_bmp = new Bitmap(_imageMemoryStream);

			_originalImageSize = _originalImage.Size;

			if (_cropRectangle != Rectangle.Empty)
			{
				_originalImage = _originalImage.CropImage(_cropRectangle);
				_bmp = _bmp.CropImage(_cropRectangle);
			}

			if (currSize == Size.Empty) currSize = _bmp.Size;
			Size newSize = currSize;

			if (!retain)
			{
				_zoomMultiplier = 1;
				if (_originalImage.Width > pnlPicture.Width)
				{
					_zoomMultiplier = (float)pnlPicture.Width / (float)_originalImage.Width;

					// allow some padding
					_zoomMultiplier -= .01F;

					newSize = new Size((int)(_originalImage.Width * _zoomMultiplier), (int)(_originalImage.Height * _zoomMultiplier));
					_originalImage = new Bitmap(Common.Imaging.ResizeImage(_originalImage, newSize));
					_bmp = new Bitmap(Common.Imaging.ResizeImage(_originalImage, newSize));
				}
				if (_originalImage.Height > pnlPicture.Height)
				{
					_zoomMultiplier = (float)pnlPicture.Height / (float)_originalImage.Height;

					// allow some padding
					_zoomMultiplier -= .01F;

					newSize = new Size((int)(_originalImage.Width * _zoomMultiplier), (int)(_originalImage.Height * _zoomMultiplier));
					_originalImage = new Bitmap(Common.Imaging.ResizeImage(_originalImage, newSize));
					_bmp = new Bitmap(Common.Imaging.ResizeImage(_originalImage, newSize));
				}
			}
			else
			{
				newSize = new Size((int)(_originalImage.Width * _zoomMultiplier), (int)(_originalImage.Height * _zoomMultiplier));
				_originalImage = new Bitmap(Common.Imaging.ResizeImage(_originalImage, newSize));
				_bmp = new Bitmap(Common.Imaging.ResizeImage(_originalImage, newSize));
			}

			bool locked = _lock;
			_lock = true;
			numZoom.Value = (int)(_zoomMultiplier * 100);
			_lock = locked;

			if (_originalImage.Width > pnlPicture.Width || _originalImage.Height > pnlPicture.Height)
			{
				pictMain.Size = _originalImage.Size;
				if (pictMain.Width < pnlPicture.Width)
					pictMain.Width = pnlPicture.Width;
				if (pictMain.Height < pnlPicture.Height)
					pictMain.Height = pnlPicture.Height;
			}
			else
				pictMain.Size = pnlPicture.Size;

			_currGraphics = Graphics.FromImage(_bmp);

			deserializeDrawObjects();

			//foreach (var obj in _drawObjects)
			//{
			//	obj.UpdatePosition(_currGraphics, _zoomMultiplier);
			//}

			pictMain.Image = _bmp;
			updateImagePosition();

			if (_drawObjects.Any())
				redraw(true);
		}

		private void updateImagePosition()
		{
			if (pictMain.Image == null) return;
			var xDiff = pictMain.Width - pictMain.Image.Width;
			if (xDiff < 0)
				xDiff = 0;

			var x = xDiff / 2;

			var yDiff = pictMain.Height - pictMain.Image.Height;
			if (yDiff < 0)
				yDiff = 0;

			var y = yDiff / 2;

			_imageRectangle = new Rectangle(x, y, pictMain.Image.Width, pictMain.Image.Height);
		}

		private void redraw(bool refresh, bool ignoreSelection = false)
		{
			_currGraphics.Clear(Color.Empty);
			if (_originalImage == null) return;

			drawObjects(null);

			if (CurrentDrawObject is SelectDrawObject && !ignoreSelection)
				CurrentDrawObject.Draw();

			if (refresh)
				pictMain.Refresh();
		}

		private void drawObjects(FillDrawObject withNew = null)
		{
			_currGraphics.DrawImage(_originalImage, 0, 0);

			// draw fills first
			foreach (var o in _drawObjects.OfType<FillDrawObject>())
			{
				if (o is FillDrawObject)
					(o as FillDrawObject).CurrentImage = _bmp;

				o.Draw();
			}

			if (withNew != null)
			{
				withNew.CurrentImage = _bmp;
				withNew.Draw();
			}

			foreach (var o in _drawObjects)
			{
				if (o is SelectDrawObject || o is FillDrawObject)
					continue;

				if (o is IRadiusDrawObject)
					(o as IRadiusDrawObject).CurrentImage = _bmp;

				o.Draw();
			}
		}

		private void moveDeselected(Point newPoint, MouseEventArgs e)
		{
			var cursor = Cursors.Default;
			foreach (var dobj in _drawObjects)
			{
				if (dobj.IsInDrawObject(newPoint))
					cursor = Cursors.Hand;
			}

			if (CurrentDrawObject != null)
			{
				if (_mouseDownLocation != Point.Empty)
				{
					if (_resizeRectangle.Key != Rectangle.Empty)
					{
						cursor = _resizeRectangle.Value;
						CurrentDrawObject.ResizeTo(newPoint);
					}
					else
						CurrentDrawObject.MoveTo(_mouseDownLocation, e.Location);
					_mouseDownLocation = e.Location;
					redraw(true);
				}
				else if ((_resizeRectangle = CurrentDrawObject.GetResizeRectangle(newPoint)).Key
					!= Rectangle.Empty)
				{
					cursor = _resizeRectangle.Value;
				}
				else if (CurrentDrawObject.IsInDrawObject(newPoint))
				{
					cursor = Cursors.Hand;
				}
			}

			this.Cursor = cursor;
		}

		private void pictMain_MouseMove(object sender, MouseEventArgs e)
		{
			Point relativePoint = new Point(e.Location.X - _imageRectangle.X, e.Location.Y - _imageRectangle.Y);
			if (_drawObjectType == null)
			{
				moveDeselected(relativePoint, e);
				return;
			}

			_resizeRectangle = default(KeyValuePair<Rectangle, Cursor>);

			if (_imageRectangle.Contains(e.Location))
			{
				//if (_drawObjectType.Equals(typeof(FillDrawObject)))
				//	this.Cursor = Cursors.
				//else
				this.Cursor = Cursors.Cross;
				if (!_drawObjectType.Equals(typeof(TextDrawObject)))
				{
					if (_mouseDownLocation != Point.Empty)
					{
						redraw(false);
						CurrentDrawObject.Draw(new Point((int)(relativePoint.X / _zoomMultiplier),
							(int)(relativePoint.Y / _zoomMultiplier)));
						pictMain.Refresh();
					}
				}
			}
			else
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void deselect(bool colorWidth)
		{
			if (CurrentDrawObject != null)
			{
				CurrentDrawObject.Selected = false;
				redraw(true);
				CurrentDrawObject = null;
				setVisibilities();
				btnDelete.Visible = false;

                if (colorWidth)
                    updateColorWidth(false);
			}
		}

		private void pictMain_MouseDown(object sender, MouseEventArgs e)
		{
			endText();

			var previouslySelected = CurrentDrawObject;
			deselect(true);
			if (_drawObjectType == null)
			{
				for (int i = _drawObjects.Count - 1; i >= 0; i--)
				{
					var dobj = _drawObjects[i];
					if (dobj.IsInDrawObject(new Point(e.Location.X - _imageRectangle.X,
						e.Location.Y - _imageRectangle.Y)) ||
						(previouslySelected != null && previouslySelected.Equals(dobj) && _resizeRectangle.Key != Rectangle.Empty))
					{
						CurrentDrawObject = dobj;
						CurrentDrawObject.Selected = true;
						_lock = true;
						chkErase.Checked = CurrentDrawObject.Erase;
						chkHighlight.Checked = CurrentDrawObject.Highlight;
						_color = CurrentDrawObject.GetColor();
						_currentLineWidth = CurrentDrawObject.GetWidth();
						updateColorWidth(true);
						_lock = false;
						setVisibilities();

						addUndoAction(UndoActionType.MoveResize);
						if (_resizeRectangle.Key != Rectangle.Empty)
							CurrentDrawObject.StartResize(_resizeRectangle.Key);


						btnDelete.Visible = true;

						redraw(true);
						_mouseDownLocation = e.Location;
						break;
					}
				}
			}

			if (previouslySelected != null && previouslySelected is SelectDrawObject && _cropRectangle == Rectangle.Empty)
			{
				removeCurrentDrawObject(previouslySelected);
				redraw(true);
			}

			if (_drawObjectType == null) return;

			if (_imageRectangle.Contains(e.Location))
			{
				_mouseDownLocation = e.Location;
				CurrentDrawObject = DrawObject.CreateDrawObject(_drawObjectType,
					!_drawObjects.Any() ? 1 : _drawObjects.Max(d => d.ID) + 1,
					_currGraphics, new Pen(CurrentColor, CurrentWidth),
						new Point((int)((_mouseDownLocation.X - _imageRectangle.X) / _zoomMultiplier),
							(int)((_mouseDownLocation.Y - _imageRectangle.Y) / _zoomMultiplier)),
						_zoomMultiplier);

				CurrentDrawObject.Erase = chkErase.Checked;
				CurrentDrawObject.Highlight = chkHighlight.Checked;

				if (CurrentDrawObject is IRadiusDrawObject)
				{
					(CurrentDrawObject as IRadiusDrawObject).CurrentImage = _bmp;
					(CurrentDrawObject as IRadiusDrawObject).Radius = CurrentRadius;
				}

				if (_drawObjectType.GetInterface(typeof(ITextDrawObject).Name) != null)
				{
					(CurrentDrawObject as ITextDrawObject).Font = CurrentFont;
				}

				if (CurrentDrawObject is NumberDrawObject)
				{
					(CurrentDrawObject as NumberDrawObject).CurrentText = _drawObjects.Count(d => d is NumberDrawObject).ToString();
				}

				if (_drawObjectType.Equals(typeof(TextDrawObject)))
				{
					redraw(false);
					//(CurrentDrawObject as TextDrawObject).Typing = true;
					CurrentDrawObject.Draw(new Point((int)((e.Location.X - _imageRectangle.X) / _zoomMultiplier),
						(int)((e.Location.Y - _imageRectangle.Y) / _zoomMultiplier)));
					//pictMain.Focus();
					//pictMain.LostFocus += pictMain_LostFocus;
					//pictMain.KeyPress += pictMain_KeyPress;
				}
			}
		}

		//private void pictMain_LostFocus(object sender, EventArgs e)
		//{
		//	endText();
		//}

		public void ProcessKey(KeyPressEventArgs e)
		{
			if (CurrentDrawObject != null && CurrentDrawObject is ITextDrawObject && CurrentDrawObject.Selected)
			{
				var txt = CurrentDrawObject as ITextDrawObject;
				var intVal = (int)e.KeyChar;
				if (intVal == 13)
					txt.CurrentText += "\r\n";
				else if (intVal == 8)
				{
					if (txt.CurrentText.Length > 0)
					{
						txt.CurrentText = txt.CurrentText.Substring(0, txt.CurrentText.Length - 1);
					}
				}
				else
				{
					txt.CurrentText += e.KeyChar;
				}
				serializeDrawObjects();
				redraw(true);
			}
		}

		private void pictMain_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				DeleteCurrentDrawObject();
			else if (e.KeyCode == Keys.Z && e.Modifiers == Keys.Control)
				btnUndo_Click(sender, e);
			else if (e.KeyCode == Keys.Y && e.Modifiers == Keys.Control)
				btnRedo_Click(sender, e);
			else if (e.KeyCode == Keys.C && e.Modifiers == Keys.Control)
				btnCopy_Click(sender, e);
			else if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
				btnSave_Click(sender, e);
			else if ((e.KeyCode == Keys.Down || e.KeyCode == Keys.Up || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
				&& CurrentDrawObject != null)
			{
				int xDelta = 0;
				int yDelta = 0;
				if (e.KeyCode == Keys.Down) yDelta = 1;
				else if (e.KeyCode == Keys.Up) yDelta = -1;
				else if (e.KeyCode == Keys.Left) xDelta = -1;
				else if (e.KeyCode == Keys.Right) xDelta = 1;

				if (e.Modifiers == Keys.Control)
				{
					xDelta *= 10;
					yDelta *= 10;
				}

				if (e.Modifiers == Keys.Shift)
					CurrentDrawObject.MoveEnd(xDelta, yDelta);
				else
					CurrentDrawObject.Move(xDelta, yDelta);
				redraw(true);
			}
		}

		private void pictMain_MouseWheel(object sender, MouseEventArgs e)
		{
			if (ModifierKeys == Keys.Control && e.Delta != 0)
			{
				if (e.Delta > 0)
					_zoomMultiplier += .01F;
				else
					_zoomMultiplier -= .01F;
				zoomChanged();
			}
		}

		//private void pictMain_KeyPress(object sender, KeyPressEventArgs e)
		//{
		//	if (CurrentDrawObject is TextDrawObject)
		//	{
		//		if (e.KeyChar == 13)
		//			(CurrentDrawObject as TextDrawObject).CurrentText += "\r\n";
		//		else
		//			(CurrentDrawObject as TextDrawObject).CurrentText += e.KeyChar.ToString();
		//		redraw(true);
		//	}
		//}

		private void pictMain_MouseUp(object sender, MouseEventArgs e)
		{
			if (CurrentDrawObject != null && !_drawObjects.Contains(CurrentDrawObject))
			{
				_drawObjects.Add(CurrentDrawObject);
				addUndoAction(UndoActionType.AddDrawObject);
				if (!(CurrentDrawObject is TextDrawObject))
					serializeDrawObjects();
				btnCrop.Visible = CurrentDrawObject is SelectDrawObject;
				CurrentDrawObject.Selected = true;
				//if (!(_currentDrawObject is TextDrawObject) && !(_currentDrawObject is SelectDrawObject))
				//	_currentDrawObject = null;
				_lock = true;
				foreach (var tool in _tools)
				{
					tool.Checked = false;
				}
				_drawObjectType = null;
				setVisibilities();
				_lock = false;
				if (CurrentDrawObject is FillDrawObject)
					drawObjects(CurrentDrawObject as FillDrawObject);

				redraw(true);
			}
			else if (CurrentDrawObject != null)
			{
				var peek = _undos.Peek();
				if (peek.UndoActionType == UndoActionType.MoveResize)
				{
					var test = CurrentDrawObject.GetSerializable();
					if (test.ID == peek.DrawObject.ID
						&& test.StartPoint.X == peek.DrawObject.StartPoint.X
						&& test.StartPoint.Y == peek.DrawObject.StartPoint.Y
						&& test.EndPoint.X == peek.DrawObject.EndPoint.X
						&& test.EndPoint.Y == peek.DrawObject.EndPoint.Y)
						_undos.Pop();
				}
				serializeDrawObjects();
			}

			_mouseDownLocation = Point.Empty;
		}

		private void ucImageEditor_Resize(object sender, EventArgs e)
		{
			if (_currGraphics == null) return;

			LoadImage(true);
			redraw(true);
		}

		private void undoRedo(UndoAction action, Stack<UndoAction> opposites, bool redo)
		{
			var opposite = new UndoAction();
			if (action.UndoActionType == UndoActionType.AddDrawObject)
				opposite.UndoActionType = UndoActionType.DeleteDrawObject;
			else if (action.UndoActionType == UndoActionType.DeleteDrawObject)
				opposite.UndoActionType = UndoActionType.AddDrawObject;
			else
				opposite.UndoActionType = action.UndoActionType;

			if (action.UndoActionType == UndoActionType.DeleteDrawObject)
			{
				CurrentDrawObject = DrawObject.FromSerialized(_currGraphics, action.DrawObject, _zoomMultiplier);
				opposite.DrawObject = CurrentDrawObject.GetSerializable();
				_drawObjects.Add(CurrentDrawObject);
			}
			else
			{
				var drawObj = _drawObjects.First(d => d.ID == action.DrawObject.ID);
				opposite.DrawObject = drawObj.GetSerializable();
				if (action.UndoActionType == UndoActionType.AddDrawObject)
					_drawObjects.Remove(drawObj);
				else if (action.UndoActionType == UndoActionType.CropImage)
				{
					_cropRectangle = redo ?
						(CurrentDrawObject as SelectDrawObject).GetSelectionRectangle()
						: Rectangle.Empty;
					LoadImage(true);
				}
				else
					drawObj.Undo(action.DrawObject);
			}

			opposites.Push(opposite);

			serializeDrawObjects();

			redraw(true);

		}

		private void btnUndo_Click(object sender, EventArgs e)
		{
			if (!_undos.Any()) return;

			var undo = _undos.Pop();
			undoRedo(undo, _redos, false);
		}

		private void btnRedo_Click(object sender, EventArgs e)
		{
			if (!_redos.Any()) return;

			var redo = _redos.Pop();
			undoRedo(redo, _undos, true);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(FileName))
				return;

			dlgSaveAs.FileName = Path.GetFileName(FileName);
			if (dlgSaveAs.ShowDialog() == DialogResult.OK)
			{
				redraw(true, true);
				_bmp.Save(dlgSaveAs.FileName, ImageFormat.Png);
				//LoadImage();
			}
		}

		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
		{
			if (keyData == (Keys.Control | Keys.C))
			{
				CopyImage(false);
				return true;
			}
			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void pictMain_DoubleClick(object sender, EventArgs e)
		{
			if (ImageDoubleClick != null)
				ImageDoubleClick(sender, e);
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			CopyImage(false);
		}

		private void removeCurrentDrawObject(DrawObject drawObj)
		{
			_drawObjects.Remove(drawObj);
			serializeDrawObjects();
		}

		private void endText()
		{
			if (CurrentDrawObject != null && CurrentDrawObject is TextDrawObject)
			{
				var tdo = CurrentDrawObject as TextDrawObject;
				if (tdo.Selected)
				{
					//pictMain.KeyPress -= pictMain_KeyPress;
					//pictMain.LostFocus -= pictMain_LostFocus;
					//(CurrentDrawObject as TextDrawObject).Typing = false;
					CurrentDrawObject.Selected = false;
					if (string.IsNullOrEmpty(tdo.CurrentText) && _drawObjects.Last() == CurrentDrawObject)
						removeCurrentDrawObject(CurrentDrawObject);
					else
						serializeDrawObjects();
					CurrentDrawObject = null;
					redraw(true);
				}
			}
		}

		private void setVisibilities()
		{
			Type[] lineToolTypes = new Type[]
			{
				typeof(ArrowDrawObject), 
				typeof(EllipseDrawObject), 
				typeof(RectangleDrawObject), 
				typeof(LineDrawObject),
				typeof(DoubleArrowDrawObject),
				typeof(FreeFormDrawObject),
				typeof(CurlyBracesDrawObject)
			};

			pnlLineTools.Visible = (_drawObjectType != null && lineToolTypes.Contains(_drawObjectType))
				|| (CurrentDrawObject != null && lineToolTypes.Contains(CurrentDrawObject.GetType()));

			btnFont.Visible = (_drawObjectType != null && _drawObjectType.GetInterface(typeof(ITextDrawObject).Name) != null)
				|| (CurrentDrawObject != null && CurrentDrawObject is ITextDrawObject);

			pnlBlurTools.Visible = (_drawObjectType != null && _drawObjectType.GetInterface(typeof(IRadiusDrawObject).Name) != null)
				|| (CurrentDrawObject != null && CurrentDrawObject is IRadiusDrawObject);
		}

		private void chkTool_CheckChanged(object sender, EventArgs e)
		{
			if (_lock) return;

			var chk = sender as CheckBox;
			_currentTool = chk;
			var drawObjectType = chk.Tag as Type;

			if (!chk.Checked)
				_drawObjectType = null;
			else
				_drawObjectType = drawObjectType;

			_lock = true;
			foreach (var chk2 in _tools)
			{
				if (chk2.Equals(chk)) continue;
				chk2.Checked = false;
			}
			_lock = false;

			deselect(true);

			setVisibilities();
		}

		public void CopyImage(bool fromFile)
		{
			var img = Image.FromStream(new MemoryStream(File.ReadAllBytes(FileName)));
			if (fromFile && FileName != null)
				Clipboard.SetImage(img);
			else
			{
				using (var graphics = Graphics.FromImage(img))
				{
					foreach (var drawObj in _drawObjects)
					{
						if (drawObj is SelectDrawObject)
							continue;

						drawObj.DrawScaled(graphics);
					}
				}

				if (_drawObjects.Any())
				{
					var peek = _drawObjects.Last();
					if (peek is SelectDrawObject)
					{
						var selection = peek as SelectDrawObject;
						var selectionRect = selection.GetSelectionRectangle();
						img = img.CropImage(selectionRect);
					}
				}

				Clipboard.SetImage(img);

				LoadImage(true);
				redraw(true);
			}

			img.Dispose();
			img = null;
		}

		private void btnColor_Click(object sender, EventArgs e)
		{
			dlgColor.Color = CurrentColor;
			if (dlgColor.ShowDialog() == DialogResult.OK)
			{
				CurrentColor = Color.FromArgb((int)numAlpha.Value, dlgColor.Color);
				if (CurrentDrawObject != null)
				{
					CurrentDrawObject.UpdateColor(CurrentColor);
					redraw(true);
				}
				if (chkErase.Checked)
					EraseColor = CurrentColor;
				else if (chkHighlight.Checked)
					HighlightColor = CurrentColor;
				else
					DrawColor = CurrentColor;
			}
		}

		private void numAlpha_ValueChanged(object sender, EventArgs e)
		{
			if (_lock) return;

			CurrentColor = Color.FromArgb((int)numAlpha.Value, CurrentColor);
            if (CurrentDrawObject != null)
            {
                CurrentDrawObject.UpdateColor(CurrentColor);
                redraw(true);
            }
            if (chkErase.Checked)
                EraseColor = CurrentColor;
            else if (chkHighlight.Checked)
                HighlightColor = CurrentColor;
            else
                DrawColor = CurrentColor;
		}

		private void numWidth_ValueChanged(object sender, EventArgs e)
		{
			if (_lock) return;

			CurrentWidth = (float)numWidth.Value;
			if (chkErase.Checked)
				EraseWidth = CurrentWidth;
			else if (chkHighlight.Checked)
				HighlightWidth = CurrentWidth;
			else
				DrawWidth = CurrentWidth;
		}

		private void numRadius_ValueChanged(object sender, EventArgs e)
		{
			CurrentRadius = (int)numRadius.Value;
		}

		private void btnCrop_Click(object sender, EventArgs e)
		{
			if (CurrentDrawObject is SelectDrawObject)
			{
				addUndoAction(UndoActionType.CropImage);
				_cropRectangle = (CurrentDrawObject as SelectDrawObject).GetSelectionRectangle();
				LoadImage(true);
				_currentTool.Checked = false;
				btnCrop.Visible = false;
			}
		}

		private void btnFont_Click(object sender, EventArgs e)
		{
			dlgFont.Font = CurrentFont;
			if (dlgFont.ShowDialog() == DialogResult.OK)
			{
				CurrentFont = dlgFont.Font;
			}
		}

		public void DeleteCurrentDrawObject()
		{
			if (CurrentDrawObject != null)
			{
				addUndoAction(UndoActionType.DeleteDrawObject);
				removeCurrentDrawObject(CurrentDrawObject);
				CurrentDrawObject = null;
				btnDelete.Visible = false;
				redraw(true);
			}

			updateColorWidth(false);
		}

		private void btnDelete_Click(object sender, EventArgs e)
		{
			DeleteCurrentDrawObject();
		}

		private void pnlEraseColor_DoubleClick(object sender, EventArgs e)
		{
			dlgColor.Color = EraseColor;
			if (dlgColor.ShowDialog() == DialogResult.OK)
			{
				EraseColor = dlgColor.Color;
			}
		}

		private void updateColorWidth(bool selected)
		{
			_lock = true;
			var color = CurrentColor;
			float width = CurrentWidth;
			if (!selected)
			{
				if (chkHighlight.Checked)
				{
					color = HighlightColor;
					width = HighlightWidth;
				}
				else if (chkErase.Checked)
				{
					color = EraseColor;
					width = EraseWidth;
				}
				else
				{
					color = DrawColor;
					width = DrawWidth;
				}
			}
			_color = color;
			pnlCurrentColor.BackColor = color;
			numAlpha.Value = color.A;
			_currentLineWidth = width;
			if ((int)width < numWidth.Minimum)
				numWidth.Value = numWidth.Minimum;
			else
				numWidth.Value = (decimal)width;
			_lock = false;
		}

		private void chkHighlight_CheckedChanged(object sender, EventArgs e)
		{
			if (_lock) return;
			deselect(false);
			_lock = true;
			chkErase.Checked = false;
			_lock = false;
			updateColorWidth(false);
		}

		private void chkErase_CheckedChanged(object sender, EventArgs e)
		{
			if (_lock) return;
			deselect(false);
			_lock = true;
			chkHighlight.Checked = false;
			_lock = false;
			updateColorWidth(false);
		}

		private void zoomChanged()
		{
			if (_zoomMultiplier <= 0)
				_zoomMultiplier = 0.05F;

			_zoomMultiplier = (float)Math.Round(_zoomMultiplier, 2);

			LoadImage(true);
			_lock = true;
			numZoom.Value = (decimal)(_zoomMultiplier * 100);
			_lock = false;
		}

		private void btnZoomOut_Click(object sender, EventArgs e)
		{
			_zoomMultiplier -= 0.05F;
			zoomChanged();
		}

		private void btnZoomIn_Click(object sender, EventArgs e)
		{
			_zoomMultiplier += 0.05F;
			zoomChanged();
		}

		private void numZoom_ValueChanged(object sender, EventArgs e)
		{
			if (_lock) return;
            _zoomMultiplier = (float)(numZoom.Value / 100);
			zoomChanged();
		}
	}
}
