using OpenBots.Core.Script;
using System;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.Sequence_Forms
{
    partial class frmSequence
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
                SelectedTabScriptActions.DrawSubItem -= lstScriptActions_DrawSubItem;
                SelectedTabScriptActions.ItemDrag -= lstScriptActions_ItemDrag;
                SelectedTabScriptActions.DragDrop -= lstScriptActions_DragDrop;
                SelectedTabScriptActions.DragEnter -= lstScriptActions_DragEnter;
                SelectedTabScriptActions.DoubleClick -= lstScriptActions_DoubleClick;
                SelectedTabScriptActions.KeyDown -= lstScriptActions_KeyDown;
                SelectedTabScriptActions.MouseClick -= lstScriptActions_MouseClick;
                SelectedTabScriptActions.MouseMove -= lstScriptActions_MouseMove;
                tmrNotify.Tick -= tmrNotify_Tick;
                enableSelectedCodeToolStripMenuItem.Click -= enableSelectedCodeToolStripMenuItem_Click;
                disableSelectedCodeToolStripMenuItem.Click -= disableSelectedCodeToolStripMenuItem_Click;
                addRemoveBreakpointToolStripMenuItem.Click -= addRemoveBreakpointToolStripMenuItem_Click;
                cutSelectedCodeToolStripMenuItem.Click -= cutSelectedCodeToolStripMenuItem_Click;
                copySelectedCodeToolStripMenuItem.Click -= copySelectedCodeToolStripMenuItem_Click;
                pasteSelectedCodeToolStripMenuItem.Click -= pasteSelectedCodeToolStripMenuItem_Click;
                deleteSelectedCodeToolStripMenuItem.Click -= deleteSelectedCodeToolStripMenuItem_Click;
                moveToParentToolStripMenuItem.Click -= moveToParentToolStripMenuItem_Click;
                viewCodeToolStripMenuItem.Click -= viewCodeToolStripMenuItem_Click;
                openShortcutMenuToolStripMenuItem.Click -= openShortcutMenuToolStripMenuItem_Click;
                notifyTray.MouseDoubleClick -= notifyTray_MouseDoubleClick;
                uiBtnRenameSequence.Click -= uiBtnRenameSequence_Click;
                uiBtnClose.Click -= uiBtnClose_Click;
                uiBtnSaveSequence.Click -= uiBtnSaveSequence_Click;
                uiBtnAddArgument.Click -= uiBtnAddArgument_Click;
                uiBtnAddElement.Click -= uiBtnAddElement_Click;
                uiBtnClearAll.Click -= uiBtnClearAll_Click;
                uiBtnSettings.Click -= uiBtnSettings_Click;
                uiBtnAddVariable.Click -= uiBtnAddVariable_Click;
                pnlStatus.Paint -= pnlStatus_Paint;
                pnlStatus.DoubleClick -= pnlStatus_DoubleClick;
                tvCommands.ItemDrag -= tvCommands_ItemDrag;
                tvCommands.DoubleClick -= tvCommands_DoubleClick;
                tvCommands.KeyPress -= tvCommands_KeyPress;
                uiBtnClearCommandSearch.Click -= uiBtnClearCommandSearch_Click;
                txtCommandSearch.TextChanged -= txtCommandSearch_TextChanged;
                txtCommandSearch.Enter -= txtCommandSearch_Enter;
                txtCommandSearch.Leave -= txtCommandSearch_Leave;
                dgvVariables.CellEndEdit -= dgvVariablesArguments_CellEndEdit;
                dgvVariables.CellEnter -= dgvVariablesArguments_CellEnter;
                dgvVariables.DataBindingComplete -= dgvVariablesArguments_DataBindingComplete;
                dgvVariables.RowsAdded -= dgvVariablesArguments_RowsAdded;
                dgvVariables.UserDeletingRow -= dgvVariablesArguments_UserDeletingRow;
                dgvVariables.KeyDown -= dgvVariablesArguments_KeyDown;
                dgvArguments.CellEndEdit -= dgvVariablesArguments_CellEndEdit;
                dgvArguments.CellEnter -= dgvVariablesArguments_CellEnter;
                dgvArguments.CellValueChanged -= dgvArguments_CellValueChanged;
                dgvArguments.CurrentCellDirtyStateChanged -= dgvArguments_CurrentCellDirtyStateChanged;
                dgvArguments.DataBindingComplete -= dgvVariablesArguments_DataBindingComplete;
                dgvArguments.DefaultValuesNeeded -= dgvArguments_DefaultValuesNeeded;
                dgvArguments.RowsAdded -= dgvVariablesArguments_RowsAdded;
                dgvArguments.UserDeletingRow -= dgvVariablesArguments_UserDeletingRow;
                dgvArguments.KeyDown -= dgvVariablesArguments_KeyDown;
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSequence));
            OpenBots.Core.Utilities.FormsUtilities.Theme theme1 = new OpenBots.Core.Utilities.FormsUtilities.Theme();
            this.tmrNotify = new System.Windows.Forms.Timer(this.components);
            this.cmsScriptActions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.enableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRemoveBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToParentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openShortcutMenuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.pnlControlContainer = new System.Windows.Forms.Panel();
            this.grpSaveClose = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.uiBtnRenameSequence = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnClose = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnSaveSequence = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.grpVariable = new OpenBots.UI.CustomControls.CustomUIControls.UIGroupBox();
            this.uiBtnAddArgument = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnAddElement = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnClearAll = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnSettings = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnAddVariable = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.splitContainerStudioControls = new OpenBots.UI.CustomControls.CustomUIControls.UISplitContainer();
            this.pnlPaneTabs = new OpenBots.UI.CustomControls.CustomUIControls.UIPanel();
            this.uiPaneTabs = new System.Windows.Forms.TabControl();
            this.tpCommands = new System.Windows.Forms.TabPage();
            this.tlpCommands = new System.Windows.Forms.TableLayoutPanel();
            this.tvCommands = new OpenBots.UI.CustomControls.CustomUIControls.UITreeView();
            this.pnlCommandSearch = new System.Windows.Forms.Panel();
            this.uiBtnClearCommandSearch = new OpenBots.UI.CustomControls.CustomUIControls.UIIconButton();
            this.txtCommandSearch = new System.Windows.Forms.TextBox();
            this.splitContainerScript = new OpenBots.UI.CustomControls.CustomUIControls.UISplitContainer();
            this.uiScriptTabControl = new OpenBots.UI.CustomControls.CustomUIControls.UITabControl();
            this.uiVariableArgumentTabs = new OpenBots.UI.CustomControls.CustomUIControls.UITabControl();
            this.variables = new System.Windows.Forms.TabPage();
            this.dgvVariables = new System.Windows.Forms.DataGridView();
            this.variableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.variableValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.arguments = new System.Windows.Forms.TabPage();
            this.dgvArguments = new System.Windows.Forms.DataGridView();
            this.argumentName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.argumentValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.direction = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmCommand = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlDivider = new System.Windows.Forms.Panel();
            this.tlpControls = new System.Windows.Forms.TableLayoutPanel();
            this.ttScriptBuilder = new System.Windows.Forms.ToolTip(this.components);
            this.cmsScriptActions.SuspendLayout();
            this.pnlControlContainer.SuspendLayout();
            this.grpSaveClose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRenameSequence)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveSequence)).BeginInit();
            this.grpVariable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddArgument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddElement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStudioControls)).BeginInit();
            this.splitContainerStudioControls.Panel1.SuspendLayout();
            this.splitContainerStudioControls.Panel2.SuspendLayout();
            this.splitContainerStudioControls.SuspendLayout();
            this.pnlPaneTabs.SuspendLayout();
            this.uiPaneTabs.SuspendLayout();
            this.tpCommands.SuspendLayout();
            this.tlpCommands.SuspendLayout();
            this.pnlCommandSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearCommandSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScript)).BeginInit();
            this.splitContainerScript.Panel1.SuspendLayout();
            this.splitContainerScript.Panel2.SuspendLayout();
            this.splitContainerScript.SuspendLayout();
            this.uiVariableArgumentTabs.SuspendLayout();
            this.variables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).BeginInit();
            this.arguments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArguments)).BeginInit();
            this.tlpControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrNotify
            // 
            this.tmrNotify.Enabled = true;
            this.tmrNotify.Interval = 500;
            this.tmrNotify.Tick += new System.EventHandler(this.tmrNotify_Tick);
            // 
            // cmsScriptActions
            // 
            this.cmsScriptActions.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.cmsScriptActions.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.cmsScriptActions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableSelectedCodeToolStripMenuItem,
            this.disableSelectedCodeToolStripMenuItem,
            this.addRemoveBreakpointToolStripMenuItem,
            this.cutSelectedCodeToolStripMenuItem,
            this.copySelectedCodeToolStripMenuItem,
            this.pasteSelectedCodeToolStripMenuItem,
            this.deleteSelectedCodeToolStripMenuItem,
            this.moveToParentToolStripMenuItem,
            this.viewCodeToolStripMenuItem,
            this.openShortcutMenuToolStripMenuItem});
            this.cmsScriptActions.Name = "cmsScriptActions";
            this.cmsScriptActions.Size = new System.Drawing.Size(329, 312);
            // 
            // enableSelectedCodeToolStripMenuItem
            // 
            this.enableSelectedCodeToolStripMenuItem.Name = "enableSelectedCodeToolStripMenuItem";
            this.enableSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.enableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.enableSelectedCodeToolStripMenuItem.Text = "Enable Selected Code";
            this.enableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.enableSelectedCodeToolStripMenuItem_Click);
            // 
            // disableSelectedCodeToolStripMenuItem
            // 
            this.disableSelectedCodeToolStripMenuItem.Name = "disableSelectedCodeToolStripMenuItem";
            this.disableSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.disableSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.disableSelectedCodeToolStripMenuItem.Text = "Disable Selected Code";
            this.disableSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.disableSelectedCodeToolStripMenuItem_Click);
            // 
            // addRemoveBreakpointToolStripMenuItem
            // 
            this.addRemoveBreakpointToolStripMenuItem.Enabled = false;
            this.addRemoveBreakpointToolStripMenuItem.Name = "addRemoveBreakpointToolStripMenuItem";
            this.addRemoveBreakpointToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.addRemoveBreakpointToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.addRemoveBreakpointToolStripMenuItem.Text = "Add/Remove Breakpoint";
            this.addRemoveBreakpointToolStripMenuItem.Click += new System.EventHandler(this.addRemoveBreakpointToolStripMenuItem_Click);
            // 
            // cutSelectedCodeToolStripMenuItem
            // 
            this.cutSelectedCodeToolStripMenuItem.Name = "cutSelectedCodeToolStripMenuItem";
            this.cutSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.cutSelectedCodeToolStripMenuItem.Text = "Cut Selected Code";
            this.cutSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.cutSelectedCodeToolStripMenuItem_Click);
            // 
            // copySelectedCodeToolStripMenuItem
            // 
            this.copySelectedCodeToolStripMenuItem.Name = "copySelectedCodeToolStripMenuItem";
            this.copySelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copySelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.copySelectedCodeToolStripMenuItem.Text = "Copy Selected Code";
            this.copySelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.copySelectedCodeToolStripMenuItem_Click);
            // 
            // pasteSelectedCodeToolStripMenuItem
            // 
            this.pasteSelectedCodeToolStripMenuItem.Name = "pasteSelectedCodeToolStripMenuItem";
            this.pasteSelectedCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.pasteSelectedCodeToolStripMenuItem.Text = "Paste Selected Code";
            this.pasteSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.pasteSelectedCodeToolStripMenuItem_Click);
            // 
            // deleteSelectedCodeToolStripMenuItem
            // 
            this.deleteSelectedCodeToolStripMenuItem.Name = "deleteSelectedCodeToolStripMenuItem";
            this.deleteSelectedCodeToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.deleteSelectedCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.deleteSelectedCodeToolStripMenuItem.Text = "Delete Selected Code";
            this.deleteSelectedCodeToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedCodeToolStripMenuItem_Click);
            // 
            // moveToParentToolStripMenuItem
            // 
            this.moveToParentToolStripMenuItem.Name = "moveToParentToolStripMenuItem";
            this.moveToParentToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.moveToParentToolStripMenuItem.Text = "Move Out To Parent";
            this.moveToParentToolStripMenuItem.Click += new System.EventHandler(this.moveToParentToolStripMenuItem_Click);
            // 
            // viewCodeToolStripMenuItem
            // 
            this.viewCodeToolStripMenuItem.Name = "viewCodeToolStripMenuItem";
            this.viewCodeToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.viewCodeToolStripMenuItem.Text = "View Code";
            this.viewCodeToolStripMenuItem.Click += new System.EventHandler(this.viewCodeToolStripMenuItem_Click);
            // 
            // openShortcutMenuToolStripMenuItem
            // 
            this.openShortcutMenuToolStripMenuItem.Name = "openShortcutMenuToolStripMenuItem";
            this.openShortcutMenuToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.openShortcutMenuToolStripMenuItem.Size = new System.Drawing.Size(328, 28);
            this.openShortcutMenuToolStripMenuItem.Text = "Open Shortcut Menu";
            this.openShortcutMenuToolStripMenuItem.Click += new System.EventHandler(this.openShortcutMenuToolStripMenuItem_Click);
            // 
            // notifyTray
            // 
            this.notifyTray.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyTray.BalloonTipText = "OpenBots Studio is still running in your system tray. Double-click to restore Ope" +
    "nBots Studio to full size!\r\n";
            this.notifyTray.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyTray.Icon")));
            this.notifyTray.Text = "OpenBots Studio, Open Source Automation for All";
            this.notifyTray.Visible = true;
            this.notifyTray.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyTray_MouseDoubleClick);
            // 
            // pnlControlContainer
            // 
            this.pnlControlContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tlpControls.SetColumnSpan(this.pnlControlContainer, 3);
            this.pnlControlContainer.Controls.Add(this.grpSaveClose);
            this.pnlControlContainer.Controls.Add(this.grpVariable);
            this.pnlControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlControlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlContainer.Name = "pnlControlContainer";
            this.pnlControlContainer.Size = new System.Drawing.Size(1776, 98);
            this.pnlControlContainer.TabIndex = 7;
            // 
            // grpSaveClose
            // 
            this.grpSaveClose.BackColor = System.Drawing.Color.Transparent;
            this.grpSaveClose.Controls.Add(this.uiBtnRenameSequence);
            this.grpSaveClose.Controls.Add(this.uiBtnClose);
            this.grpSaveClose.Controls.Add(this.uiBtnSaveSequence);
            this.grpSaveClose.Location = new System.Drawing.Point(339, 0);
            this.grpSaveClose.Margin = new System.Windows.Forms.Padding(4);
            this.grpSaveClose.Name = "grpSaveClose";
            this.grpSaveClose.Padding = new System.Windows.Forms.Padding(4);
            this.grpSaveClose.Size = new System.Drawing.Size(187, 89);
            this.grpSaveClose.TabIndex = 19;
            this.grpSaveClose.TabStop = false;
            this.grpSaveClose.Text = "Save and Close";
            this.grpSaveClose.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpSaveClose.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpSaveClose.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpSaveClose.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnRenameSequence
            // 
            this.uiBtnRenameSequence.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnRenameSequence.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnRenameSequence.DisplayText = "Rename";
            this.uiBtnRenameSequence.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnRenameSequence.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnRenameSequence.Image = global::OpenBots.Properties.Resources.action_bar_options;
            this.uiBtnRenameSequence.IsMouseOver = false;
            this.uiBtnRenameSequence.Location = new System.Drawing.Point(120, 24);
            this.uiBtnRenameSequence.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnRenameSequence.Name = "uiBtnRenameSequence";
            this.uiBtnRenameSequence.Size = new System.Drawing.Size(60, 62);
            this.uiBtnRenameSequence.TabIndex = 20;
            this.uiBtnRenameSequence.TabStop = false;
            this.uiBtnRenameSequence.Text = "Rename";
            this.uiBtnRenameSequence.Click += new System.EventHandler(this.uiBtnRenameSequence_Click);
            // 
            // uiBtnClose
            // 
            this.uiBtnClose.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClose.DisplayText = "Close";
            this.uiBtnClose.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClose.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClose.Image = global::OpenBots.Properties.Resources.action_bar_close;
            this.uiBtnClose.IsMouseOver = false;
            this.uiBtnClose.Location = new System.Drawing.Point(60, 24);
            this.uiBtnClose.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnClose.Name = "uiBtnClose";
            this.uiBtnClose.Size = new System.Drawing.Size(60, 62);
            this.uiBtnClose.TabIndex = 13;
            this.uiBtnClose.TabStop = false;
            this.uiBtnClose.Text = "Close";
            this.uiBtnClose.Click += new System.EventHandler(this.uiBtnClose_Click);
            // 
            // uiBtnSaveSequence
            // 
            this.uiBtnSaveSequence.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSaveSequence.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSaveSequence.DisplayText = "Save";
            this.uiBtnSaveSequence.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSaveSequence.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSaveSequence.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnSaveSequence.Image")));
            this.uiBtnSaveSequence.IsMouseOver = false;
            this.uiBtnSaveSequence.Location = new System.Drawing.Point(2, 24);
            this.uiBtnSaveSequence.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnSaveSequence.Name = "uiBtnSaveSequence";
            this.uiBtnSaveSequence.Size = new System.Drawing.Size(60, 62);
            this.uiBtnSaveSequence.TabIndex = 16;
            this.uiBtnSaveSequence.TabStop = false;
            this.uiBtnSaveSequence.Text = "Save";
            this.uiBtnSaveSequence.Click += new System.EventHandler(this.uiBtnSaveSequence_Click);
            // 
            // grpVariable
            // 
            this.grpVariable.BackColor = System.Drawing.Color.Transparent;
            this.grpVariable.Controls.Add(this.uiBtnAddArgument);
            this.grpVariable.Controls.Add(this.uiBtnAddElement);
            this.grpVariable.Controls.Add(this.uiBtnClearAll);
            this.grpVariable.Controls.Add(this.uiBtnSettings);
            this.grpVariable.Controls.Add(this.uiBtnAddVariable);
            this.grpVariable.Location = new System.Drawing.Point(8, 0);
            this.grpVariable.Margin = new System.Windows.Forms.Padding(4);
            this.grpVariable.Name = "grpVariable";
            this.grpVariable.Padding = new System.Windows.Forms.Padding(4);
            this.grpVariable.Size = new System.Drawing.Size(323, 89);
            this.grpVariable.TabIndex = 17;
            this.grpVariable.TabStop = false;
            this.grpVariable.Text = "Variables/Elements and Settings";
            this.grpVariable.TitleBackColor = System.Drawing.Color.Transparent;
            this.grpVariable.TitleFont = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpVariable.TitleForeColor = System.Drawing.Color.GhostWhite;
            this.grpVariable.TitleHatchStyle = System.Drawing.Drawing2D.HatchStyle.Horizontal;
            // 
            // uiBtnAddArgument
            // 
            this.uiBtnAddArgument.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddArgument.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddArgument.DisplayText = "Arguments";
            this.uiBtnAddArgument.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddArgument.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddArgument.Image = global::OpenBots.Properties.Resources.action_bar_variable;
            this.uiBtnAddArgument.IsMouseOver = false;
            this.uiBtnAddArgument.Location = new System.Drawing.Point(70, 24);
            this.uiBtnAddArgument.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnAddArgument.Name = "uiBtnAddArgument";
            this.uiBtnAddArgument.Size = new System.Drawing.Size(62, 62);
            this.uiBtnAddArgument.TabIndex = 16;
            this.uiBtnAddArgument.TabStop = false;
            this.uiBtnAddArgument.Text = "Arguments";
            this.uiBtnAddArgument.Click += new System.EventHandler(this.uiBtnAddArgument_Click);
            // 
            // uiBtnAddElement
            // 
            this.uiBtnAddElement.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddElement.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddElement.DisplayText = "Elements";
            this.uiBtnAddElement.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddElement.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddElement.Image = global::OpenBots.Properties.Resources.action_bar_element;
            this.uiBtnAddElement.IsMouseOver = false;
            this.uiBtnAddElement.Location = new System.Drawing.Point(132, 24);
            this.uiBtnAddElement.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnAddElement.Name = "uiBtnAddElement";
            this.uiBtnAddElement.Size = new System.Drawing.Size(60, 62);
            this.uiBtnAddElement.TabIndex = 15;
            this.uiBtnAddElement.TabStop = false;
            this.uiBtnAddElement.Text = "Elements";
            this.uiBtnAddElement.Click += new System.EventHandler(this.uiBtnAddElement_Click);
            // 
            // uiBtnClearAll
            // 
            this.uiBtnClearAll.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearAll.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearAll.DisplayText = "Clear";
            this.uiBtnClearAll.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnClearAll.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnClearAll.Image = global::OpenBots.Properties.Resources.action_bar_clear;
            this.uiBtnClearAll.IsMouseOver = false;
            this.uiBtnClearAll.Location = new System.Drawing.Point(252, 24);
            this.uiBtnClearAll.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnClearAll.Name = "uiBtnClearAll";
            this.uiBtnClearAll.Size = new System.Drawing.Size(60, 62);
            this.uiBtnClearAll.TabIndex = 14;
            this.uiBtnClearAll.TabStop = false;
            this.uiBtnClearAll.Text = "Clear";
            this.uiBtnClearAll.Click += new System.EventHandler(this.uiBtnClearAll_Click);
            // 
            // uiBtnSettings
            // 
            this.uiBtnSettings.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSettings.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSettings.DisplayText = "Settings";
            this.uiBtnSettings.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnSettings.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnSettings.Image = global::OpenBots.Properties.Resources.action_bar_options;
            this.uiBtnSettings.IsMouseOver = false;
            this.uiBtnSettings.Location = new System.Drawing.Point(192, 24);
            this.uiBtnSettings.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnSettings.Name = "uiBtnSettings";
            this.uiBtnSettings.Size = new System.Drawing.Size(60, 62);
            this.uiBtnSettings.TabIndex = 12;
            this.uiBtnSettings.TabStop = false;
            this.uiBtnSettings.Text = "Settings";
            this.uiBtnSettings.Click += new System.EventHandler(this.uiBtnSettings_Click);
            // 
            // uiBtnAddVariable
            // 
            this.uiBtnAddVariable.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnAddVariable.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnAddVariable.DisplayText = "Variables";
            this.uiBtnAddVariable.DisplayTextBrush = System.Drawing.Color.AliceBlue;
            this.uiBtnAddVariable.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnAddVariable.Image = global::OpenBots.Properties.Resources.action_bar_variable;
            this.uiBtnAddVariable.IsMouseOver = false;
            this.uiBtnAddVariable.Location = new System.Drawing.Point(8, 24);
            this.uiBtnAddVariable.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnAddVariable.Name = "uiBtnAddVariable";
            this.uiBtnAddVariable.Size = new System.Drawing.Size(62, 62);
            this.uiBtnAddVariable.TabIndex = 13;
            this.uiBtnAddVariable.TabStop = false;
            this.uiBtnAddVariable.Text = "Variables";
            this.uiBtnAddVariable.Click += new System.EventHandler(this.uiBtnAddVariable_Click);
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.SetColumnSpan(this.pnlStatus, 3);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.pnlStatus.Location = new System.Drawing.Point(0, 823);
            this.pnlStatus.Margin = new System.Windows.Forms.Padding(0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(1776, 39);
            this.pnlStatus.TabIndex = 3;
            this.pnlStatus.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlStatus_Paint);
            this.pnlStatus.DoubleClick += new System.EventHandler(this.pnlStatus_DoubleClick);
            // 
            // splitContainerStudioControls
            // 
            this.tlpControls.SetColumnSpan(this.splitContainerStudioControls, 3);
            this.splitContainerStudioControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerStudioControls.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerStudioControls.Location = new System.Drawing.Point(4, 108);
            this.splitContainerStudioControls.Margin = new System.Windows.Forms.Padding(4);
            this.splitContainerStudioControls.Name = "splitContainerStudioControls";
            // 
            // splitContainerStudioControls.Panel1
            // 
            this.splitContainerStudioControls.Panel1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerStudioControls.Panel1.Controls.Add(this.pnlPaneTabs);
            // 
            // splitContainerStudioControls.Panel2
            // 
            this.splitContainerStudioControls.Panel2.BackColor = System.Drawing.Color.Transparent;
            this.splitContainerStudioControls.Panel2.Controls.Add(this.splitContainerScript);
            this.splitContainerStudioControls.Size = new System.Drawing.Size(1768, 711);
            this.splitContainerStudioControls.SplitterDistance = 328;
            this.splitContainerStudioControls.SplitterWidth = 5;
            this.splitContainerStudioControls.TabIndex = 4;
            // 
            // pnlPaneTabs
            // 
            this.pnlPaneTabs.Controls.Add(this.uiPaneTabs);
            this.pnlPaneTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPaneTabs.Location = new System.Drawing.Point(0, 0);
            this.pnlPaneTabs.Name = "pnlPaneTabs";
            this.pnlPaneTabs.Size = new System.Drawing.Size(328, 711);
            this.pnlPaneTabs.TabIndex = 2;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.pnlPaneTabs.Theme = theme1;
            // 
            // uiPaneTabs
            // 
            this.uiPaneTabs.Controls.Add(this.tpCommands);
            this.uiPaneTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPaneTabs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiPaneTabs.Location = new System.Drawing.Point(0, 0);
            this.uiPaneTabs.Margin = new System.Windows.Forms.Padding(4);
            this.uiPaneTabs.Name = "uiPaneTabs";
            this.uiPaneTabs.SelectedIndex = 0;
            this.uiPaneTabs.Size = new System.Drawing.Size(328, 711);
            this.uiPaneTabs.TabIndex = 26;
            // 
            // tpCommands
            // 
            this.tpCommands.Controls.Add(this.tlpCommands);
            this.tpCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.tpCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.tpCommands.Location = new System.Drawing.Point(4, 32);
            this.tpCommands.Margin = new System.Windows.Forms.Padding(2);
            this.tpCommands.Name = "tpCommands";
            this.tpCommands.Padding = new System.Windows.Forms.Padding(2);
            this.tpCommands.Size = new System.Drawing.Size(320, 675);
            this.tpCommands.TabIndex = 4;
            this.tpCommands.Text = "Commands";
            this.tpCommands.UseVisualStyleBackColor = true;
            // 
            // tlpCommands
            // 
            this.tlpCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.tlpCommands.ColumnCount = 1;
            this.tlpCommands.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Controls.Add(this.tvCommands, 0, 1);
            this.tlpCommands.Controls.Add(this.pnlCommandSearch, 0, 0);
            this.tlpCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCommands.Location = new System.Drawing.Point(2, 2);
            this.tlpCommands.Margin = new System.Windows.Forms.Padding(2);
            this.tlpCommands.Name = "tlpCommands";
            this.tlpCommands.RowCount = 2;
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tlpCommands.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpCommands.Size = new System.Drawing.Size(316, 671);
            this.tlpCommands.TabIndex = 10;
            // 
            // tvCommands
            // 
            this.tvCommands.AllowDrop = true;
            this.tvCommands.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tvCommands.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tvCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvCommands.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.tvCommands.ForeColor = System.Drawing.Color.White;
            this.tvCommands.Location = new System.Drawing.Point(4, 35);
            this.tvCommands.Margin = new System.Windows.Forms.Padding(4);
            this.tvCommands.Name = "tvCommands";
            this.tvCommands.ShowLines = false;
            this.tvCommands.ShowNodeToolTips = true;
            this.tvCommands.Size = new System.Drawing.Size(308, 632);
            this.tvCommands.TabIndex = 9;
            this.tvCommands.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvCommands_ItemDrag);
            this.tvCommands.DoubleClick += new System.EventHandler(this.tvCommands_DoubleClick);
            this.tvCommands.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tvCommands_KeyPress);
            // 
            // pnlCommandSearch
            // 
            this.pnlCommandSearch.Controls.Add(this.uiBtnClearCommandSearch);
            this.pnlCommandSearch.Controls.Add(this.txtCommandSearch);
            this.pnlCommandSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommandSearch.Location = new System.Drawing.Point(2, 2);
            this.pnlCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.pnlCommandSearch.Name = "pnlCommandSearch";
            this.pnlCommandSearch.Size = new System.Drawing.Size(312, 27);
            this.pnlCommandSearch.TabIndex = 10;
            // 
            // uiBtnClearCommandSearch
            // 
            this.uiBtnClearCommandSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiBtnClearCommandSearch.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnClearCommandSearch.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnClearCommandSearch.DisplayText = null;
            this.uiBtnClearCommandSearch.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnClearCommandSearch.Image = global::OpenBots.Properties.Resources.commandsearch_clear;
            this.uiBtnClearCommandSearch.IsMouseOver = false;
            this.uiBtnClearCommandSearch.Location = new System.Drawing.Point(284, 1);
            this.uiBtnClearCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.uiBtnClearCommandSearch.Name = "uiBtnClearCommandSearch";
            this.uiBtnClearCommandSearch.Size = new System.Drawing.Size(25, 25);
            this.uiBtnClearCommandSearch.TabIndex = 1;
            this.uiBtnClearCommandSearch.TabStop = false;
            this.uiBtnClearCommandSearch.Text = null;
            this.ttScriptBuilder.SetToolTip(this.uiBtnClearCommandSearch, "Clear Search");
            this.uiBtnClearCommandSearch.Click += new System.EventHandler(this.uiBtnClearCommandSearch_Click);
            // 
            // txtCommandSearch
            // 
            this.txtCommandSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCommandSearch.ForeColor = System.Drawing.Color.LightGray;
            this.txtCommandSearch.Location = new System.Drawing.Point(0, 0);
            this.txtCommandSearch.Margin = new System.Windows.Forms.Padding(2);
            this.txtCommandSearch.Name = "txtCommandSearch";
            this.txtCommandSearch.Size = new System.Drawing.Size(280, 30);
            this.txtCommandSearch.TabIndex = 0;
            this.txtCommandSearch.Text = "Type Here to Search";
            this.txtCommandSearch.TextChanged += new System.EventHandler(this.txtCommandSearch_TextChanged);
            this.txtCommandSearch.Enter += new System.EventHandler(this.txtCommandSearch_Enter);
            this.txtCommandSearch.Leave += new System.EventHandler(this.txtCommandSearch_Leave);
            // 
            // splitContainerScript
            // 
            this.splitContainerScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerScript.Location = new System.Drawing.Point(0, 0);
            this.splitContainerScript.Name = "splitContainerScript";
            this.splitContainerScript.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerScript.Panel1
            // 
            this.splitContainerScript.Panel1.Controls.Add(this.uiScriptTabControl);
            // 
            // splitContainerScript.Panel2
            // 
            this.splitContainerScript.Panel2.Controls.Add(this.uiVariableArgumentTabs);
            this.splitContainerScript.Size = new System.Drawing.Size(1435, 711);
            this.splitContainerScript.SplitterDistance = 527;
            this.splitContainerScript.TabIndex = 4;
            // 
            // uiScriptTabControl
            // 
            this.uiScriptTabControl.AllowDrop = true;
            this.uiScriptTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiScriptTabControl.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiScriptTabControl.Location = new System.Drawing.Point(0, 0);
            this.uiScriptTabControl.Margin = new System.Windows.Forms.Padding(2);
            this.uiScriptTabControl.Name = "uiScriptTabControl";
            this.uiScriptTabControl.SelectedIndex = 0;
            this.uiScriptTabControl.ShowToolTips = true;
            this.uiScriptTabControl.Size = new System.Drawing.Size(1435, 527);
            this.uiScriptTabControl.TabIndex = 3;
            // 
            // uiVariableArgumentTabs
            // 
            this.uiVariableArgumentTabs.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.uiVariableArgumentTabs.AllowDrop = true;
            this.uiVariableArgumentTabs.Controls.Add(this.variables);
            this.uiVariableArgumentTabs.Controls.Add(this.arguments);
            this.uiVariableArgumentTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiVariableArgumentTabs.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.uiVariableArgumentTabs.Location = new System.Drawing.Point(0, 0);
            this.uiVariableArgumentTabs.Multiline = true;
            this.uiVariableArgumentTabs.Name = "uiVariableArgumentTabs";
            this.uiVariableArgumentTabs.SelectedIndex = 0;
            this.uiVariableArgumentTabs.Size = new System.Drawing.Size(1435, 180);
            this.uiVariableArgumentTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiVariableArgumentTabs.TabIndex = 0;
            // 
            // variables
            // 
            this.variables.Controls.Add(this.dgvVariables);
            this.variables.Location = new System.Drawing.Point(4, 4);
            this.variables.Name = "variables";
            this.variables.Padding = new System.Windows.Forms.Padding(3);
            this.variables.Size = new System.Drawing.Size(1427, 144);
            this.variables.TabIndex = 0;
            this.variables.Text = "Variables";
            this.variables.UseVisualStyleBackColor = true;
            // 
            // dgvVariables
            // 
            this.dgvVariables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVariables.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.variableName,
            this.variableValue});
            this.dgvVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVariables.Location = new System.Drawing.Point(3, 3);
            this.dgvVariables.Name = "dgvVariables";
            this.dgvVariables.RowHeadersWidth = 51;
            this.dgvVariables.RowTemplate.Height = 24;
            this.dgvVariables.Size = new System.Drawing.Size(1421, 138);
            this.dgvVariables.TabIndex = 0;
            this.dgvVariables.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEndEdit);
            this.dgvVariables.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEnter);
            this.dgvVariables.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVariablesArguments_DataBindingComplete);
            this.dgvVariables.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvVariablesArguments_RowsAdded);
            this.dgvVariables.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvVariablesArguments_UserDeletingRow);
            this.dgvVariables.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVariablesArguments_KeyDown);
            this.dgvVariables.SelectionChanged += new System.EventHandler(this.dgvVariables_SelectionChanged);
            // 
            // variableName
            // 
            this.variableName.DataPropertyName = "VariableName";
            this.variableName.HeaderText = "Variable Name";
            this.variableName.MinimumWidth = 6;
            this.variableName.Name = "variableName";
            // 
            // variableValue
            // 
            this.variableValue.DataPropertyName = "VariableValue";
            this.variableValue.HeaderText = "Variable Value";
            this.variableValue.MinimumWidth = 6;
            this.variableValue.Name = "variableValue";
            // 
            // arguments
            // 
            this.arguments.Controls.Add(this.dgvArguments);
            this.arguments.Location = new System.Drawing.Point(4, 4);
            this.arguments.Name = "arguments";
            this.arguments.Padding = new System.Windows.Forms.Padding(3);
            this.arguments.Size = new System.Drawing.Size(1427, 144);
            this.arguments.TabIndex = 1;
            this.arguments.Text = "Arguments";
            this.arguments.UseVisualStyleBackColor = true;
            // 
            // dgvArguments
            // 
            this.dgvArguments.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvArguments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArguments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.argumentName,
            this.argumentValue,
            this.direction});
            this.dgvArguments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArguments.Location = new System.Drawing.Point(3, 3);
            this.dgvArguments.Name = "dgvArguments";
            this.dgvArguments.RowHeadersWidth = 51;
            this.dgvArguments.RowTemplate.Height = 24;
            this.dgvArguments.Size = new System.Drawing.Size(1421, 138);
            this.dgvArguments.TabIndex = 2;
            this.dgvArguments.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEndEdit);
            this.dgvArguments.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvVariablesArguments_CellEnter);
            this.dgvArguments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArguments_CellValueChanged);
            this.dgvArguments.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvArguments_CurrentCellDirtyStateChanged);
            this.dgvArguments.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvVariablesArguments_DataBindingComplete);
            this.dgvArguments.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvArguments_DefaultValuesNeeded);
            this.dgvArguments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvVariablesArguments_RowsAdded);
            this.dgvArguments.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvVariablesArguments_UserDeletingRow);
            this.dgvArguments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvVariablesArguments_KeyDown);
            this.dgvArguments.SelectionChanged += new System.EventHandler(this.dgvArguments_SelectionChanged);
            // 
            // argumentName
            // 
            this.argumentName.DataPropertyName = "ArgumentName";
            this.argumentName.HeaderText = "Argument Name";
            this.argumentName.MinimumWidth = 6;
            this.argumentName.Name = "argumentName";
            // 
            // argumentValue
            // 
            this.argumentValue.DataPropertyName = "ArgumentValue";
            this.argumentValue.HeaderText = "Argument Value";
            this.argumentValue.MinimumWidth = 6;
            this.argumentValue.Name = "argumentValue";
            // 
            // direction
            // 
            this.direction.DataPropertyName = "Direction";
            this.direction.HeaderText = "Direction";
            this.direction.MinimumWidth = 6;
            this.direction.Name = "direction";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 20;
            // 
            // clmCommand
            // 
            this.clmCommand.Text = "Script Commands";
            this.clmCommand.Width = 800;
            // 
            // pnlDivider
            // 
            this.pnlDivider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.SetColumnSpan(this.pnlDivider, 4);
            this.pnlDivider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDivider.Location = new System.Drawing.Point(0, 98);
            this.pnlDivider.Margin = new System.Windows.Forms.Padding(0);
            this.pnlDivider.Name = "pnlDivider";
            this.pnlDivider.Size = new System.Drawing.Size(1776, 6);
            this.pnlDivider.TabIndex = 13;
            // 
            // tlpControls
            // 
            this.tlpControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.tlpControls.ColumnCount = 3;
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 319F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 655F));
            this.tlpControls.Controls.Add(this.pnlDivider, 0, 1);
            this.tlpControls.Controls.Add(this.splitContainerStudioControls, 0, 2);
            this.tlpControls.Controls.Add(this.pnlStatus, 0, 3);
            this.tlpControls.Controls.Add(this.pnlControlContainer, 0, 0);
            this.tlpControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControls.Location = new System.Drawing.Point(0, 0);
            this.tlpControls.Margin = new System.Windows.Forms.Padding(0);
            this.tlpControls.Name = "tlpControls";
            this.tlpControls.RowCount = 4;
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tlpControls.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpControls.Size = new System.Drawing.Size(1776, 862);
            this.tlpControls.TabIndex = 2;
            // 
            // frmSequence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1776, 862);
            this.Controls.Add(this.tlpControls);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmSequence";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sequence";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSequence_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSequence_FormClosed);
            this.Load += new System.EventHandler(this.frmSequence_Load);
            this.SizeChanged += new System.EventHandler(this.frmSequence_SizeChanged);
            this.Resize += new System.EventHandler(this.frmSequence_Resize);
            this.cmsScriptActions.ResumeLayout(false);
            this.pnlControlContainer.ResumeLayout(false);
            this.grpSaveClose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnRenameSequence)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSaveSequence)).EndInit();
            this.grpVariable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddArgument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddElement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnAddVariable)).EndInit();
            this.splitContainerStudioControls.Panel1.ResumeLayout(false);
            this.splitContainerStudioControls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerStudioControls)).EndInit();
            this.splitContainerStudioControls.ResumeLayout(false);
            this.pnlPaneTabs.ResumeLayout(false);
            this.uiPaneTabs.ResumeLayout(false);
            this.tpCommands.ResumeLayout(false);
            this.tlpCommands.ResumeLayout(false);
            this.pnlCommandSearch.ResumeLayout(false);
            this.pnlCommandSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnClearCommandSearch)).EndInit();
            this.splitContainerScript.Panel1.ResumeLayout(false);
            this.splitContainerScript.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScript)).EndInit();
            this.splitContainerScript.ResumeLayout(false);
            this.uiVariableArgumentTabs.ResumeLayout(false);
            this.variables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVariables)).EndInit();
            this.arguments.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArguments)).EndInit();
            this.tlpControls.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer tmrNotify;
        private System.Windows.Forms.ContextMenuStrip cmsScriptActions;
        private System.Windows.Forms.ToolStripMenuItem enableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRemoveBreakpointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copySelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutSelectedCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToParentToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyTray;
        private System.Windows.Forms.ToolStripMenuItem viewCodeToolStripMenuItem;
        private Panel pnlControlContainer;
        private TableLayoutPanel tlpControls;
        private Panel pnlDivider;
        private CustomControls.CustomUIControls.UISplitContainer splitContainerStudioControls;
        private Panel pnlStatus;
        private CustomControls.CustomUIControls.UIGroupBox grpSaveClose;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnClose;
        private CustomControls.CustomUIControls.UIGroupBox grpVariable;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnClearAll;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnSettings;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnAddVariable;
        public CustomControls.CustomUIControls.UITabControl uiScriptTabControl;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader clmCommand;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnAddElement;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnSaveSequence;
        private ToolStripMenuItem deleteSelectedCodeToolStripMenuItem;
        private ToolStripMenuItem openShortcutMenuToolStripMenuItem;
        private Core.UI.Controls.UIPictureButton uiBtnRenameSequence;
        private CustomControls.CustomUIControls.UIPanel pnlPaneTabs;
        private ToolTip ttScriptBuilder;
        private CustomControls.CustomUIControls.UISplitContainer splitContainerScript;
        private CustomControls.CustomUIControls.UITabControl uiVariableArgumentTabs;
        private TabPage variables;
        private TabPage arguments;
        public DataGridView dgvVariables;
        private DataGridViewTextBoxColumn variableName;
        private DataGridViewTextBoxColumn variableValue;
        public DataGridView dgvArguments;
        private DataGridViewTextBoxColumn argumentName;
        private DataGridViewTextBoxColumn argumentValue;
        private DataGridViewComboBoxColumn direction;
        private Core.UI.Controls.UIPictureButton uiBtnAddArgument;
        private TabControl uiPaneTabs;
        private TabPage tpCommands;
        private TableLayoutPanel tlpCommands;
        private CustomControls.CustomUIControls.UITreeView tvCommands;
        private Panel pnlCommandSearch;
        private CustomControls.CustomUIControls.UIIconButton uiBtnClearCommandSearch;
        private TextBox txtCommandSearch;
    }
}

