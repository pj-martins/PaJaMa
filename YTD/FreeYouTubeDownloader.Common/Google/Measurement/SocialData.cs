// Decompiled with JetBrains decompiler
// Type: Google.Measurement.SocialData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text;

namespace Google.Measurement
{
  public sealed class SocialData : PayloadData
  {
    private const int MaxLengthBytesSocialNetwork = 50;
    private const int MaxLengthBytesSocialAction = 50;
    private const int MaxLengthBytesSocialActionTarget = 2048;
    private const string HitTypeParameter = "&t=social";
    private const string SocialNetworkParameter = "&sn=";
    private const string SocialActionParameter = "&sa=";
    private const string SocialActionTargetParameter = "&st=";
    private string _socialNetwork;
    private string _socialNetworkEncoded;
    private string _socialAction;
    private string _socialActionEncoded;
    private string _socialActionTarget;
    private string _socialActionTargetEncoded;

    public string SocialNetwork
    {
      get
      {
        return this._socialNetwork;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._socialNetwork = this._socialNetworkEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 50)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"SocialNetwork\" property should not exceed of {0} bytes", (object) 50));
          this._socialNetworkEncoded = str;
          this._socialNetwork = value;
        }
      }
    }

    public string SocialAction
    {
      get
      {
        return this._socialAction;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._socialAction = this._socialActionEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 50)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"SocialAction\" property should not exceed of {0} bytes", (object) 50));
          this._socialActionEncoded = str;
          this._socialAction = value;
        }
      }
    }

    public string SocialActionTarget
    {
      get
      {
        return this._socialActionTarget;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._socialActionTarget = this._socialActionTargetEncoded = value;
        }
        else
        {
          string str = value.UrlEncode();
          if (PayloadData.ThrowExceptions && str.Length > 2048)
            throw new ArgumentOutOfRangeException("value", string.Format("The URL encoded length of \"SocialActionTarget\" property should not exceed of {0} bytes", (object) 2048));
          this._socialActionTargetEncoded = str;
          this._socialActionTarget = value;
        }
      }
    }

    internal override string GetPayloadData()
    {
      string payloadData = base.GetPayloadData();
      StringBuilder stringBuilder = new StringBuilder(2048);
      if (this._socialNetworkEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&sn=");
        stringBuilder.Append(this._socialNetworkEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("The value of \"SocialNetwork\" must not be empty");
      if (this._socialActionEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&sa=");
        stringBuilder.Append(this._socialActionEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("The value of \"SocialAction\" must not be empty");
      if (this._socialActionTargetEncoded.NotNullOrEmpty())
      {
        stringBuilder.Append("&st=");
        stringBuilder.Append(this._socialActionTargetEncoded);
      }
      else if (PayloadData.ThrowExceptions)
        throw new InvalidOperationException("The value of \"SocialActionTarget\" must not be empty");
      stringBuilder.Append("&t=social");
      stringBuilder.Append(payloadData);
      return stringBuilder.ToString();
    }
  }
}
