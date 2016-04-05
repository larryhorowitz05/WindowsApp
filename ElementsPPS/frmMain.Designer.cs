namespace ElementsPPS
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.lblversion = new System.Windows.Forms.Label();
            this.chkUseFiles = new System.Windows.Forms.CheckBox();
            this.grpDebug = new System.Windows.Forms.GroupBox();
            this.chkSaveImg = new System.Windows.Forms.CheckBox();
            this.btnPnl = new System.Windows.Forms.Panel();
            this.prefBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.templateImg = new AxGdPicturePro5.AxImaging();
            this.axImg = new AxGdPicturePro5.AxImaging();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.grpDebug.SuspendLayout();
            this.btnPnl.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.templateImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axImg)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBox1
            // 
            this.PictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("PictureBox1.BackgroundImage")));
            this.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.PictureBox1.Location = new System.Drawing.Point(-37, 0);
            this.PictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(827, 75);
            this.PictureBox1.TabIndex = 14;
            this.PictureBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(600, 10);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(167, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpenFolder.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenFolder.Location = new System.Drawing.Point(203, 10);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(167, 28);
            this.btnOpenFolder.TabIndex = 1;
            this.btnOpenFolder.Text = "Open Folder";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.btnOpenFolder_Click);
            // 
            // btnScan
            // 
            this.btnScan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnScan.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScan.Location = new System.Drawing.Point(4, 10);
            this.btnScan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(167, 28);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(16, 98);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtOutput.Size = new System.Drawing.Size(772, 418);
            this.txtOutput.TabIndex = 9;
            // 
            // lblversion
            // 
            this.lblversion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblversion.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblversion.Location = new System.Drawing.Point(165, 79);
            this.lblversion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblversion.Name = "lblversion";
            this.lblversion.Size = new System.Drawing.Size(624, 16);
            this.lblversion.TabIndex = 8;
            this.lblversion.Text = "lblversion";
            this.lblversion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkUseFiles
            // 
            this.chkUseFiles.AutoSize = true;
            this.chkUseFiles.Location = new System.Drawing.Point(64, 25);
            this.chkUseFiles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseFiles.Name = "chkUseFiles";
            this.chkUseFiles.Size = new System.Drawing.Size(222, 21);
            this.chkUseFiles.TabIndex = 16;
            this.chkUseFiles.Text = "Acquire Images From Files";
            this.chkUseFiles.UseVisualStyleBackColor = true;
            // 
            // grpDebug
            // 
            this.grpDebug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDebug.Controls.Add(this.chkSaveImg);
            this.grpDebug.Controls.Add(this.chkUseFiles);
            this.grpDebug.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDebug.Location = new System.Drawing.Point(16, 588);
            this.grpDebug.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpDebug.Name = "grpDebug";
            this.grpDebug.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpDebug.Size = new System.Drawing.Size(773, 58);
            this.grpDebug.TabIndex = 17;
            this.grpDebug.TabStop = false;
            this.grpDebug.Text = "Debug Options";
            // 
            // chkSaveImg
            // 
            this.chkSaveImg.AutoSize = true;
            this.chkSaveImg.Location = new System.Drawing.Point(340, 25);
            this.chkSaveImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkSaveImg.Name = "chkSaveImg";
            this.chkSaveImg.Size = new System.Drawing.Size(194, 21);
            this.chkSaveImg.TabIndex = 17;
            this.chkSaveImg.Text = "Save Template Images";
            this.chkSaveImg.UseVisualStyleBackColor = true;
            // 
            // btnPnl
            // 
            this.btnPnl.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPnl.Controls.Add(this.prefBtn);
            this.btnPnl.Controls.Add(this.btnScan);
            this.btnPnl.Controls.Add(this.btnOpenFolder);
            this.btnPnl.Controls.Add(this.btnClose);
            this.btnPnl.Location = new System.Drawing.Point(16, 524);
            this.btnPnl.Margin = new System.Windows.Forms.Padding(0);
            this.btnPnl.Name = "btnPnl";
            this.btnPnl.Size = new System.Drawing.Size(773, 42);
            this.btnPnl.TabIndex = 19;
            // 
            // prefBtn
            // 
            this.prefBtn.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.prefBtn.Location = new System.Drawing.Point(401, 10);
            this.prefBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.prefBtn.Name = "prefBtn";
            this.prefBtn.Size = new System.Drawing.Size(167, 28);
            this.prefBtn.TabIndex = 2;
            this.prefBtn.Text = "Select Scanner";
            this.prefBtn.UseVisualStyleBackColor = true;
            this.prefBtn.Click += new System.EventHandler(this.prefBtn_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(137)))), ((int)(((byte)(137)))));
            this.panel2.Controls.Add(this.templateImg);
            this.panel2.Controls.Add(this.axImg);
            this.panel2.Controls.Add(this.PictureBox1);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(789, 75);
            this.panel2.TabIndex = 20;
            // 
            // templateImg
            // 
            this.templateImg.Enabled = true;
            this.templateImg.Location = new System.Drawing.Point(47, 13);
            this.templateImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.templateImg.Name = "templateImg";
            this.templateImg.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("templateImg.OcxState")));
            this.templateImg.Size = new System.Drawing.Size(35, 35);
            this.templateImg.TabIndex = 18;
            // 
            // axImg
            // 
            this.axImg.Enabled = true;
            this.axImg.Location = new System.Drawing.Point(12, 13);
            this.axImg.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.axImg.Name = "axImg";
            this.axImg.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axImg.OcxState")));
            this.axImg.Size = new System.Drawing.Size(35, 35);
            this.axImg.TabIndex = 15;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(805, 569);
            this.Controls.Add(this.btnPnl);
            this.Controls.Add(this.grpDebug);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.lblversion);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(821, 358);
            this.Name = "frmMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Elements PPS";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.grpDebug.ResumeLayout(false);
            this.grpDebug.PerformLayout();
            this.btnPnl.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.templateImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axImg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.PictureBox PictureBox1;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnOpenFolder;
        internal System.Windows.Forms.Button btnScan;
        internal System.Windows.Forms.TextBox txtOutput;
        internal System.Windows.Forms.Label lblversion;
        private System.Windows.Forms.CheckBox chkUseFiles;
        private System.Windows.Forms.GroupBox grpDebug;
        private System.Windows.Forms.CheckBox chkSaveImg;
				private System.Windows.Forms.Panel btnPnl;
				private System.Windows.Forms.Button prefBtn;
				private System.Windows.Forms.Panel panel2;
				private AxGdPicturePro5.AxImaging templateImg;
				private AxGdPicturePro5.AxImaging axImg;
    }
}

