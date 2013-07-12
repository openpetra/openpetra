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
TLogging.Log("foreach (TControlDef childCtrl in ctrl.Children) -- Control: " + childCtrl.controlName);
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
                        TLogging.Log("Warning: please remove the Dock attribute from control(s) " + StringHelper.StrMerge(clearDockAttributeChildren, ','));    
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
        private const string PNL_FILTER_AND_FIND = "pnlFilterAndFind";
        
        /// <summary>constructor</summary>
        public PanelGenerator()
            : base("pnl", typeof(Panel))
        {
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

Console.WriteLine("Adjusted Height of Panel '" + ctrl.controlName + "' as it is a horizontal Grid Button Panel");
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
            if (ctrl.controlName == PNL_FILTER_AND_FIND) 
            {
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
                                
                writer.Template.SetCodelet("FINDANDFILTERAPPLYFILTERBUTTONCONTEXT", "TUcoFilterAndFind.FilterContext.fcNone");
                writer.Template.SetCodelet("FINDANDFILTERSHOWKEEPFILTERTURNEDONBUTTONCONTEXT", "TUcoFilterAndFind.FilterContext.fcNone");
                writer.Template.SetCodelet("FINDANDFILTERSHOWFILTERISALWAYSONLABELCONTEXT", "TUcoFilterAndFind.FilterContext.fcNone");
                
                writer.Template.SetCodelet("CUSTOMDISPOSING", 
                    "if (FucoFilterAndFind != null)" + Environment.NewLine + 
                    "{" + Environment.NewLine + 
                    "    FucoFilterAndFind.Dispose();" + Environment.NewLine + 
                    "}");              

                ProcessTemplate snippetFilterAndFindDeclarations = writer.Template.GetSnippet("FILTERANDFINDDECLARATIONS");                
                writer.Template.InsertSnippet("FILTERANDFINDDECLARATIONS", snippetFilterAndFindDeclarations);

                ProcessTemplate snippetFilterAndFindMethods = writer.Template.GetSnippet("FILTERANDFINDMETHODS");                
                writer.Template.InsertSnippet("FILTERANDFINDMETHODS", snippetFilterAndFindMethods);                
            }
            
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
TLogging.Log("Iteration 0:  Child: '" + childCtrl.controlName + "'");                        
                            
                            pnlButtonsInner.Children.Add(childCtrl);
                            childCtrl.SetAttribute("Top", "3");
                            childCtrl.parentName = pnlButtonsInner.controlName;                            
                        }
                    }
                }

                pnlButtonsInner.SetAttribute("Dock", "Fill");
                pnlButtonsInner.SetAttribute("ControlsOrientation", "horizontal");
                pnlButtonsInner.SetAttribute("AutoScroll", "false");
    
                ctrl.Children.Add(pnlButtonsInner);

                TControlDef pnlButtonsRecordCounter = writer.CodeStorage.FindOrCreateControl("pnlButtonsRecordCounter", ctrl.controlName);
    
                pnlButtonsRecordCounter.SetAttribute("AutoSize", "true");
                pnlButtonsRecordCounter.SetAttribute("Padding", "0, 4, 5, 2");
                pnlButtonsRecordCounter.SetAttribute("Dock", "Right");
                
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
                
                if (writer.CodeStorage.GetControl(PNL_FILTER_AND_FIND) != null)
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