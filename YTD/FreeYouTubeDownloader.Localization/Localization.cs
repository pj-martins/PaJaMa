// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Localization.Localization
// Assembly: FreeYouTubeDownloader.Localization, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D2556678-3B1C-43D0-B59D-BD461B5CB139
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Localization.dll

using System.Globalization;
using System.Reflection;
using System.Resources;

namespace FreeYouTubeDownloader.Localization
{
    public class Localization
    {
        private readonly ResourceManager _resourceManager;
        private static FreeYouTubeDownloader.Localization.Localization _instance;

        public static FreeYouTubeDownloader.Localization.Localization Instance
        {
            get
            {
                return FreeYouTubeDownloader.Localization.Localization._instance ?? (FreeYouTubeDownloader.Localization.Localization._instance = new FreeYouTubeDownloader.Localization.Localization());
            }
        }

        public event FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler LanguageChanged;

        public Localization()
        {
            this._resourceManager = new ResourceManager("FreeYouTubeDownloader.Localization.Strings", Assembly.GetExecutingAssembly());
        }

        public void SetLanguage(string languageCode)
        {
            this.RaiseLanguageChanged(languageCode);
        }

        private void RaiseLanguageChanged(string languageCode)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.LanguageChanged == null)
                return;
            // ISSUE: reference to a compiler-generated field
            this.LanguageChanged((object)this, new LanguageChangedEventArgs()
            {
                LanguageCode = languageCode
            });
        }

        internal string GetString(string name)
        {
            return this._resourceManager.GetString(name);
        }

        internal string GetString(string name, string countryCode)
        {
            return this._resourceManager.GetString(name, new CultureInfo(countryCode));
        }

        public delegate void LanguageChangedEventHandler(object sender, LanguageChangedEventArgs e);
    }
}
