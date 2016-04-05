using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace ElementsPPS
{
#if false
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void frmConfig_Load(object sender, EventArgs e)
        {
            //Set up the initial Vals From Settings
            txtClientId.Text = Elements.Default.clientID;
            cboFormType.SelectedText = Elements.Default.formType;
            txtServer.Text = Elements.Default.server;
            cboUseHttps.SelectedText = Elements.Default.secureHttp.ToString();
            txtDocFolder.Text = Elements.Default.docFolder;
            cboAutoUpdate.SelectedText = Elements.Default.autoUpdate.ToString();
            cboDebug.SelectedText = Elements.Default.debugMode.ToString();
            txtShift.Text = Elements.Default.cornerShift.ToString();
            txtSearch.Text = Elements.Default.cornerSearch.ToString();
            txtThrInc.Text = Elements.Default.thresholdIncrease.ToString();
        }

        private void txtClientId_TextChanged(object sender, EventArgs e)
        {
            //Now Check that it'a a valid ClientID
            //  If it is, set it as the new ClientId, Otherwise set it back to what it was
            if (true)
            {
                Elements.Default.clientID = txtClientId.Text;
            }
            else
            {
                txtClientId.Text = Elements.Default.clientID;
            }
        }

        private void cboFormType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Elements.Default.formType = cboFormType.SelectedText;
        }

        private void cboDebug_SelectedIndexChanged(object sender, EventArgs e)
        {
            Elements.Default.debugMode = Convert.ToBoolean(cboDebug.SelectedText);
        }

        private void txtServer_TextChanged(object sender, EventArgs e)
        {
            Elements.Default.server = txtServer.Text;
        }

        private void cboUseHttps_SelectedIndexChanged(object sender, EventArgs e)
        {
            Elements.Default.secureHttp = Convert.ToBoolean(cboUseHttps.SelectedText);
        }

        private void txtDocFolder_TextChanged(object sender, EventArgs e)
        {
            Elements.Default.docFolder = txtDocFolder.Text;
        }

        private void cboAutoUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Elements.Default.autoUpdate = Convert.ToBoolean(cboAutoUpdate.SelectedText);
        }

        private void txtShift_TextChanged(object sender, EventArgs e)
        {
            Elements.Default.cornerShift = Convert.ToInt32(txtShift.Text);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Elements.Default.cornerSearch = Convert.ToInt32(txtSearch.Text);
        }

        private void txtThrInc_TextChanged(object sender, EventArgs e)
        {
            Elements.Default.thresholdIncrease = Convert.ToInt32(txtThrInc.Text);
        }

    }
#endif
}
