namespace PaJaMa.DatabaseStudio.Compare
{
	partial class ucCompare
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnTargetQuery = new System.Windows.Forms.Button();
			this.btnSourceQuery = new System.Windows.Forms.Button();
			this.btnSwitch = new System.Windows.Forms.Button();
			this.btnRemoveTargetConnString = new System.Windows.Forms.Button();
			this.btnRemoveSourceConnString = new System.Windows.Forms.Button();
			this.cboTargetDatabase = new System.Windows.Forms.ComboBox();
			this.cboSourceDatabase = new System.Windows.Forms.ComboBox();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cboTarget = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cboSource = new System.Windows.Forms.ComboBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btnDataDifferences = new System.Windows.Forms.Button();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.btnViewCreates = new System.Windows.Forms.Button();
			this.btnViewMissingDependencies = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnGo = new System.Windows.Forms.Button();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.tabTables = new System.Windows.Forms.TabPage();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.gridTables = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TargetTable = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.SelectTableForStructure = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.SelectTableForData = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Delete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Truncate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.Identity = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.ForeignKeys = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.RemoveAddIndexes = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.TransferBatchSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.DataDetails = new System.Windows.Forms.DataGridViewButtonColumn();
			this.mnuMain = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectTop1000ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabObjects = new System.Windows.Forms.TabPage();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.gridObjects = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ObjectType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ProgTargetObject = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.tabDrop = new System.Windows.Forms.TabPage();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.gridDropObjects = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ObjectType2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setBatchSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.diffTables = new PaJaMa.DatabaseStudio.Compare.ucDifferences();
			this.diffObjects = new PaJaMa.DatabaseStudio.Compare.ucDifferences();
			this.diffDrops = new PaJaMa.DatabaseStudio.Compare.ucDifferences();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabMain.SuspendLayout();
			this.tabTables.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).BeginInit();
			this.mnuMain.SuspendLayout();
			this.tabObjects.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridObjects)).BeginInit();
			this.tabDrop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridDropObjects)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnTargetQuery);
			this.panel1.Controls.Add(this.btnSourceQuery);
			this.panel1.Controls.Add(this.btnSwitch);
			this.panel1.Controls.Add(this.btnRemoveTargetConnString);
			this.panel1.Controls.Add(this.btnRemoveSourceConnString);
			this.panel1.Controls.Add(this.cboTargetDatabase);
			this.panel1.Controls.Add(this.cboSourceDatabase);
			this.panel1.Controls.Add(this.btnDisconnect);
			this.panel1.Controls.Add(this.btnConnect);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.cboTarget);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.cboSource);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 24);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1065, 71);
			this.panel1.TabIndex = 8;
			// 
			// btnTargetQuery
			// 
			this.btnTargetQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnTargetQuery.Enabled = false;
			this.btnTargetQuery.Location = new System.Drawing.Point(768, 37);
			this.btnTargetQuery.Name = "btnTargetQuery";
			this.btnTargetQuery.Size = new System.Drawing.Size(66, 23);
			this.btnTargetQuery.TabIndex = 17;
			this.btnTargetQuery.Text = "Query";
			this.btnTargetQuery.UseVisualStyleBackColor = true;
			this.btnTargetQuery.Click += new System.EventHandler(this.btnTargetQuery_Click);
			// 
			// btnSourceQuery
			// 
			this.btnSourceQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSourceQuery.Enabled = false;
			this.btnSourceQuery.Location = new System.Drawing.Point(768, 12);
			this.btnSourceQuery.Name = "btnSourceQuery";
			this.btnSourceQuery.Size = new System.Drawing.Size(66, 23);
			this.btnSourceQuery.TabIndex = 16;
			this.btnSourceQuery.Text = "Query";
			this.btnSourceQuery.UseVisualStyleBackColor = true;
			this.btnSourceQuery.Click += new System.EventHandler(this.btnSourceQuery_Click);
			// 
			// btnSwitch
			// 
			this.btnSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSwitch.Location = new System.Drawing.Point(699, 15);
			this.btnSwitch.Name = "btnSwitch";
			this.btnSwitch.Size = new System.Drawing.Size(63, 45);
			this.btnSwitch.TabIndex = 15;
			this.btnSwitch.Text = "Switch";
			this.btnSwitch.UseVisualStyleBackColor = true;
			this.btnSwitch.Click += new System.EventHandler(this.btnSwitch_Click);
			// 
			// btnRemoveTargetConnString
			// 
			this.btnRemoveTargetConnString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveTargetConnString.Enabled = false;
			this.btnRemoveTargetConnString.Location = new System.Drawing.Point(840, 37);
			this.btnRemoveTargetConnString.Name = "btnRemoveTargetConnString";
			this.btnRemoveTargetConnString.Size = new System.Drawing.Size(121, 23);
			this.btnRemoveTargetConnString.TabIndex = 14;
			this.btnRemoveTargetConnString.Text = "Remove From List";
			this.btnRemoveTargetConnString.UseVisualStyleBackColor = true;
			this.btnRemoveTargetConnString.Click += new System.EventHandler(this.btnRemoveTargetConnString_Click);
			// 
			// btnRemoveSourceConnString
			// 
			this.btnRemoveSourceConnString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnRemoveSourceConnString.Enabled = false;
			this.btnRemoveSourceConnString.Location = new System.Drawing.Point(840, 12);
			this.btnRemoveSourceConnString.Name = "btnRemoveSourceConnString";
			this.btnRemoveSourceConnString.Size = new System.Drawing.Size(121, 23);
			this.btnRemoveSourceConnString.TabIndex = 3;
			this.btnRemoveSourceConnString.Text = "Remove From List";
			this.btnRemoveSourceConnString.UseVisualStyleBackColor = true;
			this.btnRemoveSourceConnString.Click += new System.EventHandler(this.btnRemoveSourceConnString_Click);
			// 
			// cboTargetDatabase
			// 
			this.cboTargetDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cboTargetDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboTargetDatabase.FormattingEnabled = true;
			this.cboTargetDatabase.Location = new System.Drawing.Point(840, 39);
			this.cboTargetDatabase.Name = "cboTargetDatabase";
			this.cboTargetDatabase.Size = new System.Drawing.Size(121, 21);
			this.cboTargetDatabase.TabIndex = 13;
			this.cboTargetDatabase.Visible = false;
			this.cboTargetDatabase.SelectedIndexChanged += new System.EventHandler(this.cboTargetDatabase_SelectedIndexChanged);
			// 
			// cboSourceDatabase
			// 
			this.cboSourceDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cboSourceDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSourceDatabase.FormattingEnabled = true;
			this.cboSourceDatabase.Location = new System.Drawing.Point(840, 12);
			this.cboSourceDatabase.Name = "cboSourceDatabase";
			this.cboSourceDatabase.Size = new System.Drawing.Size(121, 21);
			this.cboSourceDatabase.TabIndex = 12;
			this.cboSourceDatabase.Visible = false;
			this.cboSourceDatabase.SelectedIndexChanged += new System.EventHandler(this.cboSourceDatabase_SelectedIndexChanged);
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDisconnect.Location = new System.Drawing.Point(967, 15);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(86, 45);
			this.btnDisconnect.TabIndex = 11;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Visible = false;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnect.Location = new System.Drawing.Point(967, 15);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(86, 45);
			this.btnConnect.TabIndex = 10;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(125, 13);
			this.label2.TabIndex = 9;
			this.label2.Text = "Target Connection String";
			// 
			// cboTarget
			// 
			this.cboTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboTarget.FormattingEnabled = true;
			this.cboTarget.Location = new System.Drawing.Point(142, 39);
			this.cboTarget.Name = "cboTarget";
			this.cboTarget.Size = new System.Drawing.Size(551, 21);
			this.cboTarget.TabIndex = 8;
			this.cboTarget.SelectedIndexChanged += new System.EventHandler(this.cboConnString_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(128, 13);
			this.label1.TabIndex = 7;
			this.label1.Text = "Source Connection String";
			// 
			// cboSource
			// 
			this.cboSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboSource.FormattingEnabled = true;
			this.cboSource.Location = new System.Drawing.Point(142, 12);
			this.cboSource.Name = "cboSource";
			this.cboSource.Size = new System.Drawing.Size(551, 21);
			this.cboSource.TabIndex = 6;
			this.cboSource.SelectedIndexChanged += new System.EventHandler(this.cboConnString_SelectedIndexChanged);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btnDataDifferences);
			this.panel2.Controls.Add(this.btnSelectAll);
			this.panel2.Controls.Add(this.btnViewCreates);
			this.panel2.Controls.Add(this.btnViewMissingDependencies);
			this.panel2.Controls.Add(this.btnRefresh);
			this.panel2.Controls.Add(this.btnGo);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 609);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(1065, 30);
			this.panel2.TabIndex = 11;
			// 
			// btnDataDifferences
			// 
			this.btnDataDifferences.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnDataDifferences.Location = new System.Drawing.Point(282, 3);
			this.btnDataDifferences.Name = "btnDataDifferences";
			this.btnDataDifferences.Size = new System.Drawing.Size(138, 23);
			this.btnDataDifferences.TabIndex = 7;
			this.btnDataDifferences.Text = "Data Differences";
			this.btnDataDifferences.UseVisualStyleBackColor = true;
			this.btnDataDifferences.Click += new System.EventHandler(this.btnDataDifferences_Click);
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSelectAll.Enabled = false;
			this.btnSelectAll.Location = new System.Drawing.Point(814, 3);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(86, 23);
			this.btnSelectAll.TabIndex = 6;
			this.btnSelectAll.Text = "Select All";
			this.btnSelectAll.UseVisualStyleBackColor = true;
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// btnViewCreates
			// 
			this.btnViewCreates.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnViewCreates.Enabled = false;
			this.btnViewCreates.Location = new System.Drawing.Point(426, 3);
			this.btnViewCreates.Name = "btnViewCreates";
			this.btnViewCreates.Size = new System.Drawing.Size(188, 23);
			this.btnViewCreates.TabIndex = 5;
			this.btnViewCreates.Text = "Compare Create Scripts";
			this.btnViewCreates.UseVisualStyleBackColor = true;
			this.btnViewCreates.Click += new System.EventHandler(this.btnViewCreates_Click);
			// 
			// btnViewMissingDependencies
			// 
			this.btnViewMissingDependencies.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnViewMissingDependencies.Enabled = false;
			this.btnViewMissingDependencies.Location = new System.Drawing.Point(620, 3);
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
			this.btnRefresh.Location = new System.Drawing.Point(906, 3);
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
			this.btnGo.Location = new System.Drawing.Point(987, 3);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(75, 23);
			this.btnGo.TabIndex = 2;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// tabMain
			// 
			this.tabMain.Controls.Add(this.tabTables);
			this.tabMain.Controls.Add(this.tabObjects);
			this.tabMain.Controls.Add(this.tabDrop);
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Location = new System.Drawing.Point(0, 95);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(1065, 514);
			this.tabMain.TabIndex = 13;
			this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
			// 
			// tabTables
			// 
			this.tabTables.Controls.Add(this.splitContainer1);
			this.tabTables.Location = new System.Drawing.Point(4, 22);
			this.tabTables.Name = "tabTables";
			this.tabTables.Padding = new System.Windows.Forms.Padding(3);
			this.tabTables.Size = new System.Drawing.Size(1057, 488);
			this.tabTables.TabIndex = 0;
			this.tabTables.Text = "Tables";
			this.tabTables.UseVisualStyleBackColor = true;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(3, 3);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.gridTables);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.diffTables);
			this.splitContainer1.Size = new System.Drawing.Size(1051, 482);
			this.splitContainer1.SplitterDistance = 751;
			this.splitContainer1.TabIndex = 12;
			// 
			// gridTables
			// 
			this.gridTables.AllowUserToAddRows = false;
			this.gridTables.AllowUserToDeleteRows = false;
			this.gridTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridTables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.TargetTable,
            this.SelectTableForStructure,
            this.SelectTableForData,
            this.Delete,
            this.Truncate,
            this.Identity,
            this.ForeignKeys,
            this.RemoveAddIndexes,
            this.TransferBatchSize,
            this.DataDetails});
			this.gridTables.ContextMenuStrip = this.mnuMain;
			this.gridTables.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridTables.Location = new System.Drawing.Point(0, 0);
			this.gridTables.Name = "gridTables";
			this.gridTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridTables.Size = new System.Drawing.Size(751, 482);
			this.gridTables.TabIndex = 9;
			this.gridTables.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTables_CellContentClick);
			this.gridTables.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTables_CellValueChanged);
			this.gridTables.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
			this.gridTables.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.gridTables_DataError);
			this.gridTables.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.gridTables_EditingControlShowing);
			this.gridTables.SelectionChanged += new System.EventHandler(this.gridTables_SelectionChanged);
			this.gridTables.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridTables_MouseClick);
			this.gridTables.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gridTables_MouseDown);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.DataPropertyName = "SourceTable";
			this.dataGridViewTextBoxColumn1.HeaderText = "Table";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Width = 220;
			// 
			// TargetTable
			// 
			this.TargetTable.DataPropertyName = "TargetTable";
			this.TargetTable.HeaderText = "Target";
			this.TargetTable.Name = "TargetTable";
			this.TargetTable.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.TargetTable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.TargetTable.Width = 220;
			// 
			// SelectTableForStructure
			// 
			this.SelectTableForStructure.DataPropertyName = "Select";
			this.SelectTableForStructure.HeaderText = "Structure";
			this.SelectTableForStructure.Name = "SelectTableForStructure";
			// 
			// SelectTableForData
			// 
			this.SelectTableForData.DataPropertyName = "SelectTableForData";
			this.SelectTableForData.HeaderText = "Data";
			this.SelectTableForData.Name = "SelectTableForData";
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
			// Identity
			// 
			this.Identity.DataPropertyName = "KeepIdentity";
			this.Identity.HeaderText = "Keep Identity";
			this.Identity.Name = "Identity";
			this.Identity.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.Identity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// ForeignKeys
			// 
			this.ForeignKeys.DataPropertyName = "RemoveAddKeys";
			this.ForeignKeys.HeaderText = "Remove & Add Foreign Keys";
			this.ForeignKeys.Name = "ForeignKeys";
			this.ForeignKeys.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ForeignKeys.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// RemoveAddIndexes
			// 
			this.RemoveAddIndexes.DataPropertyName = "RemoveAddIndexes";
			this.RemoveAddIndexes.HeaderText = "Remove & Add Indexes";
			this.RemoveAddIndexes.Name = "RemoveAddIndexes";
			this.RemoveAddIndexes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			// 
			// TransferBatchSize
			// 
			this.TransferBatchSize.DataPropertyName = "TransferBatchSize";
			this.TransferBatchSize.HeaderText = "Transfer Batch Size";
			this.TransferBatchSize.Name = "TransferBatchSize";
			this.TransferBatchSize.Visible = false;
			// 
			// DataDetails
			// 
			this.DataDetails.HeaderText = "Data";
			this.DataDetails.Name = "DataDetails";
			this.DataDetails.Text = "Data";
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.selectTop1000ToolStripMenuItem,
            this.setBatchSizeToolStripMenuItem});
			this.mnuMain.Name = "mnuTree";
			this.mnuMain.Size = new System.Drawing.Size(156, 92);
			this.mnuMain.Opening += new System.ComponentModel.CancelEventHandler(this.mnuMain_Opening);
			// 
			// selectToolStripMenuItem
			// 
			this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
			this.selectToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.selectToolStripMenuItem.Text = "Select";
			this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
			// 
			// selectTop1000ToolStripMenuItem
			// 
			this.selectTop1000ToolStripMenuItem.Name = "selectTop1000ToolStripMenuItem";
			this.selectTop1000ToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.selectTop1000ToolStripMenuItem.Text = "Select Top 1000";
			this.selectTop1000ToolStripMenuItem.Click += new System.EventHandler(this.selectTop1000ToolStripMenuItem_Click);
			// 
			// tabObjects
			// 
			this.tabObjects.Controls.Add(this.splitContainer3);
			this.tabObjects.Location = new System.Drawing.Point(4, 22);
			this.tabObjects.Name = "tabObjects";
			this.tabObjects.Padding = new System.Windows.Forms.Padding(3);
			this.tabObjects.Size = new System.Drawing.Size(1057, 488);
			this.tabObjects.TabIndex = 1;
			this.tabObjects.Text = "Objects";
			this.tabObjects.UseVisualStyleBackColor = true;
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(3, 3);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.gridObjects);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.diffObjects);
			this.splitContainer3.Size = new System.Drawing.Size(1051, 482);
			this.splitContainer3.SplitterDistance = 744;
			this.splitContainer3.TabIndex = 12;
			// 
			// gridObjects
			// 
			this.gridObjects.AllowUserToAddRows = false;
			this.gridObjects.AllowUserToDeleteRows = false;
			this.gridObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.ObjectType,
            this.ProgTargetObject,
            this.dataGridViewCheckBoxColumn2});
			this.gridObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridObjects.Location = new System.Drawing.Point(0, 0);
			this.gridObjects.Name = "gridObjects";
			this.gridObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridObjects.Size = new System.Drawing.Size(744, 482);
			this.gridObjects.TabIndex = 11;
			this.gridObjects.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridObjects_CellContentClick);
			this.gridObjects.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
			this.gridObjects.SelectionChanged += new System.EventHandler(this.gridObjects_SelectionChanged);
			this.gridObjects.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridObjects_MouseClick);
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.DataPropertyName = "Source";
			this.dataGridViewTextBoxColumn2.HeaderText = "Object";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Width = 220;
			// 
			// ObjectType
			// 
			this.ObjectType.DataPropertyName = "Type";
			this.ObjectType.HeaderText = "Type";
			this.ObjectType.Name = "ObjectType";
			this.ObjectType.ReadOnly = true;
			// 
			// ProgTargetObject
			// 
			this.ProgTargetObject.DataPropertyName = "Target";
			this.ProgTargetObject.HeaderText = "Target";
			this.ProgTargetObject.Name = "ProgTargetObject";
			this.ProgTargetObject.ReadOnly = true;
			this.ProgTargetObject.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.ProgTargetObject.Width = 220;
			// 
			// dataGridViewCheckBoxColumn2
			// 
			this.dataGridViewCheckBoxColumn2.DataPropertyName = "Select";
			this.dataGridViewCheckBoxColumn2.HeaderText = "Select";
			this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
			// 
			// tabDrop
			// 
			this.tabDrop.Controls.Add(this.splitContainer2);
			this.tabDrop.Location = new System.Drawing.Point(4, 22);
			this.tabDrop.Name = "tabDrop";
			this.tabDrop.Padding = new System.Windows.Forms.Padding(3);
			this.tabDrop.Size = new System.Drawing.Size(1057, 488);
			this.tabDrop.TabIndex = 2;
			this.tabDrop.Text = "Drop";
			this.tabDrop.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(3, 3);
			this.splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.gridDropObjects);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.diffDrops);
			this.splitContainer2.Size = new System.Drawing.Size(1051, 482);
			this.splitContainer2.SplitterDistance = 750;
			this.splitContainer2.TabIndex = 3;
			// 
			// gridDropObjects
			// 
			this.gridDropObjects.AllowUserToAddRows = false;
			this.gridDropObjects.AllowUserToDeleteRows = false;
			this.gridDropObjects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridDropObjects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.ObjectType2,
            this.dataGridViewCheckBoxColumn1});
			this.gridDropObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.gridDropObjects.Location = new System.Drawing.Point(0, 0);
			this.gridDropObjects.Name = "gridDropObjects";
			this.gridDropObjects.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridDropObjects.Size = new System.Drawing.Size(750, 482);
			this.gridDropObjects.TabIndex = 2;
			this.gridDropObjects.CurrentCellDirtyStateChanged += new System.EventHandler(this.grid_CurrentCellDirtyStateChanged);
			this.gridDropObjects.SelectionChanged += new System.EventHandler(this.gridDropObjects_SelectionChanged);
			// 
			// dataGridViewTextBoxColumn3
			// 
			this.dataGridViewTextBoxColumn3.DataPropertyName = "Name";
			this.dataGridViewTextBoxColumn3.HeaderText = "Object";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Width = 220;
			// 
			// ObjectType2
			// 
			this.ObjectType2.DataPropertyName = "Type";
			this.ObjectType2.HeaderText = "Type";
			this.ObjectType2.Name = "ObjectType2";
			// 
			// dataGridViewCheckBoxColumn1
			// 
			this.dataGridViewCheckBoxColumn1.DataPropertyName = "Select";
			this.dataGridViewCheckBoxColumn1.HeaderText = "Select";
			this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
			// 
			// menuStrip1
			// 
			this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1065, 24);
			this.menuStrip1.TabIndex = 14;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// setBatchSizeToolStripMenuItem
			// 
			this.setBatchSizeToolStripMenuItem.Name = "setBatchSizeToolStripMenuItem";
			this.setBatchSizeToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			this.setBatchSizeToolStripMenuItem.Text = "Set Batch Size";
			this.setBatchSizeToolStripMenuItem.Visible = false;
			this.setBatchSizeToolStripMenuItem.Click += new System.EventHandler(this.setBatchSizeToolStripMenuItem_Click);
			// 
			// diffTables
			// 
			this.diffTables.CompareHelper = null;
			this.diffTables.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diffTables.Location = new System.Drawing.Point(0, 0);
			this.diffTables.Name = "diffTables";
			this.diffTables.Size = new System.Drawing.Size(296, 482);
			this.diffTables.TabIndex = 0;
			this.diffTables.Workspace = null;
			// 
			// diffObjects
			// 
			this.diffObjects.CompareHelper = null;
			this.diffObjects.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diffObjects.Location = new System.Drawing.Point(0, 0);
			this.diffObjects.Name = "diffObjects";
			this.diffObjects.Size = new System.Drawing.Size(303, 482);
			this.diffObjects.TabIndex = 0;
			this.diffObjects.Workspace = null;
			// 
			// diffDrops
			// 
			this.diffDrops.CompareHelper = null;
			this.diffDrops.Dock = System.Windows.Forms.DockStyle.Fill;
			this.diffDrops.Location = new System.Drawing.Point(0, 0);
			this.diffDrops.Name = "diffDrops";
			this.diffDrops.Size = new System.Drawing.Size(297, 482);
			this.diffDrops.TabIndex = 1;
			this.diffDrops.Workspace = null;
			// 
			// ucCompare
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.Name = "ucCompare";
			this.Size = new System.Drawing.Size(1065, 639);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.tabMain.ResumeLayout(false);
			this.tabTables.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridTables)).EndInit();
			this.mnuMain.ResumeLayout(false);
			this.tabObjects.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridObjects)).EndInit();
			this.tabDrop.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.gridDropObjects)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnRemoveTargetConnString;
		private System.Windows.Forms.Button btnRemoveSourceConnString;
		private System.Windows.Forms.ComboBox cboTargetDatabase;
		private System.Windows.Forms.ComboBox cboSourceDatabase;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cboTarget;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cboSource;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TabControl tabMain;
		private System.Windows.Forms.TabPage tabTables;
		private System.Windows.Forms.TabPage tabObjects;
		private System.Windows.Forms.TabPage tabDrop;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView gridTables;
		private System.Windows.Forms.DataGridView gridDropObjects;
		private System.Windows.Forms.DataGridView gridObjects;
		private ucDifferences diffTables;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private ucDifferences diffObjects;
		private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.Button btnViewMissingDependencies;
		private System.Windows.Forms.Button btnSwitch;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn ObjectType;
		private System.Windows.Forms.DataGridViewTextBoxColumn ProgTargetObject;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Button btnViewCreates;
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.DataGridViewTextBoxColumn ObjectType2;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
		private ucDifferences diffDrops;
		private System.Windows.Forms.Button btnTargetQuery;
		private System.Windows.Forms.Button btnSourceQuery;
		private System.Windows.Forms.ContextMenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectTop1000ToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.Button btnDataDifferences;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewComboBoxColumn TargetTable;
		private System.Windows.Forms.DataGridViewCheckBoxColumn SelectTableForStructure;
		private System.Windows.Forms.DataGridViewCheckBoxColumn SelectTableForData;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Delete;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Truncate;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Identity;
		private System.Windows.Forms.DataGridViewCheckBoxColumn ForeignKeys;
		private System.Windows.Forms.DataGridViewCheckBoxColumn RemoveAddIndexes;
		private System.Windows.Forms.DataGridViewTextBoxColumn TransferBatchSize;
		private System.Windows.Forms.DataGridViewButtonColumn DataDetails;
		private System.Windows.Forms.ToolStripMenuItem setBatchSizeToolStripMenuItem;
	}
}

