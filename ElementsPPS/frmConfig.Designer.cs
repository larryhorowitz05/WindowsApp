namespace ElementsPPS
{
#if false
    partial class frmConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfig));
            this.label1 = new System.Windows.Forms.Label();
            this.txtClientId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboFormType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtThrInc = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtShift = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboDebug = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDocFolder = new System.Windows.Forms.Button();
            this.txtDocFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboAutoUpdate = new System.Windows.Forms.ComboBox();
            this.lblAutoUpdate = new System.Windows.Forms.Label();
            this.cboUseHttps = new System.Windows.Forms.ComboBox();
            this.lblUseHttps = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.lblServer = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client ID:";
            // 
            // txtClientId
            // 
            this.txtClientId.Location = new System.Drawing.Point(90, 12);
            this.txtClientId.Name = "txtClientId";
            this.txtClientId.Size = new System.Drawing.Size(100, 21);
            this.txtClientId.TabIndex = 1;
            this.txtClientId.TextChanged += new System.EventHandler(this.txtClientId_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Form Type:";
            // 
            // cboFormType
            // 
            this.cboFormType.FormattingEnabled = true;
            this.cboFormType.Items.AddRange(new object[] {
            "PlainPaper",
            "HALO"});
            this.cboFormType.Location = new System.Drawing.Point(90, 39);
            this.cboFormType.Name = "cboFormType";
            this.cboFormType.Size = new System.Drawing.Size(121, 21);
            this.cboFormType.TabIndex = 3;
            this.cboFormType.SelectedIndexChanged += new System.EventHandler(this.cboFormType_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txtThrInc);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtShift);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboDebug);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnDocFolder);
            this.groupBox1.Controls.Add(this.txtDocFolder);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cboAutoUpdate);
            this.groupBox1.Controls.Add(this.lblAutoUpdate);
            this.groupBox1.Controls.Add(this.cboUseHttps);
            this.groupBox1.Controls.Add(this.lblUseHttps);
            this.groupBox1.Controls.Add(this.txtServer);
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.lblWarning);
            this.groupBox1.Controls.Add(this.shapeContainer1);
            this.groupBox1.Location = new System.Drawing.Point(12, 66);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(392, 293);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Advanced";
            // 
            // txtThrInc
            // 
            this.txtThrInc.Location = new System.Drawing.Point(135, 262);
            this.txtThrInc.Name = "txtThrInc";
            this.txtThrInc.Size = new System.Drawing.Size(43, 21);
            this.txtThrInc.TabIndex = 20;
            this.txtThrInc.TextChanged += new System.EventHandler(this.txtThrInc_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 265);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Threshold Increase:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(135, 235);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(43, 21);
            this.txtSearch.TabIndex = 18;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 238);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Search Area:";
            // 
            // txtShift
            // 
            this.txtShift.Location = new System.Drawing.Point(135, 208);
            this.txtShift.Name = "txtShift";
            this.txtShift.Size = new System.Drawing.Size(43, 21);
            this.txtShift.TabIndex = 16;
            this.txtShift.TextChanged += new System.EventHandler(this.txtShift_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(91, 211);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Shift:";
            // 
            // cboDebug
            // 
            this.cboDebug.FormattingEnabled = true;
            this.cboDebug.Items.AddRange(new object[] {
            "true",
            "false"});
            this.cboDebug.Location = new System.Drawing.Point(135, 52);
            this.cboDebug.Name = "cboDebug";
            this.cboDebug.Size = new System.Drawing.Size(64, 21);
            this.cboDebug.TabIndex = 14;
            this.cboDebug.SelectedIndexChanged += new System.EventHandler(this.cboDebug_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(80, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Debug:";
            // 
            // btnDocFolder
            // 
            this.btnDocFolder.Location = new System.Drawing.Point(312, 153);
            this.btnDocFolder.Name = "btnDocFolder";
            this.btnDocFolder.Size = new System.Drawing.Size(75, 23);
            this.btnDocFolder.TabIndex = 12;
            this.btnDocFolder.Text = "Browse";
            this.btnDocFolder.UseVisualStyleBackColor = true;
            // 
            // txtDocFolder
            // 
            this.txtDocFolder.Location = new System.Drawing.Point(135, 153);
            this.txtDocFolder.Name = "txtDocFolder";
            this.txtDocFolder.Size = new System.Drawing.Size(171, 21);
            this.txtDocFolder.TabIndex = 11;
            this.txtDocFolder.TextChanged += new System.EventHandler(this.txtDocFolder_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(56, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Doc Folder:";
            // 
            // cboAutoUpdate
            // 
            this.cboAutoUpdate.FormattingEnabled = true;
            this.cboAutoUpdate.Items.AddRange(new object[] {
            "true",
            "false"});
            this.cboAutoUpdate.Location = new System.Drawing.Point(135, 181);
            this.cboAutoUpdate.Name = "cboAutoUpdate";
            this.cboAutoUpdate.Size = new System.Drawing.Size(64, 21);
            this.cboAutoUpdate.TabIndex = 9;
            this.cboAutoUpdate.SelectedIndexChanged += new System.EventHandler(this.cboAutoUpdate_SelectedIndexChanged);
            // 
            // lblAutoUpdate
            // 
            this.lblAutoUpdate.AutoSize = true;
            this.lblAutoUpdate.Location = new System.Drawing.Point(47, 184);
            this.lblAutoUpdate.Name = "lblAutoUpdate";
            this.lblAutoUpdate.Size = new System.Drawing.Size(82, 13);
            this.lblAutoUpdate.TabIndex = 8;
            this.lblAutoUpdate.Text = "Auto Update:";
            // 
            // cboUseHttps
            // 
            this.cboUseHttps.FormattingEnabled = true;
            this.cboUseHttps.Items.AddRange(new object[] {
            "true",
            "false"});
            this.cboUseHttps.Location = new System.Drawing.Point(135, 79);
            this.cboUseHttps.Name = "cboUseHttps";
            this.cboUseHttps.Size = new System.Drawing.Size(64, 21);
            this.cboUseHttps.TabIndex = 7;
            this.cboUseHttps.SelectedIndexChanged += new System.EventHandler(this.cboUseHttps_SelectedIndexChanged);
            // 
            // lblUseHttps
            // 
            this.lblUseHttps.AutoSize = true;
            this.lblUseHttps.Location = new System.Drawing.Point(50, 82);
            this.lblUseHttps.Name = "lblUseHttps";
            this.lblUseHttps.Size = new System.Drawing.Size(79, 13);
            this.lblUseHttps.TabIndex = 6;
            this.lblUseHttps.Text = "Secure Http:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(135, 126);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(171, 21);
            this.txtServer.TabIndex = 5;
            this.txtServer.TextChanged += new System.EventHandler(this.txtServer_TextChanged);
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(78, 129);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(51, 13);
            this.lblServer.TabIndex = 4;
            this.lblServer.Text = "Server:";
            // 
            // lblWarning
            // 
            this.lblWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWarning.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWarning.Location = new System.Drawing.Point(6, 17);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(379, 32);
            this.lblWarning.TabIndex = 0;
            this.lblWarning.Text = "Do NOT change the folowing settings unless instructed to do so by Elements suppor" +
                "t staff. ";
            this.lblWarning.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 17);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(386, 273);
            this.shapeContainer1.TabIndex = 21;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.SelectionColor = System.Drawing.SystemColors.ControlLight;
            this.lineShape1.X1 = 20;
            this.lineShape1.X2 = 361;
            this.lineShape1.Y1 = 94;
            this.lineShape1.Y2 = 94;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 373);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cboFormType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtClientId);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration Settings";
            this.Load += new System.EventHandler(this.frmConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

				#endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClientId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboFormType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.ComboBox cboUseHttps;
        private System.Windows.Forms.Label lblUseHttps;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.ComboBox cboAutoUpdate;
        private System.Windows.Forms.Label lblAutoUpdate;
        private System.Windows.Forms.TextBox txtThrInc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtShift;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboDebug;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnDocFolder;
        private System.Windows.Forms.TextBox txtDocFolder;
        private System.Windows.Forms.Label label3;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
    }
#endif
}