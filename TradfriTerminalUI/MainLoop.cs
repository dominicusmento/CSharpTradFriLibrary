using Martijn.Extensions.Linq;
using Spectre.Console;
using Spectre.Console.Json;
using Tomidix.NetStandard.Dirigera.Devices;

namespace TradfriTerminalUI
{
    public class Details
    {
        public static async Task<int> DetailView(Device? d)
        {
            AnsiConsole.Clear();

            return d switch
            {
                EnvironmentSensor environmentSensor => EnvironmentDetailView(environmentSensor),
                Light light => await LightDetailView(light),
                _ => DetailViewFallback(d)
            };
        }

        public static int DetailViewFallback(Device? device)
        {
            AnsiConsole.Confirm("The following value does not have a detail view: " + device?.Type, true);
            return 1;
        }

        public static int EnvironmentDetailView(EnvironmentSensor sensor)
        {
            var table = new Table();
            table.AddColumns(new TableColumn("key"), new TableColumn("value"));

            var dict = new Dictionary<string, string> {
                {  "Temperature", sensor.Attributes.CurrentTemperature.ToString() },
                {  "PM25", sensor.Attributes.CurrentPM25.ToString() },
                {  "VOCIndex", sensor.Attributes.VocIndex.ToString() },
                {  "Humidity", sensor.Attributes.CurrentRH.ToString() },
            };

            dict.Foreach(item =>
            {
                table.AddRow(item.Key, item.Value);
            });

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("");

            while (true)
            {
                string choice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .MoreChoicesText("[grey](Move up and down to reveal more devices)[/]")
                                    .AddChoices([
                                        "Exit"
                                    ]));


                if (choice == "Exit")
                {
                    break;
                }
            }

            return 1;

        }

        public static async Task<int> LightDetailView(Light light)
        {
            var table = new Table();
            table.AddColumns(new TableColumn("key"), new TableColumn("value"));

            var dict = new Dictionary<string, string> {
                {  "Lightlevel", light.Attributes.LightLevel.ToString() },
                {  "IsOn", light.Attributes.IsOn.ToString() },
                {  "ColorTemperature", light.Attributes.ColorTemperature.ToString() },
                {  "ColorTemperatureMax", light.Attributes.ColorTemperatureMax.ToString() },
                {  "ColorTemperatureMin", light.Attributes.ColorTemperatureMin.ToString() },
            };

            dict.Foreach(item =>
            {
                table.AddRow(item.Key, item.Value);
            });

            AnsiConsole.Write(table);
            AnsiConsole.WriteLine("");

            while (true)
            {
                string choice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .MoreChoicesText("[grey](Move up and down to reveal more devices)[/]")
                                    .AddChoices([
                                        "Set Temperature",
                                        "Set Lightlevel",
                                        "Toggle",
                                        "Exit"
                                    ]));
                if (choice == "Set Temperature")
                {
                    var response = AnsiConsole.Ask<int>("What should the temperature be?");
                    await light.SetLightTemperature(response);
                }

                if (choice == "Set Lightlevel")
                {
                    var response = AnsiConsole.Ask<int>("What should the lightlevel be?");
                    await light.SetLightLevel(response);
                }

                if (choice == "Toggle")
                {
                    await light.Toggle();
                }

                if (choice == "Exit")
                {
                    break;
                }
            }

            return 1;

        }

        public static int JsonView(string input, string? name = null)
        {
            var json = new JsonText(input);

            AnsiConsole.Clear();
            AnsiConsole.Write(
                new Panel(json)
                    .Header(name)
                    .RoundedBorder()
                    .BorderColor(Color.Yellow));

            AnsiConsole.Confirm("Press enter to go back", true);

            return 0;
        }
    }
}