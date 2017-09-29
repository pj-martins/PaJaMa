// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.ConversionTask
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;
using System;

namespace FreeYouTubeDownloader.Converter
{
  internal class ConversionTask : IConverter
  {
    public event ConversionTask.ProgressChangedEvent ProgressChanged;

    public event ConversionTask.ConversionCompletedEvent ConversionCompleted;

    public virtual void Convert(string inputFileName, ConversionProfile conversionProfile, VideoQualityInfo inputVideoQualityInfo)
    {
      throw new NotImplementedException("ConversionTask.Convert(string, ConversionProfile, VideoQualityInfo)");
    }

    public virtual void Cancel()
    {
      throw new NotImplementedException("ConversionTask.Cancel()");
    }

    protected void RaiseProgressChanged(int progressInPercent)
    {
      // ISSUE: reference to a compiler-generated field
      ConversionTask.ProgressChangedEvent progressChanged = this.ProgressChanged;
      if (progressChanged == null)
        return;
      ProgressChangedEventArgs args = new ProgressChangedEventArgs(progressInPercent);
      progressChanged((object) this, args);
    }

    protected void RaiseConversionCompleted(bool succeeded, bool isCanceled = false, Exception error = null)
    {
      // ISSUE: reference to a compiler-generated field
      ConversionTask.ConversionCompletedEvent conversionCompleted = this.ConversionCompleted;
      if (conversionCompleted == null)
        return;
      ConversionCompletedEventArgs args = new ConversionCompletedEventArgs(succeeded, isCanceled, error);
      conversionCompleted((object) this, args);
    }

    public delegate void ProgressChangedEvent(object sender, ProgressChangedEventArgs args);

    public delegate void ConversionCompletedEvent(object sender, ConversionCompletedEventArgs args);
  }
}
