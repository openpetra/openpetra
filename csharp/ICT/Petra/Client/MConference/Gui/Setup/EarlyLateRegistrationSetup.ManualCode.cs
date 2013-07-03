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
    public partial class TFrmEarlyLateRegistrationSetup
    {
        private Int64 FPartnerKey = 1110198;
        
        private void InitializeManualCode()
        {
            string CurrencyCode;
            string CurrencyName;
            string ConferenceName;
            TPartnerClass PartnerClass;
        
            // display the conference name in the title bar and in a text box at the top of the screen
            TRemote.MPartner.Partner.ServerLookups.WebConnectors.GetPartnerShortName(FPartnerKey, out ConferenceName, out PartnerClass);
            this.Text = this.Text + " [" + ConferenceName + "]";
            txtConferenceName.Text = ConferenceName;
            
            txtConferenceDates.Text = TRemote.MConference.Conference.WebConnectors.GetStartDate(FPartnerKey).ToShortDateString() + " to " +
                TRemote.MConference.Conference.WebConnectors.GetEndDate(FPartnerKey).ToShortDateString();
            
            // display the conference currency in a text box at the top of the screen and in pnlDetails
            TRemote.MConference.Conference.WebConnectors.GetCurrency(FPartnerKey, out CurrencyCode, out CurrencyName);
            txtDetailAmount.CurrencySymbol = CurrencyCode;
        }
        
        private void NewRowManual(ref PcEarlyLateRow ARow)
        {
            // set the conference key
            ARow.ConferenceKey = FPartnerKey;
            
            ARow.Applicable = GetLatestAvailableEarlyDate(ARow);
        }
        
        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPcEarlyLate();
        }
        
        // sets ARow.Type and ARow.Applicable when Early/Late radio buttons are changed
        private void UpdateRegistrationType(object sender, EventArgs e)
        {
            PcEarlyLateRow ARow = GetSelectedDetailRow();
            
            // does nothing if the check box is only changed because a new row is selected
            if (rbtEarly.Checked && !ARow.Type)
            {
                ARow.Type = true;
                
                // update ARow.Applicable if it allowed to be changed
                if (dtpDetailApplicable.Enabled)
                {
                    ARow.Applicable = GetLatestAvailableEarlyDate(ARow);
                    SelectRowInGrid(GetDataTableRowIndexByPrimaryKeys(ARow.Applicable));    // keep the current row highlighted
                }
            }
            // does nothing if the check box is only changed because a new row is selected
            else if (rbtLate.Checked && ARow.Type)
            {
                ARow.Type = false;
                
                // update ARow.Applicable if it allowed to be changed
                if (dtpDetailApplicable.Enabled)
                {
                    ARow.Applicable = GetEarliestAvailableLateDate(ARow);
                    SelectRowInGrid(GetDataTableRowIndexByPrimaryKeys(ARow.Applicable));    // keep the current row highlighted
                }
            }
        }
        
        private DateTime GetLatestAvailableEarlyDate(PcEarlyLateRow ARow)
        {
            DateTime ApplicableDate = TRemote.MConference.Conference.WebConnectors.GetStartDate(FPartnerKey).AddDays(-1);
            
            if (FMainDS.PcEarlyLate.Count > 1)
            {
                DateTime EarliestDate = ApplicableDate;
                DataRowCollection AllRecords = FMainDS.PcEarlyLate.Rows;
                int i = 0;
                
                // find the earliest late registration date for conference
                while (i < AllRecords.Count)
                {
                    if (((PcEarlyLateRow) AllRecords[i]).RowState != DataRowState.Deleted 
                        &&((PcEarlyLateRow) AllRecords[i]).Type == false && ((PcEarlyLateRow) AllRecords[i]).Applicable < EarliestDate)
                    {
                        EarliestDate = ((PcEarlyLateRow) AllRecords[i]).Applicable;
                    }
                
                    i++;
                }
                
                // if the earliest late registration date is already the row's current date then nothing needs changed
                if (EarliestDate == ARow.Applicable)
                {
                    return ApplicableDate;
                }
                
                // find the first free date before the earliest late registration date
                while (FMainDS.PcEarlyLate.Rows.Find(new object[] { FPartnerKey, EarliestDate }) != null)
                {
                    EarliestDate = EarliestDate.AddDays(-1);
                }
                        
                ApplicableDate = EarliestDate;
            }
            
            return ApplicableDate;
        }
        
        private DateTime GetEarliestAvailableLateDate(PcEarlyLateRow ARow)
        {
            DateTime ApplicableDate = TRemote.MConference.Conference.WebConnectors.GetStartDate(FPartnerKey).AddDays(-1);
            
            DateTime LatestDate = ApplicableDate;
            DataRowCollection AllRecords = FMainDS.PcEarlyLate.Rows;
            int i = 0;
                
            // find the latest early registration date for conference
            while (i < AllRecords.Count)
            {
                if (((PcEarlyLateRow) AllRecords[i]).RowState != DataRowState.Deleted &&((PcEarlyLateRow) AllRecords[i]).Type == true 
                    && ((((PcEarlyLateRow) AllRecords[i]).Applicable > LatestDate) || (LatestDate == ApplicableDate)))
                {
                    LatestDate = ((PcEarlyLateRow) AllRecords[i]).Applicable;
                }
                
                i++;
            }
                
            // if the latest early registration date is already the row's current date then nothing needs changed
            if (LatestDate == ARow.Applicable)
            {
                return ApplicableDate;
            }
                
            // find the first free date after the latest early registration date
            while (FMainDS.PcEarlyLate.Rows.Find(new object[] { FPartnerKey, LatestDate }) != null)
            {
                        
                LatestDate = LatestDate.AddDays(1);
            }
                        
            ApplicableDate = LatestDate;
            
            return ApplicableDate;
        }

        // return the grid index for the current row being worked on
        private int GetDataTableRowIndexByPrimaryKeys(DateTime AApplicable)
        {
            int RowPos = 0;

            foreach (DataRowView rowView in FMainDS.PcEarlyLate.DefaultView)
            {
                PcEarlyLateRow Row = (PcEarlyLateRow)rowView.Row;

                if ((Row.ConferenceKey == FPartnerKey) && (Row.Applicable == AApplicable))
                {
                    break;
                }

                RowPos++;
            }

            //remember grid is out of sync with DataView by 1 because of grid header rows
            return RowPos + 1;
        }
        
        // sets ARow.Percent and changes which text box is readonly when Amount/Percent radio buttons are changed
        private void UpdateAmountPercent(object sender, EventArgs e)
        {
            PcEarlyLateRow ARow = (PcEarlyLateRow) FMainDS.PcEarlyLate.Rows.Find(new object[] { FPartnerKey, dtpDetailApplicable.Date });
            
            if (rbtAmount.Checked)
            {
                ARow.AmountPercent = true;
                
                txtDetailAmount.ReadOnly = false;
                txtDetailPercent.ReadOnly = true;
            }
            else
            {
                ARow.AmountPercent = false;
                
                txtDetailAmount.ReadOnly = true;
                txtDetailPercent.ReadOnly = false;
            }
        }
        
        // check the correct radio buttons and set the correct text box to read only on screen load and record change
        private void ShowDetailsManual(PcEarlyLateRow ARow)
        {
            // only run if screen in not empty
            if (ARow != null)
            {
                if (ARow.Type == true)
                {
                    rbtEarly.Checked = true;
                }
                else
                {
                    rbtLate.Checked = true;
                }
            
                if (ARow.AmountPercent == true)
                {
                    rbtAmount.Checked = true;
                    txtDetailPercent.ReadOnly = true;
                }
                else
                {
                    rbtPercent.Checked = true;
                    txtDetailAmount.ReadOnly = true;
                }
            }
        }
        
        private void ValidateDataDetailsManual(PcEarlyLateRow ARow)
        {   
            // this is used to compare with the row that is being validated            
            DataRowCollection GridData = FMainDS.PcEarlyLate.Rows;

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            
            TSharedConferenceValidation_Conference.ValidateEarlyLateRegistration(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict, GridData);
        }
    }
}