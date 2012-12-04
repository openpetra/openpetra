//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2012 by OM International
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
using System.Windows.Forms;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// <summary>
    /// base class for radio button groups
    /// </summary>
    public class RadioGroupBase : GroupBoxGenerator
    {
        /// <summary>
        /// default button
        /// </summary>
        protected string FDefaultValueRadioButton = String.Empty;
        /// <summary>
        /// there is no default value
        /// </summary>
        protected bool FNoDefaultValue = false;

        /// <summary>constructor</summary>
        public RadioGroupBase()
            : base("rgr")
        {
        }

        /// <summary>constructor</summary>
        public RadioGroupBase(string prefix, System.Type type)
            : base(prefix, type)
        {
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            string IfStatement = String.Empty;
            bool FirstIfStatement = true;

            if (AFieldOrNull == null)
            {
                if (!FNoDefaultValue)
                {
                    IfStatement += "if(ARow.RowState == DataRowState.Added)" + Environment.NewLine + "{" + Environment.NewLine;

                    foreach (TControlDef optBtn in ctrl.Children)
                    {
                        IfStatement += "    " + optBtn.controlName + ".Checked = ";
                        IfStatement += optBtn.controlName == FDefaultValueRadioButton ? "true" : "false";
                        IfStatement += ";" + Environment.NewLine;
                    }

                    IfStatement += "}" + Environment.NewLine + "else" + Environment.NewLine + "{" + Environment.NewLine;

                    foreach (TControlDef optBtn in ctrl.Children)
                    {
                        IfStatement += "    " + optBtn.controlName + ".Checked = false;" + Environment.NewLine;
                    }

                    IfStatement += "}" + Environment.NewLine;
                }
                else
                {
                    foreach (TControlDef optBtn in ctrl.Children)
                    {
                        IfStatement += optBtn.controlName + ".Checked = false;" + Environment.NewLine;
                    }
                }

                return IfStatement;
            }

            foreach (TControlDef optBtn in ctrl.Children)
            {
                if (!FirstIfStatement)
                {
                    IfStatement += Environment.NewLine + "else ";
                }

                IfStatement += "if(" + AFieldOrNull + " == \"" + optBtn.Label + "\")" + Environment.NewLine + "{" + Environment.NewLine;

                foreach (TControlDef optBtnInner in ctrl.Children)
                {
                    IfStatement += "    " + optBtnInner.controlName + ".Checked = ";
                    IfStatement += optBtnInner.controlName == optBtn.controlName ? "true" : "false";
                    IfStatement += ";" + Environment.NewLine;
                }

                IfStatement += "}";

                FirstIfStatement = false;
            }

            return IfStatement;
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            string IfStatement = String.Empty;
            bool FirstIfStatement = true;

            if (AFieldTypeDotNet == null)
            {
                return null;
            }

            foreach (TControlDef optBtn in ctrl.Children)
            {
                if (!FirstIfStatement)
                {
                    IfStatement += " : ";
                }

                IfStatement += optBtn.controlName + ".Checked ? " + optBtn.controlName + ".Text";

                FirstIfStatement = false;
            }

            return IfStatement + " : null";
        }
    }

    /// this is for radiogroup just with several strings in OptionalValues
    public class RadioGroupSimpleGenerator : RadioGroupBase
    {
        /// <summary>constructor</summary>
        public RadioGroupSimpleGenerator()
            : base()
        {
            FChangeEventName = "";
        }

        /// <summary>constructor</summary>
        public RadioGroupSimpleGenerator(string prefix, System.Type type)
            : base(prefix, type)
        {
            FChangeEventName = "";
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TXMLParser.GetChild(curNode, "OptionalValues") != null)
                {
                    return !TYml2Xml.HasAttribute(curNode,
                        "BorderVisible") || TYml2Xml.GetAttribute(curNode, "BorderVisible").ToLower() != "false";
                }
            }

            return false;
        }

        /// <summary>
        /// get the radio buttons
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(ctrl.xmlNode, "OptionalValues"));
            string DefaultValue;

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "NoDefaultValue")
                 && ((TYml2Xml.GetAttribute(ctrl.xmlNode, "NoDefaultValue")) == "true")))
            {
                DefaultValue = String.Empty;
                FNoDefaultValue = true;
            }
            else
            {
                DefaultValue = optionalValues[0];
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                DefaultValue = TYml2Xml.GetAttribute(ctrl.xmlNode, "DefaultValue");
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

            foreach (string optionalValue in optionalValues)
            {
                string radioButtonName = "rbt" +
                                         StringHelper.UpperCamelCase(optionalValue.Replace("'", "").Replace(" ",
                        "_").Replace("&",
                        ""), false, false);
                TControlDef newCtrl = writer.CodeStorage.FindOrCreateControl(radioButtonName, ctrl.controlName);
                newCtrl.Label = optionalValue;

                if (StringHelper.IsSame(DefaultValue, optionalValue))
                {
                    newCtrl.SetAttribute("RadioChecked", "true");
                    FDefaultValueRadioButton = radioButtonName;
                }

                if (TYml2Xml.HasAttribute(ctrl.xmlNode, "SuppressChangeDetection"))
                {
                    newCtrl.SetAttribute("SuppressChangeDetection", TYml2Xml.GetAttribute(ctrl.xmlNode, "SuppressChangeDetection"));
                }

                if (TYml2Xml.HasAttribute(ctrl.xmlNode, "OnChange"))
                {
                    newCtrl.SetAttribute("OnChange", TYml2Xml.GetAttribute(ctrl.xmlNode, "OnChange"));
                }

                ctrl.Children.Add(newCtrl);
            }

            base.ProcessChildren(writer, ctrl);
        }
    }

    /// this is for radiogroup just with several strings in OptionalValues, but no border; uses a panel instead
    public class RadioGroupNoBorderGenerator : RadioGroupSimpleGenerator
    {
        /// <summary>constructor</summary>
        public RadioGroupNoBorderGenerator()
            : base("rgr", typeof(System.Windows.Forms.Panel))
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                if (TYml2Xml.HasAttribute(curNode, "Label"))
                {
                    base.FGenerateLabel = true;
                }

                if (TXMLParser.GetChild(curNode, "Controls") == null)
                {
                    return TYml2Xml.HasAttribute(curNode,
                        "BorderVisible") && TYml2Xml.GetAttribute(curNode, "BorderVisible").ToLower() == "false";
                }
            }

            return false;
        }
    }

    /// this is for radiogroup with all sorts of sub controls
    public class RadioGroupComplexGenerator : RadioGroupBase
    {
        /// <summary>constructor</summary>
        public RadioGroupComplexGenerator() : base()
        {
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
        /// get the radio buttons
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            StringCollection Controls =
                TYml2Xml.GetElements(TXMLParser.GetChild(ctrl.xmlNode, "Controls"));
            string DefaultValue = Controls[0];

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                DefaultValue = TYml2Xml.GetAttribute(ctrl.xmlNode, "DefaultValue");
            }

            foreach (string controlName in Controls)
            {
                TControlDef radioButton = writer.CodeStorage.GetControl(controlName);

                if (StringHelper.IsSame(DefaultValue, controlName))
                {
                    radioButton.SetAttribute("RadioChecked", "true");
                }
            }

            base.ProcessChildren(writer, ctrl);
        }
    }
}