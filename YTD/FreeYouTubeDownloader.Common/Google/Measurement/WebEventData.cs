// Decompiled with JetBrains decompiler
// Type: Google.Measurement.WebEventData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System.Text;

namespace Google.Measurement
{
  public sealed class WebEventData : EventData
  {
    private readonly WebProperty _webProperty = new WebProperty();

    public string DocumentLocation
    {
      get
      {
        return this._webProperty.DocumentLocation;
      }
      set
      {
        this._webProperty.DocumentLocation = value;
      }
    }

    public string DocumentHostName
    {
      get
      {
        return this._webProperty.DocumentHostName;
      }
      set
      {
        this._webProperty.DocumentHostName = value;
      }
    }

    public string DocumentPath
    {
      get
      {
        return this._webProperty.DocumentPath;
      }
      set
      {
        this._webProperty.DocumentPath = value;
      }
    }

    public string DocumentTitle
    {
      get
      {
        return this._webProperty.DocumentTitle;
      }
      set
      {
        this._webProperty.DocumentTitle = value;
      }
    }

    internal override string GetPayloadData()
    {
      string payloadData = base.GetPayloadData();
      StringBuilder stringBuilder = new StringBuilder(2048);
      if (this._webProperty.DocumentLocationEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dl=");
        stringBuilder.Append(this._webProperty.DocumentLocationEncoded);
      }
      if (this._webProperty.DocumentHostNameEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dh=");
        stringBuilder.Append(this._webProperty.DocumentHostNameEncoded);
      }
      if (this._webProperty.DocumentPathEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dp=");
        stringBuilder.Append(this._webProperty.DocumentPathEncoded);
      }
      if (this._webProperty.DocumentTitleEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dt=");
        stringBuilder.Append(this._webProperty.DocumentTitleEncoded);
      }
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
