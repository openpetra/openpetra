//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       markusm, timop
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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Ict.Common;

namespace Ict.Common.Controls
{
    /// The cmbVersatile ComboBox is a ComboBox which offers the possibility of having
    /// up to 4 columns in its drop down pane. Within this columns can be either some
    /// text or an 16 by 16 image. The cmbVersatile ComboBox has several new properties.
    /// Here the user is not allowed to add new items to the ComboBox. The following
    /// properties are essential for this control to work:
    /// - DataSource:
    ///   Here the underlying table / view has to be set. This is mostly done in the
    ///   following way:
    ///     cmbVersatile.BeginUpdate();
    ///     cmbVersatile.DataSource := YOUR_DATA_TABLE / YOUR_DATA_VIEW;
    ///     cmbVersatile.DisplayMember := COLUMN NAME;
    ///     cmbVersatile.ValueMember := COLUMN NAME;
    ///     cmbVersatile.EndUpdate();
    /// - DisplayInColumn1 ... DisplayIn Column4:
    ///   Here the colomn of the underlying DataSource table / view has to be set. If
    ///   no table column name for the 1st column of the drop down is given
    ///   DisplayMember respectively ValueMember is used.
    /// - ColumnWidthCol1 ... ColumnWidthCol4:
    ///   Here the width of the columns of the drop down pane is adjusted. Please note
    ///   that it is not allowed to set the with of a previous column to 0 and the next
    ///   column to a value other than 0 (ColumnWidthCol2 = 0, ColumnWidthCol3 = 100 is
    ///   a forbidden scenario.
    /// - GridLine... ;
    ///   If set to true a grid line in the drop down pane is drawn
    /// - Images:
    ///   Here an ImageList is needed. All images in that list have indices. If you note
    ///   these indices in one column of the DataSource the cmbVersatile ComboBox will
    ///   automatically grab these images and display them in the colomn you assign.
    /// - ImageColumn:
    ///   The column number of the drop down's column which holds the image indices.
    ///   Please note: 0 refers to no column!, 1 to column 1 ... 4 to column 4.
    /// TODO: Known features from .Net:
    /// The databinding is a little bit troublesome. You can use it if the rows of the
    /// DataSource table have been added with the routine 'Add'. Otherwise the databinding
    /// just breaks. For additional information on this topic visit the Petra - Wiki.
    /// Here is the link to the complete article:
    /// TODO: http://ict.om.org/PetraWiki/current/index.php?title=Problems_in_DataBinding_a_View_to_a_ComboBox
    public class TCmbVersatile : TCmbAutoComplete
    {
        private const Int32 UNIT_SLIDER_WIDTH = 20;

        private int FColumnNum;
        private int FColumnWidthCol1;
        private int FColumnWidthCol2;
        private int FColumnWidthCol3;
        private int FColumnWidthCol4;
        private string FDisplayInColumn1;
        private string FDisplayInColumn2;
        private string FDisplayInColumn3;
        private string FDisplayInColumn4;
        private bool FGridLineVertical;
        private bool FGridLineHorizontal;
        private System.Drawing.Color FGridLineColor;
        private int FImageColumn;
        private System.Windows.Forms.ImageList FImages;

        /**
         * This property determines which column should be sorted. This may be esential
         * for heirs of this class which use more the one column in the combobox. The
         * default value for this property is therefore NIL.
         *
         */
        [ReadOnlyAttribute(true)]
        public new bool AcceptNewValues
        {
            get
            {
                return base.AcceptNewValues;
            }
        }

        /// <summary>
        /// This property manages number of columns within the ComboBox.
        ///
        /// </summary>
        public int ColumnNum
        {
            get
            {
                return this.FColumnNum;
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol1
        {
            get
            {
                return this.FColumnWidthCol1;
            }

            set
            {
                System.Int32 WholeWidth;

                if (value >= 0)
                {
                    if ((value == 0) && (this.FColumnWidthCol2 == 0) && (this.FColumnWidthCol3 == 0) && (this.FColumnWidthCol4 == 0))
                    {
                        if (this.FColumnWidthCol1 < 0)
                        {
                            this.FColumnWidthCol1 = this.Width - UNIT_SLIDER_WIDTH;
                        }
                    }
                    else
                    {
                        this.FColumnWidthCol1 = value;
                    }

                    WholeWidth = this.FColumnWidthCol1 + this.FColumnWidthCol2 + this.FColumnWidthCol3 + this.FColumnWidthCol4 + UNIT_SLIDER_WIDTH;
                    DropDownWidth = WholeWidth;
                    this.CheckColumnNumber();
                }
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol2
        {
            get
            {
                return this.FColumnWidthCol2;
            }

            set
            {
                System.Int32 WholeWidth;

                if ((value >= 0) && (this.FColumnWidthCol1 > 0))
                {
                    if ((value == 0) && (this.FColumnWidthCol1 == 0) && (this.FColumnWidthCol3 == 0) && (this.FColumnWidthCol4 == 0))
                    {
                        if (this.FColumnWidthCol2 < 0)
                        {
                            this.FColumnWidthCol2 = this.Width - UNIT_SLIDER_WIDTH;
                        }
                    }
                    else
                    {
                        this.FColumnWidthCol2 = value;
                    }

                    WholeWidth = this.FColumnWidthCol1 + this.FColumnWidthCol2 + this.FColumnWidthCol3 + this.FColumnWidthCol4 + UNIT_SLIDER_WIDTH;
                    DropDownWidth = WholeWidth;
                    this.CheckColumnNumber();
                }
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol3
        {
            get
            {
                return this.FColumnWidthCol3;
            }

            set
            {
                System.Int32 WholeWidth;

                if ((value >= 0) && (this.FColumnWidthCol1 > 0) && (this.FColumnWidthCol2 > 0))
                {
                    if ((value == 0) && (this.FColumnWidthCol1 == 0) && (this.FColumnWidthCol2 == 0) && (this.FColumnWidthCol4 == 0))
                    {
                        if (this.FColumnWidthCol3 < 0)
                        {
                            this.FColumnWidthCol3 = this.Width - UNIT_SLIDER_WIDTH;
                        }
                    }
                    else
                    {
                        this.FColumnWidthCol3 = value;
                    }

                    WholeWidth = this.FColumnWidthCol1 + this.FColumnWidthCol2 + this.FColumnWidthCol3 + this.FColumnWidthCol4 + UNIT_SLIDER_WIDTH;
                    this.DropDownWidth = WholeWidth;
                    this.CheckColumnNumber();
                }
            }
        }

        /// <summary>
        /// This property manages the width of a column within the ComboBox.
        ///
        /// </summary>
        public int ColumnWidthCol4
        {
            get
            {
                return this.FColumnWidthCol4;
            }

            set
            {
                System.Int32 WholeWidth;

                if ((value >= 0) && (this.FColumnWidthCol1 > 0) && (this.FColumnWidthCol2 > 0) && (this.FColumnWidthCol3 > 0))
                {
                    if ((value == 0) && (this.FColumnWidthCol1 == 0) && (this.FColumnWidthCol2 == 0) && (this.FColumnWidthCol3 == 0))
                    {
                        if (this.FColumnWidthCol4 <= 0)
                        {
                            this.FColumnWidthCol4 = this.Width - UNIT_SLIDER_WIDTH;
                        }
                    }
                    else
                    {
                        this.FColumnWidthCol4 = value;
                    }

                    WholeWidth = this.FColumnWidthCol1 + this.FColumnWidthCol2 + this.FColumnWidthCol3 + this.FColumnWidthCol4 + UNIT_SLIDER_WIDTH;
                    DropDownWidth = WholeWidth;
                    this.CheckColumnNumber();
                }
            }
        }

        /// <summary>
        /// This property manages the vertical grid line in the drop down list.
        ///
        /// </summary>
        public bool GridLineVertical
        {
            get
            {
                return this.FGridLineVertical;
            }

            set
            {
                this.FGridLineVertical = value;
            }
        }

        /// <summary>
        /// This property manages the horizontal grid line in the drop down list.
        ///
        /// </summary>
        public bool GridLineHorizontal
        {
            get
            {
                return this.FGridLineHorizontal;
            }

            set
            {
                this.FGridLineHorizontal = value;
            }
        }

        /// <summary>
        /// This property manages the color of the grid lines in the drop down list.
        ///
        /// </summary>
        public System.Drawing.Color GridLineColor
        {
            get
            {
                return this.FGridLineColor;
            }

            set
            {
                this.FGridLineColor = value;
            }
        }

        /// <summary>
        /// This property manages the image column.
        ///
        /// </summary>
        public int ImageColumn
        {
            get
            {
                return this.FImageColumn;
            }

            set
            {
                this.FImageColumn = value;
            }
        }

        /// <summary>
        /// This property holds the images.
        ///
        /// </summary>
        public System.Windows.Forms.ImageList Images
        {
            get
            {
                return this.FImages;
            }

            set
            {
                this.FImages = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 1st column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn1
        {
            get
            {
                return this.FDisplayInColumn1;
            }

            set
            {
                this.FDisplayInColumn1 = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 2nd column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn2
        {
            get
            {
                return this.FDisplayInColumn2;
            }

            set
            {
                this.FDisplayInColumn2 = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 3rd column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn3
        {
            get
            {
                return this.FDisplayInColumn3;
            }

            set
            {
                this.FDisplayInColumn3 = value;
            }
        }

        /// <summary>
        /// This property manages the content of the 4th column in the combobox
        ///
        /// </summary>
        public string DisplayInColumn4
        {
            get
            {
                return this.FDisplayInColumn4;
            }

            set
            {
                this.FDisplayInColumn4 = value;
            }
        }

        /// <summary>
        /// This property controls whether items in the list portion are sorted. Here
        /// it is only in the declaration to hide the property and make it readonly.
        /// The sorting is now adjusted with the DropDownSorting property.
        /// write set_Sorted;
        /// </summary>
        public new bool Sorted
        {
            get
            {
                return base.Sorted;
            }
        }

        #region Creation and Disposal

        /// <summary>
        /// This is the constructer of this class
        ///
        /// </summary>
        /// <returns>void</returns>
        public TCmbVersatile() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();
            this.Set_DefaultProperties();
        }

        /// <summary>
        /// This procedure checks and sets the right
        ///
        /// </summary>
        /// <returns>void</returns>
        private void CheckColumnNumber()
        {
            System.Int32 NumberOfColumns;
            NumberOfColumns = 0;

            if (this.FColumnWidthCol1 > 0)
            {
                NumberOfColumns = NumberOfColumns + 1;
            }

            if (this.FColumnWidthCol2 > 0)
            {
                NumberOfColumns = NumberOfColumns + 1;
            }

            if (this.FColumnWidthCol3 > 0)
            {
                NumberOfColumns = NumberOfColumns + 1;
            }

            if (this.FColumnWidthCol4 > 0)
            {
                NumberOfColumns = NumberOfColumns + 1;
            }

            this.Set_ColumnNum(NumberOfColumns);
        }

        /// <summary>
        /// check if the datasource knows that column
        /// </summary>
        /// <param name="AColumn">column to look for</param>
        /// <returns></returns>
        public bool DataSourceContainsColumn(String AColumn)
        {
            // check for null is required for Mono; called from DataSourceContainsColumn with null parameter
            if (!(DesignMode) && (AColumn != null))
            {
                return GetDataSourceTableFromSelf().Columns.Contains(AColumn);
            }

            return false;
        }

        /// <summary>
        /// This procedure disposes the TcmbVersatile.
        /// </summary>
        /// <param name="Disposing">true to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.
        /// </param>
        /// <returns>void</returns>
        protected override void Dispose(Boolean Disposing)
        {
            if (Disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(Disposing);
        }

        /// <summary>
        /// This procedure needs a object in
        /// order to come up with the table.
        /// </summary>
        /// <param name="ADataSource">The object to be transformed into a table.</param>
        /// <returns>The table of the System.ComponentModel.MarshalByValueComponent System.Object.
        /// </returns>
        private System.Data.DataTable GetDataSourceTable(object ADataSource)
        {
            System.Data.DataTable ReturnValue;
            System.Data.DataTable mDataSourceTable;
            String mLogLine;
            ReturnValue = null;
            mDataSourceTable = new System.Data.DataTable();

            if (ADataSource != null)
            {
                CheckDataSourceType();

                if (ADataSource is System.Data.DataTable)
                {
                    mDataSourceTable = (System.Data.DataTable)ADataSource;
                    ReturnValue = mDataSourceTable;
                }
                else if (ADataSource is System.Data.DataView)
                {
                    mDataSourceTable = ((System.Data.DataView)ADataSource).Table;
                    ReturnValue = mDataSourceTable;
                }
            }
            else
            {
                mLogLine = "DataSourceTable could not be built. Since DataSource is \"null\".";
                throw new ArgumentException(mLogLine);
            }

            return ReturnValue;
        }

        /// <summary>
        /// returns the currently used DataTable
        /// </summary>
        /// <returns>the DataTable from the DataSource</returns>
        private System.Data.DataTable GetDataSourceTableFromSelf()
        {
            System.Data.DataTable ReturnValue;
            String mLogLine;
            ReturnValue = this.GetDataSourceTable(this.DataSource);

            if (ReturnValue == null)
            {
                mLogLine = "GetDataSourceTableFromSelf: Table is \"null\"";
                throw new ArgumentException(mLogLine);
            }

            return ReturnValue;
        }

        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            //
            // TcmbVersatile
            //
            this.Leave += new System.EventHandler(this.TcmbVersatile_Leave);
        }

        /// <summary>
        /// This procedure sets the default properties.
        ///
        /// </summary>
        /// <returns>void</returns>
        private void Set_DefaultProperties()
        {
            this.Set_ColumnNum(1);
            this.CheckColumnNumber();
            this.ColumnWidthCol1 = this.Width - UNIT_SLIDER_WIDTH;
            this.GridLineColor = System.Drawing.SystemColors.ControlLight;
            this.GridLineVertical = true;
            this.GridLineHorizontal = true;

            if ((this.FColumnsToSearch == null) || (this.FColumnsToSearch.Count < 1))
            {
                this.ColumnsToSearch = "";
            }
        }

        #endregion

        /// <summary>
        /// This procedure sets the number of columns within the ComboBox
        /// </summary>
        /// <param name="Value">The number of columns.
        /// </param>
        /// <returns>void</returns>
        private void Set_ColumnNum(System.Int32 Value)
        {
            if (Value > 4)
            {
                this.FColumnNum = 4;
            }
            else
            {
                if (Value <= 0)
                {
                    this.FColumnNum = 1;
                }
                else
                {
                    this.FColumnNum = Value;
                }
            }
        }

        private void TcmbVersatile_Leave(System.Object sender, System.EventArgs e)
        {
            if (this.Text == "")
            {
                this.SelectedIndex = -1;
            }
        }

        #region Routines dealing with the Drop Down

        /// <summary>
        /// This procedure gets the colum nnumber from a given DataSource if the
        /// name of the column is provided.
        /// </summary>
        /// <param name="DataSource">The DataSource the column is in.</param>
        /// <param name="ColumnName">The name of the column to get the ordial from.</param>
        /// <returns>The column ordial.
        /// </returns>
        private int Get_ColumnNumber(object DataSource, string ColumnName)
        {
            int ReturnValue;

            System.Data.DataTable mDataTable;
            System.Data.DataColumn mDataColumn;
            String mLogLine;
            mDataTable = new System.Data.DataTable();
            mDataTable = this.GetDataSourceTable(DataSource);
            try
            {
                mDataColumn = mDataTable.Columns[ColumnName];
                ReturnValue = mDataColumn.Ordinal;
            }
            catch (Exception)
            {
                mLogLine = "The column number could not retrieved for column: " + ColumnName + '!';
                throw new ArgumentException(mLogLine);
            }
            return ReturnValue;
        }

        /// <summary>
        /// This procedure gets the string representation of the item identified by the
        /// row number and the column number of the DataSource property of this control.
        /// </summary>
        /// <param name="Row">The row number of the item</param>
        /// <param name="Col">The column number of the item</param>
        /// <returns>The string representation of the item
        /// </returns>
        private string Get_ItemString(int Row, int Col)
        {
            string ReturnValue;

            System.Data.DataView mDataView;
            System.Data.DataRowView mDataRowView;
            String mLogLine;
            ReturnValue = null;
            mDataView = null;

            // Check whether the data source is of DataTable or DataView
            if (this.DataSource is System.Data.DataTable)
            {
                mDataView = ((System.Data.DataTable) this.DataSource).DefaultView;
#if TRACEMODE
                TLogging.Log("The DataSource is of type DataTable!");
#endif
            }
            else if (this.DataSource is System.Data.DataView)
            {
                mDataView = (System.Data.DataView) this.DataSource;
#if TRACEMODE
                TLogging.Log("The DataSource is of type DataView!");
#endif
            }

            // Get DataRowView
            try
            {
                mDataRowView = mDataView[Row];
            }
            catch (Exception)
            {
                mLogLine = "The DataRowView of item (" + Row.ToString() + " could not be retrieved!";

                if (mDataView == null)
                {
                    mLogLine = mLogLine + "DataTable is NIL";
                }

                throw new ArgumentException(mLogLine);
            }

            // Get the String
            try
            {
                ReturnValue = mDataRowView[Col].ToString();
#if TRACEMODE
                TLogging.Log("The String of the item[" + Row.ToString() + "], [" + Col.ToString() + "];");
#endif
            }
            catch (Exception)
            {
                mLogLine = "The string of item (" + Row.ToString() + "; " + Col.ToString() + " could not be retrieved!";

                if (mDataView == null)
                {
                    mLogLine = mLogLine + "DataTable is NIL";
                }

                throw new ArgumentException(mLogLine);
            }
            return ReturnValue;
        }

        /// <summary>
        /// This event occurs when a row in the DropDown pane is drawn.
        /// </summary>
        /// <param name="e">The DrawItemEventArgs for this event.
        /// </param>
        /// <returns>void</returns>
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            System.Drawing.Rectangle BackGroundRectangle;
            System.Drawing.Rectangle BackGroundRectangleCol1;
            System.Drawing.Rectangle BackGroundRectangleCol2;
            System.Drawing.Rectangle BackGroundRectangleCol3;
            System.Drawing.Rectangle BackGroundRectangleCol4;
            System.Drawing.Rectangle ImageRectangleCol1;
            System.Drawing.Rectangle ImageRectangleCol2;
            System.Drawing.Rectangle ImageRectangleCol3;
            System.Drawing.Rectangle ImageRectangleCol4;
            System.Int32 Column1StartX;
            System.Int32 Column2StartX;
            System.Int32 Column3StartX;
            System.Int32 Column4StartX;
            System.Int32 ColumnStartY;
            System.Drawing.SolidBrush DefaultBackGroundBrush;
            System.Drawing.SolidBrush SelectedBackGroundBrush;
            System.Drawing.SolidBrush DefaultForeGroundBrush;
            System.Drawing.SolidBrush SelectedForeGroundBrush;
            System.Drawing.StringFormat pStringFormat;
            System.Int32 mItemIndex;
            String StringColumn1 = "";
            String StringColumn2 = "";
            String StringColumn3 = "";
            String StringColumn4 = "";
            System.Int32 ColumnIndex;

            // Set the different brushes
            DefaultBackGroundBrush = new System.Drawing.SolidBrush(this.BackColor);
            SelectedBackGroundBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.Highlight);
            DefaultForeGroundBrush = new System.Drawing.SolidBrush(this.ForeColor);
            SelectedForeGroundBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.HighlightText);

            // Preset the base rectangle
            BackGroundRectangle = e.Bounds;

            // messagebox.Show('Hoehe der Box: ' + (BackGroundRectangle.Height).toString);
            // Preset boundaries
            Column1StartX = BackGroundRectangle.X;
            Column2StartX = BackGroundRectangle.X + this.ColumnWidthCol1;
            Column3StartX = BackGroundRectangle.X + this.ColumnWidthCol1 + this.ColumnWidthCol2;
            Column4StartX = BackGroundRectangle.X + this.ColumnWidthCol1 + this.ColumnWidthCol2 + this.ColumnWidthCol3;
            ColumnStartY = BackGroundRectangle.Y;

            // Preset the rectangles
            BackGroundRectangleCol1 = BackGroundRectangle;
            BackGroundRectangleCol2 = BackGroundRectangle;
            BackGroundRectangleCol2.X = Column2StartX;
            BackGroundRectangleCol3 = BackGroundRectangle;
            BackGroundRectangleCol3.X = Column3StartX;
            BackGroundRectangleCol4 = BackGroundRectangle;
            BackGroundRectangleCol4.X = Column4StartX;
            ImageRectangleCol1 = BackGroundRectangleCol1;
            ImageRectangleCol1.Width = ImageRectangleCol1.Height;
            ImageRectangleCol2 = BackGroundRectangleCol2;
            ImageRectangleCol2.Width = ImageRectangleCol2.Height;
            ImageRectangleCol3 = BackGroundRectangleCol3;
            ImageRectangleCol3.Width = ImageRectangleCol3.Height;
            ImageRectangleCol4 = BackGroundRectangleCol4;
            ImageRectangleCol4.Width = ImageRectangleCol4.Height;

            if (e.Index >= 0)
            {
                BackGroundRectangle.Width = BackGroundRectangle.Left + this.DropDownWidth;
                pStringFormat = new System.Drawing.StringFormat();
                pStringFormat.Alignment = System.Drawing.StringAlignment.Near;

                // Get the "right" index
                mItemIndex = e.Index;

                // Getting the items into a string;
                try
                {
                    ColumnIndex = this.Get_ColumnNumber(this.DataSource, this.FDisplayInColumn1);
                    StringColumn1 = this.Get_ItemString(mItemIndex, ColumnIndex);
                }
                catch (Exception)
                {
                    StringColumn1 = "";
                    throw;
                }

                if (this.ColumnNum > 1)
                {
                    try
                    {
                        ColumnIndex = this.Get_ColumnNumber(this.DataSource, this.FDisplayInColumn2);
                        StringColumn2 = this.Get_ItemString(mItemIndex, ColumnIndex);
                    }
                    catch (Exception)
                    {
                        StringColumn2 = "";
                        throw;
                    }
                }

                if (this.ColumnNum > 2)
                {
                    try
                    {
                        ColumnIndex = this.Get_ColumnNumber(this.DataSource, this.FDisplayInColumn3);
                        StringColumn3 = this.Get_ItemString(mItemIndex, ColumnIndex);
                    }
                    catch (Exception)
                    {
                        StringColumn3 = "";
                        throw;
                    }
                }

                if (this.ColumnNum > 3)
                {
                    try
                    {
                        ColumnIndex = this.Get_ColumnNumber(this.DataSource, this.FDisplayInColumn4);
                        StringColumn4 = this.Get_ItemString(mItemIndex, ColumnIndex);
                    }
                    catch (Exception)
                    {
                        StringColumn4 = "";
                        throw;
                    }
                }

                // Draw every single item. First the selected Items.
                if ((e.State & System.Windows.Forms.DrawItemState.Selected) == System.Windows.Forms.DrawItemState.Selected)
                {
                    // Store the selected index
                    switch (this.ColumnNum)
                    {
                        case 1:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // End of "1:"
                            break;

                        case 2:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // Write Data in the second column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol2,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol2,
                            ImageRectangleCol2,
                            2,
                            StringColumn2,
                            pStringFormat,
                            Column2StartX,
                            ColumnStartY);

                            // End of "2:"
                            break;

                        case 3:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // Write Data in the second column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol2,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol2,
                            ImageRectangleCol2,
                            2,
                            StringColumn2,
                            pStringFormat,
                            Column2StartX,
                            ColumnStartY);

                            // Write Data in the third column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol3,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol3,
                            ImageRectangleCol3,
                            3,
                            StringColumn3,
                            pStringFormat,
                            Column3StartX,
                            ColumnStartY);

                            // End of "3:"
                            break;

                        case 4:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // Write Data in the second column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol2,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol2,
                            ImageRectangleCol2,
                            2,
                            StringColumn2,
                            pStringFormat,
                            Column2StartX,
                            ColumnStartY);

                            // Write Data in the third column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol3,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol3,
                            ImageRectangleCol3,
                            3,
                            StringColumn3,
                            pStringFormat,
                            Column3StartX,
                            ColumnStartY);

                            // Write Data in the third column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol4,
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol4,
                            ImageRectangleCol4,
                            4,
                            StringColumn4,
                            pStringFormat,
                            Column4StartX,
                            ColumnStartY);

                            // End of "4:"
                            break;
                    }

                    // END of "case this.ColumnNum of"
                }
                else
                {
                    switch (this.ColumnNum)
                    {
                        case 1:
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // End of "1:"
                            break;

                        case 2:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // Write Data in the second column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol2,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol2,
                            ImageRectangleCol2,
                            2,
                            StringColumn2,
                            pStringFormat,
                            Column2StartX,
                            ColumnStartY);

                            // End of "2:"
                            break;

                        case 3:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // Write Data in the second column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol2,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol2,
                            ImageRectangleCol2,
                            2,
                            StringColumn2,
                            pStringFormat,
                            Column2StartX,
                            ColumnStartY);

                            // Write Data in the third column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol3,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol3,
                            ImageRectangleCol3,
                            3,
                            StringColumn3,
                            pStringFormat,
                            Column3StartX,
                            ColumnStartY);

                            // End of "3:"
                            break;

                        case 4:

                            // Write Data in the first column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol1,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangle,
                            ImageRectangleCol1,
                            1,
                            StringColumn1,
                            pStringFormat,
                            Column1StartX,
                            ColumnStartY);

                            // Write Data in the second column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol2,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol2,
                            ImageRectangleCol2,
                            2,
                            StringColumn2,
                            pStringFormat,
                            Column2StartX,
                            ColumnStartY);

                            // Write Data in the third column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol3,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol3,
                            ImageRectangleCol3,
                            3,
                            StringColumn3,
                            pStringFormat,
                            Column3StartX,
                            ColumnStartY);

                            // Write Data in the third column of the Drop Down
                            DrawDropDownEntry(this.ColumnWidthCol4,
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol4,
                            ImageRectangleCol4,
                            4,
                            StringColumn4,
                            pStringFormat,
                            Column4StartX,
                            ColumnStartY);

                            // End of "4:"
                            break;
                    }

                    // END of "case this.ColumnNum of"
                }

                // End of "if (e.State ... ) then"
            }

            // END of "  if (e.Index >= 0) then"
            DrawGridlines(BackGroundRectangle, e);
        }

        /// <summary>
        /// This function draws an entry in the DropDown pane.
        /// </summary>
        /// <param name="AColumnWidth">The width of the current column</param>
        /// <param name="AnItemIndex">The index for this very row.</param>
        /// <param name="AnEvent">The current DrawItemEventArgs.</param>
        /// <param name="ABackgroundBrush">The brush for the background</param>
        /// <param name="AForegroundBrush">The brush for the foreground</param>
        /// <param name="ABackgroundRectangle">The drawing / writing area</param>
        /// <param name="AnImageRectangle">The area the image should expand to</param>
        /// <param name="ACurrColumnNumber">The current column number</param>
        /// <param name="ADisplayString">The string to display</param>
        /// <param name="ADisplayStringFormat">The format of the string</param>
        /// <param name="AColumnXStartCoord">The X Coordinate of the start point for writing</param>
        /// <param name="AColumnYStartCoord">The Y Coordinate of the start point for writing</param>
        /// <returns>Zero for success, -1 for failure.
        /// </returns>
        private int DrawDropDownEntry(int AColumnWidth,
            int AnItemIndex,
            System.Windows.Forms.DrawItemEventArgs AnEvent,
            System.Drawing.SolidBrush ABackgroundBrush,
            System.Drawing.SolidBrush AForegroundBrush,
            System.Drawing.Rectangle ABackgroundRectangle,
            System.Drawing.Rectangle AnImageRectangle,
            int ACurrColumnNumber,
            string ADisplayString,
            System.Drawing.StringFormat ADisplayStringFormat,
            int AColumnXStartCoord,
            int AColumnYStartCoord)
        {
            int ReturnValue;

            // Appearance The current row The DrawItemEvent Background Brush Foreground Brush Background Rectangle Image Rectangle Current Column Number (Which counting table or design???) A display string A format for the display string X Start
            // Coord Y Start Coord
            System.Int32 mTableColumnIndex;
            System.Int32 mTmpImageIndex = -1;
            System.Drawing.Image mTmpImage;
            String mLogString;
            ReturnValue = -1;

            if (AColumnWidth > 0)
            {
                // Calculate some Column index
                mTableColumnIndex = ACurrColumnNumber - 1;

                // Paint background
                AnEvent.Graphics.FillRectangle(ABackgroundBrush, ABackgroundRectangle);

                if (this.FImageColumn == ACurrColumnNumber)
                {
                    try
                    {
                        // Get number
                        mTmpImageIndex = System.Convert.ToInt32(ADisplayString);
                        mTmpImage = this.FImages.Images[mTmpImageIndex];
                        AnEvent.Graphics.DrawImage(mTmpImage, AnImageRectangle);
                        mLogString = "mTableColumnIndex: >" + mTableColumnIndex.ToString() + "<\r\n" + "ACurrColumnNumber: >" +
                                     ACurrColumnNumber.ToString() + "<\r\n" +
                                     "AnItemIndex:       >" + AnItemIndex.ToString() + "<\r\n" + "mTmpImageIndex:    >" + mTmpImageIndex.ToString() +
                                     "<\r\n";
                        mLogString = mLogString + "The given datasource item is not of type integer!";
                        ReturnValue = 0;
                    }
                    catch (Exception)
                    {
                        mLogString = "mTableColumnIndex: >" + mTableColumnIndex.ToString() + "<\r\n" + "ACurrColumnNumber: >" +
                                     ACurrColumnNumber.ToString() + "<\r\n" +
                                     "AnItemIndex:       >" + AnItemIndex.ToString() + "<\r\n" + "mTmpImageIndex:    >" + mTmpImageIndex.ToString() +
                                     "<\r\n";
                        mLogString = mLogString + "The given datasource item is not of type integer!";

                        // raise ArgumentException.Create(mLogString, e);
                        throw;
                    }

                    // End of "try"
                }
                // End of "if (this.FImageColumn = 1) then"
                else
                {
                    // Just write the string
                    AnEvent.Graphics.DrawString(ADisplayString,
                        this.Font,
                        AForegroundBrush,
                        AColumnXStartCoord,
                        AColumnYStartCoord,
                        ADisplayStringFormat);
                    ReturnValue = 0;
                }

                // End of "if (this.FImageColumn = ACurrColumnNumber) then"
            }

            // END of "if (AColumnWidth > 0) then"
            return ReturnValue;
        }

        /// <summary>
        /// This procedure draws the gridlines within the DropDown pane. The procedure
        /// is called from the OnDrawItem routine.
        /// </summary>
        /// <param name="ABackgroundRectangle">The drawing area.</param>
        /// <param name="AnEvent">Event Arguments from the OnDrawItem routine.
        /// </param>
        /// <returns>void</returns>
        private void DrawGridlines(System.Drawing.Rectangle ABackgroundRectangle, System.Windows.Forms.DrawItemEventArgs AnEvent)
        {
            System.Int32 Column2StartX;
            System.Int32 Column3StartX;
            System.Int32 Column4StartX;
            System.Int32 ColumnStartY;
            System.Int32 ColumnEndY;
            System.Drawing.SolidBrush GridLineBrush;
            System.Drawing.Pen GridLinePen;

            // Calculate required coordinates
            // Column1StartX := ABackgroundRectangle.X;
            Column2StartX = ABackgroundRectangle.X + this.ColumnWidthCol1;
            Column3StartX = ABackgroundRectangle.X + this.ColumnWidthCol1 + this.ColumnWidthCol2;
            Column4StartX = ABackgroundRectangle.X + this.ColumnWidthCol1 + this.ColumnWidthCol2 + this.ColumnWidthCol3;
            ColumnStartY = ABackgroundRectangle.Y;
            ColumnEndY = ColumnStartY + ABackgroundRectangle.Height;

            // Get required pen
            GridLineBrush = new System.Drawing.SolidBrush(this.FGridLineColor);
            GridLinePen = new System.Drawing.Pen(GridLineBrush, 1);

            // Draw vertical GridLines
            // Devider between column 1 and column 2
            if (this.FGridLineVertical == true)
            {
                if (this.ColumnWidthCol2 > 0)
                {
                    AnEvent.Graphics.DrawLine(GridLinePen, Column2StartX - 2, ColumnStartY, Column2StartX - 2, ColumnEndY);
                }

                // End of "if (this.FColumnWidthCol2 > 0) then"
                // Devider between column 2 and column 3
                if (this.FColumnWidthCol3 > 0)
                {
                    AnEvent.Graphics.DrawLine(GridLinePen, Column3StartX - 2, ColumnStartY, Column3StartX - 2, ColumnEndY);
                }

                // End of "if (this.FColumnWidthCol3 > 0) then"
                // Devider between column 3 and column 4
                if (this.FColumnWidthCol4 > 0)
                {
                    // Draw vertical GridLine
                    AnEvent.Graphics.DrawLine(GridLinePen, Column4StartX - 2, ColumnStartY, Column4StartX - 2, ColumnEndY);
                }

                // End of "if (this.FColumnWidthCol4 > 0) then"
            }

            // End of "if (this.FGridLineVertical = true) then"
            // Draw horizontal GridLine
            if (this.FGridLineHorizontal == true)
            {
                AnEvent.Graphics.DrawLine(GridLinePen,
                    ABackgroundRectangle.X,
                    ABackgroundRectangle.Y + ABackgroundRectangle.Height - 1,
                    ABackgroundRectangle.X + this.DropDownWidth,
                    ABackgroundRectangle.Y + ABackgroundRectangle.Height - 1);
            }

            // End of "if (this.FGridLineHorizontal) then"
        }

        #endregion

        #region Events

        /// <summary>
        /// This event occurs when the control is created.
        ///
        /// </summary>
        /// <returns>void</returns>
        protected override void OnCreateControl()
        {
            // Set draw mode
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;

            // Set default display in first column
            if (((this.DisplayInColumn1 == null) || (this.DisplayInColumn1 == "")) && (this.ColumnNum == 1))
            {
                this.DisplayInColumn1 = this.DisplayMember;
                this.ColumnWidthCol4 = 0;
                this.ColumnWidthCol3 = 0;
                this.ColumnWidthCol2 = 0;
            }

            // Set Column width for 1st column
            if ((this.ColumnNum == 1) && (this.ColumnWidthCol1 != this.Width))
            {
                this.ColumnWidthCol1 = this.Width;
            }

            base.OnCreateControl();
        }

        #endregion
    }
}