using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region Variable/Argument Tab Events
        private void dgvVariablesArguments_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //creates a list of all existing variable/argument names to check against, prior to creating a new one
                DataGridView dgv = (DataGridView)sender;

                _preEditVarArgName = dgv.Rows[e.RowIndex].Cells[0].Value?.ToString();

                _existingVarArgSearchList = new List<string>();
                _existingVarArgSearchList.AddRange(_scriptArguments.Select(arg => arg.ArgumentName).ToList());
                _existingVarArgSearchList.AddRange(_scriptVariables.Select(var => var.VariableName).ToList());
                _existingVarArgSearchList.AddRange(CommonMethods.GenerateSystemVariables().Select(var => var.VariableName).ToList());
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (e.ColumnIndex == 0)
                {
                    var cellValue = dgv.Rows[e.RowIndex].Cells[0].Value;

                    //deletes an empty row if it's created without assigning values
                    if ((cellValue == null && _preEditVarArgName != null) ||
                        (cellValue != null && string.IsNullOrEmpty(cellValue.ToString().Trim())))
                    {
                        dgv.Rows.RemoveAt(e.RowIndex);
                        return;
                    }
                    //removes an empty uncommitted row
                    else if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
                        return;

                    //trims any space characters before reassigning the value to the cell
                    string variableName = dgv.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                    dgv.Rows[e.RowIndex].Cells[0].Value = variableName;

                    //prevents user from creating a new variable/argument with an already used name
                    if (_existingVarArgSearchList.Contains(variableName) && variableName != _preEditVarArgName)
                    {
                        Notify($"A variable or argument with the name '{variableName}' already exists", Color.Red);
                        dgv.Rows.RemoveAt(e.RowIndex);
                    }
                    //if the variable/argument name is valid, set value cell's readonly as false
                    else
                    {
                        foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                            cell.ReadOnly = false;                          

                        dgv.Rows[e.RowIndex].Cells[0].Value = variableName.Trim();

                        //marks the script as unsaved with changes
                        if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                            uiScriptTabControl.SelectedTab.Text += " *";
                    }
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                if (dgv.Rows[e.RowIndex].Cells[0].Value == null)
                {
                    //when a new row is added, all cells except for the variable/argument name will be readonly until the name is validated
                    foreach (DataGridViewCell cell in dgv.Rows[e.RowIndex].Cells)
                    {
                        if (cell.ColumnIndex != 0)
                            cell.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                //prevents the ProjectPath row from being deleted
                if (e.Row.Cells[0].Value?.ToString() == "ProjectPath")
                    e.Cancel = true;
                else
                {
                    //marks the script as unsaved with changes
                    if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                        uiScriptTabControl.SelectedTab.Text += " *";
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;
               
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    //sets the entire ProjectPath row as readonly
                    if (row.Cells[0].Value?.ToString() == "ProjectPath")
                        row.ReadOnly = true;

                    //sets Value cell to readonly if the Direction is Out
                    if (row.Cells.Count == 3 && row.Cells[2].Value != null && (ScriptArgumentDirection)row.Cells[2].Value == ScriptArgumentDirection.Out)
                        row.Cells[1].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariables_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                var dgvBindingList = (BindingList<ScriptVariable>)dgv.DataSource;
                var nullScriptVariable = dgvBindingList.Where(x => string.IsNullOrEmpty(x.VariableName)).FirstOrDefault();

                if (nullScriptVariable != null && dgvBindingList.Count > 1)
                    dgvBindingList.Remove(nullScriptVariable);

            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvArguments_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                var dgvBindingList = (BindingList<ScriptArgument>)dgv.DataSource;
                var nullScriptVariable = dgvBindingList.Where(x => string.IsNullOrEmpty(x.ArgumentName)).FirstOrDefault();

                if (nullScriptVariable != null && dgvBindingList.Count > 1)
                    dgvBindingList.Remove(nullScriptVariable);

            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvArguments_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
                //sets Direction to In by default when a new row is added. Prevents cell from ever being null
                e.Row.Cells["Direction"].Value = ScriptArgumentDirection.In;
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvArguments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    DataGridView dgv = (DataGridView)sender;
                    DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv.Rows[e.RowIndex].Cells[2];

                    if (cb.Value != null)
                    {
                        //Sets value cell to read only if the argument direction is set to Out
                        if ((ScriptArgumentDirection)cb.Value == ScriptArgumentDirection.Out)
                        {
                            dgv.Rows[e.RowIndex].Cells[1].Value = null;
                            dgv.Rows[e.RowIndex].Cells[1].ReadOnly = true;
                        }

                        else if ((ScriptArgumentDirection)cb.Value == ScriptArgumentDirection.In)
                            dgv.Rows[e.RowIndex].Cells[1].ReadOnly = false;
                    }                    
                }
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }                       
        }

        private void dgvArguments_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                DataGridView dgv = (DataGridView)sender;

                //this fires the cell value changed handler above
                if (dgv.IsCurrentCellDirty)
                    dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
            catch (Exception ex)
            {
                //datagridview event failure
                Console.WriteLine(ex);
            }
        }

        private void dgvVariablesArguments_KeyDown(object sender, KeyEventArgs e)
        {
            //various advanced keystroke shortcuts for saving, creating new var/arg/elem, shortcut menu, etc
            if (e.Control)
            {
                if (e.Shift)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            SaveAllFiles();
                            break;
                    }
                }
                else
                {
                    switch (e.KeyCode)
                    {
                        case Keys.S:
                            ClearSelectedListViewItems();
                            SaveToFile(false);
                            break;
                        case Keys.J:
                            OpenArgumentManager();
                            break;
                        case Keys.K:
                            OpenVariableManager();
                            break;
                        case Keys.L:
                            OpenElementManager();
                            break;
                        case Keys.M:
                            shortcutMenuToolStripMenuItem_Click(null, null);
                            break;
                        case Keys.O:
                            aboutOpenBotsToolStripMenuItem_Click(null, null);
                            break;
                    }
                }
            }
        }
        #endregion             
    }
}
