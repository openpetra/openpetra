//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using System.Drawing;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;

namespace Ict.Petra.Client.MReporting.Gui.MPersonnel.ShortTerm
{
    /// <summary>
    /// manual code for TFrmSelectEvent class
    /// </summary>
    public partial class TFrmSelectEvent
    {
        /// <summary>Holds the current event data</summary>
        private DataTable FEventTable;
        /// <summary>Prevents updates during initialisation</summary>
        private bool DuringInitialization;

        /// <summary>String holding the selected unit name</summary>
        public String FSelectedUnitName;
        /// <summary>String that holds the selected campaign code</summary>
        public String FSelectedCampaignCode;
        /// <summary>PartnerKey of the selected unit</summary>
        public Int64 FSelectedPartnerKey;

        private void EventTypeChanged(System.Object sender, EventArgs e)
        {
            RadioButton RadioBtn = (RadioButton)sender;

            if (RadioBtn.Checked)
            {
                this.Cursor = Cursors.WaitCursor;

                InitEventTable();

                this.Cursor = Cursors.Default;
            }
        }

        private void EventDateChanged(System.Object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            SetTableFilter();
            grdEvent.AutoSizeCells();

            this.Cursor = Cursors.Default;
        }

        private void grdEventDoubleClick(System.Object sender, EventArgs e)
        {
            AcceptSelection(sender, e);
        }

        private void AcceptSelection(System.Object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            if (grdEvent.SelectedDataRows.Length == 1)
            {
                this.DialogResult = DialogResult.OK;

                FSelectedUnitName = (String)((DataRowView)grdEvent.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerShortNameDBName()];
                FSelectedCampaignCode = (String)((DataRowView)grdEvent.SelectedDataRows[0]).Row[PUnitTable.GetXyzTbdCodeDBName()];
                String PartnerKey = ((DataRowView)grdEvent.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerKeyDBName()].ToString();
                FSelectedPartnerKey = Convert.ToInt64(PartnerKey);
            }

            this.Close();
        }

        private void CancelClick(System.Object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void InitUserControlsManually()
        {
            DuringInitialization = true;
            // Get list of commitment statuses
            FEventTable = TDataCache.TMPersonnel.GetCacheableUnitsTable(
                TCacheableUnitTablesEnum.ConferenceList);

            grdEvent.Columns.Clear();

            grdEvent.AddTextColumn("Event Name", FEventTable.Columns[PPartnerTable.GetPartnerShortNameDBName()]);
            grdEvent.AddTextColumn("Event Code", FEventTable.Columns[PUnitTable.GetXyzTbdCodeDBName()]);
            grdEvent.AddTextColumn("Country", FEventTable.Columns[PCountryTable.GetCountryNameDBName()]);
            grdEvent.AddDateColumn("Start Date", FEventTable.Columns[PPartnerLocationTable.GetDateEffectiveDBName()]);
            grdEvent.AddDateColumn("End Date", FEventTable.Columns[PPartnerLocationTable.GetDateGoodUntilDBName()]);
            grdEvent.AddTextColumn("Event Key", FEventTable.Columns[PPartnerTable.GetPartnerKeyDBName()]);
            grdEvent.AddTextColumn("Event Type", FEventTable.Columns[PUnitTable.GetUnitTypeCodeDBName()]);

            FEventTable.DefaultView.AllowDelete = false;
            FEventTable.DefaultView.AllowEdit = false;
            FEventTable.DefaultView.AllowNew = false;

            grdEvent.DataSource = new DevAge.ComponentModel.BoundDataView(FEventTable.DefaultView);
            grdEvent.AutoSizeCells();
            grdEvent.Selection.EnableMultiSelection = false;

            chkCurrentFutureOnly.Checked = true;

            // set DuringInitialization to false now, so that we call at least once InitEventTable()
            DuringInitialization = false;
            rbtConference.Checked = true;
        }

        private void InitEventTable()
        {
            if (DuringInitialization)
            {
                return;
            }

            grdEvent.DataSource = null;

            FEventTable.Rows.Clear();

            if (rbtCampaign.Checked || rbtTeenstreet.Checked || rbtAll.Checked)
            {
                // get all the campaigns
                DataTable TmpTable = TDataCache.TMPersonnel.GetCacheableUnitsTable(
                    TCacheableUnitTablesEnum.CampaignList);

                AddTableToGrid(TmpTable);
            }

            if (rbtConference.Checked || rbtTeenstreet.Checked || rbtAll.Checked)
            {
                // get all the conferences
                DataTable TmpTable = TDataCache.TMPersonnel.GetCacheableUnitsTable(
                    TCacheableUnitTablesEnum.ConferenceList);

                AddTableToGrid(TmpTable);
            }

            SetTableFilter();

            grdEvent.DataSource = new DevAge.ComponentModel.BoundDataView(FEventTable.DefaultView);
            grdEvent.AutoSizeCells();
        }

        private void AddTableToGrid(DataTable ATmpTable)
        {
            FEventTable.BeginLoadData();

            if (ATmpTable.Columns.Count != FEventTable.Columns.Count)
            {
                TLogging.Log("Can't show events in Events Grid for " + this.Name);
            }

            foreach (DataRow Row in ATmpTable.Rows)
            {
                FEventTable.LoadDataRow(Row.ItemArray, true);
            }

            FEventTable.EndLoadData();
        }

        /// <summary>
        /// Update the filter for the data table so that only those events are shown which are in accordance
        /// with the radio button selection and the chekc box
        /// </summary>
        private void SetTableFilter()
        {
            String RowFilter = "";

            if (chkCurrentFutureOnly.Checked)
            {
                RowFilter = PPartnerLocationTable.GetDateGoodUntilDBName() + " >= #" + DateTime.Today.ToString("MM/dd/yyyy") + "#";
            }

            if (rbtTeenstreet.Checked)
            {
                if (RowFilter.Length > 0)
                {
                    RowFilter = RowFilter + " AND ";
                }

                RowFilter = RowFilter + PUnitTable.GetXyzTbdCodeDBName() + " LIKE 'TS%'";
            }

            FEventTable.DefaultView.RowFilter = RowFilter;
        }
    }
}