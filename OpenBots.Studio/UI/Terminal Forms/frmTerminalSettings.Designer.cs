namespace OpenBots.Commands.Terminal.Forms
{
    partial class frmTerminalSettings
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
            this.lblHost = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblTerminalType = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtTerminalType = new System.Windows.Forms.TextBox();
            this.cbxUseSSL = new System.Windows.Forms.CheckBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.BackColor = System.Drawing.Color.Transparent;
            this.lblHost.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblHost.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHost.Location = new System.Drawing.Point(10, 13);
            this.lblHost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(54, 25);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.BackColor = System.Drawing.Color.Transparent;
            this.lblPort.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblPort.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblPort.Location = new System.Drawing.Point(10, 53);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(63, 31);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";
            // 
            // lblTerminalType
            // 
            this.lblTerminalType.AutoSize = true;
            this.lblTerminalType.BackColor = System.Drawing.Color.Transparent;
            this.lblTerminalType.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lblTerminalType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblTerminalType.Location = new System.Drawing.Point(10, 93);
            this.lblTerminalType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTerminalType.Name = "lblTerminalType";
            this.lblTerminalType.Size = new System.Drawing.Size(132, 25);
            this.lblTerminalType.TabIndex = 2;
            this.lblTerminalType.Text = "Terminal Type:";
            // 
            // txtHost
            // 
            this.txtHost.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtHost.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtHost.Location = new System.Drawing.Point(153, 10);
            this.txtHost.Margin = new System.Windows.Forms.Padding(4);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(230, 32);
            this.txtHost.TabIndex = 3;
            this.txtHost.Text = "20.185.145.250";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtPort.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPort.Location = new System.Drawing.Point(153, 50);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(230, 32);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "3270";
            // 
            // txtTerminalType
            // 
            this.txtTerminalType.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.txtTerminalType.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtTerminalType.Location = new System.Drawing.Point(153, 90);
            this.txtTerminalType.Margin = new System.Windows.Forms.Padding(4);
            this.txtTerminalType.Name = "txtTerminalType";
            this.txtTerminalType.Size = new System.Drawing.Size(230, 32);
            this.txtTerminalType.TabIndex = 5;
            this.txtTerminalType.Text = "IBM-3278-2-E";
            // 
            // cbxUseSSL
            // 
            this.cbxUseSSL.AutoSize = true;
            this.cbxUseSSL.BackColor = System.Drawing.Color.Transparent;
            this.cbxUseSSL.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cbxUseSSL.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.cbxUseSSL.Location = new System.Drawing.Point(15, 133);
            this.cbxUseSSL.Margin = new System.Windows.Forms.Padding(4);
            this.cbxUseSSL.Name = "cbxUseSSL";
            this.cbxUseSSL.Size = new System.Drawing.Size(99, 29);
            this.cbxUseSSL.TabIndex = 6;
            this.cbxUseSSL.Text = "Use SSL";
            this.cbxUseSSL.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.btnOk.Location = new System.Drawing.Point(280, 130);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(103, 32);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmTerminalSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 174);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cbxUseSSL);
            this.Controls.Add(this.txtTerminalType);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.lblTerminalType);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.lblHost);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTerminalSettings";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblTerminalType;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtTerminalType;
        private System.Windows.Forms.CheckBox cbxUseSSL;
        private System.Windows.Forms.Button btnOk;
    }
}