using ApiLibs.General;
using Com.AugustCellars.CoAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tradfri.Models;

namespace Tradfri.Controllers
{
    public class SmartTaskController : SubService
    {
        public SmartTaskController(TradfriController controller) : base(controller) { }

        /// <summary>
        /// Acquires TradfriGroup object
        /// </summary>
        /// <param name="id">Id of the group</param>
        /// <returns></returns>
        public Task<TradfriSmartTask> GetTradfriSmartTask(long id)
        {
            return MakeRequest<TradfriSmartTask>($"/{(int)TradfriConstRoot.SmartTasks}/{id}");
        }

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
