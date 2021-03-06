﻿using Microsoft.VisualBasic;
using Newtonsoft.Json;
using OpenBots.Core.Command;
using OpenBots.Nuget;
using OpenBots.Core.Project;
using OpenBots.Core.Script;
using OpenBots.Studio.Utilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VBFileSystem = Microsoft.VisualBasic.FileIO.FileSystem;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Enums;
using OpenBots.Core.IO;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form
    {

        #region Project Tool Strip, Buttons and Pane
        private void addProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddProject();
        }

        private void uiBtnProject_Click(object sender, EventArgs e)
        {
            addProjectToolStripMenuItem_Click(sender, e);
        }

        public DialogResult AddProject()
        {
            tvProject.Nodes.Clear();
            var projectBuilder = new frmProjectBuilder();
            projectBuilder.ShowDialog();

            //close OpenBots if add project form is closed at startup
            if (projectBuilder.DialogResult == DialogResult.Cancel && ScriptProject == null)
            {
                Application.Exit();
                return DialogResult.Abort;
            }

            //create new OpenBots project
            else if (projectBuilder.Action == frmProjectBuilder.ProjectAction.CreateProject)
            {
                DialogResult result = CheckForUnsavedScripts();
                if (result == DialogResult.Cancel)
                    return DialogResult.Cancel;

                uiScriptTabControl.TabPages.Clear();
                ScriptProjectPath = projectBuilder.NewProjectPath;

                //create new project
                ScriptProject = new Project(projectBuilder.NewProjectName);
                string configPath = Path.Combine(ScriptProjectPath, "project.config");

                //create config file
                File.WriteAllText(configPath, JsonConvert.SerializeObject(ScriptProject));

                NotifySync("Loading package assemblies...", Color.White);               

                var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
                _builder = AppDomainSetupManager.LoadBuilder(assemblyList);
                AContainer = _builder.Build();                
                        
                string mainScriptPath = Path.Combine(ScriptProjectPath, "Main.json");
                string mainScriptName = Path.GetFileNameWithoutExtension(mainScriptPath);
                UIListView mainScriptActions = NewLstScriptActions(mainScriptName);

                List<ScriptVariable> mainScriptVariables = new List<ScriptVariable>();
                List<ScriptArgument> mainScriptArguments = new List<ScriptArgument>();
                List<ScriptElement> mainScriptElements = new List<ScriptElement>();

                try
                {
                    dynamic helloWorldCommand = TypeMethods.CreateTypeInstance(AContainer, "ShowMessageCommand");
                    helloWorldCommand.v_Message = "Hello World";
                    mainScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));
                }
                catch (Exception)
                {
                    var brokenHelloWorldCommand = new BrokenCodeCommentCommand();
                    brokenHelloWorldCommand.v_Comment = "Hello World";
                    mainScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(brokenHelloWorldCommand));
                }
                
                //begin saving as main.xml
                ClearSelectedListViewItems();

                try
                {
                    //serialize main script
                    EngineContext engineContext = new EngineContext
                    {
                        Variables = mainScriptVariables,
                        Arguments = mainScriptArguments,
                        Elements = mainScriptElements,
                        FilePath = mainScriptPath,
                        Container = AContainer
                    };

                    var mainScript = Script.SerializeScript(mainScriptActions.Items, engineContext);
                    
                    _mainFileName = ScriptProject.Main;
                   
                    OpenFile(mainScriptPath);
                    ScriptFilePath = mainScriptPath;

                    //show success dialog
                    Notify("Project has been created successfully!", Color.White);
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message, Color.Red);
                }
            }

            //open existing OpenBots project
            else if (projectBuilder.Action == frmProjectBuilder.ProjectAction.OpenProject)
            {
                DialogResult result = CheckForUnsavedScripts();
                if (result == DialogResult.Cancel)
                    return DialogResult.Cancel;

                try
                {
                    //open project
                    Project project = Project.OpenProject(projectBuilder.ExistingConfigPath);
                    string mainFileName = project.Main;

                    string mainFilePath = Directory.GetFiles(projectBuilder.ExistingProjectPath, mainFileName, SearchOption.AllDirectories).FirstOrDefault();
                    if (mainFilePath == null)
                        throw new Exception("Main script not found");

                    NotifySync("Loading package assemblies...", Color.White);

                    var assemblyList = NugetPackageManager.LoadPackageAssemblies(projectBuilder.ExistingConfigPath);
                    _builder = AppDomainSetupManager.LoadBuilder(assemblyList);
                    AContainer = _builder.Build();

                    ScriptProject = project;
                    _mainFileName = mainFileName;                 
                    ScriptProjectPath = projectBuilder.ExistingProjectPath;
                    uiScriptTabControl.TabPages.Clear();

                    //open Main
                    OpenFile(mainFilePath);

                    //show success dialog
                    Notify("Project has been opened successfully!", Color.White);
                }
                catch (Exception ex)
                {
                    projectBuilder.Dispose();

                    //show fail dialog
                    Notify("An Error Occured: " + ex.Message, Color.Red);

                    //try adding project again
                    AddProject();                    
                    return DialogResult.None;
                }
            }

            projectBuilder.Dispose();

            DirectoryInfo projectDirectoryInfo = new DirectoryInfo(ScriptProjectPath);
            TreeNode projectNode = new TreeNode(projectDirectoryInfo.Name);
            projectNode.Text = projectDirectoryInfo.Name;
            projectNode.Tag = projectDirectoryInfo.FullName;
            projectNode.Nodes.Add("Empty");
            projectNode.ContextMenuStrip = cmsProjectMainFolderActions;          
            tvProject.Nodes.Add(projectNode);
            projectNode.Expand();
            LoadCommands(this);

            //save to recent projects 
            if (_appSettings.ClientSettings.RecentProjects == null)
                _appSettings.ClientSettings.RecentProjects = new List<string>();

            if (_appSettings.ClientSettings.RecentProjects.Contains(ScriptProjectPath))
                _appSettings.ClientSettings.RecentProjects.Remove(ScriptProjectPath);

            _appSettings.ClientSettings.RecentProjects.Insert(0, ScriptProjectPath);

            if (_appSettings.ClientSettings.RecentProjects.Count > 10)
                _appSettings.ClientSettings.RecentProjects.RemoveAt(10);

            _appSettings.Save(_appSettings);

            return DialogResult.OK;
        }

        private void OpenProject(string projectPath)
        {
            tvProject.Nodes.Clear();

            DialogResult result = CheckForUnsavedScripts();
            if (result == DialogResult.Cancel)
                return;

            try
            {
                string configPath = Path.Combine(projectPath, "project.config");

                //open project
                Project project = Project.OpenProject(configPath);
                string mainFileName = project.Main;

                string mainFilePath = Directory.GetFiles(projectPath, mainFileName, SearchOption.AllDirectories).FirstOrDefault();
                if (mainFilePath == null)
                    throw new Exception("Main script not found");

                var assemblyList = NugetPackageManager.LoadPackageAssemblies(configPath);
                _builder = AppDomainSetupManager.LoadBuilder(assemblyList);
                AContainer = _builder.Build();

                _mainFileName = mainFileName;
                ScriptProject = project;
                ScriptProjectPath = projectPath;
                uiScriptTabControl.TabPages.Clear();

                //open Main
                OpenFile(mainFilePath);

                //show success dialog
                Notify("Project has been opened successfully!", Color.White);
            }
            catch (Exception ex)
            {
                //show fail dialog
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        

            DirectoryInfo projectDirectoryInfo = new DirectoryInfo(ScriptProjectPath);
            TreeNode projectNode = new TreeNode(projectDirectoryInfo.Name);
            projectNode.Text = projectDirectoryInfo.Name;
            projectNode.Tag = projectDirectoryInfo.FullName;
            projectNode.Nodes.Add("Empty");
            projectNode.ContextMenuStrip = cmsProjectMainFolderActions;          
            tvProject.Nodes.Add(projectNode);
            projectNode.Expand();
            LoadCommands(this);

            //save to recent projects 
            if (_appSettings.ClientSettings.RecentProjects == null)
                _appSettings.ClientSettings.RecentProjects = new List<string>();

            if (_appSettings.ClientSettings.RecentProjects.Contains(ScriptProjectPath))
                _appSettings.ClientSettings.RecentProjects.Remove(ScriptProjectPath);

            _appSettings.ClientSettings.RecentProjects.Insert(0, ScriptProjectPath);

            if (_appSettings.ClientSettings.RecentProjects.Count > 10)
                _appSettings.ClientSettings.RecentProjects.RemoveAt(10);

            _appSettings.Save(_appSettings);
        }

        private void LoadChildren(TreeNode parentNode, string directory)
        {
            DirectoryInfo parentDirectoryInfo = new DirectoryInfo(directory);
            try
            {
                foreach (DirectoryInfo childDirectoryInfo in parentDirectoryInfo.GetDirectories())
                {
                    if (childDirectoryInfo.Attributes != FileAttributes.Hidden)
                        NewNode(parentNode, childDirectoryInfo.FullName, "folder");
                }
                foreach (FileInfo fileInfo in parentDirectoryInfo.GetFiles())
                {
                    if (fileInfo.Attributes != FileAttributes.Hidden)
                        NewNode(parentNode, fileInfo.FullName, "file");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void NewNode(TreeNode parentNode, string childPath, string type)
        {
            if (type == "folder")
            {
                DirectoryInfo childDirectoryInfo = new DirectoryInfo(childPath);

                TreeNode innerFolderNode = new TreeNode(childDirectoryInfo.Name);
                innerFolderNode.Name = childDirectoryInfo.Name;
                innerFolderNode.Text = childDirectoryInfo.Name;
                innerFolderNode.Tag = childDirectoryInfo.FullName;
                innerFolderNode.Nodes.Add("Empty");
                innerFolderNode.ContextMenuStrip = cmsProjectFolderActions;
                innerFolderNode.ImageIndex = 0; //folder icon
                innerFolderNode.SelectedImageIndex = 0;
                parentNode.Nodes.Add(innerFolderNode);
            }
            else if (type == "file")
            {
                FileInfo childFileInfo = new FileInfo(childPath);

                TreeNode fileNode = new TreeNode(childFileInfo.Name);
                fileNode.Name = childFileInfo.Name;
                fileNode.Text = childFileInfo.Name;
                fileNode.Tag = childFileInfo.FullName;
                
                if (fileNode.Name != "project.config")
                    fileNode.ContextMenuStrip = cmsProjectFileActions;

                if (fileNode.Tag.ToString().ToLower().Contains(".json"))
                {
                    fileNode.ImageIndex = 1; //script file icon
                    fileNode.SelectedImageIndex = 1;
                }
                else if (fileNode.Tag.ToString().ToLower().Contains(".xlsx") ||
                         fileNode.Tag.ToString().ToLower().Contains(".csv"))
                {
                    fileNode.ImageIndex = 3; //excel file icon
                    fileNode.SelectedImageIndex = 3;
                }
                else if (fileNode.Tag.ToString().ToLower().Contains(".docx"))
                {
                    fileNode.ImageIndex = 4; //word file icon
                    fileNode.SelectedImageIndex = 4;
                }
                else if (fileNode.Tag.ToString().ToLower().Contains(".pdf"))
                {
                    fileNode.ImageIndex = 5; //pdf file icon
                    fileNode.SelectedImageIndex = 5;
                }
                else
                {
                    fileNode.ImageIndex = 2; //default file icon
                    fileNode.SelectedImageIndex = 2;
                }

                parentNode.Nodes.Add(fileNode);
            }
        }
        #endregion

        #region Project TreeView Events
        private void tvProject_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (Directory.Exists(e.Node.Tag.ToString()))
            {
                e.Node.Nodes.Clear();
                LoadChildren(e.Node, e.Node.Tag.ToString());
            }
            else
                e.Cancel = true;
        }

        private void tvProject_DoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (IsScriptRunning)
                return;

            if (e == null || e.Button == MouseButtons.Left)
            {
                try
                {
                    string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                    string currentOpenScriptFilePath = _scriptFilePath;

                    if (File.Exists(selectedNodePath) && selectedNodePath.ToLower().Contains(".json"))
                        OpenFile(selectedNodePath);
                    else if (File.Exists(selectedNodePath))
                        Process.Start(selectedNodePath);
                }
                catch (Exception ex)
                {
                    Notify("An Error Occured: " + ex.Message, Color.Red);
                }
            }
        }

        private void tvProject_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            tvProject.SelectedNode = e.Node;
        }

        private void tvProject_KeyDown(object sender, KeyEventArgs e)
        {
            string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
            bool isFolder;
            if (Directory.Exists(selectedNodePath))
                isFolder = true;
            else
                isFolder = false;
            if (e.KeyCode == Keys.Delete && isFolder)
                tsmiDeleteFolder_Click(sender, e);
            else if (e.KeyCode == Keys.Delete && !isFolder)
                tsmiDeleteFile_Click(sender, e);
            else if (e.KeyCode == Keys.Enter && !isFolder)
                tvProject_DoubleClick(sender, null);
            else if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                    tsmiCopyFolder_Click(sender, e);
                if (e.KeyCode == Keys.V)
                    tsmiPasteFolder_Click(sender, e);
            }
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
        #endregion

        #region Project Folder Context Menu Strip
        private void tsmiCopyFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                Clipboard.SetData(DataFormats.Text, selectedNodePath);
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiDeleteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                if (selectedNodeName != ScriptProject.ProjectName)
                {
                    DialogResult result = MessageBox.Show($"Are you sure you would like to delete {selectedNodeName}?",
                                                 $"Delete {selectedNodeName}", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (Directory.Exists(selectedNodePath))
                        {
                            Directory.Delete(selectedNodePath, true);
                            tvProject.Nodes.Remove(tvProject.SelectedNode);
                        }
                        else
                            throw new FileNotFoundException();
                    }
                }
                else
                {
                    throw new Exception($"Cannot delete {selectedNodeName}");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiNewFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string newName = "";
                var newNameForm = new frmInputBox("Enter the name of the new folder", "New Folder");
                newNameForm.ShowDialog();

                if (newNameForm.DialogResult == DialogResult.OK)
                {
                    newName = newNameForm.txtInput.Text;
                    newNameForm.Dispose();
                }
                else if (newNameForm.DialogResult == DialogResult.Cancel)
                {
                    newNameForm.Dispose();
                    return;
                }

                if (newName.EndsWith(".json"))
                    throw new Exception("Invalid folder name");

                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string newFolderPath = Path.Combine(selectedNodePath, newName);

                if (!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);
                    DirectoryInfo newDirectoryInfo = new DirectoryInfo(newFolderPath);
                    NewNode(tvProject.SelectedNode, newFolderPath, "folder");
                }
                else
                {
                    int count = 1;
                    string newerFolderPath = newFolderPath;
                    while (Directory.Exists(newerFolderPath))
                    {
                        newerFolderPath = $"{newFolderPath} ({count})";
                        count += 1;
                    }
                    Directory.CreateDirectory(newerFolderPath);
                    DirectoryInfo newDirectoryInfo = new DirectoryInfo(newerFolderPath);

                    NewNode(tvProject.SelectedNode, newerFolderPath, "folder");
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }        

        private void tsmiPasteFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string copiedNodePath = Clipboard.GetData(DataFormats.Text).ToString();

                if (Directory.Exists(copiedNodePath))
                {
                    DirectoryInfo copiedNodeDirectoryInfo = new DirectoryInfo(copiedNodePath);

                    if (Directory.Exists(Path.Combine(selectedNodePath, copiedNodeDirectoryInfo.Name)))
                        throw new Exception("A directory with this name already exists in this location");

                    else if (copiedNodePath == ScriptProjectPath)
                        throw new Exception("The project directory cannot be copied or moved");

                    else
                    {
                        VBFileSystem.CopyDirectory(copiedNodePath, Path.Combine(selectedNodePath, copiedNodeDirectoryInfo.Name));
                        NewNode(tvProject.SelectedNode, copiedNodePath, "folder");
                    }
                }
                else if (File.Exists(copiedNodePath))
                {
                    FileInfo copiedNodeFileInfo = new FileInfo(copiedNodePath);

                    if (File.Exists(Path.Combine(selectedNodePath, copiedNodeFileInfo.Name)))
                        throw new Exception("A file with this name already exists in this location");

                    else if (copiedNodeFileInfo.Name == "project.config")
                        throw new Exception("This file cannot be copied or moved");

                    else
                    {
                        File.Copy(copiedNodePath, Path.Combine(selectedNodePath, copiedNodeFileInfo.Name));
                        NewNode(tvProject.SelectedNode, copiedNodePath, "file");
                    }
                }
                else
                    throw new Exception("Attempted to paste something that isn't a file or folder");

            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiRenameFolder_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                
                DirectoryInfo selectedNodeDirectoryInfo = new DirectoryInfo(selectedNodePath);

                string newName = "";

                string prompt;
                string title;

                if (selectedNodePath == ScriptProjectPath)
                {
                    prompt = "Enter the new name of the project";
                    title = "Rename Project";
                }
                else
                {
                    prompt = "Enter the new name of the folder";
                    title = "Rename Folder";
                }

                var newNameForm = new frmInputBox(prompt, title);
                newNameForm.txtInput.Text = tvProject.SelectedNode.Name;
                newNameForm.ShowDialog();

                if (newNameForm.DialogResult == DialogResult.OK)
                {
                    newName = newNameForm.txtInput.Text;
                    newNameForm.Dispose();
                }
                else if (newNameForm.DialogResult == DialogResult.Cancel)
                {
                    newNameForm.Dispose();
                    return;
                }

                string newPath = Path.Combine(selectedNodeDirectoryInfo.Parent.FullName, newName);
                bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newName.Contains(c));

                if (isInvalidProjectName)
                    throw new Exception("Illegal characters in path");

                if (Directory.Exists(newPath))
                    throw new Exception("A folder with this name already exists");

                if (CloseAllFiles())
                {
                    FileSystem.Rename(selectedNodePath, newPath);
                    tvProject.SelectedNode.Name = newName;
                    tvProject.SelectedNode.Text = newName;
                    tvProject.SelectedNode.Tag = newPath;
                    tvProject.SelectedNode.Collapse();
                    tvProject.SelectedNode.Expand();

                    if (selectedNodePath == ScriptProjectPath)
                    {
                        ScriptProject.ProjectName = newName;
                        ScriptProjectPath = newPath;
                        Project.RenameProject(ScriptProject, ScriptProjectPath);
                        _appSettings.ClientSettings.RecentProjects.RemoveAt(0);
                        _appSettings.ClientSettings.RecentProjects.Insert(0, ScriptProjectPath);
                        _appSettings.Save(_appSettings);
                    }

                    string mainFilePath = Path.Combine(ScriptProjectPath, ScriptProject.Main);
                    OpenFile(mainFilePath);
                }               
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiNewScriptFile_Click(object sender, EventArgs e)
        {
            try
            {               
                string newName = "";
                var newNameForm = new frmInputBox("Enter the name of the new file without extension", "New File");
                newNameForm.txtInput.Text = tvProject.SelectedNode.Name;
                newNameForm.ShowDialog();

                if (newNameForm.DialogResult == DialogResult.OK)
                {
                    newName = newNameForm.txtInput.Text;
                    newNameForm.Dispose();
                }
                else if (newNameForm.DialogResult == DialogResult.Cancel)
                {
                    newNameForm.Dispose();
                    return;
                }

                if (newName.EndsWith(".json"))
                    throw new Exception("Invalid file name");

                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string newFilePath = Path.Combine(selectedNodePath, newName + ".json");
                UIListView newScriptActions = NewLstScriptActions();
                List<ScriptVariable> newScriptVariables = new List<ScriptVariable>();
                List<ScriptArgument> newScriptArguments = new List<ScriptArgument>();
                List<ScriptElement> newScriptElements = new List<ScriptElement>();

                dynamic helloWorldCommand = TypeMethods.CreateTypeInstance(AContainer, "ShowMessageCommand");
                helloWorldCommand.v_Message = "Hello World";
                newScriptActions.Items.Insert(0, CreateScriptCommandListViewItem(helloWorldCommand));

                EngineContext engineContext = new EngineContext
                {
                    Variables = newScriptVariables,
                    Arguments = newScriptArguments,
                    Elements = newScriptElements,
                    FilePath = newFilePath,
                    Container = AContainer
                };

                if (!File.Exists(newFilePath))
                {
                    Script.SerializeScript(newScriptActions.Items, engineContext);
                    NewNode(tvProject.SelectedNode, newFilePath, "file");
                    OpenFile(newFilePath);
                }
                else
                {
                    int count = 1;
                    string newerFilePath = newFilePath;
                    while (File.Exists(newerFilePath))
                    {
                        string newDirectoryPath = Path.GetDirectoryName(newFilePath);
                        string newFileNameWithoutExtension = Path.GetFileNameWithoutExtension(newFilePath);
                        newerFilePath = Path.Combine(newDirectoryPath, $"{newFileNameWithoutExtension} ({count}).json");
                        count += 1;
                    }

                    engineContext.FilePath = newerFilePath;
                    Script.SerializeScript(newScriptActions.Items, engineContext);
                    NewNode(tvProject.SelectedNode, newerFilePath, "file");
                    OpenFile(newerFilePath);
                }

            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }
        #endregion

        #region Project File Context Menu Strip
        private void tsmiCopyFile_Click(object sender, EventArgs e)
        {
            tsmiCopyFolder_Click(sender, e);
        }

        private void tsmiDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                if (selectedNodeName != "project.config")
                {
                    var result = MessageBox.Show($"Are you sure you would like to delete {selectedNodeName}?",
                                             $"Delete {selectedNodeName}", MessageBoxButtons.YesNo);

                    if (result == DialogResult.Yes)
                    {
                        if (File.Exists(selectedNodePath))
                        {
                            string selectedFileName = Path.GetFileNameWithoutExtension(selectedNodePath);
                            File.Delete(selectedNodePath);
                            tvProject.Nodes.Remove(tvProject.SelectedNode);
                            var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>()
                                                                      .Where(t => t.ToolTipText == selectedNodePath)
                                                                      .FirstOrDefault();
                            if (foundTab != null)
                                uiScriptTabControl.TabPages.Remove(foundTab);
                        }
                        else
                            throw new FileNotFoundException();
                    }
                }
                else
                    throw new Exception($"Cannot delete {selectedNodeName}");
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }

        private void tsmiRenameFile_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedNodePath = tvProject.SelectedNode.Tag.ToString();
                string selectedNodeName = tvProject.SelectedNode.Text.ToString();
                string selectedNodeNameWithoutExtension = Path.GetFileNameWithoutExtension(selectedNodeName);
                string selectedNodeFileExtension = Path.GetExtension(selectedNodePath);

                if (selectedNodeName != "project.config")
                {
                    FileInfo selectedNodeDirectoryInfo = new FileInfo(selectedNodePath);

                    string newNameWithoutExtension = "";
                    var newNameForm = new frmInputBox("Enter the new name of the file without extension", "Rename File");
                    newNameForm.txtInput.Text = Path.GetFileNameWithoutExtension(selectedNodeDirectoryInfo.Name);
                    newNameForm.ShowDialog();

                    if (newNameForm.DialogResult == DialogResult.OK)
                    {
                        newNameWithoutExtension = newNameForm.txtInput.Text;
                        newNameForm.Dispose();
                    }
                    else if (newNameForm.DialogResult == DialogResult.Cancel)
                    {
                        newNameForm.Dispose();
                        return;
                    }

                    string newName = newNameWithoutExtension + selectedNodeFileExtension;
                    string newPath = Path.Combine(selectedNodeDirectoryInfo.DirectoryName, newName);

                    bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newNameWithoutExtension.Contains(c));
                    if (isInvalidProjectName)
                        throw new Exception("Illegal characters in path");

                    if (File.Exists(newPath))
                        throw new Exception("A file with this name already exists");

                    var foundTab = uiScriptTabControl.TabPages.Cast<TabPage>().Where(t => t.ToolTipText == selectedNodePath)
                                                                          .FirstOrDefault();

                    if (foundTab != null)
                    {
                        DialogResult result = CheckForUnsavedScript(foundTab);
                        if (result == DialogResult.Cancel)
                            return;

                        uiScriptTabControl.TabPages.Remove(foundTab);
                    }

                    FileSystem.Rename(selectedNodePath, newPath);

                    if (selectedNodeName == _mainFileName)
                    {
                        string newMainName = Path.GetFileName(newPath);
                        _mainFileName = newMainName;
                        ScriptProject.Main = newMainName;
                        ScriptProject.SaveProject(newPath);
                    }

                    tvProject.SelectedNode.Name = newName;
                    tvProject.SelectedNode.Text = newName;
                    tvProject.SelectedNode.Tag = newPath;                   
                }
            }
            catch (Exception ex)
            {
                Notify("An Error Occured: " + ex.Message, Color.Red);
            }
        }
        #endregion

        #region Project Pane Buttons
        private void uiBtnRefresh_Click(object sender, EventArgs e)
        {
            tvProject.CollapseAll();
            tvProject.TopNode.Expand();
        }

        private void uiBtnExpand_Click(object sender, EventArgs e)
        {
            tvProject.ExpandAll();
        }

        private void uiBtnCollapse_Click(object sender, EventArgs e)
        {
            tvProject.CollapseAll();
        }

        private void uiBtnOpenDirectory_Click(object sender, EventArgs e)
        {
            Process.Start(ScriptProjectPath);
        }
        #endregion
    }
}
