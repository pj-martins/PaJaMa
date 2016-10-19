namespace PaJaMa.DatabaseStudio.Compare
{
	partial class frmDataDetails
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataDetails));
			this.gridMain = new System.Windows.Forms.DataGridView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.chkRight = new System.Windows.Forms.CheckBox();
			this.chkLeft = new System.Windows.Forms.CheckBox();
			this.chkDifferent = new System.Windows.Forms.CheckBox();
			this.chkAll = new System.Windows.Forms.CheckBox();
			this.lblTotalPages = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnLast = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.numPage = new System.Windows.Forms.NumericUpDown();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnSync = new System.Windows.Forms.Button();
			this.cboOverrideKeyField = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPage)).BeginInit();
			this.SuspendLayout();
			// 
			// gridMain
			// 
			this.gridMain.AllowUserToAddRows = false;
			this.gridMain.AllowUserToDeleteRows = false;
			this.gridMain.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridMain.Location = new System.Drawing.Point(0, 0);
			this.gridMain.Name = "gridMain";
			this.gridMain.Size = new System.Drawing.Size(1085, 581);
			this.gridMain.TabIndex = 0;
			this.gridMain.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gridMain_CellFormatting);
			this.gridMain.Sorted += new System.EventHandler(this.gridMain_Sorted);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.cboOverrideKeyField);
			this.panel1.Controls.Add(this.chkRight);
			this.panel1.Controls.Add(this.chkLeft);
			this.panel1.Controls.Add(this.chkDifferent);
			this.panel1.Controls.Add(this.chkAll);
			this.panel1.Controls.Add(this.lblTotalPages);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.btnNext);
			this.panel1.Controls.Add(this.btnLast);
			this.panel1.Controls.Add(this.btnFirst);
			this.panel1.Controls.Add(this.btnPrevious);
			this.panel1.Controls.Add(this.numPage);
			this.panel1.Controls.Add(this.btnRefresh);
			this.panel1.Controls.Add(this.btnSync);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 581);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1085, 36);
			this.panel1.TabIndex = 1;
			// 
			// chkRight
			// 
			this.chkRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkRight.AutoSize = true;
			this.chkRight.Location = new System.Drawing.Point(785, 9);
			this.chkRight.Name = "chkRight";
			this.chkRight.Size = new System.Drawing.Size(51, 17);
			this.chkRight.TabIndex = 12;
			this.chkRight.Text = "Right";
			this.chkRight.UseVisualStyleBackColor = true;
			this.chkRight.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
			// 
			// chkLeft
			// 
			this.chkLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkLeft.AutoSize = true;
			this.chkLeft.Location = new System.Drawing.Point(737, 9);
			this.chkLeft.Name = "chkLeft";
			this.chkLeft.Size = new System.Drawing.Size(44, 17);
			this.chkLeft.TabIndex = 11;
			this.chkLeft.Text = "Left";
			this.chkLeft.UseVisualStyleBackColor = true;
			this.chkLeft.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
			// 
			// chkDifferent
			// 
			this.chkDifferent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkDifferent.AutoSize = true;
			this.chkDifferent.Location = new System.Drawing.Point(665, 9);
			this.chkDifferent.Name = "chkDifferent";
			this.chkDifferent.Size = new System.Drawing.Size(66, 17);
			this.chkDifferent.TabIndex = 10;
			this.chkDifferent.Text = "Different";
			this.chkDifferent.UseVisualStyleBackColor = true;
			this.chkDifferent.CheckedChanged += new System.EventHandler(this.chk_CheckedChanged);
			// 
			// chkAll
			// 
			this.chkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.chkAll.AutoSize = true;
			this.chkAll.Checked = true;
			this.chkAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkAll.Location = new System.Drawing.Point(613, 9);
			this.chkAll.Name = "chkAll";
			this.chkAll.Size = new System.Drawing.Size(37, 17);
			this.chkAll.TabIndex = 9;
			this.chkAll.Text = "All";
			this.chkAll.UseVisualStyleBackColor = true;
			this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
			// 
			// lblTotalPages
			// 
			this.lblTotalPages.AutoSize = true;
			this.lblTotalPages.Location = new System.Drawing.Point(181, 11);
			this.lblTotalPages.Name = "lblTotalPages";
			this.lblTotalPages.Size = new System.Drawing.Size(13, 13);
			this.lblTotalPages.TabIndex = 8;
			this.lblTotalPages.Text = "1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(157, 11);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(18, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Of";
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(267, 6);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(33, 23);
			this.btnNext.TabIndex = 6;
			this.btnNext.Text = ">";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnLast
			// 
			this.btnLast.Location = new System.Drawing.Point(306, 6);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(33, 23);
			this.btnLast.TabIndex = 5;
			this.btnLast.Text = ">>";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.Location = new System.Drawing.Point(12, 6);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(33, 23);
			this.btnFirst.TabIndex = 4;
			this.btnFirst.Text = "<<";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.Location = new System.Drawing.Point(51, 6);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(33, 23);
			this.btnPrevious.TabIndex = 3;
			this.btnPrevious.Text = "<";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// numPage
			// 
			this.numPage.Location = new System.Drawing.Point(90, 8);
			this.numPage.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
			this.numPage.Name = "numPage";
			this.numPage.Size = new System.Drawing.Size(61, 20);
			this.numPage.TabIndex = 2;
			this.numPage.ValueChanged += new System.EventHandler(this.numPage_ValueChanged);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Location = new System.Drawing.Point(844, 6);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(81, 23);
			this.btnRefresh.TabIndex = 1;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnSync
			// 
			this.btnSync.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSync.Location = new System.Drawing.Point(931, 6);
			this.btnSync.Name = "btnSync";
			this.btnSync.Size = new System.Drawing.Size(142, 23);
			this.btnSync.TabIndex = 0;
			this.btnSync.Text = "Synchronize Selected";
			this.btnSync.UseVisualStyleBackColor = true;
			this.btnSync.Click += new System.EventHandler(this.btnSync_Click);
			// 
			// cboOverrideKeyField
			// 
			this.cboOverrideKeyField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboOverrideKeyField.FormattingEnabled = true;
			this.cboOverrideKeyField.Location = new System.Drawing.Point(376, 8);
			this.cboOverrideKeyField.Name = "cboOverrideKeyField";
			this.cboOverrideKeyField.Size = new System.Drawing.Size(215, 21);
			this.cboOverrideKeyField.TabIndex = 13;
			this.cboOverrideKeyField.SelectedIndexChanged += new System.EventHandler(this.cboOverrideKeyField_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(345, 11);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(25, 13);
			this.label2.TabIndex = 14;
			this.label2.Text = "Key";
			// 
			// frmDataDetails
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1085, 617);
			this.Controls.Add(this.gridMain);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "frmDataDetails";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Details";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDataDetails_FormClosing);
			this.Load += new System.EventHandler(this.frmDataDetails_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numPage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnSync;
		private System.Windows.Forms.DataGridView gridMain;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.NumericUpDown numPage;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnLast;
		private System.Windows.Forms.Label lblTotalPages;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox chkAll;
		private System.Windows.Forms.CheckBox chkRight;
		private System.Windows.Forms.CheckBox chkLeft;
		private System.Windows.Forms.CheckBox chkDifferent;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboOverrideKeyField;

	}
}