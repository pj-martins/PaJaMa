// Decompiled with JetBrains decompiler
// Type: Google.Measurement.AppEventData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public sealed class AppEventData : EventData
  {
    private readonly AppProperty _appProperty = new AppProperty();

    public string ScreenName
    {
      get
      {
        return this._appProperty.ScreenName;
      }
      set
      {
        this._appProperty.ScreenName = value;
      }
    }

    public string ApplicationName
    {
      get
      {
        return this._appProperty.ApplicationName;
      }
      set
      {
        this._appProperty.ApplicationName = value;
      }
    }

    public string ApplicationVersion
    {
      get
      {
        return this._appProperty.ApplicationVersion;
      }
      set
      {
        this._appProperty.ApplicationVersion = value;
      }
    }

    internal override string GetPayloadData()
    {
      string payloadData = base.GetPayloadData();
      StringBuilder stringBuilder = new StringBuilder(2048);
      if (this._appProperty.ApplicationNameEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&an=");
        stringBuilder.Append(this._appProperty.ApplicationNameEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("\"ApplicationName\" property is required for all hit types sent to app properties");
      if (this._appProperty.ScreenNameEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&cd=");
        stringBuilder.Append(this._appProperty.ScreenNameEncoded);
      }
      if (this._appProperty.ApplicationVersion.NotNullOrEmpty())
      {
        stringBuilder.Append("&av=");
        stringBuilder.Append(this._appProperty.ApplicationVersion);
      }
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
