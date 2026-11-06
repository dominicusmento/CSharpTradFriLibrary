using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class LightSensor : DirigeraDevice
{
    [JsonProperty("attributes")]
    public LightSensorAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }

    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.Illuminance;
        return Attributes.CustomName + $"[{state}]";
    }
}

public class LightSensorAttributes : Attributes
{
    [JsonProperty("illuminance")]
    public long Illuminance { get; set; }
}

public class LightSensorEventAttributes : EventAttributes
{
    [JsonProperty("illuminance")]
    public long? Illuminance { get; set; }
}