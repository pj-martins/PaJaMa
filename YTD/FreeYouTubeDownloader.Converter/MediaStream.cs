// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.MediaStream
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using System;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Converter
{
  public class MediaStream
  {
    private static readonly Regex StreamTypeRegex = new Regex("(?<type>Video|Audio)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex StreamDimensionRegex = new Regex("(?<dimension>(\\d+){3}x(\\d+){2})", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

    public bool IsVideo { get; private set; }

    public string Dimension { get; private set; }

    public int? Width { get; private set; }

    public int? Height { get; private set; }

    public static MediaStream FromFFMpegLog(string ffmpegLog)
    {
      MediaStream mediaStream1 = new MediaStream();
      int num = MediaStream.StreamTypeRegex.Match(ffmpegLog).Value.ToLowerInvariant() == "video" ? 1 : 0;
      mediaStream1.IsVideo = num != 0;
      MediaStream mediaStream2 = mediaStream1;
      if (mediaStream2.IsVideo)
      {
        mediaStream2.Dimension = MediaStream.StreamDimensionRegex.Match(ffmpegLog).Value;
        string[] strArray = mediaStream2.Dimension.Split('x');
        mediaStream2.Width = new int?(Convert.ToInt32(strArray[0]));
        mediaStream2.Height = new int?(Convert.ToInt32(strArray[1]));
      }
      return mediaStream2;
    }
  }
}
