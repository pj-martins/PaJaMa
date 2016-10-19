using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 115)]
	public class CheckDrawObject : RectangleDrawObject
	{
		private int thirdX
		{
			get { return (2 * relativeStartPoint.X + relativeEndPoint.X) / 3; }
		}
		public override void Draw()
		{
			var rect = getDrawRectangle();
			_graphics.DrawLine(relativePen, relativeStartPoint.X, (relativeStartPoint.Y + relativeEndPoint.Y) / 2, thirdX, relativeEndPoint.Y);
			_graphics.DrawLine(relativePen, thirdX, relativeEndPoint.Y, relativeEndPoint.X, relativeStartPoint.Y);
			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
		}

		public override bool IsInDrawObject(System.Drawing.Point point)
		{
			var maxLength = Math.Max(Math.Abs(relativeStartPoint.X - relativeEndPoint.X), Math.Abs(relativeStartPoint.Y - relativeEndPoint.Y));
			if (maxLength == 0)
				return point.Equals(relativeStartPoint);

			var xIncrement = (float)(relativeEndPoint.X - relativeStartPoint.X) / (3F * (float)maxLength);
			var yIncrement = (float)(relativeEndPoint.Y - relativeStartPoint.Y) / (2F * (float)maxLength);

			var currX = (float)relativeStartPoint.X;
			var currY = (float)(relativeStartPoint.Y + relativeEndPoint.Y) / 2;

			if (checkInDrawObject(point, xIncrement, yIncrement, currX, currY)) return true;

			currX = (float)thirdX;
			currY = (float)relativeEndPoint.Y;
			xIncrement = 2 * (float)(relativeEndPoint.X - relativeStartPoint.X) / (3F * (float)maxLength);
			yIncrement = (float)(relativeStartPoint.Y - relativeEndPoint.Y) / (float)maxLength;
			

			if (checkInDrawObject(point, xIncrement, yIncrement, currX, currY)) return true;
			return false;
		}

		private bool checkInDrawObject(Point point, float xIncrement, float yIncrement, float currX, float currY)
		{
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

			return false;

		}
	}
}
