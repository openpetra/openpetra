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
}