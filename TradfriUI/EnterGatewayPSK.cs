using System;
using System.Windows.Forms;

namespace TradfriUI
{
    public partial class EnterGatewayPSK : Form
    {
        public string AppSecret { get; set; }
        public EnterGatewayPSK(string appName)
        {
            InitializeComponent();
            txtAppName.Text = appName;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtAppSecret.Text))
            {
                AppSecret = txtAppSecret.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }


    }
}
