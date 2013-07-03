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
using Ict.Petra.Shared;
using Ict.Petra.Shared.MConference;
using Ict.Petra.Shared.MConference.Data;
using Ict.Petra.Shared.MConference.Validation;
using Ict.Petra.Server.MPartner.Partner.ServerLookups.WebConnectors;

namespace Ict.Petra.Client.MConference.Gui.Setup
{
    public partial class TFrmChildDiscountSetup
    {
        private Int64 FPartnerKey = 1110198;
        
        private void InitializeManualCode()
        {
            string ConferenceName;
            TPartnerClass PartnerClass;
        
            // display the conference name in the title bar and in a text box at the top of the screen
            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(FPartnerKey, out ConferenceName, out PartnerClass);
            this.Text = this.Text + " [" + ConferenceName + "]";
            txtConferenceName.Text = ConferenceName;
        }
        
        private void NewRowManual(ref PcDiscountRow ARow)
        {
            string DiscountCriteriaCode = "CHILD";
            string CostType = "CONFERENCE";
            string Validity = "ALWAYS";
            int NewAge = 0;  // starts at 0 years
            int i = 0;

            // if a row already exists for 0 years find the next available integer
            while (FMainDS.PcDiscount.Rows.Find(new object[] { FPartnerKey, DiscountCriteriaCode, CostType, Validity, NewAge + i}) != null)
            {
                if (CostType == "CONFERENCE")
                {
                    CostType = "ACCOMMODATION";
                }
                else
                {
                    CostType = "CONFERENCE";
                    i++;
                }
            }

            NewAge += i;
            
            ARow.DiscountCriteriaCode = DiscountCriteriaCode;
            ARow.CostTypeCode = CostType;
            ARow.Validity = Validity;
            ARow.UpToAge = NewAge;
            
            // set the conference key
            ARow.ConferenceKey = FPartnerKey;
            
            TRemote.MConference.Conference.WebConnectors.CheckDiscountCriteriaCode("CHILD", true);
        }
        
        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPcDiscount();
        }
        
        // check the correct radio buttons and set the correct text box to read only on screen load and record change
        private void ShowDetailsManual(PcDiscountRow ARow)
        {
            // only run if screen in not empty
            if (ARow != null)
            {
                if (ARow.CostTypeCode == "CONFERENCE")
                {
                    cmbCostTypeCode.SelectedItem = "Conference";
                }
                
                else if (ARow.CostTypeCode == "ACCOMMODATION")
                {
                    cmbCostTypeCode.SelectedItem = "Accommodation";
                }
            }
        }
        
    private void GetDetailsFromControls(PcDiscountRow ARow, bool AIsNewRow = false, Control AControl=null)
    {}
    
        private void ValidateDataDetailsManual(PcDiscountRow ARow)
        {   
            // this is used to compare with the row that is being validated            
            /*DataRowCollection GridData = FMainDS.PcEarlyLate.Rows;

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            
            TSharedConferenceValidation_Conference.ValidateEarlyLateRegistration(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict, GridData);*/
        }
    }
}