// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Converter.MediaMetadata
// Assembly: FreeYouTubeDownloader.Converter, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EE873E4D-BE5F-4D44-8042-B4B08F380794
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Converter.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Converter
{
  public class MediaMetadata
  {
    private static readonly Regex InputSectionRegex = new Regex("Input #(?<input>.*?)Successfully opened the file", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex DurationRegex = new Regex("Duration:\\s(?<duration>[0-9:.]+)([,]|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex StreamSectionRegex = new Regex("Stream #\\d:\\d(?<stream>.*?)\\n", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
    private static readonly Regex FileNameRegex = new Regex("from '(?<inputfile>.*?)':", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);

    internal string FileName { get; private set; }

    internal TimeSpan Duration { get; private set; }

    internal List<MediaStream> Streams { get; private set; }

    internal static List<MediaMetadata> FromFFMpegLog(string ffmpegLog)
    {
      MatchCollection matchCollection = MediaMetadata.InputSectionRegex.Matches(ffmpegLog);
      List<MediaMetadata> mediaMetadataList = new List<MediaMetadata>(matchCollection.Count);
      foreach (Match match in matchCollection)
      {
        string input = match.Groups["input"].Value;
        MediaMetadata mediaMetadata = new MediaMetadata()
        {
          FileName = MediaMetadata.FileNameRegex.Match(input).Value,
          Duration = TimeSpan.Parse(MediaMetadata.DurationRegex.Match(input).Groups["duration"].Value)
        };
        MatchCollection source = MediaMetadata.StreamSectionRegex.Matches(input);
        mediaMetadata.Streams = new List<MediaStream>(source.Count);
        foreach (string ffmpegLog1 in source.Cast<Match>().Select<Match, string>((Func<Match, string>) (streamSectionMatch => streamSectionMatch.Groups["stream"].Value)))
          mediaMetadata.Streams.Add(MediaStream.FromFFMpegLog(ffmpegLog1));
        mediaMetadataList.Add(mediaMetadata);
      }
      return mediaMetadataList;
    }
  }
}
