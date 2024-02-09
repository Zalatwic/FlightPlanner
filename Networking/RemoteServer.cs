using System.Net;

namespace FlightPlanner.Networking {
    internal class RemoteServer {

        #region Variables
        
        private string serverName;
        private string serverURL;

        #endregion

        #region Constructor

        public RemoteServer(string serverNameIn) {
            serverName = serverNameIn;
            serverURL = "https://" + serverNameIn + ".airlinesim.aero/";
        }

        #endregion

        #region Functions

        public string ReadWebsite(string requestExtension) {
            Uri uri = new Uri(serverURL + requestExtension);
            HttpWebRequest currentRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            currentRequest.Method = "GET";
            currentRequest.CookieContainer = new CookieContainer();
            currentRequest.CookieContainer.Add(new Cookie("__as_fc", WebCookies.ASFC, "/", ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("airlinesim-selectedEnterpriseId-" + serverName, WebCookies.ENT, "/", serverName + ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("as-sid", WebCookies.SID, "/", ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("_sl_lp", WebCookies.LP, "/", ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("JSESSIONID", WebCookies.JSESSIONID, "/", serverName + ".airlinesim.aero"));

            WebResponse currentResponse = currentRequest.GetResponse();
            StreamReader webStream = new StreamReader(currentResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string responseHTML = webStream.ReadToEnd();

            webStream.Close();
            currentResponse.Close();

            return responseHTML;
        }

        #endregion

    }
}
