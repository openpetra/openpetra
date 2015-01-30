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
using System.Xml;
using System.IO;
using Ict.Tools.CodeGeneration;
using Ict.Common.IO;
using Ict.Common;
using Ict.Tools.DBXML;
using Owf.Controls;

namespace Ict.Tools.CodeGeneration.Winforms
{
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

            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "DrawMode", "System.Windows.Forms.TabDrawMode.OwnerDrawFixed");

            // by default the tab order cannot be changed
            if (!ctrl.HasAttribute("DragTabPageEnabled") || (ctrl.GetAttribute("DragTabPageEnabled").ToLower() == "false"))
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
        /// process the tab pages
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ATabControl)
        {
            FTabControlName = ATabControl.controlName;

            // need to save tab pages in a temporary list,
            // because TableLayoutPanelGenerator.CreateLayout will add to the FControlList
            foreach (TControlDef ctrl in ATabControl.FCodeStorage.FSortedControlList.Values)
            {
                if (ctrl.controlTypePrefix == "tpg")
                {
                    ATabControl.Children.Add(ctrl);
                    ctrl.parentName = ATabControl.controlName;
                }
            }

            foreach (TControlDef ctrl in ATabControl.Children)
            {
                TabPageGenerator tabGenerator = new TabPageGenerator();
                tabGenerator.GenerateControl(writer, ctrl);
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
        /// generate the children
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            base.ProcessChildren(writer, ctrl);

            XmlNode controlsNode = TXMLParser.GetChild(ctrl.xmlNode, "Controls");

            if ((controlsNode != null) && TYml2Xml.GetChildren(controlsNode, true)[0].Name.StartsWith("Row"))
            {
                // this defines the layout with several rows with several controls per row
                Int32 countRow = 0;

                foreach (XmlNode row in TYml2Xml.GetChildren(controlsNode, true))
                {
                    StringCollection controls = TYml2Xml.GetElements(row);

                    foreach (string ctrlname in controls)
                    {
                        TControlDef childCtrl = writer.CodeStorage.GetControl(ctrlname);

                        if (ctrlname.StartsWith("Empty"))
                        {
                            childCtrl = writer.CodeStorage.FindOrCreateControl("pnlEmpty", ctrl.controlName);
                        }

                        if (childCtrl == null)
                        {
                            throw new Exception("cannot find control with name " + ctrlname + "; it belongs to " +
                                ctrl.controlName);
                        }

                        // add control itself
                        ctrl.Children.Add(childCtrl);
                        childCtrl.parentName = ctrl.controlName;
                        IControlGenerator ctrlGenerator = writer.FindControlGenerator(childCtrl);
                        ctrlGenerator.GenerateControl(writer, childCtrl);

                        childCtrl.rowNumber = countRow;
                    }

                    countRow++;
                }
            }
            else
            {
                if ((controlsNode != null) && (ctrl.Children.Count == 0))
                {
                    StringCollection controlNamesCollection = TYml2Xml.GetElements(TXMLParser.GetChild(ctrl.xmlNode, "Controls"));

                    foreach (string childCtrlName in controlNamesCollection)
                    {
                        TControlDef childCtrl = writer.CodeStorage.GetControl(childCtrlName);

                        if (childCtrlName.StartsWith("Empty"))
                        {
                            childCtrl = writer.CodeStorage.FindOrCreateControl("pnlEmpty", ctrl.controlName);
                        }

                        if (childCtrl == null)
                        {
                            throw new Exception("cannot find control with name " + childCtrlName + "; it belongs to " +
                                ctrl.controlName);
                        }

                        childCtrl.parentName = ctrl.controlName;
                        ctrl.Children.Add(childCtrl);
                    }
                }

                foreach (TControlDef childCtrl in ctrl.Children)
                {
                    TLogging.LogAtLevel(1, "foreach (TControlDef childCtrl in ctrl.Children) -- Control: " + childCtrl.controlName);

                    if (!childCtrl.controlName.StartsWith("pnlEmpty"))
                    {
                        // process the control itself
                        IControlGenerator ctrlGenerator = writer.FindControlGenerator(childCtrl);
                        ctrlGenerator.GenerateControl(writer, childCtrl);
                    }
                }
            }

            bool hasDockingChildren = false;

            // don't use a tablelayout for controls where all children have the Dock property set
            foreach (TControlDef ChildControl in ctrl.Children)
            {
                if (!ChildControl.HasAttribute("Dock"))
                {
                    ctrl.SetAttribute("UseTableLayout", "true");
                }
                else
                {
                    hasDockingChildren = true;
                }
            }

            if (ctrl.GetAttribute("UseTableLayout") == "true")
            {
                // show a warning if there are some controls with Docking, and some without
                if (hasDockingChildren)
                {
                    StringCollection clearDockAttributeChildren = new StringCollection();

                    foreach (TControlDef ChildControl in ctrl.Children)
                    {
                        if ((ChildControl.HasAttribute("Dock"))
                            && ((!ChildControl.IsHorizontalGridButtonPanelStrict)
                                && (ChildControl.controlName != TControlDef.STR_GRID_DETAILS_NAME)))
                        {
                            ChildControl.ClearAttribute("Dock");
                            clearDockAttributeChildren.Add(ChildControl.controlName);
                        }
                    }

                    if (clearDockAttributeChildren.Count > 0)
                    {
                        TLogging.Log("Warning: please remove the Dock attribute from control(s) " +
                            StringHelper.StrMerge(clearDockAttributeChildren, ','));
                    }
                }

                if (ctrl.GetAttribute("Margin") == "0")
                {
                    if (!ctrl.HasAttribute("MarginTop"))
                    {
                        ctrl.SetAttribute("MarginTop", "0");
                        ctrl.SetAttribute("MarginBottom", "0");
                        ctrl.SetAttribute("MarginLeft", "0");
                    }
                }

                PanelLayoutGenerator TlpGenerator = new PanelLayoutGenerator();
                TlpGenerator.SetOrientation(ctrl);
                TlpGenerator.CreateLayout(writer, ctrl, null, -1, -1);

                foreach (TControlDef ChildControl in ctrl.Children)
                {
                    TlpGenerator.InsertControl(writer, ChildControl);
                }

                TlpGenerator.WriteTableLayout(writer, ctrl);

                if (ctrl.GetAttribute("Dock", "None") != "None")
                {
                    writer.SetControlProperty(ctrl.controlName, "Dock", ctrl.GetAttribute("Dock"), false);

                    // groupboxes do not have AutoScroll property (grp, rgr)
                    if ((this.FPrefix == "pnl") || (this.FPrefix == "tab") || (this.FPrefix == "rng"))
                    {
                        writer.SetControlProperty(ctrl.controlName, "AutoScroll", "true", false);
                    }
                }
            }

            return;
        }

        /// <summary>
        /// add the children
        /// </summary>
        public override void AddChildren(TFormWriter writer, TControlDef ctrl)
        {
            if (ctrl.GetAttribute("UseTableLayout") != "true")
            {
                // first add the control that has Dock=Fill, then the others
                foreach (TControlDef ChildControl in ctrl.Children)
                {
                    if (ChildControl.GetAttribute("Dock") == "Fill")
                    {
                        writer.CallControlFunction(ctrl.controlName,
                            "Controls.Add(this." +
                            ChildControl.controlName + ")");
                    }
                }

                List <TControlDef>ControlsReverse = new List <TControlDef>();

                foreach (TControlDef ChildControl in ctrl.Children)
                {
                    ControlsReverse.Insert(0, ChildControl);
                }

                foreach (TControlDef ChildControl in ControlsReverse)
                {
                    if (ChildControl.GetAttribute("Dock") != "Fill")
                    {
                        writer.CallControlFunction(ctrl.controlName,
                            "Controls.Add(this." +
                            ChildControl.controlName + ")");
                    }
                }
            }
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

            base.CreateControlsAddStatements = false;
            base.SetControlProperties(writer, ctrl);

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
        /// <summary>Name of the Filter and Find Panel</summary>
        public const string PNL_FILTER_AND_FIND = "pnlFilterAndFind";

        /// <summary>A list of columns that are numeric - so we don't get duplicates on filter and find</summary>
        private List <string>listNumericColumns = new List <string>();

        /// <summary>A list of dummy controls that we generate so they can be cloned - so we don't get duplicates on filter and find</summary>
        private List <string>listCloneableControlNames = new List <string>();

        /// <summary>constructor</summary>
        public PanelGenerator()
            : base("pnl", typeof(Panel))
        {
        }

        /// <summary>constructor</summary>
        public PanelGenerator(string prefix, System.Type type)
            : base(prefix, type)
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (base.ControlFitsNode(curNode))
            {
                TControlDef ctrl = FCodeStorage.GetControl(curNode.Name);

                if (ctrl.IsHorizontalGridButtonPanelStrict)
                {
                    return false;
                }

                if (TYml2Xml.GetAttribute(curNode, "ExtendedPanel").ToLower() == "true")
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            string Width;

            base.SetControlProperties(writer, ctrl);

            if (ctrl.GetAttribute("Height").ToString() == "36")  // 36 is the Height of pnlButtons/pnlDetailButtons Panels that have ControlsOrientation=horizontal and whose Buttons have been shrinked in size to 23 Pixels by the ButtonGenerator - and
            {                                                    // whose 'Height' Element hasn't been set in the YAML file...
                if (ctrl.IsGridButtonPanel)
                {
                    Width = ctrl.GetAttribute("Width").ToString();

                    // Somehow we can run into a situation where Width isn't specified. This would lead to writing out an
                    // invalid Size Property. To prevent that we set it to some value which we can find easily in files and
                    // which will be ignored anyway at runtime as the Panel will be Docked with 'DockStyle.Bottom'!
                    if (Width.Length == 0)
                    {
                        Width = "1111";
                    }

                    FDefaultHeight = 28;

                    TLogging.LogAtLevel(1, "Adjusted Height of Panel '" + ctrl.controlName + "' as it is a horizontal Grid Button Panel");
                    writer.SetControlProperty(ctrl, "Size", "new System.Drawing.Size(" +
                        Width + ", " + FDefaultHeight.ToString() + ")");

                    writer.SetControlProperty(ctrl, "BackColor", "System.Drawing.Color.Green");
                }
            }

            return writer.FTemplate;
        }

        /// <summary>get the label text for this control</summary>
        public override bool GenerateLabel(TControlDef ctrl)
        {
            base.GenerateLabel(ctrl);

            if ((base.FPrefix == "pnl") && (TYml2Xml.GetAttribute(ctrl.xmlNode, "Label").Length > 0))
            {
                ctrl.hasLabel = true;
            }

            return ctrl.hasLabel;
        }

        /// <summary>
        /// Handle 'special' Panels
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            SortedSet <int>ButtonWidths = new SortedSet <int>();
            SortedSet <int>ButtonTops = new SortedSet <int>();
            List <TControlDef>Buttons = new List <TControlDef>();

            int NewButtonWidthForAll = -1;

            if (ctrl.controlName == PNL_FILTER_AND_FIND)
            {
                listNumericColumns.Clear();

                writer.Template.SetCodelet("FILTERANDFIND", "true");

                writer.SetControlProperty(ctrl, "Dock", "Left");
                writer.SetControlProperty(ctrl, "BackColor", "System.Drawing.Color.LightSteelBlue");
                writer.SetControlProperty(ctrl, "Width", "0");

                if (!ctrl.HasAttribute("ExpandedWidth"))
                {
                    writer.Template.SetCodelet("FINDANDFILTERINITIALWIDTH", "150");
                }
                else
                {
                    writer.Template.SetCodelet("FINDANDFILTERINITIALWIDTH", ctrl.GetAttribute("ExpandedWidth"));
                }

                if ((ctrl.HasAttribute("InitiallyExpanded"))
                    && (ctrl.GetAttribute("InitiallyExpanded").ToLower() != "false"))
                {
                    writer.Template.SetCodelet("FINDANDFILTERINITIALLYEXPANDED", "true");
                }
                else
                {
                    writer.Template.SetCodelet("FINDANDFILTERINITIALLYEXPANDED", "false");
                }

                if (!ctrl.HasAttribute("ShowApplyFilterButton"))
                {
                    writer.Template.SetCodelet("FINDANDFILTERAPPLYFILTERBUTTONCONTEXT", "TUcoFilterAndFind.FilterContext.None");
                }
                else
                {
                    writer.Template.SetCodelet("FINDANDFILTERAPPLYFILTERBUTTONCONTEXT", "TUcoFilterAndFind." +
                        ctrl.GetAttribute("ShowApplyFilterButton"));
                }

                if (!ctrl.HasAttribute("ShowKeepFilterTurnedOnButton"))
                {
                    writer.Template.SetCodelet("FINDANDFILTERSHOWKEEPFILTERTURNEDONBUTTONCONTEXT", "TUcoFilterAndFind.FilterContext.None");
                }
                else
                {
                    writer.Template.SetCodelet("FINDANDFILTERSHOWKEEPFILTERTURNEDONBUTTONCONTEXT", "TUcoFilterAndFind." +
                        ctrl.GetAttribute("ShowKeepFilterTurnedOnButton"));
                }

                if (!ctrl.HasAttribute("ShowFilterIsAlwaysOnLabel"))
                {
                    writer.Template.SetCodelet("FINDANDFILTERSHOWFILTERISALWAYSONLABELCONTEXT", "TUcoFilterAndFind.FilterContext.None");
                }
                else
                {
                    writer.Template.SetCodelet("FINDANDFILTERSHOWFILTERISALWAYSONLABELCONTEXT", "TUcoFilterAndFind." +
                        ctrl.GetAttribute("ShowFilterIsAlwaysOnLabel"));
                }

                writer.Template.SetCodelet("CUSTOMDISPOSING",
                    "if (FFilterAndFindObject != null && FFilterAndFindObject.FilterFindPanel != null)" + Environment.NewLine +
                    "{" + Environment.NewLine +
                    "    FFilterAndFindObject.FilterFindPanel.Dispose();" + Environment.NewLine +
                    "}");

                XmlNodeList controlAttributesList = null;
                XmlNode ctrlNode = ctrl.xmlNode;

                foreach (XmlNode child in ctrlNode.ChildNodes)
                {
                    if (child.Name == "ControlAttributes")
                    {
                        controlAttributesList = child.ChildNodes;
                    }
                }

                ProcessTemplate snippetFilterAndFindDeclarations = writer.Template.GetSnippet("FILTERANDFINDDECLARATIONS");
                writer.Template.InsertSnippet("FILTERANDFINDDECLARATIONS", snippetFilterAndFindDeclarations);

                ProcessTemplate snippetFilterAndFindMethods = writer.Template.GetSnippet("FILTERANDFINDMETHODS");

                writer.Template.SetCodelet("INDIVIDUALFILTERPANELS", "");
                writer.Template.SetCodelet("INDIVIDUALEXTRAFILTERPANELS", "");
                writer.Template.SetCodelet("INDIVIDUALFINDPANELS", "");
                writer.Template.SetCodelet("INDIVIDUALFILTERFINDPANELEVENTS", "");
                writer.Template.SetCodelet("INDIVIDUALFILTERFINDPANELPROPERTIES", "");
                writer.Template.SetCodelet("NUMERICFILTERFINDCOLUMNS", "");

                // Process each of the three Filter/Find definitions
                int TotalPanels = ProcessIndividualFilterFindPanel(writer,
                    "FilterControls",
                    "Filter",
                    "StandardFilter",
                    controlAttributesList,
                    "INDIVIDUALFILTERPANELS");
                TotalPanels += ProcessIndividualFilterFindPanel(writer,
                    "ExtraFilterControls",
                    "Filter",
                    "ExtraFilter",
                    controlAttributesList,
                    "INDIVIDUALEXTRAFILTERPANELS");
                TotalPanels += ProcessIndividualFilterFindPanel(writer, "FindControls", "Find", "Find", controlAttributesList, "INDIVIDUALFINDPANELS");

                if (TotalPanels == 0)
                {
                    throw new Exception("Found no controls for the Filter/Find panel");
                }

                // Manual code methods
                if (FCodeStorage.ManualFileExistsAndContains("void CreateFilterFindPanelsManual()"))
                {
                    writer.Template.SetCodelet("CREATEFILTERFINDPANELSMANUAL", "CreateFilterFindPanelsManual();" + Environment.NewLine);
                }
                else
                {
                    writer.Template.SetCodelet("CREATEFILTERFINDPANELSMANUAL", "");
                }

                if (FCodeStorage.ManualFileExistsAndContains("void FilterToggledManual(bool"))
                {
                    writer.Template.SetCodelet("FILTERTOGGLEDMANUAL", "FilterToggledManual(pnlFilterAndFind.Width == 0);" + Environment.NewLine);
                }
                else
                {
                    writer.Template.SetCodelet("FILTERTOGGLEDMANUAL", "");
                }

                if (FCodeStorage.ManualFileExistsAndContains("void ApplyFilterManual(ref"))
                {
                    writer.Template.SetCodelet("APPLYFILTERMANUAL", "ApplyFilterManual(ref AFilterString);" + Environment.NewLine);
                }
                else
                {
                    writer.Template.SetCodelet("APPLYFILTERMANUAL", "");
                }

                if (FCodeStorage.ManualFileExistsAndContains("bool IsMatchingRowManual("))
                {
                    writer.Template.SetCodelet("ISMATCHINGROW", "IsMatchingRowManual");
                }
                else
                {
                    writer.Template.SetCodelet("ISMATCHINGROW", "FFilterAndFindObject.FindPanelControls.IsMatchingRow");
                }

                // Write the whole thing out
                writer.Template.InsertSnippet("FILTERANDFINDMETHODS", snippetFilterAndFindMethods);
            }
            else
            {
                TControlDef subChildCtrl;

                TLogging.LogAtLevel(1, "PanelGenerator:Processing Children '" + ctrl.controlName + "' (about to call base method)");

                base.ProcessChildren(writer, ctrl);

                foreach (TControlDef childCtrl in ctrl.Children)
                {
                    TLogging.LogAtLevel(1, "Child Name: " + childCtrl.controlName);

                    if ((childCtrl.HasAttribute("AutoButtonMaxWidths"))
                        || (childCtrl.GetAttribute("AutoButtonMaxWidths") == "true"))
                    {
                        TLogging.LogAtLevel(1, "Control '" + childCtrl.controlName + "' has AutoButtonMaxWidths = true!");


                        XmlNode controlsNode = TXMLParser.GetChild(childCtrl.xmlNode, "Controls");

                        if ((controlsNode != null) && TYml2Xml.GetChildren(controlsNode, true)[0].Name.StartsWith("Row"))
                        {
                            TLogging.LogAtLevel(1, "Row processing");

                            // this defines the layout with several rows with several controls per row
                            foreach (XmlNode row in TYml2Xml.GetChildren(controlsNode, true))
                            {
                                StringCollection controls = TYml2Xml.GetElements(row);

                                foreach (string ctrlname in controls)
                                {
                                    subChildCtrl = writer.CodeStorage.GetControl(ctrlname);

                                    TLogging.LogAtLevel(1, "Child: '" + subChildCtrl.controlName + "'");

                                    ProcessButtonsForMaxWidthDetermination(writer, subChildCtrl, ButtonWidths, ButtonTops, Buttons);
                                }

                                ProcessButtonsForMaxWidthSizing(writer, childCtrl, ButtonWidths, ButtonTops, Buttons, out NewButtonWidthForAll);
                            }
                        }
                        else
                        {
                            if ((controlsNode != null) && (childCtrl.Children.Count != 0))
                            {
                                TLogging.LogAtLevel(1, "Controls processing");

                                foreach (TControlDef subChildCtrl2 in childCtrl.Children)
                                {
                                    TLogging.LogAtLevel(1, "Child: '" + subChildCtrl2.controlName + "'");

                                    ProcessButtonsForMaxWidthDetermination(writer, subChildCtrl2, ButtonWidths, ButtonTops, Buttons);
                                }

                                ProcessButtonsForMaxWidthSizing(writer, childCtrl, ButtonWidths, ButtonTops, Buttons, out NewButtonWidthForAll);
                            }
                        }

                        if ((Buttons.Count > 0)
                            && ((childCtrl.HasAttribute("AutoButtonMaxWidthsAutoSizesContainerWidth"))
                                && ((childCtrl.GetAttribute("AutoButtonMaxWidthsAutoSizesContainerWidth").ToLower() == "true"))))
                        {
                            int NewContainerWidth = 5 + 7 + (Buttons.Count * NewButtonWidthForAll) + ((Buttons.Count - 1) * 5); // + ((Buttons.Count - 1) * 12) - (Buttons.Count * 2);
                            TLogging.LogAtLevel(1, "NewContainerWidth: " + NewContainerWidth.ToString());
                            writer.SetControlProperty(childCtrl, "Size", "new System.Drawing.Size(" +
                                NewContainerWidth.ToString() + ", " + childCtrl.GetAttribute("Height").ToString() + ")");
                        }

                        Buttons.Clear();
                        ButtonWidths.Clear();
                    }
                }
            }
        }

        private void ProcessButtonsForMaxWidthDetermination(TFormWriter writer, TControlDef ctrl,
            SortedSet <int>AButtonWidths, SortedSet <int>AButtonTops, List <TControlDef>AButtons)
        {
            if (ctrl.controlName.StartsWith("btn"))
            {
                TLogging.LogAtLevel(1, "Found Button: " + ctrl.controlName + ", Width: " + ctrl.Width.ToString());

                if (ctrl.GetAttribute("Visible") != "false")
                {
                    AButtons.Add(ctrl);
                    AButtonWidths.Add(ctrl.Width);


                    string ControlLocation = writer.GetControlProperty(ctrl.controlName, "Location");
//                TLogging.LogAtLevel(1, "ControlLocation '" + Button.controlName + "' ControlLocation: " + ControlLocation);
                    string StrY = ControlLocation.Substring(ControlLocation.IndexOf(',') + 1, ControlLocation.Length - ControlLocation.IndexOf(
                            ',') - 2);
//                TLogging.LogAtLevel(1, "ControlLocation '" + Button.controlName + "' StrY: " + StrY);
                    int Y = Convert.ToInt32(StrY);

                    AButtonTops.Add(Y);
                }
                else
                {
                    TLogging.LogAtLevel(1, "Button isn't visible, therefore ignoring it for max. Width determination!");
                }
            }
        }

        private void ProcessButtonsForMaxWidthSizing(TFormWriter writer, TControlDef ctrl, SortedSet <int>ButtonWidths,
            SortedSet <int>ButtonTops, List <TControlDef>Buttons, out int NewButtonWidthForAll)
        {
            const int MIN_BUTTON_WIDTH = 76; // 76 seems to be pretty much a standard width (on the Windows OS)
            int LeftPos = PanelLayoutGenerator.MARGIN_LEFT;
            int ButtonCounter = 0;
            bool EnforceMinimumWidth = false;
            bool RepositionButtonsHorizontally = ButtonTops.Max == ButtonTops.Min;

            NewButtonWidthForAll = MIN_BUTTON_WIDTH;

            TLogging.LogAtLevel(1, "Max. Button Width: " + ButtonWidths.Max.ToString() + ", Number of Buttons: " + Buttons.Count.ToString());

            if ((!ctrl.HasAttribute("AutoButtonMaxWidthsEnforceMinimumWidth")
                 || (ctrl.GetAttribute("AutoButtonMaxWidthsEnforceMinimumWidth").ToLower() == "true")))
            {
                EnforceMinimumWidth = true;
            }

            if ((EnforceMinimumWidth)
                && (ButtonWidths.Max < MIN_BUTTON_WIDTH))
            {
                if (Buttons.Count > 0)
                {
                    NewButtonWidthForAll = MIN_BUTTON_WIDTH;
                    TLogging.LogAtLevel(1,
                        "Max. Button width (" + ButtonWidths.Max.ToString() + ") was smaller than MIN_BUTTON_WIDTH, adjusting to the latter!");
                }
            }
            else
            {
                NewButtonWidthForAll = ButtonWidths.Max;
            }

            foreach (var Button in Buttons)
            {
// TLogging.LogAtLevel(1, "Button ' " + Button.controlName + "': Width = " + Button.GetAttribute("Width"));

                if ((!Button.HasAttribute("NoLabel")
                     || (Button.GetAttribute("NoLabel").ToLower() == "false")))
                {
                    TLogging.LogAtLevel(1, "Applying Width to Button '" + Button.controlName + "'");
                    writer.SetControlProperty(Button, "Size", "new System.Drawing.Size(" +
                        NewButtonWidthForAll.ToString() + ", " + Button.GetAttribute("Height").ToString() + ")");

                    if (ButtonCounter != 0)
                    {
                        LeftPos += NewButtonWidthForAll + PanelLayoutGenerator.HORIZONTAL_SPACE;
                    }
                }
                else
                {
                    TLogging.LogAtLevel(
                        1,
                        "Not applying Width to Button '" + Button.controlName + "' as its 'NoLabel' Element is set to 'true'! (Current Width: " +
                        Button.Width.ToString() + ")");

                    if (ButtonCounter != 0)
                    {
                        LeftPos += Button.Width + PanelLayoutGenerator.HORIZONTAL_SPACE;
                    }
                }

                if (RepositionButtonsHorizontally)
                {
                    writer.SetControlProperty(Button, "Location", String.Format("new System.Drawing.Point({0}, {1})", LeftPos, ButtonTops.Max));
                    TLogging.LogAtLevel(1,
                        "Repositioned Button '" + Button.controlName + "' horizontally. New Location: " + LeftPos.ToString() + "," +
                        ButtonTops.Max.ToString());
                }

                //               TLogging.LogAtLevel(1, "Button '" + Button.controlName + "' Width: " + Button.Width.ToString());

                ButtonCounter++;
            }
        }

        /// <summary>
        /// Process all the definitions for a specified panel set.  Return the number of items on the panel
        /// </summary>
        private int ProcessIndividualFilterFindPanel(TFormWriter writer,
            string AXmlNodeName,
            string APanelType,
            string APanelSubType,
            XmlNodeList AControlAttributesList,
            string ATargetCodelet)
        {
            int NumItemsOnThisPanel = 0;

            List <XmlNode>children = TYml2Xml.GetChildren((XmlNode)FCodeStorage.FXmlNodes[AXmlNodeName], false);

            foreach (XmlNode child in children)
            {
                string controlName = child.Attributes["name"].Value;           // eg txtDetailSomeColumn, txtSomeColumn$1, or Column:table.column

                if (controlName.StartsWith("Column:"))
                {
                    // The column is specified directly
                    // We create a prototype label and control to clone from with names based on the column name
                    // Decide on control name, label name, column name
                    controlName = controlName.Substring(7).TrimStart();
                    string tableName, columnName, lblName;

                    if (controlName.Contains("."))
                    {
                        int p = controlName.IndexOf('.');
                        tableName = controlName.Substring(0, p);
                        columnName = controlName.Substring(p + 1).TrimEnd();
                    }
                    else
                    {
                        tableName = FCodeStorage.GetAttribute("DetailTable");
                        columnName = controlName.TrimEnd();
                    }

                    // The column name may include an instance - remember this and work out the column name without the instance
                    string instanceName = String.Empty;
                    string controlNameWithInstance;

                    if (columnName.Contains("-"))
                    {
                        instanceName = columnName.Substring(columnName.LastIndexOf('-'));
                        columnName = columnName.Substring(0, columnName.LastIndexOf('-'));
                    }

                    lblName = "lbl" + columnName;
                    string lblText = StringHelper.ReverseUpperCamelCase(columnName);

                    bool bHasALabel = true;
                    string columnDataType = GetColumnDataType(writer, tableName, columnName);
                    string controlType = (columnDataType == "bit") ? "CheckBox" : "TextBox";

                    // Controls based on a column name are either checkbox or textbox
                    if (columnDataType == "bit")
                    {
                        controlName = "chk" + columnName;
                        bHasALabel = false;
                    }
                    else
                    {
                        controlName = "txt" + columnName;
                    }

                    // Get additional attributes for these dummy controls
                    controlNameWithInstance = controlName + instanceName;
                    XmlAttributeCollection controlAttributes = GetAdditionalAttributes(controlNameWithInstance, AControlAttributesList);

                    if ((controlAttributes != null) && (controlAttributes["NoLabel"] != null) && (controlType != "CheckBox"))
                    {
                        bHasALabel = (controlAttributes["NoLabel"].Value.ToLower() != "true");
                    }

                    // Create the throw-away label and control so that we can clone them (unless they have been created for a previous panel)
                    if (bHasALabel)
                    {
                        if ((controlAttributes != null) && (controlAttributes["Label"] != null) && (controlType != "CheckBox"))
                        {
                            lblText = controlAttributes["Label"].Value;
                        }

                        CreateCloneableControl(writer, lblName, "Label", lblText, ATargetCodelet);
                    }

                    if (columnDataType == "bit")
                    {
                        CreateCloneableControl(writer, controlName, "CheckBox", lblText, ATargetCodelet);
                    }
                    else
                    {
                        CreateCloneableControl(writer, controlName, "TextBox", null, ATargetCodelet);
                    }

                    bool bHasClearButton;
                    AddFilterFindPanel(writer,
                        controlType,
                        APanelType,
                        APanelSubType,
                        controlNameWithInstance,
                        lblName,
                        bHasALabel,
                        columnName,
                        columnDataType,
                        controlAttributes,
                        ATargetCodelet,
                        out bHasClearButton);

                    WriteAdditionalProperties(writer, controlType, APanelType, controlNameWithInstance, bHasClearButton, controlAttributes);

                    NumItemsOnThisPanel++;
                }
                else if (controlName.StartsWith("pnl") && !FCodeStorage.FControlList.ContainsKey(controlName))
                {
                    // It is a panel that does not exist as a details panel - so it might be (must be) one of ours
                    // We will need to discover what controls go on this panel
                    XmlNode panelNode = (XmlNode)FCodeStorage.FXmlNodes[controlName];

                    if (panelNode == null)
                    {
                        throw new Exception("Could not find an definition for " + controlName);
                    }

                    // Get the list of controls on this dynamic panel
                    XmlNode dynamicControlsNode = TXMLParser.GetChild(panelNode, "Controls");

                    if ((dynamicControlsNode == null) || (dynamicControlsNode.ChildNodes.Count == 0))
                    {
                        throw new Exception("No controls specified for " + controlName);
                    }

                    // take each dynamic control in turn
                    foreach (XmlNode dynamicControl in dynamicControlsNode)
                    {
                        string dynamicControlName = dynamicControl.Attributes["name"].Value;

                        // The control name may include an instance - remember this and work out the control name without the instance
                        string dynamicControlNameWithInstance = dynamicControlName;

                        if (dynamicControlNameWithInstance.Contains("-"))
                        {
                            dynamicControlName = dynamicControlName.Substring(0, dynamicControlName.LastIndexOf('-'));
                        }

                        // find the node for this name
                        XmlNode dynamicControlNode = TXMLParser.GetChild(panelNode, dynamicControlNameWithInstance);

                        if (dynamicControlNode == null)
                        {
                            throw new Exception("Could not find an definition for " + dynamicControlNameWithInstance);
                        }

                        string simpleControlType = String.Empty;

                        if (dynamicControlName.StartsWith("rgr"))
                        {
                            // Create a radio button group
                            CreateDynamicFilterFindRadioButtonGroup(writer, dynamicControlNode, APanelType, APanelSubType, ATargetCodelet);
                        }
                        else if (dynamicControlName.StartsWith("cmb"))
                        {
                            // Create a dynamic ComboBox
                            CreateDynamicFilterFindComboBox(writer, dynamicControlNode, APanelType, APanelSubType, ATargetCodelet);
                        }
                        else if (dynamicControlName.StartsWith("txt"))
                        {
                            // Create a dynamic TextBox
                            simpleControlType = "TextBox";
                        }
                        else if (dynamicControlName.StartsWith("dtp"))
                        {
                            // Create a dynamic TtxtPetraDate
                            simpleControlType = "TtxtPetraDate";
                        }
                        else if (dynamicControlName.StartsWith("chk"))
                        {
                            // Create a dynamic CheckBox
                            simpleControlType = "CheckBox";
                        }
                        else
                        {
                            throw new NotImplementedException(
                                "No code written yet to create a dynamic instance of this control: " + dynamicControlName);
                        }

                        if (simpleControlType != String.Empty)
                        {
                            CreateDynamicFilterFindSimpleControl(writer,
                                dynamicControlName,
                                dynamicControlNameWithInstance,
                                simpleControlType,
                                APanelType,
                                APanelSubType,
                                ATargetCodelet);
                        }
                    }

                    NumItemsOnThisPanel++;
                }
                else
                {
                    // This is a control that we are cloning direct from the details panel
                    // The control name may include an instance - remember this and work out the control name without the instance
                    string controlNameWithInstance = controlName;

                    if (controlNameWithInstance.Contains("-"))
                    {
                        controlName = controlName.Substring(0, controlName.LastIndexOf('-'));
                    }

                    // Does it exist in the main list??
                    if (!FCodeStorage.FControlList.ContainsKey(controlName))
                    {
                        throw new Exception("Could not find a reference to the control: " + controlName);
                    }

                    string lblName = "lbl" + controlName.Substring(3);

                    // The column is specified by its control name (or by the DataColumn control attribute)
                    string columnName = controlName.Substring(3);

                    if (FCodeStorage.FControlList[controlName].HasAttribute("DataColumn"))
                    {
                        columnName = FCodeStorage.FControlList[controlName].GetAttribute("DataColumn");

                        if (columnName.Contains("."))
                        {
                            if (columnName.Substring(0, columnName.IndexOf('.')).CompareTo(FCodeStorage.GetAttribute("DetailTable")) != 0)
                            {
                                throw new Exception(
                                    "When specifying a DataColumn for a Find/Filter control the table must refer to the DetailTable. The control is: "
                                    +
                                    controlName);
                            }

                            columnName = columnName.Substring(columnName.LastIndexOf('.') + 1);
                            TLogging.Log("Using DataColumn: " + columnName + " for Filter/Find");
                        }
                    }

                    if (columnName.StartsWith("Detail"))
                    {
                        columnName = columnName.Substring(6);
                    }

                    string columnDataType = GetColumnDataType(writer, FCodeStorage.GetAttribute("DetailTable"), columnName);

                    string ctrlNamePrefix = controlName.Substring(0, 3);
                    string controlType = null;

                    switch (ctrlNamePrefix)
                    {
                        case "cmb":
                            // we have to work out the type of combo we are cloning FROM
                            TControlDef comboCtrl = FCodeStorage.FControlList[controlName];

                            if (comboCtrl.HasAttribute("List"))
                            {
                                controlType = "TCmbAutoPopulated";
                            }
                            else if (comboCtrl.HasAttribute("MultiColumn"))
                            {
                                controlType = "TCmbVersatile";
                            }
                            else
                            {
                                controlType = "TCmbAutoComplete";
                            }

                            break;

                        case "chk":
                            controlType = "CheckBox";
                            break;

                        case "txt":
                            controlType = "TextBox";
                            break;

                        case "dtp":
                            controlType = "TtxtPetraDate";
                            break;

                        case "rbt":
                            controlType = "RadioButton";
                            break;

                        case "pnl":
                            controlType = "Panel";
                            break;

                        case "rgr":
                            controlType = "GroupBox";
                            break;

                        case "lbl":
                            controlType = "Label";
                            break;

                        default:
                            throw new Exception("Unsupported control type to clone from: " + controlName);
                    }

                    XmlAttributeCollection controlAttributes = GetAdditionalAttributes(controlNameWithInstance, AControlAttributesList);
                    bool bHasALabel = true;

                    if ((controlAttributes != null) && (controlType != "CheckBox"))
                    {
                        if (controlAttributes["NoLabel"] != null)
                        {
                            bHasALabel = (controlAttributes["NoLabel"].Value.ToLower() != "true");
                        }
                    }

                    if (bHasALabel)
                    {
                        // Our new control is to have a label
                        if ((FCodeStorage.FControlList[controlName].GetAttribute("NoLabel", "false").ToLower() == "true")
                            || (controlType == "GroupBox")
                            || (controlType == "Panel")
                            || (controlType == "RadioButton"))
                        {
                            // The cloned-from control has no label so we can only do domething if the YAML specifies a label text
                            if ((controlAttributes != null) && (controlAttributes["Label"] != null))
                            {
                                // the cloned-from control has no label so we will need to create one to clone from
                                CreateCloneableControl(writer, lblName, "Label", controlAttributes["Label"].Value, ATargetCodelet);
                            }
                            else
                            {
                                // We cannot have a label after all
                                bHasALabel = false;
                            }
                        }
                    }

                    bool bHasClearButton;
                    AddFilterFindPanel(writer,
                        controlType,
                        APanelType,
                        APanelSubType,
                        controlNameWithInstance,
                        lblName,
                        bHasALabel,
                        columnName,
                        columnDataType,
                        controlAttributes,
                        ATargetCodelet,
                        out bHasClearButton);

                    WriteAdditionalProperties(writer, controlType, APanelType, controlNameWithInstance, bHasClearButton, controlAttributes);

                    NumItemsOnThisPanel++;
                }
            }

            return NumItemsOnThisPanel;
        }

        /// <summary>
        /// Gets the attribute collection for a specific control in the list of all nodes at this level
        /// </summary>
        private XmlAttributeCollection GetAdditionalAttributes(string AControlName, XmlNodeList AAdditionalPropertyNodeList)
        {
            XmlAttributeCollection additionalAttributes = null;

            if (AAdditionalPropertyNodeList != null)
            {
                foreach (XmlNode controlNode in AAdditionalPropertyNodeList)
                {
                    if (controlNode.Name == AControlName)
                    {
                        additionalAttributes = controlNode.Attributes;
                    }
                }
            }

            return additionalAttributes;
        }

        /// <summary>
        /// Write the additional properties specified in the attribute list
        /// </summary>
        private void WriteAdditionalProperties(TFormWriter writer,
            string AControlType,
            string APanelType,
            string AControlName,
            bool AHasClearButton,
            XmlAttributeCollection AControlAttributesList)
        {
            if (AControlAttributesList == null)
            {
                return;
            }

            foreach (XmlAttribute att in AControlAttributesList)
            {
                if ((att.Name == "depth")
                    || (att.Name == "ClearButton")
                    || (att.Name == "ClearValue")
                    || (att.Name == "NoLabel")
                    || (att.Name == "Comparison")
                    || (att.Name == "FindComparison")
                    || (att.Name == "CloneToComboBox"))
                {
                    // we have dealt with these already
                    continue;
                }
                else if (att.Name == "Width")
                {
                    string width = String.Format("Math.Min({0}, FFilterAndFindObject.FilterAndFindParameters.FindAndFilterInitialWidth)", att.Value);
                    AddFilterFindProperty(writer, AControlType, APanelType, AControlName, att.Name, width);
                }
                else if (att.Name == "Label")
                {
                    if (!listCloneableControlNames.Contains(AControlName))
                    {
                        string lblName = "lbl" + AControlName.Substring(3);
                        AddFilterFindProperty(writer, "Label", APanelType, lblName, "Text", "\"" + att.Value + "\"");
                    }
                }
                else if (att.Name == "Text")
                {
                    AddFilterFindProperty(writer, AControlType, APanelType, AControlName, "Text", "\"" + att.Value + "\"");
                }
                else if (att.Name == "List")
                {
                    AddFilterFindProperty(writer,
                        AControlType,
                        APanelType,
                        AControlName,
                        "ListTable",
                        "TCmbAutoPopulated.TListTableEnum." + att.Value);
                }
                else if (att.Name == "OnChange")
                {
                    string eventName = "TextChanged";

                    if (!AHasClearButton)
                    {
                        if (AControlName.StartsWith("cmb"))
                        {
                            eventName = "SelectedValueChanged";
                        }
                        else if (AControlName.StartsWith("rbt"))
                        {
                            eventName = "CheckedChanged";
                        }
                        else if (AControlName.StartsWith("chk"))
                        {
                            eventName = "CheckStateChanged";
                        }
                    }

                    AddFilterFindEvent(writer, AControlType, APanelType, AControlName, eventName, att.Value);

                    // ComboBoxes that can be cleared need two events
                    if (AHasClearButton && AControlName.StartsWith("cmb"))
                    {
                        AddFilterFindEvent(writer, AControlType, APanelType, AControlName, "SelectedValueChanged", att.Value);
                    }
                }
                else
                {
                    AddFilterFindProperty(writer, AControlType, APanelType, AControlName, att.Name, att.Value);
                }
            }
        }

        /// <summary>
        /// Get the tag value for the control using its attributes - includes whether the control has a clear button and the clear value
        /// </summary>
        private string GetFilterFindTagValue(string AInstanceName, XmlAttributeCollection AControlAttributesList, out bool AHasClearButton)
        {
            AHasClearButton = true;

            if ((AControlAttributesList != null) && (AControlAttributesList["ClearButton"] != null))
            {
                AHasClearButton = (AControlAttributesList["ClearButton"].Value != "false");
            }

            string clearValue = String.Empty;

            if ((AControlAttributesList != null) && (AControlAttributesList["ClearValue"] != null))
            {
                clearValue = AControlAttributesList["ClearValue"].Value;
            }

            string comparisonValue = String.Empty;

            if ((AControlAttributesList != null) && (AControlAttributesList["Comparison"] != null))
            {
                comparisonValue = AControlAttributesList["Comparison"].Value;

                if ((comparisonValue != "gt")
                    && (comparisonValue != "gte")
                    && (comparisonValue != "lt")
                    && (comparisonValue != "lte")
                    && (comparisonValue != "eq"))
                {
                    throw new NotSupportedException("Only the following comaparisons are allowed: gt, gte, lt, lte, eq");
                }
            }

            string findComparisonValue = String.Empty;

            if ((AControlAttributesList != null) && (AControlAttributesList["FindComparison"] != null))
            {
                findComparisonValue = AControlAttributesList["FindComparison"].Value;

                if ((findComparisonValue != "gt")
                    && (findComparisonValue != "gte")
                    && (findComparisonValue != "lt")
                    && (findComparisonValue != "lte")
                    && (findComparisonValue != "eq")
                    && (findComparisonValue != "StartsWith")
                    && (findComparisonValue != "EndsWith")
                    && (findComparisonValue != "Contains"))
                {
                    throw new NotSupportedException(
                        "Only the following comaparisons are allowed: gt, gte, lt, lte, eq, StartsWith, EndsWith, Contains");
                }
            }

            // Now assemble the tag string
            string strTag = String.Empty;

            if (AInstanceName != String.Empty)
            {
                strTag += String.Format("{0}{1};", CommonTagString.INSTANCE_EQUALS, AInstanceName);
            }

            if (comparisonValue != String.Empty)
            {
                strTag += String.Format("{0}{1};", CommonTagString.COMPARISON_EQUALS, comparisonValue);
            }

            if (findComparisonValue != String.Empty)
            {
                strTag += String.Format("{0}{1};", CommonTagString.FIND_COMPARISON_EQUALS, findComparisonValue);
            }

            if (!AHasClearButton)
            {
                strTag += CommonTagString.ARGUMENTPANELTAG_NO_AUTOM_ARGUMENTCLEARBUTTON;
            }

            if (clearValue != String.Empty)
            {
                strTag += String.Format("{0}={1};", CommonTagString.ARGUMENTCONTROLTAG_CLEARVALUE, clearValue);
            }

            return strTag == String.Empty ? "String.Empty" : String.Format("\"{0}\"", strTag);
        }

        /// <summary>
        /// Create a cloneable control that forms the basis of the cloned control on the filter/find panel
        /// </summary>
        private void CreateCloneableControl(TFormWriter writer, string AControlName, string AControlType, string AControlText, string ATargetCodelet)
        {
            if (listCloneableControlNames.Contains(AControlName))
            {
                return;
            }

            ProcessTemplate snippetControl;

            if (AControlText == null)
            {
                snippetControl = writer.Template.GetSnippet("SNIPDYNAMICCREATECONTROL");
            }
            else
            {
                snippetControl = writer.Template.GetSnippet("SNIPDYNAMICCREATECONTROLWITHTEXT");
                snippetControl.SetCodelet("CONTROLTEXT", AControlText);
            }

            snippetControl.SetCodelet("CONTROLNAME", AControlName);
            snippetControl.SetCodelet("CONTROLTYPE", AControlType);
            writer.Template.InsertSnippet(ATargetCodelet, snippetControl);
            writer.Template.AddToCodelet(ATargetCodelet, Environment.NewLine);

            listCloneableControlNames.Add(AControlName);
        }

        /// <summary>
        /// Add a new panel set (label and Control) based on a pair of cloneable controls.
        /// Column name and data type can be null
        /// </summary>
        private void AddFilterFindPanel(TFormWriter writer,
            string AControlType,
            string APanelType,
            string APanelSubType,
            string AControlName,
            string ALabelName,
            bool AHasALabel,
            string AColumnName,
            string AColumnDataType,
            XmlAttributeCollection AControlAttributesList,
            string ATargetCodelet,
            out bool AHasClearButton)
        {
            string instance = String.Empty;

            if (AControlName.Contains("-"))
            {
                int pos = AControlName.LastIndexOf('-');
                instance = AControlName.Substring(pos);
                AControlName = AControlName.Substring(0, pos);
            }

            ProcessTemplate snippetFilterFind;

            if (AColumnName == null)
            {
                snippetFilterFind = writer.Template.GetSnippet("SNIPINDIVIDUALFILTERFINDPANELNOCOLUMN");
            }
            else
            {
                snippetFilterFind = writer.Template.GetSnippet("SNIPINDIVIDUALFILTERFINDPANEL");
                snippetFilterFind.SetCodelet("COLUMNNAME", AColumnName);
                snippetFilterFind.SetCodelet("COLUMNDATATYPE", AColumnDataType);
            }

            ProcessTemplate snippetLabel = writer.Template.GetSnippet("SNIPCLONELABEL");

            if (AHasALabel)
            {
                snippetLabel.SetCodelet("CLONEDFROMLABEL", ALabelName);
                snippetLabel.SetCodelet("PANELTYPE", ATargetCodelet.Contains("FILTER") ? "Filter" : "Find");
                snippetLabel.SetCodelet("PANELTYPEUC", ATargetCodelet.Contains("FILTER") ? "FILTER" : "FIND");
                snippetFilterFind.InsertSnippet("CLONELABEL", snippetLabel);
            }
            else
            {
                snippetFilterFind.SetCodelet("CLONELABEL", "null," + Environment.NewLine);
            }

            bool cloneToComboBox = AControlName.StartsWith("cmb");

            if ((AControlAttributesList != null) && (AControlAttributesList["CloneToComboBox"] != null))
            {
                cloneToComboBox = (AControlAttributesList["CloneToComboBox"].Value == "true");
            }

            snippetFilterFind.SetCodelet("CONTROLCLONE", cloneToComboBox ? "ShallowCloneToComboBox" : "ShallowClone");
            snippetFilterFind.SetCodelet("CONTROLTYPE", AControlType);
            snippetFilterFind.SetCodelet("CLONEDFROMCONTROL", AControlName);
            snippetFilterFind.SetCodelet("PANELTYPE", APanelType);
            snippetFilterFind.SetCodelet("PANELTYPEUC", APanelType.ToUpper());
            snippetFilterFind.SetCodelet("PANELSUBTYPE", APanelSubType);
            snippetFilterFind.SetCodelet("TAG", GetFilterFindTagValue(instance, AControlAttributesList, out AHasClearButton));

            writer.Template.InsertSnippet(ATargetCodelet, snippetFilterFind);
            writer.Template.AddToCodelet(ATargetCodelet, Environment.NewLine);
        }

        /// <summary>
        /// Create a completely dynamic checkBox control that has no direct relationship to the database
        /// </summary>
        private void CreateDynamicFilterFindSimpleControl(TFormWriter writer,
            string AControlName,
            string AControlNameWithInstance,
            string AControlType,
            string APanelType,
            string APanelSubType,
            string ATargetCodelet)
        {
            string lblName = "lbl" + AControlName.Substring(3);
            XmlNode dateboxCtrlNode = (XmlNode)FCodeStorage.FXmlNodes[AControlNameWithInstance];
            XmlAttributeCollection controlAttributes = dateboxCtrlNode.Attributes;

            CreateCloneableControl(writer, lblName, "Label", StringHelper.ReverseUpperCamelCase(AControlName.Substring(3)), ATargetCodelet);
            CreateCloneableControl(writer, AControlName, AControlType, null, ATargetCodelet);

            bool bHasClearButton;
            AddFilterFindPanel(writer,
                AControlType,
                APanelType,
                APanelSubType,
                AControlNameWithInstance,
                lblName,
                true,
                null,
                null,
                controlAttributes,
                ATargetCodelet,
                out bHasClearButton);

            WriteAdditionalProperties(writer, AControlType, APanelType, AControlNameWithInstance, bHasClearButton, controlAttributes);
        }

        /// <summary>
        /// Create a completely dynamic comboBox control that has no direct relationship to the database
        /// </summary>
        private void CreateDynamicFilterFindComboBox(TFormWriter writer,
            XmlNode ADynamicControlNode,
            string APanelType,
            string APanelSubType,
            string ATargetCodelet)
        {
            string ctrlName = ADynamicControlNode.Name;
            string lblName = "lbl" + ctrlName.Substring(3);
            XmlNode comboCtrlNode = (XmlNode)FCodeStorage.FXmlNodes[ctrlName];
            XmlAttributeCollection controlAttributes = comboCtrlNode.Attributes;

            CreateCloneableControl(writer, lblName, "Label", StringHelper.ReverseUpperCamelCase(ctrlName.Substring(3)), ATargetCodelet);

            if (!listCloneableControlNames.Contains(ctrlName))
            {
                CreateCloneableControl(writer, ctrlName, "TCmbAutoComplete", null, ATargetCodelet);

                XmlNode optionalValuesNode = TXMLParser.GetChild(ADynamicControlNode, "OptionalValues");

                if ((optionalValuesNode != null) && (optionalValuesNode.ChildNodes.Count > 0))
                {
                    string specifiedValues = String.Format("{0}.Items.AddRange( new object[] {{ ", ctrlName);

                    foreach (XmlNode optionalValueNode in optionalValuesNode)
                    {
                        string optionalValueName = optionalValueNode.Attributes["name"].Value;
                        specifiedValues += String.Format("\"{0}\", ", optionalValueName);
                    }

                    specifiedValues = specifiedValues.Substring(0, specifiedValues.Length - 2) + " } );" + Environment.NewLine + Environment.NewLine;
                    writer.FTemplate.AddToCodelet(ATargetCodelet, specifiedValues);
                }
            }

            bool bHasClearButton;
            AddFilterFindPanel(writer,
                "TCmbAutoComplete",
                APanelType,
                APanelSubType,
                ctrlName,
                lblName,
                true,
                null,
                null,
                controlAttributes,
                ATargetCodelet,
                out bHasClearButton);

            WriteAdditionalProperties(writer, "TCmbAutoComplete", APanelType, ctrlName, bHasClearButton, controlAttributes);
        }

        /// <summary>
        /// Create a completely dynamic radio button group that has no direct relationship to the database
        /// </summary>
        private void CreateDynamicFilterFindRadioButtonGroup(TFormWriter writer,
            XmlNode ADynamicControlNode,
            string APanelType,
            string APanelSubType,
            string ATargetCodelet)
        {
            string ctrlName = ADynamicControlNode.Name;
            XmlNode rgrCtrlNode = (XmlNode)FCodeStorage.FXmlNodes[ctrlName];
            XmlAttributeCollection controlAttributes = rgrCtrlNode.Attributes;

            string labelText = StringHelper.ReverseUpperCamelCase(ctrlName.Substring(3));

            if (controlAttributes["Label"] != null)
            {
                labelText = controlAttributes["Label"].Value;
            }

            CreateCloneableControl(writer, ctrlName, "GroupBox", labelText, ATargetCodelet);

            XmlNode optionalValuesNode = TXMLParser.GetChild(ADynamicControlNode, "OptionalValues");

            foreach (XmlNode optionalValueNode in optionalValuesNode)
            {
                bool bIsDefault = false;
                string optionalValueName = optionalValueNode.Attributes["name"].Value;

                if (optionalValueName.StartsWith("="))
                {
                    bIsDefault = true;
                    optionalValueName = optionalValueName.Substring(1);
                }

                string rbtName = "rbt" + optionalValueName;

                CreateCloneableControl(writer, rbtName, "RadioButton", StringHelper.ReverseUpperCamelCase(optionalValueName), ATargetCodelet);
                writer.Template.AddToCodelet(ATargetCodelet, String.Format("{0}.Controls.Add({1});{2}{2}", ctrlName, rbtName, Environment.NewLine));

                if (bIsDefault)
                {
                    AddFilterFindProperty(writer, "RadioButton", APanelType, rbtName, "Checked", "true");
                }

                if (FCodeStorage.FXmlNodes[rbtName] != null)
                {
                    XmlAttributeCollection rbtAttributes = ((XmlNode)FCodeStorage.FXmlNodes[rbtName]).Attributes;
                    WriteAdditionalProperties(writer, "RadioButton", APanelType, rbtName, false, rbtAttributes);
                }
            }

            bool bHasClearButton;
            AddFilterFindPanel(writer,
                "GroupBox",
                APanelType,
                APanelSubType,
                ctrlName,
                "null," + Environment.NewLine,
                false,
                null,
                null,
                controlAttributes,
                ATargetCodelet,
                out bHasClearButton);
        }

        /// <summary>
        /// Add an event for a control (typically based on OnChange)
        /// </summary>
        private void AddFilterFindEvent(TFormWriter writer,
            string AControlType,
            string APanelType,
            string AControlName,
            string AEventName,
            string AHandler)
        {
            ProcessTemplate snippetHandler = writer.Template.GetSnippet("SNIPDYNAMICEVENTHANDLER");

            snippetHandler.SetCodelet("CONTROLTYPE", AControlType);
            snippetHandler.SetCodelet("PANELTYPE", APanelType);
            snippetHandler.SetCodelet("CONTROLNAME", AControlName);
            snippetHandler.SetCodelet("EVENTNAME", AEventName);
            snippetHandler.SetCodelet("EVENTHANDLER", AHandler);

            writer.Template.InsertSnippet("INDIVIDUALFILTERFINDPANELEVENTS", snippetHandler);
        }

        /// <summary>
        /// Add an property for a control
        /// </summary>
        private void AddFilterFindProperty(TFormWriter writer,
            string AControlType,
            string APanelType,
            string AControlName,
            string APropertyName,
            string APropertyValue)
        {
            ProcessTemplate snippetProperty = writer.Template.GetSnippet("SNIPDYNAMICSETPROPERTY");

            snippetProperty.SetCodelet("CONTROLTYPE", AControlType);
            snippetProperty.SetCodelet("PANELTYPE", APanelType);
            snippetProperty.SetCodelet("CONTROLNAME", AControlName);
            snippetProperty.SetCodelet("PROPERTYNAME", APropertyName);
            snippetProperty.SetCodelet("PROPERTYVALUE", APropertyValue);

            writer.Template.InsertSnippet("INDIVIDUALFILTERFINDPANELPROPERTIES", snippetProperty);
        }

        /// <summary>
        /// Gets the data type for the specified column.  If the data type is a number the method also creates a shadow column in the data table
        /// so that numeric filtering is done on the basis of LIKE rather than equals.
        /// </summary>
        private string GetColumnDataType(TFormWriter writer, string ATableName, string AColumnName)
        {
            string columnDataType = null;
            TTable table = null;

            SortedList <string, TTable>DataSetTables = null;
            TCodeStorage codeStorage = writer.FCodeStorage;

            // load the dataset if there is a dataset defined for this screen. this allows us to reference customtables and custom fields
            if (codeStorage.HasAttribute("DatasetType"))
            {
                // also check the plugin directory of the yaml file, for plugins can have a file TypedDataSets.xml
                string PluginPath =
                    (writer.YamlFilename.Contains("Plugins")) ?
                    Path.GetFullPath(Path.GetDirectoryName(
                            writer.YamlFilename) + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "TypedDataSets.xml")
                    : string.Empty;

                DataSetTables = TDataBinding.LoadDatasetTables(CSParser.ICTPath, codeStorage.GetAttribute("DatasetType"), codeStorage, PluginPath);
            }

            if ((DataSetTables != null) && DataSetTables.ContainsKey(codeStorage.GetAttribute("DetailTable")))
            {
                table = DataSetTables[codeStorage.GetAttribute("DetailTable")];
            }
            else
            {
                table = TDataBinding.FPetraXMLStore.GetTable(ATableName);
            }

            if (table != null)
            {
                TTableField column = table.GetField(AColumnName);

                if (column != null)
                {
                    // We will have raised a warning if we failed to find the column
                    columnDataType = column.strType;

                    if ((columnDataType == "integer") || (columnDataType == "number"))
                    {
                        string dbColumnName = column.strName;

                        if (!listNumericColumns.Contains(dbColumnName))
                        {
                            ProcessTemplate snippetNumericColumn = writer.Template.GetSnippet("SNIPNUMERICFILTERFINDCOLUMN");
                            snippetNumericColumn.SetCodelet("DETAILTABLE", ATableName);
                            snippetNumericColumn.SetCodelet("COLUMNNAME", dbColumnName);
                            writer.Template.InsertSnippet("NUMERICFILTERFINDCOLUMNS", snippetNumericColumn);

                            listNumericColumns.Add(dbColumnName);
                        }
                    }
                }
            }

            return columnDataType;
        }
    }

    /// <summary>
    /// generator for a panel that has extended features (such as gradient background, border, shadow, etc)
    /// </summary>
    public class ExtendedPanelGenerator : PanelGenerator
    {
        private bool FManualExtendedPanel = false;

        /// <summary>constructor</summary>
        public ExtendedPanelGenerator()
            : base("pnl", typeof(Owf.Controls.A1Panel))
        {
        }

        /// <summary>check if the generator fits the given control by checking the prefix and perhaps some of the attributes</summary>
        public override bool ControlFitsNode(XmlNode curNode)
        {
            if (SimplePrefixMatch(curNode))
            {
                TControlDef ctrl = FCodeStorage.GetControl(curNode.Name);

                if (TYml2Xml.GetAttribute(curNode, "ExtendedPanel").ToLower() == "true")
                {
                    FManualExtendedPanel = true;

                    return true;
                }

                if (!ctrl.IsHorizontalGridButtonPanelStrict)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            if (!FManualExtendedPanel)
            {
                writer.SetControlProperty(ctrl, "ShadowOffSet", "0");
                writer.SetControlProperty(ctrl, "RoundCornerRadius", "0");
                writer.SetControlProperty(ctrl, "GradientStartColor", "System.Drawing.Color.AntiqueWhite");
                writer.SetControlProperty(ctrl, "GradientEndColor", "System.Drawing.Color.LightGray");
                writer.SetControlProperty(ctrl, "GradientDirection", "System.Drawing.Drawing2D.LinearGradientMode.Vertical");
            }
            else
            {
                if (ctrl.HasAttribute("ShadowOffSet"))
                {
                    writer.SetControlProperty(ctrl, "ShadowOffSet", ctrl.GetAttribute("ShadowOffSet"));
                }
                else
                {
                    writer.SetControlProperty(ctrl, "ShadowOffSet", "0");
                }

                if (ctrl.HasAttribute("RoundCornerRadius"))
                {
                    writer.SetControlProperty(ctrl, "RoundCornerRadius", ctrl.GetAttribute("RoundCornerRadius"));
                }
                else
                {
                    writer.SetControlProperty(ctrl, "RoundCornerRadius", "0");
                }

                if (ctrl.HasAttribute("GradientDirection"))
                {
                    writer.SetControlProperty(ctrl, "GradientDirection", ctrl.GetAttribute("GradientDirection"));
                }
                else
                {
                    writer.SetControlProperty(ctrl, "GradientDirection", "System.Drawing.Drawing2D.LinearGradientMode.Horizontal");
                }

                if (ctrl.HasAttribute("GradientStartColor"))
                {
                    writer.SetControlProperty(ctrl, "GradientStartColor", ctrl.GetAttribute("GradientStartColor"));
                }
                else
                {
                    writer.SetControlProperty(ctrl, "GradientStartColor", "System.Drawing.Color.Yellow");
                }

                if (ctrl.HasAttribute("GradientEndColor"))
                {
                    writer.SetControlProperty(ctrl, "GradientEndColor", ctrl.GetAttribute("GradientEndColor"));
                }
                else
                {
                    writer.SetControlProperty(ctrl, "GradientEndColor", "System.Drawing.Color.Green");
                }

                if (ctrl.HasAttribute("BorderColor"))
                {
                    writer.SetControlProperty(ctrl, "BorderColor", ctrl.GetAttribute("BorderColor"));
                }

                if (ctrl.HasAttribute("BorderWidth"))
                {
                    writer.SetControlProperty(ctrl, "BorderWidth", ctrl.GetAttribute("BorderWidth"));
                }
            }

            return writer.FTemplate;
        }

        /// <summary>
        /// Handle 'special' Panels
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            if (ctrl.IsHorizontalGridButtonPanelStrict)
            {
                writer.Template.SetCodelet("BUTTONPANEL", "true");

                XmlNode controlsNode = TXMLParser.GetChild(ctrl.xmlNode, "Controls");

                TControlDef pnlButtonsInner = writer.CodeStorage.FindOrCreateControl("pnlButtonsInner", ctrl.controlName);

                if ((controlsNode != null) && (ctrl.Children.Count == 0))
                {
                    StringCollection controlNamesCollection = TYml2Xml.GetElements(TXMLParser.GetChild(ctrl.xmlNode, "Controls"));

                    foreach (string childCtrlName in controlNamesCollection)
                    {
                        if (childCtrlName != "pnlButtonsInner")
                        {
                            TControlDef childCtrl = writer.CodeStorage.GetControl(childCtrlName);
                            TLogging.LogAtLevel(1, "Iteration 0:  Child: '" + childCtrl.controlName + "'");

                            pnlButtonsInner.Children.Add(childCtrl);
                            childCtrl.SetAttribute("Top", "3");
                            childCtrl.parentName = pnlButtonsInner.controlName;
                        }
                    }
                }

                pnlButtonsInner.SetAttribute("Dock", "Fill");
                pnlButtonsInner.SetAttribute("ControlsOrientation", "horizontal");
                pnlButtonsInner.SetAttribute("AutoScroll", "false");
                pnlButtonsInner.SetAttribute("BackColor", "System.Drawing.Color.Transparent");

                ctrl.Children.Add(pnlButtonsInner);

                TControlDef pnlButtonsRecordCounter = writer.CodeStorage.FindOrCreateControl("pnlButtonsRecordCounter", ctrl.controlName);

                pnlButtonsRecordCounter.SetAttribute("AutoSize", "true");
                pnlButtonsRecordCounter.SetAttribute("Padding", "0, 4, 5, 2");
                pnlButtonsRecordCounter.SetAttribute("Dock", "Right");
                pnlButtonsRecordCounter.SetAttribute("BackColor", "System.Drawing.Color.Transparent");

                TControlDef lblRecordCounter = writer.CodeStorage.FindOrCreateControl("lblRecordCounter", pnlButtonsRecordCounter.controlName);
                lblRecordCounter.SetAttribute("AutoSize", "true");
                lblRecordCounter.SetAttribute("Text", "n records");
                lblRecordCounter.SetAttribute("Dock", "Fill");

                if ((ctrl.HasAttribute("ShowRecordCounter"))
                    && (ctrl.GetAttribute("ShowRecordCounter").ToLower() == "false"))
                {
                    lblRecordCounter.SetAttribute("Visible", "false");
                }

                pnlButtonsRecordCounter.Children.Add(lblRecordCounter);

                if (writer.CodeStorage.GetControl(PanelGenerator.PNL_FILTER_AND_FIND) != null)
                {
                    TControlDef chkToggleFilter = writer.CodeStorage.FindOrCreateControl("chkToggleFilter", pnlButtonsRecordCounter.controlName);
                    chkToggleFilter.SetAttribute("Height", "22");
                    chkToggleFilter.SetAttribute("Dock", "Left");
                    chkToggleFilter.SetAttribute("Height", "22");
                    chkToggleFilter.SetAttribute("Tag", "SuppressChangeDetection");

                    pnlButtonsRecordCounter.Children.Add(chkToggleFilter);
                }

                ctrl.Children.Add(pnlButtonsRecordCounter);

                base.ProcessChildren(writer, ctrl);
            }
            else
            {
                base.ProcessChildren(writer, ctrl);
            }
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

        /// <summary>
        /// create the two panels
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(ctrl.GetAttribute("Panel1"));

            ctrl.Children.Add(ChildCtrl);
            ChildCtrl.parentName = ctrl.controlName;
            IControlGenerator ChildGenerator = writer.FindControlGenerator(ChildCtrl);
            ChildGenerator.GenerateControl(writer, ChildCtrl);

            ChildCtrl = ctrl.FCodeStorage.GetControl(ctrl.GetAttribute("Panel2"));
            ctrl.Children.Add(ChildCtrl);
            ChildCtrl.parentName = ctrl.controlName;
            ChildGenerator = writer.FindControlGenerator(ChildCtrl);
            ChildGenerator.GenerateControl(writer, ChildCtrl);
        }

        /// <summary>
        /// add the children to the control
        /// </summary>
        public override void AddChildren(TFormWriter writer, TControlDef ctrl)
        {
            // add one control for panel1, and one other control for panel2
            // at the moment, only one control is supported per panel of the splitcontainer
            writer.CallControlFunction(ctrl.controlName,
                "Panel1.Controls.Add(this." +
                ctrl.GetAttribute("Panel1") + ")");
            writer.CallControlFunction(ctrl.controlName,
                "Panel2.Controls.Add(this." +
                ctrl.GetAttribute("Panel2") + ")");
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

            return writer.FTemplate;
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
        public override void ProcessChildren(TFormWriter writer, TControlDef ctrl)
        {
            // add the toolbar and the print preview control
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

            ctrl.Children.Add(toolbar);
            ctrl.Children.Add(printPreview);

            IControlGenerator ctrlGenerator = writer.FindControlGenerator(toolbar);
            ctrlGenerator.GenerateControl(writer, toolbar);
            ctrlGenerator = writer.FindControlGenerator(printPreview);
            ctrlGenerator.GenerateControl(writer, printPreview);
        }
    }

    /// <summary>
    /// base class for generators for container controls
    /// </summary>
    public class ContainerGenerator : TControlGenerator
    {
        bool FCreateControlsAddStatements = true;

        /// <summary>
        /// code for creating the controls and adding them to the container
        /// </summary>
        public bool CreateControlsAddStatements
        {
            get
            {
                return FCreateControlsAddStatements;
            }

            set
            {
                FCreateControlsAddStatements = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="type"></param>
        public ContainerGenerator(string prefix, System.Type type)
            : base(prefix, type)
        {
        }

        /// constructor
        public ContainerGenerator(string prefix, System.String type)
            : base(prefix, type)
        {
        }

        /// <summary>
        /// declaring the container control
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ctrl"></param>
        public override void GenerateDeclaration(TFormWriter writer, TControlDef ctrl)
        {
            base.GenerateDeclaration(writer, ctrl);
            writer.AddContainer(ctrl.controlName);
        }

        /// <summary>
        /// generate the children, and calculate their size
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef container)
        {
            // add all the children
            if (container.Children.Count == 0)
            {
                foreach (TControlDef child in container.FCodeStorage.FSortedControlList.Values)
                {
                    if (child.parentName == container.controlName)
                    {
                        container.Children.Add(child);
                    }
                }
            }

            container.Children.Sort(new CtrlItemOrderComparer());
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef container)
        {
            base.SetControlProperties(writer, container);

            if (FCreateControlsAddStatements)
            {
                foreach (TControlDef child in container.Children)
                {
                    writer.CallControlFunction(container.controlName,
                        "Controls.Add(this." +
                        child.controlName + ")");
                }
            }

            return writer.FTemplate;
        }
    }
}