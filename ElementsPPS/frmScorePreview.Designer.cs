namespace ElementsPPS
{
    partial class frmScorePreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScorePreview));
            this.grdScorePreview = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdScorePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // grdScorePreview
            // 
            this.grdScorePreview.BackgroundColor = System.Drawing.Color.White;
            this.grdScorePreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdScorePreview.EnableHeadersVisualStyles = false;
            this.grdScorePreview.Location = new System.Drawing.Point(0, 0);
            this.grdScorePreview.Name = "grdScorePreview";
            this.grdScorePreview.RowTemplate.Height = 24;
            this.grdScorePreview.Size = new System.Drawing.Size(381, 554);
            this.grdScorePreview.TabIndex = 0;
            // 
            // frmScorePreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 556);
            this.Controls.Add(this.grdScorePreview);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScorePreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Student\'s Answer";
            this.Load += new System.EventHandler(this.frmScorePreview_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdScorePreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdScorePreview;
    }
}