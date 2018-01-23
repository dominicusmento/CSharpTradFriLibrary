using Newtonsoft.Json;
using System;
using Tomidix.CSharpTradFriLibrary.Extensions;

namespace Tomidix.CSharpTradFriLibrary.Models
{
    public class TradFriSmartTask
    {
        [JsonProperty("5850")]
        public long LightState { get; set; }

        [JsonProperty("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("9003")]
        public long ID { get; set; }

        [JsonProperty("9040")]
        public TradFriConstAttr TaskType { get; set; }

        [JsonProperty("9041")]
        public long RepeatDays { get; set; }

        [JsonProperty("9042")]
        public TradFriAction ActionTurnOn { get; set; }

        [JsonProperty("9043")]
        public TradFriAction ActionTurnOff { get; set; }

        [JsonProperty("9044")]
        public SmartTaskTrigger[] TriggerTimeInterval { get; set; }
    }

    public class SmartTaskTrigger
    {
        [JsonProperty("9046")]
        public long StartHour { get; set; }

        [JsonProperty("9047")]
        public long StartMin { get; set; }

        [JsonProperty("9048")]
        public long EndHour { get; set; }

        [JsonProperty("9049")]
        public long EndMin { get; set; }
    }

    public class TradFriAction
    {
        [JsonProperty("15013")]
        public TradFriDevice[] Devices { get; set; }

        [JsonProperty("5850")]
        public long LightState { get; set; }
    }
}
