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
using System.Xml;
using System.Collections.Specialized;
using System.Windows.Forms;
using Ict.Tools.CodeGeneration;

//using Ict.Common.Controls;
using Ict.Common.IO;
using Ict.Common;

//using Ict.Petra.Client.CommonControls;

namespace Ict.Tools.CodeGeneration.Winforms
{
    public class TableLayoutPanelGenerator : TControlGenerator
    {
        private Int32 FColumnCount = -1, FRowCount = -1;
        public TableLayoutPanelGenerator()
            : base("tlp", typeof(TableLayoutPanel))
        {
            FAutoSize = true;
        }

        public static Int32 countTableLayoutPanel = 0;
        public string CalculateName()
        {
            countTableLayoutPanel++;
            return "tableLayoutPanel" + countTableLayoutPanel.ToString();
        }

        public override void GenerateDeclaration(IFormWriter writer, TControlDef ctrl)
        {
            base.GenerateDeclaration(writer, ctrl);
            writer.AddContainer(ctrl.controlName);
        }

        public override void SetControlProperties(IFormWriter writer, TControlDef ctrl)
        {
            base.SetControlProperties(writer, ctrl);
            writer.SetControlProperty(ctrl.controlName, "Dock", "System.Windows.Forms.DockStyle.Fill");
            writer.SetControlProperty(ctrl.controlName, "ColumnCount", FColumnCount.ToString());

            for (Int32 countCol = 0; countCol < FColumnCount; countCol++)
            {
                writer.CallControlFunction(ctrl.controlName, "ColumnStyles.Add(new System.Windows.Forms.ColumnStyle())");
            }

            writer.SetControlProperty(ctrl.controlName, "RowCount", FRowCount.ToString());

            for (Int32 countRow = 0; countRow < FRowCount; countRow++)
            {
                writer.CallControlFunction(ctrl.controlName, "RowStyles.Add(new System.Windows.Forms.RowStyle())");
            }
        }

        public void AddControl(IFormWriter writer,
            string tlpControlName,
            string childControlName,
            Int32 column, Int32 row)
        {
            writer.CallControlFunction(tlpControlName,
                "Controls.Add(this." +
                childControlName + ", " +
                column.ToString() + ", " + row.ToString() + ")");
        }

        public enum eOrientation
        {
            Vertical, Horizontal
        };

        protected eOrientation FOrientation = eOrientation.Vertical;
        protected Int32 FCurrentRow = 0;
        protected string FTlpName = "";

        /**
         * this function should be used for any collection of controls: on a TabPage, in a table, in a groupbox, radio button list etc.
         */
        public void CreateLayout(IFormWriter writer, string parentContainerName, StringCollection controls, eOrientation orientation)
        {
            // create TableLayoutPanel that has a column for the labels and as many rows as needed
            FOrientation = orientation;
            FCurrentRow = 0;

            if (orientation == eOrientation.Vertical)
            {
                FColumnCount = 2;
                FRowCount = controls.Count;
            }
            else
            {
                // horizontal: label and control, all controls in one row
                FColumnCount = controls.Count * 2;
                FRowCount = 1;
            }

            FTlpName = CalculateName();
            TControlDef newTableLayoutPanel = writer.CodeStorage.FindOrCreateControl(FTlpName, parentContainerName);
            GenerateDeclaration(writer, newTableLayoutPanel);
            SetControlProperties(writer, newTableLayoutPanel);

            foreach (string controlName in controls)
            {
                TControlDef ctrl = writer.CodeStorage.GetControl(controlName);
                ctrl.parentName = FTlpName;
            }

            writer.CallControlFunction(parentContainerName,
                "Controls.Add(this." +
                FTlpName + ")");
        }

        public void CreateCode(IFormWriter writer, TControlDef ctrl)
        {
            XmlNode curNode = ctrl.xmlNode;
            IControlGenerator ctrlGenerator = writer.FindControlGenerator(curNode);

            string controlName = ctrl.controlName;

            // add control itself
            ctrlGenerator.GenerateDeclaration(writer, ctrl);
            ctrlGenerator.SetControlProperties(writer, ctrl);
            ctrlGenerator.OnChangeDataType(writer, curNode, controlName);
            writer.InitialiseDataSource(curNode, controlName);

            writer.ApplyDerivedFunctionality(ctrlGenerator, curNode);

            if (ctrl.HasAttribute("GenerateWithRadioButton"))
            {
                // this is a special case: radiobutton with another control
                string rbtName = "rbt" + ctrl.controlName.Substring(ctrl.controlTypePrefix.Length);
                TControlDef rbtControl = writer.CodeStorage.GetControl(rbtName);
                RadioButtonGenerator rbtGenerator = new RadioButtonGenerator();
                rbtGenerator.GenerateDeclaration(writer, rbtControl);
                rbtGenerator.SetControlProperties(writer, rbtControl);

                // add and install event handler for change of selection
                string RadioButtonCheckedChangedName = "";

                if (RadioButtonCheckedChangedName.Length == 0)
                {
                    RadioButtonCheckedChangedName = rbtName;
                    writer.CodeStorage.FEventHandlersImplementation += "void " + rbtName + "CheckedChanged(object sender, System.EventArgs e)" +
                                                                       Environment.NewLine + "{" + Environment.NewLine + "  {#CHECKEDCHANGED_" +
                                                                       rbtName + "}" + Environment.NewLine +
                                                                       "}" + Environment.NewLine + Environment.NewLine;
                    writer.Template.AddToCodelet("INITIALISESCREEN", rbtName + "CheckedChanged(null, null);" + Environment.NewLine);
                }

                writer.Template.AddToCodelet("CONTROLINITIALISATION",
                    "this." + rbtName +
                    ".CheckedChanged += new System.EventHandler(this." +
                    RadioButtonCheckedChangedName +
                    "CheckedChanged);" + Environment.NewLine);

                // make sure the control is enabled/disabled depending on the selection of the radiobutton
                writer.Template.AddToCodelet("CHECKEDCHANGED_" + RadioButtonCheckedChangedName,
                    controlName + ".Enabled = " + rbtName + ".Checked;" + Environment.NewLine);

                if (FOrientation == eOrientation.Vertical)
                {
                    AddControl(writer, FTlpName, rbtName, 0, FCurrentRow);
                    AddControl(writer, FTlpName, controlName, 1, FCurrentRow);
                }
                else
                {
                    AddControl(writer, FTlpName, rbtName, FCurrentRow * 2, 0);
                    AddControl(writer, FTlpName, controlName, FCurrentRow * 2 + 1, 0);
                }
            }
            else if (ctrl.HasAttribute("GenerateCheckBoxWithOtherControls"))
            {
                StringCollection childControls = TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));

                // only support one child at the moment
                TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(childControls[0]);
                IControlGenerator ChildGenerator = writer.FindControlGenerator(ChildCtrl.xmlNode);
                ChildGenerator.GenerateDeclaration(writer, ChildCtrl);
                ChildGenerator.SetControlProperties(writer, ChildCtrl);

                // add and install event handler for change of selection
                writer.CodeStorage.FEventHandlersImplementation += "void " + controlName + "CheckedChanged(object sender, System.EventArgs e)" +
                                                                   Environment.NewLine + "{" + Environment.NewLine + "  {#CHECKEDCHANGED_" +
                                                                   controlName + "}" + Environment.NewLine +
                                                                   "}" + Environment.NewLine + Environment.NewLine;
                writer.Template.AddToCodelet("INITIALISESCREEN", controlName + "CheckedChanged(null, null);" + Environment.NewLine);
                writer.Template.AddToCodelet("CONTROLINITIALISATION",
                    "this." + controlName +
                    ".CheckedChanged += new System.EventHandler(this." +
                    controlName +
                    "CheckedChanged);" + Environment.NewLine);

                // make sure the control is enabled/disabled depending on the selection of the radiobutton
                writer.Template.AddToCodelet("CHECKEDCHANGED_" + controlName,
                    ChildCtrl.controlName + ".Enabled = " + controlName + ".Checked;" + Environment.NewLine);

                if (FOrientation == eOrientation.Vertical)
                {
                    AddControl(writer, FTlpName, ctrl.controlName, 0, FCurrentRow);
                    AddControl(writer, FTlpName, ChildCtrl.controlName, 1, FCurrentRow);
                }
                else
                {
                    AddControl(writer, FTlpName, ctrl.controlName, FCurrentRow * 2, 0);
                    AddControl(writer, FTlpName, ChildCtrl.controlName, FCurrentRow * 2 + 1, 0);
                }
            }
            else if (ctrlGenerator.GenerateLabel)
            {
                // add label
                LabelGenerator lblGenerator = new LabelGenerator();
                string lblName = lblGenerator.CalculateName(controlName);
                TControlDef newLabel = writer.CodeStorage.FindOrCreateControl(lblName, FTlpName);
                newLabel.Label = ctrl.Label;
                lblGenerator.GenerateDeclaration(writer, newLabel);
                lblGenerator.SetControlProperties(writer, newLabel);

                if (FOrientation == eOrientation.Vertical)
                {
                    AddControl(writer, FTlpName, lblName, 0, FCurrentRow);
                    AddControl(writer, FTlpName, controlName, 1, FCurrentRow);
                }
                else
                {
                    AddControl(writer, FTlpName, lblName, FCurrentRow * 2, 0);
                    AddControl(writer, FTlpName, controlName, FCurrentRow * 2 + 1, 0);
                }
            }
            else
            {
                // checkbox, radiobutton, groupbox: no label
                // no label: merge cells
                if (FOrientation == eOrientation.Vertical)
                {
                    AddControl(writer, FTlpName, controlName, 0, FCurrentRow);
                }
                else
                {
                    AddControl(writer, FTlpName, controlName, FCurrentRow * 2, 0);
                }

                writer.CallControlFunction(FTlpName, "SetColumnSpan(this." + controlName + ", 2)");
            }

            FCurrentRow++;
        }
    }
}