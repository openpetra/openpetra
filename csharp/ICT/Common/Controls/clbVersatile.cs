//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, christiank
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using SourceGrid;
using Ict.Common;

namespace Ict.Common.Controls
{
    /// <summary>
    /// delegate for the situation when the data column changes
    /// </summary>
    public delegate void TDataColumnChangedEventHandler(System.Object sender, DataColumnChangeEventArgs e);

    /// <summary>
    /// This control is a checked listbox, that can be automatically sorted by the
    /// checked items and the names of the items.
    /// It is derived from sgrdDatagrid, because that it is more powerful than System.Windows.Forms.CheckedListBox
    /// </summary>
    public class TClbVersatile : TSgrdDataGrid
    {
        private System.Data.DataView FDataView;
        private System.Data.DataTable FDataTable;
        private String FCheckedColumn;
        private String FKeyColumn;

        /// <summary>
        /// the number of checked items
        /// </summary>
        public System.Int32 CheckedItemsCount
        {
            get
            {
                System.Int32 ReturnValue = 0;

                foreach (DataRowView Row in FDataView)
                {
                    if (Row[FCheckedColumn].GetType() != typeof(System.DBNull))
                    {
                        if (Convert.ToBoolean(Row[FCheckedColumn]) == true)
                        {
                            ReturnValue++;
                        }
                    }
                }

                return ReturnValue;
            }
        }

        /// <summary>
        /// allows popping up a question whether to check the CheckBox
        ///
        /// </summary>
        public event TDataColumnChangedEventHandler DataColumnChanged;

        #region Creation and Disposal

        /// <summary>
        /// constructor
        /// </summary>
        public TClbVersatile() : base()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.DoubleClickCell += new TDoubleClickCellEventHandler(Clb_DoubleClickCell);
            this.SpaceKeyPressed += new TKeyPressedEventHandler(Clb_SpaceKeyPressed);
            this.EnterKeyPressed += new TKeyPressedEventHandler(Clb_EnterKeyPressed);
        }

        #endregion

        private void Clb_DoubleClickCell(System.Object Sender, SourceGrid.CellContextEventArgs e)
        {
            Int32 Row = e.CellContext.Position.Row;

            FDataView[Row][FCheckedColumn] = (System.Object)(!Convert.ToBoolean(FDataView[Row][FCheckedColumn]));
        }

        private void Clb_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            Int32 Row = e.Row;

            FDataView[Row][FCheckedColumn] = (System.Object)(!Convert.ToBoolean(FDataView[Row][FCheckedColumn]));
        }

        private void Clb_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            Int32 Row = e.Row;

            FDataView[Row][FCheckedColumn] = (System.Object)(!Convert.ToBoolean(FDataView[Row][FCheckedColumn]));
        }

        /// <summary>
        /// This will bind the table to the grid, using the first column for the checked boxes,
        /// and sorting the rest of the grid by ASortColumn.
        /// If you want to use the event DataColumnChanged, first assign the property DataColumnChanged before calling this procedure.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void DataBindGrid(System.Data.DataTable ATable,
            String ASortColumn,
            String ACheckedColumn,
            String AKeyColumn,
            String ALabelColumn,
            bool AAllowNew,
            bool AAllowEdit,
            bool AAllowDelete)
        {
            FDataTable = ATable;
            FDataView = FDataTable.DefaultView;
            FDataView.Sort = ASortColumn;
            FCheckedColumn = ACheckedColumn;
            FKeyColumn = AKeyColumn;
            FDataView.AllowNew = AAllowNew;
            FDataView.AllowEdit = AAllowEdit;
            FDataView.AllowDelete = AAllowDelete;

            // DataBind the DataGrid
            DataSource = new DevAge.ComponentModel.BoundDataView(FDataView);

            // Hook event that allows popping up a question whether to check the CheckBox
            if ((DataColumnChanged != null) && (!(DesignMode)))
            {
                FDataTable.ColumnChanged += new DataColumnChangeEventHandler(this.DataColumnChanged);
            }
        }

        private DataRowView FindString(DataTable ATable, String AColumn, String ANeedle)
        {
            DataRowView ReturnValue;
            DataView View;

            ReturnValue = null;
            View = new DataView(ATable);
            View.RowFilter = AColumn + " = '" + ANeedle + "'";

            if (View.Count > 0)
            {
                ReturnValue = View[0];
            }

            return ReturnValue;
        }

        private DataRowView FindInt32(DataTable ATable, String AColumn, System.Int32 ANeedle)
        {
            DataRowView ReturnValue;
            DataView View;

            ReturnValue = null;
            View = new DataView(ATable);

            if (ATable.Columns[AColumn].DataType == typeof(String))
            {
                View.RowFilter = AColumn + " = '" + ANeedle.ToString() + "'";
            }
            else
            {
                View.RowFilter = AColumn + " = " + ANeedle.ToString();
            }

            if (View.Count > 0)
            {
                ReturnValue = View[0];
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function returns the comma separated list of the currently selected row,
        /// identified by their codes (using FKeyColumn)
        ///
        /// </summary>
        /// <returns>void</returns>
        public String GetCheckedStringList()
        {
            String ReturnValue;

            ReturnValue = "";

            if (FDataView != null)
            {
                foreach (DataRowView Row in FDataView)
                {
                    if (Convert.ToBoolean(Row[FCheckedColumn]) == true)
                    {
                        // notice: the value in the string list might be in pairs, comma separated; addCSV will put quotes around it
                        // eg. motivation group and detail
                        ReturnValue = StringHelper.AddCSV(ReturnValue, Row[FKeyColumn].ToString());
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function returns the comma separated list of all row,
        /// identified by their codes (using FKeyColumn)
        ///
        /// </summary>
        /// <returns>String</returns>
        public String GetAllStringList()
        {
            String ReturnValue;

            ReturnValue = "";

            if (FDataView != null)
            {
                foreach (DataRowView Row in FDataView)
                {
                    // notice: the value in the string list might be in pairs, comma separated; addCSV will put quotes around it
                    // eg. motivation group and detail
                    ReturnValue = StringHelper.AddCSV(ReturnValue, Row[FKeyColumn].ToString());
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Clear the checked state for all items
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ClearSelected()
        {
            if (FDataView != null)
            {
                foreach (DataRowView Row in FDataView)
                {
                    Row[FCheckedColumn] = (System.Object)false;
                }
            }
        }

        /// <summary>
        /// This function selects the strings retrieved from the parameter AStringsToCheck;
        /// All other items are unselected.
        /// </summary>
        /// <param name="AStringsToCheck">a comma separated list of keys of the rows that should be selected</param>
        /// <returns>s false if a string could not be found in the control datasource
        /// </returns>
        public bool SetCheckedStringList(String AStringsToCheck)
        {
            bool ReturnValue;
            String itemString;
            DataRowView RowView;

            ReturnValue = true;

            // clear checked state for all items
            ClearSelected();

            while (AStringsToCheck.Length > 0)
            {
                itemString = StringHelper.GetNextCSV(ref AStringsToCheck);
                RowView = FindString(FDataTable, FKeyColumn, itemString);

                if (RowView == null)
                {
                    try
                    {
                        // Convert.ToInt32 might fail, if not a integer in the string
                        RowView = FindInt32(FDataTable, FKeyColumn, Convert.ToInt32(itemString));
                    }
                    catch (System.FormatException)
                    {
                        RowView = null;
                    }
                }

                if (RowView != null)
                {
                    RowView[FCheckedColumn] = (System.Object)true;
                }
                else
                {
                    ReturnValue = false;
                }
            }

            return ReturnValue;
        }
    }
}