// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.ConfirmationWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Properties;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
  public class ConfirmationWindow : Form
  {
    private IContainer components;
    private PictureBox pictureBoxExitPrompt;
    private Label lblMessage;
    private CheckBox checkBoxRememberAction;
    private Button btnYes;
    private Button btnNo;
    private Button btnIgnore;

    public bool RememberAction
    {
      get
      {
        return this.checkBoxRememberAction.Checked;
      }
    }

    public ConfirmationWindow(string title, string message, string rememberActionText)
    {
      this.InitializeComponent();
      this.InitializeControls(title, message, rememberActionText);
    }

    private void InitializeControls(string title, string message, string rememberActionText)
    {
      this.ControlBox = false;
      this.Text = title;
      this.lblMessage.Text = message;
      if (string.IsNullOrWhiteSpace(rememberActionText))
        this.checkBoxRememberAction.Visible = false;
      else
        this.checkBoxRememberAction.Text = rememberActionText;
      this.btnYes.Text = Strings.ExtendSubscription;
      this.btnNo.Text = Strings.ContactSupport;
      this.btnIgnore.Text = Strings.MaybeLater;
    }

    private void ConfirmationWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (this.DialogResult != DialogResult.Cancel)
        return;
      e.Cancel = true;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.pictureBoxExitPrompt = new PictureBox();
      this.lblMessage = new Label();
      this.checkBoxRememberAction = new CheckBox();
      this.btnYes = new Button();
      this.btnNo = new Button();
      this.btnIgnore = new Button();
      ((ISupportInitialize) this.pictureBoxExitPrompt).BeginInit();
      this.SuspendLayout();
      this.pictureBoxExitPrompt.Image = (Image) Resources.ExitPromptQuestionMark;
      this.pictureBoxExitPrompt.InitialImage = (Image) null;
      this.pictureBoxExitPrompt.Location = new Point(13, 12);
      this.pictureBoxExitPrompt.Margin = new Padding(3, 2, 3, 2);
      this.pictureBoxExitPrompt.Name = "pictureBoxExitPrompt";
      this.pictureBoxExitPrompt.Size = new Size(53, 49);
      this.pictureBoxExitPrompt.SizeMode = PictureBoxSizeMode.Zoom;
      this.pictureBoxExitPrompt.TabIndex = 0;
      this.pictureBoxExitPrompt.TabStop = false;
      this.lblMessage.Location = new Point(73, 21);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new Size(494, 74);
      this.lblMessage.TabIndex = 1;
      this.lblMessage.Text = "Do you want to exit the application? Click 'Yes' to shut down ClipClip or 'No' to minimize it to the notification area.";
      this.checkBoxRememberAction.AutoSize = true;
      this.checkBoxRememberAction.Location = new Point(19, 116);
      this.checkBoxRememberAction.Margin = new Padding(3, 2, 3, 2);
      this.checkBoxRememberAction.Name = "checkBoxRememberAction";
      this.checkBoxRememberAction.Size = new Size(141, 21);
      this.checkBoxRememberAction.TabIndex = 5;
      this.checkBoxRememberAction.Text = "Remember action";
      this.checkBoxRememberAction.UseVisualStyleBackColor = true;
      this.btnYes.DialogResult = DialogResult.Yes;
      this.btnYes.Font = new Font("Microsoft Sans Serif", 7f);
      this.btnYes.Location = new Point(157, 110);
      this.btnYes.Margin = new Padding(3, 2, 3, 2);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new Size(135, 31);
      this.btnYes.TabIndex = 3;
      this.btnYes.Text = "Yes";
      this.btnYes.UseVisualStyleBackColor = true;
      this.btnNo.DialogResult = DialogResult.No;
      this.btnNo.Font = new Font("Microsoft Sans Serif", 7f);
      this.btnNo.Location = new Point(295, 110);
      this.btnNo.Margin = new Padding(3, 2, 3, 2);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new Size(135, 31);
      this.btnNo.TabIndex = 4;
      this.btnNo.Text = "No";
      this.btnNo.UseVisualStyleBackColor = true;
      this.btnIgnore.DialogResult = DialogResult.Ignore;
      this.btnIgnore.Font = new Font("Microsoft Sans Serif", 7f);
      this.btnIgnore.Location = new Point(433, 110);
      this.btnIgnore.Margin = new Padding(3, 2, 3, 2);
      this.btnIgnore.Name = "btnIgnore";
      this.btnIgnore.Size = new Size(135, 31);
      this.btnIgnore.TabIndex = 6;
      this.btnIgnore.Text = "Ignore";
      this.btnIgnore.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(576, 149);
      this.Controls.Add((Control) this.btnIgnore);
      this.Controls.Add((Control) this.btnNo);
      this.Controls.Add((Control) this.btnYes);
      this.Controls.Add((Control) this.checkBoxRememberAction);
      this.Controls.Add((Control) this.lblMessage);
      this.Controls.Add((Control) this.pictureBoxExitPrompt);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Margin = new Padding(3, 2, 3, 2);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ConfirmationWindow";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "ClipClip";
      this.FormClosing += new FormClosingEventHandler(this.ConfirmationWindow_FormClosing);
      ((ISupportInitialize) this.pictureBoxExitPrompt).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
