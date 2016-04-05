using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementsPPS
{
    public partial class frmUpload : Form
    {
        private Boolean _hasRows = false;

        public frmUpload()
        {
            InitializeComponent();
        }

        public Boolean hasRows
        {
            get { return _hasRows; }
        }

        public void addResult(Boolean debug, String seqNum, String FormID, String StudentID)
        {
            addResult(debug, seqNum, FormID, StudentID, "", "");
        }

        public void addResult(Boolean debug, String seqNum, String FormID, String StudentID, String TestForm, String MCQuestions)
        {
            dgvResults.Rows.Add(seqNum, FormID, StudentID, TestForm, MCQuestions);
            _hasRows = true;
            if (debug)
            {
                this.Width = 900;
                dgvResults.Columns["MCQuestions"].Visible = true;
            }
            else
            {
                this.Width = 490;
                dgvResults.Columns["MCQuestions"].Visible = false;
            }
        }
    }
}