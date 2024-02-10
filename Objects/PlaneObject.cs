using System.Text.RegularExpressions;

namespace FlightPlanner.Objects {

    [Serializable]
    internal class PlaneObject {

        #region Constructor

        private PlaneObject(
            int priceIn,
            string letterCodeIn,
            string manufacturerIn,
            string manufacturingLocationIn,
            string productionPeriodIn,
            string productionRateIn,
            int quantityProducedIn,
            double popularityIn,
            string typeRatingIn,
            int cockpitCrewIn,
            int maxPayloadIn,
            int maxPaxIn,
            int maxCargoIn,
            double maxCargoSpaceIn,
            int bulkCargoIn,
            double bulkCargoSpaceIn,
            int MTOWIn,
            string rangeIn,
            int speedIn,
            string rollTakeoffIn,
            string rollLandingIn,
            string noiseCategoryIn,
            string maintenanceCategoryIn,
            bool[][] routeRestrictionsIn) {

            Price = priceIn;
            LetterCode = letterCodeIn;
            Manufacturer = manufacturerIn;
            ManufacturingLocation = manufacturingLocationIn;
            ProductionPeriod = productionPeriodIn;
            ProductionRate = productionRateIn;
            QuantityProduced = quantityProducedIn;
            Popularity = popularityIn;
            TypeRating = typeRatingIn;
            CockpitCrew = cockpitCrewIn;
            MaxPayload = maxPayloadIn;
            MaxPax = maxPaxIn;
            MaxCargo = maxCargoIn;
            MaxCargoSpace = maxCargoSpaceIn;
            BulkCargo = bulkCargoIn;
            BulkCargoSpace = bulkCargoSpaceIn;
            MTOW = MTOWIn;
            Range = rangeIn;
            Speed = speedIn;
            RollTakeoff = rollTakeoffIn;
            RollLanding = rollLandingIn;
            NoiseCategory = noiseCategoryIn;
            MaintenanceCategory = maintenanceCategoryIn;
            RouteRestrictions = routeRestrictionsIn;
        }

        #endregion

        #region Properties

        public int Price { 
            get;
            private set;
        }

        public string LetterCode {
            get;
            private set;
        }

        public string Manufacturer {
            get;
            private set;
        }

        public string ManufacturingLocation {
            get;
            private set;
        }

        public string ProductionPeriod {
            get;
            private set;
        }

        public string ProductionRate {
            get;
            private set;
        }

        public int QuantityProduced {
            get;
            private set;
        }

        public double Popularity {
            get;
            private set;
        }

        public string TypeRating {
            get;
            private set;
        }

        public int CockpitCrew {
            get;
            private set;
        }

        public int MaxPayload {
            get;
            private set;
        }

        public int MaxPax {
            get;
            private set;
        }

        public int MaxCargo {
            get;
            private set;
        }

        public double MaxCargoSpace {
            get;
            private set;
        }

        public int BulkCargo {
            get;
            private set;
        }

        public double BulkCargoSpace {
            get;
            private set;
        }

        public int MTOW {
            get;
            private set;
        }

        public string Range {
            get;
            private set;
        }

        public int Speed {
            get;
            private set;
        }

        public string RollTakeoff {
            get;
            private set;
        }

        public string RollLanding {
            get;
            private set;
        }

        public string NoiseCategory {
            get;
            private set;
        }

        public string MaintenanceCategory {
            get;
            private set;
        }

        public bool[][] RouteRestrictions {
            get;
            private set;
        }

        #endregion

        #region Functions

        public static PlaneObject CreateFromAircraftTypeHTML(string currentHTML, ref WarningCarrier warningInstance) {
            string currentRegex = "";
            int price = 0;
            string letterCode = "FAIL";
            string manufacturer = "FAIL";
            string manufacturingLocation = "FAIL";
            string productionPeriod = "FAIL";
            string productionRate = "FAIL";
            int quantityProduced = 0;
            double popularity = 0.0;
            string typeRating = "FAIL";
            int cockpitCrew = 0;
            int maxPayload = 0;
            int maxPax = 0;
            int maxCargo = 0;
            double maxCargoSpace = 0;
            int bulkCargo = 0;
            double bulkCargoSpace = 0;
            int MTOW = 0;
            string range = "FAIL";
            int speed = 0;
            string rollTakeoff = "0";
            string rollLanding = "0";
            string noiseCategory = "FAIL";
            string maintenanceCategory = "FAIL";
            bool[][] routeRestrictions = new bool[11][];

            for (int x = 0; x < 11; x++) {
                routeRestrictions[x] = new bool[x];
            }

            // Extract Price
            try {
                currentRegex = "(?<=<td>List price<\\/td>\\s*<td class=\"text-right\">\\s*)(.*)(?= AS\\$)";
                price = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value.Trim(), System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Letter Code
            try {
                currentRegex = "(?<=<td>3 letter code<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                letterCode = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Manufacturer
            try {
                currentRegex = "(?<=<td>Manufacturer<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                manufacturer = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Manufacturing Location
            try {
                currentRegex = "(?<=<td>Manufacturing location<\\/td>\\s*<td class=\"text-right\">\\s*<a href=\".*</a> \\()(.{3})";
                manufacturingLocation = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Production Period
            try {
                currentRegex = "(?<=<td>Production period<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S.*)";
                productionPeriod = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Production Rate
            try {
                currentRegex = "(?<=<td>Production rate<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S.*)";
                productionRate = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Quantity Produced
            try {
                currentRegex = "(?<=<td>Quantity produced<\\/td>\\s*<td class=\"text-right\">\\s*.)(\\S+)";
                quantityProduced = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Popularity
            try {
                currentRegex = "(?<=<td>Popularity with passengers<\\/td>\\s*<td class=\"text-right\">\\s*<img src=\"../../assets/img/rating/)(\\d*)";
                popularity = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Type Rating
            try {
                currentRegex = "(?<=<td>Type rating<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                typeRating = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Cockpit Crew
            try {
                currentRegex = "(?<=<td>Cockpit crew<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                cockpitCrew = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }


            // Extract Max Payload
            try {
                currentRegex = "(?<=<td>MPL \\(Maximum Payload\\)<\\/td>\\s*<td class=\"text-right\" colspan=\"2\">\\s*)(\\S+)";
                maxPayload = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Max Passengers
            try {
                currentRegex = "(?<=<td>Passengers \\(max\\)<\\/td>\\s*<td class=\"text-right\" colspan=\"2\">\\s*\\D*)(\\d+)";
                maxPax = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Max Cargo (KG)
            try {
                currentRegex = "(?<=<td>Total Cargo Capacity<\\/td>\\s*<td class=\"text-right\">\\s*[0123456789\\.]* m&sup3;\\s*</td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                maxCargo = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Max Cargo (SQM)
            try {
                currentRegex = "(?<=<td>Total Cargo Capacity<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                maxCargoSpace = double.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Bulk Cargo (KG)
            try {
                currentRegex = "(?<=<td>Bulk Cargo<\\/td>\\s*<td class=\"text-right\">\\s*[0123456789\\.]* m&sup3;\\s*</td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                bulkCargo = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Bulk Cargo (SQM)
            try {
                currentRegex = "(?<=<td>Bulk Cargo<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                bulkCargoSpace = double.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Max Takeoff Weight (KG)
            try {
                currentRegex = "(?<=<td>MTOW<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                MTOW = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Range (KM)
            try {
                currentRegex = "(?<=<td>Range<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                range = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Speed (KMPH)
            try {
                currentRegex = "(?<=<td>Speed<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                speed = int.Parse(Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value, System.Globalization.NumberStyles.AllowThousands);
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Takeoff Roll (M)
            try {
                currentRegex = "(?<=<td>Ground roll takeoff<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                rollTakeoff = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Landing Roll (M)
            try {
                currentRegex = "(?<=<td>Ground roll landing<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                rollLanding = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Noise Category
            try {
                currentRegex = "(?<=<td>Noise category<\\/td>\\s*<td class=\"text-right\">\\s*Stage\\s)(\\S*)";
                noiseCategory = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Maintenance Category
            try {
                currentRegex = "(?<=<td>Maintenance category<\\/td>\\s*<td class=\"text-right\">\\s*)(\\S+)";
                maintenanceCategory = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value;
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            // Extract Route Restrictions
            try {
                for (int x = 0; x < 11; x++) {
                    for (int y = 0; y < 11; y++) {
                        currentRegex = "(?<=fa fa-check\\s)(.*)(?=\" title=\"" + x.ToString() + " - " + y.ToString() + ")";
                        routeRestrictions[x][y] = Regex.Match(currentHTML, currentRegex, RegexOptions.IgnoreCase).Value == "good" ? true : false;
                    }
                }
            }

            catch {
                warningInstance.AddWarning("Could not process REGEX line: " + currentRegex);
            }

            return new PlaneObject(
                price,
                letterCode,
                manufacturer,
                manufacturingLocation,
                productionPeriod,
                productionRate,
                quantityProduced,
                popularity,
                typeRating,
                cockpitCrew,
                maxPayload,
                maxPax,
                maxCargo,
                maxCargoSpace,
                bulkCargo,
                bulkCargoSpace,
                MTOW,
                range,
                speed,
                rollTakeoff,
                rollLanding,
                noiseCategory,
                maintenanceCategory,
                routeRestrictions);
        }

        #endregion

    }

}
