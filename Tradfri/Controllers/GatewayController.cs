using ApiLibs;
using ApiLibs.General;
using Com.AugustCellars.CoAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tradfri.Models;

namespace Tradfri.Controllers
{
    public class GatewayController : SubService
    {
        private TradfriController mainController;

        public GatewayController(TradfriController controller) : base(controller) {
            this.mainController = controller;
        }

        protected virtual async Task<T> MakeRequest<T>(int url, Call m = Call.GET, List<Param> parameters = null, List<Param> header = null, object content = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return await MakeRequest<T>(url.ToString(), m, parameters, header, content, statusCode);
        }

        #region Functions regarding Gateway
        /// <summary>
        /// Acquires GatewayInfo object
        /// </summary>
        /// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        /// <returns></returns>
        public async Task<GatewayInfo> GetGatewayInfo()
        {
            return await MakeRequest<GatewayInfo>($"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.GatewayInfo}");
        }

        /// <summary>
        /// Reboot the gateway
        /// </summary>
        /// <returns></returns>
        public async Task Reboot()
        {
            await MakeRequest<string>($"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.GatewayReboot}", Call.POST);
        }
        #endregion

        /// <summary>
        /// Acquire IDs of connected devices
        /// </summary>
        /// <returns></returns>
        public Task<List<long>> GetDevices()
        {
            return GetEntityCollectionIDs(TradfriConstRoot.Devices);
        }


        /// <summary>
        /// Acquire all groups
        /// </summary>
        /// <returns></returns>
        public async Task<TradfriDevice[]> GetDeviceObjects()
        {
            var tasks = (await GetEntityCollectionIDs(TradfriConstRoot.Devices)).Select(i => mainController.DeviceController.GetTradfriDevice(i));
            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Acquire IDs of groups
        /// </summary>
        /// <returns></returns>
        public Task<List<long>> GetGroups()
        {
            return GetEntityCollectionIDs(TradfriConstRoot.Groups);
        }

        /// <summary>
        /// Acquire all groups
        /// </summary>
        /// <returns></returns>
        public async Task<TradfriGroup[]> GetGroupObjects()
        {
            var tasks = (await GetEntityCollectionIDs(TradfriConstRoot.Groups)).Select(i => mainController.GroupController.GetTradfriGroup(i));
            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Acquire TradfriMoods by groups
        /// </summary>
        /// <returns></returns>
        public async Task<List<TradfriMood>> GetMoods()
        {
            List<TradfriMood> moods = new List<TradfriMood>();
            foreach (int groupID in await MakeRequest<List<int>>($"/{(int)TradfriConstRoot.Moods}"))
            {
                foreach (int moodID in await MakeRequest<List<int>>($"/{(int)TradfriConstRoot.Moods}/{groupID}"))
                {
                    TradfriMood mood = await MakeRequest<TradfriMood>($"/{(int)TradfriConstRoot.Moods}/{groupID}/{moodID}");
                    mood.GroupID = groupID;
                    moods.Add(mood);
                }
            }
            return moods;
        }

        public Task<List<long>> GetSmartTasks()
        {
            return GetEntityCollectionIDs(TradfriConstRoot.SmartTasks);
        }

        public void FactoryReset()
        {
            throw new NotImplementedException();
        }

        private Task<List<long>> GetEntityCollectionIDs(TradfriConstRoot rootConst)
        {
            return MakeRequest<List<long>>($"/{(int)rootConst}");
        }
    }
}
