namespace PaJaMa.DatabaseStudio.Compare
{
	partial class frmCreates
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
			this.txtFromScript = new System.Windows.Forms.TextBox();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.label1 = new System.Windows.Forms.Label();
			this.txtToScript = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFromScript
			// 
			this.txtFromScript.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtFromScript.Location = new System.Drawing.Point(0, 13);
			this.txtFromScript.Multiline = true;
			this.txtFromScript.Name = "txtFromScript";
			this.txtFromScript.ReadOnly = true;
			this.txtFromScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtFromScript.Size = new System.Drawing.Size(769, 294);
			this.txtFromScript.TabIndex = 0;
			this.txtFromScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
			// 
			// splitMain
			// 
			this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitMain.Location = new System.Drawing.Point(0, 0);
			this.splitMain.Name = "splitMain";
			this.splitMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitMain.Panel1
			// 
			this.splitMain.Panel1.Controls.Add(this.txtFromScript);
			this.splitMain.Panel1.Controls.Add(this.label1);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.txtToScript);
			this.splitMain.Panel2.Controls.Add(this.label2);
			this.splitMain.Size = new System.Drawing.Size(769, 615);
			this.splitMain.SplitterDistance = 307;
			this.splitMain.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Top;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(41, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Source";
			// 
			// txtToScript
			// 
			this.txtToScript.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtToScript.Location = new System.Drawing.Point(0, 13);
			this.txtToScript.Multiline = true;
			this.txtToScript.Name = "txtToScript";
			this.txtToScript.ReadOnly = true;
			this.txtToScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtToScript.Size = new System.Drawing.Size(769, 291);
			this.txtToScript.TabIndex = 1;
			this.txtToScript.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_KeyDown);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Top;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(38, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Target";
			// 
			// frmObjectStructureDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(769, 615);
			this.Controls.Add(this.splitMain);
			this.Name = "frmObjectStructureDetails";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Details";
			this.Load += new System.EventHandler(this.frmStructureDetails_Load);
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel1.PerformLayout();
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtFromScript;
		private System.Windows.Forms.TextBox txtToScript;

	}
}