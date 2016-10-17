//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

using Ict.Common;
using Ict.Common.Verification;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.Security;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.CommonDialogs;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupCorporateExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger = null;

        /// <summary>
        /// The International currency is used to initialize the "to" combobox
        /// </summary>
        private String intlCurrencyOfLedger = null;

        /// <summary>
        /// A ledger table containing all the ledgers that this client has access to
        /// </summary>
        private ALedgerTable FAvailableLedgers = null;

        /// <summary>
        /// This variable will normally be 1, but if there is a ledger with a different first day of accounting period it will have that value
        /// </summary>
        private int FAlternativeFirstDayInMonth = 1;

        /// <summary>
        /// Holds the value of the user preference for display format of exchange rates
        /// </summary>
        private bool FUseCurrencyFormatForDecimal = true;

        /// <summary>
        /// We use this to hold inverse exchange rate items that will need saving at the end
        /// </summary>
        private struct tInverseItem
        {
            public string FromCurrencyCode;
            public string ToCurrencyCode;
            public DateTime DateEffective;
            public decimal RateOfExchange;
        }

        /// <summary>
        /// The definition of the ledger number is used to define some
        /// default values and it initializes the dialog to run in the non modal
        /// form ...
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, value))[0];
                baseCurrencyOfLedger = ledger.BaseCurrency;
                intlCurrencyOfLedger = ledger.IntlCurrency;

                mniImport.Enabled = true;
                tbbImport.Enabled = true;
            }
        }

        private void InitializeManualCode()
        {
            // load the data
            Ict.Common.Data.TTypedDataTable TypedTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(ACorporateExchangeRateTable.GetTableDBName(), null, out TypedTable);
            FMainDS.ACorporateExchangeRate.Merge(TypedTable);

            FUseCurrencyFormatForDecimal = TUserDefaults.GetBooleanDefault(Ict.Common.StringHelper.FINANCE_DECIMAL_FORMAT_AS_CURRENCY, true);

            FPetraUtilsObject.ApplySecurity(TSecurityChecks.SecurityPermissionsSetupScreensEditingAndSaving);

            if (FPetraUtilsObject.SecurityReadOnly)
            {
                mniImport.Enabled = false;
                tbbImport.Enabled = false;
            }
        }

        private void RunOnceOnActivationManual()
        {
            // Set the Tag for the checkbox since we don't want changes to the checkbox to look like we have to save the data
            this.chkHideOthers.Tag = MCommon.MCommonResourcestrings.StrCtrlSuppressChangeDetection;

            // Activate events we will use in manual code
            this.txtDetailRateOfExchange.TextChanged +=
                new System.EventHandler(this.txtDetailRateOfExchange_TextChanged);

            // These Leave events are all fired before validation updates the row
            // This is important because we need to suggest a new date/rate depending on the selection made
            //  and so we need to be able to check on the existence of specific rows in the data table before they get updated on validation
            this.cmbDetailFromCurrencyCode.Leave +=
                new System.EventHandler(this.CurrencyCodeComboBox_Leave);
            this.cmbDetailToCurrencyCode.Leave +=
                new System.EventHandler(this.CurrencyCodeComboBox_Leave);
            this.dtpDetailDateEffectiveFrom.Leave +=
                new EventHandler(dtpDetailDateEffectiveFrom_Leave);

            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);
            this.chkHideOthers.CheckedChanged +=
                new EventHandler(chkHideOthers_CheckedChanged);

            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);

            // What ledgers does the user have access to??
            FAvailableLedgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();
            DataView ledgerView = FAvailableLedgers.DefaultView;
            ledgerView.RowFilter = "a_ledger_status_l = 1";     // Only view 'in use' ledgers

            for (int i = 0; i < ledgerView.Count; i++)
            {
                // Have a last attempt at deciding what the base currency is...
                if (baseCurrencyOfLedger == null)
                {
                    // we default to the first one we find
                    baseCurrencyOfLedger = ((ALedgerRow)ledgerView[i].Row).BaseCurrency;
                }

                if (intlCurrencyOfLedger == null)
                {
                    // we default to the first one we find
                    intlCurrencyOfLedger = ((ALedgerRow)ledgerView[i].Row).IntlCurrency;
                }

                // Get the accounting periods for this ledger
                AAccountingPeriodTable periods = (AAccountingPeriodTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                    TCacheableFinanceTablesEnum.AccountingPeriodList,
                    ((ALedgerRow)ledgerView[i].Row).LedgerNumber);

                if ((periods != null) && (periods.Rows.Count > 0))
                {
                    int firstDay = ((AAccountingPeriodRow)periods.Rows[0]).PeriodStartDate.Day;

                    if ((FAlternativeFirstDayInMonth == 1) && (firstDay != 1))
                    {
                        // Now we have an alternative first day of month
                        FAlternativeFirstDayInMonth = firstDay;
                    }
                    else if ((FAlternativeFirstDayInMonth != 1) && (firstDay != 1) && (firstDay != FAlternativeFirstDayInMonth))
                    {
                        // Ooops.  Now we seem to have more than one alternative first day of month.
                        // We can't cope with that level of complexity!
                        FAlternativeFirstDayInMonth = 0;
                    }
                }
            }

            DataView myView = FMainDS.ACorporateExchangeRate.DefaultView;
            myView.Sort = ACorporateExchangeRateTable.GetToCurrencyCodeDBName() + ", " +
                          ACorporateExchangeRateTable.GetFromCurrencyCodeDBName() + ", " +
                          ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + " DESC";
            myView.RowFilter = "";

            if (myView.Count > 0)
            {
                // We have to use this construct because simple ShoWDetails requires two cursor down keypresses to move the cursor
                // because we have changed the row filter.
                grdDetails.Selection.Focus(new SourceGrid.Position(1, 0), false);
                ShowDetails();
            }
            else
            {
                ShowDetails(null);
            }
        }

        void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            // The user has clicked Save.  We need to consider if we need to make any Inverse currency additions...
            // We need to update the details and validate them first
            // When we return from this method the standard code will do the validation again and might not allow the save to go ahead
            FPetraUtilsObject.VerificationResultCollection.Clear();
            ValidateAllData(false, TErrorProcessingMode.Epm_None);

            if (!TVerificationHelper.IsNullOrOnlyNonCritical(FPetraUtilsObject.VerificationResultCollection))
            {
                return;
            }

            // Now go through all the grid rows (view) checking all the added rows.  Keep a list of inverses
            List <tInverseItem>lstInverses = new List <tInverseItem>();
            DataView gridView = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = 0; i < gridView.Count; i++)
            {
                ACorporateExchangeRateRow ARow = (ACorporateExchangeRateRow)gridView[i].Row;

                if (ARow.RowState == DataRowState.Added)
                {
                    tInverseItem item = new tInverseItem();
                    item.FromCurrencyCode = ARow.ToCurrencyCode;
                    item.ToCurrencyCode = ARow.FromCurrencyCode;
                    item.RateOfExchange = Math.Round(1 / ARow.RateOfExchange, 10);
                    item.DateEffective = ARow.DateEffectiveFrom;
                    lstInverses.Add(item);
                }
            }

            if (lstInverses.Count == 0)
            {
                return;
            }

            // Now go through our list and check if any items need adding to the data Table
            // The user may already have put an inverse currency in by hand
            DataView dv = new DataView(FMainDS.ACorporateExchangeRate);

            for (int i = 0; i < lstInverses.Count; i++)
            {
                tInverseItem item = lstInverses[i];

                // Does the item exist already?
                dv.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4}=#{5}#",
                    ACorporateExchangeRateTable.GetFromCurrencyCodeDBName(),
                    item.FromCurrencyCode,
                    ACorporateExchangeRateTable.GetToCurrencyCodeDBName(),
                    item.ToCurrencyCode,
                    ACorporateExchangeRateTable.GetDateEffectiveFromDBName(),
                    item.DateEffective.ToString("d", CultureInfo.InvariantCulture));

                if (dv.Count == 0)
                {
                    ACorporateExchangeRateRow NewRow = FMainDS.ACorporateExchangeRate.NewRowTyped();
                    NewRow.FromCurrencyCode = item.FromCurrencyCode;
                    NewRow.ToCurrencyCode = item.ToCurrencyCode;
                    NewRow.DateEffectiveFrom = DateTime.Parse(item.DateEffective.ToLongDateString());
                    NewRow.RateOfExchange = item.RateOfExchange;

                    FMainDS.ACorporateExchangeRate.Rows.Add(NewRow);
                }
            }

            // Now make sure to select the row that was currently selected when we started the Save operation
            SelectRowInGrid(grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1);
        }

        private void ValidateDataDetailsManual(ACorporateExchangeRateRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GLSetup.ValidateCorporateExchangeRate(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict, FAvailableLedgers, FAlternativeFirstDayInMonth);

            // In MODAL mode we can validate that the date is the same as an accounting period...
        }

        /// <summary>
        /// Create a new CorporateExchangeRateRow ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            CreateNewACorporateExchangeRate();

            UpdateExchangeRateLabels();
        }

        private void NewRowManual(ref ACorporateExchangeRateRow ARow)
        {
            // We just need to decide on the appropriate currency pair and then call the standard method to get a suggested rate and date
            if (FPreviouslySelectedDetailRow == null)
            {
                // Corporate Exchange rates are not part of any ledger, so baseCurrencyOfLedger may be null...
                if ((baseCurrencyOfLedger == null) || (intlCurrencyOfLedger == null) || (baseCurrencyOfLedger == intlCurrencyOfLedger))
                {
                    ARow.FromCurrencyCode = "GBP";
                    ARow.ToCurrencyCode = "USD";
                }
                else
                {
                    ARow.FromCurrencyCode = baseCurrencyOfLedger;
                    ARow.ToCurrencyCode = intlCurrencyOfLedger;
                }
            }
            else
            {
                // Use the same settings as the highlighted row
                ARow.FromCurrencyCode = cmbDetailFromCurrencyCode.GetSelectedString();
                ARow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
            }

            DateTime suggestedDate;
            decimal suggestedRate;
            GetSuggestedDateAndRateForCurrencyPair(ARow.FromCurrencyCode, ARow.ToCurrencyCode, out suggestedDate, out suggestedRate);
            ARow.DateEffectiveFrom = suggestedDate;
            ARow.RateOfExchange = suggestedRate;

            // The time is always 0 for corporate exchange rate
            ARow.TimeEffectiveFrom = 0;
        }

        private bool PreDeleteManual(ACorporateExchangeRateRow ARowToDelete, ref string ADeletionQuestion)
        {
            // Check if corporate exchange rate can be deleted.
            // Cannot be deleted if it is effective for a period in the current year which has at least one batch.
            if (!TRemote.MFinance.Setup.WebConnectors.CanDeleteCorporateExchangeRate(
                    ARowToDelete.DateEffectiveFrom, ARowToDelete.ToCurrencyCode, ARowToDelete.FromCurrencyCode))
            {
                MessageBox.Show(Catalog.GetString("Corporate Exchange Rate cannot be deleted because there are still accounts with balances."),
                    Catalog.GetString("Delete Corporate Exchange Rate"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            ADeletionQuestion = Catalog.GetString("Are you sure you want to delete the current row?");
            ADeletionQuestion += String.Format(Catalog.GetString("{0}{0}({1} to {2} effective from {3})"),
                Environment.NewLine,
                ARowToDelete.FromCurrencyCode,
                ARowToDelete.ToCurrencyCode,
                ARowToDelete.DateEffectiveFrom.ToString("dd-MMM-yyyy"));
            return true;
        }

        /// <summary>
        /// TextChanged Event for txtDetailRateOfExchange
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void txtDetailRateOfExchange_TextChanged(System.Object sender, EventArgs e)
        {
            if (txtDetailRateOfExchange.Text.Trim() != String.Empty)
            {
                UpdateExchangeRateLabels();
            }
        }

        /// <summary>
        /// Updates the lblValueOneDirection and lblValueOtherDirection labels
        /// </summary>
        private void UpdateExchangeRateLabels(Object sender = null, EventArgs e = null)
        {
            // Call can cope with null for Row, but rate must have a valid value
            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                TSetupExchangeRates.SetExchangeRateLabels(cmbDetailFromCurrencyCode.GetSelectedString(),
                    cmbDetailToCurrencyCode.GetSelectedString(), GetSelectedDetailRow(),
                    txtDetailRateOfExchange.NumberValueDecimal.Value, FUseCurrencyFormatForDecimal, lblValueOneDirection, lblValueOtherDirection);
            }
            else
            {
                TSetupExchangeRates.SetExchangeRateLabels(String.Empty,
                    String.Empty,
                    null,
                    1.0m,
                    FUseCurrencyFormatForDecimal,
                    lblValueOneDirection,
                    lblValueOtherDirection);
            }
        }

        /// <summary>
        /// Leave Event for the currency boxes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void CurrencyCodeComboBox_Leave(System.Object sender, EventArgs e)
        {
            // This gets called whenever the user leaves a currency box
            // This could be a real change or it could just be a tab through
            // The key thing is that we get called before control validation so the data will not be updated yet
            string strFrom = cmbDetailFromCurrencyCode.GetSelectedString();
            string strTo = cmbDetailToCurrencyCode.GetSelectedString();

            // Compare these current values with what we had last time
            if ((strFrom != FPreviouslySelectedDetailRow.FromCurrencyCode)
                || (strTo != FPreviouslySelectedDetailRow.ToCurrencyCode))
            {
                // It must be a real change - so we should calculate a new effective date and propose an exchange rate
                DateTime suggestedDate;
                decimal suggestedRate;
                GetSuggestedDateAndRateForCurrencyPair(strFrom, strTo, out suggestedDate, out suggestedRate);
                dtpDetailDateEffectiveFrom.Date = suggestedDate;
                txtDetailRateOfExchange.NumberValueDecimal = suggestedRate;
            }

            SetEnabledStates(true);
        }

        void dtpDetailDateEffectiveFrom_Leave(object sender, EventArgs e)
        {
            // Note that we use Leave because it is fired before control validation
            // Get a new rate for the date
            decimal suggestedRate;

            try
            {
                DateTime dt = dtpDetailDateEffectiveFrom.Date.Value;
                DateTime dtFirstOfMonth = new DateTime(dt.Year, dt.Month, 1);

                if (FAlternativeFirstDayInMonth != 0)
                {
                    // We do have ledgers that start either on day 1 or a uniform alternative day
                    DateTime dtAlternativeFirstOfMonth = new DateTime(dt.Year, dt.Month, FAlternativeFirstDayInMonth);

                    if ((dt != dtFirstOfMonth) && (dt != dtAlternativeFirstOfMonth))
                    {
                        dt = dtFirstOfMonth;
                        dtpDetailDateEffectiveFrom.Date = dt;
                    }
                }

                if (dt != FPreviouslySelectedDetailRow.DateEffectiveFrom)
                {
                    // The date in the control is different from the value in the table
                    GetSuggestedRateForCurrencyAndDate(cmbDetailFromCurrencyCode.GetSelectedString(),
                        cmbDetailToCurrencyCode.GetSelectedString(), dt, out suggestedRate);
                    txtDetailRateOfExchange.NumberValueDecimal = suggestedRate;
                }
            }
            catch (InvalidOperationException)
            {
                // ooops.  The date is empty or badly formed
                txtDetailRateOfExchange.NumberValueDecimal = 0.0m;
            }
        }

        /// <summary>
        /// This routines supports a small gui-calculator. The user can easily calculate the
        /// reciproke value of the exchange rate.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void InvertExchangeRate(System.Object sender, EventArgs e)
        {
            try
            {
                txtDetailRateOfExchange.NumberValueDecimal = Math.Round(1 / txtDetailRateOfExchange.NumberValueDecimal.Value, 10);
            }
            catch (Exception)
            {
            }

            UpdateExchangeRateLabels();
        }

        void chkHideOthers_CheckedChanged(object sender, EventArgs e)
        {
            string rowFilter = String.Empty;

            if (chkHideOthers.Checked)
            {
                rowFilter = String.Format("{0}='{1}'",
                    ACorporateExchangeRateTable.GetToCurrencyCodeDBName(),
                    cmbDetailToCurrencyCode.GetSelectedString());
            }

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(rowFilter, !chkHideOthers.Checked);
            FFilterAndFindObject.ApplyFilter();
            SelectRowInGrid(grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1);

            SetEnabledStates(FPreviouslySelectedDetailRow.RowState == DataRowState.Added);
        }

        private void GetDetailDataFromControlsManual(ACorporateExchangeRateRow ARow)
        {
            // only do this is the user has actually changed the exchange rate
            if ((ARow.RowState != DataRowState.Added)
                && (Convert.ToDecimal(ARow[ACorporateExchangeRateTable.GetRateOfExchangeDBName(), DataRowVersion.Original]) != ARow.RateOfExchange))
            {
                // Check if we have an inverse rate for this date/time and currency pair
                ACorporateExchangeRateRow mainRow = (ACorporateExchangeRateRow)FMainDS.ACorporateExchangeRate.Rows.Find(
                    new object[] { ARow.ToCurrencyCode, ARow.FromCurrencyCode, ARow.DateEffectiveFrom });

                if ((mainRow != null) && (ARow.RateOfExchange != 0.0m))
                {
                    // Checking to see if we have a matching rate is tricky because rounding errors mean that the inverse of an inverse
                    // does not always get you back where you started.  So we check both ways to look for a match.
                    // If neither way matches we need to do an update, but if there is a match in at least one direction, we leave the other row as it is.
                    decimal inverseRate = Math.Round(1 / ARow.RateOfExchange, 10);
                    decimal inverseRateAlt = Math.Round(1 / mainRow.RateOfExchange, 10);

                    if ((mainRow.RateOfExchange != inverseRate) && (ARow.RateOfExchange != inverseRateAlt))
                    {
                        // Neither way matches so we must have made a change that requires an update to the inverse row
                        mainRow.BeginEdit();
                        mainRow.RateOfExchange = inverseRate;
                        mainRow.EndEdit();
                    }
                }
            }
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ACorporateExchangeRateRow ARow)
        {
            if (ARow != null)
            {
                if (ARow.FromCurrencyCode == ARow.ToCurrencyCode)
                {
                    ARow.RateOfExchange = 1.0m;
                }

                SetEnabledStates(ARow.RowState == DataRowState.Added);
            }
            else
            {
                txtDetailRateOfExchange.NumberValueDecimal = null;
            }

            UpdateExchangeRateLabels();
        }

        private void Import(System.Object sender, EventArgs e)
        {
            if (ValidateAllData(true, TErrorProcessingMode.Epm_All))
            {
                TVerificationResultCollection results = FPetraUtilsObject.VerificationResultCollection;

                int nRowsImported = TImportExchangeRates.ImportCurrencyExRates(FMainDS.ACorporateExchangeRate, "Corporate", results);

                if (results.Count > 0)
                {
                    string formatter;

                    if (nRowsImported == 0)
                    {
                        formatter = MCommonResourcestrings.StrExchRateImportNoRows;
                    }
                    else if (nRowsImported == 1)
                    {
                        formatter = MCommonResourcestrings.StrExchRateImportOneRow;
                    }
                    else
                    {
                        formatter = MCommonResourcestrings.StrExchRateImportMultiRow;
                    }

                    formatter += "{0}{0}{1}{0}{0}{3}{0}{0}{4}";

                    TFrmExtendedMessageBox messageBox = new TFrmExtendedMessageBox(this);
                    messageBox.ShowDialog(String.Format(
                            formatter,
                            Environment.NewLine,
                            results[0].ResultText,
                            nRowsImported,
                            results[0].ResultSeverity ==
                            TResultSeverity.Resv_Critical ? MCommonResourcestrings.StrExchRateImportTryAgain : String.Empty,
                            results[0].ResultCode),
                        MCommonResourcestrings.StrExchRateImportTitle, String.Empty, TFrmExtendedMessageBox.TButtons.embbOK,
                        results[0].ResultSeverity ==
                        TResultSeverity.Resv_Critical ? TFrmExtendedMessageBox.TIcon.embiError : TFrmExtendedMessageBox.TIcon.embiInformation);

                    results.Clear();
                }
                else if (nRowsImported == 0)
                {
                    MessageBox.Show(MCommonResourcestrings.StrExchRateImportNoRows, MCommonResourcestrings.StrExchRateImportTitle);
                }
                else if (nRowsImported == 1)
                {
                    MessageBox.Show(MCommonResourcestrings.StrExchRateImportOneRowSuccess, MCommonResourcestrings.StrExchRateImportTitle);
                }
                else
                {
                    MessageBox.Show(String.Format(MCommonResourcestrings.StrExchRateImportMultiRowSuccess,
                            nRowsImported), MCommonResourcestrings.StrExchRateImportTitle);
                }

                if (nRowsImported > 0)
                {
                    FPetraUtilsObject.SetChangedFlag();
                }
            }
        }

        private void SetEnabledStates(bool RowCanBeChanged)
        {
            bool bEnable = (cmbDetailFromCurrencyCode.GetSelectedString() != cmbDetailToCurrencyCode.GetSelectedString());

            txtDetailRateOfExchange.Enabled = bEnable;
            btnInvertExchangeRate.Enabled = bEnable;
            cmbDetailToCurrencyCode.Enabled = RowCanBeChanged && !chkHideOthers.Checked;
            cmbDetailFromCurrencyCode.Enabled = RowCanBeChanged;
        }

        /// <summary>
        /// This is the standard method that is used to suggest a rate and effective date for a new condition.
        /// The suggestions depend on the FromCurrency and ToCurrency and is based on the other values in the table
        /// The method is called both when creating a new row and when modifying the currencies of an existing row
        /// </summary>
        /// <param name="FromCurrency">The FromCurrency</param>
        /// <param name="ToCurrency">The ToCurrency</param>
        /// <param name="SuggestedDate">The suggested effective date for the currency pair</param>
        /// <param name="SuggestedRate">The suggested effective rate of exchange for the currency pair</param>
        private void GetSuggestedDateAndRateForCurrencyPair(string FromCurrency,
            string ToCurrency,
            out DateTime SuggestedDate,
            out decimal SuggestedRate)
        {
            // Date will be the first of the current month.  If that is already used then try successive months
            DateTime NewDateEffectiveFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            while (FMainDS.ACorporateExchangeRate.Rows.Find(new object[] {
                           FromCurrency, ToCurrency, NewDateEffectiveFrom.ToString()
                       }) != null)
            {
                NewDateEffectiveFrom = NewDateEffectiveFrom.AddMonths(1);
            }

            SuggestedDate = NewDateEffectiveFrom;

            GetSuggestedRateForCurrencyAndDate(FromCurrency, ToCurrency, SuggestedDate, out SuggestedRate);
        }

        /// <summary>
        /// This is the standard method that is used to suggest a rate for a new condition.
        /// The suggestions depend on the FromCurrency, ToCurrency and effective date and is based on the other values in the table
        /// The method is called both when creating a new row and when modifying the currencies of an existing row
        /// </summary>
        /// <param name="FromCurrency">The FromCurrency</param>
        /// <param name="ToCurrency">The ToCurrency</param>
        /// <param name="EffectiveDate">The Effective Date for the currency pair</param>
        /// <param name="SuggestedRate">The suggested effective rate of exchange for the currency pair on that date</param>
        private void GetSuggestedRateForCurrencyAndDate(string FromCurrency,
            string ToCurrency,
            DateTime EffectiveDate,
            out decimal SuggestedRate)
        {
            SuggestedRate = 0.0m;

            if (FromCurrency == ToCurrency)
            {
                // Always 1.0
                SuggestedRate = 1.0m;
            }
            else
            {
                decimal tryRate;

                if (GetSuggestedRateForCurrencyAndDateInternal(FromCurrency, ToCurrency, EffectiveDate, out tryRate))
                {
                    // Use this rate
                    SuggestedRate = tryRate;
                }
                else
                {
                    //Petra 2.x tries to work it out if you know the rates of both currencies to a third one (USD)
                    if (!((FromCurrency == "USD") || (ToCurrency == "USD")))
                    {
                        decimal From2USD, USD2To, To2USD, USD2From;

                        if (GetSuggestedRateForCurrencyAndDateInternal(FromCurrency, "USD", EffectiveDate, out From2USD)
                            && GetSuggestedRateForCurrencyAndDateInternal(ToCurrency, "USD", EffectiveDate, out To2USD))
                        {
                            SuggestedRate = Math.Round(From2USD / To2USD, 10);
                        }
                        else if (GetSuggestedRateForCurrencyAndDateInternal("USD", FromCurrency, EffectiveDate, out USD2From)
                                 && GetSuggestedRateForCurrencyAndDateInternal("USD", ToCurrency, EffectiveDate, out USD2To))
                        {
                            SuggestedRate = Math.Round(USD2To / USD2From, 10);
                        }
                    }
                }
            }
        }

        private bool GetSuggestedRateForCurrencyAndDateInternal(string FromCurrency,
            string ToCurrency,
            DateTime EffectiveDate,
            out decimal SuggestedRate)
        {
            // If we cannot come up with a rate, it will be 0.0 (which is not allowed so it will force the user to enter a better number)
            SuggestedRate = 0.0m;

            // Rate of exchange will be the latest value used, if there is one
            // Get the most recent value for this currency pair
            string rowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4} <= #{5}#",
                ACorporateExchangeRateTable.GetFromCurrencyCodeDBName(),
                FromCurrency,
                ACorporateExchangeRateTable.GetToCurrencyCodeDBName(),
                ToCurrency,
                ACorporateExchangeRateTable.GetDateEffectiveFromDBName(),
                EffectiveDate.ToString("d", CultureInfo.InvariantCulture));
            string sortBy = String.Format("{0} DESC", ACorporateExchangeRateTable.GetDateEffectiveFromDBName());
            DataView dv = new DataView(FMainDS.ACorporateExchangeRate, rowFilter, sortBy, DataViewRowState.CurrentRows);

            if (dv.Count > 0)
            {
                // Use this rate
                SuggestedRate = ((ACorporateExchangeRateRow)dv[0].Row).RateOfExchange;
                return true;
            }

            return false;
        }

        private TSubmitChangesResult StoreManualCode(ref CorporateExchangeSetupTDS ASubmitChanges,
            out TVerificationResultCollection AVerificationResult)
        {
            AVerificationResult = null;
            int TransactionsChanged;

            TSubmitChangesResult Result = TRemote.MFinance.Setup.WebConnectors.SaveCorporateExchangeSetupTDS(
                ref ASubmitChanges, out TransactionsChanged, out AVerificationResult);

            if ((Result == TSubmitChangesResult.scrOK) && (TransactionsChanged > 0) && (FPetraUtilsObject.GetCallerForm() != null))
            {
                MessageBox.Show(string.Format(Catalog.GetPluralString(
                            "{0} GL Transaction was automatically updated with the new corporate exchange rate.",
                            "{0} GL Transactions were automatically updated with the new corporate exchange rate.",
                            TransactionsChanged, true), TransactionsChanged),
                    Catalog.GetString("Save Corporate Exchange Rates"), MessageBoxButtons.OK);
            }

            return Result;
        }

        private void PrintGrid(TStandardFormPrint.TPrintUsing APrintApplication, bool APreviewMode)
        {
            TFrmSelectPrintFields.SelectAndPrintGridFields(this, APrintApplication, APreviewMode, TModule.mPartner, this.Text, grdDetails,
                new int[]
                {
                    ACorporateExchangeRateTable.ColumnFromCurrencyCodeId,
                    ACorporateExchangeRateTable.ColumnToCurrencyCodeId,
                    ACorporateExchangeRateTable.ColumnDateEffectiveFromId,
                    ACorporateExchangeRateTable.ColumnRateOfExchangeId
                });
        }
    }
}