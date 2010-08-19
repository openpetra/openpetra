//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Collections.Specialized;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.ExtJs
{
    public class TextFieldGenerator : TControlGenerator
    {
        public TextFieldGenerator()
            : base("txt", "textfield")
        {
            FDefaultWidth = -1;
        }
    }
    public class LabelGenerator : TControlGenerator
    {
        public LabelGenerator()
            : base("lbl", "displayfield")
        {
            FControlDefinitionSnippetName = "LABELDEFINITION";
        }
    }
    public class FieldSetGenerator : TControlGenerator
    {
        public FieldSetGenerator()
            : base("pnl", "fieldset")
        {
        }
    }
    public class ButtonGenerator : TControlGenerator
    {
        public ButtonGenerator()
            : base("btn", "button")
        {
            FControlDefinitionSnippetName = "SUBMITBUTTONDEFINITION";
        }

        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            if (ctrl.HasAttribute("AjaxRequestUrl"))
            {
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "VALIDATIONERRORTITLE", ctrl, ctrl.GetAttribute("ValidationErrorTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "VALIDATIONERRORMESSAGE", ctrl, ctrl.GetAttribute("ValidationErrorMessage"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "SENDINGDATATITLE", ctrl, ctrl.GetAttribute("SendingMessageTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "SENDINGDATAMESSAGE", ctrl, ctrl.GetAttribute("SendingMessage"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTSUCCESSTITLE", ctrl, ctrl.GetAttribute("SuccessMessageTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTSUCCESSMESSAGE", ctrl, ctrl.GetAttribute("SuccessMessage"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTFAILURETITLE", ctrl, ctrl.GetAttribute("FailureMessageTitle"));
                ((TExtJsFormsWriter)writer).AddResourceString(ctrlSnippet, "REQUESTFAILUREMESSAGE", ctrl, ctrl.GetAttribute("FailureMessage"));
            }

            ctrlSnippet.SetCodelet("REQUESTURL", ctrl.GetAttribute("AjaxRequestUrl"));

            XmlNode AjaxParametersNode = TXMLParser.GetChild(ctrl.xmlNode, "AjaxRequestParameters");

            if (AjaxParametersNode != null)
            {
                string ParameterString = String.Empty;

                foreach (XmlAttribute attr in AjaxParametersNode.Attributes)
                {
                    ParameterString += attr.Name + ": '" + attr.Value + "', ";
                }

                ctrlSnippet.SetCodelet("REQUESTPARAMETERS", ParameterString);
            }

            return ctrlSnippet;
        }
    }
    public class CheckboxGenerator : TControlGenerator
    {
        public CheckboxGenerator()
            : base("chk", "checkbox")
        {
            FControlDefinitionSnippetName = "CHECKBOXDEFINITION";
        }
    }
    public class RadioButtonGenerator : TControlGenerator
    {
        public RadioButtonGenerator()
            : base("rbt", "radio")
        {
            FControlDefinitionSnippetName = "CHECKBOXDEFINITION";
        }

        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ctrl);

            if (TXMLParser.HasAttribute(ctrl.xmlNode, "RadioChecked"))
            {
                ctrlSnippet.SetCodelet("CHECKED", "true");
            }

            ctrlSnippet.SetCodelet("BOXLABEL", ctrlSnippet.FCodelets["LABEL"].ToString());
            ctrlSnippet.SetCodelet("LABEL", "''");

            return ctrlSnippet;
        }
    }
    public class DateTimePickerGenerator : TControlGenerator
    {
        public DateTimePickerGenerator()
            : base("dtp", "datefield")
        {
            FDefaultWidth = -1;
        }

        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ACtrl)
        {
            ProcessTemplate ctrlSnippet = base.SetControlProperties(writer, ACtrl);

            // TODO: adjust date format to localisation? see http://dev.sencha.com/deploy/dev/docs/?class=Date
            ctrlSnippet.SetCodelet("CUSTOMATTRIBUTES", "format: 'd.m.Y'," +
                Environment.NewLine +
                "boxMaxWidth: 175,");

            return ctrlSnippet;
        }
    }

    public class CompositeGenerator : GroupBoxBaseGenerator
    {
        public CompositeGenerator()
            : base("cmp")
        {
            FControlDefinitionSnippetName = "COMPOSITEDEFINITION";
        }
    }

    public class GroupBoxGenerator : GroupBoxBaseGenerator
    {
        public GroupBoxGenerator()
            : base("grp")
        {
            FControlDefinitionSnippetName = "GROUPBOXDEFINITION";
        }
    }

    public class RadioGroupSimpleGenerator : GroupBoxBaseGenerator
    {
        public RadioGroupSimpleGenerator()
            : base("rgr")
        {
            FControlDefinitionSnippetName = "RADIOGROUPDEFINITION";
        }

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
}