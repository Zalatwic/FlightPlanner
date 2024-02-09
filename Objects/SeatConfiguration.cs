using FlightPlanner.Enums;

namespace FlightPlanner.Objects {

    [Serializable]
    internal class SeatConfiguration {

        #region Constructors
        public SeatConfiguration(string nameIn, int IDIn) {
            Name = nameIn;
            ID = IDIn;
            rating = new RatingObject();
            Cabins = new List<CabinObject>();
        }

        public SeatConfiguration(string nameIn, int IDIn, RatingObject ratingIn) {
            Name = nameIn;
            ID = IDIn;
            rating = ratingIn;
            Cabins = new List<CabinObject>();
        }

        public SeatConfiguration(string nameIn, int IDIn, RatingObject ratingIn, List<CabinObject> cabinsIn) {
            Name = nameIn;
            ID = IDIn;
            rating = ratingIn;
            Cabins = cabinsIn;
        }

        #endregion

        #region Properties

        private RatingObject rating {
            get;
            set;
        }

        public List<CabinObject> Cabins {
            get;
            private set;
        }

        public int ID {
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

        #endregion

        #region Functions

        public void SetAircraftModel(int modelID) {
            AircraftModelID = modelID;
        }

        public void AddCabin(CabinObject cabin) {
            Cabins.Add(cabin);
        }

        public void ChangeRating(PassengerType type, RatingsDistance distance, int ratingValue) {
            rating.SetRating(type, distance, ratingValue);
        }

        public double GetRating(PassengerType type, RatingsDistance distance) {
            return rating.GetRating(type, distance);
        }

        public (int, int, int) GetCapacity() {
            int economySeats = 0;
            int businessSeats = 0;
            int firstSeats = 0;
            
            foreach (CabinObject cabin in Cabins) {
                if (cabin.Class == PassengerType.Economy) {
                    economySeats += cabin.Capacity;
                }

                if (cabin.Class == PassengerType.Business) {
                    businessSeats += cabin.Capacity;
                }

                if (cabin.Class == PassengerType.First) {
                    firstSeats += cabin.Capacity;
                }
            }

            return (economySeats, businessSeats, firstSeats);
        }

        #endregion

    }

}
