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

namespace Ict.Petra.Client.MCommon.Gui
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
        /// <summary>String that holds the selected outreach code</summary>
        public String FSelectedOutreachCode;
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

        private void FilterEvents(System.Object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            SetTableFilter();
            grdEvent.AutoSizeCells();

            this.Cursor = Cursors.Default;
        }

        private void ClearFilterEvents(System.Object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            txtEventName.Text = "";
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

                FSelectedUnitName = (((DataRowView)grdEvent.SelectedDataRows[0]).Row[PPartnerTable.GetPartnerShortNameDBName()]).ToString();
                FSelectedOutreachCode = (((DataRowView)grdEvent.SelectedDataRows[0]).Row[PUnitTable.GetOutreachCodeDBName()]).ToString();
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
            grdEvent.AddTextColumn("Event Code", FEventTable.Columns[PUnitTable.GetOutreachCodeDBName()]);
            grdEvent.AddTextColumn("Country", FEventTable.Columns[PCountryTable.GetCountryNameDBName()]);
            grdEvent.AddDateColumn("Start Date", FEventTable.Columns[PPartnerLocationTable.GetDateEffectiveDBName()]);
            grdEvent.AddDateColumn("End Date", FEventTable.Columns[PPartnerLocationTable.GetDateGoodUntilDBName()]);
            grdEvent.AddTextColumn("Event Key", FEventTable.Columns[PPartnerTable.GetPartnerKeyDBName()]);
            grdEvent.AddTextColumn("Event Type", FEventTable.Columns[PUnitTable.GetUnitTypeCodeDBName()], 80);

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

            if (rbtOutreach.Checked || rbtAll.Checked)
            {
                // get all the outreaches
                DataTable TmpTable = TDataCache.TMPersonnel.GetCacheableUnitsTable(
                    TCacheableUnitTablesEnum.OutreachList);

                AddTableToGrid(TmpTable);
            }

            if (rbtConference.Checked || rbtAll.Checked)
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
            String EventName = "";

            if (chkCurrentFutureOnly.Checked)
            {
                RowFilter = PPartnerLocationTable.GetDateGoodUntilDBName() + " >= #" + DateTime.Today.ToString("yyyy-MM-dd") + "#";
            }

            if (txtEventName.Text.Length > 0)
            {
                // in case there is a filter set for the event name

                if (RowFilter.Length > 0)
                {
                    RowFilter = RowFilter + " AND ";
                }

                EventName = txtEventName.Text.Replace('*', '%') + "%";
                RowFilter = RowFilter + PPartnerTable.GetPartnerShortNameDBName() + " LIKE '" + EventName + "'";
            }

            FEventTable.DefaultView.RowFilter = RowFilter;
        }
    }

    /// <summary>
    /// Manages the opening of a new/showing of an existing Instance of the Event Find Screen.
    /// </summary>
    public static class TEventFindScreenManager
    {
        /// <summary>
        /// Opens a Modal instance of the Event Find screen.
        /// </summary>
        /// <param name="AEventNamePattern">Mathcing pattern for the event name</param>
        /// <param name="AEventKey">Partner key of the found event</param>
        /// <param name="AEventName">Partner ShortName name of the found event</param>
        /// <param name="AOutreachCode">Matching patterns for the outreach code</param>
        /// <param name="AParentForm"></param>
        /// <returns>True if an event was found and accepted by the user,
        /// otherwise false.</returns>
        public static bool OpenModalForm(String AEventNamePattern,
            out Int64 AEventKey,
            out String AEventName,
            out String AOutreachCode,
            Form AParentForm)
        {
            DialogResult dlgResult;

            AEventKey = -1;
            AEventName = String.Empty;
            AOutreachCode = String.Empty;

            TFrmSelectEvent SelectEvent = new TFrmSelectEvent(AParentForm);

            dlgResult = SelectEvent.ShowDialog();

            if (dlgResult == DialogResult.OK)
            {
                AEventKey = SelectEvent.FSelectedPartnerKey;
                AEventName = SelectEvent.FSelectedUnitName;
                AOutreachCode = SelectEvent.FSelectedOutreachCode;
                return true;
            }

            return false;
        }
    }
}