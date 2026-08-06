using Martijn.Extensions.Linq;

using Spectre.Console;
using Tomidix.NetStandard.Dirigera.Controller;
using Tomidix.NetStandard.Dirigera.Devices;

namespace TradfriTerminalUI
{
    public class DetailsView<T> where T : DirigeraDevice
    {
        public static async Task Show(T input, DeviceController deviceController, Func<T, Dictionary<string, string>> map, Dictionary<string, Func<Task>> options, Func<Task<T>>? update = null)
        {
            while (true)
            {
                AnsiConsole.Clear();

                var table = new Table();
                table.AddColumns(new TableColumn("key"), new TableColumn("value"));

                var dict = map(input);

                dict.Foreach(item =>
                {
                    table.AddRow(item.Key, item.Value);
                });

                AnsiConsole.Write(table);
                AnsiConsole.WriteLine("");

                string choice = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                                    .AddChoices(new string[] { "Update" }.Concat(options.Keys).Concat([
                                        "Exit"
                                    ])));
                if (choice == "Exit")
                {
                    break;
                }

                if (options.ContainsKey(choice))
                {
                    await options[choice]();
                }

                if (deviceController != null)
                {
                    input = (await deviceController.GetDevices()).OfType<T>().First(i => i.Id == input.Id);
                }

            }
        }

    }
}