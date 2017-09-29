// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Reporting.Freshdesk.TypeOfProblem
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

namespace FreeYouTubeDownloader.Common.Reporting.Freshdesk
{
  public enum TypeOfProblem
  {
    [StringValue("General Technical Problem")] GeneralTechnicalProblem,
    [StringValue("Download Errors")] DownloadErrors,
    [StringValue("Conversion Issues")] ConversionIssues,
    [StringValue("Billing or License Activation")] BillingOrLicenseActivation,
    [StringValue("Unhandled Exception")] UnhandledException,
  }
}
