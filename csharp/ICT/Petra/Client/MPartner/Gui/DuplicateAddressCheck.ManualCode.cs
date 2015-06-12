//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonDialogs;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;

namespace Ict.Petra.Client.MPartner.Gui
{
    public partial class TFrmDuplicateAddressCheck
    {
        private DataTable FDuplicateLocations;

        private void InitializeManualCode()
        {
            chkNumber.Text = "Addresses must have identical numbers";
            chkNumber.BackColor = Color.Transparent;
            chkNumber.Checked = true;

            mniFilePrint.Enabled = false;

            lblRecordCounter.Text = "";

            // create custom table that will contain duplicate location data
            FDuplicateLocations = new DataTable();
            FDuplicateLocations.Columns.Add("SiteKeyA", typeof(Int32));
            FDuplicateLocations.Columns.Add("LocationKeyA", typeof(Int32));
            FDuplicateLocations.Columns.Add("LocalityA");
            FDuplicateLocations.Columns.Add("StreetNameA");
            FDuplicateLocations.Columns.Add("Address3A");
            FDuplicateLocations.Columns.Add("CityA");
            FDuplicateLocations.Columns.Add("CountyA");
            FDuplicateLocations.Columns.Add("PostcodeA");
            FDuplicateLocations.Columns.Add("CountryCodeA");
            FDuplicateLocations.Columns.Add("SiteKeyB", typeof(Int32));
            FDuplicateLocations.Columns.Add("LocationKeyB", typeof(Int32));
            FDuplicateLocations.Columns.Add("LocalityB");
            FDuplicateLocations.Columns.Add("StreetNameB");
            FDuplicateLocations.Columns.Add("Address3B");
            FDuplicateLocations.Columns.Add("CityB");
            FDuplicateLocations.Columns.Add("CountyB");
            FDuplicateLocations.Columns.Add("PostcodeB");
            FDuplicateLocations.Columns.Add("CountryCodeB");

            FDuplicateLocations.PrimaryKey = new DataColumn[] {
                FDuplicateLocations.Columns["SiteKeyA"], FDuplicateLocations.Columns["LocationKeyA"],
                FDuplicateLocations.Columns["SiteKeyB"], FDuplicateLocations.Columns["LocationKeyB"]
            };

            SetupGrid();
        }

        private void SetupGrid()
        {
            grdResults.Columns.Clear();
            grdResults.AddTextColumn(Catalog.GetString("SiteKey"), FDuplicateLocations.Columns["SiteKeyA"]);
            grdResults.AddTextColumn(Catalog.GetString("LocationKey"), FDuplicateLocations.Columns["LocationKeyA"]);
            grdResults.AddTextColumn(Catalog.GetString("Locality"), FDuplicateLocations.Columns["LocalityA"]);
            grdResults.AddTextColumn(Catalog.GetString("StreetName"), FDuplicateLocations.Columns["StreetNameA"]);
            grdResults.AddTextColumn(Catalog.GetString("Address3"), FDuplicateLocations.Columns["Address3A"]);
            grdResults.AddTextColumn(Catalog.GetString("City"), FDuplicateLocations.Columns["CityA"]);
            grdResults.AddTextColumn(Catalog.GetString("County"), FDuplicateLocations.Columns["CountyA"]);
            grdResults.AddTextColumn(Catalog.GetString("Postcode"), FDuplicateLocations.Columns["PostcodeA"]);
            grdResults.AddTextColumn(Catalog.GetString("Country"), FDuplicateLocations.Columns["CountryCodeA"]);
            grdResults.AddTextColumn(Catalog.GetString("SiteKey"), FDuplicateLocations.Columns["SiteKeyB"]);
            grdResults.AddTextColumn(Catalog.GetString("LocationKey"), FDuplicateLocations.Columns["LocationKeyB"]);
            grdResults.AddTextColumn(Catalog.GetString("Locality"), FDuplicateLocations.Columns["LocalityB"]);
            grdResults.AddTextColumn(Catalog.GetString("StreetName"), FDuplicateLocations.Columns["StreetNameB"]);
            grdResults.AddTextColumn(Catalog.GetString("Address3"), FDuplicateLocations.Columns["Address3B"]);
            grdResults.AddTextColumn(Catalog.GetString("City"), FDuplicateLocations.Columns["CityB"]);
            grdResults.AddTextColumn(Catalog.GetString("County"), FDuplicateLocations.Columns["CountyB"]);
            grdResults.AddTextColumn(Catalog.GetString("Postcode"), FDuplicateLocations.Columns["PostcodeB"]);
            grdResults.AddTextColumn(Catalog.GetString("Country"), FDuplicateLocations.Columns["CountryCodeB"]);

            // make the border to the right of the fixed columns bold
            ((TSgrdTextColumn)grdResults.Columns[8]).BoldRightBorder = true;
        }

        private void StartCheck(Object Sender, EventArgs e)
        {
            // run in thread so we can have Progress Dialog
            Thread t = new Thread(() => TRemote.MPartner.Mailroom.WebConnectors.FindAddressDuplicates(ref FDuplicateLocations, chkNumber.Checked));

            using (TProgressDialog dialog = new TProgressDialog(t))
            {
                dialog.ShowDialog();
            }

            DataView myDataView = FDuplicateLocations.DefaultView;
            myDataView.Sort = "SiteKeyA ASC, LocationKeyA ASC, SiteKeyB ASC, LocationKeyB ASC";
            myDataView.AllowNew = false;
            grdResults.DataSource = new DevAge.ComponentModel.BoundDataView(myDataView);
            grdResults.AutoResizeGrid();

            UpdateRecordNumberDisplay();

            if (FDuplicateLocations.Rows.Count > 0)
            {
                btnMergeAddresses.Enabled = true;
                btnPrintReport.Enabled = true;
                mniFilePrint.Enabled = true;
                grdResults.SelectRowInGrid(1);
            }
            else
            {
                btnMergeAddresses.Enabled = false;
                btnPrintReport.Enabled = false;
                mniFilePrint.Enabled = false;
            }
        }

        private void MergeAddresses(Object Sender, EventArgs e)
        {
            MessageBox.Show("This feature has not yet been implemented!");
        }

        /// <summary>
        /// Print out the Hierarchy using FastReports template.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilePrint(object sender, EventArgs e)
        {
            FastReportsWrapper ReportingEngine = new FastReportsWrapper("Duplicate Address Check");

            if (!ReportingEngine.LoadedOK)
            {
                ReportingEngine.ShowErrorPopup();
                return;
            }

            ReportingEngine.RegisterData(FDuplicateLocations, "DuplicateLocations");
            TRptCalculator Calc = new TRptCalculator();

            if (ModifierKeys.HasFlag(Keys.Control))
            {
                ReportingEngine.DesignReport(Calc);
            }
            else
            {
                ReportingEngine.GenerateReport(Calc);
            }
        }

        ///<summary>
        /// Update the text in the button panel indicating details of the record count
        /// </summary>
        public void UpdateRecordNumberDisplay()
        {
            if (grdResults.DataSource != null)
            {
                int RecordCount = ((DevAge.ComponentModel.BoundDataView)grdResults.DataSource).Count;
                lblRecordCounter.Text = String.Format(
                    Catalog.GetPluralString(MCommonResourcestrings.StrSingularRecordCount, MCommonResourcestrings.StrPluralRecordCount, RecordCount,
                        true),
                    RecordCount);
            }
        }
    }
}