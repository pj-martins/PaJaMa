// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.NativeMethods
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Runtime.InteropServices;

namespace FreeYouTubeDownloader.Common
{
  internal static class NativeMethods
  {
    internal static class Kernel32
    {
      [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
      public static extern NativeMethods.EXECUTION_STATE SetThreadExecutionState(NativeMethods.EXECUTION_STATE esFlags);
    }

    internal static class User32
    {
      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

      [DllImport("user32.dll", SetLastError = true)]
      public static extern bool SetProcessDPIAware();

      [DllImport("user32.dll")]
      public static extern IntPtr GetDC(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDC);

      [DllImport("user32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool AddClipboardFormatListener(IntPtr hwnd);

      [DllImport("user32.dll", SetLastError = true)]
      [return: MarshalAs(UnmanagedType.Bool)]
      public static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

      [DllImport("user32.dll")]
      public static extern uint GetClipboardSequenceNumber();
    }

    internal static class Gdi32
    {
      [DllImport("gdi32.dll")]
      public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
    }

    internal static class DwmApi
    {
      [DllImport("dwmapi.dll", EntryPoint = "#127")]
      internal static extern void DwmGetColorizationParameters(ref NativeMethods.DWMCOLORIZATIONPARAMS colorizationParams);
    }

    [Flags]
    public enum EXECUTION_STATE : uint
    {
      ES_AWAYMODE_REQUIRED = 64,
      ES_CONTINUOUS = 2147483648,
      ES_DISPLAY_REQUIRED = 2,
      ES_SYSTEM_REQUIRED = 1,
    }

    public enum DeviceCap
    {
      DRIVERVERSION = 0,
      TECHNOLOGY = 2,
      HORZSIZE = 4,
      VERTSIZE = 6,
      HORZRES = 8,
      VERTRES = 10,
      BITSPIXEL = 12,
      PLANES = 14,
      NUMBRUSHES = 16,
      NUMPENS = 18,
      NUMMARKERS = 20,
      NUMFONTS = 22,
      NUMCOLORS = 24,
      PDEVICESIZE = 26,
      CURVECAPS = 28,
      LINECAPS = 30,
      POLYGONALCAPS = 32,
      TEXTCAPS = 34,
      CLIPCAPS = 36,
      RASTERCAPS = 38,
      ASPECTX = 40,
      ASPECTY = 42,
      ASPECTXY = 44,
      SHADEBLENDCAPS = 45,
      LOGPIXELSX = 88,
      LOGPIXELSY = 90,
      SIZEPALETTE = 104,
      NUMRESERVED = 106,
      COLORRES = 108,
      PHYSICALWIDTH = 110,
      PHYSICALHEIGHT = 111,
      PHYSICALOFFSETX = 112,
      PHYSICALOFFSETY = 113,
      SCALINGFACTORX = 114,
      SCALINGFACTORY = 115,
      VREFRESH = 116,
      DESKTOPVERTRES = 117,
      DESKTOPHORZRES = 118,
      BLTALIGNMENT = 119,
    }

    public struct DWMCOLORIZATIONPARAMS
    {
      public uint ColorizationColor;
      public uint ColorizationAfterglow;
      public uint ColorizationColorBalance;
      public uint ColorizationAfterglowBalance;
      public uint ColorizationBlurBalance;
      public uint ColorizationGlassReflectionIntensity;
      public uint ColorizationOpaqueBlend;
    }
  }
}
