using Newtonsoft.Json;
using Open3270;
using OpenBots.Commands.Terminal.Forms;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Utilities.CommonUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;

namespace OpenBots.Commands.Input
{
    [Serializable]
	[Category("Terminal Commands")]
	[Description("This command sends advanced keystrokes to a targeted terminal.")]
	public class TerminalSendKeyKeyCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Terminal Session** command.")]
		[SampleUsage("MyWordInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Terminal Session** command will cause an error.")]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("X Position")]
		[Description("Input the new horizontal coordinate of the terminal. Starts from 0 on the left and increases going right.")]
		[SampleUsage("0 || {vXPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by the terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_XMousePosition { get; set; }

		[Required]
		[DisplayName("Y Position")]
		[Description("Input the new vertical coordinate of the terminal. Starts from 0 at the top and increases going down.")]
		[SampleUsage("0 || {vYPosition}")]
		[Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by ther terminal.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_YMousePosition { get; set; }

		[Required]
		[DisplayName("Keystroke Parameters")]
		[Description("Define the parameters for the keystroke actions.")]
		[SampleUsage("[Enter [Return] | Key Press (Down + Up)]")]
		[Remarks("")]
		public string v_TerminalKey { get; set; }

		[Required]
		[DisplayName("Timeout (Seconds)")]
		[Description("Specify how many seconds to wait before throwing an exception.")]
		[SampleUsage("30 || {vSeconds}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_Timeout { get; set; }

		public TerminalSendKeyKeyCommand()
		{
			CommandName = "TerminalSendKeyKeyCommand";
			SelectionName = "Send Key";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultTerminal";
			v_Timeout = "30";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var mouseX = int.Parse(v_XMousePosition.ConvertUserVariableToString(engine));
			var mouseY = int.Parse(v_YMousePosition.ConvertUserVariableToString(engine));
			var vTimeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine)) * 1000;
			OpenEmulator terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);

			TnKey selectedKey = (TnKey)Enum.Parse(typeof(TnKey), v_TerminalKey);
			
			terminalObject.TN3270.SetCursor(mouseX, mouseY);
			terminalObject.TN3270.SendKey(false, selectedKey, vTimeout);
			terminalObject.Redraw();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));

			var terminalKeyNameLabel = commandControls.CreateDefaultLabelFor("v_TerminalKey", this);
			var terminalKeyNameComboBox = (ComboBox)commandControls.CreateDropdownFor("v_TerminalKey", this);
			terminalKeyNameComboBox.DataSource = Enum.GetValues(typeof(TnKey));

			RenderedControls.Add(terminalKeyNameLabel);
			RenderedControls.Add(terminalKeyNameComboBox);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

			return RenderedControls;
		}
	 
		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Terminal Key '{v_TerminalKey}' - Instance Name '{v_InstanceName}']";
		}
	}
}