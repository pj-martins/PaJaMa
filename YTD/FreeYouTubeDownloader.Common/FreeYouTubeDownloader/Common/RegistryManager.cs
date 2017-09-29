// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.RegistryManager
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeYouTubeDownloader.Common
{
  public static class RegistryManager
  {
    public const string RegistrySubKeyAutoRun = "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\";
    public const string RegistrySubKeyApplication = "Software\\Vitzo\\FreeYouTubeDownloader\\";

    public static bool IsValueName(RegistryKey key, string subKey, string valueName)
    {
      bool flag = false;
      RegistryKey registryKey = key.OpenSubKey(subKey, false);
      if (registryKey != null)
        flag = ((IEnumerable<string>) registryKey.GetValueNames()).FirstOrDefault<string>((Func<string, bool>) (value => value.Equals(valueName))) != null;
      return flag;
    }

    public static bool IsValue(RegistryKey key, string subKey, string valueName)
    {
      bool flag = false;
      RegistryKey registryKey = key.OpenSubKey(subKey, false);
      if (registryKey != null)
      {
        flag = registryKey.GetValue(valueName) != null;
        registryKey.Close();
      }
      return flag;
    }

    public static bool SetValue(RegistryKey key, string subKey, string valueName, object value, RegistryValueKind registryValueKind)
    {
      bool flag = false;
      RegistryKey registryKey = key.OpenSubKey(subKey, true) ?? key.CreateSubKey(subKey);
      if (registryKey != null)
      {
        registryKey.SetValue(valueName, value, registryValueKind);
        flag = true;
        registryKey.Close();
      }
      return flag;
    }

    public static T GetValue<T>(RegistryKey key, string subKey, string valueName)
    {
      RegistryKey registryKey = key.OpenSubKey(subKey, false);
      if (registryKey != null)
      {
        object obj = registryKey.GetValue(valueName);
        registryKey.Close();
        if (obj != null)
          return (T) obj;
      }
      return default (T);
    }

    public static bool DeleteValue(RegistryKey key, string subKey, string valueName)
    {
      bool flag = false;
      RegistryKey registryKey = key.OpenSubKey(subKey, true);
      if (registryKey != null)
      {
        if ((string) registryKey.GetValue(valueName) != null)
        {
          registryKey.DeleteValue(valueName);
          flag = true;
        }
        registryKey.Close();
      }
      return flag;
    }
  }
}
