//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
//
// This file is part of OpenPetra.org.
//
// OpenPetra.org is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// OpenPetra.org is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.ExtJs
{
    /// <summary>
    /// generator for a TextField
    /// </summary>
    public class TextFieldGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TextFieldGenerator()
            : base("txt", "textfield")
        {
            FDefaultWidth = -1;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Multiline") != "true"))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Generator for TextArea
    /// </summary>
    public class TextAreaGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TextAreaGenerator()
            : base("txt", "textarea")
        {
            FDefaultWidth = -1;
            FControlDefinitionSnippetName = "TEXTAREADEFINITION";
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Multiline") == "true"))
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Generator for Hidden Fields
    /// </summary>
    public class HiddenFieldGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public HiddenFieldGenerator()
            : base("hid", "hidden")
        {
            FControlDefinitionSnippetName = "HIDDENFIELDDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet,
                "VALUE",
                ctrl,
                TYml2Xml.GetAttribute(ctrl.xmlNode, "value"));

            ctrlSnippet.SetCodelet("VALUE", "this." + ctrl.controlName + "VALUE");

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// Generator for file uploads
    /// </summary>
    public class UploadGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public UploadGenerator()
            : base("upl", "fileuploadfield")
        {
            FControlDefinitionSnippetName = "FILEUPLOADDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            ProcessTemplate uploadCheckAssistantSnippet = writer.FTemplate.GetSnippet("ASSISTANTPAGEWITHUPLOADVALID");

            ((TExtJsFormsWriter)writer).AddResourceString(uploadCheckAssistantSnippet, "MISSINGUPLOADTITLE", ctrl,
                ctrl.GetAttribute("MissingUploadTitle"));
            ((TExtJsFormsWriter)writer).AddResourceString(uploadCheckAssistantSnippet, "MISSINGUPLOADMESSAGE", ctrl,
                ctrl.GetAttribute("MissingUploadMessage"));

            writer.FTemplate.InsertSnippet("ISVALID", uploadCheckAssistantSnippet);
            writer.FTemplate.InsertSnippet("ONSHOW", writer.FTemplate.GetSnippet("ASSISTANTPAGEWITHUPLOADSHOW"));
            writer.FTemplate.InsertSnippet("ONHIDE", writer.FTemplate.GetSnippet("ASSISTANTPAGEWITHUPLOADHIDE"));

            ProcessTemplate uploadSnippet = writer.FTemplate.GetSnippet("UPLOADFORMDEFINITION");

            if (ctrl.HasAttribute("UploadButtonLabel"))
            {
                ((TExtJsFormsWriter)writer).AddResourceString(uploadCheckAssistantSnippet, "UPLOADBUTTONLABEL", ctrl,
                    ctrl.GetAttribute("UploadButtonLabel"));
                uploadSnippet.SetCodelet("UPLOADBUTTONLABEL", ctrl.controlName + "UPLOADBUTTONLABEL");
            }

            writer.FTemplate.InsertSnippet("UPLOADFORM", uploadSnippet);

            ProcessTemplate uploadCheckSnippet = writer.FTemplate.GetSnippet("VALIDUPLOADCHECK");
            writer.FTemplate.InsertSnippet("CHECKFORVALIDUPLOAD", uploadCheckSnippet);

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// Generator for labels
    /// </summary>
    public class LabelGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public LabelGenerator()
            : base("lbl", "displayfield")
        {
            FControlDefinitionSnippetName = "LABELDEFINITION";
        }
    }

    /// <summary>
    /// generator for panels, sets of fields
    /// </summary>
    public class FieldSetGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public FieldSetGenerator()
            : base("pnl", "fieldset")
        {
        }
    }

    /// <summary>
    /// generator for grids, ie data tables
    /// </summary>
    public class GridGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public GridGenerator()
            : base("grd", "")
        {
            FControlDefinitionSnippetName = "GRIDDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            writer.FTemplate.SetCodelet("DATAGRID", "true");

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for button
    /// </summary>
    public class ButtonGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ButtonGenerator()
            : base("btn", "button")
        {
            FControlDefinitionSnippetName = "SUBMITBUTTONDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            if (ctrlSnippet.FTemplateCode.Contains("{#REDIRECTONSUCCESS}"))
            {
                ProcessTemplate redirectSnippet = writer.FTemplate.GetSnippet("REDIRECTONSUCCESS");

                ctrlSnippet.SetCodelet("REDIRECTONSUCCESS", redirectSnippet.FTemplateCode.ToString());
            }

            if (ctrl.HasAttribute("AjaxRequestUrl"))
            {
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "VALIDATIONERRORTITLE", ctrl, ctrl.GetAttribute("ValidationErrorTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "VALIDATIONERRORMESSAGE", ctrl, ctrl.GetAttribute("ValidationErrorMessage"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "SENDINGDATATITLE", ctrl, ctrl.GetAttribute("SendingMessageTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "SENDINGDATAMESSAGE", ctrl, ctrl.GetAttribute("SendingMessage"));

                if (ctrl.HasAttribute("SuccessMessage"))
                {
                    ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTSUCCESSTITLE", ctrl, ctrl.GetAttribute("SuccessMessageTitle"));
                    ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTSUCCESSMESSAGE", ctrl, ctrl.GetAttribute("SuccessMessage"));
                }

                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTFAILURETITLE", ctrl, ctrl.GetAttribute("FailureMessageTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTFAILUREMESSAGE", ctrl, ctrl.GetAttribute("FailureMessage"));
            }

            ctrlSnippet.SetCodelet("REQUESTURL", ctrl.GetAttribute("AjaxRequestUrl"));
            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REDIRECTURLONSUCCESS", ctrl, ctrl.GetAttribute("RedirectURLOnSuccess"));

            if (ctrl.GetAttribute("DownloadOnSuccess").StartsWith("jsonData"))
            {
                ctrlSnippet.SetCodelet("REDIRECTDOWNLOAD", ctrl.GetAttribute("DownloadOnSuccess"));
            }

            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REDIRECTURLONCANCEL", ctrl, ctrl.GetAttribute("RedirectURLOnCancel"));
            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "CANCELQUESTIONTITLE", ctrl, ctrl.GetAttribute("CancelQuestionTitle"));
            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "CANCELQUESTIONMESSAGE", ctrl, ctrl.GetAttribute("CancelQuestionMessage"));

            XmlNode AjaxParametersNode = TYml2Xml.GetChild(ctrl.xmlNode, "AjaxRequestParameters");

            if (AjaxParametersNode != null)
            {
                string ParameterString = String.Empty;

                foreach (XmlAttribute attr in AjaxParametersNode.Attributes)
                {
                    if (!attr.Name.Equals("depth"))
                    {
                        ParameterString += attr.Name + ": '" + attr.Value + "', ";
                    }
                }

                ctrlSnippet.SetCodelet("REQUESTPARAMETERS", ParameterString);
                writer.FTemplate.SetCodelet("REQUESTPARAMETERS", "true");
            }

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for comboboxes
    /// </summary>
    public class ComboboxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ComboboxGenerator()
            : base("cmb", "checkbox")
        {
            FDefaultWidth = 190;
            FControlDefinitionSnippetName = "COMBOBOXDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            string valuesArray = "[";

            List <XmlNode>optionalValues =
                TYml2Xml.GetChildren(TXMLParser.GetChild(ctrl.xmlNode, "OptionalValues"), true);

            // DefaultValue with = sign before control name
            for (int counter = 0; counter < optionalValues.Count; counter++)
            {
                string loopValue = TYml2Xml.GetElementName(optionalValues[counter]);

                if (loopValue.StartsWith("="))
                {
                    loopValue = loopValue.Substring(1).Trim();
                    ctrlSnippet.SetCodelet("VALUE", loopValue);
                }

                if (counter > 0)
                {
                    valuesArray += ", ";
                }

                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "OPTION" + counter.ToString(), ctrl, loopValue);

                string strName = "this." + ctrl.controlName + "OPTION" + counter.ToString();

                valuesArray += "['" + loopValue + "', " + strName + "]";
            }

            valuesArray += "]";

            ctrlSnippet.SetCodelet("OPTIONALVALUESARRAY", valuesArray);

            if (ctrl.HasAttribute("width"))
            {
                ctrlSnippet.SetCodelet("WIDTH", ctrl.GetAttribute("width"));
            }

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for checkboxes
    /// </summary>
    public class CheckboxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public CheckboxGenerator()
            : base("chk", "checkbox")
        {
            FControlDefinitionSnippetName = "CHECKBOXDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            ctrlSnippet.SetCodelet("BOXLABEL", ctrlSnippet.FCodelets["LABEL"].ToString());
            ctrlSnippet.SetCodelet("LABEL", "strEmpty");
            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for a single radio button
    /// </summary>
    public class RadioButtonGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public RadioButtonGenerator()
            : base("rbt", "radio")
        {
            FControlDefinitionSnippetName = "CHECKBOXDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            if (TXMLParser.HasAttribute(ctrl.xmlNode, "RadioChecked"))
            {
                ctrlSnippet.SetCodelet("CHECKED", "true");
            }

            ctrlSnippet.SetCodelet("BOXLABEL", ctrlSnippet.FCodelets["LABEL"].ToString());
            ctrlSnippet.SetCodelet("LABEL", "strEmpty");
            ctrlSnippet.SetCodelet("INPUTVALUE", ctrl.controlName.Substring(3));

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for a date picker
    /// </summary>
    public class DateTimePickerGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public DateTimePickerGenerator()
            : base("dtp", "datefield")
        {
            FDefaultWidth = -1;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ACtrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ACtrl);

            string customAttributes = "boxMaxWidth: 175,";

            ctrlSnippet.SetCodelet("CUSTOMATTRIBUTES", customAttributes);

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for HTML iFrames
    /// </summary>
    public class InlineGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public InlineGenerator()
            : base("inl", "displayfield")
        {
            FControlDefinitionSnippetName = "INLINEDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ACtrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ACtrl);

            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet,
                "URL",
                ACtrl,
                TYml2Xml.GetAttribute(ACtrl.xmlNode, "url"));
            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet,
                "BROWSERMISSINGIFRAMESUPPORT",
                null,
                "Your browser is not able to display embedded documents. Please click on the following link to read the text: ");
            ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet,
                "IFRAMEINBIGGERWINDOW",
                null,
                "Open this document in a bigger window");

            ctrlSnippet.SetCodelet("HEIGHT", "250");

            if (ACtrl.HasAttribute("Height"))
            {
                ctrlSnippet.SetCodelet("HEIGHT", ACtrl.GetAttribute("Height"));
            }

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for putting several controls together
    /// </summary>
    public class CompositeGenerator : GroupBoxBaseGenerator
    {
        /// <summary>constructor</summary>
        public CompositeGenerator()
            : base("cmp")
        {
            FControlDefinitionSnippetName = "COMPOSITEDEFINITION";
        }
    }

    /// <summary>
    /// generator for group boxes
    /// </summary>
    public class GroupBoxGenerator : GroupBoxBaseGenerator
    {
        /// <summary>constructor</summary>
        public GroupBoxGenerator()
            : base("grp")
        {
            FControlDefinitionSnippetName = "GROUPBOXDEFINITION";
        }
    }

    /// <summary>
    /// generator for assistant driven UI
    /// </summary>
    public class AssistantGenerator : GroupBoxBaseGenerator
    {
        /// <summary>constructor</summary>
        public AssistantGenerator()
            : base("ass")
        {
            FControlDefinitionSnippetName = "ASSISTANTDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            writer.FTemplate.SetCodelet("ASSISTANT", "true");

            string AssistantHeader = "true";

            if (ctrl.HasAttribute("AssistantHeader"))
            {
                AssistantHeader = ctrl.GetAttribute("AssistantHeader");
            }

            ctrlSnippet.SetCodelet("ASSISTANTHEADER", AssistantHeader);

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for a single page of an assistant
    /// </summary>
    public class AssistantPageGenerator : GroupBoxBaseGenerator
    {
        private int PageCounter = 0;

        /// <summary>constructor</summary>
        public AssistantPageGenerator()
            : base("asp")
        {
            FControlDefinitionSnippetName = "ASSISTANTPAGEDEFINITION";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ACtrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ACtrl);

            ctrlSnippet.SetCodelet("PAGENUMBER", PageCounter.ToString());

            if (writer.FTemplate.FCodelets.Keys.Contains("CUSTOMFUNCTIONS"))
            {
                ctrlSnippet.SetCodelet("CUSTOMFUNCTIONS", writer.FTemplate.FCodelets["CUSTOMFUNCTIONS"].ToString());
                writer.FTemplate.FCodelets.Remove("CUSTOMFUNCTIONS");
            }
            else
            {
                ctrlSnippet.SetCodelet("CUSTOMFUNCTIONS", String.Empty);
            }

            if (writer.FTemplate.FCodelets.Keys.Contains("ONSHOW"))
            {
                ctrlSnippet.SetCodelet("ONSHOW", writer.FTemplate.FCodelets["ONSHOW"].ToString());
                writer.FTemplate.FCodelets.Remove("ONSHOW");
            }

            if (writer.FTemplate.FCodelets.Keys.Contains("ISVALID"))
            {
                ctrlSnippet.SetCodelet("ISVALID", writer.FTemplate.FCodelets["ISVALID"].ToString());
                writer.FTemplate.FCodelets.Remove("ISVALID");
            }

            if (writer.FTemplate.FCodelets.Keys.Contains("ONHIDE"))
            {
                ctrlSnippet.SetCodelet("ONHIDE", writer.FTemplate.FCodelets["ONHIDE"].ToString());
                writer.FTemplate.FCodelets.Remove("ONHIDE");
            }

            if (ACtrl.HasAttribute("Height"))
            {
                ctrlSnippet.AddToCodelet("ONSHOW", String.Format("MainForm.setHeight({0});", ACtrl.GetAttribute("Height")));
            }

            if (ACtrl.HasAttribute("LabelWidth"))
            {
                ctrlSnippet.SetCodelet("LABELWIDTH", ACtrl.GetAttribute("LabelWidth"));
            }

            PageCounter++;

            return ctrlSnippet;
        }
    }

    /// <summary>
    /// generator for a simple group of radio buttons.
    /// simple because they just have labels, and no controls depend on them
    /// </summary>
    public class RadioGroupSimpleGenerator : GroupBoxBaseGenerator
    {
        /// <summary>constructor</summary>
        public RadioGroupSimpleGenerator()
            : base("rgr")
        {
            FControlDefinitionSnippetName = "RADIOGROUPDEFINITION";
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TXMLParser.GetChild(curNode, "OptionalValues") != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// create the radio buttons
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public override StringCollection FindContainedControls(TFormWriter writer, XmlNode curNode)
        {
            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "OptionalValues"));
            string DefaultValue = optionalValues[0];

            if (TXMLParser.HasAttribute(curNode, "DefaultValue"))
            {
                DefaultValue = TXMLParser.GetAttribute(curNode, "DefaultValue");
            }
            else
            {
                // DefaultValue with = sign before control name
                for (int counter = 0; counter < optionalValues.Count; counter++)
                {
                    if (optionalValues[counter].StartsWith("="))
                    {
                        optionalValues[counter] = optionalValues[counter].Substring(1).Trim();
                        DefaultValue = optionalValues[counter];
                    }
                }
            }

            // add the radiobuttons on the fly
            StringCollection Controls = new StringCollection();

            foreach (string optionalValue in optionalValues)
            {
                string radioButtonName = "rbt" +
                                         StringHelper.UpperCamelCase(optionalValue.Replace("'", "").Replace(" ",
                        "_").Replace("&",
                        ""), false, false);
                TControlDef newCtrl = writer.CodeStorage.FindOrCreateControl(radioButtonName, curNode.Name);
                newCtrl.Label = optionalValue;

                if (StringHelper.IsSame(DefaultValue, optionalValue))
                {
                    newCtrl.SetAttribute("RadioChecked", "true");
                }

                Controls.Add(radioButtonName);
            }

            return Controls;
        }
    }

    /// this is for radiogroup with all sorts of sub controls
    public class RadioGroupComplexGenerator : GroupBoxBaseGenerator
    {
        /// <summary>constructor</summary>
        public RadioGroupComplexGenerator()
            : base("rgr")
        {
            FControlDefinitionSnippetName = "RADIOGROUPDEFINITION";
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return TXMLParser.GetChild(curNode, "Controls") != null;
            }

            return false;
        }

        /// <summary>
        /// create the radio buttons and the controls that depend on them
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public override StringCollection FindContainedControls(TFormWriter writer, XmlNode curNode)
        {
            StringCollection Controls =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));
            string DefaultValue = Controls[0];

            if (TXMLParser.HasAttribute(curNode, "DefaultValue"))
            {
                DefaultValue = TXMLParser.GetAttribute(curNode, "DefaultValue");
            }

            foreach (string controlName in Controls)
            {
                TControlDef radioButton = writer.CodeStorage.GetControl(controlName);

                if (radioButton == null)
                {
                    throw new Exception("cannot find control " + controlName + " used in RadioGroup " + curNode.Name);
                }

                if (StringHelper.IsSame(DefaultValue, controlName))
                {
                    radioButton.SetAttribute("RadioChecked", "true");
                }
            }

            return Controls;
        }
    }
}