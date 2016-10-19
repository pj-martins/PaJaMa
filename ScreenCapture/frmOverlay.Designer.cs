namespace PaJaMa.ScreenCapture
{
	partial class frmOverlay
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
			this.pnlSelection = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// pnlSelection
			// 
			this.pnlSelection.BackColor = System.Drawing.Color.White;
			this.pnlSelection.Location = new System.Drawing.Point(27, 48);
			this.pnlSelection.Name = "pnlSelection";
			this.pnlSelection.Size = new System.Drawing.Size(200, 100);
			this.pnlSelection.TabIndex = 0;
			this.pnlSelection.Visible = false;
			// 
			// frmOverlay
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.pnlSelection);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmOverlay";
			this.Opacity = 0.45D;
			this.Text = "frmOverlay";
			this.TopMost = true;
			this.TransparencyKey = System.Drawing.Color.White;
			this.Load += new System.EventHandler(this.frmOverlay_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlSelection;
	}
}