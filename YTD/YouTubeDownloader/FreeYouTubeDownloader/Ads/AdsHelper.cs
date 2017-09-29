// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Ads.AdsHelper
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.Ads
{
  public static class AdsHelper
  {
    public const int CloseButtonShowInterval = 1000;
    public const int RefreshAdInterval = 604800000;
    public const string BottomAdUrl = "https://youtubedownloader.com/wd/ads/728x90.html";
    public const string RightAdUrl = "https://youtubedownloader.com/wd/ads/160x600.html";

    public static IAdsHost AdsHost { get; private set; }

    public static void Init(IAdsHost adsHost)
    {
      AdsHelper.AdsHost = adsHost;
    }

    public static void ShowBottomAd()
    {
      AdsHelper.AdsHost.ShowBottomAd(true);
    }

    public static void HideBottomAd()
    {
      AdsHelper.AdsHost.HideBottomAd(true);
    }

    public static void ShowRightAd()
    {
      AdsHelper.AdsHost.ShowRightAd(true);
    }

    public static void HideRightAd()
    {
      AdsHelper.AdsHost.HideRightAd(true);
    }

    public static PictureBox CreateCloseButton(int left, int top)
    {
      PictureBox pictureBox = new PictureBox();
      pictureBox.Image = (Image) Resources.close_ad;
      int num1 = left;
      pictureBox.Left = num1;
      int num2 = top;
      pictureBox.Top = num2;
      int num3 = 16;
      pictureBox.Width = num3;
      int num4 = 16;
      pictureBox.Height = num4;
      Cursor hand = Cursors.Hand;
      pictureBox.Cursor = hand;
      Color whiteSmoke = Color.WhiteSmoke;
      pictureBox.BackColor = whiteSmoke;
      int num5 = 0;
      pictureBox.Visible = num5 != 0;
      return pictureBox;
    }
  }
}
