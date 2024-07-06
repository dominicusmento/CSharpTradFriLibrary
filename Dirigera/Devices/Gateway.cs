using Newtonsoft.Json;

namespace Tomidix.NetStandard.Dirigera.Devices;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

public class Gateway : Device
{
    [JsonProperty("relationId")]
    public string RelationId { get; set; }

    [JsonProperty("deviceSet")]
    public object[] DeviceSet { get; set; }

    [JsonProperty("remoteLinks")]
    public object[] RemoteLinks { get; set; }

    [JsonProperty("attributes")]
    public GatewayAttributes Attributes { get; set; }

    public override string ToString()
    {
        return Attributes.CustomName;
    }
}

public partial class GatewayAttributes : Attributes
{
    [JsonProperty("backendConnected")]
    public bool BackendConnected { get; set; }

    [JsonProperty("backendConnectionPersistent")]
    public bool BackendConnectionPersistent { get; set; }

    [JsonProperty("backendOnboardingComplete")]
    public bool BackendOnboardingComplete { get; set; }

    [JsonProperty("backendRegion")]
    public string BackendRegion { get; set; }

    [JsonProperty("backendCountryCode")]
    public string BackendCountryCode { get; set; }

    [JsonProperty("userConsents")]
    public UserConsent[] UserConsents { get; set; }

    [JsonProperty("logLevel")]
    public long LogLevel { get; set; }

    [JsonProperty("coredump")]
    public bool Coredump { get; set; }

    [JsonProperty("timezone")]
    public string Timezone { get; set; }

    [JsonProperty("nextSunSet")]
    public object NextSunSet { get; set; }

    [JsonProperty("nextSunRise")]
    public object NextSunRise { get; set; }

    [JsonProperty("homestate")]
    public string Homestate { get; set; }

    [JsonProperty("countryCode")]
    public string CountryCode { get; set; }

    [JsonProperty("isOn")]
    public bool IsOn { get; set; }
}

public partial class UserConsent
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
