using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 200)]
	public class RectangleDrawObject : DrawObject
	{
		private Rectangle _topSelectionRect;
		private Rectangle _leftSelectionRect;
		private Rectangle _rightSelectionRect;
		private Rectangle _bottomSelectionRect;
		private Rectangle _topLeftSelectionRect;
		private Rectangle _topRightSelectionRect;
		private Rectangle _bottomLeftSelectionRect;
		private Rectangle _bottomRightSelectionRect;

		private bool _topLeft;
		private bool _topRight;
		private bool _bottomLeft;
		private bool _bottomRight;

		private bool _top;
		private bool _right;
		private bool _left;
		private bool _bottom;

		public override void Draw()
		{
			_graphics.DrawRectangle(relativePen, getDrawRectangle());
			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
		}

		protected void drawSelected(Point relativeStartPoint, Point relativeEndPoint, Pen relativePen)
		{
			if (Selected)
			{
				_topLeftSelectionRect = _topRightSelectionRect = _bottomLeftSelectionRect = _bottomRightSelectionRect =
				_topSelectionRect = _bottomSelectionRect = _leftSelectionRect = _rightSelectionRect
					= Rectangle.Empty;
				int penDelta = (int)(relativePen.Width / 2);
				int startX = relativeStartPoint.X;
				int startY = relativeStartPoint.Y;
				int endX = relativeEndPoint.X;
				int endY = relativeEndPoint.Y;

				_topLeftSelectionRect = drawAndAddSelectionCircle(startX, startY, Cursors.SizeNWSE);
				_bottomRightSelectionRect = drawAndAddSelectionCircle(endX, endY, Cursors.SizeNWSE);
				_bottomLeftSelectionRect = drawAndAddSelectionCircle(startX, endY, Cursors.SizeNESW);
				_topRightSelectionRect = drawAndAddSelectionCircle(endX, startY, Cursors.SizeNESW);

				_topSelectionRect = drawAndAddSelectionCircle(startX + ((endX - startX) / 2), startY, Cursors.SizeNS);
				_bottomSelectionRect = drawAndAddSelectionCircle(startX + ((endX - startX) / 2), endY, Cursors.SizeNS);
				_leftSelectionRect = drawAndAddSelectionCircle(startX, startY + ((endY - startY) / 2), Cursors.SizeWE);
				_rightSelectionRect = drawAndAddSelectionCircle(endX, startY + ((endY - startY) / 2), Cursors.SizeWE);
			}
		}

		public override void StartResize(Rectangle resizeRect)
		{
			_topLeft = _topRight = _bottomLeft = _bottomRight = _top = _bottom = _left = _right = false;

			if (resizeRect == _topRightSelectionRect)
				_topRight = true;
			else if (resizeRect == _bottomRightSelectionRect)
				_bottomRight = true;
			else if (resizeRect == _topLeftSelectionRect)
				_topLeft = true;
			else if (resizeRect == _bottomLeftSelectionRect)
				_bottomLeft = true;
			else if (resizeRect == _topSelectionRect)
				_top = true;
			else if (resizeRect == _bottomSelectionRect)
				_bottom = true;
			else if (resizeRect == _leftSelectionRect)
				_left = true;
			else if (resizeRect == _rightSelectionRect)
				_right = true;
		}

		protected override void resizeTo(Point relativePoint)
		{
			if (_topLeft)
				_startPoint = relativePoint;
			else if (_bottomRight)
				_endPoint = relativePoint;
			else if (_topRight)
			{
				_endPoint.X = relativePoint.X;
				_startPoint.Y = relativePoint.Y;
			}
			else if (_bottomLeft)
			{
				_startPoint.X = relativePoint.X;
				_endPoint.Y = relativePoint.Y;
			}
			else if (_top)
				_startPoint.Y = relativePoint.Y;
			else if (_bottom)
				_endPoint.Y = relativePoint.Y;
			else if (_left)
				_startPoint.X = relativePoint.X;
			else if (_right)
				_endPoint.X = relativePoint.X;

			draw(true);
		}

		public override bool IsInDrawObject(Point point)
		{
			var delta = (int)((float)relativePen.Width / 2);

			var rect = new Rectangle(Math.Min(relativeStartPoint.X, relativeEndPoint.X) - delta, Math.Min(relativeStartPoint.Y, relativeEndPoint.Y) - delta,
				Math.Abs(relativeStartPoint.X - relativeEndPoint.X) + (int)relativePen.Width, Math.Abs(relativeStartPoint.Y - relativeEndPoint.Y) + (int)relativePen.Width);

			if (rect.Contains(point))
			{
				// top
				if (point.Y <= relativeStartPoint.Y + delta && point.Y >= relativeStartPoint.Y - delta)
					return true;

				// bottom
				if (point.Y <= relativeEndPoint.Y + delta && point.Y >= relativeEndPoint.Y - delta)
					return true;

				// left
				if (point.X <= relativeStartPoint.X + delta && point.X >= relativeStartPoint.X - delta)
					return true;

				// right
				if (point.X <= relativeEndPoint.X + delta && point.X >= relativeEndPoint.X - delta)
					return true;

			}

			return false;
		}
	}

	[DrawObject(Sequence = 300)]
	public class FillRectangleDrawObject : RectangleDrawObject
	{
		public override void Draw()
		{
			_graphics.FillRectangle(new SolidBrush(relativePen.Color), getDrawRectangle());

			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
		}

		public override bool IsInDrawObject(Point point)
		{
			var delta = (int)((float)relativePen.Width / 2);

			var rect = new Rectangle(Math.Min(relativeStartPoint.X, relativeEndPoint.X) - delta,
				Math.Min(relativeStartPoint.Y, relativeEndPoint.Y) - delta,
				Math.Abs(relativeStartPoint.X - relativeEndPoint.X) + (int)relativePen.Width,
				Math.Abs(relativeStartPoint.Y - relativeEndPoint.Y) + (int)relativePen.Width);

			return rect.Contains(point);
		}
	}
}
