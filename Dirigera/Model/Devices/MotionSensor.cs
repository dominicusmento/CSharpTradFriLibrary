using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera.Devices;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class MotionSensor : DirigeraDevice
{
    [JsonProperty("attributes")]
    public MotionSensorAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }

    public Task SetMotionDetectedDelay(int delay)
    {
        return Service.DeviceController.SetMotionDetectedDelay(this, delay);
    }

    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsDetected ? "Seen motion" : "No Motion";
        return Attributes.CustomName + $"[{state}]";
    }
}

public class MotionSensorAttributes : Attributes
{
    [JsonProperty("batteryPercentage")]
    public long BatteryPercentage { get; set; }

    [JsonProperty("isOn")]
    public bool IsOn { get; set; }

    [JsonProperty("isDetected")]
    public bool IsDetected { get; set; }

    [JsonProperty("motionDetectedDelay")]
    public long MotionDetectedDelay { get; set; }

    [JsonProperty("sensorConfig")]
    public SensorConfig SensorConfig { get; set; }

    [JsonProperty("circadianPresets")]
    public object[] CircadianPresets { get; set; }

    [JsonProperty("lightLevel")]
    public int? LightLevel { get; set; }
}

public partial class SensorConfig
{
    [JsonProperty("scheduleOn")]
    public bool ScheduleOn { get; set; }

    [JsonProperty("onDuration")]
    public long OnDuration { get; set; }

    [JsonProperty("schedule")]
    public Schedule Schedule { get; set; }
}

public partial class Schedule
{
    [JsonProperty("onCondition")]
    public Condition OnCondition { get; set; }

    [JsonProperty("offCondition")]
    public Condition OffCondition { get; set; }
}

public partial class Condition
{
    [JsonProperty("time")]
    public string Time { get; set; }
}

public class MotionSensorEventAttributes : EventAttributes
{
    [JsonProperty("batteryPercentage")]
    public long? BatteryPercentage { get; set; }

    [JsonProperty("isOn")]
    public bool? IsOn { get; set; }

    [JsonProperty("isDetected")]
    public bool? IsDetected { get; set; }

    [JsonProperty("motionDetectedDelay")]
    public long? MotionDetectedDelay { get; set; }

    [JsonProperty("sensorConfig")]
    public SensorConfig? SensorConfig { get; set; }

    [JsonProperty("circadianPresets")]
    public object[]? CircadianPresets { get; set; }

    [JsonProperty("lightLevel")]
    public int? LightLevel { get; set; }
}