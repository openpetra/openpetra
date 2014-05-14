//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb
//
// Copyright 2004-2012 by OM International
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MPersonnel.Personnel.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MCommon.Gui;


namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    public partial class TFrmPartnerByEventRole
    {
        private bool FCalledForConferences;

        /// helper object for the whole screen
        public Boolean CalledForConferences
        {
            get
            {
                return FCalledForConferences;
            }

            set
            {
                FCalledForConferences = value;
            }
        }

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

        /// <summary>
        /// only run this code once during activation
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            // no columns tab needed if called from extracts
            if (CalledFromExtracts)
            {
                tabReportSettings.Controls.Remove(tpgColumns);
            }

            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbEvent.AutoFindColumn = ((Int16)(1));
            this.clbEvent.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbEvent.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // set controls in filter to default values
            ucoFilter.InitialiseUserControl();

            // Hook up EventFilterChanged Event to be able to react to changed filter
            ucoFilter.EventFilterChanged += new TEventHandlerEventFilterChanged(this.EventFilterChanged);

            // populate list with data to be loaded
            this.LoadEventListData();

            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbEventRole.AutoFindColumn = ((Int16)(1));
            this.clbEventRole.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;

            clbEventRole.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // populate list with data to be loaded
            this.LoadEventRoleListData();

            FPetraUtilsObject.LoadDefaultSettings();
        }

        private void LoadEventListData()
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PPartnerTable.GetPartnerKeyDBName();
            string DisplayMember = PPartnerTable.GetPartnerShortNameDBName();
            string EventCodeMember = PUnitTable.GetOutreachCodeDBName();
            DataTable Table;

            Table = TRemote.MPartner.Partner.WebConnectors.GetEventUnits
                        (ucoFilter.IncludeConferenceUnits, ucoFilter.IncludeOutreachUnits,
                        ucoFilter.NameFilter, false, ucoFilter.CurrentAndFutureEventsOnly);

            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { DisplayMember, ValueMember, EventCodeMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbEvent.Columns.Clear();
            clbEvent.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbEvent.AddTextColumn(Catalog.GetString("Event Name"), NewTable.Columns[DisplayMember], 240);
            clbEvent.AddPartnerKeyColumn(Catalog.GetString("Partner Key"), NewTable.Columns[ValueMember], 100);

            // outreach/event code column only needed in case of displaying Outreaches
            if (ucoFilter.IncludeOutreachUnits)
            {
                clbEvent.AddTextColumn(Catalog.GetString("Event Code"), NewTable.Columns[EventCodeMember], 110);
            }

            clbEvent.DataBindGrid(NewTable, DisplayMember, CheckedMember, ValueMember, false, true, false);

            //TODO: only temporarily until settings file exists
            clbEvent.SetCheckedStringList("");
        }

        private void LoadEventRoleListData()
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PtCongressCodeTable.GetCodeDBName();
            string DisplayMember = PtCongressCodeTable.GetDescriptionDBName();
            PtCongressCodeTable Table;

            Table = (PtCongressCodeTable)TDataCache.TMPersonnel.GetCacheablePersonnelTable(TCacheablePersonTablesEnum.EventRoleList);

            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { ValueMember, DisplayMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbEventRole.Columns.Clear();
            clbEventRole.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbEventRole.AddTextColumn(Catalog.GetString("Event Role"), NewTable.Columns[ValueMember], 100);
            clbEventRole.AddTextColumn(Catalog.GetString("Description"), NewTable.Columns[DisplayMember], 240);

            clbEventRole.DataBindGrid(NewTable, DisplayMember, CheckedMember, ValueMember, false, true, false);

            //TODO: only temporarily until settings file exists
            clbEventRole.SetCheckedStringList("");
        }

        private void ReadControlsVerify(TRptCalculator ACalc, TReportActionEnum AReportAction)
        {
            if (clbEvent.GetCheckedStringList().Length == 0)
            {
                TVerificationResult VerificationResult = new TVerificationResult(
                    Catalog.GetString("Select at least one Event"),
                    Catalog.GetString("Please select at least one Event, to avoid listing the whole database!"),
                    TResultSeverity.Resv_Critical);
                FPetraUtilsObject.AddVerificationResult(VerificationResult);
            }
        }
    }
}