// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.WBEmulator
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using Microsoft.Win32;
using System;
using System.IO;
using System.Security;

namespace FreeYouTubeDownloader
{
  public static class WBEmulator
  {
    private const string InternetExplorerRootKey = "Software\\Microsoft\\Internet Explorer";
    private const string BrowserEmulationKey = "Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";

    public static int GetInternetExplorerMajorVersion()
    {
      int result = 0;
      try
      {
        RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Internet Explorer");
        if (registryKey != null)
        {
          object obj = registryKey.GetValue("svcVersion", (object) null) ?? registryKey.GetValue("Version", (object) null);
          if (obj != null)
          {
            string str = obj.ToString();
            int length = str.IndexOf('.');
            if (length != -1)
              int.TryParse(str.Substring(0, length), out result);
          }
        }
      }
      catch (SecurityException ex)
      {
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      return result;
    }

    public static BrowserEmulationVersion GetBrowserEmulationVersion()
    {
      BrowserEmulationVersion emulationVersion = BrowserEmulationVersion.Default;
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
        if (registryKey != null)
        {
          string fileName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
          object obj = registryKey.GetValue(fileName, (object) null);
          if (obj != null)
            emulationVersion = (BrowserEmulationVersion) Convert.ToInt32(obj);
        }
      }
      catch (SecurityException ex)
      {
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      return emulationVersion;
    }

    public static bool SetBrowserEmulationVersion(BrowserEmulationVersion browserEmulationVersion)
    {
      bool flag = false;
      try
      {
        RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", true);
        if (registryKey != null)
        {
          string fileName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
          if (browserEmulationVersion != BrowserEmulationVersion.Default)
            registryKey.SetValue(fileName, (object) browserEmulationVersion, RegistryValueKind.DWord);
          else
            registryKey.DeleteValue(fileName, false);
          flag = true;
        }
      }
      catch (SecurityException ex)
      {
      }
      catch (UnauthorizedAccessException ex)
      {
      }
      return flag;
    }

    public static bool SetBrowserEmulationVersion()
    {
      int explorerMajorVersion = WBEmulator.GetInternetExplorerMajorVersion();
      BrowserEmulationVersion browserEmulationVersion;
      if (explorerMajorVersion >= 11)
      {
        browserEmulationVersion = BrowserEmulationVersion.Version11;
      }
      else
      {
        switch (explorerMajorVersion - 8)
        {
          case 0:
            browserEmulationVersion = BrowserEmulationVersion.Version8;
            break;
          case 1:
            browserEmulationVersion = BrowserEmulationVersion.Version9;
            break;
          case 2:
            browserEmulationVersion = BrowserEmulationVersion.Version10;
            break;
          default:
            browserEmulationVersion = BrowserEmulationVersion.Version7;
            break;
        }
      }
      return WBEmulator.SetBrowserEmulationVersion(browserEmulationVersion);
    }

    public static bool IsBrowserEmulationSet()
    {
      return (uint) WBEmulator.GetBrowserEmulationVersion() > 0U;
    }
  }
}
