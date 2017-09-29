// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.AboutWindow
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
    public class AboutWindow : Form, ILocalizableForm
    {
        private IContainer components;
        private PictureBox picBoxLogo;
        private Label labelAppName;
        private Label labelAppVersion;
        private Label labelCopyright;
        private Button btnOk;
        private Label labelLicensedTo;

        public AboutWindow()
        {
            this.InitializeComponent();
            FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler)((sender, args) => this.ApplyCurrentLocalization());
            this.ApplyCurrentLocalization();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ApplyCurrentLocalization()
        {
            this.Text = Strings.About;
            this.labelAppName.Text = "YouTube downloader cracked by PJ!!";
            this.labelAppVersion.Text = string.Format("{0}    {1}", (object)Strings.Version, (object)Application.ProductVersion);
            this.labelCopyright.Text = string.Format("© {0} PJ Inc.", (object)DateTime.Now.Year);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AboutWindow));
            this.picBoxLogo = new PictureBox();
            this.labelAppName = new Label();
            this.labelAppVersion = new Label();
            this.labelCopyright = new Label();
            this.btnOk = new Button();
            this.labelLicensedTo = new Label();
            ((ISupportInitialize)this.picBoxLogo).BeginInit();
            this.SuspendLayout();
            this.picBoxLogo.Image = (Image)Resources.logo;
            this.picBoxLogo.Location = new Point(13, 13);
            this.picBoxLogo.Name = "picBoxLogo";
            this.picBoxLogo.Size = new Size(64, 64);
            this.picBoxLogo.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picBoxLogo.TabIndex = 0;
            this.picBoxLogo.TabStop = false;
            this.labelAppName.AutoSize = true;
            this.labelAppName.Location = new Point(102, 13);
            this.labelAppName.Name = "labelAppName";
            this.labelAppName.Size = new Size(178, 17);
            this.labelAppName.TabIndex = 1;
            this.labelAppName.Text = "Free YouTube Downloader";
            this.labelAppVersion.AutoSize = true;
            this.labelAppVersion.Location = new Point(102, 46);
            this.labelAppVersion.Name = "labelAppVersion";
            this.labelAppVersion.Size = new Size(120, 17);
            this.labelAppVersion.TabIndex = 2;
            this.labelAppVersion.Text = "Version    4.1.630";
            this.labelCopyright.AutoSize = true;
            this.labelCopyright.Location = new Point(102, 81);
            this.labelCopyright.Name = "labelCopyright";
            this.labelCopyright.Size = new Size(111, 17);
            this.labelCopyright.TabIndex = 3;
            this.labelCopyright.Text = "© 2017 How Inc.";
            this.btnOk.Location = new Point(250, 160);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new Size(76, 30);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new EventHandler(this.btnOk_Click);
            this.labelLicensedTo.AutoSize = true;
            this.labelLicensedTo.Location = new Point(102, 116);
            this.labelLicensedTo.Name = "labelLicensedTo";
            this.labelLicensedTo.Size = new Size(185, 17);
            this.labelLicensedTo.TabIndex = 5;
            this.labelLicensedTo.Text = "Registered to Nazar Grynko";
            this.labelLicensedTo.Visible = false;
            this.AutoScaleDimensions = new SizeF(8f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(338, 202);
            this.Controls.Add((Control)this.labelLicensedTo);
            this.Controls.Add((Control)this.btnOk);
            this.Controls.Add((Control)this.labelCopyright);
            this.Controls.Add((Control)this.labelAppVersion);
            this.Controls.Add((Control)this.labelAppName);
            this.Controls.Add((Control)this.picBoxLogo);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "About";
            ((ISupportInitialize)this.picBoxLogo).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
