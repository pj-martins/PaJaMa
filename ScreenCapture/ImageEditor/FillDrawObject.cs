using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	[DrawObject(Sequence = 950)]
	public class FillDrawObject : DrawObject, IRadiusDrawObject
	{
		private Bitmap _resultBitmap;
		public Bitmap CurrentImage { get; set; }
		public int Radius { get; set; }

		public FillDrawObject()
		{
			Radius = 1;
		}

		//private int _radius;
		//public int Radius
		//{
		//	get { return _radius; }
		//	set
		//	{
		//		if (_radius != value)
		//		{
		//			if (_resultBitmap != null)
		//			{
		//				_resultBitmap.Dispose();
		//				_resultBitmap = null;
		//			}
		//			_radius = value;
		//		}
		//	}
		//}

		//private Pen _pen;
		//public override Pen Pen
		//{
		//	get { return _pen; }
		//	protected set
		//	{
		//		if (_pen != null && _pen.Color != value.Color)
		//		{
		//			if (_resultBitmap != null)
		//			{
		//				_resultBitmap.Dispose();
		//				_resultBitmap = null;
		//			}
		//		}
		//		_pen = value;
		//	}
		//}

		//private bool draw(int x, int y)
		//{
		//	int byteOffset = y * _stride + x * 4;
		//	if (Math.Abs((_rgbValues[byteOffset + 2] + _rgbValues[byteOffset + 1] + _rgbValues[byteOffset])
		//		- (_currentR + _currentG + _currentB)) <= Radius)
		//	{
		//		_rgbValues[byteOffset + 2] = Pen.Color.R;
		//		_rgbValues[byteOffset + 1] = Pen.Color.G;
		//		_rgbValues[byteOffset] = Pen.Color.B;
		//		return true;
		//	}

		//	return false;
		//}

		//private void recursivelyDrawPoint(List<Tuple<int, int>> checkedPoints, int x, int y)
		//{
		//	if (checkedPoints.Any(p => p.Item1 == x && p.Item2 == y) || checkedPoints.Count > 10000)
		//		return;

		//	checkedPoints.Add(new Tuple<int, int>(x, y));

		//	if (!draw(x, y))
		//		return;

		//	recursivelyDrawPoint(checkedPoints, x + 1, y);
		//	recursivelyDrawPoint(checkedPoints, x + 1, y + 1);
		//	recursivelyDrawPoint(checkedPoints, x, y + 1);
		//	recursivelyDrawPoint(checkedPoints, x - 1, y + 1);
		//	recursivelyDrawPoint(checkedPoints, x - 1, y);
		//	recursivelyDrawPoint(checkedPoints, x - 1, y - 1);
		//	recursivelyDrawPoint(checkedPoints, x, y - 1);
		//	recursivelyDrawPoint(checkedPoints, x + 1, y - 1);
		//}

		public override void Draw()
		{
			if (_resultBitmap == null)
			{
				if (Radius < 1) Radius = 1;

				using (var bmp = new Bitmap(CurrentImage))
				{
					_resultBitmap = new Bitmap(bmp);
					Imaging.FloodFill(_resultBitmap, relativeStartPoint.X, relativeStartPoint.Y, relativePen.Color, Radius);
				}
			}

			_graphics.DrawImage(_resultBitmap, 0, 0, _resultBitmap.Width, _resultBitmap.Height);
		}

		public override string GetCargo()
		{
			if (Radius < 1) return string.Empty;

			return XmlSerialize.SerializeObject<SerializableRadiusDrawObject>(new SerializableRadiusDrawObject()
			{
				Radius = Radius
			});
		}

		public override void SetCargo(string cargo)
		{
			if (string.IsNullOrEmpty(cargo)) return;
			try
			{
				var sdo = XmlSerialize.DeserializeObject<SerializableRadiusDrawObject>(cargo);
				Radius = sdo.Radius;
			}
			catch
			{
				return;
			}
		}

	}
}
