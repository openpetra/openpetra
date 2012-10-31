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
using Ict.Petra.Client.App.Core.RemoteObjects;
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

        /// <summary>String holding the selected unit name</summary>
        public String FSelectedUnitName;
        /// <summary>String that holds the selected outreach code</summary>
        public String FSelectedOutreachCode;
        /// <summary>PartnerKey of the selected unit</summary>
        public Int64 FSelectedPartnerKey;

        /// <summary>
        /// This Procedure will get called when the event filter criteria are changed
        /// </summary>
        /// <param name="sender">The Object that throws this Event</param>
        /// <param name="e">Event Arguments.
        /// </param>
        /// <returns>void</returns>
        private void EventFilterChanged(System.Object sender, System.EventArgs e)
        {
            LoadEventListData();
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
            // set controls in filter to default values
            ucoFilter.InitialiseUserControl();

            // Hook up EventFilterChanged Event to be able to react to changed filter
            ucoFilter.EventFilterChanged += new TEventHandlerEventFilterChanged(this.EventFilterChanged);

            // now the filter is initialized we can load the initial data
            LoadEventListData();

            grdEvent.AutoSizeCells();

            grdEvent.Columns.Clear();

            grdEvent.AddTextColumn("Event Name", FEventTable.Columns[PPartnerTable.GetPartnerShortNameDBName()]);
            grdEvent.AddTextColumn("Event Code", FEventTable.Columns[PUnitTable.GetOutreachCodeDBName()]);
            grdEvent.AddTextColumn("Country", FEventTable.Columns[PCountryTable.GetCountryNameDBName()]);
            grdEvent.AddDateColumn("Start Date", FEventTable.Columns[PPartnerLocationTable.GetDateEffectiveDBName()]);
            grdEvent.AddDateColumn("End Date", FEventTable.Columns[PPartnerLocationTable.GetDateGoodUntilDBName()]);
            grdEvent.AddPartnerKeyColumn("Event Key", FEventTable.Columns[PPartnerTable.GetPartnerKeyDBName()]);
            grdEvent.AddTextColumn("Event Type", FEventTable.Columns[PUnitTable.GetUnitTypeCodeDBName()], 80);

            FEventTable.DefaultView.AllowDelete = false;
            FEventTable.DefaultView.AllowEdit = false;
            FEventTable.DefaultView.AllowNew = false;

            grdEvent.Selection.EnableMultiSelection = false;
        }

        private void LoadEventListData()
        {
            FEventTable = TRemote.MPartner.Partner.WebConnectors.GetEventUnits
                        (ucoFilter.IncludeConferenceUnits, ucoFilter.IncludeOutreachUnits,
                        ucoFilter.NameFilter, true, ucoFilter.CurrentAndFutureEventsOnly);
            
            // set "AllowNew" to false as otherwise an empty line is shown in the grid when filter is refreshed
            FEventTable.DefaultView.AllowNew = false;
            
            grdEvent.DataSource = new DevAge.ComponentModel.BoundDataView(FEventTable.DefaultView);
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