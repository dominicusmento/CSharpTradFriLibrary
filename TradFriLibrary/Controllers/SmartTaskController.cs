using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Tomidix.CSharpTradFriLibrary.Models;

namespace Tomidix.CSharpTradFriLibrary.Controllers
{
    public class SmartTaskController
    {
        private readonly CoapClient cc;
        private long id { get; }
        private TradFriSmartTask task { get; set; }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="_id">group id</param>
        /// <param name="_cc">existing coap client</param>
        /// <param name="loadAutomatically">Load group object automatically (default: true)</param>
        public SmartTaskController(long _id, CoapClient _cc, bool loadAutomatically = true)
        {
            id = _id;
            cc = _cc;
            if (loadAutomatically)
                GetTradFriSmartTask();
        }

        /// <summary>
        /// Get group information from gateway
        /// </summary>
        /// <returns></returns>
        public Response Get()
        {
            return cc.GetValues(new TradFriRequest { UriPath = $"/{(int)TradFriConstRoot.SmartTasks}/{id}" });
        }
        /// <summary>
        /// Acquires TradFriGroup object
        /// </summary>
        /// <param name="refresh">If set to true, than it will ignore existing cached value and ask the gateway for the object</param>
        /// <returns></returns>
        public TradFriSmartTask GetTradFriSmartTask(bool refresh = false)
        {
            if (!refresh && task != null)
                return task;
            task = JsonConvert.DeserializeObject<TradFriSmartTask>(Get().PayloadString);
            return task;
        }

        public List<string> GetSelectedRepeatDays()
        {
            List<string> days = new List<string>();
            if (task?.RepeatDays > 0)
            {
                int tempDaysVariable = (int)task.RepeatDays;
                Array daysArray = Enum.GetValues(typeof(Days));
                //Array.Reverse(daysArray);
                string selectedDaysBinary = Convert.ToString(task.RepeatDays, 2);
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
