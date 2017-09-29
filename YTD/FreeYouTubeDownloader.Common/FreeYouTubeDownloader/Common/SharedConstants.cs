// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.SharedConstants
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.IO;

namespace FreeYouTubeDownloader.Common
{
  public static class SharedConstants
  {
    public static readonly string SerializedDownloadsFileName;
    public static readonly string SettingsFileName;
    public const string ApplicationName = "Free YouTube Downloader";
    public const string TempFileNamePrefix = "fytdl_";
    public const string CompanyName = "Vitzo";
    public const string LogFileName = "log.txt";
    public const string SupportUrl = "https://youtubedownloader.com/support";

    static SharedConstants()
    {
      string path = string.Format("{0}{1}{2}", (object) Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), (object) Path.DirectorySeparatorChar, (object) "Free YouTube Downloader");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      SharedConstants.SerializedDownloadsFileName = string.Format("{0}{1}{2}", (object) path, (object) Path.DirectorySeparatorChar, (object) "Downloads.data");
      SharedConstants.SettingsFileName = string.Format("{0}{1}{2}", (object) path, (object) Path.DirectorySeparatorChar, (object) "Settings.data");
    }
  }
}
