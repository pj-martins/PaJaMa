// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.ProblemReporterWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common.Reporting.Freshdesk;
using FreeYouTubeDownloader.Helpers;
using FreeYouTubeDownloader.License;
using FreeYouTubeDownloader.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public class ProblemReporterWindow : Form, ILocalizableForm
  {
    private readonly Dictionary<string, TypeOfProblem> _typeOfProblems;
    private IContainer components;
    private TableLayoutPanel tableLayoutPanel;
    private TextBox textBoxTitle;
    private TextBox textBoxDescription;
    private TableLayoutPanel tableLayoutPanel2;
    private TableLayoutPanel tableLayoutPanel3;
    private Button buttonCancel;
    private Button buttonSend;
    private ComboBox comboBoxTypeOfProblem;
    private TextBox textBoxEmail;

    public string Title
    {
      get
      {
        return this.textBoxTitle.Text;
      }
      set
      {
        this.textBoxTitle.Text = value;
      }
    }

    public string Description
    {
      get
      {
        return this.textBoxDescription.Text;
      }
      set
      {
        this.textBoxDescription.Text = value;
      }
    }

    public string Email
    {
      get
      {
        return this.textBoxEmail.Text;
      }
      set
      {
        this.textBoxEmail.Text = value;
      }
    }

    public TypeOfProblem TypeOfProblem
    {
      get
      {
        return this._typeOfProblems[(string) this.comboBoxTypeOfProblem.SelectedItem];
      }
      set
      {
        this.comboBoxTypeOfProblem.SelectedItem = (object) this._typeOfProblems.Single<KeyValuePair<string, TypeOfProblem>>((Func<KeyValuePair<string, TypeOfProblem>, bool>) (keyValue => keyValue.Value == value)).Key;
      }
    }

    public ProblemReporterWindow()
    {
      this.InitializeComponent();
      this.ApplyCurrentLocalization();
      this._typeOfProblems = new Dictionary<string, TypeOfProblem>()
      {
        {
          TypeOfProblem.GeneralTechnicalProblem.GetLocalizedText(),
          TypeOfProblem.GeneralTechnicalProblem
        },
        {
          TypeOfProblem.DownloadErrors.GetLocalizedText(),
          TypeOfProblem.DownloadErrors
        },
        {
          TypeOfProblem.ConversionIssues.GetLocalizedText(),
          TypeOfProblem.ConversionIssues
        }
      };
      this.comboBoxTypeOfProblem.Items.AddRange(this._typeOfProblems.Keys.Cast<object>().ToArray<object>());
      this.comboBoxTypeOfProblem.SelectedIndex = 0;
      this.textBoxTitle.Focus();
    }

    private void buttonCancel_Click(object sender, EventArgs e)
    {
      this.DialogResult = DialogResult.Cancel;
      this.Close();
    }

    private void buttonSend_Click(object sender, EventArgs e)
    {
      FreshdeskClient freshdeskClient = new FreshdeskClient();
      List<FreshdeskAttachment> freshdeskAttachmentList = new List<FreshdeskAttachment>(2);
      try
      {
        byte[] pdfReport = ReportingHelper.CreatePdfReport((Exception) null, this.textBoxDescription.Text);
        freshdeskAttachmentList.Add(new FreshdeskAttachment("report.pdf", "application/pdf", pdfReport));
      }
      catch
      {
        string emailBody = ReportingHelper.CreateEmailBody((Exception) null, this.textBoxDescription.Text);
        freshdeskAttachmentList.Add(new FreshdeskAttachment("report.html", "text/html", Encoding.UTF8.GetBytes(emailBody)));
      }
      if (File.Exists(Settings.Instance.LogFileName))
        freshdeskAttachmentList.Add(new FreshdeskAttachment("log.txt", "text/plain", Encoding.UTF8.GetBytes(File.ReadAllText(Settings.Instance.LogFileName))));
      FreshdeskTicket ticket = new FreshdeskTicket()
      {
        UserEmail = this.textBoxEmail.Text,
        Subject = string.Format("FYTDL: {0} ({1})", (object) this.textBoxTitle.Text, (object) Application.ProductVersion),
        Description = this.textBoxDescription.Text,
        Attachments = freshdeskAttachmentList,
        AppVersion = Application.ProductVersion,
        TypeOfProblem = new TypeOfProblem?(this._typeOfProblems[(string) this.comboBoxTypeOfProblem.SelectedItem]),
        ResponderId = new ulong?(5025617592UL),
        Status = new TicketStatus?(TicketStatus.Open),
        GroupId = new ulong?(LicenseHelper.IsGenuine ? 12000002233UL : 12000002234UL)
      };
      string[] failures;
      if (ticket.Validate(out failures))
      {
        try
        {
          freshdeskClient.PostTicket(ticket);
        }
        catch
        {
        }
      }
      else if (failures.Length != 0)
      {
        int num = (int) MessageBox.Show(string.Join(Environment.NewLine, failures), Strings.WrongValues, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return;
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    public void ApplyCurrentLocalization()
    {
      this.Text = Strings.ReportAProblem;
      this.buttonSend.Text = Strings.Send;
      this.buttonCancel.Text = Strings.Cancel;
      this.textBoxTitle.Text = Strings.NameYourProblem;
      this.textBoxDescription.Text = Strings.DescribeYourProblem;
      this.textBoxEmail.Text = Strings.WeNeedYourEmail;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ProblemReporterWindow));
      this.tableLayoutPanel = new TableLayoutPanel();
      this.textBoxTitle = new TextBox();
      this.textBoxDescription = new TextBox();
      this.tableLayoutPanel2 = new TableLayoutPanel();
      this.comboBoxTypeOfProblem = new ComboBox();
      this.textBoxEmail = new TextBox();
      this.tableLayoutPanel3 = new TableLayoutPanel();
      this.buttonCancel = new Button();
      this.buttonSend = new Button();
      this.tableLayoutPanel.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tableLayoutPanel3.SuspendLayout();
      this.SuspendLayout();
      this.tableLayoutPanel.ColumnCount = 1;
      this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel.Controls.Add((Control) this.textBoxTitle, 0, 0);
      this.tableLayoutPanel.Controls.Add((Control) this.textBoxDescription, 0, 1);
      this.tableLayoutPanel.Controls.Add((Control) this.tableLayoutPanel2, 0, 2);
      this.tableLayoutPanel.Controls.Add((Control) this.tableLayoutPanel3, 0, 3);
      this.tableLayoutPanel.Dock = DockStyle.Fill;
      this.tableLayoutPanel.Location = new Point(0, 0);
      this.tableLayoutPanel.Name = "tableLayoutPanel";
      this.tableLayoutPanel.RowCount = 4;
      this.tableLayoutPanel.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel.RowStyles.Add(new RowStyle());
      this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35f));
      this.tableLayoutPanel.Size = new Size(461, 293);
      this.tableLayoutPanel.TabIndex = 0;
      this.textBoxTitle.Dock = DockStyle.Fill;
      this.textBoxTitle.Location = new Point(3, 3);
      this.textBoxTitle.Name = "textBoxTitle";
      this.textBoxTitle.Size = new Size(455, 20);
      this.textBoxTitle.TabIndex = 1;
      this.textBoxTitle.Text = "Name your problem";
      this.textBoxDescription.Dock = DockStyle.Fill;
      this.textBoxDescription.Location = new Point(3, 29);
      this.textBoxDescription.Multiline = true;
      this.textBoxDescription.Name = "textBoxDescription";
      this.textBoxDescription.Size = new Size(455, 191);
      this.textBoxDescription.TabIndex = 2;
      this.textBoxDescription.Text = "Describe your problem";
      this.tableLayoutPanel2.ColumnCount = 2;
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
      this.tableLayoutPanel2.Controls.Add((Control) this.comboBoxTypeOfProblem, 1, 0);
      this.tableLayoutPanel2.Controls.Add((Control) this.textBoxEmail, 0, 0);
      this.tableLayoutPanel2.Location = new Point(3, 226);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 1;
      this.tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel2.Size = new Size(455, 29);
      this.tableLayoutPanel2.TabIndex = 3;
      this.comboBoxTypeOfProblem.Dock = DockStyle.Top;
      this.comboBoxTypeOfProblem.DropDownStyle = ComboBoxStyle.DropDownList;
      this.comboBoxTypeOfProblem.FormattingEnabled = true;
      this.comboBoxTypeOfProblem.Location = new Point(230, 3);
      this.comboBoxTypeOfProblem.Name = "comboBoxTypeOfProblem";
      this.comboBoxTypeOfProblem.Size = new Size(222, 21);
      this.comboBoxTypeOfProblem.TabIndex = 4;
      this.textBoxEmail.Location = new Point(3, 3);
      this.textBoxEmail.Name = "textBoxEmail";
      this.textBoxEmail.Size = new Size(221, 20);
      this.textBoxEmail.TabIndex = 3;
      this.textBoxEmail.Text = "We need your email to reach you back";
      this.tableLayoutPanel3.ColumnCount = 2;
      this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
      this.tableLayoutPanel3.Controls.Add((Control) this.buttonCancel, 1, 0);
      this.tableLayoutPanel3.Controls.Add((Control) this.buttonSend, 0, 0);
      this.tableLayoutPanel3.Dock = DockStyle.Fill;
      this.tableLayoutPanel3.Location = new Point(3, 261);
      this.tableLayoutPanel3.Name = "tableLayoutPanel3";
      this.tableLayoutPanel3.RowCount = 1;
      this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
      this.tableLayoutPanel3.Size = new Size(455, 29);
      this.tableLayoutPanel3.TabIndex = 4;
      this.buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonCancel.Location = new Point(377, 3);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new Size(75, 23);
      this.buttonCancel.TabIndex = 6;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
      this.buttonSend.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.buttonSend.Location = new Point(296, 3);
      this.buttonSend.Name = "buttonSend";
      this.buttonSend.Size = new Size(75, 23);
      this.buttonSend.TabIndex = 5;
      this.buttonSend.Text = "Send";
      this.buttonSend.UseVisualStyleBackColor = true;
      this.buttonSend.Click += new EventHandler(this.buttonSend_Click);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(254, 254, 254);
      this.ClientSize = new Size(461, 293);
      this.Controls.Add((Control) this.tableLayoutPanel);
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.Name = "ProblemReporterWindow";
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Report a problem";
      this.tableLayoutPanel.ResumeLayout(false);
      this.tableLayoutPanel.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.tableLayoutPanel3.ResumeLayout(false);
      this.ResumeLayout(false);
    }
  }
}
