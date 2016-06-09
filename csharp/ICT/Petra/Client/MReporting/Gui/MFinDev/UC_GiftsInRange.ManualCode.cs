//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MReporting;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MReporting.Gui.MFinDev
{
    /// <summary>
    /// Description of TFrmUC_GiftsInRange
    /// </summary>
    public partial class TFrmUC_GiftsInRange
    {
        private Int32 FLedgerNumber = -1;

        private DataTable FRangeTable = new DataTable();

        /// <summary>
        /// Manually set the label for the group
        /// </summary>
        public string GroupLabel
        {
            set
            {
                grpGiftsInRange.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the table containing the range.
        /// </summary>
        public DataTable RangeTable
        {
            get
            {
                return FRangeTable;
            }
            set
            {
                FRangeTable = value;
            }
        }

        /// <summary>
        /// Sets the 'year' values in a control, allowing custom defaults.
        /// </summary>
        public int Year
        {
            set
            {
                txtFromYear.NumberValueInt = value;
                txtToYear.NumberValueInt = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        public void InitialiseLedger(int ALedgerNumber)
        {
            FLedgerNumber = ALedgerNumber;
        }

        /// <summary>
        /// initialisation
        /// </summary>
        public void InitialiseData(TFrmPetraReportingUtils APetraUtilsObject)
        {
            FPetraUtilsObject = APetraUtilsObject;

            txtFromMonth.NumberValueInt = 1;
            txtToMonth.NumberValueInt = 12;
            txtFromYear.NumberValueInt = DateTime.Now.Year;
            txtToYear.NumberValueInt = DateTime.Now.Year;

            FRangeTable.Columns.Add("From");
            FRangeTable.Columns.Add("To");

            grdRange.Columns.Clear();
            grdRange.AddTextColumn(Catalog.GetString("From"), FRangeTable.Columns["From"]);
            grdRange.AddTextColumn(Catalog.GetString("To"), FRangeTable.Columns["To"]);

            grdRange.AutoResizeGrid();

            DataView myDataView = FRangeTable.DefaultView;
            myDataView.AllowNew = false;
            grdRange.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
        }

        /// <summary>
        /// This will add functions to the list of available functions
        /// </summary>
        public void SetAvailableFunctions(ArrayList AAvailableFunctions)
        {
            FLedgerNumber = TLstTasks.CurrentLedger;
        }

        /// <summary>
        /// Sets the selected values in the controls, using the parameters loaded from a file
        /// </summary>
        /// <param name="AParameters"></param>
        public void SetControls(TParameterList AParameters)
        {
        }

        /// <summary>
        /// Reads the selected values from the controls, and stores them into the parameter system of FCalculator
        /// </summary>
        /// <param name="ACalc"></param>
        /// <param name="AReportAction"></param>
        public void ReadControls(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
        }

        private void AddRange(object sender, EventArgs e)
        {
            DateTime ValidDate;

            if (string.IsNullOrEmpty(txtFromMonth.Text) || (txtFromMonth.NumberValueInt < 1) || (txtFromMonth.NumberValueInt > 12))
            {
                MessageBox.Show(Catalog.GetString(" Please enter a number between 1 and 12 for the From Month."), Catalog.GetString("Add Range"));
                return;
            }

            if (string.IsNullOrEmpty(txtToMonth.Text) || (txtToMonth.NumberValueInt < 1) || (txtToMonth.NumberValueInt > 12))
            {
                MessageBox.Show(Catalog.GetString(" Please enter a number between 1 and 12 for the To Month."), Catalog.GetString("Add Range"));
                return;
            }

            if (string.IsNullOrEmpty(txtFromYear.Text))
            {
                MessageBox.Show(Catalog.GetString(" Please enter a number for the From Year."), Catalog.GetString("Add Range"));
                return;
            }
            else if (DateTime.TryParseExact(((int)txtFromMonth.NumberValueInt).ToString("00") + "/" + ((int)txtFromYear.NumberValueInt).ToString("##00"), new String[] {"MM/yyyy", "MM/yy"}, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out ValidDate)) {
                txtFromYear.NumberValueInt = ValidDate.Year;
            }
            else {
                MessageBox.Show(Catalog.GetString(" Please enter a valid 2 or 4 digit From Year."), Catalog.GetString("Add Range"));
                return;
            }

            if (string.IsNullOrEmpty(txtToYear.Text))
            {
                MessageBox.Show(Catalog.GetString(" Please enter a number for the To Year."), Catalog.GetString("Add Range"));
                return;
            }
            else if (DateTime.TryParseExact(((int)txtToMonth.NumberValueInt).ToString("00") + "/" + ((int)txtToYear.NumberValueInt).ToString("##00"), new String[] {"MM/yyyy", "MM/yy"}, System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out ValidDate)) {
                txtToYear.NumberValueInt = ValidDate.Year;
            }
            else {
                MessageBox.Show(Catalog.GetString(" Please enter a valid 2 or 4 digit To Year."), Catalog.GetString("Add Range"));
                return;
            }

            // create new row
            DataRow NewRange = FRangeTable.NewRow();
            NewRange["From"] = txtFromYear.Text + "-" +
                               Convert.ToInt32(txtFromMonth.Text).ToString("00") + "-01";
            NewRange["To"] = txtToYear.Text + "-" +
                             Convert.ToInt32(txtToMonth.Text).ToString("00") + "-" +
                             DateTime.DaysInMonth(Convert.ToInt32(txtToYear.Text), Convert.ToInt32(txtToMonth.Text));
            FRangeTable.Rows.Add(NewRange);

            // select last created row
            grdRange.SelectRowInGrid(FRangeTable.Rows.Count);
        }

        private void RemoveRange(object sender, EventArgs e)
        {
            // get selected row
            DataRowView rowView = (DataRowView)grdRange.Rows.IndexToDataSourceRow(grdRange.Selection.ActivePosition.Row);

            if (rowView != null)
            {
                // remove selected row
                DataRow SelectedRow = rowView.Row;
                FRangeTable.Rows.Remove(SelectedRow);

                // select last created row
                grdRange.SelectRowInGrid(FRangeTable.Rows.Count);
            }
        }
    }
}