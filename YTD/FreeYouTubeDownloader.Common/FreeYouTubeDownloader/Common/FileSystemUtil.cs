// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.FileSystemUtil
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FreeYouTubeDownloader.Common
{
  public class FileSystemUtil
  {
    public static long GetTotalFreeSpace(string filePath)
    {
      string driveName = Path.GetPathRoot(filePath);
      if (string.IsNullOrEmpty(driveName))
        return long.MaxValue;
      using (IEnumerator<DriveInfo> enumerator = ((IEnumerable<DriveInfo>) DriveInfo.GetDrives()).Where<DriveInfo>((Func<DriveInfo, bool>) (drive =>
      {
        if (drive.IsReady)
          return drive.Name == driveName;
        return false;
      })).GetEnumerator())
      {
        if (enumerator.MoveNext())
          return enumerator.Current.TotalFreeSpace;
      }
      return -1;
    }

    public static void SafeDeleteFile(string path)
    {
      try
      {
        File.Delete(path);
      }
      catch
      {
      }
    }

    public static string GetTempFileName(string extension = null, string basePath = null)
    {
      return Path.Combine(basePath ?? FileSystemUtil.GetApplicationTempFolderPath(), FileSystemUtil.CreateFileName(extension));
    }

    public static string GetApplicationTempFolderPath()
    {
      string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Free YouTube Downloader", "Temp");
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    internal static int GetIndexForFileNameIfSameFileAlreadyExists(string fileName, int index = 1)
    {
      if (File.Exists(Path.Combine(Path.GetDirectoryName(fileName), string.Format("{0} ({1}){2}", (object) Path.GetFileNameWithoutExtension(fileName), (object) index, (object) Path.GetExtension(fileName)))))
        return FileSystemUtil.GetIndexForFileNameIfSameFileAlreadyExists(fileName, ++index);
      return index;
    }

    protected static string CreateFileName(string extension)
    {
      string str = Guid.NewGuid().ToString().Replace("-", string.Empty);
      return !string.IsNullOrWhiteSpace(extension) ? string.Format("{0}.{1}", (object) str, (object) extension) : string.Format("{0}.tmp", (object) str);
    }
  }
}
