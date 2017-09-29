// Decompiled with JetBrains decompiler
// Type: FreeYouTubeDownloader.DeactivationWindow
// Assembly: YouTubeDownloader, Version=4.2.795.0, Culture=neutral, PublicKeyToken=null
// MVID: E906DA98-D8CA-4CAE-B96B-249F2467E375
// Assembly location: C:\Program Files (x86)\Free YouTube Downloader\YouTubeDownloader.exe

using FreeYouTubeDownloader.Debug;
using FreeYouTubeDownloader.License;
using FreeYouTubeDownloader.Localization;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using wyDay.TurboActivate;

namespace FreeYouTubeDownloader
{
  public class DeactivationWindow : Form, ILocalizableForm
  {
    private IContainer components;
    private Label labelDeactivate;
    private Button btnDeactivate;

    public DeactivationWindow()
    {
      this.InitializeComponent();
      FreeYouTubeDownloader.Localization.Localization.Instance.LanguageChanged += (FreeYouTubeDownloader.Localization.Localization.LanguageChangedEventHandler) ((sender, args) => this.ApplyCurrentLocalization());
      this.ApplyCurrentLocalization();
    }

    private void OnButtonDeactivateClick(object sender, EventArgs e)
    {
      try
      {
        //LicenseHelper.TurboActivate.Deactivate(true);
        this.DialogResult = DialogResult.OK;
      }
      catch (TurboActivateException ex)
      {
        Log.Error("Failed to deactivate", (Exception) ex);
        int num = (int) MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
      }
    }

    public void ApplyCurrentLocalization()
    {
      this.Text = Strings.Deactivate + " Free YouTube Downloader Pro";
      this.btnDeactivate.Text = Strings.Deactivate;
      this.labelDeactivate.Text = Strings.DeactivateFYDProDescription;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (DeactivationWindow));
      this.labelDeactivate = new Label();
      this.btnDeactivate = new Button();
      this.SuspendLayout();
      this.labelDeactivate.Location = new Point(13, 13);
      this.labelDeactivate.Name = "labelDeactivate";
      this.labelDeactivate.Size = new Size(451, 64);
      this.labelDeactivate.TabIndex = 0;
      this.labelDeactivate.Text = "Here you can deactivate FYD Pro from this computer. This is particularly helpful in case you want to activate your subscription in another computer. ";
      this.btnDeactivate.Location = new Point(174, 89);
      this.btnDeactivate.Name = "btnDeactivate";
      this.btnDeactivate.Size = new Size(125, 46);
      this.btnDeactivate.TabIndex = 1;
      this.btnDeactivate.Text = "Deactivate";
      this.btnDeactivate.UseVisualStyleBackColor = true;
      this.btnDeactivate.Click += new EventHandler(this.OnButtonDeactivateClick);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.AutoSize = true;
      this.ClientSize = new Size(476, 154);
      this.Controls.Add((Control) this.btnDeactivate);
      this.Controls.Add((Control) this.labelDeactivate);
      this.FormBorderStyle = FormBorderStyle.FixedSingle;
      this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DeactivationWindow";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "Deactivate FYD Pro";
      this.ResumeLayout(false);
    }
  }
}
