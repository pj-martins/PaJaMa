using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace WindowsExplorer
{
	/// <summary>
	/// Summary description for frmAbout.
	/// </summary>
	public class frmOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.CheckBox chkMyD;
		private System.Windows.Forms.CheckBox chkMyF;
		private System.Windows.Forms.CheckBox chkMyN;
		private System.Windows.Forms.Label label1;
		
		public bool myDocument =false;
		public bool myFavorite =false;
		public bool myNetwork =false;
		
		public bool myAddressbar =false;
		public bool myToolbar =false;

		private System.Windows.Forms.Label lblAbout;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox chkMyA;
		private System.Windows.Forms.CheckBox chkMyT;
		private System.ComponentModel.IContainer components;

		public frmOptions(bool myD, bool myF, bool myN, bool myA, bool myT)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			myDocument = myD ;
			myFavorite = myF ;
			myNetwork = myN ;
			myAddressbar = myA;
			myToolbar = myT;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmOptions));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.okButton = new System.Windows.Forms.Button();
			this.chkMyD = new System.Windows.Forms.CheckBox();
			this.chkMyF = new System.Windows.Forms.CheckBox();
			this.chkMyN = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lblAbout = new System.Windows.Forms.Label();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.chkMyT = new System.Windows.Forms.CheckBox();
			this.chkMyA = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(8, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(56, 48);
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(128, 200);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 20;
			this.okButton.Text = "Ok";
			// 
			// chkMyD
			// 
			this.chkMyD.Location = new System.Drawing.Point(96, 24);
			this.chkMyD.Name = "chkMyD";
			this.chkMyD.TabIndex = 21;
			this.chkMyD.Text = "My Documents";
			this.chkMyD.CheckedChanged += new System.EventHandler(this.chkMyD_CheckedChanged);
			// 
			// chkMyF
			// 
			this.chkMyF.Location = new System.Drawing.Point(96, 48);
			this.chkMyF.Name = "chkMyF";
			this.chkMyF.TabIndex = 22;
			this.chkMyF.Text = "My Favorites";
			this.chkMyF.CheckedChanged += new System.EventHandler(this.chkMyF_CheckedChanged);
			// 
			// chkMyN
			// 
			this.chkMyN.Location = new System.Drawing.Point(96, 72);
			this.chkMyN.Name = "chkMyN";
			this.chkMyN.TabIndex = 23;
			this.chkMyN.Text = "My Networks ";
			this.chkMyN.CheckedChanged += new System.EventHandler(this.chkMyN_CheckedChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(72, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 16);
			this.label1.TabIndex = 24;
			this.label1.Text = "Customize Explorer Tree";
			// 
			// lblAbout
			// 
			this.lblAbout.Location = new System.Drawing.Point(96, 144);
			this.lblAbout.Name = "lblAbout";
			this.lblAbout.Size = new System.Drawing.Size(200, 32);
			this.lblAbout.TabIndex = 25;
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.SystemColors.ControlDark;
			this.pictureBox2.Location = new System.Drawing.Point(32, 112);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(248, 1);
			this.pictureBox2.TabIndex = 26;
			this.pictureBox2.TabStop = false;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(96, 120);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(124, 16);
			this.label2.TabIndex = 27;
			this.label2.Text = "About Explorer Tree 1.0";
			// 
			// linkLabel1
			// 
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.Location = new System.Drawing.Point(96, 176);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(143, 16);
			this.linkLabel1.TabIndex = 28;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "For comments/suggestions ";
			this.toolTip1.SetToolTip(this.linkLabel1, "I hope you liked it , take care and don\'t forget to vote :)");
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// chkMyT
			// 
			this.chkMyT.Location = new System.Drawing.Point(200, 48);
			this.chkMyT.Name = "chkMyT";
			this.chkMyT.TabIndex = 30;
			this.chkMyT.Text = "Toolbar ";
			this.chkMyT.CheckedChanged += new System.EventHandler(this.chkMyT_CheckedChanged);
			// 
			// chkMyA
			// 
			this.chkMyA.Location = new System.Drawing.Point(200, 24);
			this.chkMyA.Name = "chkMyA";
			this.chkMyA.TabIndex = 29;
			this.chkMyA.Text = "Addressbar ";
			this.chkMyA.CheckedChanged += new System.EventHandler(this.chkMyA_CheckedChanged);
			// 
			// frmOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(322, 232);
			this.Controls.Add(this.chkMyT);
			this.Controls.Add(this.chkMyA);
			this.Controls.Add(this.linkLabel1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.lblAbout);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chkMyN);
			this.Controls.Add(this.chkMyF);
			this.Controls.Add(this.chkMyD);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmOptions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Customize Windows Explorer";
			this.Load += new System.EventHandler(this.frmOptions_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			
		}

		private void frmOptions_Load(object sender, System.EventArgs e)
		{
		 
			chkMyD.Checked = myDocument;
			chkMyF.Checked = myFavorite ;
			chkMyN.Checked = myNetwork;
			chkMyA.Checked = myAddressbar;
			chkMyT.Checked = myToolbar;  

			lblAbout.Text = "Windows Explorer Control\nCopyright: 2006 Rajesh Lal (quartz)";
	
			
		}

		private void chkMyD_CheckedChanged(object sender, System.EventArgs e)
		{
			myDocument =chkMyD.Checked ;
		}

		private void chkMyF_CheckedChanged(object sender, System.EventArgs e)
		{
			myFavorite =chkMyF.Checked ;
		}

		private void chkMyN_CheckedChanged(object sender, System.EventArgs e)
		{
			myNetwork = chkMyN.Checked;   
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			 
			System.Diagnostics.Process proc = new System.Diagnostics.Process();
			proc.EnableRaisingEvents=false;
			proc.StartInfo.FileName="iexplore";
			proc.StartInfo.Arguments= @"http://www.codeproject.com/script/Articles/list_articles.asp?userid=81898";
			proc.Start();
		}

		private void chkMyA_CheckedChanged(object sender, System.EventArgs e)
		{
			myAddressbar = chkMyA.Checked;  
		}

		private void chkMyT_CheckedChanged(object sender, System.EventArgs e)
		{
//			
//			if (!chkMyT.Checked)
//			{	
//				DialogResult = MessageBox.Show(" You won't be able to customize the settings, once you make the toolbar invisible. Do you want to continue?","Information ExplorerTree",MessageBoxButtons.YesNo ,MessageBoxIcon.Information );
//				if (DialogResult == DialogResult.Yes)  
//					chkMyT.Checked = true;
//				else
//					chkMyT.Checked = false;
//
//			}
			myToolbar = chkMyT.Checked; 



		}
	}
}
