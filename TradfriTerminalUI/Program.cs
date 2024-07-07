
using Newtonsoft.Json;
using Spectre.Console;
using Tomidix.NetStandard.Dirigera;
using Martijn.Extensions.Memory;
using Martijn.Extensions.Time;
using System.Net;
using Tomidix.NetStandard.Dirigera.Devices;

namespace TradfriTerminalUI
{
    public class UserData
    {
        [JsonProperty("ip")]
        public string? IPAddress { get; set; }

        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [JsonProperty("hub_name")]
        public string? HubName { get; set; }
    }

    public partial class Program
    {
        private static readonly string SettingsFile = "userdata.json";

        public static async Task Main(string[] args)
        {
            // We need to disable some server certification for these calls
            ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

            Memory memory = new Memory(AppContext.BaseDirectory);
            var userData = await memory.ReadOrCalculate(SettingsFile, () => new UserData());
            try
            {
                if (string.IsNullOrEmpty(userData.AccessToken))
                {
                    await Authenticate(userData);
                }
                else
                {
                    await MainMenu(userData);
                }
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine("[red]Encountered fatal error. Exiting...[/]");
                AnsiConsole.WriteException(e);
            }
            await memory.Write(SettingsFile, userData);
        }

        public static async Task MainMenu(UserData data)
        {
            var controller = new DirigeraController(data.IPAddress!, data.AccessToken!);

            var me = await controller.UserController.GetMe();

            AnsiConsole.MarkupLine("Hi, " + me.Name);

            // var devices = await controller.DeviceController.GetDevices();
            await MainLoop(controller, new List<Device>());

            // AnsiConsole.MarkupLine(Markup.Escape(devices));



        }

        public static async Task MainLoop(DirigeraController controller, List<Device> devices)
        {
            while (true)
            {
                AnsiConsole.Clear();
                var mapDeviceToString = (Device i) =>
                {
                    var text = i switch
                    {
                        Gateway g => g.ToString(),
                        EnvironmentSensor environmentSensor => environmentSensor.ToString(),
                        Light light => light.ToString(),
                        Device d => "Unknown device:" + d.Type,
                        _ => "Unknown value"
                    };
                    return Markup.Escape(text);
                };

                string chosenDevice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Which device would you like to look into?")
                        .EnableSearch()
                        .MoreChoicesText("[grey](Move up and down to reveal more devices)[/]")
                        .AddChoiceGroup("Devices", devices.Select(i => mapDeviceToString(i)))
                        .AddChoiceGroup("System", ["Update devices", "Json Dump of devices", "Exit"]));
                if (chosenDevice == "Update devices")
                {
                    devices = await controller.DeviceController.GetDevices();
                    continue;
                }

                if (chosenDevice == "Json Dump of devices")
                {
                    Details.JsonView(await controller.DeviceController.GetDevicesJson(), "All devices");
                    continue;
                }

                if (chosenDevice == "Exit")
                {
                    break;
                }

                await Details.DetailView(devices.FirstOrDefault(i => mapDeviceToString(i) == chosenDevice));
            }
        }

        public static async Task Authenticate(UserData userData)
        {
            AnsiConsole.MarkupLine("Did not find a connection token. Starting authorization flow");

            string code = DirigeraController.generateCodeVerifier();
            // string code = "Hello";
            AnsiConsole.MarkupLine("Generated code: [bold]{0}[/]", Markup.Escape(code));

            string challenge = DirigeraController.calculateCodeChallenge(code);
            Console.WriteLine(challenge);
            AnsiConsole.MarkupLine("Using challenge: [bold]{0}[/]", Markup.Escape(challenge));

            /// IP Address
            if (!string.IsNullOrEmpty(userData.IPAddress) && !AnsiConsole.Confirm($"Use {userData.IPAddress} to connect?"))
            {
                userData.IPAddress = null;
            }
            if (string.IsNullOrEmpty(userData.IPAddress))
            {
                userData.IPAddress = AnsiConsole.Ask<string>("What's the ip of the app? (it needs to be https://)");
            }


            /// Hub Name
            if (!string.IsNullOrEmpty(userData.HubName) && !AnsiConsole.Confirm($"Use {userData.HubName} to connect?"))
            {
                userData.HubName = null;
            }
            if (string.IsNullOrEmpty(userData.HubName))
            {
                userData.HubName = AnsiConsole.Ask("How would you like to name the hub?", "Debug Application");
            }

            /// Connecting

            DirigeraController controller = new DirigeraController(userData.IPAddress);
            string? dirigeraCode = null;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Connecting to Dirigera Hub", async ctx =>
                {
                    dirigeraCode = (await controller.Authorize(challenge)).Code;
                });



            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .StartAsync("Trying to get an access token", async ctx =>
                {
                    AnsiConsole.MarkupLine("Press the Action Button on the bottom of your Dirigera Hub within 60 seconds.");


                    for (int i = 0; i < 30; i++)
                    {
                        try
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(3));
                            userData.AccessToken = (await controller.Pair(dirigeraCode!, code, userData.HubName)).AccessToken;
                            break;
                        }
                        catch (Exception e)
                        {
                            AnsiConsole.MarkupLine("Got an exception: " + e.Message);
                            ctx.Status = $"Trying to get an access token [[{i}/30]]";
                        }
                    }


                });

            AnsiConsole.MarkupLine("🎉 Successfully got the code 🎉");
        }
    }
}
