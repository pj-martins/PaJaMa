// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Debug.Log
// Assembly: FreeYouTubeDownloader.Debug, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0983BC95-B55A-44DE-AED0-739C8DD882F6
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Debug.dll

using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections;
using System.Text;

namespace FreeYouTubeDownloader.Debug
{
  public static class Log
  {
    private const string Asterisk = "*";
    private const string TargetNameFile = "file";
    private const string TargetNameDebugger = "debugger";
    private static readonly Logger Logger;
    private static LogLevel _logLevel;
    private const string DefaultOutputFormat = "[${level:upperCase=true}] ${longdate} > ${message}${onexception:${newline}  ${exception:format=Type}${newline}  ${exception:format=Message}${newline}  ${exception:format=StackTrace}}";

    static Log()
    {
      LogManager.Configuration = new LoggingConfiguration();
      Log.Logger = LogManager.GetCurrentClassLogger();
    }

    private static bool HasLogLevel(LogLevel logLevel)
    {
      return Log._logLevel.HasFlag((Enum) logLevel);
    }

    public static void SetLogLevel(LogLevel logLevel)
    {
      Log._logLevel = logLevel;
    }

    public static void AddFileTarget(string fileName, bool keepFileOpen)
    {
      if (LogManager.Configuration.FindTargetByName("file") != null)
        return;
      FileTarget fileTarget1 = new FileTarget();
      Layout layout1 = (Layout) "[${level:upperCase=true}] ${longdate} > ${message}${onexception:${newline}  ${exception:format=Type}${newline}  ${exception:format=Message}${newline}  ${exception:format=StackTrace}}";
      fileTarget1.Layout = layout1;
      Layout layout2 = (Layout) fileName;
      fileTarget1.FileName = layout2;
      int num = keepFileOpen ? 1 : 0;
      fileTarget1.KeepFileOpen = num != 0;
      FileTarget fileTarget2 = fileTarget1;
      LogManager.Configuration.AddTarget("file", (Target) fileTarget2);
      LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, (Target) fileTarget2));
      LogManager.ReconfigExistingLoggers();
    }

    public static void AddDebuggerTarget()
    {
      if (LogManager.Configuration.FindTargetByName("debugger") != null)
        return;
      DebuggerTarget debuggerTarget1 = new DebuggerTarget();
      Layout layout = (Layout) "[${level:upperCase=true}] ${longdate} > ${message}${onexception:${newline}  ${exception:format=Type}${newline}  ${exception:format=Message}${newline}  ${exception:format=StackTrace}}";
      debuggerTarget1.Layout = layout;
      DebuggerTarget debuggerTarget2 = debuggerTarget1;
      LogManager.Configuration.AddTarget("debugger", (Target) debuggerTarget2);
      LogManager.Configuration.LoggingRules.Add(new LoggingRule("*", NLog.LogLevel.Trace, (Target) debuggerTarget2));
      LogManager.ReconfigExistingLoggers();
    }

    public static void Trace(string message, Exception exception = null)
    {
      if (!Log.HasLogLevel(LogLevel.Trace))
        return;
      if (exception == null)
        Log.Logger.Trace(message);
      else
        Log.Logger.TraceException(message, exception);
    }

    public static void Debug(string message, Exception exception = null)
    {
      if (!Log.HasLogLevel(LogLevel.Debug))
        return;
      if (exception == null)
        Log.Logger.Debug(message);
      else
        Log.Logger.DebugException(message, exception);
    }

    public static void Info(string message, Exception exception = null)
    {
      if (!Log.HasLogLevel(LogLevel.Info))
        return;
      if (exception == null)
        Log.Logger.Info(message);
      else
        Log.Logger.InfoException(message, exception);
    }

    public static void Warning(string message, Exception exception = null)
    {
      if (!Log.HasLogLevel(LogLevel.Warning))
        return;
      if (exception == null)
        Log.Logger.Warn(message);
      else
        Log.Logger.WarnException(message, exception);
    }

    public static void Error(string message, Exception exception = null)
    {
      if (!Log.HasLogLevel(LogLevel.Error))
        return;
      if (exception == null)
        Log.Logger.Error(message);
      else
        Log.Logger.ErrorException(message, exception);
    }

    public static void Fatal(string message, Exception exception = null)
    {
      if (!Log.HasLogLevel(LogLevel.Fatal))
        return;
      if (exception == null)
        Log.Logger.Fatal(message);
      else
        Log.Logger.FatalException(message, exception);
    }

    public static string FormatException(Exception exception)
    {
      string infoFromException = Log.GetAdditionalInfoFromException(exception);
      return string.Format("{0}" + Environment.NewLine + "{1}" + Environment.NewLine + "{2}" + Environment.NewLine + "{3}" + Environment.NewLine + "{4}" + Environment.NewLine, (object) DateTime.Now.ToLocalTime(), (object) exception.Message, (object) infoFromException, (object) exception.GetType(), (object) exception.StackTrace);
    }

    public static string GetAdditionalInfoFromException(Exception exception)
    {
      if (exception == null || exception.Data.Count == 0)
        return "No additional exception info";
      StringBuilder stringBuilder = new StringBuilder();
      foreach (DictionaryEntry dictionaryEntry in exception.Data)
        stringBuilder.AppendLine(string.Format("{0}:   {1}", dictionaryEntry.Key, dictionaryEntry.Value));
      return stringBuilder.ToString();
    }
  }
}
