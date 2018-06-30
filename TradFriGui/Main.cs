using Com.AugustCellars.CoAP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Tomidix.CSharpTradFriLibrary;
using Tomidix.CSharpTradFriLibrary.Controllers;
using Tomidix.CSharpTradFriLibrary.Models;

namespace TradFriGui
{
    public partial class Main : Form
    {
        private List<TradFriDevice> _devices;
        private TradFriCoapConnector _gatewayConnection;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            Initialize();
            InitializePsk();
            Connect();
            LoadAllDevices();
            ShowDGVData();
        }

        private void Initialize()
        {
            _devices = new List<TradFriDevice>();
            _gatewayConnection = new TradFriCoapConnector(Properties.Settings.Default.GatewayName, Properties.Settings.Default.GatewayIP);
        }

        private void InitializePsk()
        {
            try
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.PSK))
                {
                    TradFriAuth appSecret = _gatewayConnection.GeneratePSK(Properties.Settings.Default.GatewaySecret, Properties.Settings.Default.AppName);
                    Properties.Settings.Default.PSK = appSecret.PSK;
                    Properties.Settings.Default.Save();
                }

            }
            catch(Exception exception)
            {
                Console.WriteLine("Unable to create PSK Key: " + exception);
            }
        }
        private void Connect()
        {
            _gatewayConnection.ConnectAppKey(Properties.Settings.Default.PSK, Properties.Settings.Default.AppName);
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // dispose, close connections, etc.. not needed
        }

        private void LoadAllDevices()
        {
            GatewayController gwc = new GatewayController(_gatewayConnection.Client);
            foreach (long deviceID in gwc.GetDevices())
            {
                DeviceController dc = new DeviceController(deviceID, _gatewayConnection.Client);
                TradFriDevice device = dc.GetTradFriDevice();
                _devices.Add(device);
            }
        }

        //set dimmer to specific group example
        private List<TradFriGroup> AcquireGroups()
        {
            GatewayController gwc = new GatewayController(_gatewayConnection.Client);
            List<TradFriGroup> groups = new List<TradFriGroup>();
            foreach (long groupID in gwc.GetGroups())
            {
                GroupController gc = new GroupController(groupID, _gatewayConnection.Client);
                if (groupID == 143700)
                {
                    gc.SetDimmer(230);
                }
                //not neccessary for controlling the group, it is used when we need the group properties
                TradFriGroup group = gc.GetTradFriGroup();
                groups.Add(group);
            }
            return groups;
        }

        private void ShowDGVData()
        {
            dgvDevices.DataSource = _devices;
            dgvDevices.AutoGenerateColumns = true;
        }

        // Set mood example
        private void SetMood()
        {
            GatewayController gwc = new GatewayController(_gatewayConnection.Client);
            List<TradFriMood> moods = gwc.GetMoods();

            List<TradFriGroup> groups = new List<TradFriGroup>();
            foreach (long groupID in gwc.GetGroups())
            {
                GroupController gc = new GroupController(groupID, _gatewayConnection.Client);
                //not neccessary for controlling the group, it is used when we need the group properties
                TradFriGroup group = gc.GetTradFriGroup();
                //check if group name property is 'TestGroup'
                if (group.Name.Equals("TestGroup"))
                {
                    List<TradFriMood> groupMoods = moods.Where(x => x.GroupID.Equals(groupID)).ToList();
                    //gc.SetDimmer(230);
                    //set mood
                    Response response = gc.SetMood(groupMoods[2]);
                }
                groups.Add(group);
            }
        }

        // set color example
        private void SetColor()
        {
            TradFriDevice deviceToChangeProperties = _devices[0];
            DeviceController dc = new DeviceController(deviceToChangeProperties.ID, _gatewayConnection.Client);
            dc.SetColor(TradFriColors.CoolDaylight);
        }

        // set color example
        private void GetSmartTasks()
        {
            GatewayController gwc = new GatewayController(_gatewayConnection.Client);
            foreach (long smartTaskID in gwc.GetSmartTasks())
            {
                SmartTaskController stc = new SmartTaskController(smartTaskID, _gatewayConnection.Client);
                stc.GetTradFriSmartTask();
                stc.GetSelectedRepeatDays();
            }
        }

        private void btnTurnOn_Click(object sender, EventArgs e)
        {
            // turn on the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradFriDevice currentSelectedDevice = (TradFriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                DeviceController dc = new DeviceController(currentSelectedDevice.ID, _gatewayConnection.Client);
                dc.TurnOn();
            }
        }

        private void btnTurnOff_Click(object sender, EventArgs e)
        {
            // turn off the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradFriDevice currentSelectedDevice = (TradFriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                DeviceController dc = new DeviceController(currentSelectedDevice.ID, _gatewayConnection.Client);
                dc.TurnOff();
            }
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            GatewayController gwc = new GatewayController(_gatewayConnection.Client);
            foreach (long groupID in gwc.GetGroups())
            {
                GroupController gc = new GroupController(groupID, _gatewayConnection.Client);
                TradFriGroup currentGroup = gc.GetTradFriGroup();
                if (currentGroup.Name.Contains("Test"))
                    gc.TurnOff();
            }
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            // manual testing method for acquiring data / testing directly while developing
            int id = 12345;
            long value = 0;
            TradFriRequest req = new TradFriRequest
            {
                UriPath = $"/15004/{id}",
                Payload = @"{ ""5850"":" + value + "}"
            };
            _gatewayConnection.Client.UpdateValues(req);
            //gatewayConnection.Client.GetValues(req);
        }

        private void btnTest3_Click(object sender, EventArgs e)
        {

        }

        private void btnTest4_Click(object sender, EventArgs e)
        {

        }

        private void btnRebootGW_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reboot the device?", "Gateway Reboot", MessageBoxButtons.YesNo).Equals(DialogResult.Yes))
            {
                GatewayController gwc = new GatewayController(_gatewayConnection.Client);
                gwc.Reboot();
            }
        }
    }
}
