using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaJaMa.ScreenCapture.ImageEditor
{
	public class UndoAction
	{
		public UndoActionType UndoActionType { get; set; }
		public SerializableDrawObject DrawObject { get; set; }
	}

	public enum UndoActionType
	{
		AddDrawObject,
		DeleteDrawObject,
		MoveResize,
		StyleChange,
		CropImage
	}
}
