using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;

namespace PaJaMa.Common
{
	public class Pop3Client
	{
		public string Host { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
		public int Port { get; set; }
		private TcpClient _tcpClient = null;
		private SslStream _netStream = null;
		byte[] _writeBuffer = new byte[1024];
		ASCIIEncoding _encoder = new System.Text.ASCIIEncoding();
		StreamReader _streamReader = null;

		public Pop3Client()
		{ }

		public Pop3Client(string host, string userName, string password)
			: this(host, userName, password, false)
		{ }

		public Pop3Client(string host, string userName, string password, bool useSsl)
		{
			this.Host = host;
			this.UserName = userName;
			this.Password = password;
			this.Port = (useSsl ? 995 : 110);
			Connect();
		}

		public void Connect()
		{
			_tcpClient = new TcpClient(this.Host, this.Port);
			_netStream = new SslStream(_tcpClient.GetStream());
			_netStream.AuthenticateAsClient(this.Host);
			_streamReader = new StreamReader(_netStream);
			Console.WriteLine(_streamReader.ReadLine());

			_writeBuffer = _encoder.GetBytes("USER " + this.UserName + "\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			_netStream.Flush();
			Console.WriteLine(_streamReader.ReadLine());

			_writeBuffer = _encoder.GetBytes("PASS " + this.Password + "\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			_netStream.Flush();
			Console.WriteLine(_streamReader.ReadLine());

		}

		public string GetList()
		{
			StringBuilder sb = new StringBuilder();
			_writeBuffer = _encoder.GetBytes("LIST\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			Console.WriteLine(_streamReader.ReadLine());

			String listMessage;
			while (true)
			{
				listMessage = _streamReader.ReadLine();
				if (listMessage == ".")
				{
					break;
				}
				else
				{
					sb.AppendLine(listMessage);
					continue;
				}
			}

			return sb.ToString();
		}

		//public string GetList(int listNumber)
		//{
		//    _writeBuffer = _encoder.GetBytes("LIST " + listNumber.ToString() + "\r\n");
		//    _netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
		//    return _streamReader.ReadLine();
		//}

		public void DeleteMessage(int listNumber)
		{
			_writeBuffer = _encoder.GetBytes("DELE " + listNumber.ToString() + "\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			_netStream.Flush();
			Console.WriteLine(_streamReader.ReadLine());

		}

		public void Quit()
		{
			_writeBuffer = _encoder.GetBytes("QUIT\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			_netStream.Flush();
			Console.WriteLine(_streamReader.ReadLine());
		}

		public Message GetMessage(int listNumber)
		{
			return GetMessage(listNumber, false);
		}

		public Message GetMessage(int listNumber, bool headerOnly)
		{
			StringBuilder sb = new StringBuilder();
			_writeBuffer = _encoder.GetBytes((headerOnly ? "TOP " : "RETR ") + listNumber.ToString() + 
				(headerOnly ? " 0" : "") + "\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			string line = _streamReader.ReadLine();
			if (line[0] != '-')	//errors begins with -
			{
				while (line != ".")	// . - is the end of the server response
				{
					sb.AppendLine(line);
					line = _streamReader.ReadLine();
				}
			}
			else
			{
				return null;
			}
			Message rtv = new Message(sb.ToString(), headerOnly);
			Console.WriteLine(listNumber.ToString() + 
				(rtv != null ? " (" + rtv.Date + ") - " + rtv.Subject : string.Empty));
			return rtv;
		}
	}
}
