//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       jakob.englert
//
// Copyright 2004-2016 by OM International
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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Controls;

using Ict.Petra.Client.App.Gui;
using Ict.Petra.Shared;

namespace Ict.Petra.Client.CommonDialogs
{
    /// manual methods for the generated window
    public partial class TFrmSelectPrintFields
    {
        DataView FView;
        DataTable fieldTable;

        string FCheckedColumn = "CHECKED";
        string FNameColumn = "Name";
        string FSortIdColumn = "sortId";

        private int[] finalColumnId;
        private int[] finalColumnOrder;

        private void RunOnceOnActivationManual()
        {
            //Removes the btnHelp buttton
            this.pnlLeftButtons.Controls.Remove(this.btnHelp);

            clbFields.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            clbFields.Columns.Clear();
            clbFields.AddCheckBoxColumn("", fieldTable.Columns[FCheckedColumn], 17, false);
            clbFields.AddTextColumn(Catalog.GetString("Column Name"), fieldTable.Columns[FNameColumn], 240);

            clbFields.DataBindGrid(fieldTable, FSortIdColumn, FCheckedColumn, FNameColumn, false, true, false);

            //Initially all selected
            clbFields.SelectAll();
        }

        private void InitializeManualCode()
        {
        }

        private void BtnOK_Click(Object Sender, EventArgs e)
        {
            String checkedListAsString = clbFields.GetCheckedStringList(false);

            if (checkedListAsString == "")
            {
                return;
            }

            String[] checkedListAsArray = checkedListAsString.Split(',');

            fieldTable.PrimaryKey = new DataColumn[] {
                fieldTable.Columns["sortId"]
            };
            finalColumnId = new int[checkedListAsArray.Length];
            finalColumnOrder = new int[checkedListAsArray.Length];

            int helperCounter = 0;

            for (int Counter2 = 0; Counter2 < fieldTable.Rows.Count; Counter2++)
            {
                DataRow foundRow = fieldTable.Rows.Find(Counter2);

                if (StringHelper.ContainsCSV(checkedListAsString, foundRow["Name"].ToString()))
                {
                    finalColumnId[helperCounter] = int.Parse(foundRow[2].ToString());
                    finalColumnOrder[helperCounter] = int.Parse(foundRow[3].ToString());
                    helperCounter++;
                }
            }

            this.DialogResult = DialogResult.OK;
        }

        private void Form_Shown(Object Sender, EventArgs e)
        {
            btnOK.Focus();
        }

        /// <summary>
        /// Initialises the data for the Select Print Fields dialog.
        /// </summary>
        /// <param name="AColumnId"></param>
        /// <param name="AGrid"></param>
        /// <param name="APreviewMode"></param>
        public void InitData(int[] AColumnId, TSgrdDataGrid AGrid, bool APreviewMode)
        {
            if (APreviewMode)
            {
                btnOK.Text = "Preview";
            }
            else
            {
                btnOK.Text = "Print";
            }

            TFrmSelectPrintFields SelectPrintFields = new TFrmSelectPrintFields(this, "bliblablub");
            DataTable fieldTable = new DataTable();
            fieldTable.Columns.Add(new DataColumn("CHECKED", typeof(bool)));
            fieldTable.Columns.Add(new DataColumn("Name", typeof(string)));
            fieldTable.Columns.Add(new DataColumn("Id", typeof(int)));
            fieldTable.Columns.Add(new DataColumn("Details", typeof(int)));
            fieldTable.Columns.Add(new DataColumn("sortId", typeof(int)));
            DataRow fieldRow;

            for (int Counter1 = 0; Counter1 < AColumnId.Length; Counter1++)
            {
                fieldRow = fieldTable.NewRow();
                fieldRow[0] = true;
                fieldRow[1] = AGrid.Columns[Counter1].HeaderCell.ToString();
                fieldRow[2] = AColumnId[Counter1];
                int index = Counter1;
                fieldRow[3] = index;
                fieldRow[4] = index;
                fieldTable.Rows.Add(fieldRow);
            }

            this.fieldTable = fieldTable;
            FView = new DataView(fieldTable);

            FView.AllowNew = false;

            clbFields.DataSource = new DevAge.ComponentModel.BoundDataView(FView);
        }

        /// <summary>
        /// Returns the ids of the selected fields.
        /// </summary>
        /// <returns></returns>
        public int[] GetColumnID()
        {
            return finalColumnId;
        }

        /// <summary>
        /// Returns the order of the column names.
        /// </summary>
        /// <returns></returns>
        public int[] GetColumnOrder()
        {
            return finalColumnOrder;
        }

        private void DataFieldMoveUp(System.Object sender, System.EventArgs e)
        {
            string[] currentPositionAsString = clbFields.Selection.ActivePosition.ToString().Split(';');
            int currentPostion = int.Parse(currentPositionAsString[0]) - 1;

            int currentRowNumber = 0;

            for (int Counter1 = 0; Counter1 < fieldTable.Rows.Count; Counter1++)
            {
                if (int.Parse(fieldTable.Rows[Counter1]["sortId"].ToString()) == currentPostion)
                {
                    currentRowNumber = Counter1;
                }
            }

            int nextRowNumber = 0;

            for (int Counter1 = 0; Counter1 < fieldTable.Rows.Count; Counter1++)
            {
                if (int.Parse(fieldTable.Rows[Counter1]["sortId"].ToString()) == currentPostion - 1)
                {
                    nextRowNumber = Counter1;
                }
            }

            if (currentPostion > 0)
            {
                fieldTable.Rows[currentRowNumber]["sortId"] = int.Parse(fieldTable.Rows[currentRowNumber]["sortId"].ToString()) - 1;
                fieldTable.Rows[nextRowNumber]["sortId"] = int.Parse(fieldTable.Rows[nextRowNumber]["sortId"].ToString()) + 1;
                clbFields.Selection.SelectRow(currentPostion, true);
            }
        }

        private void DataFieldMoveDown(System.Object sender, System.EventArgs e)
        {
            string[] currentPositionAsString = clbFields.Selection.ActivePosition.ToString().Split(';');
            int currentPostion = int.Parse(currentPositionAsString[0]) - 1;
            //Find row
            int currentRowNumber = 0;

            for (int Counter1 = 0; Counter1 < fieldTable.Rows.Count; Counter1++)
            {
                if (int.Parse(fieldTable.Rows[Counter1]["sortId"].ToString()) == currentPostion)
                {
                    currentRowNumber = Counter1;
                    break;
                }
            }

            int nextRowNumber = 0;

            for (int Counter1 = 0; Counter1 < fieldTable.Rows.Count; Counter1++)
            {
                if (int.Parse(fieldTable.Rows[Counter1]["sortId"].ToString()) == currentPostion + 1)
                {
                    nextRowNumber = Counter1;
                    break;
                }
            }

            if (currentPostion < fieldTable.Rows.Count - 1)
            {
                fieldTable.Rows[currentRowNumber]["sortId"] = int.Parse(fieldTable.Rows[currentRowNumber]["sortId"].ToString()) + 1;
                fieldTable.Rows[nextRowNumber]["sortId"] = int.Parse(fieldTable.Rows[nextRowNumber]["sortId"].ToString()) - 1;
                clbFields.Selection.SelectRow(currentPostion + 2, true);
            }
        }

        /// <summary>
        /// Print the data that is shown in a grid
        /// </summary>
        /// <param name="AParentForm">The parent form (since a modal dialog is called)</param>
        /// <param name="APrintApplication">The print application to use - either Word or Excel</param>
        /// <param name="APreviewOnly">True if preview, False to print without preview</param>
        /// <param name="AModule">The module that is making the call</param>
        /// <param name="ATitleText">Title for the page</param>
        /// <param name="AGrid">A grid displaying data</param>
        /// <param name="ATableColumnOrder">Zero-based table column order that matches the grid columns</param>
        public static void SelectAndPrintGridFields(Form AParentForm,
            TStandardFormPrint.TPrintUsing APrintApplication,
            bool APreviewOnly,
            TModule AModule,
            string ATitleText,
            TSgrdDataGrid AGrid,
            int[] ATableColumnOrder)
        {
            TFrmSelectPrintFields SelectPrintFields = new TFrmSelectPrintFields(AParentForm, "SelectPrintFields");

            SelectPrintFields.InitData(ATableColumnOrder, AGrid, APreviewOnly);

            SelectPrintFields.ShowDialog();

            if (SelectPrintFields.DialogResult == DialogResult.OK)
            {
                TStandardFormPrint.PrintGrid(APrintApplication, APreviewOnly, TModule.mPartner, ATitleText, AGrid, SelectPrintFields.GetColumnOrder(),
                    SelectPrintFields.GetColumnID());
            }
        }
    }
}