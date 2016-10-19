using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PaJaMa.Common
{
	public class Message
	{
		public string From { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
		public string Date { get; set; }

		public Message(string rawMsg)
			: this(rawMsg, false)
		{ }

		public Message(string rawMsg, bool headerOnly)
		{
			this.From = GetMatch(rawMsg, "\r\nFrom:(.*)\r\n");
			this.Date = GetMatch(rawMsg, "\r\nDate:(.*)\r\n");
			this.Subject = GetMatch(rawMsg, "\r\nSubject:(.*)\r\n");
			if (!headerOnly)
				this.Body = rawMsg.Substring(rawMsg.IndexOf("\r\n\r\n") + 4);
		}

		public string GetMatch(string input, string pattern)
		{
			Match m;
			m = Regex.Match(input, pattern);
			if (m.Success)
				return m.Groups[1].Value.Trim();
			return null;
		}
	}
}
