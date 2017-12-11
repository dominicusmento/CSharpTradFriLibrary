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
        List<TradFriDevice> devices;
        TradFriCoapConnector gatewayConnection;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.GatewayName.Equals("GW-Nickname")
                && Properties.Settings.Default.GatewayIP.Equals("192.168.1.1")
                && Properties.Settings.Default.GatewaySecret.Equals("someSecretOnTheBackOfTheGateway"))
            {
                MessageBox.Show("Please provide the settings for your Gateway into app.config of 'TradFriGui' project.");
                Environment.Exit(0);
            }
            try
            {
                devices = new List<TradFriDevice>();
                gatewayConnection = new TradFriCoapConnector(Properties.Settings.Default.GatewayName, Properties.Settings.Default.GatewayIP, Properties.Settings.Default.GatewaySecret);
                gatewayConnection.Connect();
            }
            catch (Exception)
            {
                MessageBox.Show("Couldn't connect to TradFri gateway with provided settings");
                Environment.Exit(0);
            }

            LoadAllDevices();
            ShowDGVData();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            // dispose, close connections, etc.. not needed
        }

        private void LoadAllDevices()
        {
            GatewayController gwc = new GatewayController(gatewayConnection.Client);
            //List<WebLink> allResources = gwc.GetResources();
            //filter devices
            //foreach (WebLink deviceResource in allResources.Where(x => x.Uri.Contains(TradFriConst.Devices.ValueAsString() + '/')))
            foreach (long deviceID in gwc.GetDevices())
            {
                DeviceController dc = new DeviceController(deviceID, gatewayConnection.Client);
                TradFriDevice device = dc.GetTradFriDevice();
                devices.Add(device);
            }
        }

        //set dimmer to specific group example
        private List<TradFriGroup> AcquireGroups()
        {
            GatewayController gwc = new GatewayController(gatewayConnection.Client);
            List<TradFriGroup> groups = new List<TradFriGroup>();
            foreach (long groupID in gwc.GetGroups())
            {
                GroupController gc = new GroupController(groupID, gatewayConnection.Client);
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
            dgvDevices.DataSource = devices;
            dgvDevices.AutoGenerateColumns = true;
        }

        private void btnTurnOn_Click(object sender, EventArgs e)
        {
            // turn on the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradFriDevice currentSelectedDevice = (TradFriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                DeviceController dc = new DeviceController(currentSelectedDevice.ID, gatewayConnection.Client);
                dc.TurnOn();
            }
        }

        private void btnTurnOff_Click(object sender, EventArgs e)
        {
            // turn off the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradFriDevice currentSelectedDevice = (TradFriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                DeviceController dc = new DeviceController(currentSelectedDevice.ID, gatewayConnection.Client);
                dc.TurnOff();
            }
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            GatewayController gwc = new GatewayController(gatewayConnection.Client);
            foreach (long groupID in gwc.GetGroups())
            {
                GroupController gc = new GroupController(groupID, gatewayConnection.Client);
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
            gatewayConnection.Client.SetValues(req);
            //gatewayConnection.Client.GetValues(req);
        }

        private void btnTest3_Click(object sender, EventArgs e)
        {
            GatewayController gwc = new GatewayController(gatewayConnection.Client);
            List<TradFriMood> moods = gwc.GetMoods();

            List<TradFriGroup> groups = new List<TradFriGroup>();
            foreach (long groupID in gwc.GetGroups())
            {
                GroupController gc = new GroupController(groupID, gatewayConnection.Client);
                //not neccessary for controlling the group, it is used when we need the group properties
                TradFriGroup group = gc.GetTradFriGroup();
                //check if group name property is 'TestGroup'
                if (group.Name.Equals("TestGroup"))
                {
                    List<TradFriMood> groupMoods = moods.Where(x => x.GroupID.Equals(groupID)).ToList();
                    //gc.SetDimmer(230);
                    //set mood
                    Com.AugustCellars.CoAP.Response response = gc.SetMood(groupMoods[2]);
                }
                groups.Add(group);
            }
        }

        private void btnTest4_Click(object sender, EventArgs e)
        {
            TradFriDevice deviceToChangeProperties = devices[0];
            DeviceController dc = new DeviceController(deviceToChangeProperties.ID, gatewayConnection.Client);
            dc.SetColor(TradFriColors.CoolDaylight);
        }
    }
}
