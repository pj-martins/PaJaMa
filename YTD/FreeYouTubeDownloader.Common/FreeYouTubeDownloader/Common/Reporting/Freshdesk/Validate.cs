// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Reporting.Freshdesk.Validate
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Text.RegularExpressions;

namespace FreeYouTubeDownloader.Common.Reporting.Freshdesk
{
  public class Validate : Attribute
  {
    public Regex Expression { get; private set; }

    public string Failure { get; private set; }

    public Validate(string pattern)
    {
      this.Expression = new Regex(pattern);
    }

    public Validate(string pattern, string failure)
      : this(pattern)
    {
      this.Failure = failure;
    }
  }
}
