using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System;
using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary.Controllers
{
    public class DeviceController
    {
        private readonly CoapClient cc;
        private long id { get; }
        private TradFriDevice device { get; set; }
        public bool HasLight
        {
            get { return device?.LightControl != null; }
        }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_id">device id</param>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load device object automatically (default: true)</param>
        public DeviceController(long _id, CoapClient _cc, bool loadAutomatically = true)
        {
            id = _id;
            cc = _cc;
            if (loadAutomatically)
                GetTradFriDevice();
        }

        /// <summary>
        /// Get device information from gateway
        /// </summary>
        /// <returns></returns>
        public Response Get()
        {
            return cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Devices}/{id}" });
        }

        /// <summary>
        /// Acquires TradFriDevice object
        /// </summary>
        /// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        /// <returns></returns>
        public TradFriDevice GetTradFriDevice(bool refresh = false)
        {
            if (!refresh && device != null)
                return device;
            device = JsonConvert.DeserializeObject<TradFriDevice>(Get().PayloadString);
            return device;
        }

        /// <summary>
        /// Observes a device and gets update notifications
        /// </summary>
        /// <param name="callback">Action to take for each device update</param>
        /// <param name="error">Action to take on internal error</param>
        /// <returns></returns>
        public CoapObserveRelation ObserveDevice(Action<TradFriDevice> callback, Action<CoapClient.FailReason> error = null)
        {
            Action<Response> update = delegate (Response response) {
                OnDeviceObservationUpdate(response, callback);
            };
            return cc.Observe(
                new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Devices}/{id}" },
                update,
                error
            );
        }

        /// <summary>
        /// Turn Off Device
        /// </summary>
        /// <returns></returns>
        public Response TurnOff()
        {
            Response deviceResponse = cc.UpdateValues(SwitchState(0));
            if (HasLight && deviceResponse.CodeString.Equals("2.04 Changed"))
                device.LightControl[0].State = Bool.False;
            return deviceResponse;
        }
        /// <summary>
        /// Turn On Device
        /// </summary>
        /// <returns></returns>
        public Response TurnOn()
        {
            Response deviceResponse = cc.UpdateValues(SwitchState(1));
            if (HasLight && deviceResponse.CodeString.Equals("2.04 Changed"))
                device.LightControl[0].State = Bool.True;
            return deviceResponse;
        }

        /// <summary>
        /// Changes the color of the light device
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Response SetColor(string value)
        {
            Response deviceColor = cc.UpdateValues(new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Devices}/{id}",
                Payload = string.Format(@"{{""{0}"":[{{ ""{1}"":""{2}""}}]}}", (int)TradFriConstAttr.LightControl, (int)TradFriConstAttr.LightColorHex, value)
            });
            if (HasLight && deviceColor.CodeString.Equals("2.04 Changed"))
                device.LightControl[0].ColorHex = value;
            return deviceColor;
        }
        /// <summary>
        /// Set Dimmer for Light Device
        /// </summary>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public Response SetDimmer(int value)
        {
            Response deviceDimmer = cc.UpdateValues(new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Devices}/{id}",
                Payload = string.Format(@"{{""{0}"":[{{ ""{1}"":{2}}}]}}", (int)TradFriConstAttr.LightControl, (int)TradFriConstAttr.LightDimmer, value)
            });
            if (HasLight && deviceDimmer.CodeString.Equals("2.04 Changed"))
                device.LightControl[0].Dimmer = value;
            return deviceDimmer;
        }

        private TradFriRequest SwitchState(int turnOn = 1)
        {
            if (HasLight)
            {
                device.LightControl[0].State = (Bool)turnOn;
            }
            return new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Devices}/{id}",
                Payload = string.Format(@"{{""{0}"":[{{ ""{1}"":{2}}}]}}", (int)TradFriConstAttr.LightControl, (int)TradFriConstAttr.LightState, turnOn)  //@"{ ""3311"":[{ ""5850"":" + turnOn + "}]}"
            };
        }

        private void OnDeviceObservationUpdate(Response response, Action<TradFriDevice> callback)
        {
            device = JsonConvert.DeserializeObject<TradFriDevice>(response.PayloadString);
            callback.Invoke(device);
        }
    }
}
