using System.Collections.Generic;

namespace TradfriUI.Settings
{
    public class UserData
    {
        public UserData()
        {
            SelectedDevices = new List<long>();
        }

        public UserData(List<long> selectedDevices)
        {
            SelectedDevices = selectedDevices;
        }

        public List<long> SelectedDevices { get; set; }
    }
}
