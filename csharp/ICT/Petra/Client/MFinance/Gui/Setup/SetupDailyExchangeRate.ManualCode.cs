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
using GNU.Gettext;
using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.IO;
using Ict.Common.Remoting.Shared;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Client.App.Gui;
using System.Collections.Generic;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupDailyExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger;

        private const decimal EXCHANGE_RATE_WARNING_RATIO = 1.15m;

        // Private variables relating to use when we are MODAL
        private bool blnIsInModalMode;
        private bool blnUseDateTimeDefault = false;
        private String strCurrencyToDefault;
        private DateTime dateTimeDefault;

        private decimal modalRateOfExchange = 1.0m;
        private DateTime modalEffectiveDate;
        private int modalEffectiveTime = 0;

        // Filters and sorting
        private string SortByDateDescending =
            ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + ", " +
            ADailyExchangeRateTable.GetToCurrencyCodeDBName() + ", " +
            ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
            ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";

        private string JournalRowFilter = "(" + AJournalTable.GetTransactionCurrencyDBName() + " = '{0}' OR " +
                                          AJournalTable.GetTransactionCurrencyDBName() + " = '{1}') AND " +
                                          AJournalTable.GetDateEffectiveDBName() + " = #{2}# AND " +
                                          AJournalTable.GetExchangeRateTimeDBName() + " = {3} AND " +
                                          AJournalTable.GetJournalStatusDBName() + " = '{4}'";
        private string GiftBatchRowFilter = "(" + AGiftBatchTable.GetCurrencyCodeDBName() + " = '{0}' OR " +
                                            AGiftBatchTable.GetCurrencyCodeDBName() + " = '{1}') AND " +
                                            AGiftBatchTable.GetGlEffectiveDateDBName() + " = #{2}# AND " +
                                            AGiftBatchTable.GetExchangeRateToBaseDBName() + " = {3} AND " +
                                            AGiftBatchTable.GetBatchStatusDBName() + " = 'Unposted'";

        // Used for edit/delete
        bool bCanEditDelete = false;
        ToolTip tooltipDeleteInfo = new ToolTip();

        // Create a RateAlert tooltip
        ToolTip tooltipRateAlert = new ToolTip();

        #region Base Class for Serializable Data

        private class SerialisableDS
        {
            public static bool SaveChanges(TTypedDataTable ATable, TTypedDataTable ATableChanges, string ATableDbName)
            {
                bool ReturnValue = false;
                TSubmitChangesResult SubmissionResult;
                TVerificationResultCollection VerificationResult;

                // Submit changes to the PETRAServer
                try
                {
                    SubmissionResult = TRemote.MCommon.DataReader.WebConnectors.SaveData(ATableDbName, ref ATableChanges, out VerificationResult);
                }
                catch (ESecurityDBTableAccessDeniedException Exp)
                {
                    TMessages.MsgSecurityException(Exp, typeof(SerialisableDS));

                    ReturnValue = false;
                    return ReturnValue;
                }
                catch (EDBConcurrencyException Exp)
                {
                    TMessages.MsgDBConcurrencyException(Exp, typeof(SerialisableDS));

                    ReturnValue = false;
                    return ReturnValue;
                }
                catch (Exception)
                {
                    throw;
                }

                switch (SubmissionResult)
                {
                    case TSubmitChangesResult.scrOK:
                        // Call AcceptChanges to get rid now of any deleted columns before we Merge with the result from the Server
                        ATable.AcceptChanges();

                        // Merge back with data from the Server (eg. for getting Sequence values)
                        ATableChanges.AcceptChanges();
                        ATable.Merge(ATableChanges, false);

                        // need to accept the new modification ID
                        ATable.AcceptChanges();

                        ReturnValue = true;

                        if ((VerificationResult != null)
                            && (VerificationResult.HasCriticalOrNonCriticalErrors))
                        {
                            TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                                typeof(SerialisableDS), null);
                        }

                        break;

                    case TSubmitChangesResult.scrError:
                        TDataValidation.ProcessAnyDataValidationErrors(false, VerificationResult,
                        typeof(SerialisableDS), null);
                        ReturnValue = false;
                        break;

                    case TSubmitChangesResult.scrNothingToBeSaved:

                        ReturnValue = true;
                        break;

                    case TSubmitChangesResult.scrInfoNeeded:

                        // TODO scrInfoNeeded
                        break;
                }

                return ReturnValue;
            }
        }

        #endregion

        #region Additional DataSets for Journal and Gift Batch

        /// <summary>
        /// We need access to all the journal table entries to decide if we can delete an exchange rate entry
        /// </summary>
        private class FJournalDS : SerialisableDS
        {
            /// <summary>
            /// This table holds ALL the journal table data for all ledgers and all batches (could be quite a lot!)
            /// </summary>
            public static AJournalTable JournalTable;
            public static bool HasMatchingUnpostedRate = false;
            public static decimal MatchingRate = 0.0m;
            public static bool NeedsSave = false;

            public static bool SaveChanges()
            {
                TTypedDataTable TableChanges = JournalTable.GetChangesTyped();

                if (TableChanges == null)
                {
                    // There is nothing to be saved.
                    return true;
                }

                return SerialisableDS.SaveChanges(JournalTable, TableChanges, AJournalTable.GetTableDBName());
            }
        }

        /// <summary>
        /// We need access to all the gift batch table entries to decide if we can delete an exchange rate entry
        /// </summary>
        private class FGiftBatchDS : SerialisableDS
        {
            public static AGiftBatchTable GiftBatchTable;
            public static bool HasMatchingUnpostedRate = false;
            public static decimal MatchingRate = 0.0m;
            public static bool NeedsSave = false;

            public static bool SaveChanges()
            {
                TTypedDataTable TableChanges = GiftBatchTable.GetChangesTyped();

                if (TableChanges == null)
                {
                    // There is nothing to be saved.
                    return true;
                }

                return SerialisableDS.SaveChanges(GiftBatchTable, TableChanges, AJournalTable.GetTableDBName());
            }
        }

        #endregion

        /// <summary>
        /// We use this to hold inverse exchange rate items that will need saving at the end
        /// </summary>
        private struct tInverseItem
        {
            public string FromCurrencyCode;
            public string ToCurrencyCode;
            public DateTime DateEffective;
            public int TimeEffective;
            public decimal RateOfExchange;
        }

        /// <summary>
        /// Public property that is not really used in release builds, but might be useful
        /// when developing to set the initial value for the ledger number
        /// </summary>
        public Int32 LedgerNumber
        {
            set
            {
                ALedgerRow ledger =
                    ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                         TCacheableFinanceTablesEnum.LedgerDetails, value))[0];
                baseCurrencyOfLedger = ledger.BaseCurrency;
            }
        }

        private void InitializeManualCode()
        {
            // This code runs just before the auto-generated code binds the data to the grid
            // We need to set the RowFilter to something that returns no rows because we will return the rows we actually want
            // in RunOnceOnActivation.  By returning no rows now we reduce some horrible flicker on the screen (and save time!)
            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = FMainDS.ADailyExchangeRate.ColumnDateEffectiveFrom + " = '" +
                                                               DateTime.MaxValue.ToShortDateString() + "'";

            // Now we set some default settings that apply when the screen is MODELESS
            //  (If the screen will be MODAL one of the ShowDialog methods will be called below)
            btnClose.Visible = false;           // Do not show the modal buttons
            btnCancel.Visible = false;
            btnDelete.Top = btnClose.Top + btnClose.Height / 2;
            mniImport.Enabled = true;           // Allow imports
            tbbImport.Enabled = true;
            blnIsInModalMode = false;

            tooltipDeleteInfo.ToolTipTitle = Catalog.GetString("Usage of this Exchange Rate:");
            tooltipDeleteInfo.ShowAlways = true;
            tooltipDeleteInfo.ToolTipIcon = ToolTipIcon.Info;

            tooltipRateAlert.ToolTipTitle = Catalog.GetString("Exchange rate value alert:");
            tooltipRateAlert.ShowAlways = true;
            tooltipRateAlert.ToolTipIcon = ToolTipIcon.Warning;

            lblEnableEditDelete.BorderStyle = BorderStyle.FixedSingle;
            lblEnableEditDelete.BackColor = Color.LightGreen;
            lblEnableEditDelete.Visible = false;
            lblEnableEditDelete.Top = btnEnableEdit.Top;
            lblEnableEditDelete.Height = btnEnableEdit.Height;
            lblEnableEditDelete.TextAlign = ContentAlignment.MiddleCenter;
        }

        private void RunOnceOnActivationManual()
        {
            // Activate events we will use in manual code
            this.txtDetailRateOfExchange.TextChanged +=
                new EventHandler(txtDetailRateOfExchange_TextChanged);
            this.cmbDetailFromCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);
            this.cmbDetailToCurrencyCode.SelectedValueChanged +=
                new System.EventHandler(this.ValueChangedCurrencyCode);

            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);

            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);

            // Set a non-standard sort order (newest record first)
            DataView theView = FMainDS.ADailyExchangeRate.DefaultView;
            theView.Sort = SortByDateDescending;

            // Set the RowFilter - in MODAL mode it has already been set, but in MODELESS mode we need to set up to see all rows
            if (!blnIsInModalMode)
            {
                theView.RowFilter = "";
            }

            // Having changed the sort order we need to put the correct details in the panel (assuming we have a row to display)
            if (theView.Count > 0)
            {
                SelectRowInGrid(1);
            }
        }

        /// <summary>
        /// Do not use this method signature for OpenPetra
        /// </summary>
        /// <returns>An error</returns>
        public new DialogResult ShowDialog()
        {
            throw new NotSupportedException(
                "You cannot call ShowDialog with empty parameters.  Use one of the method signatures with multiple parameters");
        }

        /// <summary>
        /// Do not use this method signature for OpenPetra
        /// </summary>
        /// <returns>An error</returns>
        public new DialogResult ShowDialog(IWin32Window Parent)
        {
            throw new NotSupportedException(
                "You cannot call ShowDialog with a single parameter.  Use one of the method signatures with multiple parameters");
        }

        /// <summary>
        /// Main method to invoke the dialog in the modal form based on a single effective date.
        /// The table will be filtered by the value of the base currency of the selected ledger,
        /// and the values of the date and foreign currency.
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency will be extracted</param>
        /// <param name="dteEffective">Effective date of the actual acounting process.  The grid will show all entries on or before this date.</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <param name="ExchangeDefault">Default value for the exchange rate</param>
        /// <param name="SelectedExchangeRate">The selected value for the exchange rate</param>
        /// <param name="SelectedEffectiveDate">The selected value for the effective date</param>
        /// <param name="SelectedEffectiveTime">The selected value for the effective time</param>
        public DialogResult ShowDialog(Int32 LedgerNumber, DateTime dteEffective,
            string strCurrencyTo,
            decimal ExchangeDefault,
            out decimal SelectedExchangeRate,
            out DateTime SelectedEffectiveDate,
            out int SelectedEffectiveTime)
        {
            // We can just call our alternate method, setting the start date to the beginning of time!
            return ShowDialog(LedgerNumber,
                DateTime.MinValue,
                dteEffective,
                strCurrencyTo,
                ExchangeDefault,
                out SelectedExchangeRate,
                out SelectedEffectiveDate,
                out SelectedEffectiveTime);
        }

        /// <summary>
        /// Main method to invoke the dialog in the modal form based on a date range.
        /// The table will be filtered by the value of the base currency of the selected ledger,
        /// and the values of the date range and foreign currency.
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency will be extracted</param>
        /// <param name="dteStart">The start date for the date range</param>
        /// <param name="dteEnd">The end date for the date range.  The grid will show all entries between the start and end dates.</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <param name="ExchangeDefault">Default value for the exchange rate</param>
        /// <param name="SelectedExchangeRate">The selected value for the exchange rate</param>
        /// <param name="SelectedEffectiveDate">The selected value for the effective date</param>
        /// <param name="SelectedEffectiveTime">The selected value for the effective time</param>
        public DialogResult ShowDialog(Int32 LedgerNumber, DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyTo,
            decimal ExchangeDefault,
            out decimal SelectedExchangeRate,
            out DateTime SelectedEffectiveDate,
            out int SelectedEffectiveTime)
        {
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, LedgerNumber))[0];

            baseCurrencyOfLedger = ledger.BaseCurrency;

            DateTime dateEnd2 = dteEnd.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string strDteStart = dteStart.ToString("d", dateTimeFormat);
            string strDteEnd = dateEnd2.ToString("d", dateTimeFormat);

            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + baseCurrencyOfLedger + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + strCurrencyTo + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < #" + strDteEnd + "#";

            if (dteStart > DateTime.MinValue)
            {
                filter += (" and " + ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " > #" + strDteStart + "#");
            }

            DataView myDataView = FMainDS.ADailyExchangeRate.DefaultView;
            myDataView.RowFilter = filter;
            myDataView.Sort = SortByDateDescending;

            modalRateOfExchange = ExchangeDefault;
            strCurrencyToDefault = strCurrencyTo;
            dateTimeDefault = dteEnd;

            DefineModalSettings();

            DialogResult dlgResult = base.ShowDialog();

            SelectedExchangeRate = modalRateOfExchange;
            SelectedEffectiveDate = modalEffectiveDate;
            SelectedEffectiveTime = modalEffectiveTime;

            return dlgResult;
        }

        /// <summary>
        /// Get the most recent exchange rate value of the interval.  This method does not display a dialog
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency will be extracted</param>
        /// <param name="dteStart">The start date for the date range</param>
        /// <param name="dteEnd">The end date for the date range.</param>
        /// <param name="strCurrencyTo">The actual foreign currency value</param>
        /// <returns>The most recent exchange rate in the specified date range</returns>
        public decimal GetLastExchangeValueOfInterval(Int32 LedgerNumber, DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyTo)
        {
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, LedgerNumber))[0];

            baseCurrencyOfLedger = ledger.BaseCurrency;

            DateTime dateEnd2 = dteEnd.AddDays(1.0);
            // Do not use local formats here!
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            string strDteStart = dteStart.ToString("d", dateTimeFormat);
            string strDteEnd = dateEnd2.ToString("d", dateTimeFormat);

            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + baseCurrencyOfLedger + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + strCurrencyTo + "' and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " < #" + strDteEnd + "# and " +
                ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " > #" + strDteStart + "#";
            DataView myView = new DataView(FMainDS.ADailyExchangeRate, filter, SortByDateDescending, DataViewRowState.CurrentRows);

            if (myView.Count > 0)
            {
                return ((ADailyExchangeRateRow)(myView[0].Row)).RateOfExchange;
            }
            else
            {
                return 1.0m;
            }
        }

        private void DefineModalSettings()
        {
            blnUseDateTimeDefault = true;

            // We need the accept/cancel buttons when MODAL.  It looks better if they are at the top and New is beneath
            btnClose.Visible = true;
            btnCancel.Visible = true;
            int pos1 = btnNew.Top;
            int pos2 = btnClose.Top;
            int spacing = pos2 - pos1;
            btnClose.Top = pos1;
            btnCancel.Top = pos2;
            btnNew.Top = btnCancel.Top + spacing + (btnNew.Height / 2);
            btnDelete.Top = btnNew.Top + spacing;

            // Import not allowed when MODAL
            mniImport.Enabled = false;
            tbbImport.Enabled = false;

            // Different Dialog Title text - and set the buttons
            this.Text = "Select an Exchange Rate";
            this.AcceptButton = btnClose;
            this.CancelButton = btnCancel;

            blnIsInModalMode = true;
            DialogResult = DialogResult.Cancel;     // assume it is cancelled for now
        }

        /// <summary>
        /// Called in MODAL mode when the user clicks the Accept/Close button ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void CloseDialog(object sender, EventArgs e)
        {
            // If there have been changes we save them without asking, since that is part of the deal of clicking OK
            if (FPetraUtilsObject.HasChanges && !SaveChanges())
            {
                return;
            }

            blnUseDateTimeDefault = false;

            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                modalRateOfExchange = txtDetailRateOfExchange.NumberValueDecimal.Value;
            }

            modalEffectiveDate = FPreviouslySelectedDetailRow.DateEffectiveFrom;
            modalEffectiveTime = FPreviouslySelectedDetailRow.TimeEffectiveFrom;

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Called in MODAL mode when the user clicks the Cancel button ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void CancelDialog(object sender, EventArgs e)
        {
            // Although the user has clicked Cancel, we need to ask if we need to save any changes that have been made
            if (FPetraUtilsObject.CloseFormCheck())
            {
                blnUseDateTimeDefault = false;
                Close();
            }
        }

        /// <summary>
        /// Create a new DailyExchangeRateRow ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void NewRow(System.Object sender, EventArgs e)
        {
            CreateNewADailyExchangeRate();
        }

        private void NewRowManual(ref ADailyExchangeRateRow ARow)
        {
            // Now create an appropriate new record
            // Start with the effective date
            if (blnUseDateTimeDefault)
            {
                ARow.DateEffectiveFrom = DateTime.Parse(dateTimeDefault.ToLongDateString());
            }
            else
            {
                //For Corpoate Exchange Rate must be 1st of the month, for Daily Exchange Rate it must be now
                ARow.DateEffectiveFrom = DateTime.Parse(DateTime.Now.ToLongDateString());
            }

            // FromCurrency code
            if (baseCurrencyOfLedger == null)
            {
                ARow.FromCurrencyCode = "USD";
            }
            else
            {
                ARow.FromCurrencyCode = baseCurrencyOfLedger;
            }

            // ToCurrency code
            if (strCurrencyToDefault == null)
            {
                if (FPreviouslySelectedDetailRow == null)
                {
                    // No default specified and no highlighted row
                    if (baseCurrencyOfLedger == null)
                    {
                        if (ARow.FromCurrencyCode == "USD")
                        {
                            ARow.ToCurrencyCode = "GBP";
                        }
                        else
                        {
                            ARow.ToCurrencyCode = "USD";
                        }
                    }
                    else
                    {
                        ARow.ToCurrencyCode = baseCurrencyOfLedger;
                    }

                    ARow.RateOfExchange = 1.0m;
                }
                else
                {
                    // No default specified - we will assume it is the same as the highlighted row
                    ARow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
                    ARow.RateOfExchange = txtDetailRateOfExchange.NumberValueDecimal.Value;
                }
            }
            else
            {
                // Use the spcified default currencyTo
                ARow.ToCurrencyCode = strCurrencyToDefault;
                ARow.RateOfExchange = 1.0m;
            }

            // Now do the effective time.  We default to 2am but if there is a row selected we use the time from there
            // I don't think that the current time is very useful, since the idea is that this rate should apply to the day as a whole
            // Use 2am so that if runs are done on a schedule during the night the new rate does not kick in too soon
            int tryEffectiveTime = 7200;

            if (FPreviouslySelectedDetailRow != null)
            {
                tryEffectiveTime = FPreviouslySelectedDetailRow.TimeEffectiveFrom;
            }

            // Ensure we don't create a duplicate record
            while (FMainDS.ADailyExchangeRate.Rows.Find(new object[] {
                           ARow.FromCurrencyCode, ARow.ToCurrencyCode,
                           ARow.DateEffectiveFrom.ToString(), tryEffectiveTime.ToString()
                       }) != null)
            {
                tryEffectiveTime = tryEffectiveTime + 600;              // 10 minute increments

                if (tryEffectiveTime >= 86400)
                {
                    tryEffectiveTime = 60;                              // Do not pass midnight!
                }
            }

            ARow.TimeEffectiveFrom = tryEffectiveTime;
        }

        /// <summary>
        /// EnableEditDelete button has been clicked ...
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void EnableEditDelete_Clicked(System.Object sender, EventArgs e)
        {
            btnEnableEdit.Visible = false;
            lblEnableEditDelete.Visible = true;
            bCanEditDelete = true;
            grdDetails.Focus();

            FJournalDS.JournalTable = new AJournalTable();
            Ict.Common.Data.TTypedDataTable TypedJournalTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(AJournalTable.GetTableDBName(), null, out TypedJournalTable);
            FJournalDS.JournalTable.Merge(TypedJournalTable);

            FGiftBatchDS.GiftBatchTable = new AGiftBatchTable();
            Ict.Common.Data.TTypedDataTable TypedGiftBatchTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(AGiftBatchTable.GetTableDBName(), null, out TypedGiftBatchTable);
            FGiftBatchDS.GiftBatchTable.Merge(TypedGiftBatchTable);

            SetEnabledStates();
        }

        private void DeleteRecord(Object sender, EventArgs e)
        {
            DeleteADailyExchangeRate();
        }

        /// <summary>
        /// Updates the lblValueOneDirection and lblValueOtherDirection labels
        /// </summary>
        private void UpdateExchangeRateLabels()
        {
            // Call can cope with null for Row, but rate must have a valid value
            if (txtDetailRateOfExchange.NumberValueDecimal.HasValue)
            {
                TSetupExchangeRates.SetExchangeRateLabels(cmbDetailFromCurrencyCode.GetSelectedString(),
                    cmbDetailToCurrencyCode.GetSelectedString(), FPreviouslySelectedDetailRow,
                    txtDetailRateOfExchange.NumberValueDecimal.Value, lblValueOneDirection, lblValueOtherDirection);
            }
            else
            {
                TSetupExchangeRates.SetExchangeRateLabels(String.Empty, String.Empty, null, 1.0m, lblValueOneDirection, lblValueOtherDirection);
            }
        }

        /// <summary>
        /// ValueChanged Event for the currency boxes
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void ValueChangedCurrencyCode(System.Object sender, EventArgs e)
        {
            SetEnabledStates();
        }

        /// <summary>
        /// This routines supports a small gui-calculator. The user can easily calculate the
        /// reciprocal value of the exchange rate.
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void InvertExchangeRate(System.Object sender, EventArgs e)
        {
            decimal? exchangeRate;

            try
            {
                exchangeRate = txtDetailRateOfExchange.NumberValueDecimal;
                exchangeRate = Math.Round(1 / exchangeRate.Value, 10);
                txtDetailRateOfExchange.NumberValueDecimal = exchangeRate;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ADailyExchangeRateRow ARow)
        {
            if (ARow == null)
            {
                txtDetailRateOfExchange.NumberValueDecimal = null;
                UpdateExchangeRateLabels();
            }

            // Sadly this may get called twice, if the currencies are different, but if they were the same we need to be sure to call it once at least
            SetEnabledStates();
        }

        private void SetEnabledStates()
        {
            // Set the Enabled state of the two combo boxes
            ADailyExchangeRateRow row = FPreviouslySelectedDetailRow;

            btnClose.Enabled = (row != null);

            if (row == null)
            {
                return;
            }

            // Enable or disable the combo boxes
            bool bEnable = (row.RowState == DataRowState.Added && !blnIsInModalMode);
            cmbDetailFromCurrencyCode.Enabled = bEnable;
            cmbDetailToCurrencyCode.Enabled = bEnable;

            // Set the Enabled states of txtRateOfExchange and the Invert and Delete buttons
            if (cmbDetailFromCurrencyCode.GetSelectedString() ==
                cmbDetailToCurrencyCode.GetSelectedString())
            {
                // Both currencies the same
                txtDetailRateOfExchange.NumberValueDecimal = 1.0m;
                txtDetailRateOfExchange.Enabled = false;
                btnInvertExchangeRate.Enabled = false;
                btnDelete.Enabled = true;
            }
            else
            {
                // Currencies differ
                bool bCanEdit;
                bool bCanDelete;
                CheckIfRateHasBeenUsed(
                    row.FromCurrencyCode,
                    row.ToCurrencyCode,
                    dtpDetailDateEffectiveFrom.Text,
                    row.TimeEffectiveFrom,
                    row.RateOfExchange,
                    out bCanEdit,
                    out bCanDelete);
                txtDetailRateOfExchange.Enabled = (row.RowState == DataRowState.Added || bCanEdit);
                btnDelete.Enabled = (row.RowState == DataRowState.Added || bCanDelete);
                btnInvertExchangeRate.Enabled = true;
            }

            UpdateExchangeRateLabels();
        }

        private void txtDetailRateOfExchange_TextChanged(object sender, EventArgs e)
        {
            if (txtDetailRateOfExchange.Text.Trim() != String.Empty)
            {
                UpdateExchangeRateLabels();
            }
        }

        private void GetDetailDataFromControlsManual(ADailyExchangeRateRow ARow)
        {
            // We are going to check if the rate of exchange is sensible.  We need our own view because we don't know how the grid is currently sorted
            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + ARow.FromCurrencyCode + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + ARow.ToCurrencyCode + "'";
            DataView myView = new DataView(FMainDS.ADailyExchangeRate, filter, SortByDateDescending, DataViewRowState.CurrentRows);

            // Find our current row
            int nThis = myView.Find(new object[] { ARow.FromCurrencyCode, ARow.ToCurrencyCode, ARow.DateEffectiveFrom, ARow.TimeEffectiveFrom });
            ADailyExchangeRateRow drThis = null;
            ADailyExchangeRateRow drPrev = null;
            ADailyExchangeRateRow drNext = null;
            decimal ratio = 1.0m;
            string tipText = String.Empty;

            if (nThis >= 0)
            {
                drThis = (ADailyExchangeRateRow)(myView[nThis]).Row;

                if (nThis >= 1)
                {
                    drPrev = (ADailyExchangeRateRow)(myView[nThis - 1]).Row;
                }

                if (nThis < myView.Count - 1)
                {
                    drNext = (ADailyExchangeRateRow)(myView[nThis + 1]).Row;
                }

                if (drPrev != null)
                {
                    ratio = drThis.RateOfExchange / drPrev.RateOfExchange;

                    if (ratio < 1.0m)
                    {
                        ratio = drPrev.RateOfExchange / drThis.RateOfExchange;
                    }
                }

                if (drNext != null)
                {
                    decimal tryRatio = drThis.RateOfExchange / drNext.RateOfExchange;

                    if (tryRatio < 1.0m)
                    {
                        tryRatio = drNext.RateOfExchange / drThis.RateOfExchange;
                    }

                    if (tryRatio > ratio)
                    {
                        ratio = tryRatio;
                    }
                }

                if (ratio > EXCHANGE_RATE_WARNING_RATIO)
                {
                    tipText = String.Format(
                        Catalog.GetString(
                            "The rate you have entered for {0}->{1} on {2} at {3} differs from the previous or next rate for the same currencies by more than {0:0%}."),
                        ARow.FromCurrencyCode,
                        ARow.ToCurrencyCode,
                        dtpDetailDateEffectiveFrom.Text,
                        txtDetailTimeEffectiveFrom.Text,
                        ratio - 1.0m);
                }
            }

            if (tipText == String.Empty)
            {
                tooltipRateAlert.Hide(txtDetailRateOfExchange);
            }
            else
            {
                tooltipRateAlert.Show(tipText, txtDetailRateOfExchange, 6000);
            }

            // Check if the rate was changed - if it was, do we need to save to an external table??
            if (!txtDetailRateOfExchange.Enabled)
            {
                return;
            }

            if (ARow.RowState == DataRowState.Added)
            {
                return;
            }

            // OK, so we might have changed the rate
            if (FJournalDS.MatchingRate != ARow.RateOfExchange)
            {
                // change all the relevant entries
                DataView dvJournal = FJournalDS.JournalTable.DefaultView;
                dvJournal.RowFilter =
                    String.Format(JournalRowFilter, ARow.FromCurrencyCode, ARow.ToCurrencyCode, ARow.DateEffectiveFrom.ToString(
                            "yyyy-MMM-dd"), ARow.RateOfExchange, "Unposted");

                for (int i = 0; i < dvJournal.Count; i++)
                {
                    AJournalRow row = ((AJournalRow)dvJournal[i].Row);
                    row.BeginEdit();
                    row.ExchangeRateToBase = ARow.RateOfExchange;
                    row.EndEdit();
                }

                FJournalDS.MatchingRate = ARow.RateOfExchange;
                FJournalDS.NeedsSave = true;
            }

            if (FGiftBatchDS.MatchingRate != ARow.RateOfExchange)
            {
                // change all the relevant entries
                DataView dvGift = FGiftBatchDS.GiftBatchTable.DefaultView;
                dvGift.RowFilter =
                    String.Format(GiftBatchRowFilter, ARow.FromCurrencyCode, ARow.ToCurrencyCode, ARow.DateEffectiveFrom.ToString(
                            "yyyy-MMM-dd"), ARow.RateOfExchange);

                for (int i = 0; i < dvGift.Count; i++)
                {
                    AGiftBatchRow row = ((AGiftBatchRow)dvGift[i].Row);
                    row.BeginEdit();
                    row.ExchangeRateToBase = ARow.RateOfExchange;
                    row.EndEdit();
                }

                FGiftBatchDS.MatchingRate = ARow.RateOfExchange;
                FGiftBatchDS.NeedsSave = true;
            }

            // Finally check if we have an inverse rate for this date/time and currency pair
            ADailyExchangeRateRow mainRow = (ADailyExchangeRateRow)FMainDS.ADailyExchangeRate.Rows.Find(
                new object[] { ARow.ToCurrencyCode, ARow.FromCurrencyCode, ARow.DateEffectiveFrom, ARow.TimeEffectiveFrom });

            if (mainRow != null)
            {
                decimal inverseRate = Math.Round(1 / ARow.RateOfExchange, 10);
                decimal difference = Math.Abs(mainRow.RateOfExchange - inverseRate);

                if (difference > 0.0000000001m)
                {
                    // update this too
                    mainRow.BeginEdit();
                    mainRow.RateOfExchange = inverseRate;
                    mainRow.EndEdit();
                }
            }
        }

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ADailyExchangeRate, "Daily");
            FPetraUtilsObject.SetChangedFlag();
        }

        private void ValidateDataDetailsManual(ADailyExchangeRateRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GLSetup.ValidateDailyExchangeRate(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict);
        }

        private void CheckIfRateHasBeenUsed(string FromCurrency,
            string ToCurrency,
            string FromDate,
            int FromTime,
            decimal rate,
            out bool CanEdit,
            out bool CanDelete)
        {
            if (rate == 0.0m)
            {
                CanEdit = true;
                CanDelete = true;
                FJournalDS.HasMatchingUnpostedRate = false;
                FGiftBatchDS.HasMatchingUnpostedRate = false;
                tooltipDeleteInfo.Hide(txtDetailRateOfExchange);
                return;
            }

            if (!bCanEditDelete)
            {
                CanEdit = false;
                CanDelete = false;
                return;       // Assume it has been used
            }

            // Display a tooltip with our findings
            string tipText = String.Empty;
            // Set up a row filter on the two tables
            DataView dvGift = FGiftBatchDS.GiftBatchTable.DefaultView;
            dvGift.RowFilter = String.Format(GiftBatchRowFilter, FromCurrency, ToCurrency, FromDate, rate);
            FGiftBatchDS.HasMatchingUnpostedRate = (dvGift.Count > 0);

            if (FGiftBatchDS.HasMatchingUnpostedRate)
            {
                FGiftBatchDS.MatchingRate = rate;
                tipText += String.Format(Catalog.GetString("Used by {0} row(s) in the Gift table"), dvGift.Count);
            }

            DataView dvJournal = FJournalDS.JournalTable.DefaultView;
            dvJournal.RowFilter = String.Format(JournalRowFilter, FromCurrency, ToCurrency, FromDate, FromTime, "Unposted");
            FJournalDS.HasMatchingUnpostedRate = (dvJournal.Count > 0);

            if (FJournalDS.HasMatchingUnpostedRate)
            {
                FJournalDS.MatchingRate = rate;

                if (tipText != String.Empty)
                {
                    tipText += Environment.NewLine;
                }

                if (dvJournal.Count == 1)
                {
                    tipText +=
                        String.Format(Catalog.GetString("Used by 1 unposted row in the Journal table dated {0}"),
                            ((DateTime)dvJournal[0][AJournalTable.ColumnDateOfEntryId]).ToString("dd-MMM-yyyy").ToUpper());
                }
                else
                {
                    tipText += String.Format(Catalog.GetString("Used by {0} unposted rows in the Journal table between {1} and {2}"),
                        dvJournal.Count,
                        ((DateTime)dvJournal[dvJournal.Count - 1][AJournalTable.ColumnDateOfEntryId]).ToString("dd-MMM-yyyy").ToUpper(),
                        ((DateTime)dvJournal[0][AJournalTable.ColumnDateOfEntryId]).ToString("dd-MMM-yyyy").ToUpper());
                }
            }

            dvJournal.RowFilter = String.Format(JournalRowFilter, FromCurrency, ToCurrency, FromDate, FromTime, "Posted");
            bool bHasPostedJournalEntries = (dvJournal.Count > 0);

            if (bHasPostedJournalEntries)
            {
                if (tipText != String.Empty)
                {
                    tipText += Environment.NewLine;
                }

                if (dvJournal.Count == 1)
                {
                    tipText +=
                        String.Format(Catalog.GetString("Used by 1 posted row in the Journal table dated {0}"),
                            ((DateTime)dvJournal[0][AJournalTable.ColumnDateOfEntryId]).ToString("dd-MMM-yyyy").ToUpper());
                }
                else
                {
                    tipText += String.Format(Catalog.GetString("Used by {0} posted rows in the Journal table between {1} and {2}"),
                        dvJournal.Count,
                        ((DateTime)dvJournal[dvJournal.Count - 1][AJournalTable.ColumnDateOfEntryId]).ToString("dd-MMM-yyyy").ToUpper(),
                        ((DateTime)dvJournal[0][AJournalTable.ColumnDateOfEntryId]).ToString("dd-MMM-yyyy").ToUpper());
                }
            }

            if (tipText == String.Empty)
            {
                tooltipDeleteInfo.Hide(btnInvertExchangeRate);
            }
            else
            {
                tooltipDeleteInfo.Show(tipText, btnInvertExchangeRate, 6000);
            }

            // return true if the rate has been used
            CanEdit = !bHasPostedJournalEntries;
            CanDelete = !FJournalDS.HasMatchingUnpostedRate && !FGiftBatchDS.HasMatchingUnpostedRate && !bHasPostedJournalEntries;
            return;
        }

        void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
        {
            // The user has clicked Save.  We need to consider if we need to make any Inverse currency additions...
            GetDetailsFromControls(FPreviouslySelectedDetailRow);

            // Now go through all the grid rows (view) checking all the added rows.  Keep a list of inverses
            List <tInverseItem>lstInverses = new List <tInverseItem>();
            DataView gridView = ((DevAge.ComponentModel.BoundDataView)grdDetails.DataSource).DataView;

            for (int i = 0; i < gridView.Count; i++)
            {
                ADailyExchangeRateRow ARow = (ADailyExchangeRateRow)gridView[i].Row;

                if (ARow.RowState == DataRowState.Added)
                {
                    tInverseItem item = new tInverseItem();
                    item.FromCurrencyCode = ARow.ToCurrencyCode;
                    item.ToCurrencyCode = ARow.FromCurrencyCode;
                    item.RateOfExchange = Math.Round(1 / ARow.RateOfExchange, 10);
                    item.DateEffective = ARow.DateEffectiveFrom;
                    item.TimeEffective = ARow.TimeEffectiveFrom;
                    lstInverses.Add(item);
                }
            }

            if (lstInverses.Count == 0)
            {
                return;
            }

            // Now go through our list and check if any items need adding to the data Table
            // The user may already have put an inverse currency in by hand
            DateTimeFormatInfo dateTimeFormat =
                new System.Globalization.CultureInfo(String.Empty, false).DateTimeFormat;
            DataView dv = new DataView(FMainDS.ADailyExchangeRate);

            for (int i = 0; i < lstInverses.Count; i++)
            {
                tInverseItem item = lstInverses[i];

                // Does the item exist already?
                dv.RowFilter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=#{5}# AND {6}={7} AND {8}={9}",
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                    item.FromCurrencyCode,
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    item.ToCurrencyCode,
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                    item.DateEffective.ToString("d", dateTimeFormat),
                    ADailyExchangeRateTable.GetTimeEffectiveFromDBName(),
                    item.TimeEffective,
                    ADailyExchangeRateTable.GetRateOfExchangeDBName(),
                    item.RateOfExchange);

                if (dv.Count == 0)
                {
                    ADailyExchangeRateRow NewRow = FMainDS.ADailyExchangeRate.NewRowTyped();
                    NewRow.FromCurrencyCode = item.FromCurrencyCode;;
                    NewRow.ToCurrencyCode = item.ToCurrencyCode;
                    NewRow.DateEffectiveFrom = DateTime.Parse(item.DateEffective.ToLongDateString());
                    NewRow.TimeEffectiveFrom = item.TimeEffective;
                    NewRow.RateOfExchange = item.RateOfExchange;

                    FMainDS.ADailyExchangeRate.Rows.Add(NewRow);
                }
            }

            // Now make sure to select the row that was currently selected when we started the Save operation
            SelectRowInGrid(grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1);
        }

        void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
        {
            // Just quit if we didn't save our stuff
            if (!e.Success)
            {
                return;
            }

            // Finally, save any changes to the extra data sets
            if (FJournalDS.NeedsSave && FJournalDS.SaveChanges())
            {
                FJournalDS.NeedsSave = false;
            }

            if (FGiftBatchDS.NeedsSave && FGiftBatchDS.SaveChanges())
            {
                FGiftBatchDS.NeedsSave = false;
            }
        }
    }
}