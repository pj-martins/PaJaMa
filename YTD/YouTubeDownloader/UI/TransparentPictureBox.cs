// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.UI.TransparentPictureBox
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Drawing;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.UI
{
  public class TransparentPictureBox : PictureBox
  {
    protected override void OnPaintBackground(PaintEventArgs e)
    {
      base.OnPaintBackground(e);
      if (this.Parent == null)
        return;
      int childIndex = this.Parent.Controls.GetChildIndex((Control) this);
      for (int index = 0; index < this.Parent.Controls.Count; ++index)
      {
        if (index != childIndex)
        {
          Control control = this.Parent.Controls[index];
          if (control.Bounds.IntersectsWith(this.Bounds) && control.Visible)
          {
            using (Bitmap bitmap = new Bitmap(control.Width, control.Height, e.Graphics))
            {
              control.DrawToBitmap(bitmap, control.ClientRectangle);
              e.Graphics.TranslateTransform((float) (control.Left - this.Left), (float) (control.Top - this.Top));
              e.Graphics.DrawImageUnscaled((Image) bitmap, Point.Empty);
              e.Graphics.TranslateTransform((float) (this.Left - control.Left), (float) (this.Top - control.Top));
            }
          }
        }
      }
    }
  }
}
