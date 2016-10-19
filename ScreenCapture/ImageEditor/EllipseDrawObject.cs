using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 400)]
	public class EllipseDrawObject : RectangleDrawObject
	{
		public override void Draw()
		{
			_graphics.DrawEllipse(relativePen, relativeStartPoint.X, relativeStartPoint.Y,
				(relativeEndPoint.X - relativeStartPoint.X), relativeEndPoint.Y - relativeStartPoint.Y);
			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
		}

		public override bool IsInDrawObject(Point point)
		{
			int delta = (int)((float)relativePen.Width / 2);
			for (int i = 0; i < 360; i++)
			{
				var pt = getCoordinate(i * Math.PI / 180, relativeStartPoint, relativeEndPoint);
				var rectToCheck = new Rectangle(new Point((int)(pt.X - delta), (int)(pt.Y - delta)),
						new Size((int)relativePen.Width, (int)relativePen.Width));
				if (rectToCheck.Contains(point))
					return true;
			}

			return false;
		}

		protected Point getCoordinate(double angleRadians, Point relativeStartPoint, Point relativeEndPoint)
		{
			int startX = Math.Min(relativeStartPoint.X, relativeEndPoint.X);
			int startY = Math.Min(relativeStartPoint.Y, relativeEndPoint.Y);
			int width = Math.Abs(relativeStartPoint.X - relativeEndPoint.X);
			int height = Math.Abs(relativeStartPoint.Y - relativeEndPoint.Y);

			return new Point(
				startX + (int)(width / 2) + (int)(((double)width / 2) * Math.Cos(angleRadians)),
				startY + (int)(height / 2) + (int)(((double)height / 2) * Math.Sin(angleRadians))
				);
		}
	}

	[DrawObject(Sequence = 500)]
	public class FillEllipseDrawObject : EllipseDrawObject
	{
		public override void Draw()
		{
			_graphics.FillEllipse(new SolidBrush(relativePen.Color), relativeStartPoint.X, relativeStartPoint.Y,
				(relativeEndPoint.X - relativeStartPoint.X), relativeEndPoint.Y - relativeStartPoint.Y);
			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
		}

		public override bool IsInDrawObject(Point point)
		{
			List<Point> coordinates = new List<Point>();
			for (int i = 0; i < 360; i++)
			{
				coordinates.Add(getCoordinate(i * Math.PI / 180, relativeStartPoint, relativeEndPoint));
			}
			
			var matching = coordinates.Where(c => c.X >= point.X - 1
				&& c.X <= point.X + 1);
			if (!matching.Any()) return false;

			var yMin = matching.Min(c => c.Y);
			var yMax = matching.Max(c => c.Y);

			if (point.Y >= yMin && point.Y <= yMax)
				return true;

			return false;
		}
	}
}
