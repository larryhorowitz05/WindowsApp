using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementsPPS
{
    public partial class frmDialogPreview : Form
    {
        public frmDialogPreview()
        {
            InitializeComponent();
        }

        public string FileName
        {
            get;
            set;
        }

        public Bitmap BitmapImage
        {
            get;
            set;
        }

        private void frmDialogPreview_Load(object sender, EventArgs e)
        {

            pctPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            //pctPreview.ImageLocation = FileName;
            pctPreview.Image = BitmapImage;
           
        }
    }
}
