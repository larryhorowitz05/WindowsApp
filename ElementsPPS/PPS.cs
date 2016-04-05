using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace ElementsPPS
{
    public partial class PPS : Form
    {
        private commonUtil appCommonUtil;
        private Boolean currDebugMode = false;
        private Boolean _hasRows = false;
        private StringBuilder dataFile = null;
        private Boolean someCardScanned = false;

        private string currFolderName = string.Empty;
        private string appDataPath = string.Empty;

        private List<string> studentIds = new List<string>();

        int totalQuestionsInAssessment = 0;

        private Boolean isOuterChecked = false;

        private Thread cThread;

        const int THUMBNAIL_COLINDEX = 7;
        const int FILEPATH_COLINDEX = 8;
        const int LINK_COLINDEX = 9;
        const int ERROR_COLINDEX = 10;

        DataGridViewLinkColumn linkColumn;// = new DataGridViewLinkColumn();
        DataGridViewImageColumn imageColumn;// = new DataGridViewImageColumn();

        public PPS()
        {
            appCommonUtil = new commonUtil();
            InitializeComponent();
        }

        public PPS(commonUtil currCommonUtil)
        {
            appCommonUtil = currCommonUtil;
            InitializeComponent();
        }

        private void CollapseSection()
        {
            pnlCollapse.Visible = false;
            pnlExpand.Visible = true;

            pctSmall.Visible = true;
            pctCollapse.Visible = false;
            pctExpand.Visible = true;

            pnlExpand.Top -= 115;
            txtOutput.Top -= 115;
            txtOutput.Height += 115;
            dgvResults.Top -= 115;
            dgvResults.Height += 115;
        }

        private void ExpandSection()
        {
            pnlCollapse.Visible = true;
            pnlExpand.Visible = true;

            pctSmall.Visible = false;
            pctCollapse.Visible = true;
            pctExpand.Visible = false;

            pnlExpand.Top += 115;
            txtOutput.Top += 115;
            txtOutput.Height -= 115;
            dgvResults.Top += 115;
            dgvResults.Height -= 115;
        }

        private void pctCollapse_Click(object sender, EventArgs e)
        {
            CollapseSection();
        }

        private void pctExpand_Click(object sender, EventArgs e)
        {
            ExpandSection();
        }

        private void btnScanner_Click(object sender, EventArgs e)
        {
            try
            {
                if (axImg.TwainSelectSource())
                {
                    appCommonUtil.defaultScanner = axImg.TwainGetDefaultSourceName();
                    txtScanner.Text = appCommonUtil.defaultScanner;

                    //set the default scanner value
                    saveAppConfig("defaultScanner", appCommonUtil.defaultScanner);
                }
                else
                {
                    //appCommonUtil.defaultScanner = "";
                    //txtScanner.Text = appCommonUtil.defaultScanner;
                }
            }
            catch(Exception ex)
            {
               
            }
        }

        private void btnAppServer_Click(object sender, EventArgs e)
        {
            String appServer = VersaITInputBox.ShowInputBox("Please Enter Appication Server URL", "");

            if (appServer == "CanceledByUser")
                return;
            else if (appServer == "")
                MessageBox.Show("Appication Server URL cannot be left blank");
            else
            {
                String connectResponse = string.Empty;
                string protocolName = appCommonUtil.useHttps ? "https://" : "http://";
                string webAPI = ConfigurationManager.AppSettings["WebAPI"];
                string webAddress = protocolName + appCommonUtil.server + "/" + webAPI;

                try
                {
                    connectResponse = webGet(webAddress);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                appCommonUtil.server = appServer;
                txtAppServer.Text = appCommonUtil.server;

                //set the application server address
                saveAppConfig("serverAddress", appCommonUtil.server);
            }
        }

        public void saveAppConfig(string setting, string value)
        {
            appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + Application.CompanyName + "\\" + Application.ProductName;

            XmlDocument configDoc = new XmlDocument();

            configDoc.Load(appDataPath + "\\elementsppsconfig.xml");
            configDoc.SelectSingleNode("config/" + setting).Attributes["val"].Value = value;
            configDoc.Save(appDataPath + "\\elementsppsconfig.xml");
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PPS_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void PPS_Load(object sender, EventArgs e)
        {
            pctSmall.Visible = false;
            pctExpand.Visible = false;
            dgvResults.Visible = false;
            txtOutput.Visible = true;
            //PPSSkin.SkinFile = Application.StartupPath + @"\skins\diamondblue.ssk";// ' In order to initialize the Thems 
            btnClose.ForeColor = Color.White;
            btnSubmit.ForeColor = Color.White;

            try
            {
                txtScanner.Text = appCommonUtil.defaultScanner;
                txtAppServer.Text = appCommonUtil.server;

                DirectoryInfo dr = new DirectoryInfo(appCommonUtil.docFolder + "\\images");
                foreach (DirectoryInfo drChild in dr.GetDirectories())
                {
                    drChild.Delete(true);
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void RemoveGridColumn(string colName)
        {
            if (dgvResults.Columns.Contains(colName))
                dgvResults.Columns.RemoveAt(dgvResults.Columns[colName].Index);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            if(txtScanner.Text== string.Empty)
            {
                MessageBox.Show("Please select the Scanner before Scanning!");
                return;
            }

            lblCounter.Visible = true;
            lblCounter.Text = "";
            Boolean isError = false;
            dgvResults.Visible = false;
            dgvResults.Rows.Clear();

            if (!dgvResults.Columns.Contains("chkAll"))
                AddCheckBoxColumn();

            if (dgvResults.Columns.Contains("chkAll"))
                dgvResults.Columns.RemoveAt(dgvResults.Columns["chkAll"].Index);

            //RemoveGridColumn("chkAll");
            RemoveGridColumn("Thumnail");
            RemoveGridColumn("ImagePath");
            RemoveGridColumn("HyperLink");
            RemoveGridColumn("ErrorMessage"); 

            currFolderName = string.Empty;

            pctProgress.Show();

            btnClose.Enabled = false;
            btnSubmit.Enabled = false;
            btnClose.ForeColor = Color.White;
            btnSubmit.ForeColor = Color.White;
            btnScan.Enabled = false;
            grpImageType.Enabled = false;
            btnScanner.Enabled = false;

            String fileName = string.Empty; ;
            Int64 nImageID;
            Int32 nImageCount = 0;
            dataFile = new StringBuilder();
            StringBuilder dataFileDisplay = new StringBuilder();
            String imagesPath = appCommonUtil.docFolder + "\\images";

            //create the upload display form
            //frmUpload uploadForm = new frmUpload();

            //These are used when uploading the datafile
            //WebClient Client = new WebClient();
            //byte[] responseArray;

#if !RETRYTEST
            try
            {
                //if (someCardScanned)
                //{
                //    DialogResult response = MessageBox.Show(null, "Previous scanned card will be lost without submit if you click again Scan button! Are you sure", "Re-Scanning confirmation", MessageBoxButtons.YesNo);
                //    if (response.ToString() == "No")
                //    {                       
                //        pctProgress.Hide();
                //        return;
                //    }
                //}

                // If this is the first execution of the program, the images directory does not exist,
                // create it.
                if (!Directory.Exists(imagesPath))
                    Directory.CreateDirectory(imagesPath);


                if (currFolderName == string.Empty)
                {
                    currFolderName = "\\img_" + DateTime.Now.ToString().Replace(" ", "").Replace("/", "").Replace(":", "");
                    imagesPath += currFolderName;
                    if (!Directory.Exists(imagesPath))
                        Directory.CreateDirectory(imagesPath);
                }

                //delete old upload files???
                //delete old images
                foreach (String currFileName in Directory.GetFiles(imagesPath, "*.bmp"))
                {
                    FileInfo currentFile = new FileInfo(currFileName);
                    currentFile.Delete();
                    //File.Delete(currFileName);
                }

                //delete old template images
                foreach (String currFileName in Directory.GetFiles(imagesPath, "*.jpg"))
                {
                    File.Delete(currFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("One scanned job is already in queue. Please discard or submit this job first, then you can go for Re-Scan.");
                //MessageBox.Show("Unable to find required system folder or perform required function. Please reinstall the software.");
                btnClose.Enabled = true;
                btnSubmit.Enabled = true;
                btnScan.Enabled = true;
                grpImageType.Enabled = true;
                btnScanner.Enabled = true;
                //this.Close();
                pctProgress.Hide();
                return;
            }

            axImg.SetLicenseNumber("1519667525273646862381256");
            templateImg.SetLicenseNumber("1519667525273646862381256");

            //Acquire and score the images
            if (chkUseFiles.Checked == false)
            {

                // Get Images from the scanner and process them
                if ((String.IsNullOrEmpty(appCommonUtil.defaultScanner) && axImg.TwainOpenDefaultSource()) ||
                                        axImg.TwainOpenSource(appCommonUtil.defaultScanner))
                {
                    //set upt the twain component
                    axImg.TwainSetHideUI(true);
                    axImg.TwainSetIndicators(false);
                    axImg.TwainSetAutoBrightness(true);
                    axImg.TwainSetCurrentContrast(0);
                    axImg.TwainEnableDuplex(false);
                    axImg.TwainSetAutomaticDeskew(true);

                    axImg.TwainSetAutoFeed(true); //Set AutoFeed Enabled
                    axImg.TwainSetAutoScan(true); //To achieve the maximum scanning rate

                    axImg.TwainSetCurrentResolution(200);
                    axImg.TwainSetCurrentPixelType(GdPicturePro5.TwainPixelType.TWPT_BW); //BW
                    axImg.TwainSetCurrentBitDepth(1); // 1 bpp

                    string src = axImg.TwainGetCurrentSourceName();
                    src = src.ToUpper();
                    // Specific setup for insight 20 and 30.
                    if (src.Contains("INSIGHT 20") || src.Contains("INSIGHT 30"))
                    {
                        Boolean b = axImg.TwainSetCapCurrentString(GdPicturePro5.TwainCapabilities.ICAP_HALFTONES, GdPicturePro5.TwainItemTypes.TWTY_STR32, "Fixed Thresholding");
                        b = axImg.TwainSetCapCurrentNumeric(GdPicturePro5.TwainCapabilities.ICAP_THRESHOLD, GdPicturePro5.TwainItemTypes.TWTY_INT32, 90);
                    }
                    else if (src.Contains("HP") && src.Contains("7000"))
                    {
                        Boolean b = axImg.TwainSetCurrentContrast(800);
                    }

                    // We can override brightness and contrast if there are entries in the configuration xml file.
                    if (appCommonUtil.brightness.HasValue)
                    {
                        axImg.TwainSetAutoBrightness(false);
                        axImg.TwainSetCurrentBrightness(appCommonUtil.brightness.Value);
                    }

                    if (appCommonUtil.contrast.HasValue)
                    {
                        axImg.TwainSetCurrentContrast(appCommonUtil.contrast.Value);
                    }

                    nImageID = axImg.CreateImageFromTwain();

                    //scan all of the forms in the scanner
                    while (nImageID != 0)
                    {
                        try
                        {

                            nImageCount++;
                            lblCounter.Text = nImageCount.ToString();
                            //writeToList("Scoring item number : " + nImageCount);

                       //     nImageID = axImg.GetNativeImage();

                            //set up scorecardreader and read the form
                            VersaITScoreReader vr;
                            vr = new VersaITScoreReader(appCommonUtil, axImg);
                            if (chkImageSave.Checked && radioActual.Checked)
                            {
                                fileName = imagesPath + "\\PPScanImages_" + nImageCount + ".bmp";
                                axImg.SaveAsBmp(fileName);
                            }
                            if (chkImageSave.Checked && radioTemplate.Checked)
                            {
                                fileName = imagesPath + "\\PPScanImages_" + nImageCount + ".jpg";
                                axImg.SaveAsJPEG(fileName);

                                templateImg.CreateImageFromFile(fileName);
                                templateImg.ConvertTo32BppARGB();
                                vr.TemplateImageObject = templateImg;
                            }
                            string errorMessage = vr.readForm(fileName);

                            someCardScanned = true;

                            //add score to file
                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                dataFile.Append(buildFileString(vr.formID, vr.studentID, vr.testForm, vr.miscData, vr.MCQuestions));
                                addResult(appCommonUtil.debug, nImageCount.ToString(), Int32.Parse(vr.formID), vr.studentID, vr.testForm, vr.MCQuestions);
                                if (appCommonUtil.debug)
                                {
                                    writeToList(vr.debugOutput);
                                    writeToList(nImageCount + "," + vr.formID + "," + vr.studentID + "," + vr.testForm + "," + vr.MCQuestions + Environment.NewLine);
                                }
                            }
                            else
                            {
                                dataFile.Append(buildFileString("-1", "", "", "", errorMessage));
                                addResult(appCommonUtil.debug, nImageCount.ToString(), -1, "", "", errorMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                            someCardScanned = false;
                            MessageBox.Show("Error Scoring Item Number " + nImageCount + Environment.NewLine + ex.Message);
                        }

                        axImg.ReleaseGdPictureImageDC(Convert.ToInt32(nImageID));
                        axImg.CloseImage(Convert.ToInt32(nImageID));

                        axImg.CloseNativeImage();
                        axImg.RemoveAllImage();

                        nImageID = axImg.CreateImageFromTwain();
                    }
                    axImg.RemoveAllImage();
                    axImg.TwainCloseSource();
                    axImg.TwainCloseSourceManager();
                }
                else
                {
                    isError = true;
                    throw new Exception("An error has occurred. Make sure the scanner is turned on and installed. ErrorCode: " + axImg.TwainGetState());
                }
            }
            else
            {
                //Get images from files and process them
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Select Image Folder";
                if (Directory.Exists(appCommonUtil.templatesFolder + "\\images")) { fd.SelectedPath = appCommonUtil.templatesFolder + "\\images"; }
                fd.ShowNewFolderButton = false;

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    foreach (String currFileName in Directory.GetFiles(fd.SelectedPath, "*.bmp"))
                    {
                        try
                        {
                            nImageCount++;
                            writeToList("Scoring item number : " + nImageCount);

                            axImg.CreateImageFromFile(currFileName);
                            fileName = appCommonUtil.docFolder + "\\images\\PPScanImages_" + nImageCount + ".bmp";
                            axImg.SaveAsBmp(fileName);

                            //read the form
                            VersaITScoreReader vr = new VersaITScoreReader(appCommonUtil, axImg);
                            {
                                axImg.SaveAsJPEG(appCommonUtil.docFolder + "\\images\\template.jpg");
                                templateImg.CreateImageFromFile(appCommonUtil.docFolder + "\\images\\template.jpg");
                                templateImg.ConvertTo32BppARGB();
                                vr.TemplateImageObject = templateImg;
                            }
                            vr.readForm("");

                            someCardScanned = true;

                            //add score to file
                            dataFile.Append(buildFileString(vr.formID, vr.studentID, vr.testForm, vr.miscData, vr.MCQuestions));
                            addResult(appCommonUtil.debug, nImageCount.ToString(), Int32.Parse(vr.formID), vr.studentID, vr.testForm, vr.MCQuestions);
                            if (appCommonUtil.debug)
                            {
                                writeToList(vr.debugOutput);
                                writeToList(nImageCount + "," + vr.formID + "," + vr.studentID + "," + vr.testForm + "," + vr.MCQuestions + Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            isError = true;
                            someCardScanned = false;
                            writeToList("Error Scoring Item Number " + nImageCount + Environment.NewLine + ex.Message);
                        }
                    }
                }
            }

            if (File.Exists(imagesPath + "\\images\\template.jpg"))
            {
                File.Delete(imagesPath + "\\images\\template.jpg");
            }
            createGraphicsColumn(imagesPath);

            //createThumnailColumn(imagesPath);
#endif
            if (isError == false || (isError == true && dgvResults.Rows.Count > 0))
            {
                btnClose.Enabled = true;
                btnSubmit.Enabled = true;
                btnScan.Enabled = false;
                grpImageType.Enabled = false;
                btnDiscard.Enabled = true;
                btnScanner.Enabled = true;
            }
            else
            {

                btnClose.Enabled = true;
                btnSubmit.Enabled = false;
                btnScan.Enabled = true;
                grpImageType.Enabled = true;
                btnDiscard.Enabled = false;
                btnScanner.Enabled = true;

            }
            dgvResults.Visible = true;
            pctProgress.Hide();
            lblCounter.Visible = false;
            TotalCheckBoxes = dgvResults.RowCount;
            
        }

        DataGridViewCheckBoxColumn c1;
        CheckBox HeaderCheckBox;
        int TotalCheckBoxes = 0;
        int TotalCheckedCheckBoxes = 0;
        bool IsHeaderCheckBoxClicked = false;
        private void AddCheckBoxColumn()
        {


            HeaderCheckBox = new CheckBox();
            //HeaderCheckBox.Name = "chkHeader";
            //HeaderCheckBox.Text = "";
            //Get the column header cell bounds
            Rectangle rect = this.dgvResults.GetCellDisplayRectangle(0, -1, true);
            rect = new Rectangle(rect.X + 10, rect.Y + 10, rect.Width, rect.Height);
            HeaderCheckBox.Size = new Size(13, 13);
            //Change the location of the CheckBox to make it stay on the header
            HeaderCheckBox.Location = rect.Location;
            HeaderCheckBox.CheckedChanged += new EventHandler(HeaderCheckBox_CheckedChanged);

            //Add the CheckBox into the DataGridView

            this.dgvResults.Controls.Add(HeaderCheckBox);

            TotalCheckedCheckBoxes = 0;
            HeaderCheckBox.Checked = false;

        }
        void HeaderCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (isCellChecked)
            {
                isCellChecked = false;
                return;
            }
            for (int j = 0; j < this.dgvResults.RowCount; j++)
            {
                if (dgvResults.Rows[j].Visible == true)
                {
                    DataGridViewTextBoxCell studentIdCol = new DataGridViewTextBoxCell();
                    studentIdCol = (DataGridViewTextBoxCell)dgvResults.Rows[j].Cells["StudentID"];


                    this.dgvResults[0, j].Value = this.HeaderCheckBox.Checked;
                    if (this.HeaderCheckBox.Checked)
                    {
                        studentIds.Add(studentIdCol.Value.ToString());
                        ++TotalCheckedCheckBoxes;
                    }
                    else
                    {
                        studentIds.Remove(studentIdCol.Value.ToString());
                        --TotalCheckedCheckBoxes;
                    }
                }
            }
            if (TotalCheckedCheckBoxes < 0)
                TotalCheckedCheckBoxes = 0;
            this.dgvResults.EndEdit();
        }

        private void createGraphicsColumn(string imagesPath)
        {

            c1 = new DataGridViewCheckBoxColumn();
            c1.Name = "chkAll";
            c1.HeaderText = "";
            c1.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            c1.Width = 30;
            c1.FlatStyle = FlatStyle.Popup;
            this.dgvResults.Columns.Insert(0, c1);


            if (!dgvResults.Columns.Contains("Thumnail") && chkImageSave.Checked)
            {
                Bitmap bmpImage = null;
                //DataGridViewImageColumn imageColumn = new DataGridViewImageColumn();

                imageColumn = new DataGridViewImageColumn();
                imageColumn.Image = bmpImage;
                imageColumn.Name = "Thumnail";
                //imageColumn.Width = radioHyperlink.Checked == true ? 0 : 140;
                //imageColumn.MinimumWidth = radioHyperlink.Checked == true ? 2 : 100;
                imageColumn.HeaderText = "Thumbnail";

                dgvResults.Columns.Insert(THUMBNAIL_COLINDEX, imageColumn);

                DataGridViewTextBoxColumn hiddenCol = new DataGridViewTextBoxColumn();
                hiddenCol.HeaderText = "FilePath";
                hiddenCol.Name = "ImagePath";
                hiddenCol.Width = 0;
                hiddenCol.Visible = false;

                dgvResults.Columns.Insert(FILEPATH_COLINDEX, hiddenCol);

                //DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
                //imageColumn.Image = bmpImage;
                linkColumn = new DataGridViewLinkColumn();
                linkColumn.LinkColor = Color.Blue;
                linkColumn.ActiveLinkColor = Color.Red;
                linkColumn.VisitedLinkColor = Color.FromArgb(128, 0, 128);
                linkColumn.Name = "HyperLink";
                linkColumn.Width = 120;
                //linkColumn.Width = radioHyperlink.Checked == true ? 140 : 0;
                //linkColumn.MinimumWidth = radioHyperlink.Checked == true ? 100 : 2;
                linkColumn.HeaderText = "Scanned Answers";
                dgvResults.Columns.Insert(LINK_COLINDEX, linkColumn);

                //if (radioThumbnail.Checked)
                //    linkColumn.Visible = false;
                //if (radioHyperlink.Checked)
                //    imageColumn.Visible = false;
                //DataGridViewTextBoxColumn scoreCol = new DataGridViewTextBoxColumn();
                //scoreCol.HeaderText = "Score";
                //scoreCol.Name = "Score";
                //scoreCol.Width = 50;
                //dgvResults.Columns.Insert(7, scoreCol);

                DataGridViewLinkColumn errorCol = new DataGridViewLinkColumn();
                errorCol.LinkColor = Color.Red;
                errorCol.ActiveLinkColor = Color.Red;
                errorCol.VisitedLinkColor = Color.Red;
                errorCol.Name = "ErrorMessage";
                errorCol.HeaderText = "Error";
                errorCol.Width = 120;

                dgvResults.Columns.Insert(ERROR_COLINDEX, errorCol);

                foreach (String currFileName in Directory.GetFiles(imagesPath, radioTemplate.Checked == true ? "*.jpg" : "*.bmp"))
                {
                    string fileName = System.IO.Path.GetFileName(currFileName);
                    string fileExtension = fileName.Substring(fileName.IndexOf("."));
                    string fileIndexNo = fileName.Split('.')[0].Split('_')[1];
                  //  Bitmap  bmp = (Bitmap)Image.FromFile(currFileName, true);
                    // Open a Stream and decode a TIFF image

                   
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(currFileName, FileMode.Open, FileAccess.Read);



                        for (int i = (Convert.ToInt16(fileIndexNo) - 1); i < dgvResults.Rows.Count; i++)
                        {

                            imageColumn.Image = Image.FromStream(fs);
                            imageColumn.ImageLayout = DataGridViewImageCellLayout.Stretch;

                            //dgvDisplayTiles.Rows.Add();
                            dgvResults.Rows[i].Cells[THUMBNAIL_COLINDEX].Value = imageColumn.Image;
                            dgvResults.Rows[i].Height = 100;

                            dgvResults.Rows[i].Cells[FILEPATH_COLINDEX].Value = currFileName;

                            dgvResults.Rows[i].Cells[LINK_COLINDEX].Value = "Scanned Answers";


                            if (dgvResults["FormID", i].Value.ToString() == "-1")
                            {
                                dgvResults.Rows[i].Cells[ERROR_COLINDEX].Value = dgvResults.Rows[i].Cells["MCQuestions"].Value;
                                dgvResults.Rows[i].Cells[ERROR_COLINDEX].Style.ForeColor = Color.Red;
                                dgvResults.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                            }
                            break;
                        }
                    }
                    finally
                    {
                        fs.Close();
                    }
                   
                }
                ShowHideThumnailHyperlinkColumn(radioThumbnail.Checked);
                
            }
        }

        private void ShowHideThumnailHyperlinkColumn(bool showthumnail)
        {

            dgvResults.AutoSizeRowsMode = radioThumbnail.Checked == false ? DataGridViewAutoSizeRowsMode.AllCells : DataGridViewAutoSizeRowsMode.None;

            if (imageColumn != null && linkColumn != null)
            {
                imageColumn.Width = 140;
                linkColumn.Width = 140;
                imageColumn.Visible = showthumnail;
               
                linkColumn.Visible = !showthumnail;
            }
        }

        public Boolean hasRows
        {
            get { return _hasRows; }
        }

        private DataTable GetDataTable()
        {
            DataTable table = new DataTable();

            #region fiveQuestionSet implementation
            //string Col;
            //for (int i = 0; i < 10; i++)
            //{
            //    if (i % 2 == 0)
            //    {
            //        Col = "Col_Q" + (i + 1).ToString();
            //    }
            //    else
            //    {
            //        Col = "Col_Ans" + (i + 1).ToString();
            //    }

            //    table.Columns.Add(Col, typeof(string));              
            //}
            #endregion

            table.Columns.Add("QuestionNo", typeof(int));
            table.Columns.Add("Answers", typeof(string));
            return table;
        }

        private DataTable GetScoreList(string rawScore)
        {
            DataTable dtScore = GetDataTable();
            int strLen = rawScore.Length;
            int count = 0;
            string[,] qset;
            string strChar;
            int num;
            Boolean isNum = false;
            int lastAnswered = 0;
            for (int charIndex = 0; charIndex < rawScore.Length; charIndex++)
            {
                strChar = rawScore.Substring(charIndex, 1);
                isNum = int.TryParse(strChar, out num);
                if (strChar == "#" || isNum)
                    lastAnswered = charIndex + 1;
            }
            //int lastAnswered = totalQuestionsInAssessment; 
            //while (count<lastAnswered && count+5<strLen)
            //{
            //    qset = new string[5, 2];
            //    for (int index = count; index < count + 5; index++)
            //    {
            //        qset[index % 5, 0] = (index + 1).ToString();
            //        qset[index % 5, 1] = rawScore.Substring(index, 1) == "*" || index > lastAnswered ? "" : rawScore.Substring(index, 1);
            //    }
            //    dtScore.NewRow();
            //    dtScore.Rows.Add(qset[0, 0], qset[0, 1], qset[1, 0], qset[1, 1], qset[2, 0], qset[2, 1], qset[3, 0], qset[3, 1], qset[4, 0], qset[4, 1]);
            //        count += 5;
            //}

            string strAnswer = string.Empty;
            for (int index = 0; index < lastAnswered; index++)
            {
                strAnswer = rawScore.Substring(index, 1) == "*" || index > lastAnswered ? "" : rawScore.Substring(index, 1);
                dtScore.NewRow();
                dtScore.Rows.Add(index + 1, strAnswer);
            }

            return dtScore;
        }

        public void addResult(Boolean debug, String seqNum, Int32 FormID, String StudentID, String TestForm, String MCQuestions)
        {
            dgvResults.Rows.Add(seqNum, FormID, StudentID, TestForm, MCQuestions, FormID > 0 ? "View Answers" : "");
            _hasRows = true;
            if (debug)
            {
                //this.Width = 900;
                dgvResults.Columns["MCQuestions"].Visible = true;
            }
            else
            {
                //this.Width = 490;
                dgvResults.Columns["MCQuestions"].Visible = false;
            }
        }

        private void writeToList(String text)
        {
            txtOutput.Text += text + Environment.NewLine;
            txtOutput.Invalidate();
        }

        private String buildFileString(String FormID, String StudentID, String TestForm, String MiscData, String MCQuestions)
        {
            String ReturnVal;
            if (appCommonUtil.formType == "PlainPaper")
            {
                ReturnVal = "PPSV4,\"" + StudentID + "\",\"" + FormID + "\",\"" + TestForm + "\",\"" + MCQuestions + "\"" + Environment.NewLine;
            }
            else
            {
                ReturnVal = "HALOV4,\"" + StudentID + "\",\"" + FormID + "\",\"" + TestForm + "\",\"" + MiscData + "\"," + MCQuestions + Environment.NewLine;
            }
            return ReturnVal;
        }

        private void setDebug(Boolean mode)
        {
            const int delta = 75;

            if (currDebugMode != mode)
            {
                if (currDebugMode == false)
                {
                    currDebugMode = true;
                    appCommonUtil.debug = true;
                    this.Height += delta;
                    txtOutput.Height -= delta;
                    grpDebug.Top -= delta;
                }
                else
                {
                    currDebugMode = false;
                    appCommonUtil.debug = false;
                    this.Height -= delta;
                    txtOutput.Height += delta;
                    grpDebug.Top += delta;
                    chkUseFiles.Checked = false;
                    chkSaveImg.Checked = false;
                }
            }
        }

        private void btnDiscard_Click(object sender, EventArgs e)
        {

            if (studentIds.Count == 0)
            {
                MessageBox.Show("Select scanned items from the list to Discard.", "G2D Scanning 5.0.1", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Are you sure to discard seleted test response(s) ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;

            foreach (DataGridViewRow dr in dgvResults.Rows)
            {
                DataGridViewCheckBoxCell chkBox = new DataGridViewCheckBoxCell();
                chkBox = (DataGridViewCheckBoxCell)dr.Cells[0];

                DataGridViewTextBoxCell studentIdCol = new DataGridViewTextBoxCell();
                studentIdCol = (DataGridViewTextBoxCell)dr.Cells["StudentID"];

                if (chkBox.Value != null && (chkBox.Value.ToString()) == "True")
                {
                    dr.Visible = false;
                    chkBox.Value = false;
                    studentIds.Remove(studentIdCol.Value.ToString());
                }
            }

            int rowCount = 0;
            for (int i = 0; i < dgvResults.Rows.Count; i++)
            {
                if (dgvResults.Rows[i].Visible == true)
                {
                    rowCount += 1;
                }
            }

            if (rowCount == 0)
            {
                dgvResults.Visible = false;
                txtOutput.Visible = true;

                btnScan.Enabled = true;
                grpImageType.Enabled = true;
                btnDiscard.Enabled = false;
                btnSubmit.Enabled = false;
                pctPictureToolTip.Visible = false;
            }
            HeaderCheckBox.Checked = false;
            
        }

        /// <summary>

        /// This function is used to check specified file being used or not

        /// </summary>

        /// <param name="file">FileInfo of required file</param>

        /// <returns>If that specified file is being processed 

        /// or not found is return true</returns>

        public static Boolean IsFileLocked(FileInfo file)
        {

            FileStream stream = null;



            try
            {

                //Don't change FileAccess to ReadWrite, 

                //because if a file is in readOnly, it fails.

                stream = file.Open

                (

                    FileMode.Open,

                    FileAccess.Read,

                    FileShare.None

                );

            }

            catch (IOException)
            {

                //the file is unavailable because it is:

                //still being written to

                //or being processed by another thread

                //or does not exist (has already been processed)

                return true;

            }

            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked

            return false;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (studentIds.Count == 0)
            {
                MessageBox.Show("Select scanned items from the list to Submit and Upload to the Server.", "G2D Scanning 5.0.1", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            pctProgress.Show();
            WebClient Client = new WebClient();
            frmImport importForm = new frmImport();
            int rownum = 0;
            try
            {
                string confirmMessage = "Are you sure to submit seleted test response(s) ?";
                for(int i=0;i<dgvResults.Rows.Count;i++)
                {
                    DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dgvResults.Rows[i].Cells[0];

                    if (dgvResults["FormID", i].Value.ToString() == "-1" && checkBoxCell.Value != null && checkBoxCell.Value.ToString() == "True")
                    {
                        string warningMessage = "WARNING: Records with errors will not be submitted.\n\n";
                        confirmMessage = warningMessage + confirmMessage;
                        break;
                    }
                }

                if (MessageBox.Show(confirmMessage, "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    return;

                //OK We have some data to upload, prompt the user to upload the 
                //    results if they say OK... Send the results to the server and 
                //    reset everything so that we're ready to scan the next set 
                //    of results
#if !RETRYTEST
                if (hasRows)
                {

                    dgvResults.Visible = true;
                    txtOutput.Visible = false;

                    String dataFileName;
                    //Upload the file to testgate
                    //if (uploadForm.ShowDialog() == DialogResult.OK)
                    //{
                    //MessageBox.Show("Result = OK");
                    if (appCommonUtil.formType == "PlainPaper")
                    {
                        dataFileName = appCommonUtil.clientID + "_" + "PPS" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                    }
                    else
                    {
                        dataFileName = appCommonUtil.clientID + "_" + "HALO" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".csv";
                    }

                    //First... Save the data file to the docFolder
                    TextWriter tw = new StreamWriter(appCommonUtil.docFolder + "\\" + dataFileName);

                    int gridRowNum = 0;
                    foreach (var myString in dataFile.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        for (int idx = gridRowNum; idx < dgvResults.Rows.Count; idx++)
                        {
                            DataGridViewTextBoxCell formID = (DataGridViewTextBoxCell)dgvResults.Rows[idx].Cells["FormID"];
                            DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)dgvResults.Rows[idx].Cells[0];
                            if (dgvResults.Rows[idx].Visible == true &&
                                checkBoxCell.Value != null && checkBoxCell.Value.ToString() == "True" &&
                                formID.Value.ToString() != "-1" && gridRowNum == idx)
                            {
                                rownum++;
                                tw.WriteLine(myString.ToString());
                                dgvResults.Rows[idx].Visible = false;
                                checkBoxCell.Value = "False";
                                break;
                            }
                        }
                        gridRowNum++;
                    }

                    tw.Close();
                    tw.Dispose();
#else // #if !RETRYTEST
							String dataFileName = "test_PPS20101109125708.csv";
#endif // #else // #if !RETRYTEST

#if false // Original code. Did not handle timeouts.
                     
                     
#endif
                    // We will make each upload a maximum of 500 records.

                    // Upload to the FTP Server
                      

                    Int32 recCount = 0, successCount = 0, errorCount = 0, maxRecsUpload = 500, i;
                    StreamReader rdr = File.OpenText(appCommonUtil.docFolder + "\\" + dataFileName);



                    string sData = string.Empty;
                    while (!rdr.EndOfStream)
                    {
                        // Create a temporary file to use for this batch of uploads. We add one second to make sure that the name is not the same as the original file.
                        String fileName = appCommonUtil.clientID + "_" + "PPS" + DateTime.Now.AddSeconds(1).ToString("yyyyMMddhhmmss") + ".csv";
                        String tempFilePath = appCommonUtil.docFolder + "\\" + dataFileName;
                        for (i = 0; i < maxRecsUpload && !rdr.EndOfStream; i++)
                        {
                            sData += "START," + rdr.ReadLine() + ",END" + System.Environment.NewLine;
                        }

                        Boolean retry = false;
                        do
                        {
                            retry = false;
                            try
                            {
                                if (i > 0)
                                {
                                    #region Legacy Code Replacement

                                   


                                    string protocolName = appCommonUtil.useHttps ? "https://" : "http://";
                                    string webAPI = ConfigurationManager.AppSettings["WebAPI"];
                                   
                                    //string address = protocolName + appCommonUtil.server + "/" + webAPI + "/api/Upload/UploadFiles";
                                    //  Random r = new Random();
                                    //  long n = r.Next(99999, 999999999);

                                    //  UploadParam oParam = new UploadParam();
                                    //  oParam.fileName = n.ToString() + "_" + dataFileName;
                                    //  oParam.fileData = sData;

                                    ////  address += "?fileName=" + oParam.fileName + "&fileData=" + oParam.fileData;

                                    //  object webDataUpload = "value=" + oParam.fileName + "~" + sData;
                                    //  string jsonUpload = Newtonsoft.Json.JsonConvert.SerializeObject(webDataUpload);

                                    //using (var client = new WebClient())
                                    //{
                                    //    client.Headers[HttpRequestHeader.Accept] = "application/json";
                                    //    string result = client.DownloadString(address);
                                    //    //List<string> resultSet = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(result);
                                    //    if (result == null && result == string.Empty)
                                    //    {

                                    //        MessageBox.Show("Error has occured in Uploading the files. Please contact to Administrator");
                                    //        return;

                                    //    }
                                    //}


                                    //string arenaImportFolder = @"\\localhost\DBAccess"; //ConfigurationManager.AppSettings["ArenaImportFolder"];
                                    //Random r = new Random();
                                    //long n = r.Next(99999, 999999999);
                                    //fileName = n.ToString() + "_" + fileName;

                                    //File.Copy(tempFilePath, arenaImportFolder + fileName);

                              
                                    string webAddress = protocolName + appCommonUtil.server + "/" + webAPI + "/api/Common/SaveTestResponseCard";
                                    object webData = "User:" + appCommonUtil.currUserId + "~Filename: " + fileName + "~FileData: " + sData + "~TestID: 0~Memo: Auto Upload~UploadJobId: 0~filename:";
                                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(webData);



                                    using (var client = new WebClient())
                                    {
                                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                                        string result = client.UploadString(new Uri(webAddress), "POST", json);
                                        List<TestResponseCardResult> testResponses = (Newtonsoft.Json.JsonConvert.DeserializeObject<List<TestResponseCardResult>>(result));

                                        if (testResponses.Count > 0)
                                        {
                                            for (int index = 0; index < testResponses.Count; index++)
                                            {
                                                //String seqNum = (index + 1).ToString();
                                                //String message = testResponses[index].Message;
                                                recCount = testResponses[index].CardsRead;
                                                successCount = testResponses[index].CardsImported;
                                                errorCount = testResponses[index].CardsRead - testResponses[index].CardsImported;
                                                importForm.jobID = testResponses[index].ReturnJobIDs.ToString();
                                            }
                                        }
                                    }

                                    #endregion
                                }
                            }
                            catch (Exception x)
                            {
                                // If the user wants to retry.
                                if (MessageBox.Show("Error Uploading Data" + Environment.NewLine + x.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                                    retry = true;
                                // Terminate the upload by rethrowing the exception to the outer catch block.
                                else
                                    throw x;
                            }
                        } while (retry == true);
                    }
                    rdr.Close();
                    pctProgress.Hide();
                    studentIds.Clear();
                    importForm.ShowDialog();
#if !RETRYTEST
                    // }
                }
#endif // #if !RETRYTEST

                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Uploading Data" + Environment.NewLine + ex.Message);
            }
            finally
            {
                int visibleRowCount = 0;
                foreach (DataGridViewRow row in dgvResults.Rows)
                {
                    if (row.Visible == true)
                    {
                        visibleRowCount++;
                    }
                }

                if (visibleRowCount == 0)
                {
                    pctSmall.Visible = false;
                    pctExpand.Visible = false;
                    dgvResults.Visible = false;
                    txtOutput.Visible = true;

                    btnClose.ForeColor = Color.White;
                    btnSubmit.ForeColor = Color.White;

                    btnClose.Enabled = true;
                    btnSubmit.Enabled = false;
                    btnScan.Enabled = true;
                    grpImageType.Enabled = true;
                    btnDiscard.Enabled = false;
                    btnScanner.Enabled = true;
                }
                // checking and unchecking header checkbox to call header checkbox click event to uncheck all rows checkboxes.
                HeaderCheckBox.Checked = true;
                HeaderCheckBox.Checked = false;
                pctProgress.Hide();
            }
        }

        private bool UploadFile(string source, string fileName, string ftpurl, string ftpusername, string ftppassword)
        {
            bool result = false;
            try
            {

                string ftpfullpath = ftpurl;

                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath + "/" + fileName);
                ftp.Credentials = new NetworkCredential(ftpusername, ftppassword);

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                // Set buffer size
                int intBufferLength = 16 * 1024;
                byte[] objBuffer = new byte[intBufferLength];

                FileStream fs = File.OpenRead(source);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);



                ftp.ContentLength = buffer.Length;
                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
                fs.Close();


                //// Get Stream of the file
                //Stream objStream = objFTPRequest.GetRequestStream();

                //int len = 0;

                //while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
                //{
                //    // Write file Content 
                //    objStream.Write(objBuffer, 0, len);

                //}

                //objStream.Close();
                //objFileStream.Close();

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        frmDialogPreview frmFileDialogPreview = new frmDialogPreview();

        private void dgvResults_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridView view = (DataGridView)sender;

                if (e.Button == System.Windows.Forms.MouseButtons.Left && e.RowIndex >= 0)
                {
                    if (e.ColumnIndex == THUMBNAIL_COLINDEX)
                    {
                        frmFileDialogPreview.BitmapImage = (Bitmap)pctPictureToolTip.Image;
                        frmFileDialogPreview.FileName = view.Rows[e.RowIndex].Cells[7].Value.ToString();//File path kept hidden.
                        frmFileDialogPreview.ShowDialog();
                    }
                    else if (e.ColumnIndex == LINK_COLINDEX)
                    {
                        frmFileDialogPreview.FileName = view.Rows[e.RowIndex].Cells[FILEPATH_COLINDEX].Value.ToString();//File path kept hidden.
                        frmFileDialogPreview.BitmapImage = new Bitmap(frmFileDialogPreview.FileName);
                        frmFileDialogPreview.ShowDialog();
                    }
                    else if (e.ColumnIndex == ERROR_COLINDEX)
                    {
                        frmDialog errorDialog = new frmDialog();

                        errorDialog.ErrorMessage = view.Rows[e.RowIndex].Cells[ERROR_COLINDEX].Value.ToString();
                        errorDialog.ShowDialog();
                    }
                    else if (e.ColumnIndex == 6)
                    {
                        // No action when there is an error on scanning form
                        if (view.Rows[e.RowIndex].Cells["FormID"].Value.ToString() == "-1")
                            return;

                        frmScorePreview frmPreviewScore = new frmScorePreview(appCommonUtil);
                        string strScores = view.Rows[e.RowIndex].Cells[5].Value.ToString(); //Score col
                        frmPreviewScore.FormDefID = Convert.ToInt32(view.Rows[e.RowIndex].Cells[2].Value);//FormDef Col
                        frmPreviewScore.TableAssessment = frmPreviewScore.GetBubbleSheetItems(frmPreviewScore.FormDefID);//This function can be written in this class also. Later it will implement with API/Srvices. SO may be removed.

                        totalQuestionsInAssessment = frmPreviewScore.TableAssessment.Count;

                        //frmPreviewScore.StudentScore = GetScoreList(strScores);
                        frmPreviewScore.StudentScore = GetScoreList(strScores);
                        frmPreviewScore.ShowDialog();

                        //MessageBox.Show(view.Rows[e.RowIndex].Cells[5].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvResults_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Skip the Column and Row headers
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                {
                    return;
                }
                var dataGridView = (sender as DataGridView);
                //Check the condition as per the requirement casting the cell value to the appropriate type
                if (e.ColumnIndex >= THUMBNAIL_COLINDEX && e.ColumnIndex <= FILEPATH_COLINDEX)//Thumbnail Column(6)
                {
                    pctPictureToolTip.Visible = true;
                    pctPictureToolTip.Left = MousePosition.X - (dataGridView.Width / 2);
                    pctPictureToolTip.Top = MousePosition.Y - (dataGridView.Height / 2);
                    pctPictureToolTip.SizeMode = PictureBoxSizeMode.StretchImage;
                    pctPictureToolTip.Image = (Bitmap)dataGridView.Rows[e.RowIndex].Cells[THUMBNAIL_COLINDEX].Value;

                    frmFileDialogPreview.BitmapImage = (Bitmap)pctPictureToolTip.Image;

                    // pctPictureToolTip.ImageLocation = dataGridView.Rows[e.RowIndex].Cells[7].Value.ToString();
                    //pctPictureToolTip.filena
                    dataGridView.Cursor = Cursors.Hand;
                    pctPictureToolTip.Cursor = Cursors.Hand;
                }
                else
                {
                    pctPictureToolTip.Visible = false;
                }

                if (e.ColumnIndex == THUMBNAIL_COLINDEX)//Score detail column
                {
                    dataGridView.Cursor = Cursors.Hand;
                }
                else
                    dataGridView.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {

            }
        }

        private void dgvResults_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           // dgvResults.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dgvResults.AutoSizeRowsMode = radioThumbnail.Checked == false ? DataGridViewAutoSizeRowsMode.AllCells : DataGridViewAutoSizeRowsMode.None;

           // dgvResults.Rows[e.RowIndex].Height = radioThumbnail.Checked ? 22 : 10;

            if (dgvResults["StudentID", e.RowIndex].Value.ToString().Contains("#"))
            {
                dgvResults["StudentID", e.RowIndex].Value += Environment.NewLine + "                             CAUTION: error with bubbles";
                dgvResults["StudentID", e.RowIndex].Style = new DataGridViewCellStyle { ForeColor = Color.Red, WrapMode = DataGridViewTriState.True };
            }
            else
            {
                //dgvResults["StudentID", e.RowIndex].Style = dgvResults.DefaultCellStyle;
            }
        }

        private void pctPictureToolTip_Click(object sender, EventArgs e)
        {
            frmFileDialogPreview.ShowDialog();
        }
        //int TotalCheckBoxes = 0;
        //int TotalCheckedCheckBoxes = 0;
        bool isCellChecked = false;

        private void dgvResults_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && isOuterChecked == false)
            {
                isCellChecked = true;
                DataGridViewCheckBoxCell chkBox = new DataGridViewCheckBoxCell();
                chkBox = (DataGridViewCheckBoxCell)dgvResults.Rows[dgvResults.CurrentRow.Index].Cells[0];

                DataGridViewTextBoxCell studentIdCol = new DataGridViewTextBoxCell();
                studentIdCol = (DataGridViewTextBoxCell)dgvResults.Rows[dgvResults.CurrentRow.Index].Cells["StudentID"];

                if (chkBox.Value == null)
                    chkBox.Value = false;
                switch (chkBox.Value.ToString())
                {
                    case "True":
                        chkBox.Value = false;
                        studentIds.Remove(studentIdCol.Value.ToString());
                        break;
                    case "False":
                        chkBox.Value = true;
                        studentIds.Add(studentIdCol.Value.ToString());
                        break;
                }

                if ((bool)chkBox.Value && TotalCheckedCheckBoxes < TotalCheckBoxes)
                    TotalCheckedCheckBoxes++;
                else if (TotalCheckedCheckBoxes > 0)
                    TotalCheckedCheckBoxes--;

                //Change state of the header CheckBox.
                if (TotalCheckedCheckBoxes < TotalCheckBoxes)
                {
                    HeaderCheckBox.Checked = false;
                    isCellChecked = false;
                }

                else if (TotalCheckedCheckBoxes == TotalCheckBoxes && TotalCheckBoxes > 0)
                    HeaderCheckBox.Checked = true;
            }
        }

        private void chkImageSave_CheckedChanged(object sender, EventArgs e)
        {
            grpImageType.Visible = grpDisplay.Visible = chkImageSave.Checked;
        }

        private void radioThumbnail_CheckedChanged(object sender, EventArgs e)
        {
            ShowHideThumnailHyperlinkColumn(radioThumbnail.Checked);
        }

        private void dgvResults_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            //if (e.ColumnIndex == this.dgvResults.Columns["chkAll"].Index
            //    && e.RowIndex > -1 && e.RowIndex != this.dgvResults.NewRowIndex)
            //{
            //    if (this.dgvResults["FormID", e.RowIndex].Value.ToString() == "-1")
            //    {
            //        e.PaintBackground(e.CellBounds, true);
            //        Rectangle r = e.CellBounds;
            //        r.Width = 13;
            //        r.Height = 13;
            //        r.X += e.CellBounds.Width / 2 - 6;
            //        r.Y += e.CellBounds.Height / 2 - 6;
            //        ControlPaint.DrawCheckBox(e.Graphics, r, ButtonState.Inactive);
            //        e.Handled = true;
            //    }
            //}
        }
    }

    public class TestResponseCardResult
    {
        public string Message { get; set; }
        public int CardsRead { get; set; }
        public int CardsImported { get; set; }
        public int ReturnJobIDs { get; set; }
    }

    public class UploadParam
    {
      public   string fileName { get; set; }
      public   string fileData { get; set; }
    }

}
