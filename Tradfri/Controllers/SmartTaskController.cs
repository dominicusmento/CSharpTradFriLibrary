using ApiLibs.General;
using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tradfri.Models;

namespace Tradfri.Controllers
{
    public class SmartTaskController : SubService
    {
        //private readonly CoapClient cc;
        //private long id { get; }
        //private TradfriSmartTask task { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_id">group id</param>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load group object automatically (default: true)</param>
        public SmartTaskController(TradfriController controller) : base(controller)
        {
            //id = _id;
            //cc = _cc;
            //if (loadAutomatically)
            //    GetTradfriSmartTask();
        }

        /// <summary>
        /// Get group information from gateway
        /// </summary>
        /// <returns></returns>
        //public Response Get()
        //{
        //    return cc.GetValues(new TradfriRequest { UriPath = $"/{(int)TradfriConstRoot.SmartTasks}/{id}" });
        //}

        ///// <summary>
        ///// Acquires TradfriGroup object
        ///// </summary>
        ///// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        ///// <returns></returns>
        //public TradfriSmartTask GetTradfriSmartTask(bool refresh = false)
        //{
        //    if (!refresh && task != null)
        //        return task;
        //    task = JsonConvert.DeserializeObject<TradfriSmartTask>(Get().PayloadString);
        //    return task;
        //}

        public List<string> GetSelectedRepeatDays(TradfriSmartTask task)
        {
            return GetSelectedRepeatDays(task.RepeatDays);
        }

        public List<string> GetSelectedRepeatDays(long repeatDays)
        {
            List<string> days = new List<string>();
            if (repeatDays > 0)
            {
                int tempDaysVariable = (int)repeatDays;
                Array daysArray = Enum.GetValues(typeof(Days));
                //Array.Reverse(daysArray);
                string selectedDaysBinary = Convert.ToString(repeatDays, 2);
                int currentSign = selectedDaysBinary.Length - 1;
                if (currentSign >= 0)
                {
                    foreach (Days currentDay in daysArray)
                    {
                        if (selectedDaysBinary[currentSign].Equals('1'))
                        {
                            days.Add(currentDay.ToString());
                        }
                        currentSign--;
                        if (currentSign < 0)
                            break;
                    }
                }
            }
            return days;
        }




    }
}
