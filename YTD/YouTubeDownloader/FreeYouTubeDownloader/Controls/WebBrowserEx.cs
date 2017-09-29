// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Controls.WebBrowserEx
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace FreeYouTubeDownloader.Controls
{
  public class WebBrowserEx : WebBrowser
  {
    private AxHost.ConnectionPointCookie _cookie;
    private WebBrowserEx.WebBrowserExEventHelper _helper;

    public event WebBrowserExNavigateErrorEventHandler NavigateError;

    public event WebBrowserExBeforeNavigate2EventHandler BeforeNavigate2;

    public event WebBrowserExNewWindow3EventHandler NewWindow3;

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void CreateSink()
    {
      base.CreateSink();
      this._helper = new WebBrowserEx.WebBrowserExEventHelper(this);
      this._cookie = new AxHost.ConnectionPointCookie(this.ActiveXInstance, (object) this._helper, typeof (DWebBrowserEvents2));
    }

    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected override void DetachSink()
    {
      if (this._cookie != null)
      {
        this._cookie.Disconnect();
        this._cookie = (AxHost.ConnectionPointCookie) null;
      }
      base.DetachSink();
    }

    protected virtual void OnNavigateError(WebBrowserExNavigateErrorEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      WebBrowserExNavigateErrorEventHandler navigateError = this.NavigateError;
      if (navigateError == null)
        return;
      WebBrowserExNavigateErrorEventArgs e1 = e;
      navigateError((object) this, e1);
    }

    protected virtual void OnBeforeNavigate2(WebBrowserExBeforeNavigate2EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      WebBrowserExBeforeNavigate2EventHandler beforeNavigate2 = this.BeforeNavigate2;
      if (beforeNavigate2 == null)
        return;
      WebBrowserExBeforeNavigate2EventArgs e1 = e;
      beforeNavigate2((object) this, e1);
    }

    protected virtual void OnNewWindow3(WebBrowserExNewWindow3EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      WebBrowserExNewWindow3EventHandler newWindow3 = this.NewWindow3;
      if (newWindow3 == null)
        return;
      WebBrowserExNewWindow3EventArgs e1 = e;
      newWindow3((object) this, e1);
    }

    private class WebBrowserExEventHelper : StandardOleMarshalObject, DWebBrowserEvents2
    {
      private readonly WebBrowserEx _parent;

      public WebBrowserExEventHelper(WebBrowserEx parent)
      {
        this._parent = parent;
      }

      public void NavigateError(object pDisp, ref object url, ref object frame, ref object statusCode, ref bool cancel)
      {
        WebBrowserExNavigateErrorEventArgs e = new WebBrowserExNavigateErrorEventArgs((string) url, (string) frame, (int) statusCode, cancel);
        this._parent.OnNavigateError(e);
        cancel = e.Cancel;
      }

      public void BeforeNavigate2(object pDisp, ref object url, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
      {
        WebBrowserExBeforeNavigate2EventArgs e = new WebBrowserExBeforeNavigate2EventArgs((string) url, flags, (string) targetFrameName, (string) postData, headers, cancel);
        this._parent.OnBeforeNavigate2(e);
        cancel = e.Cancel;
      }

      public void NewWindow3(ref object ppDisp, ref bool cancel, uint dwFlags, string bstrUrlContext, string bstrUrl)
      {
        WebBrowserExNewWindow3EventArgs e = new WebBrowserExNewWindow3EventArgs(bstrUrl, dwFlags, bstrUrlContext, cancel);
        this._parent.OnNewWindow3(e);
        cancel = e.Cancel;
      }
    }
  }
}
