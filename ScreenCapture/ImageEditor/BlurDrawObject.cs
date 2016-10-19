using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	public interface IRadiusDrawObject
	{
		Bitmap CurrentImage { get; set; }
		int Radius { get; set; }
	}

	[DrawObject(Sequence = 1000)]
	public class RectangleBlurDrawObject : FillRectangleDrawObject, IRadiusDrawObject
	{
		public Bitmap CurrentImage { get; set; }
		public int Radius { get; set; }

		public RectangleBlurDrawObject() 
		{
			Radius = 8;
		}

		public override void Draw()
		{
			var crop = getDrawRectangle();

			if (crop.Width <= 0 || crop.Height <= 0) return;

			var bmp = new Bitmap(crop.Width, crop.Height);
			using (var gr = Graphics.FromImage(bmp))
			{
				gr.DrawImage(CurrentImage, new Rectangle(0, 0, bmp.Width, bmp.Height), crop, GraphicsUnit.Pixel);
			}

			bmp = Imaging.Blur(bmp, Radius);

			_graphics.DrawImage(bmp, getDrawRectangle());

			drawSelected(relativeStartPoint, relativeEndPoint, relativePen);
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

	//[DrawObject(Sequence = 1100)]
	//public class EllipseBlurDrawObject : FillEllipseDrawObject, IBlurDrawObject
	//{
	//	public Bitmap OriginalImage { get; set; }

	//}

	public class SerializableRadiusDrawObject
	{
		public int Radius { get; set; }
	}
}
