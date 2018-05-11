using Newtonsoft.Json;

namespace Tomidix.CSharpTradFriLibrary.Models
{
    public class TradFriAuth
    {
        [JsonProperty("9091")]
        public string PSK { get; set; }
        [JsonProperty("9029")]
        public string FirmwareVersion { get; set; }
    }
}
