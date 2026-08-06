using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tomidix.NetStandard.Dirigera.Devices;

public class LightAttributeConverter : JsonConverter<LightAttributes>

{
    public override LightAttributes ReadJson(JsonReader reader, Type objectType, [AllowNull] LightAttributes existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jToken = JToken.ReadFrom(reader);

        LightAttributes getLightAttribute()
        {
            try
            {
                if (jToken.Type is not JTokenType.None and not JTokenType.Null && jToken is JObject jObject)
                {
                    if (jObject.Property("colorMode") != null)
                    {
                        return new TemperatureLightAttributes();
                    }
                }
            }
            catch { }
            return new LightAttributes();
        };

        LightAttributes result = getLightAttribute();
        serializer.Populate(jToken.CreateReader(), result);
        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, [AllowNull] LightAttributes value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Light : DirigeraDevice
{
    [JsonProperty("attributes")]
    public LightAttributes Attributes { get; set; }

    [JsonProperty("room")]
    public Room Room { get; set; }
    public override string GetName() => Attributes.CustomName;

    public override string ToString()
    {
        var state = Attributes.IsOn ? Attributes.LightLevel.ToString() : "Off";
        return Attributes.CustomName + $"[{state}]";
    }

    public Task Toggle()
    {
        return Service.DeviceController.Toggle(this);
    }

    public Task SetLightLevel(int lightLevel)
    {
        return Service.DeviceController.SetLightLevel(this, lightLevel);
    }

    public Task SetLightTemperature(int colorTemperature)
    {
        return Service.DeviceController.SetLightTemperature(this, colorTemperature);
    }
}

[JsonConverter(typeof(LightAttributeConverter))]
public class LightAttributes : Attributes
{

    [JsonProperty("isOn")]
    public bool IsOn { get; set; }

    [JsonProperty("startupOnOff")]
    public string StartupOnOff { get; set; }

    [JsonProperty("lightLevel")]
    public long LightLevel { get; set; }

    [JsonProperty("startUpCurrentLevel")]
    public long StartUpCurrentLevel { get; set; }
}

public class TemperatureLightAttributes : LightAttributes
{
    [JsonProperty("colorMode")]
    public string ColorMode { get; set; }

    [JsonProperty("startupTemperature")]
    public long StartupTemperature { get; set; }

    [JsonProperty("colorTemperature")]
    public long ColorTemperature { get; set; }

    [JsonProperty("colorTemperatureMax")]
    public long ColorTemperatureMax { get; set; }

    [JsonProperty("colorTemperatureMin")]
    public long ColorTemperatureMin { get; set; }
}

public class LightEventAttributes : EventAttributes
{
    [JsonProperty("isOn")]
    public bool? IsOn { get; set; }

    [JsonProperty("startupOnOff")]
    public string? StartupOnOff { get; set; }

    [JsonProperty("lightLevel")]
    public long? LightLevel { get; set; }

    [JsonProperty("startUpCurrentLevel")]
    public long? StartUpCurrentLevel { get; set; }

    [JsonProperty("colorMode")]
    public string? ColorMode { get; set; }

    [JsonProperty("startupTemperature")]
    public long? StartupTemperature { get; set; }

    [JsonProperty("colorTemperature")]
    public long? ColorTemperature { get; set; }

    [JsonProperty("colorTemperatureMax")]
    public long? ColorTemperatureMax { get; set; }

    [JsonProperty("colorTemperatureMin")]
    public long? ColorTemperatureMin { get; set; }
}