// Decompiled with JetBrains decompiler
// Type: Google.Measurement.ExceptionData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public class ExceptionData : PayloadData
  {
    private readonly AppProperty _appProperty = new AppProperty();
    private const int MaxLengthBytesExceptionDescription = 150;
    private const int MaxLengthBytesApplicationVersion = 100;
    private const string HitTypeParameter = "&t=exception";
    private const string ExceptionDescriptionParameter = "&exd=";
    private const string IsExceptionFatalParameter = "&exf=";
    private const string ApplicationVersionParameter = "&av=";
    private string _exceptionDescription;
    private string _exceptionDescriptionEncoded;
    private string _applicationVersion;

    public string ExceptionDescription
    {
      get
      {
        return this._exceptionDescription;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._exceptionDescription = this._exceptionDescriptionEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 150)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"ExceptionDescription\" property should not exceed of {0} bytes", (object) 150));
          this._exceptionDescriptionEncoded = str;
          this._exceptionDescription = value;
        }
      }
    }

    public bool? IsFatal { get; set; }

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
        return this._applicationVersion;
      }
      set
      {
        if (PayloadData.ThrowExceptions && value.NotNullOrEmpty() && value.Length > 100)
          throw new ArgumentOutOfRangeException("value", string.Format("The length of \"ApplicationVersion\" property should not exceed of {0} bytes", (object) 100));
        this._applicationVersion = value;
      }
    }

    internal override string GetPayloadData()
    {
      string payloadData = base.GetPayloadData();
      StringBuilder stringBuilder = new StringBuilder(2048);
      if (this._exceptionDescriptionEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&exd=");
        stringBuilder.Append(this._exceptionDescriptionEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("The value of \"ExceptionDescription\" must not be empty");
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
      if (this.IsFatal.HasValue)
      {
        stringBuilder.Append("&exf=");
        stringBuilder.Append(this.IsFatal.Value ? 1 : 0);
      }
      if (this._applicationVersion.NotNullOrEmpty())
      {
        stringBuilder.Append("&av=");
        stringBuilder.Append(this._applicationVersion);
      }
      stringBuilder.Append("&t=exception");
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
