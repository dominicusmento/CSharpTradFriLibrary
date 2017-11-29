using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using TradFriLibrary.Extensions;
using TradFriLibrary.Models;

namespace TradFriLibrary.Controllers
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

        public DeviceController(long _id, CoapClient _cc)
        {
            id = _id;
            cc = _cc;
        }

        /// <summary>
        /// Get device information from gateway
        /// </summary>
        /// <returns></returns>
        public Response Get()
        {
            return cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Devices}/{id}" });
        }

        public TradFriDevice GetTradFriDevice()
        {
            device = JsonConvert.DeserializeObject<TradFriDevice>(Get().PayloadString);
            return device;
        }

        public Response TurnOff()
        {
            return cc.SetValues(SwitchState(0));
        }
        public Response TurnOn()
        {
            return cc.SetValues(SwitchState(1));
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
    }
}
