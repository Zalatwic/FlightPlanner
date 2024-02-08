using Spectre.Console;
using FlightPlanner;
using FlightPlanner.Objects;

public static class Controller {
    public static void Main(string[] args) {
        WarningCarrier warnings = new WarningCarrier();
        RemoteServer currentServer = new RemoteServer("junkers");
        WebScraper currentScraper = new WebScraper(ref currentServer, ref warnings);
        List<PlaneObject> planeObjects = currentScraper.GetNewPlanesForSale();

        foreach (PlaneObject planeObject in planeObjects) {
            AnsiConsole.Markup(planeObject.LetterCode);
        }

        AnsiConsole.Markup("[underline red]Hello[/] World!");
    }
}
