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
using System.Xml;
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// <summary>
    /// generator for the controls on a report parameter screen
    /// </summary>
    public class ReportControls
    {
        /// <summary>
        /// form the name of a parameter
        /// </summary>
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

        /// <summary>
        /// give information if a value should be cleared if there is no value in the settings file
        /// </summary>
        public static bool GetClearIfSettingEmpty(XmlNode curNode)
        {
            if (TYml2Xml.HasAttribute(curNode, "ClearIfSettingEmpty"))
            {
                if (TYml2Xml.GetAttribute(curNode, "ClearIfSettingEmpty").ToLower() == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// write the code for reading and writing the controls with the parameters
        /// </summary>
        public static void GenerateReadSetControls(TFormWriter writer, XmlNode curNode, ProcessTemplate ATargetTemplate, string ATemplateControlType)
        {
            string controlName = curNode.Name;

            // check if this control is already part of an optional group of controls depending on a radiobutton
            TControlDef ctrl = writer.CodeStorage.GetControl(controlName);

            if (ctrl.GetAttribute("DependsOnRadioButton") == "true")
            {
                return;
            }

            if (ctrl.GetAttribute("NoParameter") == "true")
            {
                return;
            }

            string paramName = ReportControls.GetParameterName(curNode);

            if (paramName == null)
            {
                return;
            }

            bool clearIfSettingEmpty = ReportControls.GetClearIfSettingEmpty(curNode);

            ProcessTemplate snippetReadControls = writer.Template.GetSnippet(ATemplateControlType + "READCONTROLS");
            snippetReadControls.SetCodelet("CONTROLNAME", controlName);
            snippetReadControls.SetCodelet("PARAMNAME", paramName);
            ATargetTemplate.InsertSnippet("READCONTROLS", snippetReadControls);

            ProcessTemplate snippetWriteControls = writer.Template.GetSnippet(ATemplateControlType + "SETCONTROLS");
            snippetWriteControls.SetCodelet("CONTROLNAME", controlName);
            snippetWriteControls.SetCodelet("PARAMNAME", paramName);

            if (clearIfSettingEmpty)
            {
                snippetWriteControls.SetCodelet("CLEARIFSETTINGEMPTY", clearIfSettingEmpty.ToString().ToLower());
            }

            ATargetTemplate.InsertSnippet("SETCONTROLS", snippetWriteControls);
        }
    }

    /// <summary>
    /// generator for auto populated comboboxes
    /// </summary>
    public class TcmbAutoPopulatedReportGenerator : TcmbAutoPopulatedGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "TCMBAUTOPOPULATED");
        }
    }

    /// <summary>
    /// generator for combobox
    /// </summary>
    public class ComboBoxReportGenerator : ComboBoxGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "COMBOBOX");
        }
    }

    /// <summary>
    /// generator for checkbox
    /// </summary>
    public class CheckBoxReportGenerator : CheckBoxGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef ctrl)
        {
            ReportControls.GenerateReadSetControls(writer, ctrl.xmlNode, writer.Template, "CHECKBOX");

            // we are not doing it the same way as in RadioGroupComplexReportGenerator.
            // difference here: we will always store the value of the dependant controls, even when the checkbox is not ticked
            foreach (TControlDef childCtrl in ctrl.Children)
            {
                childCtrl.SetAttribute("DependsOnRadioButton", "");
                IControlGenerator ctrlGenerator = writer.FindControlGenerator(childCtrl);
                ctrlGenerator.ApplyDerivedFunctionality(writer, childCtrl);
                childCtrl.SetAttribute("DependsOnRadioButton", "true");
            }
        }
    }

    /// <summary>
    /// generator for textbox
    /// </summary>
    public class TextBoxReportGenerator : TextBoxGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "TEXTBOX");
        }
    }

    /// <summary>
    /// generator for numeric textbox
    /// </summary>
    public class TTxtNumericTextBoxReportGenerator : TTxtNumericTextBoxGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            TControlDef ctrl = writer.CodeStorage.GetControl(control.xmlNode.Name);
            string numericType = ctrl.GetAttribute("Format");

            if (numericType == "Integer")
            {
                ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "INTEGERTEXTBOX");
            }
            else if (numericType == "Decimal")
            {
                ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "DECIMALTEXTBOX");
            }
        }
    }

    /// <summary>
    /// generator for currency textbox
    /// </summary>
    public class TTxtCurrencyTextBoxReportGenerator : TTxtCurrencyTextBoxGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            TControlDef ctrl = writer.CodeStorage.GetControl(control.xmlNode.Name);
            string numericType = ctrl.GetAttribute("Format");

            if (numericType == "Currency")
            {
                ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "DECIMALTEXTBOX");
            }
        }
    }

    /// <summary>
    /// generator for versatile checked listbox
    /// </summary>
    public class TClbVersatileReportGenerator : TClbVersatileGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "TCLBVERSATILE");
        }
    }

    /// <summary>
    /// generator for date picker
    /// </summary>
    public class DateTimePickerReportGenerator : DateTimePickerGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            ReportControls.GenerateReadSetControls(writer, control.xmlNode, writer.Template, "TTXTPETRADATE");
        }
    }

    /// <summary>
    /// Generator for a group with no border
    /// </summary>
    public class RadioGroupNoBorderReportGenerator : RadioGroupNoBorderGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            string paramName = ReportControls.GetParameterName(control.xmlNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(control.xmlNode, "OptionalValues"));

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

    /// <summary>
    /// generator for simple radio group, values are defined as strings only
    /// </summary>
    public class RadioGroupSimpleReportGenerator : RadioGroupSimpleGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            string paramName = ReportControls.GetParameterName(control.xmlNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(control.xmlNode, "OptionalValues"));

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

    /// <summary>
    /// generator for complex radio group, selected options can contain more controls
    /// </summary>
    public class RadioGroupComplexReportGenerator : RadioGroupComplexGenerator
    {
        /// <summary>add GeneratedReadSetControls, and all dependent controls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            string paramName = ReportControls.GetParameterName(control.xmlNode);

            if (paramName == null)
            {
                return;
            }

            StringCollection Controls =
                TYml2Xml.GetElements(TXMLParser.GetChild(control.xmlNode, "Controls"));

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

                if (controlName.StartsWith("layoutPanel"))
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
                        if (childName.StartsWith("layoutPanel"))
                        {
                            continue;
                        }

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
                        if (childName.StartsWith("layoutPanel"))
                        {
                            continue;
                        }

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

    /// <summary>
    /// generator for a single radio button
    /// </summary>
    public class RadioButtonReportGenerator : RadioButtonGenerator
    {
        /// <summary>not needed</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            // no writing or reading of parameters, should be done in RadioGroup
        }
    }

    /// <summary>
    /// generator for a user control
    /// </summary>
    public class UserControlReportGenerator : UserControlGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            string controlName = control.xmlNode.Name;

            writer.Template.AddToCodelet("INITIALISESCREEN",
                controlName + ".InitialiseData(FPetraUtilsObject);" +
                Environment.NewLine);

            if (!(TYml2Xml.HasAttribute(control.xmlNode, "NoParameter") && (TYml2Xml.GetAttribute(control.xmlNode, "NoParameter").ToLower() == "true")))
            {
                writer.Template.AddToCodelet("READCONTROLS",
                    controlName + ".ReadControls(ACalc, AReportAction);" +
                    Environment.NewLine);

                writer.Template.AddToCodelet("SETCONTROLS",
                    controlName + ".SetControls(AParameters);" +
                    Environment.NewLine);
            }

            writer.Template.AddToCodelet("SETAVAILABLEFUNCTIONS",
                controlName + ".SetAvailableFunctions(FPetraUtilsObject.GetAvailableFunctions());" +
                Environment.NewLine);
        }
    }

    /// <summary>
    /// generator for a SourceGrid data grid
    /// </summary>
    public class SourceGridReportGenerator : SourceGridGenerator
    {
        /// <summary>add GeneratedReadSetControls</summary>
        public override void ApplyDerivedFunctionality(TFormWriter writer, TControlDef control)
        {
            string controlName = control.xmlNode.Name;

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