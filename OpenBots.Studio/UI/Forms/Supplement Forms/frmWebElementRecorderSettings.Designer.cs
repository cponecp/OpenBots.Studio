﻿namespace OpenBots.UI.Supplement_Forms
{
    partial class frmHTMLElementRecorderSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLElementRecorderSettings));
            this.btnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.btnOkay = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.txtBrowserInstanceName = new System.Windows.Forms.TextBox();
            this.lblBrowserInstanceName = new System.Windows.Forms.Label();
            this.lblScreenRecorder = new System.Windows.Forms.Label();
            this.lblBrowserEngineType = new System.Windows.Forms.Label();
            this.cbxBrowserEngineType = new System.Windows.Forms.ComboBox();
            this.lblSearchParameters = new System.Windows.Forms.Label();
            this.dgvParameterSettings = new System.Windows.Forms.DataGridView();
            this.enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.parameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameterSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCancel.DisplayText = "Cancel";
            this.btnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.IsMouseOver = false;
            this.btnCancel.Location = new System.Drawing.Point(60, 390);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(45, 49);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOkay.DisplayText = "OK";
            this.btnOkay.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOkay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOkay.Image = ((System.Drawing.Image)(resources.GetObject("btnOkay.Image")));
            this.btnOkay.IsMouseOver = false;
            this.btnOkay.Location = new System.Drawing.Point(15, 390);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(45, 49);
            this.btnOkay.TabIndex = 31;
            this.btnOkay.TabStop = false;
            this.btnOkay.Text = "OK";
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // txtBrowserInstanceName
            // 
            this.txtBrowserInstanceName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBrowserInstanceName.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtBrowserInstanceName.Location = new System.Drawing.Point(15, 73);
            this.txtBrowserInstanceName.Name = "txtBrowserInstanceName";
            this.txtBrowserInstanceName.Size = new System.Drawing.Size(413, 27);
            this.txtBrowserInstanceName.TabIndex = 34;
            // 
            // lblBrowserInstanceName
            // 
            this.lblBrowserInstanceName.BackColor = System.Drawing.Color.Transparent;
            this.lblBrowserInstanceName.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrowserInstanceName.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblBrowserInstanceName.Location = new System.Drawing.Point(11, 49);
            this.lblBrowserInstanceName.Name = "lblBrowserInstanceName";
            this.lblBrowserInstanceName.Size = new System.Drawing.Size(362, 24);
            this.lblBrowserInstanceName.TabIndex = 33;
            this.lblBrowserInstanceName.Text = "Browser Instance Name";
            // 
            // lblScreenRecorder
            // 
            this.lblScreenRecorder.AutoSize = true;
            this.lblScreenRecorder.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenRecorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenRecorder.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblScreenRecorder.Location = new System.Drawing.Point(9, 7);
            this.lblScreenRecorder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreenRecorder.Name = "lblScreenRecorder";
            this.lblScreenRecorder.Size = new System.Drawing.Size(427, 37);
            this.lblScreenRecorder.TabIndex = 35;
            this.lblScreenRecorder.Text = "sequence recorder settings";
            // 
            // lblBrowserEngineType
            // 
            this.lblBrowserEngineType.BackColor = System.Drawing.Color.Transparent;
            this.lblBrowserEngineType.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrowserEngineType.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblBrowserEngineType.Location = new System.Drawing.Point(11, 106);
            this.lblBrowserEngineType.Name = "lblBrowserEngineType";
            this.lblBrowserEngineType.Size = new System.Drawing.Size(362, 24);
            this.lblBrowserEngineType.TabIndex = 36;
            this.lblBrowserEngineType.Text = "Browser Engine Type";
            // 
            // cbxBrowserEngineType
            // 
            this.cbxBrowserEngineType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBrowserEngineType.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.cbxBrowserEngineType.ForeColor = System.Drawing.Color.SteelBlue;
            this.cbxBrowserEngineType.FormattingEnabled = true;
            this.cbxBrowserEngineType.Items.AddRange(new object[] {
            "Chrome",
            "Firefox",
            "Internet Explorer",
            "None"});
            this.cbxBrowserEngineType.Location = new System.Drawing.Point(15, 130);
            this.cbxBrowserEngineType.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbxBrowserEngineType.Name = "cbxBrowserEngineType";
            this.cbxBrowserEngineType.Size = new System.Drawing.Size(413, 28);
            this.cbxBrowserEngineType.TabIndex = 37;
            // 
            // lblSearchParameters
            // 
            this.lblSearchParameters.BackColor = System.Drawing.Color.Transparent;
            this.lblSearchParameters.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchParameters.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearchParameters.Location = new System.Drawing.Point(11, 162);
            this.lblSearchParameters.Name = "lblSearchParameters";
            this.lblSearchParameters.Size = new System.Drawing.Size(362, 24);
            this.lblSearchParameters.TabIndex = 38;
            this.lblSearchParameters.Text = "Default Search Parameters";
            // 
            // dgvParameterSettings
            // 
            this.dgvParameterSettings.AllowUserToAddRows = false;
            this.dgvParameterSettings.AllowUserToDeleteRows = false;
            this.dgvParameterSettings.AllowUserToResizeColumns = false;
            this.dgvParameterSettings.AllowUserToResizeRows = false;
            this.dgvParameterSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvParameterSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParameterSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enabled,
            this.parameterName});
            this.dgvParameterSettings.DataBindings.Add(new System.Windows.Forms.Binding("DataSource", this, "ParameterSettingsDT", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvParameterSettings.Location = new System.Drawing.Point(15, 187);
            this.dgvParameterSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvParameterSettings.Name = "dgvParameterSettings";
            this.dgvParameterSettings.RowHeadersWidth = 51;
            this.dgvParameterSettings.RowTemplate.Height = 24;
            this.dgvParameterSettings.Size = new System.Drawing.Size(413, 196);
            this.dgvParameterSettings.TabIndex = 39;
            // 
            // enabled
            // 
            this.enabled.DataPropertyName = "Enabled";
            this.enabled.FillWeight = 15F;
            this.enabled.HeaderText = "Enabled";
            this.enabled.MinimumWidth = 6;
            this.enabled.Name = "enabled";
            // 
            // parameterName
            // 
            this.parameterName.DataPropertyName = "Parameter Name";
            this.parameterName.FillWeight = 50F;
            this.parameterName.HeaderText = "Parameter Name";
            this.parameterName.MinimumWidth = 6;
            this.parameterName.Name = "parameterName";
            this.parameterName.ReadOnly = true;
            this.parameterName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parameterName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmHTMLElementRecorderSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 453);
            this.Controls.Add(this.dgvParameterSettings);
            this.Controls.Add(this.lblSearchParameters);
            this.Controls.Add(this.cbxBrowserEngineType);
            this.Controls.Add(this.lblBrowserEngineType);
            this.Controls.Add(this.lblScreenRecorder);
            this.Controls.Add(this.txtBrowserInstanceName);
            this.Controls.Add(this.lblBrowserInstanceName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHTMLElementRecorderSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sequence Recorder Settings";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameterSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private OpenBots.Core.UI.Controls.UIPictureButton btnCancel;
        private OpenBots.Core.UI.Controls.UIPictureButton btnOkay;
        public System.Windows.Forms.TextBox txtBrowserInstanceName;
        private System.Windows.Forms.Label lblBrowserInstanceName;
        private System.Windows.Forms.Label lblScreenRecorder;
        private System.Windows.Forms.Label lblBrowserEngineType;
        public System.Windows.Forms.ComboBox cbxBrowserEngineType;
        private System.Windows.Forms.Label lblSearchParameters;
        private System.Windows.Forms.DataGridView dgvParameterSettings;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterName;
    }
}