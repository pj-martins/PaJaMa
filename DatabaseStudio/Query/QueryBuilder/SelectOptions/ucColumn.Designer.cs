namespace PaJaMa.DatabaseStudio.Query.QueryBuilder.SelectOptions
{
	partial class ucColumn
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
			this.cboColumn = new System.Windows.Forms.ComboBox();
			this.btnOptions = new System.Windows.Forms.Button();
			this.RemoveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cboColumn
			// 
			this.cboColumn.FormattingEnabled = true;
			this.cboColumn.Location = new System.Drawing.Point(3, 3);
			this.cboColumn.Name = "cboColumn";
			this.cboColumn.Size = new System.Drawing.Size(294, 21);
			this.cboColumn.TabIndex = 0;
			this.cboColumn.Text = "<All>";
			// 
			// btnOptions
			// 
			this.btnOptions.Location = new System.Drawing.Point(303, 3);
			this.btnOptions.Name = "btnOptions";
			this.btnOptions.Size = new System.Drawing.Size(73, 21);
			this.btnOptions.TabIndex = 1;
			this.btnOptions.Text = "Options";
			this.btnOptions.UseVisualStyleBackColor = true;
			// 
			// RemoveButton
			// 
			this.RemoveButton.Location = new System.Drawing.Point(382, 3);
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(73, 21);
			this.RemoveButton.TabIndex = 2;
			this.RemoveButton.Text = "Remove";
			this.RemoveButton.UseVisualStyleBackColor = true;
			// 
			// ucColumn
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.RemoveButton);
			this.Controls.Add(this.btnOptions);
			this.Controls.Add(this.cboColumn);
			this.Name = "ucColumn";
			this.Size = new System.Drawing.Size(520, 30);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox cboColumn;
		private System.Windows.Forms.Button btnOptions;
		internal System.Windows.Forms.Button RemoveButton;
	}
}
