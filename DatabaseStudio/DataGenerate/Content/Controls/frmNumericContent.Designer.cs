namespace PaJaMa.DatabaseStudio.DataGenerate.Content.Controls
{
	partial class frmNumericContent
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
			this.label1 = new System.Windows.Forms.Label();
			this.numMin = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.numMax = new System.Windows.Forms.NumericUpDown();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.numMax);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.numMin);
			this.panel1.Size = new System.Drawing.Size(353, 49);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Min";
			// 
			// numMin
			// 
			this.numMin.Location = new System.Drawing.Point(42, 12);
			this.numMin.Name = "numMin";
			this.numMin.Size = new System.Drawing.Size(120, 20);
			this.numMin.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(185, 14);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Max";
			// 
			// numMax
			// 
			this.numMax.Location = new System.Drawing.Point(215, 12);
			this.numMax.Name = "numMax";
			this.numMax.Size = new System.Drawing.Size(120, 20);
			this.numMax.TabIndex = 5;
			// 
			// frmNumericContent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(353, 80);
			this.Name = "frmNumericContent";
			this.Text = "frmNumericContent";
			this.Load += new System.EventHandler(this.frmNumericContent_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.NumericUpDown numMax;
		private System.Windows.Forms.NumericUpDown numMin;
	}
}