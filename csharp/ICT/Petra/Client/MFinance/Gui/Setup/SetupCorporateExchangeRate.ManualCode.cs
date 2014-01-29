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
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Common;
using Ict.Common.IO;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Gui;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupCorporateExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger = null;

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

                mniImport.Enabled = true;
                tbbImport.Enabled = true;
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

            if (baseCurrencyOfLedger == null)
            {
                // Have a last attempt at deciding what the base currency is...
                // What ledgers does the user have access to??
                ALedgerTable ledgers = TRemote.MFinance.Setup.WebConnectors.GetAvailableLedgers();
                DataView ledgerView = ledgers.DefaultView;
                ledgerView.RowFilter = "a_ledger_status_l = 1";     // Only view 'in use' ledgers

                if (ledgerView.Count > 0)
                {
                    // There is at least one - so default to the currency of the first one
                    baseCurrencyOfLedger = ((ALedgerRow)ledgerView.Table.Rows[0]).BaseCurrency;
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
            ValidateAllData(false, false);

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
                dv.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4}=#{5}# AND {6}={7}",
                    ACorporateExchangeRateTable.GetFromCurrencyCodeDBName(),
                    item.FromCurrencyCode,
                    ACorporateExchangeRateTable.GetToCurrencyCodeDBName(),
                    item.ToCurrencyCode,
                    ACorporateExchangeRateTable.GetDateEffectiveFromDBName(),
                    item.DateEffective.ToString("d", CultureInfo.InvariantCulture),
                    ACorporateExchangeRateTable.GetRateOfExchangeDBName(),
                    item.RateOfExchange);

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
                FPetraUtilsObject.ValidationControlsDict);

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
                if (baseCurrencyOfLedger == null)
                {
                    ARow.FromCurrencyCode = "GBP";
                    ARow.ToCurrencyCode = "USD";
                }
                else
                {
                    if (baseCurrencyOfLedger == "USD")
                    {
                        ARow.FromCurrencyCode = "GBP";
                    }
                    else
                    {
                        ARow.FromCurrencyCode = "USD";
                    }

                    ARow.ToCurrencyCode = baseCurrencyOfLedger;
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
                    txtDetailRateOfExchange.NumberValueDecimal.Value, lblValueOneDirection, lblValueOtherDirection);
            }
            else
            {
                TSetupExchangeRates.SetExchangeRateLabels(String.Empty, String.Empty, null, 1.0m, lblValueOneDirection, lblValueOtherDirection);
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

            FMainDS.ACorporateExchangeRate.DefaultView.RowFilter = rowFilter;
            SelectRowInGrid(grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1);

            SetEnabledStates(FPreviouslySelectedDetailRow.RowState == DataRowState.Added);
        }

        private void GetDetailDataFromControlsManual(ACorporateExchangeRateRow ARow)
        {
            // Check if we have an inverse rate for this date/time and currency pair
            ACorporateExchangeRateRow mainRow = (ACorporateExchangeRateRow)FMainDS.ACorporateExchangeRate.Rows.Find(
                new object[] { ARow.ToCurrencyCode, ARow.FromCurrencyCode, ARow.DateEffectiveFrom });

            if (mainRow != null)
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
            if (ValidateAllData(true, true))
            {
                TVerificationResultCollection results = FPetraUtilsObject.VerificationResultCollection;

                TImportExchangeRates.ImportCurrencyExRates(FMainDS.ACorporateExchangeRate, "Corporate", results);

                if (results.Count > 0)
                {
                    TDataValidation.ProcessAnyDataValidationErrors(true, results, this.GetType(), null);
                }

                FPetraUtilsObject.SetChangedFlag();
                results.Clear();
            }
        }

        private void SetEnabledStates(bool RowCanBeChanged)
        {
            bool bEnable = (RowCanBeChanged && cmbDetailFromCurrencyCode.GetSelectedString() != cmbDetailToCurrencyCode.GetSelectedString());

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
    }
}