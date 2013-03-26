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
    ///   Here the underlying view has to be set. This is mostly done in the
    ///   following way:
    ///     cmbVersatile.BeginUpdate();
    ///     cmbVersatile.DataSource := YOUR_DATA_VIEW;
    ///     cmbVersatile.DisplayMember := COLUMN NAME;
    ///     cmbVersatile.ValueMember := COLUMN NAME;
    ///     cmbVersatile.EndUpdate();
    /// - DisplayInColumn1 ... DisplayIn Column4:
    ///   Here the column of the underlying DataSource table / view has to be set. If
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
    ///   automatically grab these images and display them in the column you assign.
    /// - ImageColumn:
    ///   The column number of the drop down's column which holds the image indices.
    ///   Please note: 0 refers to no column!, 1 to column 1 ... 4 to column 4.
    public class TCmbVersatile : TCmbAutoComplete
    {
        private const Int32 UNIT_SLIDER_WIDTH = 20;
        private const Int32 MAXCOLUMN = 4;

        private int[] FColumnWidth = new int[] {
            0, 0, 0, 0
        };
        private string[] FDisplayInColumn = new String[] {
            string.Empty, string.Empty, string.Empty, string.Empty
        };

        /// <summary>
        /// this combobox will never accept new values
        /// </summary>
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
        /// </summary>
        public int ColumnNum
        {
            get;
            private set;
        }

        /// <summary>
        /// set the width of a column within the ComboBox.
        /// </summary>
        public void SetColumnWidth(int AColumnNr, int AWidth)
        {
            if ((AWidth == 0) && (AColumnNr == 0))
            {
                this.FColumnWidth[AColumnNr] = this.Width - UNIT_SLIDER_WIDTH;
            }
            else
            {
                this.FColumnWidth[AColumnNr] = AWidth;
            }

            DropDownWidth = UNIT_SLIDER_WIDTH;
            ColumnNum = 0;

            for (int counter = 0; counter < MAXCOLUMN; counter++)
            {
                DropDownWidth += this.FColumnWidth[counter];

                if (this.FColumnWidth[counter] > 0)
                {
                    ColumnNum++;
                }
            }
        }

        /// <summary>
        /// get the width of the column
        /// </summary>
        /// <param name="AColumnNr"></param>
        /// <returns></returns>
        public int GetColumnWidth(int AColumnNr)
        {
            return FColumnWidth[AColumnNr];
        }

        /// <summary>
        /// set the content of a column in the combobox
        /// </summary>
        public void DisplayInColumn(int AColumnNr, string ADisplay)
        {
            FDisplayInColumn[AColumnNr] = ADisplay;
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn1
        {
            set
            {
                DisplayInColumn(0, value);
            }
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn2
        {
            set
            {
                DisplayInColumn(1, value);
            }
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn3
        {
            set
            {
                DisplayInColumn(2, value);
            }
        }

        /// <summary>
        /// set the content of the column
        /// </summary>
        public string DisplayInColumn4
        {
            set
            {
                DisplayInColumn(3, value);
            }
        }

        /// <summary>
        /// This property manages the vertical grid line in the drop down list.
        /// </summary>
        public bool GridLineVertical
        {
            get;
            set;
        }

        /// <summary>
        /// This property manages the horizontal grid line in the drop down list.
        ///
        /// </summary>
        public bool GridLineHorizontal
        {
            get;
            set;
        }

        /// <summary>
        /// This property manages the color of the grid lines in the drop down list.
        ///
        /// </summary>
        public System.Drawing.Color GridLineColor
        {
            get;
            set;
        }

        /// <summary>
        /// This property manages the image column.
        ///
        /// </summary>
        public int ImageColumn
        {
            get;
            set;
        }

        /// <summary>
        /// This property holds the images.
        ///
        /// </summary>
        public System.Windows.Forms.ImageList Images
        {
            get;
            set;
        }

        /// <summary>
        /// This property controls whether items in the list portion are sorted. Here
        /// it is only in the declaration to hide the property and make it readonly.
        /// The sorting is now adjusted with the DropDownSorting property.
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
        public TCmbVersatile() : base()
        {
            //
            // Required for Windows Form Designer support
            //
            this.InitializeComponent();

            this.ColumnNum = 1;
            this.ImageColumn = 0;
            this.FColumnWidth[0] = this.Width - UNIT_SLIDER_WIDTH;
            this.GridLineColor = System.Drawing.SystemColors.ControlLight;
            this.GridLineVertical = true;
            this.GridLineHorizontal = true;

            if ((this.FColumnsToSearch == null) || (this.FColumnsToSearch.Count < 1))
            {
                this.ColumnsToSearch = "";
            }
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

        private System.ComponentModel.IContainer components = null;

        private void InitializeComponent()
        {
            //
            // TcmbVersatile
            //
            this.Leave += new System.EventHandler(this.TcmbVersatile_Leave);
        }

        #endregion

        private void TcmbVersatile_Leave(System.Object sender, System.EventArgs e)
        {
            if (this.Text == "")
            {
                this.SelectedIndex = -1;
            }
        }

        #region Routines dealing with the Drop Down

        /// <summary>
        /// This procedure gets the colum number of the current DataSource if the
        /// name of the column is provided.
        /// </summary>
        /// <param name="ColumnName">The name of the column to get the ordinal from.</param>
        /// <returns>The column ordinal.
        /// </returns>
        private int Get_ColumnNumber(string ColumnName)
        {
            DataTable mDataTable = ((DataView)DataSource).Table;

            try
            {
                return mDataTable.Columns[ColumnName].Ordinal;
            }
            catch (Exception)
            {
                string mLogLine = "The column number could not be retrieved for column: " + ColumnName + '!';
                throw new ArgumentException(mLogLine);
            }
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
            DataRowView mDataRowView = ((DataView) this.DataSource)[Row];

            return mDataRowView[Col].ToString();
        }

        /// <summary>
        /// This event occurs when a row in the DropDown pane is drawn.
        /// </summary>
        /// <param name="e">The DrawItemEventArgs for this event.</param>
        /// <returns>void</returns>
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            Rectangle[] BackGroundRectangleCol = new Rectangle[MAXCOLUMN];
            Rectangle[] ImageRectangleCol = new Rectangle[MAXCOLUMN];
            System.Int32[] ColumnStartX = new int[MAXCOLUMN];
            System.Int32 ColumnStartY;
            System.Drawing.StringFormat pStringFormat;
            System.Int32 mItemIndex;

            // Set the different brushes
            SolidBrush DefaultBackGroundBrush = new System.Drawing.SolidBrush(this.BackColor);
            SolidBrush SelectedBackGroundBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.Highlight);
            SolidBrush DefaultForeGroundBrush = new System.Drawing.SolidBrush(this.ForeColor);
            SolidBrush SelectedForeGroundBrush = new System.Drawing.SolidBrush(System.Drawing.SystemColors.HighlightText);

            // Preset the base rectangle
            Rectangle BackGroundRectangle = e.Bounds;

            ColumnStartY = BackGroundRectangle.Y;

            int CountingX = BackGroundRectangle.X;

            for (int counter = 0; counter < MAXCOLUMN; counter++)
            {
                // Preset boundaries
                ColumnStartX[counter] = CountingX;
                CountingX += FColumnWidth[counter];

                // Preset the rectangles
                BackGroundRectangleCol[counter] = BackGroundRectangle;
                BackGroundRectangleCol[counter].X = ColumnStartX[counter];

                ImageRectangleCol[counter] = BackGroundRectangleCol[counter];
                ImageRectangleCol[counter].Width = ImageRectangleCol[counter].Height;
            }

            if (e.Index >= 0)
            {
                BackGroundRectangle.Width = BackGroundRectangle.Left + this.DropDownWidth;
                pStringFormat = new System.Drawing.StringFormat();
                pStringFormat.Alignment = System.Drawing.StringAlignment.Near;

                // Get the "right" index
                mItemIndex = e.Index;

                String[] StringColumn = new string[MAXCOLUMN];

                // Getting the items into a string;
                for (int counter = 0; counter < this.ColumnNum; counter++)
                {
                    Int32 ColumnIndex = this.Get_ColumnNumber(this.FDisplayInColumn[counter]);

                    if (ColumnIndex != -1)
                    {
                        StringColumn[counter] = this.Get_ItemString(mItemIndex, ColumnIndex);
                    }
                }

                // Draw every single item. First the selected Items.
                if ((e.State & System.Windows.Forms.DrawItemState.Selected) == System.Windows.Forms.DrawItemState.Selected)
                {
                    for (int counter = 0; counter < this.ColumnNum; counter++)
                    {
                        DrawDropDownEntry(
                            this.FColumnWidth[counter],
                            mItemIndex,
                            e,
                            SelectedBackGroundBrush,
                            SelectedForeGroundBrush,
                            BackGroundRectangleCol[counter],
                            ImageRectangleCol[counter],
                            counter,
                            StringColumn[counter],
                            pStringFormat,
                            ColumnStartX[counter],
                            ColumnStartY);
                    }
                }
                else
                {
                    for (int counter = 0; counter < this.ColumnNum; counter++)
                    {
                        DrawDropDownEntry(
                            this.FColumnWidth[counter],
                            mItemIndex,
                            e,
                            DefaultBackGroundBrush,
                            DefaultForeGroundBrush,
                            BackGroundRectangleCol[counter],
                            ImageRectangleCol[counter],
                            counter,
                            StringColumn[counter],
                            pStringFormat,
                            ColumnStartX[counter],
                            ColumnStartY);
                    }
                }
            }

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
            if (AColumnWidth > 0)
            {
                // Paint background
                AnEvent.Graphics.FillRectangle(ABackgroundBrush, ABackgroundRectangle);

                if (this.ImageColumn == ACurrColumnNumber + 1)
                {
                    Int32 mTmpImageIndex = System.Convert.ToInt32(ADisplayString);
                    AnEvent.Graphics.DrawImage(this.Images.Images[mTmpImageIndex], AnImageRectangle);
                    return 0;
                }
                else
                {
                    // Just write the string
                    AnEvent.Graphics.DrawString(ADisplayString,
                        this.Font,
                        AForegroundBrush,
                        AColumnXStartCoord,
                        AColumnYStartCoord,
                        ADisplayStringFormat);
                    return 0;
                }
            }

            return -1;
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
            // Calculate required coordinates
            int CountingX = ABackgroundRectangle.X;

            int[] ColumnStartX = new int[MAXCOLUMN];

            for (int counter = 0; counter < MAXCOLUMN; counter++)
            {
                // Preset boundaries
                ColumnStartX[counter] = CountingX;
                CountingX += FColumnWidth[counter];
            }

            Int32 ColumnStartY = ABackgroundRectangle.Y;
            Int32 ColumnEndY = ColumnStartY + ABackgroundRectangle.Height;

            // Get required pen
            SolidBrush GridLineBrush = new System.Drawing.SolidBrush(this.GridLineColor);
            Pen GridLinePen = new System.Drawing.Pen(GridLineBrush, 1);

            // Draw vertical GridLines
            // Devider between column 1 and column 2
            if (this.GridLineVertical == true)
            {
                for (int counter = 1; counter < this.ColumnNum; counter++)
                {
                    if (this.FColumnWidth[counter] > 0)
                    {
                        AnEvent.Graphics.DrawLine(GridLinePen, ColumnStartX[counter] - 2, ColumnStartY, ColumnStartX[counter] - 2, ColumnEndY);
                    }
                }
            }

            // Draw horizontal GridLine
            if (this.GridLineHorizontal == true)
            {
                AnEvent.Graphics.DrawLine(GridLinePen,
                    ABackgroundRectangle.X,
                    ABackgroundRectangle.Y + ABackgroundRectangle.Height - 1,
                    ABackgroundRectangle.X + this.DropDownWidth,
                    ABackgroundRectangle.Y + ABackgroundRectangle.Height - 1);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// This event occurs when the control is created.
        /// </summary>
        protected override void OnCreateControl()
        {
            // Set draw mode
            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;

            // Set default display in first column
            if (((this.FDisplayInColumn[0] == null) || (this.FDisplayInColumn[0] == "")) && (this.ColumnNum == 1))
            {
                this.FDisplayInColumn[0] = this.DisplayMember;

                for (int counter = 1; counter < MAXCOLUMN; counter++)
                {
                    this.FColumnWidth[counter] = 0;
                }
            }

            // Set Column width for 1st column
            if ((this.ColumnNum == 1) && (this.FColumnWidth[0] != this.Width))
            {
                this.FColumnWidth[0] = this.Width;
            }

            base.OnCreateControl();
        }

        #endregion
    }
}