// Decompiled with JetBrains decompiler
// Type: Google.Measurement.PayloadData
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System.Text;

namespace Google.Measurement
{
  public abstract class PayloadData
  {
    private const string DataSourceParameter = "&ds=";
    private string _dataSource;
    private string _dataSourceEncoded;

    internal static Encoding Encoding { get; private set; }

    internal static bool ThrowExceptions { get; set; }

    public string DataSource
    {
      get
      {
        return this._dataSource;
      }
      set
      {
        if (value.IsNullOrEmpty())
        {
          this._dataSource = this._dataSourceEncoded = value;
        }
        else
        {
          this._dataSourceEncoded = value.UrlEncode();
          this._dataSource = value;
        }
      }
    }

    static PayloadData()
    {
      PayloadData.Encoding = Encoding.UTF8;
      PayloadData.ThrowExceptions = true;
    }

    internal virtual string GetPayloadData()
    {
      StringBuilder stringBuilder = new StringBuilder(this._dataSourceEncoded.GetSafeLength());
      if (this._dataSourceEncoded.NotNullOrEmpty())
        stringBuilder.Append("&ds=");
      return stringBuilder.ToString();
    }
  }
}
