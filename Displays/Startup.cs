using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightPlanner.Displays {
    internal class Startup {
        Table mainTable;

        public Startup() {
        }

        public async Task StartupInitialize() {
            mainTable = new Table().Centered();

            AnsiConsole.Live(mainTable).Start(ctx => {
                Panel loginPanel = new Panel("Login");
                loginPanel.Header = new PanelHeader("AirlineSim World");
                loginPanel.Border = BoxBorder.Ascii;
                loginPanel.Padding = new Padding(2, 2, 2, 2);
                loginPanel.Expand = true;

                //main
            });
        }

    }
}
