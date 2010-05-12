//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank
//
// Copyright 2004-2010 by OM International
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
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Ict.Petra.Client.CommonControls
{
    /// <summary>
    /// Rudimentary inplementation of a DataGrid that prevents editing of columns.
    /// Not used yet, not much tested either.
    ///
    /// </summary>
    public class TUneditableDataGrid : DataGrid
    {
        private const Int32 OFFSET = 5;

        #region TUneditableDataGrid

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            MouseEventArgs e1;

            DataGrid.HitTestInfo hti;
            hti = this.HitTest(e.X, e.Y);

            if (hti.Type == HitTestType.Cell)
            {
                e1 = new MouseEventArgs(e.Button, e.Clicks, OFFSET, e.Y, e.Delta);
                base.OnMouseDown(e1);
            }
            else
            {
                base.OnMouseDown(e);
            }
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="e"></param>
        protected override void OnCurrentCellChanged(System.EventArgs e)
        {
            MouseEventArgs e1;

            System.Drawing.Rectangle rect;
            base.OnCurrentCellChanged(e);

            if (Control.MouseButtons != System.Windows.Forms.MouseButtons.Left)
            {
                rect = this.GetCellBounds(this.CurrentCell);
                e1 = new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 0, OFFSET, (rect.Y + OFFSET), 0);
                OnMouseDown(e1);
            }
        }

        #endregion
    }

    /// <summary>
    /// A DataGrid that provides the ability to prevent sorting of certain columns
    /// and that fires an OnSort event as data is about to be sorted.
    /// Additionally, the PrimaryKey(s) of the row that was selected before the
    /// DataGrid was sorted can be retrieved.
    ///
    /// </summary>
    public class TDataGridControlledSort : DataGrid
    {
        /// <summary>Holds the value of NotSortableColumns property to prevent sorting of certain columns</summary>
        private ArrayList FNotSortableColumns;

        /// <summary>Holds an array of PrimaryKeys after the DataGrid was sorted.</summary>
        private object[] FCurrentlySelectedRowPK;

        /// <summary>Stores the last column that the DataGrid was sorted on</summary>
        private String FLastSortedColumn;

        /// <summary>
        /// Disallows sorting of all columns which names are passed in the ArrayList
        ///
        /// </summary>
        public ArrayList NotSortableColumns
        {
            get
            {
                return FNotSortableColumns;
            }

            set
            {
                FNotSortableColumns = value;
            }
        }

        /// <summary>Event that fires when the DataGrid is about to be sorted.</summary>
        public event System.EventHandler OnSort;

        #region TDataGridControlledSort

        /// <summary>
        /// constructor
        /// </summary>
        public TDataGridControlledSort()
            : base()
        {
            FNotSortableColumns = new ArrayList();
        }

        /// <summary>
        /// Can be called by the form/control that hosts this DataGrid to retrieve the
        /// PrimaryKey(s) of the row that was selected before the DataGrid was sorted.
        ///
        /// </summary>
        /// <param name="APKArray">Array of PrimaryKeys
        /// </param>
        /// <returns>void</returns>
        public void GetCurrentlySelectedRowPKAfterSorting(out object[] APKArray)
        {
            APKArray = FCurrentlySelectedRowPK;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AEvent"></param>
        protected override void OnMouseDown(MouseEventArgs AEvent)
        {
            DataGrid.HitTestInfo Hti;
            Point pt;
            pt = new Point(AEvent.X, AEvent.Y);
            Hti = this.HitTest(pt);

            // check if column header was clicked
            if (Hti.Type == HitTestType.ColumnHeader)
            {
                // check if a column that is listed in NotSortableColumns was clicked
                if (FNotSortableColumns.Contains((System.Object)Hti.Column))
                {
                    // don't sort this column
                    return;

                    // don't call baseclass
                }
            }

            base.OnMouseDown(AEvent);
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="AEvent"></param>
        protected override void OnMouseUp(MouseEventArgs AEvent)
        {
            DataGrid.HitTestInfo Hti;
            Point pt;
            CurrencyManager DataTableCurrencyManager;
            DataRowView CurrentRowView;
            DataColumn[] PrimaryKeyColumns;
            Int16 Counter;
            String ColumnName;
            pt = new Point(AEvent.X, AEvent.Y);
            Hti = this.HitTest(pt);

            // check if column header was clicked
            if (Hti.Type == HitTestType.ColumnHeader)
            {
                // Get the CurrencyManager for the bound DataTable
                DataTableCurrencyManager = (CurrencyManager) this.BindingContext[this.DataSource];
                CurrentRowView = (DataRowView)DataTableCurrencyManager.Current;
                PrimaryKeyColumns = ((DataView) this.DataSource).Table.PrimaryKey;
                FCurrentlySelectedRowPK = new object[PrimaryKeyColumns.Length];

                // messagebox.show('Length(PrimaryKeyColumns): ' + Convert.ToInt16(Length(PrimaryKeyColumns)).ToString);
                for (Counter = 0; Counter <= PrimaryKeyColumns.Length - 1; Counter += 1)
                {
                    FCurrentlySelectedRowPK[Counter] = (System.Object)CurrentRowView;

                    // messagebox.show('PK column: ' + PrimaryKeyColumns[Counter].ToString + ';  PK value: ' + FCurrentlySelectedRowPK[Counter].ToString);
                }

                // Fire OnSort event
                if (OnSort != null)
                {
                    OnSort(this, EventArgs.Empty);
                    ColumnName = ((DataView) this.DataSource).Table.Columns[Hti.Column].ColumnName;

                    // Do sorting of a column first on our own and let only
                    // subsequent sorts of the same column be done by the DataGrid.
                    // The reason: The DataGrid doesn't sort correctly when loading data in this event handler
                    // (strange but true!)
                    if (FLastSortedColumn != ColumnName)
                    {
                        FLastSortedColumn = ColumnName;
                        ((DataView) this.DataSource).Sort = ColumnName + " ASC";
                        return;
                    }
                }
            }

            base.OnMouseUp(AEvent);
        }

        /// <summary>
        /// Needs to be called by the form/control that hosts this DataGrid to set the
        /// Array of PrimaryKeys to nil after the form/control has eg. re-selected the
        /// row that was selected before.
        ///
        /// </summary>
        /// <returns>void</returns>
        public void ResetCurrentlySelectedRowPKAfterSorting()
        {
            FCurrentlySelectedRowPK = null;
        }

        #endregion
    }
}