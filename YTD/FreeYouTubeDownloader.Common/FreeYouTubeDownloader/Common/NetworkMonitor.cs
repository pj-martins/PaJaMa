// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.NetworkMonitor
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using FreeYouTubeDownloader.Common.Collections;
using FreeYouTubeDownloader.Debug;
using System;
using System.Linq;
using System.Net;
using System.Timers;

namespace FreeYouTubeDownloader.Common
{
  public sealed class NetworkMonitor : IDisposable
  {
    private readonly CircularStack<bool> _pingsResults = new CircularStack<bool>(5);
    private bool _hasInternetConnection = true;
    private const string CheckUrl = "http://wd.youtubedownloader.com/check.jpg";
    private const string CheckUrlFallback = "http://wd.getyoutubedownloader.com/check/check.jpg";
    private const double DefaultTimerInterval = 10000.0;
    private const int Timeout = 5000;
    private const int MaxPingResults = 5;
    private readonly System.Timers.Timer _timer;
    private double _interval;

    public double Interval
    {
      get
      {
        return this._interval;
      }
      set
      {
        if (this._interval == value)
          return;
        this._interval = value;
        this._timer.Interval = this._interval;
      }
    }

    public bool Connected
    {
      get
      {
        return this._hasInternetConnection;
      }
    }

    public bool IsActive
    {
      get
      {
        return this._timer.Enabled;
      }
    }

    public event NetworkMonitor.InternetAvailabilityChangedEventHandler InternetAvailabilityChanged;

    public NetworkMonitor()
    {
      this._interval = 10000.0;
      this._timer = new System.Timers.Timer(this.Interval);
      this._timer.Elapsed += new ElapsedEventHandler(this.OnTimerElapsed);
    }

    ~NetworkMonitor()
    {
      this.Dispose(false);
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
    {
      bool hasInternetConnection = NetworkMonitor.CheckInternetConnection();
      this._pingsResults.Push(hasInternetConnection);
      if (hasInternetConnection == this._hasInternetConnection || !this._pingsResults.All<bool>((Func<bool, bool>) (result => result == hasInternetConnection)))
        return;
      this._hasInternetConnection = hasInternetConnection;
      this.RaiseInternetAvailabilityChanged(this._hasInternetConnection);
    }

    public static bool CheckInternetConnection()
    {
      try
      {
        return NetworkMonitor.Ping("http://wd.youtubedownloader.com/check.jpg");
      }
      catch (WebException ex1)
      {
        Log.Info("NetworkMonitor.CheckInternetConnection => " + ex1.Message, (Exception) null);
        try
        {
          return NetworkMonitor.Ping("http://wd.getyoutubedownloader.com/check/check.jpg");
        }
        catch (WebException ex2)
        {
          return false;
        }
      }
    }

    private static bool Ping(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      int num1;
      int num2 = num1 = 5000;
      httpWebRequest.ReadWriteTimeout = num1;
      int num3 = num2;
      httpWebRequest.Timeout = num3;
      using (HttpWebResponse response = (HttpWebResponse) httpWebRequest.GetResponse())
        return response.StatusCode >= HttpStatusCode.OK && response.StatusCode <= HttpStatusCode.SeeOther;
    }

    internal void RaiseInternetAvailabilityChanged(bool hasInternetConnection)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.InternetAvailabilityChanged == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.InternetAvailabilityChanged(new InternetAvailabilityEventArgs(hasInternetConnection));
    }

    public void StartMonitorInternetConnection()
    {
      this._timer.Start();
    }

    public void StopMonitorInternetConnection()
    {
      this._timer.Stop();
    }

    private void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this._timer.Stop();
      this._timer.Dispose();
    }

    public void Dispose()
    {
      this.Dispose(true);
    }

    public delegate void InternetAvailabilityChangedEventHandler(InternetAvailabilityEventArgs eventArgs);
  }
}
