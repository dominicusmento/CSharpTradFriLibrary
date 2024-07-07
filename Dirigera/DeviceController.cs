using ApiLibs.General;
using Tomidix.NetStandard.Dirigera.Devices;
using Newtonsoft.Json;
using ApiLibs;

namespace Tomidix.NetStandard.Dirigera;

public class DeviceController : SubService
{
    private Service service;
    public DeviceController(DirigeraController controller) : base(controller)
    {
        service = controller;
    }

    public Task<List<Device>> GetDevices() => MakeRequest<List<Device>>("devices/").ContinueWith((item) => item.Result.Select(i =>
    {
        i.service = service;
        return i;
    }).ToList());
    public Task<string> GetDevicesJson() => MakeRequest<string>("devices/");

    public Task<string> ChangeAttributes<T>(string deviceId, PostingAttributes<T> attributes) where T : PostingAttributesProperties => MakeRequest<string>("devices/" + deviceId, Call.PATCH, content: new object[] { attributes }, statusCode: System.Net.HttpStatusCode.Accepted);

    public Task<string> Toggle(Light l) => Toggle(l.Id, !l.Attributes.IsOn).ContinueWith((a) =>
    {
        l.Attributes.IsOn = !l.Attributes.IsOn;
        return a.Result;
    });

    public Task<string> Toggle(string id, bool isOn) => ChangeAttributes(id, new PostingAttributes<ToggleProperty>(new ToggleProperty
    {
        IsOn = isOn
    }));

    public Task<string> SetLightLevel(Light l, int level) => SetLightLevel(l.Id, level).ContinueWith((a) =>
        {
            l.Attributes.LightLevel = level;
            return a.Result;
        });

    public Task<string> SetLightLevel(string id, int level) => ChangeAttributes(id, new PostingAttributes<LightLevelProperty>(new LightLevelProperty
    {
        LightLevel = level
    }));


    public Task<string> SetLightTemperature(Light l, int temperature) => Toggle(l.Id, !l.Attributes.IsOn).ContinueWith((a) =>
        {
            l.Attributes.ColorTemperature = temperature;
            return a.Result;
        });

    public Task<string> SetLightTemperature(string id, int temperature) => ChangeAttributes(id, new PostingAttributes<LightTemperatureProperty>(new LightTemperatureProperty
    {
        ColorTemperature = temperature
    }));

}

public class PostingAttributes<T> where T : PostingAttributesProperties
{
    public PostingAttributes(T properties, int? transitionTime = null)
    {
        // TransitionTime = transitionTime;
        Attributes = properties;
    }

    [JsonProperty("attributes")]
    public T Attributes { get; set; }


    [JsonProperty("transitionTime")]
    public int? TransitionTime { get; set; }
}

public interface PostingAttributesProperties
{

}

public class ToggleProperty : PostingAttributesProperties
{
    [JsonProperty("isOn")]
    public required bool IsOn { get; set; }
}


public class LightLevelProperty : PostingAttributesProperties
{
    [JsonProperty("lightLevel")]
    public required int LightLevel { get; set; }
}

public class LightTemperatureProperty : PostingAttributesProperties
{
    [JsonProperty("colorTemperature")]
    public required int ColorTemperature { get; set; }
}


