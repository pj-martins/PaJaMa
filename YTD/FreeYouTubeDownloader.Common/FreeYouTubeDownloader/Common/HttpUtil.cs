// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.HttpUtil
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace FreeYouTubeDownloader.Common
{
  public class HttpUtil
  {
    private string _boundary;
    private static HttpUtil _instance;

    public WebExceptionStatus LastOperationStatus { get; private set; }

    public static HttpUtil Instance
    {
      get
      {
        return HttpUtil._instance ?? (HttpUtil._instance = new HttpUtil());
      }
    }

    public void GetData(string url, Action<string> dataRetrieved, string method = "GET", Dictionary<string, object> postData = null)
    {
      this.GetData(new Uri(url, UriKind.Absolute), dataRetrieved, method, postData);
    }

    public void GetData(Uri uri, Action<string> dataRetrieved, string method = "GET", Dictionary<string, object> postData = null)
    {
      if (dataRetrieved == null)
        throw new ArgumentNullException("dataRetrieved");
      HttpWebRequest webRequest = this.CreateWebRequest(uri);
      this.ConfigureWebRequest(ref webRequest, method);
      WebResponseAsyncCallbackUserObject callbackUserObject = new WebResponseAsyncCallbackUserObject()
      {
        Request = webRequest,
        DataRetrieved = dataRetrieved,
        PostData = postData
      };
      if (method.Equals("POST"))
        webRequest.BeginGetRequestStream(new AsyncCallback(this.GetRequestStreamCallback), (object) callbackUserObject);
      else
        webRequest.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), (object) callbackUserObject);
    }

    public string GetDataSync(string url, string method = "GET", Dictionary<string, object> postData = null)
    {
      return this.GetDataSync(new Uri(url, UriKind.Absolute), method, postData);
    }

    public string GetDataSync(Uri uri, string method = "GET", Dictionary<string, object> postData = null)
    {
      string data = string.Empty;
      HttpWebRequest webRequest = this.CreateWebRequest(uri);
      this.ConfigureWebRequest(ref webRequest, method);
      ManualResetEvent waitobject = new ManualResetEvent(false);
      webRequest.BeginGetResponse((AsyncCallback) (asyncCallback =>
      {
        try
        {
          using (StreamReader streamReader = new StreamReader(((WebRequest) asyncCallback.AsyncState).EndGetResponse(asyncCallback).GetResponseStream()))
            data = streamReader.ReadToEnd();
          this.LastOperationStatus = WebExceptionStatus.Success;
        }
        catch (WebException ex)
        {
          this.LastOperationStatus = ex.Status;
        }
        catch
        {
        }
        finally
        {
          waitobject.Set();
        }
      }), (object) webRequest);
      waitobject.WaitOne();
      return data;
    }

    private byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
    {
      MemoryStream memoryStream = new MemoryStream();
      foreach (KeyValuePair<string, object> postParameter in postParameters)
      {
        if (postParameter.Value is byte[])
        {
          byte[] buffer = postParameter.Value as byte[];
          string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}.jpg\";\r\nContent-Type: application/octet-stream\r\n\r\n", (object) boundary, (object) postParameter.Key, (object) postParameter.Key);
          memoryStream.Write(Encoding.UTF8.GetBytes(s), 0, s.Length);
          memoryStream.Write(buffer, 0, buffer.Length);
        }
        else
        {
          foreach (byte num in Encoding.UTF8.GetBytes(string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n", (object) boundary, (object) postParameter.Key, postParameter.Value)))
            memoryStream.WriteByte(num);
        }
      }
      byte[] bytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
      memoryStream.Write(bytes, 0, bytes.Length);
      memoryStream.Position = 0L;
      byte[] buffer1 = new byte[memoryStream.Length];
      memoryStream.Read(buffer1, 0, buffer1.Length);
      memoryStream.Flush();
      memoryStream.Close();
      return buffer1;
    }

    private HttpWebRequest CreateWebRequest(Uri uri)
    {
      return (HttpWebRequest) WebRequest.Create(uri);
    }

    private void ConfigureWebRequest(ref HttpWebRequest request, string method)
    {
      request.Method = method;
      request.Timeout = 3000;
      request.ReadWriteTimeout = 3000;
      if (!method.Equals("POST"))
        return;
      this._boundary = "-----------------------" + DateTime.Now.Ticks.ToString("x");
      request.ContentType = "multipart/form-data; boundary=" + this._boundary;
    }

    private void GetRequestStreamCallback(IAsyncResult ar)
    {
      WebResponseAsyncCallbackUserObject asyncState = (WebResponseAsyncCallbackUserObject) ar.AsyncState;
      Stream requestStream = asyncState.Request.EndGetRequestStream(ar);
      byte[] multipartFormData = this.GetMultipartFormData(asyncState.PostData, this._boundary);
      byte[] buffer = multipartFormData;
      int offset = 0;
      int length = multipartFormData.Length;
      requestStream.Write(buffer, offset, length);
      requestStream.Close();
      requestStream.Dispose();
      asyncState.Request.BeginGetResponse(new AsyncCallback(this.GetResponseCallback), (object) asyncState);
    }

    private void GetResponseCallback(IAsyncResult asynchronousResult)
    {
      WebResponseAsyncCallbackUserObject asyncState = (WebResponseAsyncCallbackUserObject) asynchronousResult.AsyncState;
      try
      {
        using (StreamReader streamReader = new StreamReader(asyncState.Request.EndGetResponse(asynchronousResult).GetResponseStream()))
          asyncState.DataRetrieved(streamReader.ReadToEnd());
        this.LastOperationStatus = WebExceptionStatus.Success;
      }
      catch (WebException ex)
      {
        this.LastOperationStatus = ex.Status;
      }
    }
  }
}
