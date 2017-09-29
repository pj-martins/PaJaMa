// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Extensions.PanelExtension
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Windows.Forms;

namespace FreeYouTubeDownloader.Extensions
{
  public static class PanelExtension
  {
    public static void ScrollDown(this Panel p, int pos)
    {
      Control control = new Control();
      control.Parent = (Control) p;
      control.Height = 1;
      int num = p.ClientSize.Height + pos;
      control.Top = num;
      using (Control activeControl = control)
        p.ScrollControlIntoView(activeControl);
    }

    public static void ScrollUp(this Panel p, int pos)
    {
      using (Control activeControl = new Control()
      {
        Parent = (Control) p,
        Height = 1,
        Top = pos
      })
        p.ScrollControlIntoView(activeControl);
    }
  }
}
