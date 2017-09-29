// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.ProgressChangedEventArgs
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

namespace FreeYouTubeDownloader.Converter
{
  public class ProgressChangedEventArgs
  {
    public int ProgressInPercent { get; set; }

    public ProgressChangedEventArgs(int progressInPercent)
    {
      this.ProgressInPercent = progressInPercent;
    }
  }
}
