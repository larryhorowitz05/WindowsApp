using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Text;
using System.IO;
using System.Threading;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Net;

namespace ElementsPPS
{
    static class Program
    {
        //[System.Runtime.InteropServices.DllImport("advapi32.dll")]
        //public static extern bool LogonUser(string userName, string domainName, string password, int LogonType, int LogonProvider, ref IntPtr phToken);

        static commonUtil appCommonUtil;

        private static string GetloggedinUserName()
        {
            System.Security.Principal.WindowsIdentity currentUser = System.Security.Principal.WindowsIdentity.GetCurrent();
            string userName = currentUser.Name;

            if (userName.IndexOf('\\') > 0)
                return userName.Substring(userName.IndexOf('\\') + 1);
            else
                return userName;
        }

        private static void SSOLogin()
        {
            string userName = GetloggedinUserName();
            string configDomainName = ConfigurationManager.AppSettings["DomainName"];
            string currentDomainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            bool isDomainAccess = false;

#if DEBUG
            if (configDomainName == "FLMiamiDade")
            {
                isDomainAccess = true;
                userName = "tgtmanager";
            }
#else
            if (configDomainName == currentDomainName)
            {
                isDomainAccess = true;
            }
#endif

            if (isDomainAccess)
            {
                VersaITCommon commonClass = new VersaITCommon(appCommonUtil);
                string message = commonClass.ValidateUser(userName, true);

                if (!string.IsNullOrEmpty(message))
                {
                    MessageBox.Show(message, "Alert");
                    Application.Exit();
                }
                else
                {
                    PPS main = new PPS(appCommonUtil);
                    main.Show();
                    Application.Run();
                }
            }
            else
            {
                MessageBox.Show("You are not logged on to the correct domain. Please contact your system administrator.");
                Application.Exit();
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string[] args = Environment.GetCommandLineArgs();
            try
            {
                appCommonUtil = new commonUtil();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //First we need to make sure we're only running 1 instance of the application
            String currProcessName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
            String currApplication = System.IO.Path.GetFileNameWithoutExtension(currProcessName);
            Boolean running = true;

            Mutex mutex = new Mutex(true, currApplication, out running);
            if (!running)
            {
                System.Windows.Forms.Application.Exit();
            }

            if (appCommonUtil.useSSO)
                SSOLogin();
            else
            {
                frmConnect login = new frmConnect(appCommonUtil);
                login.Show();
                Application.Run();
            }
        }
    }
}