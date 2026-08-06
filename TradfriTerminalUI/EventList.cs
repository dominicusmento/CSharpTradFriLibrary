using System.Collections.Concurrent;
using ApiLibs.MicrosoftGraph;
using Martijn.Extensions.Linq;


// using Martijn.Extensions.Linq;
using Spectre.Console;
using Spectre.Console.Json;
using Tomidix.NetStandard.Dirigera.Controller;
using Tomidix.NetStandard.Dirigera.Devices;
using Tomidix.NetStandard.Dirigera.Model.Events;

namespace TradfriTerminalUI
{
    public class EventView
    {
        public static async Task Start(EventController controller, List<DirigeraDevice> devices)
        {
            AnsiConsole.Clear();

            var q = new ConcurrentQueue<EventController.DirigeraEventArgs>();

            controller.OnEventSent += (sender, ev) =>
            {
                q.Enqueue(ev);
            };

            controller.SendKeepAliveMessages(CancellationToken.None);

            await AnsiConsole.Status()
                .StartAsync("Waiting for messages", async ctx =>
                {
                    while (true)
                    {
                        await Task.Delay(500);
                        while (q.Any())
                        {
                            EventController.DirigeraEventArgs? newEvent = null;
                            q.TryDequeue(out newEvent);
                            if (newEvent != null)
                            {
                                var getStateChangeText = (DirigeraStateChangedEvent events) =>
                                {
                                    return MapAttributeToText(newEvent.Message, devices.FirstOrDefault(i => i.Id == events.Data.Id)?.ToString(), events.Data.Attributes)
                                        .Select(text => $"[dim][[{newEvent.Event.Time}]][/] " + Markup.Escape(text));
                                };

                                var lines = newEvent.Event switch
                                {
                                    DirigeraStateChangedEvent events => getStateChangeText(events),
                                    DirigeraPongEvent pongEvent => new List<string> { $"[dim][[{newEvent.Event.Time}]][/] Replied pong" },
                                    UnknownEvent unknownEvent => new List<string> { Markup.Escape(newEvent.Message) },
                                    _ => new List<string>()
                                };

                                lines.Foreach(AnsiConsole.MarkupLine);
                            }
                        }
                    }
                });
        }

        public static List<string> MapAttributeToText(string text, string? deviceName, EventAttributes changedEvent)
        {
            List<string> lines = [];
            if (changedEvent is EnvironmentSensorEventAttributes environmentSensorEvent)
            {
                if (environmentSensorEvent.VocIndex != null)
                {
                    lines.Add($"{deviceName} measured a volatile organic compounds of {environmentSensorEvent.VocIndex}");
                }

                if (environmentSensorEvent.CurrentTemperature != null)
                {
                    lines.Add($"{deviceName} measured a temperature of {environmentSensorEvent.CurrentTemperature}");
                }

                if (environmentSensorEvent.CurrentPM25 != null)
                {
                    lines.Add($"{deviceName} measured a PM25 of {environmentSensorEvent.CurrentPM25}");
                }

                if (environmentSensorEvent.CurrentRH != null)
                {
                    lines.Add($"{deviceName} measured a relative humidity of {environmentSensorEvent.CurrentRH}");
                }

                if (environmentSensorEvent.CurrentCO2 != null)
                {
                    lines.Add($"{deviceName} measured a CO2 level of {environmentSensorEvent.CurrentCO2}");
                }
            }

            if (changedEvent is LightEventAttributes lightEvent)
            {
                if (lightEvent.IsOn != null)
                {
                    lines.Add(lightEvent.IsOn.Value ? $"{deviceName} was turned on" : $"{deviceName} was turned off");
                }

                if (lightEvent.LightLevel != null)
                {
                    lines.Add($"The lightlevel of {deviceName} was changed to {lightEvent.LightLevel}");
                }
            }

            if (changedEvent is LightSensorEventAttributes lightSensorEvent)
            {
                if (lightSensorEvent.Illuminance != null)
                {
                    lines.Add($"The illuminance of {deviceName} was changed to {lightSensorEvent.Illuminance}");
                }
            }

            if (changedEvent is MotionSensorEventAttributes motionSensorEvent)
            {
                if (motionSensorEvent.IsDetected != null)
                {
                    lines.Add($"{deviceName} detected motion");
                }

                if (motionSensorEvent.LightLevel != null)
                {
                    lines.Add($"The lightlevel of {deviceName} was changed to {motionSensorEvent.LightLevel}");
                }

                if (motionSensorEvent.BatteryPercentage != null)
                {
                    lines.Add($"{deviceName} has a batterypercentage of {motionSensorEvent.BatteryPercentage}%");
                }
            }


            if (changedEvent is OutletEventAttributes outletEvent)
            {
                if (outletEvent.CurrentVoltage != null)
                {
                    lines.Add($"{deviceName} measured voltage of {outletEvent.CurrentVoltage}");
                }

                if (outletEvent.CurrentAmps != null)
                {
                    lines.Add($"{deviceName} measured ampere of {outletEvent.CurrentAmps}");
                }

                if (outletEvent.CurrentActivePower != null)
                {
                    lines.Add($"{deviceName} measured power of {outletEvent.CurrentActivePower}");
                }

                if (outletEvent.TotalEnergyConsumed != null)
                {
                    lines.Add($"{deviceName} has consumed a total of {outletEvent.TotalEnergyConsumed} power");
                }
            }


            if (changedEvent is WaterSensorEventAttributes waterSensorEvent)
            {
                if (waterSensorEvent.WaterLeakDetected != null)
                {
                    lines.Add($"{deviceName} detected something with water");
                }
                if (waterSensorEvent.BatteryPercentage != null)
                {
                    lines.Add($"{deviceName} has a batterypercentage of {waterSensorEvent.BatteryPercentage}%");
                }
            }

            if (changedEvent is UnknownEventAttributes unknownEvent)
            {
                lines.Add(unknownEvent.Json);
            }

            return lines;
        }

    }
}