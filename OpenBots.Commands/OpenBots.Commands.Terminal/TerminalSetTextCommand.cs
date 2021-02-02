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
	[Description("This command sets text at a targeted terminal screen's coordinates.")]
	public class TerminalSetTextCommand : ScriptCommand
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
		[DisplayName("Text to Set")]
		[Description("Enter the text to be sent to the specified terminal.")]
		[SampleUsage("Hello, World! || {vText}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_TextToSet { get; set; }

		public TerminalSetTextCommand()
		{
			CommandName = "TerminalSetTextCommand";
			SelectionName = "Set Text";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;
			v_InstanceName = "DefaultTerminal";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var mouseX = int.Parse(v_XMousePosition.ConvertUserVariableToString(engine));
			var mouseY = int.Parse(v_YMousePosition.ConvertUserVariableToString(engine));
			string textToSend = v_TextToSet.ConvertUserVariableToString(engine);
			var terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);

			if (terminalObject.TN3270 == null || !terminalObject.TN3270.IsConnected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			terminalObject.TN3270.SetCursor(mouseX, mouseY);
			terminalObject.TN3270.SetText(textToSend);

			terminalObject.Redraw();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Text '{v_TextToSet}' at Coordinates '{{{v_XMousePosition}, {v_YMousePosition}}}' - Instance Name '{v_InstanceName}']";
		}     
	}
}