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
using System.Collections.Generic;
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
        private List <String>FKeyColumns;

        /// <summary>
        ///
        /// </summary>
        public DataTable BoundDataTable
        {
            get
            {
                return FDataTable;
            }
        }
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
        /// The checked column
        /// </summary>
        public string CheckedColumn
        {
            set
            {
                FCheckedColumn = value;
            }
        }

        /// <summary>
        /// allows popping up a question whether to check the CheckBox
        ///
        /// </summary>
        public event EventHandler ValueChanged;

        #region Creation and Disposal

        /// <summary>
        /// constructor
        /// </summary>
        public TClbVersatile() : base()
        {
            InitializeComponent();
            FKeyColumns = new List <String>();
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

            if (FDataView[Row][FCheckedColumn].GetType() != typeof(System.DBNull))
            {
                FDataView[Row][FCheckedColumn] = (System.Object)(!Convert.ToBoolean(FDataView[Row][FCheckedColumn]));
            }
        }

        private void Clb_EnterKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            Int32 Row = e.Row;

            if (FDataView[Row][FCheckedColumn].GetType() != typeof(System.DBNull))
            {
                FDataView[Row][FCheckedColumn] = (System.Object)(!Convert.ToBoolean(FDataView[Row][FCheckedColumn]));
            }
        }

        private void Clb_SpaceKeyPressed(System.Object Sender, SourceGrid.RowEventArgs e)
        {
            Int32 Row = e.Row;

            if (FDataView[Row][FCheckedColumn].GetType() != typeof(System.DBNull))
            {
                FDataView[Row][FCheckedColumn] = (System.Object)(!Convert.ToBoolean(FDataView[Row][FCheckedColumn]));
            }
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
            bool AAllowNew,
            bool AAllowEdit,
            bool AAllowDelete)
        {
            List <String>KeyColumns = new List <String>();
            KeyColumns.Add(AKeyColumn);

            DataBindGrid(ATable, ASortColumn, ACheckedColumn, KeyColumns, AAllowNew, AAllowEdit, AAllowDelete);
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
            List <String>AKeyColumns,
            bool AAllowNew,
            bool AAllowEdit,
            bool AAllowDelete)
        {
            FDataTable = ATable;
            FDataView = FDataTable.DefaultView;
            FDataView.Sort = ASortColumn;
            FCheckedColumn = ACheckedColumn;
            FKeyColumns = new List <String>(AKeyColumns);
            FDataView.AllowNew = AAllowNew;
            FDataView.AllowEdit = AAllowEdit;
            FDataView.AllowDelete = AAllowDelete;

            // DataBind the DataGrid
            DataSource = new DevAge.ComponentModel.BoundDataView(FDataView);
            this.SelectRowWithoutFocus(1);

            // Hook event that allows popping up a question whether to check the CheckBox
            if (!DesignMode)
            {
                FDataTable.ColumnChanged += MyDataColumnChangedEventHandler;
            }
        }

        private void MyDataColumnChangedEventHandler(object sender, DataColumnChangeEventArgs e)
        {
            if (this.ValueChanged != null)
            {
                ValueChanged(this, null);
            }
        }

        private DataRowView FindRow(DataTable ATable, List <String>AColumns, List <String>ANeedles)
        {
            DataRowView ReturnValue;
            DataView View;
            String Filter = "";
            int Counter = 0;

            ReturnValue = null;
            View = new DataView(ATable);

            for (Counter = 0; Counter < AColumns.Count; Counter++)
            {
                if (Counter > 0)
                {
                    Filter = Filter + " AND ";
                }

                if (ATable.Columns[AColumns[Counter]].DataType == typeof(String))
                {
                    Filter = Filter + AColumns[Counter] + " = '" + ANeedles[Counter] + "'";
                }
                else
                {
                    Filter = Filter + AColumns[Counter] + " = " + ANeedles[Counter];
                }
            }

            View.RowFilter = Filter;

            if (View.Count > 0)
            {
                ReturnValue = View[0];
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function returns the comma separated list of the currently selected row,
        /// identified by their codes (using FKeyColumns)
        ///
        /// </summary>
        /// <returns>void</returns>
        public String GetCheckedStringList(Boolean AddQuotes = false)
        {
            String ReturnValue;
            Boolean RetEmpty = true;

            ReturnValue = "";

            // The values in the string list might be in pairs, comma separated,
            // eg. motivation group and detail.
            // If this is the case, the AddQuotes option should be specified.

            String OptionalQuote = AddQuotes ? "\"" : "";

            if (FDataView != null)
            {
                foreach (DataRowView Row in FDataView)
                {
                    if (Row[FCheckedColumn].GetType() != typeof(System.DBNull))
                    {
                        if (Convert.ToBoolean(Row[FCheckedColumn]) == true)
                        {
                            foreach (String KeyColumn in FKeyColumns)
                            {
                                if (!RetEmpty)
                                {
                                    ReturnValue += ",";
                                }

                                RetEmpty = false;

                                ReturnValue += (OptionalQuote + Row[KeyColumn].ToString() + OptionalQuote);
                                // This was changed from AddCsv because
                            }   // I need it to consistently add quotes to all of the values in the list
                        }       // (Or no quotes would also be fine, but not some with and some without!)
                    }           // AddCsv Adds quotes if the string has leading zeroes,

                    // so for example it adds quotes to Cost Code "0300" but not 3000.
                    // Tim Ingham, Nov 2013, Jan 2014
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// This function returns the comma separated list of all rows,
        /// identified by their codes (using FKeyColumns)
        ///
        /// </summary>
        /// <returns>String</returns>
        public String GetAllStringList(Boolean AddQuotes = false)
        {
            String ReturnValue = "";
            Boolean RetEmpty = true;
            String OptionalQuote = AddQuotes ? "\"" : "";

            if (FDataView != null)
            {
                foreach (DataRowView Row in FDataView)
                {
                    foreach (String KeyColumn in FKeyColumns)
                    {
                        if (!RetEmpty)
                        {
                            ReturnValue += ",";
                        }

                        RetEmpty = false;

                        ReturnValue += (OptionalQuote + Row[KeyColumn].ToString() + OptionalQuote);
                        // This was changed from AddCsv because
                    }   // I need it to consistently add quotes to all of the values in the list

                }       // (Or no quotes would also be fine, but not some with and some without!)

            }           // AddCsv Adds quotes if the string has leading zeroes,

            // so for example it adds quotes to Cost Code "0300" but not 3000.
            // Tim Ingham, Apr 2015

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
        /// Clear the checked state for all items
        ///
        /// </summary>
        /// <returns>void</returns>
        public void SelectAll()
        {
            if (FDataView != null)
            {
                foreach (DataRowView Row in FDataView)
                {
                    Row[FCheckedColumn] = (System.Object)true;
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

            List <String>ItemStringList = new List <String>();;
            DataRowView RowView;
            int Counter;

            ReturnValue = true;

            // clear checked state for all items
            ClearSelected();

            if (FKeyColumns.Count == 0)
            {
                ReturnValue = false;
            }

            if (ReturnValue != false)
            {
                while (AStringsToCheck.Length > 0)
                {
                    ItemStringList.Clear();

                    for (Counter = 0; Counter < FKeyColumns.Count; Counter++)
                    {
                        ItemStringList.Add(StringHelper.GetNextCSV(ref AStringsToCheck));
                    }

                    RowView = FindRow(FDataTable, FKeyColumns, ItemStringList);

                    if (RowView != null)
                    {
                        RowView[FCheckedColumn] = (System.Object)true;
                    }
                    else
                    {
                        ReturnValue = false;
                    }
                }
            }

            return ReturnValue;
        }
    }
}
