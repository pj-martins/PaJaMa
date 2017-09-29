// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.FileMergeTask
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Common;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;

namespace FreeYouTubeDownloader.Converter
{
  public class FileMergeTask
  {
    private static readonly object Locker = new object();
    private Process _ffmpeg;
    private FileMergeTaskParameters _fileMergeTaskParameters;
    private AutoResetEvent _waitHandle;
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

    public Exception Error { get; private set; }

    public event FileMergeTask.ProgressChangedEvent ProgressChanged;

    public event FileMergeTask.ConversionCompletedEvent ConversionCompleted;

    public FileMergeTask()
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

    ~FileMergeTask()
    {
      this.EnsureFfmpegTerminated(false, (FileMergeTaskParameters) null);
    }

    public void Merge(string[] input, string output)
    {
      FreeYouTubeDownloader.Debug.Log.Trace("CALL FileMergeTask.Merge(string[], string)", (Exception) null);
      this._waitHandle = new AutoResetEvent(false);
      this.Error = (Exception) null;
      this._fileMergeTaskParameters = new FileMergeTaskParameters()
      {
        Input = input,
        Output = output
      };
      this._backgroundWorker.RunWorkerAsync((object) this._fileMergeTaskParameters);
      this._waitHandle.WaitOne();
    }

    public void Cancel()
    {
      FreeYouTubeDownloader.Debug.Log.Trace("CALL FileMergeTask.Cancel()", (Exception) null);
      if (!this._backgroundWorker.WorkerSupportsCancellation || !this._backgroundWorker.IsBusy || this._backgroundWorker.CancellationPending)
        return;
      this._backgroundWorker.CancelAsync();
      FreeYouTubeDownloader.Debug.Log.Debug("FfmpegWrapper => Cancel conversion request", (Exception) null);
    }

    protected void RaiseProgressChanged(int progressInPercent)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.ProgressChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ProgressChanged((object) this, new ProgressChangedEventArgs(progressInPercent));
    }

    protected void RaiseConversionCompleted(bool succeeded, bool isCanceled = false, Exception error = null)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.ConversionCompleted == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.ConversionCompleted((object) this, new ConversionCompletedEventArgs(succeeded, isCanceled, error));
    }

    private void OnBackgroundWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      FreeYouTubeDownloader.Debug.Log.Trace("FileMergeTask.OnBackgroundWorkerCompleted(object, RunWorkerCompletedEventArgs)", (Exception) null);
      this.Error = e.Error;
      if (this.Error != null)
        FreeYouTubeDownloader.Debug.Log.Error("FileMergeTask.OnBackgroundWorkerCompleted => ", this.Error);
      this.EnsureFfmpegTerminated(!e.Cancelled || e.Error == null, this._fileMergeTaskParameters);
      if (e.Cancelled)
        this.RaiseConversionCompleted(false, true, (Exception) null);
      else if (e.Error != null)
        this.RaiseConversionCompleted(false, false, e.Error);
      else
        this.RaiseConversionCompleted(true, false, (Exception) null);
      this._waitHandle.Set();
    }

    private void OnBackgroundWorkerProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
    {
      this.RaiseProgressChanged(e.ProgressPercentage);
    }

    private void OnBackgroundWorkerDoWork(object sender, DoWorkEventArgs e)
    {
      FreeYouTubeDownloader.Debug.Log.Trace("FileMergeTask.OnBackgroundWorkerDoWork(object, DoWorkEventArgs)", (Exception) null);
      this.EnsureFFMpegLibs();
      FileMergeTaskParameters mergeTaskParameters = (FileMergeTaskParameters) e.Argument;
      if (!File.Exists(this._ffmpegFileName))
        throw new FileNotFoundException(this._ffmpegFileName + " not found");
      ProcessStartInfo processStartInfo = new ProcessStartInfo(this._ffmpegFileName, mergeTaskParameters.GetCommandLine())
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
        FreeYouTubeDownloader.Debug.Log.Info("FileMergeTask.OnBackgroundWorkerDoWork => ffmpeg.exe has started", (Exception) null);
        string message;
        do
        {
          string str = message = standardError.ReadLine();
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
          else
          {
            TimeSpan result;
            if (str.Contains("size=") && str.Contains("time=") && TimeSpan.TryParse(str.JustAfter("time=", "."), out result))
            {
              int totalSeconds = (int) result.TotalSeconds;
              this._backgroundWorker.ReportProgress(totalSeconds <= num ? (num > 0 ? totalSeconds * 100 / num : 0) : 100, (object) num);
            }
          }
          if (this._backgroundWorker.CancellationPending)
          {
            e.Cancel = true;
            break;
          }
        }
        while (!standardError.EndOfStream);
        FreeYouTubeDownloader.Debug.Log.Info("FileMergeTask.OnBackgroundWorkerDoWork => ffmpeg.exe has ended", (Exception) null);
        if (this._ffmpeg == null)
          throw new FFMpegException(-1, "FFMpeg process was aborted");
        if (this._ffmpeg.ExitCode != 0)
        {
          FreeYouTubeDownloader.Debug.Log.Warning(string.Format("FFMPEG failed, last known string \"{0}\"", (object) message), (Exception) null);
          throw new FFMpegException(this._ffmpeg.ExitCode, message);
        }
        this._ffmpeg.Close();
        this._ffmpeg = (Process) null;
      }
    }

    private void EnsureFfmpegTerminated(bool deleteInputFiles, FileMergeTaskParameters parameters)
    {
      if (this._ffmpeg != null && !this._ffmpeg.HasExited)
      {
        this._ffmpeg.Kill();
        this._ffmpeg.WaitForExit();
        this._ffmpeg.Close();
        this._ffmpeg.Dispose();
        this._ffmpeg = (Process) null;
      }
      if (!deleteInputFiles || parameters == null)
        return;
      foreach (string path in parameters.Input)
        FileSystemUtil.SafeDeleteFile(path);
    }

    private void EnsureFFMpegLibs()
    {
      FreeYouTubeDownloader.Debug.Log.Trace("FileMergeTask.EnsureFFMpegLibs()", (Exception) null);
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      foreach (string manifestResourceName in executingAssembly.GetManifestResourceNames())
      {
        if (manifestResourceName.StartsWith("FreeYouTubeDownloader.Converter."))
        {
          this._ffmpegFileName = Path.Combine(this._ffmpegDirectory, Path.GetFileNameWithoutExtension(manifestResourceName.Substring("FreeYouTubeDownloader.Converter.".Length)));
          if (!Directory.Exists(this._ffmpegDirectory))
            Directory.CreateDirectory(this._ffmpegDirectory);
          lock (FileMergeTask.Locker)
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

    public delegate void ProgressChangedEvent(object sender, ProgressChangedEventArgs args);

    public delegate void ConversionCompletedEvent(object sender, ConversionCompletedEventArgs args);
  }
}
