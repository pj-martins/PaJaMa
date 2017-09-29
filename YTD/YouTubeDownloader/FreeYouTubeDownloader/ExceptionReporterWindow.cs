// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.ExceptionReporterWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Common.Reporting.Freshdesk;
using FreeYouTubeDownloader.Helpers;
using FreeYouTubeDownloader.License;
using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public class ExceptionReporterWindow : Form, ILocalizableForm
  {
    private Exception _exception;
    private IContainer components;
    private TabControl tabs;
    private TabPage tabPageGeneral;
    private TabPage tabPageException;
    private PictureBox pictureBoxErrorBig;
    private TabPage tabPageSystem;
    private TextBox textBoxTime;
    private TextBox textBoxApplication;
    private TextBox textBoxDate;
    private TextBox textBoxVersion;
    private Label lblDescribeTheBug;
    private Label lblTime;
    private Label lblApplication;
    private Label lblDate;
    private Label lblVersion;
    private TextBox textBoxUserMessage;
    private Button btnCancel;
    private Button btnSendReport;
    private Label lblMessage;
    private Label lblErrorMessage;
    private Label lblStackTrace;
    private TextBox textBoxStackTrace;
    private TextBox textBoxErrorMessage;
    private TextBox textBoxTarget;
    private Label lblTarget;
    private TextBox textBoxTypeFullName;
    private Label lblTypeFullName;
    private TreeView treeViewSysInfo;
    private Label lblSystemInformation;
    private Label labelEmail;
    private TextBox textBoxEmail;

    public ExceptionReporterWindow()
    {
      this.InitializeComponent();
      FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler) ((sender, args) => this.ApplyCurrentLocalization());
      this.ApplyCurrentLocalization();
      this.InitializeTagsTabs();
      this.InitializeTabPageGeneral(0);
      this.ActiveControl = (Control) this.btnSendReport;
    }

    public void ApplyCurrentLocalization()
    {
      this.tabs.TabPages[0].Text = Strings.General;
      this.lblMessage.Text = Strings.ExceptionReporterMessage;
      this.lblApplication.Text = Strings.Application;
      this.lblVersion.Text = Strings.Version;
      this.lblDate.Text = Strings.Date;
      this.lblTime.Text = Strings.Time;
      this.lblDescribeTheBug.Text = Strings.DescribeTheBug;
      this.tabs.TabPages[1].Text = Strings.Exception;
      this.lblTypeFullName.Text = Strings.TypeFullName;
      this.lblTarget.Text = Strings.Target;
      this.lblErrorMessage.Text = Strings.ErrorMessage;
      this.lblStackTrace.Text = Strings.StackTrace;
      this.tabs.TabPages[2].Text = Strings.System;
      this.lblSystemInformation.Text = Strings.SystemInformation;
      this.labelEmail.Text = Strings.YourEmailToBeNotifiedAboutFix;
      this.btnCancel.Text = Strings.Cancel;
      this.btnSendReport.Text = Strings.SendReport;
    }

    private void InitializeTagsTabs()
    {
      foreach (Control tabPage in this.tabs.TabPages)
        tabPage.Tag = (object) false;
    }

    private void InitializeTabPageGeneral(int selectedIndex)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        this.textBoxApplication.Text = Application.ProductName;
        this.textBoxVersion.Text = Application.ProductVersion;
        DateTime now = DateTime.Now;
        this.textBoxDate.Text = now.ToShortDateString();
        this.textBoxTime.Text = now.ToString("hh:mm tt", (IFormatProvider) new CultureInfo("en-US"));
        this.tabs.TabPages[selectedIndex].Tag = (object) true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private void InitializeTabPageException(int selectedIndex)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        MemberInfo targetSite = (MemberInfo) this._exception.TargetSite;
        this.textBoxTypeFullName.Text = targetSite.DeclaringType.ToString();
        this.textBoxTarget.Text = string.Format("{0} {1}", (object) targetSite.MemberType, (object) targetSite.Name);
        this.textBoxErrorMessage.Text = this._exception.Message;
        this.textBoxStackTrace.Text = this._exception.StackTrace;
        this.tabs.TabPages[selectedIndex].Tag = (object) true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private void InitializeTabPageSystem(int selectedIndex)
    {
      this.Cursor = Cursors.WaitCursor;
      try
      {
        this.treeViewSysInfo.Nodes.Clear();
        this.GetSystemInfo();
        this.tabs.TabPages[selectedIndex].Tag = (object) true;
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message);
      }
      finally
      {
        this.Cursor = Cursors.Default;
      }
    }

    private TreeNodeCollection CreateParentNode(string labelText)
    {
      TreeNode node = new TreeNode(labelText);
      this.treeViewSysInfo.Nodes.Add(node);
      return node.Nodes;
    }

    private void CreateChildNode(TreeNodeCollection nodeCollection, string labelText)
    {
      TreeNode node = new TreeNode(labelText);
      nodeCollection.Add(node);
    }

    private void GetSystemInfo()
    {
      try
      {
        this.treeViewSysInfo.Nodes.Clear();
        Dictionary<string, string> operatingSystemInfo = ReportingHelper.OperatingSystemInfo;
        int index1 = 0;
        TreeNodeCollection parentNode1 = this.CreateParentNode("User Info");
        string format1 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair1 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index1);
        string key1 = keyValuePair1.Key;
        keyValuePair1 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index1);
        string str1 = keyValuePair1.Value;
        string labelText1 = string.Format(format1, (object) key1, (object) str1);
        this.CreateChildNode(parentNode1, labelText1);
        int index2 = index1 + 1;
        this.Update();
        TreeNodeCollection parentNode2 = this.CreateParentNode("Operating System");
        TreeNodeCollection nodeCollection1 = parentNode2;
        string format2 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair2 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index2);
        string key2 = keyValuePair2.Key;
        keyValuePair2 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index2);
        string str2 = keyValuePair2.Value;
        string labelText2 = string.Format(format2, (object) key2, (object) str2);
        this.CreateChildNode(nodeCollection1, labelText2);
        int index3 = index2 + 1;
        TreeNodeCollection nodeCollection2 = parentNode2;
        string format3 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair3 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index3);
        string key3 = keyValuePair3.Key;
        keyValuePair3 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index3);
        string str3 = keyValuePair3.Value;
        string labelText3 = string.Format(format3, (object) key3, (object) str3);
        this.CreateChildNode(nodeCollection2, labelText3);
        int index4 = index3 + 1;
        TreeNodeCollection nodeCollection3 = parentNode2;
        string format4 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair4 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index4);
        string key4 = keyValuePair4.Key;
        keyValuePair4 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index4);
        string str4 = keyValuePair4.Value;
        string labelText4 = string.Format(format4, (object) key4, (object) str4);
        this.CreateChildNode(nodeCollection3, labelText4);
        int index5 = index4 + 1;
        this.Update();
        TreeNodeCollection parentNode3 = this.CreateParentNode(".Net Framework Versions");
        TreeNodeCollection nodeCollection4 = parentNode3;
        string format5 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair5 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index5);
        string key5 = keyValuePair5.Key;
        keyValuePair5 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index5);
        string str5 = keyValuePair5.Value;
        string labelText5 = string.Format(format5, (object) key5, (object) str5);
        this.CreateChildNode(nodeCollection4, labelText5);
        int index6 = index5 + 1;
        TreeNodeCollection nodeCollection5 = parentNode3;
        string format6 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair6 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index6);
        string key6 = keyValuePair6.Key;
        keyValuePair6 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index6);
        string str6 = keyValuePair6.Value;
        string labelText6 = string.Format(format6, (object) key6, (object) str6);
        this.CreateChildNode(nodeCollection5, labelText6);
        int index7 = index6 + 1;
        this.Update();
        TreeNodeCollection parentNode4 = this.CreateParentNode("Computer System");
        TreeNodeCollection nodeCollection6 = parentNode4;
        string format7 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair7 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index7);
        string key7 = keyValuePair7.Key;
        keyValuePair7 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index7);
        string str7 = keyValuePair7.Value;
        string labelText7 = string.Format(format7, (object) key7, (object) str7);
        this.CreateChildNode(nodeCollection6, labelText7);
        int index8 = index7 + 1;
        TreeNodeCollection nodeCollection7 = parentNode4;
        string format8 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair8 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index8);
        string key8 = keyValuePair8.Key;
        keyValuePair8 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index8);
        string str8 = keyValuePair8.Value;
        string labelText8 = string.Format(format8, (object) key8, (object) str8);
        this.CreateChildNode(nodeCollection7, labelText8);
        int index9 = index8 + 1;
        this.Update();
        TreeNodeCollection parentNode5 = this.CreateParentNode("System Time Zone");
        string format9 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair9 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index9);
        string key9 = keyValuePair9.Key;
        keyValuePair9 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index9);
        string str9 = keyValuePair9.Value;
        string labelText9 = string.Format(format9, (object) key9, (object) str9);
        this.CreateChildNode(parentNode5, labelText9);
        int index10 = index9 + 1;
        TreeNodeCollection parentNode6 = this.CreateParentNode("Anti Viruses");
        string format10 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair10 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index10);
        string key10 = keyValuePair10.Key;
        keyValuePair10 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index10);
        string str10 = keyValuePair10.Value;
        string labelText10 = string.Format(format10, (object) key10, (object) str10);
        this.CreateChildNode(parentNode6, labelText10);
        int index11 = index10 + 1;
        TreeNodeCollection parentNode7 = this.CreateParentNode("Network");
        TreeNodeCollection nodeCollection8 = parentNode7;
        string format11 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair11 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index11);
        string key11 = keyValuePair11.Key;
        keyValuePair11 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index11);
        string str11 = keyValuePair11.Value;
        string labelText11 = string.Format(format11, (object) key11, (object) str11);
        this.CreateChildNode(nodeCollection8, labelText11);
        int index12 = index11 + 1;
        TreeNodeCollection nodeCollection9 = parentNode7;
        string format12 = "{0}: {1}";
        KeyValuePair<string, string> keyValuePair12 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index12);
        string key12 = keyValuePair12.Key;
        keyValuePair12 = operatingSystemInfo.ElementAt<KeyValuePair<string, string>>(index12);
        string str12 = keyValuePair12.Value;
        string labelText12 = string.Format(format12, (object) key12, (object) str12);
        this.CreateChildNode(nodeCollection9, labelText12);
        int num = index12 + 1;
      }
      catch
      {
      }
      this.Update();
      this.treeViewSysInfo.ExpandAll();
    }

    private bool GetStatusPageByIndex(int selectedIndex)
    {
      bool flag = false;
      if (selectedIndex < this.tabs.TabPages.Count)
        flag = (bool) this.tabs.TabPages[selectedIndex].Tag;
      return flag;
    }

    internal void Show(Exception exception)
    {
      if (exception == null)
        return;
      this._exception = exception;
      this.Show();
    }

    internal void ShowModal(Exception exception)
    {
      if (exception == null)
        return;
      this._exception = exception;
      int num = (int) this.ShowDialog();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    private void btnSend_Click(object sender, EventArgs e)
    {
      string appSetting = ConfigurationManager.AppSettings["receiver"];
      string str = string.Format("FYTDL: Bug report ({0})", (object) Program.Version);
      List<FreshdeskAttachment> freshdeskAttachmentList = new List<FreshdeskAttachment>();
      try
      {
        byte[] pdfReport = ReportingHelper.CreatePdfReport(this._exception, this.textBoxUserMessage.Text);
        freshdeskAttachmentList.Add(new FreshdeskAttachment("report.pdf", "application/pdf", pdfReport));
      }
      catch
      {
        string emailBody = ReportingHelper.CreateEmailBody(this._exception, this.textBoxUserMessage.Text);
        freshdeskAttachmentList.Add(new FreshdeskAttachment("report.html", "text/html", Encoding.UTF8.GetBytes(emailBody)));
      }
      if (!string.IsNullOrWhiteSpace(appSetting) && !string.IsNullOrWhiteSpace(str))
      {
        FreshdeskClient freshdeskClient = new FreshdeskClient();
        bool flag = !string.IsNullOrEmpty(this.textBoxEmail.Text);
        if (File.Exists(Settings.Instance.LogFileName))
          freshdeskAttachmentList.Add(new FreshdeskAttachment("log.txt", "text/plain", Encoding.UTF8.GetBytes(File.ReadAllText(Settings.Instance.LogFileName))));
        FreshdeskTicket ticket = new FreshdeskTicket()
        {
          UserEmail = flag ? this.textBoxEmail.Text : "nazar.grynko@vitzo.com",
          Subject = str,
          Description = this.textBoxUserMessage.Text,
          Attachments = freshdeskAttachmentList,
          AppVersion = Application.ProductVersion,
          TypeOfProblem = new TypeOfProblem?(TypeOfProblem.UnhandledException),
          ResponderId = new ulong?(5007904674UL),
          Status = new TicketStatus?(flag ? TicketStatus.Open : TicketStatus.Closed),
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
      }
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void tabs_SelectedIndexChanged(object sender, EventArgs e)
    {
      int selectedIndex = ((TabControl) sender).SelectedIndex;
      switch (selectedIndex)
      {
        case 0:
          if (this.GetStatusPageByIndex(selectedIndex))
            break;
          this.InitializeTabPageGeneral(selectedIndex);
          break;
        case 1:
          if (this.GetStatusPageByIndex(selectedIndex))
            break;
          this.InitializeTabPageException(selectedIndex);
          break;
        case 2:
          if (this.GetStatusPageByIndex(selectedIndex))
            break;
          this.InitializeTabPageSystem(selectedIndex);
          break;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (ExceptionReporterWindow));
      this.tabs = new TabControl();
      this.tabPageGeneral = new TabPage();
      this.lblMessage = new Label();
      this.textBoxUserMessage = new TextBox();
      this.textBoxTime = new TextBox();
      this.textBoxApplication = new TextBox();
      this.textBoxDate = new TextBox();
      this.textBoxVersion = new TextBox();
      this.lblDescribeTheBug = new Label();
      this.lblTime = new Label();
      this.lblApplication = new Label();
      this.lblDate = new Label();
      this.lblVersion = new Label();
      this.pictureBoxErrorBig = new PictureBox();
      this.tabPageException = new TabPage();
      this.textBoxTypeFullName = new TextBox();
      this.lblTypeFullName = new Label();
      this.textBoxTarget = new TextBox();
      this.lblTarget = new Label();
      this.lblErrorMessage = new Label();
      this.lblStackTrace = new Label();
      this.textBoxStackTrace = new TextBox();
      this.textBoxErrorMessage = new TextBox();
      this.tabPageSystem = new TabPage();
      this.lblSystemInformation = new Label();
      this.treeViewSysInfo = new TreeView();
      this.btnCancel = new Button();
      this.btnSendReport = new Button();
      this.labelEmail = new Label();
      this.textBoxEmail = new TextBox();
      this.tabs.SuspendLayout();
      this.tabPageGeneral.SuspendLayout();
      ((ISupportInitialize) this.pictureBoxErrorBig).BeginInit();
      this.tabPageException.SuspendLayout();
      this.tabPageSystem.SuspendLayout();
      this.SuspendLayout();
      this.tabs.Controls.Add((Control) this.tabPageGeneral);
      this.tabs.Controls.Add((Control) this.tabPageException);
      this.tabs.Controls.Add((Control) this.tabPageSystem);
      this.tabs.Location = new Point(7, 8);
      this.tabs.Name = "tabs";
      this.tabs.SelectedIndex = 0;
      this.tabs.Size = new Size(471, 250);
      this.tabs.TabIndex = 0;
      this.tabs.SelectedIndexChanged += new EventHandler(this.tabs_SelectedIndexChanged);
      this.tabPageGeneral.Controls.Add((Control) this.textBoxEmail);
      this.tabPageGeneral.Controls.Add((Control) this.labelEmail);
      this.tabPageGeneral.Controls.Add((Control) this.lblMessage);
      this.tabPageGeneral.Controls.Add((Control) this.textBoxUserMessage);
      this.tabPageGeneral.Controls.Add((Control) this.textBoxTime);
      this.tabPageGeneral.Controls.Add((Control) this.textBoxApplication);
      this.tabPageGeneral.Controls.Add((Control) this.textBoxDate);
      this.tabPageGeneral.Controls.Add((Control) this.textBoxVersion);
      this.tabPageGeneral.Controls.Add((Control) this.lblDescribeTheBug);
      this.tabPageGeneral.Controls.Add((Control) this.lblTime);
      this.tabPageGeneral.Controls.Add((Control) this.lblApplication);
      this.tabPageGeneral.Controls.Add((Control) this.lblDate);
      this.tabPageGeneral.Controls.Add((Control) this.lblVersion);
      this.tabPageGeneral.Controls.Add((Control) this.pictureBoxErrorBig);
      this.tabPageGeneral.Location = new Point(4, 22);
      this.tabPageGeneral.Name = "tabPageGeneral";
      this.tabPageGeneral.Padding = new Padding(3);
      this.tabPageGeneral.Size = new Size(463, 224);
      this.tabPageGeneral.TabIndex = 0;
      this.tabPageGeneral.Tag = (object) "";
      this.tabPageGeneral.Text = "General";
      this.tabPageGeneral.UseVisualStyleBackColor = true;
      this.lblMessage.Location = new Point(54, 6);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(400, 39);
      this.lblMessage.TabIndex = 14;
      this.lblMessage.Text = " Message";
      this.textBoxUserMessage.Location = new Point(3, 133);
      this.textBoxUserMessage.Multiline = true;
      this.textBoxUserMessage.Name = "textBoxUserMessage";
      this.textBoxUserMessage.ScrollBars = ScrollBars.Vertical;
      this.textBoxUserMessage.Size = new Size(454, 86);
      this.textBoxUserMessage.TabIndex = 13;
      this.textBoxTime.Location = new Point(298, 88);
      this.textBoxTime.Name = "textBoxTime";
      this.textBoxTime.ReadOnly = true;
      this.textBoxTime.Size = new Size(159, 20);
      this.textBoxTime.TabIndex = 12;
      this.textBoxTime.Visible = false;
      this.textBoxApplication.Location = new Point(74, 62);
      this.textBoxApplication.Name = "textBoxApplication";
      this.textBoxApplication.ReadOnly = true;
      this.textBoxApplication.Size = new Size(159, 20);
      this.textBoxApplication.TabIndex = 11;
      this.textBoxApplication.Visible = false;
      this.textBoxDate.Location = new Point(298, 62);
      this.textBoxDate.Name = "textBoxDate";
      this.textBoxDate.ReadOnly = true;
      this.textBoxDate.Size = new Size(159, 20);
      this.textBoxDate.TabIndex = 10;
      this.textBoxDate.Visible = false;
      this.textBoxVersion.Location = new Point(74, 88);
      this.textBoxVersion.Name = "textBoxVersion";
      this.textBoxVersion.ReadOnly = true;
      this.textBoxVersion.Size = new Size(159, 20);
      this.textBoxVersion.TabIndex = 9;
      this.textBoxVersion.Visible = false;
      this.lblDescribeTheBug.AutoSize = true;
      this.lblDescribeTheBug.Location = new Point(3, 117);
      this.lblDescribeTheBug.Name = "lblDescribeTheBug";
      this.lblDescribeTheBug.Size = new Size(93, 13);
      this.lblDescribeTheBug.TabIndex = 7;
      this.lblDescribeTheBug.Text = "Describe The Bug";
      this.lblTime.AutoSize = true;
      this.lblTime.Location = new Point(260, 91);
      this.lblTime.Name = "lblTime";
      this.lblTime.Size = new Size(30, 13);
      this.lblTime.TabIndex = 6;
      this.lblTime.Text = "Time";
      this.lblTime.Visible = false;
      this.lblApplication.AutoSize = true;
      this.lblApplication.Location = new Point(3, 65);
      this.lblApplication.Name = "lblApplication";
      this.lblApplication.Size = new Size(59, 13);
      this.lblApplication.TabIndex = 5;
      this.lblApplication.Text = "Application";
      this.lblApplication.Visible = false;
      this.lblDate.AutoSize = true;
      this.lblDate.Location = new Point(260, 65);
      this.lblDate.Name = "lblDate";
      this.lblDate.Size = new Size(30, 13);
      this.lblDate.TabIndex = 4;
      this.lblDate.Text = "Date";
      this.lblDate.Visible = false;
      this.lblVersion.AutoSize = true;
      this.lblVersion.Location = new Point(3, 91);
      this.lblVersion.Name = "lblVersion";
      this.lblVersion.Size = new Size(42, 13);
      this.lblVersion.TabIndex = 3;
      this.lblVersion.Text = "Version";
      this.lblVersion.Visible = false;
      this.pictureBoxErrorBig.Image = (Image) Resources.ErrorBig;
      this.pictureBoxErrorBig.Location = new Point(6, 6);
      this.pictureBoxErrorBig.Name = "pictureBoxErrorBig";
      this.pictureBoxErrorBig.Size = new Size(42, 40);
      this.pictureBoxErrorBig.SizeMode = PictureBoxSizeMode.Zoom;
      this.pictureBoxErrorBig.TabIndex = 0;
      this.pictureBoxErrorBig.TabStop = false;
      this.tabPageException.Controls.Add((Control) this.textBoxTypeFullName);
      this.tabPageException.Controls.Add((Control) this.lblTypeFullName);
      this.tabPageException.Controls.Add((Control) this.textBoxTarget);
      this.tabPageException.Controls.Add((Control) this.lblTarget);
      this.tabPageException.Controls.Add((Control) this.lblErrorMessage);
      this.tabPageException.Controls.Add((Control) this.lblStackTrace);
      this.tabPageException.Controls.Add((Control) this.textBoxStackTrace);
      this.tabPageException.Controls.Add((Control) this.textBoxErrorMessage);
      this.tabPageException.Location = new Point(4, 22);
      this.tabPageException.Name = "tabPageException";
      this.tabPageException.Padding = new Padding(3);
      this.tabPageException.Size = new Size(463, 224);
      this.tabPageException.TabIndex = 1;
      this.tabPageException.Text = "Exception";
      this.tabPageException.UseVisualStyleBackColor = true;
      this.textBoxTypeFullName.Location = new Point(100, 19);
      this.textBoxTypeFullName.Name = "textBoxTypeFullName";
      this.textBoxTypeFullName.ReadOnly = true;
      this.textBoxTypeFullName.Size = new Size(356, 20);
      this.textBoxTypeFullName.TabIndex = 7;
      this.lblTypeFullName.AutoSize = true;
      this.lblTypeFullName.Location = new Point(6, 22);
      this.lblTypeFullName.Name = "lblTypeFullName";
      this.lblTypeFullName.Size = new Size(76, 13);
      this.lblTypeFullName.TabIndex = 6;
      this.lblTypeFullName.Text = "Type full name";
      this.textBoxTarget.Location = new Point(100, 45);
      this.textBoxTarget.Name = "textBoxTarget";
      this.textBoxTarget.ReadOnly = true;
      this.textBoxTarget.Size = new Size(356, 20);
      this.textBoxTarget.TabIndex = 5;
      this.lblTarget.AutoSize = true;
      this.lblTarget.Location = new Point(6, 48);
      this.lblTarget.Name = "lblTarget";
      this.lblTarget.Size = new Size(38, 13);
      this.lblTarget.TabIndex = 4;
      this.lblTarget.Text = "Target";
      this.lblErrorMessage.AutoSize = true;
      this.lblErrorMessage.Location = new Point(6, 81);
      this.lblErrorMessage.Name = "lblErrorMessage";
      this.lblErrorMessage.Size = new Size(74, 13);
      this.lblErrorMessage.TabIndex = 3;
      this.lblErrorMessage.Text = "Error message";
      this.lblStackTrace.AutoSize = true;
      this.lblStackTrace.Location = new Point(6, 140);
      this.lblStackTrace.Name = "lblStackTrace";
      this.lblStackTrace.Size = new Size(62, 13);
      this.lblStackTrace.TabIndex = 2;
      this.lblStackTrace.Text = "Stack trace";
      this.textBoxStackTrace.Location = new Point(6, 156);
      this.textBoxStackTrace.Multiline = true;
      this.textBoxStackTrace.Name = "textBoxStackTrace";
      this.textBoxStackTrace.ReadOnly = true;
      this.textBoxStackTrace.ScrollBars = ScrollBars.Vertical;
      this.textBoxStackTrace.Size = new Size(450, 62);
      this.textBoxStackTrace.TabIndex = 1;
      this.textBoxErrorMessage.Location = new Point(7, 97);
      this.textBoxErrorMessage.Multiline = true;
      this.textBoxErrorMessage.Name = "textBoxErrorMessage";
      this.textBoxErrorMessage.ReadOnly = true;
      this.textBoxErrorMessage.ScrollBars = ScrollBars.Vertical;
      this.textBoxErrorMessage.Size = new Size(450, 35);
      this.textBoxErrorMessage.TabIndex = 0;
      this.tabPageSystem.Controls.Add((Control) this.lblSystemInformation);
      this.tabPageSystem.Controls.Add((Control) this.treeViewSysInfo);
      this.tabPageSystem.Location = new Point(4, 22);
      this.tabPageSystem.Name = "tabPageSystem";
      this.tabPageSystem.Padding = new Padding(3);
      this.tabPageSystem.Size = new Size(463, 224);
      this.tabPageSystem.TabIndex = 2;
      this.tabPageSystem.Text = "System";
      this.tabPageSystem.UseVisualStyleBackColor = true;
      this.lblSystemInformation.AutoSize = true;
      this.lblSystemInformation.Location = new Point(7, 7);
      this.lblSystemInformation.Name = "lblSystemInformation";
      this.lblSystemInformation.Size = new Size(95, 13);
      this.lblSystemInformation.TabIndex = 1;
      this.lblSystemInformation.Text = "System information";
      this.treeViewSysInfo.Location = new Point(7, 28);
      this.treeViewSysInfo.Name = "treeViewSysInfo";
      this.treeViewSysInfo.Size = new Size(450, 190);
      this.treeViewSysInfo.TabIndex = 0;
      this.btnCancel.DialogResult = DialogResult.Cancel;
      this.btnCancel.Location = new Point(13, 262);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new Size(100, 26);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
      this.btnSendReport.Location = new Point(368, 262);
      this.btnSendReport.Name = "btnSendReport";
      this.btnSendReport.Size = new Size(100, 26);
      this.btnSendReport.TabIndex = 2;
      this.btnSendReport.Text = "Send Report";
      this.btnSendReport.UseVisualStyleBackColor = true;
      this.btnSendReport.Click += new EventHandler(this.btnSend_Click);
      this.labelEmail.AutoSize = true;
      this.labelEmail.Location = new Point(3, 65);
      this.labelEmail.Name = "labelEmail";
      this.labelEmail.Size = new Size(262, 13);
      this.labelEmail.TabIndex = 15;
      this.labelEmail.Text = "Leave us your email if you want to be notified about fix";
      this.textBoxEmail.Location = new Point(2, 85);
      this.textBoxEmail.Name = "textBoxEmail";
      this.textBoxEmail.Size = new Size(455, 20);
      this.textBoxEmail.TabIndex = 16;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = Color.FromArgb(254, 254, 254);
      this.CancelButton = (IButtonControl) this.btnCancel;
      this.ClientSize = new Size(482, 296);
      this.Controls.Add((Control) this.btnSendReport);
      this.Controls.Add((Control) this.btnCancel);
      this.Controls.Add((Control) this.tabs);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExceptionReporterWindow";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Oooops !";
      this.TopMost = true;
      this.tabs.ResumeLayout(false);
      this.tabPageGeneral.ResumeLayout(false);
      this.tabPageGeneral.PerformLayout();
      ((ISupportInitialize) this.pictureBoxErrorBig).EndInit();
      this.tabPageException.ResumeLayout(false);
      this.tabPageException.PerformLayout();
      this.tabPageSystem.ResumeLayout(false);
      this.tabPageSystem.PerformLayout();
      this.ResumeLayout(false);
    }
  }
}
