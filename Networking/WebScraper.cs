using System.Text.RegularExpressions;
using Spectre.Console;
using FlightPlanner.Objects;
using FlightPlanner.Enums;

namespace FlightPlanner.Networking {
    internal class WebScraper {

        #region Variables

        private RemoteServer currentInst;
        private WarningCarrier warnings;

        #endregion

        #region Constructor

        public WebScraper(ref RemoteServer server, ref WarningCarrier warningsIn) {
            currentInst = server;
            warnings = warningsIn;
        }

        #endregion

        #region Functions

        public Dictionary<int, PlaneObject> GetNewPlanesForSale() {
            // Start by loading all of the aircraft family IDs.
            string currentRegex = "(?<=aircraftsFamily\\?id=)(\\d+)";
            string currentHTML = currentInst.ReadWebsite("app/aircraft/manufacturers");

            MatchCollection regexPlaneFamilyIDs = Regex.Matches(currentHTML, currentRegex);
            List<string> planeFamilyIDs = regexPlaneFamilyIDs.Cast<Match>().Select(x => x.Value).ToList();

            // Iterate through each aircraft type and get IDs for each specific aircraft.
            List<string> planeIDs = new List<string>();

            AnsiConsole.Progress().Start(ctx => {
                // Define tasks
                var task = ctx.AddTask("[GREEN]GETTING PLANE IDS[/]");
                double inc = 100 / planeFamilyIDs.Count();

                foreach (string planeFamilyID in planeFamilyIDs) {
                    currentRegex = "(?<=aircraftsType\\?id=)(\\d+)";
                    currentHTML = currentInst.ReadWebsite("action/enterprise/aircraftsFamily?id=" + planeFamilyID.ToString());

                    MatchCollection regexPlaneIDs = Regex.Matches(currentHTML, currentRegex);
                    planeIDs.AddRange(regexPlaneIDs.Cast<Match>().Select(x => x.Value).ToList());

                    task.Increment(inc);
                }

                task.StopTask();
            });

            // Create a new plane object for each unique ID.
            Dictionary<int, PlaneObject> planeObjects = new Dictionary<int, PlaneObject>();

            AnsiConsole.Progress().Start(ctx => {
                // Define tasks
                var task = ctx.AddTask("[BLUE]GETTING PLANE INFORMATION[/]");
                double inc = 100 / planeIDs.Count();

                foreach (string planeID in planeIDs) {
                    planeObjects.Add(int.Parse(planeID), PlaneObject.CreateFromAircraftTypeHTML(currentInst.ReadWebsite("action/enterprise/aircraftsType?id=" + planeID.ToString()), ref warnings));
                    task.Increment(inc);
                }

                task.StopTask();
            });

            return planeObjects;
        }

        // 0 NAME           3 FRST #\SEAT   6 BUSI FA
        // 1 ECON #\SEAT    4 #FA           7 FRST FA
        // 2 BUSI #\SEAT    5 #ECON FA      9 ID
        //
        public List<SeatConfiguration> GetSeatConfigurations() {
            string currentRegex = Expressions.GetConfigurationInformation;
            string currentHTML = currentInst.ReadWebsite("app/seating");
            List<SeatConfiguration> allConfigurations = new List<SeatConfiguration>();

            MatchCollection regexConfigurationIDs = Regex.Matches(currentHTML, currentRegex);
            List<int> fleetIDs = regexConfigurationIDs.Cast<Match>().Select(x => int.Parse(x.Value)).ToList();

            currentRegex = Expressions.GetConfigurationInformation;
            MatchCollection configurationInfo = Regex.Matches(currentHTML, currentRegex);

            // Go into each configuation to read information about it.
            foreach (Match currentMatch in configurationInfo) {
                GroupCollection currentGroup = currentMatch.Groups;
                SeatConfiguration currentConfiguration = new SeatConfiguration(currentGroup[0].Value, int.Parse(currentGroup[9].Value));

                // Add the classes.
                foreach (PassengerType type in Enum.GetValues(typeof(PassengerType))) {
                    string[] currentValue = currentGroup[(int)type].Value.Split("<br/>");

                    // String split successfully.
                    if (currentGroup[(int)type].Value != currentValue[0]) {
                        CabinObject currentCabin = new CabinObject(
                            type,
                            (SeatType)Enum.Parse(typeof(SeatType), currentValue[1].Replace("-", "").Replace(" ", "")),
                            int.Parse(currentValue[0]),
                            int.Parse(currentGroup[(int)type + 4].Value));
                    }
                }

                // Build the current ratings, currentGroup[9] is the configuration ID.
                currentHTML = currentInst.ReadWebsite("app/seating/editor/" + currentGroup[9].Value + "4203?1-1.0-editor~form-editor-deck~ratings~tab-link");
                int currentDistance = 500;

                foreach (RatingsDistance distance in Enum.GetValues(typeof(RatingsDistance))) {
                    currentRegex = String.Format(Expressions.GetConfigurationRating, currentDistance);

                    Match currentRatings = Regex.Match(currentHTML, currentRegex);
                    GroupCollection currentRatingMatches = currentRatings.Groups;

                    currentConfiguration.ChangeRating(PassengerType.Economy, distance, int.Parse(currentRatingMatches[0].Value));
                    currentConfiguration.ChangeRating(PassengerType.Business, distance, int.Parse(currentRatingMatches[1].Value));
                    currentConfiguration.ChangeRating(PassengerType.First, distance, int.Parse(currentRatingMatches[2].Value));
                }

                // Get the ID of the current plane type.
                currentRegex = String.Format(Expressions.GetConfigurationRating, currentDistance);
                currentConfiguration.SetAircraftModel(int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value));

                allConfigurations.Add(currentConfiguration);
            }

            return allConfigurations;
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
