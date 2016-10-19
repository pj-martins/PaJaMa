using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace PaJaMa.Common
{
	
	public static class EMail
	{
		public static bool Send(string host, int port, bool enableSsl, string username, string password, MailAddress sender, List<MailAddress> recipients, string subject,
			string body, bool isBodyHtml, List<Attachment> attachments)
		{
			return Send(host, port, enableSsl, username, password, sender, recipients, null, subject, body, isBodyHtml, attachments);
		}

		public static bool Send(string host, int port, bool enableSsl, string username, string password, MailAddress sender, List<MailAddress> recipients, List<MailAddress> ccs,
			string subject, string body, bool isBodyHtml, List<Attachment> attachments)
		{
			return Send(host, port, enableSsl, username, password, sender, recipients, ccs, null, subject, body, isBodyHtml, attachments);
		}

		public static bool Send(string host, int port, bool enableSsl, string username, string password, MailAddress sender, List<MailAddress> recipients, List<MailAddress> ccs,
			List<MailAddress> bccs, string subject, string body, bool isBodyHtml, List<Attachment> attachments)
		{
			MailMessage msg = new MailMessage();
			try
			{
				SmtpClient client = new SmtpClient();
				client.UseDefaultCredentials = false;
				client.EnableSsl = enableSsl;
				client.Host = host;
				client.Port = port;
				if (!string.IsNullOrEmpty(username))
					client.Credentials = new System.Net.NetworkCredential(username, password);
				msg.IsBodyHtml = isBodyHtml;
				msg.From = sender;
				foreach (MailAddress recipient in recipients)
				{
					msg.To.Add(recipient);
				}

				if (ccs != null)
				{
					foreach (MailAddress cc in ccs)
					{
						msg.CC.Add(cc);
					}
				}

				if (bccs != null)
				{
					foreach (MailAddress bcc in bccs)
					{
						msg.Bcc.Add(bcc);
					}
				}

				if (attachments != null && attachments.Count > 0)
					attachments.ForEach(a => msg.Attachments.Add(a));

				msg.Subject = subject;
				msg.Body = body;
				client.Send(msg);
			}
			catch (Exception ex)
			{
				EMailException emex = new EMailException(msg, ex);
				Logging.LogException(emex);
				throw emex;
			}
			return true;
		}

		public static bool Send(string host, string username, string password, MailAddress sender, List<MailAddress> recipients, string subject,
			string body, bool isBodyHtml, List<Attachment> attachments)
		{
			return Send(host, 25, false, username, password, sender, recipients, subject, body, isBodyHtml, attachments);
		}

		public static bool Send(string host, string username, string password, MailAddress sender, List<MailAddress> recipients, List<MailAddress> ccs,
			string subject, string body, bool isBodyHtml, List<Attachment> attachments)
		{
			return Send(host, 25, false, username, password, sender, recipients, ccs, subject, body, isBodyHtml, attachments);
		}

        public static bool Send(string host, string username, string password, MailAddress sender, List<MailAddress> recipients, string subject, string body, int port = 25, bool enableSsl = false,
            bool isBodyHtml = true, List<Attachment> attachments = null, List<MailAddress> ccs = null, List<MailAddress> bccs = null)
        {
            return Send(host, port, enableSsl, username, password, sender, recipients, ccs, bccs, subject, body, isBodyHtml, attachments);
        }

		//public static bool Send(string host, int port, bool enableSsl, string username, string password, MailAddress sender, MailAddress recipient, string subject, string body, bool isBodyHtml)
		//{
		//    return Send(host, port, enableSsl, username, password, sender, new List<MailAddress>() { recipient }, subject, body, isBodyHtml);
		//}

		//public static bool Send(string host, int port, bool enableSsl, string username, string password, MailAddress sender, MailAddress recipient, string subject, string body)
		//{
		//    return Send(host, port, enableSsl, username, password, sender, new List<MailAddress>() { recipient }, subject, body, false);
		//}

		//public static bool Send(string host, int port, bool enableSsl, string username, string password, string sender, string recipient, string subject, string body, bool isBodyHtml)
		//{
		//    return Send(host, port, enableSsl, username, password, new MailAddress(sender), new MailAddress(recipient), subject, body, isBodyHtml);
		//}

		//public static bool Send(string host, int port, bool enableSsl, string username, string password, string sender, string recipient, string subject, string body)
		//{
		//    return Send(host, port, enableSsl, username, password, new MailAddress(sender), new MailAddress(recipient), subject, body, false);
		//}

		//public static bool Send(string host, int port, bool enableSsl, string username, string password, string subject, string body, bool isBodyHtml)
		//{
		//    return Send(host, port, enableSsl, username, password, new MailAddress("pj.martins@gmail.com", "PJServer"), new MailAddress("pj_martins@hotmail.com", "PJ Martins"), subject, body, isBodyHtml);
		//}

		//public static bool Send(MailAddress recipient, string subject, string body, bool isBodyHtml)
		//{
		//}

		//public static bool Send(MailAddress recipient, string subject, string body)
		//{
		//    return Send(recipient, subject, body, false);
		//}

		//public static bool Send(string recipient, string subject, string body, bool isBodyHtml)
		//{
		//    return Send(new MailAddress(recipient), subject, body, isBodyHtml);
		//}

		//public static bool Send(string recipient, string subject, string body)
		//{
		//    return Send(recipient, subject, body, false);
		//}

		//public static bool Send(string subject, string body, bool isBodyHtml)
		//{
		//}

		//public static bool Send(string subject, string body)
		//{
		//    return Send(subject, body, false);
		//}

		//public static bool SendException(Exception ex)
		//{
		//    return Send("Exception thrown in " + System.Reflection.Assembly.GetCallingAssembly().FullName,
		//        "Exception thrown in " + System.Reflection.Assembly.GetCallingAssembly().FullName + "\r\n\r\n" +
		//        "Source: " + ex.Source + "\r\nException: " + Common.GetFullExceptionText(ex));
		//}
	}

	public class EMailException : Exception
	{
		public MailMessage MailMessage { get; private set; }

		public EMailException(MailMessage message, Exception exception)
			: base(exception.Message, exception.InnerException)
		{
			MailMessage = message;
		}
	}
}
