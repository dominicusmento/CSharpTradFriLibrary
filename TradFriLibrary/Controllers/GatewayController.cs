using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary.Controllers
{
    public class GatewayController
    {
        private readonly CoapClient cc;
        private GatewayInfo gatewayInfo { get; set; }
        public GatewayController(CoapClient _cc)
        {
            cc = _cc;
        }
        #region Functions regarding Gateway
        public GatewayInfo GetGatewayInfo()
        {
            gatewayInfo = JsonConvert.DeserializeObject<GatewayInfo>(cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Gateway}/{(int)TradFriConstAttr.GatewayInfo}" }).PayloadString);
            return gatewayInfo;
        }
        public Response Reboot()
        {
            return cc.SetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Gateway}/{(int)TradFriConstAttr.GatewayReboot}" });
        }
        #endregion


        public List<long> GetDevices()
        {
            return GetEntityCollectionIDs(TradFriConstRoot.Devices);
        }

        public List<long> GetGroups()
        {
            return GetEntityCollectionIDs(TradFriConstRoot.Groups);
        }
        
        public List<WebLink> GetResources()
        {
            return cc.Discover().ToList();
        }

        private List<long> GetEntityCollectionIDs(TradFriConstRoot rootConst)
        {
            return JsonConvert.DeserializeObject<List<long>>(cc.GetValues(new TradFriRequest { UriPath = $"/{(int)rootConst}" }).PayloadString);
        }
    }
}
