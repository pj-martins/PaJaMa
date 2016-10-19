using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaJaMa.ScreenCapture
{
	public class UserSettings
	{
		public int FormWidth { get; set; }
		public int FormHeight { get; set; }
		public bool Maximized { get; set; }
		public int ThumbnailSize { get; set; }
		public int SplitterDistance { get; set; }
		public int DrawColorArgb { get; set; }
		public float DrawWidth { get; set; }
		public int EraseColorArgb { get; set; }
		public float EraseWidth { get; set; }
		public int HighlightColorArgb { get; set; }
		public float HighlightWidth { get; set; }
		public string Font { get; set; }
		public int Radius { get; set; }
	}
}
