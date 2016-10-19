using AE.Net.Mail;
using PaJaMa.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace PaJaMa.RestartMe
{
    public partial class svcMain : ServiceBase
    {
        const string PASSWORD = "__Re$t4rt__Me__";
        private Timer _timer;
        private string _smtpServer;
        private int _smtpPort;
        private bool _useSsl;
        private string _imapServer;
        private int _imapPort;
        private bool _imapUseSsl;
        private string _userName;
        private string _password;
        private string _sendTo;
        private string _sendFrom;
        private DateTime _lastCheck;

        public svcMain()
        {
            InitializeComponent();
            _smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
            _smtpPort = Convert.ToInt16(ConfigurationManager.AppSettings["SMTPPort"]);
            _useSsl = Common.Parse.ParseBool(ConfigurationManager.AppSettings["UseSSL"]);
            _imapServer = ConfigurationManager.AppSettings["IMAPServer"];
            _imapPort = Convert.ToInt16(ConfigurationManager.AppSettings["IMAPPort"]);
            _imapUseSsl = Common.Parse.ParseBool(ConfigurationManager.AppSettings["IMAPUseSSL"]);
            _userName = ConfigurationManager.AppSettings["UserName"];
            _password = EncrypterDecrypter.Instance.Decrypt(ConfigurationManager.AppSettings["Password"], PASSWORD);
            _sendTo = ConfigurationManager.AppSettings["SendTo"];
            _sendFrom = ConfigurationManager.AppSettings["SendFrom"];
            _lastCheck = DateTime.Now;
        }

        private void sendEmail(string subject, string body)
        {
            using (var smtp = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtp.EnableSsl = _useSsl;
                smtp.Credentials = new NetworkCredential(_userName, _password);
                using (var msg = new System.Net.Mail.MailMessage(_sendFrom, _sendTo, subject, body))
                {
                    smtp.Send(msg);
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            sendEmail("PJLAPTOP RESTARTED", "PJLAPTOP RESTARTED");
            _timer = new Timer();
            _timer.Elapsed += _timer_Elapsed;
            _timer.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
            _timer_Elapsed(_timer, null);
        }

        private void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            using (var client = new ImapClient(_imapServer, _userName, _password, AuthMethods.Login, _imapPort, _imapUseSsl))
            {
                int msgCount = client.GetMessageCount();
                var msg = client.GetMessages(msgCount - 10, msgCount)
                    .FirstOrDefault(m => m.Subject.ToLower().Contains("pj restart me") && m.Date >= _lastCheck.AddMinutes(-2));
                if (msg != null)
                {
                    sendEmail("PJ RESTARTING", "PJ RESTARTING");
                    try
                    {
                        client.DeleteMessage(msg);
                    }
                    catch { }
                    Process.Start("shutdown", "/r /f");
                }
            }
            _lastCheck = DateTime.Now;
            _timer.Start();
        }

        protected override void OnStop()
        {
            _timer.Stop();
            _timer.Dispose();
            _timer = null;
        }

#if DEBUG
        public void Start()
        {
            OnStart(null);
        }

        public void End()
        {
            OnStop();
        }
#endif
    }
}
