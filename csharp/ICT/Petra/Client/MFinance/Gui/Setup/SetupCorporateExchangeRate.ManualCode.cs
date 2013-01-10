//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
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
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance.Validation;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupCorporateExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger;

        private bool blnSelectedRowChangeable = false;

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
            this.txtDetailRateOfExchange.TextChanged +=
                new System.EventHandler(this.txtDetailRateOfExchange_TextChanged);

            this.cmbDetailFromCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);
            this.cmbDetailToCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);

            //this.tbbSave.Click +=
            //    new System.EventHandler(this.SetTheFocusToTheGrid);

            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);

            FMainDS.ACorporateExchangeRate.DefaultView.Sort = ACorporateExchangeRateTable.GetToCurrencyCodeDBName() + ", " +
                    ACorporateExchangeRateTable.GetFromCurrencyCodeDBName() + ", " +
                    ACorporateExchangeRateTable.GetDateEffectiveFromDBName() + " DESC";
            FMainDS.ACorporateExchangeRate.DefaultView.RowFilter = "";
            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
        }

        void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            // The user has clicked Save.  We need to consider if we need to make any Inverse currency additions...
            // We need to update the details and validate them first
            // When we return from this method the standard code will do the validation again and might not allow the save to go ahead
            FPetraUtilsObject.VerificationResultCollection.Clear();
            ValidateAllData(false, false);
            if (FPetraUtilsObject.VerificationResultCollection.HasCriticalErrors)
            {
                return;
            }

            // Now go through all the grid rows (view) checking all the added rows.  Keep a list of inverses
            List<tInverseItem> lstInverses = new List<tInverseItem>();
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
                    item.DateEffective,
                    ACorporateExchangeRateTable.GetRateOfExchangeDBName(),
                    item.RateOfExchange);

                if (dv.Count == 0)
                {
                    ACorporateExchangeRateRow NewRow = FMainDS.ACorporateExchangeRate.NewRowTyped();
                    NewRow.FromCurrencyCode = item.FromCurrencyCode; ;
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
            DateTime NewDateEffectiveFrom;

            // Calculate the Date from which the Exchange Rate will be effective. It needs to be preset to the first day of the current month.
            NewDateEffectiveFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            if (FPreviouslySelectedDetailRow == null)
            {
                // Corporate Exchange rates are not part of any ledger, so baseCurrencyOfLedger may be null...
                if (baseCurrencyOfLedger != null)
                {
                    ARow.FromCurrencyCode = "USD";
                    ARow.ToCurrencyCode = baseCurrencyOfLedger;
                }
                else
                {
                    ARow.FromCurrencyCode = "GBP";
                    ARow.ToCurrencyCode = "USD";
                }

                if (ARow.FromCurrencyCode == ARow.ToCurrencyCode)
                {
                    ARow.RateOfExchange = 1.0m;
                }
                else
                {
                    ARow.RateOfExchange = 0.0m;
                }
            }
            else
            {
                // Use the same settings as the highlighted row
                ARow.FromCurrencyCode = cmbDetailFromCurrencyCode.GetSelectedString();
                ARow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();

                // Get the most recent value for this currency pair
                string rowFilter = String.Format("{0}='{1}' AND {2}='{3}'", 
                        ACorporateExchangeRateTable.GetFromCurrencyCodeDBName(),
                        ARow.FromCurrencyCode,
                        ACorporateExchangeRateTable.GetToCurrencyCodeDBName(),
                        ARow.ToCurrencyCode);
                string sortBy = String.Format("{0} DESC", ACorporateExchangeRateTable.GetDateEffectiveFromDBName());
                DataView dv = new DataView(FMainDS.ACorporateExchangeRate, rowFilter, sortBy, DataViewRowState.CurrentRows);
                ARow.RateOfExchange = ((ACorporateExchangeRateRow)dv[0].Row).RateOfExchange;
            }

            // Ensure we don't create a duplicate record
            while (FMainDS.ACorporateExchangeRate.Rows.Find(new object[] {
                           ARow.FromCurrencyCode, ARow.ToCurrencyCode, NewDateEffectiveFrom.ToString()
                       }) != null)
            {
                NewDateEffectiveFrom = NewDateEffectiveFrom.AddMonths(1);
            }

            ARow.DateEffectiveFrom = NewDateEffectiveFrom;
            ARow.TimeEffectiveFrom = 0;
        }

        /// <summary>
        /// Validated Event for txtDetailRateOfExchange
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
        private void UpdateExchangeRateLabels()
        {
            TSetupExchangeRates.SetExchangeRateLabels(cmbDetailFromCurrencyCode.GetSelectedString(),
                cmbDetailToCurrencyCode.GetSelectedString(), GetSelectedDetailRow(),
                txtDetailRateOfExchange.NumberValueDecimal.Value, lblValueOneDirection, lblValueOtherDirection);
        }

        /// <summary>
        /// ValueChanged Event for the currency boxes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValueChangedCurrencyCode(System.Object sender, EventArgs e)
        {
            ValueChangedCurrencyCode();
        }

        /// <summary>
        /// Main routine for the ValueChanged Event of the currency boxes
        /// </summary>
        private void ValueChangedCurrencyCode()
        {
            if (cmbDetailFromCurrencyCode.GetSelectedString() ==
                cmbDetailToCurrencyCode.GetSelectedString())
            {
                txtDetailRateOfExchange.NumberValueDecimal = 1.0m;
                UpdateExchangeRateLabels();
                txtDetailRateOfExchange.Enabled = false;
                btnInvertExchangeRate.Enabled = false;
            }
            else
            {
                if (blnSelectedRowChangeable)
                {
                    txtDetailRateOfExchange.Enabled = true;
                    btnInvertExchangeRate.Enabled = true;
                }
            }

            if (blnSelectedRowChangeable)
            {
                cmbDetailToCurrencyCode.Enabled = true;
                cmbDetailFromCurrencyCode.Enabled = true;
            }

            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                UpdateExchangeRateLabels();
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
            decimal? exchangeRate;

            try
            {
                exchangeRate = txtDetailRateOfExchange.NumberValueDecimal;
                exchangeRate = 1 / exchangeRate;
                exchangeRate = Math.Round(exchangeRate.Value, 10);
                txtDetailRateOfExchange.NumberValueDecimal = exchangeRate;
            }
            catch (Exception)
            {
            }

            UpdateExchangeRateLabels();
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

                if (mainRow.RateOfExchange != inverseRate && ARow.RateOfExchange != inverseRateAlt)
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
                blnSelectedRowChangeable = !(ARow.RowState == DataRowState.Unchanged);
                UpdateExchangeRateLabels();
                txtDetailRateOfExchange.Enabled = true;
                btnInvertExchangeRate.Enabled = (ARow.RowState == DataRowState.Added);
                blnSelectedRowChangeable = (ARow.RowState == DataRowState.Added);
                ValueChangedCurrencyCode();
            }
            else
            {
                blnSelectedRowChangeable = false;
                txtDetailRateOfExchange.Enabled = false;
                txtDetailRateOfExchange.NumberValueDecimal = null;
            }
        }

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ACorporateExchangeRate, "Corporate");
            FPetraUtilsObject.SetChangedFlag();
        }
    }
}