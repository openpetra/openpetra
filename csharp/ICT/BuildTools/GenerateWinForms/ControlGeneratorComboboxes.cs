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
    /// generator for an auto complete combobox
    /// </summary>
    public class TcmbAutoCompleteGenerator : ComboBoxGenerator
    {
        /// <summary>constructor</summary>
        public TcmbAutoCompleteGenerator()
            : base("cmb", "Ict.Common.Controls.TCmbAutoComplete")
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                return TYml2Xml.HasAttribute(curNode, "AutoComplete");
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (ctrl.GetAttribute("AutoComplete").EndsWith("History"))
            {
                writer.SetControlProperty(ctrl, "AcceptNewValues", "true");
                writer.SetEventHandlerToControl(ctrl.controlName,
                    "AcceptNewEntries",
                    "TAcceptNewEntryEventHandler",
                    "FPetraUtilsObject.AddComboBoxHistory");
                writer.CallControlFunction(ctrl.controlName, "SetDataSourceStringList(\"\")");
                writer.Template.AddToCodelet("INITUSERCONTROLS",
                    "FPetraUtilsObject.LoadComboBoxHistory(" + ctrl.controlName + ");" + Environment.NewLine);
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for an auto populated combobox
    /// </summary>
    public class TcmbAutoPopulatedGenerator : ComboBoxGenerator
    {
        /// <summary>constructor</summary>
        public TcmbAutoPopulatedGenerator()
            : base("cmb", "Ict.Petra.Client.CommonControls.TCmbAutoPopulated")
        {
            this.FDefaultWidth = 300;
            this.FChangeEventName = "SelectedValueChanged";
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                return TYml2Xml.HasAttribute(curNode, "List");
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl, "ListTable", "TCmbAutoPopulated.TListTableEnum." + ctrl.GetAttribute("List"));

            if (ctrl.GetAttribute("List") != "UserDefinedList")
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS", ctrl.controlName + ".InitialiseUserControl();" + Environment.NewLine);

                if (ctrl.HasAttribute("AllowDbNull"))
                {
                    writer.SetControlProperty(ctrl, "AllowDbNull", ctrl.GetAttribute("AllowDbNull"));
                }

                if (ctrl.HasAttribute("NullValueDesciption"))
                {
                    writer.SetControlProperty(ctrl, "NullValueDesciption", "\"" + ctrl.GetAttribute("NullValueDesciption") + "\"");
                }
            }
            else
            {
                // user defined lists have to be either filled in manual code
                // eg UC_GLJournals.ManualCode.cs, BeforeShowDetailsManual
                // or UC_GLTransactions.ManualCode.cs, LoadTransactions
            }

            if (ctrl.HasAttribute("ComboBoxWidth"))
            {
                writer.SetControlProperty(ctrl, "ComboBoxWidth", ctrl.GetAttribute("ComboBoxWidth"));
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a versatile combobox
    /// </summary>
    public class TCmbVersatileGenerator : ComboBoxGenerator
    {
        /// <summary>constructor</summary>
        public TCmbVersatileGenerator()
            : base("cmb", "Ict.Common.Controls.TCmbVersatile")
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                return TYml2Xml.HasAttribute(curNode, "MultiColumn");
            }

            return false;
        }
    }

    /// <summary>
    /// generator for a simple combobox
    /// </summary>
    public class ComboBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ComboBoxGenerator()
            : base("cmb", "Ict.Common.Controls.TCmbAutoComplete")
        {
            this.FChangeEventName = "SelectedValueChanged";
            FDefaultHeight = 22;
        }

        /// <summary>constructor</summary>
        public ComboBoxGenerator(string APrefix, string AType)
            : base(APrefix, AType)
        {
            this.FChangeEventName = "SelectedValueChanged";
            FDefaultHeight = 22;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return !TYml2Xml.HasAttribute(curNode, "List")
                       && !TYml2Xml.HasAttribute(curNode, "AutoComplete")
                       && !TYml2Xml.HasAttribute(curNode, "MultiColumn");
            }

            return false;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".SelectedIndex = -1;";
            }

            if (AFieldTypeDotNet == "Boolean")
            {
                return ctrl.controlName + ".SelectedIndex = (" + AFieldOrNull + "?1:0);";
            }

            if (AFieldTypeDotNet == "String")
            {
                return ctrl.controlName + ".SetSelected" + AFieldTypeDotNet + "(" + AFieldOrNull + ", -1);";
            }

            return ctrl.controlName + ".SetSelected" + AFieldTypeDotNet + "(" + AFieldOrNull + ");";
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == "Boolean")
            {
                return ctrl.controlName + ".SelectedIndex = (((bool)" + AFieldOrNull + ") ? 1 :0);";
            }

            if (AFieldTypeDotNet == "String")
            {
                return ctrl.controlName + ".SetSelectedString((" + AFieldTypeDotNet + ")" + AFieldOrNull + ", -1);";
            }

            return ctrl.controlName + ".SetSelected" + AFieldTypeDotNet + "((" + AFieldTypeDotNet + ")" + AFieldOrNull + ");";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".SelectedIndex == -1";
            }

            if (AFieldTypeDotNet == "Boolean")
            {
                return ctrl.controlName + ".SelectedIndex == 1";
            }

            return ctrl.controlName + ".GetSelected" + AFieldTypeDotNet + "()";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            StringCollection OptionalValues = TYml2Xml.GetElements(ctrl.xmlNode, "OptionalValues");

            if (OptionalValues.Count > 0)
            {
                string formattedValues = "";
                string defaultValue = "";

                foreach (string value in OptionalValues)
                {
                    if (formattedValues.Length > 0)
                    {
                        formattedValues += ",";
                    }

                    if (value.StartsWith("="))
                    {
                        formattedValues += "\"" + value.Substring(1).Trim() + "\"";
                        defaultValue = value.Substring(1).Trim();
                    }
                    else
                    {
                        formattedValues += "\"" + value + "\"";
                    }
                }

                writer.CallControlFunction(ctrl.controlName, "Items.AddRange(new object[] {" + formattedValues + "})");

                if (defaultValue.Length > 0)
                {
                    writer.SetControlProperty(ctrl, "Text", "\"" + defaultValue + "\"");
                }
            }

            return writer.FTemplate;
        }
    }
}