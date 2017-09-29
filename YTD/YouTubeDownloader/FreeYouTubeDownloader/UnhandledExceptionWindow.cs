// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.UnhandledExceptionWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public class UnhandledExceptionWindow : Form, ILocalizableForm
  {
    private const string SmtpHost = "mail.youtubedownloader.com";
    private const string SmtpTo = "bugs@youtubedownloader.com";
    private const string SmtpFrom = "bugs@youtubedownloader.com";
    private const string SmtpSubject = "Bug report ({0})";
    private const string SmtpUser = "smtp@youtubedownloader.com";
    private const string SmtpPassword = "QKMJqg6t";
    private const int SmtpPort = 587;
    private Exception _exception;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel1;
    private PictureBox pictureBox1;
    private Button buttonOk;
    private Button buttonReport;
    private TableLayoutPanel tableLayoutPanel2;
    private Label labelDescription;
    private TableLayoutPanel tableLayoutPanel3;
    private TextBox textBoxBugInfo;

    public UnhandledExceptionWindow()
    {
      this.InitializeComponent();
      FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler) ((sender, args) => this.ApplyCurrentLocalization());
      this.ApplyCurrentLocalization();
      this.ActiveControl = (Control) this.buttonOk;
    }

    public void ApplyCurrentLocalization()
    {
      this.buttonReport.Text = Strings.Report;
      this.labelDescription.Text = Strings.UnhandledErrorTitle;
      this.textBoxBugInfo.Text = Strings.DescribeTheBug;
    }

    internal void Show(Exception exception)
    {
      if (exception != null)
        this._exception = exception;
      this.Show();
    }

    internal void ShowModal(Exception exception)
    {
      if (exception != null)
        this._exception = exception;
      int num = (int) this.ShowDialog();
    }

    private void buttonOk_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void buttonReport_Click(object sender, EventArgs e)
    {
      string description = string.Equals(this.textBoxBugInfo.Text, Strings.DescribeTheBug, StringComparison.OrdinalIgnoreCase) ? "(No description)" : this.textBoxBugInfo.Text;
      string exceptionDump = "(No exception dump)";
      ThreadPool.QueueUserWorkItem((WaitCallback) (foo =>
      {
        try
        {
          MailMessage mailMessage = new MailMessage("bugs@youtubedownloader.com", "bugs@youtubedownloader.com");
          SmtpClient smtpClient = new SmtpClient("mail.youtubedownloader.com", 587);
          smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
          smtpClient.UseDefaultCredentials = false;
          NetworkCredential networkCredential = new NetworkCredential("smtp@youtubedownloader.com", "QKMJqg6t");
          smtpClient.Credentials = (ICredentialsByHost) networkCredential;
          mailMessage.Subject = string.Format("Bug report ({0})", (object) Program.Version);
          mailMessage.Body = string.Format("{0}\n\n{1}", (object) description, (object) exceptionDump);
          MailMessage message = mailMessage;
          smtpClient.Send(message);
        }
        catch
        {
        }
      }));
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void textBoxBugInfo_Enter(object sender, EventArgs e)
    {
      if (!string.Equals(this.textBoxBugInfo.Text, Strings.DescribeTheBug, StringComparison.OrdinalIgnoreCase))
        return;
      this.textBoxBugInfo.Text = string.Empty;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (UnhandledExceptionWindow));
      this.tableLayoutPanel1 = new TableLayoutPanel();
      this.pictureBox1 = new PictureBox();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.labelDescription = new Label();
      this.tableLayoutPanel3 = new TableLayoutPanel();
      this.buttonOk = new Button();
      this.buttonReport = new Button();
      this.textBoxBugInfo = new TextBox();
      this.tableLayoutPanel1.SuspendLayout();
      ((ISupportInitialize) this.pictureBox1).BeginInit();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.Controls.Add((Control) this.pictureBox1, 0, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel2, 1, 0);
      this.tableLayoutPanel1.Controls.Add((Control) this.tableLayoutPanel3, 0, 1);
      this.tableLayoutPanel1.Dock = DockStyle.Fill;
      this.tableLayoutPanel1.Location = new Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 2;
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel1.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel1.Size = new Size(380, 187);
      this.tableLayoutPanel1.TabIndex = 0;
      this.pictureBox1.Image = (Image) Resources.ErrorBig;
      this.pictureBox1.Location = new Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new Size(64, 64);
      this.pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.Controls.Add((Control) this.labelDescription, 0, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.textBoxBugInfo, 0, 1);
      this.tableLayoutPanel2.Dock = DockStyle.Fill;
      this.tableLayoutPanel2.Location = new Point(73, 3);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 2;
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.Size = new Size(304, 145);
      this.tableLayoutPanel2.TabIndex = 3;
      this.labelDescription.AutoSize = true;
      this.labelDescription.Location = new Point(3, 0);
      this.labelDescription.Name = "labelDescription";
      this.labelDescription.Size = new Size(0, 13);
      this.labelDescription.TabIndex = 1;
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel1.SetColumnSpan((Control) this.tableLayoutPanel3, 2);
      this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel3.Controls.Add((Control) this.buttonOk, 1, 0);
      this.tableLayoutPanel3.Controls.Add((Control) this.buttonReport, 0, 0);
      this.tableLayoutPanel3.Dock = DockStyle.Fill;
      this.tableLayoutPanel3.Location = new Point(3, 154);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel3.Size = new Size(374, 30);
      this.tableLayoutPanel3.TabIndex = 4;
      this.buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonOk.Location = new Point(296, 4);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new Size(75, 23);
      this.buttonOk.TabIndex = 1;
      this.buttonOk.Text = "Ok";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
      this.buttonReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      this.buttonReport.Location = new Point(3, 4);
      this.buttonReport.Name = "buttonReport";
      this.buttonReport.Size = new Size(99, 23);
      this.buttonReport.TabIndex = 2;
      this.buttonReport.Text = "Report";
      this.buttonReport.UseVisualStyleBackColor = true;
      this.buttonReport.Click += new EventHandler(this.buttonReport_Click);
      this.textBoxBugInfo.Dock = DockStyle.Fill;
      this.textBoxBugInfo.Location = new Point(3, 22);
      this.textBoxBugInfo.Margin = new Padding(3, 9, 3, 3);
      this.textBoxBugInfo.MaxLength = 1024;
      this.textBoxBugInfo.Multiline = true;
      this.textBoxBugInfo.Name = "textBoxBugInfo";
      this.textBoxBugInfo.Size = new Size(298, 120);
      this.textBoxBugInfo.TabIndex = 2;
      this.textBoxBugInfo.Enter += new EventHandler(this.textBoxBugInfo_Enter);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(254, 254, 254);
      this.ClientSize = new Size(380, 187);
      this.Controls.Add((Control) this.tableLayoutPanel1);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.Name = "UnhandledExceptionWindow";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Oooops!";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      ((ISupportInitialize) this.pictureBox1).EndInit();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
