using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;

namespace PaJaMaCommon
{
	public class TcpConnector
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public bool UseSSL { get; set; }
		private TcpClient _tcpClient = null;
		private Stream _netStream = null;
		private byte[] _writeBuffer = new byte[1024];
		ASCIIEncoding _encoder = new System.Text.ASCIIEncoding();

		public TcpConnector() {}

		public TcpConnector(string host, int port) : this(host, port, false)
		{
		}

		~TcpConnector()
		{
			try
			{
				_netStream.Close();
				_tcpClient.Close();
			}
			catch { }
		}

		public TcpConnector(string host, int port, bool useSSL)
		{
			this.Host = host;
			this.Port = port;
			this.UseSSL = useSSL;
		}


		public string Connect()
		{
			_tcpClient = new TcpClient(this.Host, this.Port);
			if (this.UseSSL)
			{
				_netStream = new SslStream(_tcpClient.GetStream());
				((SslStream)_netStream).AuthenticateAsClient(this.Host);
			}
			else
			{
				_netStream = _tcpClient.GetStream();
			}
			return Common.GetStringFromStream(_netStream);
		}

		public string Send(string input)
		{
			_writeBuffer = _encoder.GetBytes(input + "\r\n");
			_netStream.Write(_writeBuffer, 0, _writeBuffer.Length);
			_netStream.Flush();
			return Common.GetStringFromStream(_netStream);
		}
	}
}
