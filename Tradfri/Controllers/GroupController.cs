using ApiLibs.General;
using System.Threading.Tasks;
using Tradfri.Models;

namespace Tradfri.Controllers
{
    public class GroupController : SubService
    {
        //private readonly CoapClient cc;
        //private long id { get; }
        //private TradfriGroup group { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_id">group id</param>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load group object automatically (default: true)</param>
        public GroupController(TradfriController controller) : base(controller) { }

        /// <summary>
        /// Acquires TradfriGroup object
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <returns></returns>
        public async Task<TradfriGroup> GetTradfriGroup(long id)
        {
            return await MakeRequest<TradfriGroup>($"/{(int)TradfriConstRoot.Groups}/{id}");
        }

        public async Task SetDimmer(TradfriGroup group, TradfriMood mood)
        {
            await SetMood(group.ID, mood);
            group.ActiveMood = mood.ID;
        }

        /// <summary>
        /// Sets a mood for the group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="mood">TradfriMood object which needs to be set</param>
        /// <returns></returns>
        public async Task SetMood(long id, TradfriMood mood)
        {
            await HandleRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: mood.MoodProperties[0], statusCode: System.Net.HttpStatusCode.NoContent);

            var set = new SwitchStateRequestOption()
            {
                Mood = mood.ID
            };
            await HandleRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Set Dimmer for Light Devices in Group
        /// </summary>
        /// <param name="group">A <see cref="TradfriGroup"/></param>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public async Task SetDimmer(TradfriGroup group, int value)
        {
            await SetDimmer(group.ID, value);
            group.LightDimmer = value;
        }

        /// <summary>
        /// Set Dimmer for Light Devices in Group
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="value">Dimmer intensity (0-255)</param>
        /// <returns></returns>
        public async Task SetDimmer(long id, int value)
        {
            var set = new SwitchStateRequestOption()
            {
                LightIntensity = value
            };
            await HandleRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set, statusCode: System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Turns a group of lights on or off
        /// </summary>
        /// <param name="group">A <see cref="TradfriGroup"/></param>
        /// <param name="state">On (True) or Off(false)</param>
        /// <returns></returns>
        public Task SetLight(TradfriGroup group, bool state)
        {
            group.LightState = 1;

            return SetLight(group.ID, state);
        }


        /// <summary>
        /// Turns a group of lights on or off
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <param name="state">On (True) or Off(false)</param>
        /// <returns></returns>
        public async Task SetLight(long id, bool state)
        {
            var set = new SwitchStateRequestOption()
            {
                isOn = state ? 1 : 0
            };

            await HandleRequest($"/{(int)TradfriConstRoot.Groups}/{id}", Call.PUT, content: set);
        }
    }
}
