using Open3270.TN3270;
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
using System.Linq;
using Newtonsoft.Json;

namespace OpenBots.Commands.Input
{
    [Serializable]
	[Category("Terminal Commands")]
	[Description("This command retrieves a field index from a targeted terminal screen.")]
	public class TerminalGetFieldIndexCommand : ScriptCommand
	{
		[Required]
		[DisplayName("Terminal Instance Name")]
		[Description("Enter the unique instance that was specified in the **Create Terminal Session** command.")]
		[SampleUsage("MyWordInstance")]
		[Remarks("Failure to enter the correct instance or failure to first call the **Create Terminal Session** command will cause an error.")]
		public string v_InstanceName { get; set; }

		[Required]
		[DisplayName("Search Option")]
		[PropertyUISelectionOption("Coordinates")]
		[PropertyUISelectionOption("Field Text")]
		[Description("Select whether the DataRow value should be found by column index or column name.")]
		[SampleUsage("")]
		[Remarks("")]
		public string v_Option { get; set; }		

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
		[DisplayName("Text to Search for")]
		[Description("Enter the text to search for on the terminal.")]
		[SampleUsage("Hello, World! || {vText}")]
		[Remarks("")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_FieldText { get; set; }

		[Required]
		[Editable(false)]
		[DisplayName("Output Field Index Variable")]
		[Description("Create a new variable or select a variable from the list.")]
		[SampleUsage("{vUserVariable}")]
		[Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
		public string v_OutputUserVariableName { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _coordinateControls;

		[JsonIgnore]
		[Browsable(false)]
		private List<Control> _fieldTextControls;

		public TerminalGetFieldIndexCommand()
		{
			CommandName = "TerminalGetFieldIndexCommand";
			SelectionName = "Get Field Index";
			CommandEnabled = true;
			CommandIcon = Resources.command_system;

			v_InstanceName = "DefaultTerminal";
			v_Option = "Coordinates";
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			var terminalObject = (OpenEmulator)v_InstanceName.GetAppInstance(engine);
			
			if (terminalObject.TN3270 == null || !terminalObject.TN3270.IsConnected)
				throw new Exception($"Terminal Instance {v_InstanceName} is not connected.");

			XMLScreenField field = null;
			List<XMLScreenField> fields = terminalObject.TN3270.CurrentScreenXML.Fields.ToList();

			if (v_Option == "Coordinates")
            {
				var mouseX = int.Parse(v_XMousePosition.ConvertUserVariableToString(engine));
				var mouseY = int.Parse(v_YMousePosition.ConvertUserVariableToString(engine));
				field = fields.Where(f => (mouseY * 80 + mouseX) >= f.Location.position && (mouseY * 80 + mouseX) < f.Location.position + f.Location.length).FirstOrDefault();
			}
            else
            {
				var fieldText = v_FieldText.ConvertUserVariableToString(engine);
				field = fields.Where(f => f.Text != null && f.Text.ToLower().Contains(fieldText.ToLower())).FirstOrDefault();
			}

			int fieldIndex = -1;
			if (field != null)
				fieldIndex = Array.IndexOf(terminalObject.TN3270.CurrentScreenXML.Fields, field);

			fieldIndex.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
			RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));

			((ComboBox)RenderedControls[3]).SelectedIndexChanged += searchOptionComboBox_SelectedValueChanged;

			_coordinateControls = new List<Control>();
			_coordinateControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
			_coordinateControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));

			foreach (var ctrl in _coordinateControls)
				ctrl.Visible = false;

			RenderedControls.AddRange(_coordinateControls);

			_fieldTextControls = new List<Control>();
			_fieldTextControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FieldText", this, editor));

			foreach (var ctrl in _fieldTextControls)
				ctrl.Visible = false;

			RenderedControls.AddRange(_fieldTextControls);

			RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			if (v_Option == "Coordinates")
				return base.GetDisplayValue() + $" [At Coordinates '{{{v_XMousePosition}, {v_YMousePosition}}}' - Store Field Index in {v_OutputUserVariableName} - Instance Name '{v_InstanceName}']";
			else
				return base.GetDisplayValue() + $" [With Field Text '{v_FieldText}' - Store Field Index in {v_OutputUserVariableName} - Instance Name '{v_InstanceName}']";

		}

		private void searchOptionComboBox_SelectedValueChanged(object sender, EventArgs e)
		{
			if (((ComboBox)RenderedControls[3]).Text == "Coordinates")
			{
				foreach (var ctrl in _coordinateControls)
					ctrl.Visible = true;

				foreach (var ctrl in _fieldTextControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}
			}
			else
			{
				foreach (var ctrl in _coordinateControls)
				{
					ctrl.Visible = false;
					if (ctrl is TextBox)
						((TextBox)ctrl).Clear();
				}

				foreach (var ctrl in _fieldTextControls)
					ctrl.Visible = true;
			}
		}
	}
}