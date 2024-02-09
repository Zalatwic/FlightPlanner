using FlightPlanner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Objects {
    internal class RatingObject {

        #region Constructor

        public RatingObject() {
            ratings = generateNewRatingsMatrix();
        }

        #endregion

        #region Properties

        private int[,] ratings {
            get;
            set;
        }

        #endregion

        #region Functions

        private static int[,] generateNewRatingsMatrix() {
            int xMax = Enum.GetValues(typeof(PassengerType)).Length;
            int yMax = Enum.GetValues(typeof(RatingsDistance)).Length;
            
            return new int[xMax, yMax];
        }

        public void SetRating(PassengerType type, RatingsDistance distance, int rating) {
            ratings[(int)type - 1, (int)distance - 1] = rating;
        }

        public double GetRating(PassengerType type, RatingsDistance distance) {
            return ratings[(int)type - 1, (int)distance - 1];
        }

        #endregion
    }
}
