/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Xml;
using Ict.Tools.CodeGeneration;
using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.Winforms
{
    public class LabelGenerator : TControlGenerator
    {
        public LabelGenerator()
            : base("lbl", typeof(Label))
        {
            FAutoSize = true;
            FGenerateLabel = false;
        }

        public string CalculateName(string controlName)
        {
            return "lbl" + controlName.Substring(3);
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
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

            writer.SetControlProperty(ctrl.controlName, "Text", "\"" + labelText + "\"");
            writer.SetControlProperty(ctrl.controlName, "Margin", "new System.Windows.Forms.Padding(3, 7, 3, 0)");
        }
    }
    public class ButtonGenerator : TControlGenerator
    {
        public ButtonGenerator()
            : base("btn", typeof(Button))
        {
            FAutoSize = true;
            FGenerateLabel = false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (ctrl.GetAttribute("AcceptButton").ToLower() == "true")
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS", "this.AcceptButton = " + ctrl.controlName + ";");
            }

            writer.SetControlProperty(ctrl.controlName, "Text", "\"" + ctrl.Label + "\"");
        }
    }
    public class TabPageGenerator : GroupBoxGenerator
    {
        public TabPageGenerator()
            : base("tpg", typeof(TabPage))
        {
            FAutoSize = true;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            string controlName = ctrl.controlName;

            base.SetControlProperties(writer, ctrl);
        }

/*
 *  public void CreateCode(IFormWriter writer, TControlDef ctrl)
 *  {
 *    // add page control itself
 *    GenerateDeclaration(writer, ctrl);
 *
 *    TableLayoutPanelGenerator TlpGenerator = new TableLayoutPanelGenerator();
 *    StringCollection controls = new StringCollection();
 *    foreach (TControlDef ctrl2 in ctrl.FCodeStorage.FControlList.Values)
 *    {
 *      if (ctrl2.parentName == ctrl.controlName)
 *      {
 *          controls.Add(ctrl2.controlName);
 *      }
 *    }
 *
 *    // one control per row, align labels
 *    TlpGenerator.CreateLayout(writer,
 *                                           ctrl.controlName,
 *                                           controls,
 *                                           TableLayoutPanelGenerator.eOrientation.Vertical);
 *    foreach (string ControlName in controls)
 *    {
 *        TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ControlName);
 *        TlpGenerator.CreateCode(writer, ChildControl);
 *    }
 *    this.SetControlProperties(writer, ctrl);
 *    writer.ApplyDerivedFunctionality(this, ctrl.xmlNode);
 *  }
 */
    }

    public class RadioButtonGenerator : TControlGenerator
    {
        public RadioButtonGenerator()
            : base("rbt", typeof(RadioButton))
        {
            FAutoSize = true;
            FGenerateLabel = false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            string controlName = base.FPrefix + ctrl.controlName.Substring(3);

            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl.controlName, "Text", "\"" + ctrl.Label + "\"");

            if (TXMLParser.HasAttribute(ctrl.xmlNode, "RadioChecked"))
            {
                writer.SetControlProperty(ctrl.controlName,
                    "Checked",
                    TXMLParser.GetAttribute(ctrl.xmlNode, "RadioChecked"));
            }
        }
    }
    public class DateTimePickerGenerator : TControlGenerator
    {
        public DateTimePickerGenerator()
            : base("dtp", typeof(DateTimePicker))
        {
        }
    }
    public class TcmbAutoCompleteGenerator : ComboBoxGenerator
    {
        public TcmbAutoCompleteGenerator()
            : base("cmb", "Ict.Common.Controls.TCmbAutoComplete")
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (this.FPrefix == "cmb")
            {
                return TYml2Xml.HasAttribute(curNode, "AutoComplete");
            }

            return false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (ctrl.GetAttribute("AutoComplete").EndsWith("History"))
            {
                writer.SetControlProperty(ctrl.controlName, "AcceptNewValues", "true");
                writer.SetEventHandlerToControl(ctrl.controlName,
                    "AcceptNewEntries",
                    "TAcceptNewEntryEventHandler",
                    "FPetraUtilsObject.AddComboBoxHistory");
                writer.CallControlFunction(ctrl.controlName, "SetDataSourceStringList(\"\")");
                writer.Template.AddToCodelet("INITUSERCONTROLS",
                    "FPetraUtilsObject.LoadComboBoxHistory(" + ctrl.controlName + ");" + Environment.NewLine);
            }
        }
    }
    public class TcmbAutoPopulatedGenerator : ComboBoxGenerator
    {
        public TcmbAutoPopulatedGenerator()
            : base("cmb", "Ict.Petra.Client.CommonControls.TCmbAutoPopulated")
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (this.FPrefix == "cmb")
            {
                return TYml2Xml.HasAttribute(curNode, "List");
            }

            return false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl.controlName, "ListTable", "TCmbAutoPopulated.TListTableEnum." + ctrl.GetAttribute("List"));
            writer.Template.AddToCodelet("INITUSERCONTROLS", ctrl.controlName + ".InitialiseUserControl();" + Environment.NewLine);
        }
    }
    public class ComboBoxGenerator : TControlGenerator
    {
        public ComboBoxGenerator()
            : base("cmb", "Ict.Common.Controls.TCmbAutoComplete")
        {
        }

        public ComboBoxGenerator(string APrefix, string AType)
            : base(APrefix, AType)
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return !TYml2Xml.HasAttribute(curNode, "List")
                       && !TYml2Xml.HasAttribute(curNode, "AutoComplete");
            }

            return false;
        }

        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".SelectedIndex = -1;";
            }

            if (AFieldTypeDotNet != "String")
            {
                return ctrl.controlName + ".SetSelectedInt(" + AFieldOrNull + ");";
            }

            return ctrl.controlName + ".SetSelectedString(" + AFieldOrNull + ");";
        }

        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".SelectedIndex == -1";
            }

            if (AFieldTypeDotNet != "String")
            {
                return ctrl.controlName + ".GetSelectedInt()";
            }

            return ctrl.controlName + ".GetSelectedString()";
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            StringCollection OptionalValues = TYml2Xml.GetElements(ctrl.xmlNode, "OptionalValues");

            if (OptionalValues.Count > 0)
            {
                string formattedValues = "";

                foreach (string value in OptionalValues)
                {
                    if (formattedValues.Length > 0)
                    {
                        formattedValues += ",";
                    }

                    formattedValues += "\"" + value + "\"";
                }

                writer.CallControlFunction(ctrl.controlName, "Items.AddRange(new object[] {" + formattedValues + "});");
            }
        }
    }
    public class CheckBoxGenerator : TControlGenerator
    {
        public CheckBoxGenerator()
            : base("chk", typeof(CheckBox))
        {
            base.FAutoSize = true;
            base.FGenerateLabel = false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            XmlNode Controls = TXMLParser.GetChild(ctrl.xmlNode, "Controls");

            if (Controls != null)
            {
                StringCollection childControls = TYml2Xml.GetElements(Controls);

                // this is a checkbox that enables another control or a group of controls
                ctrl.SetAttribute("GenerateCheckBoxWithOtherControls", "yes");

                if (childControls.Count == 1)
                {
                    TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(childControls[0]);
                    ChildCtrl.parentName = ctrl.controlName;

                    // use the label of the child control
                    if (ChildCtrl.HasAttribute("Label"))
                    {
                        ctrl.Label = ChildCtrl.Label;
                    }
                }
                else
                {
                    foreach (string child in childControls)
                    {
                        TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(child);

                        if (ChildCtrl == null)
                        {
                            throw new Exception("cannot find control " + child + " which should belong to " + ctrl.controlName);
                        }

                        ChildCtrl.parentName = ctrl.controlName;
                    }
                }
            }

            writer.SetControlProperty(ctrl.controlName, "Text", "\"" + ctrl.Label + "\"");
            writer.SetControlProperty(ctrl.controlName, "Margin", "new System.Windows.Forms.Padding(3, 5, 3, 0)");
        }
    }
    public class TClbVersatileGenerator : TControlGenerator
    {
        public TClbVersatileGenerator()
            : base("clb", typeof(TClbVersatile))
        {
            FHeight = 100;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl.controlName, "FixedRows", "0");
        }
    }
    public class TextBoxGenerator : TControlGenerator
    {
        public TextBoxGenerator()
            : base("txt", typeof(TextBox))
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.GetAttribute(curNode, "ReadOnly").ToLower() == "true")
                {
                    return true;
                }

                if ((TXMLParser.GetAttribute(curNode, "Type") == "PartnerKey")
                    || (TXMLParser.GetAttribute(curNode, "Type") == "Extract"))
                {
                    return false;
                }

                return true;
            }

            return false;
        }

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

        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            if (AFieldTypeDotNet.ToLower().Contains("int"))
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("double"))
            {
                return "Convert.ToDouble(" + ctrl.controlName + ".Text)";
            }

            return ctrl.controlName + ".Text";
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "ReadOnly")
                && (TYml2Xml.GetAttribute(ctrl.xmlNode, "ReadOnly").ToLower() == "true"))
            {
                writer.SetControlProperty(ctrl.controlName,
                    "ReadOnly",
                    "true");
            }

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "DefaultValue"))
            {
                writer.SetControlProperty(ctrl.controlName,
                    "Text",
                    "\"" + TXMLParser.GetAttribute(ctrl.xmlNode, "DefaultValue") + "\"");
            }
        }
    }

    public class NumericUpDownGenerator : TControlGenerator
    {
        public NumericUpDownGenerator()
            : base("nud", typeof(NumericUpDown))
        {
        }

        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Value = 0;";
            }

            return ctrl.controlName + ".Value = " + AFieldOrNull + ";";
        }

        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                // this control cannot have a null value
                return null;
            }

            return "(" + AFieldTypeDotNet + ")" + ctrl.controlName + ".Value";
        }
    }
    public class GridGenerator : TControlGenerator
    {
        public GridGenerator()
            : base("grd", typeof(Ict.Common.Controls.TSgrdDataGridPaged))
        {
            FGenerateLabel = false;
        }
    }

    public class TTxtAutoPopulatedButtonLabelGenerator : TControlGenerator
    {
        String FButtonLabelType = "";

        public TTxtAutoPopulatedButtonLabelGenerator()
            : base("txt", "Ict.Petra.Client.CommonControls.TtxtAutoPopulatedButtonLabel")
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TYml2Xml.GetAttribute(curNode, "ReadOnly").ToLower() == "true")
                {
                    return false;
                }

                if (TYml2Xml.GetAttribute(curNode, "Type") == "PartnerKey")
                {
                    FButtonLabelType = "PartnerKey";
                    return true;
                }
                else if (TYml2Xml.GetAttribute(curNode, "Type") == "Extract")
                {
                    FButtonLabelType = "Extract";
                    return true;
                }
            }

            return false;
        }

        protected override string AssignValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            if (AFieldOrNull == null)
            {
                return ctrl.controlName + ".Text = String.Empty;";
            }

            if (AFieldTypeDotNet.ToLower().Contains("int"))
            {
                return "Convert.ToInt64(" + ctrl.controlName + ".Text)";
            }
            else if (AFieldTypeDotNet.ToLower().Contains("double"))
            {
                return "Convert.ToDouble(" + ctrl.controlName + ".Text)";
            }

            return ctrl.controlName + ".Text = String.Format(\"{0:0000000000}\", " + AFieldOrNull + ");";
        }

        protected override string GetControlValue(TControlDef ctrl, string AFieldTypeDotNet)
        {
            if (AFieldTypeDotNet == null)
            {
                return ctrl.controlName + ".Text.Length == 0";
            }

            return ctrl.controlName + ".Text";
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            string ControlName = ctrl.controlName;
            Int32 buttonWidth = 40, textBoxWidth = 80;

            base.SetControlProperties(writer, ctrl);

            if (!(ctrl.HasAttribute("ReadOnly") && (ctrl.GetAttribute("ReadOnly").ToLower() == "true")))
            {
                writer.SetControlProperty(ControlName, "ASpecialSetting", "true");
                writer.SetControlProperty(ControlName, "ButtonTextAlign", "System.Drawing.ContentAlignment.MiddleCenter");
                writer.SetControlProperty(ControlName, "ButtonWidth", buttonWidth.ToString());
                writer.SetControlProperty(ControlName, "MaxLength", "32767");
                writer.SetControlProperty(ControlName, "ReadOnly", "false");
                writer.SetControlProperty(ControlName, "TextBoxWidth", textBoxWidth.ToString());
                writer.SetControlProperty(ControlName,
                    "Font",
                    "new System.Drawing.Font(\"Verdana\", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0)");
                writer.SetControlProperty(ControlName, "ListTable", "TtxtAutoPopulatedButtonLabel.TListTableEnum." + FButtonLabelType);
                writer.SetControlProperty(ControlName, "PartnerClass", "\"\"");
                writer.SetControlProperty(ControlName, "Tag", "\"CustomDisableAlthoughInvisible\"");
                writer.SetControlProperty(ControlName, "ButtonText", "\"Find\"");

                // TODO for all (or most) controls add events and event handler
                writer.SetEventHandlerToControl(ControlName, "Click");

                string EventHandlerImplementation = "\t" + ControlName + ".Select();";
                writer.SetEventHandlerFunction(ControlName, "Click", EventHandlerImplementation);
            }
        }
    }

    public class TabControlGenerator : ContainerGenerator
    {
        public TabControlGenerator()
            : base("tab", typeof(TabControl))
        {
            FGenerateLabel = false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            CreateCode(writer, ctrl);
            base.SetControlProperties(writer, ctrl);

            if (ctrl.HasAttribute("Dock"))
            {
                writer.SetControlProperty(ctrl, "Dock");
            }
        }

        protected void CreateCode(IFormWriter writer, TControlDef ATabControl)
        {
            ArrayList tabPages = new ArrayList();

            // need to save tab pages in a temporary list,
            // because TableLayoutPanelGenerator.CreateLayout will add to the FControlList
            foreach (TControlDef ctrl in ATabControl.FCodeStorage.FControlList.Values)
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
    public class GroupBoxGenerator : ContainerGenerator
    {
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

        public GroupBoxGenerator()
            : this("grp", typeof(GroupBox))
        {
        }

        public GroupBoxGenerator(string prefix)
            : this(prefix, typeof(GroupBox))
        {
        }

        public virtual StringCollection FindContainedControls(IFormWriter writer, XmlNode curNode)
        {
            return TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            string ControlName = ctrl.controlName;

            StringCollection Controls = FindContainedControls(writer, ctrl.xmlNode);
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
                    IControlGenerator ctrlGenerator = writer.FindControlGenerator(curNode);

                    // add control itself
                    ctrlGenerator.GenerateDeclaration(writer, ChildControl);
                    ctrlGenerator.SetControlProperties(writer, ChildControl);
                }
            }
            else
            {
                TableLayoutPanelGenerator TlpGenerator = new TableLayoutPanelGenerator();
                TlpGenerator.SetOrientation(ctrl);
                string tlpControlName = TlpGenerator.CreateLayout(writer, ControlName, Controls);
                writer.CallControlFunction(ControlName,
                    "Controls.Add(this." +
                    tlpControlName + ")");

                foreach (string ChildControlName in Controls)
                {
                    TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);
                    TlpGenerator.CreateCode(writer, ChildControl);
                }
            }

            if ((base.FPrefix == "grp") || (base.FPrefix == "rgr") || (base.FPrefix == "tpg"))
            {
                writer.SetControlProperty(ControlName, "Text", "\"" + ctrl.Label + "\"");
            }
        }
    }

    // this is for radiogroup just with several strings in OptionalValues
    public class RadioGroupSimpleGenerator : GroupBoxGenerator
    {
        public RadioGroupSimpleGenerator()
            : base("rgr")
        {
        }

        public RadioGroupSimpleGenerator(string prefix, System.Type type)
            : base(prefix, type)
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                if (TXMLParser.GetChild(curNode, "OptionalValues") != null)
                {
                    return !TYml2Xml.HasAttribute(curNode, "BorderVisible") || TYml2Xml.GetAttribute(curNode, "BorderVisible").ToLower() != "false";
                }
            }

            return false;
        }

        public override StringCollection FindContainedControls(IFormWriter writer, XmlNode curNode)
        {
            StringCollection optionalValues =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "OptionalValues"));
            string DefaultValue = optionalValues[0];

            if (TXMLParser.HasAttribute(curNode, "DefaultValue"))
            {
                DefaultValue = TXMLParser.GetAttribute(curNode, "DefaultValue");
            }

            // add the radiobuttons on the fly
            StringCollection Controls = new StringCollection();

            foreach (string optionalValue in optionalValues)
            {
                string radioButtonName = "rbt" + StringHelper.UpperCamelCase(optionalValue.Replace("'", "").Replace(" ", "_").Replace("&",
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

    // this is for radiogroup just with several strings in OptionalValues, but no border; uses a panel instead
    public class RadioGroupNoBorderGenerator : RadioGroupSimpleGenerator
    {
        public RadioGroupNoBorderGenerator()
            : base("rgr", typeof(System.Windows.Forms.Panel))
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (curNode.Name.StartsWith(FPrefix))
            {
                if (TXMLParser.GetChild(curNode, "Controls") == null)
                {
                    return TYml2Xml.HasAttribute(curNode, "BorderVisible") && TYml2Xml.GetAttribute(curNode, "BorderVisible").ToLower() == "false";
                }
            }

            return false;
        }
    }

    // this is for radiogroup with all sorts of sub controls
    public class RadioGroupComplexGenerator : GroupBoxGenerator
    {
        public RadioGroupComplexGenerator()
            : base("rgr")
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return TXMLParser.GetChild(curNode, "Controls") != null;
            }

            return false;
        }

        public override StringCollection FindContainedControls(IFormWriter writer, XmlNode curNode)
        {
            StringCollection Controls =
                TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));
            string DefaultValue = Controls[0];

            if (TXMLParser.HasAttribute(curNode, "DefaultValue"))
            {
                DefaultValue = TXMLParser.GetAttribute(curNode, "DefaultValue");
            }

            // prepare the controls so that they have a radiobutton instead of a label
            StringCollection radioControls = new StringCollection();

            foreach (string controlName in Controls)
            {
                TControlDef radioButton = null;

                if (controlName.StartsWith("rbt"))
                {
                    // allow a simple radiobutton in the group of radiobuttons
                    radioButton = writer.CodeStorage.GetControl(controlName);
                }
                else
                {
                    // this is a complex radio button, that enables several other controls (hence the name complex)
                    TControlDef controlWithRadioButton = writer.CodeStorage.GetControl(controlName);
                    controlWithRadioButton.SetAttribute("GenerateWithRadioButton", "yes");
                    radioButton = writer.CodeStorage.FindOrCreateControl("rbt" +
                        controlWithRadioButton.controlName.Substring(controlWithRadioButton.controlTypePrefix.Length),
                        controlWithRadioButton.controlName);
                    radioButton.parentName = curNode.Name;
                    radioButton.Label = controlWithRadioButton.Label;
                }

                if (StringHelper.IsSame(DefaultValue, controlName))
                {
                    radioButton.SetAttribute("RadioChecked", "true");
                }
            }

            return Controls;
        }
    }

    // rng: implemented as a panel
    public class RangeGenerator : GroupBoxGenerator
    {
        public RangeGenerator()
            : base("rng", typeof(Panel))
        {
        }
    }

    public class PanelGenerator : GroupBoxGenerator
    {
        public PanelGenerator()
            : base("pnl", typeof(Panel))
        {
        }
    }

    public class MenuItemGenerator : TControlGenerator
    {
        public MenuItemGenerator(string APrefix, System.Type AType)
            : base(APrefix, AType)
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        public MenuItemGenerator()
            : this("mni", typeof(ToolStripMenuItem))
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return (!curNode.Name.StartsWith("mniSeparator")) && (TYml2Xml.GetAttribute(curNode, "Label") != "-");
            }

            return false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
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
            if (!ctrl.HasAttribute("Action") && !ctrl.HasAttribute("ActionClick") && (ctrl.NumberChildren == 0))
            {
                string ActionEnabling = ctrl.controlName + ".Enabled = false;" + Environment.NewLine;
                writer.Template.AddToCodelet("ACTIONENABLING", ActionEnabling);
            }

            writer.SetControlProperty(ctrl.controlName, "Text", "\"" + ctrl.Label + "\"");

            // todo: this.toolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        }
    }

    public class MenuItemSeparatorGenerator : MenuItemGenerator
    {
        public MenuItemSeparatorGenerator()
            : base("mni", typeof(ToolStripSeparator))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (curNode.Name.StartsWith(FPrefix))
            {
                return !base.ControlFitsNode(curNode);
            }

            return false;
        }
    }

    public class MenuGenerator : ToolStripGenerator
    {
        public MenuGenerator()
            : base("mnu", typeof(MenuStrip))
        {
        }
    }

    public class StatusBarGenerator : ToolStripGenerator
    {
        public StatusBarGenerator()
            : base("stb", typeof(Ict.Common.Controls.TExtStatusBarHelp))
        {
            FDocking = "Bottom";
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            string controlName = ctrl.controlName;

            base.SetControlProperties(writer, ctrl);
        }
    }

    public class ToolBarGenerator : ToolStripGenerator
    {
        public ToolBarGenerator()
            : base("tbr", typeof(System.Windows.Forms.ToolStrip))
        {
            FRequiresChildren = true;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            string controlName = ctrl.controlName;

            base.SetControlProperties(writer, ctrl);

            // todo: toolbar properties
        }
    }

    public class ToolbarButtonGenerator : TControlGenerator
    {
        public ToolbarButtonGenerator(string APrefix, System.Type AType)
            : base(APrefix, AType)
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        public ToolbarButtonGenerator()
            : this("tbb", typeof(ToolStripButton))
        {
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                return (curNode.Name != "tbbSeparator") && (TYml2Xml.GetAttribute(curNode, "Label") != "-");
            }

            return false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl.controlName, "Text", "\"" + ctrl.Label + "\"");
        }
    }

    public class ToolbarSeparatorGenerator : ToolbarButtonGenerator
    {
        public ToolbarSeparatorGenerator()
            : base("tbb", typeof(ToolStripSeparator))
        {
            FAutoSize = true;
            FLocation = false;
            FGenerateLabel = false;
        }

        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (curNode.Name.StartsWith(FPrefix))
            {
                return !base.ControlFitsNode(curNode);
            }

            return false;
        }
    }

    public class UserControlGenerator : TControlGenerator
    {
        public UserControlGenerator()
            : base("uco", typeof(System.Windows.Forms.Control))
        {
            FGenerateLabel = false;
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            string controlName = ctrl.controlName;

            FWidth = 650;
            FHeight = 386;
            base.SetControlProperties(writer, ctrl);

            // todo: use properties from yaml

            writer.Template.AddToCodelet("INITUSERCONTROLS", controlName + ".PetraUtilsObject = FPetraUtilsObject;" + Environment.NewLine);
        }
    }
#if TODO
    public class StatusBarTextGenerator : ProviderGenerator
    {
        public StatusBarTextGenerator()
            : base("sbt", typeof(EWSoftware.StatusBarText.StatusBarTextProvider))
        {
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            // todo: add properties for StatusBarTextProvider?
        }
    }
#endif
}