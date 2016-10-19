namespace PaJaMa.ScreenCapture.ImageEditor
{
	partial class ucImageEditor
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
			this.pnlTools = new System.Windows.Forms.Panel();
			this.btnZoomIn = new System.Windows.Forms.Button();
			this.numZoom = new System.Windows.Forms.NumericUpDown();
			this.btnZoomOut = new System.Windows.Forms.Button();
			this.btnRedo = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnFont = new System.Windows.Forms.Button();
			this.btnUndo = new System.Windows.Forms.Button();
			this.pnlLineTools = new System.Windows.Forms.Panel();
			this.numWidth = new System.Windows.Forms.NumericUpDown();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.pnlBlurTools = new System.Windows.Forms.Panel();
			this.numRadius = new System.Windows.Forms.NumericUpDown();
			this.btnCrop = new System.Windows.Forms.Button();
			this.chkHighlight = new System.Windows.Forms.CheckBox();
			this.chkErase = new System.Windows.Forms.CheckBox();
			this.pnlCurrentColor = new System.Windows.Forms.Panel();
			this.numAlpha = new System.Windows.Forms.NumericUpDown();
			this.dlgColor = new System.Windows.Forms.ColorDialog();
			this.dlgFont = new System.Windows.Forms.FontDialog();
			this.dlgSaveAs = new System.Windows.Forms.SaveFileDialog();
			this.pnlPicture = new System.Windows.Forms.Panel();
			this.pictMain = new PaJaMa.ScreenCapture.ImageEditor.SelectablePictureBox();
			this.pnlTools.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numZoom)).BeginInit();
			this.pnlLineTools.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
			this.pnlBlurTools.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numRadius)).BeginInit();
			this.pnlCurrentColor.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numAlpha)).BeginInit();
			this.pnlPicture.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictMain)).BeginInit();
			this.SuspendLayout();
			// 
			// pnlTools
			// 
			this.pnlTools.Controls.Add(this.btnZoomIn);
			this.pnlTools.Controls.Add(this.numZoom);
			this.pnlTools.Controls.Add(this.btnZoomOut);
			this.pnlTools.Controls.Add(this.btnRedo);
			this.pnlTools.Controls.Add(this.btnDelete);
			this.pnlTools.Controls.Add(this.btnFont);
			this.pnlTools.Controls.Add(this.btnUndo);
			this.pnlTools.Controls.Add(this.pnlLineTools);
			this.pnlTools.Controls.Add(this.btnCopy);
			this.pnlTools.Controls.Add(this.btnSave);
			this.pnlTools.Controls.Add(this.pnlBlurTools);
			this.pnlTools.Controls.Add(this.btnCrop);
			this.pnlTools.Controls.Add(this.chkHighlight);
			this.pnlTools.Controls.Add(this.chkErase);
			this.pnlTools.Controls.Add(this.pnlCurrentColor);
			this.pnlTools.Dock = System.Windows.Forms.DockStyle.Left;
			this.pnlTools.Location = new System.Drawing.Point(0, 0);
			this.pnlTools.Name = "pnlTools";
			this.pnlTools.Size = new System.Drawing.Size(49, 489);
			this.pnlTools.TabIndex = 4;
			// 
			// btnZoomIn
			// 
			this.btnZoomIn.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnZoomIn.Image = global::PaJaMa.ScreenCapture.Properties.Resources.zoomin;
			this.btnZoomIn.Location = new System.Drawing.Point(0, 306);
			this.btnZoomIn.Name = "btnZoomIn";
			this.btnZoomIn.Size = new System.Drawing.Size(49, 27);
			this.btnZoomIn.TabIndex = 21;
			this.btnZoomIn.UseVisualStyleBackColor = true;
			this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
			// 
			// numZoom
			// 
			this.numZoom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.numZoom.Location = new System.Drawing.Point(0, 333);
			this.numZoom.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
			this.numZoom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numZoom.Name = "numZoom";
			this.numZoom.Size = new System.Drawing.Size(49, 20);
			this.numZoom.TabIndex = 22;
			this.numZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.numZoom.ValueChanged += new System.EventHandler(this.numZoom_ValueChanged);
			// 
			// btnZoomOut
			// 
			this.btnZoomOut.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnZoomOut.Image = global::PaJaMa.ScreenCapture.Properties.Resources.zoomout;
			this.btnZoomOut.Location = new System.Drawing.Point(0, 353);
			this.btnZoomOut.Name = "btnZoomOut";
			this.btnZoomOut.Size = new System.Drawing.Size(49, 27);
			this.btnZoomOut.TabIndex = 20;
			this.btnZoomOut.UseVisualStyleBackColor = true;
			this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
			// 
			// btnRedo
			// 
			this.btnRedo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnRedo.Image = global::PaJaMa.ScreenCapture.Properties.Resources.redo;
			this.btnRedo.Location = new System.Drawing.Point(0, 380);
			this.btnRedo.Name = "btnRedo";
			this.btnRedo.Size = new System.Drawing.Size(49, 27);
			this.btnRedo.TabIndex = 16;
			this.btnRedo.UseVisualStyleBackColor = true;
			this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnDelete.Image = global::PaJaMa.ScreenCapture.Properties.Resources.delete;
			this.btnDelete.Location = new System.Drawing.Point(0, 217);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(49, 27);
			this.btnDelete.TabIndex = 14;
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Visible = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnFont
			// 
			this.btnFont.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnFont.Image = global::PaJaMa.ScreenCapture.Properties.Resources.font;
			this.btnFont.Location = new System.Drawing.Point(0, 190);
			this.btnFont.Name = "btnFont";
			this.btnFont.Size = new System.Drawing.Size(49, 27);
			this.btnFont.TabIndex = 13;
			this.btnFont.UseVisualStyleBackColor = true;
			this.btnFont.Visible = false;
			this.btnFont.Click += new System.EventHandler(this.btnFont_Click);
			// 
			// btnUndo
			// 
			this.btnUndo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnUndo.Image = global::PaJaMa.ScreenCapture.Properties.Resources.undo;
			this.btnUndo.Location = new System.Drawing.Point(0, 407);
			this.btnUndo.Name = "btnUndo";
			this.btnUndo.Size = new System.Drawing.Size(49, 27);
			this.btnUndo.TabIndex = 1;
			this.btnUndo.UseVisualStyleBackColor = true;
			this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
			// 
			// pnlLineTools
			// 
			this.pnlLineTools.Controls.Add(this.numWidth);
			this.pnlLineTools.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlLineTools.Location = new System.Drawing.Point(0, 156);
			this.pnlLineTools.Name = "pnlLineTools";
			this.pnlLineTools.Size = new System.Drawing.Size(49, 34);
			this.pnlLineTools.TabIndex = 12;
			this.pnlLineTools.Visible = false;
			// 
			// numWidth
			// 
			this.numWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.numWidth.Location = new System.Drawing.Point(4, 6);
			this.numWidth.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWidth.Name = "numWidth";
			this.numWidth.Size = new System.Drawing.Size(42, 20);
			this.numWidth.TabIndex = 7;
			this.numWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numWidth.ValueChanged += new System.EventHandler(this.numWidth_ValueChanged);
			// 
			// btnCopy
			// 
			this.btnCopy.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnCopy.Image = global::PaJaMa.ScreenCapture.Properties.Resources.copy;
			this.btnCopy.Location = new System.Drawing.Point(0, 434);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(49, 28);
			this.btnCopy.TabIndex = 6;
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnSave
			// 
			this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.btnSave.Image = global::PaJaMa.ScreenCapture.Properties.Resources.save;
			this.btnSave.Location = new System.Drawing.Point(0, 462);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(49, 27);
			this.btnSave.TabIndex = 3;
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// pnlBlurTools
			// 
			this.pnlBlurTools.Controls.Add(this.numRadius);
			this.pnlBlurTools.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlBlurTools.Location = new System.Drawing.Point(0, 122);
			this.pnlBlurTools.Name = "pnlBlurTools";
			this.pnlBlurTools.Size = new System.Drawing.Size(49, 34);
			this.pnlBlurTools.TabIndex = 17;
			this.pnlBlurTools.Visible = false;
			// 
			// numRadius
			// 
			this.numRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.numRadius.Location = new System.Drawing.Point(4, 6);
			this.numRadius.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numRadius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRadius.Name = "numRadius";
			this.numRadius.Size = new System.Drawing.Size(42, 20);
			this.numRadius.TabIndex = 7;
			this.numRadius.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.numRadius.ValueChanged += new System.EventHandler(this.numRadius_ValueChanged);
			// 
			// btnCrop
			// 
			this.btnCrop.Dock = System.Windows.Forms.DockStyle.Top;
			this.btnCrop.Image = global::PaJaMa.ScreenCapture.Properties.Resources.crop;
			this.btnCrop.Location = new System.Drawing.Point(0, 95);
			this.btnCrop.Name = "btnCrop";
			this.btnCrop.Size = new System.Drawing.Size(49, 27);
			this.btnCrop.TabIndex = 10;
			this.btnCrop.UseVisualStyleBackColor = true;
			this.btnCrop.Visible = false;
			this.btnCrop.Click += new System.EventHandler(this.btnCrop_Click);
			// 
			// chkHighlight
			// 
			this.chkHighlight.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkHighlight.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkHighlight.Image = global::PaJaMa.ScreenCapture.Properties.Resources.highlight;
			this.chkHighlight.Location = new System.Drawing.Point(0, 68);
			this.chkHighlight.Name = "chkHighlight";
			this.chkHighlight.Size = new System.Drawing.Size(49, 27);
			this.chkHighlight.TabIndex = 19;
			this.chkHighlight.UseVisualStyleBackColor = true;
			this.chkHighlight.CheckedChanged += new System.EventHandler(this.chkHighlight_CheckedChanged);
			// 
			// chkErase
			// 
			this.chkErase.Appearance = System.Windows.Forms.Appearance.Button;
			this.chkErase.Dock = System.Windows.Forms.DockStyle.Top;
			this.chkErase.Image = global::PaJaMa.ScreenCapture.Properties.Resources.erase;
			this.chkErase.Location = new System.Drawing.Point(0, 41);
			this.chkErase.Name = "chkErase";
			this.chkErase.Size = new System.Drawing.Size(49, 27);
			this.chkErase.TabIndex = 18;
			this.chkErase.UseVisualStyleBackColor = true;
			this.chkErase.CheckedChanged += new System.EventHandler(this.chkErase_CheckedChanged);
			// 
			// pnlCurrentColor
			// 
			this.pnlCurrentColor.Controls.Add(this.numAlpha);
			this.pnlCurrentColor.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlCurrentColor.Location = new System.Drawing.Point(0, 0);
			this.pnlCurrentColor.Name = "pnlCurrentColor";
			this.pnlCurrentColor.Size = new System.Drawing.Size(49, 41);
			this.pnlCurrentColor.TabIndex = 8;
			this.pnlCurrentColor.Visible = false;
			this.pnlCurrentColor.DoubleClick += new System.EventHandler(this.btnColor_Click);
			// 
			// numAlpha
			// 
			this.numAlpha.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.numAlpha.Location = new System.Drawing.Point(4, 16);
			this.numAlpha.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numAlpha.Name = "numAlpha";
			this.numAlpha.Size = new System.Drawing.Size(42, 20);
			this.numAlpha.TabIndex = 8;
			this.numAlpha.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.numAlpha.ValueChanged += new System.EventHandler(this.numAlpha_ValueChanged);
			// 
			// pnlPicture
			// 
			this.pnlPicture.AutoScroll = true;
			this.pnlPicture.Controls.Add(this.pictMain);
			this.pnlPicture.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlPicture.Location = new System.Drawing.Point(49, 0);
			this.pnlPicture.Name = "pnlPicture";
			this.pnlPicture.Size = new System.Drawing.Size(649, 489);
			this.pnlPicture.TabIndex = 7;
			// 
			// pictMain
			// 
			this.pictMain.ImageLocation = "";
			this.pictMain.Location = new System.Drawing.Point(0, 0);
			this.pictMain.Name = "pictMain";
			this.pictMain.Size = new System.Drawing.Size(646, 486);
			this.pictMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictMain.TabIndex = 6;
			this.pictMain.TabStop = false;
			this.pictMain.DoubleClick += new System.EventHandler(this.pictMain_DoubleClick);
			this.pictMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictMain_MouseDown);
			this.pictMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictMain_MouseMove);
			this.pictMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictMain_MouseUp);
			// 
			// ucImageEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.pnlPicture);
			this.Controls.Add(this.pnlTools);
			this.Name = "ucImageEditor";
			this.Size = new System.Drawing.Size(698, 489);
			this.Load += new System.EventHandler(this.ucImageEditor_Load);
			this.Resize += new System.EventHandler(this.ucImageEditor_Resize);
			this.pnlTools.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numZoom)).EndInit();
			this.pnlLineTools.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
			this.pnlBlurTools.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numRadius)).EndInit();
			this.pnlCurrentColor.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.numAlpha)).EndInit();
			this.pnlPicture.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictMain)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel pnlTools;
		private System.Windows.Forms.Button btnUndo;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.ColorDialog dlgColor;
		private System.Windows.Forms.Panel pnlCurrentColor;
		private System.Windows.Forms.Button btnCrop;
		private System.Windows.Forms.Button btnFont;
		private System.Windows.Forms.FontDialog dlgFont;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.SaveFileDialog dlgSaveAs;
		private System.Windows.Forms.Button btnRedo;
		private System.Windows.Forms.Panel pnlLineTools;
		private System.Windows.Forms.NumericUpDown numWidth;
		private System.Windows.Forms.Panel pnlBlurTools;
		private System.Windows.Forms.NumericUpDown numRadius;
		private System.Windows.Forms.NumericUpDown numAlpha;
		private System.Windows.Forms.CheckBox chkHighlight;
		private System.Windows.Forms.CheckBox chkErase;
		private SelectablePictureBox pictMain;
		private System.Windows.Forms.Panel pnlPicture;
		private System.Windows.Forms.Button btnZoomIn;
		private System.Windows.Forms.Button btnZoomOut;
		private System.Windows.Forms.NumericUpDown numZoom;
	}
}
