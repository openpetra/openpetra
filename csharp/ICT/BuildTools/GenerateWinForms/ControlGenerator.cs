//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2014 by OM International
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
using System.Drawing;
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
            // when the top margin is 5 pixel, and the overall height should be 22, then 17 is the height for the label
            FDefaultHeight = 17;
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
            // TODO this does not work yet. see EventRole Maintain screen
            if ((!ctrl.HasAttribute("Align"))
                && (!ctrl.HasAttribute("Width")))
            {
                ctrl.SetAttribute("Stretch", "horizontally");
            }

            base.SetControlProperties(writer, ctrl);

            // stretch at design time, but do not align to the right
            writer.SetControlProperty(ctrl, "Anchor",
                "((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)))");

            string labelText = "";

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "Text"))
            {
                labelText = TYml2Xml.GetAttribute(ctrl.xmlNode, "Text");
            }
            else
            {
                labelText = ctrl.Label + ":";
            }

            if (ctrl.HasAttribute("Width"))
            {
                ctrl.SetAttribute("Width", ctrl.GetAttribute("Width"));
            }
            else
            {
                ctrl.SetAttribute("Width", (PanelLayoutGenerator.MeasureTextWidth(labelText) + 5).ToString());
            }

            if (ctrl.HasAttribute("Height"))
            {
                ctrl.SetAttribute("Height", ctrl.GetAttribute("Height"));
            }

            writer.SetControlProperty(ctrl, "Text", "\"" + labelText + "\"");
            writer.SetControlProperty(ctrl, "Padding", "new System.Windows.Forms.Padding(0, 5, 0, 0)");

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
            bool OverrideImageAlign = false;
            bool OverrideTextAlign = false;
            bool NoLabel = false;

            TLogging.LogAtLevel(1, "ButtonGenerator.SetControlProperties for Control " + ctrl.controlName);

            if (!ctrl.HasAttribute("Width"))
            {
                ctrl.SetAttribute("Width", (PanelLayoutGenerator.MeasureTextWidth(ctrl.Label) + 15).ToString());
            }

            if (ctrl.HasAttribute("NoLabel") && (ctrl.GetAttribute("NoLabel").ToLower() == "true"))
            {
                writer.SetControlProperty(ctrl, "Text", "\"\"");

                NoLabel = true;
            }
            else
            {
                writer.SetControlProperty(ctrl, "Size", "new System.Drawing.Size(" +
                    ctrl.GetAttribute("Width").ToString() + ", " + ctrl.GetAttribute("Height").ToString() + ")");
                writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");
            }

            if (ctrl.IsOnHorizontalGridButtonPanel)
            {
                TLogging.LogAtLevel(1, "Setting Height for Control '" + ctrl.controlName + "' to 23 as it is on a horizontal Grid Button Panel");
                FDefaultHeight = 23;

                if (!ctrl.HasAttribute("ImageAlign"))
                {
                    if (NoLabel)
                    {
                        //TLogging.LogAtLevel(1, "Setting ImageAlign Attribute of Control '" + ctrl.controlName + "' to System.Drawing.ContentAlignment.BottomCenter as it is on a horizontal Grid Button Panel (no Text)");
                        writer.SetControlProperty(ctrl, "ImageAlign", "System.Drawing.ContentAlignment.BottomCenter");
                    }
                    else
                    {
                        //TLogging.LogAtLevel(1, "Setting ImageAlign Attribute of Control '" + ctrl.controlName + "' to System.Drawing.ContentAlignment.BottomLeft as it is on a horizontal Grid Button Panel");
                        writer.SetControlProperty(ctrl, "ImageAlign", "System.Drawing.ContentAlignment.BottomLeft");

                        // Note: In this case want the text centered on the Button, which the TextAlign Property will achieve.
                        // However, its default value is System.Drawing.ContentAlignment.MiddleCenter which means we don't need to explicitly write this out into the Designer file...
                        //TLogging.LogAtLevel(1, "Setting TextAlign Attribute of Control '" + ctrl.controlName + "' to System.Drawing.ContentAlignment.MiddleCenter as it is on a horizontal Grid Button Panel");
//                        writer.SetControlProperty(ctrl, "TextAlign", "System.Drawing.ContentAlignment.MiddleCenter");
                    }

                    OverrideImageAlign = true;
                    OverrideTextAlign = true;
                }
            }
            else
            {
                if (!ctrl.HasAttribute("Height"))
                {
                    ctrl.SetAttribute("Height", FDefaultHeight.ToString());
                }
            }

            base.SetControlProperties(writer, ctrl);

            if (ctrl.GetAttribute("AcceptButton").ToLower() == "true")
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS", "this.AcceptButton = " + ctrl.controlName + ";" + Environment.NewLine);
            }

            if (ctrl.GetAction() != null)
            {
                string img = ctrl.GetAction().actionImage;

                if (img.Length > 0)
                {
                    ctrl.SetAttribute("Width", (Convert.ToInt32(ctrl.GetAttribute("Width")) +
                                                Convert.ToInt32(ctrl.GetAttribute("IconWidth", "15"))).ToString());
                    writer.SetControlProperty(ctrl, "Size", "new System.Drawing.Size(" +
                        ctrl.GetAttribute("Width").ToString() + ", " + ctrl.GetAttribute("Height").ToString() + ")");

                    if (writer.GetControlProperty(ctrl.controlName, "Text") == "\"\"")
                    {
                        if ((!ctrl.HasAttribute("ImageAlign"))
                            && !OverrideImageAlign)
                        {
                            // Note: In this case we want the Image centered on the Button, which the ImageAlign Property will achieve.
                            // However, its default value is System.Drawing.ContentAlignment.MiddleCenter which means we don't need to explicitly write this out into the Designer file...

//Console.WriteLine("Setting ImageAlign Attribute of Control '" + ctrl.controlName + "' to System.Drawing.ContentAlignment.MiddleCenter as it is NOT on a horizontal Grid Button Panel (no Text)");
//                            writer.SetControlProperty(ctrl, "ImageAlign", "System.Drawing.ContentAlignment.MiddleCenter");
                        }
                    }
                    else
                    {
                        if ((!ctrl.HasAttribute("ImageAlign"))
                            && !OverrideImageAlign)
                        {
//Console.WriteLine("Setting ImageAlign Attribute of Control '" + ctrl.controlName + "' to System.Drawing.ContentAlignment.MiddleLeft as it is NOT on a horizontal Grid Button Panel");
                            writer.SetControlProperty(ctrl, "ImageAlign", "System.Drawing.ContentAlignment.MiddleLeft");
                        }
                    }

                    if (!OverrideTextAlign)
                    {
//Console.WriteLine("Setting TextAlign Attribute of Control '" + ctrl.controlName + "' to System.Drawing.ContentAlignment.MiddleRight as it is NOT on a horizontal Grid Button Panel");
                        writer.SetControlProperty(ctrl, "TextAlign", "System.Drawing.ContentAlignment.MiddleRight");
                    }
                }
            }

            return writer.FTemplate;
        }
    }

    /// <summary>
    /// generator for a single radio button
    /// </summary>
    public class RadioButtonGenerator : TControlWithDependantControlsGenerator
    {
        /// <summary>constructor</summary>
        public RadioButtonGenerator()
            : base("rbt", typeof(RadioButton))
        {
            FAutoSize = true;
            FGenerateLabel = false;
            FDefaultHeight = 25;
            FChangeEventName = "CheckedChanged";
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            // Support NoLabel=true
            FGenerateLabel = true;

            if (GenerateLabel(ctrl))
            {
                writer.SetControlProperty(ctrl, "Text", "\"" + ctrl.Label + "\"");
                ctrl.SetAttribute("Width", (PanelLayoutGenerator.MeasureTextWidth(ctrl.Label) + 30).ToString());
            }
            else
            {
                ctrl.SetAttribute("Width", "15");
            }

            base.SetControlProperties(writer, ctrl);

            FGenerateLabel = false;

            if (TYml2Xml.HasAttribute(ctrl.xmlNode, "RadioChecked"))
            {
                writer.SetControlProperty(ctrl,
                    "Checked",
                    TYml2Xml.GetAttribute(ctrl.xmlNode, "RadioChecked"));
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
        /// how to undo the change of a value of a control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            return ctrl.controlName + ".Checked = (bool)" + AFieldOrNull + ";";
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
    /// generator for a checkbox
    /// </summary>
    public class CheckBoxGenerator : TControlWithDependantControlsGenerator
    {
        /// <summary>constructor</summary>
        public CheckBoxGenerator()
            : base("chk", "Ict.Common.Controls.TchkVisibleFocus")
        {
            base.FGenerateLabel = true;
            this.FChangeEventName = "CheckedChanged";
            FDefaultHeight = 22;
            FTemplateSnippetName = "CHECKBOX";
        }

        /// <summary>
        /// make sure there is a label or not
        /// </summary>
        public override bool GenerateLabel(TControlDef ctrl)
        {
            if (ctrl.HasAttribute("CheckBoxAttachedLabel")
                && ((ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "left")
                    || (ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "right")))
            {
                ctrl.hasLabel = false;
                return false;
            }

            return base.GenerateLabel(ctrl);
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            if ((ctrl.HasAttribute("CheckBoxAttachedLabel"))
                && ((ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "left")
                    || (ctrl.GetAttribute("CheckBoxAttachedLabel").ToLower() == "right")))
            {
                base.FAutoSize = true;

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

                    writer.SetControlProperty(ctrl, "Margin", "new System.Windows.Forms.Padding(3, 4, 3, 0)");

                    ctrl.SetAttribute("Width", (PanelLayoutGenerator.MeasureTextWidth(ctrl.Label) + 30).ToString());
                    ctrl.SetAttribute("Height", "17");
                }

                base.SetControlProperties(writer, ctrl);
            }
            else
            {
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
        /// how to undo the change of a value of a control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            return ctrl.controlName + ".Checked = (bool)" + AFieldOrNull + ";";
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
            writer.Template.AddToCodelet("INITMANUALCODE", ctrl.controlName + ".CancelEditingWithEscapeKey = false;" + Environment.NewLine);
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
            : base("ppv", "Ict.Petra.Client.CommonControls.TUC_PrintPreviewControl")
        {
            FGenerateLabel = false;
        }
    }

    /// <summary>
    /// generator for a browser control (used for email preview at the moment)
    /// </summary>
    public class BrowserGenerator : TControlGenerator
    {
        /// <summary>constructor</summary>
        public BrowserGenerator()
            : base("brw", typeof(WebBrowser))
        {
            FGenerateLabel = false;
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
        /// how to undo the change of a value of a control
        /// </summary>
        protected override string UndoValue(TControlDef ctrl, string AFieldOrNull, string AFieldTypeDotNet)
        {
            return ctrl.controlName + ".Value = (decimal)" + AFieldOrNull + ";";
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
            FAutoSize = true;
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

            // Note: The follwing code *assumes* that the nested UserControl has got 'DataLoadingStarted' and
            // 'DataLoadingFinished' Events. If it hasn't, the code is generated and won't compile.
            // I (ChristianK) could find no way to determine whether the Class of the nested UserControl
            // would actually contain those Events, so I went ahead and added those Events to all
            // Templates for UserControls and to the few manually developed UserControls that get nested
            // in Generated Code files....
            if (writer.Template.FTemplateCode.Contains("OnDataLoadingStarted"))
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS",
                    controlName + ".DataLoadingStarted += new System.EventHandler(OnDataLoadingStarted);" +
                    Environment.NewLine);
            }

            if (writer.Template.FTemplateCode.Contains("OnDataLoadingFinished"))
            {
                writer.Template.AddToCodelet("INITUSERCONTROLS",
                    controlName + ".DataLoadingFinished += new System.EventHandler(OnDataLoadingFinished);" +
                    Environment.NewLine);
            }

            return writer.FTemplate;
        }
    }
}