namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmError
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.uiBtnIgnore = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnReport = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCopyError = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnBreak = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnIgnore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCopyError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnBreak)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblErrorMessage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(580, 169);
            this.panel1.TabIndex = 0;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblErrorMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblErrorMessage.Font = new System.Drawing.Font("Segoe UI Light", 10F, System.Drawing.FontStyle.Bold);
            this.lblErrorMessage.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblErrorMessage.Location = new System.Drawing.Point(0, 0);
            this.lblErrorMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(580, 169);
            this.lblErrorMessage.TabIndex = 18;
            this.lblErrorMessage.Text = "lblErrorMessage";
            // 
            // uiBtnIgnore
            // 
            this.uiBtnIgnore.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnIgnore.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnIgnore.DisplayText = "Ignore";
            this.uiBtnIgnore.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnIgnore.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnIgnore.Image = global::OpenBots.Properties.Resources.engine_ignore;
            this.uiBtnIgnore.IsMouseOver = false;
            this.uiBtnIgnore.Location = new System.Drawing.Point(450, 178);
            this.uiBtnIgnore.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnIgnore.Name = "uiBtnIgnore";
            this.uiBtnIgnore.Size = new System.Drawing.Size(60, 60);
            this.uiBtnIgnore.TabIndex = 23;
            this.uiBtnIgnore.TabStop = false;
            this.uiBtnIgnore.Text = "Ignore";
            this.uiBtnIgnore.Click += new System.EventHandler(this.uiBtnIgnore_Click);
            // 
            // uiBtnReport
            // 
            this.uiBtnReport.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnReport.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnReport.DisplayText = "Report";
            this.uiBtnReport.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnReport.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnReport.Image = global::OpenBots.Properties.Resources.engine_resume;
            this.uiBtnReport.IsMouseOver = false;
            this.uiBtnReport.Location = new System.Drawing.Point(510, 178);
            this.uiBtnReport.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnReport.Name = "uiBtnReport";
            this.uiBtnReport.Size = new System.Drawing.Size(60, 60);
            this.uiBtnReport.TabIndex = 24;
            this.uiBtnReport.TabStop = false;
            this.uiBtnReport.Text = "Report";
            this.uiBtnReport.Click += new System.EventHandler(this.uiBtnReport_Click);
            // 
            // uiBtnCopyError
            // 
            this.uiBtnCopyError.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCopyError.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCopyError.DisplayText = "Copy";
            this.uiBtnCopyError.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCopyError.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCopyError.Image = global::OpenBots.Properties.Resources.copy;
            this.uiBtnCopyError.IsMouseOver = false;
            this.uiBtnCopyError.Location = new System.Drawing.Point(10, 178);
            this.uiBtnCopyError.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCopyError.Name = "uiBtnCopyError";
            this.uiBtnCopyError.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCopyError.TabIndex = 25;
            this.uiBtnCopyError.TabStop = false;
            this.uiBtnCopyError.Text = "Copy";
            this.uiBtnCopyError.Click += new System.EventHandler(this.uiBtnCopyError_Click);
            // 
            // uiBtnBreak
            // 
            this.uiBtnBreak.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnBreak.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnBreak.DisplayText = "Break";
            this.uiBtnBreak.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnBreak.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnBreak.Image = global::OpenBots.Properties.Resources.engine_pause;
            this.uiBtnBreak.IsMouseOver = false;
            this.uiBtnBreak.Location = new System.Drawing.Point(390, 178);
            this.uiBtnBreak.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnBreak.Name = "uiBtnBreak";
            this.uiBtnBreak.Size = new System.Drawing.Size(60, 60);
            this.uiBtnBreak.TabIndex = 26;
            this.uiBtnBreak.TabStop = false;
            this.uiBtnBreak.Text = "Break";
            this.uiBtnBreak.Visible = false;
            this.uiBtnBreak.Click += new System.EventHandler(this.uiBtnBreak_Click);
            // 
            // frmError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 247);
            this.Controls.Add(this.uiBtnBreak);
            this.Controls.Add(this.uiBtnCopyError);
            this.Controls.Add(this.uiBtnIgnore);
            this.Controls.Add(this.uiBtnReport);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::OpenBots.Properties.Resources.OpenBots_ico;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnIgnore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCopyError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnBreak)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnIgnore;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnReport;
        private System.Windows.Forms.Label lblErrorMessage;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnCopyError;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnBreak;
    }
}