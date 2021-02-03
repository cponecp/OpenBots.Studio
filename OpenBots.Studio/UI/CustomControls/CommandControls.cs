using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Utilities;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Properties;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.UI.Controls.CustomControls;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenBots.Studio.Utilities;
using OpenBots.UI.Forms;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using IContainer = Autofac.IContainer;

namespace OpenBots.UI.CustomControls
{
    public class CommandControls : ICommandControls
    {
        private frmCommandEditor _currentEditor;
        private IContainer _container;
        private string _projectPath;

        //used in capture window helper
        private CommandItemControl _inputBox;
        private ApplicationSettings _settings;
        private bool _minimizePreference;

        public CommandControls()
        {
        }

        public CommandControls(frmCommandEditor editor, EngineContext engineContext)
        {
            _currentEditor = editor;
            _container = engineContext.Container;
            _projectPath = engineContext.ProjectPath;
        }

        public List<Control> CreateDefaultInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor, int height = 30, int width = 300)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var input = CreateDefaultInputFor(parameterName, parent, height, width);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(input);

            return controlList;
        }

        public List<Control> CreateDefaultPasswordInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var passwordInput = CreateDefaultInputFor(parameterName, parent);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { passwordInput }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(passwordInput);

            ((TextBox)passwordInput).PasswordChar = '*';

            return controlList;
        }

        public List<Control> CreateDefaultOutputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var variableNameControl = AddVariableNames(CreateStandardComboboxFor(parameterName, parent), editor);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { variableNameControl }, editor);

            variableNameControl.Click += (sender, e) => VariableNameControl_Click(sender, e, editor);
            variableNameControl.KeyPress += DropdownBox_KeyPress;
            variableNameControl.KeyDown += DropdownBox_KeyDown;

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(variableNameControl);
            return controlList;
        }

        private void VariableNameControl_Click(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            ComboBox outputBox = (ComboBox)sender;
            AddVariableNames(outputBox, editor);
        }

        public List<Control> CreateDefaultDropdownGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var input = CreateDropdownFor(parameterName, parent);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(input);

            return controlList;
        }

        public List<Control> CreateDataGridViewGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var gridview = CreateDataGridView(parent, parameterName);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { gridview }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(gridview);

            return controlList;
        }

        public List<Control> CreateDefaultWindowControlGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var windowNameControl = AddWindowNames(CreateStandardComboboxFor(parameterName, parent));
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { windowNameControl }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(windowNameControl);

            return controlList;
        }

        public Control CreateDefaultLabelFor(string parameterName, ScriptCommand parent)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();

            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(DisplayNameAttribute), true);

            Label inputLabel = new Label();
            if (propertyAttributesAssigned.Length > 0)
            {
                var attribute = (DisplayNameAttribute)propertyAttributesAssigned[0];
                inputLabel.Text = attribute.DisplayName;
            }
            else
            {
                inputLabel.Text = parameterName;
            }

            inputLabel.AutoSize = true;
            inputLabel.Font = new Font("Segoe UI Light", 12);
            inputLabel.ForeColor = Color.White;
            inputLabel.Name = "lbl_" + parameterName;
            CreateDefaultToolTipFor(parameterName, parent, inputLabel);
            return inputLabel;
        }

        public void CreateDefaultToolTipFor(string parameterName, ScriptCommand parent, Control label)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var inputSpecificationAttributesAssigned = variableProperties.GetCustomAttributes(typeof(DescriptionAttribute), true);
            var sampleUsageAttributesAssigned = variableProperties.GetCustomAttributes(typeof(SampleUsage), true);
            var remarksAttributesAssigned = variableProperties.GetCustomAttributes(typeof(Remarks), true);

            string toolTipText = "";
            if (inputSpecificationAttributesAssigned.Length > 0)
            {
                var attribute = (DescriptionAttribute)inputSpecificationAttributesAssigned[0];
                toolTipText = attribute.Description;
            }
            if (sampleUsageAttributesAssigned.Length > 0)
            {
                var attribute = (SampleUsage)sampleUsageAttributesAssigned[0];
                if (attribute.Usage.Length > 0)
                    toolTipText += "\nSample: " + attribute.Usage;
            }
            if (remarksAttributesAssigned.Length > 0)
            {
                var attribute = (Remarks)remarksAttributesAssigned[0];
                if (attribute.Remark.Length > 0)
                    toolTipText += "\n" + attribute.Remark;
            }

            ToolTip inputToolTip = new ToolTip();
            inputToolTip.ToolTipIcon = ToolTipIcon.Info;
            inputToolTip.IsBalloon = true;
            inputToolTip.ShowAlways = true;
            inputToolTip.ToolTipTitle = label.Text;
            inputToolTip.AutoPopDelay = 15000;
            inputToolTip.SetToolTip(label, toolTipText);
        }

        public Control CreateDefaultInputFor(string parameterName, ScriptCommand parent, int height = 30, int width = 300)
        {
            var inputBox = new TextBox();
            inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = height;
            inputBox.Width = width;

            if (height > 30)
            {
                inputBox.Multiline = true;
            }

            inputBox.Name = parameterName;
            inputBox.KeyDown += InputBox_KeyDown;
            inputBox.KeyPress += InputBox_KeyPress;

            if (parameterName == "v_Comment")
                inputBox.Margin = new Padding(0, 0, 0, 20);
            return inputBox;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox inputBox = (TextBox)sender;
            if (e.Control && e.KeyCode == Keys.K)
            {
                frmScriptVariables scriptVariableEditor = new frmScriptVariables
                {
                    ScriptVariables = _currentEditor.ScriptEngineContext.Variables,
                    ScriptArguments = _currentEditor.ScriptEngineContext.Arguments
                };

                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    _currentEditor.ScriptEngineContext.Variables = scriptVariableEditor.ScriptVariables;

                    if (!string.IsNullOrEmpty(scriptVariableEditor.LastModifiedVariableName))
                        inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, "{" + scriptVariableEditor.LastModifiedVariableName + "}");
                }

                scriptVariableEditor.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.J)
            {
                frmScriptArguments scriptArgumentEditor = new frmScriptArguments
                {
                    ScriptVariables = _currentEditor.ScriptEngineContext.Variables,
                    ScriptArguments = _currentEditor.ScriptEngineContext.Arguments
                };

                if (scriptArgumentEditor.ShowDialog() == DialogResult.OK)
                {
                    _currentEditor.ScriptEngineContext.Arguments = scriptArgumentEditor.ScriptArguments;

                    if (!string.IsNullOrEmpty(scriptArgumentEditor.LastModifiedArgumentName))
                        inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, "{" + scriptArgumentEditor.LastModifiedArgumentName + "}");
                }

                scriptArgumentEditor.Dispose();

            }
            else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Enter)
                return;
            else if (e.KeyCode == Keys.Enter)
                _currentEditor.uiBtnAdd_Click(null, null);
        }

        private void InputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Control + K or Control + J
            if (e.KeyChar == '\v' || e.KeyChar == '\n')
                e.Handled = true;
        }

        public CheckBox CreateCheckBoxFor(string parameterName, ScriptCommand parent)
        {
            var checkBox = new CheckBox();
            checkBox.DataBindings.Add("Checked", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            checkBox.Name = parameterName;
            checkBox.AutoSize = true;
            checkBox.Size = new Size(20, 20);
            checkBox.UseVisualStyleBackColor = true;
            checkBox.Margin = new Padding(0, 8, 0, 0);
            return checkBox;
        }

        public Control CreateDropdownFor(string parameterName, ScriptCommand parent)
        {
            var dropdownBox = new ComboBox();
            dropdownBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            dropdownBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            dropdownBox.Height = 30;
            dropdownBox.Width = 300;
            dropdownBox.Name = parameterName;

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(PropertyUISelectionOption), true);

            foreach (PropertyUISelectionOption option in propertyAttributesAssigned)
            {
                dropdownBox.Items.Add(option.UIOption);
            }

            dropdownBox.Click += DropdownBox_Click;
            dropdownBox.KeyDown += DropdownBox_KeyDown;
            dropdownBox.KeyPress += DropdownBox_KeyPress;
            dropdownBox.MouseWheel += DropdownBox_MouseWheel;

            return dropdownBox;
        }

        private void DropdownBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                e.Handled = true;
        }

        private void DropdownBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up && e.KeyCode != Keys.Enter)
                e.Handled = true;
        }

        private void DropdownBox_Click(object sender, EventArgs e)
        {
            ComboBox clickedDropdownBox = (ComboBox)sender;
            clickedDropdownBox.DroppedDown = true;
        }

        private void DropdownBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        public ComboBox CreateStandardComboboxFor(string parameterName, ScriptCommand parent)
        {
            var standardComboBox = new ComboBox();
            standardComboBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            standardComboBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            standardComboBox.Height = 30;
            standardComboBox.Width = 300;
            standardComboBox.Name = parameterName;
            standardComboBox.Click += StandardComboBox_Click;
            standardComboBox.KeyDown += StandardComboBox_KeyDown;
            standardComboBox.KeyPress += StandardComboBox_KeyPress;
            standardComboBox.MouseWheel += StandardComboBox_MouseWheel;

            return standardComboBox;
        }      

        private void StandardComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.K)
            {
                frmScriptVariables scriptVariableEditor = new frmScriptVariables
                {
                    ScriptVariables = _currentEditor.ScriptEngineContext.Variables,
                    ScriptArguments = _currentEditor.ScriptEngineContext.Arguments
                };

                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    _currentEditor.ScriptEngineContext.Variables = scriptVariableEditor.ScriptVariables;

                    if (!string.IsNullOrEmpty(scriptVariableEditor.LastModifiedVariableName))
                        ((ComboBox)sender).Text = "{" + scriptVariableEditor.LastModifiedVariableName + "}";
                }

                scriptVariableEditor.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.J)
            {
                frmScriptArguments scriptArgumentEditor = new frmScriptArguments
                {
                    ScriptArguments = _currentEditor.ScriptEngineContext.Arguments,
                    ScriptVariables = _currentEditor.ScriptEngineContext.Variables
                };

                if (scriptArgumentEditor.ShowDialog() == DialogResult.OK)
                {
                    _currentEditor.ScriptEngineContext.Arguments = scriptArgumentEditor.ScriptArguments;

                    if (!string.IsNullOrEmpty(scriptArgumentEditor.LastModifiedArgumentName))
                        ((ComboBox)sender).Text = "{" + scriptArgumentEditor.LastModifiedArgumentName + "}";
                }

                scriptArgumentEditor.Dispose();
            }
            else if (e.KeyCode == Keys.Enter)
                _currentEditor.uiBtnAdd_Click(null, null);
        }

        private void StandardComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Control + K or Control + J
            if (e.KeyChar == '\v' || e.KeyChar == '\n')
                e.Handled = true;
        }

        private void StandardComboBox_Click(object sender, EventArgs e)
        {
            ComboBox clickedStandardComboBox = (ComboBox)sender;
            clickedStandardComboBox.DroppedDown = true;
        }

        private void StandardComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        public DataGridView CreateDataGridView(object sourceCommand, string dataSourceName)
        {
            var gridView = new DataGridView();
            gridView.AllowUserToAddRows = true;
            gridView.AllowUserToDeleteRows = true;
            gridView.Size = new Size(400, 250);
            gridView.ColumnHeadersHeight = 30;
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridView.DataBindings.Add("DataSource", sourceCommand, dataSourceName, false, DataSourceUpdateMode.OnPropertyChanged);
            gridView.AllowUserToResizeRows = false;
            return gridView;
        }


        public List<Control> CreateUIHelpersFor(string parameterName, ScriptCommand parent, Control[] targetControls, IfrmCommandEditor editor)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var propertyUIHelpers = variableProperties.GetCustomAttributes(typeof(EditorAttribute), true);
            var controlList = new List<Control>();

            if (propertyUIHelpers.Count() == 0)
            {
                return controlList;
            }

            foreach (EditorAttribute attrib in propertyUIHelpers)
            {
                CommandItemControl helperControl = new CommandItemControl();
                helperControl.Padding = new Padding(10, 0, 0, 0);
                helperControl.ForeColor = Color.AliceBlue;
                helperControl.Font = new Font("Segoe UI Semilight", 10);
                helperControl.Name = parameterName + "_helper";
                helperControl.Tag = targetControls.FirstOrDefault();
                helperControl.HelperType = (UIAdditionalHelperType)Enum.Parse(typeof(UIAdditionalHelperType), attrib.EditorTypeName, true);

                switch (helperControl.HelperType)
                {
                    case UIAdditionalHelperType.ShowVariableHelper:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_parse;
                        helperControl.CommandDisplay = "Insert Variable/Argument";
                        helperControl.Click += (sender, e) => ShowVariableSelector(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowElementHelper:
                        //show element selector
                        helperControl.CommandImage = Resources.command_element;
                        helperControl.CommandDisplay = "Insert Element";
                        helperControl.Click += (sender, e) => ShowElementSelector(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowFileSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = Resources.command_files;
                        helperControl.CommandDisplay = "Select a File";
                        helperControl.Click += (sender, e) => ShowFileSelector(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowFolderSelectionHelper:
                        //show folder selector
                        helperControl.CommandImage = Resources.command_folders;
                        helperControl.CommandDisplay = "Select a Folder";
                        helperControl.Click += (sender, e) => ShowFolderSelector(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowImageCaptureHelper:
                        //show image capture
                        helperControl.CommandImage = Resources.command_camera;
                        helperControl.CommandDisplay = "Capture Reference Image";
                        helperControl.Click += (sender, e) => ShowImageCapture(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowImageRecognitionTestHelper:
                        //show image recognition test
                        helperControl.CommandImage = Resources.command_camera;
                        helperControl.CommandDisplay = "Run Image Recognition Test";
                        helperControl.Click += (sender, e) => RunImageCapture(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowCodeBuilder:
                        //show code builder
                        helperControl.CommandImage = Resources.command_script;
                        helperControl.CommandDisplay = "Code Builder";
                        helperControl.Click += (sender, e) => ShowCodeBuilder(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowMouseCaptureHelper:
                        //show mouse capture
                        helperControl.CommandImage = Resources.command_input;
                        helperControl.CommandDisplay = "Capture Mouse Position";
                        helperControl.ForeColor = Color.AliceBlue;
                        helperControl.Click += (sender, e) => ShowMouseCaptureForm(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.ShowElementRecorder:
                        //show element recorder
                        helperControl.CommandImage = Resources.command_camera;
                        helperControl.CommandDisplay = "Element Recorder";
                        helperControl.Click += (sender, e) => ShowElementRecorder(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.GenerateDLLParameters:
                        //show dll parameters
                        helperControl.CommandImage = Resources.command_run_code;
                        helperControl.CommandDisplay = "Generate Parameters";
                        helperControl.Click += (sender, e) => GenerateDLLParameters(sender, e);
                        break;
                    case UIAdditionalHelperType.ShowDLLExplorer:
                        //show dll explorer
                        helperControl.CommandImage = Resources.command_run_code;
                        helperControl.CommandDisplay = "Launch DLL Explorer";
                        helperControl.Click += (sender, e) => ShowDLLExplorer(sender, e);
                        break;
                    case UIAdditionalHelperType.AddInputParameter:
                        //show new input parameter
                        helperControl.CommandImage = Resources.command_run_code;
                        helperControl.CommandDisplay = "Add Input Parameter";
                        helperControl.Click += (sender, e) => AddInputParameter(sender, e);
                        break;
                    case UIAdditionalHelperType.ShowHTMLBuilder:
                        //show html builder
                        helperControl.CommandImage = Resources.command_web;
                        helperControl.CommandDisplay = "Launch HTML Builder";
                        helperControl.Click += (sender, e) => ShowHTMLBuilder(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.ShowIfBuilder:
                        //show if builder
                        helperControl.CommandImage = Resources.command_begin_if;
                        helperControl.CommandDisplay = "Add New If Statement";
                        break;
                    case UIAdditionalHelperType.ShowLoopBuilder:
                        //show loop builder
                        helperControl.CommandImage = Resources.command_startloop;
                        helperControl.CommandDisplay = "Add New Loop Statement";
                        break;
                    case UIAdditionalHelperType.ShowEncryptionHelper:
                        //show encryption helper
                        helperControl.CommandImage = Resources.command_password;
                        helperControl.CommandDisplay = "Encrypt Text";
                        helperControl.Click += (sender, e) => EncryptText(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.CaptureWindowHelper:
                        //show window name helper
                        helperControl.CommandImage = Resources.command_window;
                        helperControl.CommandDisplay = "Capture Window Name";
                        helperControl.Click += (sender, e) => GetWindowName(sender, e);
                        break;
                }

                controlList.Add(helperControl);
            }

            if(targetControls.FirstOrDefault() is DataGridView)
            {
                targetControls.FirstOrDefault().KeyDown += DataGridView_KeyDown;
                targetControls.FirstOrDefault().KeyPress += DataGridView_KeyPress;
            }

            return controlList;
        }

        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            if (e.Control && e.KeyCode == Keys.K)
            {
                frmScriptVariables scriptVariableEditor = new frmScriptVariables
                {
                    ScriptVariables = _currentEditor.ScriptEngineContext.Variables,
                    ScriptArguments = _currentEditor.ScriptEngineContext.Arguments
                };

                if (scriptVariableEditor.ShowDialog() == DialogResult.OK)
                {
                    _currentEditor.ScriptEngineContext.Variables = scriptVariableEditor.ScriptVariables;

                    if (!string.IsNullOrEmpty(scriptVariableEditor.LastModifiedVariableName))
                        dataGridView.CurrentCell.Value = "{" + scriptVariableEditor.LastModifiedVariableName + "}";                   
                }

                scriptVariableEditor.Dispose();
            }
            else if (e.Control && e.KeyCode == Keys.J)
            {
                frmScriptArguments scriptArgumentEditor = new frmScriptArguments
                {
                    ScriptArguments = _currentEditor.ScriptEngineContext.Arguments,
                    ScriptVariables = _currentEditor.ScriptEngineContext.Variables,
                };

                if (scriptArgumentEditor.ShowDialog() == DialogResult.OK)
                {
                    _currentEditor.ScriptEngineContext.Arguments = scriptArgumentEditor.ScriptArguments;

                    if (!string.IsNullOrEmpty(scriptArgumentEditor.LastModifiedArgumentName))
                        dataGridView.CurrentCell.Value = "{" + scriptArgumentEditor.LastModifiedArgumentName + "}";
                }

                scriptArgumentEditor.Dispose();
            }
            else if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Enter)
                return;
            else if (e.KeyCode == Keys.Enter)
                _currentEditor.uiBtnAdd_Click(null, null);
        }

        private void DataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Control + K or Control + J
            if (e.KeyChar == '\v' || e.KeyChar == '\n')
                e.Handled = true;
        }

        private void ShowCodeBuilder(object sender, EventArgs e)
        {
            //get textbox text
            CommandItemControl commandItem = (CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)commandItem.Tag;

            frmCodeBuilder codeBuilder = new frmCodeBuilder(targetTextbox.Text);

            if (codeBuilder.ShowDialog() == DialogResult.OK)
            {
                targetTextbox.Text = codeBuilder.rtbCode.Text;
            }

            codeBuilder.Dispose();
        }

        private void ShowMouseCaptureForm(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            frmShowCursorPosition frmShowCursorPos = new frmShowCursorPosition();

            //if user made a successful selection
            if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
            {
                //add selected variables to associated control text
                ((frmCommandEditor)editor).flw_InputVariables.Controls["v_XMousePosition"].Text = frmShowCursorPos.XPosition.ToString();
                ((frmCommandEditor)editor).flw_InputVariables.Controls["v_YMousePosition"].Text = frmShowCursorPos.YPosition.ToString();
            }

            frmShowCursorPos.Dispose();
        }

        public void ShowVariableSelector(object sender, EventArgs e)
        {
            //create variable selector form
            frmVariableArgumentSelector newVariableSelector = new frmVariableArgumentSelector();

            //get copy of user variables and append system variables, then load to listview
            var variableList = _currentEditor.ScriptEngineContext.Variables.Select(f => f.VariableName).ToList();
            variableList.AddRange(CommonMethods.GenerateSystemVariables().Select(f => f.VariableName));
            newVariableSelector.lstVariables.Items.AddRange(variableList.ToArray());

            //get copy of user arguments, then load to listview
            var argumentList = _currentEditor.ScriptEngineContext.Arguments.Select(f => f.ArgumentName).ToList();
            newVariableSelector.lstArguments.Items.AddRange(argumentList.ToArray());

            //if user pressed "OK"
            if (newVariableSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a variable was actually selected
                if (newVariableSelector.ReturnVariableArgument == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no variables selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert variable' button instance
                CommandItemControl inputBox = (CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes

                //load settings
                var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

                if (inputBox.Tag is TextBox)
                {
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = targetTextbox.Text.Insert(targetTextbox.SelectionStart, string.Concat("{",
                        newVariableSelector.ReturnVariableArgument, "}"));
                }
                else if (inputBox.Tag is ComboBox)
                {
                    ComboBox targetCombobox = (ComboBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetCombobox.Text = targetCombobox.Text.Insert(targetCombobox.SelectionStart, string.Concat("{",
                        newVariableSelector.ReturnVariableArgument, "}"));
                }
                else if (inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    if (targetDGV.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please make sure you have selected an action and selected a cell before attempting" +
                            " to insert a variable!", "No Cell Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (targetDGV.SelectedCells[0].ColumnIndex == 0)
                    {
                        MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    targetDGV.SelectedCells[0].Value = targetDGV.SelectedCells[0].Value +
                        string.Concat("{", newVariableSelector.ReturnVariableArgument, "}");

                    //TODO - Insert variables at cursor position instead of at the end of a cell
                    //targetDGV.CurrentCell = targetDGV.SelectedCells[0];
                    //targetDGV.BeginEdit(false);
                    //TextBox editor = (TextBox)targetDGV.EditingControl;
                    //editor.Text = editor.Text.Insert(editor.SelectionStart, string.Concat("{",
                    //    newVariableSelector.lstVariables.SelectedItem.ToString(), "}"));

                }
            }

            newVariableSelector.Dispose();
        }

        public void ShowElementSelector(object sender, EventArgs e)
        {
            //create element selector form
            frmElementSelector newElementSelector = new frmElementSelector();

            //get copy of user element and append system elements, then load to combobox
            var elementList = _currentEditor.ScriptEngineContext.Elements.Select(f => f.ElementName).ToList();

            newElementSelector.lstElements.Items.AddRange(elementList.ToArray());

            //if user pressed "OK"
            if (newElementSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a element was actually selected
                if (newElementSelector.lstElements.SelectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no elements selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert element' button instance
                CommandItemControl inputBox = (CommandItemControl)sender;

                if (inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    targetDGV.DataSource = _currentEditor.ScriptEngineContext.Elements
                        .Where(x => x.ElementName == newElementSelector.lstElements.SelectedItem.ToString().Replace("<", "").Replace(">", ""))
                        .FirstOrDefault().ElementValue;
                }
            }

            newElementSelector.Dispose();
        }

        private void ShowFileSelector(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = _projectPath;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CommandItemControl inputBox = (CommandItemControl)sender;

                TextBox targetTextbox = (TextBox)inputBox.Tag;

                string filePath = ofd.FileName;

                if (filePath.StartsWith(_projectPath))
                    filePath = filePath.Replace(_projectPath, "{ProjectPath}");

                targetTextbox.Text = filePath;
            }
        }

        private void ShowFolderSelector(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = _projectPath;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                CommandItemControl inputBox = (CommandItemControl)sender;
                TextBox targetTextBox = (TextBox)inputBox.Tag;

                string folderPath = fbd.SelectedPath;

                if (folderPath.StartsWith(_projectPath))
                    folderPath = folderPath.Replace(_projectPath, "{ProjectPath}");

                targetTextBox.Text = folderPath;
            }
        }

        private void ShowImageCapture(object sender, EventArgs e)
        {
            ApplicationSettings settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            var minimizePreference = settings.ClientSettings.MinimizeToTray;

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = false;
                settings.Save(settings);
            }

            HideAllForms();

            var userAcceptance = MessageBox.Show("The image capture process will now begin and display a screenshot of the" +
                " current desktop in a custom full-screen window.  You may stop the capture process at any time by pressing" +
                " the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start" +
                " the rectangle and clicking again to finish. The image will be cropped to the boundary within the red rectangle." +
                " Shall we proceed?", "Image Capture", MessageBoxButtons.YesNo);

            if (userAcceptance == DialogResult.Yes)
            {
                frmImageCapture imageCaptureForm = new frmImageCapture();

                if (imageCaptureForm.ShowDialog() == DialogResult.OK)
                {
                    CommandItemControl inputBox = (CommandItemControl)sender;
                    UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
                    targetPictureBox.Image = imageCaptureForm.UserSelectedBitmap;
                    var convertedImage = CommonMethods.ImageToBase64(imageCaptureForm.UserSelectedBitmap);
                    targetPictureBox.EncodedImage = convertedImage;
                    targetPictureBox.DataBindings[0].WriteValue();
                }

                imageCaptureForm.Dispose();
            }

            ShowAllForms();

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = true;
                settings.Save(settings);
            }
        }

        private void RunImageCapture(object sender, EventArgs e)
        {
            //get input control
            CommandItemControl inputBox = (CommandItemControl)sender;
            UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
            string imageSource = targetPictureBox.EncodedImage;

            if (string.IsNullOrEmpty(imageSource))
            {
                MessageBox.Show("Please capture an image before attempting to test!");
                return;
            }

            //hide all
            HideAllForms();

            try
            {
                ImageElement element = FindImageElementTest(new Bitmap(CommonMethods.Base64ToImage(imageSource)), 0.8);
                if (element == null)
                    MessageBox.Show("Image not found");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            //show all forms
            ShowAllForms();
        }

        public ImageElement FindImageElementTest(Bitmap smallBmp, double accuracy)
        {
            HideAllForms();

            dynamic element = null;
            double tolerance = 1.0 - accuracy;

            Bitmap bigBmp = ImageMethods.Screenshot();

            Bitmap smallTestBmp = new Bitmap(smallBmp);

            Bitmap bigTestBmp = new Bitmap(bigBmp);
            Graphics bigTestGraphics = Graphics.FromImage(bigTestBmp);

            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound)
                                break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            element = new ImageElement
                            {
                                LeftX = x,
                                MiddleX = x + smallBmp.Width / 2,
                                RightX = x + smallBmp.Width,
                                TopY = y,
                                MiddleY = y + smallBmp.Height / 2,
                                BottomY = y + smallBmp.Height
                            };

                            //draw on output to demonstrate finding
                            var Rectangle = new Rectangle(x, y, smallBmp.Width - 1, smallBmp.Height - 1);
                            Pen pen = new Pen(Color.Red);
                            pen.Width = 5.0F;
                            bigTestGraphics.DrawRectangle(pen, Rectangle);

                            frmImageCapture captureOutput = new frmImageCapture();
                            captureOutput.pbTaggedImage.Image = smallTestBmp;
                            captureOutput.pbSearchResult.Image = bigTestBmp;
                            captureOutput.TopMost = true;
                            captureOutput.Show();
                            
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound)
                        break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);
            bigTestGraphics.Dispose();
            return element;
        }

        public Tuple<string, string> ShowConditionElementRecorder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            IConditionCommand cmd = (IConditionCommand)editor.SelectedCommand;
            //create recorder
            frmAdvancedUIElementRecorder newElementRecorder = new frmAdvancedUIElementRecorder(_container);
            newElementRecorder.chkStopOnClick.Checked = true;

            DataTable searchParameters = new DataTable();
            searchParameters.Columns.Add("Enabled");
            searchParameters.Columns.Add("Parameter Name");
            searchParameters.Columns.Add("Parameter Value");
            searchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            newElementRecorder.SearchParameters = searchParameters;

            //show form
            newElementRecorder.ShowDialog();

            var requestedSearchParameter = cmd.v_ActionParameterTable.Rows[1][1].ToString();

            var check = newElementRecorder.SearchParameters;
            var parameterRow = newElementRecorder.SearchParameters.AsEnumerable().Where(x => x.Field<string>("Parameter Name") == requestedSearchParameter).FirstOrDefault();

            string parameterValue = "";
            if (parameterRow != null)
                parameterValue = parameterRow.ItemArray[2].ToString();

            var elementParameters = new Tuple<string, string>(newElementRecorder.WindowName, parameterValue);

            newElementRecorder.Dispose();

            return elementParameters;
        }

        private void ShowElementRecorder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            //get command reference
            IUIAutomationCommand cmd = (IUIAutomationCommand)editor.SelectedCommand;

            //create recorder
            frmAdvancedUIElementRecorder newElementRecorder = new frmAdvancedUIElementRecorder(_container);
            newElementRecorder.SearchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            ComboBox txtWindowName = (ComboBox)((frmCommandEditor)editor).flw_InputVariables.Controls["v_WindowName"];
            txtWindowName.Text = newElementRecorder.cboWindowTitle.Text;

            ((frmCommandEditor)editor).WindowState = FormWindowState.Normal;
            ((frmCommandEditor)editor).BringToFront();
        }

        private void GenerateDLLParameters(object sender, EventArgs e)
        {
            IExecuteDLLCommand cmd = (IExecuteDLLCommand)_currentEditor.SelectedCommand;

            var filePath = _currentEditor.flw_InputVariables.Controls["v_FilePath"].Text;
            var className = _currentEditor.flw_InputVariables.Controls["v_ClassName"].Text;
            var methodName = _currentEditor.flw_InputVariables.Controls["v_MethodName"].Text;
            DataGridView parameterBox = (DataGridView)_currentEditor.flw_InputVariables.Controls["v_MethodParameters"];

            //clear all rows
            cmd.v_MethodParameters.Rows.Clear();

            //Load Assembly
            try
            {
                Assembly requiredAssembly = Assembly.LoadFrom(filePath);

                //get type
                Type t = requiredAssembly.GetType(className);

                //verify type was found
                if (t == null)
                {
                    MessageBox.Show("The class '" + className + "' was not found in assembly loaded at '" + filePath + "'",
                        "Class Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get method
                MethodInfo m = t.GetMethod(methodName);

                //verify method found
                if (m == null)
                {
                    MessageBox.Show("The method '" + methodName + "' was not found in assembly loaded at '" + filePath + "'",
                        "Method Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get parameters
                var reqdParams = m.GetParameters();

                if (reqdParams.Length > 0)
                {
                    cmd.v_MethodParameters.Rows.Clear();
                    foreach (var param in reqdParams)
                    {
                        cmd.v_MethodParameters.Rows.Add(param.Name, "");
                    }
                }
                else
                {
                    MessageBox.Show("There are no parameters required for this method!", "No Parameters Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error generating the parameters: " + ex.ToString());
            }
        }

        private void ShowDLLExplorer(object sender, EventArgs e)
        {
            //create form
            frmDLLExplorer dllExplorer = new frmDLLExplorer();

            //show dialog
            if (dllExplorer.ShowDialog() == DialogResult.OK)
            {
                //user accepted the selections
                //declare command
                IExecuteDLLCommand cmd = (IExecuteDLLCommand)_currentEditor.SelectedCommand;

                //add file name
                if (!string.IsNullOrEmpty(dllExplorer.FileName))
                {
                    _currentEditor.flw_InputVariables.Controls["v_FilePath"].Text = dllExplorer.FileName;
                }

                //add class name
                if (dllExplorer.lstClasses.SelectedItem != null)
                {
                    _currentEditor.flw_InputVariables.Controls["v_ClassName"].Text = dllExplorer.lstClasses.SelectedItem.ToString();
                }

                //add method name
                if (dllExplorer.lstMethods.SelectedItem != null)
                {
                    _currentEditor.flw_InputVariables.Controls["v_MethodName"].Text = dllExplorer.lstMethods.SelectedItem.ToString();
                }

                cmd.v_MethodParameters.Rows.Clear();

                //add parameters
                if ((dllExplorer.lstParameters.Items.Count > 0) &&
                    (dllExplorer.lstParameters.Items[0].ToString() != "This method requires no parameters!"))
                {
                    foreach (var param in dllExplorer.SelectedParameters)
                    {
                        cmd.v_MethodParameters.Rows.Add(param, "");
                    }
                }
            }

            dllExplorer.Dispose();
        }

        private void AddInputParameter(object sender, EventArgs e)
        {
            DataGridView inputControl = (DataGridView)_currentEditor.flw_InputVariables.Controls["v_UserInputConfig"];
            var inputTable = (DataTable)inputControl.DataSource;
            var newRow = inputTable.NewRow();
            newRow["Size"] = "500,100";
            inputTable.Rows.Add(newRow);
        }

        private void ShowHTMLBuilder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            var htmlForm = new frmHTMLBuilder();

            TextBox inputControl = (TextBox)((frmCommandEditor)editor).flw_InputVariables.Controls["v_InputHTML"];
            htmlForm.rtbHTML.Text = ((frmCommandEditor)editor).flw_InputVariables.Controls["v_InputHTML"].Text;

            if (htmlForm.ShowDialog() == DialogResult.OK)
            {
                inputControl.Text = htmlForm.rtbHTML.Text;
            }

            htmlForm.Dispose();
        }

        private void EncryptText(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            CommandItemControl inputBox = (CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)inputBox.Tag;

            if (string.IsNullOrEmpty(targetTextbox.Text))
                return;

            var encrypted = EncryptionServices.EncryptString(targetTextbox.Text, "OPENBOTS");
            targetTextbox.Text = encrypted;

            ComboBox comboBoxControl = (ComboBox)((frmCommandEditor)editor).flw_InputVariables.Controls["v_EncryptionOption"];
            comboBoxControl.Text = "Encrypted";
        }

        private void GetWindowName(object sender, EventArgs e)
        {
            GlobalHook.MouseEvent -= GlobalHook_MouseEvent;
            _inputBox = (CommandItemControl)sender;
            _settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            _minimizePreference = _settings.ClientSettings.MinimizeToTray;

            if (_minimizePreference)
            {
                _settings.ClientSettings.MinimizeToTray = false;
                _settings.Save(_settings);
            }

            SendAllFormsToBack();
            GlobalHook.StartElementCaptureHook(true);
            GlobalHook.MouseEvent += GlobalHook_MouseEvent;
        }

        private void GlobalHook_MouseEvent(object sender, MouseCoordinateEventArgs e)
        {
            //mouse down has occured
            if (e != null)
            {
                try
                {
                    Point point = new Point((int)e.MouseCoordinates.X, (int)e.MouseCoordinates.Y);
                    var window = User32Functions.WindowFromPoint(point);

                    User32Functions.GetWindowThreadProcessId(window, out uint processId);
                    Process process = Process.GetProcessById((int)processId);

                    var windowName = process.MainWindowTitle;

                    if (string.IsNullOrEmpty(windowName))
                    {
                        MessageBox.Show("Could not find Window", "Error");
                        GlobalHook.MouseEvent -= GlobalHook_MouseEvent;
                        return;
                    }

                    if (_inputBox.Tag is ComboBox)
                    {
                        ComboBox targetComboBox = (ComboBox)_inputBox.Tag;
                        targetComboBox.Text = windowName;
                    }
                  
                    if (_minimizePreference)
                    {
                        _settings.ClientSettings.MinimizeToTray = true;
                        _settings.Save(_settings);
                    }

                    GlobalHook.MouseEvent -= GlobalHook_MouseEvent;                  
                }
                catch (Exception)
                {                  
                    if (_minimizePreference)
                    {
                        _settings.ClientSettings.MinimizeToTray = true;
                        _settings.Save(_settings);
                    }

                    GlobalHook.MouseEvent -= GlobalHook_MouseEvent;

                    MessageBox.Show("Could not find Window", "Error");
                }
            } 
        }

        public void SendAllFormsToBack()
        {
            foreach (Form form in Application.OpenForms)
                SendFormToBack(form);

            Thread.Sleep(1000);
        }

        public delegate void MoveFormToBackDelegate(Form form);
        public void SendFormToBack(Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new MoveFormToBackDelegate(SendFormToBack);
                form.Invoke(d, new object[] { form });
            }
            else
            {
                form.TopMost = false;
                form.SendToBack();
            }              
        }

        public void ShowAllForms()
        {
            foreach (Form form in Application.OpenForms)
                ShowForm(form);

            Thread.Sleep(1000);
        }

        public delegate void ShowFormDelegate(Form form);
        public void ShowForm(Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new ShowFormDelegate(ShowForm);
                form.Invoke(d, new object[] { form });
            }
            else
                form.WindowState = FormWindowState.Normal;
        }

        public void HideAllForms()
        {
            foreach (Form form in Application.OpenForms)
                HideForm(form);

            Thread.Sleep(1000);
        }

        public delegate void HideFormDelegate(Form form);
        public void HideForm(Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new HideFormDelegate(HideForm);
                form.Invoke(d, new object[] { form });
            }
            else
                form.WindowState = FormWindowState.Minimized;
        }

        public ComboBox AddWindowNames(ComboBox cbo)
        {
            if (cbo == null)
                return null;

            cbo.Items.Clear();
            cbo.Items.Add("Current Window");

            Process[] processlist = Process.GetProcesses();

            //pull the main window title for each
            foreach (Process process in processlist)
            {
                if (!string.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    cbo.Items.Add(process.MainWindowTitle);
                }
            }

            return cbo;
        }

        public ComboBox AddVariableNames(ComboBox cbo, IfrmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            if (editor != null)
            {
                cbo.Items.Clear();

                List<string> varArgNames = new List<string>();

                foreach (var variable in ((frmCommandEditor)editor).ScriptEngineContext.Variables)
                {
                    if (variable.VariableName != "ProjectPath")
                        varArgNames.Add("{" + variable.VariableName + "}");
                }

                foreach (var argument in ((frmCommandEditor)editor).ScriptEngineContext.Arguments)
                    varArgNames.Add("{" + argument.ArgumentName + "}");

                cbo.Items.AddRange(varArgNames.OrderBy(x => x).ToArray());               
            }
            return cbo;
        }

        public ComboBox AddElementNames(ComboBox cbo, frmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            if (editor != null)
            {
                cbo.Items.Clear();

                foreach (var element in editor.ScriptEngineContext.Elements)
                    cbo.Items.Add("<" + element.ElementName + ">");
            }
            return cbo;
        }

        public IfrmScriptEngine CreateScriptEngineForm(EngineContext engineContext, bool blnCloseWhenDone, bool isDebugMode)
        {
            return new frmScriptEngine(engineContext, blnCloseWhenDone, isDebugMode);
        }

        public IAutomationEngineInstance CreateAutomationEngineInstance(EngineContext engineContext)
        {
            return new AutomationEngineInstance(engineContext);
        }

        public IfrmWebElementRecorder CreateWebElementRecorderForm(string startURL)
        {
            return new frmWebElementRecorder(_container, startURL);
        }

        public IfrmAdvancedUIElementRecorder CreateAdvancedUIElementRecorderForm()
        {
            return new frmAdvancedUIElementRecorder(_container);
        }

        public IfrmCommandEditor CreateCommandEditorForm(List<AutomationCommand> commands, List<ScriptCommand> existingCommands)
        {
            frmCommandEditor editor = new frmCommandEditor(commands, existingCommands);
            editor.ScriptEngineContext.Container = _container;

            return editor;
        }

        public ScriptCommand CreateBeginIfCommand(string commandData)
        {
            if(string.IsNullOrEmpty(commandData))
                return (dynamic)TypeMethods.CreateTypeInstance(_container, "BeginIfCommand");
            else
            {
                Type ifCommandType = TypeMethods.GetTypeByName(_container, "BeginIfCommand");
                return (dynamic) JsonConvert.DeserializeObject(commandData, ifCommandType);
                //return JsonConvert.DeserializeObject<typeof(ifCommand)> (commandData);
            }
        }

        public Type GetCommandType(string commandName)
        {
            Type type = null;
            if (commandName == "BeginIfCommand")
                type = TypeMethods.GetTypeByName(_container, "BeginIfCommand");

            return type;
        }
    }
}
