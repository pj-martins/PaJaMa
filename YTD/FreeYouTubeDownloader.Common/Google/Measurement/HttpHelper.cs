// Decompiled with JetBrains decompiler
// Type: Google.Measurement.HttpHelper
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Google.Measurement
{
  internal static class HttpHelper
  {
    internal static void SendPostRequest(string url, string body, Encoding encoding)
    {
      HttpWebRequest httpWebRequest1 = (HttpWebRequest) WebRequest.Create(url);
      HttpHelper.PostData postData1 = new HttpHelper.PostData();
      HttpWebRequest httpWebRequest2 = httpWebRequest1;
      postData1.Request = httpWebRequest2;
      string str = body;
      postData1.Body = str;
      Encoding encoding1 = encoding;
      postData1.Encoding = encoding1;
      ManualResetEvent manualResetEvent = new ManualResetEvent(false);
      postData1.WaitHandle = manualResetEvent;
      HttpHelper.PostData postData2 = postData1;
      httpWebRequest1.Method = "POST";
      httpWebRequest1.BeginGetRequestStream(new AsyncCallback(HttpHelper.BeginGetRequestStreamCallback), (object) postData2);
      postData2.WaitHandle.WaitOne();
    }

    internal static void SendPostRequestAsync(string url, string body, Encoding encoding)
    {
      HttpWebRequest httpWebRequest1 = (HttpWebRequest) WebRequest.Create(url);
      HttpHelper.PostData postData1 = new HttpHelper.PostData();
      HttpWebRequest httpWebRequest2 = httpWebRequest1;
      postData1.Request = httpWebRequest2;
      string str = body;
      postData1.Body = str;
      Encoding encoding1 = encoding;
      postData1.Encoding = encoding1;
      HttpHelper.PostData postData2 = postData1;
      httpWebRequest1.Method = "POST";
      httpWebRequest1.BeginGetRequestStream(new AsyncCallback(HttpHelper.BeginGetRequestStreamCallback), (object) postData2);
    }

    internal static void SendGetRequest(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      HttpHelper.CallbackToken callbackToken1 = new HttpHelper.CallbackToken();
      callbackToken1.Request = httpWebRequest;
      ManualResetEvent manualResetEvent = new ManualResetEvent(false);
      callbackToken1.WaitHandle = manualResetEvent;
      HttpHelper.CallbackToken callbackToken2 = callbackToken1;
      httpWebRequest.BeginGetResponse(new AsyncCallback(HttpHelper.BeginGetResponseCallback), (object) callbackToken2);
      callbackToken2.WaitHandle.WaitOne();
    }

    internal static void SendGetRequestAsync(string url)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(url);
      HttpHelper.CallbackToken callbackToken = new HttpHelper.CallbackToken()
      {
        Request = httpWebRequest
      };
      httpWebRequest.BeginGetResponse(new AsyncCallback(HttpHelper.BeginGetResponseCallback), (object) callbackToken);
    }

    private static void BeginGetRequestStreamCallback(IAsyncResult ar)
    {
      HttpHelper.PostData asyncState = (HttpHelper.PostData) ar.AsyncState;
      try
      {
        using (Stream requestStream = asyncState.Request.EndGetRequestStream(ar))
        {
          using (StreamWriter streamWriter = new StreamWriter(requestStream, asyncState.Encoding))
            streamWriter.Write(asyncState.Body);
        }
        asyncState.Request.BeginGetResponse(new AsyncCallback(HttpHelper.BeginGetResponseCallback), (object) asyncState);
      }
      catch
      {
      }
    }

    private static void BeginGetResponseCallback(IAsyncResult ar)
    {
      HttpHelper.CallbackToken asyncState = (HttpHelper.CallbackToken) ar.AsyncState;
      try
      {
        using (asyncState.Request.EndGetResponse(ar))
          ;
      }
      catch
      {
      }
      finally
      {
        if (asyncState.WaitHandle != null)
          asyncState.WaitHandle.Set();
      }
    }

    private class CallbackToken
    {
      internal HttpWebRequest Request;
      internal ManualResetEvent WaitHandle;
    }

    private sealed class PostData : HttpHelper.CallbackToken
    {
      internal string Body;
      internal Encoding Encoding;
    }
  }
}
