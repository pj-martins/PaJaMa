using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 600)]
	public class FreeFormDrawObject : DrawObject
	{
		private List<Point> _pointsToDraw = new List<Point>();
		protected override void OnDrawObjectCreated()
		{
			if (!_pointsToDraw.Any())
				_pointsToDraw.Add(_startPoint);
		}

		public override void Draw(Point endPoint)
		{
			_pointsToDraw.Add(endPoint);
			base.Draw(endPoint);
		}

		public override void Draw()
		{
			List<Point> relativePointsToDraw = new List<Point>();
			foreach (var pt in _pointsToDraw)
			{
				relativePointsToDraw.Add(new Point((int)(pt.X * ZoomMultiplier), (int)(pt.Y * ZoomMultiplier)));
			}
			_graphics.DrawCurve(relativePen, relativePointsToDraw.ToArray());
			if (Selected)
			{
				drawAndAddSelectionCircle(relativeStartPoint.X, relativeStartPoint.Y, Cursors.SizeAll);
				drawAndAddSelectionCircle(relativeEndPoint.X, relativeEndPoint.Y, Cursors.SizeAll);
			}
		}

		public override bool IsInDrawObject(Point point)
		{
			if (relativePen.Width > 2)
			{
				//int delta = (int)((float)relativePen.Width / 2);
				//foreach (var pt in _pointsToDraw)
				//{
				//	var rectToCheck = new Rectangle(new Point((int)((pt.X * ZoomMultiplier) - delta), 
				//		(int)((pt.Y * ZoomMultiplier) - delta)),
				//		new Size((int)relativePen.Width, (int)relativePen.Width));
				//	if (rectToCheck.Contains(point))
				//		return true;
				//}
				for (int i = 1; i < _pointsToDraw.Count; i++)
				{
					var pt1 = _pointsToDraw[i - 1];
					var pt2 = _pointsToDraw[i];
					var rectToCheck = new Rectangle(
						(int)(Math.Min(pt1.X, pt2.X) * ZoomMultiplier),
						(int)(Math.Min(pt1.Y, pt2.Y) * ZoomMultiplier),
						(int)(Math.Abs(pt1.X - pt2.X) * ZoomMultiplier),
						(int)(Math.Abs(pt1.Y - pt2.Y) * ZoomMultiplier)
						);
					if (rectToCheck.Contains(point))
						return true;
				}
			}

			return _pointsToDraw.Contains(point);
		}

		public override string GetCargo()
		{
			List<SerializablePoint> points = new List<SerializablePoint>();
			points.AddRange(_pointsToDraw.Select(p => new SerializablePoint() { X = p.X, Y = p.Y }));
			return XmlSerialize.SerializeObject<List<SerializablePoint>>(points);
		}

		public override void SetCargo(string cargo)
		{
			try
			{
				List<SerializablePoint> points = XmlSerialize.DeserializeObject<List<SerializablePoint>>(cargo);
				_pointsToDraw = points.Select(p => new Point(p.X, p.Y)).ToList();
			}
			catch { }
		}

		public override void MoveTo(Point originalPoint, Point newPoint)
		{
			base.MoveTo(originalPoint, newPoint);
			var xDelta = (int)((newPoint.X - originalPoint.X) / ZoomMultiplier);
			var yDelta = (int)((newPoint.Y - originalPoint.Y) / ZoomMultiplier);
			var currPoints = _pointsToDraw.ToList();
			_pointsToDraw.Clear();
			foreach (var pt in currPoints)
			{
				_pointsToDraw.Add(new Point(pt.X + xDelta, pt.Y + yDelta));
			}
			_startPoint = _pointsToDraw.First();
			_endPoint = _pointsToDraw.Last();
			Draw();
		}
	}
}
