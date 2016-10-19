using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 975)]
	public class CurlyBracesDrawObject : FillRectangleDrawObject
	{
		public override void Draw()
		{
			var rect = getDrawRectangle();
			if (rect.Width < 2) return;
			if (rect.Height < 2) return;

			int width = relativeEndPoint.X - relativeStartPoint.X;


			List<Point> points = new List<Point>();
			points.Add(new Point(relativeStartPoint.X, rect.Y));
			points.Add(new Point(relativeStartPoint.X + (width / 2), rect.Y + (rect.Height / 15)));
			points.Add(new Point(relativeStartPoint.X + (width / 2), rect.Y + (int)((float)rect.Height / 2.5)));
			
			
			points.Add(new Point(relativeEndPoint.X, rect.Y + (rect.Height / 2)));

			_graphics.DrawCurve(relativePen, points.ToArray());

			points = new List<Point>();
			points.Add(new Point(relativeEndPoint.X, rect.Y + (rect.Height / 2)));

			int endY = rect.Y + rect.Height;
			points.Add(new Point(relativeStartPoint.X + (width / 2), endY - (int)((float)rect.Height / 2.5)));
			points.Add(new Point(relativeStartPoint.X + (width / 2), endY - (rect.Height / 15)));
			points.Add(new Point(relativeStartPoint.X, endY));



			_graphics.DrawCurve(relativePen, points.ToArray());



			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
		}
	}
}
