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
        public GroupController(long _id, CoapClient _cc)
        {
            id = _id;
            cc = _cc;
        }

        /// <summary>
        /// Get group information from gateway
        /// </summary>
        /// <returns></returns>
        public Response Get()
        {
            return cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Groups}/{id}" });
        }

        public TradFriGroup GetTradFriGroup()
        {
            group = JsonConvert.DeserializeObject<TradFriGroup>(Get().PayloadString);
            return group;
        }

        public Response TurnOff()
        {
            return cc.SetValues(SwitchState(0));
        }

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

        public Response SetDimmer(int value)
        {
            return cc.SetValues(new TradFriRequest
            {
                UriPath = $"/{(int)TradFriConstRoot.Groups}/{id}",
                Payload = string.Format(@"{{""{0}"":{1}}}", (int)TradFriConstAttr.LightDimmer, value)
            });
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
