using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Xml;
using System.IO;
using System.Security.AccessControl;
using System.Net;


namespace ElementsPPS
{
    [RunInstaller(true)]
    public partial class CustomInstaller : Installer
    {
        public CustomInstaller()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {

            base.Install(stateSaver);

            //set full control rights on the [commonappdata]/Thinkgate folder
            String dataPath = (String)Context.Parameters["datapath"];
            DirectoryInfo di = new DirectoryInfo(dataPath).Parent;
            DirectorySecurity ds = di.GetAccessControl();
            ds.AddAccessRule(new FileSystemAccessRule("Users", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));
            di.SetAccessControl(ds);

            //now we need to update the config file with the appropriate values
            String server = (String)Context.Parameters["server"];
            String clientID = (String)Context.Parameters["clientID"];
            String FormType = "PlainPaper"; //(String)Context.Parameters["formtype"];
            //String useHttps = "1"; //(String)Context.Parameters["useHttps"];

            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(dataPath + "\\elementsppsconfig.xml");

            //set the server access
            //configDoc.SelectSingleNode("config/useHttps").Attributes["val"].Value = (useHttps == "1") ? "True" : "False";

            //set the server address
            configDoc.SelectSingleNode("config/serverAddress").Attributes["val"].Value = server;

            //check the clientID
            WebClient webClient = new WebClient();

            try
            {
                string url = "http://" + server + "/" + clientID + "/TGLogin.aspx";
                String ConnectResponse = webGet(url);
                configDoc.SelectSingleNode("config/clientID").Attributes["val"].Value = clientID;
                configDoc.SelectSingleNode("config/installID").Attributes["val"].Value = DateTime.Now.ToString("ddMMyyyyhhmmssff");
            }
            catch (Exception ex)
            {
                configDoc.SelectSingleNode("config/clientID").Attributes["val"].Value = "";
            }


            configDoc.SelectSingleNode("config/formType").Attributes["val"].Value = FormType;

            configDoc.Save(dataPath + "\\elementsppsconfig.xml");

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
    }
}
