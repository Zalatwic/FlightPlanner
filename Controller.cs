using System.Text.Json;
using Spectre.Console;
using FlightPlanner;
using FlightPlanner.Objects;
using FlightPlanner.Networking;

public static class Controller {
    public static async Task Main(string[] args) {
        WarningCarrier warnings = new WarningCarrier();
        RemoteServer currentServer = new RemoteServer("junkers");
        WebScraper currentScraper = new WebScraper(ref currentServer, ref warnings);
        Dictionary<int, PlaneObject> planeObjects = currentScraper.GetNewPlanesForSale();

        await using FileStream createStream = File.Create(@"D:\planeObjects.json");
        await JsonSerializer.SerializeAsync(createStream, planeObjects);

        AnsiConsole.Markup("[underline red]Hello[/] World!");
    }
}
