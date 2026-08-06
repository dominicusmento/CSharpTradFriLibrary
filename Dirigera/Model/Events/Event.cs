using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tomidix.NetStandard.Dirigera.Devices;

namespace Tomidix.NetStandard.Dirigera.Model.Events;

public class EventConverter : JsonConverter<DirigeraEvent>

{
    public override DirigeraEvent ReadJson(JsonReader reader, Type objectType, [AllowNull] DirigeraEvent existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JToken jObject = JToken.ReadFrom(reader);

        string type = "";

        try
        {
            if (jObject.Type is not JTokenType.None and not JTokenType.Null)
            {
                type = jObject["type"]?.ToObject<string>() ?? "";
            }
        }
        catch { }

        DirigeraEvent result = type switch
        {
            "deviceStateChanged" => new DirigeraStateChangedEvent(),
            "pong" => new DirigeraPongEvent(),
            _ => new UnknownEvent()
        };


        serializer.Populate(jObject.CreateReader(), result);
        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, [AllowNull] DirigeraEvent value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

[JsonConverter(typeof(EventConverter))]
public class DirigeraEvent
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("time")]
    public DateTimeOffset Time { get; set; }

    [JsonProperty("specversion")]
    public string Specversion { get; set; }

    [JsonProperty("source")]
    public string Source { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public class UnknownEvent : DirigeraEvent
{
    [JsonProperty("data")]
    public dynamic Data { get; set; }
}

public class DirigeraStateChangedEvent : DirigeraEvent
{
    [JsonProperty("data")]
    public DirigeraStateChangedEventData Data { get; set; }
}

public class DirigeraPongEvent : DirigeraEvent
{
    [JsonProperty("data")]
    public DirigeraPongEventData Data { get; set; }
}


public class DirigeraPongEventData
{
    [JsonProperty("lastModified")]
    public string LastModified { get; set; }
}

[JsonConverter(typeof(DirigeraStateChangedEventDataConverter))]
public class DirigeraStateChangedEventData : EventDirigeraDevice
{
    [JsonProperty("attributes")]
    public EventAttributes Attributes { get; set; }
}

public class DirigeraStateChangedEventDataConverter : JsonConverter<DirigeraStateChangedEventData>

{
    public override DirigeraStateChangedEventData ReadJson(JsonReader reader, Type objectType, [AllowNull] DirigeraStateChangedEventData existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JObject jObject = JObject.Load(reader);

        string deviceType = jObject["deviceType"]?.ToObject<string>() ?? "";

        EventAttributes attributes = deviceType switch
        {
            "environmentSensor" => new EnvironmentSensorEventAttributes(),
            "light" => new LightEventAttributes(),
            "motionSensor" => new MotionSensorEventAttributes(),
            "lightSensor" => new LightSensorEventAttributes(),
            "outlet" => new OutletEventAttributes(),
            "waterSensor" => new WaterSensorEventAttributes(),
            _ => new UnknownEventAttributes
            {
                DeviceType = deviceType,
                Json = jObject["attributes"]?.ToString(Formatting.Indented)
            }
        };

        JToken attributesToken = jObject["attributes"];
        if (attributesToken != null)
            serializer.Populate(attributesToken.CreateReader(), attributes);

        // Populate all other properties on DirigeraEvent automatically
        var result = new DirigeraStateChangedEventData { Attributes = attributes };
        serializer.Populate(jObject.CreateReader(), result);

        return result;
    }

    public override bool CanWrite => false;

    public override void WriteJson(JsonWriter writer, [AllowNull] DirigeraStateChangedEventData value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class UnknownEventAttributes : EventAttributes
{
    public required string DeviceType { get; set; }
    public required string Json { get; set; }
}

