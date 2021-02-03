﻿using Newtonsoft.Json;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.Commands.Database
{
	[Serializable]
	[Category("Database Commands")]
	[Description("This command connects to an OleDb database.")]
	public class DefineDatabaseConnectionCommand : ScriptCommand
	{

		[Required]
		[DisplayName("Database Instance Name")]
		[Description("Enter a unique name that will represent the application instance.")]
		[SampleUsage("MyDatabaseInstance")]
		[Remarks("This unique name allows you to refer to the instance by name in future commands, " +
				 "ensuring that the commands you specify run against the correct application.")]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Connection String")]
		[Description("Define the string to use when connecting to the OleDb database.")]
		[SampleUsage("Provider=sqloledb;Data Source=myServerAddress;Initial Catalog=myDataBase;Integrated Security=SSPI; || {vConnectionString}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_ConnectionString { get; set; }

		[Required]
		[DisplayName("Connection String Password")]
		[Description("Define the password to use when connecting to the OleDb database.")]
		[SampleUsage("password || {vPassword}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_ConnectionStringPassword { get; set; }

		[Required]
		[DisplayName("Test Connection Before Proceeding")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Select the appropriate option.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_TestConnection { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private TextBox _connectionString;

		[JsonIgnore]
		[Browsable(false)]
		private TextBox _connectionStringPassword;

		public DefineDatabaseConnectionCommand()
		{
			CommandName = "DefineDatabaseConnectionCommand";
			SelectionName = "Define Database Connection";
			CommandEnabled = true;
			CommandIcon = Resources.command_database;

			v_InstanceName = "DefaultDatabase";
			v_TestConnection = "Yes";
		}

		public override void RunCommand(object sender)
		{
			//get engine and preference
			var engine = (IAutomationEngineInstance)sender;

			//create connection
			var oleDBConnection = CreateConnection(sender);

			//attempt to open and close connection
			if (v_TestConnection == "Yes")
			{
				oleDBConnection.Open();
				oleDBConnection.Close();
			}

			oleDBConnection.AddAppInstance(engine, v_InstanceName);
		}

		private OleDbConnection CreateConnection(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var connection = v_ConnectionString.ConvertUserVariableToString(engine);
			var connectionPass = v_ConnectionStringPassword.ConvertUserVariableToString(engine);

			if (connectionPass.StartsWith("!"))
			{
				connectionPass = connectionPass.Substring(1);
				connectionPass = EncryptionServices.DecryptString(connectionPass, "openbots-database-automation");
			}
			connection = connection.Replace("#pwd", connectionPass);

			return new OleDbConnection(connection);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

			CommandItemControl helperControl = new CommandItemControl();
			helperControl.Padding = new Padding(10, 0, 0, 0);
			helperControl.ForeColor = Color.AliceBlue;
			helperControl.Font = new Font("Segoe UI Semilight", 10);
			helperControl.Name = "connection_helper";
			helperControl.CommandImage = Resources.command_database;
			helperControl.CommandDisplay = "Build Connection String";
			helperControl.Click += (sender, e) => Button_Click(sender, e);

			_connectionString = (TextBox)commandControls.CreateDefaultInputFor("v_ConnectionString", this);

			var connectionLabel = commandControls.CreateDefaultLabelFor("v_ConnectionString", this);
			var connectionHelpers = commandControls.CreateUIHelpersFor("v_ConnectionString", this, new[] { _connectionString }, editor);
			CommandItemControl testConnectionControl = new CommandItemControl();
			testConnectionControl.Padding = new Padding(10, 0, 0, 0);
			testConnectionControl.ForeColor = Color.AliceBlue;
			testConnectionControl.Font = new Font("Segoe UI Semilight", 10);
			testConnectionControl.Name = "connection_helper";
			testConnectionControl.CommandImage = Resources.command_database;
			testConnectionControl.CommandDisplay = "Test Connection";
			testConnectionControl.Click += (sender, e) => TestConnection(sender, e, editor, commandControls);

			RenderedControls.Add(connectionLabel);
			RenderedControls.AddRange(connectionHelpers);
			RenderedControls.Add(helperControl);
			RenderedControls.Add(testConnectionControl);
			RenderedControls.Add(_connectionString);

			_connectionStringPassword = (TextBox)commandControls.CreateDefaultInputFor("v_ConnectionStringPassword", this);

			var connectionPassLabel = commandControls.CreateDefaultLabelFor("v_ConnectionStringPassword", this);
			var connectionPassHelpers = commandControls.CreateUIHelpersFor("v_ConnectionStringPassword", this, new[] { _connectionStringPassword }, editor);

			RenderedControls.Add(connectionPassLabel);
			RenderedControls.AddRange(connectionPassHelpers);

			CommandItemControl passwordHelperControl = new CommandItemControl();
			passwordHelperControl.Padding = new Padding(10, 0, 0, 0);
			passwordHelperControl.ForeColor = Color.AliceBlue;
			passwordHelperControl.Font = new Font("Segoe UI Semilight", 10);
			passwordHelperControl.Name = "show_pass_helper";
			passwordHelperControl.CommandImage = Resources.command_password;
			passwordHelperControl.CommandDisplay = "Show Password";
			passwordHelperControl.Click += (sender, e) => TogglePasswordChar(passwordHelperControl, e);
			RenderedControls.Add(passwordHelperControl);

			CommandItemControl encryptHelperControl = new CommandItemControl();
			encryptHelperControl.Padding = new Padding(10, 0, 0, 0);
			encryptHelperControl.ForeColor = Color.AliceBlue;
			encryptHelperControl.Font = new Font("Segoe UI Semilight", 10);
			encryptHelperControl.Name = "show_pass_helper";
			encryptHelperControl.CommandImage = Resources.command_password;
			encryptHelperControl.CommandDisplay = "Encrypt Password";
			encryptHelperControl.Click += (sender, e) => EncryptPassword(passwordHelperControl, e);
			RenderedControls.Add(encryptHelperControl);
		   
			var label = new Label();
			label.AutoSize = true;
			label.Font = new Font("Segoe UI Semilight", 9);
			label.ForeColor = Color.White;
			label.Text = "NOTE: If storing the password in the textbox below, please ensure the connection string " +
				"above contains a database-specific placeholder with #pwd to be replaced at runtime. (;Password=#pwd)";
			RenderedControls.Add(label);

			RenderedControls.Add(_connectionStringPassword);
			_connectionStringPassword.PasswordChar = '*';

			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_TestConnection", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
		}

		private void TestConnection(object sender, EventArgs e, IfrmCommandEditor editor, ICommandControls commandControls)
		{
			
			try
			{
				var engine = (IAutomationEngineInstance)commandControls.CreateAutomationEngineInstance(editor.ScriptEngineContext);
				var oleDBConnection = CreateConnection(engine);
				oleDBConnection.Open();
				oleDBConnection.Close();
				MessageBox.Show("Connection Successful", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Connection Failed: {ex}", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Button_Click(object sender, EventArgs e)
		{
			ShowConnectionBuilder();
		}

		private void TogglePasswordChar(CommandItemControl sender, EventArgs e)
		{
			//if password is hidden
			if (_connectionStringPassword.PasswordChar == '*')
			{
				//show password plain text
				sender.CommandDisplay = "Hide Password";
				_connectionStringPassword.PasswordChar = '\0';
			}
			else
			{
				//mask password with chars
				sender.CommandDisplay = "Show Password";
				_connectionStringPassword.PasswordChar = '*';
			}
		}

		private void EncryptPassword(CommandItemControl sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(_connectionStringPassword.Text))
				return;

			var acknowledgement =  MessageBox.Show("WARNING! This function will encrypt the password locally but " + 
												   "is not extremely secure as the client knows the secret! " + 
												   "Consider using a password management service instead. The encrypted " +
												   "password will be stored with a leading exclamation ('!') whch the " +
												   "automation engine will detect and know to decrypt the value automatically " +
												   "at run-time. Do not encrypt the password multiple times or the decryption " +
												   "will be invalid!  Would you like to proceed?", "Encryption Warning", 
												   MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

			if (acknowledgement == DialogResult.Yes)
				_connectionStringPassword.Text = string.Concat($"!{EncryptionServices.EncryptString(_connectionStringPassword.Text, "openbots-database-automation")}");
		}

		public void ShowConnectionBuilder()
		{
			var MSDASCObj = new MSDASC.DataLinks();
			var connection = new ADODB.Connection();
			MSDASCObj.PromptEdit(connection);

			if (!string.IsNullOrEmpty(connection.ConnectionString))
				_connectionString.Text = connection.ConnectionString;
		}
	}
}
