using MimeKit;
using Newtonsoft.Json;
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
using System.IO;
using System.Windows.Forms;
using OBFile = System.IO.File;

namespace OpenBots.Commands.Email
{
    [Serializable]
    [Category("Outlook Commands")]
    [Description("This command gets attachments from a selected email in Outlook.")]

    public class GetIMAPEmailAttachmentsCommand : ScriptCommand
    {
        [Required]
        [DisplayName("MimeMessage")]
        [Description("Enter the MimeMessage to delete.")]
        [SampleUsage("{vMimeMessage}")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        public string v_IMAPMimeMessage { get; set; }

        [Required]
        [DisplayName("Output Attachment Directory")]
        [Description("Enter or Select the path to the directory to store the attachments in.")]
        [SampleUsage(@"C:\temp\myfolder\attachments || {vFolderPath} || {ProjectPath}\myFolder\attachments")]
        [Remarks("")]
        [Editor("ShowVariableHelper", typeof(UIAdditionalHelperType))]
        [Editor("ShowFolderSelectionHelper", typeof(UIAdditionalHelperType))]
        public string v_IMAPAttachmentDirectory { get; set; }

        [Required]
        [DisplayName("Include Embedded Images")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [Description("Specify whether to consider images in body as attachments.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IncludeEmbeddedImagesAsAttachments { get; set; }

        [Required]
        [Editable(false)]
        [DisplayName("Output Attachment List Variable")]
        [Description("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetIMAPEmailAttachmentsCommand()
        {
            CommandName = "GetIMAPEmailAttachmentsCommand";
            SelectionName = "Get IMAP Email Attachments";
            CommandEnabled = true;
            CommandIcon = Resources.command_smtp;
        }

        public override void RunCommand(object sender)
        {
            var engine = (IAutomationEngineInstance)sender;
            MimeMessage email = (MimeMessage)v_IMAPMimeMessage.ConvertUserVariableToObject(engine);
            bool includeEmbeds = v_IncludeEmbeddedImagesAsAttachments.ConvertUserVariableToString(engine).Equals("Yes");
            string attDirectory = v_IMAPAttachmentDirectory.ConvertUserVariableToString(engine);

            List<string> attachmentList = new List<string>();
            
            foreach (var attachment in email.Attachments)
            {
                if (attachment is MessagePart)
                {
                    var fileName = attachment.ContentDisposition?.FileName;
                    var rfc822 = (MessagePart)attachment;

                    if (string.IsNullOrEmpty(fileName))
                        fileName = "attached-message.eml";
                    if (!email.HtmlBody.Contains(fileName) || includeEmbeds)
                    {
                        using (var stream = OBFile.Create(Path.Combine(attDirectory, fileName)))
                            rfc822.Message.WriteTo(stream);

                        attachmentList.Add(Path.Combine(attDirectory, fileName));
                    }
                }
                else
                {
                    var part = (MimePart)attachment;
                    var fileName = part.FileName;
                    if (!email.HtmlBody.Contains(fileName) || includeEmbeds)
                    {
                        using (var stream = OBFile.Create(Path.Combine(attDirectory, fileName)))
                            part.Content.DecodeTo(stream);

                        attachmentList.Add(Path.Combine(attDirectory, fileName));
                    }
                }
            }
            
            attachmentList.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPMimeMessage", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPAttachmentDirectory", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IncludeEmbeddedImagesAsAttachments", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_IMAPMimeMessage}' - Store Attachment List in '{v_OutputUserVariableName}']";
        }
    }
}
