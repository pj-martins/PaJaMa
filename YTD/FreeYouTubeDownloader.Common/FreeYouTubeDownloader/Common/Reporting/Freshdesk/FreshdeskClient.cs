// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.Common.Reporting.Freshdesk.FreshdeskClient
// Assembly: FreeYouTubeDownloader.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 06EDD06F-245A-4D5E-A05A-E271B59D1186
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\FreeYouTubeDownloader.Common.dll

using System;
using System.IO;
using System.Net;
using System.Text;

namespace FreeYouTubeDownloader.Common.Reporting.Freshdesk
{
  public class FreshdeskClient
  {
    private const string NewLine = "\r\n";
    private const string ApiKey = "3NJWvonzmoWoE4lWITEI";
    private const string ApiUrl = "https://youtubedownloader.freshdesk.com/helpdesk/tickets.json";
    private readonly string _apiKey;
    private readonly string _apiUrl;
    private readonly Encoding _encoding;
    private string _boundary;

    public FreshdeskClient()
    {
      this._encoding = Encoding.UTF8;
      this._apiKey = "3NJWvonzmoWoE4lWITEI";
      this._apiUrl = "https://youtubedownloader.freshdesk.com/helpdesk/tickets.json";
    }

    public FreshdeskClient(string apiKey, string apiUrl)
      : this()
    {
      this._apiKey = apiKey;
      this._apiUrl = apiUrl;
    }

    public void PostTicket(FreshdeskTicket ticket)
    {
      this._boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(this._apiUrl);
      httpWebRequest.Headers.Clear();
      httpWebRequest.ContentType = "multipart/form-data; boundary=" + this._boundary;
      httpWebRequest.Method = "POST";
      httpWebRequest.Accept = "text/html, application/xhtml+xml, */*";
      httpWebRequest.AllowAutoRedirect = true;
      httpWebRequest.KeepAlive = true;
      string src = this._apiKey + ":X";
      httpWebRequest.Headers[HttpRequestHeader.Authorization] = "Basic " + this.ConvertToBase64(src);
      using (Stream requestStream = httpWebRequest.GetRequestStream())
      {
        this.WriteParameter(requestStream, "helpdesk_ticket[email]", ticket.UserEmail);
        this.WriteParameter(requestStream, "helpdesk_ticket[subject]", ticket.Subject);
        this.WriteParameter(requestStream, "helpdesk_ticket[description]", ticket.Description);
        if (!string.IsNullOrEmpty(ticket.AppVersion))
          this.WriteParameter(requestStream, "helpdesk_ticket[custom_field][version_number_204360]", ticket.AppVersion);
        TypeOfProblem? typeOfProblem = ticket.TypeOfProblem;
        if (typeOfProblem.HasValue)
        {
          Stream targetStream = requestStream;
          string paramName = "helpdesk_ticket[ticket_type]";
          typeOfProblem = ticket.TypeOfProblem;
          string paramValue = EnumHelper.ToString((System.Enum) typeOfProblem.Value);
          this.WriteParameter(targetStream, paramName, paramValue);
        }
        TicketStatus? status = ticket.Status;
        if (status.HasValue)
        {
          Stream targetStream = requestStream;
          string paramName = "helpdesk_ticket[status]";
          status = ticket.Status;
          string paramValue = EnumHelper.ToInt32((System.Enum) status.Value).ToString();
          this.WriteParameter(targetStream, paramName, paramValue);
        }
        ulong? nullable = ticket.ResponderId;
        if (nullable.HasValue)
        {
          Stream targetStream = requestStream;
          string paramName = "helpdesk_ticket[responder_id]";
          nullable = ticket.ResponderId;
          string paramValue = nullable.ToString();
          this.WriteParameter(targetStream, paramName, paramValue);
        }
        nullable = ticket.GroupId;
        if (nullable.HasValue)
        {
          Stream targetStream = requestStream;
          string paramName = "helpdesk_ticket[group_id]";
          nullable = ticket.GroupId;
          string paramValue = nullable.ToString();
          this.WriteParameter(targetStream, paramName, paramValue);
        }
        if (ticket.Attachments != null)
        {
          foreach (FreshdeskAttachment attachment in ticket.Attachments)
          {
            if (attachment != null && attachment.Content.Length != 0)
              this.WriteAttachment(requestStream, "helpdesk_ticket[attachments][][resource]", attachment.Content, attachment.Name, attachment.MimeType);
          }
        }
        this.WriteBoundary(requestStream, true);
      }
      using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
      {
        if (responseStream == null)
          return;
        using (StreamReader streamReader = new StreamReader(responseStream))
          streamReader.ReadToEnd();
      }
    }

    private string ConvertToBase64(string src)
    {
      return Convert.ToBase64String(this._encoding.GetBytes(src));
    }

    private void WriteBoundary(Stream targetStream, bool isFinalBoundary)
    {
      byte[] bytes = this._encoding.GetBytes(isFinalBoundary ? "--" + this._boundary + "--" : "--" + this._boundary + "\r\n");
      targetStream.Write(bytes, 0, bytes.Length);
    }

    private void WriteStringToStream(Stream targetStream, string str)
    {
      byte[] bytes = this._encoding.GetBytes(str);
      targetStream.Write(bytes, 0, bytes.Length);
    }

    private void WriteContentDispositionFormDataHeader(Stream targetStream, string name)
    {
      string str = "Content-Disposition: form-data; name=\"" + name + "\"\r\n\r\n";
      this.WriteStringToStream(targetStream, str);
    }

    private void WriteContentDispositionFileHeader(Stream targetStream, string name, string fileName, string contentType)
    {
      string str = "Content-Disposition: form-data; name=\"" + name + "\"; filename=\"" + fileName + "\"\r\n" + "Content-Type: " + contentType + "\r\n\r\n";
      this.WriteStringToStream(targetStream, str);
    }

    private void WriteParameter(Stream targetStream, string paramName, string paramValue)
    {
      byte[] bytes = this._encoding.GetBytes(paramValue);
      this.WriteParameter(targetStream, paramName, bytes);
    }

    private void WriteParameter(Stream targetStream, string paramName, byte[] paramValue)
    {
      this.WriteBoundary(targetStream, false);
      this.WriteContentDispositionFormDataHeader(targetStream, paramName);
      targetStream.Write(paramValue, 0, paramValue.Length);
      this.WriteStringToStream(targetStream, "\r\n");
    }

    private void WriteAttachment(Stream targetStream, string paramName, byte[] attachment, string attachmentName, string attachmentType)
    {
      this.WriteBoundary(targetStream, false);
      this.WriteContentDispositionFileHeader(targetStream, paramName, attachmentName, attachmentType);
      targetStream.Write(attachment, 0, attachment.Length);
      this.WriteStringToStream(targetStream, "\r\n");
    }
  }
}
