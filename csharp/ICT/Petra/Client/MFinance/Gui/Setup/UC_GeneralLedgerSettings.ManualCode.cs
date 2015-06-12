//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       wolfgangb, alanP
//
// Copyright 2004-2014 by OM International
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
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance;

namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TUC_GeneralLedgerSettings
    {
        private TFrmLedgerSettingsDialog FMainForm;
        private Int32 FLedgerNumber;
        private Boolean FEditCalendar;

        /// <summary>
        /// Sets the local reference to the main form
        /// </summary>
        public TFrmLedgerSettingsDialog MainForm
        {
            set
            {
                FMainForm = value;
            }
        }

        /// <summary>
        /// This is called as part of the constructor
        /// </summary>
        private void InitializeManualCode()
        {
        }

        /// <summary>
        /// This method is called as soon as the ledger number has been set on the main form, which follows immediately after the constructor.
        /// It is used to initialize the controls on the form, based on the values in FMainDS
        /// </summary>
        /// <param name="AMainForm"></param>
        /// <param name="ALedgerNumber"></param>
        public void InitializeScreenData(TFrmLedgerSettingsDialog AMainForm, Int32 ALedgerNumber)
        {
            FMainForm = AMainForm;
            FLedgerNumber = ALedgerNumber;

            if (!FMainForm.CurrencyChangeAllowed)
            {
                cmbBaseCurrency.Enabled = false;
                cmbIntlCurrency.Enabled = false;
            }

            if (!FMainForm.CalendarChangeAllowed)
            {
                rgrCalendarModeRadio.Enabled = false;
                nudNumberOfAccountingPeriods.Enabled = false;
                dtpFinancialYearStartDate.Enabled = false;
                nudCurrentPeriod.Enabled = false;
                btnViewCalendar.Text = MCommonResourcestrings.StrFinanceViewCalendarTitle;
                FEditCalendar = false;
            }
            else
            {
                FEditCalendar = true;
            }

            // Now we can populate the controls with data
            if (FMainDS.ALedger.Rows.Count > 0)
            {
                // check if number of forward posting periods is set to 0. This should not be allowed, therefore the value has to be changed here.
                if (((ALedgerRow)FMainDS.ALedger.Rows[0]).IsNumberFwdPostingPeriodsNull()
                    || (((ALedgerRow)FMainDS.ALedger.Rows[0]).NumberFwdPostingPeriods == 0))
                {
                    int DefaultNumber = MFinanceConstants.GL_DEFAULT_FWD_POSTING_PERIODS;

                    MessageBox.Show(String.Format(Catalog.GetString("Number of Forward Posting Periods for this ledger must not be 0 and will " +
                                "therefore be set to the default value of {0} unless you cancel the Ledger Settings Screen."),
                            DefaultNumber.ToString()),
                        Catalog.GetString("Number of Forward Posting Periods"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    ((ALedgerRow)FMainDS.ALedger.Rows[0]).NumberFwdPostingPeriods = DefaultNumber;
                }

                ShowData((ALedgerRow)FMainDS.ALedger.Rows[0]);
            }
        }

        /// <summary>
        /// Show ledger settings data from given data set
        /// This will be called after the ledger has been set but before the screen's Show() method has been called.
        /// </summary>
        /// <param name="ARow">The ledger row for which details will be shown</param>
        private void ShowDataManual(ALedgerRow ARow)
        {
            // Someone at some time must have thought that this table might be useful...
            // It does contain a lot of this info as well
            //AAccountingSystemParameterRow ParameterRow = (AAccountingSystemParameterRow)FMainDS.AAccountingSystemParameter.Rows[0];

            // In these steps we 'force' the data to be within specified limits.  Then we are free to set the max/min of the spin buttons.
            int currentPeriodMax = 13;
            int currentPeriodMin = 1;

            nudCurrentPeriod.Value = Math.Max(Math.Min(ARow.CurrentPeriod, currentPeriodMax), currentPeriodMin);
            nudCurrentPeriod.Minimum = currentPeriodMin;
            nudCurrentPeriod.Maximum = currentPeriodMax;

            int accountingPeriodsMax = 13;
            int accountingPeriodsMin = 12;
            nudNumberOfAccountingPeriods.Value = Math.Max(Math.Min(ARow.NumberOfAccountingPeriods, accountingPeriodsMax), accountingPeriodsMin);
            nudNumberOfAccountingPeriods.Minimum = accountingPeriodsMin;
            nudNumberOfAccountingPeriods.Maximum = accountingPeriodsMax;

            int fwdPostingPeriodsMax = 8;
            nudNumberFwdPostingPeriods.Value = Math.Max(Math.Min(ARow.NumberFwdPostingPeriods, fwdPostingPeriodsMax), 0);
            nudNumberFwdPostingPeriods.Maximum = fwdPostingPeriodsMax;

            int actualsDataRetentionMin = 1;
            nudActualsDataRetention.Value = Math.Max(Math.Min(ARow.ActualsDataRetention, 100), actualsDataRetentionMin);
            nudActualsDataRetention.Minimum = actualsDataRetentionMin;

            int giftDataRetentionMin = 1;
            nudGiftDataRetention.Value = Math.Max(Math.Min(ARow.GiftDataRetention, 100), giftDataRetentionMin);
            nudGiftDataRetention.Minimum = giftDataRetentionMin;

            // comment out budget data retention settings for now until they are properly used in OpenPetra
            //int budgetDataRetentionMin = 1;
            //nudBudgetDataRetention.Value = Math.Max(Math.Min(ARow.BudgetDataRetention, 100), budgetDataRetentionMin);
            //nudBudgetDataRetention.Minimum = budgetDataRetentionMin;

            if (ARow.CalendarMode)
            {
                rbtMonthly.Checked = true;
            }
            else
            {
                rbtNonMonthly.Checked = true;
            }

            CalendarModeChanged(null, null);

            if (FMainForm.CalendarStartDate != DateTime.MinValue)
            {
                dtpFinancialYearStartDate.Date = FMainForm.CalendarStartDate;
            }
        }

        // This is called after the screen's Show() method has been called
        private void RunOnceOnParentActivation()
        {
            nudNumberOfAccountingPeriods.KeyDown += numericUpDown_KeyDown;
            nudNumberFwdPostingPeriods.KeyDown += numericUpDown_KeyDown;
            nudCurrentPeriod.KeyDown += numericUpDown_KeyDown;
            nudActualsDataRetention.KeyDown += numericUpDown_KeyDown;
            nudGiftDataRetention.KeyDown += numericUpDown_KeyDown;
        }

        /// <summary>
        /// This will be called by the parent form when data needs to be saved
        /// It is when we need to update the dataset based on the screens control's current values
        /// </summary>
        public bool GetValidatedData()
        {
            // call the auto-generated code
            return ValidateAllData(TErrorProcessingMode.Epm_None);
        }

        /// <summary>
        /// Additional tasks in addition to those defined by the auto-generator
        /// </summary>
        private void GetDataFromControlsManual(ALedgerRow ARow)
        {
            ARow.CalendarMode = rbtMonthly.Checked;

            if (dtpFinancialYearStartDate.Text.Trim() == "")
            {
                FMainForm.CalendarStartDate = DateTime.MinValue;
            }
            else
            {
                // set ledger row to modified if calendar date has changed (this is necessary as otherwise SaveChanges() would
                // not call StoreManualCode(...) if there was no other change on that screen and then the calendar change
                // would not get triggered on server
                if (FMainForm.CalendarStartDate != dtpFinancialYearStartDate.Date.Value)
                {
                    ARow.SetModified();
                }

                FMainForm.CalendarStartDate = dtpFinancialYearStartDate.Date.Value;
            }
        }

        private void CalendarModeChanged(System.Object sender, EventArgs e)
        {
            nudNumberOfAccountingPeriods.Enabled = !rbtMonthly.Checked;

            if (rbtMonthly.Checked)
            {
                nudNumberOfAccountingPeriods.Enabled = false;
                nudNumberOfAccountingPeriods.Value = 12;
                dtpFinancialYearStartDate.Enabled = true;
                btnViewCalendar.Text = MCommonResourcestrings.StrFinanceViewCalendarTitle;
                FEditCalendar = false;
            }
            else
            {
                nudNumberOfAccountingPeriods.Enabled = true;
                dtpFinancialYearStartDate.Enabled = true;
                btnViewCalendar.Text = MCommonResourcestrings.StrFinanceEditCalendarTitle;
                FEditCalendar = true;
            }

            if (!FMainForm.CalendarChangeAllowed)
            {
                rgrCalendarModeRadio.Enabled = false;
                nudNumberOfAccountingPeriods.Enabled = false;
                dtpFinancialYearStartDate.Enabled = false;
                nudCurrentPeriod.Enabled = false;
                btnViewCalendar.Text = MCommonResourcestrings.StrFinanceViewCalendarTitle;
                FEditCalendar = false;
            }
        }

        private void OnBtnCalendar(System.Object sender, EventArgs e)
        {
            if (FPetraUtilsObject.HasChanges
                && FEditCalendar)
            {
                // If it is an editable calendar we need to save any outstanding changes first because they impact the calendar edit form
                string msg = MCommonResourcestrings.StrFinanceSaveBeforeEditCalendar;
                msg += "  ";
                msg += MCommonResourcestrings.StrFinanceSaveByUsingApply;
                MessageBox.Show(msg, MCommonResourcestrings.StrFinanceEditCalendarTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                TFrmSetupAccountingPeriod Calendar = new TFrmSetupAccountingPeriod(FMainForm);
                Calendar.LedgerNumber = FLedgerNumber;
                Calendar.ReadOnly = !FEditCalendar;
                Calendar.Show();
            }
        }

        // The main validation method for the controls on this tab page
        private void ValidateDataManual(ALedgerRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;
            DataColumn ValidationColumn;
            TValidationControlsData ValidationControlsData;
            TVerificationResult VerificationResult = null;

            // make sure that Financial Year Start Date is no later than 28th of a month
            // (this field is not part of a_ledger but will be stored in period 1 of a_accounting_period)
            if (rbtMonthly.Checked)
            {
                // make sure that Financial Year Start Date is not empty
                // (this field is not part of a_ledger but will be stored in period 1 of a_accounting_period)
                if (dtpFinancialYearStartDate.Date == null)
                {
                    VerificationResult = new TScreenVerificationResult(
                        TDateChecks.IsNotUndefinedDateTime(
                            dtpFinancialYearStartDate.Date,
                            lblFinancialYearStartDate.Text.Trim(':'),
                            true,
                            this),
                        null,
                        dtpFinancialYearStartDate);
                }
                else if (dtpFinancialYearStartDate.Date.Value.Day > 28)
                {
                    VerificationResult = new TScreenVerificationResult(
                        this,
                        new DataColumn(),
                        ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_PERIOD_START_DAY_AFTER_28).ErrorMessageText,
                        PetraErrorCodes.ERR_PERIOD_START_DAY_AFTER_28,
                        dtpFinancialYearStartDate,
                        TResultSeverity.Resv_Critical);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, null);
            }

            // check that there no suspense accounts for this ledger if box is unticked
            ValidationColumn = ARow.Table.Columns[ALedgerTable.ColumnSuspenseAccountFlagId];

            if (FValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (!ARow.SuspenseAccountFlag)
                {
                    if (TRemote.MFinance.Common.ServerLookups.WebConnectors.HasSuspenseAccounts(FLedgerNumber))
                    {
                        VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                                ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NO_SUSPENSE_ACCOUNTS_ALLOWED)),
                            ValidationColumn, ValidationControlsData.ValidationControl);
                    }
                    else
                    {
                        VerificationResult = null;
                    }

                    // Handle addition/removal to/from TVerificationResultCollection
                    VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
                }
            }

            // check that the number of forwarding periods is not less than the already used ones
            ValidationColumn = ARow.Table.Columns[ALedgerTable.ColumnNumberFwdPostingPeriodsId];

            if (FValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.NumberFwdPostingPeriods < FMainForm.CurrentForwardPostingPeriods)
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_NUMBER_FWD_PERIODS_TOO_SMALL,
                                new string[] { FMainForm.CurrentForwardPostingPeriods.ToString(), FMainForm.CurrentForwardPostingPeriods.ToString() })),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
            }

            // check that current period is not greater than number of ledger periods
            ValidationColumn = ARow.Table.Columns[ALedgerTable.ColumnNumberOfAccountingPeriodsId];

            if (FValidationControlsDict.TryGetValue(ValidationColumn, out ValidationControlsData))
            {
                if (ARow.CurrentPeriod > ARow.NumberOfAccountingPeriods)
                {
                    VerificationResult = new TScreenVerificationResult(new TVerificationResult(this,
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_CURRENT_PERIOD_TOO_LATE)),
                        ValidationColumn, ValidationControlsData.ValidationControl);
                }
                else
                {
                    VerificationResult = null;
                }

                // Handle addition/removal to/from TVerificationResultCollection
                VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, VerificationResult, ValidationColumn);
            }
        }

        /// <summary>
        /// Checks for limited number of digits and no negatives
        /// in a Numeric Up/Down box
        /// </summary>
        private void numericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.KeyData == Keys.Back) || (e.KeyData == Keys.Delete)))
            {
                if ((sender == nudNumberOfAccountingPeriods)
                    || (sender == nudNumberFwdPostingPeriods)
                    || (sender == nudCurrentPeriod)
                    || (sender == nudActualsDataRetention)
                    || (sender == nudGiftDataRetention))
                {
                    if ((((NumericUpDown)sender).Text.Length >= 2) || (e.KeyValue == 109))
                    {
                        e.SuppressKeyPress = true;
                        e.Handled = true;
                    }
                }
            }
        }
    }
}