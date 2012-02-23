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
    /// Generator for label
    /// </summary>
    public class LabelGenerator : TControlGenerator
    {
        bool FRightAlign = false;

        /// <summary>
        /// constructor
        /// </summary>
        public LabelGenerator()
            : base("lbl", typeof(Label))
        {
            FAutoSize = true;
            FGenerateLabel = false;
        }

        /// <summary>
        /// should the label be right aligned
        /// </summary>
        public bool RightAlign
        {
            get
            {
                return FRightAlign;
            }

            set
            {
                FRightAlign = true;
            }
        }

        /// <summary>
        /// get the name for the label from the name of the associated control
        /// </summary>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public string CalculateName(string controlName)
        {
            return "lbl" + controlName.Substring(3);
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            string labelText = "";

            if (TXMLParser.HasAttribute(ctrl.xmlNode, "Text"))
            {
                labelText = TXMLParser.GetAttribute(ctrl.xmlNode, "Text");
            }
            else
            {
                labelText = ctrl.Label + ":";
            }

            writer.SetControlProperty(ctrl, "Text", "\"" + labelText + "\"");
            writer.SetControlProperty(ctrl, "Margin", "new System.Windows.Forms.Padding(3, 7, 3, 0)");

            if (FRightAlign)
            {
                writer.SetControlProperty(ctrl, "TextAlign", "System.Drawing.ContentAlignment.TopRight");
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// Generator for label
    /// </summary>
    public class LinkLabelGenerator : TControlGenerator
    {
        /// <summary>
        /// constructor
        /// </summary>
        public LinkLabelGenerator()
            : base("llb", typeof(LinkLabel))
        {
            FAutoSize = true;
            FGenerateLabel = false;
        }

        /// <summary>
        /// get the name for the LinkLabel from the name of the associated control
        /// </summary>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public string CalculateName(string controlName)
        {
            return "llb" + controlName.Substring(3);
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");
            writer.SetControlProperty(ctrl, "Margin", "new System.Windows.Forms.Padding(3, 7, 3, 0)");

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for buttons
    /// </summary>
    public class ButtonGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ButtonGenerator()
            : base("btn", typeof(Button))
        {
            FAutoSize = true;
            FGenerateLabel = false;
            FDefaultWidth = 80;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (ctrl.GetAttribute("AcceptButton").ToLower() == "true")
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS", "this.AcceptButton = " + ctrl.controlName + ";" + Environment.NewLine);
            }

            if (ctrl.HasAttribute("NoLabel") && (ctrl.GetAttribute("NoLabel").ToLower() == "true"))
            {
                writer.SetControlProperty(ctrl, "Text", "\"\"");
            }
            else
            {
                writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for tab page control
    /// </summary>
    public class TabPageGenerator : GroupBoxGenerator
    {
        /// <summary>constructor</summary>
        public TabPageGenerator()
            : base("tpg", typeof(TabPage))
        {
            FAutoSize = true;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            string CntrlNameWithoutPrefix = ctrl.controlName.Substring(3);
            string CntrlVaribleNameWithoutPrefix;
            StringCollection DynamicControlTypes;

            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl, "Dock", "Fill");

            if (ctrl.HasAttribute("ToolTip"))
            {
                writer.SetControlProperty(ctrl, "ToolTipText", "\"" + ctrl.GetAttribute("ToolTip") + "\"");
            }

            #region Dynamic TabPage UserControl loading ('LoadPageDynamically' attribute and 'DynamicControlTypes' Element)

            if (ctrl.HasAttribute("LoadPageDynamically") && (ctrl.GetAttribute("LoadPageDynamically").ToLower() == "true"))
            {
                if ((!ctrl.HasAttribute("DynamicControlType")
                     && (TYml2Xml.GetElements(ctrl.xmlNode, "DynamicControlTypes").Count == 0)))
                {
                    throw new Exception(
                        "TabPage '" + ctrl.controlName +
                        "': Either the 'DynamicControlType' or the 'DynamicControlTypes' property need to be specified if 'LoadPageDynamically' is specified");
                }
                else if (ctrl.HasAttribute("DynamicControlType"))
                {
                    DynamicControlTypes = new StringCollection();
                    DynamicControlTypes.Add(ctrl.GetAttribute("DynamicControlType"));
                }
                else
                {
                    DynamicControlTypes = TYml2Xml.GetElements(ctrl.xmlNode, "DynamicControlTypes");
                }

                ProcessTemplate snippetUserControlInitialisation = writer.Template.GetSnippet("USERCONTROLINITIALISATION");
                ProcessTemplate snippetUserControlSetupMethod = writer.Template.GetSnippet("DYNAMICTABPAGEUSERCONTROLSETUPMETHOD");

                for (int Counter = 0; Counter < DynamicControlTypes.Count; Counter = Counter + 2)
                {
                    if (DynamicControlTypes.Count == 1)
                    {
                        CntrlVaribleNameWithoutPrefix = CntrlNameWithoutPrefix;
                    }
                    else
                    {
                        CntrlVaribleNameWithoutPrefix = CntrlNameWithoutPrefix + DynamicControlTypes[Counter + 1];
                    }

//Console.WriteLine("CntrlVaribleNameWithoutPrefix: " + CntrlVaribleNameWithoutPrefix + "; Counter: " + Counter.ToString());
                    writer.Template.AddToCodelet("DYNAMICTABPAGEUSERCONTROLDECLARATION",
                        "private " + DynamicControlTypes[Counter] + " FUco" + CntrlVaribleNameWithoutPrefix + ";" + Environment.NewLine);

                    // Declare an Enum for each dynamically loaded TabPage
                    string DynamicTabPageEnums = "";
                    DynamicTabPageEnums += "///<summary>Denotes dynamic loadable UserControl" + " FUco" + CntrlVaribleNameWithoutPrefix +
                                           "</summary>" + Environment.NewLine;
                    DynamicTabPageEnums += "dluc" + CntrlVaribleNameWithoutPrefix + "," + Environment.NewLine;
                    writer.Template.AddToCodelet("DYNAMICTABPAGEUSERCONTROLENUM", DynamicTabPageEnums);

                    // Dispose UserControl for each dynamically loaded TabPage
                    string CustomDisposingOfControl = "";
                    CustomDisposingOfControl += "if (FUco" + CntrlVaribleNameWithoutPrefix + " != null)" + Environment.NewLine;
                    CustomDisposingOfControl += "{" + Environment.NewLine;
                    CustomDisposingOfControl += "    FUco" + CntrlVaribleNameWithoutPrefix + ".Dispose();" + Environment.NewLine;
                    CustomDisposingOfControl += "}" + Environment.NewLine;
                    writer.Template.AddToCodelet("CUSTOMDISPOSING", CustomDisposingOfControl);


                    // Initialise each dynamically loaded TabPage

                    ProcessTemplate snippetUserControlSetup = writer.Template.GetSnippet("DYNAMICTABPAGEUSERCONTROLSETUP");
                    ProcessTemplate snippetTabPageSubseqAct = writer.Template.GetSnippet("DYNAMICTABPAGESUBSEQUENTACTIVATION");

                    if (writer.IsUserControlTemplate)
                    {
                        snippetUserControlSetup.SetCodelet("ISUSERCONTROL", "true");
                    }

                    snippetUserControlInitialisation.SetCodelet("CONTROLNAME", ctrl.controlName);
                    snippetUserControlInitialisation.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);

                    snippetUserControlSetup.SetCodelet("CONTROLNAME", ctrl.controlName);

                    snippetUserControlInitialisation.SetCodelet("TABCONTROLNAME", TabControlGenerator.TabControlName);
                    snippetUserControlSetup.SetCodelet("DYNAMICCONTROLTYPE", DynamicControlTypes[Counter]);

                    ProcessTemplate snippetUserControlLoading = writer.Template.GetSnippet("USERCONTROLLOADING");

                    if ((ctrl.HasAttribute("SeparateDynamicControlSetupMethod")
                         && (ctrl.GetAttribute("SeparateDynamicControlSetupMethod").ToLower() == "true"))
                        || (DynamicControlTypes.Count > 1))
                    {
                        if (DynamicControlTypes.Count == 1)
                        {
                            snippetUserControlInitialisation.SetCodelet("TABKEY", "TDynamicLoadableUserControls.dluc" + CntrlNameWithoutPrefix + ")");
                            snippetUserControlSetupMethod = writer.Template.GetSnippet("DYNAMICTABPAGEUSERCONTROLSETUPMETHOD");
                            snippetUserControlSetup.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                            snippetUserControlSetupMethod.SetCodelet("DYNLOADINFO", "UserControl 'FUco" + CntrlNameWithoutPrefix + "'.");
                            snippetUserControlSetupMethod.SetCodelet("USERORTABCONTROLNAMEWITHOUTPREFIX", "UserControl" + CntrlNameWithoutPrefix);
                            snippetUserControlSetupMethod.InsertSnippet("DYNAMICTABPAGEUSERCONTROLSETUPINLINE2", snippetUserControlSetup);
                            writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLSETUPMETHODS", snippetUserControlSetupMethod);

                            snippetUserControlInitialisation.AddToCodelet("DYNAMICTABPAGEUSERCONTROLSETUPINLINE1",
                                "SetupUserControl" + CntrlNameWithoutPrefix + "();" + Environment.NewLine);

                            snippetTabPageSubseqAct.SetCodelet("CONTROLNAME", ctrl.controlName);
                            snippetTabPageSubseqAct.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                            snippetUserControlInitialisation.InsertSnippet("DYNAMICTABPAGESUBSEQUENTACTIVATION", snippetTabPageSubseqAct);

                            writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLINITIALISATION", snippetUserControlInitialisation);

                            snippetUserControlLoading.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                        }
                        else
                        {
                            snippetUserControlInitialisation.SetCodelet("TABKEY", "Get" + CntrlNameWithoutPrefix + "VariableUC())");
                            ProcessTemplate snippetUserControlSetupMethodMultiUC = writer.Template.GetSnippet(
                                "DYNAMICTABPAGEUSERCONTROLSETUPMULTIUCPART");
                            ProcessTemplate snippetTabPageSubseqActIfStmt = writer.Template.GetSnippet("DYNAMICTABPAGEUSERCONTROLSETUPMULTIUCPART");

                            snippetUserControlSetup.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlVaribleNameWithoutPrefix);
                            snippetUserControlSetupMethod.SetCodelet("DYNLOADINFO", "TabPage '" + ctrl.controlName + "' with varying UserControls.");
                            snippetUserControlSetupMethod.SetCodelet("USERORTABCONTROLNAMEWITHOUTPREFIX",
                                "VariableUserControlForTabPage" + CntrlNameWithoutPrefix);
                            snippetUserControlSetupMethodMultiUC.SetCodelet("TABCONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                            snippetUserControlSetupMethodMultiUC.SetCodelet("USERCONTROLNAMEWITHOUTPREFIX", CntrlVaribleNameWithoutPrefix);

                            snippetUserControlSetupMethodMultiUC.InsertSnippet("MULTIUCCODE", snippetUserControlSetup);

                            if (Counter < DynamicControlTypes.Count - 2)
                            {
                                snippetUserControlSetupMethodMultiUC.SetCodelet("ELSESTATEMENT", "else");
                                snippetTabPageSubseqActIfStmt.SetCodelet("ELSESTATEMENT", "else");
                            }

                            snippetUserControlSetupMethod.InsertSnippet("DYNAMICTABPAGEUSERCONTROLSETUPINLINE2", snippetUserControlSetupMethodMultiUC);

                            snippetTabPageSubseqActIfStmt.SetCodelet("TABCONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                            snippetTabPageSubseqActIfStmt.SetCodelet("USERCONTROLNAMEWITHOUTPREFIX", CntrlVaribleNameWithoutPrefix);

                            snippetTabPageSubseqAct.SetCodelet("CONTROLNAME", ctrl.controlName);
                            snippetTabPageSubseqAct.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlVaribleNameWithoutPrefix);

                            snippetTabPageSubseqActIfStmt.InsertSnippet("MULTIUCCODE", snippetTabPageSubseqAct);
                            snippetUserControlInitialisation.InsertSnippet("DYNAMICTABPAGESUBSEQUENTACTIVATION", snippetTabPageSubseqActIfStmt);

                            if (Counter + 2 == DynamicControlTypes.Count)
                            {
                                writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLSETUPMETHODS", snippetUserControlSetupMethod);
                                snippetUserControlInitialisation.AddToCodelet("DYNAMICTABPAGEUSERCONTROLSETUPINLINE1",
                                    "SetupVariableUserControlForTabPage" + CntrlNameWithoutPrefix + "();" + Environment.NewLine);
                                writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLINITIALISATION", snippetUserControlInitialisation);
                            }

                            snippetUserControlLoading.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlVaribleNameWithoutPrefix);
                        }
                    }
                    else
                    {
                        snippetUserControlInitialisation.SetCodelet("TABKEY", "TDynamicLoadableUserControls.dluc" + CntrlNameWithoutPrefix + ")");
                        snippetUserControlSetup.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                        snippetUserControlInitialisation.InsertSnippet("DYNAMICTABPAGEUSERCONTROLSETUPINLINE1", snippetUserControlSetup);

                        snippetTabPageSubseqAct.SetCodelet("CONTROLNAME", ctrl.controlName);
                        snippetTabPageSubseqAct.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                        snippetUserControlInitialisation.InsertSnippet("DYNAMICTABPAGESUBSEQUENTACTIVATION", snippetTabPageSubseqAct);

                        writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLINITIALISATION", snippetUserControlInitialisation);

                        snippetUserControlLoading.SetCodelet("CONTROLNAMEWITHOUTPREFIX", CntrlNameWithoutPrefix);
                    }

                    // Dynamically load each dynamically loaded TabPage
                    snippetUserControlLoading.SetCodelet("CONTROLNAME", ctrl.controlName);
                    snippetUserControlLoading.SetCodelet("DYNAMICCONTROLTYPE", DynamicControlTypes[Counter]);
                    writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLLOADING", snippetUserControlLoading);
                }
            }

            #endregion

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a single radio button
    /// </summary>
    public class RadioButtonGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public RadioButtonGenerator()
            : base("rbt", typeof(RadioButton))
        {
            FAutoSize = true;
            FGenerateLabel = false;
            FChangeEventName = "CheckedChanged";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            CheckForOtherControls(ctrl);

            base.SetControlProperties(writer, ctrl);

            // Support NoLabel=true
            FGenerateLabel = true;

            if (GenerateLabel(ctrl))
            {
                writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");
            }

            FGenerateLabel = false;

            if (TXMLParser.HasAttribute(ctrl.xmlNode, "RadioChecked"))
            {
                writer.SetControlProperty(ctrl,
                    "Checked",
                    TXMLParser.GetAttribute(ctrl.xmlNode, "RadioChecked"));
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a date picker
    /// </summary>
    public class DateTimePickerGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public DateTimePickerGenerator()
            : base("dtp", "Ict.Petra.Client.CommonControls.TtxtPetraDate")
        {
            this.FChangeEventName = "DateChanged";
            this.FChangeEventHandlerType = "TPetraDateChangedEventHandler";
            FDefaultWidth = 94;
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Date == null";
            }

            if (AFieldTypeDotNet.Contains("Date?"))
            {
                return ctrl.controlName + ".Date";
            }

            return ctrl.controlName + ".Date.Value";
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Date = null;";
            }

            return ctrl.controlName + ".Date = " + AFieldOrNull + ";";
        }
    }

    /// <summary>
    /// generator for a tree view
    /// </summary>
    public class TreeViewGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TreeViewGenerator()
            : base("trv", "Ict.Common.Controls.TTrvTreeView")
        {
        }
    }

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
        }

        /// <summary>constructor</summary>
        public ComboBoxGenerator(string APrefix, string AType)
            : base(APrefix, AType)
        {
            this.FChangeEventName = "SelectedValueChanged";
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

    /// <summary>
    /// generator for a checkbox
    /// </summary>
    public class CheckBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public CheckBoxGenerator()
            : base("chk", typeof(CheckBox))
        {
            this.FChangeEventName = "CheckedChanged";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            CheckForOtherControls(ctrl);

            if ((ctrl.HasAttribute("CheckBoxAttachedLabel"))
                && ((ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "left")
                    || (ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "right")))
            {
                base.FGenerateLabel = false;
                base.FAutoSize = true;

                base.SetControlProperties(writer, ctrl);

                if (ctrl.HasAttribute("NoLabel") && (ctrl.GetAttribute("NoLabel").ToLower() == "true"))
                {
                    writer.SetControlProperty(ctrl, "Text", "\"\"");
                }
                else
                {
                    writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");

                    if (ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "left")
                    {
                        writer.SetControlProperty(ctrl, "CheckAlign", "System.Drawing.ContentAlignment.MiddleRight");
                    }
                    else
                    {
                        writer.SetControlProperty(ctrl, "CheckAlign", "System.Drawing.ContentAlignment.MiddleLeft");
                    }

                    writer.SetControlProperty(ctrl, "Margin", "new System.Windows.Forms.Padding(3, 6, 3, 0)");
                }
            }
            else
            {
                base.FGenerateLabel = true;
                base.FAutoSize = false;
                ctrl.SetAttribute("Width", 30.ToString ());

                base.SetControlProperties(writer, ctrl);

                writer.SetControlProperty(ctrl, "Text", "\"\"");
                writer.SetControlProperty(ctrl, "Margin", "new System.Windows.Forms.Padding(3, 0, 3, 0)");
            }

            return writer.FTemplate;
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Checked = false;";
            }

            return ctrl.controlName + ".Checked = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return null;
            }

            return ctrl.controlName + ".Checked";
        }
    }

    /// <summary>
    /// generator for a versatile checked list box
    /// </summary>
    public class TClbVersatileGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TClbVersatileGenerator()
            : base("clb", "Ict.Common.Controls.TClbVersatile")
        {
            FDefaultHeight = 100;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl, "FixedRows", "1");
            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a print preview control
    /// </summary>
    public class PrintPreviewGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public PrintPreviewGenerator()
            : base("ppv", typeof(PrintPreviewControl))
        {
            FGenerateLabel = false;
        }
    }

    /// this will generate the printpreview with a toolbar for navigating through pages and printing all or specific pages
    public class PrintPreviewWithToolbarGenerator : GroupBoxGenerator
    {
        /// <summary>constructor</summary>
        public PrintPreviewWithToolbarGenerator()
            : base("pre")
        {
        }

        /// <summary>
        /// add adhoc controls for the print preview
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="curNode"></param>
        /// <returns></returns>
        public override StringCollection FindContainedControls(TFormWriter writer, XmlNode curNode)
        {
            // add the toolbar and the print preview control
            TControlDef ctrl = writer.CodeStorage.FindOrCreateControl(curNode.Name, null);

            TControlDef toolbar = writer.CodeStorage.FindOrCreateControl("tbr" + ctrl.controlName.Substring(
                    ctrl.controlTypePrefix.Length), ctrl.controlName);
            TControlDef ttxCurrentPage = writer.CodeStorage.FindOrCreateControl("ttxCurrentPage", toolbar.controlName);

            ttxCurrentPage.SetAttribute("OnChange", "CurrentPageTextChanged");
            writer.CodeStorage.FindOrCreateControl("tblTotalNumberPages", toolbar.controlName);
            TControlDef tbbPrevPage = writer.CodeStorage.FindOrCreateControl("tbbPrevPage", toolbar.controlName);
            tbbPrevPage.SetAttribute("ActionClick", "PrevPageClick");
            TControlDef tbbNextPage = writer.CodeStorage.FindOrCreateControl("tbbNextPage", toolbar.controlName);
            tbbNextPage.SetAttribute("ActionClick", "NextPageClick");
            TControlDef tbbPrintCurrentPage = writer.CodeStorage.FindOrCreateControl("tbbPrintCurrentPage", toolbar.controlName);
            tbbPrintCurrentPage.SetAttribute("ActionClick", "PrintCurrentPage");
            TControlDef tbbPrint = writer.CodeStorage.FindOrCreateControl("tbbPrint", toolbar.controlName);
            tbbPrint.SetAttribute("ActionClick", "PrintAllPages");

            TControlDef printPreview = writer.CodeStorage.FindOrCreateControl("ppv" + ctrl.controlName.Substring(
                    ctrl.controlTypePrefix.Length), ctrl.controlName);
            printPreview.SetAttribute("Dock", "Fill");

            StringCollection Controls = new StringCollection();
            Controls.Add(toolbar.controlName);
            Controls.Add(printPreview.controlName);
            return Controls;
        }
    }

    /// <summary>
    /// generator for a text box
    /// </summary>
    public class TextBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public TextBoxGenerator()
            : base("txt", typeof(TextBox))
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Format") != String.Empty))
                {
                    return false;
                }

                if (TYml2Xml.GetAttribute(curNode, "ReadOnly").ToLower() == "true")
                {
                    if ((TXMLParser.GetAttribute(curNode, "Type") != "PartnerKey"))
                    {
                        return true;
                    }
                }

                if ((TXMLParser.GetAttribute(curNode, "Type") == "PartnerKey")
                    || (TXMLParser.GetAttribute(curNode, "Type") == "Extract")
                    || (TXMLParser.GetAttribute(curNode, "Type") == "Occupation")
                    || (TXMLParser.GetAttribute(curNode, "Type") == "Conference"))
                {
                    return false;
                }

                return true;
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
                return ctrl.controlName + ".Text = String.Empty;";
            }

            if (!AFieldTypeDotNet.ToLower().Contains("string"))
            {
                if (ctrl.GetAttribute("Type") == "PartnerKey")
                {
                    // for readonly text box
                    return ctrl.controlName + ".Text = String.Format(\"{0:0000000000}\", " + AFieldOrNull + ");";
                }

                return ctrl.controlName + ".Text = " + AFieldOrNull + ".ToString();";
            }

            return ctrl.controlName + ".Text = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            if (AFieldTypeDotNet.ToLower().Contains("int64"))
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("int"))
            {
                return "Convert.ToInt32(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".Text)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            CreateCode(writer, ctrl);
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl,
                    "Text",
                    "\"" + TXMLParser.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "Multiline")) && (TXMLParser.GetAttribute(ctrl.xmlNode, "Multiline") == "true"))
            {
                writer.SetControlProperty(ctrl, "Multiline", "true");

                if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "WordWrap")) && (TXMLParser.GetAttribute(ctrl.xmlNode, "WordWrap") == "false"))
                {
                    writer.SetControlProperty(ctrl, "WordWrap", "false");
                }

                if (TYml2Xml.HasAttribute(ctrl.xmlNode, "ScrollBars"))
                {
                    writer.SetControlProperty(ctrl, "ScrollBars", "ScrollBars." + TXMLParser.GetAttribute(ctrl.xmlNode, "ScrollBars"));
                }
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "TextAlign"))
            {
                writer.SetControlProperty(ctrl, "TextAlign", "HorizontalAlignment." + TXMLParser.GetAttribute(ctrl.xmlNode, "TextAlign"));
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "CharacterCasing"))
            {
                writer.SetControlProperty(ctrl, "CharacterCasing", "CharacterCasing." +
                    TXMLParser.GetAttribute(ctrl.xmlNode, "CharacterCasing"));
            }

            if ((TYml2Xml.HasAttribute(ctrl.xmlNode, "PasswordEntry")) && (TXMLParser.GetAttribute(ctrl.xmlNode, "PasswordEntry") == "true"))
            {
                writer.SetControlProperty(ctrl, "UseSystemPasswordChar", "true");
            }

            return writer.FTemplate;
        }

        /// <summary>
        /// generate the control
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ATextControl"></param>
        protected void CreateCode(TFormWriter writer, TControlDef ATextControl)
        {
            writer.Template.AddToCodelet("ASSIGNFONTATTRIBUTES",
                "this." + ATextControl.controlName + ".Font = TAppSettingsManager.GetDefaultBoldFont();" + Environment.NewLine);
        }
    }

    /// <summary>
    /// generator for a control for defining integer values
    /// </summary>
    public class NumericUpDownGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public NumericUpDownGenerator()
            : base("nud", typeof(NumericUpDown))
        {
        }

        /// <summary>
        /// how to assign a value to the control
        /// </summary>
        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Value = 0;";
            }

            return ctrl.controlName + ".Value = " + AFieldOrNull + ";";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                // this control cannot have a null value
                return null;
            }

            return "(" + AFieldTypeDotNet + ")" + ctrl.controlName + ".Value";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "PositiveValueActivates"))
            {
                if (ctrl.HasAttribute("OnChange"))
                {
                    throw new Exception(ctrl.controlName + " cannot have OnChange and PositiveValueActivates at the same time");
                }

                AssignEventHandlerToControl(writer, ctrl, "ValueChanged", ctrl.controlName + "ValueChanged");
                writer.CodeStorage.FEventHandlersImplementation +=
                    "private void " + ctrl.controlName + "ValueChanged" +
                    "(object sender, EventArgs e)" + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "    ActionEnabledEvent(null, new ActionEventArgs(\"" + TYml2Xml.GetAttribute(ctrl.xmlNode, "PositiveValueActivates") +
                    "\", " + ctrl.controlName + ".Value > 0));" + Environment.NewLine +
                    "}" + Environment.NewLine + Environment.NewLine;
            }

            return writer.FTemplate;
        }
    }

    /// generator for the SourceGrid data grid
    public class SourceGridGenerator : TControlGenerator
    {
        Int16 FDecimalPrecision = 2;

        /// <summary>constructor</summary>
        public SourceGridGenerator()
            : base("grd", "Ict.Common.Controls.TSgrdDataGridPaged")
        {
            FGenerateLabel = false;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.GetAttribute(curNode, "Type").ToLower() != "winforms")
                {
                    return true;
                }
            }

            return false;
        }

        private void AddColumnToGrid(TFormWriter writer, string AGridControlName, string AColumnType, string ALabel,
            string ATableName, string AColumnName)
        {
            string ColumnType = "Text";
            string PotentialDecimalPrecision;
            string TrueString = string.Empty;
            string FalseString = string.Empty;

            if (AColumnType.Contains("DateTime"))
            {
                ColumnType = "Date";
            }
            else if (AColumnType.Contains("Currency"))
            {
                ColumnType = "Currency";

                if (AColumnType.Contains("Currency("))
                {
                    PotentialDecimalPrecision = AColumnType.Substring(AColumnType.IndexOf('(') + 1,
                        AColumnType.IndexOf(')') - AColumnType.IndexOf('(') - 1);

//Console.WriteLine("AddColumnToGrid: PotentialDecimalPrecision: " + PotentialDecimalPrecision);

                    if (PotentialDecimalPrecision != String.Empty)
                    {
                        try
                        {
                            FDecimalPrecision = Convert.ToInt16(PotentialDecimalPrecision);
                        }
                        catch (System.FormatException)
                        {
                            throw new ApplicationException(
                                "Grid Column with currency formatting: The specifier for the currency precision '" + PotentialDecimalPrecision +
                                "' is not a number!");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
            else if (AColumnType.Contains("Boolean"))
            {
                if (AColumnType.Contains("("))
                {
                    string BooleanNames = AColumnType.Substring(AColumnType.IndexOf('(') + 1,
                        AColumnType.IndexOf(')') - AColumnType.IndexOf('(') - 1);
                    TrueString = BooleanNames.Split(',')[0];
                    FalseString = BooleanNames.Split(',')[1];
                }

                if ((TrueString.Length > 0) || (FalseString.Length > 0))
                {
                    ColumnType = "Boolean";
                }
                else
                {
                    ColumnType = "CheckBox";
                }
            }

            if (ColumnType == "Boolean")
            {
                writer.Template.AddToCodelet("INITMANUALCODE",
                    AGridControlName + ".Add" + ColumnType + "Column(\"" + ALabel + "\", " +
                    "FMainDS." +
                    ATableName + ".Column" +
                    AColumnName + ", Catalog.GetString(\"" + TrueString + "\"), Catalog.GetString(\"" + FalseString + "\"));" + Environment.NewLine);
            }
            else if ((ColumnType != "Currency")
                     || ((ColumnType == "Currency") && (FDecimalPrecision == 2)))
            {
                writer.Template.AddToCodelet("INITMANUALCODE",
                    AGridControlName + ".Add" + ColumnType + "Column(\"" + ALabel + "\", " +
                    "FMainDS." +
                    ATableName + ".Column" +
                    AColumnName + ");" + Environment.NewLine);
            }
            else
            {
                writer.Template.AddToCodelet("INITMANUALCODE",
                    AGridControlName + ".Add" + ColumnType + "Column(\"" + ALabel + "\", " +
                    "FMainDS." +
                    ATableName + ".Column" +
                    AColumnName + ", " + FDecimalPrecision.ToString() + ");" + Environment.NewLine);
            }
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "SelectedRowActivates"))
            {
                // TODO: this function needs to be called by the manual code at the moment when eg a search finishes
                // TODO: call "Activate" + TYml2Xml.GetAttribute(ctrl.xmlNode, "SelectedRowActivates")
            }

            StringCollection Columns = TYml2Xml.GetElements(ctrl.xmlNode, "Columns");

            if (Columns.Count > 0)
            {
                writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Clear();" + Environment.NewLine);

                foreach (string ColumnFieldName in Columns)
                {
                    bool IsDetailNotMaster;
                    TTableField field = null;
                    string TableFieldTable;
                    string ColumnFieldNameResolved;

                    // customfield, eg. UC_GLTransactions, ATransaction.DateEntered and ATransaction.AnalysisAttributes
                    // there needs to be a list of CustomColumns
                    XmlNode CustomColumnsNode = TYml2Xml.GetChild(ctrl.xmlNode, "CustomColumns");
                    XmlNode CustomColumnNode = null;

                    if (CustomColumnsNode != null)
                    {
                        CustomColumnNode = TYml2Xml.GetChild(CustomColumnsNode, ColumnFieldName);
                    }

                    if ((ctrl.controlName == "grdDetails") && FCodeStorage.HasAttribute("DetailTable"))
                    {
                        TableFieldTable = FCodeStorage.GetAttribute("DetailTable");

                        if (ColumnFieldName.StartsWith("Detail"))
                        {
                            ColumnFieldNameResolved = ColumnFieldName.Substring(6);     // Drop 'Details' out of 'Details...'
                        }
                        else
                        {
                            ColumnFieldNameResolved = ColumnFieldName;
                        }
                    }
                    else
                    {
                        TableFieldTable = ctrl.GetAttribute("TableName");
                        ColumnFieldNameResolved = ColumnFieldName;
                    }

                    if (CustomColumnNode != null)
                    {
                        AddColumnToGrid(writer, ctrl.controlName,
                            TYml2Xml.GetAttribute(CustomColumnNode, "Type"),
                            TYml2Xml.GetAttribute(CustomColumnNode, "Label"),
                            TableFieldTable,
                            ColumnFieldNameResolved);
                    }
                    else if (ctrl.HasAttribute("TableName"))
                    {
                        field =
                            TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + ColumnFieldName, out IsDetailNotMaster,
                                true);
                    }
                    else
                    {
                        field = TDataBinding.GetTableField(null, ColumnFieldName, out IsDetailNotMaster, true);
                    }

                    if (field != null)
                    {
                        AddColumnToGrid(writer, ctrl.controlName,
                            field.iDecimals == 10 && field.iLength == 24 ? "Currency" : field.GetDotNetType(),
                            field.strLabel,
                            TTable.NiceTableName(field.strTableName),
                            TTable.NiceFieldName(field.strName));
                    }
                }
            }

            if (ctrl.HasAttribute("ActionLeavingRow"))
            {
                AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowLeaving", "SourceGrid.RowCancelEventHandler",
                    ctrl.GetAttribute("ActionLeavingRow"));
            }

            if (ctrl.HasAttribute("ActionFocusRow"))
            {
                AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowEntered", "SourceGrid.RowEventHandler",
                    ctrl.GetAttribute("ActionFocusRow"));
            }

            if ((ctrl.controlName == "grdDetails") && FCodeStorage.HasAttribute("DetailTable")
                && FCodeStorage.HasAttribute("DatasetType"))
            {
                writer.Template.AddToCodelet("SHOWDATA", "");

                if (ctrl.HasAttribute("SortOrder"))
                {
                    // SortOrder is comma separated and has DESC or ASC after the column name
                    string SortOrder = ctrl.GetAttribute("SortOrder");

                    foreach (string SortOrderPart in SortOrder.Split(','))
                    {
                        bool temp;
                        TTableField field = null;

                        if ((SortOrderPart.Split(' ')[0].IndexOf(".") == -1) && ctrl.HasAttribute("TableName"))
                        {
                            field = TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + SortOrderPart.Split(
                                    ' ')[0], out temp, true);
                        }
                        else
                        {
                            field =
                                TDataBinding.GetTableField(
                                    null,
                                    SortOrderPart.Split(' ')[0],
                                    out temp, true);
                        }

                        if (field != null)
                        {
                            SortOrder = SortOrder.Replace(SortOrderPart.Split(' ')[0], field.strName);
                        }
                    }

                    writer.Template.AddToCodelet("DETAILTABLESORT", SortOrder);
                }

                if (ctrl.HasAttribute("RowFilter"))
                {
                    // this references a field in the table, and assumes there exists a local variable with the same name
                    // eg. FBatchNumber in GL Journals
                    string RowFilter = ctrl.GetAttribute("RowFilter");

                    String FilterString = "\"";

                    foreach (string RowFilterPart in RowFilter.Split(','))
                    {
                        bool temp;
                        string columnName =
                            TDataBinding.GetTableField(
                                null,
                                RowFilterPart,
                                out temp, true).strName;

                        if (FilterString.Length > 1)
                        {
                            FilterString += " + \" and ";
                        }

                        FilterString += columnName + " = \" + F" + TTable.NiceFieldName(columnName) + ".ToString()";
                    }

                    writer.Template.AddToCodelet("DETAILTABLEFILTER", FilterString);
                }
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for the winforms data grid
    /// </summary>
    public class WinformsGridGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public WinformsGridGenerator()
            : base("grd", typeof(System.Windows.Forms.DataGridView))
        {
            FGenerateLabel = false;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.GetAttribute(curNode, "Type").ToLower() == "winforms")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "SelectedRowActivates"))
            {
                // TODO: this function needs to be called by the manual code at the moment when eg a search finishes
                // TODO: call "Activate" + TYml2Xml.GetAttribute(ctrl.xmlNode, "SelectedRowActivates")
            }

            StringCollection Columns = TYml2Xml.GetElements(ctrl.xmlNode, "Columns");

            if (Columns.Count > 0)
            {
                writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Clear();" + Environment.NewLine);

                foreach (string ColumnFieldName in Columns)
                {
                    bool IsDetailNotMaster;
                    TTableField field = null;

                    // customfield, eg. UC_GLTransactions, ATransaction.DateEntered and ATransaction.AnalysisAttributes
                    // there needs to be a list of CustomColumns
                    XmlNode CustomColumnsNode = TYml2Xml.GetChild(ctrl.xmlNode, "CustomColumns");
                    XmlNode CustomColumnNode = null;

                    if (CustomColumnsNode != null)
                    {
                        CustomColumnNode = TYml2Xml.GetChild(CustomColumnsNode, ColumnFieldName);
                    }

                    if (CustomColumnNode != null)
                    {
                        //string ColumnType = "System.String";

                        /* TODO DateTime (tracker: #58)
                         * if (TYml2Xml.GetAttribute(CustomColumnNode, "Type") == "System.DateTime")
                         * {
                         *  ColumnType = "DateTime";
                         * }
                         */

                        // TODO: different behaviour for double???
                        if (TYml2Xml.GetAttribute(CustomColumnNode, "Type") == "Boolean")
                        {
                            //ColumnType = "CheckBox";
                        }

                        writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Add(" +
                            "FMainDS." + ctrl.GetAttribute("TableName") + ".Get" + ColumnFieldName + "DBName(), \"" +
                            TYml2Xml.GetAttribute(CustomColumnNode, "Label") + "\");" + Environment.NewLine);
                    }
                    else if (ctrl.HasAttribute("TableName"))
                    {
                        field =
                            TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + ColumnFieldName,
                                out IsDetailNotMaster,
                                true);
                    }
                    else
                    {
                        field = TDataBinding.GetTableField(null, ColumnFieldName, out IsDetailNotMaster, true);
                    }

                    if (field != null)
                    {
                        //string ColumnType = "System.String";

                        /* TODO DateTime (tracker: #58)
                         * if (field.GetDotNetType() == "System.DateTime")
                         * {
                         *  ColumnType = "DateTime";
                         * }
                         */

                        // TODO: different behaviour for double???
                        if (field.GetDotNetType() == "Boolean")
                        {
                            //ColumnType = "CheckBox";
                        }

                        writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".Columns.Add(" +
                            TTable.NiceTableName(field.strTableName) + "Table.Get" +
                            TTable.NiceFieldName(field.strName) + "DBName(), \"" +
                            field.strLabel + "\");" + Environment.NewLine);
                    }
                }
            }

            if (ctrl.HasAttribute("ActionLeavingRow"))
            {
                AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowLeaving", "SourceGrid.RowCancelEventHandler",
                    ctrl.GetAttribute("ActionLeavingRow"));
            }

            if (ctrl.HasAttribute("ActionFocusRow"))
            {
// TODO                AssignEventHandlerToControl(writer, ctrl, "Selection.FocusRowEntered", "SourceGrid.RowEventHandler",
//                    ctrl.GetAttribute("ActionFocusRow"));
            }

            if ((ctrl.controlName == "grdDetails") && FCodeStorage.HasAttribute("DetailTable")
                && FCodeStorage.HasAttribute("DatasetType"))
            {
                writer.Template.AddToCodelet("SHOWDATA", "");

                if (ctrl.HasAttribute("SortOrder"))
                {
                    // SortOrder is comma separated and has DESC or ASC after the column name
                    string SortOrder = ctrl.GetAttribute("SortOrder");

                    foreach (string SortOrderPart in SortOrder.Split(','))
                    {
                        bool temp;
                        TTableField field = null;

                        if ((SortOrderPart.Split(' ')[0].IndexOf(".") == -1) && ctrl.HasAttribute("TableName"))
                        {
                            field =
                                TDataBinding.GetTableField(null, ctrl.GetAttribute("TableName") + "." + SortOrderPart.Split(
                                        ' ')[0], out temp, true);
                        }
                        else
                        {
                            field =
                                TDataBinding.GetTableField(
                                    null,
                                    SortOrderPart.Split(' ')[0],
                                    out temp, true);
                        }

                        if (field != null)
                        {
                            SortOrder = SortOrder.Replace(SortOrderPart.Split(' ')[0], field.strName);
                        }
                    }

                    writer.Template.AddToCodelet("DETAILTABLESORT", SortOrder);
                }

                if (ctrl.HasAttribute("RowFilter"))
                {
                    // this references a field in the table, and assumes there exists a local variable with the same name
                    // eg. FBatchNumber in GL Journals
                    string RowFilter = ctrl.GetAttribute("RowFilter");

                    String FilterString = "\"";

                    foreach (string RowFilterPart in RowFilter.Split(','))
                    {
                        bool temp;
                        string columnName =
                            TDataBinding.GetTableField(
                                null,
                                RowFilterPart,
                                out temp, true).strName;

                        if (FilterString.Length > 1)
                        {
                            FilterString += " + \" and ";
                        }

                        FilterString += columnName + " = \" + F" + TTable.NiceFieldName(columnName) + ".ToString()";
                    }

                    writer.Template.AddToCodelet("DETAILTABLEFILTER", FilterString);
                }
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for the control that has an autopopulated text box with a button for a find dialog
    /// </summary>
    public class TTxtAutoPopulatedButtonLabelGenerator : TControlGenerator
    {
        String FButtonLabelType = "";

        /// <summary>constructor</summary>
        public TTxtAutoPopulatedButtonLabelGenerator()
            : base("txt", "Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel")
        {
            this.FChangeEventHandlerType = "TDelegatePartnerChanged";
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if ((TYml2Xml.GetAttribute(curNode, "Format") != String.Empty))
                {
                    return false;
                }

                if (TYml2Xml.GetAttribute(curNode, "Type") == "PartnerKey")
                {
                    FButtonLabelType = "PartnerKey";

                    if (!(TYml2Xml.HasAttribute(curNode,
                              "ShowLabel") && (TYml2Xml.GetAttribute(curNode, "ShowLabel").ToLower() == "false")))
                    {
                        FDefaultWidth = 370;
                    }
                    else
                    {
                        FDefaultWidth = 80;
                    }

                    FHasReadOnlyProperty = true;

                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Extract")
                {
                    FButtonLabelType = "Extract";
                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Occupation")
                {
                    FButtonLabelType = "OccupationList";
                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Conference")
                {
                    FButtonLabelType = "Conference";
                    return true;
                }
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
                return ctrl.controlName + ".Text = String.Empty;";
            }

            return ctrl.controlName + ".Text = String.Format(\"{0:0000000000}\", " + AFieldOrNull + ");";
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            if (AFieldTypeDotNet.ToLower().Contains("int64"))
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("int"))
            {
                return "Convert.ToInt32(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".Text)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            Int32 buttonWidth = 40;
            Int32 textBoxWidth = 80;

            base.SetControlProperties(writer, ctrl);

            if ((ctrl.HasAttribute("ShowLabel") && (ctrl.GetAttribute("ShowLabel").ToLower() == "false")))
            {
                writer.SetControlProperty(ctrl, "ShowLabel", "false");
            }

            // Note: the control defaults to 'ShowLabel' true, so this doesn't need to be set to 'true' in code.

            writer.SetControlProperty(ctrl, "ASpecialSetting", "true");
            writer.SetControlProperty(ctrl, "ButtonTextAlign", "System.Drawing.ContentAlignment.MiddleCenter");
            writer.SetControlProperty(ctrl, "ListTable", "TtxtAutoPopulatedButtonLabel.TListTableEnum." +
                FButtonLabelType);
            writer.SetControlProperty(ctrl, "PartnerClass", "\"" + ctrl.GetAttribute("PartnerClass") + "\"");
            writer.SetControlProperty(ctrl, "MaxLength", "32767");
            writer.SetControlProperty(ctrl, "Tag", "\"CustomDisableAlthoughInvisible\"");
            writer.SetControlProperty(ctrl, "TextBoxWidth", textBoxWidth.ToString());

            if (!(ctrl.HasAttribute("ReadOnly") && (ctrl.GetAttribute("ReadOnly").ToLower() == "true")))
            {
                writer.SetControlProperty(ctrl, "ButtonWidth", buttonWidth.ToString());
                writer.SetControlProperty(ctrl, "ReadOnly", "false");
                writer.SetControlProperty(ctrl,
                    "Font",
                    "new System.Drawing.Font(\"Verdana\", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0)");
                writer.SetControlProperty(ctrl, "ButtonText", "\"Find\"");
            }
            else
            {
                writer.SetControlProperty(ctrl, "ButtonWidth", "0");
                writer.SetControlProperty(ctrl, "BorderStyle", "System.Windows.Forms.BorderStyle.None");
                writer.SetControlProperty(ctrl, "Padding", "new System.Windows.Forms.Padding(0, 2, 0, 0)");
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl,
                    "Text",
                    "\"" + TXMLParser.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a numeric text box control
    /// </summary>
    public class TTxtNumericTextBoxGenerator : TControlGenerator
    {
        string FControlMode;
        Int16 FDecimalPrecision = 2;
        bool FNullValueAllowed = true;

        /// <summary>constructor</summary>
        public TTxtNumericTextBoxGenerator()
            : base("txt", "Ict.Common.Controls.TTxtNumericTextBox")
        {
            FChangeEventName = "TextChanged";
            FHasReadOnlyProperty = true;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            bool ReturnValue = false;
            string NumberFormat;
            string PotentialDecimalPrecision;
            string PotentialNullValue;

//Console.WriteLine("TTxtNumericTextBoxGenerator ControlFitsNode");
            if (base.ControlFitsNode(curNode))
            {
                FDefaultWidth = 80;

                NumberFormat = TYml2Xml.GetAttribute(curNode, "Format");

//Console.WriteLine("TTxtNumericTextBoxGenerator Format: '" + NumberFormat + "'");
                if ((NumberFormat == "Integer")
                    || (NumberFormat == "PercentInteger"))
                {
                    FControlMode = "Integer";

                    ReturnValue = true;
                }

                if (NumberFormat == "LongInteger")
                {
                    FControlMode = "LongInteger";

                    ReturnValue = true;
                }

                if ((NumberFormat == "Decimal")
                    || (NumberFormat == "PercentDecimal")
                    || (NumberFormat.StartsWith("Decimal("))
                    || (NumberFormat.StartsWith("PercentDecimal(")))
                {
                    FControlMode = "Decimal";

                    ReturnValue = true;
                }

                if ((NumberFormat == "Currency")
                    || (NumberFormat.StartsWith("Currency(")))
                {
                    FControlMode = "Currency";
                    FDefaultWidth = 150;
                    ReturnValue = true;
                }

                if (ReturnValue)
                {
                    if ((NumberFormat.StartsWith("Decimal("))
                        || (NumberFormat.StartsWith("PercentDecimal("))
                        || (NumberFormat.StartsWith("Currency(")))
                    {
                        PotentialDecimalPrecision = NumberFormat.Substring(NumberFormat.IndexOf('(') + 1,
                            NumberFormat.IndexOf(')') - NumberFormat.IndexOf('(') - 1);

//Console.WriteLine("TTxtNumericTextBoxGenerator: PotentialDecimalPrecision: " + PotentialDecimalPrecision);
                        if (PotentialDecimalPrecision != String.Empty)
                        {
                            try
                            {
                                FDecimalPrecision = Convert.ToInt16(PotentialDecimalPrecision);
                            }
                            catch (System.FormatException)
                            {
                                throw new ApplicationException(
                                    "TextBox with decimal formatting: The specifier for the decimal precision '" + PotentialDecimalPrecision +
                                    "' is not a number!");
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }

                    if (TYml2Xml.HasAttribute(curNode, "NullValueAllowed"))
                    {
                        PotentialNullValue = TYml2Xml.GetAttribute(curNode, "NullValueAllowed");

                        if ((PotentialNullValue == "true")
                            || (PotentialNullValue == "false"))
                        {
                            FNullValueAllowed = Convert.ToBoolean(PotentialNullValue);
                        }
                        else
                        {
                            throw new ApplicationException(
                                "TextBox with number formatting: Value for 'NullValueAllowed' needs to be either 'true' or 'false', but is '" +
                                PotentialNullValue + "'.");
                        }
                    }
                }

                return ReturnValue;
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
                if ((FControlMode == "Decimal")
                    || (FControlMode == "Currency"))
                {
                    return ctrl.controlName + ".NumberValueDecimal = null;";
                }
                else
                {
                    if (FControlMode == "Integer")
                    {
                        return ctrl.controlName + ".NumberValueInt = null;";
                    }
                    else
                    {
                        return ctrl.controlName + ".NumberValueLongInt = null;";
                    }
                }
            }
            else
            {
                if (AFieldTypeDotNet.ToLower() == "int32")
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueInt = null;";
                    }

                    return ctrl.controlName + ".NumberValueInt = " + AFieldOrNull + ";";
                }
                else if (AFieldTypeDotNet.ToLower() == "int64")
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueLongInt = null;";
                    }

                    return ctrl.controlName + ".NumberValueLongInt = " + AFieldOrNull + ";";
                }
                else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueDecimal = null;";
                    }

                    return ctrl.controlName + ".NumberValueDecimal = Convert.ToDecimal(" + AFieldOrNull + ");";
                }
                else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
                {
                    if (AFieldOrNull == null)
                    {
                        return ctrl.controlName + ".NumberValueDecimal = null;";
                    }

                    return ctrl.controlName + ".NumberValueDecimal = Convert.ToDecimal(" + AFieldOrNull + ");";
                }
                else
                {
                    return "?????";
                }
            }
        }

        /// <summary>
        /// how to get the value from the control
        /// </summary>
        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                if ((FControlMode == "Decimal")
                    || (FControlMode == "Currency"))
                {
                    return ctrl.controlName + ".NumberValueDecimal == null";
                }
                else
                {
                    if (FControlMode == "Integer")
                    {
                        return ctrl.controlName + ".NumberValueInt == null";
                    }
                    else
                    {
                        return ctrl.controlName + ".NumberValueLongInt == null";
                    }
                }
            }

            if (AFieldTypeDotNet.ToLower() == "int64")
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".NumberValueLongInt)";
            }
            else if (AFieldTypeDotNet.ToLower() == "int32")
            {
                return "Convert.ToInt32(" + ctrl.controlName + ".NumberValueInt)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".NumberValueDecimal)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("decimal"))
            {
                return "Convert.ToDecimal(" + ctrl.controlName + ".NumberValueDecimal)";
            }

            return ctrl.controlName + ".Text";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            string NumberFormat = String.Empty;

            base.SetControlProperties(writer, ctrl);

            if ((ctrl.HasAttribute("ShowLabel") && (ctrl.GetAttribute("ShowLabel").ToLower() == "false")))
            {
                writer.SetControlProperty(ctrl, "ShowLabel", "false");
            }

            // Note: the control defaults to 'ShowLabel' true, so this doesn't need to be set to 'true' in code.

            writer.SetControlProperty(ctrl, "ControlMode", "TTxtNumericTextBox.TNumericTextBoxMode." + FControlMode);
            writer.SetControlProperty(ctrl, "DecimalPlaces", FDecimalPrecision.ToString());
            writer.SetControlProperty(ctrl, "NullValueAllowed", FNullValueAllowed.ToString().ToLower());

            if (ctrl.HasAttribute("Format"))
            {
                NumberFormat = ctrl.GetAttribute("Format");
            }

            if ((NumberFormat.StartsWith("PercentInteger"))
                || (NumberFormat.StartsWith("PercentDecimal")))
            {
                writer.SetControlProperty(ctrl, "ShowPercentSign", "true");
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a tab control
    /// </summary>
    public class TabControlGenerator : ContainerGenerator
    {
        static string FTabControlName;

        /// <summary>constructor</summary>
        public TabControlGenerator()
            : base("tab", "Ict.Common.Controls.TTabVersatile")
        {
            FGenerateLabel = false;
        }

        /// <summary>
        /// name of the tab control
        /// </summary>
        public static string TabControlName
        {
            get
            {
                return FTabControlName;
            }
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ProcessTemplate snippetDynamicTabPage = null;
            ProcessTemplate snippetTabPageSelectionChanged = null;
            string IgnoreFirstTabSel = String.Empty;

            CreateCode(writer, ctrl);
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "DrawMode", "System.Windows.Forms.TabDrawMode.OwnerDrawFixed");

            if (ctrl.HasAttribute("DragTabPageEnabled") && (ctrl.GetAttribute("DragTabPageEnabled").ToLower() == "false"))
            {
                writer.SetControlProperty(ctrl, "AllowDrop", "false");
            }

            if (ctrl.HasAttribute("ShowToolTips") && (ctrl.GetAttribute("ShowToolTips").ToLower() == "true"))
            {
                writer.SetControlProperty(ctrl, "ShowToolTips", "true");
            }

            writer.Template.SetCodelet("TABPAGECTRL", ctrl.controlName);

            if (ctrl.HasAttribute("IgnoreFirstTabPageSelectionChange")
                && (ctrl.GetAttribute("IgnoreFirstTabPageSelectionChange").ToLower() == "true"))
            {
                IgnoreFirstTabSel += "if (FirstTabPageSelectionChanged)" + Environment.NewLine;
                IgnoreFirstTabSel += "{" + Environment.NewLine;
                IgnoreFirstTabSel += "    // The first time we run this Method we exit straight away!" + Environment.NewLine;
                IgnoreFirstTabSel += "    return;" + Environment.NewLine;
                IgnoreFirstTabSel += "}" + Environment.NewLine + Environment.NewLine + Environment.NewLine;

                writer.Template.AddToCodelet("IGNOREFIRSTTABPAGESELECTIONCHANGEDEVENT", IgnoreFirstTabSel);
            }

            if (IgnoreFirstTabSel != String.Empty)
            {
                writer.Template.SetCodelet("FIRSTTABPAGESELECTIONCHANGEDVAR", "true");
            }

            if (ctrl.HasAttribute("LoadPagesDynamically") && (ctrl.GetAttribute("LoadPagesDynamically").ToLower() == "true"))
            {
                snippetDynamicTabPage = writer.Template.GetSnippet("DYNAMICTABPAGE");

                if (IgnoreFirstTabSel != String.Empty)
                {
                    snippetDynamicTabPage.SetCodelet("FIRSTTABPAGESELECTIONCHANGEDVAR", "true");
                }

                if (writer.IsUserControlTemplate)
                {
                    snippetDynamicTabPage.SetCodelet("ISUSERCONTROL", "true");
                }

                writer.Template.InsertSnippet("DYNAMICTABPAGEBASICS", snippetDynamicTabPage);

                snippetTabPageSelectionChanged = writer.Template.GetSnippet("TABPAGESELECTIONCHANGED");

                if (writer.IsUserControlTemplate)
                {
                    snippetTabPageSelectionChanged.SetCodelet("ISUSERCONTROL", "true");
                }

                writer.Template.InsertSnippet("DYNAMICTABPAGEUSERCONTROLSELECTIONCHANGED", snippetTabPageSelectionChanged);
            }
            else
            {
                writer.Template.AddToCodelet("DYNAMICTABPAGEUSERCONTROLSELECTIONCHANGED", "");
            }

            // writer.Template.FTemplateCode.Contains is not very clean, since it might be in a snippet or in an ifdef that will not be part of the resulting file
            if ((writer.CodeStorage.ManualFileExistsAndContains("void TabSelectionChanged"))
                || (writer.Template.FTemplateCode.Contains("void TabSelectionChanged"))
                || (snippetDynamicTabPage != null))
            {
                AssignEventHandlerToControl(writer, ctrl, "SelectedIndexChanged", "TabSelectionChanged");

                writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".SelectedIndex = 0;" + Environment.NewLine);
                writer.Template.AddToCodelet("INITMANUALCODE", "TabSelectionChanged(null, null);" + Environment.NewLine);
            }

            return writer.FTemplate;
        }

        /// <summary>
        /// generate the code
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ATabControl"></param>
        protected void CreateCode(TFormWriter writer, TControlDef ATabControl)
        {
            ArrayList tabPages = new ArrayList();

            FTabControlName = ATabControl.controlName;

            // need to save tab pages in a temporary list,
            // because TableLayoutPanelGenerator.CreateLayout will add to the FControlList
            foreach (TControlDef ctrl in ATabControl.FCodeStorage.FSortedControlList.Values)
            {
                if (ctrl.controlTypePrefix == "tpg")
                {
                    tabPages.Add(ctrl);
                    ctrl.parentName = ATabControl.controlName;
                }
            }

            foreach (TControlDef ctrl in tabPages)
            {
                TabPageGenerator tabGenerator = new TabPageGenerator();
                tabGenerator.GenerateDeclaration(writer, ctrl);
                tabGenerator.SetControlProperties(writer, ctrl);
            }
        }
    }

    /// <summary>
    /// generator for a group box
    /// </summary>
    public class GroupBoxGenerator : ContainerGenerator
    {
        /// <summary>constructor</summary>
        public GroupBoxGenerator(string prefix, System.Type type)
            : base(prefix, type)
        {
            FAutoSize = true;
            FGenerateLabel = false;

            if (base.FPrefix == "rng")
            {
                FGenerateLabel = true;
            }
        }

        /// <summary>constructor</summary>
        public GroupBoxGenerator()
            : this("grp", typeof(GroupBox))
        {
        }

        /// <summary>constructor</summary>
        public GroupBoxGenerator(string prefix)
            : this(prefix, typeof(GroupBox))
        {
        }

        /// <summary>
        /// get the children in this group box
        /// </summary>
        public virtual StringCollection FindContainedControls(TFormWriter writer, XmlNode curNode)
        {
            StringCollection controlNamesCollection;
            int Width = 0;
            int Height = 0;

            XmlNode controlsNode = TXMLParser.GetChild(curNode, "Controls");

            if ((controlsNode != null) && TYml2Xml.GetChildren(controlsNode, true)[0].Name.StartsWith("Row"))
            {
                // this defines the layout with several rows with several controls per row
                string result = "";
                Int32 countRow = 0;

                foreach (XmlNode row in TYml2Xml.GetChildren(controlsNode, true))
                {
                    int RowWidth = 0;
                    int RowHeight = 0;
                    StringCollection controls = TYml2Xml.GetElements(row);

                    foreach (string ctrlname in controls)
                    {
                        TControlDef ctrl = writer.CodeStorage.GetControl(ctrlname);

                        if ((ctrl == null)
                            && (!ctrlname.StartsWith("Empty")))
                        {
                            throw new Exception("cannot find control with name " + ctrlname + "; it belongs to " +
                                curNode.Name);
                        }

                        ctrl.rowNumber = countRow;
                        RowWidth += ctrl.Width;

                        if (RowHeight < ctrl.Height)
                        {
                            RowHeight = ctrl.Height;
                        }
                    }

                    result = StringHelper.ConcatCSV(result, StringHelper.StrMerge(controls, ","), ",");
                    countRow++;

                    if (RowWidth > Width)
                    {
                        Width = RowWidth;
                    }

                    Height += RowHeight;
                }

                controlNamesCollection = StringHelper.StrSplit(result, ",");
            }
            else
            {
                controlNamesCollection = TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));

                TableLayoutPanelGenerator.eOrientation orientation = TableLayoutPanelGenerator.eOrientation.Vertical;

                if (TYml2Xml.HasAttribute(curNode, "ControlsOrientation")
                    && (TYml2Xml.GetAttribute(curNode, "ControlsOrientation").ToLower() == "horizontal"))
                {
                    orientation = TableLayoutPanelGenerator.eOrientation.Vertical;
                }

                foreach (string ctrlname in controlNamesCollection)
                {
                    TControlDef ctrl = writer.CodeStorage.GetControl(ctrlname);

                    if ((ctrl == null)
                        && (!ctrlname.StartsWith("Empty")))
                    {
                        throw new Exception("cannot find control with name " + ctrlname + "; it belongs to " +
                            curNode.Name);
                    }

                    if (orientation == TableLayoutPanelGenerator.eOrientation.Horizontal)
                    {
                        Width += ctrl.Width;
                    }
                    else
                    {
                        Height += ctrl.Height;
                    }
                }
            }

            // set the parent control for all children
            foreach (string ctrlname in controlNamesCollection)
            {
                TControlDef ctrl = writer.CodeStorage.GetControl(ctrlname);

                if (ctrl != null)
                {
                    ctrl.parentName = curNode.Name;
                }
                else if (!ctrlname.StartsWith("Empty"))
                {
                    throw new Exception("cannot find control with name " + ctrlname + "; it belongs to " + curNode.Name);
                }
            }

            TXMLParser.SetAttribute(curNode, "Height", Height.ToString());
            TXMLParser.SetAttribute(curNode, "Width", Width.ToString());

            return controlNamesCollection;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            if (ctrl.HasAttribute("Width") && ctrl.HasAttribute("Height"))
            {
                FAutoSize = false;
            }
            else if (ctrl.HasAttribute("Height"))
            {
                // assume width of parent control
                ctrl.SetAttribute("Width", (FCodeStorage.FWidth - 10).ToString());
                FAutoSize = false;
            }
            else if (ctrl.HasAttribute("Width") && (ctrl.GetAttribute("Dock") != "Left")
                     && (ctrl.GetAttribute("Dock") != "Right"))
            {
                throw new Exception(
                    "Control " + ctrl.controlName +
                    " must have both Width and Height attributes, or just Height, but not Width alone");
            }

            StringCollection Controls = FindContainedControls(writer, ctrl.xmlNode);

            base.CreateControlsAddStatements = false;
            base.SetControlProperties(writer, ctrl);

            string ControlName = ctrl.controlName;

            bool UseTableLayout = false;

            // don't use a tablelayout for controls where all children have the Dock property set
            foreach (string ChildControlName in Controls)
            {
                TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);

                if (ChildControl == null)
                {
                    throw new Exception("cannot find definition of child control " + ChildControlName);
                }

                if (!ChildControl.HasAttribute("Dock"))
                {
                    UseTableLayout = true;
                }
            }

            if (!UseTableLayout)
            {
                // first add the control that has Dock=Fill, then the others
                foreach (string ChildControlName in Controls)
                {
                    TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);

                    if (ChildControl.GetAttribute("Dock") == "Fill")
                    {
                        writer.CallControlFunction(ctrl.controlName,
                            "Controls.Add(this." +
                            ChildControlName + ")");
                    }
                }

                StringCollection ControlsReverse = new StringCollection();

                foreach (string ChildControlName in Controls)
                {
                    ControlsReverse.Insert(0, ChildControlName);
                }

                foreach (string ChildControlName in ControlsReverse)
                {
                    TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);

                    if (ChildControl.GetAttribute("Dock") != "Fill")
                    {
                        writer.CallControlFunction(ctrl.controlName,
                            "Controls.Add(this." +
                            ChildControlName + ")");
                    }
                }

                foreach (string ChildControlName in Controls)
                {
                    TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);
                    XmlNode curNode = ChildControl.xmlNode;
                    IControlGenerator ctrlGenerator = writer.FindControlGenerator(ChildControl);

                    // add control itself
                    ctrlGenerator.GenerateDeclaration(writer, ChildControl);
                    ctrlGenerator.SetControlProperties(writer, ChildControl);
                    writer.ApplyDerivedFunctionality(ctrlGenerator, curNode);
                }
            }
            else
            {
                TableLayoutPanelGenerator TlpGenerator = new TableLayoutPanelGenerator();
                TlpGenerator.SetOrientation(ctrl);
                string tlpControlName = TlpGenerator.CreateLayout(writer, ControlName, Controls, -1, -1);
                writer.CallControlFunction(ControlName,
                    "Controls.Add(this." +
                    tlpControlName + ")");

                foreach (string ChildControlName in Controls)
                {
                    TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);
                    TlpGenerator.CreateCode(writer, ChildControl);
                }

                TlpGenerator.WriteTableLayout(writer, tlpControlName);
            }

            if ((base.FPrefix == "grp") || (base.FPrefix == "rgr") || (base.FPrefix == "tpg"))
            {
                FGenerateLabel = true;

                if (GenerateLabel(ctrl))
                {
                    writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");
                }

                FGenerateLabel = false;
            }

            return writer.FTemplate;
        }
    }

    /// this is for radiogroup just with several strings in OptionalValues
    public class RadioGroupSimpleGenerator : GroupBoxGenerator
    {
        string FDefaultValueRadioButton = String.Empty;
        bool FNoDefaultValue = false;

        /// <summary>constructor</summary>
        public RadioGroupSimpleGenerator()
            : base("rgr")
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
        public override StringCollection FindContainedControls(TFormWriter writer, XmlNode curNode)
        {
            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "OptionalValues"));
            string DefaultValue;

            if ((TXMLParser.HasAttribute(curNode, "NoDefaultValue")
                 && ((TXMLParser.GetAttribute(curNode, "NoDefaultValue")) == "true")))
            {
                DefaultValue = String.Empty;
                FNoDefaultValue = true;
            }
            else
            {
                DefaultValue = optionalValues[0];
            }

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
                    FDefaultValueRadioButton = radioButtonName;
                }

                if (TYml2Xml.HasAttribute(curNode, "SuppressChangeDetection"))
                {
                    newCtrl.SetAttribute("SuppressChangeDetection", TYml2Xml.GetAttribute(curNode, "SuppressChangeDetection"));
                }

                if (TYml2Xml.HasAttribute(curNode, "OnChange"))
                {
                    newCtrl.SetAttribute("OnChange", TYml2Xml.GetAttribute(curNode, "OnChange"));
                }

                Controls.Add(radioButtonName);
                this.Children.Add(newCtrl);
            }

            return Controls;
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

                    foreach (TControlDef optBtn in this.Children)
                    {
                        IfStatement += "    " + optBtn.controlName + ".Checked = ";
                        IfStatement += optBtn.controlName == FDefaultValueRadioButton ? "true" : "false";
                        IfStatement += ";" + Environment.NewLine;
                    }

                    IfStatement += "}" + Environment.NewLine + "else" + Environment.NewLine + "{" + Environment.NewLine;

                    foreach (TControlDef optBtn in this.Children)
                    {
                        IfStatement += "    " + optBtn.controlName + ".Checked = false;" + Environment.NewLine;
                    }

                    IfStatement += "}" + Environment.NewLine;
                }
                else
                {
                    foreach (TControlDef optBtn in this.Children)
                    {
                        IfStatement += optBtn.controlName + ".Checked = false;" + Environment.NewLine;
                    }
                }

                return IfStatement;
            }

            foreach (TControlDef optBtn in this.Children)
            {
                if (!FirstIfStatement)
                {
                    IfStatement += Environment.NewLine + "else ";
                }

                IfStatement += "if(" + AFieldOrNull + " == \"" + optBtn.Label + "\")" + Environment.NewLine + "{" + Environment.NewLine;

                foreach (TControlDef optBtnInner in this.Children)
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

            foreach (TControlDef optBtn in this.Children)
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
    public class RadioGroupComplexGenerator : GroupBoxGenerator
    {
        /// <summary>constructor</summary>
        public RadioGroupComplexGenerator()
            : base("rgr")
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

                if (StringHelper.IsSame(DefaultValue, controlName))
                {
                    radioButton.SetAttribute("RadioChecked", "true");
                }
            }

            return Controls;
        }
    }

    /// rng: implemented as a panel
    public class RangeGenerator : GroupBoxGenerator
    {
        /// <summary>constructor</summary>
        public RangeGenerator()
            : base("rng", typeof(Panel))
        {
        }
    }

    /// <summary>
    /// generator for a panel
    /// </summary>
    public class PanelGenerator : GroupBoxGenerator
    {
        /// <summary>constructor</summary>
        public PanelGenerator()
            : base("pnl", typeof(Panel))
        {
        }
    }

    /// <summary>
    /// generator for a splitter (eg. of two panels)
    /// </summary>
    public class SplitContainerGenerator : GroupBoxGenerator
    {
        /// <summary>constructor</summary>
        public SplitContainerGenerator()
            : base("spt", typeof(SplitContainer))
        {
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            writer.AddContainer(ctrl.controlName + ".Panel1");
            writer.AddContainer(ctrl.controlName + ".Panel2");

            base.SetControlProperties(writer, ctrl);

            if (ctrl.HasAttribute("SplitterDistance"))
            {
                writer.SetControlProperty(ctrl, "SplitterDistance");
            }

            if (ctrl.HasAttribute("SplitterOrientation"))
            {
                writer.SetControlProperty(ctrl, "Orientation", "System.Windows.Forms.Orientation." +
                    StringHelper.UpperCamelCase(ctrl.GetAttribute("SplitterOrientation")));
            }

            // add one control for panel1, and one other control for panel2
            // at the moment, only one control is supported per panel of the splitcontainer
            writer.CallControlFunction(ctrl.controlName,
                "Panel1.Controls.Add(this." +
                ctrl.GetAttribute("Panel1") + ")");
            writer.CallControlFunction(ctrl.controlName,
                "Panel2.Controls.Add(this." +
                ctrl.GetAttribute("Panel2") + ")");

            TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(ctrl.GetAttribute("Panel1"));
            ChildCtrl.parentName = ctrl.controlName;
            IControlGenerator ChildGenerator = writer.FindControlGenerator(ChildCtrl);
            ChildGenerator.GenerateDeclaration(writer, ChildCtrl);
            ChildGenerator.SetControlProperties(writer, ChildCtrl);

            ChildCtrl = ctrl.FCodeStorage.GetControl(ctrl.GetAttribute("Panel2"));
            ChildCtrl.parentName = ctrl.controlName;
            ChildGenerator = writer.FindControlGenerator(ChildCtrl);
            ChildGenerator.GenerateDeclaration(writer, ChildCtrl);
            ChildGenerator.SetControlProperties(writer, ChildCtrl);

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for menu items
    /// </summary>
    public class MenuItemGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public MenuItemGenerator(string APrefix, System.Type AType)
            : base(APrefix, AType)
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        /// <summary>constructor</summary>
        public MenuItemGenerator()
            : this("mni", typeof(ToolStripMenuItem))
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return (!curNode.Name.StartsWith("mniSeparator")) && (TYml2Xml.GetAttribute(curNode, "Label") != "-");
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            string controlName = base.FPrefix + ctrl.controlName.Substring(3);

            // add all the children
            string addChildren = ToolStripGenerator.GetListOfChildren(writer, ctrl);

            base.SetControlProperties(writer, ctrl);

            if (addChildren.Length > 0)
            {
                writer.CallControlFunction(controlName,
                    "DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {" + Environment.NewLine +
                    "               " + addChildren +
                    "})");
            }

            // deactivate menu items that have no action assigned yet.
            if ((ctrl.GetAction() == null) && !ctrl.HasAttribute("ActionClick") && !ctrl.HasAttribute("ActionOpenScreen")
                && (ctrl.NumberChildren == 0) && !(this is MenuItemSeparatorGenerator))
            {
                string ActionEnabling = ctrl.controlName + ".Enabled = false;" + Environment.NewLine;
                writer.Template.AddToCodelet("ACTIONENABLINGDISABLEMISSINGFUNCS", ActionEnabling);
            }

            writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");

            // todo: this.toolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for seperators between menu items
    /// </summary>
    public class MenuItemSeparatorGenerator : MenuItemGenerator
    {
        /// <summary>constructor</summary>
        public MenuItemSeparatorGenerator()
            : base("mni", typeof(ToolStripSeparator))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                return !base.ControlFitsNode(curNode);
            }

            return false;
        }
    }

    /// <summary>
    /// generator for a whole menu
    /// </summary>
    public class MenuGenerator : ToolStripGenerator
    {
        /// <summary>constructor</summary>
        public MenuGenerator()
            : base("mnu", typeof(MenuStrip))
        {
        }
    }

    /// <summary>
    /// generator for a status bar
    /// </summary>
    public class StatusBarGenerator : ToolStripGenerator
    {
        /// <summary>constructor</summary>
        public StatusBarGenerator()
            : base("stb", "Ict.Common.Controls.TExtStatusBarHelp")
        {
            FDocking = "Bottom";
        }
    }

    /// <summary>
    /// generator for a tool bar
    /// </summary>
    public class ToolBarGenerator : ToolStripGenerator
    {
        /// <summary>constructor</summary>
        public ToolBarGenerator()
            : base("tbr", typeof(System.Windows.Forms.ToolStrip))
        {
            FRequiresChildren = true;
        }
    }

    /// <summary>
    /// generator for a host of a tool bar
    /// </summary>
    public class ToolbarControlHostGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ToolbarControlHostGenerator()
            : base("tch", typeof(ToolStripControlHost))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        /// <summary>
        /// declare the control
        /// </summary>
        public override void GenerateDeclaration(TFormWriter writer, TControlDef ctrl)
        {
            string hostedControlName = TYml2Xml.GetAttribute(ctrl.xmlNode, "HostedControl");
            TControlDef hostedCtrl = FCodeStorage.FindOrCreateControl(hostedControlName, ctrl.controlName);

            IControlGenerator ctrlGenerator = writer.FindControlGenerator(hostedCtrl);

            // add control itself
            if ((hostedCtrl != null) && (ctrlGenerator != null))
            {
                ctrlGenerator.GenerateDeclaration(writer, hostedCtrl);
            }

            string localControlType = ControlType;

            if (ctrl.controlType.Length > 0)
            {
                localControlType = ctrl.controlType;
            }

            writer.Template.AddToCodelet("CONTROLDECLARATION", "private " + localControlType + " " + ctrl.controlName + ";" +
                Environment.NewLine);
            writer.Template.AddToCodelet("CONTROLCREATION", "this." + ctrl.controlName + " = new " + localControlType + "(" +
                TYml2Xml.GetAttribute(ctrl.xmlNode, "HostedControl") + ");" + Environment.NewLine);
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            // first create the hosted control
            string hostedControlName = TYml2Xml.GetAttribute(ctrl.xmlNode, "HostedControl");
            TControlDef hostedCtrl = FCodeStorage.FindOrCreateControl(hostedControlName, ctrl.controlName);

            IControlGenerator ctrlGenerator = writer.FindControlGenerator(hostedCtrl);

            // add control itself
            if ((hostedCtrl != null) && (ctrlGenerator != null))
            {
                ctrlGenerator.SetControlProperties(writer, hostedCtrl);
            }

            return base.SetControlProperties(writer, ctrl);
        }
    }

    /// generator for a textbox that lives in a toolbar
    public class ToolbarTextBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ToolbarTextBoxGenerator()
            : base("ttx", typeof(ToolStripTextBox))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
            FChangeEventName = "TextChanged";
        }
    }

    /// <summary>
    /// generator for a label that lives in a toolbar
    /// </summary>
    public class ToolbarLabelGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ToolbarLabelGenerator()
            : base("tbl", typeof(ToolStripLabel))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a button that lives in a toolbar
    /// </summary>
    public class ToolbarButtonGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ToolbarButtonGenerator(string APrefix, System.Type AType)
            : base(APrefix, AType)
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        /// <summary>constructor</summary>
        public ToolbarButtonGenerator()
            : this("tbb", typeof(ToolStripButton))
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return (curNode.Name != "tbbSeparator") && (TYml2Xml.GetAttribute(curNode, "Label") != "-");
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a combobox that lives in a toolbar
    /// </summary>
    public class ToolbarComboBoxGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public ToolbarComboBoxGenerator(string APrefix, System.Type AType)
            : base(APrefix, AType)
        {
            FAutoSize = true;
            FLocation = false;
        }

        /// <summary>constructor</summary>
        public ToolbarComboBoxGenerator()
            : this("tbc", typeof(ToolStripComboBox))
        {
        }
    }

    /// <summary>
    /// generator for a separator on a toolbar
    /// </summary>
    public class ToolbarSeparatorGenerator : ToolbarButtonGenerator
    {
        /// <summary>constructor</summary>
        public ToolbarSeparatorGenerator()
            : base("tbb", typeof(ToolStripSeparator))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                return !base.ControlFitsNode(curNode);
            }

            return false;
        }
    }

    /// <summary>
    /// generator for embedding an user control
    /// </summary>
    public class UserControlGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public UserControlGenerator()
            : base("uco", typeof(System.Windows.Forms.Control))
        {
            FGenerateLabel = false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            string controlName = ctrl.controlName;

            FDefaultWidth = 650;
            FDefaultHeight = 386;
            base.SetControlProperties(writer, ctrl);

            // todo: use properties from yaml

            writer.Template.AddToCodelet("INITUSERCONTROLS", controlName + ".PetraUtilsObject = FPetraUtilsObject;" +
                Environment.NewLine);

            if (writer.CodeStorage.HasAttribute("DatasetType"))
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS", controlName + ".MainDS = FMainDS;" + Environment.NewLine);
            }

            writer.Template.AddToCodelet("INITUSERCONTROLS", controlName + ".InitUserControl();" + Environment.NewLine);

            return writer.FTemplate;
        }
    }
#if TODO
    /// <summary>
    /// generator for a text box in the statusbar
    /// </summary>
    public class StatusBarTextGenerator : ProviderGenerator
    {
        /// <summary>constructor</summary>
        public StatusBarTextGenerator()
            : base("sbt", typeof(EWSoftware.StatusBarText.StatusBarTextProvider))
        {
        }
    }
#endif
}