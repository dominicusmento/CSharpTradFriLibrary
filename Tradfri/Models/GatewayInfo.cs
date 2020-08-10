using Newtonsoft.Json;
using System;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
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
        public long DST_StartMonth { get; set; }

        [JsonProperty("9073")]
        public long DST_StartDay { get; set; }

        [JsonProperty("9074")]
        public long DST_StartHour { get; set; }

        [JsonProperty("9075")]
        public long DST_StartMinute { get; set; }

        [JsonProperty("9076")]
        public long DST_EndMonth { get; set; }

        [JsonProperty("9077")]
        public long DST_EndDay { get; set; }

        [JsonProperty("9078")]
        public long DST_EndHour { get; set; }

        [JsonProperty("9079")]
        public long DST_EndMinute { get; set; }

        [JsonProperty("9080")]
        public long DST_TimeOffset { get; set; }

        [JsonProperty("9081")]
        public string GatewayID { get; set; }

        [JsonProperty("9082")]
        public bool The9082 { get; set; }

        [JsonProperty("9083")]
        public string HomekitID { get; set; }

        [JsonProperty("9092")]
        public long CertificateProv { get; set; }

        [JsonProperty("9093")]
        public long AlexaPairStatus { get; set; }

        [JsonProperty("9106")]
        public long The9106 { get; set; }
    }
}
