using FlightPlanner.Enums;

namespace FlightPlanner.Objects {

    [Serializable]
    public class CabinObject {

        #region Constructor

        public CabinObject(PassengerType classIn, SeatType seatIn, int capacityIn, int attendantsIn) {
            Class = classIn;
            Seat = seatIn;
            Capacity = capacityIn;
            AdditionalAttendants = attendantsIn;
        }

        #endregion

        #region Properties

        public PassengerType Class {
            get;
            set;
        }

        public SeatType Seat {
            get;
            set;
        }

        public int Capacity {
            get;
            set;
        }

        public int AdditionalAttendants {
            get;
            set;
        }

        #endregion

    }

}
