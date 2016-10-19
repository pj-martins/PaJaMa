using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 900)]
	public class TextDrawObject : TextDrawObjectBase
	{
		public void Draw(Graphics graphics)
		{
			var tempGraphics = _graphics;
			_graphics = graphics;
			Draw();
			_graphics = tempGraphics;
		}

		public override void Draw()
		{
			if (Font == null) return;

			if (string.IsNullOrEmpty(CurrentText))
			{
				if (!Selected) return;

				var empty = _graphics.MeasureString("A", Font);
				_graphics.DrawLine(new Pen(Color.Black, 1), relativeStartPoint, new Point(relativeStartPoint.X,
					(int)(relativeStartPoint.Y + empty.Height / 2)));
				return;
			}

			var measure = _graphics.MeasureString(CurrentText + (CurrentText.EndsWith("\n") ? "A" : ""), Font);
			_graphics.DrawString(CurrentText, Font, new SolidBrush(relativePen.Color), relativeStartPoint);

			if (Selected)
			{
				int length = 2;

				int curr = relativeStartPoint.X;
				int endPointX = (int)(relativeStartPoint.X + measure.Width);
				int endPointY = (int)(relativeStartPoint.Y + measure.Height);
				while (curr + length < endPointX)
				{
					int end = curr + length;
					if (end > endPointX)
						end = endPointX;

					_graphics.DrawLine(new Pen(Color.Black, 1), new Point(curr, relativeStartPoint.Y), new Point(end, relativeStartPoint.Y));
					_graphics.DrawLine(new Pen(Color.Black, 1), new Point(curr, endPointY), new Point(end, endPointY));

					curr += length * 2;
				}

				curr = relativeStartPoint.Y;
				while (curr < endPointY)
				{
					int end = curr + length;
					if (end > endPointY)
						end = endPointY;

					_graphics.DrawLine(new Pen(Color.Black, 1), new Point(relativeStartPoint.X, curr), new Point(relativeStartPoint.X, end));
					_graphics.DrawLine(new Pen(Color.Black, 1), new Point(endPointX, curr), new Point(endPointX, end));

					curr += length * 2;
				}
			}
		}
	}

	public abstract class TextDrawObjectBase : DrawObject, ITextDrawObject
	{
		public string CurrentText { get; set; }
		public Font Font { get; set; }

		public TextDrawObjectBase()
		{
			CurrentText = string.Empty;
		}

		public override string GetCargo()
		{
			if (string.IsNullOrEmpty(CurrentText)) return string.Empty;

			return XmlSerialize.SerializeObject<SerializableTextDrawObject>(new SerializableTextDrawObject()
				{
					CurrentText = CurrentText,
					Font = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(Font)
				});
		}

		public override void SetCargo(string cargo)
		{
			if (string.IsNullOrEmpty(cargo)) return;
			try
			{
				var sdo = XmlSerialize.DeserializeObject<SerializableTextDrawObject>(cargo);
				CurrentText = sdo.CurrentText;
				Font = (Font)TypeDescriptor.GetConverter(typeof(Font)).ConvertFromString(sdo.Font);
			}
			catch
			{
				return;
			}
		}

		public void MoveTo(Point newPoint)
		{
			_startPoint = newPoint;
			draw(true);
		}

		public override bool IsInDrawObject(Point point)
		{
			var measure = _graphics.MeasureString(CurrentText + (CurrentText.EndsWith("\n") ? "A" : ""), Font);
			var rect = new Rectangle(relativeStartPoint.X, relativeStartPoint.Y, (int)measure.Width, (int)measure.Height);
			return rect.Contains(point);
		}
	}

	public class SerializableTextDrawObject
	{
		public string CurrentText { get; set; }
		public string Font { get; set; }
	}

	public interface ITextDrawObject
	{
		Font Font { get; set; }
		string CurrentText { get; set; }
	}
}
