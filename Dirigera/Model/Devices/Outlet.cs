using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.


public class Outlet : DirigeraDevice
{
    [JsonProperty("attributes")]
    public OutletAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsOn ? "On" : "Off";
        return Attributes.CustomName + $"[{state}]";
    }

    public Task Toggle()
    {
        return Service.DeviceController.Toggle(this);
    }
}

public class OutletAttributes : Attributes
{
    [JsonProperty("isOn")]
    public bool IsOn { get; set; }

    [JsonProperty("startupOnOff")]
    public string StartupOnOff { get; set; }

    [JsonProperty("lightLevel")]
    public long LightLevel { get; set; }

    [JsonProperty("startUpCurrentLevel")]
    public long StartUpCurrentLevel { get; set; }

    [JsonProperty("currentVoltage")]
    public long CurrentVoltage { get; set; }

    [JsonProperty("currentAmps")]
    public long CurrentAmps { get; set; }

    [JsonProperty("currentActivePower")]
    public long CurrentActivePower { get; set; }

    [JsonProperty("totalEnergyConsumed")]
    public long TotalEnergyConsumed { get; set; }

    [JsonProperty("totalEnergyConsumedLastUpdated")]
    public DateTime TotalEnergyConsumedLastUpdated { get; set; }

    [JsonProperty("energyConsumedAtLastReset")]
    public long EnergyConsumedAtLastReset { get; set; }

    [JsonProperty("timeOfLastEnergyReset")]
    public DateTime TimeOfLastEnergyReset { get; set; }

    [JsonProperty("childLock")]
    public bool ChildLock { get; set; }

    [JsonProperty("statusLight")]
    public bool StatusLight { get; set; }
}

public class OutletEventAttributes : EventAttributes
{
    [JsonProperty("isOn")]
    public bool? IsOn { get; set; }

    [JsonProperty("startupOnOff")]
    public string? StartupOnOff { get; set; }

    [JsonProperty("lightLevel")]
    public long? LightLevel { get; set; }

    [JsonProperty("startUpCurrentLevel")]
    public long? StartUpCurrentLevel { get; set; }

    [JsonProperty("currentVoltage")]
    public long? CurrentVoltage { get; set; }

    [JsonProperty("currentAmps")]
    public long? CurrentAmps { get; set; }

    [JsonProperty("currentActivePower")]
    public long? CurrentActivePower { get; set; }

    [JsonProperty("totalEnergyConsumed")]
    public long? TotalEnergyConsumed { get; set; }

    [JsonProperty("totalEnergyConsumedLastUpdated")]
    public DateTime? TotalEnergyConsumedLastUpdated { get; set; }

    [JsonProperty("energyConsumedAtLastReset")]
    public long? EnergyConsumedAtLastReset { get; set; }

    [JsonProperty("timeOfLastEnergyReset")]
    public DateTime? TimeOfLastEnergyReset { get; set; }

    [JsonProperty("childLock")]
    public bool? ChildLock { get; set; }

    [JsonProperty("statusLight")]
    public bool? StatusLight { get; set; }
}