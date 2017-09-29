// Decompiled with JetBrains decompiler
// Type: Google.Measurement.MeasurementClient
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public sealed class MeasurementClient
  {
    private const string ApiEndpointUrl = "http://www.google-analytics.com/collect";
    private const string ApiEndpointUrlSsl = "https://ssl.google-analytics.com/collect";
    private const int MaxLengthBytesPostBody = 8192;
    private const int MaxLengthBytesUrl = 2000;
    private const int MaxLengthBytesUserLanguage = 20;
    private const string VersionParameter = "v=1";
    private const string TrackingIdParameter = "&tid=";
    private const string ClientIdParameter = "&cid=";
    private const string UserAgentParameter = "&ua=";
    private const string UserLanguageParameter = "&ul=";
    private string _method;
    private string _userAgent;
    private string _userAgentEncoded;
    private string _userLanguage;

    public string TrackingId { get; private set; }

    public Guid? ClientId { get; set; }

    public string Method
    {
      get
      {
        return this._method;
      }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");
        value = value.ToUpperInvariant();
        if (!(value == "GET") && !(value == "POST"))
          throw new ArgumentOutOfRangeException("value", "Only \"POST\" or \"GET\" are valid values");
        this._method = value;
      }
    }

    public bool UseSsl { get; set; }

    public string UserAgent
    {
      get
      {
        return this._userAgent;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._userAgent = this._userAgentEncoded = value;
        }
        else
        {
          this._userAgentEncoded = value.UrlEncode();
          this._userAgent = value;
        }
      }
    }

    public bool ThrowExceptions
    {
      get
      {
        return PayloadData.ThrowExceptions;
      }
      set
      {
        PayloadData.ThrowExceptions = value;
      }
    }

    public string UserLanguage
    {
      get
      {
        return this._userLanguage;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._userLanguage = value;
        }
        else
        {
          if (PayloadData.ThrowExceptions && value.Length > 20)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"UserLanguage\" property should not exceed of {0} bytes", (object) 20));
          this._userLanguage = value;
        }
      }
    }

    public MeasurementClient()
    {
      this.Method = "POST";
    }

    public MeasurementClient(string trackingId)
      : this()
    {
      this.TrackingId = trackingId;
    }

    public MeasurementClient(string trackingId, Guid clientId)
      : this(trackingId)
    {
      this.ClientId = new Guid?(clientId);
    }

    public void PageView(PageViewData pageViewData)
    {
      this.SendHit(HitType.PageView, (PayloadData) pageViewData);
    }

    public void PageView(string documentHostname, string documentPath, string documentTitle)
    {
      this.PageView(new PageViewData()
      {
        DocumentHostName = documentHostname,
        DocumentPath = documentPath,
        DocumentTitle = documentTitle
      });
    }

    public void PageView(string documentLocation, string documentTitle)
    {
      this.PageView(new PageViewData()
      {
        DocumentLocation = documentLocation,
        DocumentTitle = documentTitle
      });
    }

    public void PageViewAsync(PageViewData pageViewData)
    {
      this.SendHitAsync(HitType.PageView, (PayloadData) pageViewData);
    }

    public void PageViewAsync(string documentHostname, string documentPath, string documentTitle)
    {
      this.PageViewAsync(new PageViewData()
      {
        DocumentHostName = documentHostname,
        DocumentPath = documentPath,
        DocumentTitle = documentTitle
      });
    }

    public void PageViewAsync(string documentLocation, string documentTitle)
    {
      this.PageViewAsync(new PageViewData()
      {
        DocumentLocation = documentLocation,
        DocumentTitle = documentTitle
      });
    }

    public void ScreenView(ScreenViewData screenViewData)
    {
      this.SendHit(HitType.ScreenView, (PayloadData) screenViewData);
    }

    public void ScreenView(string screenName, string applicationName, string applicationVersion)
    {
      this.ScreenView(new ScreenViewData()
      {
        ScreenName = screenName,
        ApplicationName = applicationName,
        ApplicationVersion = applicationVersion
      });
    }

    public void ScreenViewAsync(ScreenViewData screenViewData)
    {
      this.SendHitAsync(HitType.ScreenView, (PayloadData) screenViewData);
    }

    public void ScreenViewAsync(string screenName, string applicationName, string applicationVersion)
    {
      this.ScreenViewAsync(new ScreenViewData()
      {
        ScreenName = screenName,
        ApplicationName = applicationName,
        ApplicationVersion = applicationVersion
      });
    }

    public void Event(EventData eventData)
    {
      this.SendHit(HitType.Event, (PayloadData) eventData);
    }

    public void Event(string eventCategory, string eventAction)
    {
      this.Event(new EventData()
      {
        EventCategory = eventCategory,
        EventAction = eventAction
      });
    }

    public void Event(string eventCategory, string eventAction, string applicationName, string screenName, string applicationVersion)
    {
      AppEventData appEventData = new AppEventData();
      string str1 = eventCategory;
      appEventData.EventCategory = str1;
      string str2 = eventAction;
      appEventData.EventAction = str2;
      string str3 = applicationName;
      appEventData.ApplicationName = str3;
      string str4 = screenName;
      appEventData.ScreenName = str4;
      string str5 = applicationVersion;
      appEventData.ApplicationVersion = str5;
      this.Event((EventData) appEventData);
    }

    public void EventAsync(EventData eventData)
    {
      this.SendHitAsync(HitType.Event, (PayloadData) eventData);
    }

    public void EventAsync(string eventCategory, string eventAction)
    {
      this.EventAsync(new EventData()
      {
        EventCategory = eventCategory,
        EventAction = eventAction
      });
    }

    public void EventAsync(string eventCategory, string eventAction, string applicationName, string screenName, string applicationVersion)
    {
      AppEventData appEventData = new AppEventData();
      string str1 = eventCategory;
      appEventData.EventCategory = str1;
      string str2 = eventAction;
      appEventData.EventAction = str2;
      string str3 = applicationName;
      appEventData.ApplicationName = str3;
      string str4 = screenName;
      appEventData.ScreenName = str4;
      string str5 = applicationVersion;
      appEventData.ApplicationVersion = str5;
      this.EventAsync((EventData) appEventData);
    }

    public void Exception(string exceptionDescription, string applicationName, string applicationVersion = null, bool? isFatal = null)
    {
      this.Exception(new ExceptionData()
      {
        ExceptionDescription = exceptionDescription,
        IsFatal = isFatal,
        ApplicationName = applicationName,
        ApplicationVersion = applicationVersion
      });
    }

    public void Exception(ExceptionData exceptionData)
    {
      this.SendHit(HitType.Exception, (PayloadData) exceptionData);
    }

    public void ExceptionAsync(string exceptionDescription, string applicationName, string applicationVersion = null, bool? isFatal = null)
    {
      this.ExceptionAsync(new ExceptionData()
      {
        ExceptionDescription = exceptionDescription,
        IsFatal = isFatal,
        ApplicationName = applicationName,
        ApplicationVersion = applicationVersion
      });
    }

    public void ExceptionAsync(ExceptionData exceptionData)
    {
      this.SendHitAsync(HitType.Exception, (PayloadData) exceptionData);
    }

    public void Social(string socialNetwork, string socialAction, string socialActionTarget)
    {
      this.Social(new SocialData()
      {
        SocialNetwork = socialNetwork,
        SocialAction = socialAction,
        SocialActionTarget = socialActionTarget
      });
    }

    public void Social(SocialData socialData)
    {
      this.SendHit(HitType.Social, (PayloadData) socialData);
    }

    public void SocialAsync(string socialNetwork, string socialAction, string socialActionTarget)
    {
      this.SocialAsync(new SocialData()
      {
        SocialNetwork = socialNetwork,
        SocialAction = socialAction,
        SocialActionTarget = socialActionTarget
      });
    }

    public void SocialAsync(SocialData socialData)
    {
      this.SendHitAsync(HitType.Social, (PayloadData) socialData);
    }

    private string GetPayloadData()
    {
      StringBuilder stringBuilder = new StringBuilder(512);
      stringBuilder.Append("v=1");
      stringBuilder.Append("&tid=");
      stringBuilder.Append(this.TrackingId);
      stringBuilder.Append("&cid=");
      stringBuilder.Append((object) (this.ClientId ?? Guid.NewGuid()));
      if (this._userAgentEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&ua=");
        stringBuilder.Append(this._userAgentEncoded);
      }
      if (this._userLanguage.NotNullOrEmpty())
      {
        stringBuilder.Append("&ul=");
        stringBuilder.Append(this._userLanguage);
      }
      return stringBuilder.ToString();
    }

    public void SendHit(HitType hitType, PayloadData payload)
    {
      this.SendHit(this.GetPayloadData() + payload.GetPayloadData());
    }

    public void SendHit(string payloadData)
    {
      string url1 = this.UseSsl ? "https://ssl.google-analytics.com/collect" : "http://www.google-analytics.com/collect";
      if (this.Method == "POST")
      {
        if (PayloadData.ThrowExceptions && payloadData.Length > 8192)
          throw new ArgumentOutOfRangeException(string.Format("The body of the post request exceeds {0} bytes", (object) 8192), (Exception) null);
        HttpHelper.SendPostRequest(url1, payloadData, PayloadData.Encoding);
      }
      else
      {
        string url2 = url1 + "?" + payloadData;
        if (PayloadData.ThrowExceptions && url2.Length > 2000)
          throw new ArgumentOutOfRangeException(string.Format("The length of the entire encoded URL must be no longer than {0} Bytes", (object) 2000), (Exception) null);
        HttpHelper.SendGetRequest(url2);
      }
    }

    public void SendHitAsync(HitType hitType, PayloadData payload)
    {
      this.SendHitAsync(this.GetPayloadData() + payload.GetPayloadData());
    }

    public void SendHitAsync(string payloadData)
    {
      string url1 = this.UseSsl ? "https://ssl.google-analytics.com/collect" : "http://www.google-analytics.com/collect";
      if (this.Method == "POST")
      {
        if (PayloadData.ThrowExceptions && payloadData.Length > 8192)
          throw new ArgumentOutOfRangeException(string.Format("The body of the post request exceeds {0} bytes", (object) 8192), (Exception) null);
        HttpHelper.SendPostRequestAsync(url1, payloadData, PayloadData.Encoding);
      }
      else
      {
        string url2 = url1 + "?" + payloadData;
        if (PayloadData.ThrowExceptions && url2.Length > 2000)
          throw new ArgumentOutOfRangeException(string.Format("The length of the entire encoded URL must be no longer than {0} Bytes", (object) 2000), (Exception) null);
        HttpHelper.SendGetRequestAsync(url2);
      }
    }
  }
}
