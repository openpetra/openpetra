//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.Interfaces.MPartner.Partner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;


namespace Ict.Petra.Client.MReporting.Gui.MPersonnel
{
    public partial class TFrmPartnerByEvent
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

        private void InitializeManualCode()
        {
            ucoChkFilter.ShowFamiliesOnly(false);
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
            
            // Prepare the window title and settings directory (will be used later by TFrmPetraReportingUtils).
            // Normally the settings directory is set earlier but since this is one form that covers two extracts
            // we need to initialize it a second time here with the correct directory.
            if (FCalledForConferences)
            {
            	this.FindForm().Text = Catalog.GetString("Partner by Conference");
            	FPetraUtilsObject.InitialiseStoredSettings("Partner by Conference");
            }
            else
            {
            	this.FindForm().Text = Catalog.GetString("Partner by Outreach");
            	FPetraUtilsObject.InitialiseStoredSettings("Partner by Outreach");
            }

            // need to load settings again after they have been re-initialized
			FPetraUtilsObject.LoadDefaultSettings();

            // enable autofind in list for first character (so the user can press character to find list entry)
            this.clbEvent.AutoFindColumn = ((Int16)(1));
            this.clbEvent.AutoFindMode = Ict.Common.Controls.TAutoFindModeEnum.FirstCharacter;
            
            clbEvent.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));

            // populate list with data to be loaded
            this.LoadListData("");
        }

        private void LoadListData(string AFilter)
        {
            string CheckedMember = "CHECKED";
            string ValueMember = PUnitTable.GetPartnerKeyDBName();
            string DisplayMember = PUnitTable.GetUnitNameDBName();
            string EventCodeMember = PUnitTable.GetOutreachCodeDBName();
            PUnitTable Table;

            if (FCalledForConferences)
            {
                Table = TRemote.MPartner.Partner.WebConnectors.GetConferenceUnits("");
            }
            else
            {
                Table = TRemote.MPartner.Partner.WebConnectors.GetOutreachUnits("");
            }
            
            DataView view = new DataView(Table);

            DataTable NewTable = view.ToTable(true, new string[] { DisplayMember, ValueMember, EventCodeMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));
            
            clbEvent.Columns.Clear();
            clbEvent.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
            clbEvent.AddTextColumn(Catalog.GetString("Event Name"), NewTable.Columns[DisplayMember], 240);
            clbEvent.AddTextColumn(Catalog.GetString("Partner Key"), NewTable.Columns[ValueMember], 80);

            // outreach/event code column only needed in case of displaying Outreaches
            if (!FCalledForConferences)
            {
                clbEvent.AddTextColumn(Catalog.GetString("Event Code"), NewTable.Columns[EventCodeMember], 110);
            }

            clbEvent.DataBindGrid(NewTable, DisplayMember, CheckedMember, ValueMember, DisplayMember, false, true, false);

            //TODO: only temporarily until settings file exists
            clbEvent.SetCheckedStringList("");
            
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