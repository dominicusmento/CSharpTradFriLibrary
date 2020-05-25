using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace TradfriUI
{
    public partial class EnterGatewayPSK : Form
    {
        public string GatewayName { get; set; }
        public string AppSecret { get; set; }
        public string IP { get; set; }

        public EnterGatewayPSK(string gwName, string appName, string ip)
        {
            InitializeComponent();
            txtGatewayName.Text = gwName;
            txtAppName.Text = appName;
            txtGatewayIP.Text = ip;
        }



        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtGatewayName.Text) && !string.IsNullOrWhiteSpace(txtAppSecret.Text) && !string.IsNullOrWhiteSpace(txtGatewayIP.Text))
            {
                GatewayName = txtGatewayName.Text;
                AppSecret = txtAppSecret.Text;
                IP = txtGatewayIP.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtGatewayName.Text) && !string.IsNullOrWhiteSpace(txtAppSecret.Text) && !string.IsNullOrWhiteSpace(txtGatewayIP.Text))
            {
                GatewayName = txtGatewayName.Text;
                AppSecret = txtAppSecret.Text;
                IP = txtGatewayIP.Text;
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
        }

        private void EnterGatewayPSK_Load(object sender, EventArgs e)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    lblVersion.Text = $"Version: {ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4)}";
                }
                catch { }
            }
            else
            {
                lblVersion.Text = $"Version: {Application.ProductVersion}";
            }
        }
    }
}
