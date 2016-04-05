using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace ElementsPPS
{
    public partial class frmImport : Form
    {
        private commonUtil appCommonUtil;
        bool isImportError = false;

        public string jobID { get; set; }

        public frmImport()
        {
            appCommonUtil = new commonUtil();
            InitializeComponent();
        }

        public frmImport(commonUtil currCommonUtil)
        {
            appCommonUtil = currCommonUtil;
            InitializeComponent();
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            BindResults();
        }

        private void BindResults()
        {
            try
            {
                string protocolName = appCommonUtil.useHttps ? "https://" : "http://";
                string webAPI = ConfigurationManager.AppSettings["WebAPI"];
                string webAddress = protocolName + appCommonUtil.server + "/" + webAPI + "/api/Common/GetTestResponseByJobId?jobIds=" + jobID;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string result = client.DownloadString(webAddress);
                    List<TestResponse> testResponses = (Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestResponse>>(result));

                    if (testResponses.Count > 0)
                    {
                        for (int i = 0; i < testResponses.Count; i++)
                        {
                            String seqNum = (i + 1).ToString();
                            String studentName = testResponses[i].StudentName;
                            String studentId = testResponses[i].StudentID;
                            String testName = testResponses[i].TestName;
                            String testId = testResponses[i].TestID;
                            String isSuccess = testResponses[i].isSuccess;
                            String error = testResponses[i].ErrorCode;

                            addResult(seqNum, studentName, studentId, testName, testId, isSuccess, error);

                            isImportError = isSuccess == "Fail" ? true : isImportError;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void addResult(String seqNum, String studentName, String studentId, String testName, String testId, String isSuccess, String error)
        {
            dgvResults.Rows.Add(seqNum, studentName, studentId, testName, testId, isSuccess, error);
        }

        private void frmImport_Shown(object sender, EventArgs e)
        {
            if (isImportError)
            {
                frmDialog errorMessage = new frmDialog();
                errorMessage.ShowDialog();
            }
        }
    }

    public class TestResponse
    {
        public int seqNum { get; set; }
        public string StudentName { get; set; }
        public string StudentID { get; set; }
        public string TestName { get; set; }
        public string TestID { get; set; }
        public string isSuccess { get; set; }
        public string ErrorCode { get; set; }
    }
}
