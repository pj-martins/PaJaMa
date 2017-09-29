// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Search.TimerEx
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Runtime;
using System.Timers;

namespace FreeYouTubeDownloader.Search
{
  public class TimerEx : Timer
  {
    public string Keyword { get; set; }

    public bool IsIgnored { get; set; }

    public TimerEx()
    {
    }

    public TimerEx(double interval)
      : base(interval)
    {
    }

    [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
    public void Start(string keyword)
    {
      if (this.IsIgnored)
        return;
      this.Keyword = string.Copy(keyword);
      this.Start();
    }
  }
}
