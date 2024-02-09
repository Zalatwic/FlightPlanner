namespace FlightPlanner {
    internal class WarningCarrier {

        #region Constructor

        public WarningCarrier() {
            Warnings = new List<string>();
        }

        #endregion

        #region Properties

        public List<string> Warnings {
            get;
            set;
        }

        #endregion

        #region Functions

        public void AddWarning(string warning) {
            Warnings.Add(warning);
        }

        #endregion
    }
}
