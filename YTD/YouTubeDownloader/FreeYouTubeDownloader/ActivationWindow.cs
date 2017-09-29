// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.ActivationWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Ads;
using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.License;
using FreeYouTubeDownloader.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using wyDay.TurboActivate;

namespace FreeYouTubeDownloader
{
    public class ActivationWindow : Form, ILocalizableForm
    {
        private readonly Regex _productKeyRegex = new Regex("^([A-Z1-9]{4})-([A-Z1-9]{4})-([A-Z1-9]{4})-([A-Z1-9]{4})-([A-Z1-9]{4})-([A-Z1-9]{4})-([A-Z1-9]{4})$", RegexOptions.IgnoreCase);
        private IContainer components;
        private Label lblIntro;
        private TextBox textBoxActivationKey;
        private LinkLabel linkLabelSubscribe;
        private Button btnActivate;

        public ActivationWindow()
        {
            this.InitializeComponent();
            FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler)((sender, args) => this.ApplyCurrentLocalization());
            this.ApplyCurrentLocalization();
        }

        private void OnActivateButtonClick(object sender, EventArgs e)
        {
            string text = this.textBoxActivationKey.Text;
            try
            {
                if (!this._productKeyRegex.IsMatch(text))
                    throw new InvalidProductKeyException();
                //if (LicenseHelper.TurboActivate.CheckAndSavePKey(text, TA_Flags.TA_SYSTEM))
                //{
                //    LicenseHelper.TurboActivate.Activate((string)null);
                //    LicenseHelper.CheckGenuineness();
                //}
            }
            catch (TurboActivateException ex)
            {
                Log.Error("Failed to activate: " + ex.Message, (Exception)null);
                int num = (int)MessageBox.Show((IWin32Window)this, ex.Message, Strings.ActivateProVersion, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            if (LicenseHelper.IsGenuine)
            {
                AdsHelper.HideBottomAd();
                AdsHelper.HideRightAd();
                int num = (int)MessageBox.Show((IWin32Window)this, Strings.ProVersionIsActivated, Strings.ActivateProVersion, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Program.MainWindow.SetStatusStrip(string.Empty);
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                if (!LicenseHelper.IsExpired)
                    return;
                try
                {
                    //LicenseHelper.TurboActivate.Deactivate(true);
                }
                catch (TurboActivateException ex)
                {
                }
                string path;
                switch (new ConfirmationWindow(Application.ProductName, Strings.KeyExpiredMessage, (string)null).ShowDialog())
                {
                    case DialogResult.Ignore:
                        this.DialogResult = DialogResult.Ignore;
                        return;
                    case DialogResult.Yes:
                        path = "https://youtubedownloader.com/subscribe/";
                        break;
                    default:
                        path = "http://youtubedownloader.com/support";
                        break;
                }
                int num1 = 0;
                int num2 = 0;
                MainWindow.RunDefaultApp(path, num1 != 0, num2 != 0);
                this.DialogResult = DialogResult.Ignore;
                return;
            }
        }

        private void OnSubscribeClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainWindow.RunProcessStart(MainWindow.VerifyUrl("https://youtubedownloader.com/subscribe/", UriKind.Absolute), "https://youtubedownloader.com/subscribe/", false);
        }

        public void ApplyCurrentLocalization()
        {
            this.Text = Strings.ActivateProVersion;
            this.lblIntro.Text = Strings.EnterActivationKey;
            this.linkLabelSubscribe.Text = Strings.ClickToSubscribe;
            this.btnActivate.Text = Strings.Activate;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ActivationWindow));
            this.lblIntro = new Label();
            this.textBoxActivationKey = new TextBox();
            this.linkLabelSubscribe = new LinkLabel();
            this.btnActivate = new Button();
            this.SuspendLayout();
            this.lblIntro.AutoSize = true;
            this.lblIntro.Location = new Point(13, 13);
            this.lblIntro.Name = "lblIntro";
            this.lblIntro.Size = new Size(164, 17);
            this.lblIntro.TabIndex = 0;
            this.lblIntro.Text = "Enter your activation key";
            this.textBoxActivationKey.Location = new Point(16, 42);
            this.textBoxActivationKey.Name = "textBoxActivationKey";
            this.textBoxActivationKey.Size = new Size(354, 22);
            this.textBoxActivationKey.TabIndex = 1;
            this.linkLabelSubscribe.Location = new Point(13, 80);
            this.linkLabelSubscribe.Name = "linkLabelSubscribe";
            this.linkLabelSubscribe.Size = new Size(357, 17);
            this.linkLabelSubscribe.TabIndex = 2;
            this.linkLabelSubscribe.TabStop = true;
            this.linkLabelSubscribe.Text = "Click here if you haven't subscribed to FYD Pro yet";
            this.linkLabelSubscribe.LinkClicked += new LinkLabelLinkClickedEventHandler(this.OnSubscribeClicked);
            this.btnActivate.Location = new Point(258, 138);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new Size(112, 30);
            this.btnActivate.TabIndex = 3;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new EventHandler(this.OnActivateButtonClick);
            this.AcceptButton = (IButtonControl)this.btnActivate;
            this.AutoScaleDimensions = new SizeF(8f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(382, 180);
            this.Controls.Add((Control)this.btnActivate);
            this.Controls.Add((Control)this.linkLabelSubscribe);
            this.Controls.Add((Control)this.textBoxActivationKey);
            this.Controls.Add((Control)this.lblIntro);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ActivationWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Activate FYD Pro";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
