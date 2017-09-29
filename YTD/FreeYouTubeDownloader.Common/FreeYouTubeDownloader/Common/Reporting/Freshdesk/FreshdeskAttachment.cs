// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Reporting.Freshdesk.FreshdeskAttachment
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

namespace FreeYouTubeDownloader.Common.Reporting.Freshdesk
{
  public class FreshdeskAttachment
  {
    public byte[] Content { get; private set; }

    public string Name { get; private set; }

    public string MimeType { get; private set; }

    public FreshdeskAttachment(string name, string mimeType, byte[] content)
    {
      this.Name = name;
      this.MimeType = mimeType;
      this.Content = content;
    }
  }
}
