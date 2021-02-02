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
            this.msTerminal = new System.Windows.Forms.MenuStrip();
            this.connectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.coordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenEmulator = new OpenBots.Commands.Terminal.Forms.OpenEmulator();
            this.fieldIndexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.msTerminal.SuspendLayout();
            this.SuspendLayout();
            // 
            // msTerminal
            // 
            this.msTerminal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.msTerminal.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.msTerminal.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.msTerminal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToolStripMenuItem,
            this.disconnectToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.fieldsToolStripMenuItem,
            this.coordinatesToolStripMenuItem,
            this.fieldIndexToolStripMenuItem});
            this.msTerminal.Location = new System.Drawing.Point(0, 0);
            this.msTerminal.Name = "msTerminal";
            this.msTerminal.Size = new System.Drawing.Size(815, 45);
            this.msTerminal.TabIndex = 5;
            this.msTerminal.Text = "menuStrip1";
            // 
            // connectToolStripMenuItem
            // 
            this.connectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.connectToolStripMenuItem.Name = "connectToolStripMenuItem";
            this.connectToolStripMenuItem.Size = new System.Drawing.Size(98, 41);
            this.connectToolStripMenuItem.Text = "Connect";
            this.connectToolStripMenuItem.Click += new System.EventHandler(this.connectToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(97, 41);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // disconnectToolStripMenuItem
            // 
            this.disconnectToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.disconnectToolStripMenuItem.Name = "disconnectToolStripMenuItem";
            this.disconnectToolStripMenuItem.Size = new System.Drawing.Size(122, 41);
            this.disconnectToolStripMenuItem.Text = "Disconnect";
            this.disconnectToolStripMenuItem.Click += new System.EventHandler(this.disconnectToolStripMenuItem_Click);
            // 
            // coordinatesToolStripMenuItem
            // 
            this.coordinatesToolStripMenuItem.Enabled = false;
            this.coordinatesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.coordinatesToolStripMenuItem.Name = "coordinatesToolStripMenuItem";
            this.coordinatesToolStripMenuItem.Size = new System.Drawing.Size(141, 41);
            this.coordinatesToolStripMenuItem.Text = "Coordinates: ";
            // 
            // fieldsToolStripMenuItem
            // 
            this.fieldsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fieldsToolStripMenuItem.Name = "fieldsToolStripMenuItem";
            this.fieldsToolStripMenuItem.Size = new System.Drawing.Size(76, 41);
            this.fieldsToolStripMenuItem.Text = "Fields";
            this.fieldsToolStripMenuItem.Click += new System.EventHandler(this.fieldsToolStripMenuItem_Click);
            // 
            // OpenEmulator
            // 
            this.OpenEmulator.BackColor = System.Drawing.SystemColors.WindowText;
            this.OpenEmulator.Coordinates = new System.Drawing.Point(0, 0);
            this.OpenEmulator.FieldIndex = 0;
            this.OpenEmulator.Location = new System.Drawing.Point(0, 36);
            this.OpenEmulator.Name = "OpenEmulator";
            this.OpenEmulator.Size = new System.Drawing.Size(815, 562);
            this.OpenEmulator.TabIndex = 0;
            this.OpenEmulator.Text = "";
            this.OpenEmulator.SelectionChanged += new System.EventHandler(this.OpenEmulator_SelectionChanged);
            this.OpenEmulator.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OpenEmulator_PreviewKeyDown);
            // 
            // fieldIndexToolStripMenuItem
            // 
            this.fieldIndexToolStripMenuItem.Enabled = false;
            this.fieldIndexToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.fieldIndexToolStripMenuItem.Name = "fieldIndexToolStripMenuItem";
            this.fieldIndexToolStripMenuItem.Size = new System.Drawing.Size(129, 41);
            this.fieldIndexToolStripMenuItem.Text = "Field Index: ";
            // 
            // frmTerminal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 480);
            this.Controls.Add(this.OpenEmulator);
            this.Controls.Add(this.msTerminal);
            this.MainMenuStrip = this.msTerminal;
            this.Name = "frmTerminal";
            this.Text = "frmTerminal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTerminal_FormClosing);
            this.msTerminal.ResumeLayout(false);
            this.msTerminal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public OpenBots.Commands.Terminal.Forms.OpenEmulator OpenEmulator;
        private System.Windows.Forms.MenuStrip msTerminal;
        private System.Windows.Forms.ToolStripMenuItem connectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disconnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem coordinatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fieldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fieldIndexToolStripMenuItem;
    }
}