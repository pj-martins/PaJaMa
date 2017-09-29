// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Controls.DWebBrowserEvents2
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Runtime.InteropServices;

namespace FreeYouTubeDownloader.Controls
{
  [Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
  [TypeLibType(TypeLibTypeFlags.FHidden)]
  [ComImport]
  public interface DWebBrowserEvents2
  {
    [DispId(271)]
    void NavigateError([MarshalAs(UnmanagedType.IDispatch), In] object pDisp, [In] ref object URL, [In] ref object frame, [In] ref object statusCode, [In, Out] ref bool cancel);

    [DispId(250)]
    void BeforeNavigate2([MarshalAs(UnmanagedType.IDispatch), In] object pDisp, [In] ref object URL, [In] ref object Flags, [In] ref object TargetFrameName, [In] ref object PostData, [In] ref object Headers, [MarshalAs(UnmanagedType.VariantBool), In, Out] ref bool Cancel);

    [DispId(273)]
    void NewWindow3([MarshalAs(UnmanagedType.IDispatch), In, Out] ref object ppDisp, [MarshalAs(UnmanagedType.VariantBool), In, Out] ref bool Cancel, uint dwFlags, [MarshalAs(UnmanagedType.BStr)] string bstrUrlContext, [MarshalAs(UnmanagedType.BStr)] string bstrUrl);
  }
}
