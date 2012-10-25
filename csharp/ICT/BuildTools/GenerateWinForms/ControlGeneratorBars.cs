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

        /// <summary>
        /// process the children
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef container)
        {
            // usually, the toolbar buttons are direct children of the toolbar control
            List <XmlNode>childrenlist = TYml2Xml.GetChildren(container.xmlNode, true);

            foreach (XmlNode childNode in childrenlist)
            {
                /* Get unique name if we need it
                 * at the moment we need it only for menu separators
                 */
                String UniqueChildName = childNode.Name;
                TControlDef childCtrl = container.FCodeStorage.GetControl(childNode.Name);

                if (childCtrl == null)
                {
                    UniqueChildName = TYml2Xml.GetAttribute(childNode, "UniqueName");
                    childCtrl = container.FCodeStorage.GetControl(UniqueChildName);
                }

                container.Children.Add(childCtrl);
                IControlGenerator ctrlGenerator = writer.FindControlGenerator(childCtrl);
                ctrlGenerator.GenerateControl(writer, childCtrl);
            }
        }

        /// <summary>
        /// add children to the control
        /// </summary>
        public override void AddChildren(TFormWriter writer, TControlDef container)
        {
            if (container.Children.Count > 0)
            {
                string addChildren = string.Empty;

                foreach (TControlDef child in container.Children)
                {
                    if (addChildren.Length > 0)
                    {
                        addChildren += "," + Environment.NewLine + "            ";
                    }

                    addChildren += child.controlName;
                }

                writer.CallControlFunction(container.controlName,
                    "DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {" + Environment.NewLine +
                    "               " + addChildren +
                    "})");
            }
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

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
    /// generator for the toolstrip
    /// </summary>
    public class ToolStripGenerator : TControlGenerator
    {
        /// <summary>
        /// where to dock
        /// </summary>
        public string FDocking = "Top";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="AType"></param>
        public ToolStripGenerator(string prefix, System.Type AType)
            : base(prefix, AType)
        {
            FGenerateLabel = false;
            FLocation = false;
            FDefaultHeight = 24;
            FDefaultWidth = 10;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="AType"></param>
        public ToolStripGenerator(string prefix, string AType)
            : base(prefix, AType)
        {
            FGenerateLabel = false;
            FLocation = false;
            FDefaultHeight = 24;
            FDefaultWidth = 10;
        }

        /// <summary>
        /// declare the control
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ctrl"></param>
        public override void GenerateDeclaration(TFormWriter writer, TControlDef ctrl)
        {
            base.GenerateDeclaration(writer, ctrl);
            writer.AddContainer(ctrl.controlName);
        }

        /// <summary>
        /// get the controls that belong to the toolstrip
        /// </summary>
        public override void ProcessChildren(TFormWriter writer, TControlDef container)
        {
            // add all the children

            // TODO add Container elements in statusbar
            if (container.controlName.StartsWith("stb"))
            {
                return;
            }

            List <XmlNode>childrenlist;

            if (TYml2Xml.GetChild(container.xmlNode, "Controls") != null)
            {
                // this is for generated toolbar, eg. for the PrintPreviewControl
                StringCollection childrenNames = TYml2Xml.GetElements(container.xmlNode, "Controls");
                childrenlist = new List <XmlNode>();

                foreach (string name in childrenNames)
                {
                    childrenlist.Add(container.xmlNode.OwnerDocument.CreateElement(name));
                }
            }
            else
            {
                // usually, the toolbar buttons are direct children of the toolbar control
                childrenlist = TYml2Xml.GetChildren(container.xmlNode, true);
            }

            //Console.WriteLine("Container: " + container.controlName);
            foreach (XmlNode child in childrenlist)
            {
                // Console.WriteLine("Child: " + child.Name);

                /* Get unique name if we need it
                 * at the moment we need it only for menu separators
                 */
                String UniqueChildName = child.Name;
                TControlDef ControlDefChild = container.FCodeStorage.GetControl(child.Name);

                if (ControlDefChild == null)
                {
                    UniqueChildName = TYml2Xml.GetAttribute(child, "UniqueName");
                    ControlDefChild = container.FCodeStorage.GetControl(UniqueChildName);
                }

                container.Children.Add(ControlDefChild);

                if (ControlDefChild != null)
                {
                    IControlGenerator ctrlGenerator = writer.FindControlGenerator(ControlDefChild);

                    // add control itself
                    if (ctrlGenerator != null)
                    {
                        ctrlGenerator.GenerateControl(writer, ControlDefChild);
                    }
                }
            }
        }

        /// <summary>
        /// add children to the control
        /// </summary>
        public override void AddChildren(TFormWriter writer, TControlDef container)
        {
            if (container.Children.Count > 0)
            {
                string addChildren = string.Empty;

                foreach (TControlDef child in container.Children)
                {
                    if (addChildren.Length > 0)
                    {
                        addChildren += "," + Environment.NewLine + "            ";
                    }

                    addChildren += child.controlName;
                }

                writer.CallControlFunction(container.controlName,
                    "Items.AddRange(new System.Windows.Forms.ToolStripItem[] {" + Environment.NewLine +
                    "               " + addChildren +
                    "})");
            }
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef container)
        {
            container.SetAttribute("Dock", FDocking);
            base.SetControlProperties(writer, container);

            // todo: location?
            // todo: event handler

            /*
             * this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
             * this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
             * this.toolStripMenuItem1});
             * this.menuStrip1.Location = new System.Drawing.Point(0, 0);
             * this.menuStrip1.Name = "menuStrip1";
             * this.menuStrip1.Size = new System.Drawing.Size(138, 24);
             * this.menuStrip1.TabIndex = 1;
             * this.menuStrip1.Text = "menuStrip1";
             * this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuStrip1ItemClicked);
             */

            return writer.FTemplate;
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
        
        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "Renderer", "new ToolStripProfessionalRenderer(new TOpenPetraMenuColours())");
            writer.SetControlProperty(ctrl, "GripStyle", "ToolStripGripStyle.Visible");
            writer.SetControlProperty(ctrl, "GripMargin", "new System.Windows.Forms.Padding(0)");

            return writer.FTemplate;
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
        
        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);

            writer.SetControlProperty(ctrl, "Renderer", "new ToolStripProfessionalRenderer(new TOpenPetraMenuColours())");
            writer.SetControlProperty(ctrl, "GripStyle", "ToolStripGripStyle.Visible");
            writer.SetControlProperty(ctrl, "GripMargin", "new System.Windows.Forms.Padding(3)");

            return writer.FTemplate;
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
}