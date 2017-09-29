// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Program
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.UI;
using Google.Measurement;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
    static class Program
    {
        internal static readonly NetworkMonitor NetworkMonitor = new NetworkMonitor();
        private const string AppGuid = "cac59cef-f70f-43e2-80ed-013c89cd7666";
        internal static MainWindow MainWindow;
        private static FreeYouTubeDownloader.Common.NativeMethods.EXECUTION_STATE _previousExecutionState;

        internal static string Version
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
            }
        }

        static Program()
        {
        }

        [STAThread]
        private static void Main(string[] args)
        {
            int num1 = Program.ProcessCommandArgs(args);
            using (new Mutex(false, "Global\\cac59cef-f70f-43e2-80ed-013c89cd7666"))
            {
                FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler)((sender, eventArgs) => Program.ApplyCurrentLocalization(eventArgs.LanguageCode));
                Program.ApplyCurrentLocalization(Settings.Instance.LanguageCode);
                Application.ApplicationExit += new EventHandler(Program.OnApplicationExit);
                Application.ThreadException += new ThreadExceptionEventHandler(Program.OnThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.OnUnhandledException);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                ApplicationManager.MeasurementClient = new MeasurementClient("UA-19260618-4", Guid.NewGuid())
                {
                    UserAgent = string.Format("FYTDL/{0} ({1})", (object)Program.Version, (object)Program.GetUserAgentOSVersion())
                };
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                ToolStripManager.Renderer = (ToolStripRenderer)new ToolStripAeroRenderer(ToolbarTheme.Toolbar);
                Program._previousExecutionState = FreeYouTubeDownloader.Common.NativeMethods.Kernel32.SetThreadExecutionState(FreeYouTubeDownloader.Common.NativeMethods.EXECUTION_STATE.ES_CONTINUOUS | FreeYouTubeDownloader.Common.NativeMethods.EXECUTION_STATE.ES_SYSTEM_REQUIRED);
                MainWindow mainWindow = new MainWindow();
                int num2 = num1 == 1 ? 1 : 0;
                mainWindow.HiddenStart = num2 != 0;
                Program.MainWindow = mainWindow;
                Application.Run((Form)Program.MainWindow);
            }
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            Exception exceptionObject = (Exception)unhandledExceptionEventArgs.ExceptionObject;
            if (Program.NetworkMonitor.Connected)
                ApplicationManager.MeasurementClient.ExceptionAsync(exceptionObject.Message, Application.ProductName, Program.Version, new bool?(true));
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs threadExceptionEventArgs)
        {
            if (Program.NetworkMonitor.Connected)
                ApplicationManager.MeasurementClient.ExceptionAsync(threadExceptionEventArgs.Exception.Message, Application.ProductName, Program.Version, new bool?(true));
        }

        private static void OnApplicationExit(object sender, EventArgs e)
        {
            ApplicationStateManager.Instance.Save();
        }

        private static void ApplyCurrentLocalization(string languageCode)
        {
            CultureInfo cultureInfo = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private static int ProcessCommandArgs(string[] args)
        {
            int num = 0;
            if (args == null || args.Length == 0)
            {
                num = -1;
            }
            else
            {
                string[] strArray1 = args;
                Func<string, bool> func1 = (Func<string, bool>)(arg => string.Equals(arg, "-h", StringComparison.InvariantCultureIgnoreCase));
                Func<string, bool> predicate1 = null;
                if (((IEnumerable<string>)strArray1).Any<string>(predicate1))
                {
                    num = 1;
                }
                else
                {
                    string[] strArray2 = args;
                    Func<string, bool> func2 = (Func<string, bool>)(arg => string.Equals(arg, "-ias", StringComparison.InvariantCultureIgnoreCase));
                    Func<string, bool> predicate2 = null;
                    if (((IEnumerable<string>)strArray2).Any<string>(predicate2))
                    {
                        Settings.MakeStartWithWindows(true);
                        Environment.Exit(0);
                    }
                    else
                    {
                        string[] strArray3 = args;
                        Func<string, bool> func3 = (Func<string, bool>)(arg => string.Equals(arg, "-ras", StringComparison.InvariantCultureIgnoreCase));
                        Func<string, bool> predicate3 = null;
                        if (((IEnumerable<string>)strArray3).Any<string>(predicate3))
                        {
                            Settings.MakeStartWithWindows(false);
                            Environment.Exit(0);
                        }
                    }
                }
            }
            return num;
        }

        private static string GetUserAgentOSVersion()
        {
            switch (Environment.OSVersion.Version.Major)
            {
                case 4:
                case 5:
                    return "Windows NT " + Environment.OSVersion.Version.ToString(2);
                case 6:
                    try
                    {
                        using (RegistryKey registryKey = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, string.Empty).OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion"))
                        {
                            if (registryKey != null)
                            {
                                int? nullable1 = (int?)registryKey.GetValue("CurrentMajorVersionNumber", (object)null);
                                int? nullable2 = (int?)registryKey.GetValue("CurrentMinorVersionNumber", (object)null);
                                if (nullable1.HasValue && nullable2.HasValue)
                                    return string.Format("Windows NT {0}.{1}", (object)nullable1, (object)nullable2);
                                string str = (string)registryKey.GetValue("CurrentVersion", (object)null);
                                if (!string.IsNullOrEmpty(str))
                                    return string.Format("Windows NT {0}", (object)str);
                                goto case 4;
                            }
                            else
                                break;
                        }
                    }
                    catch
                    {
                        goto case 4;
                    }
            }
            return string.Empty;
        }

        internal static void Exit()
        {
            int num = (int)FreeYouTubeDownloader.Common.NativeMethods.Kernel32.SetThreadExecutionState(Program._previousExecutionState);
            Program.NetworkMonitor.StopMonitorInternetConnection();
            Application.Exit();
        }
    }
}
