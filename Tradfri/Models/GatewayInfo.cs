using Newtonsoft.Json;
using System;
using Tradfri.Extensions;

namespace Tradfri.Models
{
    public class GatewayInfo
    {
        //properties needs naming schema mapping according to https://github.com/ggravlingen/pytradfri/blob/master/pytradfri/const.py
        [JsonProperty("9023")]
        public string NTP { get; set; }

        [JsonProperty("9029")]
        public string Firmware { get; set; }

        [JsonProperty("9054")]
        public long OtaUpdateState { get; set; }

        [JsonProperty("9055")]
        public long GatewayUpdateProgress { get; set; }

        [JsonProperty("9059")]
        public long CurrentTimeUnix { get; set; }

        [JsonProperty("9060")]
        public string CurrentTimeISO8601 { get; set; }

        [JsonProperty("9061")]
        public long CommissioningMode { get; set; }

        [JsonProperty("9062")]
        public long The9062 { get; set; }

        [JsonProperty("9066")]
        public long OtaType { get; set; }

        [JsonProperty("9069")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime FirstSetup { get; set; }

        [JsonProperty("9071")]
        public long GatewayTimeSource { get; set; }

        [JsonProperty("9072")]
        public long The9072 { get; set; }

        [JsonProperty("9073")]
        public long The9073 { get; set; }

        [JsonProperty("9074")]
        public long The9074 { get; set; }

        [JsonProperty("9075")]
        public long The9075 { get; set; }

        [JsonProperty("9076")]
        public long The9076 { get; set; }

        [JsonProperty("9077")]
        public long The9077 { get; set; }

        [JsonProperty("9078")]
        public long The9078 { get; set; }

        [JsonProperty("9079")]
        public long The9079 { get; set; }

        [JsonProperty("9080")]
        public long The9080 { get; set; }

        [JsonProperty("9081")]
        public string GatewayID { get; set; }

        [JsonProperty("9082")]
        public bool The9082 { get; set; }

        [JsonProperty("9083")]
        public string HomekitID { get; set; }

        [JsonProperty("9092")]
        public long The9092 { get; set; }

        [JsonProperty("9093")]
        public long The9093 { get; set; }

        [JsonProperty("9106")]
        public long The9106 { get; set; }
    }
}
