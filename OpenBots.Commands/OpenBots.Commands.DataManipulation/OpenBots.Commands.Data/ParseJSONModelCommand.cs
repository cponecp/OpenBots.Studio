﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using OBDataTable = System.Data.DataTable;

namespace OpenBots.Commands.Data
{
	[Serializable]
	[Category("Data Commands")]
	[Description("This command runs a number of queries on a JSON object and saves the results in the specified list variables.")]
	public class ParseJSONModelCommand : ScriptCommand
	{

		[Required]
		[DisplayName("JSON Object")]
		[Description("Provide a variable or JSON object value.")]
		[SampleUsage("{\"rect\":{\"length\":10, \"width\":5}} || {vJsonObject}")]
		[Remarks("Providing data of a type other than a 'JSON Object' will result in an error.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public string v_JsonObject { get; set; }

		[Required]
		[DisplayName("Parameters")]
		[Description("Specify JSON Selector(s) (JPath) and Output Variable(s).")]
		[SampleUsage("[$.rect.length | vOutputList] || [{Selector} | {vOutputList}]")]
		[Remarks("'$.rect.length' is a JSON Selector to query on an inputted JSON Object and store its results in {vOutputList}.")]
		[Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
		public OBDataTable v_ParseObjects { get; set; }

		[JsonIgnore]
		[Browsable(false)]
		private DataGridView _parseObjectsGridViewHelper;

		public ParseJSONModelCommand()
		{
			CommandName = "ParseJSONModelCommand";
			SelectionName = "Parse JSON Model";
			CommandEnabled = true;
			CommandIcon = Resources.command_parse;

			v_ParseObjects = new OBDataTable();
			v_ParseObjects.Columns.Add("Json Selector");
			v_ParseObjects.Columns.Add("Output Variable");
			v_ParseObjects.TableName = $"ParseJsonObjectsTable{DateTime.Now.ToString("MMddyyhhmmss")}";

			_parseObjectsGridViewHelper = new DataGridView();
			_parseObjectsGridViewHelper.AllowUserToAddRows = true;
			_parseObjectsGridViewHelper.AllowUserToDeleteRows = true;
			_parseObjectsGridViewHelper.Size = new Size(400, 250);
			_parseObjectsGridViewHelper.ColumnHeadersHeight = 30;
			_parseObjectsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
			_parseObjectsGridViewHelper.DataBindings.Add("DataSource", this, "v_ParseObjects", false, DataSourceUpdateMode.OnPropertyChanged);
		}

		public override void RunCommand(object sender)
		{
			var engine = (IAutomationEngineInstance)sender;
			
			//get variablized input
			var variableInput = v_JsonObject.ConvertUserVariableToString(engine);

			foreach (DataRow rw in v_ParseObjects.Rows)
			{
				var jsonSelector = rw.Field<string>("Json Selector").ConvertUserVariableToString(engine);
				var targetVariableName = rw.Field<string>("Output Variable");

				//create objects
				JObject o;
				IEnumerable<JToken> searchResults;
				List<string> resultList = new List<string>();

				//parse json
				try
				{
					o = JObject.Parse(variableInput);
				}
				catch (Exception ex)
				{
					throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
				}

				//select results
				try
				{
					searchResults = o.SelectTokens(jsonSelector);
				}
				catch (Exception ex)
				{
					throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
				}

				//add results to result list since list<string> is supported
				foreach (var result in searchResults)
				{
					resultList.Add(result.ToString());
				}

				resultList.StoreInUserVariable(engine, targetVariableName);               
			}
		}

		public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
		{
			base.Render(editor, commandControls);

			//create standard group controls
			RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_JsonObject", this, editor));

			RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ParseObjects", this));
			RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ParseObjects", this, new[] { _parseObjectsGridViewHelper }, editor));
			RenderedControls.Add(_parseObjectsGridViewHelper);

			return RenderedControls;
		}

		public override string GetDisplayValue()
		{
			return $"{base.GetDisplayValue()} [Select {v_ParseObjects.Rows.Count} Item(s) From JSON]";
		}
	}
}