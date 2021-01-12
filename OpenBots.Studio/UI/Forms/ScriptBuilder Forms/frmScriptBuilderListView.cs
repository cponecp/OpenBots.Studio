﻿using Newtonsoft.Json;
using OpenBots.Core.Command;
using OpenBots.Core.Common;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Properties;
using OpenBots.Core.UI.DTOs;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CoreResources = OpenBots.Core.Properties.Resources;
using System.ComponentModel;
using OpenBots.Core.Script;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {
        #region ListView Events
        private UIListView NewLstScriptActions(string title = "newLstScriptActions")
        {
            UIListView newLstScriptActions = new UIListView();
            newLstScriptActions.AllowDrop = true;
            newLstScriptActions.BackColor = Color.WhiteSmoke;
            newLstScriptActions.BorderStyle = BorderStyle.None;
            newLstScriptActions.Columns.AddRange(new ColumnHeader[] {
            new ColumnHeader(),
            new ColumnHeader{ Width = 20 },
            new ColumnHeader{ Text = "Script Commands", Width = -2 } });
            newLstScriptActions.Dock = DockStyle.Fill;
            newLstScriptActions.Font = new Font("Segoe UI", 11, FontStyle.Bold, GraphicsUnit.Point, 0);
            newLstScriptActions.FullRowSelect = true;
            newLstScriptActions.HeaderStyle = ColumnHeaderStyle.None;
            newLstScriptActions.HideSelection = false;
            newLstScriptActions.Location = new Point(3, 3);
            newLstScriptActions.Margin = new Padding(6);
            newLstScriptActions.Name = title;
            newLstScriptActions.OwnerDraw = true;
            newLstScriptActions.Size = new Size(1063, 411);
            newLstScriptActions.TabIndex = 6;
            newLstScriptActions.UseCompatibleStateImageBehavior = false;
            newLstScriptActions.View = View.Details;
            newLstScriptActions.DrawSubItem += new DrawListViewSubItemEventHandler(lstScriptActions_DrawSubItem);
            newLstScriptActions.ItemDrag += new ItemDragEventHandler(lstScriptActions_ItemDrag);
            newLstScriptActions.SelectedIndexChanged += new EventHandler(lstScriptActions_SelectedIndexChanged);
            newLstScriptActions.DragDrop += new DragEventHandler(lstScriptActions_DragDrop);
            newLstScriptActions.DragEnter += new DragEventHandler(lstScriptActions_DragEnter);
            newLstScriptActions.DoubleClick += new EventHandler(lstScriptActions_DoubleClick);
            newLstScriptActions.KeyDown += new KeyEventHandler(lstScriptActions_KeyDown);
            newLstScriptActions.MouseClick += new MouseEventHandler(lstScriptActions_MouseClick);
            newLstScriptActions.MouseMove += new MouseEventHandler(lstScriptActions_MouseMove);
            newLstScriptActions.Tag = new ScriptActionTag();
            newLstScriptActions.ShowItemToolTips = true;

            return newLstScriptActions;
        }

        #region ListView DragDrop
        private void lstScriptActions_ItemDrag(object sender, ItemDragEventArgs e)
        {
            _selectedTabScriptActions.DoDragDrop(_selectedTabScriptActions.SelectedItems, DragDropEffects.Move);
        }

        private void lstScriptActions_DragEnter(object sender, DragEventArgs e)
        {
            int len = e.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.
                    e.Effect = DragDropEffects.Move;
                }
                else if (e.Data.GetFormats()[i].Equals("System.Windows.Forms.TreeNode"))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        private void lstScriptActions_DragDrop(object sender, DragEventArgs e)
        {
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = _selectedTabScriptActions.PointToClient(new Point(e.X, e.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = _selectedTabScriptActions.GetItemAt(cp.X, cp.Y);

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                TreeNode commandNode = ((TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode"));

                if (commandNode.Nodes.Count != 0)
                    return;

                var commandName = commandNode.Text;
                var commandGroupName = commandNode.Parent.Text;

                var newCommandName = _automationCommands.Where(x => x.ShortName == commandName && x.DisplayGroup == commandGroupName)
                                                              .Select(x => x.Command).FirstOrDefault().GetType();

                dynamic newCommandInstance = TypeMethods.CreateTypeInstance(AContainer, newCommandName.Name);

                CreateUndoSnapshot();
                if (dragToItem != null)
                    AddCommandToListView(newCommandInstance, dragToItem.Index);
                else
                    AddCommandToListView(newCommandInstance, _selectedTabScriptActions.Items.Count);
            }
            else
            {
                //Return if the items are not selected in the ListView control.
                if (_selectedTabScriptActions.SelectedItems.Count == 0)
                {
                    return;
                }

                CreateUndoSnapshot();
              
                if (dragToItem == null)
                {
                    return;
                }

                //drag and drop for sequence
                if ((dragToItem.Tag.GetType().Name == "SequenceCommand") && (_appSettings.ClientSettings.EnableSequenceDragDrop))
                {
                    //sequence command for drag drop
                    var sequence = (ISequenceCommand)dragToItem.Tag;

                    //add command to script actions
                    for (int i = 0; i <= _selectedTabScriptActions.SelectedItems.Count - 1; i++)
                    {
                        var command = (ScriptCommand)_selectedTabScriptActions.SelectedItems[i].Tag;
                        sequence.ScriptActions.Add(command);
                    }

                    //remove originals
                    for (int i = _selectedTabScriptActions.SelectedItems.Count - 1; i >= 0; i--)
                    {
                        _selectedTabScriptActions.Items.Remove(_selectedTabScriptActions.SelectedItems[i]);
                    }

                    //return back
                    return;
                }

                //Obtain the index of the item at the mouse pointer.
                int dragIndex = dragToItem.Index;

                ListViewItem[] sel = new ListViewItem[_selectedTabScriptActions.SelectedItems.Count];
                for (int i = 0; i <= _selectedTabScriptActions.SelectedItems.Count - 1; i++)
                {
                    sel[i] = _selectedTabScriptActions.SelectedItems[i];
                }
                for (int i = 0; i < sel.GetLength(0); i++)
                {
                    //Obtain the ListViewItem to be dragged to the target location.
                    ListViewItem dragItem = sel[i];
                    int itemIndex = dragIndex;
                    if (itemIndex == dragItem.Index)
                    {
                        return;
                    }
                    if (dragItem.Index < itemIndex)
                        itemIndex++;
                    else
                        itemIndex = dragIndex + i;

                    //Insert the item at the mouse pointer.
                    ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                    _selectedTabScriptActions.Items.Insert(itemIndex, insertItem);
                    //Removes the item from the initial location while
                    //the item is moved to the new location.
                    _selectedTabScriptActions.Items.Remove(dragItem);
                    //FormatCommandListView();
                    _selectedTabScriptActions.Invalidate();
                }
            }
        }

        #endregion

        #region ListView Copy, Paste, Edit, Delete
        private void lstScriptActions_KeyDown(object sender, KeyEventArgs e)
        {
            //delete from listview if required
            if (e.KeyCode == Keys.Delete)
            {
                CreateUndoSnapshot();
                DeleteSelectedCode();          
            }
            else if (e.KeyCode == Keys.Enter)
            {
                //if user presses enter simulate double click event
                lstScriptActions_DoubleClick(null, null);
            }
            else if (e.Control)
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
                        case Keys.X:
                            CutRows();
                            break;
                        case Keys.C:
                            CopyRows();
                            break;
                        case Keys.V:
                            PasteRows();
                            break;
                        case Keys.Z:
                            UndoChange();
                            break;
                        case Keys.R:
                            RedoChange();
                            break;
                        case Keys.A:
                            foreach (ListViewItem item in _selectedTabScriptActions.Items)
                                item.Selected = true;
                            break;
                        case Keys.S:
                            if (_isSequence)
                                uiBtnSaveSequence_Click(null, null);
                            else
                            {
                                ClearSelectedListViewItems();
                                SaveToFile(false);
                            }
                            break;
                        case Keys.E:
                            SetSelectedCodeToCommented(false);
                            break;
                        case Keys.D:
                            SetSelectedCodeToCommented(true);
                            break;
                        case Keys.B:
                            AddRemoveBreakpoint();
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
                        case Keys.Back:
                            clearAllToolStripMenuItem_Click(null, null);
                            break;
                    }
                }                
            }
        }

        private void lstScriptActions_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedTabScriptActions.SelectedItems.Count != 1)
                return;

            //bring up edit mode to edit the action
            ListViewItem selectedCommandItem = _selectedTabScriptActions.SelectedItems[0];

            //set selected command from the listview item tag object which was assigned to the command
            var currentCommand = (ScriptCommand)selectedCommandItem.Tag;

            //check if editing a sequence
            if (currentCommand is ISequenceCommand)
            {
                if (_editMode)
                {
                    MessageBox.Show("Embedding Sequence Commands within Sequence Commands not yet supported.");
                    return;
                }

                //get sequence events
                ISequenceCommand sequence = currentCommand as ISequenceCommand;
                frmScriptBuilder newBuilder = new frmScriptBuilder();

                LoadCommands(newBuilder);

                newBuilder.ScriptProject = ScriptProject;
                newBuilder.ScriptProjectPath = ScriptProjectPath;

                //add variables/elements/arguments
                newBuilder._scriptVariables = _scriptVariables;
                newBuilder._scriptElements = _scriptElements;
                newBuilder._scriptArguments = _scriptArguments;

                newBuilder.dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptVariables);
                newBuilder.dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptArguments);

                TabPage newtabPage = new TabPage("Sequence");
                newtabPage.Name = "Sequence";
                newtabPage.ToolTipText = "Sequence";

                newBuilder.uiScriptTabControl.TabPages.Add(newtabPage);
                newtabPage.Controls.Add(newBuilder._selectedTabScriptActions);
                newBuilder.uiScriptTabControl.SelectedTab = newtabPage;

                //append to new builder
                foreach (var cmd in sequence.ScriptActions)
                {
                    newBuilder._selectedTabScriptActions.Items.Add(CreateScriptCommandListViewItem(cmd));
                }

                //apply editor style format
                newBuilder.ApplyEditorFormat(sequence.v_Comment);

                newBuilder._parentBuilder = this;

                //if data has been changed
                if (newBuilder.ShowDialog() == DialogResult.OK)
                {
                    CreateUndoSnapshot();
                    //create updated list
                    List<ScriptCommand> updatedList = new List<ScriptCommand>();

                    //update to list
                    for (int i = 0; i < newBuilder._selectedTabScriptActions.Items.Count; i++)
                    {
                        var command = (ScriptCommand)newBuilder._selectedTabScriptActions.Items[i].Tag;
                        updatedList.Add(command);
                    }

                    //apply new list to existing sequence
                    sequence.ScriptActions = updatedList;
                    sequence.v_Comment = newBuilder.Text;

                    //update label
                    selectedCommandItem.Text = sequence.GetDisplayValue();

                    //update variables/elements/arguments
                    _scriptVariables = newBuilder._scriptVariables;
                    _scriptElements = newBuilder._scriptElements;
                    _scriptArguments = newBuilder._scriptArguments;

                    dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptVariables);
                    dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptArguments);
                }
            }
            else
            {
                //create new command editor form
                frmCommandEditor editCommand = new frmCommandEditor(_automationCommands, GetConfiguredCommands());

                editCommand.ScriptEngineContext.Container = AContainer;

                //creation mode edit locks form to current command
                editCommand.CreationModeInstance = CreationMode.Edit;

                //editCommand.defaultStartupCommand = currentCommand.SelectionName;
                editCommand.EditingCommand = currentCommand;

                //create clone of current command so databinding does not affect if changes are not saved
                editCommand.OriginalCommand = Common.Clone(currentCommand);

                //set variables
                editCommand.ScriptEngineContext.Variables = _scriptVariables;

                //set arguments 
                editCommand.ScriptEngineContext.Arguments = _scriptArguments;

                //set elements
                editCommand.ScriptEngineContext.Elements = _scriptElements;

                editCommand.ScriptEngineContext.ProjectPath = ScriptProjectPath;

                if (currentCommand.CommandName == "SeleniumElementActionCommand")
                    editCommand.HTMLElementRecorderURL = HTMLElementRecorderURL;

                //show edit command form and save changes on OK result
                if (editCommand.ShowDialog() == DialogResult.OK)
                {
                    CreateUndoSnapshot();
                    selectedCommandItem.Tag = editCommand.SelectedCommand;
                    selectedCommandItem.Text = editCommand.SelectedCommand.GetDisplayValue();
                    selectedCommandItem.SubItems.Add(editCommand.SelectedCommand.GetDisplayValue());

                    _scriptVariables = editCommand.ScriptEngineContext.Variables;
                    _scriptArguments = editCommand.ScriptEngineContext.Arguments;
                    dgvVariables.DataSource = new BindingList<ScriptVariable>(_scriptVariables);
                    dgvArguments.DataSource = new BindingList<ScriptArgument>(_scriptArguments);
                }

                if (editCommand.SelectedCommand.CommandName == "SeleniumElementActionCommand")
                {
                    CreateUndoSnapshot();
                    _scriptElements = editCommand.ScriptEngineContext.Elements;
                    HTMLElementRecorderURL = editCommand.HTMLElementRecorderURL;
                }
            }
        }

        private void ApplyEditorFormat(string formText)
        {
            _editMode = true;
            _isSequence = true;
            Text = formText;
            _selectedTabScriptActions.Invalidate();
            pnlCommandHelper.Hide();
            grpSaveClose.Location = new Point(5, grpFileActions.Location.Y - 10);
            uiBtnRestart.Hide();
            uiBtnRenameSequence.Show();
            uiBtnSaveSequence.Show();
            grpSaveClose.Show();
            grpSaveClose.Text = string.Empty;
            grpRecordRun.Hide();
            grpFileActions.Hide();
            grpVariable.Hide();           
            grpSearch.Hide();
            moveToParentToolStripMenuItem.Visible = true;
            uiPaneTabs.TabPages.Remove(tpProject);    
        }

        private void CutRows()
        {
            CreateUndoSnapshot();
            //initialize list of items to copy
            if (_rowsSelectedForCopy == null)
            {
                _rowsSelectedForCopy = new List<ListViewItem>();
            }
            else
            {
                _rowsSelectedForCopy.Clear();
            }

            //copy into list for all selected
            if (_selectedTabScriptActions.SelectedItems.Count >= 1)
            {
                foreach (ListViewItem item in _selectedTabScriptActions.SelectedItems)
                {
                    _rowsSelectedForCopy.Add(item);
                    _selectedTabScriptActions.Items.Remove(item);
                }

                Notify(_rowsSelectedForCopy.Count + " item(s) cut to clipboard!", Color.White);
            }
        }

        private void CopyRows()
        {

            //initialize list of items to copy
            if (_rowsSelectedForCopy == null)
            {
                _rowsSelectedForCopy = new List<ListViewItem>();
            }
            else
            {
                _rowsSelectedForCopy.Clear();
            }

            //copy into list for all selected
            if (_selectedTabScriptActions.SelectedItems.Count >= 1)
            {
                foreach (ListViewItem item in _selectedTabScriptActions.SelectedItems)
                {
                    _rowsSelectedForCopy.Add(item);
                }

                Notify(_rowsSelectedForCopy.Count + " item(s) copied to clipboard!", Color.White);
            }
        }

        private void PasteRows()
        {
            CreateUndoSnapshot();

            if (_rowsSelectedForCopy != null)
            {

                if (_selectedTabScriptActions.SelectedItems.Count == 0)
                {
                    MessageBox.Show("In order to paste, you must first select a command to paste under.",
                        "Select Command To Paste Under");
                    return;
                }

                int destinationIndex = _selectedTabScriptActions.SelectedItems[0].Index + 1;

                foreach (ListViewItem item in _rowsSelectedForCopy)
                {
                    dynamic duplicatedCommand = Common.Clone(item.Tag);
                    duplicatedCommand.GenerateID();
                    _selectedTabScriptActions.Items.Insert(destinationIndex, CreateScriptCommandListViewItem(duplicatedCommand));
                    destinationIndex += 1;
                }

                _selectedTabScriptActions.Invalidate();
                Notify(_rowsSelectedForCopy.Count + " item(s) pasted!", Color.White);
            }
        }

        private void UndoChange()
        {
            if (((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList.Count > 0)
            {
                CreateRedoSnapshot();
                _selectedTabScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in ((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList.Last())
                {
                    _selectedTabScriptActions.Items.Add(rowItem);
                }

                ((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList
                    .RemoveAt(((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList.Count - 1);

                _selectedTabScriptActions.Invalidate();
            }
        }

        private void RedoChange()
        {
            if (((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList.Count > 0)
            {
                CreateUndoSnapshot();
                _selectedTabScriptActions.Items.Clear();

                foreach (ListViewItem rowItem in ((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList.Last())
                {
                    _selectedTabScriptActions.Items.Add(rowItem);
                }

                ((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList
                    .RemoveAt(((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList.Count - 1);

                _selectedTabScriptActions.Invalidate();
            }
        }

        private void CreateUndoSnapshot()
        {
            if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                uiScriptTabControl.SelectedTab.Text += " *";

            List<ListViewItem> itemList = new List<ListViewItem>();
            foreach (ListViewItem rowItem in _selectedTabScriptActions.Items)
            {
                ListViewItem copiedRowItem = (ListViewItem)rowItem.Clone();
                itemList.Add(copiedRowItem);
            }

            ((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList.Add(itemList);

            if (((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList.Count > 10)
                ((ScriptActionTag)_selectedTabScriptActions.Tag).UndoList.RemoveAt(0);
        }

        private void CreateRedoSnapshot()
        {
            if (!uiScriptTabControl.SelectedTab.Text.Contains(" *"))
                uiScriptTabControl.SelectedTab.Text += " *";

            List<ListViewItem> itemList = new List<ListViewItem>();
            foreach (ListViewItem rowItem in _selectedTabScriptActions.Items)
            {
                ListViewItem copiedRowItem = (ListViewItem)rowItem.Clone();
                itemList.Add(copiedRowItem);
            }

            ((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList.Add(itemList);

            if (((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList.Count > 10)
                ((ScriptActionTag)_selectedTabScriptActions.Tag).RedoList.RemoveAt(0);
        }
        #endregion

        #region ListView Create Item
        private ListViewItem CreateScriptCommandListViewItem(ScriptCommand cmdDetails)
        {
            ListViewItem newCommand = new ListViewItem();
            newCommand.Text = cmdDetails.GetDisplayValue();
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            newCommand.SubItems.Add(cmdDetails.GetDisplayValue());
            //cmdDetails.RenderedControls = null;
            newCommand.Tag = cmdDetails;
            newCommand.ForeColor = Color.SteelBlue;
            newCommand.BackColor = Color.DimGray;
            if (_uiImages != null)
                newCommand.ImageIndex = _uiImages.Images.IndexOfKey(cmdDetails.GetType().Name);
            newCommand.ToolTipText = cmdDetails.GetDisplayValue();

            return newCommand;
        }
        #endregion

        #region ListView Comment, Coloring, ToolStrip
        private void IndentListViewItems()
        {
            int indent = 0;
            ScriptCommand cmd;
            foreach (ListViewItem rowItem in _selectedTabScriptActions.Items)
            {
                if (rowItem is null)
                {
                    continue;
                }
                cmd = ((ScriptCommand)(rowItem.Tag));
                if ((cmd.CommandName == "BeginIfCommand") || (cmd.CommandName == "BeginMultiIfCommand") ||
                    (cmd.CommandName == "LoopCollectionCommand") || (cmd.CommandName == "LoopContinuouslyCommand") ||
                    (cmd.CommandName == "LoopNumberOfTimesCommand") || (cmd.CommandName == "BeginTryCommand") ||
                    (cmd.CommandName == "BeginLoopCommand") || (cmd.CommandName == "BeginMultiLoopCommand") ||
                    (cmd.CommandName == "BeginRetryCommand"))
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 2;
                }
                else if (cmd.CommandName == "BeginSwitchCommand")
                {
                    indent += 2;
                    rowItem.IndentCount = indent;
                    indent += 4;
                }
                else if ((cmd.CommandName == "EndLoopCommand") || (cmd.CommandName == "EndIfCommand") ||
                    (cmd.CommandName == "EndTryCommand") || (cmd.CommandName == "EndRetryCommand"))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if (cmd.CommandName == "EndSwitchCommand")
                {
                    indent -= 4;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent -= 2;
                    if (indent < 0) indent = 0;
                }
                else if ((cmd.CommandName == "ElseCommand") || (cmd.CommandName == "CatchCommand") ||
                    (cmd.CommandName == "FinallyCommand") || (cmd.CommandName == "CaseCommand"))
                {
                    indent -= 2;
                    if (indent < 0) indent = 0;
                    rowItem.IndentCount = indent;
                    indent += 2;
                    if (indent < 0) indent = 0;
                }
                else
                {
                    rowItem.IndentCount = indent;
                }

            }
        }

        private void AutoSizeLineNumberColumn()
        {
            //auto adjust column width based on # of commands
            int columnWidth = (20 * _selectedTabScriptActions.Items.Count.ToString().Length);
            _selectedTabScriptActions.Columns[0].Width = columnWidth;
        }

        private void lstScriptActions_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            //handle indents
            IndentListViewItems();

            //auto size line numbers based on command count
            AutoSizeLineNumberColumn();

            //get listviewitem
            ListViewItem item = e.Item;

            //get script command reference
            var command = (ScriptCommand)item.Tag;

            //create modified bounds
            var modifiedBounds = e.Bounds;
            //modifiedBounds.Y += 2;

            //switch between column index
            switch (e.ColumnIndex)
            {
                case 0:
                    //draw row number
                    e.Graphics.DrawString((e.ItemIndex + 1).ToString(),
                        _selectedTabScriptActions.Font, Brushes.LightSlateGray, modifiedBounds);
                    break;
                case 1:
                    try
                    {
                        if (command.PauseBeforeExecution)
                        {
                            var breakPointImg = new Bitmap(CoreResources.command_breakpoint, new Size(18, 18));
                            e.Graphics.DrawImage(breakPointImg, modifiedBounds.Left, modifiedBounds.Top + 3);
                        }
                        else if (command.IsCommented)
                        {
                            var commentedImg = new Bitmap(CoreResources.command_disabled, new Size(18, 18));
                            e.Graphics.DrawImage(commentedImg, modifiedBounds.Left, modifiedBounds.Top + 3);
                        }
                        else
                        {
                            //draw command icon
                            var img = _uiImages.Images[command.GetType().Name];
                            if (img != null)
                                e.Graphics.DrawImage(img, modifiedBounds.Left, modifiedBounds.Top + 3);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        //icon draw failure
                        Console.WriteLine(ex);
                    }
                    break;
                case 2:
                    //write command text
                    Brush commandNameBrush, commandBackgroundBrush;
                    if ((_debugLine > 0) && (e.ItemIndex == _debugLine - 1) && !command.PauseBeforeExecution && !IsUnhandledException)
                    {
                        //debugging coloring
                        commandNameBrush = Brushes.White;
                        commandBackgroundBrush = Brushes.LimeGreen;
                        IsScriptPaused = false;

                        if (!IsScriptSteppedOver && !IsScriptSteppedInto)
                        {
                            RemoveDebugTab();
                        }
                    }
                    else if((_debugLine > 0) && (e.ItemIndex == _debugLine - 1) && IsUnhandledException)
                    {
                        commandNameBrush = Brushes.Red;
                        commandBackgroundBrush = Brushes.Black;

                        if (!IsScriptPaused && _isDebugMode)
                        {
                            CreateDebugTab();
                            IsScriptPaused = true;
                        }
                        
                    }
                    else if ((_debugLine > 0) && (e.ItemIndex == _debugLine - 1) && command.PauseBeforeExecution && !IsUnhandledException)
                    {
                        commandNameBrush = Brushes.White;
                        commandBackgroundBrush = Brushes.Red;

                        if (!IsScriptPaused && _isDebugMode)
                        {                           
                            CreateDebugTab();
                            IsScriptPaused = true;
                        }
                    }
                    else if ((_currentIndex >= 0) && (e.ItemIndex == _currentIndex))
                    {
                        //search primary item coloring
                        commandNameBrush = Brushes.Black;
                        commandBackgroundBrush = Brushes.Goldenrod;
                    }
                    else if (_matchingSearchIndex.Contains(e.ItemIndex))
                    {
                        //search match item coloring
                        commandNameBrush = Brushes.Black;
                        commandBackgroundBrush = Brushes.LightYellow;
                    }
                    else if ((e.Item.Focused) || (e.Item.Selected))
                    {
                        //selected item coloring
                        commandNameBrush = Brushes.White;
                        commandBackgroundBrush = Brushes.DodgerBlue;
                    }
                    else if (command.PauseBeforeExecution)
                    {
                        //pause before execution coloring
                        commandNameBrush = Brushes.Red;
                        commandBackgroundBrush = Brushes.MistyRose;
                    }
                    else if ((command.CommandName == "AddCodeCommentCommand") || (command.IsCommented))
                    {
                        //comments and commented command coloring
                        commandNameBrush = Brushes.MediumSeaGreen;
                        commandBackgroundBrush = Brushes.Honeydew;
                    }
                    else if ((command.CommandName == "BrokenCodeCommentCommand"))
                    {
                        //comments and commented command coloring
                        commandNameBrush = Brushes.Black;
                        commandBackgroundBrush = Brushes.DarkGray;
                    }
                    else
                    {
                        //standard coloring
                        commandNameBrush = Brushes.SteelBlue;
                        commandBackgroundBrush = Brushes.WhiteSmoke;
                    }

                    //fill with background color
                    e.Graphics.FillRectangle(commandBackgroundBrush, modifiedBounds);

                    //get indent count
                    var indentPixels = (item.IndentCount * 15);

                    //set indented X position
                    modifiedBounds.X += indentPixels;

                    //draw string
                    e.Graphics.DrawString(command.GetDisplayValue(), _selectedTabScriptActions.Font,
                                          commandNameBrush, modifiedBounds);
                    break;  
            }
        }

        private void lstScriptActions_MouseMove(object sender, MouseEventArgs e)
        {
            _selectedTabScriptActions.Invalidate();
        }

        private void lstScriptActions_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (_selectedTabScriptActions.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    cmsScriptActions.Show(Cursor.Position);
                }
            }
        }

        private void lstScriptActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_appSettings.ClientSettings.InsertCommandsInline)
                return;

            //check to see if an item has been selected last
            if (_selectedTabScriptActions.SelectedItems.Count > 0)
            {
                _selectedIndex = _selectedTabScriptActions.SelectedItems[0].Index;
                //FormatCommandListView();
            }
            else
            {
                //nothing is selected
                _selectedIndex = -1;
            }
        }

        private void DeleteSelectedCode()
        {
            SelectAllScopedCode();

            foreach (ListViewItem item in _selectedTabScriptActions.SelectedItems)
                _selectedTabScriptActions.Items.Remove(item);

            _selectedTabScriptActions.Invalidate();
        }

        private void SelectAllScopedCode()
        {
            foreach (ListViewItem item in _selectedTabScriptActions.SelectedItems)
            {
                switch (((ScriptCommand)item.Tag).CommandName)
                {
                    case "LoopCollectionCommand":
                    case "LoopContinuouslyCommand":
                    case "LoopNumberOfTimesCommand":
                    case "BeginLoopCommand":
                    case "BeginMultiLoopCommand":
                        FindEndCommand(item, "EndLoopCommand");
                        break;
                    case "BeginIfCommand":
                    case "BeginMultiIfCommand":
                        FindEndCommand(item, "EndIfCommand");
                        break;
                    case "BeginTryCommand":
                        FindEndCommand(item, "EndTryCommand");
                        break;
                    case "BeginRetryCommand":
                        FindEndCommand(item, "EndRetryCommand");
                        break;
                    case "BeginSwitchCommand":
                        FindEndCommand(item, "EndSwitchCommand");
                        break;
                    default:
                        break;
                }
            }
        }

        private void FindEndCommand(ListViewItem item, string endCommandName)
        {
            for (int itemIndex = item.Index; itemIndex < _selectedTabScriptActions.Items.Count; itemIndex++)
            {
                _selectedTabScriptActions.Items[itemIndex].Selected = true;
                if (((ScriptCommand)_selectedTabScriptActions.Items[itemIndex].Tag).CommandName == endCommandName &&
                    _selectedTabScriptActions.Items[itemIndex].IndentCount == item.IndentCount)
                    break;
            }
        }

        private void SetSelectedCodeToCommented(bool setCommented)
        {
            SelectAllScopedCode();

            //warn if nothing was selected
            if (_selectedTabScriptActions.SelectedItems.Count == 0)
                Notify("No code was selected!", Color.Yellow);
            else
                CreateUndoSnapshot();

            //get each item and set appropriately
            foreach (ListViewItem item in _selectedTabScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.IsCommented = setCommented;
            }

            //recolor
            _selectedTabScriptActions.Invalidate();
        }

        private void AddRemoveBreakpoint()
        {
            //warn if nothing was selected
            if (_selectedTabScriptActions.SelectedItems.Count == 0)
                Notify("No code was selected!", Color.Yellow);
            else
                CreateUndoSnapshot();

            //get each item and set appropriately
            foreach (ListViewItem item in _selectedTabScriptActions.SelectedItems)
            {
                var selectedCommand = (ScriptCommand)item.Tag;
                selectedCommand.PauseBeforeExecution = !selectedCommand.PauseBeforeExecution;
            }

            //recolor
            _selectedTabScriptActions.Invalidate();
        }

        private void disableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(true);
        }

        private void enableSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetSelectedCodeToCommented(false);
        }

        private void addRemoveBreakpointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddRemoveBreakpoint();
        }

        private void cutSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CutRows();
        }

        private void copySelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CopyRows();
        }

        private void pasteSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PasteRows();
        }

        private void deleteSelectedCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedCode();
        }

        private void viewCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var currentCommand = _selectedTabScriptActions.SelectedItems[0].Tag;
            var jsonText = JsonConvert.SerializeObject(currentCommand, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
            var dialog = new frmDialog(jsonText, "Command Code", DialogType.OkOnly, 0);
            dialog.ShowDialog();
        }

        private void moveToParentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create command list
            var commandList = new List<ScriptCommand>();

            //loop each
            for (int i = _selectedTabScriptActions.SelectedItems.Count - 1; i >= 0; i--)
            {
                //add to list and remove existing
                commandList.Add((ScriptCommand)_selectedTabScriptActions.SelectedItems[i].Tag);
                _selectedTabScriptActions.Items.Remove(_selectedTabScriptActions.SelectedItems[i]);
            }

            //reverse commands only if not inserting inline
            if (!_appSettings.ClientSettings.InsertCommandsInline)
            {
                commandList.Reverse();
            }

            //add to parent
            commandList.ForEach(x => _parentBuilder.AddCommandToListView(x));
        }

        public void AddCommandToListView(ScriptCommand selectedCommand, int index = -1)
        {
            if (pnlCommandHelper.Visible)
            {
                uiScriptTabControl.SelectedTab.Controls.Remove(pnlCommandHelper);
                uiScriptTabControl.SelectedTab.Controls[0].Show();
            }
            else if(!uiScriptTabControl.SelectedTab.Controls[0].Visible)
                uiScriptTabControl.SelectedTab.Controls[0].Show();

            ListViewItem command;

            //valid command verification for drag/dropped commands
            if (selectedCommand != null)
                command = CreateScriptCommandListViewItem(selectedCommand);
            else
                return;

            int insertionIndex;

            if (index == -1)
            {
                //insert to end by default
                insertionIndex = _selectedTabScriptActions.Items.Count;

                //verify setting to insert inline is selected and if an item is currently selected
                if ((_appSettings.ClientSettings.InsertCommandsInline) && (_selectedTabScriptActions.SelectedItems.Count > 0))
                {
                    //insert inline
                    insertionIndex = _selectedTabScriptActions.SelectedItems[0].Index + 1;
                }
            }
            else
                insertionIndex = index;           

            //insert command
            _selectedTabScriptActions.Items.Insert(insertionIndex, command);
            ClearSelectedListViewItems();
            command.Selected = true;

            //special types also get a following command and comment
            if ((selectedCommand.CommandName == "LoopCollectionCommand") || (selectedCommand.CommandName == "LoopContinuouslyCommand") ||
                (selectedCommand.CommandName == "LoopNumberOfTimesCommand") || (selectedCommand.CommandName == "BeginLoopCommand") ||
                (selectedCommand.CommandName == "BeginMultiLoopCommand"))
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will run within the loop";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));

                dynamic endLoopCommand = TypeMethods.CreateTypeInstance(AContainer, "EndLoopCommand");
                _selectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(endLoopCommand));
            }
            else if ((selectedCommand.CommandName == "BeginIfCommand") || (selectedCommand.CommandName == "BeginMultiIfCommand"))
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will run if the statement is true";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));
                
                dynamic endIfCommand = TypeMethods.CreateTypeInstance(AContainer, "EndIfCommand");
                _selectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(endIfCommand));
            }
            else if (selectedCommand.CommandName == "BeginTryCommand")
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will be handled if error occurs";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));

                dynamic catchCommand = TypeMethods.CreateTypeInstance(AContainer, "CatchCommand");
                _selectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(catchCommand));

                dynamic codeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                codeCommentCommand.v_Comment = "This section executes if error occurs above";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(codeCommentCommand));

                dynamic endTryCommand = TypeMethods.CreateTypeInstance(AContainer, "EndTryCommand");
                _selectedTabScriptActions.Items.Insert(insertionIndex + 4, CreateScriptCommandListViewItem(endTryCommand));
            }
            else if (selectedCommand.CommandName == "BeginRetryCommand")
            {
                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will be retried as long as the condition is not met or an error is thrown";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(addCodeCommentCommand));
                
                dynamic endRetryCommand = TypeMethods.CreateTypeInstance(AContainer, "EndRetryCommand");
                _selectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(endRetryCommand));
            }
            else if (selectedCommand.CommandName == "BeginSwitchCommand")
            {
                dynamic caseCommand = TypeMethods.CreateTypeInstance(AContainer, "CaseCommand");
                caseCommand.v_CaseValue = "Default";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 1, CreateScriptCommandListViewItem(caseCommand));

                dynamic addCodeCommentCommand = TypeMethods.CreateTypeInstance(AContainer, "AddCodeCommentCommand");
                addCodeCommentCommand.v_Comment = "Items in this section will run if no case statements match";
                _selectedTabScriptActions.Items.Insert(insertionIndex + 2, CreateScriptCommandListViewItem(addCodeCommentCommand));
                
                dynamic endSwitchCommand = TypeMethods.CreateTypeInstance(AContainer, "EndSwitchCommand");
                _selectedTabScriptActions.Items.Insert(insertionIndex + 3, CreateScriptCommandListViewItem(endSwitchCommand));
            }

            CreateUndoSnapshot();
            _selectedTabScriptActions.Invalidate();
            AutoSizeLineNumberColumn();
        }

        private void pnlStatus_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawString(_notificationText, pnlStatus.Font, new SolidBrush(_notificationColor), 30, 4);
            e.Graphics.DrawImage(Resources.OpenBots_icon, 5, 3, 20, 20);
            _notificationPaintedText = _notificationText;
        }

        private void pnlStatus_DoubleClick(object sender, EventArgs e)
        {
            MessageBox.Show(_notificationPaintedText);
        }
        #endregion

        #region ListView Search
        private void txtScriptSearch_TextChanged(object sender, EventArgs e)
        {
            if (_selectedTabScriptActions.Items.Count == 0)
                return;

            _reqdIndex = 0;

            if (txtScriptSearch.Text == "")
            {
                //hide info
                HideSearchInfo();

                //clear indexes
                _matchingSearchIndex.Clear();
                _currentIndex = -1;

                //repaint
                _selectedTabScriptActions.Invalidate();
            }
            else
            {
                lblCurrentlyViewing.Show();
                lblTotalResults.Show();
                SearchForItemInListView();

                //repaint
                _selectedTabScriptActions.Invalidate();
            }
        }

        private void HideSearchInfo()
        {
            lblCurrentlyViewing.Hide();
            lblTotalResults.Hide();
        }

        private void pbSearch_Click(object sender, EventArgs e)
        {
            if (txtScriptSearch.Text != "" || tsSearchBox.Text != "")
            {
                _reqdIndex++;
                SearchForItemInListView();
            }
        }

        private void SearchForItemInListView()
        {
            var searchCriteria = txtScriptSearch.Text;

            if (searchCriteria == "")
            {
                searchCriteria = tsSearchBox.Text;
            }

            var matchingItems = _selectedTabScriptActions.Items.OfType<ListViewItem>()
                                                               .Where(x => x.Text.Contains(searchCriteria))
                                                               .ToList();

            int? matchCount = matchingItems.Count();
            int totalMatches = matchCount ?? 0;

            if ((_reqdIndex == matchingItems.Count) || (_reqdIndex < 0))
            {
                _reqdIndex = 0;
            }

            lblTotalResults.Show();

            if (totalMatches == 0)
            {
                _reqdIndex = -1;
                lblTotalResults.Text = "No Matches Found";
                lblCurrentlyViewing.Hide();
                //clear indexes
                _matchingSearchIndex.Clear();
                _reqdIndex = -1;
                _selectedTabScriptActions.Invalidate();
                return;
            }
            else
            {
                lblCurrentlyViewing.Text = "Viewing " + (_reqdIndex + 1) + " of " + totalMatches + "";
                tsSearchResult.Text = "Viewing " + (_reqdIndex + 1) + " of " + totalMatches + "";
                lblTotalResults.Text = totalMatches + " total results found";
            }

            _matchingSearchIndex = new List<int>();
            foreach (ListViewItem itm in matchingItems)
            {
                _matchingSearchIndex.Add(itm.Index);
                itm.BackColor = Color.LightGoldenrodYellow;
            }

            _currentIndex = matchingItems[_reqdIndex].Index;

            _selectedTabScriptActions.Invalidate();
            _selectedTabScriptActions.EnsureVisible(_currentIndex);
        }

        private void pbSearch_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void pbSearch_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
        #endregion
        #endregion
    }
}
