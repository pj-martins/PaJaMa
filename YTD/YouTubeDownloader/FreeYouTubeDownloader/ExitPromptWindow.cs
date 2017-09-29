// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.ExitPromptWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Localization;
using FreeYouTubeDownloader.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FreeYouTubeDownloader
{
    public class ExitPromptWindow : Form
    {
        private IContainer components;
        private PictureBox pictureBoxExitPrompt;
        private Label lblMessage;
        private CheckBox checkBoxRememberAction;
        private Button btnYes;
        private Button btnNo;

        public ExitPromptWindow()
        {
            this.InitializeComponent();
            this.InitializeControls();
        }

        private void InitializeControls()
        {
            this.Text = "YouTube downloader cracked by PJ!";
            this.lblMessage.Text = Strings.ExitPromptMessage;
            this.checkBoxRememberAction.Text = Strings.RememberAction;
            this.btnYes.Text = Strings.Yes;
            this.btnNo.Text = Strings.No;
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            if (!this.checkBoxRememberAction.Checked)
                return;
            Settings.Instance.CloseAppAction = CloseAppActions.ExitApplication;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            if (!this.checkBoxRememberAction.Checked)
                return;
            Settings.Instance.CloseAppAction = CloseAppActions.MinimizeToTray;
        }

        private void ExitPromptWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            ((ISupportInitialize)this.pictureBoxExitPrompt).BeginInit();
            this.SuspendLayout();
            this.pictureBoxExitPrompt.Image = (Image)Resources.ExitPromptQuestionMark;
            this.pictureBoxExitPrompt.InitialImage = (Image)null;
            this.pictureBoxExitPrompt.Location = new Point(10, 10);
            this.pictureBoxExitPrompt.Margin = new Padding(2);
            this.pictureBoxExitPrompt.Name = "pictureBoxExitPrompt";
            this.pictureBoxExitPrompt.Size = new Size(40, 40);
            this.pictureBoxExitPrompt.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxExitPrompt.TabIndex = 0;
            this.pictureBoxExitPrompt.TabStop = false;
            this.lblMessage.Location = new Point(55, 17);
            this.lblMessage.Margin = new Padding(2, 0, 2, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new Size(295, 60);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.Text = "Do you want to exit the application? Click 'Yes' to shut down ClipClip or 'No' to minimize it to the notification area.";
            this.checkBoxRememberAction.AutoSize = true;
            this.checkBoxRememberAction.Location = new Point(14, 94);
            this.checkBoxRememberAction.Margin = new Padding(2);
            this.checkBoxRememberAction.Name = "checkBoxRememberAction";
            this.checkBoxRememberAction.Size = new Size(109, 17);
            this.checkBoxRememberAction.TabIndex = 2;
            this.checkBoxRememberAction.Text = "Remember action";
            this.checkBoxRememberAction.UseVisualStyleBackColor = true;
            this.btnYes.DialogResult = DialogResult.Yes;
            this.btnYes.Location = new Point(195, 89);
            this.btnYes.Margin = new Padding(2);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new Size(75, 25);
            this.btnYes.TabIndex = 3;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new EventHandler(this.btnYes_Click);
            this.btnNo.DialogResult = DialogResult.No;
            this.btnNo.Location = new Point(276, 89);
            this.btnNo.Margin = new Padding(2);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new Size(75, 25);
            this.btnNo.TabIndex = 4;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new EventHandler(this.btnNo_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(358, 121);
            this.Controls.Add((Control)this.btnNo);
            this.Controls.Add((Control)this.btnYes);
            this.Controls.Add((Control)this.checkBoxRememberAction);
            this.Controls.Add((Control)this.lblMessage);
            this.Controls.Add((Control)this.pictureBoxExitPrompt);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Margin = new Padding(2);
            this.Name = "ExitPromptWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ClipClip";
            this.FormClosing += new FormClosingEventHandler(this.ExitPromptWindow_FormClosing);
            ((ISupportInitialize)this.pictureBoxExitPrompt).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
