using Newtonsoft.Json;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class TradfriAuth
    {
        [JsonProperty("9091")]
        public string PSK { get; set; }
        [JsonProperty("9029")]
        public string FirmwareVersion { get; set; }
    }
}
