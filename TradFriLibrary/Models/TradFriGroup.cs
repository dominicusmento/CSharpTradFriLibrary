using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tomidix.CSharpTradFriLibrary.Extensions;

namespace Tomidix.CSharpTradFriLibrary.Models
{
    public class TradFriGroup
    {
        [JsonProperty("5850")]
        public long LightState { get; set; }

        [JsonProperty("5851")]
        public long LightDimmer { get; set; }

        [JsonProperty("9001")]
        public string Name { get; set; }

        [JsonProperty("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("9003")]
        public long ID { get; set; }

        [JsonProperty("9018")]
        public The9018 Devices { get; set; }

        [JsonProperty("9039")]
        public long Mood { get; set; }
    }

    public class The9018
    {
        [JsonProperty("15002")]
        public The15002 The15002 { get; set; }
    }

    public class The15002
    {
        [JsonProperty("9003")]
        public List<long> ID { get; set; }
    }

}
