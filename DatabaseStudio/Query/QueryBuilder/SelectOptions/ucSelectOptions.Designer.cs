namespace PaJaMa.DatabaseStudio.Query.QueryBuilder.SelectOptions
{
	partial class ucSelectOptions
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnAddJoin = new System.Windows.Forms.Button();
			this.pnlFromJoins = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnAddJoin);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 424);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(642, 31);
			this.panel1.TabIndex = 0;
			// 
			// btnAddJoin
			// 
			this.btnAddJoin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddJoin.Location = new System.Drawing.Point(526, 3);
			this.btnAddJoin.Name = "btnAddJoin";
			this.btnAddJoin.Size = new System.Drawing.Size(113, 23);
			this.btnAddJoin.TabIndex = 0;
			this.btnAddJoin.Text = "Add Join";
			this.btnAddJoin.UseVisualStyleBackColor = true;
			this.btnAddJoin.Click += new System.EventHandler(this.btnAddJoin_Click);
			// 
			// pnlFromJoins
			// 
			this.pnlFromJoins.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlFromJoins.Location = new System.Drawing.Point(0, 0);
			this.pnlFromJoins.Name = "pnlFromJoins";
			this.pnlFromJoins.Size = new System.Drawing.Size(642, 455);
			this.pnlFromJoins.TabIndex = 1;
			// 
			// ucSelectOptions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.pnlFromJoins);
			this.Name = "ucSelectOptions";
			this.Size = new System.Drawing.Size(642, 455);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnAddJoin;
		private System.Windows.Forms.Panel pnlFromJoins;
	}
}
