using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Objects {

    [Serializable]
    internal class OwnedAircraft {

        #region Constructor

        public OwnedAircraft(int idIn, string tailNumberIn, string nameIn, int modelIn, string modelNameIn, string currentLocationIn, double ageIn, double conditionIn, double workloadIn, SeatConfiguration configurationIn) {
            ID = idIn;
            TailNumber = tailNumberIn;
            Name = nameIn;
            AircraftModelID = modelIn;
            AircraftModelName = modelNameIn;
            CurrentLocation = currentLocationIn;
            Age = ageIn;
            Condition = conditionIn;
            Workload = workloadIn;
            CurrentConfiguration = configurationIn;

            CurrentDestination = null;
            CurrentFlight = null;
        }

        public OwnedAircraft(int idIn, string tailNumberIn, string nameIn, int modelIn, string modelNameIn, string currentLocationIn, double ageIn, double conditionIn, double workloadIn, SeatConfiguration configurationIn, string destination, string flight) {
            ID = idIn;
            TailNumber = tailNumberIn;
            Name = nameIn;
            AircraftModelID = modelIn;
            AircraftModelName = modelNameIn;
            CurrentLocation = currentLocationIn;
            Age = ageIn;
            Condition = conditionIn;
            Workload = workloadIn;
            CurrentConfiguration = configurationIn;

            CurrentDestination = destination;
            CurrentFlight = flight;
        }

        #endregion

        #region Properties

        public int ID {
            get;
            private set;
        }

        public string TailNumber {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public int AircraftModelID {
            get;
            private set;
        }

        public string AircraftModelName {
            get;
            private set;
        }

        public string CurrentLocation {
            get;
            private set;
        }

        public string? CurrentDestination {
            get;
            private set;
        }

        public string? CurrentFlight {
            get;
            private set;
        }

        public double Age {
            get;
            set;
        }

        public double Condition {
            get;
            set;
        }

        public double Workload {
            get;
            set;
        }

        public SeatConfiguration CurrentConfiguration {
            get;
            set;
        }

        #endregion

        #region Functions

        public void StartFlight(string flightNumber, string destination) {
            CurrentDestination = destination;
            CurrentFlight = flightNumber;
        }

        public void EndFlight() {
            if (CurrentDestination != null) {
                CurrentLocation = CurrentDestination;
                CurrentDestination = null;
                CurrentFlight = null;
            }
        }

        #endregion

    }

}
