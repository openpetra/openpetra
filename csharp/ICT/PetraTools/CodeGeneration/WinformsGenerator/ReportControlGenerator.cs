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
using System.Xml;
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Tools.CodeGeneration;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common;

//using Ict.Petra.Client.CommonControls;

namespace Ict.Tools.CodeGeneration.Winforms
{
    public class ReportControls
    {
        public static string GetParameterName(XmlNode curNode)
        {
            if (TYml2Xml.HasAttribute(curNode, "NoParameter") && (TYml2Xml.GetAttribute(curNode, "NoParameter").ToLower() == "true"))
            {
                return null;
            }

            string result = "param_" + curNode.Name;

            if (TYml2Xml.HasAttribute(curNode, "ParameterName"))
            {
                result = TYml2Xml.GetAttribute(curNode, "ParameterName");
            }

            return result;
        }

        public static void GenerateReadSetControls(TFormWriter writer, XmlNode curNode, ProcessTemplate ATargetTemplate, string ATemplateControlType)
        {
            string controlName = curNode.Name;

            // check if this control is already part of an optional group of controls depending on a radiobutton
            TControlDef ctrl = writer.CodeStorage.GetControl(controlName);

            if (ctrl.GetAttribute("DependsOnRadioButton") == "true")
            {
                return;
            }

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            ProcessTemplate snippetReadControls = writer.Template.GetSnippet(ATemplateControlType + "READCONTROLS");
            snippetReadControls.SetCodelet("CONTROLNAME", controlName);
            snippetReadControls.SetCodelet("PARAMNAME", paramName);
            ATargetTemplate.InsertSnippet("READCONTROLS", snippetReadControls);

            ProcessTemplate snippetWriteControls = writer.Template.GetSnippet(ATemplateControlType + "SETCONTROLS");
            snippetWriteControls.SetCodelet("CONTROLNAME", controlName);
            snippetWriteControls.SetCodelet("PARAMNAME", paramName);
            ATargetTemplate.InsertSnippet("SETCONTROLS", snippetWriteControls);
        }
    }
    public class TcmbAutoPopulatedReportGenerator : TcmbAutoPopulatedGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            ReportControls.GenerateReadSetControls(writer, curNode, writer.Template, "TCMBAUTOPOPULATED");
        }
    }
    public class ComboBoxReportGenerator : ComboBoxGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            ReportControls.GenerateReadSetControls(writer, curNode, writer.Template, "COMBOBOX");
        }
    }

    public class CheckBoxReportGenerator : CheckBoxGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            ReportControls.GenerateReadSetControls(writer, curNode, writer.Template, "CHECKBOX");
        }
    }
    public class TextBoxReportGenerator : TextBoxGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            ReportControls.GenerateReadSetControls(writer, curNode, writer.Template, "TEXTBOX");
        }
    }

    public class TClbVersatileReportGenerator : TClbVersatileGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            ReportControls.GenerateReadSetControls(writer, curNode, writer.Template, "TCLBVERSATILE");
        }
    }

    public class DateTimePickerReportGenerator : DateTimePickerGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            ReportControls.GenerateReadSetControls(writer, curNode, writer.Template, "TTXTPETRADATE");
        }
    }

    public class RadioGroupSimpleReportGenerator : RadioGroupSimpleGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "OptionalValues"));

            foreach (string rbtValueText in optionalValues)
            {
                string rbtValue = StringHelper.UpperCamelCase(rbtValueText.Replace("'", "").Replace(" ", "_"), false, false);
                string rbtName = "rbt" + rbtValue;
                writer.Template.AddToCodelet("READCONTROLS",
                    "if (" + rbtName + ".Checked) " + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "  ACalc.AddParameter(\"" + paramName + "\", \"" + rbtValue + "\");" + Environment.NewLine +
                    "}" + Environment.NewLine);
                writer.Template.AddToCodelet("SETCONTROLS",
                    rbtName + ".Checked = " +
                    "AParameters.Get(\"" + paramName + "\").ToString() == \"" + rbtValue + "\";" +
                    Environment.NewLine);
            }
        }
    }

    public class RadioGroupComplexReportGenerator : RadioGroupComplexGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection Controls =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));

            foreach (string controlName in Controls)
            {
                TControlDef rbtCtrl = writer.CodeStorage.GetControl(controlName);
                string rbtValue = rbtCtrl.Label;
                rbtValue = StringHelper.UpperCamelCase(rbtValue.Replace("'", "").Replace(" ", "_"), false, false);

                if (rbtCtrl.HasAttribute("ParameterValue"))
                {
                    rbtValue = rbtCtrl.GetAttribute("ParameterValue");
                }

                string rbtName = "rbt" + controlName.Substring(3);

                if (controlName.StartsWith("tableLayoutPanel"))
                {
                    // the table layouts of sub controls for each radio button need to be skipped
                    continue;
                }

                ProcessTemplate RadioButtonReadControlsSnippet = writer.Template.GetSnippet("RADIOBUTTONREADCONTROLS");
                RadioButtonReadControlsSnippet.SetCodelet("RBTNAME", rbtName);
                RadioButtonReadControlsSnippet.SetCodelet("PARAMNAME", paramName);
                RadioButtonReadControlsSnippet.SetCodelet("RBTVALUE", rbtValue);
                RadioButtonReadControlsSnippet.SetCodelet("READCONTROLS", "");

                XmlNode childControls = TXMLParser.GetChild(rbtCtrl.xmlNode, "Controls");

                // only assign variables that make sense
                if (childControls != null)
                {
                    StringCollection childControlNames = TYml2Xml.GetElements(childControls);

                    foreach (string childName in childControlNames)
                    {
                        TControlDef childCtrl = writer.CodeStorage.GetControl(childName);
                        IControlGenerator generator = writer.FindControlGenerator(childCtrl);

                        // make sure we ignore Button etc
                        if (generator.GetType().ToString().EndsWith("ReportGenerator"))
                        {
                            childCtrl.SetAttribute("DependsOnRadioButton", "");
                            ReportControls.GenerateReadSetControls(writer,
                                childCtrl.xmlNode,
                                RadioButtonReadControlsSnippet,
                                generator.TemplateSnippetName);
                            childCtrl.SetAttribute("DependsOnRadioButton", "true");
                        }
                    }
                }

                writer.Template.InsertSnippet("READCONTROLS", RadioButtonReadControlsSnippet);

                ProcessTemplate RadioButtonSetControlsSnippet = writer.Template.GetSnippet("RADIOBUTTONSETCONTROLS");
                RadioButtonSetControlsSnippet.SetCodelet("RBTNAME", rbtName);
                RadioButtonSetControlsSnippet.SetCodelet("PARAMNAME", paramName);
                RadioButtonSetControlsSnippet.SetCodelet("RBTVALUE", rbtValue);

                // only assign variables that make sense
                if (childControls != null)
                {
                    StringCollection childControlNames = TYml2Xml.GetElements(childControls);

                    foreach (string childName in childControlNames)
                    {
                        TControlDef childCtrl = writer.CodeStorage.GetControl(childName);
                        IControlGenerator generator = writer.FindControlGenerator(childCtrl);

                        // make sure we ignore Button etc
                        if (generator.GetType().ToString().EndsWith("ReportGenerator"))
                        {
                            childCtrl.SetAttribute("DependsOnRadioButton", "");
                            ReportControls.GenerateReadSetControls(writer,
                                childCtrl.xmlNode,
                                RadioButtonSetControlsSnippet,
                                generator.TemplateSnippetName);
                            childCtrl.SetAttribute("DependsOnRadioButton", "true");
                        }
                    }
                }

                writer.Template.InsertSnippet("SETCONTROLS", RadioButtonSetControlsSnippet);
            }
        }
    }

    public class RadioButtonReportGenerator : RadioButtonGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            // no writing or reading of parameters, should be done in RadioGroup
        }
    }

    public class UserControlReportGenerator : UserControlGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            writer.Template.AddToCodelet("INITIALISESCREEN",
                controlName + ".InitialiseData(FPetraUtilsObject);" +
                Environment.NewLine);

            writer.Template.AddToCodelet("READCONTROLS",
                controlName + ".ReadControls(ACalc, AReportAction);" +
                Environment.NewLine);

            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + ".SetControls(AParameters);" +
                Environment.NewLine);

            writer.Template.AddToCodelet("SETAVAILABLEFUNCTIONS",
                controlName + ".SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());" +
                Environment.NewLine);
        }
    }

    public class SourceGridReportGenerator : SourceGridGenerator
    {
        public override void ApplyDerivedFunctionality(TFormWriter writer, XmlNode curNode)
        {
            string controlName = curNode.Name;

            writer.Template.AddToCodelet("INITIALISESCREEN",
                controlName + "_InitialiseData(FPetraUtilsObject);" +
                Environment.NewLine);

            writer.Template.AddToCodelet("READCONTROLS",
                controlName + "_ReadControls(ACalc, AReportAction);" +
                Environment.NewLine);

            writer.Template.AddToCodelet("SETCONTROLS",
                controlName + "_SetControls(AParameters);" +
                Environment.NewLine);

//            writer.Template.AddToCodelet("SETAVAILABLEFUNCTIONS",
//                controlName + "_SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());" +
//                Environment.NewLine);
        }
    }
}