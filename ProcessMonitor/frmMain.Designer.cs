namespace PaJaMa.ProcessMonitor
{
	partial class frmMain
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.gridMain = new System.Windows.Forms.DataGridView();
			this.btnKill = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnKill);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 647);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(993, 45);
			this.panel1.TabIndex = 0;
			// 
			// gridMain
			// 
			this.gridMain.AllowUserToAddRows = false;
			this.gridMain.AllowUserToDeleteRows = false;
			this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridMain.Location = new System.Drawing.Point(0, 0);
			this.gridMain.Name = "gridMain";
			this.gridMain.ReadOnly = true;
			this.gridMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridMain.Size = new System.Drawing.Size(993, 647);
			this.gridMain.TabIndex = 1;
			this.gridMain.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridMain_DataError);
			// 
			// btnKill
			// 
			this.btnKill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnKill.Location = new System.Drawing.Point(906, 10);
			this.btnKill.Name = "btnKill";
			this.btnKill.Size = new System.Drawing.Size(75, 23);
			this.btnKill.TabIndex = 0;
			this.btnKill.Text = "Kill";
			this.btnKill.UseVisualStyleBackColor = true;
			this.btnKill.Click += new System.EventHandler(this.btnKill_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(993, 692);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.panel1);
			this.Name = "frmMain";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridView gridMain;
		private System.Windows.Forms.Button btnKill;
	}
}

