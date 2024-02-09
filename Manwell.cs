using FlightPlanner.Objects;

namespace FlightPlanner {
    internal class Manwell {

        #region Functions

        public static SeatConfiguration FindSeatConfiguration(List<SeatConfiguration> currentConfigurations, int planeModelID, int economySeats, int businessSeats, int firstSeats) {
            if (economySeats == 0 && businessSeats == 0 && firstSeats == 0) {
                return new SeatConfiguration("UNASSIGNED", 0);
            }
            
            foreach (SeatConfiguration seatConfiguration in currentConfigurations) {
                if (seatConfiguration.AircraftModelID == planeModelID && seatConfiguration.GetCapacity() == (economySeats, businessSeats, firstSeats)) {
                    return seatConfiguration;
                }
            }

            return new SeatConfiguration("UNASSIGNED", 0);
        }

        #endregion

    }
}
