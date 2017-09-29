// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Reporting.Freshdesk.FreshdeskTicket
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FreeYouTubeDownloader.Common.Reporting.Freshdesk
{
  public class FreshdeskTicket
  {
    public const string FieldEmail = "helpdesk_ticket[email]";
    public const string FieldSubject = "helpdesk_ticket[subject]";
    public const string FieldDescription = "helpdesk_ticket[description]";
    public const string FieldVersionNumber = "helpdesk_ticket[custom_field][version_number_204360]";
    public const string FieldTicketType = "helpdesk_ticket[ticket_type]";
    public const string FieldAttachment = "helpdesk_ticket[attachments][][resource]";
    public const string FieldStatus = "helpdesk_ticket[status]";
    public const string FieldResponderId = "helpdesk_ticket[responder_id]";
    public const string FieldGroupId = "helpdesk_ticket[group_id]";

    [Validate("^[\\w!#$%&'*+\\-/=?\\^_`{|}~]+(\\.[\\w!#$%&'*+\\-/=?\\^_`{|}~]+)*@((([\\-\\w]+\\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\\.){3}[0-9]{1,3}))$", "It is necessary to provide a valid email")]
    public string UserEmail { get; set; }

    public string Subject { get; set; }

    public string Description { get; set; }

    public List<FreshdeskAttachment> Attachments { get; set; }

    public string AppVersion { get; set; }

    public ulong? ResponderId { get; set; }

    public FreeYouTubeDownloader.Common.Reporting.Freshdesk.TypeOfProblem? TypeOfProblem { get; set; }

    public TicketStatus? Status { get; set; }

    public ulong? GroupId { get; set; }

    public bool Validate()
    {
      string[] failures;
      return this.Validate(out failures);
    }

    public bool Validate(out string[] failures)
    {
      PropertyInfo[] properties = this.GetType().GetProperties();
      bool flag = true;
      List<string> stringList = new List<string>();
      Func<PropertyInfo, IEnumerable<Validate>> func = (Func<PropertyInfo, IEnumerable<Validate>>) (propertyInfo => propertyInfo.GetCustomAttributes(typeof (Validate), false).Cast<Validate>());
      Func<PropertyInfo, IEnumerable<Validate>> collectionSelector = null;
      foreach (Validate validate in ((IEnumerable<PropertyInfo>) properties).SelectMany(collectionSelector, (propertyInfo, validate) => new
      {
        propertyInfo = propertyInfo,
        validate = validate
      }).Where(param0 => !param0.validate.Expression.IsMatch((string) param0.propertyInfo.GetValue((object) this, (object[]) null))).Select(param0 => param0.validate))
      {
        flag = false;
        if (!string.IsNullOrEmpty(validate.Failure))
          stringList.Add(validate.Failure);
      }
      failures = stringList.ToArray();
      return flag;
    }
  }
}
