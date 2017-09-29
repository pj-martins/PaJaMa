// Decompiled with JetBrains decompiler
// Type: Google.Measurement.WebProperty
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;

namespace Google.Measurement
{
  internal class WebProperty
  {
    private const int MaxLengthBytesDocumentLocation = 2048;
    private const int MaxLengthBytesDocumentHostName = 100;
    private const int MaxLengthBytesDocumentPath = 2048;
    private const int MaxLengthBytesDocumentTitle = 1500;
    internal const string DocumentLocationParameter = "&dl=";
    internal const string DocumentHostNameParameter = "&dh=";
    internal const string DocumentPathParameter = "&dp=";
    internal const string DocumentTitleParameter = "&dt=";
    private string _documentLocation;
    private string _documentLocationEncoded;
    private string _documentHostName;
    private string _documentHostNameEncoded;
    private string _documentPath;
    private string _documentPathEncoded;
    private string _documentTitle;
    private string _documentTitleEncoded;

    internal string DocumentLocation
    {
      get
      {
        return this._documentLocation;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._documentLocation = this._documentLocationEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 2048)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentLocation\" property should not exceed of {0} bytes", (object) 2048));
          this._documentLocationEncoded = str;
          this._documentLocation = value;
        }
      }
    }

    internal string DocumentLocationEncoded
    {
      get
      {
        return this._documentLocationEncoded;
      }
    }

    internal string DocumentHostName
    {
      get
      {
        return this._documentHostName;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._documentHostName = this._documentHostNameEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 100)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentHostName\" property should not exceed of {0} bytes", (object) 100));
          this._documentHostNameEncoded = str;
          this._documentHostName = value;
        }
      }
    }

    internal string DocumentHostNameEncoded
    {
      get
      {
        return this._documentHostNameEncoded;
      }
    }

    internal string DocumentPath
    {
      get
      {
        return this._documentPath;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._documentPath = this._documentPathEncoded = value;
        }
        else
        {
          if (PayloadData.ThrowExceptions && (int) value[0] != 47)
            throw new ArgumentException("\"DocumentPath\" should begin with '/'", "value");
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 2048)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentPath\" property should not exceed of {0} bytes", (object) 2048));
          this._documentPathEncoded = str;
          this._documentPath = value;
        }
      }
    }

    internal string DocumentPathEncoded
    {
      get
      {
        return this._documentPathEncoded;
      }
    }

    internal string DocumentTitle
    {
      get
      {
        return this._documentTitle;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._documentTitle = this._documentTitleEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 1500)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"DocumentTitle\" property should not exceed of {0} bytes", (object) 1500));
          this._documentTitleEncoded = str;
          this._documentTitle = value;
        }
      }
    }

    internal string DocumentTitleEncoded
    {
      get
      {
        return this._documentTitleEncoded;
      }
    }
  }
}
