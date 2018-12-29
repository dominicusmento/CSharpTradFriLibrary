using ApiLibs.General;
using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Tradfri.Models;

namespace Tradfri.Controllers
{
    public class DeviceController : SubService
    {
        public DeviceController(TradfriController controller) : base(controller) { }

        /// <summary>
        // Acquires TradfriDevice object
        /// </summary>
        /// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        /// <returns></returns>
        public Task<TradfriDevice> GetTradfriDevice(long id)
        {
            return MakeRequest<TradfriDevice>($"/{(int)TradfriConstRoot.Devices}/{id}");
        }

        private bool HasLight(TradfriDevice device)
        {
            return device?.LightControl != null;
        }

        /// <summary>
        /// Changes the color of the light device
        /// </summary>
        /// <param name="device">A <see cref="TradfriDevice"/></param>
        /// <param name="value">A color from the <see cref="TradfriColors"/> class</param>
        /// <returns></returns>
        public async Task SetColor(TradfriDevice device, string value)
        {
            await SetColor(device.ID, value);
            if(HasLight(device))
            {
                device.LightControl[0].ColorHex = value;
            }
        }


        /// <summary>
        /// Changes the color of the light device
        /// </summary>
        /// <param name="id">Id of the device</param>
        /// <param name="value">A color from the <see cref="TradfriColors"/> class</param>
        /// <returns></returns>
        public async Task SetColor(long id, string value)
        {
            var set = new SwitchStateRequest()
            {
                Options = new[]
                {
                    new SwitchStateRequestOption()
                    {
                        Color = value
                    }
                }
            };
            await HandleRequest($"/{(int)TradfriConstRoot.Devices}/{id}", Call.PUT, content: set, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        public async Task SetDimmer(TradfriDevice device, int value)
        {
            await SetDimmer(device.ID, value);
            device.LightControl[0].Dimmer = value;
        }

        /// <summary>
        /// Set Dimmer for Light Device
        /// </summary>
        /// <param name="id">Id of the device</param>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public async Task SetDimmer(long id, int value)
        {
            var set = new SwitchStateRequest()
            {
                Options = new[]
                {
                    new SwitchStateRequestOption()
                    {
                        LightIntensity = value
                    }
                }
            };
            await HandleRequest($"/{(int)TradfriConstRoot.Devices}/{id}", Call.PUT, content: set, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Turns a specific light on or off
        /// </summary>
        /// <param name="device">A <see cref="TradfriDevice"/></param>
        /// <param name="state">On (True) or Off(false)</param>
        /// <returns></returns>
        public async Task SetLight(TradfriDevice device, bool state)
        {
            await SetLight(device.ID, state);
            if (HasLight(device))
            {
                device.LightControl[0].State = state ? Bool.True : Bool.False;
            }
        }


        /// <summary>
        /// Turns a specific light on or off
        /// </summary>
        /// <param name="id">Id of the device</param>
        /// <param name="state">On (True) or Off(false)</param>
        /// <returns></returns>
        public async Task SetLight(long id, bool state)
        {
            var set = new SwitchStateRequest()
            {
                Options = new[]
                {
                    new SwitchStateRequestOption()
                    {
                        isOn = state ? 1 : 0
                    }
                }
            };
            await HandleRequest($"/{(int)TradfriConstRoot.Devices}/{id}", Call.PUT, content: set);
        }


    }

    class SwitchStateRequest
    {
        [JsonProperty("3311")]
        public SwitchStateRequestOption[] Options { get; set; }
    }

    class SwitchStateRequestOption
    {
        [JsonProperty("5850")] //TradfriConstAttr.LightState
        public int? isOn { get; set; }

        [JsonProperty("5851")] //TradfriConstAttr.LightDimmer
        public int? LightIntensity { get; set; }

        [JsonProperty("5706")] 
        public string Color { get; set; }

        [JsonProperty("5851")] //TradfriConstAttr.Mood
        public long? Mood { get; set; }


    }
}
