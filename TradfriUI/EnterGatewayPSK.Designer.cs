namespace TradfriUI
{
    partial class EnterGatewayPSK
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnterGatewayPSK));
            this.btnOK = new System.Windows.Forms.Button();
            this.txtAppSecret = new System.Windows.Forms.TextBox();
            this.lblAppSecret = new System.Windows.Forms.Label();
            this.txtAppName = new System.Windows.Forms.TextBox();
            this.lblAppName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGatewayIP = new System.Windows.Forms.TextBox();
            this.lblGatewayIP = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGatewayName = new System.Windows.Forms.TextBox();
            this.lblGatewayName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(372, 382);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(185, 36);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Generate and Save";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtAppSecret
            // 
            this.txtAppSecret.Location = new System.Drawing.Point(292, 304);
            this.txtAppSecret.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAppSecret.Name = "txtAppSecret";
            this.txtAppSecret.Size = new System.Drawing.Size(265, 22);
            this.txtAppSecret.TabIndex = 3;
            // 
            // lblAppSecret
            // 
            this.lblAppSecret.AutoSize = true;
            this.lblAppSecret.Location = new System.Drawing.Point(153, 307);
            this.lblAppSecret.Name = "lblAppSecret";
            this.lblAppSecret.Size = new System.Drawing.Size(138, 17);
            this.lblAppSecret.TabIndex = 12;
            this.lblAppSecret.Text = "Gateway/app secret:";
            // 
            // txtAppName
            // 
            this.txtAppName.Enabled = false;
            this.txtAppName.Location = new System.Drawing.Point(292, 236);
            this.txtAppName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtAppName.Name = "txtAppName";
            this.txtAppName.Size = new System.Drawing.Size(265, 22);
            this.txtAppName.TabIndex = 2;
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Location = new System.Drawing.Point(153, 239);
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(120, 17);
            this.lblAppName.TabIndex = 10;
            this.lblAppName.Text = "Application name:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(332, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Your application name which is written in app.config";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(156, 382);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(170, 36);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save already generated";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(239, 332);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(324, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Original Gateway or already generated app secret";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(560, 32);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(118, 17);
            this.lblVersion.TabIndex = 14;
            this.lblVersion.Text = "Unknown Version";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(448, 195);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Your gateway ip";
            // 
            // txtGatewayIP
            // 
            this.txtGatewayIP.Location = new System.Drawing.Point(292, 167);
            this.txtGatewayIP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtGatewayIP.Name = "txtGatewayIP";
            this.txtGatewayIP.Size = new System.Drawing.Size(265, 22);
            this.txtGatewayIP.TabIndex = 1;
            // 
            // lblGatewayIP
            // 
            this.lblGatewayIP.AutoSize = true;
            this.lblGatewayIP.Location = new System.Drawing.Point(153, 170);
            this.lblGatewayIP.Name = "lblGatewayIP";
            this.lblGatewayIP.Size = new System.Drawing.Size(83, 17);
            this.lblGatewayIP.TabIndex = 8;
            this.lblGatewayIP.Text = "Gateway IP:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(298, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(264, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Gateway name - just for your information";
            // 
            // txtGatewayName
            // 
            this.txtGatewayName.Location = new System.Drawing.Point(292, 92);
            this.txtGatewayName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtGatewayName.Name = "txtGatewayName";
            this.txtGatewayName.Size = new System.Drawing.Size(265, 22);
            this.txtGatewayName.TabIndex = 0;
            // 
            // lblGatewayName
            // 
            this.lblGatewayName.AutoSize = true;
            this.lblGatewayName.Location = new System.Drawing.Point(153, 95);
            this.lblGatewayName.Name = "lblGatewayName";
            this.lblGatewayName.Size = new System.Drawing.Size(106, 17);
            this.lblGatewayName.TabIndex = 6;
            this.lblGatewayName.Text = "Gateway name:";
            // 
            // EnterGatewayPSK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 508);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtGatewayName);
            this.Controls.Add(this.lblGatewayName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtGatewayIP);
            this.Controls.Add(this.lblGatewayIP);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtAppSecret);
            this.Controls.Add(this.lblAppSecret);
            this.Controls.Add(this.txtAppName);
            this.Controls.Add(this.lblAppName);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "EnterGatewayPSK";
            this.Text = "Enter Gateway Settings";
            this.Load += new System.EventHandler(this.EnterGatewayPSK_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtAppSecret;
        private System.Windows.Forms.Label lblAppSecret;
        private System.Windows.Forms.TextBox txtAppName;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGatewayIP;
        private System.Windows.Forms.Label lblGatewayIP;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGatewayName;
        private System.Windows.Forms.Label lblGatewayName;
    }
}