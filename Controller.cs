using System.Text.Json;
using Terminal.Gui;
using FlightPlanner;
using FlightPlanner.Objects;
using FlightPlanner.Networking;
using FlightPlanner.Displays;

public static class Controller {
    public static async Task Main(string[] args) {
        //Startup startup = new Startup();
        //await startup.StartupInitialize();
        List<FleetObject> currentFleets = SaveFleetsToFile().Result;
        CreateWindow(currentFleets);
    }

    #region Functions

    private static async void SavePlanesToFile() {
        WarningCarrier warnings = new WarningCarrier();
        RemoteServer currentServer = new RemoteServer("junkers");
        WebScraper currentScraper = new WebScraper(ref currentServer, ref warnings);
        Dictionary<int, PlaneObject> planeObjects = currentScraper.GetNewPlanesForSale();

        await using FileStream createStream = File.Create(@"D:\planeObjects.json");
        await JsonSerializer.SerializeAsync(createStream, planeObjects);
    }

    private static async Task<List<FleetObject>> SaveFleetsToFile() {
        WarningCarrier warnings = new WarningCarrier();
        RemoteServer currentServer = new RemoteServer("junkers");
        WebScraper currentScraper = new WebScraper(ref currentServer, ref warnings);
        List<SeatConfiguration> currentConfigurations = currentScraper.GetSeatConfigurations();
        List<FleetObject> currentFleets = currentScraper.GetFleetsOwned(currentConfigurations);

        await using FileStream createStream = File.Create(@"D:\fleetObjects.json");
        await JsonSerializer.SerializeAsync(createStream, currentFleets);

        return currentFleets;
    }

    private static async void CreateWindow(List<FleetObject> currentFleets) {
        Fleets fleetsWindow = new Fleets();

        Application.Init();
        var top = new Toplevel() {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var win = new View() {
            X = 0,
            Y = 1,
            Width = Dim.Fill(),
            Height = Dim.Fill() - 1
        };

        var menu = new MenuBar(new MenuBarItem[] {
            new MenuBarItem ("_AIRCRAFT", new MenuItem [] {
                new MenuItem ("_FLEETS", "", () => {
                    win.RemoveAll();
                    win.Add(fleetsWindow);
                    fleetsWindow.AddFleets(currentFleets);
                    win.SetNeedsDisplay();
                })
            }),
        });

        // Add both menu and win in a single call
        top.Add(win, menu);
        Application.Run(top);

    }

    #endregion
}
