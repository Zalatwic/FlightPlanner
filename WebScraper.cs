using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using FlightPlanner.Objects;
using FlightPlanner.Enums;

namespace FlightPlanner {
    internal class WebScraper {
        private RemoteServer currentInst;
        private WarningCarrier warnings;

        #region Constructor

        public WebScraper(ref RemoteServer server, ref WarningCarrier warningsIn) {
            currentInst = server;
            warnings = warningsIn;
        }

        #endregion

        #region Functions

        public Dictionary<string, PlaneObject> GetNewPlanesForSale() {
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
            Dictionary<string, PlaneObject> planeObjects = new Dictionary<string, PlaneObject>();

            foreach (string planeID in planeIDs) {
                planeObjects.Add(planeID, PlaneObject.CreateFromAircraftTypeHTML(currentInst.ReadWebsite("action/enterprise/aircraftsType?id=" + planeID.ToString()), ref warnings));
            }

            return planeObjects;
        }

        public List<SeatConfiguration> GetSeatConfigurations() {
            string currentRegex = Expressions.GetConfigurationInformation;
            string currentHTML = currentInst.ReadWebsite("app/seating");
            List<SeatConfiguration> allConfigurations = new List<SeatConfiguration>();

            MatchCollection regexConfigurationIDs = Regex.Matches(currentHTML, currentRegex);
            List<int> fleetIDs = regexConfigurationIDs.Cast<Match>().Select(x => int.Parse(x.Value)).ToList();

            currentRegex = ""//Expressions.GetConfigurationInformation;
            MatchCollection configurationInfo = Regex.Matches(currentHTML, currentRegex);

            for (int x = 0; x < configurationInfo.Count; x++) {

            }

            // Go into each configuation to read information about it.
            foreach (int configurationID in fleetIDs) {
                SeatConfiguration currentConfiguration = new SeatConfiguration();

                // Build the current ratings.
                currentHTML = currentInst.ReadWebsite("app/seating/editor/" + configurationID.ToString() + "4203?1-1.0-editor~form-editor-deck~ratings~tab-link");
                int currentDistance = 500;

                foreach (RatingsDistance distance in Enum.GetValues(typeof(RatingsDistance))) {
                    currentRegex = String.Format(Expressions.GetConfigurationRating, currentDistance);

                    Match currentRatings = Regex.Match(currentHTML, currentRegex);
                    GroupCollection currentRatingMatches = currentRatings.Groups;

                    ratingObject.ChangeRating(PassengerType.Economy, distance, int.Parse(currentRatingMatches[0].Value));
                    ratingObject.ChangeRating(PassengerType.Business, distance, int.Parse(currentRatingMatches[1].Value));
                    ratingObject.ChangeRating(PassengerType.First, distance, int.Parse(currentRatingMatches[2].Value));
                }
            }
        }

        public List<FleetObject> GetFleetsOwned(List<SeatConfiguration> currentConfigurations) {
            // All fleets must be iterated through.
            string currentRegex = Expressions.GetFleetNames;
            string currentHTML = currentInst.ReadWebsite("app/fleets");
            List<FleetObject> ownedFleets = new List<FleetObject>();
            List<string> fleetNames = new List<string>();
            List<int> fleetIDs = new List<int>();

            try {
                MatchCollection regexFleetNames = Regex.Matches(currentHTML, currentRegex);
                fleetNames = regexFleetNames.Cast<Match>().Select(x => x.Value).ToList();
            }

            catch {
                warnings.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            try {
                currentRegex = Expressions.GetFleetIDs;

                MatchCollection regexFleetIDs = Regex.Matches(currentHTML, currentRegex);
                fleetIDs = regexFleetIDs.Cast<Match>().Select(x => int.Parse(x.Value)).ToList();
            }

            catch {
                warnings.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            if (fleetNames.Count != fleetIDs.Count) {
                warnings.AddWarning("Number of fleet names and fleet IDs in conflict");
            }

            // Operations for each fleet.
            for (int x = 0; x < fleetNames.Count; x++) {
                // Create new fleet object.
                FleetObject currentFleet = new FleetObject(fleetIDs[x], fleetNames[x]);

                // Get the aircraft information from the website, crossreference the object dictionary as needed.
                currentRegex = Expressions.GetAircraftFromFleet;
                currentHTML = currentInst.ReadWebsite("app/fleets/" + fleetIDs[x].ToString());

                MatchCollection currentPlanes = Regex.Matches(currentHTML, currentRegex);

                foreach (Match plane in currentPlanes) {
                    GroupCollection currentPlaneMatches = plane.Groups;

                    currentFleet.AddAircraft(new OwnedAircraft(
                        int.Parse(currentPlaneMatches[0].Value),
                        currentPlaneMatches[1].Value,
                        currentPlaneMatches[2].Value,
                        int.Parse(currentPlaneMatches[3].Value),
                        currentPlaneMatches[4].Value,
                        currentPlaneMatches[5].Value,
                        double.Parse(currentPlaneMatches[6].Value),
                        double.Parse(currentPlaneMatches[7].Value),
                        double.Parse(currentPlaneMatches[8].Value),
                        Manwell.FindSeatConfiguration(currentConfigurations, int.Parse(currentPlaneMatches[3].Value), int.Parse(currentPlaneMatches[10].Value), int.Parse(currentPlaneMatches[11].Value), int.Parse(currentPlaneMatches[12].Value))
                        ));
                }

                ownedFleets.Add(currentFleet);
            }

            return ownedFleets;
        }

        #endregion

    }
}
