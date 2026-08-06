using Martijn.Extensions.Linq;
using Spectre.Console;
using Spectre.Console.Json;
using Tomidix.NetStandard.Dirigera.Controller;
using Tomidix.NetStandard.Dirigera.Devices;

namespace TradfriTerminalUI
{
    public class Details
    {
        public static Task DetailView(DirigeraDevice? d, DeviceController deviceController)
        {
            AnsiConsole.Clear();

            return d switch
            {
                EnvironmentSensor environmentSensor => EnvironmentDetailView(environmentSensor, deviceController),
                Light light => LightDetailView(light, deviceController),
                MotionSensor motionSensor => MotionSensorDetailView(motionSensor, deviceController),
                Outlet outlet => OutletView(outlet, deviceController),
                _ => DetailViewFallback(d)
            };
        }

        public static Task DetailViewFallback(DirigeraDevice? device)
        {
            AnsiConsole.Confirm("The following value does not have a detail view: " + device?.Type, true);
            return Task.CompletedTask;
        }

        public static Task EnvironmentDetailView(EnvironmentSensor sensor, DeviceController deviceController)
        {
            return DetailsView<EnvironmentSensor>.Show(sensor, deviceController, (sensor) =>
            {
                return new Dictionary<string, string> {
                {  "Temperature", sensor.Attributes.CurrentTemperature.ToString() },
                {  "PM25", sensor.Attributes.CurrentPM25.ToString() },
                {  "VOCIndex", sensor.Attributes.VocIndex.ToString() },
                {  "Humidity", sensor.Attributes.CurrentRH.ToString() },
                };
            },
            new Dictionary<string, Func<Task>> { });
        }

        public static Task OutletView(Outlet outlet, DeviceController deviceController)
        {
            return DetailsView<Outlet>.Show(outlet, deviceController, (sensor) =>
            {
                return new Dictionary<string, string> {
                    {  "State", sensor.Attributes.IsOn.ToString() },
                };
            },
            new Dictionary<string, Func<Task>> {
                { "Toggle", outlet.Toggle}
            });
        }

        public static Task LightDetailView(Light light, DeviceController deviceController)
        {
            return DetailsView<Light>.Show(light, deviceController, (light) =>
            {
                if (light.Attributes is TemperatureLightAttributes temperatureAttributes)
                {
                    return new Dictionary<string, string> {
                        {  "Lightlevel", light.Attributes.LightLevel.ToString() },
                        {  "IsOn", light.Attributes.IsOn.ToString() },
                        {  "ColorTemperature", temperatureAttributes.ColorTemperature.ToString() },
                        {  "ColorTemperatureMax", temperatureAttributes.ColorTemperatureMax.ToString() },
                        {  "ColorTemperatureMin", temperatureAttributes.ColorTemperatureMin.ToString() },
                    };
                }

                return new Dictionary<string, string> {
                        {  "Lightlevel", light.Attributes.LightLevel.ToString() },
                        {  "IsOn", light.Attributes.IsOn.ToString() },
                    };
            },
            new Dictionary<string, Func<Task>> {
                { "Set Temperature", async () => {
                    var response = AnsiConsole.Ask<int>("What should the temperature be?");
                    await light.SetLightTemperature(response);
                }},
                { "Set Lightlevel", async () => {
                    var response = AnsiConsole.Ask<int>("What should the lightlevel be?");
                    await light.SetLightLevel(response);
                }},
                { "Toggle", light.Toggle}
            });
        }

        public static Task MotionSensorDetailView(MotionSensor motionSensor, DeviceController deviceController)
        {
            return DetailsView<MotionSensor>.Show(motionSensor, deviceController, (motionSensor) =>
            {
                return new Dictionary<string, string> {
                    {  "BatteryPercentage", motionSensor.Attributes.BatteryPercentage.ToString() },
                    {  "IsDetected", motionSensor.Attributes.IsDetected.ToString() },
                    {  "IsOn", motionSensor.Attributes.IsOn.ToString() },
                    {  "MotionDetectedDelay", motionSensor.Attributes.MotionDetectedDelay.ToString() },
                    {  "ScheduleOn", motionSensor.Attributes.SensorConfig.ScheduleOn.ToString() },
                };
            },
            new Dictionary<string, Func<Task>> {
                { "Change MotionDetectedDelay", async () => {
                    var response = AnsiConsole.Ask<int>("How long?");
                    await motionSensor.SetMotionDetectedDelay(response);
                }},
            });
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