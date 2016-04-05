using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using AxGdPicturePro5;
using GdPicturePro5;


namespace ElementsPPS
{
    class VersaITScoreReader
    {
        #region Member Variables

        private commonUtil appUtility;

        //output values
        private String _formID = "";
        private String _studentID = "";
        private String _testForm = "";
        private String _miscData = "";
        private String _MCQuestions = "";
        private String _debugOutput = "";

        //Image information
        private AxImaging axScoreImg; // = new AxImaging();
        private AxImaging templateImg; // = new AxImaging();

        //template Documents
        private XmlDocument formNumberTemplate;
        private XmlDocument docTemplate;
        
        //location Vars of the corners, Actual and Expected
        private Point topLeft;
        private Point topRight;
        private Point bottomLeft;
        private Point bottomRight;
        private Point topLeftExp;
        private Point topRightExp;
        private Point bottomLeftExp;
        private Point bottomRightExp;
				private Double expectedWidth;
				private Double expectedHeight;
				private Double actualWidth;
				private Double actualHeight;

        #endregion

        # region Properties

        /// <summary>
        /// gets or sets the image object to be processed
        /// </summary>
        public AxImaging ImageObject
        {
            get { return axScoreImg; }
            set { axScoreImg = value; }
        }

        /// <summary>
        /// gets or sets the template image object to be processed
        /// </summary>
        public AxImaging TemplateImageObject
        {
            get { return templateImg; }
            set { templateImg = value; }
        }

        /// <summary>
        /// gets the form id after processing the image
        /// </summary>
        public String formID
        {
            get { return _formID; }
        }

        /// <summary>
        /// gets the student id after processing the image
        /// </summary>
        public String studentID
        {
            get { return _studentID; }
        }

        /// <summary>
        /// gets the multiple choice question string (answers) after processing the image
        /// </summary>
        public String MCQuestions
        {
            get { return _MCQuestions; }
        }

        /// <summary>
        /// gets the test form after processing the image
        /// </summary>
        public String testForm
        {
            get { return _testForm; }
        }

        /// <summary>
        /// gets the misc data after processing the image
        /// </summary>
        public String miscData
        {
            get { return _miscData; }
        }

        /// <summary>
        /// gets the debug output information from the processing
        /// </summary>
        public String debugOutput
        {
            get { return _debugOutput; }
        }

        # endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apputil">the common utility object for the application</param>
        public VersaITScoreReader(commonUtil appUtil)
        {
            appUtility = appUtil;
            topLeft = new Point();
            topRight = new Point();
            bottomLeft = new Point();
            bottomRight = new Point();
            topLeftExp = new Point();
            topRightExp = new Point();
            bottomLeftExp = new Point();
            bottomRightExp = new Point();

            //Try to load the form number selection document
            try
            {
                formNumberTemplate = new XmlDocument();
                formNumberTemplate.Load(appUtility.templateChooser);
            }
            catch (Exception e) { throw new Exception("Unable to load Form Number Template Document", e); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apputil">the common utility object for the application</param>
        /// <param name="imagingObject">imaging object that we are going to perform all of the processing on</param>
        public VersaITScoreReader(commonUtil appUtil, AxImaging imagingObject)
        {
            axScoreImg = imagingObject;
            appUtility = appUtil;
            topLeft = new Point();
            topRight = new Point();
            bottomLeft = new Point();
            bottomRight = new Point();
            topLeftExp = new Point();
            topRightExp = new Point();
            bottomLeftExp = new Point();
            bottomRightExp = new Point();


            //Try to load the form number selection document
            try
            {
                formNumberTemplate = new XmlDocument();
                formNumberTemplate.Load(appUtility.templateChooser);
            }
            catch (Exception e) { throw new Exception("Unable to load Form Number Template Document", e); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apputil">the common utility object for the application</param>
        /// <param name="imagingObject">the imaging object that we are going to perform all of the processing on</param>
        /// <param name="templateObject">the imaging object to draw the template onto</param>
        public VersaITScoreReader(commonUtil appUtil, AxImaging imagingObject, AxImaging templateObject)
        {
            axScoreImg = imagingObject;
            templateImg = templateObject;
            appUtility = appUtil;
            topLeft = new Point();
            topRight = new Point();
            bottomLeft = new Point();
            bottomRight = new Point();
            topLeftExp = new Point();
            topRightExp = new Point();
            bottomLeftExp = new Point();
            bottomRightExp = new Point();


            //Try to load the form number selection document
            try
            {
                formNumberTemplate = new XmlDocument();
                formNumberTemplate.Load(appUtility.templateChooser);
            }
            catch (Exception e) { throw new Exception("Unable to load Form Number Template Document", e); }
        }

        #region Methods

        /// <summary>
        /// Reads the form and scores it. Retrieve the results with FormID, StudentID, MCQuestions and SAQuestions
        /// </summary>
        public string readForm(string fileName)
        {
            String formNumber = "";
            String errorMessage = "";
            Int32 scoreThreshold = 80;
            //String templateName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + ".jpg";

            string isError = findCorners();
            if (!string.IsNullOrEmpty(isError))
                return isError;

            #region template image - draw the corners on the form

            if (templateImg != null)
            {
                try
                {
                    templateImg.DrawFillRectangle(topLeft.X, topLeft.Y, 20, 20, Colors.Red, false);
                    templateImg.DrawFillRectangle(bottomLeft.X, bottomLeft.Y - 20, 20, 20, Colors.Red, false);
                    templateImg.DrawFillRectangle(topRight.X - 20, topRight.Y, 20, 20, Colors.Red, false);
                    templateImg.DrawFillRectangle(bottomRight.X - 20, bottomRight.Y - 20, 20, 20, Colors.Red, false);
                }
                catch (Exception) { /*supress errors. we don't want this to break the main functionality*/ }
            }

            #endregion

            //Open the formNumber Template and set the Expected Corners
            try
            {
                XmlNode corners = formNumberTemplate.SelectSingleNode("template/itemDef[@name = 'Alignment']");
                topLeftExp.setVals(0, 0);
                bottomLeftExp.setVals(0, Convert.ToInt32(corners.SelectSingleNode("ObjShiftY").Attributes["val"].Value) + Convert.ToInt32(corners.SelectSingleNode("ObjHeight").Attributes["val"].Value));
                topRightExp.setVals(Convert.ToInt32(corners.SelectSingleNode("ObjShiftX").Attributes["val"].Value) + Convert.ToInt32(corners.SelectSingleNode("ObjWidth").Attributes["val"].Value), 0);
                bottomRightExp.setVals(Convert.ToInt32(corners.SelectSingleNode("ObjShiftX").Attributes["val"].Value) + Convert.ToInt32(corners.SelectSingleNode("ObjWidth").Attributes["val"].Value), Convert.ToInt32(corners.SelectSingleNode("ObjShiftY").Attributes["val"].Value) + Convert.ToInt32(corners.SelectSingleNode("ObjHeight").Attributes["val"].Value));
								expectedWidth = topRightExp.X - topLeftExp.X;
								expectedHeight = bottomLeftExp.Y - topLeftExp.Y;
								actualWidth = topRight.X - topLeft.X;
								actualHeight = bottomLeft.Y - topLeft.Y;
						}
            catch (Exception e)
            {
                //throw new Exception("Error reading form (unable to read form number template)", e);
                return "Error reading form (unable to read form number template)";
            }

            #region template image - draw where we are going to look for the form number

            //This section is only used in Debugmode and allows the template that is used to be drawn onto the 
            //    Image file. The resulting file is saved as a jpeg in the same folder.
            if (templateImg != null)
            {
                try
                {
                    drawTemplate(formNumberTemplate.SelectSingleNode("template/itemDef[@name = 'FormNumber']"));
                    //If we are in HALO mode write out the alignment markers
                    if (appUtility.formType == "HALO") { drawTemplate(formNumberTemplate.SelectSingleNode("template/itemDef[@name = 'HALOAlignment']")); }
                    templateImg.SaveAsJPEG(fileName);
                }
                catch (Exception) { /*supress errors. we don't want this to break the main functionality*/ }
            }

            #endregion

            //figure out what form it is
            try
            {
                formNumber = (scoreItems(formNumberTemplate.SelectSingleNode("template/itemDef[@name = 'FormNumber']"), 50)).Replace("*", "0");
                //If this is a HALO form check the alignment markers
                if (appUtility.formType == "HALO" && formNumber == "000000")
                {
                    XmlNode currNode = formNumberTemplate.SelectSingleNode("template/itemDef[@name = 'HALOAlignment']");
                    Point expected = new Point();
                    Point actual = new Point();
                    Int32 HALOThreshold = Convert.ToInt32(currNode.SelectSingleNode("Threshold").Attributes["val"].Value);

                    for (Int32 i = 0; i < Convert.ToInt32(currNode.SelectSingleNode("ObjColumns").Attributes["val"].Value); i++)
                    {
                        for (Int32 j = 0; j < Convert.ToInt32(currNode.SelectSingleNode("ObjRows").Attributes["val"].Value); j++)
                        {
                            for (Int32 k = 0; k < Convert.ToInt32(currNode.SelectSingleNode("ItemCount").Attributes["val"].Value);  k++)
                            {
                                expected.X = (Int32)(Convert.ToDecimal(currNode.SelectSingleNode("ObjStartX").Attributes["val"].Value) + (i * Convert.ToDecimal(currNode.SelectSingleNode("ObjShiftX").Attributes["val"].Value)) + Convert.ToDecimal(currNode.SelectSingleNode("ItemStartX").Attributes["val"].Value) + (k * Convert.ToDecimal(currNode.SelectSingleNode("ItemShiftX").Attributes["val"].Value)));
                                expected.Y = (Int32)(Convert.ToDecimal(currNode.SelectSingleNode("ObjStartY").Attributes["val"].Value) + (j * Convert.ToDecimal(currNode.SelectSingleNode("ObjShiftY").Attributes["val"].Value)) + Convert.ToDecimal(currNode.SelectSingleNode("ItemStartY").Attributes["val"].Value) + (k * Convert.ToDecimal(currNode.SelectSingleNode("ItemShiftY").Attributes["val"].Value)));
                                actual = getActual(expected);
                                if (isFilledIn(actual.X, actual.Y, (Int32)(actual.ScaleX * Convert.ToInt32(currNode.SelectSingleNode("ItemWidth").Attributes["val"].Value)), (Int32)(actual.ScaleY * Convert.ToInt32(currNode.SelectSingleNode("ItemHeight").Attributes["val"].Value)), HALOThreshold) == false)
                                {
                                    //throw new Exception("Form Alignment is incorrect");
                                    return "Form Alignment is incorrect";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //throw new Exception("Error reading form (unable to determine form number)", e);
                return "Error reading form (unable to determine form number)";
            }

            if (appUtility.formType == "HALO" && formNumber != "000000") 
            { 
                //throw new Exception("Invalid form number"); 
                return "Invalid form number";
            }
            if (appUtility.formType == "PlainPaper" && formNumber == "000000") 
            { 
                //throw new Exception("Invalid form number");
                return "Invalid form number";
            }

            if (appUtility.debug) { _debugOutput += "Form Number: " + formNumber + Environment.NewLine; }

            //try to load the appropriate template document
            try
            {
                docTemplate = new XmlDocument();
                docTemplate.Load(appUtility.templatesFolder + "\\" + formNumber + ".xml");
            }
            catch (Exception e)
            {
                //throw new Exception("Error reading form (unable to load template)", e);
                return "Error reading form (unable to load template)";
            }

            #region template image - draw threshold markers

            if (templateImg != null)
            {
                try
                {
                    drawTemplate(docTemplate.SelectSingleNode("template/itemDef[@name = 'Threshold']"));
                    templateImg.SaveAsJPEG(fileName);
                }
                catch (Exception) { /*supress errors. we don't want this to break the main functionality*/ }
            }

            #endregion


            // Calculate the threshold to be used when scoring the form
            try
            {
                XmlNode currNode = docTemplate.SelectSingleNode("template/itemDef[@name = 'Threshold']");
                Point expected = new Point();
                Point actual = new Point();
                Int32 width;
                Int32 height;
                Array pixels;
                Int32 pixelcount = 0;
                Int32 lowestThreshold = 100;
                Decimal currThreshold = 0;

                for (Int32 i = 0; i < Convert.ToInt32(currNode.SelectSingleNode("ObjColumns").Attributes["val"].Value); i++)
                {
                    for (Int32 j = 0; j < Convert.ToInt32(currNode.SelectSingleNode("ObjRows").Attributes["val"].Value); j++)
                    {
                        for (Int32 k = 0; k < Convert.ToInt32(currNode.SelectSingleNode("ItemCount").Attributes["val"].Value); k++)
                        {
                            pixels = new Int32[0];
                            pixelcount = 0;
                            expected.X = (Int32)(Convert.ToDecimal(currNode.SelectSingleNode("ObjStartX").Attributes["val"].Value) + (i * Convert.ToDecimal(currNode.SelectSingleNode("ObjShiftX").Attributes["val"].Value)) + Convert.ToDecimal(currNode.SelectSingleNode("ItemStartX").Attributes["val"].Value) + (k * Convert.ToDecimal(currNode.SelectSingleNode("ItemShiftX").Attributes["val"].Value)));
                            expected.Y = (Int32)(Convert.ToDecimal(currNode.SelectSingleNode("ObjStartY").Attributes["val"].Value) + (j * Convert.ToDecimal(currNode.SelectSingleNode("ObjShiftY").Attributes["val"].Value)) + Convert.ToDecimal(currNode.SelectSingleNode("ItemStartY").Attributes["val"].Value) + (k * Convert.ToDecimal(currNode.SelectSingleNode("ItemShiftY").Attributes["val"].Value)));
                            actual = getActual(expected);
                            width = (Int32)(actual.ScaleX * Convert.ToInt32(currNode.SelectSingleNode("ItemWidth").Attributes["val"].Value));
                            height = (Int32)(actual.ScaleY * Convert.ToInt32(currNode.SelectSingleNode("ItemHeight").Attributes["val"].Value));
														//axScoreImg.GetPixelArray1D(ref pixels, actual.Y, actual.X, width, height);
														axScoreImg.GetPixelArray1D(ref pixels, actual.X, actual.Y, width, height);

                            for (Int32 l = 0; l < pixels.Length; l++)
                            {
                                if (((Int32[])(pixels))[l] != -1) { pixelcount += 1; }
                            }
                            currThreshold = ((Decimal)(pixelcount) / (width * height)) * 100;
                            if ((Int32)(currThreshold) < lowestThreshold) { lowestThreshold = (Int32)(currThreshold); }
                            pixels = null;
                        }
                    }
                }

                scoreThreshold = lowestThreshold + appUtility.thresholdIncrease;
            }
            catch (Exception e)
            {
                //throw new Exception("Error reading form (unable to calculate scoring threshold)", e);
                return "Error reading form (unable to calculate scoring threshold)";
            }

            if (appUtility.debug) { _debugOutput += "Score Threshold: " + scoreThreshold.ToString() + Environment.NewLine; }

            #region template image - draw the template on the form

            //This section is only used in Debugmode and allows the template that is used to be drawn onto the 
            //    Image file. The resulting file is saved as a jpeg in the same folder.
            if (templateImg != null)
            {
                try
                {
                    //Grab each FormDef Node and Process for results
                    foreach (XmlNode currNode in docTemplate.SelectNodes("template/itemDef"))
                    {
                        drawTemplate(currNode);
                    }
                    templateImg.SaveAsJPEG(fileName);
                }
                catch (Exception) { /*supress errors. we don't want this to break the main functionality*/ }
            }
            #endregion

            // Process image
            try
            {
                //Grab each FormDef Node and Process for results
                foreach (XmlNode currNode in docTemplate.SelectNodes("template/itemDef"))
                {
                    switch (currNode.Attributes["name"].Value)
                    {
                        case ("FormID"):
                            _formID = validateAndConvertFormID(scoreItems(currNode, scoreThreshold).Replace("*", "0"));
                            break;
                        case ("TestID"):
                            _formID = scoreItems(currNode, scoreThreshold, true);
                            break;
                        case "StudentID":
                            _studentID = scoreItems(currNode, scoreThreshold, true);
                            break;
                        case "StudentList":
                            _studentID = scoreItems(currNode, scoreThreshold);
                            break;
                        case "MCQuestions":
                            _MCQuestions = scoreItems(currNode, scoreThreshold);
                            break;
                        case "TestForm":
                            _testForm = scoreItems(currNode, scoreThreshold, true);
                            break;
                        case "MiscData":
                            _miscData = scoreItems(currNode, scoreThreshold, true);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                //throw new Exception(e.Message);
                return e.Message;
            }

            return errorMessage;
        }

        #endregion

        #region Class Functions

        private string findCorners()
        {
            //find the Corners
            try
            {
								// Some scanners allow a black border to appear around the page.
								// First we must get rid of it.
								Int32 wd = axScoreImg.GetWidth();
								Int32 ht = axScoreImg.GetHeight();
								Int32 dw;
								Int32 dh;
								// Crop less if HALO.
								if (appUtility.formType == "HALO")
								{
									dw = (Int32)(wd * 0.01);
									dh = (Int32)(ht * 0.01);
								}
								// Crop 3% from each edge if plain paper.
								else
								{
									dw = (Int32)(wd * 0.03);
									dh = (Int32)(ht * 0.03);
								}
												
								// Top rect.
								axScoreImg.DrawFillRectangle(0, 0, wd, dh, Colors.White, false);
								//Left rect.
								axScoreImg.DrawFillRectangle(0, 0, dw, ht, Colors.White, false);
								// Bottom rect.
								axScoreImg.DrawFillRectangle(0, ht - dh - 1, wd, dh, Colors.White, false);
								// Right rect.
								axScoreImg.DrawFillRectangle(wd - dw - 1, 0, dw, ht, Colors.White, false);
																
                //Start in each corner and work your way in diagonally until you find the corner
                //top left
                for (Int32 i = 10; i < 500; i += appUtility.cornerShift)
                {
                    Int32 y = 10;
                    for (Int32 x = i; x >= 10; x -= appUtility.cornerShift)
                    {
                        //axScoreImg.FiltersToZone(x, y, appUtility.cornerSearchArea, appUtility.cornerSearchArea);
                        if (isFilledIn(x, y, appUtility.cornerSearchArea, appUtility.cornerSearchArea, 80))
                        {
                            topLeft.setVals(x, y);
                            break;
                        }
                        else
                        {
                            y += appUtility.cornerShift;
                        }
                    }
                    if (topLeft.X != 0 && topLeft.Y != 0) { break; }
                }
                //bottom left
                for (Int32 i = 10; i < 500; i += appUtility.cornerShift)
                {
                    Int32 y = (axScoreImg.GetHeight() - 10);
                    for (Int32 x = i; x >= 10; x -= appUtility.cornerShift)
                    {
                        //axScoreImg.FiltersToZone(x, (y - appUtility.cornerSearchArea), appUtility.cornerSearchArea, appUtility.cornerSearchArea);
                        if (isFilledIn(x, (y - appUtility.cornerSearchArea), appUtility.cornerSearchArea, appUtility.cornerSearchArea, 80))
                        {
                            bottomLeft.setVals(x, y);
                            break;
                        }
                        else
                        {
                            y -= appUtility.cornerShift;
                        }
                    }
                    if (bottomLeft.X != 0 && bottomLeft.Y != 0) { break; }
                }

                //If this is a HALO Install we don't need to worry about finding the 
                //    right side corners as there are none. We will calculate them instead
                //    based on a known form width (1570) and the known left side points
                if (appUtility.formType == "HALO")
                {
                    //Calculate the skew angle then calculate the shift x and y 
                    // then apply the shift to the top and bottom points

                    //first calculate the skew angle in terms of the sine and cosine of theta
                    Double r; //length of the side
                    Double sinTheta; //used to calculate the new points
                    Double cosTheta; //used to calculate the new points

                    r = Math.Sqrt(Math.Pow((bottomLeft.X - topLeft.X), 2.0) + Math.Pow((bottomLeft.Y - topLeft.Y), 2.0));
                    sinTheta = ((bottomLeft.X - topLeft.X) / r);
                    cosTheta = ((bottomLeft.Y - topLeft.Y) / r);

                    //OK we now have everything we need to calculate the new point
                    if (sinTheta < 0) //
                    {
                        topRight.setVals(topLeft.X + (Int32)(1570 * cosTheta), topLeft.Y + (Int32)(1570 * Math.Abs(sinTheta)));
                        bottomRight.setVals(bottomLeft.X + (Int32)(1570 * cosTheta), bottomLeft.Y + (Int32)(1570 * Math.Abs(sinTheta)));
                    }
                    else
                    {
                        topRight.setVals(topLeft.X + (Int32)(1570 * cosTheta), topLeft.Y - (Int32)(1568 * Math.Abs(sinTheta)));
                        bottomRight.setVals(bottomLeft.X + (Int32)(1570 * cosTheta), bottomLeft.Y - (Int32)(1570 * Math.Abs(sinTheta)));
                    }
                }
                else
                {
                    //top right
                    for (Int32 i = 10; i < 500; i += appUtility.cornerShift)
                    {
                        Int32 y = 10;
                        for (Int32 x = (axScoreImg.GetWidth() - i); x <= (axScoreImg.GetWidth() - 10); x += appUtility.cornerShift)
                        {
                            //axScoreImg.FiltersToZone((x - appUtility.cornerSearchArea), y, appUtility.cornerSearchArea, appUtility.cornerSearchArea);
                            if (isFilledIn((x - appUtility.cornerSearchArea), y, appUtility.cornerSearchArea, appUtility.cornerSearchArea, 80))
                            {
                                topRight.setVals(x, y);
                                break;
                            }
                            else
                            {
                                y += appUtility.cornerShift;
                            }
                        }
                        if (topRight.X != 0 && topRight.Y != 0) { break; }
                    }
                    //bottom right
                    for (Int32 i = 10; i < 500; i += appUtility.cornerShift)
                    {
                        Int32 y = (axScoreImg.GetHeight() - 10);
                        for (Int32 x = (axScoreImg.GetWidth() - i); x <= (axScoreImg.GetWidth() - 10); x += appUtility.cornerShift)
                        {
                            //axScoreImg.FiltersToZone((x - appUtility.cornerSearchArea), (y - appUtility.cornerSearchArea), appUtility.cornerSearchArea, appUtility.cornerSearchArea);
                            if (isFilledIn((x - appUtility.cornerSearchArea), (y - appUtility.cornerSearchArea), appUtility.cornerSearchArea, appUtility.cornerSearchArea, 80))
                            {
                                bottomRight.setVals(x, y);
                                break;
                            }
                            else
                            {
                                y -= appUtility.cornerShift;
                            }
                        }
                        if (bottomRight.X != 0 && bottomRight.Y != 0) { break; }
                    }
                }

                if ((topLeft.X == 0 && topLeft.Y == 0) || (topRight.X == 0 && topRight.Y == 0) || (bottomLeft.X == 0 && bottomLeft.Y == 0) || (bottomRight.X == 0 && bottomRight.Y == 0))
                {
                    //throw new Exception("Error reading form (corner locations invalid)");
                    return "Error reading form (corner locations invalid)";
                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception e)
            {
                //throw new Exception("Error reading form (unable to find alignment markers)", e);
                return "Error reading form (unable to find alignment markers)";
            }

        }

        private Point getActual(Point expected)
        {
            Point actual = new Point(); //expected.X + topLeft.X, expected.Y + topLeft.Y);
            //return actual;

#if true
            try
            {
							Double x, y, u, v;
							u = (expected.X - topLeftExp.X) / expectedWidth;
							v = (expected.Y - topLeftExp.Y) / expectedHeight;
							x = (1.0 - u) * (1.0 - v) * topLeft.X + u * (1.0 - v) * topRight.X + (1.0 - u) * v * bottomLeft.X + u * v * bottomRight.X;
							y = (1.0 - u) * (1.0 - v) * topLeft.Y + u * (1.0 - v) * topRight.Y + (1.0 - u) * v * bottomLeft.Y + u * v * bottomRight.Y;
							actual.X = (Int32)Math.Round(x);
							actual.Y = (Int32)Math.Round(y);
							actual.ScaleX = (Decimal)(1.2 * actualWidth / expectedWidth);
							actual.ScaleY = (Decimal)(1.2 * actualWidth / expectedHeight);
							return actual;
						}
						catch (Exception e)
						{
							throw new Exception("Actual point calculation error", e);
						}
#else
            try
            {
                Decimal tExpectedWidth = (Decimal)(topRightExp.X) - (Decimal)(topLeftExp.X);
                //Top Scale Factor = (actual width - expected width)/expected width
                Decimal tWidthScaleFactor = ((topRight.X - topLeft.X) - tExpectedWidth) / tExpectedWidth;
                //Top X Shift = top left offset - (expected top left x * scale factor)
                Decimal tShift = (topLeft.X - topLeftExp.X) - (tWidthScaleFactor * topLeftExp.X);

                Decimal bExpectedWidth = (Decimal)(bottomRightExp.X) - (Decimal)(bottomLeftExp.X);
                //Bottom Scale Factor = (actual width - expected width)/expected width
                Decimal bWidthScaleFactor = ((bottomRight.X - bottomLeft.X) - bExpectedWidth) / bExpectedWidth;
                //Bottom X Shift = bottom left offset - (expected bottom left x * scale factor)
                Decimal bShift = (bottomLeft.X - bottomLeftExp.X) - (tWidthScaleFactor * bottomLeftExp.X);

                Decimal lExpectedHeight = (Decimal)(bottomLeftExp.Y) - (Decimal)(topLeftExp.Y);
                //Left Scale Factor = (actual height - expected height)/expected height
                Decimal lHeightScaleFactor = ((bottomLeft.Y - topLeft.Y) - lExpectedHeight) / lExpectedHeight;
                //Left Y Shift = top left offset - (expected top left y * scale factor)
                Decimal lShift = (topLeft.Y - topLeftExp.Y) - (lHeightScaleFactor * topLeftExp.Y);

                Decimal rExpectedHeight = (Decimal)(bottomRightExp.Y) - (Decimal)(topRightExp.Y);
                //Right Scale Factor = (actual height - expected height)/expected height
                Decimal rHeightScaleFactor = ((bottomRight.Y - topRight.Y) - lExpectedHeight) / lExpectedHeight;
                //Right Y Shift = top right offset - (expected top right y * scale factor)
                Decimal rShift = (topRight.Y - topRightExp.Y) - (rHeightScaleFactor * topRightExp.Y);

                //PdistanceFromT		PExpectedY-TLExpectedY
                //PdistanceFromTFactor		PDistanceFromT/LHeightExpected
                Decimal pDistanceFromTFactor = (expected.Y - topLeftExp.Y) / lExpectedHeight;
                //PWidthScaleFactor		(TWidthScaleFactor*(1-PDistanceFromTFactor)+BWidthScaleFactor*(PDistanceFromTFactor))
                Decimal pWidthScaleFactor = (tWidthScaleFactor * (1 - pDistanceFromTFactor)) + (bWidthScaleFactor * pDistanceFromTFactor);
                //PShiftX		(TShiftX*(1-PDistanceFromTFactor)+BShiftX*(PDistanceFromTFactor))
                Decimal pShiftX = (tShift * (1 - pDistanceFromTFactor)) + (bShift * pDistanceFromTFactor);
                //PActualX		(1+PWidthScaleFactor)*PExpectedX+PShiftX
                Decimal pActualX = ((1 + pWidthScaleFactor) * expected.X) + pShiftX;

                //PdistanceFromL		PExpectedx-TLExpectedx
                //PdistanceFromLFactor		PDistanceFromL/TWidthExpected
                Decimal pDistanceFromLFactor = (expected.X - topLeftExp.X) / tExpectedWidth;
                //PHeightScaleFactor		(LHeightScaleFactor*(1-PDistanceFromLFactor)+RHeightScaleFactor*(PDistanceFromLFactor))
                Decimal pHeightScaleFactor = (lHeightScaleFactor * (1 - pDistanceFromLFactor)) + (rHeightScaleFactor * pDistanceFromLFactor);
                //PShiftY		(LShiftY*(1-PDistanceFromLFactor)+RShiftY*(PDistanceFromLFactor))
                Decimal pShiftY = (lShift * (1 - pDistanceFromLFactor)) + (rShift * pDistanceFromLFactor);
                //PActualY		(1+PHeightScaleFactor)*PExpectedY+PShiftY
                Decimal pActualY = ((1 + pHeightScaleFactor) * expected.Y) + pShiftY;

                actual.X = (Int32)(pActualX);
                actual.Y = (Int32)(pActualY);
                actual.ScaleX = (1 + pWidthScaleFactor) * (Decimal)(1.2);
                actual.ScaleY = (1 + pHeightScaleFactor) * (Decimal)(1.2);

                return actual;
            }
            catch (Exception e) { throw new Exception("Actual point calculation error", e); }
#endif
        }

        private String validateAndConvertFormID(String formID)
        {
            String initCheckString = formID.Substring(0, 5);
            String newCheckString = "";
            String initFormID = formID.Substring(8);
            String newFormID = "";
            Int32 oneCounter = 0;
            try
            {
                //we need to parse out the 2 primary sections of the formID and then validate that
                //    the form ID has not been changed

                // the check string is inverted (1's are 0's and vice versa) and reversed we need to fix that
                for (Int32 i = initCheckString.Length - 1; i >= 0; i--)
                {
                    if (initCheckString.Substring(i, 1) == "1") { newCheckString += "0"; }
                    else { newCheckString += "1"; }
                }

                // the formID is read in backwards we need to reverse it and count how many 1's we have while we do it
                for (Int32 i = initFormID.Length - 1; i >= 0; i--)
                {
                    if (initFormID.Substring(i, 1) == "1") { oneCounter += 1; }
                    newFormID += initFormID.Substring(i, 1);
                }

                // now we need to validate everything - check the spacer string and that the 
                //    formID has the corrct number of 1's
                //if (formID.Substring(5, 3) != "010" || oneCounter != Convert.ToUInt32(newCheckString, 2))
                //{
                //    throw new Exception("FormID is invalid");
                //}

                return Convert.ToString(Convert.ToUInt32(newFormID, 2));
            }
            catch (Exception e) { throw new Exception(e.Message); }
        }

        private String scoreItems(XmlNode template)
        {
            return scoreItems(template, 80, false);
        }

        private String scoreItems(XmlNode template, Int32 threshold)
        {
            return scoreItems(template, threshold, false);
        }

        private String scoreItems(XmlNode template, Boolean zeroBased)
        {
            return scoreItems(template, 80, zeroBased);
        }

        private String scoreItems(XmlNode template, Int32 threshold, Boolean zeroBased)
        {
            String result = "";
            String currResult = "";
            Point expected = new Point();
            Point actual = new Point();

#if true

						try
						{
							Int32 objColumns = Convert.ToInt32(template.SelectSingleNode("ObjColumns").Attributes["val"].Value);
							Int32 objRows = Convert.ToInt32(template.SelectSingleNode("ObjRows").Attributes["val"].Value);
							Int32 itemCount = Convert.ToInt32(template.SelectSingleNode("ItemCount").Attributes["val"].Value);
							Decimal objStartX = Convert.ToDecimal(template.SelectSingleNode("ObjStartX").Attributes["val"].Value);
							Decimal objStartY = Convert.ToDecimal(template.SelectSingleNode("ObjStartY").Attributes["val"].Value);
							Decimal objShiftX = Convert.ToDecimal(template.SelectSingleNode("ObjShiftX").Attributes["val"].Value);
							Decimal objShiftY = Convert.ToDecimal(template.SelectSingleNode("ObjShiftY").Attributes["val"].Value);
							Decimal itemStartX = Convert.ToDecimal(template.SelectSingleNode("ItemStartX").Attributes["val"].Value);
							Decimal itemStartY = Convert.ToDecimal(template.SelectSingleNode("ItemStartY").Attributes["val"].Value);
							Decimal itemShiftX = Convert.ToDecimal(template.SelectSingleNode("ItemShiftX").Attributes["val"].Value);
							Decimal itemShiftY = Convert.ToDecimal(template.SelectSingleNode("ItemShiftY").Attributes["val"].Value);
							Int32 itemWidth = Convert.ToInt32(template.SelectSingleNode("ItemWidth").Attributes["val"].Value);
							Int32 itemHeight = Convert.ToInt32(template.SelectSingleNode("ItemHeight").Attributes["val"].Value);
							for (Int32 i = 0; i < objColumns; i++)
							{
								for (Int32 j = 0; j < objRows; j++)
								{
									currResult = "";
									for (Int32 k = 0; k < itemCount; k++)
									{
										expected.X = (Int32)(objStartX + (i * objShiftX) + itemStartX + (k * itemShiftX));
										expected.Y = (Int32)(objStartY + (j * objShiftY) + itemStartY + (k * itemShiftY));
										actual = getActual(expected);

										if (isFilledIn(actual.X, actual.Y, (Int32)(actual.ScaleX * itemWidth), (Int32)(actual.ScaleY * itemHeight), threshold))
										{
											if(currResult == "")
											{
												if (zeroBased)
												 currResult = Convert.ToString(k);
												else
												 currResult = Convert.ToString(k + 1);
											}
											else
											{
												currResult = "#";
											} 
										}
									}
									if (currResult == "")
									  result += "*";
									else
									 result += currResult;
								}
							}

							return result;
						}
						catch (Exception e) { throw new Exception("Scoring Error", e); }
#else
            try
            {
                for (Int32 i = 0; i < Convert.ToInt32(template.SelectSingleNode("ObjColumns").Attributes["val"].Value); i++)
                {
                    for (Int32 j = 0; j < Convert.ToInt32(template.SelectSingleNode("ObjRows").Attributes["val"].Value); j++)
                    {
                        currResult = "";
                        for (Int32 k = 0; k < Convert.ToInt32(template.SelectSingleNode("ItemCount").Attributes["val"].Value); k++)
                        {
                            expected.X = (Int32)(Convert.ToDecimal(template.SelectSingleNode("ObjStartX").Attributes["val"].Value) + (i * Convert.ToDecimal(template.SelectSingleNode("ObjShiftX").Attributes["val"].Value)) + Convert.ToDecimal(template.SelectSingleNode("ItemStartX").Attributes["val"].Value) + (k * Convert.ToDecimal(template.SelectSingleNode("ItemShiftX").Attributes["val"].Value)));
                            expected.Y = (Int32)(Convert.ToDecimal(template.SelectSingleNode("ObjStartY").Attributes["val"].Value) + (j * Convert.ToDecimal(template.SelectSingleNode("ObjShiftY").Attributes["val"].Value)) + Convert.ToDecimal(template.SelectSingleNode("ItemStartY").Attributes["val"].Value) + (k * Convert.ToDecimal(template.SelectSingleNode("ItemShiftY").Attributes["val"].Value)));
                            actual = getActual(expected);
                            //axScoreImg.FiltersToZone(actual.X, actual.Y, (Int32)(actual.ScaleX * Convert.ToInt32(template.SelectSingleNode("ItemWidth").Attributes["val"].Value)), (Int32)(actual.ScaleY * Convert.ToInt32(template.SelectSingleNode("ItemHeight").Attributes["val"].Value)));
														if (isFilledIn(actual.X, actual.Y, (Int32)(actual.ScaleX * Convert.ToInt32(template.SelectSingleNode("ItemWidth").Attributes["val"].Value)), (Int32)(actual.ScaleY * Convert.ToInt32(template.SelectSingleNode("ItemHeight").Attributes["val"].Value)), threshold) && currResult == "")
                            {
                                if (zeroBased) { currResult = Convert.ToString(k); }
                                else { currResult = Convert.ToString(k + 1); }
                            }
                            else if (isFilledIn(actual.X, actual.Y, (Int32)(actual.ScaleX * Convert.ToInt32(template.SelectSingleNode("ItemWidth").Attributes["val"].Value)), (Int32)(actual.ScaleY * Convert.ToInt32(template.SelectSingleNode("ItemHeight").Attributes["val"].Value)), threshold) && currResult != "")
                            {
                                currResult = "#";
                            }
                        }
                        if (currResult == "") { result += "*"; }
                        else { result += currResult; }
                    }
                }

                return result;
            }
            catch (Exception e) { throw new Exception("Scoring Error", e); }
#endif
        }

        private void drawTemplate(XmlNode template)
        {
            Point expected = new Point();
            Point actual;

            try
            {

                for (Int32 i = 0; i < Convert.ToInt32(template.SelectSingleNode("ObjColumns").Attributes["val"].Value); i++)
                {
                    for (Int32 j = 0; j < Convert.ToInt32(template.SelectSingleNode("ObjRows").Attributes["val"].Value); j++)
                    {
                        for (Int32 k = 0; k < Convert.ToInt32(template.SelectSingleNode("ItemCount").Attributes["val"].Value); k++)
                        {
                            expected.X = (Int32)(Convert.ToDecimal(template.SelectSingleNode("ObjStartX").Attributes["val"].Value) + (i * Convert.ToDecimal(template.SelectSingleNode("ObjShiftX").Attributes["val"].Value)) + Convert.ToDecimal(template.SelectSingleNode("ItemStartX").Attributes["val"].Value) + (k * Convert.ToDecimal(template.SelectSingleNode("ItemShiftX").Attributes["val"].Value)));
                            expected.Y = (Int32)(Convert.ToDecimal(template.SelectSingleNode("ObjStartY").Attributes["val"].Value) + (j * Convert.ToDecimal(template.SelectSingleNode("ObjShiftY").Attributes["val"].Value)) + Convert.ToDecimal(template.SelectSingleNode("ItemStartY").Attributes["val"].Value) + (k * Convert.ToDecimal(template.SelectSingleNode("ItemShiftY").Attributes["val"].Value)));
                            actual = getActual(expected);
                            templateImg.DrawFillRectangle(actual.X, actual.Y, (Int32)(actual.ScaleX * Convert.ToDecimal(template.SelectSingleNode("ItemWidth").Attributes["val"].Value)), (Int32)(actual.ScaleX * Convert.ToDecimal(template.SelectSingleNode("ItemHeight").Attributes["val"].Value)), (Colors)templateImg.argb(100, 255, 0, 0), false);
                        }
                    }
                }
            }
            catch (Exception e) { throw new Exception("Draw Template Error", e); }
        }

        private Boolean isFilledIn(Int32 X, Int32 Y, Int32 width, Int32 height, Int32 threshold) 
        {
						//axScoreImg.FiltersToZone(X, Y, width, height); // Depreciated, use SetROI
						axScoreImg.SetROI(X, Y, width, height);
						if (axScoreImg.IsBlank(100 - threshold))
            {
                //templateImg.DrawFillRectangle(X, Y, width, height, Colors.Red, false);
                //templateImg.SaveAsJpeg(appUtility.docFolder + "\\images\\Test.jpg");
                return false;
            }
            else
            {
                //templateImg.DrawFillRectangle(X, Y, width, height, Colors.Green, false);
                //templateImg.SaveAsJpeg(appUtility.docFolder + "\\images\\Test.jpg");
                return true;
            }
        }

        #endregion
    }

    #region Point class
    class Point
    {
        private Int32 _X;
        private Int32 _Y;
        private Decimal _ScaleX;
        private Decimal _ScaleY;

        public Int32 X
        {
            get { return _X; }
            set { _X = value; }
        }
        public Int32 Y
        {
            get { return _Y; }
            set { _Y = value; }
        }
        public Decimal ScaleX
        {
            get { return _ScaleX; }
            set { _ScaleX = value; }
        }
        public Decimal ScaleY
        {
            get { return _ScaleY; }
            set { _ScaleY = value; }
        }
        public Point()
        {
            _X = 0;
            _Y = 0;
            _ScaleX = 1;
            _ScaleY = 1;
        }

        public Point(Int32 X, Int32 Y)
        {
            _X = X;
            _Y = Y;
            _ScaleX = 1;
            _ScaleY = 1;
        }

        public Point(Int32 X, Int32 Y, Decimal ScaleX, Decimal ScaleY)
        {
            _X = X;
            _Y = Y;
            _ScaleX = ScaleX;
            _ScaleY = ScaleY;
        }

        public void Clear()
        {
            _X = 0;
            _Y = 0;
            _ScaleX = 1;
            _ScaleY = 1;
        }

        public void setVals(Int32 X, Int32 Y, Decimal ScaleX, Decimal ScaleY)
        {
            _X = X;
            _Y = Y;
            _ScaleX = ScaleX;
            _ScaleY = ScaleY;
        }

        public void setVals(Int32 X, Int32 Y)
        {
            _X = X;
            _Y = Y;
            _ScaleX = 1;
            _ScaleY = 1;
        }
    }
    #endregion
}
