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

namespace Ict.Petra.Client.MConference.Gui.Setup
{
    public partial class TFrmConferenceStandardCostSetup
    {
        /// PartnerKey for selected conference to be set from outside
        public static Int64 FPartnerKey {
            private get; set;
        }

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

            // display the conference currency in a text box at the top of the screen and in pnlDetails
            TRemote.MConference.Conference.WebConnectors.GetCurrency(FPartnerKey, out CurrencyCode, out CurrencyName);
            txtConferenceCurrency.Text = CurrencyCode + ": " + CurrencyName;
            txtDetailCharge.CurrencyCode = CurrencyCode;
        }

        private void NewRowManual(ref PcConferenceCostRow ARow)
        {
            int NewOptionDays = 1;  // starts at 1 day
            int i = 1;

            // if a row already exists for 1 day find the next available integer
            if (FMainDS.PcConferenceCost.Rows.Find(new object[] { FPartnerKey, NewOptionDays }) != null)
            {
                while (FMainDS.PcConferenceCost.Rows.Find(new object[] { FPartnerKey, NewOptionDays + i }) != null)
                {
                    i++;
                }

                NewOptionDays += i;
            }

            ARow.OptionDays = NewOptionDays;

            // set the conference key
            ARow.ConferenceKey = FPartnerKey;
        }

        private void NewRecord(Object sender, EventArgs e)
        {
            CreateNewPcConferenceCost();
        }

        private void ValidateDataDetailsManual(PcConferenceCostRow ARow)
        {
            // this is used to compare with the row that is being validated
            DataRowCollection GridData = FMainDS.PcConferenceCost.Rows;
            
            if (txtDetailCharge.Text == "")
            {
                txtDetailCharge.Text = "0.00";
                ARow.Charge = 0;
            }

            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedConferenceValidation_Conference.ValidateConferenceStandardCost(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict, GridData);
        }
    }
}