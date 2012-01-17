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
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Client.MReporting.Gui;
using Ict.Petra.Client.MReporting.Logic;


namespace Ict.Petra.Client.MReporting.Gui.MPartner
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
            
            string CheckedMember = "CHECKED";
            string ValueMember = PPartnerTable.GetPartnerKeyDBName();
            string DisplayMember = PPartnerTable.GetPartnerShortNameDBName();
            //TODO:Add the eventcode to EventCodeMember
            string EventCodeMember = PPartnerTable.GetStatusCodeDBName();
            DataTable Table;
            
            //TODO:The difference between outreach and conference
            if (FCalledForConferences)
            {
                 Table = TRemote.MConference.WebConnectors.GetConferences("*", "").PPartner;
            }
            else
            {
                 Table = TRemote.MConference.WebConnectors.GetConferences("*", "").PPartner;
            }
            
            DataView view = new DataView(Table);

            // TODO view.RowFilter = only active types?
            view.Sort = DisplayMember;

            DataTable NewTable = view.ToTable(true, new string[] {DisplayMember, ValueMember, EventCodeMember });
            NewTable.Columns.Add(new DataColumn(CheckedMember, typeof(bool)));

            clbEvent.SpecialKeys =
                ((SourceGrid.GridSpecialKeys)((((((SourceGrid.GridSpecialKeys.Arrows |
                                                   SourceGrid.GridSpecialKeys.PageDownUp) |
                                                  SourceGrid.GridSpecialKeys.Enter) |
                                                 SourceGrid.GridSpecialKeys.Escape) |
                                                SourceGrid.GridSpecialKeys.Control) | SourceGrid.GridSpecialKeys.Shift)));
            if (FCalledForConferences)
            {
                clbEvent.Columns.Clear();
                clbEvent.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
                clbEvent.AddTextColumn(Catalog.GetString("Event Name"), NewTable.Columns[DisplayMember], 240);
                clbEvent.AddTextColumn(Catalog.GetString("Partner Key"), NewTable.Columns[ValueMember], 80);
                clbEvent.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);
            }
            else
                
            {
                clbEvent.Columns.Clear();
                clbEvent.AddCheckBoxColumn("", NewTable.Columns[CheckedMember], 17, false);
                clbEvent.AddTextColumn(Catalog.GetString("Event Name"), NewTable.Columns[DisplayMember], 240);
                clbEvent.AddTextColumn(Catalog.GetString("Partner Key"), NewTable.Columns[ValueMember], 80);
                clbEvent.AddTextColumn(Catalog.GetString("Event Code"), NewTable.Columns[EventCodeMember], 80);
                clbEvent.DataBindGrid(NewTable, ValueMember, CheckedMember, ValueMember, DisplayMember, false, true, false);
            }
            
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