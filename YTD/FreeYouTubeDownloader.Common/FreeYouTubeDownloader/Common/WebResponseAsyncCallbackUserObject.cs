// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.WebResponseAsyncCallbackUserObject
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Collections.Generic;
using System.Net;

namespace FreeYouTubeDownloader.Common
{
  internal class WebResponseAsyncCallbackUserObject
  {
    public HttpWebRequest Request { get; set; }

    public Action<string> DataRetrieved { get; set; }

    public Dictionary<string, object> PostData { get; set; }
  }
}
