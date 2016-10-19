using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 950)]
	public class SpeechBubbleDrawObject : FillRectangleDrawObject, ITextDrawObject
	{
		private TextDrawObject _textDrawObject;

		public Font Font
		{
			get { return _textDrawObject.Font; }
			set { _textDrawObject.Font = value; }
		}

		public string CurrentText
		{
			get { return _textDrawObject.CurrentText; }
			set { _textDrawObject.CurrentText = value; }
		}

		public override bool Selected
		{
			get { return base.Selected; }
			set
			{
				base.Selected = value;
				if (_textDrawObject == null) return;
				_textDrawObject.Selected = value;
			}
		}

		protected override void OnDrawObjectCreated()
		{
			base.OnDrawObjectCreated();
			if (_textDrawObject == null)
			{
				_textDrawObject = (TextDrawObject)CreateDrawObject(typeof(TextDrawObject), this.ID, _graphics, _pen, _startPoint,
					ZoomMultiplier);
			}
		}

		public override void Draw()
		{
			int minLength = Math.Min(Math.Abs(relativeEndPoint.Y - relativeStartPoint.Y), Math.Abs(relativeEndPoint.X - relativeStartPoint.X));

			int circleradius = (int)((float)minLength / 12);
			int tagLength = (int)((float)minLength / 7);

			if (circleradius == 0 || tagLength == 0) return;

			int startX1 = relativeStartPoint.X + tagLength;
			int startX2 = relativeStartPoint.X + tagLength;
			int endX1 = relativeEndPoint.X;
			int endX2 = relativeEndPoint.X;
			int startY1 = relativeStartPoint.Y + tagLength;
			int startY2 = relativeStartPoint.Y + tagLength;
			int endY1 = relativeEndPoint.Y;
			int endY2 = relativeEndPoint.Y;

			int tagStartX = relativeStartPoint.X;
			int tagStartY = relativeStartPoint.Y;
			int tagEnd1X = relativeStartPoint.X + (int)(tagLength * 1.5);
			int tagEnd1Y = relativeStartPoint.Y + tagLength;
			int tagEnd2X = relativeStartPoint.X + tagLength;
			int tagEnd2Y = relativeStartPoint.Y + (int)(tagLength * 1.5);

			int startAngle1 = -90;
			int startAngle2 = 90;
			int startAngle3 = 90;
			int sweepAngle1 = 90;
			int sweepAngle2 = -90;
			int sweepAngle3 = 90;

			int addYRadius = 0;
			int addXRadius = 0;

			int radiusXMultiplier = 1;

			int radiusYMultiplier = 1;

			if (relativeStartPoint.X > relativeEndPoint.X)
			{
				if (relativeStartPoint.Y > relativeEndPoint.Y)
				{
					// top left
					tagEnd1X = relativeStartPoint.X - (int)(tagLength * 1.5);
					tagEnd1Y = relativeStartPoint.Y - tagLength;
					tagEnd2X = relativeStartPoint.X - tagLength;
					tagEnd2Y = relativeStartPoint.Y - (int)(tagLength * 1.5);
					startY1 = relativeStartPoint.Y - tagLength;
					startX1 = relativeStartPoint.X - tagLength;
					startX2 = relativeStartPoint.X - tagLength;
					startY2 = relativeStartPoint.Y - tagLength;
					radiusXMultiplier = -1;
					radiusYMultiplier = -1;
					startAngle1 = 90;
					startAngle2 = -90;
					startAngle3 = -90;
					addYRadius = circleradius * 2;
					addXRadius = circleradius * 2;
				}
				else
				{
					// bottom left
					tagEnd1X = relativeStartPoint.X - (int)(tagLength * 1.5);
					tagEnd1Y = relativeStartPoint.Y + tagLength;
					tagEnd2X = relativeStartPoint.X - tagLength;
					tagEnd2Y = relativeStartPoint.Y + (int)(tagLength * 1.5);
					startX1 = relativeStartPoint.X - tagLength;
					startX2 = relativeStartPoint.X - tagLength;
					radiusXMultiplier = -1;
					sweepAngle1 = -90;
					sweepAngle2 = 90;
					sweepAngle3 = -90;
					addXRadius = circleradius * 2;
				}
			}
			else if (relativeStartPoint.Y > relativeEndPoint.Y)
			{
				// top right
				tagEnd1X = relativeStartPoint.X + (int)(tagLength * 1.5);
				tagEnd1Y = relativeStartPoint.Y - tagLength;
				tagEnd2X = relativeStartPoint.X + tagLength;
				tagEnd2Y = relativeStartPoint.Y - (int)(tagLength * 1.5);
				startY1 = relativeStartPoint.Y - tagLength;
				startY2 = relativeStartPoint.Y - tagLength;
				radiusYMultiplier = -1;
				startAngle1 = 90;
				startAngle2 = -90;
				startAngle3 = -90;
				sweepAngle1 = -90;
				sweepAngle2 = 90;
				sweepAngle3 = -90;
				addYRadius = circleradius * 2;
			}

			#region BACKGROUND

			var fillPoints = new List<Point>();

			fillPoints.Add(new Point(startX1, startY1));
			fillPoints.Add(new Point(endX1 - (radiusXMultiplier * circleradius), startY1));
			fillPoints.Add(new Point(endX1, startY1 + (radiusYMultiplier * circleradius)));
			fillPoints.Add(new Point(endX1, endY2 - (radiusYMultiplier * circleradius)));
			fillPoints.Add(new Point(endX1 - (radiusXMultiplier * circleradius), endY2));
			fillPoints.Add(new Point(startX1 + (radiusXMultiplier * circleradius), endY1));
			fillPoints.Add(new Point(startX1, endY1 - (radiusYMultiplier * circleradius)));

			_graphics.FillPolygon(new SolidBrush(Color.White), fillPoints.ToArray());


			fillPoints = new List<Point>();

			// tag
			fillPoints.Add(new Point(tagStartX, tagStartY));
			fillPoints.Add(new Point(tagEnd1X, tagEnd1Y));
			fillPoints.Add(new Point(tagEnd2X, tagEnd2Y));

			_graphics.FillPolygon(new SolidBrush(Color.White), fillPoints.ToArray());

			// top right
			_graphics.FillEllipse(new SolidBrush(Color.White), endX1 - (circleradius * 2) + addXRadius, startY2 - addYRadius, (circleradius * 2), (circleradius * 2));

			// bottom right
			_graphics.FillEllipse(new SolidBrush(Color.White), endX1 - (circleradius * 2) + addXRadius, endY2 - (circleradius * 2) + addYRadius, (circleradius * 2), (circleradius * 2));

			// bottom left
			_graphics.FillEllipse(new SolidBrush(Color.White), startX1 - addXRadius, endY2 - (circleradius * 2) + addYRadius, (circleradius * 2), (circleradius * 2));
			#endregion

			#region FOREGROUND
			_graphics.DrawLine(relativePen, tagEnd1X, startY1, endX1 - (radiusXMultiplier * circleradius), startY1);
			_graphics.DrawLine(relativePen, startX1, tagEnd2Y, startX1, endY1 - (radiusYMultiplier * circleradius));
			_graphics.DrawLine(relativePen, endX1, startY2 + (radiusYMultiplier * circleradius), endX1, endY2 - (radiusYMultiplier * circleradius));
			_graphics.DrawLine(relativePen, startX2 + (radiusXMultiplier * circleradius), endY2,
				endX2 - (radiusXMultiplier * circleradius), endY2);

			// tag 1
			_graphics.DrawLine(relativePen, tagStartX, tagStartY, tagEnd1X, tagEnd1Y);

			// tag 2
			_graphics.DrawLine(relativePen, tagStartX, tagStartY, tagEnd2X, tagEnd2Y);


			// end1 start2
			_graphics.DrawArc(relativePen, endX1 - (circleradius * 2) + addXRadius, startY2 - addYRadius, (circleradius * 2), (circleradius * 2),
				startAngle1, sweepAngle1);

			// end1 end2
			_graphics.DrawArc(relativePen, endX1 - (circleradius * 2) + addXRadius, endY2 - (circleradius * 2) + addYRadius, (circleradius * 2), (circleradius * 2),
				startAngle2, sweepAngle2);

			// start1 end2
			_graphics.DrawArc(relativePen, startX1 - addXRadius, endY2 - (circleradius * 2) + addYRadius, (circleradius * 2), (circleradius * 2),
				startAngle3, sweepAngle3);
			#endregion

			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);

			_textDrawObject.ZoomMultiplier = ZoomMultiplier;

			var textSize = _graphics.MeasureString(_textDrawObject.CurrentText, _textDrawObject.Font);

			_textDrawObject.MoveTo(new Point(
				_startPoint.X + (int)((float)(_endPoint.X - _startPoint.X) / 2 - textSize.Width / 2 + radiusXMultiplier * tagLength / 1.5),
				_startPoint.Y + (int)((float)(_endPoint.Y - _startPoint.Y) / 2 - textSize.Height / 2 + radiusYMultiplier * tagLength / 1.5)
				));
			_textDrawObject.Draw(_graphics);
		}

		public override string GetCargo()
		{
			return _textDrawObject.GetCargo();
		}

		public override void SetCargo(string cargo)
		{
			_textDrawObject.SetCargo(cargo);
		}

		public override void UpdateColor(Color newColor)
		{
			base.UpdateColor(newColor);
			_textDrawObject.UpdateColor(newColor);
		}
	}
}
