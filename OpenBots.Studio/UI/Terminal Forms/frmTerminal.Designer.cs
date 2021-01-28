namespace OpenBots.Commands.Terminal.Forms
{
    partial class frmTerminal
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
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.OpenEmulator = new OpenBots.Commands.Terminal.Forms.OpenEmulator();
            this.lblCoordinates = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(1097, 31);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(172, 58);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(1097, 119);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(172, 58);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(1097, 205);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(172, 58);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // OpenEmulator
            // 
            this.OpenEmulator.BackColor = System.Drawing.SystemColors.WindowText;
            this.OpenEmulator.Location = new System.Drawing.Point(12, 12);
            this.OpenEmulator.Name = "OpenEmulator";
            this.OpenEmulator.Size = new System.Drawing.Size(1017, 681);
            this.OpenEmulator.TabIndex = 0;
            this.OpenEmulator.Text = "";
            this.OpenEmulator.SelectionChanged += new System.EventHandler(this.OpenEmulator_SelectionChanged);
            this.OpenEmulator.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OpenEmulator_PreviewKeyDown);
            // 
            // lblCoordinates
            // 
            this.lblCoordinates.AutoSize = true;
            this.lblCoordinates.Location = new System.Drawing.Point(1125, 449);
            this.lblCoordinates.Name = "lblCoordinates";
            this.lblCoordinates.Size = new System.Drawing.Size(115, 21);
            this.lblCoordinates.TabIndex = 4;
            this.lblCoordinates.Text = "Coordinates: ";
            // 
            // frmTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1349, 725);
            this.Controls.Add(this.lblCoordinates);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.OpenEmulator);
            this.Name = "frmTerminal";
            this.Text = "frmTerminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTerminal_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public OpenBots.Commands.Terminal.Forms.OpenEmulator OpenEmulator;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblCoordinates;
    }
}