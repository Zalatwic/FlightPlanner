namespace FlightPlanner.Objects {

    [Serializable]
    public class FleetObject {

        #region Constructors

        public FleetObject(int idIn, string nameIn) {
            ID = idIn;
            Name = nameIn;
            AircraftList = new List<OwnedAircraft>();
        }

        public FleetObject(int idIn, string nameIn, List<OwnedAircraft> aircraftList) {
            ID = idIn;
            Name = nameIn;
            AircraftList = aircraftList;
        }

        #endregion

        #region Properties

        public int ID {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public List<OwnedAircraft> AircraftList {
            get;
            private set;
        }

        #endregion

        #region Functions

        public void AddAircraft(OwnedAircraft aircraft) {
            AircraftList.Add(aircraft);
        }

        public void RenameFleet(string newName) {
            Name = newName;
        }

        #endregion

    }

}
