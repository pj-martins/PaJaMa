// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Localization.LanguageItem
// Assembly: FreeYouTubeDownloader.Localization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2556678-3B1C-43D0-B59D-BD461B5CB139
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Localization.dll

using System.Globalization;

namespace FreeYouTubeDownloader.Localization
{
  public class LanguageItem
  {
    private readonly CultureInfo _cultureInfo;

    public string CountryCode
    {
      get
      {
        return this._cultureInfo.Name;
      }
    }

    public LanguageItem(string languageCode)
    {
      this._cultureInfo = new CultureInfo(languageCode);
    }

    public override string ToString()
    {
      return this._cultureInfo.NativeName;
    }
  }
}
