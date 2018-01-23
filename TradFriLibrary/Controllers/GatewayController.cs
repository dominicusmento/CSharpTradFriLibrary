using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary.Controllers
{
    public class GatewayController
    {
        private readonly CoapClient cc;
        private GatewayInfo gatewayInfo { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load GatewayInfo object automatically (default: true)</param>
        public GatewayController(CoapClient _cc, bool loadAutomatically = true)
        {
            cc = _cc;
            if (loadAutomatically)
                GetGatewayInfo();
        }
        #region Functions regarding Gateway
        /// <summary>
        /// Acquires GatewayInfo object
        /// </summary>
        /// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        /// <returns></returns>
        public GatewayInfo GetGatewayInfo(bool refresh = false)
        {
            if (!refresh && gatewayInfo != null)
                return gatewayInfo;
            gatewayInfo = JsonConvert.DeserializeObject<GatewayInfo>(cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Gateway}/{(int)TradFriConstAttr.GatewayInfo}" }).PayloadString);
            return gatewayInfo;
        }
        /// <summary>
        /// Reboot the gateway
        /// </summary>
        /// <returns></returns>
        public Response Reboot()
        {
            return cc.SetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Gateway}/{(int)TradFriConstAttr.GatewayReboot}" });
        }
        #endregion

        /// <summary>
        /// Acquire IDs of connected devices
        /// </summary>
        /// <returns></returns>
        public List<long> GetDevices()
        {
            return GetEntityCollectionIDs(TradFriConstRoot.Devices);
        }
        /// <summary>
        /// Acquire IDs of groups
        /// </summary>
        /// <returns></returns>
        public List<long> GetGroups()
        {
            return GetEntityCollectionIDs(TradFriConstRoot.Groups);
        }
        /// <summary>
        /// Acquire TradFriMoods by groups
        /// </summary>
        /// <returns></returns>
        public List<TradFriMood> GetMoods()
        {
            List<TradFriMood> moods = new List<TradFriMood>();
            string groupIDsResponse = cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Moods}" }).PayloadString;
            foreach (int groupID in JsonConvert.DeserializeObject<List<int>>(groupIDsResponse))
            {
                string moodsForGroup = cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Moods}/{groupID}" }).PayloadString;
                foreach (int moodID in JsonConvert.DeserializeObject<List<int>>(moodsForGroup))
                {
                    string moodResponse = cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.Moods}/{groupID}/{moodID}" }).PayloadString;
                    TradFriMood mood = JsonConvert.DeserializeObject<TradFriMood>(moodResponse);
                    mood.GroupID = groupID;
                    moods.Add(mood);
                }
            }
            return moods;
        }

        public List<long> GetSmartTasks()
        {
            return GetEntityCollectionIDs(TradFriConstRoot.SmartTasks);
        }

        public void FactoryReset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Acquire All Resources
        /// </summary>
        /// <returns></returns>
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
