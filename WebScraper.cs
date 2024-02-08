using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FlightPlanner.Objects;

namespace FlightPlanner {
    internal class WebScraper {
        private RemoteServer currentInst;
        private WarningCarrier warnings;

        public WebScraper(ref RemoteServer server, ref WarningCarrier warningsIn) {
            currentInst = server;
            warnings = warningsIn;
        }

        public List<PlaneObject> GetNewPlanesForSale() {
            // Start by loading all of the aircraft family IDs.
            string currentRegex = "(?<=aircraftsFamily\\?id=)(\\d+)";
            string currentHTML = currentInst.ReadWebsite("app/aircraft/manufacturers");

            MatchCollection regexPlaneFamilyIDs = Regex.Matches(currentHTML, currentRegex);
            List<string> planeFamilyIDs = regexPlaneFamilyIDs.Cast<Match>().Select(x => x.Value).ToList();

            // Iterate through each aircraft type and get IDs for each specific aircraft.
            List<string> planeIDs = new List<string>();

            foreach (string planeFamilyID in planeFamilyIDs) {
                currentRegex = "(?<=aircraftsType\\?id=)(\\d+)";
                currentHTML = currentInst.ReadWebsite("action/enterprise/aircraftsFamily?id=" + planeFamilyID.ToString());

                MatchCollection regexPlaneIDs = Regex.Matches(currentHTML, currentRegex);
                planeIDs.AddRange(regexPlaneFamilyIDs.Cast<Match>().Select(x => x.Value).ToList());
            }

            // Create a new plane object for each unique ID.
            List<PlaneObject> planeObjects = new List<PlaneObject>();

            foreach (string planeID in planeIDs) {
                planeObjects.Add(PlaneObject.CreateFromAircraftTypeHTML(currentInst.ReadWebsite("action/enterprise/aircraftsType?id=" + planeID.ToString()), ref warnings));
            }

            return planeObjects;
        }

        //public string GetPlanesOwned() {


        //}



    }
}
