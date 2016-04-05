using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Security;
using System.Security.AccessControl;

namespace ElementsPPS
{
    public class commonUtil
    {
        #region Member Variables

        private String _dataPath;
        private String _programPath;
        private String _configFile;
        private String _formType;

        //template file information
        private String _templateChooser;
        private String _templatesFolder;

        //config vars
        private String _versionNum;
        private String _docFolder;
        private Int32 _daysToExpireCsv;
        private Boolean _debug;
        private Int32 _cornerShift;
        private Int32 _cornerSearchArea;
        private Int32 _thresholdIncrease;
        private Int32? _brightness = null;
        private Int32? _contrast = null;

        //application vars
        private String _applicationID;
        private String _installID;
        private String _clientID;
        private Boolean _useHttps;
        private String _currUserId;
        private String _defaultScanner;
        private Boolean _useSSO;

        private String _server;

        //Generic dataset that can be used when needed
        private DataSet _resultSet;

        #endregion

        # region Properties
        public String dataPath
        {
            get { return _dataPath; }
        }

        public String programPath
        {
            get { return _programPath; }
        }

        public String configFile
        {
            get { return _configFile; }
        }

        //template file information
        public String templateChooser
        {
            get { return _templateChooser; }
        }

        public String templatesFolder
        {
            get { return _templatesFolder; }
        }

        //config vars

        public String formType
        {
            get { return _formType; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/formType").Attributes["val"].Value = value;
                    xmlDoc.Save(_configFile);
                    _formType = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public String versionNum
        {
            get { return _versionNum; }
        }

        public String shortVersionNum
        {
            get
            {
                return versionNum.Substring(0, versionNum.LastIndexOf('.'));
            }
        }

        public String docFolder
        {
            get { return _docFolder; }
        }

        public Int32 daysToExpireCsv
        {
            get { return _daysToExpireCsv; }
        }

        public Boolean debug
        {
            get { return _debug; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/debug").Attributes["val"].Value = value.ToString();
                    xmlDoc.Save(_configFile);
                    _debug = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Int32 cornerShift
        {
            get { return _cornerShift; }
        }

        public Int32 cornerSearchArea
        {
            get { return _cornerSearchArea; }
        }

        public Int32 thresholdIncrease
        {
            get { return _thresholdIncrease; }
        }

        public Int32? brightness
        {
            get { return _brightness; }
        }

        public Int32? contrast
        {
            get { return _contrast; }
        }

        //application vars
        public String applicationID
        {
            get { return _applicationID; }
            set { _applicationID = value; }
        }

        public String installID
        {
            get { return _installID; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/installID").Attributes["val"].Value = value;
                    xmlDoc.Save(_configFile);
                    _installID = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }



        public String clientID
        {
            get { return _clientID; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/clientID").Attributes["val"].Value = value;
                    xmlDoc.Save(_configFile);
                    _clientID = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Boolean useSSO
        {
            get { return _useSSO; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/useSSO").Attributes["val"].Value = value.ToString();
                    xmlDoc.Save(_configFile);
                    _useHttps = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public Boolean useHttps
        {
            get { return _useHttps; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/useHttps").Attributes["val"].Value = value.ToString();
                    xmlDoc.Save(_configFile);
                    _useHttps = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public String currUserId
        {
            get { return _currUserId; }
            set { _currUserId = value; }
        }

        // Update Server
        public String server
        {
            get { return _server; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/serverAddress").Attributes["val"].Value = value;
                    xmlDoc.Save(_configFile);
                    _server = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        // Default scanner
        public String defaultScanner
        {
            get { return _defaultScanner; }
            set
            {
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(_configFile);
                    xmlDoc.SelectSingleNode("config/defaultScanner").Attributes["val"].Value = value;
                    xmlDoc.Save(_configFile);
                    _defaultScanner = value;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        //Generic dataset that can be used when needed
        public DataSet resultSet
        {
            get { return _resultSet; }
            set { _resultSet = value; }
        }

        # endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public commonUtil()
        {
            try
            {
                _programPath = Application.StartupPath;
                _dataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\" + Application.CompanyName + "\\" + Application.ProductName;
                _configFile = _dataPath + "\\elementsppsconfig.xml";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(_configFile);
                if (xmlDoc.SelectSingleNode("config/clientID") == null)
                    throw new Exception("System File Corrupted. Please reinstall the software to restore the corrupted system file. Error CSF1");

                // Attempt to merge updates into the configuration file
                // that have been written to a special update file.
                mergeUpdates(xmlDoc);

                _installID = xmlDoc.SelectSingleNode("config/installID").Attributes["val"].Value;
                _clientID = xmlDoc.SelectSingleNode("config/clientID").Attributes["val"].Value;
                _useHttps = Convert.ToBoolean(xmlDoc.SelectSingleNode("config/useHttps").Attributes["val"].Value);

                //Config Vars
                _formType = xmlDoc.SelectSingleNode("config/formType").Attributes["val"].Value;

                String version = Application.ProductVersion;
                _versionNum = version.Substring(0, version.LastIndexOf('.'));

                _docFolder = dataPath;
                _templateChooser = programPath + "\\formNumber.xml";
                _templatesFolder = programPath;
                _daysToExpireCsv = Convert.ToInt32(xmlDoc.SelectSingleNode("config/daysToExpireCsv").Attributes["val"].Value);
                _debug = Convert.ToBoolean(xmlDoc.SelectSingleNode("config/debug").Attributes["val"].Value);
                _cornerShift = Convert.ToInt32(xmlDoc.SelectSingleNode("config/cornerShift").Attributes["val"].Value);
                _cornerSearchArea = Convert.ToInt32(xmlDoc.SelectSingleNode("config/cornerSearchArea").Attributes["val"].Value);
                _thresholdIncrease = Convert.ToInt32(xmlDoc.SelectSingleNode("config/thresholdIncrease").Attributes["val"].Value);
                _defaultScanner = xmlDoc.SelectSingleNode("config/defaultScanner").Attributes["val"].Value.ToString();
                _useSSO = Convert.ToBoolean(xmlDoc.SelectSingleNode("config/useSSO").Attributes["val"].Value);

                XmlNode node = xmlDoc.SelectSingleNode("config/brightness");
                if (node != null)
                    _brightness = Convert.ToInt32(node.Attributes["val"].Value);

                node = xmlDoc.SelectSingleNode("config/contrast");
                if (node != null)
                    _contrast = Convert.ToInt32(node.Attributes["val"].Value);

                //server
                _server = xmlDoc.SelectSingleNode("config/serverAddress").Attributes["val"].Value;

                // Save the doc in case any changes were made.
                xmlDoc.Save(_configFile);
            }
            catch (Exception)
            {
                //throw new Exception(e.Message, e); 
                throw new Exception("System File Corrupted. Please reinstall the software to restore the corrupted system file. Error CSF2");
            }
        }

        #region Methods

        /// <summary>
        /// Attempt to merge updates into the configuration file
        /// that have been written to a special update file.
        /// </summary>
        /// <param name="configDoc"></param>
        private void mergeUpdates(XmlDocument configDoc)
        {
            String updateConfigFile = _dataPath + "\\elementsppsconfig_update.xml";
            if (!File.Exists(updateConfigFile))
                return;

            try
            {
                XmlDocument updateDoc = new XmlDocument();
                updateDoc.Load(updateConfigFile);
                String rootNode = "config";
                // Get the root node of both docs.
                XmlNode configNode = configDoc.SelectSingleNode(rootNode);
                XmlNode updateConfigNode = updateDoc.SelectSingleNode(rootNode);
                // Loop through the updated nodes in the update file.
                foreach (XmlNode updateChild in updateConfigNode.ChildNodes)
                {
                    // We must make a copy of the update node in the context of the configDoc
                    // before it can be put into the configDoc. Do a deep copy.
                    XmlNode copiedUpdateChild = configDoc.ImportNode(updateChild, true);
                    // See if the node exists in the current config file.
                    XmlNode configChild = configNode.SelectSingleNode(updateChild.Name);
                    // Replace it if it does.
                    if (configChild != null)
                        configNode.ReplaceChild(copiedUpdateChild, configChild);
                    // Add a new node to the config doc.
                    // Note: We could add a mechanism to delete nodes also, perhaps by having 0 attributes.
                    else
                        configNode.AppendChild(copiedUpdateChild);
                }
                // Remove the update file so that it won't read it again.
                File.Delete(updateConfigFile);
            }
            catch (Exception)
            {
                throw new Exception("Configuration file could not be updated. Please reinstall the software to restore the corrupted system file. Error XXXX");
            }
        }

        /// <summary>
        /// This method takes a filename and returns the contents of the file as a string
        /// </summary>
        /// <param name="filename">filename to read from</param>
        /// <returns>the contents of the file as a string</returns>
        public String readFile(String filename)
        {
            System.IO.StreamReader sr = new System.IO.StreamReader(filename);
            return sr.ReadToEnd();
        }

        #endregion

        #region Class Functions

        #endregion
    }
}
