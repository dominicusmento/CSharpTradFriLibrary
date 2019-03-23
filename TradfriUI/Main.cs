using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tomidix.NetStandard.Tradfri;
using Tomidix.NetStandard.Tradfri.Controllers;
using Tomidix.NetStandard.Tradfri.Models;
using TradfriUI.Settings;

namespace TradfriUI
{
    public partial class Main : Form
    {
        private string UserAppDataFilePath = Application.UserAppDataPath + "\\userdata.json";
        private bool loadingSelectedRows = false;

        public delegate void Print(int value);

        private UserData userData;
        private TradfriController tradfriController;
        private DeviceController controller;


        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            //prepare/load settings
            userData = loadUserData();

            //connect
            //set your values in app.config file
            tradfriController = new TradfriController(Properties.Settings.Default.gatewayName, Properties.Settings.Default.gatewayIp, Properties.Settings.Default.PSK);
            List<TradfriDevice> devices = new List<TradfriDevice>(await tradfriController.GatewayController.GetDeviceObjects()).OrderBy(x => x.DeviceType.ToString()).ToList();
            List<TradfriDevice> lights = devices.Where(i => i.DeviceType.Equals(DeviceType.Light)).ToList();


            dgvDevices.DataSource = devices;
            dgvDevices.AutoGenerateColumns = true;

            //reselect rows
            loadingSelectedRows = true;
            if (userData.SelectedDevices.Count > 0 && devices.Count > 0)
            {
                foreach (DataGridViewRow row in dgvDevices.Rows)
                {
                    if (userData.SelectedDevices.Any(x => x == (long)row.Cells["ID"].Value))
                    {
                        row.Selected = true;
                    }
                }
            }
            loadingSelectedRows = false;

            // observe first light from grid
            TradfriDevice first = lights.FirstOrDefault();
            tradfriController.DeviceController.ObserveDevice2(first,
                (x) =>
                {
                    TradfriDevice eventDevice = devices.Where(d => d.ID.Equals(x.ID)).SingleOrDefault();
                    eventDevice = x;
                    if (dgvDevices.SelectedRows.Count > 0)
                    {
                        foreach (object item in dgvDevices.SelectedRows)
                        {
                            if (((TradfriDevice)((DataGridViewRow)item).DataBoundItem).ID.Equals(eventDevice.ID))
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    Debug.WriteLine("Event triggered: " + DateTime.Now);
                                    trbBrightness.Value = Convert.ToInt16(Math.Round((decimal)(eventDevice.LightControl[0].Dimmer * 10 / 254), MidpointRounding.AwayFromZero));
                                });
                            }
                        }
                    }
                    //MessageBox.Show(x.ID.ToString());
                });

        }

        private void btnOff_Click(object sender, EventArgs e)
        {
            SetSelectedLights(false);
        }

        private void btnOn_Click(object sender, EventArgs e)
        {
            SetSelectedLights(true);
        }

        private void SetSelectedLights(bool state)
        {
            // turn off the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {

                TradfriDevice currentSelectedDevice = (TradfriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                if (currentSelectedDevice.DeviceType.Equals(DeviceType.Light))
                {
                    tradfriController.DeviceController.SetLight(currentSelectedDevice, state);
                }
            }
        }

        private void btnGWReboot_Click(object sender, EventArgs e)
        {
            tradfriController.GatewayController.Reboot();
        }

        private void dgvDevices_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDevices.SelectedRows.Count > 0)
            {
                TradfriDevice firstSelectedLight = (TradfriDevice)dgvDevices.SelectedRows[0].DataBoundItem;
                if (firstSelectedLight.DeviceType.Equals(DeviceType.Light))
                {
                    trbBrightness.Value = Convert.ToInt16(Math.Round((decimal)(firstSelectedLight.LightControl[0].Dimmer * 10 / 254), MidpointRounding.AwayFromZero));
                }
                if (!loadingSelectedRows)
                {
                    userData.SelectedDevices = new List<long>();
                    foreach (object item in dgvDevices.SelectedRows)
                    {
                        userData.SelectedDevices.Add(((TradfriDevice)((DataGridViewRow)item).DataBoundItem).ID);
                    }
                    File.WriteAllText(UserAppDataFilePath, JsonConvert.SerializeObject(userData));
                }
            }
        }

        private UserData loadUserData()
        {
            if (File.Exists(UserAppDataFilePath))
            {
                return JsonConvert.DeserializeObject<UserData>(File.ReadAllText(UserAppDataFilePath));
            }
            else
            {
                return new UserData();
            }
        }

        private void btnBrightness_Click(object sender, EventArgs e)
        {
            // turn off the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradfriDevice currentSelectedDevice = (TradfriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                if (currentSelectedDevice.DeviceType.Equals(DeviceType.Light))
                {
                    tradfriController.DeviceController.SetDimmer(currentSelectedDevice, trbBrightness.Value * 10 * 254 / 100);
                }
            }
        }
    }
}
