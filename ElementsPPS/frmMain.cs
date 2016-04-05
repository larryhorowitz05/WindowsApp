using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace ElementsPPS
{
    public partial class frmMain : Form
    {
        private commonUtil appCommonUtil;
        private Boolean currDebugMode = false;

        public frmMain()
        {
            appCommonUtil = new commonUtil();
            InitializeComponent();
        }

        public frmMain(commonUtil currCommonUtil)
        {
            appCommonUtil = currCommonUtil;
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblversion.Text = "PPS Version " + appCommonUtil.versionNum + " - " + appCommonUtil.formType;
            setDebug(appCommonUtil.debug);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
						axImg.SetLicenseNumber("1519667525273646862381256");
						templateImg.SetLicenseNumber("1519667525273646862381256");
            btnClose.Enabled = false;
            btnOpenFolder.Enabled = false;
            btnScan.Enabled = false;
						prefBtn.Enabled = false;
						
            String fileName;
            Int64 nImageID;
            Int32 nImageCount = 0;
            StringBuilder dataFile = new StringBuilder();
            StringBuilder dataFileDisplay = new StringBuilder();
						String imagesPath = appCommonUtil.docFolder + "\\images";
	
            //create the upload display form
            frmUpload uploadForm = new frmUpload();

            //These are used when uploading the datafile
            WebClient Client = new WebClient();
            byte[] responseArray;

#if !RETRYTEST
            try
            {
								// If this is the first execution of the program, the images directory does not exist,
								// create it.
								if(!Directory.Exists(imagesPath))
									Directory.CreateDirectory(imagesPath);
									
                //delete old upload files???
                //delete old images
									foreach (String currFileName in Directory.GetFiles(imagesPath, "*.bmp"))
                {
                    File.Delete(currFileName);
                }

                //delete old template images
								foreach (String currFileName in Directory.GetFiles(imagesPath, "*.jpg"))
                {
                    File.Delete(currFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to find required system folder or perform required function. Please reinstall the software.");
                btnClose.Enabled = true;
                btnOpenFolder.Enabled = true;
                btnScan.Enabled = true;
								prefBtn.Enabled = true;
                this.Close();
                return;
            }

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
										if(src.Contains("INSIGHT 20") || src.Contains("INSIGHT 30"))
										{
											Boolean b = axImg.TwainSetCapCurrentString(GdPicturePro5.TwainCapabilities.ICAP_HALFTONES, GdPicturePro5.TwainItemTypes.TWTY_STR32, "Fixed Thresholding");
											b = axImg.TwainSetCapCurrentNumeric(GdPicturePro5.TwainCapabilities.ICAP_THRESHOLD, GdPicturePro5.TwainItemTypes.TWTY_INT32, 90);
										}
										else if(src.Contains("HP") && src.Contains("7000"))
										{
											Boolean b = axImg.TwainSetCurrentContrast(800);
										}

										// We can override brightness and contrast if there are entries in the configuration xml file.
										if(appCommonUtil.brightness.HasValue)
										{
											axImg.TwainSetAutoBrightness(false);
											axImg.TwainSetCurrentBrightness(appCommonUtil.brightness.Value);
										}
										
										if(appCommonUtil.contrast.HasValue)
										{
											axImg.TwainSetCurrentContrast(appCommonUtil.contrast.Value);
										}
										
                    //scan all of the forms in the scanner
                    while (axImg.CreateImageFromTwain() != 0)
                    {
                        try
                        {
                            nImageCount++;
                            writeToList("Scoring item number : " + nImageCount);

                            nImageID = axImg.GetNativeImage();
                            fileName = imagesPath + "\\PPScanImages_" + nImageCount + ".bmp";
                            axImg.SaveAsBmp(fileName);

                            //set up scorecardreader and read the form
                            VersaITScoreReader vr;
                            vr = new VersaITScoreReader(appCommonUtil, axImg);
                            if (chkSaveImg.Checked)
                            {
                                axImg.SaveAsJPEG(imagesPath + "\\template.jpg");
                                templateImg.CreateImageFromFile(imagesPath + "\\template.jpg");
                                templateImg.ConvertTo32BppARGB();
                                vr.TemplateImageObject = templateImg;
                            }
                            vr.readForm("");

                            //add score to file
                            dataFile.Append(buildFileString(vr.formID, vr.studentID, vr.testForm, vr.miscData, vr.MCQuestions));
                            uploadForm.addResult(appCommonUtil.debug, nImageCount.ToString(), vr.formID, vr.studentID, vr.testForm, vr.MCQuestions);
                            if (appCommonUtil.debug) { 
                                writeToList(vr.debugOutput); 
                                writeToList(nImageCount + "," + vr.formID + "," + vr.studentID + "," + vr.testForm + "," + vr.MCQuestions + Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Scoring Item Number " + nImageCount + Environment.NewLine + ex.Message);
                        }
                    }
                    axImg.TwainCloseSource();
                    axImg.TwainCloseSourceManager();
                }
                else
                {
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

                            //add score to file
                            dataFile.Append(buildFileString(vr.formID, vr.studentID, vr.testForm, vr.miscData, vr.MCQuestions));
                            uploadForm.addResult(appCommonUtil.debug, nImageCount.ToString(), vr.formID, vr.studentID, vr.testForm, vr.MCQuestions);
                            if (appCommonUtil.debug) { 
                                writeToList(vr.debugOutput); 
                                writeToList(nImageCount + "," + vr.formID + "," + vr.studentID + "," + vr.testForm + "," + vr.MCQuestions + Environment.NewLine);
                            }
                        }
                        catch (Exception ex)
                        {
                            writeToList("Error Scoring Item Number " + nImageCount + Environment.NewLine + ex.Message);
                        }
                    }
                }
            }

            if (File.Exists(appCommonUtil.docFolder + "\\images\\template.jpg"))
            {
                File.Delete(appCommonUtil.docFolder + "\\images\\template.jpg");
            }
#endif
						try
            {
                //OK We have some data to upload, prompt the user to upload the 
                //    results if they say OK... Send the results to the server and 
                //    reset everything so that we're ready to scan the next set 
                //    of results
#if !RETRYTEST
                if (uploadForm.hasRows)
                {
                    String dataFileName;
                    //Upload the file to testgate
                    if (uploadForm.ShowDialog() == DialogResult.OK)
                    {
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
                        tw.WriteLine(dataFile.ToString());
                        tw.Close();
#else // #if !RETRYTEST
							String dataFileName = "test_PPS20101109125708.csv";
#endif // #else // #if !RETRYTEST

#if false // Original code. Did not handle timeouts.
                        //If the useHttps flag is true then first attempt to upload the files using HTTPS
                        if (appCommonUtil.useHttps)
                        {
                            responseArray = Client.UploadFile("https://" + appCommonUtil.server + "/" + appCommonUtil.clientID + "/testresponsecardimportPPS.asp?passKey=pla1npap3rscan2008&installID=PPS" + appCommonUtil.installID + "&version=" + appCommonUtil.shortVersionNum + "&x=PPS&tp=0&fileupload=True&curruser=" + appCommonUtil.currUserId, appCommonUtil.docFolder + "\\" + dataFileName);
                            MessageBox.Show(System.Text.Encoding.ASCII.GetString(responseArray));
                        }
                        else
                        {
                            responseArray = Client.UploadFile("http://" + appCommonUtil.server + "/" + appCommonUtil.clientID + "/testresponsecardimportPPS.asp?passKey=pla1npap3rscan2008&installID=PPS" + appCommonUtil.installID + "&version=" + appCommonUtil.shortVersionNum + "&x=PPS&tp=0&fileupload=True&curruser=" + appCommonUtil.currUserId, appCommonUtil.docFolder + "\\" + dataFileName);
                            MessageBox.Show(System.Text.Encoding.ASCII.GetString(responseArray));
												}
#endif
												// We will make each upload a maximum of 500 records.
												Int32 recCount = 0, successCount = 0, errorCount = 0, maxRecsUpload = 500, i;
												StreamReader rdr = File.OpenText(appCommonUtil.docFolder + "\\" + dataFileName);
												String url = String.Format(@"{0}://{1}/{2}/testresponsecardimportPPS.asp?passKey=pla1npap3rscan2008&installID=PPS{3}&version={4}&x=PPS&tp=0&fileupload=True&curruser={5}",
																									 (appCommonUtil.useHttps) ? "https" : "http", appCommonUtil.server, appCommonUtil.clientID, appCommonUtil.installID, appCommonUtil.shortVersionNum,	appCommonUtil.currUserId);

												while(!rdr.EndOfStream)
												{
													// Create a temporary file to use for this batch of uploads. We add one second to make sure that the name is not the same as the original file.
													String tempFilePath = appCommonUtil.docFolder + "\\" + appCommonUtil.clientID + "_" + "PPS" + DateTime.Now.AddSeconds(1).ToString("yyyyMMddhhmmss") + ".csv";
													StreamWriter writer = File.CreateText(tempFilePath);
													for (i = 0; i < maxRecsUpload && !rdr.EndOfStream; i++)
														writer.WriteLine(rdr.ReadLine());
													writer.Close();
														
													Boolean retry = false;
													do
													{
														retry = false;
														try
														{
															if(i > 0)
															{
																String response = System.Text.Encoding.ASCII.GetString(Client.UploadFile(url, tempFilePath));
																if(response.Contains("failed"))
																	throw new Exception();
																// Parse the response for the record counts.
																Match m = Regex.Match(response, @"(\d*)\D*(\d*)\D*(\d*).*");
																if(m.Groups.Count < 4)
																	throw new Exception();
																Int32 r, s, er;
																if(Int32.TryParse(m.Groups[1].Value, out r))
																	recCount += r;
																if (Int32.TryParse(m.Groups[2].Value, out s))
																	successCount += s;
																if (Int32.TryParse(m.Groups[3].Value, out er))
																	errorCount += er;
															}
														}
														catch(Exception x)
														{
															// If the user wants to retry.
					                    if(MessageBox.Show("Error Uploading Data" + Environment.NewLine + x.Message, "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
																retry = true;
															// Terminate the upload by rethrowing the exception to the outer catch block.
															else
																throw x;
														}
														finally
														{
															if(retry == false)
																File.Delete(tempFilePath);
														}
													} while(retry == true);
												}
												rdr.Close();
												MessageBox.Show(String.Format("{0} Test response cards read; {1} Successfully imported; {2} Errors.", recCount, successCount, errorCount));
#if !RETRYTEST
                    }
                }
#endif // #if !RETRYTEST
						}
            catch (Exception ex)
            {
	            MessageBox.Show("Error Uploading Data" + Environment.NewLine + ex.Message);
            }
            btnClose.Enabled = true;
            btnOpenFolder.Enabled = true;
            btnScan.Enabled = true;
						prefBtn.Enabled = true;
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void writeToList(String text)
        {
            txtOutput.Text += text + Environment.NewLine;
            txtOutput.Invalidate();
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            String windir = Environment.GetEnvironmentVariable("WINDIR");
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = windir + @"\explorer.exe";
            p.StartInfo.Arguments = appCommonUtil.docFolder;
            p.Start();
        }

				private void prefBtn_Click(object sender, EventArgs e)
				{
					if (axImg.TwainSelectSource())
						appCommonUtil.defaultScanner = axImg.TwainGetDefaultSourceName();
					else
						appCommonUtil.defaultScanner = "";
				}

				private void saveFile(String fileName)
        {
            SaveFileDialog saveScript = new SaveFileDialog();
            saveScript.FileName = fileName;
            saveScript.Filter = "CSV File| *.csv";
            saveScript.Title = "Save Test Results File";
            //saveScript.InitialDirectory = appCommonUtil.docFolder;
            saveScript.AddExtension = true;
            saveScript.OverwritePrompt = true;
            saveScript.ShowDialog();

            //If the file name is not an empty string open it for saving.
            if (saveScript.FileName != "")
            {
                FileStream saveFileStream = new FileStream(appCommonUtil.docFolder + saveScript.FileName, FileMode.Open, FileAccess.Read);
                StreamReader saveFileReader = new StreamReader(saveFileStream);
                FileStream writeFileStream = (FileStream)(saveScript.OpenFile());
                StreamWriter writeFileWriter = new StreamWriter(writeFileStream);
                writeFileWriter.Write(saveFileReader.ReadToEnd());
                writeFileWriter.Flush();
                writeFileStream.Close();
                saveFileStream.Close();
            }
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
                    if (appCommonUtil.debug == false) { setDebug(true); }
                    else { setDebug(false); }
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
                    lblversion.Text = "PPS Version " + appCommonUtil.versionNum + " - " + appCommonUtil.formType;
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
										btnPnl.Top -= delta;
										grpDebug.Top -= delta;
                }
                else
                {
                    currDebugMode = false;
                    appCommonUtil.debug = false;
										this.Height -= delta;
										txtOutput.Height += delta;
										btnPnl.Top += delta;
										grpDebug.Top += delta;
                    chkUseFiles.Checked = false;
                    chkSaveImg.Checked = false;
                }
            }
        }

    }
}