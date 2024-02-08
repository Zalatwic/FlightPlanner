using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner {
    internal class RemoteServer {
        private string serverName;
        private string serverURL;

        public RemoteServer(string serverNameIn) {
            serverName = serverNameIn;
            serverURL = "https://" + serverNameIn + ".airlinesim.aero/";
        }

        public string ReadWebsite(string requestExtension) {
            /*HttpClient client = new HttpClient();
            string responseHTML = await client.GetStringAsync(serverURL + requestExtension);*/

            Uri uri = new Uri(serverURL + requestExtension);
            HttpWebRequest currentRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
            currentRequest.Method = "GET";
            currentRequest.CookieContainer = new CookieContainer();
            currentRequest.CookieContainer.Add(new Cookie("__as_fc", "", "/", ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("airlinesim-selectedEnterpriseId-junkers", "740", "/", serverName + ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("as-sid", "", "/", ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("_sl_lp", "", "/", ".airlinesim.aero"));
            currentRequest.CookieContainer.Add(new Cookie("JSESSIONID", "", "/", serverName + ".airlinesim.aero"));

            WebResponse currentResponse = currentRequest.GetResponse();
            StreamReader webStream = new StreamReader(currentResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            string responseHTML = webStream.ReadToEnd();

            webStream.Close();
            currentResponse.Close();

            return responseHTML;
        }
    }
}
