using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 10000)]
	public class SelectDrawObject : DrawObject
	{
		public Rectangle GetSelectionRectangle()
		{
			int startPointX = Math.Min(_startPoint.X, _endPoint.X);
			int endPointX = Math.Max(_startPoint.X, _endPoint.X);
			int startPointY = Math.Min(_startPoint.Y, _endPoint.Y);
			int endPointY = Math.Max(_startPoint.Y, _endPoint.Y);
			return new Rectangle(new Point((int)(startPointX), (int)(startPointY)),
				new Size((int)((endPointX - startPointX)), (int)((endPointY - startPointY))));
		}

		public override void Draw()
		{
			var pen = new Pen(System.Drawing.Color.Black, 1);
			int length = 5;

			int curr = Math.Min(relativeStartPoint.X, relativeEndPoint.X);
			int endPointX = Math.Max(relativeStartPoint.X, relativeEndPoint.X);
			while (curr + length < endPointX)
			{
				int end = curr + length;
				if (end > endPointX)
					end = endPointX;

				_graphics.DrawLine(pen, new Point(curr, relativeStartPoint.Y), new Point(end, relativeStartPoint.Y));
				_graphics.DrawLine(pen, new Point(curr, relativeEndPoint.Y), new Point(end, relativeEndPoint.Y));

				curr += length * 2;
			}

			curr = Math.Min(relativeStartPoint.Y, relativeEndPoint.Y);
			int endPointY = Math.Max(relativeStartPoint.Y, relativeEndPoint.Y);
			while (curr < endPointY)
			{
				int end = curr + length;
				if (end > endPointY)
					end = endPointY;

				_graphics.DrawLine(pen, new Point(relativeStartPoint.X, curr), new Point(relativeStartPoint.X, end));
				_graphics.DrawLine(pen, new Point(relativeEndPoint.X, curr), new Point(relativeEndPoint.X, end));

				curr += length * 2;
			}
		}

		public override bool IsInDrawObject(Point point)
		{
			var rect = new Rectangle(Math.Min(relativeStartPoint.X, relativeEndPoint.X), Math.Min(relativeStartPoint.Y, relativeEndPoint.Y),
				Math.Abs(relativeStartPoint.X - relativeEndPoint.X), Math.Abs(relativeStartPoint.Y - relativeEndPoint.Y));

			return rect.Contains(point);
		}
	}
}
