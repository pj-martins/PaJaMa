// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Types.ButtonActionData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

namespace FreeYouTubeDownloader.Common.Types
{
  public struct ButtonActionData
  {
    public FreeYouTubeDownloader.Common.DesiredAction? DesiredAction { get; set; }

    public object DataQuality { get; set; }

    public ButtonActionData(FreeYouTubeDownloader.Common.DesiredAction besiredAction, object dataQuality)
    {
      this = new ButtonActionData();
      this.DesiredAction = new FreeYouTubeDownloader.Common.DesiredAction?(besiredAction);
      this.DataQuality = dataQuality;
    }
  }
}
