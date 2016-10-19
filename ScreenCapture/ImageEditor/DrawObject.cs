using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	public abstract class DrawObject
	{
		protected Graphics _graphics;
		protected int _penWidth;
		protected Dictionary<Rectangle, Cursor> _selectionRectangles;

		protected Point _startPoint;
		protected Point _endPoint;
		protected Pen _pen;


		private bool _resizeStart;
		private bool _resizeEnd;

		public int ID { get; private set; }

		public bool Erase { get; set; }
		public bool Highlight { get; set; }

		private float _zoomMultiplier;
		public float ZoomMultiplier
		{
			get { return _zoomMultiplier; }
			set
			{
				if (_zoomMultiplier != value)
				{
					_zoomMultiplier = value;
					_relativeStartPoint = Point.Empty;
					_relativeEndPoint = Point.Empty;
					_relativePen = null;
				}
			}
		}

		private bool _selected;
		public virtual bool Selected
		{
			get { return _selected; }
			set
			{
				_selected = value;
				_selectionRectangles.Clear();
			}
		}

		private Point _relativeStartPoint;
		protected Point relativeStartPoint
		{
			get
			{
				if (_relativeStartPoint == Point.Empty)
					_relativeStartPoint = new Point((int)(_startPoint.X * ZoomMultiplier), (int)(_startPoint.Y * ZoomMultiplier));
				return _relativeStartPoint;
			}
		}

		private Point _relativeEndPoint;
		protected Point relativeEndPoint
		{
			get
			{
				if (_relativeEndPoint == Point.Empty)
					_relativeEndPoint = new Point((int)(_endPoint.X * ZoomMultiplier), (int)(_endPoint.Y * ZoomMultiplier));
				return _relativeEndPoint;
			}
		}

		private Pen _relativePen;
		protected Pen relativePen
		{
			get
			{
				if (_relativePen == null)
				{
					_relativePen = (Pen)_pen.Clone();
					_relativePen.Width *= ZoomMultiplier;
				}
				return _relativePen;
			}
		}

		protected Rectangle getDrawRectangle()
		{
			return new Rectangle(Math.Min(relativeStartPoint.X, relativeEndPoint.X),
				Math.Min(relativeStartPoint.Y, relativeEndPoint.Y),
				Math.Abs(relativeEndPoint.X - relativeStartPoint.X), Math.Abs(relativeEndPoint.Y - relativeStartPoint.Y));
		}

		public virtual string GetCargo()
		{
			return string.Empty;
		}

		public virtual void SetCargo(string cargo)
		{
		}

		public SerializableDrawObject GetSerializable()
		{
			return new SerializableDrawObject()
			{
				DrawObjectType = this.GetType().FullName,
				ID = ID,
				StartPoint = new SerializablePoint() { X = _startPoint.X, Y = _startPoint.Y },
				EndPoint = new SerializablePoint() { X = _endPoint.X, Y = _endPoint.Y },
				PenColor = _pen.Color.ToArgb(),
				PenWidth = _pen.Width,
				Erase = Erase,
				Highlight = Highlight,
				Cargo = GetCargo()
			};
		}

		public static DrawObject FromSerialized(Graphics graphics, SerializableDrawObject sdo, float zoomMultiplier)
		{
			//var drawObj = Activator.CreateInstance(typeof(DrawObject).Assembly.GetType(sdo.DrawObjectType), new object[] { sdo.ID, graphics, 
			//		new Pen(Color.FromArgb(sdo.PenColor), sdo.PenWidth), new Point(sdo.StartX, sdo.StartY), sdo.Erase, sdo.Highlight }) as DrawObject;
			var drawObj = (DrawObject)CreateDrawObject(typeof(DrawObject).Assembly.GetType(sdo.DrawObjectType), sdo.ID, graphics,
				new Pen(Color.FromArgb(sdo.PenColor), sdo.PenWidth), new Point(sdo.StartPoint.X, sdo.StartPoint.Y), zoomMultiplier);
			drawObj.Erase = sdo.Erase;
			drawObj.Highlight = sdo.Highlight;
			drawObj._endPoint = new Point(sdo.EndPoint.X, sdo.EndPoint.Y);
			drawObj.ID = sdo.ID;
			drawObj.SetCargo(sdo.Cargo);
			drawObj.OnDrawObjectCreated();
			return drawObj;
		}

		public static DrawObject CreateDrawObject(Type drawObjectType, int id, Graphics graphics, Pen pen, Point startPoint, float zoomMultiplier)
		{
			var drawObject = (DrawObject)Activator.CreateInstance(drawObjectType);
			drawObject._graphics = graphics;
			drawObject._graphics.SmoothingMode = SmoothingMode.AntiAlias;

			drawObject.ID = id;
			drawObject._pen = new Pen(pen.Color, pen.Width);
			drawObject.ZoomMultiplier = zoomMultiplier;
			drawObject._startPoint = new Point((int)(startPoint.X), (int)(startPoint.Y));
			drawObject._selectionRectangles = new Dictionary<Rectangle, Cursor>();
			drawObject.OnDrawObjectCreated();
			return drawObject;
		}

		protected virtual void OnDrawObjectCreated() { }

		public void Undo(SerializableDrawObject sdo)
		{
			_startPoint = new Point(sdo.StartPoint.X, sdo.StartPoint.Y);
			_endPoint = new Point(sdo.EndPoint.X, sdo.EndPoint.Y);
			_pen.Color = Color.FromArgb(sdo.PenColor);
			_pen.Width = sdo.PenWidth;
			reset();
		}

		public void DrawScaled(Graphics graphics)
		{
			var currGraphics = _graphics;
			var currZoomMultiplier = ZoomMultiplier;

			ZoomMultiplier = 1;
			_graphics = graphics;

			draw(true);

			_graphics = currGraphics;
			ZoomMultiplier = currZoomMultiplier;
		}

		public virtual void Draw(Point endPoint)
		{
			_endPoint = endPoint;
			_relativeEndPoint = Point.Empty;
			Draw();
		}

		public virtual bool IsInDrawObject(Point point)
		{
			return false;
		}

		public KeyValuePair<Rectangle, Cursor> GetResizeRectangle(Point point)
		{
			return _selectionRectangles.FirstOrDefault(sr => sr.Key.Contains(point));
		}

		public virtual void MoveTo(Point originalPoint, Point newPoint)
		{
			var xDelta = newPoint.X - originalPoint.X;
			var yDelta = newPoint.Y - originalPoint.Y;
			Move(xDelta, yDelta);
		}

		public virtual void Move(int xDelta, int yDelta)
		{
			_startPoint = new Point(_startPoint.X + xDelta, _startPoint.Y + yDelta);
			_endPoint = new Point(_endPoint.X + xDelta, _endPoint.Y + yDelta);
			draw(true);
		}

		public void MoveEnd(int xDelta, int yDelta)
		{
			_endPoint = new Point(_endPoint.X + (int)(xDelta / ZoomMultiplier), _endPoint.Y + (int)(yDelta / ZoomMultiplier));
			draw(true);
		}

		public void ResizeTo(Point newPoint)
		{
			resizeTo(new Point((int)(newPoint.X / ZoomMultiplier), (int)(newPoint.Y / ZoomMultiplier)));
		}

		protected virtual void resizeTo(Point relativePoint)
		{
			if (_resizeStart)
			{
				_startPoint.X = relativePoint.X;
				_startPoint.Y = relativePoint.Y;
			}
			else if (_resizeEnd)
			{
				_endPoint.X = relativePoint.X;
				_endPoint.Y = relativePoint.Y;
			}

			draw(true);
		}

		public virtual void UpdateColor(Color newColor)
		{
			_pen.Color = newColor;
			reset();
		}

		public virtual void UpdateWidth(float newWidth)
		{
			_pen.Width = newWidth / ZoomMultiplier;
			reset();
		}

		public float GetWidth()
		{
			return _pen.Width * ZoomMultiplier;
		}

		public Color GetColor()
		{
			return _pen.Color;
		}

		public virtual void StartResize(Rectangle resizeRect)
		{
			_resizeStart = _resizeEnd = false;
			if (resizeRect.Contains(relativeStartPoint))
				_resizeStart = true;
			else if (resizeRect.Contains(relativeEndPoint))
				_resizeEnd = true;
		}

		protected virtual Rectangle drawAndAddSelectionCircle(int x, int y, Cursor cursor)
		{
			int selectionLength = 6;
			int delta = selectionLength / 2;
			_graphics.FillEllipse(new SolidBrush(Color.White), x - delta, y - delta, selectionLength, selectionLength);
			_graphics.DrawEllipse(new Pen(Color.Black), x - delta, y - delta, selectionLength, selectionLength);

			var rect = new Rectangle(x - delta, y - delta, selectionLength, selectionLength);
			if (!_selectionRectangles.ContainsKey(rect))
				_selectionRectangles.Add(rect, cursor);
			return rect;
		}

		protected void draw(bool resetRelatives)
		{
			if (resetRelatives)
			{
				reset();
			}
			Draw();
		}

		private void reset()
		{
			_relativeStartPoint = Point.Empty;
			_relativeEndPoint = Point.Empty;
			_relativePen = null;
		}

		public abstract void Draw();
	}

	public class DrawObjectAttribute : Attribute
	{
		public int Sequence { get; set; }
	}

	public class SerializablePoint
	{
		public int X { get; set; }
		public int Y { get; set; }
	}

	public class SerializableDrawObject
	{
		public string DrawObjectType { get; set; }
		public int ID { get; set; }
		public SerializablePoint StartPoint { get; set; }
		public SerializablePoint EndPoint { get; set; }
		public float PenWidth { get; set; }
		public int PenColor { get; set; }
		public string Cargo { get; set; }
		public bool Erase { get; set; }
		public bool Highlight { get; set; }

		public static string GetXMLForDrawObjects(List<DrawObject> objs)
		{
			List<SerializableDrawObject> serializable = objs.ConvertAll(o => o.GetSerializable());
			return XmlSerialize.SerializeObject<List<SerializableDrawObject>>(serializable);
		}

		public static List<DrawObject> GetDrawObjectsFromXML(Graphics graphics, string xml, float zoomMultiplier)
		{
			try
			{
				List<SerializableDrawObject> serializable = XmlSerialize.DeserializeObject<List<SerializableDrawObject>>(xml);
				return serializable.ConvertAll(o => DrawObject.FromSerialized(graphics, o, zoomMultiplier));
			}
			catch
			{
				return new List<DrawObject>();
			}
		}
	}
}