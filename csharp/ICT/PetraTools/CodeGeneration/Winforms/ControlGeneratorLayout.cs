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
using System.Collections.Generic;
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
            Vertical, Horizontal, TableLayout
        };

        protected eOrientation FOrientation = eOrientation.Vertical;
        protected Int32 FCurrentRow = 0;
        protected Int32 FCurrentColumn = 0;
        protected string FTlpName = "";

        /// <summary>
        /// set the orientation based on the attribute: ControlsOrientation;
        /// the default is vertical
        /// </summary>
        /// <param name="ACtrl"></param>
        public void SetOrientation(TControlDef ACtrl)
        {
            FOrientation = eOrientation.Vertical;

            if (TXMLParser.HasAttribute(ACtrl.xmlNode, "ControlsOrientation")
                && (TXMLParser.GetAttribute(ACtrl.xmlNode, "ControlsOrientation").ToLower() == "horizontal"))
            {
                FOrientation = eOrientation.Horizontal;
            }
        }

        /// <summary>
        /// this function should be used for any collection of controls: on a TabPage, in a table, in a groupbox, radio button list etc.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parentContainerName"></param>
        /// <param name="controls"></param>
        /// <returns>the name of the table layout control that still needs to be added to the parent</returns>
        public string CreateLayout(IFormWriter writer, string parentContainerName, StringCollection controls)
        {
            // first check if the table layout has already been defined in the container with sets of rows?
            XmlNode containerNode = writer.CodeStorage.GetControl(parentContainerName).xmlNode;
            XmlNode controlsNode = TXMLParser.GetChild(containerNode, "Controls");

            List <XmlNode>childNodes = TYml2Xml.GetChildren(controlsNode, true);

            if ((childNodes.Count > 0) && childNodes[0].Name.StartsWith("Row"))
            {
                // create a layout using the defined rows in Controls
                // create TableLayoutPanel that has as many columns and rows as needed
                FOrientation = eOrientation.TableLayout;
                FCurrentRow = 0;
                FCurrentColumn = 0;
                FColumnCount = 2;

                // determine maximum number of columns
                foreach (XmlNode row in TYml2Xml.GetChildren(controlsNode, true))
                {
                    // one other column for the label
                    int columnCount = 2 * TYml2Xml.GetElements(row).Count;

                    if (columnCount > FColumnCount)
                    {
                        FColumnCount = columnCount;
                    }
                }

                FRowCount = TYml2Xml.GetChildren(controlsNode, true).Count;

                FTlpName = CalculateName();
                TControlDef newTableLayoutPanel = writer.CodeStorage.FindOrCreateControl(FTlpName, parentContainerName);
                GenerateDeclaration(writer, newTableLayoutPanel);
                SetControlProperties(writer, newTableLayoutPanel);

                foreach (string controlName in controls)
                {
                    TControlDef ctrl = writer.CodeStorage.GetControl(controlName);
                    ctrl.parentName = FTlpName;
                }
            }
            else
            {
                // create TableLayoutPanel that has a column for the labels and as many rows as needed
                FCurrentRow = 0;
                FCurrentColumn = 0;

                if (FOrientation == eOrientation.Vertical)
                {
                    FColumnCount = 2;
                    FRowCount = controls.Count;
                }
                else if (FOrientation == eOrientation.Horizontal)
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
            }

            return FTlpName;
        }

        public void CreateCode(IFormWriter writer, TControlDef ctrl)
        {
            XmlNode curNode = ctrl.xmlNode;
            IControlGenerator ctrlGenerator = writer.FindControlGenerator(curNode);

            string controlName = ctrl.controlName;

            if (FOrientation == eOrientation.TableLayout)
            {
                Console.WriteLine(controlName + " " + ctrl.rowNumber.ToString());

                if (FCurrentRow != ctrl.rowNumber)
                {
                    FCurrentColumn = 0;
                    FCurrentRow = ctrl.rowNumber;
                }
            }

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
                else if (FOrientation == eOrientation.Horizontal)
                {
                    AddControl(writer, FTlpName, rbtName, FCurrentColumn * 2, 0);
                    AddControl(writer, FTlpName, controlName, FCurrentColumn * 2 + 1, 0);
                }
            }
/* this does not work yet; creates endless loop/recursion
 *          else if (ctrl.HasAttribute("LabelUnit"))
 *          {
 *              // we need another label after the control
 *              LabelGenerator lblGenerator = new LabelGenerator();
 *              string lblName = lblGenerator.CalculateName(controlName) + "Unit";
 *              TControlDef unitLabel = writer.CodeStorage.FindOrCreateControl(lblName, controlName);
 *              unitLabel.Label = ctrl.GetAttribute("LabelUnit");
 *
 *              TableLayoutPanelGenerator TlpGenerator = new TableLayoutPanelGenerator();
 *              ctrl.SetAttribute("ControlsOrientation", "horizontal");
 *              TlpGenerator.SetOrientation(ctrl);
 *              StringCollection childControls = new StringCollection();
 *              childControls.Add(controlName);
 *              childControls.Add(lblName);
 *              string subTlpControlName = TlpGenerator.CreateLayout(writer, FTlpName, childControls);
 *
 *              TlpGenerator.CreateCode(writer, ctrl);
 *              TlpGenerator.CreateCode(writer, unitLabel);
 *
 *              if (FOrientation == eOrientation.Vertical)
 *              {
 *                  AddControl(writer, FTlpName, subTlpControlName, 1, FCurrentRow);
 *              }
 *              else
 *              {
 *                  AddControl(writer, FTlpName, subTlpControlName, FCurrentColumn * 2 + 1, 0);
 *              }
 *          }
 */
            else if (ctrl.HasAttribute("GenerateCheckBoxWithOtherControls"))
            {
                // add the checkbox first
                if (FOrientation == eOrientation.Vertical)
                {
                    AddControl(writer, FTlpName, ctrl.controlName, 0, FCurrentRow);
                }
                else if (FOrientation == eOrientation.Horizontal)
                {
                    AddControl(writer, FTlpName, ctrl.controlName, FCurrentColumn * 2, 0);
                }

                StringCollection childControls = TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));

                if (childControls.Count > 1)
                {
                    // we need another tablelayout to arrange all the controls
                    TableLayoutPanelGenerator TlpGenerator = new TableLayoutPanelGenerator();
                    TlpGenerator.SetOrientation(ctrl);
                    string subTlpControlName = TlpGenerator.CreateLayout(writer, FTlpName, childControls);

                    foreach (string ChildControlName in childControls)
                    {
                        TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);
                        TlpGenerator.CreateCode(writer, ChildControl);
                    }

                    if (FOrientation == eOrientation.Vertical)
                    {
                        AddControl(writer, FTlpName, subTlpControlName, 1, FCurrentRow);
                    }
                    else if (FOrientation == eOrientation.Horizontal)
                    {
                        AddControl(writer, FTlpName, subTlpControlName, FCurrentColumn * 2 + 1, 0);
                    }
                }
                else if (childControls.Count == 1)
                {
                    // we don't need to add another table layout for just one other control
                    TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(childControls[0]);
                    IControlGenerator ChildGenerator = writer.FindControlGenerator(ChildCtrl.xmlNode);
                    ChildGenerator.GenerateDeclaration(writer, ChildCtrl);
                    ChildGenerator.SetControlProperties(writer, ChildCtrl);
                }

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
                writer.Template.AddToCodelet("INITACTIONSTATE", controlName + "CheckedChanged(null, null);" + Environment.NewLine);

                foreach (string childName in childControls)
                {
                    TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(childName);

                    // make sure the control is enabled/disabled depending on the selection of the radiobutton
                    writer.Template.AddToCodelet("CHECKEDCHANGED_" + controlName,
                        ChildCtrl.controlName + ".Enabled = " + controlName + ".Checked;" + Environment.NewLine);

                    if (childControls.Count == 1)
                    {
                        if (FOrientation == eOrientation.Vertical)
                        {
                            AddControl(writer, FTlpName, ChildCtrl.controlName, 1, FCurrentRow);
                        }
                        else if (FOrientation == eOrientation.Horizontal)
                        {
                            AddControl(writer, FTlpName, ChildCtrl.controlName, FCurrentColumn * 2 + 1, 0);
                        }
                    }
                }
            }
            else if (ctrlGenerator.GenerateLabel(ctrl))
            {
                // add label
                LabelGenerator lblGenerator = new LabelGenerator();
                string lblName = lblGenerator.CalculateName(controlName);
                TControlDef newLabel = writer.CodeStorage.FindOrCreateControl(lblName, FTlpName);
                newLabel.Label = ctrl.Label;

                if (ctrl.HasAttribute("LabelUnit"))
                {
                    // alternative implementation above does not work: add another label control after the input control
                    newLabel.Label = newLabel.Label + " (in " + ctrl.GetAttribute("LabelUnit") + ")";
                }

                lblGenerator.GenerateDeclaration(writer, newLabel);
                lblGenerator.SetControlProperties(writer, newLabel);

                AddControl(writer, FTlpName, lblName, FCurrentColumn * 2, FCurrentRow);
                AddControl(writer, FTlpName, controlName, FCurrentColumn * 2 + 1, FCurrentRow);
            }
            else
            {
                // checkbox, radiobutton, groupbox: no label
                // no label: merge cells
                AddControl(writer, FTlpName, controlName, FCurrentColumn * 2, FCurrentRow);
                writer.CallControlFunction(FTlpName, "SetColumnSpan(this." + controlName + ", 2)");
            }

            if (FOrientation == eOrientation.Vertical)
            {
                FCurrentRow++;
                FCurrentColumn = 0;
            }
            else if (FOrientation == eOrientation.Horizontal)
            {
                FCurrentColumn++;
            }
            else if (FOrientation == eOrientation.TableLayout)
            {
                FCurrentColumn++;

                // TODO: Colspan?
            }
        }
    }
}