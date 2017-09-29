// Decompiled with JetBrains decompiler
// Type: Google.Measurement.AppProperty
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;

namespace Google.Measurement
{
  internal class AppProperty
  {
    private const int MaxLengthBytesScreenName = 2048;
    private const int MaxLengthBytesApplicationName = 100;
    private const int MaxLengthBytesApplicationVersion = 100;
    internal const string ScreenNameParameter = "&cd=";
    internal const string ApplicationNameParameter = "&an=";
    internal const string ApplicationVersionParameter = "&av=";
    private string _screenName;
    private string _screenNameEncoded;
    private string _applicationName;
    private string _applicationNameEncoded;
    private string _applicationVersion;

    internal string ScreenName
    {
      get
      {
        return this._screenName;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._screenName = this._screenNameEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 2048)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"ScreenName\" property should not exceed of {0} bytes", (object) 2048));
          this._screenNameEncoded = str;
          this._screenName = value;
        }
      }
    }

    internal string ScreenNameEncoded
    {
      get
      {
        return this._screenNameEncoded;
      }
    }

    internal string ApplicationName
    {
      get
      {
        return this._applicationName;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._applicationName = this._applicationNameEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 100)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"ApplicationName\" property should not exceed of {0} bytes", (object) 100));
          this._applicationNameEncoded = str;
          this._applicationName = value;
        }
      }
    }

    internal string ApplicationNameEncoded
    {
      get
      {
        return this._applicationNameEncoded;
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
  }
}
