using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Configuration;

namespace ElementsPPS
{
    public partial class frmConnect : Form
    {
        private commonUtil appCommonUtil;
        private Boolean exitApp = true;

        public frmConnect()
        {
            appCommonUtil = new commonUtil();
            InitializeComponent();
        }

        public frmConnect(commonUtil currCommonUtil)
        {
            appCommonUtil = currCommonUtil;
            InitializeComponent();
        }

        private void frmConnect_Load(object sender, EventArgs e)
        {
            pbImage.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void frmConnect_Shown(object sender, EventArgs e)
        {
            try
            {
                //clean up old files
                deleteOldFiles();

                //check for valid client ID
                if (appCommonUtil.clientID == "")
                {
                    String newClientID = "";
                    String newInstallID = "";

                    while (newClientID == "")
                    {
                        newClientID = VersaITInputBox.ShowInputBox("Please enter your ClientID", "Client ID");
                        if (newClientID == "CanceledByUser")
                            throw new Exception("CanceledByUser");
                        if (newClientID == "")
                            MessageBox.Show("ClientID cannot be left blank");
                    }

                    //Verify the client ID is valid
                    try
                    {
                        string protocolName = appCommonUtil.useHttps ? "https://" : "http://";
                        string url = protocolName + appCommonUtil.server + "/" + newClientID + "/TGLogin.aspx";
                        webGet(url);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                    //set installID
                    newInstallID = DateTime.Now.ToString("ddMMyyyyhhmmssff");

                    //If all went according to plan then update the commonUtil Object
                    appCommonUtil.clientID = newClientID;
                    appCommonUtil.installID = newInstallID;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "CanceledByUser")
                {
                    Application.Exit();
                }
                else if (ex.Message == "The remote server returned an error: (404) Not Found.")
                {
                    MessageBox.Show("The Client ID entered is not valid.");
                    Application.Exit();
                }
                else if (ex.Message == "Secure Link Failure")
                {
                    MessageBox.Show("Unable to connect to Elements.");
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show(ex.Message);
                    Application.Exit();
                }
            }
        }

        private void frmConnect_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (exitApp) { Application.Exit(); }
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            SetControlsProp(false);

            Byte[] sendData;
            ASCIIEncoding encodedData = new ASCIIEncoding();
            appCommonUtil.currUserId = "0";

            try
            {
                if (txtUid.Text.Trim() == "" || txtPass.Text.Trim() == "")
                {
                    throw new Exception("The User ID and Password cannot be blank.");
                }

                try
                {
                    string protocolName = appCommonUtil.useHttps ? "https://" : "http://";
                    string url = protocolName + appCommonUtil.server + "/" + appCommonUtil.clientID + "/TempScan/CTExml.aspx?x=RenLe@rnP@ds1";
                    sendData = encodedData.GetBytes("u=" + txtUid.Text.Trim() + "&p=" + txtPass.Text.Trim());
                    appCommonUtil.currUserId = webPost(url, sendData);

                    //if (txtPass.Text.Trim() != "Educ@te1!")
                    //{
                    //    MessageBox.Show("This is not a valid user name or password.\nPlease contact system administrator!", "Alert");
                    //    SetControlsProp();
                    //}
                    //else
                    //{
                    //    VersaITCommon commonClass = new VersaITCommon(appCommonUtil);
                    //    string userName = txtUid.Text.Trim();
                    //    string message = commonClass.ValidateUser(userName);

                    //    if (!string.IsNullOrEmpty(message))
                    //    {
                    //        MessageBox.Show(message, "Alert");
                    //        SetControlsProp();
                    //    }
                    //    else
                    //    {
                    //        //ok the user has been validated and everything has been set up correctly
                    //        //close this connection form (w/o exiting the app) and open the main form
                    //        PPS main = new PPS(appCommonUtil);
                    //        main.Show();

                    //        exitApp = false;
                    //        this.Hide();
                    //        this.Close();
                    //        this.Dispose();
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                if (appCommonUtil.currUserId == "0")
                    throw new Exception("This User ID and Password combination is not valid.");

                //ok the user has been validated and everything has been set up correctly
                //close this connection form (w/o exiting the app) and open the main form
                PPS main = new PPS(appCommonUtil);
                main.Show();

                exitApp = false;
                this.Hide();
                this.Close();
                this.Dispose();
            }
            catch (Exception ex)
            {
                if (ex.Message == "The remote server returned an error: (404) Not Found." || ex.Message == "Secure Link Failure")
                    MessageBox.Show("Unable to connect to Elements.");
                else
                    MessageBox.Show(ex.Message);

                txtUid.Text = txtPass.Text = "";
                SetControlsProp();
            }
        }

        private void SetControlsProp(bool setEnable = true)
        {
            txtUid.ReadOnly = txtPass.ReadOnly = setEnable ? false : true;
            lblBanner.Text = setEnable ? "" : "Connecting ...";
            Cursor = setEnable ? Cursors.Default : Cursors.WaitCursor;
        }

        private void deleteOldFiles()
        {
            try
            {
                foreach (String currFileName in Directory.GetFiles(appCommonUtil.docFolder, "*.csv"))
                {
                    FileInfo currFileInfo = new FileInfo(currFileName);
                    if (currFileInfo.LastWriteTime.AddDays(appCommonUtil.daysToExpireCsv) < DateTime.Now)
                    {
                        File.Delete(currFileName);
                    }
                }
            }
            catch (Exception) {/*here we just want to supress errors as it doesn't really matter if this works*/}
        }

        private String webPost(String url, Byte[] sendData)
        {
            WebRequest wrq = WebRequest.Create(url);
            wrq.Method = "POST";
            wrq.ContentType = "application/x-www-form-urlencoded";
            wrq.ContentLength = sendData.Length;
            Stream sendStream = wrq.GetRequestStream();
            sendStream.Write(sendData, 0, sendData.Length);
            sendStream.Close();
            HttpWebResponse wrp = (HttpWebResponse)wrq.GetResponse();
            StreamReader sr = new StreamReader(wrp.GetResponseStream());
            String Text = sr.ReadToEnd();
            sr.Close();
            wrp.Close();

            return Text;
        }

        private String webGet(String url)
        {
            WebRequest wrq = WebRequest.Create(url);
            HttpWebResponse wrp = (HttpWebResponse)wrq.GetResponse();
            StreamReader sr = new StreamReader(wrp.GetResponseStream());
            String Text = sr.ReadToEnd();
            sr.Close();
            wrp.Close();

            return Text;
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            switch (keyData)
            {
                //Change Client Id
                case (Keys.Control | Keys.Shift | Keys.F8):
                    String newClientID = VersaITInputBox.ShowInputBox("Please enter your ClientID", "Client ID", appCommonUtil.clientID);
                    if (newClientID != "CanceledByUser" && newClientID != "")
                        appCommonUtil.clientID = newClientID;
                    return true;
                //Update the Server
                case (Keys.Control | Keys.Shift | Keys.F9):
                    String newServer = VersaITInputBox.ShowInputBox("Please enter the web server address", "Web Server", appCommonUtil.server);
                    if (newServer != "CanceledByUser" && newServer != "")
                    {
                        if (newServer.Contains("http")) newServer = newServer.Replace("http://", "").Replace("https://", "");
                        appCommonUtil.server = newServer;
                    }
                    return true;
                //Turn on/off debug mode
                case (Keys.Control | Keys.Shift | Keys.F11):
                    if (appCommonUtil.debug == false)
                    {
                        appCommonUtil.debug = true;
                        MessageBox.Show("Debug mode is now on");
                    }
                    else
                    {
                        appCommonUtil.debug = false;
                        MessageBox.Show("Debug mode is now off");
                    }
                    return true;
                //Change Operating mode
                case (Keys.Control | Keys.Shift | Keys.F12):
                    if (appCommonUtil.formType == "PlainPaper")
                    {
                        appCommonUtil.formType = "HALO";
                        MessageBox.Show("The form type has been changed to: HALO");
                    }
                    else
                    {
                        appCommonUtil.formType = "PlainPaper";
                        MessageBox.Show("The form type has been changed to: Plain Paper");
                    }
                    return true;
                //Change Security mode
                case (Keys.Control | Keys.Shift | Keys.S):
                    if (appCommonUtil.useHttps == false)
                    {
                        appCommonUtil.useHttps = true;
                        MessageBox.Show("The connection protocol security has been changed to: secure");
                    }
                    else
                    {
                        appCommonUtil.useHttps = false;
                        MessageBox.Show("The connection protocol security has been changed to: non-secure");
                    }
                    return true;
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}