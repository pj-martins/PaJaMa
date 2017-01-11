namespace PaJaMa.DatabaseStudio.Monitor
{
	partial class ucMonitor
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
			this.components = new System.ComponentModel.Container();
			PaJaMa.WinControls.CheckBoxProperties checkBoxProperties1 = new PaJaMa.WinControls.CheckBoxProperties();
			PaJaMa.WinControls.CheckBoxProperties checkBoxProperties2 = new PaJaMa.WinControls.CheckBoxProperties();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnRemoveConnString = new System.Windows.Forms.Button();
			this.cboDatabase = new System.Windows.Forms.ComboBox();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cboConnectionString = new System.Windows.Forms.ComboBox();
			this.gridResults = new System.Windows.Forms.DataGridView();
			this.colTextData = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colApplicationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colLoginName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnClear = new System.Windows.Forms.Button();
			this.chkLogin = new PaJaMa.WinControls.CheckBoxComboBox();
			this.chkApplication = new PaJaMa.WinControls.CheckBoxComboBox();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridResults)).BeginInit();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnRemoveConnString);
			this.panel1.Controls.Add(this.cboDatabase);
			this.panel1.Controls.Add(this.btnDisconnect);
			this.panel1.Controls.Add(this.btnConnect);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.cboConnectionString);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(898, 47);
			this.panel1.TabIndex = 14;
			// 
			// btnRemoveConnString
			// 
			this.btnRemoveConnString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveConnString.Enabled = false;
			this.btnRemoveConnString.Location = new System.Drawing.Point(673, 12);
			this.btnRemoveConnString.Name = "btnRemoveConnString";
			this.btnRemoveConnString.Size = new System.Drawing.Size(121, 23);
			this.btnRemoveConnString.TabIndex = 3;
			this.btnRemoveConnString.Text = "Remove From List";
			this.btnRemoveConnString.UseVisualStyleBackColor = true;
			// 
			// cboDatabase
			// 
			this.cboDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabase.FormattingEnabled = true;
			this.cboDatabase.Location = new System.Drawing.Point(673, 12);
			this.cboDatabase.Name = "cboDatabase";
			this.cboDatabase.Size = new System.Drawing.Size(121, 21);
			this.cboDatabase.TabIndex = 12;
			this.cboDatabase.Visible = false;
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisconnect.Location = new System.Drawing.Point(800, 12);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(86, 23);
			this.btnDisconnect.TabIndex = 11;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Visible = false;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Location = new System.Drawing.Point(800, 12);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(86, 23);
			this.btnConnect.TabIndex = 10;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Connection String";
			// 
			// cboConnectionString
			// 
			this.cboConnectionString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboConnectionString.FormattingEnabled = true;
			this.cboConnectionString.Location = new System.Drawing.Point(105, 12);
			this.cboConnectionString.Name = "cboConnectionString";
			this.cboConnectionString.Size = new System.Drawing.Size(562, 21);
			this.cboConnectionString.TabIndex = 6;
			// 
			// gridResults
			// 
			this.gridResults.AllowUserToAddRows = false;
			this.gridResults.AllowUserToDeleteRows = false;
			this.gridResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colTextData,
            this.colApplicationName,
            this.colLoginName,
            this.colStart,
            this.colEnd});
			this.gridResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridResults.Location = new System.Drawing.Point(0, 47);
			this.gridResults.Name = "gridResults";
			this.gridResults.ReadOnly = true;
			this.gridResults.Size = new System.Drawing.Size(898, 479);
			this.gridResults.TabIndex = 15;
			// 
			// colTextData
			// 
			this.colTextData.DataPropertyName = "TextData";
			this.colTextData.HeaderText = "Query";
			this.colTextData.Name = "colTextData";
			this.colTextData.ReadOnly = true;
			this.colTextData.Width = 600;
			// 
			// colApplicationName
			// 
			this.colApplicationName.DataPropertyName = "ApplicationName";
			this.colApplicationName.HeaderText = "Application";
			this.colApplicationName.Name = "colApplicationName";
			this.colApplicationName.ReadOnly = true;
			this.colApplicationName.Width = 200;
			// 
			// colLoginName
			// 
			this.colLoginName.DataPropertyName = "LoginName";
			this.colLoginName.HeaderText = "Login";
			this.colLoginName.Name = "colLoginName";
			this.colLoginName.ReadOnly = true;
			this.colLoginName.Width = 200;
			// 
			// colStart
			// 
			this.colStart.DataPropertyName = "StartTime";
			this.colStart.HeaderText = "Start";
			this.colStart.Name = "colStart";
			this.colStart.ReadOnly = true;
			this.colStart.Width = 200;
			// 
			// colEnd
			// 
			this.colEnd.DataPropertyName = "EndTime";
			this.colEnd.HeaderText = "End";
			this.colEnd.Name = "colEnd";
			this.colEnd.ReadOnly = true;
			this.colEnd.Width = 200;
			// 
			// timer1
			// 
			this.timer1.Interval = 250;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.chkLogin);
			this.panel2.Controls.Add(this.chkApplication);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Controls.Add(this.btnClear);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 526);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(898, 38);
			this.panel2.TabIndex = 16;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(394, 12);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(33, 13);
			this.label3.TabIndex = 15;
			this.label3.Text = "Login";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Application";
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClear.Location = new System.Drawing.Point(804, 7);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(86, 23);
			this.btnClear.TabIndex = 12;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// chkLogin
			// 
			checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkLogin.CheckBoxProperties = checkBoxProperties1;
			this.chkLogin.DisplayMemberSingleItem = "";
			this.chkLogin.FormattingEnabled = true;
			this.chkLogin.Location = new System.Drawing.Point(433, 9);
			this.chkLogin.Name = "chkLogin";
			this.chkLogin.Size = new System.Drawing.Size(240, 21);
			this.chkLogin.TabIndex = 18;
			this.chkLogin.CheckBoxCheckedChanged += new System.EventHandler(this.chkLogin_CheckBoxCheckedChanged);
			// 
			// chkApplication
			// 
			checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
			this.chkApplication.CheckBoxProperties = checkBoxProperties2;
			this.chkApplication.DisplayMemberSingleItem = "";
			this.chkApplication.FormattingEnabled = true;
			this.chkApplication.Location = new System.Drawing.Point(73, 9);
			this.chkApplication.Name = "chkApplication";
			this.chkApplication.Size = new System.Drawing.Size(240, 21);
			this.chkApplication.TabIndex = 17;
			this.chkApplication.CheckBoxCheckedChanged += new System.EventHandler(this.chkApplication_CheckBoxCheckedChanged);
			// 
			// ucMonitor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gridResults);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.panel2);
			this.Name = "ucMonitor";
			this.Size = new System.Drawing.Size(898, 564);
			this.Load += new System.EventHandler(this.ucMonitor_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridResults)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnRemoveConnString;
		private System.Windows.Forms.ComboBox cboDatabase;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboConnectionString;
		private System.Windows.Forms.DataGridView gridResults;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.DataGridViewTextBoxColumn colTextData;
		private System.Windows.Forms.DataGridViewTextBoxColumn colApplicationName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colLoginName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colStart;
		private System.Windows.Forms.DataGridViewTextBoxColumn colEnd;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private WinControls.CheckBoxComboBox chkApplication;
		private WinControls.CheckBoxComboBox chkLogin;
	}
}
