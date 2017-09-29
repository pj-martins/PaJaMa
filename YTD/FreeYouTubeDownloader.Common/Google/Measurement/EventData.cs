// Decompiled with JetBrains decompiler
// Type: Google.Measurement.EventData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public class EventData : PayloadData
  {
    private const int MaxLengthBytesEventCategory = 150;
    private const int MaxLengthBytesEventAction = 500;
    private const int MaxLengthBytesEventLabel = 500;
    private const string HitTypeParameter = "&t=event";
    private const string EventCategoryParameter = "&ec=";
    private const string EventActionParameter = "&ea=";
    private const string EventLabelParameter = "&el=";
    private const string EventValueParameter = "&ev=";
    private string _eventCategory;
    private string _eventCategoryEncoded;
    private string _eventAction;
    private string _eventActionEncoded;
    private string _eventLabel;
    private string _eventLabelEncoded;

    public string EventCategory
    {
      get
      {
        return this._eventCategory;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._eventCategory = this._eventCategoryEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 150)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"EventCategory\" property should not exceed of {0} bytes", (object) 150));
          this._eventCategoryEncoded = str;
          this._eventCategory = value;
        }
      }
    }

    public string EventAction
    {
      get
      {
        return this._eventAction;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._eventAction = this._eventActionEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 500)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"EventAction\" property should not exceed of {0} bytes", (object) 500));
          this._eventActionEncoded = str;
          this._eventAction = value;
        }
      }
    }

    public string EventLabel
    {
      get
      {
        return this._eventLabel;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._eventLabel = this._eventLabelEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 500)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"EventLabel\" property should not exceed of {0} bytes", (object) 500));
          this._eventLabelEncoded = str;
          this._eventLabel = value;
        }
      }
    }

    public uint? EventValue { get; set; }

    internal override string GetPayloadData()
    {
      string payloadData = base.GetPayloadData();
      StringBuilder stringBuilder = new StringBuilder(2048);
      if (this._eventCategoryEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&ec=");
        stringBuilder.Append(this._eventCategoryEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("The value of \"EventCategory\" must not be empty");
      if (this._eventActionEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&ea=");
        stringBuilder.Append(this._eventActionEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("The value of \"EventAction\" must not be empty");
      stringBuilder.Append("&t=event");
      if (this._eventLabelEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&el=");
        stringBuilder.Append(this._eventLabelEncoded);
      }
      if (this.EventValue.HasValue)
      {
        stringBuilder.Append("&ev=");
        stringBuilder.Append((object) this.EventValue);
      }
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
