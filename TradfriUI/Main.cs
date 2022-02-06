using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Tomidix.NetStandard.Tradfri;
using Tomidix.NetStandard.Tradfri.Models;
using TradfriUI.Settings;

namespace TradfriUI
{
    public partial class Main : Form
    {
        private string UserAppDataFilePath = Application.UserAppDataPath + "\\userdata.json";
        private bool loadingSelectedRows = true;

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
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    lblVersion.Text = $"App Version: {ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4)}";
                }
                catch { }
            }
            else
            {
                lblVersion.Text = $"App Version: {Application.ProductVersion}";
            }
            lblSettingsPathValue.Text = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath.ToString();

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

            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.appSecret))
            {
                using (EnterGatewayPSK form = new EnterGatewayPSK(Properties.Settings.Default.gatewayName, Properties.Settings.Default.appName, Properties.Settings.Default.gatewayIp))
                {
                    DialogResult result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        tradfriController = new TradfriController(form.GatewayName, form.IP);
                        // generating appSecret on gateway - appSecret is connected with the appName so you must use a combination
                        // Gateway generates one appSecret key per applicationName
                        TradfriAuth appSecret = tradfriController.GenerateAppSecret(form.AppSecret, Properties.Settings.Default.appName);
                        // saving programatically appSecret.PSK value to settings
                        Properties.Settings.Default.appSecret = appSecret.PSK;
                        Properties.Settings.Default.gatewayName = form.GatewayName;
                        Properties.Settings.Default.gatewayIp = form.IP;
                        Properties.Settings.Default.Save();

                    }
                    else if (result == DialogResult.Yes)
                    {
                        tradfriController = new TradfriController(form.GatewayName, form.IP);
                        // saving programatically appSecret.PSK value to settings
                        Properties.Settings.Default.appSecret = form.AppSecret;
                        Properties.Settings.Default.gatewayName = form.GatewayName;
                        Properties.Settings.Default.gatewayIp = form.IP;
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
            // initialize controller based on settings
            else
            {
                tradfriController = new TradfriController(Properties.Settings.Default.gatewayName, Properties.Settings.Default.gatewayIp);
            }

            // connection to gateway
            tradfriController.ConnectAppKey(Properties.Settings.Default.appSecret, Properties.Settings.Default.appName);
            GatewayInfo g = await tradfriController.GatewayController.GetGatewayInfo();
            lblGatewayVersion.Text += g.Firmware;
            lblGateway.Text += Properties.Settings.Default.gatewayIp;

            List<TradfriDevice> devices = new List<TradfriDevice>(await tradfriController.GatewayController.GetDeviceObjects()).OrderBy(x => x.DeviceType.ToString()).ToList();
            List<TradfriDevice> lights = devices.Where(i => i.DeviceType.Equals(DeviceType.Light)).ToList();
            List<TradfriGroup> groups = new List<TradfriGroup>(await tradfriController.GatewayController.GetGroupObjects()).OrderBy(x => x.Name).ToList();

            // set datasource for dgv
            dgvDevices.DataSource = devices;
            dgvDevices.AutoGenerateColumns = true;

            // set datasource for dgv
            dgvGroups.DataSource = groups;
            dgvGroups.AutoGenerateColumns = true;

            // temporary disable autosave on rowSelectionChange
            loadingSelectedRows = true;
            // clear default selection
            dgvDevices.ClearSelection();
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

        private async void btnMood_ClickAsync(object sender, EventArgs e)
        {
            TradfriGroup group = (TradfriGroup)dgvGroups.Rows[0].DataBoundItem;
            List<TradfriMood> moods = new List<TradfriMood>(await tradfriController.GatewayController.GetMoods()).OrderBy(x => x.Name).ToList();

            TradfriMood relaxMood = moods.First(m => m.Name.Equals("RELAX", StringComparison.OrdinalIgnoreCase) && m.GroupID.Equals(group.ID));

            // recommended if you want to use group instance later on
            tradfriController.GroupController.SetMood(group, relaxMood);

            // just change the mood
            //tradfriController.GroupController.SetMood(relaxMood.GroupID, relaxMood.ID);

            /*
             * set custom tradfri mood properties to every bulb in a group
            tradfriController.GroupController.SetMood(groups[0].ID, new TradfriMoodProperties
            {
                ColorHex = "f1e0b5",
                ColorHue = 24394,
                ColorSaturation = 5800,
                ColorX = 65535,
                ColorY = 65535,
                Dimmer = 254,
                ID = 1,
                LightState = 1,
                Mireds = 0
            });
            */

        }

        private void btnRGBColor_Click(object sender, EventArgs e)
        {
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                Color clr = colorDlg.Color;

                // set color of the lights for selected rows in grid (rows, not cells)
                for (int index = 0; index < dgvDevices.SelectedRows.Count; index++)
                {
                    TradfriDevice currentSelectedDevice = (TradfriDevice)(dgvDevices.SelectedRows[index]).DataBoundItem;
                    if (currentSelectedDevice.DeviceType.Equals(DeviceType.Light))
                    {
                        tradfriController.DeviceController.SetColor(currentSelectedDevice, clr.R, clr.G, clr.B);
                    }
                }
            }

        }

        private async void btnAddDevice_Click(object sender, EventArgs e)
        {
            tradfriController.GatewayController.AddDevice();
        }

        private async void btnAddToGroup_Click(object sender, EventArgs e)
        {
            var selectedGroup = (TradfriGroup)dgvGroups.SelectedRows[0].DataBoundItem;

            tradfriController.GatewayController.AddDevice(selectedGroup.ID, 60);
        }

        private void btnRenameDevice_Click(object sender, EventArgs e)
        {
            try
            {
                TradfriDevice currentSelectedDevice = (TradfriDevice)dgvDevices.SelectedRows[0].DataBoundItem;
                tradfriController.DeviceController.RenameTradfriDevice(currentSelectedDevice).Wait();
                MessageBox.Show("Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message
                    + Environment.NewLine
                    + "First change the Name in column, leave the row selected and press the 'Rename' button."
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRenameGroup_Click(object sender, EventArgs e)
        {
            try
            {
                TradfriGroup currentSelectedGroup = (TradfriGroup)dgvGroups.SelectedRows[0].DataBoundItem;
                tradfriController.GroupController.RenameTradfriGroup(currentSelectedGroup).Wait();
                MessageBox.Show("Done", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message
                    + Environment.NewLine
                    + "First change the Name in column, leave the row selected and press the 'Rename' button."
                    , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCleanup_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }

        private void lblSettingsPathValue_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(lblSettingsPathValue.Text);
            MessageBox.Show("Path value copied to clipboard.");
        }
    }
}
