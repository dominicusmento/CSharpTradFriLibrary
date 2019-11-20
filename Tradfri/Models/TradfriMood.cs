using Newtonsoft.Json;
using System;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public class TradfriMood
    {
        [JsonProperty("15013")]
        public TradfriMoodProperties[] MoodProperties { get; set; }

        [JsonProperty("9001")]
        public string Name { get; set; }

        [JsonProperty("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("9003")]
        public long ID { get; set; }

        [JsonProperty("9057")]
        public long The9057 { get; set; }

        [JsonProperty("9068")]
        public long The9068 { get; set; }

        public long GroupID { get; set; }
    }

    public class TradfriMoodProperties
    {
        [JsonProperty("5706")]
        public string ColorHex { get; set; }

        [JsonProperty("5707")]
        public long ColorSaturation { get; set; }

        [JsonProperty("5708")]
        public long ColorHue { get; set; }

        [JsonProperty("5709")]
        public long ColorX { get; set; }

        [JsonProperty("5710")]
        public long ColorY { get; set; }

        [JsonProperty("5711")]
        public long Mireds { get; set; }

        [JsonProperty("5712")]
        public long TransitionTime { get; set; }

        [JsonProperty("5850")]
        public long LightState { get; set; }

        [JsonProperty("5851")]
        public long Dimmer { get; set; }

        [JsonProperty("9003")]
        public long ID { get; set; }
    }
}
