using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ElementsPPS
{
    public partial class VersaITInputBox : Form
    {
        #region Member Variables

        private String _returnVal;
        private Boolean _userAction = false;

        #endregion

        #region Properties

        public String returnVal
        {
            get { return _returnVal; }
        }

        #endregion

        /// <summary>
        /// Constructor. Do not use to create an instance of this object use VersaITInputBox.ShowInputBox()
        /// </summary>
        public VersaITInputBox()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _returnVal = txtInput.Text;
            _userAction = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _returnVal = "CanceledByUser";
            _userAction = true;
            this.Close();
        }

        private void VersaITInputBox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_userAction == false)
                _returnVal = "CanceledByUser";
        }

        #region Methods
        /// <summary>
        /// Creates the intputbox, shows it as a dialog and returns the entered text 
        /// </summary>
        /// <param name="caption">The caption that appears in the input box</param>
        /// <returns>The text entered into the input box or "CanceledByUser" if the box is canceled or closed</returns>
        public static string ShowInputBox(String caption)
        {
            VersaITInputBox box = new VersaITInputBox();
            box.lblCaption.Text = caption;
            box.Text = "";
            box.ShowDialog();
            return box.returnVal;
        }

        /// <summary>
        /// Creates the intputbox, shows it as a dialog and returns the entered text 
        /// </summary>
        /// <param name="caption">The caption that appears in the input box</param>
        /// <param name="title">The title that of the input box</param>
        /// <returns>The text entered into the input box or "CanceledByUser" if the box is canceled or closed</returns>
        public static string ShowInputBox(String caption, String title)
        {
            VersaITInputBox box = new VersaITInputBox();
            box.lblCaption.Text = caption;
            box.Text = title;
            box.ShowDialog();
            return box.returnVal;
        }

        /// <summary>
        /// Creates the intputbox, shows it as a dialog and returns the entered text 
        /// </summary>
        /// <param name="caption">The caption that appears in the input box</param>
        /// <param name="title">The title that of the input box</param>
        /// <param name="width">The width to use in case the caption overruns the default space</param>
        /// <param name="height">The height to use in case the caption overruns the default space</param>
        /// <returns>The text entered into the input box or "CanceledByUser" if the box is canceled or closed</returns>
        public static string ShowInputBox(String caption, String title, Int32 width, Int32 height)
        {
            VersaITInputBox box = new VersaITInputBox();
            box.lblCaption.Text = caption;
            box.Text = title;
            box.Height = height;
            box.Width = width;
            box.ShowDialog();
            return box.returnVal;
        }

        /// <summary>
        /// Creates the intputbox, shows it as a dialog and returns the entered text 
        /// </summary>
        /// <param name="caption">The caption that appears in the input box</param>
        /// <param name="title">The title that of the input box</param>
        /// <param name="startingVal">The value that the input box should start with</param>
        /// <returns>The text entered into the input box or "CanceledByUser" if the box is canceled or closed</returns>
        public static string ShowInputBox(String caption, String title, String startingVal)
        {
            VersaITInputBox box = new VersaITInputBox();
            box.lblCaption.Text = caption;
            box.Text = title;
            box.txtInput.Text = startingVal;
            box.ShowDialog();
            return box.returnVal;
        }

        /// <summary>
        /// Creates the intputbox, shows it as a dialog and returns the entered text 
        /// </summary>
        /// <param name="caption">The caption that appears in the input box</param>
        /// <param name="title">The title that of the input box</param>
        /// <param name="startingVal">The value that the input box should start with</param>
        /// <param name="width">The width to use in case the caption overruns the default space</param>
        /// <param name="height">The height to use in case the caption overruns the default space</param>
        /// <returns>The text entered into the input box or "CanceledByUser" if the box is canceled or closed</returns>
        public static string ShowInputBox(String caption, String title, String startingVal, Int32 width, Int32 height)
        {
            VersaITInputBox box = new VersaITInputBox();
            box.lblCaption.Text = caption;
            box.Text = title;
            box.txtInput.Text = startingVal;
            box.Height = height;
            box.Width = width;
            box.ShowDialog();
            return box.returnVal;
        }

        #endregion

        #region Class Functions

        #endregion
    }
}