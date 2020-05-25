using ApiLibs.General;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cms;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tomidix.NetStandard.Tradfri.Models;

namespace Tomidix.NetStandard.Tradfri.Controllers
{
    public class GatewayController : SubService
    {
        private TradfriController mainController;

        public GatewayController(TradfriController controller) : base(controller)
        {
            this.mainController = controller;
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

        public async Task ForceCheckOTAUpdate()
        {
            AddDeviceRequest set = new AddDeviceRequest()
            {
                CommissioningMode = 5000
            };
            await MakeRequest<object>($"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.GatewayInfo}", Call.PUT, content: set);
        }

        /// <summary>
        /// Put gateway into pairing mode so you can pair the device
        /// </summary>
        /// <param name="timeout">Set the duration of the pairing process for gateway to wait</param>
        /// <returns></returns>
        public async Task AddDevice(int timeout = 60)
        {
            AddDeviceRequest set = new AddDeviceRequest()
            {
                CommissioningMode = timeout
            };
            await MakeRequest<object>($"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.OtaForceCheck}", Call.PUT, content: set);
        }

        /// <summary>
        /// Put gateway into pairing mode so you can pair the device
        /// </summary>
        /// <param name="groupId">Id of the group</param>
        /// <param name="timeout">Set the duration of the pairing process for gateway to wait</param>
        /// <returns></returns>
        public async Task AddDevice(long groupId, int timeout)
        {
            AddDeviceGroupRequest set = new AddDeviceGroupRequest()
            {
                CommissioningGroupID = groupId,
                CommissioningMode = timeout
            };
            await MakeRequest<object>($"/{(int)TradfriConstRoot.Gateway}/{(int)TradfriConstAttr.GatewayInfo}", Call.PUT, content: set);
        }

        #endregion Functions regarding Gateway

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
        public async Task<List<TradfriDevice>> GetDeviceObjects()
        {
            List<TradfriDevice> devices = new List<TradfriDevice>();
            foreach (long item in await GetEntityCollectionIDs(TradfriConstRoot.Devices))
            {
                devices.Add(await mainController.DeviceController.GetTradfriDevice(item));
            }

            return devices;
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
        public async Task<List<TradfriGroup>> GetGroupObjects()
        {
            List<TradfriGroup> groups = new List<TradfriGroup>();
            foreach (long item in await GetEntityCollectionIDs(TradfriConstRoot.Groups))
            {
                groups.Add(await mainController.GroupController.GetTradfriGroup(item));
            }
            return groups;
        }

        public Task<List<long>> GetSmartTasks()
        {
            return GetEntityCollectionIDs(TradfriConstRoot.SmartTasks);
        }

        public async Task<List<TradfriSmartTask>> GetSmartTaskObjects()
        {
            List<TradfriSmartTask> smartTasks = new List<TradfriSmartTask>();
            foreach (long item in await GetEntityCollectionIDs(TradfriConstRoot.SmartTasks))
            {
                smartTasks.Add(await mainController.SmartTasksController.GetTradfriSmartTask(item));
            }
            return smartTasks;
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

        public void FactoryReset()
        {
            throw new System.NotImplementedException();
        }

        private Task<List<long>> GetEntityCollectionIDs(TradfriConstRoot rootConst)
        {
            return MakeRequest<List<long>>($"/{(int)rootConst}");
        }
    }
    internal class AddDeviceRequest
    {
        [JsonProperty("9061")]
        public int CommissioningMode { get; set; }
    }
    internal class AddDeviceGroupRequest : AddDeviceRequest
    {
        [JsonProperty("9064")]
        public long CommissioningGroupID { get; set; }
    }
}
