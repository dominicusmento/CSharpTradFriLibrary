using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Tomidix.NetStandard.Tradfri;
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

        private delegate void ObserveLightDelegate(TradfriDevice observableDevice);

        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, EventArgs e)
        {
            //preload colors combobox
            cmbColors.DisplayMember = "ColorName";
            //cmbColors.SelectedValueChanged += (s, ev) => cmbColors.SelectedText = s.ToString();
            cmbColors.ValueMember = "ColorId";
            //cmbColors.SelectedValueChanged += (s, ev) => cmbColors.SelectedText = s.ToString();
            foreach (FieldInfo p in typeof(TradfriColors).GetFields())
            {
                object v = p.GetValue(null); // static classes cannot be instanced, so use null...
                cmbColors.Items.Add(new { ColorName = p.Name, ColorId = v });
            }

            // prepare/load settings
            userData = loadUserData();

            // connect
            tradfriController = new TradfriController(Properties.Settings.Default.gatewayName, Properties.Settings.Default.gatewayIp);

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.appSecret))
            {
                using (EnterGatewayPSK form = new EnterGatewayPSK(Properties.Settings.Default.appName))
                {
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        // generating appSecret on gateway - appSecret is connected with the appName so you must use a combination
                        // Gateway generates one appSecret key per applicationName
                        TradfriAuth appSecret = tradfriController.GenerateAppSecret(form.AppSecret, Properties.Settings.Default.appName);
                        // saving programatically appSecret.PSK value to settings
                        Properties.Settings.Default.appSecret = appSecret.PSK;
                        Properties.Settings.Default.Save();
                    }
                    else
                    {
                        MessageBox.Show("You haven't entered your Gateway PSK so applicationSecret can't be generated on gateway." + Environment.NewLine +
                            "You can also enter it manually into app.config if you have one for your app already.",
                            "Error acquiring appSecret",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        this.Close();
                    }
                }
            }

            string secret = Properties.Settings.Default.appSecret;
            // connection to gateway
            tradfriController.ConnectAppKey(Properties.Settings.Default.appSecret, Properties.Settings.Default.appName);

            List<TradfriDevice> devices = new List<TradfriDevice>(await tradfriController.GatewayController.GetDeviceObjects()).OrderBy(x => x.DeviceType.ToString()).ToList();
            List<TradfriDevice> lights = devices.Where(i => i.DeviceType.Equals(DeviceType.Light)).ToList();

            // set datasource for dgv
            dgvDevices.DataSource = devices;
            dgvDevices.AutoGenerateColumns = true;

            // temporary disable autosave on rowSelectionChange
            loadingSelectedRows = true;
            //reselect rows from settings
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
            // re-enable autosave on rowSelectionChange
            loadingSelectedRows = false;

            if (lights.Count > 0)
            {
                // acquire first and last lights from grid
                TradfriDevice firstLight = lights.FirstOrDefault();
                TradfriDevice lastLight = lights.LastOrDefault();
                // function handler for observe events
                void ObserveLightEvent(TradfriDevice x)
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
                                    Debug.WriteLine($"{DateTime.Now} - triggered: {x.DeviceType}, {x.Name}, {x.LightControl[0].State}, {x.LightControl[0].Dimmer}");
                                    lbxLog.Items.Add($"{DateTime.Now} - triggered: {x.DeviceType}, {x.Name}, {x.LightControl[0].State}, {x.LightControl[0].Dimmer}");
                                    lbxLog.SelectedIndex = lbxLog.Items.Count - 1;
                                    trbBrightness.Value = Convert.ToInt16(eventDevice.LightControl[0].Dimmer * (double)10 / 254);
                                });
                            }
                        }
                    }
                };

                ObserveLightDelegate lightDelegate = new ObserveLightDelegate(ObserveLightEvent);
                // observe first light from grid
                tradfriController.DeviceController.ObserveDevice(firstLight, x => lightDelegate(x));
                // observe last light from grid if the bulbs are different
                if (firstLight.ID != lastLight.ID)
                {
                    tradfriController.DeviceController.ObserveDevice(lastLight, x => lightDelegate(x));
                }
            }
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
                    trbBrightness.Value = Convert.ToInt16(firstSelectedLight.LightControl[0].Dimmer * (double)10 / 254);
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
            // set brightness of the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradfriDevice currentSelectedDevice = (TradfriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                if (currentSelectedDevice.DeviceType.Equals(DeviceType.Light))
                {
                    tradfriController.DeviceController.SetDimmer(currentSelectedDevice, trbBrightness.Value * 10 * 254 / 100);
                }
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            PropertyInfo propertyInfo = cmbColors.Items[0].GetType().GetProperty("ColorId");
            // set color of the lights for selected rows in grid (rows, not cells)
            for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
            {
                TradfriDevice currentSelectedDevice = (TradfriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                if (currentSelectedDevice.DeviceType.Equals(DeviceType.Light))
                {
                    tradfriController.DeviceController.SetColor(currentSelectedDevice, (string)propertyInfo.GetValue(cmbColors.SelectedItem, null));
                }
            }
        }
    }
}
