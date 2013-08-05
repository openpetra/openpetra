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
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Xml;
using GNU.Gettext;
using Ict.Common.Verification;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonControls;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Mailroom.Data;
using Ict.Petra.Shared.MPartner.Validation;

namespace Ict.Petra.Client.MPartner.Gui.Setup
{
    public partial class TFrmPostcodeRegionSetup
    {
        private void InitializeManualCode()
        {
            ShowDetailsInGrids((PPostcodeRegionRow)FMainDS.PPostcodeRegion.Rows[0]);
        }

        // refresh both grids
        private void ShowDetailsInGrids(PPostcodeRegionRow ARow)
        {
            // show only regions column in top grid
            DataView MyDataView = FMainDS.PPostcodeRegion.DefaultView;

            MyDataView.RowFilter = null;
            DataTable RegionDT = MyDataView.ToTable("DistinctRegions", true, "p_region_c");
            RegionDT.DefaultView.AllowNew = false;
            grdRegions.DataSource = new DevAge.ComponentModel.BoundDataView(RegionDT.DefaultView);

            // filter grdDetails to only show ranges corresponding to the selected region in grdRegions
            MyDataView.RowFilter = "p_region_c = " + "'" + ARow.Region + "' AND " +
                                   "p_range_c <> " + "'UNDEFINED'";
            grdDetails.DataSource = new DevAge.ComponentModel.BoundDataView(MyDataView);
        }

        private void GetDetailDataFromControlsManual(PPostcodeRegionRow ARow)
        {
            ShowDetailsInGrids(ARow);
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPPostcodeRegionManual();
        }

        // manual code adapted from generated code to deal with adding new row to grdRegions
        private bool CreateNewPPostcodeRegionManual()
        {
            if (ValidateAllData(true, true))
            {
                PPostcodeRegionRow NewRow = FMainDS.PPostcodeRegion.NewRowTyped();
                NewRowManual(ref NewRow);
                FMainDS.PPostcodeRegion.Rows.Add(NewRow);

                ShowDetailsInGrids(NewRow);

                FPetraUtilsObject.SetChangedFlag();

                grdRegions.SelectRowInGrid(grdRegions.Rows.Count - 1);
                ShowRegionDetails(grdRegions.Rows.Count - 1);

                Control[] pnl = this.Controls.Find("pnlDetails", true);

                if (pnl.Length > 0)
                {
                    //Look for Key & Description fields
                    Control keyControl = null;

                    foreach (Control detailsCtrl in pnl[0].Controls)
                    {
                        if ((keyControl == null) && (detailsCtrl is TextBox || detailsCtrl is ComboBox || detailsCtrl is TCmbAutoPopulated))
                        {
                            keyControl = detailsCtrl;
                        }

                        if (detailsCtrl is TextBox && detailsCtrl.Name.Contains("Descr") && (detailsCtrl.Text == string.Empty))
                        {
                            detailsCtrl.Text = Catalog.GetString("PLEASE ENTER DESCRIPTION");
                            break;
                        }
                    }

                    ValidateAllData(true, false);

                    if (keyControl != null)
                    {
                        keyControl.Focus();
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        private void NewRowManual(ref PPostcodeRegionRow ARow)
        {
            string NewName = Catalog.GetString("NEWREGION");
            string DefaultRangeName = "UNDEFINED";
            int CountNewDetail = 0;

            // creates default range if it does not already exist
            TRemote.MPartner.Partner.WebConnectors.CreateNewDefaultRange(DefaultRangeName);

            // increment new region's name if default name already exists
            if (FMainDS.PPostcodeRegion.Rows.Find(new object[] { NewName, DefaultRangeName }) != null)
            {
                while (FMainDS.PPostcodeRegion.Rows.Find(new object[] { NewName + CountNewDetail.ToString(), DefaultRangeName }) != null)
                {
                    CountNewDetail++;
                }

                NewName += CountNewDetail.ToString();
            }

            ARow.Region = NewName;
            ARow.Range = DefaultRangeName;
        }

        // Do not delete row if it is the last row for the region. Instead change the range to the default to allow the region to remain.
        private bool DeleteRowManual(PPostcodeRegionRow ARowToDelete, ref String ACompletionMessage)
        {
            string RegionName = ARowToDelete.Region;

            foreach (PPostcodeRegionRow Row in FMainDS.PPostcodeRegion.Rows)
            {
                if ((Row.RowState != DataRowState.Deleted) && (Row != ARowToDelete) && (Row.Region == RegionName))
                {
                    ARowToDelete.Delete();
                    return true;
                }
            }

            string DefaultRangeName = "UNDEFINED";
            TRemote.MPartner.Partner.WebConnectors.CreateNewDefaultRange(DefaultRangeName);
            ARowToDelete.Range = DefaultRangeName;

            ShowDetailsInGrids(ARowToDelete);

            return true;
        }

        // manual code adapted from generated code to deal with focus row changes in grdRanges

        private int FPrevRegionRowChangedRow = -1;        // Totally private to this method call
        private void RegionsFocusedRowChanged(System.Object sender, SourceGrid.RowEventArgs e)
        {
            // The FocusedRowChanged event simply calls ShowDetails for the new 'current' row implied by e.Row
            // We do get a duplicate event if the user tabs round all the controls multiple times
            // There is no need to call it on duplicate events, so we just remember the previous row number we changed to.
            if (!grdRegions.Sorting && (e.Row != FPrevRegionRowChangedRow))
            {
                //Console.WriteLine("{0}:   FRC ShowDetails for {1}", DateTime.Now.Millisecond, e.Row);
                ShowRegionDetails(e.Row);
            }

            FPrevRegionRowChangedRow = e.Row;
        }

        private void ShowRegionDetails(Int32 ARowNumberInGrid)
        {
            string NewRegionName = null;
            Boolean RegionNameIsReadOnly = true;
            int GridRowCount = grdRegions.Rows.Count;

            if (ARowNumberInGrid >= GridRowCount)
            {
                ARowNumberInGrid = GridRowCount - 1;
            }

            if ((ARowNumberInGrid < 1) && (GridRowCount > 1))
            {
                ARowNumberInGrid = 1;
            }

            if (ARowNumberInGrid > 0)
            {
                DataRowView rowView = (DataRowView)grdRegions.Rows.IndexToDataSourceRow(ARowNumberInGrid);

                if (rowView != null)
                {
                    NewRegionName = rowView.Row[0].ToString();

                    foreach (PPostcodeRegionRow Row in FMainDS.PPostcodeRegion.Rows)
                    {
                        if ((Row.RowState != DataRowState.Deleted) && (Row.Region == NewRegionName))
                        {
                            FPreviouslySelectedDetailRow = Row;
                        }
                    }

                    if (FPreviouslySelectedDetailRow.RowState == DataRowState.Added)
                    {
                        RegionNameIsReadOnly = false;
                    }
                }

                FPrevRowChangedRow = ARowNumberInGrid;
            }
            else
            {
                FPrevRowChangedRow = -1;
            }

            FPetraUtilsObject.DisableDataChangedEvent();

            if (NewRegionName == null)
            {
                pnlDetails.Enabled = false;
                FPetraUtilsObject.ClearControls(pnlDetails);
            }
            else
            {
                pnlDetails.Enabled = !FPetraUtilsObject.DetailProtectedMode;
                txtDetailRegion.Text = NewRegionName;

                txtDetailRegion.ReadOnly = RegionNameIsReadOnly;
            }

            ShowDetailsInGrids(FPreviouslySelectedDetailRow);

            btnDelete.Enabled = pnlDetails.Enabled;
            FPetraUtilsObject.EnableDataChangedEvent();
        }
    }
}