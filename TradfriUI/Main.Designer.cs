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
            this.btnOff = new System.Windows.Forms.Button();
            this.dgvDevices = new System.Windows.Forms.DataGridView();
            this.btnOn = new System.Windows.Forms.Button();
            this.btnGWReboot = new System.Windows.Forms.Button();
            this.trbBrightness = new System.Windows.Forms.TrackBar();
            this.btnBrightness = new System.Windows.Forms.Button();
            this.lbxLog = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbBrightness)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOff
            // 
            this.btnOff.Location = new System.Drawing.Point(12, 659);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(83, 37);
            this.btnOff.TabIndex = 0;
            this.btnOff.Text = "Turn Off";
            this.btnOff.UseVisualStyleBackColor = true;
            this.btnOff.Click += new System.EventHandler(this.btnOff_Click);
            // 
            // dgvDevices
            // 
            this.dgvDevices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDevices.Location = new System.Drawing.Point(12, 102);
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.RowTemplate.Height = 28;
            this.dgvDevices.Size = new System.Drawing.Size(1448, 524);
            this.dgvDevices.TabIndex = 1;
            this.dgvDevices.SelectionChanged += new System.EventHandler(this.dgvDevices_SelectionChanged);
            // 
            // btnOn
            // 
            this.btnOn.Location = new System.Drawing.Point(1377, 659);
            this.btnOn.Name = "btnOn";
            this.btnOn.Size = new System.Drawing.Size(83, 37);
            this.btnOn.TabIndex = 2;
            this.btnOn.Text = "Turn On";
            this.btnOn.UseVisualStyleBackColor = true;
            this.btnOn.Click += new System.EventHandler(this.btnOn_Click);
            // 
            // btnGWReboot
            // 
            this.btnGWReboot.Location = new System.Drawing.Point(12, 43);
            this.btnGWReboot.Name = "btnGWReboot";
            this.btnGWReboot.Size = new System.Drawing.Size(180, 37);
            this.btnGWReboot.TabIndex = 3;
            this.btnGWReboot.Text = "Reboot Gateway";
            this.btnGWReboot.UseVisualStyleBackColor = true;
            this.btnGWReboot.Click += new System.EventHandler(this.btnGWReboot_Click);
            // 
            // trbBrightness
            // 
            this.trbBrightness.Location = new System.Drawing.Point(363, 659);
            this.trbBrightness.Name = "trbBrightness";
            this.trbBrightness.Size = new System.Drawing.Size(361, 69);
            this.trbBrightness.TabIndex = 4;
            this.trbBrightness.Value = 7;
            // 
            // btnBrightness
            // 
            this.btnBrightness.Location = new System.Drawing.Point(760, 659);
            this.btnBrightness.Name = "btnBrightness";
            this.btnBrightness.Size = new System.Drawing.Size(164, 37);
            this.btnBrightness.TabIndex = 5;
            this.btnBrightness.Text = "Apply Brightness";
            this.btnBrightness.UseVisualStyleBackColor = true;
            this.btnBrightness.Click += new System.EventHandler(this.btnBrightness_Click);
            // 
            // lbxLog
            // 
            this.lbxLog.FormattingEnabled = true;
            this.lbxLog.ItemHeight = 20;
            this.lbxLog.Location = new System.Drawing.Point(13, 768);
            this.lbxLog.Name = "lbxLog";
            this.lbxLog.Size = new System.Drawing.Size(1447, 224);
            this.lbxLog.TabIndex = 6;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1472, 1018);
            this.Controls.Add(this.lbxLog);
            this.Controls.Add(this.btnBrightness);
            this.Controls.Add(this.trbBrightness);
            this.Controls.Add(this.btnGWReboot);
            this.Controls.Add(this.btnOn);
            this.Controls.Add(this.dgvDevices);
            this.Controls.Add(this.btnOff);
            this.Name = "Main";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbBrightness)).EndInit();
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
    }
}

