using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;

namespace ElementsPPS
{
    public class VersaITCommon
    {
        public static commonUtil appCommonUtil;

        public VersaITCommon()
        {
            appCommonUtil = new commonUtil();
        }

        public VersaITCommon(commonUtil currCommonUtil)
        {
            appCommonUtil = currCommonUtil;
        }

        public string ValidateUser(string userName, bool useSSO = false)
        {
            string webAPI = ConfigurationManager.AppSettings["WebAPI"];
            string URL = (appCommonUtil.useHttps ? "https://" : "http://") + appCommonUtil.server + "/" + webAPI;
            string errorMessage = string.Empty;

            if (!IsWebAccess(URL))
            {
                errorMessage = "Unable to access URL : " + URL;
            }
            else
            {
                URL += "/api/Common/GetUserStatus?userName=" + userName;

                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.Accept] = "application/json";
                    string result = client.DownloadString(URL);

                    List<UserStatus> userStatus = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserStatus>>(result);

                    if (userStatus.Count > 0)
                    {
                        if (!userStatus[0].IsApproved)
                            errorMessage = "This user has not been approved. Please contact system administrator for approval.";
                        else if (userStatus[0].IsLockedOut)
                            errorMessage = "This user has been locked. Please contact system administrator to unlock.";
                        else
                            appCommonUtil.currUserId = userStatus[0].Page.ToString();
                    }
                    else
                    {
                        if (useSSO)
                            errorMessage = "The windows user is not registered.\nPlease contact system administrator!";
                        else
                            errorMessage = "This is not a valid user.\nPlease contact system administrator!";
                    }
                }
            }

            return errorMessage;
        }

        public static bool IsWebAccess(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
    }

    public class UserStatus
    {
        public Nullable<int> Page { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public string Password { get; set; }
    }
}
