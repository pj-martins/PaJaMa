namespace PaJaMa.ScreenCapture
{
	partial class frmOverlayPictureBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictMain = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictMain)).BeginInit();
			this.SuspendLayout();
			// 
			// pictMain
			// 
			this.pictMain.Location = new System.Drawing.Point(63, 60);
			this.pictMain.Name = "pictMain";
			this.pictMain.Size = new System.Drawing.Size(152, 129);
			this.pictMain.TabIndex = 0;
			this.pictMain.TabStop = false;
			// 
			// frmOverlay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.pictMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmOverlay";
			this.Opacity = 0.45D;
			this.Text = "frmOverlay";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmOverlay_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictMain;

	}
}