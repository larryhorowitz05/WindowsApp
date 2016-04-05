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
    public partial class frmScorePreview : Form
    {
        private commonUtil appCommonUtil;
        public List<AssessmentItem> TableAssessment { get; set; }
        public DataTable StudentScore { get; set; }
        public int FormDefID { get; set; }
        string[] _ansOption = { "", "A", "B", "C", "D", "E" };

        int AssessmentItemCount = 0;

        public frmScorePreview()
        {
            appCommonUtil = new commonUtil();
            InitializeComponent();
        }

        public frmScorePreview(commonUtil currCommonUtil)
        {
            appCommonUtil = currCommonUtil;
            InitializeComponent();
        }

        public List<AssessmentItem> GetBubbleSheetItems(int FormDefID)
        {
            try
            {
                string protocolName = appCommonUtil.useHttps ? "https://" : "http://";
                string webAPI = ConfigurationManager.AppSettings["WebAPI"];
                string webAddress = protocolName + appCommonUtil.server + "/" + webAPI + "/api/AssessmentType?id=" + FormDefID;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string result = client.DownloadString(webAddress);
                    List<AssessmentItem> assessmentItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AssessmentItem>>(result);

                    AssessmentItemCount = assessmentItems.Count;

                    return assessmentItems;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        private string CheckQuestionType(int QuetionNo)
        {
            if (QuetionNo <= TableAssessment.Count)
            {
                if (TableAssessment[QuetionNo - 1].QuestionType.Trim() == "E" || TableAssessment[QuetionNo - 1].QuestionType.Trim() == "S")
                    return TableAssessment[QuetionNo - 1].QuestionType.Trim();
            }
            return string.Empty;
        }

        private void frmScorePreview_Load(object sender, EventArgs e)
        {
            Dictionary<int, string> ShortAndEssayTypeItems = new Dictionary<int, string>();

            string EssayOrShortQuestionType = string.Empty;
            DataTable dtTemp = StudentScore;

            //implementation with 5set of Question/Answer per row in grid
            #region fiveSetPerRowImplementation
            //int _lastRowUpdated = 0;
            //int _lastColUpdated = 0;
            //int _key = 0;
            //int _getKey = 0;
            //Boolean isTeacherRowSkipped = true;
            //for (_lastRowUpdated = 0; _lastRowUpdated < dtTemp.Rows.Count && _lastRowUpdated < TableAssessment.Count; _lastRowUpdated++)
            //{
            //    for (_lastColUpdated = 0; _lastColUpdated < dtTemp.Columns.Count; _lastColUpdated += 2)
            //    {
            //        EssayOrShortQuestionType = CheckQuestionType(Convert.ToInt32(dtTemp.Rows[_lastRowUpdated][_lastColUpdated]));
            //        if (EssayOrShortQuestionType == "E")
            //        {
            //            StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Essay";
            //            ShortAndEssayTypeItems.Add(_key, EssayOrShortQuestionType);
            //            _key++;
            //        }
            //        else if (EssayOrShortQuestionType == "S")
            //        {
            //            StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Short Ans.";
            //            ShortAndEssayTypeItems.Add(_key, EssayOrShortQuestionType);
            //            _key++;
            //        }
            //        //To check Question type Essay and Short Ans types, which as answered by student or not.
            //        if (Convert.ToInt16(StudentScore.Rows[_lastRowUpdated][_lastColUpdated]) > TableAssessment.Count)
            //        {
            //            if (_getKey < ShortAndEssayTypeItems.Count && !isTeacherRowSkipped)
            //            {
            //                if (ShortAndEssayTypeItems[_getKey].ToString().Trim() == "E")
            //                {
            //                    if (StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1].ToString().Length > 0)
            //                        StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Essay(Answered)";
            //                    else
            //                        StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Essay(UnAnswered)";
            //                    _getKey++;
            //                }
            //                else if (ShortAndEssayTypeItems[_getKey].ToString().Trim() == "S")
            //                {
            //                    if (StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1].ToString().Length > 0)
            //                        StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Short(Answered)";
            //                    else
            //                        StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Short(UnAnswered)";
            //                    _getKey++;
            //                }
            //            }
            //            isTeacherRowSkipped = false;
            //        }
            //    }
            //}
            #endregion
            List<ShortEssayAnswered> shortEaasyAnsweredTypes = new List<ShortEssayAnswered>();
            ShortEssayAnswered seAnsweredType;
            int _lastRowUpdated = 0;
            int _lastColUpdated = 0;
            int _key = 0;
            int _getKey = 0;
            Boolean isTeacherRowSkipped = true;
            string _option = string.Empty;
            int _questionIndex;
            int _ansCol = 1;
            for (_lastRowUpdated = 0; _lastRowUpdated < dtTemp.Rows.Count; _lastRowUpdated++)
            {
                for (_lastColUpdated = 0; _lastColUpdated < dtTemp.Columns.Count; _lastColUpdated += 2)
                {
                    EssayOrShortQuestionType = CheckQuestionType(Convert.ToInt32(dtTemp.Rows[_lastRowUpdated][_lastColUpdated]));

                    Boolean isNum = int.TryParse(dtTemp.Rows[_lastRowUpdated][1].ToString(), out _questionIndex);
                    if (_questionIndex > 0)
                    {
                        StudentScore.Rows[_lastRowUpdated][_ansCol] = _ansOption[_questionIndex];
                    }
                    if (EssayOrShortQuestionType == "E")
                    {
                        StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Essay";
                        ShortAndEssayTypeItems.Add(_key, EssayOrShortQuestionType);

                        seAnsweredType = new ShortEssayAnswered();
                        seAnsweredType.Row = _lastRowUpdated;
                        seAnsweredType.Col = _lastColUpdated + 1;
                        seAnsweredType.QuestionType = EssayOrShortQuestionType;
                        shortEaasyAnsweredTypes.Add(seAnsweredType);

                        _key++;
                    }
                    else if (EssayOrShortQuestionType == "S")
                    {
                        StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Short Ans.";
                        ShortAndEssayTypeItems.Add(_key, EssayOrShortQuestionType);

                        seAnsweredType = new ShortEssayAnswered();
                        seAnsweredType.Row = _lastRowUpdated;
                        seAnsweredType.Col = _lastColUpdated + 1;
                        seAnsweredType.QuestionType = EssayOrShortQuestionType;
                        shortEaasyAnsweredTypes.Add(seAnsweredType);

                        _key++;
                    }
                    //To check Question type Essay and Short Ans types, which is answered by student or not.
                    if (Convert.ToInt16(StudentScore.Rows[_lastRowUpdated][_lastColUpdated]) > TableAssessment.Count)
                    {
                        if (_getKey < ShortAndEssayTypeItems.Count && !isTeacherRowSkipped)
                        {
                            if (ShortAndEssayTypeItems[_getKey].ToString().Trim() == "E")
                            {
                                seAnsweredType = shortEaasyAnsweredTypes.Find(p => p.QuestionType == "E");

                                if (StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1].ToString().Length > 0)
                                    StudentScore.Rows[seAnsweredType.Row][seAnsweredType.Col] = "Essay (Answered)";
                                //StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Essay(Answered)";
                                else
                                    StudentScore.Rows[seAnsweredType.Row][seAnsweredType.Col] = "Essay (UnAnswered)";
                                //  StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Essay(UnAnswered)";
                                shortEaasyAnsweredTypes.Remove(seAnsweredType);
                                _getKey++;
                            }
                            else if (ShortAndEssayTypeItems[_getKey].ToString().Trim() == "S")
                            {
                                seAnsweredType = shortEaasyAnsweredTypes.Find(p => p.QuestionType == "S");
                                if (StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1].ToString().Length > 0)
                                    StudentScore.Rows[seAnsweredType.Row][seAnsweredType.Col] = "Short (Answered)";
                                //StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Short(Answered)";
                                else
                                    StudentScore.Rows[seAnsweredType.Row][seAnsweredType.Col] = "Short (UnAnswered)";
                                //StudentScore.Rows[_lastRowUpdated][_lastColUpdated + 1] = "Short(UnAnswered)";
                                shortEaasyAnsweredTypes.Remove(seAnsweredType);
                                _getKey++;
                            }
                        }
                        isTeacherRowSkipped = false;
                    }
                }
            }

            //Delete extra rows from result/score
            int totlaRows = StudentScore.Rows.Count;
            for (int index = totlaRows - 1; index >= AssessmentItemCount; index--)
            {
                StudentScore.Rows[index].Delete();
            }

            grdScorePreview.ReadOnly = true;
            grdScorePreview.AllowUserToAddRows = false;
            grdScorePreview.DataSource = StudentScore;

            //grdScorePreview.DefaultCellStyle.Font = new Font(FontFamily.GenericSerif, 12, FontStyle.Bold);
            //Header row texts
            for (int i = 0; i < grdScorePreview.Columns.Count; i++)
            {
                grdScorePreview.Columns[i].Width = 110;
                grdScorePreview.Columns[i].HeaderText = i % 2 == 0 ? "Item No." : "Student Response";
                grdScorePreview.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdScorePreview.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdScorePreview.Columns[i].HeaderCell.Style.Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold, GraphicsUnit.Pixel);
            }
            //Grid rows and values
            for (int i = 0; i < grdScorePreview.Rows.Count; i++)
            {
                for (int j = 0; j < grdScorePreview.Columns.Count; j++)
                {
                    if (j % 2 == 0)
                    {
                        grdScorePreview.Rows[i].Cells[j].Style.BackColor = Color.FromArgb(2, 171, 238);
                    }
                    else
                    {
                        grdScorePreview.Rows[i].Cells[j].Style.BackColor = Color.White;
                    }
                }
            }
            //Header text forecolor/backcolr formatting
            int alterNateIndex = 0;
            foreach (DataGridViewColumn col in grdScorePreview.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular, GraphicsUnit.Pixel);
                //if (alterNateIndex % 2 == 0)
                //{
                col.HeaderCell.Style.BackColor = Color.FromArgb(51, 103, 153);
                col.HeaderCell.Style.ForeColor = Color.White;
                //}
                alterNateIndex++;
            }

        }
    }

    public class AssessmentItem
    {
        public int ID { get; set; }
        public int Sort { get; set; }
        public string QuestionType { get; set; }
    }
    public class ShortEssayAnswered
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public string QuestionType { get; set; }
    }
}
