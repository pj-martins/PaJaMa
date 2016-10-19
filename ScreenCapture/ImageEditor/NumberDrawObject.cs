using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 975)]
	public class NumberDrawObject : TextDrawObjectBase
	{
		const float PADDING = 1.2F;
		private Rectangle getOuterRectangle()
		{
			var measure = _graphics.MeasureString("00", Font);
			return new Rectangle(relativeStartPoint.X, relativeStartPoint.Y, 
				(int)(measure.Width * PADDING), (int)(measure.Height * PADDING));
		}

		public override void Draw()
		{
			var outerRect = getOuterRectangle();
			_graphics.FillEllipse(new SolidBrush(_pen.Color), outerRect);

			var measure = _graphics.MeasureString(CurrentText, Font);
			var xpad = (outerRect.Width - measure.Width) / 2;
			var ypad = (outerRect.Height - measure.Height) / 2;
			_graphics.DrawString(CurrentText, Font, new SolidBrush(Color.White), 
				relativeStartPoint.X + xpad, relativeStartPoint.Y + ypad);
		}

		public override bool IsInDrawObject(Point point)
		{
			return getOuterRectangle().Contains(point);
		}
	}
}
