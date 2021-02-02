using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form, IfrmScriptBuilder
    {
        public delegate void CreateDebugTabDelegate();
        private void CreateDebugTab()
        {
            if (InvokeRequired)
            {
                var d = new CreateDebugTabDelegate(CreateDebugTab);
                Invoke(d, new object[] { });
            }
            else
            {
                TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "Debug")
                                                                              .FirstOrDefault();

                if (debugTab == null)
                {
                    debugTab = new TabPage();
                    debugTab.Name = "Debug";
                    debugTab.Text = "Debug";
                    uiPaneTabs.TabPages.Add(debugTab);
                    uiPaneTabs.SelectedTab = debugTab;

                    FlowLayoutPanel debugPanel = new FlowLayoutPanel();
                    debugPanel.Name = "DebugPanel";
                    debugPanel.AutoScroll = true;
                    debugPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    debugPanel.BackColor = Color.FromArgb(59, 59, 59);
                    debugPanel.Dock = DockStyle.Fill;
                    debugPanel.FlowDirection = FlowDirection.TopDown;
                    debugPanel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
                    debugPanel.Padding = new Padding(0, 5, 0, 0);
                    debugPanel.WrapContents = false;

                    debugTab.Controls.Add(debugPanel);
                }
                LoadDebugTab(debugTab);
            }          
        }

        public delegate void LoadDebugTabDelegate(TabPage debugTab);
        private void LoadDebugTab(TabPage debugTab)
        {
            if (InvokeRequired)
            {
                var d = new LoadDebugTabDelegate(LoadDebugTab);
                Invoke(d, new object[] { debugTab });
            }
            else
            {
                DataTable variableValues = new DataTable();
                variableValues.Columns.Add("Name");
                variableValues.Columns.Add("Type");
                variableValues.Columns.Add("Value");
                variableValues.TableName = "VariableValuesDataTable" + DateTime.Now.ToString("MMddyyhhmmss");

                DataGridView variableGridViewHelper = new DataGridView();
                variableGridViewHelper.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                variableGridViewHelper.AutoSize = true;
                variableGridViewHelper.Dock = DockStyle.Fill;
                variableGridViewHelper.ColumnHeadersHeight = 30;
                variableGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                variableGridViewHelper.AllowUserToAddRows = false;
                variableGridViewHelper.AllowUserToDeleteRows = false;
                variableGridViewHelper.ReadOnly = true;
                variableGridViewHelper.CellContentDoubleClick += DebugGridViewHelper_CellContentDoubleClick;
                variableGridViewHelper.BorderStyle = BorderStyle.None;

                DataTable argumentValues = new DataTable();
                argumentValues.Columns.Add("Name");
                argumentValues.Columns.Add("Type");
                argumentValues.Columns.Add("Value");
                argumentValues.Columns.Add("Direction");
                argumentValues.TableName = "ArgumentValuesDataTable" + DateTime.Now.ToString("MMddyyhhmmss");

                DataGridView argumentGridViewHelper = new DataGridView();
                argumentGridViewHelper.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                argumentGridViewHelper.AutoSize = true;
                argumentGridViewHelper.Dock = DockStyle.Fill;
                argumentGridViewHelper.ColumnHeadersHeight = 30;
                argumentGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                argumentGridViewHelper.AllowUserToAddRows = false;
                argumentGridViewHelper.AllowUserToDeleteRows = false;
                argumentGridViewHelper.ReadOnly = true;
                argumentGridViewHelper.CellContentDoubleClick += DebugGridViewHelper_CellContentDoubleClick;
                argumentGridViewHelper.BorderStyle = BorderStyle.None;

                if (debugTab.Controls[0].Controls.Count != 0)
                    debugTab.Controls[0].Controls.Clear();

                Label variableLabel = new Label();
                variableLabel.Text = "Variables";
                variableLabel.ForeColor = Color.White;

                Label argumentLabel = new Label();
                argumentLabel.Text = "Arguments";
                argumentLabel.ForeColor = Color.White;

                debugTab.Controls[0].Controls.Add(variableLabel);
                debugTab.Controls[0].Controls.Add(variableGridViewHelper);
                debugTab.Controls[0].Controls.Add(argumentLabel);
                debugTab.Controls[0].Controls.Add(argumentGridViewHelper);

                List<ScriptVariable> engineVariables = ((frmScriptEngine)CurrentEngine).EngineInstance.AutomationEngineContext.Variables;

                foreach (var variable in engineVariables)
                {
                    DataRow[] foundVariables = variableValues.Select("Name = '" + variable.VariableName + "'");
                    if (foundVariables.Length == 0)
                    {
                        string type = "null";
                        if (variable.VariableValue != null)
                            type = variable.VariableValue.GetType().FullName;

                        variableValues.Rows.Add(variable.VariableName, type, StringMethods.ConvertObjectToString(variable.VariableValue));                     
                    }
                }

                List<ScriptArgument> engineArguments = ((frmScriptEngine)CurrentEngine).EngineInstance.AutomationEngineContext.Arguments;

                foreach (var argument in engineArguments)
                {
                    DataRow[] foundArguments = argumentValues.Select("Name = '" + argument.ArgumentName + "'");
                    if (foundArguments.Length == 0)
                    {
                        string type = "null";
                        if (argument.ArgumentValue != null)
                            type = argument.ArgumentValue.GetType().FullName;

                        argumentValues.Rows.Add(argument.ArgumentName, type, StringMethods.ConvertObjectToString(argument.ArgumentValue), argument.Direction.ToString());
                    }
                }

                variableGridViewHelper.DataSource = variableValues;
                argumentGridViewHelper.DataSource = argumentValues;
                uiPaneTabs.SelectedTab = debugTab;

                variableGridViewHelper.Sort(variableGridViewHelper.Columns["Name"], ListSortDirection.Ascending);
                argumentGridViewHelper.Sort(argumentGridViewHelper.Columns["Name"], ListSortDirection.Ascending);
            }           
        }

        private void DebugGridViewHelper_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if the column is 'Value'
            if (e.ColumnIndex == 2)
            {
                string debugName = ((DataGridView)sender).Rows[e.RowIndex].Cells[0].Value.ToString();               
                string debugValue = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                frmDialog debugDialog = new frmDialog(debugValue, debugName, DialogType.CancelOnly, 0);
                debugDialog.Show();
            }
        }

        public delegate void RemoveDebugTabDelegate();
        public void RemoveDebugTab()
        {
            if (InvokeRequired)
            {
                var d = new RemoveDebugTabDelegate(RemoveDebugTab);
                Invoke(d, new object[] { });
            }
            else
            {
                TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "Debug")
                                                                              .FirstOrDefault();

                if (debugTab != null)
                    uiPaneTabs.TabPages.Remove(debugTab);
            }
        }

        public delegate DialogResult LoadErrorFormDelegate(string errorMessage);
        public DialogResult LoadErrorForm(string errorMessage)
        {
            if (InvokeRequired)
            {
                var d = new LoadErrorFormDelegate(LoadErrorForm);
                return (DialogResult)Invoke(d, new object[] { errorMessage });
            }
            else
            {
                frmError errorForm = new frmError(errorMessage);
                errorForm.Owner = this;
                errorForm.ShowDialog();

                var result = errorForm.DialogResult;

                errorForm.Dispose();
                return result;
            }          
        }
    }
}
