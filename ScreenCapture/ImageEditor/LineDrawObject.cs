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
	[DrawObject(Sequence = 1)]
	public class LineDrawObject : DrawObject
	{
		public override void Draw()
		{
			_graphics.DrawLine(relativePen, relativeStartPoint, relativeEndPoint);
			if (Selected)
			{
				Cursor cursor = Cursors.SizeAll;
				if (relativeStartPoint.X == relativeEndPoint.X)
					cursor = Cursors.SizeWE;
				else if (relativeStartPoint.Y == relativeEndPoint.Y)
					cursor = Cursors.SizeNS;
				else if (relativeStartPoint.X > relativeEndPoint.X)
				{
					if (relativeStartPoint.Y > relativeEndPoint.Y)
						cursor = Cursors.SizeNWSE;
					else
						cursor = Cursors.SizeNESW;
				}
				else
				{
					if (relativeStartPoint.Y > relativeEndPoint.Y)
						cursor = Cursors.SizeNESW;
					else
						cursor = Cursors.SizeNWSE;
				}
				drawAndAddSelectionCircle(relativeStartPoint.X, relativeStartPoint.Y, cursor);
				drawAndAddSelectionCircle(relativeEndPoint.X, relativeEndPoint.Y, cursor);
			}
		}

		public override bool IsInDrawObject(Point point)
		{
			var maxLength = Math.Max(Math.Abs(relativeStartPoint.X - relativeEndPoint.X), Math.Abs(relativeStartPoint.Y - relativeEndPoint.Y));
			if (maxLength == 0)
				return point.Equals(relativeStartPoint);

			var xIncrement = (float)(relativeEndPoint.X - relativeStartPoint.X) / (float)maxLength;
			var yIncrement = (float)(relativeEndPoint.Y - relativeStartPoint.Y) / (float)maxLength;

			var currX = (float)relativeStartPoint.X;
			var currY = (float)relativeStartPoint.Y;

			while (
				currX <= Math.Max(relativeStartPoint.X, relativeEndPoint.X) &&
				currX >= Math.Min(relativeStartPoint.X, relativeEndPoint.X) &&
				currY <= Math.Max(relativeStartPoint.Y, relativeEndPoint.Y) &&
				currY >= Math.Min(relativeStartPoint.Y, relativeEndPoint.Y)
				)
			{
				if (relativePen.Width > 2)
				{
					int delta = (int)((float)relativePen.Width / 2);
					var rectToCheck = new Rectangle(new Point((int)(currX - delta), (int)(currY - delta)),
						new Size((int)relativePen.Width, (int)relativePen.Width));
					if (rectToCheck.Contains(point))
						return true;
				}
				else
				{
					if (point.X == (int)currX && point.Y == (int)currY)
						return true;
				}
				currY += yIncrement;
				currX += xIncrement;
			}


			// account for pen width


			return false;
		}
	}

	[DrawObject(Sequence = 100)]
	public class ArrowDrawObject : LineDrawObject
	{
		protected override void OnDrawObjectCreated()
		{
			_pen.StartCap = LineCap.Round;
			_pen.CustomEndCap = new AdjustableArrowCap(4, 4);
		}
	}

	[DrawObject(Sequence = 110)]
	public class DoubleArrowDrawObject : LineDrawObject
	{
		protected override void OnDrawObjectCreated()
		{
			_pen.CustomEndCap = new AdjustableArrowCap(4, 4);
			_pen.CustomStartCap = new AdjustableArrowCap(4, 4);
		}
	}

	//[DrawObject(Sequence = 700)]
	//public class HighlightDrawObject : LineDrawObject
	//{
	//	public HighlightDrawObject(int id, Graphics graphics, Pen pen, Point startPoint, float zoomMultiplier)
	//		: base(id, graphics, pen, startPoint, zoomMultiplier)
	//	{
	//		Pen.Color = Color.FromArgb(100, Pen.Color);
	//	}
	//}
}
