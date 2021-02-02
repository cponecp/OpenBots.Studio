using Open3270;
using OpenBots.Commands.Terminal.Forms;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.Commands.Terminal
{
    [Serializable]
	[Category("Terminal Commands")]
	[Description("This command creates a terminal session.")]
	public class CreateTerminalSessionCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter a unique name that will represent the application instance.")]
		[SampleUsage("MyTerminalInstance")]
		[Remarks("This unique name allows you to refer to the instance by name in future commands, " +
				 "ensuring that the commands you specify run against the correct application.")]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Host")]
		[Description("Define the host.")]
		[SampleUsage("12.345.678.910 || {vHost}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_Host { get; set; }

		[Required]
		[DisplayName("Port")]
		[Description("Define the port number.")]
		[SampleUsage("3270 || {vPort}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_Port { get; set; }

		[Required]
		[DisplayName("Terminal Type")]
		[Description("Define the terminal type.")]
		[SampleUsage("IBM-3278-2-E || {vTerminalType}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_TerminalType { get; set; }

		[Required]
		[DisplayName("Use Ssl")]
		[PropertyUISelectionOption("Yes")]
		[PropertyUISelectionOption("No")]
		[Description("Specify whether to use Ssl or not")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_UseSsl { get; set; }

		private OpenEmulator _emulator;

		public CreateTerminalSessionCommand()
		{
			CommandName = "CreateTerminalSessionCommand";
			SelectionName = "Create Terminal Session";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultTerminal";
			v_UseSsl = "No";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;

			var host = v_Host.ConvertUserVariableToString(engine);
			var port = int.Parse(v_Port.ConvertUserVariableToString(engine));
			var terminalType = v_TerminalType.ConvertUserVariableToString(engine);
			bool useSsl;

			if (v_UseSsl == "Yes")
				useSsl = true;
			else
				useSsl = false;


			if (engine.AutomationEngineContext.ScriptEngine != null)
			{
				var result = ((Form)engine.AutomationEngineContext.ScriptEngine).Invoke(new Action(() =>
				{
					LaunchTerminalSession(host, port, terminalType, useSsl);
				}));
			}
			else
				LaunchTerminalSession(host, port, terminalType, useSsl);

			_emulator.AddAppInstance(engine, v_InstanceName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			CommandItemControl helperControl = new CommandItemControl();

			helperControl.Padding = new Padding(10, 0, 0, 0);
			helperControl.ForeColor = Color.AliceBlue;
			helperControl.Font = new Font("Segoe UI Semilight", 10);
			helperControl.CommandImage = Resources.command_system;
			helperControl.CommandDisplay = "Launch Terminal Emulator";
			helperControl.Click += new EventHandler((s, e) => LaunchTerminalSession(s, e));

			RenderedControls.Add(helperControl);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Host", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Port", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TerminalType", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_UseSsl", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Host '{v_Host} - Port '{v_Port}' - New Instance Name '{v_InstanceName}']";
		}

		public void LaunchTerminalSession(string host, int port, string terminalType, bool useSsl)
		{
			var terminalForm = new frmTerminal(host, port, terminalType, useSsl);
			terminalForm.Show();
			terminalForm.connectToolStripMenuItem_Click(null, null);
			if (!terminalForm.TN3270.IsConnected)
				throw new Exception($"Unable to connect to host: '{host}', port: '{port}'");

			_emulator = terminalForm.OpenEmulator;
		}

		public void LaunchTerminalSession(object sender, EventArgs e)
		{
			var terminalForm = new frmTerminal();
			terminalForm.Show();
			_emulator = terminalForm.OpenEmulator;
		}
	}
}
