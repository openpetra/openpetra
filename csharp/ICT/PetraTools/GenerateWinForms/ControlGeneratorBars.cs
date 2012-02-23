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