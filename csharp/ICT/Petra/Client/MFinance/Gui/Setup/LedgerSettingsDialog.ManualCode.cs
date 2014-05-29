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
using System.Windows.Forms;
using System.Reflection;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmLedgerSettingsDialog
    {
        private Int32 FLedgerNumber;
        private DateTime FCalendarStartDate;
        private Boolean FCurrencyChangeAllowed;
        private Boolean FCalendarChangeAllowed;
        private Int32 FCurrentForwardPostingPeriods;
        private string FInitialTab = "General";
        private TabPage FActiveTab = null;

        /// <summary>
        /// Special constructor for calling this screen without showing it (in order to access the ledger properties)
        /// </summary>
        /// <param name="AParentForm">The parent form for this dialog (can be null)</param>
        /// <param name="ALedgerNumber">Ledger number whose settings are required</param>
        public TFrmLedgerSettingsDialog(Form AParentForm, Int32 ALedgerNumber) : this(AParentForm)
        {
            // Set the ledger which will load the dataset
            LedgerNumber = ALedgerNumber;
        }

        /// <summary>
        /// Gets/sets the tab that is initially displayed.  It must be one of: 'General', 'AP' or 'Gift'
        /// </summary>
        public string InitialTab
        {
            set
            {
                if ((value == "General") || (value == "AP"))
                {
                    FInitialTab = value;
                }
                else
                {
                    throw new ArgumentException("The value for 'InitialTab' must be one of 'General' or 'AP'");
                }
            }
            get
            {
                return FInitialTab;
            }
        }

        /// <summary>
        /// Gets/sets the calendar start date
        /// </summary>
        public DateTime CalendarStartDate
        {
            get
            {
                return FCalendarStartDate;
            }
            set
            {
                FCalendarStartDate = value;
            }
        }

        /// <summary>
        /// Gets True/False depending on whether a currency change is allowed
        /// </summary>
        public Boolean CurrencyChangeAllowed
        {
            get
            {
                return FCurrencyChangeAllowed;
            }
        }

        /// <summary>
        /// Gets True/False depending on whether a calendar change is allowed
        /// </summary>
        public Boolean CalendarChangeAllowed
        {
            get
            {
                return FCalendarChangeAllowed;
            }
        }

        /// <summary>
        /// Gets a value for Current Forward Posting Periods for this ledger
        /// </summary>
        public Int32 CurrentForwardPostingPeriods
        {
            get
            {
                return FCurrentForwardPostingPeriods;
            }
        }

        /// <summary>
        /// Gets True/False depending on whether AP Requires Approval Before Posting
        /// </summary>
        public Boolean APRequiresApprovalBeforePosting
        {
            get
            {
                return ucoAPLedgerSettings.RequireApprovalBeforePosting;
            }
        }

        /// <summary>
        /// Maintain settings for this ledger
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                FLedgerNumber = value;

                // Load all the data for this ledger's settings
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Setup.WebConnectors.LoadLedgerSettings(FLedgerNumber, out FCalendarStartDate,
                        out FCurrencyChangeAllowed, out FCalendarChangeAllowed));
                ALedgerRow ledgerRow = (ALedgerRow)FMainDS.ALedger.Rows[0];
                FCurrentForwardPostingPeriods = ledgerRow.NumberFwdPostingPeriods;

                // Tell the pages that the data is available.  They will set the initial values of the controls accordingly.
                ucoGeneralLedgerSettings.InitializeScreenData(this, FLedgerNumber);
                ucoAPLedgerSettings.InitializeScreenData(this, FLedgerNumber);
            }
            get
            {
                return FLedgerNumber;
            }
        }

        /// <summary>
        /// The screen has been shown
        /// </summary>
        private void RunOnceOnActivationManual()
        {
            FActiveTab = tpgLedger;

            // See if we were launched with an initial tab set??
            if (FInitialTab == "AP")
            {
                tabAllSettings.SelectedTab = tpgAccountsPayable;
                FActiveTab = tpgAccountsPayable;
            }

            ALedgerRow ledgerRow = (ALedgerRow)FMainDS.ALedger.Rows[0];
            this.Text = this.Text + String.Format(" - {0}", TFinanceControls.GetLedgerNumberAndName(ledgerRow.LedgerNumber));

            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);
        }

        /// <summary>
        /// The user has clicked on the tabs area
        /// </summary>
        private void OnTabChange(object sender, EventArgs e)
        {
            // On a tab change it is good to only validate the active tab.
            // This prevents a deadlock in the (unlikely) event that data on the tab being tabbed to is invalid.
            // If that happens and we validate all the data on all tabs we prevent the user from reaching the tab with the offending data.
            // This way we just validate the active tab
            if (FActiveTab == tpgLedger)
            {
                // Validate the ledger settings tab only
                if (ValidateAllData(ucoGeneralLedgerSettings, true, true))
                {
                    FActiveTab = tabAllSettings.SelectedTab;
                }
                else
                {
                    tabAllSettings.SelectedTab = tpgLedger;
                }
            }
            else
            {
                // active tab was AP so now it must be ledger
                // There is no validation required on AP tab (yet?)
                FActiveTab = tpgLedger;
            }
        }

        // Method that allows us to get data and validate a particular tab
        private UserControl FSpecificControlToValidate = null;
        private bool ValidateAllData(UserControl ASpecificControl, bool ARecordChangeVerification, bool AProcessAnyDataValidationErrors)
        {
            FSpecificControlToValidate = ASpecificControl;
            return ValidateAllData(ARecordChangeVerification, AProcessAnyDataValidationErrors);
        }

        /// <summary>
        /// This ensures that whenever SaveChanges is called we get the data from the controls and that it is valid data.
        /// Save will fail if there are validation errors
        /// </summary>
        private void ValidateDataManual(ALedgerRow ARow)
        {
            if (FSpecificControlToValidate == null)
            {
                ucoGeneralLedgerSettings.GetValidatedData();
                ucoAPLedgerSettings.GetValidatedData();
            }
            else if (FSpecificControlToValidate is TUC_GeneralLedgerSettings)
            {
                ucoGeneralLedgerSettings.GetValidatedData();
            }
            else if (FSpecificControlToValidate is TUC_APLedgerSettings)
            {
                ucoAPLedgerSettings.GetValidatedData();
            }
            else
            {
                throw new ArgumentException("Unknown control to validate");
            }

            // Reset our variable
            FSpecificControlToValidate = null;
        }

        /// <summary>
        /// Time to save the screen and quit
        /// </summary>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            SaveChanges();

            Close();
        }

        /// <summary>
        /// Time to save the screen but don't quit
        /// </summary>
        private void BtnApply_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        /// <summary>
        /// Main method for saving the screen data.
        /// Stores the settings for all the pages, since everything shares the same FMainDS
        /// </summary>
        private TSubmitChangesResult StoreManualCode(ref GLSetupTDS ASubmitChanges, out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;

            // save ledger settings now
            // Note:  a_ledger_init_flag records are automatically added/removed on server side for the following
            //           based on the values in the a_ledger table
            // * suspense account flag: "SUSP-ACCT"
            // * budget flag: "BUDGET"
            // * branch processing: "BRANCH-PROCESS" (this is a new flag for OpenPetra)
            // * base currency: "CURRENCY"
            // * international currency: "INTL-CURRENCY" (this is a new flag for OpenPetra)
            // * current period (start of ledger date): CURRENT-PERIOD
            // * calendar settings: CAL
            // But the 'AP require approval before posting' flag is set in this GUI on the AP tab by setting the a_ledger_init_flag row directly
            //  (because there is no column in a_ledger for this option - it only exists in a_ledger_init_flag table)

            TSubmitChangesResult res = TRemote.MFinance.Setup.WebConnectors.SaveLedgerSettings(FLedgerNumber,
                FCalendarStartDate,
                ref ASubmitChanges);
            TDataCache.TMFinance.RefreshCacheableFinanceTable(TCacheableFinanceTablesEnum.LedgerDetails, FLedgerNumber);

            return res;
        }

        /// <summary>
        /// The data has been saved
        /// </summary>
        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            ucoAPLedgerSettings.OnDataSaved();
        }
    }
}