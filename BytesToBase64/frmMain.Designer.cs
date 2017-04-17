using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PaJaMa.BytesToBase64
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>

		public frmMain()
		{
			this.InitializeComponent();
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
				this.components.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.dlgFiles = new OpenFileDialog();
			this.txtFiles = new TextBox();
			this.btnConvert = new Button();
			this.btnBrowse = new Button();
			this.txtFormatString = new TextBox();
			this.label1 = new Label();
			this.txtOutput = new TextBox();
			this.SuspendLayout();
			this.dlgFiles.Multiselect = true;
			this.txtFiles.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.txtFiles.Location = new Point(12, 12);
			this.txtFiles.Name = "txtFiles";
			this.txtFiles.ReadOnly = true;
			this.txtFiles.Size = new Size(705, 20);
			this.txtFiles.TabIndex = 0;
			this.btnConvert.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.btnConvert.Location = new Point(804, 11);
			this.btnConvert.Name = "btnConvert";
			this.btnConvert.Size = new Size(75, 23);
			this.btnConvert.TabIndex = 1;
			this.btnConvert.Text = "Convert";
			this.btnConvert.UseVisualStyleBackColor = true;
			this.btnConvert.Click += new EventHandler(this.btnConvert_Click);
			this.btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.btnBrowse.Location = new Point(723, 11);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new Size(75, 23);
			this.btnBrowse.TabIndex = 2;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new EventHandler(this.btnBrowse_Click);
			this.txtFormatString.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.txtFormatString.Location = new Point(300, 38);
			this.txtFormatString.Name = "txtFormatString";
			this.txtFormatString.Size = new Size(579, 20);
			this.txtFormatString.TabIndex = 3;
			this.txtFormatString.Text = ".{0} {{ background: url(data:image/{1};base64,{2}); }}";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 41);
			this.label1.Name = "label1";
			this.label1.Size = new Size(282, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "Format String - {0}: File Name, {1}: Extension, {2}: Base 64";
			this.txtOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.txtOutput.Location = new Point(15, 64);
			this.txtOutput.Multiline = true;
			this.txtOutput.Name = "txtOutput";
			this.txtOutput.ScrollBars = ScrollBars.Both;
			this.txtOutput.Size = new Size(864, 490);
			this.txtOutput.TabIndex = 5;
			this.txtOutput.WordWrap = false;
			this.AutoScaleDimensions = new SizeF(6f, 13f);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(891, 566);
			this.Controls.Add((Control)this.txtOutput);
			this.Controls.Add((Control)this.label1);
			this.Controls.Add((Control)this.txtFormatString);
			this.Controls.Add((Control)this.btnBrowse);
			this.Controls.Add((Control)this.btnConvert);
			this.Controls.Add((Control)this.txtFiles);
			this.Name = "frmMain";
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Bytes to Base 64";
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private IContainer components = (IContainer)null;
		private OpenFileDialog dlgFiles;
		private TextBox txtFiles;
		private Button btnConvert;
		private Button btnBrowse;
		private TextBox txtFormatString;
		private Label label1;
		private TextBox txtOutput;
	}

	
}

