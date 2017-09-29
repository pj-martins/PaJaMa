// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.FfmpegWrapper
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using FreeYouTubeDownloader.Common;
using FreeYouTubeDownloader.Common.Collections;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Converter
{
  internal class FfmpegWrapper : ConversionTask
  {
    private static readonly object Locker = new object();
    private const string RegexVideoSize = "(\\d{2,5}[x|X]\\d{2,5})";
    private Process _ffmpeg;
    private VideoQualityInfo _inputVideoQualityInfo;
    private ConversionProfile _conversionProfile;
    private readonly BackgroundWorker _backgroundWorker;
    private readonly string _ffmpegDirectory;
    private string _ffmpegFileName;

    public static bool IsUnix
    {
      get
      {
        int platform = (int) Environment.OSVersion.Platform;
        switch (platform)
        {
          case 4:
          case 6:
            return true;
          default:
            return platform == 128;
        }
      }
    }

    public string VideoSize { get; set; }

    internal ConversionProfile ConversionProfile
    {
      get
      {
        return this._conversionProfile;
      }
    }

    public FfmpegWrapper()
    {
      this._ffmpegDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Free YouTube Downloader");
      this._backgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = true
      };
      this._backgroundWorker.DoWork += new DoWorkEventHandler(this.OnBackgroundWorkerDoWork);
      this._backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.OnBackgroundWorkerProgressChanged);
      this._backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.OnBackgroundWorkerCompleted);
    }

    ~FfmpegWrapper()
    {
      this.EnsureFfmpegTerminated(false, (ConversionProfile) null);
    }

    public override void Convert(string inputFileName, ConversionProfile conversionProfile, VideoQualityInfo inputVideoQualityInfo)
    {
      FreeYouTubeDownloader.Debug.Log.Trace("CALL FfmpegWrapper.Convert(string, ConversionProfile, VideoQualityInfo)", (Exception) null);
      conversionProfile.InputFileName = inputFileName;
      this._inputVideoQualityInfo = inputVideoQualityInfo;
      this._conversionProfile = conversionProfile;
      this._backgroundWorker.RunWorkerAsync((object) conversionProfile);
    }

    public override void Cancel()
    {
      FreeYouTubeDownloader.Debug.Log.Trace("CALL FfmpegWrapper.Cancel()", (Exception) null);
      if (!this._backgroundWorker.WorkerSupportsCancellation || !this._backgroundWorker.IsBusy || this._backgroundWorker.CancellationPending)
        return;
      this._backgroundWorker.CancelAsync();
      FreeYouTubeDownloader.Debug.Log.Debug("FfmpegWrapper => Cancel conversion request", (Exception) null);
    }

    private void OnBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      this.EnsureFfmpegTerminated(e.Cancelled || e.Error != null, this._conversionProfile);
      if (e.Cancelled)
        this.RaiseConversionCompleted(false, true, (Exception) null);
      else if (e.Error != null)
        this.RaiseConversionCompleted(false, false, e.Error);
      else
        this.RaiseConversionCompleted(true, false, (Exception) null);
    }

    private void OnBackgroundWorkerProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
      this.RaiseProgressChanged(e.ProgressPercentage);
    }

    private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      this.EnsureFFMpegLibs();
      ConversionProfile conversionProfile = (ConversionProfile) e.Argument;
      if (!File.Exists(this._ffmpegFileName))
        throw new FileNotFoundException(this._ffmpegFileName + " not found");
      VideoQualityInfo videoQualityInfo = this._inputVideoQualityInfo;
      ProcessStartInfo processStartInfo = new ProcessStartInfo(this._ffmpegFileName, conversionProfile.GetFfmpegCommandArgs(videoQualityInfo))
      {
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardError = true,
        RedirectStandardOutput = true
      };
      this._ffmpeg = new Process()
      {
        StartInfo = processStartInfo
      };
      if (this._backgroundWorker.CancellationPending)
      {
        e.Cancel = true;
      }
      else
      {
        int num = 0;
        this._ffmpeg.Start();
        StreamReader standardError = this._ffmpeg.StandardError;
        bool flag = false;
        LimitedQueue<string> limitedQueue = new LimitedQueue<string>(10);
        do
        {
          string str = standardError.ReadLine();
          limitedQueue.Enqueue(str);
          FreeYouTubeDownloader.Debug.Log.Debug(str, (Exception) null);
          if (str.Contains("Duration: "))
          {
            TimeSpan result;
            if (TimeSpan.TryParse(str.JustAfter("Duration: ", ","), out result))
            {
              num = (int) result.TotalSeconds;
              this._backgroundWorker.ReportProgress(0, (object) num);
            }
          }
          else if (str.Contains("size=") && str.Contains("time="))
          {
            TimeSpan result;
            if (TimeSpan.TryParse(str.JustAfter("time=", "."), out result))
            {
              int totalSeconds = (int) result.TotalSeconds;
              this._backgroundWorker.ReportProgress(totalSeconds <= num ? (num > 0 ? totalSeconds * 100 / num : 0) : 100, (object) num);
            }
          }
          else if (Regex.IsMatch(str, "Stream.+Video:.+(\\d{2,5}[x|X]\\d{2,5})"))
          {
            if (flag)
              this.VideoSize = Regex.Match(str, "(\\d{2,5}[x|X]\\d{2,5})").Value.Replace("x", " x ");
            else
              flag = true;
          }
          if (this._backgroundWorker.CancellationPending)
          {
            e.Cancel = true;
            this._ffmpeg.Kill();
            break;
          }
        }
        while (!standardError.EndOfStream);
        if (this._ffmpeg == null)
          throw new FFMpegException(-1, "FFMpeg process was aborted");
        this._ffmpeg.WaitForExit();
        if (this._ffmpeg.ExitCode != 0 && !this._backgroundWorker.CancellationPending)
        {
          FreeYouTubeDownloader.Debug.Log.Warning(string.Format("FFMPEG failed, the last strings:{0}{1}", (object) Environment.NewLine, (object) limitedQueue.ToString()), (Exception) null);
          throw new FFMpegException(this._ffmpeg.ExitCode, limitedQueue.Last.Value);
        }
        this._ffmpeg.Close();
        this._ffmpeg = (Process) null;
      }
    }

    private void EnsureFfmpegTerminated(bool deleteOutputFile = false, ConversionProfile conversionProfile = null)
    {
      if (this._ffmpeg != null && !this._ffmpeg.HasExited)
      {
        this._ffmpeg.Kill();
        this._ffmpeg.WaitForExit();
        this._ffmpeg.Close();
        this._ffmpeg.Dispose();
        this._ffmpeg = (Process) null;
      }
      if (!deleteOutputFile || conversionProfile == null)
        return;
      FileSystemUtil.SafeDeleteFile(conversionProfile.OutputFileName);
    }

    private void EnsureFFMpegLibs()
    {
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
      {
        if (manifestResourceName.StartsWith("FreeYouTubeDownloader.Converter."))
        {
          this._ffmpegFileName = Path.Combine(this._ffmpegDirectory, Path.GetFileNameWithoutExtension(manifestResourceName.Substring("FreeYouTubeDownloader.Converter.".Length)));
          if (!Directory.Exists(this._ffmpegDirectory))
            Directory.CreateDirectory(this._ffmpegDirectory);
          lock (FfmpegWrapper.Locker)
          {
            if (File.Exists(this._ffmpegFileName))
            {
              if (File.GetLastWriteTime(this._ffmpegFileName) > File.GetLastWriteTime(executingAssembly.Location))
                continue;
            }
            using (GZipStream gzipStream = new GZipStream(executingAssembly.GetManifestResourceStream(manifestResourceName), CompressionMode.Decompress, false))
            {
              using (FileStream fileStream = new FileStream(this._ffmpegFileName, FileMode.Create, FileAccess.Write, FileShare.None))
              {
                byte[] buffer = new byte[65536];
                int count;
                while ((count = gzipStream.Read(buffer, 0, buffer.Length)) > 0)
                  fileStream.Write(buffer, 0, count);
              }
            }
          }
        }
      }
    }

    internal MediaMetadata ObtainMediaMetadata(string inputFile)
    {
      this.EnsureFFMpegLibs();
      if (!File.Exists(this._ffmpegFileName))
        throw new FileNotFoundException(this._ffmpegFileName + " not found");
      ProcessStartInfo processStartInfo = new ProcessStartInfo(this._ffmpegFileName, string.Format("-loglevel 48 -nostdin -i \"{0}\"", (object) inputFile))
      {
        WindowStyle = ProcessWindowStyle.Hidden,
        CreateNoWindow = true,
        UseShellExecute = false,
        RedirectStandardInput = false,
        RedirectStandardOutput = false,
        RedirectStandardError = true
      };
      this._ffmpeg = new Process()
      {
        StartInfo = processStartInfo
      };
      this._ffmpeg.Start();
      StreamReader standardError = this._ffmpeg.StandardError;
      StringBuilder stringBuilder = new StringBuilder();
      do
      {
        string str = standardError.ReadLine();
        stringBuilder.AppendLine(str);
      }
      while (!standardError.EndOfStream);
      return MediaMetadata.FromFFMpegLog(stringBuilder.ToString()).FirstOrDefault<MediaMetadata>();
    }
  }
}
