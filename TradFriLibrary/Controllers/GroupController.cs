using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary.Controllers
{
    public class GroupController
    {
        private readonly CoapClient cc;
        private long id { get; }
        private TradFriGroup group { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_id">group id</param>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load group object automatically (default: true)</param>
        public GroupController(long _id, CoapClient _cc, bool loadAutomatically = true)
        {
            id = _id;
            cc = _cc;
            if (loadAutomatically)
                GetTradFriGroup();
        }

        /// <summary>
        /// Get group information from gateway
        /// </summary>
        /// <returns></returns>
        public Response Get()
        {
            return cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Groups}/{id}" });
        }
        /// <summary>
        /// Acquires TradFriGroup object
        /// </summary>
        /// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        /// <returns></returns>
        public TradFriGroup GetTradFriGroup(bool refresh = false)
        {
            if (!refresh && group != null)
                return group;
            group = JsonConvert.DeserializeObject<TradFriGroup>(Get().PayloadString);
            return group;
        }

        /// <summary>
        /// Turn Off Devices in Group
        /// </summary>
        /// <returns></returns>
        public Response TurnOff()
        {
            return cc.SetValues(SwitchState(0));
        }
        /// <summary>
        /// Turn On Devices in Group
        /// </summary>
        /// <returns></returns>
        public Response TurnOn()
        {
            return cc.SetValues(SwitchState(1));
        }

        /// <summary>
        /// Does not work at the moment
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Response SetMood(int value)
        {
            return cc.SetValues(new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Groups}/{id}",
                Payload = string.Format(@"{{""{0}"":{1}}}", (int)TradFriConstAttr.Mood, value)
            });
        }

        /// <summary>
        /// Set Dimmer for Light Devices in Group
        /// </summary>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public Response SetDimmer(int value)
        {
            Response dimmer = cc.SetValues(new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Groups}/{id}",
                Payload = string.Format(@"{{""{0}"":{1}}}", (int)TradFriConstAttr.LightDimmer, value)
            });
            if(group!=null)
                group.LightDimmer = value;
            return dimmer;
        }

        private TradFriRequest SwitchState(int turnOn = 1)
        {
            return new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Groups}/{id}",
                Payload = string.Format(@"{{""{0}"":{1}}}", (int)TradFriConstAttr.LightState, turnOn)
            };
        }

        
    }
}
