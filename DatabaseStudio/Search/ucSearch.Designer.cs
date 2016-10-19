namespace PaJaMa.DatabaseStudio.Search
{
	partial class ucSearch
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gridTables = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ForeignKeys = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.mnuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectTop1000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gridColumns = new System.Windows.Forms.DataGridView();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnRemoveConnString = new System.Windows.Forms.Button();
			this.cboDatabase = new System.Windows.Forms.ComboBox();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cboConnectionString = new System.Windows.Forms.ComboBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabResults = new System.Windows.Forms.TabControl();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
			this.mnuMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gridTables);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.gridColumns);
			this.splitContainer1.Size = new System.Drawing.Size(865, 250);
			this.splitContainer1.SplitterDistance = 600;
			this.splitContainer1.TabIndex = 15;
			// 
			// gridTables
			// 
			this.gridTables.AllowUserToAddRows = false;
			this.gridTables.AllowUserToDeleteRows = false;
			this.gridTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.ForeignKeys});
			this.gridTables.ContextMenuStrip = this.mnuMain;
			this.gridTables.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTables.Location = new System.Drawing.Point(0, 0);
			this.gridTables.Name = "gridTables";
			this.gridTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridTables.Size = new System.Drawing.Size(600, 250);
			this.gridTables.TabIndex = 9;
			this.gridTables.SelectionChanged += new System.EventHandler(this.gridTables_SelectionChanged);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Table";
			this.dataGridViewTextBoxColumn1.HeaderText = "Table";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 220;
			// 
			// ForeignKeys
			// 
			this.ForeignKeys.DataPropertyName = "SelectAll";
			this.ForeignKeys.HeaderText = "Select";
			this.ForeignKeys.Name = "ForeignKeys";
			this.ForeignKeys.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ForeignKeys.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.selectTop1000ToolStripMenuItem});
			this.mnuMain.Name = "mnuTree";
			this.mnuMain.Size = new System.Drawing.Size(157, 48);
			// 
			// selectToolStripMenuItem
			// 
			this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
			this.selectToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.selectToolStripMenuItem.Text = "Select";
			// 
			// selectTop1000ToolStripMenuItem
			// 
			this.selectTop1000ToolStripMenuItem.Name = "selectTop1000ToolStripMenuItem";
			this.selectTop1000ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.selectTop1000ToolStripMenuItem.Text = "Select Top 1000";
			// 
			// gridColumns
			// 
			this.gridColumns.AllowUserToAddRows = false;
			this.gridColumns.AllowUserToDeleteRows = false;
			this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.Select});
			this.gridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridColumns.Location = new System.Drawing.Point(0, 0);
			this.gridColumns.Name = "gridColumns";
			this.gridColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridColumns.Size = new System.Drawing.Size(261, 250);
			this.gridColumns.TabIndex = 10;
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Enabled = false;
			this.btnRefresh.Location = new System.Drawing.Point(787, 4);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 3;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.txtSearch);
			this.panel2.Controls.Add(this.btnRefresh);
			this.panel2.Controls.Add(this.btnSearch);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 548);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(865, 30);
			this.panel2.TabIndex = 14;
			// 
			// txtSearch
			// 
			this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtSearch.Location = new System.Drawing.Point(11, 6);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(689, 20);
			this.txtSearch.TabIndex = 4;
			// 
			// btnSearch
			// 
			this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSearch.Enabled = false;
			this.btnSearch.Location = new System.Drawing.Point(706, 4);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 23);
			this.btnSearch.TabIndex = 2;
			this.btnSearch.Text = "Search";
			this.btnSearch.UseVisualStyleBackColor = true;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnRemoveConnString
			// 
			this.btnRemoveConnString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveConnString.Enabled = false;
			this.btnRemoveConnString.Location = new System.Drawing.Point(640, 12);
			this.btnRemoveConnString.Name = "btnRemoveConnString";
			this.btnRemoveConnString.Size = new System.Drawing.Size(121, 23);
			this.btnRemoveConnString.TabIndex = 3;
			this.btnRemoveConnString.Text = "Remove From List";
			this.btnRemoveConnString.UseVisualStyleBackColor = true;
			this.btnRemoveConnString.Click += new System.EventHandler(this.btnRemoveConnString_Click);
			// 
			// cboDatabase
			// 
			this.cboDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cboDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabase.FormattingEnabled = true;
			this.cboDatabase.Location = new System.Drawing.Point(640, 12);
			this.cboDatabase.Name = "cboDatabase";
			this.cboDatabase.Size = new System.Drawing.Size(121, 21);
			this.cboDatabase.TabIndex = 12;
			this.cboDatabase.Visible = false;
			this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisconnect.Location = new System.Drawing.Point(767, 12);
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
			this.btnConnect.Location = new System.Drawing.Point(767, 12);
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
			this.cboConnectionString.Size = new System.Drawing.Size(529, 21);
			this.cboConnectionString.TabIndex = 6;
			this.cboConnectionString.SelectedIndexChanged += new System.EventHandler(this.cboConnectionString_SelectedIndexChanged);
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
			this.panel1.Size = new System.Drawing.Size(865, 47);
			this.panel1.TabIndex = 13;
			// 
			// tabResults
			// 
			this.tabResults.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabResults.Location = new System.Drawing.Point(0, 0);
			this.tabResults.Name = "tabResults";
			this.tabResults.SelectedIndex = 0;
			this.tabResults.Size = new System.Drawing.Size(865, 247);
			this.tabResults.TabIndex = 16;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 47);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.tabResults);
			this.splitContainer2.Size = new System.Drawing.Size(865, 501);
			this.splitContainer2.SplitterDistance = 250;
			this.splitContainer2.TabIndex = 17;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Column";
			this.dataGridViewTextBoxColumn2.HeaderText = "Column";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 220;
			// 
			// Select
			// 
			this.Select.DataPropertyName = "Select";
			this.Select.HeaderText = "Select";
			this.Select.Name = "Select";
			// 
			// ucSearch
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer2);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "ucSearch";
			this.Size = new System.Drawing.Size(865, 578);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
			this.mnuMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView gridTables;
		private System.Windows.Forms.ContextMenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectTop1000ToolStripMenuItem;
		private System.Windows.Forms.DataGridView gridColumns;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnRemoveConnString;
		private System.Windows.Forms.ComboBox cboDatabase;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboConnectionString;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ForeignKeys;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.TabControl tabResults;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
	}
}
