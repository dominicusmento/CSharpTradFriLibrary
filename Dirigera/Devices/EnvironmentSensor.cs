using System.Reflection;
using ApiLibs.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class EnvironmentSensor : Device
{
    [JsonProperty("attributes")]
    public EnvironmentSensorAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }

    public override string ToString()
    {
        return Attributes.CustomName;
    }
}

public partial class Room
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("color")]
    public string Color { get; set; }

    [JsonProperty("icon")]
    public string Icon { get; set; }
}

public class EnvironmentSensorAttributes : Attributes
{
    [JsonProperty("currentTemperature")]
    public int CurrentTemperature { get; set; }

    [JsonProperty("currentRH")]
    public int CurrentRH { get; set; }

    [JsonProperty("currentPM25")]
    public int CurrentPM25 { get; set; }

    [JsonProperty("maxMeasuredPM25")]
    public int MaxMeasuredPM25 { get; set; }

    [JsonProperty("minMeasuredPM25")]
    public int MinMeasuredPM25 { get; set; }

    [JsonProperty("vocIndex")]
    public int VocIndex { get; set; }
}