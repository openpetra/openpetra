//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Reflection;
using System.Windows.Forms;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Petra.Client.App.Core;

namespace Ict.Petra.Client.MSysMan.Gui
{
    /// manual methods for the generated window
    public partial class TUC_GiftPreferences
    {
        //private int FCurrentLedger;
        private int FInitiallySelectedLedger;

        private bool FNewDonorAlert = true;
        private bool FDonorZeroIsValid = false;
        private bool FAutoCopyIncludeMailingCode = false;
        private bool FAutoCopyIncludeComments = false;
        private bool FRecipientZeroIsValid = false;
        private bool FAutoSave = false;
        private bool FWarnOfInactiveValuesOnPosting = false;

        private void InitializeManualCode()
        {
            FInitiallySelectedLedger = TLstTasks.InitiallySelectedLedger;

            //New Donor alert
            FNewDonorAlert = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_NEW_DONOR_ALERT, true);
            chkNewDonorAlert.Checked = FNewDonorAlert;

            //Donor zero is valid
            FDonorZeroIsValid = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_DONOR_ZERO_IS_VALID, false);
            chkDonorZeroIsValid.Checked = FDonorZeroIsValid;

            //Auto-copying fields
            FAutoCopyIncludeMailingCode = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_MAILING_CODE, false);
            chkAutoCopyIncludeMailingCode.Checked = FAutoCopyIncludeMailingCode;

            FAutoCopyIncludeComments = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_COMMENTS, false);
            chkAutoCopyIncludeComments.Checked = FAutoCopyIncludeComments;

            //Recipient zero is valid
            FRecipientZeroIsValid = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_RECIPIENT_ZERO_IS_VALID, false);
            chkRecipientZeroIsValid.Checked = FRecipientZeroIsValid;

            //Allow auto-saving of form
            FAutoSave = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_AUTO_SAVE, false);
            chkAutoSave.Checked = FAutoSave;

            //Warn of inactive values on posting
            FWarnOfInactiveValuesOnPosting = TUserDefaults.GetBooleanDefault(TUserDefaults.FINANCE_GIFT_WARN_OF_INACTIVE_VALUES_ON_POSTING,
                true);
            chkWarnOfInactiveValuesOnPosting.Checked = FWarnOfInactiveValuesOnPosting;
        }

        /// <summary>
        /// Gets the data from all UserControls on this TabControl.
        /// </summary>
        /// <returns>void</returns>
        public void GetDataFromControls()
        {
        }

        /// <summary>
        /// Saves any changed preferences to s_user_defaults
        /// </summary>
        /// <returns>void</returns>
        public bool SaveGiftTab()
        {
            //New Donor alert
            if (FNewDonorAlert != chkNewDonorAlert.Checked)
            {
                FNewDonorAlert = chkNewDonorAlert.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_NEW_DONOR_ALERT, FNewDonorAlert);
            }

            //Donor zero is valid
            if (FDonorZeroIsValid != chkDonorZeroIsValid.Checked)
            {
                FDonorZeroIsValid = chkDonorZeroIsValid.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_DONOR_ZERO_IS_VALID, FDonorZeroIsValid);
            }

            //Auto-copying fields
            if (FAutoCopyIncludeMailingCode != chkAutoCopyIncludeMailingCode.Checked)
            {
                FAutoCopyIncludeMailingCode = chkAutoCopyIncludeMailingCode.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_MAILING_CODE, FAutoCopyIncludeMailingCode);
            }

            if (FAutoCopyIncludeComments != chkAutoCopyIncludeComments.Checked)
            {
                FAutoCopyIncludeComments = chkAutoCopyIncludeComments.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_AUTO_COPY_INCLUDE_COMMENTS, FAutoCopyIncludeComments);
            }

            //Recipient zero is valid
            if (FRecipientZeroIsValid != chkRecipientZeroIsValid.Checked)
            {
                FRecipientZeroIsValid = chkRecipientZeroIsValid.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_RECIPIENT_ZERO_IS_VALID, FRecipientZeroIsValid);
            }

            //Allow auto-saving of form
            if (FAutoSave != chkAutoSave.Checked)
            {
                FAutoSave = chkAutoSave.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_AUTO_SAVE, FAutoSave);
            }

            //Warn of inactive values on posting
            if (FWarnOfInactiveValuesOnPosting != chkWarnOfInactiveValuesOnPosting.Checked)
            {
                FWarnOfInactiveValuesOnPosting = chkWarnOfInactiveValuesOnPosting.Checked;
                TUserDefaults.SetDefault(TUserDefaults.FINANCE_GIFT_WARN_OF_INACTIVE_VALUES_ON_POSTING, FWarnOfInactiveValuesOnPosting);
            }

            return false;
        }
    }
}