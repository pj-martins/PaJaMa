namespace PaJaMa.DatabaseStudio.Query.QueryBuilder
{
	partial class frmQueryBuilder
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQueryBuilder));
			this.wizMain = new PaJaMa.WinControls.Wizard.Wizard();
			this.pageStart = new PaJaMa.WinControls.Wizard.WizardPage();
			this.grpOperations = new System.Windows.Forms.GroupBox();
			this.header1 = new PaJaMa.WinControls.Wizard.Header();
			this.rdbSelect = new System.Windows.Forms.RadioButton();
			this.pageOptions = new PaJaMa.WinControls.Wizard.WizardPage();
			this.header2 = new PaJaMa.WinControls.Wizard.Header();
			this.wizMain.SuspendLayout();
			this.pageStart.SuspendLayout();
			this.grpOperations.SuspendLayout();
			this.pageOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// wizMain
			// 
			this.wizMain.Controls.Add(this.pageStart);
			this.wizMain.Controls.Add(this.pageOptions);
			this.wizMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.wizMain.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.wizMain.Location = new System.Drawing.Point(0, 0);
			this.wizMain.Name = "wizMain";
			this.wizMain.Pages.AddRange(new PaJaMa.WinControls.Wizard.WizardPage[] {
            this.pageStart,
            this.pageOptions});
			this.wizMain.Size = new System.Drawing.Size(705, 529);
			this.wizMain.TabIndex = 0;
			// 
			// pageStart
			// 
			this.pageStart.Controls.Add(this.grpOperations);
			this.pageStart.Controls.Add(this.header1);
			this.pageStart.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pageStart.IsFinishPage = false;
			this.pageStart.Location = new System.Drawing.Point(0, 0);
			this.pageStart.Name = "pageStart";
			this.pageStart.Size = new System.Drawing.Size(705, 481);
			this.pageStart.TabIndex = 1;
			this.pageStart.CloseFromNext += new PaJaMa.WinControls.Wizard.PageEventHandler(this.pageStart_CloseFromNext);
			this.pageStart.ShowFromBack += new System.EventHandler(this.pageStart_ShowFromBack);
			// 
			// grpOperations
			// 
			this.grpOperations.Controls.Add(this.rdbSelect);
			this.grpOperations.Location = new System.Drawing.Point(32, 81);
			this.grpOperations.Name = "grpOperations";
			this.grpOperations.Size = new System.Drawing.Size(637, 376);
			this.grpOperations.TabIndex = 1;
			this.grpOperations.TabStop = false;
			// 
			// header1
			// 
			this.header1.BackColor = System.Drawing.SystemColors.Control;
			this.header1.CausesValidation = false;
			this.header1.Description = "Select operation type";
			this.header1.Dock = System.Windows.Forms.DockStyle.Top;
			this.header1.Image = ((System.Drawing.Image)(resources.GetObject("header1.Image")));
			this.header1.ImageVisible = false;
			this.header1.Location = new System.Drawing.Point(0, 0);
			this.header1.Name = "header1";
			this.header1.Size = new System.Drawing.Size(705, 64);
			this.header1.TabIndex = 0;
			this.header1.Title = "Query Builder";
			// 
			// rdbSelect
			// 
			this.rdbSelect.AutoSize = true;
			this.rdbSelect.Checked = true;
			this.rdbSelect.Location = new System.Drawing.Point(27, 36);
			this.rdbSelect.Name = "rdbSelect";
			this.rdbSelect.Size = new System.Drawing.Size(54, 17);
			this.rdbSelect.TabIndex = 0;
			this.rdbSelect.TabStop = true;
			this.rdbSelect.Text = "Select";
			this.rdbSelect.UseVisualStyleBackColor = true;
			// 
			// pageOptions
			// 
			this.pageOptions.Controls.Add(this.header2);
			this.pageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pageOptions.IsFinishPage = false;
			this.pageOptions.Location = new System.Drawing.Point(0, 0);
			this.pageOptions.Name = "pageOptions";
			this.pageOptions.Size = new System.Drawing.Size(705, 481);
			this.pageOptions.TabIndex = 2;
			this.pageOptions.ShowFromNext += new System.EventHandler(this.wizOptions_ShowFromNext);
			// 
			// header2
			// 
			this.header2.BackColor = System.Drawing.SystemColors.Control;
			this.header2.CausesValidation = false;
			this.header2.Description = "";
			this.header2.Dock = System.Windows.Forms.DockStyle.Top;
			this.header2.Image = ((System.Drawing.Image)(resources.GetObject("header2.Image")));
			this.header2.ImageVisible = false;
			this.header2.Location = new System.Drawing.Point(0, 0);
			this.header2.Name = "header2";
			this.header2.Size = new System.Drawing.Size(705, 64);
			this.header2.TabIndex = 0;
			this.header2.Title = "Options";
			// 
			// frmQueryBuilder
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(705, 529);
			this.ControlBox = false;
			this.Controls.Add(this.wizMain);
			this.Name = "frmQueryBuilder";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmQueryBuilder";
			this.Load += new System.EventHandler(this.frmQueryBuilder_Load);
			this.wizMain.ResumeLayout(false);
			this.pageStart.ResumeLayout(false);
			this.grpOperations.ResumeLayout(false);
			this.grpOperations.PerformLayout();
			this.pageOptions.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private WinControls.Wizard.Wizard wizMain;
		private WinControls.Wizard.WizardPage pageStart;
		private WinControls.Wizard.Header header1;
		private System.Windows.Forms.GroupBox grpOperations;
		private System.Windows.Forms.RadioButton rdbSelect;
		private WinControls.Wizard.WizardPage pageOptions;
		private WinControls.Wizard.Header header2;
	}
}