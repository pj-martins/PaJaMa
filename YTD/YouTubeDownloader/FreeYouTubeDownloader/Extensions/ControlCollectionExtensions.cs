// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Extensions.ControlCollectionExtensions
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.Extensions
{
  public static class ControlCollectionExtensions
  {
    public static void ForEach(this Control.ControlCollection controlCollection, Action<Control> action)
    {
      for (int index = 0; index < controlCollection.Count; ++index)
        action(controlCollection[index]);
    }

    public static void For(this Control.ControlCollection controlCollection, int start, int end, int step, Action<Control> action)
    {
      for (int index = start; index < end; ++index)
        action(controlCollection[index]);
    }

    public static void For(this Control.ControlCollection controlCollection, int start, Func<int, bool> end, int step, Action<Control> action)
    {
      for (int index = start; end(index); ++index)
        action(controlCollection[index]);
    }

    public static List<Control> Where(this Control.ControlCollection controlCollection, Func<Control, bool> condition, bool searchAllChildren)
    {
      List<Control> lstControls = new List<Control>();
      controlCollection.ForEach((Action<Control>) (ctrl =>
      {
        if (searchAllChildren)
          lstControls.AddRange((IEnumerable<Control>) ctrl.Controls.Where(condition, true));
        if (!condition(ctrl))
          return;
        lstControls.Add(ctrl);
      }));
      return lstControls;
    }

    public static List<Control> FindControlsByType(this Control.ControlCollection controlCollection, Type type, bool searchAllChildren)
    {
      List<Control> lstControls = new List<Control>();
      controlCollection.ForEach((Action<Control>) (ctrl =>
      {
        if (searchAllChildren)
          lstControls.AddRange((IEnumerable<Control>) ctrl.Controls.FindControlsByType(type, true));
        if (!(ctrl.GetType() == type))
          return;
        lstControls.Add(ctrl);
      }));
      return lstControls;
    }

    public static List<Control> ToList(this Control.ControlCollection controlCollection, bool includeChildren)
    {
      List<Control> lstControls = new List<Control>();
      controlCollection.ForEach((Action<Control>) (ctrl =>
      {
        if (includeChildren)
          lstControls.AddRange((IEnumerable<Control>) ctrl.Controls.ToList(true));
        lstControls.Add(ctrl);
      }));
      return lstControls;
    }
  }
}
