// Decompiled with JetBrains decompiler
// Type: Google.Measurement.PageViewData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public sealed class PageViewData : PayloadData
  {
    private readonly WebProperty _webProperty = new WebProperty();
    private const int MaxLengthBytesDocumentReferrer = 2048;
    private const string HitTypeParameter = "&t=pageview";
    private const string DocumentReferrerParameter = "&dr=";
    private string _documentReferrer;
    private string _documentReferrerEncoded;

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

    public string DocumentReferrer
    {
      get
      {
        return this._documentReferrer;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._documentReferrer = this._documentReferrerEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 2048)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentReferrer\" property should not exceed of {0} bytes", (object) 2048));
          this._documentReferrerEncoded = str;
          this._documentReferrer = value;
        }
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
      else if (this._webProperty.DocumentHostNameEncoded.NotNullOrEmpty() && this._webProperty.DocumentPathEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dh=");
        stringBuilder.Append(this._webProperty.DocumentHostNameEncoded);
        stringBuilder.Append("&dp=");
        stringBuilder.Append(this._webProperty.DocumentPathEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("Either \"DocumentLocation\" or both \"DocumentHostName\" and \"DocumentPath\" have to be specified for the hit to be valid");
      stringBuilder.Append("&t=pageview");
      if (this._webProperty.DocumentTitleEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dt=");
        stringBuilder.Append(this._webProperty.DocumentTitleEncoded);
      }
      if (this._documentReferrerEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&dr=");
        stringBuilder.Append(this._documentReferrerEncoded);
      }
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
