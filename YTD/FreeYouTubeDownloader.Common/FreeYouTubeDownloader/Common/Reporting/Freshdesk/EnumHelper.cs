// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Reporting.Freshdesk.EnumHelper
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using FreeYouTubeDownloader.Localization;
using System;
using System.Reflection;

namespace FreeYouTubeDownloader.Common.Reporting.Freshdesk
{
  public static class EnumHelper
  {
    public static string ToString(Enum en)
    {
      MemberInfo[] member = en.GetType().GetMember(en.ToString());
      if (member.Length == 0)
        return en.ToString();
      object[] customAttributes = member[0].GetCustomAttributes(typeof (StringValue), false);
      if (customAttributes.Length == 0)
        return en.ToString();
      return ((StringValue) customAttributes[0]).Text;
    }

    public static int ToInt32(object en)
    {
      return (int) en;
    }

    public static string GetLocalizedText(this TypeOfProblem problem)
    {
      switch (problem)
      {
        case TypeOfProblem.GeneralTechnicalProblem:
          return Strings.GeneralTechnicalProblem;
        case TypeOfProblem.DownloadErrors:
          return Strings.DownloadErrors;
        case TypeOfProblem.ConversionIssues:
          return Strings.ConversionIssues;
        default:
          return EnumHelper.ToString((Enum) problem);
      }
    }
  }
}
