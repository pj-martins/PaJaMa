namespace PaJaMa.DatabaseStudio.Query
{
	partial class ucQueryOutput
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
			this.splitQuery = new System.Windows.Forms.SplitContainer();
			this.txtQuery = new System.Windows.Forms.RichTextBox();
			this.pnlResults = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabResults = new System.Windows.Forms.TabPage();
			this.tabMessages = new System.Windows.Forms.TabPage();
			this.txtMessages = new System.Windows.Forms.TextBox();
			this.pnlButtons = new System.Windows.Forms.Panel();
			this.lblDatabase = new System.Windows.Forms.Label();
			this.cboDatabases = new System.Windows.Forms.ComboBox();
			this.lblTime = new System.Windows.Forms.Label();
			this.lblResults = new System.Windows.Forms.Label();
			this.btnStop = new System.Windows.Forms.Button();
			this.progMain = new System.Windows.Forms.ProgressBar();
			this.btnGo = new System.Windows.Forms.Button();
			this.timDuration = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitQuery)).BeginInit();
			this.splitQuery.Panel1.SuspendLayout();
			this.splitQuery.Panel2.SuspendLayout();
			this.splitQuery.SuspendLayout();
			this.pnlResults.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabMessages.SuspendLayout();
			this.pnlButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitQuery
			// 
			this.splitQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitQuery.Enabled = false;
			this.splitQuery.Location = new System.Drawing.Point(0, 0);
			this.splitQuery.Name = "splitQuery";
			this.splitQuery.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitQuery.Panel1
			// 
			this.splitQuery.Panel1.Controls.Add(this.txtQuery);
			// 
			// splitQuery.Panel2
			// 
			this.splitQuery.Panel2.Controls.Add(this.pnlResults);
			this.splitQuery.Panel2MinSize = 0;
			this.splitQuery.Size = new System.Drawing.Size(926, 488);
			this.splitQuery.SplitterDistance = 216;
			this.splitQuery.TabIndex = 8;
			// 
			// txtQuery
			// 
			this.txtQuery.AcceptsTab = true;
			this.txtQuery.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtQuery.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtQuery.Location = new System.Drawing.Point(0, 0);
			this.txtQuery.Name = "txtQuery";
			this.txtQuery.Size = new System.Drawing.Size(926, 216);
			this.txtQuery.TabIndex = 6;
			this.txtQuery.Text = "";
			this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
			// 
			// pnlResults
			// 
			this.pnlResults.AutoScroll = true;
			this.pnlResults.Controls.Add(this.tabControl1);
			this.pnlResults.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlResults.Location = new System.Drawing.Point(0, 0);
			this.pnlResults.Name = "pnlResults";
			this.pnlResults.Size = new System.Drawing.Size(926, 268);
			this.pnlResults.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabResults);
			this.tabControl1.Controls.Add(this.tabMessages);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(926, 268);
			this.tabControl1.TabIndex = 0;
			// 
			// tabResults
			// 
			this.tabResults.Location = new System.Drawing.Point(4, 22);
			this.tabResults.Name = "tabResults";
			this.tabResults.Padding = new System.Windows.Forms.Padding(3);
			this.tabResults.Size = new System.Drawing.Size(918, 242);
			this.tabResults.TabIndex = 0;
			this.tabResults.Text = "Results";
			this.tabResults.UseVisualStyleBackColor = true;
			// 
			// tabMessages
			// 
			this.tabMessages.Controls.Add(this.txtMessages);
			this.tabMessages.Location = new System.Drawing.Point(4, 22);
			this.tabMessages.Name = "tabMessages";
			this.tabMessages.Padding = new System.Windows.Forms.Padding(3);
			this.tabMessages.Size = new System.Drawing.Size(918, 242);
			this.tabMessages.TabIndex = 1;
			this.tabMessages.Text = "Messages";
			this.tabMessages.UseVisualStyleBackColor = true;
			// 
			// txtMessages
			// 
			this.txtMessages.BackColor = System.Drawing.Color.White;
			this.txtMessages.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtMessages.Font = new System.Drawing.Font("Courier New", 9.75F);
			this.txtMessages.Location = new System.Drawing.Point(3, 3);
			this.txtMessages.Multiline = true;
			this.txtMessages.Name = "txtMessages";
			this.txtMessages.ReadOnly = true;
			this.txtMessages.Size = new System.Drawing.Size(912, 236);
			this.txtMessages.TabIndex = 0;
			// 
			// pnlButtons
			// 
			this.pnlButtons.Controls.Add(this.lblDatabase);
			this.pnlButtons.Controls.Add(this.cboDatabases);
			this.pnlButtons.Controls.Add(this.lblTime);
			this.pnlButtons.Controls.Add(this.lblResults);
			this.pnlButtons.Controls.Add(this.btnStop);
			this.pnlButtons.Controls.Add(this.progMain);
			this.pnlButtons.Controls.Add(this.btnGo);
			this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlButtons.Enabled = false;
			this.pnlButtons.Location = new System.Drawing.Point(0, 488);
			this.pnlButtons.Name = "pnlButtons";
			this.pnlButtons.Size = new System.Drawing.Size(926, 39);
			this.pnlButtons.TabIndex = 9;
			// 
			// lblDatabase
			// 
			this.lblDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.lblDatabase.AutoSize = true;
			this.lblDatabase.Location = new System.Drawing.Point(608, 13);
			this.lblDatabase.Name = "lblDatabase";
			this.lblDatabase.Size = new System.Drawing.Size(53, 13);
			this.lblDatabase.TabIndex = 9;
			this.lblDatabase.Text = "Database";
			// 
			// cboDatabases
			// 
			this.cboDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cboDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDatabases.FormattingEnabled = true;
			this.cboDatabases.Location = new System.Drawing.Point(667, 9);
			this.cboDatabases.Name = "cboDatabases";
			this.cboDatabases.Size = new System.Drawing.Size(142, 21);
			this.cboDatabases.TabIndex = 8;
			this.cboDatabases.SelectedIndexChanged += new System.EventHandler(this.cboDatabases_SelectedIndexChanged);
			// 
			// lblTime
			// 
			this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTime.AutoSize = true;
			this.lblTime.BackColor = System.Drawing.Color.Transparent;
			this.lblTime.Location = new System.Drawing.Point(386, 13);
			this.lblTime.Name = "lblTime";
			this.lblTime.Size = new System.Drawing.Size(35, 13);
			this.lblTime.TabIndex = 4;
			this.lblTime.Text = "label2";
			this.lblTime.Visible = false;
			// 
			// lblResults
			// 
			this.lblResults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblResults.AutoSize = true;
			this.lblResults.Location = new System.Drawing.Point(464, 13);
			this.lblResults.Name = "lblResults";
			this.lblResults.Size = new System.Drawing.Size(42, 13);
			this.lblResults.TabIndex = 3;
			this.lblResults.Text = "Results";
			this.lblResults.Visible = false;
			// 
			// btnStop
			// 
			this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnStop.Location = new System.Drawing.Point(815, 8);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(102, 23);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "Stop";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Visible = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// progMain
			// 
			this.progMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.progMain.Location = new System.Drawing.Point(15, 8);
			this.progMain.Name = "progMain";
			this.progMain.Size = new System.Drawing.Size(365, 23);
			this.progMain.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
			this.progMain.TabIndex = 1;
			this.progMain.Visible = false;
			// 
			// btnGo
			// 
			this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnGo.Location = new System.Drawing.Point(815, 8);
			this.btnGo.Name = "btnGo";
			this.btnGo.Size = new System.Drawing.Size(102, 23);
			this.btnGo.TabIndex = 0;
			this.btnGo.Text = "Go";
			this.btnGo.UseVisualStyleBackColor = true;
			this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
			// 
			// timDuration
			// 
			this.timDuration.Interval = 1000;
			this.timDuration.Tick += new System.EventHandler(this.timDuration_Tick);
			// 
			// ucQueryOutput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.splitQuery);
			this.Controls.Add(this.pnlButtons);
			this.Name = "ucQueryOutput";
			this.Size = new System.Drawing.Size(926, 527);
			this.splitQuery.Panel1.ResumeLayout(false);
			this.splitQuery.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitQuery)).EndInit();
			this.splitQuery.ResumeLayout(false);
			this.pnlResults.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabMessages.ResumeLayout(false);
			this.tabMessages.PerformLayout();
			this.pnlButtons.ResumeLayout(false);
			this.pnlButtons.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitQuery;
		private System.Windows.Forms.Panel pnlResults;
		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Label lblTime;
		private System.Windows.Forms.Label lblResults;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.ProgressBar progMain;
		private System.Windows.Forms.Button btnGo;
		private System.Windows.Forms.Timer timDuration;
		internal System.Windows.Forms.RichTextBox txtQuery;
		private System.Windows.Forms.Label lblDatabase;
		internal System.Windows.Forms.ComboBox cboDatabases;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabResults;
		private System.Windows.Forms.TabPage tabMessages;
		private System.Windows.Forms.TextBox txtMessages;
	}
}
