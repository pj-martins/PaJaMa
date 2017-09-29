// Decompiled with JetBrains decompiler
// Type: Google.Measurement.ScreenViewData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public sealed class ScreenViewData : PayloadData
  {
    private readonly AppProperty _appProperty = new AppProperty();
    private const int MaxLengthBytesApplicationId = 150;
    private const int MaxLengthBytesApplicationInstallerId = 150;
    private const string HitTypeParameter = "&t=screenview";
    private const string ApplicationIdParameter = "&aid=";
    private const string ApplicationInstallerIdParameter = "&aiid=";
    private string _applicationId;
    private string _applicationInstallerId;

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

    public string ApplicationId
    {
      get
      {
        return this._applicationId;
      }
      set
      {
        if (PayloadData.ThrowExceptions && value.NotNullOrEmpty() && value.Length > 150)
          throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationId\" property should not exceed of {0} bytes", (object) 150));
        this._applicationId = value;
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

    public string ApplicationInstallerId
    {
      get
      {
        return this._applicationInstallerId;
      }
      set
      {
        if (PayloadData.ThrowExceptions && value.NotNullOrEmpty() && value.Length > 150)
          throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationInstallerId\" property should not exceed of {0} bytes", (object) 150));
        this._applicationInstallerId = value;
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
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("\"ScreenName\" property is required on mobile properties for screenview hits");
      stringBuilder.Append("&t=screenview");
      if (this._appProperty.ApplicationVersion.NotNullOrEmpty())
      {
        stringBuilder.Append("&av=");
        stringBuilder.Append(this._appProperty.ApplicationVersion);
      }
      if (this._applicationId.NotNullOrEmpty())
      {
        stringBuilder.Append("&aid=");
        stringBuilder.Append(this._applicationId);
      }
      if (this._applicationInstallerId.NotNullOrEmpty())
      {
        stringBuilder.Append("&aiid=");
        stringBuilder.Append(this._applicationInstallerId);
      }
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
