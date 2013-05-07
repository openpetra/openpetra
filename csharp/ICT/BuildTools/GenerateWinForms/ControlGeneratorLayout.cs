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
using System.Xml;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Ict.Tools.CodeGeneration;

using Ict.Common.IO;
using Ict.Common;

namespace Ict.Tools.CodeGeneration.Winforms
{
    /// <summary>
    /// generator for the table layout panel
    /// </summary>
    public class PanelLayoutGenerator : TControlGenerator
    {
        /// <summary>the space between two controls, that are beside each other</summary>
        public static Int32 HORIZONTAL_SPACE = 5;
        /// <summary>the space between two controls, that are above each other</summary>
        public static Int32 VERTICAL_SPACE = 3;
        /// <summary>the space from the top of a panel</summary>
        public static Int32 MARGIN_TOP = 7;
        /// <summary>the space from the bottom of a panel</summary>
        public static Int32 MARGIN_BOTTOM = 5;
        /// <summary>the space from the left of a panel</summary>
        public static Int32 MARGIN_LEFT = 5;

        private Int32 FColumnCount = -1, FRowCount = -1;

        /// <summary>
        /// constructor
        /// </summary>
        public PanelLayoutGenerator()
            : base("tlp", typeof(Panel))
        {
            FAutoSize = false;
        }

        /// <summary>
        /// count the number of generated table layout panels for the names
        /// </summary>
        public static Int32 countTableLayoutPanel = 0;

        private static bool HaveTestedForTextRenderer = false;
        private static bool TextRendererAvailable = false;
        private static Int32 LETTER_WIDTH = 7;
        private static Font DEFAULT_FONT = new Font("Verdana", 8.25f);

        /// <summary>
        /// measure the width of a text
        /// </summary>
        /// <param name="AText"></param>
        /// <returns></returns>
        public static Int32 MeasureTextWidth(string AText)
        {
            if (!HaveTestedForTextRenderer)
            {
                try
                {
                    TextRenderer.MeasureText("test", DEFAULT_FONT);
                    TextRendererAvailable = true;
                }
                catch (Exception)
                {
                    TextRendererAvailable = false;
                }
            }

            if (!TextRendererAvailable)
            {
                return AText.Length * LETTER_WIDTH;
            }

            return TextRenderer.MeasureText(AText, PanelLayoutGenerator.DEFAULT_FONT).Width;
        }

        /// <summary>
        /// generate the name for the layout panel
        /// </summary>
        /// <returns></returns>
        public string CalculateName()
        {
            countTableLayoutPanel++;
            return "layoutPanel" + countTableLayoutPanel.ToString();
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
            return base.SetControlProperties(writer, ctrl);
        }

        /// <summary>
        /// either null for no control, or TControlDef object
        /// </summary>
        private TControlDef[, ] FGrid;

        /// <summary>
        /// tab order for the set of controls. Can be ByColumn, or default is ByRow
        /// </summary>
        private string FTabOrder = string.Empty;

        /// first collect everything, in the end check for unnecessary columnspan, and then write the tablelayout
        public void InitTableLayoutGrid()
        {
            FGrid = new TControlDef[FColumnCount, FRowCount];
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

            if (!childctrl.hasLabel)
            {
                childctrl.colSpanWithLabel = childctrl.colSpan * 2;
            }
            else
            {
                childctrl.colSpanWithLabel = childctrl.colSpan * 2 - 1;
            }
        }

        private static int FCurrentTabIndex = 0;

        /// <summary>
        /// optimise the table layout, and write it;
        /// </summary>
        public void WriteTableLayout(TFormWriter writer, TControlDef LayoutCtrl)
        {
            // calculate the width and height for the columns and rows
            int[] ColumnWidth = new int[FColumnCount];
            int[] RowHeight = new int[FRowCount];

            // first go: ignore cells spanning rows and columns; second go: check that spanning cells fit as well
            for (int spanRunCounter = 0; spanRunCounter < 2; spanRunCounter++)
            {
                for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
                {
                    // initialise the summary values
                    if (spanRunCounter == 0)
                    {
                        ColumnWidth[columnCounter] = 0;

                        if (columnCounter == 0)
                        {
                            for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                            {
                                RowHeight[rowCounter] = 0;
                            }
                        }
                    }

                    for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                    {
                        if ((FGrid[columnCounter, rowCounter] != null))
                        {
                            TControlDef ctrl = FGrid[columnCounter, rowCounter];

                            int CellWidth = ctrl.Width;

                            if ((spanRunCounter == 0) && (ctrl.colSpanWithLabel == 1))
                            {
                                if (CellWidth > ColumnWidth[columnCounter])
                                {
                                    ColumnWidth[columnCounter] = CellWidth;
                                }
                            }
                            else
                            {
                                int CurrentSpanWidth = 0;

                                if (columnCounter + ctrl.colSpanWithLabel > FColumnCount)
                                {
                                    // TODO: make an exception again?
                                    TLogging.Log("Warning: invalid colspan " + ctrl.colSpan.ToString() + " in control " + ctrl.controlName +
                                        ". There are only " +
                                        (FColumnCount / 2).ToString() + " columns overall");

                                    ctrl.colSpanWithLabel = ctrl.colSpan;
                                }

                                for (int columnCounter2 = columnCounter; columnCounter2 < columnCounter + ctrl.colSpanWithLabel; columnCounter2++)
                                {
                                    CurrentSpanWidth += ColumnWidth[columnCounter2];
                                }

                                if (CurrentSpanWidth < CellWidth)
                                {
                                    ColumnWidth[columnCounter + ctrl.colSpanWithLabel - 1] += CellWidth - CurrentSpanWidth;
                                }
                            }

                            int CellHeight = ctrl.Height;

                            if (CellHeight == 17)
                            {
                                // for labels, we should consider the margin top as well.
                                CellHeight = 22;
                            }

                            if ((spanRunCounter == 0) && (ctrl.colSpanWithLabel == 1))
                            {
                                if (CellHeight > RowHeight[rowCounter])
                                {
                                    RowHeight[rowCounter] = CellHeight;
                                }
                            }
                            else
                            {
                                int CurrentSpanHeight = 0;

                                for (int rowCounter2 = rowCounter; rowCounter2 < rowCounter + ctrl.rowSpan; rowCounter2++)
                                {
                                    CurrentSpanHeight += RowHeight[rowCounter2];
                                }

                                if (CurrentSpanHeight < CellHeight)
                                {
                                    RowHeight[rowCounter + ctrl.rowSpan - 1] += CellHeight - CurrentSpanHeight;
                                }
                            }
                        }
                    }
                }
            }

            // now apply settings about the column width and row height
            if (FColWidths != null)
            {
                bool simpleColumnWidth = false;

                // for the simple column width specification, you need to provide a width for each column, without the label columns
                if (FColWidths.Count * 2 == FColumnCount)
                {
                    simpleColumnWidth = true;

                    for (int columnCounter = 0; columnCounter < FColWidths.Count; columnCounter++)
                    {
                        if (!FColWidths.ContainsKey(columnCounter))
                        {
                            simpleColumnWidth = false;
                        }
                    }
                }

                for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
                {
                    if (simpleColumnWidth)
                    {
                        // the specified width includes the label column
                        if (FColWidths.ContainsKey(columnCounter / 2))
                        {
                            string[] ColWidthSpec = FColWidths[columnCounter / 2].Split(':');

                            if (ColWidthSpec[0].ToLower() == "fixed")
                            {
                                ColumnWidth[columnCounter] = Convert.ToInt32(ColWidthSpec[1]) / 2;
                            }
                            else if (ColWidthSpec[0].ToLower() == "percent")
                            {
                                // TODO
                            }
                        }
                    }
                    else
                    {
                        if (FColWidths.ContainsKey(columnCounter))
                        {
                            string[] ColWidthSpec = FColWidths[columnCounter].Split(':');

                            if (ColWidthSpec[0].ToLower() == "fixed")
                            {
                                ColumnWidth[columnCounter] = Convert.ToInt32(ColWidthSpec[1]);
                            }
                            else if (ColWidthSpec[0].ToLower() == "percent")
                            {
                                // TODO
                                TLogging.Log("Warning: we currently don't support colwidth in percentage, control " + LayoutCtrl.controlName);
                            }
                        }
                    }
                }
            }

            if (FRowHeights != null)
            {
                for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                {
                    if (FRowHeights.ContainsKey(rowCounter))
                    {
                        string[] RowHeightSpec = FRowHeights[rowCounter].Split(':');

                        if (RowHeightSpec[0].ToLower() == "fixed")
                        {
                            RowHeight[rowCounter] = Convert.ToInt32(RowHeightSpec[1]);
                        }
                        else if (RowHeightSpec[0].ToLower() == "percent")
                        {
                            // TODO
                            TLogging.Log("Warning: we currently don't support rowheight in percentage, control " + LayoutCtrl.controlName);
                        }
                    }
                }
            }

            if (TLogging.DebugLevel >= 4)
            {
                StringCollection widthStringCollection = new StringCollection();

                for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
                {
                    widthStringCollection.Add(ColumnWidth[columnCounter].ToString());
                }

                TLogging.Log("column width for " + LayoutCtrl.controlName + ": " + StringHelper.StrMerge(widthStringCollection, ','));

                for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                {
                    string rowText = string.Empty;

                    for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
                    {
                        if (FGrid[columnCounter, rowCounter] != null)
                        {
                            TControlDef childctrl = FGrid[columnCounter, rowCounter];

                            for (int countspan = 0; countspan < childctrl.colSpanWithLabel; countspan++)
                            {
                                rowText += string.Format("{0}:{1} ", columnCounter + countspan, childctrl.controlName);
                            }
                        }
                    }

                    TLogging.Log(String.Format(" Row{0}: {1}", rowCounter, rowText));
                }
            }

            int Width = 0;
            int Height = 0;

            int CurrentLeftPosition = Convert.ToInt32(LayoutCtrl.GetAttribute("MarginLeft", MARGIN_LEFT.ToString()));

            for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
            {
                int CurrentTopPosition = Convert.ToInt32(LayoutCtrl.GetAttribute("MarginTop", MARGIN_TOP.ToString()));

                // only twice the margin for groupboxes
                if ((LayoutCtrl.controlTypePrefix == "grp") || (LayoutCtrl.controlTypePrefix == "rgr"))
                {
                    CurrentTopPosition += MARGIN_TOP;
                }

                for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                {
                    if (FGrid[columnCounter, rowCounter] != null)
                    {
                        TControlDef childctrl = FGrid[columnCounter, rowCounter];

                        if (childctrl.GetAttribute("Stretch") == "horizontally")
                        {
                            // use the full column width
                            // add up spanning columns
                            int concatenatedColumnWidth = ColumnWidth[columnCounter];

                            for (int colSpanCounter = 1; colSpanCounter < childctrl.colSpanWithLabel; colSpanCounter++)
                            {
                                concatenatedColumnWidth += ColumnWidth[columnCounter + colSpanCounter];
                            }

                            if (concatenatedColumnWidth > 0)
                            {
                                writer.SetControlProperty(childctrl, "Size",
                                    String.Format("new System.Drawing.Size({0}, {1})", concatenatedColumnWidth, childctrl.Height));
                            }
                        }

                        int ControlTopPosition = CurrentTopPosition;
                        int ControlLeftPosition = CurrentLeftPosition;

                        // add margin or padding
                        string padding = writer.GetControlProperty(childctrl.controlName, "Padding");

                        if (padding.Length > 0)
                        {
                            string[] values = padding.Substring(padding.IndexOf("(") + 1).Replace(")", "").Split(new char[] { ',' });
                            ControlLeftPosition += Convert.ToInt32(values[0]);
                            ControlTopPosition += Convert.ToInt32(values[1]);
                            writer.ClearControlProperty(childctrl.controlName, "Padding");
                        }

                        string margin = writer.GetControlProperty(childctrl.controlName, "Margin");

                        if (margin.Length > 0)
                        {
                            string[] values = margin.Substring(margin.IndexOf("(") + 1).Replace(")", "").Split(new char[] { ',' });
                            ControlLeftPosition += Convert.ToInt32(values[0]);
                            ControlTopPosition += Convert.ToInt32(values[1]);
                            writer.ClearControlProperty(childctrl.controlName, "Margin");
                        }

                        writer.SetControlProperty(childctrl.controlName,
                            "Location",
                            String.Format("new System.Drawing.Point({0},{1})",
                                ControlLeftPosition.ToString(),
                                ControlTopPosition.ToString()),
                            false);
                        writer.CallControlFunction(LayoutCtrl.controlName,
                            "Controls.Add(this." + childctrl.controlName + ")");

                        if (FTabOrder == "Horizontal")
                        {
                            writer.SetControlProperty(childctrl.controlName, "TabIndex", FCurrentTabIndex.ToString(), false);
                            FCurrentTabIndex += 10;
                        }
                    }

                    CurrentTopPosition += RowHeight[rowCounter];

                    CurrentTopPosition += Convert.ToInt32(LayoutCtrl.GetAttribute("VerticalSpace", VERTICAL_SPACE.ToString()));

                    if (CurrentTopPosition > Height)
                    {
                        Height = CurrentTopPosition;
                    }
                }

                CurrentLeftPosition += ColumnWidth[columnCounter];

                CurrentLeftPosition += Convert.ToInt32(LayoutCtrl.GetAttribute("HorizontalSpace", HORIZONTAL_SPACE.ToString()));

                if (CurrentLeftPosition > Width)
                {
                    Width = CurrentLeftPosition;
                }
            }

            Height +=
                Convert.ToInt32(LayoutCtrl.GetAttribute("MarginBottom", MARGIN_BOTTOM.ToString())) -
                Convert.ToInt32(LayoutCtrl.GetAttribute("VerticalSpace", VERTICAL_SPACE.ToString()));

            if (!LayoutCtrl.HasAttribute("Width"))
            {
                LayoutCtrl.SetAttribute("Width", Width.ToString());
            }
            else
            {
                Width = Convert.ToInt32(LayoutCtrl.GetAttribute("Width"));
            }

            if (!LayoutCtrl.HasAttribute("Height"))
            {
                LayoutCtrl.SetAttribute("Height", Height.ToString());
            }
            else
            {
                Height = Convert.ToInt32(LayoutCtrl.GetAttribute("Height"));
            }

            writer.SetControlProperty(LayoutCtrl, "Location", String.Format("new System.Drawing.Point({0}, {1})", MARGIN_LEFT, MARGIN_TOP));
            writer.SetControlProperty(LayoutCtrl, "Size", String.Format("new System.Drawing.Size({0}, {1})", Width, Height));

            // by default, the TabOrder is by column, Vertical
            if (FTabOrder != "Horizontal")
            {
                for (int rowCounter = 0; rowCounter < FRowCount; rowCounter++)
                {
                    for (int columnCounter = 0; columnCounter < FColumnCount; columnCounter++)
                    {
                        if (FGrid[columnCounter, rowCounter] != null)
                        {
                            TControlDef childctrl = FGrid[columnCounter, rowCounter];

                            writer.SetControlProperty(childctrl.controlName, "TabIndex", FCurrentTabIndex.ToString(), false);
                            FCurrentTabIndex += 10;
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
        /// create a new panel for the layout. eg. needed for radio buttons with depending controls
        /// </summary>
        public TControlDef CreateNewPanel(TFormWriter writer, TControlDef parentContainer)
        {
            TControlDef newTableLayoutPanel = writer.CodeStorage.FindOrCreateControl(CalculateName(), parentContainer.controlName);

            GenerateControl(writer, newTableLayoutPanel);
            return newTableLayoutPanel;
        }

        /// <summary>
        /// this function should be used for any collection of controls: on a TabPage, in a table, in a groupbox, radio button list etc.
        /// </summary>
        /// <returns>the layout control that still needs to be added to the parent</returns>
        public void CreateLayout(TFormWriter writer, TControlDef parentContainer, TControlDef layoutPanel, Int32 ANewWidth, Int32 ANewHeight)
        {
            if (layoutPanel == null)
            {
                layoutPanel = parentContainer;
            }

            // first check if the table layout has already been defined in the container with sets of rows?
            XmlNode containerNode = parentContainer.xmlNode;
            XmlNode controlsNode = TXMLParser.GetChild(containerNode, "Controls");

            if (controlsNode != null)
            {
                FTabOrder = TYml2Xml.GetAttribute(controlsNode, "TabOrder");
            }

            List <XmlNode>childNodes = TYml2Xml.GetChildren(controlsNode, true);

            if ((childNodes.Count > 0) && TYml2Xml.GetElementName(childNodes[0]).StartsWith("Row"))
            {
                // create a layout using the defined rows in Controls
                // create TableLayoutPanel that has as many columns (including the labels) and rows as needed
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

                foreach (TControlDef childctrl in parentContainer.Children)
                {
                    childctrl.parentName = layoutPanel.controlName;
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
                    FRowCount = parentContainer.Children.Count;
                }
                else if (FOrientation == eOrientation.Horizontal)
                {
                    // horizontal: label and control, all controls in one row
                    FColumnCount = parentContainer.Children.Count * 2;
                    FRowCount = 1;
                }

                InitTableLayoutGrid();

                foreach (TControlDef childControl in parentContainer.Children)
                {
                    childControl.parentName = layoutPanel.controlName;
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
        }

        /// <summary>
        /// create the code
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="ctrl"></param>
        public void InsertControl(TFormWriter writer, TControlDef ctrl)
        {
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

                StringCollection childControls = TYml2Xml.GetElements(TXMLParser.GetChild(ctrl.xmlNode, "Controls"));

                if (childControls.Count > 1)
                {
                    // we need another tablelayout to arrange all the controls
                    PanelLayoutGenerator TlpGenerator = new PanelLayoutGenerator();
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

                    TControlDef subTlpControl = TlpGenerator.CreateNewPanel(writer, ctrl);
                    TlpGenerator.CreateLayout(writer, ctrl, subTlpControl, NewWidth, NewHeight);

                    foreach (string ChildControlName in childControls)
                    {
                        TControlDef ChildControl = ctrl.FCodeStorage.GetControl(ChildControlName);
                        TlpGenerator.InsertControl(writer, ChildControl);
                    }

                    TlpGenerator.WriteTableLayout(writer, subTlpControl);

                    if (FOrientation == eOrientation.Vertical)
                    {
                        AddControl(subTlpControl, 1, FCurrentRow);
                    }
                    else if (FOrientation == eOrientation.Horizontal)
                    {
                        AddControl(subTlpControl, FCurrentColumn * 2 + 1, 0);
                    }
                    else if (FOrientation == eOrientation.TableLayout)
                    {
                        AddControl(subTlpControl, FCurrentColumn + 1, FCurrentRow);
                    }
                }
                else if (childControls.Count == 1)
                {
                    // we don't need to add another table layout for just one other control
                    TControlDef ChildCtrl = ctrl.FCodeStorage.GetControl(childControls[0]);
                    IControlGenerator ChildGenerator = writer.FindControlGenerator(ChildCtrl);
                    ChildGenerator.GenerateControl(writer, ChildCtrl);

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
            }
            else if (ctrl.controlName.StartsWith("pnlEmpty"))
            {
                // don't do anything here!
            }
            else if (ctrlGenerator.GenerateLabel(ctrl))
            {
                // add label
                LabelGenerator lblGenerator = new LabelGenerator();
                string lblName = lblGenerator.CalculateName(controlName);
                TControlDef newLabel = writer.CodeStorage.FindOrCreateControl(lblName, ctrl.controlName);
                newLabel.Label = ctrl.Label;

                if (ctrl.HasAttribute("LabelWidth"))
                {
                    newLabel.SetAttribute("Width", ctrl.GetAttribute("LabelWidth"));
                }

                if (ctrl.HasAttribute("LabelUnit"))
                {
                    // alternative implementation above does not work: add another label control after the input control
                    newLabel.Label = newLabel.Label + " (in " + ctrl.GetAttribute("LabelUnit") + ")";
                }

                lblGenerator.GenerateDeclaration(writer, newLabel);
                lblGenerator.RightAlign = true;
                lblGenerator.SetControlProperties(writer, newLabel);

                AddControl(newLabel,
                    FCurrentColumn * 2,
                    FCurrentRow);
                AddControl(ctrl,
                    FCurrentColumn * 2 + 1,
                    FCurrentRow);
            }
            else
            {
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
                FCurrentColumn += ctrl.colSpan;
            }
        }

        /// <summary>
        /// Call this Method to reset the TabIndex counter for each YAML file.
        /// </summary>
        public static void ResetTabIndex()
        {
            FCurrentTabIndex = 0;
        }
    }
}