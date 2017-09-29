// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Ads.IAdsHost
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

namespace FreeYouTubeDownloader.Ads
{
  public interface IAdsHost
  {
    void ShowBottomAd(bool createNew = true);

    void HideBottomAd(bool onlyHide = true);

    void ShowRightAd(bool createNew = true);

    void HideRightAd(bool onlyHide = true);
  }
}
