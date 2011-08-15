//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2011 by OM International
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
    /// <summary>
    /// generator for the table layout panel
    /// </summary>
    public class TableLayoutPanelGenerator : TControlGenerator
    {
        private Int32 FColumnCount = -1, FRowCount = -1;

        /// <summary>
        /// constructor
        /// </summary>
        public TableLayoutPanelGenerator()
            : base("tlp", typeof(TableLayoutPanel))
        {
            FAutoSize = true;
        }

        /// <summary>
        /// count the number of generated table layout panels for the names
        /// </summary>
        public static Int32 countTableLayoutPanel = 0;

        /// <summary>
        /// generate the name for the layout panel
        /// </summary>
        /// <returns></returns>
        public string CalculateName()
        {
            countTableLayoutPanel++;
            return "tableLayoutPanel" + countTableLayoutPanel.ToString();
        }

        /// <summary>
        /// add container
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ctrl"></param>
        public override void GenerateDeclaration(TFormWriter writer, TControlDef ctrl)
        {
            base.GenerateDeclaration(writer, ctrl);
            writer.AddContainer(ctrl.controlName);
        }

        /// <summary>write the code for the designer file where the properties of the control are written</summary>
        public override ProcessTemplate SetControlProperties(TFormWriter writer, TControlDef ctrl)
        {
            ctrl.SetAttribute("Dock", "Fill");
            return base.SetControlProperties(writer, ctrl);
        }

        /// <summary>
        /// either null for no control, or TControlDef object, or string object for temporary controls.
        /// </summary>
        private System.Object[, ] FGrid;

        /// first collect everything, in the end check for unnecessary columnspan, and then write the tablelayout
        public void InitTableLayoutGrid()
        {
            FGrid = new System.Object[FColumnCount, FRowCount];
        }

        /// <summary>
        /// add controls to the TableLayoutPanel, but don't write yet;
        /// writing is done in WriteTableLayout, when the layout can be optimised
        /// </summary>
        /// <param name="childctrl"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void AddControl(
            TControlDef childctrl,
            Int32 column, Int32 row)
        {
            FGrid[column, row] = childctrl;

            childctrl.colSpan = childctrl.HasAttribute("ColSpan") ? Convert.ToInt32(childctrl.GetAttribute("ColSpan")) : 1;
            childctrl.rowSpan = childctrl.HasAttribute("RowSpan") ? Convert.ToInt32(childctrl.GetAttribute("RowSpan")) : 1;
        }

        /// <summary>
        /// add controls to the TableLayoutPanel, but don't write yet;
        /// writing is done in WriteTableLayout, when the layout can be optimised
        /// </summary>
        /// <param name="tmpChildCtrlName"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void AddControl(
            string tmpChildCtrlName,
            Int32 column, Int32 row)
        {
            FGrid[column, row] = tmpChildCtrlName;
        }

        /// <summary>
        /// optimise the table layout, and write it;
        /// Mono has some problems with columnspan and autosize columns (https://bugzilla.novell.com/show_bug.cgi?id=531591)
        /// </summary>
        public void WriteTableLayout(TFormWriter writer, string ctrlname)
        {
            List <int>SkippedColumns = new List <int>();

            // check if there are empty columns, that are always spanned; remove them
            for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
            {
                bool canSkipColumn = true;

                for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                {
                    if (FGrid[columnCounter, rowCounter] != null)
                    {
                        canSkipColumn = false;
                    }
                }

                if (canSkipColumn)
                {
                    SkippedColumns.Add(columnCounter);
                }
            }

            // check all controls in previous columns and reduce ColumnSpan if they cover the skipped column
            for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
            {
                for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                {
                    if ((FGrid[columnCounter, rowCounter] != null) && (FGrid[columnCounter, rowCounter].GetType() == typeof(TControlDef)))
                    {
                        TControlDef childctrl = (TControlDef)FGrid[columnCounter, rowCounter];

                        if (childctrl.colSpan > 1)
                        {
                            int ReduceColumnSpan = 0;

                            foreach (int SkippedColumn in SkippedColumns)
                            {
                                if ((columnCounter < SkippedColumn) && (columnCounter + childctrl.colSpan > SkippedColumn))
                                {
                                    ReduceColumnSpan++;
                                }
                            }

                            childctrl.colSpan -= ReduceColumnSpan;
                        }
                    }
                }
            }

            #region ColumnStyles and RowStyles

            writer.SetControlProperty(ctrlname, "ColumnCount", (FColumnCount - SkippedColumns.Count).ToString(), false);

            /*
             * Generate ColumnStyles which influence the width of the Columns. If custom widths are specified by the user,
             * ColumStyles with the appropriate Arguments are generated, otherwise standard ColumnStyles, which means that
             * Colum Widths are AutoSized at runtime.
             */
            for (Int32 countCol = 0; countCol < FColumnCount - SkippedColumns.Count; countCol++)
            {
                if (FColWidths != null)
                {
                    if (FColWidths.ContainsKey(countCol))
                    {
                        string[] ColWidthSpec = FColWidths[countCol].Split(':');

                        if (ColWidthSpec[0].ToLower() != "auto")
                        {
                            if (ColWidthSpec[0].ToLower() == "fixed")
                            {
                                writer.CallControlFunction(ctrlname,
                                    String.Format("ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Absolute, {0}))", ColWidthSpec[1]));
                            }
                            else if (ColWidthSpec[0].ToLower() == "percent")
                            {
                                writer.CallControlFunction(ctrlname,
                                    String.Format("ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(SizeType.Percent, {0}))", ColWidthSpec[1]));
                            }
                            else
                            {
                                throw new Exception("Invalid ColWidhts Type '" + ColWidthSpec[0] + "' for Control '" + ctrlname + "'");
                            }
                        }
                        else
                        {
                            writer.CallControlFunction(ctrlname, "ColumnStyles.Add(new System.Windows.Forms.ColumnStyle())");
                        }
                    }
                    else
                    {
                        writer.CallControlFunction(ctrlname, "ColumnStyles.Add(new System.Windows.Forms.ColumnStyle())");
                    }
                }
                else
                {
                    writer.CallControlFunction(ctrlname, "ColumnStyles.Add(new System.Windows.Forms.ColumnStyle())");
                }
            }

            writer.SetControlProperty(ctrlname, "RowCount", FRowCount.ToString(), false);

            /*
             * Generate RowStyles which influence the height of the Columns. If custom heights are specified by the user,
             * RowStyles with the appropriate Arguments are generated, otherwise standard RowStyles, which means that
             * Row Heights are AutoSized at runtime.
             */
            for (Int32 countRow = 0; countRow < FRowCount; countRow++)
            {
                if (FRowHeights != null)
                {
                    if (FRowHeights.ContainsKey(countRow))
                    {
                        string[] RowHeightSpec = FRowHeights[countRow].Split(':');

                        if (RowHeightSpec[0].ToLower() != "auto")
                        {
                            if (RowHeightSpec[0].ToLower() == "fixed")
                            {
                                writer.CallControlFunction(ctrlname,
                                    String.Format("RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Absolute, {0}))", RowHeightSpec[1]));
                            }
                            else if (RowHeightSpec[0].ToLower() == "percent")
                            {
                                writer.CallControlFunction(ctrlname,
                                    String.Format("RowStyles.Add(new System.Windows.Forms.RowStyle(SizeType.Percent, {0}))", RowHeightSpec[1]));
                            }
                            else
                            {
                                throw new Exception("Invalid RowHeights Type '" + RowHeightSpec[0] + "' for Control '" + ctrlname + "'");
                            }
                        }
                        else
                        {
                            writer.CallControlFunction(ctrlname, "RowStyles.Add(new System.Windows.Forms.RowStyle())");
                        }
                    }
                    else
                    {
                        writer.CallControlFunction(ctrlname, "RowStyles.Add(new System.Windows.Forms.RowStyle())");
                    }
                }
                else
                {
                    writer.CallControlFunction(ctrlname, "RowStyles.Add(new System.Windows.Forms.RowStyle())");
                }
            }

            #endregion

            for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
            {
                if (!SkippedColumns.Contains(columnCounter))
                {
                    for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                    {
                        if (FGrid[columnCounter, rowCounter] != null)
                        {
                            string childCtrlName;

                            if (FGrid[columnCounter, rowCounter].GetType() == typeof(TControlDef))
                            {
                                TControlDef childctrl = (TControlDef)FGrid[columnCounter, rowCounter];
                                childCtrlName = childctrl.controlName;

                                if (childctrl.colSpan > 1)
                                {
                                    writer.CallControlFunction(FTlpName,
                                        "SetColumnSpan(this." + childctrl.controlName + ", " + childctrl.colSpan + ")");
                                }

                                if (childctrl.rowSpan > 1)
                                {
                                    writer.CallControlFunction(FTlpName, "SetRowSpan(this." + childctrl.controlName + ", " + childctrl.rowSpan + ")");
                                }
                            }
                            else
                            {
                                childCtrlName = (string)FGrid[columnCounter, rowCounter];
                            }

                            int NewColumn = columnCounter;

                            foreach (int SkippedColumn in SkippedColumns)
                            {
                                if (SkippedColumn < columnCounter)
                                {
                                    NewColumn--;
                                }
                            }

                            writer.CallControlFunction(ctrlname,
                                "Controls.Add(this." +
                                childCtrlName + ", " +
                                NewColumn.ToString() + ", " + rowCounter.ToString() + ")");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// orientation of a list of controls (above each other, besides, or in a table layout)
        /// </summary>
        public enum eOrientation
        {
            /// arrange controls above each other
            Vertical,
            /// arrange controls besides each other
            Horizontal,
            /// arrange controls in a table layout
            TableLayout
        };

        /// <summary>
        /// how to arrange the controls
        /// </summary>
        protected eOrientation FOrientation = eOrientation.Vertical;
        /// <summary>
        /// cursor to determine the current row
        /// </summary>
        protected Int32 FCurrentRow = 0;
        /// <summary>
        /// cursor to determine the current column
        /// </summary>
        protected Int32 FCurrentColumn = 0;
        /// <summary>
        /// name of the current table layout panel
        /// </summary>
        protected string FTlpName = "";

        /// <summary>
        /// Holds definitions for custom ColumnStyles which influence the width of the Columns.
        /// </summary>
        protected Dictionary <int, string>FColWidths;

        /// <summary>
        /// Holds definitions for custom RowStyles which influence the height of the Rows.
        /// </summary>
        protected Dictionary <int, string>FRowHeights;

        /// <summary>
        /// set the orientation based on the attribute: ControlsOrientation;
        /// the default is vertical
        /// </summary>
        /// <param name="ACtrl"></param>
        public void SetOrientation(TControlDef ACtrl)
        {
            FOrientation = eOrientation.Vertical;

            if (TYml2Xml.HasAttribute(ACtrl.xmlNode, "ControlsOrientation")
                && (TYml2Xml.GetAttribute(ACtrl.xmlNode, "ControlsOrientation").ToLower() == "horizontal"))
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
        /// <param name="ANewWidth"></param>
        /// <param name="ANewHeight"></param>
        /// <returns>the name of the table layout control that still needs to be added to the parent</returns>
        public string CreateLayout(TFormWriter writer, string parentContainerName, StringCollection controls, Int32 ANewWidth, Int32 ANewHeight)
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
                    // one other column for the label; will be cleaned up in WriteTableLayout
                    int columnCount = 2 * TYml2Xml.GetElements(row).Count;

                    if (columnCount > FColumnCount)
                    {
                        FColumnCount = columnCount;
                    }
                }

                FRowCount = TYml2Xml.GetChildren(controlsNode, true).Count;

                InitTableLayoutGrid();

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

                InitTableLayoutGrid();

                FTlpName = CalculateName();
                TControlDef newTableLayoutPanel = writer.CodeStorage.FindOrCreateControl(FTlpName, parentContainerName);

                if (!parentContainerName.StartsWith("tableLayoutPanel"))
                {
                    TControlDef parentContainer = writer.CodeStorage.GetControl(parentContainerName);

                    if (parentContainer.HasAttribute("Height"))
                    {
                        newTableLayoutPanel.SetAttribute("Height", parentContainer.GetAttribute("Height"));
                    }

                    if (parentContainer.HasAttribute("Width") && (parentContainer.controlTypePrefix != "tlp"))
                    {
                        newTableLayoutPanel.SetAttribute("Width", parentContainer.GetAttribute("Width"));
                    }
                }

                if (ANewWidth != -1)
                {
                    newTableLayoutPanel.SetAttribute("Width", ANewWidth.ToString());
                }

                if (ANewHeight != -1)
                {
                    newTableLayoutPanel.SetAttribute("Height", ANewHeight.ToString());
                }

                GenerateDeclaration(writer, newTableLayoutPanel);
                SetControlProperties(writer, newTableLayoutPanel);

                foreach (string controlName in controls)
                {
                    TControlDef ctrl = writer.CodeStorage.GetControl(controlName);
                    ctrl.parentName = FTlpName;
                }
            }

            #region Custom Column Widths and custom Row Heights

            /*
             * Record custom Column Widths, if specified.
             */
            XmlNode colWidthsNode = TXMLParser.GetChild(containerNode, "ColWidths");

            StringCollection ColWidths = TYml2Xml.GetElements(colWidthsNode);

            if (ColWidths.Count > 0)
            {
                FColWidths = new Dictionary <int, string>();

                foreach (string colWidth in ColWidths)
                {
//                    Console.WriteLine(containerNode.Name + ".colWidth: " + colWidth + "    " + String.Format("FColWidths: {0}  /   {1})",
//                            colWidth.Substring(0, colWidth.IndexOf('=')),
//                            colWidth.Substring(colWidth.IndexOf('=') + 1)));

                    FColWidths.Add(Convert.ToInt32(colWidth.Substring(0, colWidth.IndexOf('='))),
                        colWidth.Substring(colWidth.IndexOf('=') + 1));
                }
            }

            /*
             * Record custom Row Heights, if specified.
             */
            XmlNode colHeightsNode = TXMLParser.GetChild(containerNode, "RowHeights");

            StringCollection RowHeights = TYml2Xml.GetElements(colHeightsNode);

            if (RowHeights.Count > 0)
            {
                FRowHeights = new Dictionary <int, string>();

                foreach (string rowHeight in RowHeights)
                {
//                    Console.WriteLine(containerNode.Name + ".rowHeight: " + rowHeight + "    " + String.Format("FRowHeights: {0}  /   {1})",
//                            rowHeight.Substring(0, rowHeight.IndexOf('=')),
//                            rowHeight.Substring(rowHeight.IndexOf('=') + 1)));

                    FRowHeights.Add(Convert.ToInt32(rowHeight.Substring(0, rowHeight.IndexOf('='))),
                        rowHeight.Substring(rowHeight.IndexOf('=') + 1));
                }
            }

            #endregion

            return FTlpName;
        }

        /// <summary>
        /// create the code
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ctrl"></param>
        public void CreateCode(TFormWriter writer, TControlDef ctrl)
        {
            XmlNode curNode = ctrl.xmlNode;
            IControlGenerator ctrlGenerator = writer.FindControlGenerator(ctrl);

            string controlName = ctrl.controlName;

            if (FOrientation == eOrientation.TableLayout)
            {
                if (FCurrentRow != ctrl.rowNumber)
                {
                    FCurrentColumn = 0;
                    FCurrentRow = ctrl.rowNumber;
                }
            }

            // add control itself
            if (!ctrl.controlName.StartsWith("Empty"))
            {
                ctrlGenerator.GenerateDeclaration(writer, ctrl);
                ctrlGenerator.SetControlProperties(writer, ctrl);
                ctrlGenerator.OnChangeDataType(writer, curNode, controlName);
                writer.InitialiseDataSource(curNode, controlName);

                writer.ApplyDerivedFunctionality(ctrlGenerator, curNode);
            }

/* this does not work yet; creates endless loop/recursion
 *          if (ctrl.HasAttribute("LabelUnit"))
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
 *          else
 */
            if (ctrl.HasAttribute("GenerateWithOtherControls"))
            {
                // add the checkbox/radiobutton first
                if (FOrientation == eOrientation.Vertical)
                {
                    AddControl(ctrl, 0, FCurrentRow);
                }
                else if (FOrientation == eOrientation.Horizontal)
                {
                    AddControl(ctrl, FCurrentColumn * 2, 0);
                }
                else if (FOrientation == eOrientation.TableLayout)
                {
                    AddControl(ctrl, FCurrentColumn, FCurrentRow);
                }

                StringCollection childControls = TYml2Xml.GetElements(TXMLParser.GetChild(curNode, "Controls"));

                if (childControls.Count > 1)
                {
                    // we need another tablelayout to arrange all the controls
                    TableLayoutPanelGenerator TlpGenerator = new TableLayoutPanelGenerator();
                    TlpGenerator.SetOrientation(ctrl);

                    Int32 NewHeight = -1;
                    Int32 NewWidth = -1;

                    if (ctrl.HasAttribute("Height"))
                    {
                        NewHeight = Convert.ToInt32(ctrl.GetAttribute("Height"));
                        ctrl.ClearAttribute("Height");
                    }

                    if (ctrl.HasAttribute("Width"))
                    {
                        NewWidth = Convert.ToInt32(ctrl.GetAttribute("Width"));
                        ctrl.ClearAttribute("Width");
                    }

                    string subTlpControlName = TlpGenerator.CreateLayout(writer, FTlpName, childControls, NewWidth, NewHeight);

                    foreach (string ChildControlName in childControls)
                    {
                        TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);
                        ChildControl.SetAttribute("DependsOnRadioButton", "true");
                        TlpGenerator.CreateCode(writer, ChildControl);
                    }

                    TlpGenerator.WriteTableLayout(writer, subTlpControlName);

                    if (FOrientation == eOrientation.Vertical)
                    {
                        AddControl(subTlpControlName, 1, FCurrentRow);
                    }
                    else if (FOrientation == eOrientation.Horizontal)
                    {
                        AddControl(subTlpControlName, FCurrentColumn * 2 + 1, 0);
                    }
                    else if (FOrientation == eOrientation.TableLayout)
                    {
                        AddControl(subTlpControlName, FCurrentColumn + 1, FCurrentRow);
                    }
                }
                else if (childControls.Count == 1)
                {
                    // we don't need to add another table layout for just one other control
                    TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(childControls[0]);
                    ChildCtrl.SetAttribute("DependsOnRadioButton", "true");
                    IControlGenerator ChildGenerator = writer.FindControlGenerator(ChildCtrl);
                    ChildGenerator.GenerateDeclaration(writer, ChildCtrl);
                    ChildGenerator.SetControlProperties(writer, ChildCtrl);

                    if (FOrientation == eOrientation.Vertical)
                    {
                        AddControl(ChildCtrl, 1, FCurrentRow);
                    }
                    else if (FOrientation == eOrientation.Horizontal)
                    {
                        AddControl(ChildCtrl, FCurrentColumn * 2 + 1, 0);
                    }
                    else if (FOrientation == eOrientation.TableLayout)
                    {
                        AddControl(ChildCtrl, FCurrentColumn + 1, FCurrentRow);
                    }
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
                            AddControl(ChildCtrl, 1, FCurrentRow);
                        }
                        else if (FOrientation == eOrientation.Horizontal)
                        {
                            AddControl(ChildCtrl, FCurrentColumn * 2 + 1, 0);
                        }
                    }
                }
            }
            else if (ctrl.controlName.StartsWith("Empty"))
            {
                // don't do anything here!
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
                lblGenerator.RightAlign = true;
                lblGenerator.SetControlProperties(writer, newLabel);

                AddControl(lblName,
                    FCurrentColumn * 2,
                    FCurrentRow);
                AddControl(ctrl,
                    FCurrentColumn * 2 + 1,
                    FCurrentRow);
            }
            else
            {
                // checkbox, radiobutton, groupbox, label control: no label
                // no label: merge cells
                Int32 colSpan = 1;

                if (ctrl.HasAttribute("ColSpan"))
                {
                    colSpan = Convert.ToInt32(ctrl.GetAttribute("ColSpan"));
                }

                ctrl.SetAttribute("ColSpan", (colSpan + 1).ToString());
                AddControl(ctrl,
                    FCurrentColumn * 2,
                    FCurrentRow);
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
            }
        }
    }
}