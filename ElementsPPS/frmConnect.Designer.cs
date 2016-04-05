namespace ElementsPPS
{
    partial class frmConnect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConnect));
            this.lblBanner = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.txtUid = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.lblUid = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pbImage = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).BeginInit();
            this.SuspendLayout();
            // 
            // lblBanner
            // 
            this.lblBanner.AutoSize = true;
            this.lblBanner.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBanner.Location = new System.Drawing.Point(15, 11);
            this.lblBanner.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBanner.Name = "lblBanner";
            this.lblBanner.Size = new System.Drawing.Size(0, 17);
            this.lblBanner.TabIndex = 14;
            this.lblBanner.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnLogin);
            this.groupBox1.Controls.Add(this.txtPass);
            this.groupBox1.Controls.Add(this.txtUid);
            this.groupBox1.Controls.Add(this.Label1);
            this.groupBox1.Controls.Add(this.lblUid);
            this.groupBox1.Location = new System.Drawing.Point(9, 369);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(489, 127);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(171)))), ((int)(((byte)(238)))));
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(372, 88);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 28);
            this.btnLogin.TabIndex = 18;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPass
            // 
            this.txtPass.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPass.Location = new System.Drawing.Point(129, 51);
            this.txtPass.Margin = new System.Windows.Forms.Padding(4);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(345, 24);
            this.txtPass.TabIndex = 17;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // txtUid
            // 
            this.txtUid.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUid.Location = new System.Drawing.Point(129, 18);
            this.txtUid.Margin = new System.Windows.Forms.Padding(4);
            this.txtUid.Name = "txtUid";
            this.txtUid.Size = new System.Drawing.Size(345, 24);
            this.txtUid.TabIndex = 16;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(7, 52);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(82, 21);
            this.Label1.TabIndex = 15;
            this.Label1.Text = "Password:";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUid
            // 
            this.lblUid.AutoSize = true;
            this.lblUid.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUid.Location = new System.Drawing.Point(6, 19);
            this.lblUid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUid.Name = "lblUid";
            this.lblUid.Size = new System.Drawing.Size(65, 21);
            this.lblUid.TabIndex = 14;
            this.lblUid.Text = "User ID:";
            this.lblUid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblBanner);
            this.groupBox2.Controls.Add(this.pbImage);
            this.groupBox2.Location = new System.Drawing.Point(9, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(489, 375);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // pbImage
            // 
            this.pbImage.Image = ((System.Drawing.Image)(resources.GetObject("pbImage.Image")));
            this.pbImage.Location = new System.Drawing.Point(17, 33);
            this.pbImage.Margin = new System.Windows.Forms.Padding(4);
            this.pbImage.Name = "pbImage";
            this.pbImage.Size = new System.Drawing.Size(456, 332);
            this.pbImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbImage.TabIndex = 15;
            this.pbImage.TabStop = false;
            // 
            // frmConnect
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(505, 506);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(523, 553);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(523, 553);
            this.Name = "frmConnect";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmConnect_FormClosed);
            this.Load += new System.EventHandler(this.frmConnect_Load);
            this.Shown += new System.EventHandler(this.frmConnect_Shown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label lblBanner;
        private System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.Button btnLogin;
        internal System.Windows.Forms.TextBox txtPass;
        internal System.Windows.Forms.TextBox txtUid;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label lblUid;
        private System.Windows.Forms.GroupBox groupBox2;
        internal System.Windows.Forms.PictureBox pbImage;
    }
}