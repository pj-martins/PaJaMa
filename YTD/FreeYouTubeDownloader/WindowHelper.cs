// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.WindowHelper
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common;
using System;
using System.Drawing;

namespace FreeYouTubeDownloader
{
  public static class WindowHelper
  {
    public const int WM_DWMCOLORIZATIONCOLORCHANGED = 800;

    public static Color GetWindowColorizationColor(bool opaque)
    {
      NativeMethods.DWMCOLORIZATIONPARAMS colorizationParams = new NativeMethods.DWMCOLORIZATIONPARAMS();
      NativeMethods.DwmApi.DwmGetColorizationParameters(ref colorizationParams);
      return Color.FromArgb(opaque ? (int) byte.MaxValue : (int) (byte) (colorizationParams.ColorizationColor >> 24), (int) (byte) (colorizationParams.ColorizationColor >> 16), (int) (byte) (colorizationParams.ColorizationColor >> 8), (int) (byte) colorizationParams.ColorizationColor);
    }

    public static Color GetContrastColor(Color color)
    {
      if (WindowHelper.ComputeL(color) <= Math.Sqrt(21.0 / 400.0) - 0.05)
        return Color.WhiteSmoke;
      return Color.Black;
    }

    private static double ComputeL(Color color)
    {
      return 0.2126 * WindowHelper.DoMathWithColor((uint) color.R) + 447.0 / 625.0 * WindowHelper.DoMathWithColor((uint) color.G) + 0.0722 * WindowHelper.DoMathWithColor((uint) color.B);
    }

    private static double DoMathWithColor(uint color)
    {
      double num = (double) color / (double) byte.MaxValue;
      if (num > 0.03928)
        return Math.Pow((num + 0.055) / 1.055, 2.4);
      return num / 12.92;
    }
  }
}
