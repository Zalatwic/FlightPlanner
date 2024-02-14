using Terminal.Gui;
using FlightPlanner.Objects;

namespace FlightPlanner.Displays {
    public partial class Fleets {
        Dictionary<string, FleetObject> fleetDictionary;
        
        public Fleets() {
            InitializeComponent();
            fleetDictionary = new Dictionary<string, FleetObject>();
        }

        public void AddFleets(List<FleetObject> fleetListIn) {
            fleetList = new ListView(fleetListIn.Select(x => x.Name).ToList());
        }
    }
}
