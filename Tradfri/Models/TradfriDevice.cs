using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tomidix.NetStandard.Tradfri.Extensions;

namespace Tomidix.NetStandard.Tradfri.Models
{
    public enum DeviceType
    {
        Remote = 0,
        Unknown_1 = 1,
        Light = 2,
        ControlOutlet = 3,
        MotionSensor = 4,
        Unknown_3 = 5,
        Unknown_4 = 6,
        Unknown_5 = 7,
        Unknown_6 = 8,
        Unknown_7 = 9,
        Unknown_8 = 10,
        Unknown_9 = 11
    }

    public enum PowerSource
    {
        DCPower = 0,
        InternalBattery = 1,
        ExternalBattery = 2,
        Battery = 3, //used by motion sensor
        POE = 4, //power over ethernet
        USB = 5,
        ACPower = 6,
        Solar = 7,
        Unknown_1 = 8,
        Unknown_2 = 9,
    }

    public class TradfriDevice
    {
        [JsonProperty("15009")]
        public List<RootSwitch> RootSwitch { get; set; }

        [JsonProperty("3")]
        public DeviceInfo Info { get; set; }

        //0 - remote, 2 - light
        [JsonProperty("5750")]
        public DeviceType DeviceType { get; set; }

        [JsonProperty("9001")]
        public string Name { get; set; }

        [JsonProperty("9002")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("9003")]
        public long ID { get; set; }

        [JsonProperty("9019")]
        public long ReachableState { get; set; }

        [JsonProperty("9020")]
        [JsonConverter(typeof(MicrosecondEpochConverter))]
        public DateTime LastSeen { get; set; }

        [JsonProperty("9054")]
        public long OtaUpdateState { get; set; }

        [JsonProperty("3311")]
        public List<LightControl> LightControl { get; set; }
        [JsonProperty("15015")]
        public List<BlindControl> BlindControl { get; set; }
        [JsonProperty("3312")]
        public List<Control> Control { get; set; }
    }

    public class DeviceInfo
    {
        [JsonProperty("0")]
        public string Manufacturer { get; set; }

        [JsonProperty("1")]
        public string DeviceType { get; set; }

        [JsonProperty("2")]
        public string Serial { get; set; }

        [JsonProperty("3")]
        public string FirmwareVersion { get; set; }

        [JsonProperty("6")]
        public PowerSource PowerSource { get; set; }

        [JsonProperty("9")]
        public long Battery { get; set; }
    }

    public class RootSwitch
    {
        [JsonProperty("9003")]
        public long RootSwitchID { get; set; }
    }

    public class Control
    {
        [JsonProperty("5850")]
        public Bool State { get; set; }

        [JsonProperty("5851")]
        public long Dimmer { get; set; }

        [JsonProperty("9003")]
        public long ID { get; set; }
    }

    public class LightControl : Control
    {
        [JsonProperty("5706")]
        public string ColorHex { get; set; }

        [JsonProperty("5707")]
        public long ColorHue { get; set; }

        [JsonProperty("5708")]
        public long ColorSaturation { get; set; }

        [JsonProperty("5709")]
        public long ColorX { get; set; }

        [JsonProperty("5710")]
        public long ColorY { get; set; }

        [JsonProperty("5711")]
        public long Mireds { get; set; }
    }

    public class BlindControl
    {
        [JsonProperty("5536")]
        public decimal Position { get; set; }
    }

    public class DeviceList
    {
        [JsonProperty("9003")]
        public List<long> DevicesIds { get; set; }
    }
}
