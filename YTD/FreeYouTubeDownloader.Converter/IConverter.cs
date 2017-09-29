// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.IConverter
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using FreeYouTubeDownloader.Analyzer;

namespace FreeYouTubeDownloader.Converter
{
  internal interface IConverter
  {
    void Convert(string inputFileName, ConversionProfile conversionProfile, VideoQualityInfo inputVideoQualityInfo);

    void Cancel();
  }
}
