namespace PaJaMa.DatabaseStudio.Compare
{
	partial class ucDifferences
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.gridDifferences = new System.Windows.Forms.DataGridView();
			this.Omit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ObjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ObjectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DifferenceText = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.splitMain = new System.Windows.Forms.SplitContainer();
			this.txtAlterScript = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.gridDifferences)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
			this.splitMain.Panel1.SuspendLayout();
			this.splitMain.Panel2.SuspendLayout();
			this.splitMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// gridDifferences
			// 
			this.gridDifferences.AllowUserToAddRows = false;
			this.gridDifferences.AllowUserToDeleteRows = false;
			this.gridDifferences.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.gridDifferences.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
			this.gridDifferences.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridDifferences.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Omit,
            this.ObjectName,
            this.ObjectType,
            this.DifferenceText});
			this.gridDifferences.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridDifferences.Location = new System.Drawing.Point(0, 0);
			this.gridDifferences.Name = "gridDifferences";
			this.gridDifferences.Size = new System.Drawing.Size(769, 158);
			this.gridDifferences.TabIndex = 1;
			this.gridDifferences.CurrentCellDirtyStateChanged += new System.EventHandler(this.gridDifferences_CurrentCellDirtyStateChanged);
			// 
			// Omit
			// 
			this.Omit.DataPropertyName = "Omit";
			this.Omit.HeaderText = "Omit";
			this.Omit.Name = "Omit";
			this.Omit.Width = 34;
			// 
			// ObjectName
			// 
			this.ObjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ObjectName.DataPropertyName = "ObjectName";
			this.ObjectName.HeaderText = "Name";
			this.ObjectName.Name = "ObjectName";
			this.ObjectName.ReadOnly = true;
			this.ObjectName.Width = 60;
			// 
			// ObjectType
			// 
			this.ObjectType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.ObjectType.DataPropertyName = "ObjectType";
			this.ObjectType.HeaderText = "Type";
			this.ObjectType.Name = "ObjectType";
			this.ObjectType.ReadOnly = true;
			this.ObjectType.Width = 56;
			// 
			// DifferenceText
			// 
			this.DifferenceText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.DifferenceText.DataPropertyName = "DifferenceText";
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.DifferenceText.DefaultCellStyle = dataGridViewCellStyle1;
			this.DifferenceText.HeaderText = "Differences";
			this.DifferenceText.Name = "DifferenceText";
			this.DifferenceText.ReadOnly = true;
			this.DifferenceText.Width = 200;
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
			this.splitMain.Panel1.Controls.Add(this.gridDifferences);
			// 
			// splitMain.Panel2
			// 
			this.splitMain.Panel2.Controls.Add(this.txtAlterScript);
			this.splitMain.Size = new System.Drawing.Size(769, 615);
			this.splitMain.SplitterDistance = 158;
			this.splitMain.TabIndex = 3;
			// 
			// txtAlterScript
			// 
			this.txtAlterScript.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtAlterScript.Location = new System.Drawing.Point(0, 0);
			this.txtAlterScript.Multiline = true;
			this.txtAlterScript.Name = "txtAlterScript";
			this.txtAlterScript.ReadOnly = true;
			this.txtAlterScript.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtAlterScript.Size = new System.Drawing.Size(769, 453);
			this.txtAlterScript.TabIndex = 1;
			// 
			// ucDifferences
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitMain);
			this.Name = "ucDifferences";
			this.Size = new System.Drawing.Size(769, 615);
			this.Load += new System.EventHandler(this.frmStructureDetails_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridDifferences)).EndInit();
			this.splitMain.Panel1.ResumeLayout(false);
			this.splitMain.Panel2.ResumeLayout(false);
			this.splitMain.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
			this.splitMain.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView gridDifferences;
		private System.Windows.Forms.SplitContainer splitMain;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Omit;
		private System.Windows.Forms.DataGridViewTextBoxColumn ObjectName;
		private System.Windows.Forms.DataGridViewTextBoxColumn ObjectType;
		private System.Windows.Forms.DataGridViewTextBoxColumn DifferenceText;
		internal System.Windows.Forms.TextBox txtAlterScript;

	}
}