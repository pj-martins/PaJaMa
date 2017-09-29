// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.GeneralHelper
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public static class GeneralHelper
  {
    public static void ExecuteOnUIThread(this Form form, Action action)
    {
      if (!form.IsHandleCreated)
        return;
      form.BeginInvoke((Delegate) action);
    }
  }
}
