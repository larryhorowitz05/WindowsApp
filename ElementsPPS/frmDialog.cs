using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementsPPS
{
    public partial class frmDialog : Form
    {
        private string errorMessage = "There may be some error while uploading the result. Please Contact to Helpdesk.";
        public string ErrorMessage
        {
            get {return errorMessage;}
            set { errorMessage = value; }
        }

        public frmDialog()
        {
            InitializeComponent();
        }

        private void frmDialog_Load(object sender, EventArgs e)
        {
            txtMessage.Text = ErrorMessage;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
