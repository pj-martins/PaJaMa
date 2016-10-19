namespace PaJaMa.DatabaseStudio.DataGenerate
{
	partial class ucDataGenerate
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnQuery = new System.Windows.Forms.Button();
			this.btnRemoveConnString = new System.Windows.Forms.Button();
			this.cboDatabase = new System.Windows.Forms.ComboBox();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cboConnectionString = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnAddNRows = new System.Windows.Forms.Button();
			this.btnAdd10 = new System.Windows.Forms.Button();
			this.btnViewMissingDependencies = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.gridTables = new System.Windows.Forms.DataGridView();
			this.mnuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectTop1000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gridColumns = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Content = new System.Windows.Forms.DataGridViewButtonColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.CurrentRows = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.AddRows = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Delete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Truncate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ForeignKeys = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
			this.mnuMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).BeginInit();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnQuery);
			this.panel1.Controls.Add(this.btnRemoveConnString);
			this.panel1.Controls.Add(this.cboDatabase);
			this.panel1.Controls.Add(this.btnDisconnect);
			this.panel1.Controls.Add(this.btnConnect);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.cboConnectionString);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(891, 47);
			this.panel1.TabIndex = 8;
			// 
			// btnQuery
			// 
			this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnQuery.Enabled = false;
			this.btnQuery.Location = new System.Drawing.Point(594, 12);
			this.btnQuery.Name = "btnQuery";
			this.btnQuery.Size = new System.Drawing.Size(66, 23);
			this.btnQuery.TabIndex = 17;
			this.btnQuery.Text = "Query";
			this.btnQuery.UseVisualStyleBackColor = true;
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			// 
			// btnRemoveConnString
			// 
			this.btnRemoveConnString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveConnString.Enabled = false;
			this.btnRemoveConnString.Location = new System.Drawing.Point(666, 12);
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
			this.cboDatabase.Location = new System.Drawing.Point(666, 12);
			this.cboDatabase.Name = "cboDatabase";
			this.cboDatabase.Size = new System.Drawing.Size(121, 21);
			this.cboDatabase.TabIndex = 12;
			this.cboDatabase.Visible = false;
			this.cboDatabase.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisconnect.Location = new System.Drawing.Point(793, 12);
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
			this.btnConnect.Location = new System.Drawing.Point(793, 12);
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
			this.cboConnectionString.Size = new System.Drawing.Size(483, 21);
			this.cboConnectionString.TabIndex = 6;
			this.cboConnectionString.SelectedIndexChanged += new System.EventHandler(this.cboConnString_SelectedIndexChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnAddNRows);
			this.panel2.Controls.Add(this.btnAdd10);
			this.panel2.Controls.Add(this.btnViewMissingDependencies);
			this.panel2.Controls.Add(this.btnRefresh);
			this.panel2.Controls.Add(this.btnGo);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 593);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(891, 30);
			this.panel2.TabIndex = 11;
			// 
			// btnAddNRows
			// 
			this.btnAddNRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAddNRows.Enabled = false;
			this.btnAddNRows.Location = new System.Drawing.Point(549, 3);
			this.btnAddNRows.Name = "btnAddNRows";
			this.btnAddNRows.Size = new System.Drawing.Size(86, 23);
			this.btnAddNRows.TabIndex = 7;
			this.btnAddNRows.Text = "Add N Rows";
			this.btnAddNRows.UseVisualStyleBackColor = true;
			this.btnAddNRows.Click += new System.EventHandler(this.btnAddNRows_Click);
			// 
			// btnAdd10
			// 
			this.btnAdd10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAdd10.Enabled = false;
			this.btnAdd10.Location = new System.Drawing.Point(640, 3);
			this.btnAdd10.Name = "btnAdd10";
			this.btnAdd10.Size = new System.Drawing.Size(86, 23);
			this.btnAdd10.TabIndex = 6;
			this.btnAdd10.Text = "Add 10 Rows";
			this.btnAdd10.UseVisualStyleBackColor = true;
			this.btnAdd10.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// btnViewMissingDependencies
			// 
			this.btnViewMissingDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnViewMissingDependencies.Enabled = false;
			this.btnViewMissingDependencies.Location = new System.Drawing.Point(355, 3);
			this.btnViewMissingDependencies.Name = "btnViewMissingDependencies";
			this.btnViewMissingDependencies.Size = new System.Drawing.Size(188, 23);
			this.btnViewMissingDependencies.TabIndex = 4;
			this.btnViewMissingDependencies.Text = "View Missing Dependencies";
			this.btnViewMissingDependencies.UseVisualStyleBackColor = true;
			this.btnViewMissingDependencies.Click += new System.EventHandler(this.btnViewMissingDependencies_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRefresh.Enabled = false;
			this.btnRefresh.Location = new System.Drawing.Point(732, 3);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 3;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Enabled = false;
			this.btnGo.Location = new System.Drawing.Point(813, 3);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// gridTables
			// 
			this.gridTables.AllowUserToAddRows = false;
			this.gridTables.AllowUserToDeleteRows = false;
			this.gridTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.CurrentRows,
            this.AddRows,
            this.Delete,
            this.Truncate,
            this.ForeignKeys});
			this.gridTables.ContextMenuStrip = this.mnuMain;
			this.gridTables.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTables.Location = new System.Drawing.Point(0, 0);
			this.gridTables.Name = "gridTables";
			this.gridTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridTables.Size = new System.Drawing.Size(619, 546);
			this.gridTables.TabIndex = 9;
			this.gridTables.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTables_DataError);
			this.gridTables.SelectionChanged += new System.EventHandler(this.gridTables_SelectionChanged);
			this.gridTables.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridTables_MouseClick);
			this.gridTables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridTables_MouseDown);
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
			this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
			// 
			// selectTop1000ToolStripMenuItem
			// 
			this.selectTop1000ToolStripMenuItem.Name = "selectTop1000ToolStripMenuItem";
			this.selectTop1000ToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.selectTop1000ToolStripMenuItem.Text = "Select Top 1000";
			this.selectTop1000ToolStripMenuItem.Click += new System.EventHandler(this.selectTop1000ToolStripMenuItem_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 47);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gridTables);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.gridColumns);
			this.splitContainer1.Size = new System.Drawing.Size(891, 546);
			this.splitContainer1.SplitterDistance = 619;
			this.splitContainer1.TabIndex = 12;
			// 
			// gridColumns
			// 
			this.gridColumns.AllowUserToAddRows = false;
			this.gridColumns.AllowUserToDeleteRows = false;
			this.gridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.Content});
			this.gridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridColumns.Location = new System.Drawing.Point(0, 0);
			this.gridColumns.Name = "gridColumns";
			this.gridColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridColumns.Size = new System.Drawing.Size(268, 546);
			this.gridColumns.TabIndex = 10;
			this.gridColumns.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridColumns_CellContentClick);
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Column";
			this.dataGridViewTextBoxColumn2.HeaderText = "Column";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 220;
			// 
			// Content
			// 
			this.Content.DataPropertyName = "Content";
			this.Content.HeaderText = "Content";
			this.Content.Name = "Content";
			this.Content.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Content.Width = 200;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "Table";
			this.dataGridViewTextBoxColumn1.HeaderText = "Table";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 220;
			// 
			// CurrentRows
			// 
			this.CurrentRows.DataPropertyName = "CurrentRowCount";
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
			dataGridViewCellStyle1.Format = "N0";
			dataGridViewCellStyle1.NullValue = null;
			this.CurrentRows.DefaultCellStyle = dataGridViewCellStyle1;
			this.CurrentRows.HeaderText = "Current";
			this.CurrentRows.Name = "CurrentRows";
			this.CurrentRows.ReadOnly = true;
			// 
			// AddRows
			// 
			this.AddRows.DataPropertyName = "AddRowCount";
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight;
			dataGridViewCellStyle2.Format = "N0";
			dataGridViewCellStyle2.NullValue = null;
			this.AddRows.DefaultCellStyle = dataGridViewCellStyle2;
			this.AddRows.HeaderText = "Add Rows";
			this.AddRows.Name = "AddRows";
			// 
			// Delete
			// 
			this.Delete.DataPropertyName = "Delete";
			this.Delete.HeaderText = "Delete";
			this.Delete.Name = "Delete";
			// 
			// Truncate
			// 
			this.Truncate.DataPropertyName = "Truncate";
			this.Truncate.HeaderText = "Truncate";
			this.Truncate.Name = "Truncate";
			// 
			// ForeignKeys
			// 
			this.ForeignKeys.DataPropertyName = "RemoveAddKeys";
			this.ForeignKeys.HeaderText = "Remove & Add Foreign Keys";
			this.ForeignKeys.Name = "ForeignKeys";
			this.ForeignKeys.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ForeignKeys.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// ucDataGenerate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "ucDataGenerate";
			this.Size = new System.Drawing.Size(891, 623);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
			this.mnuMain.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridColumns)).EndInit();
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
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.DataGridView gridTables;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnViewMissingDependencies;
		private System.Windows.Forms.Button btnAdd10;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView gridColumns;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewButtonColumn Content;
		private System.Windows.Forms.Button btnQuery;
		private System.Windows.Forms.ContextMenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectTop1000ToolStripMenuItem;
		private System.Windows.Forms.Button btnAddNRows;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn CurrentRows;
		private System.Windows.Forms.DataGridViewTextBoxColumn AddRows;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Delete;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Truncate;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ForeignKeys;
	}
}

