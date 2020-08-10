namespace TradfriUI
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btnOff = new System.Windows.Forms.Button();
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.btnOn = new System.Windows.Forms.Button();
            this.btnGWReboot = new System.Windows.Forms.Button();
            this.trbBrightness = new System.Windows.Forms.TrackBar();
            this.btnBrightness = new System.Windows.Forms.Button();
            this.lbxLog = new System.Windows.Forms.ListBox();
            this.btnColor = new System.Windows.Forms.Button();
            this.cmbColors = new System.Windows.Forms.ComboBox();
            this.lblVersion = new System.Windows.Forms.Label();
            this.btnMood = new System.Windows.Forms.Button();
            this.btnRGBColor = new System.Windows.Forms.Button();
            this.colorDlg = new System.Windows.Forms.ColorDialog();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.dgvGroups = new System.Windows.Forms.DataGridView();
            this.btnAddToGroup = new System.Windows.Forms.Button();
            this.lblGroups = new System.Windows.Forms.Label();
            this.btnRenameDevice = new System.Windows.Forms.Button();
            this.lblDevices = new System.Windows.Forms.Label();
            this.lblGateway = new System.Windows.Forms.Label();
            this.lblGatewayVersion = new System.Windows.Forms.Label();
            this.btnRenameGroup = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOff
            // 
            this.btnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOff.Location = new System.Drawing.Point(9, 450);
            this.btnOff.Margin = new System.Windows.Forms.Padding(2);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(76, 24);
            this.btnOff.TabIndex = 0;
            this.btnOff.Text = "Turn Off";
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // dgvDevices
            // 
            this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevices.Location = new System.Drawing.Point(8, 123);
            this.dgvDevices.Margin = new System.Windows.Forms.Padding(2);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.RowHeadersWidth = 51;
            this.dgvDevices.RowTemplate.Height = 28;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(979, 309);
            this.dgvDevices.TabIndex = 1;
            this.dgvDevices.SelectionChanged += new System.EventHandler(this.dgvDevices_SelectionChanged);
            // 
            // btnOn
            // 
            this.btnOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOn.Location = new System.Drawing.Point(881, 445);
            this.btnOn.Margin = new System.Windows.Forms.Padding(2);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(106, 24);
            this.btnOn.TabIndex = 2;
            this.btnOn.Text = "Turn On";
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnGWReboot
            // 
            this.btnGWReboot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGWReboot.Location = new System.Drawing.Point(845, 45);
            this.btnGWReboot.Margin = new System.Windows.Forms.Padding(2);
            this.btnGWReboot.Name = "btnGWReboot";
            this.btnGWReboot.Size = new System.Drawing.Size(141, 24);
            this.btnGWReboot.TabIndex = 3;
            this.btnGWReboot.Text = "Reboot Gateway";
            this.btnGWReboot.UseVisualStyleBackColor = true;
            this.btnGWReboot.Click += new System.EventHandler(this.btnGWReboot_Click);
            // 
            // trbBrightness
            // 
            this.trbBrightness.Location = new System.Drawing.Point(118, 450);
            this.trbBrightness.Margin = new System.Windows.Forms.Padding(2);
            this.trbBrightness.Name = "trbBrightness";
            this.trbBrightness.Size = new System.Drawing.Size(262, 56);
            this.trbBrightness.TabIndex = 4;
            this.trbBrightness.Value = 7;
            // 
            // btnBrightness
            // 
            this.btnBrightness.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightness.Location = new System.Drawing.Point(118, 484);
            this.btnBrightness.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrightness.Name = "btnBrightness";
            this.btnBrightness.Size = new System.Drawing.Size(262, 24);
            this.btnBrightness.TabIndex = 5;
            this.btnBrightness.Text = "Apply Brightness";
            this.btnBrightness.UseVisualStyleBackColor = true;
            this.btnBrightness.Click += new System.EventHandler(this.btnBrightness_Click);
            // 
            // lbxLog
            // 
            this.lbxLog.FormattingEnabled = true;
            this.lbxLog.Location = new System.Drawing.Point(8, 794);
            this.lbxLog.Margin = new System.Windows.Forms.Padding(2);
            this.lbxLog.Name = "lbxLog";
            this.lbxLog.Size = new System.Drawing.Size(978, 108);
            this.lbxLog.TabIndex = 6;
            // 
            // btnColor
            // 
            this.btnColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColor.Location = new System.Drawing.Point(421, 484);
            this.btnColor.Margin = new System.Windows.Forms.Padding(2);
            this.btnColor.Name = "btnColor";
            this.btnColor.Size = new System.Drawing.Size(192, 24);
            this.btnColor.TabIndex = 7;
            this.btnColor.Text = "Apply Color";
            this.btnColor.UseVisualStyleBackColor = true;
            this.btnColor.Click += new System.EventHandler(this.btnColor_Click);
            // 
            // cmbColors
            // 
            this.cmbColors.FormattingEnabled = true;
            this.cmbColors.Location = new System.Drawing.Point(421, 450);
            this.cmbColors.Margin = new System.Windows.Forms.Padding(2);
            this.cmbColors.Name = "cmbColors";
            this.cmbColors.Size = new System.Drawing.Size(193, 21);
            this.cmbColors.TabIndex = 8;
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(884, 17);
            this.lblVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(103, 15);
            this.lblVersion.TabIndex = 10;
            this.lblVersion.Text = "Unknown Version";
            // 
            // btnMood
            // 
            this.btnMood.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMood.Location = new System.Drawing.Point(881, 482);
            this.btnMood.Margin = new System.Windows.Forms.Padding(2);
            this.btnMood.Name = "btnMood";
            this.btnMood.Size = new System.Drawing.Size(106, 24);
            this.btnMood.TabIndex = 11;
            this.btnMood.Text = "Set Mood Test";
            this.btnMood.UseVisualStyleBackColor = true;
            this.btnMood.Click += new System.EventHandler(this.btnMood_ClickAsync);
            // 
            // btnRGBColor
            // 
            this.btnRGBColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRGBColor.Location = new System.Drawing.Point(644, 445);
            this.btnRGBColor.Margin = new System.Windows.Forms.Padding(2);
            this.btnRGBColor.Name = "btnRGBColor";
            this.btnRGBColor.Size = new System.Drawing.Size(209, 24);
            this.btnRGBColor.TabIndex = 12;
            this.btnRGBColor.Text = "Set RGB Color";
            this.btnRGBColor.UseVisualStyleBackColor = true;
            this.btnRGBColor.Click += new System.EventHandler(this.btnRGBColor_Click);
            // 
            // colorDlg
            // 
            this.colorDlg.SolidColorOnly = true;
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDevice.Location = new System.Drawing.Point(832, 744);
            this.btnAddDevice.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(154, 24);
            this.btnAddDevice.TabIndex = 13;
            this.btnAddDevice.Text = "Add Device (Pair)";
            this.btnAddDevice.UseVisualStyleBackColor = true;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // dgvGroups
            // 
            this.dgvGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGroups.Location = new System.Drawing.Point(9, 565);
            this.dgvGroups.Margin = new System.Windows.Forms.Padding(2);
            this.dgvGroups.MultiSelect = false;
            this.dgvGroups.Name = "dgvGroups";
            this.dgvGroups.RowHeadersWidth = 51;
            this.dgvGroups.RowTemplate.Height = 28;
            this.dgvGroups.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGroups.Size = new System.Drawing.Size(979, 160);
            this.dgvGroups.TabIndex = 14;
            // 
            // btnAddToGroup
            // 
            this.btnAddToGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToGroup.Location = new System.Drawing.Point(8, 744);
            this.btnAddToGroup.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddToGroup.Name = "btnAddToGroup";
            this.btnAddToGroup.Size = new System.Drawing.Size(149, 24);
            this.btnAddToGroup.TabIndex = 15;
            this.btnAddToGroup.Text = "Add Device To Selected Group";
            this.btnAddToGroup.UseVisualStyleBackColor = true;
            this.btnAddToGroup.Click += new System.EventHandler(this.btnAddToGroup_Click);
            // 
            // lblGroups
            // 
            this.lblGroups.AutoSize = true;
            this.lblGroups.Location = new System.Drawing.Point(11, 548);
            this.lblGroups.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGroups.Name = "lblGroups";
            this.lblGroups.Size = new System.Drawing.Size(50, 15);
            this.lblGroups.TabIndex = 16;
            this.lblGroups.Text = "Groups:";
            // 
            // btnRenameDevice
            // 
            this.btnRenameDevice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameDevice.Location = new System.Drawing.Point(644, 482);
            this.btnRenameDevice.Margin = new System.Windows.Forms.Padding(2);
            this.btnRenameDevice.Name = "btnRenameDevice";
            this.btnRenameDevice.Size = new System.Drawing.Size(209, 24);
            this.btnRenameDevice.TabIndex = 17;
            this.btnRenameDevice.Text = "Rename selected device";
            this.btnRenameDevice.UseVisualStyleBackColor = true;
            this.btnRenameDevice.Click += new System.EventHandler(this.btnRenameDevice_Click);
            // 
            // lblDevices
            // 
            this.lblDevices.AutoSize = true;
            this.lblDevices.Location = new System.Drawing.Point(11, 106);
            this.lblDevices.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDevices.Name = "lblDevices";
            this.lblDevices.Size = new System.Drawing.Size(53, 15);
            this.lblDevices.TabIndex = 18;
            this.lblDevices.Text = "Devices:";
            // 
            // lblGateway
            // 
            this.lblGateway.AutoSize = true;
            this.lblGateway.Location = new System.Drawing.Point(11, 17);
            this.lblGateway.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGateway.Name = "lblGateway";
            this.lblGateway.Size = new System.Drawing.Size(57, 15);
            this.lblGateway.TabIndex = 19;
            this.lblGateway.Text = "Gateway:";
            // 
            // lblGatewayVersion
            // 
            this.lblGatewayVersion.AutoSize = true;
            this.lblGatewayVersion.Location = new System.Drawing.Point(11, 45);
            this.lblGatewayVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGatewayVersion.Name = "lblGatewayVersion";
            this.lblGatewayVersion.Size = new System.Drawing.Size(99, 15);
            this.lblGatewayVersion.TabIndex = 21;
            this.lblGatewayVersion.Text = "Gateway version:";
            // 
            // btnRenameGroup
            // 
            this.btnRenameGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenameGroup.Location = new System.Drawing.Point(606, 744);
            this.btnRenameGroup.Margin = new System.Windows.Forms.Padding(2);
            this.btnRenameGroup.Name = "btnRenameGroup";
            this.btnRenameGroup.Size = new System.Drawing.Size(209, 24);
            this.btnRenameGroup.TabIndex = 22;
            this.btnRenameGroup.Text = "Rename selected group";
            this.btnRenameGroup.UseVisualStyleBackColor = true;
            this.btnRenameGroup.Click += new System.EventHandler(this.btnRenameGroup_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 922);
            this.Controls.Add(this.btnRenameGroup);
            this.Controls.Add(this.lblGatewayVersion);
            this.Controls.Add(this.lblGateway);
            this.Controls.Add(this.lblDevices);
            this.Controls.Add(this.btnRenameDevice);
            this.Controls.Add(this.lblGroups);
            this.Controls.Add(this.btnAddToGroup);
            this.Controls.Add(this.dgvGroups);
            this.Controls.Add(this.btnAddDevice);
            this.Controls.Add(this.btnRGBColor);
            this.Controls.Add(this.btnMood);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.cmbColors);
            this.Controls.Add(this.btnColor);
            this.Controls.Add(this.lbxLog);
            this.Controls.Add(this.btnBrightness);
            this.Controls.Add(this.trbBrightness);
            this.Controls.Add(this.btnGWReboot);
            this.Controls.Add(this.btnOn);
            this.Controls.Add(this.dgvDevices);
            this.Controls.Add(this.btnOff);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGroups)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.DataGridView dgvDevices;
        private System.Windows.Forms.Button btnOn;
        private System.Windows.Forms.Button btnGWReboot;
        private System.Windows.Forms.TrackBar trbBrightness;
        private System.Windows.Forms.Button btnBrightness;
        private System.Windows.Forms.ListBox lbxLog;
        private System.Windows.Forms.Button btnColor;
        private System.Windows.Forms.ComboBox cmbColors;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnMood;
        private System.Windows.Forms.Button btnRGBColor;
        private System.Windows.Forms.ColorDialog colorDlg;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.DataGridView dgvGroups;
        private System.Windows.Forms.Button btnAddToGroup;
        private System.Windows.Forms.Label lblGroups;
        private System.Windows.Forms.Button btnRenameDevice;
        private System.Windows.Forms.Label lblDevices;
        private System.Windows.Forms.Label lblGateway;
        private System.Windows.Forms.Label lblGatewayVersion;
        private System.Windows.Forms.Button btnRenameGroup;
    }
}

