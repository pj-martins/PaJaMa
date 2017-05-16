namespace PaJaMa.DatabaseStudio.Query.QueryBuilder.SelectOptions
{
	partial class ucFromJoin
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
			this.lblFromJoin = new System.Windows.Forms.Label();
			this.DatabaseName = new System.Windows.Forms.ComboBox();
			this.TableName = new System.Windows.Forms.ComboBox();
			this.Alias = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnAddColumn = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblFromJoin
			// 
			this.lblFromJoin.AutoSize = true;
			this.lblFromJoin.Location = new System.Drawing.Point(15, 10);
			this.lblFromJoin.Name = "lblFromJoin";
			this.lblFromJoin.Size = new System.Drawing.Size(35, 13);
			this.lblFromJoin.TabIndex = 0;
			this.lblFromJoin.Text = "label1";
			// 
			// DatabaseName
			// 
			this.DatabaseName.FormattingEnabled = true;
			this.DatabaseName.Location = new System.Drawing.Point(56, 7);
			this.DatabaseName.Name = "DatabaseName";
			this.DatabaseName.Size = new System.Drawing.Size(181, 21);
			this.DatabaseName.TabIndex = 1;
			this.DatabaseName.SelectedIndexChanged += new System.EventHandler(this.cboDatabase_SelectedIndexChanged);
			// 
			// TableName
			// 
			this.TableName.FormattingEnabled = true;
			this.TableName.Location = new System.Drawing.Point(243, 7);
			this.TableName.Name = "TableName";
			this.TableName.Size = new System.Drawing.Size(179, 21);
			this.TableName.TabIndex = 2;
			this.TableName.SelectedIndexChanged += new System.EventHandler(this.cboTable_SelectedIndexChanged);
			// 
			// Alias
			// 
			this.Alias.Location = new System.Drawing.Point(451, 7);
			this.Alias.Name = "Alias";
			this.Alias.Size = new System.Drawing.Size(112, 20);
			this.Alias.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(427, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(18, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "as";
			// 
			// btnAddColumn
			// 
			this.btnAddColumn.Location = new System.Drawing.Point(569, 5);
			this.btnAddColumn.Name = "btnAddColumn";
			this.btnAddColumn.Size = new System.Drawing.Size(25, 23);
			this.btnAddColumn.TabIndex = 4;
			this.btnAddColumn.Text = "+";
			this.btnAddColumn.UseVisualStyleBackColor = true;
			this.btnAddColumn.Click += new System.EventHandler(this.btnAddColumn_Click);
			// 
			// ucFromJoin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnAddColumn);
			this.Controls.Add(this.Alias);
			this.Controls.Add(this.TableName);
			this.Controls.Add(this.DatabaseName);
			this.Controls.Add(this.lblFromJoin);
			this.Name = "ucFromJoin";
			this.Size = new System.Drawing.Size(707, 89);
			this.Load += new System.EventHandler(this.ucFromJoin_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lblFromJoin;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnAddColumn;
		internal System.Windows.Forms.ComboBox DatabaseName;
		internal System.Windows.Forms.ComboBox TableName;
		internal System.Windows.Forms.TextBox Alias;
	}
}
