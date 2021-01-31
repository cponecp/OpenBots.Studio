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
	[Description("This command sends keystrokes to a targeted terminal.")]
	public class TerminalSetFieldCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Terminal Session** command.")]
		[SampleUsage("MyWordInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Terminal Session** command will cause an error.")]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Field Index")]
		[Description("Enter the index of the field to set text in.")]
		[SampleUsage("0 || {vFieldIndex}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_FieldIndex { get; set; }

		[Required]
		[DisplayName("Text to Send")]
		[Description("Enter the text to be sent to the specified terminal.")]
		[SampleUsage("Hello, World! || {vText}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_TextToSend { get; set; }

		public TerminalSetFieldCommand()
		{
			CommandName = "TerminalSetFieldCommand";
			SelectionName = "Set Field";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultTerminal";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var fieldIndex = int.Parse(v_FieldIndex.ConvertUserVariableToString(engine));
			string textToSend = v_TextToSend.ConvertUserVariableToString(engine);
			var terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);

			terminalObject.TN3270.SetField(fieldIndex, textToSend);

			terminalObject.Redraw();
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FieldIndex", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSend", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return base.GetDisplayValue() + $" [Field Index - {v_FieldIndex} - Text '{v_TextToSend}' - Instance Name '{v_InstanceName}']";
		}     
	}
}