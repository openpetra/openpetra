//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop, alanP
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
using System.Xml;
using System.Windows.Forms;
using System.Globalization;
using System.Data;
using Ict.Common;
using Ict.Common.Controls;
using Ict.Common.Verification;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.CrossLedger.Data;
using Ict.Petra.Client.CommonForms;
using Ict.Petra.Client.MCommon;
using Ict.Petra.Client.MFinance.Logic;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Client.App.Core.RemoteObjects;
using System.Collections.Generic;


namespace Ict.Petra.Client.MFinance.Gui.Setup
{
    public partial class TFrmSetupDailyExchangeRate
    {
        /// <summary>
        /// The base currency is used to initialize the "from" combobox
        /// </summary>
        private String baseCurrencyOfLedger;

        private const decimal EXCHANGE_RATE_WARNING_RATIO = 1.10m;

        // Private variables relating to use when we are MODAL
        private bool blnIsInModalMode = false;
        private String modalCurrencyFrom = null;
        private decimal modalRateOfExchange = 1.0m;
        private DateTime modalEffectiveDate = DateTime.MinValue;
        private DateTime minModalEffectiveDate = DateTime.MinValue;
        private DateTime maxModalEffectiveDate = DateTime.MaxValue;
        private int modalEffectiveTime = 0;

        // Filters and sorting
        private string SortByDateDescending =
            ADailyExchangeRateTable.GetToCurrencyCodeDBName() + ", " +
            ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + ", " +
            ADailyExchangeRateTable.GetDateEffectiveFromDBName() + " DESC, " +
            ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";

        // Other variables
        private bool FIsRateUnused = false;

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

        /// <summary>
        /// Gets the current text from the rate usage tooltip (used by the test software)
        /// </summary>
        public string Usage
        {
            get
            {
                string usage = String.Empty;

                if (FPreviouslySelectedDetailRow != null)
                {
                    usage = String.Format("{0} Journals and {1} Gift Batches;",
                        FPreviouslySelectedDetailRow.JournalUsage, FPreviouslySelectedDetailRow.GiftBatchUsage);

                    if ((FPreviouslySelectedDetailRow.JournalUsage > 0) || (FPreviouslySelectedDetailRow.GiftBatchUsage > 0))
                    {
                        DataView usageView = ((DevAge.ComponentModel.BoundDataView)grdRateUsage.DataSource).DataView;

                        foreach (DataRowView drv in usageView)
                        {
                            ExchangeRateTDSADailyExchangeRateUsageRow row = (ExchangeRateTDSADailyExchangeRateUsageRow)drv.Row;
                            usage += Environment.NewLine;
                            usage += String.Format("Ledger:{0} Batch:{1} Journal:{2} Status:{3} at Rate:{4} on Date:{5} at Time:{6}",
                                row.LedgerNumber,
                                row.BatchNumber,
                                row.JournalNumber,
                                row.BatchStatus,
                                row.RateOfExchange,
                                row.DateEffectiveFrom.ToString("yyyy-MM-dd"),
                                row.TimeEffectiveFrom);
                        }
                    }
                }

                return usage;
            }
        }

        /// <summary>
        /// Shows only the unused rates (used by the test software)
        /// </summary>
        public void ShowUnusedRates()
        {
            ((RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtUnusedRates")).Checked = true;
        }

        /// <summary>
        /// Shows only the used rates (used by the test software)
        /// </summary>
        public void ShowUsedRates()
        {
            ((RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtUsedRates")).Checked = true;
        }

        private void InitializeManualCode()
        {
            // This code runs just before the auto-generated code binds the data to the grid
            // We need to set the RowFilter to something that returns no rows because we will return the rows we actually want
            // in RunOnceOnActivation.  By returning no rows now we reduce some horrible flicker on the screen (and save time!)
            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0} = #{1}#",
                FMainDS.ADailyExchangeRate.ColumnDateEffectiveFrom,
                DateTime.MaxValue.ToString("d", CultureInfo.InvariantCulture));

            // Now we set some default settings that apply when the screen is MODELESS
            //  (If the screen will be MODAL one of the ShowDialog methods will be called below)
            pnlModalButtons.Visible = false;
            mniImport.Enabled = true;           // Allow imports
            tbbImport.Enabled = true;
            blnIsInModalMode = false;
            chkHideOthers.Left = cmbDetailToCurrencyCode.Left;

            // Fix up the usage grid and columns
            FPetraUtilsObject.SetStatusBarText(grdRateUsage,
                Catalog.GetString("Double click a row in the 'Usage' grid to view the corresponding Gift Batch or Journal."));
            FPetraUtilsObject.SetStatusBarText(btnRateUsage,
                Catalog.GetString("Click to view the Gift Batch or Journal corresponding to the highlighted row."));

            string expression = String.Format("IIF({0}=0, 'Gift Batch', 'GL Batch')",
                ExchangeRateTDSADailyExchangeRateUsageTable.GetJournalNumberDBName());
            int batchTypeColumnOrdinal = FMainDS.ADailyExchangeRateUsage.Columns.Add("BatchType", typeof(System.String), expression).Ordinal;

            expression = String.Format("IIF({0}=0, '', CONVERT({0}, 'System.String'))",
                ExchangeRateTDSADailyExchangeRateUsageTable.GetJournalNumberDBName());
            int journalNumberAsTextColumnOrdinal = FMainDS.ADailyExchangeRateUsage.Columns.Add("JournalNumberAsString",
                typeof(System.String),
                expression).Ordinal;

            grdRateUsage.AddTextColumn(Catalog.GetString("Batch Status"), FMainDS.ADailyExchangeRateUsage.ColumnBatchStatus);
            grdRateUsage.AddTextColumn(Catalog.GetString("Batch Type"), FMainDS.ADailyExchangeRateUsage.Columns[batchTypeColumnOrdinal]);
            grdRateUsage.AddTextColumn(Catalog.GetString("Ledger Number"), FMainDS.ADailyExchangeRateUsage.ColumnLedgerNumber);
            grdRateUsage.AddTextColumn(Catalog.GetString("Batch Number"), FMainDS.ADailyExchangeRateUsage.ColumnBatchNumber);
            grdRateUsage.AddTextColumn(Catalog.GetString("Journal Number"), FMainDS.ADailyExchangeRateUsage.Columns[journalNumberAsTextColumnOrdinal]);

            // Again - set a stupid initial row filter for the usage and glue up the data source to the grid
            FMainDS.ADailyExchangeRateUsage.DefaultView.RowFilter = String.Format("{0}=0",
                ExchangeRateTDSADailyExchangeRateUsageTable.GetLedgerNumberDBName());
            DataView usageView = FMainDS.ADailyExchangeRateUsage.DefaultView;
            usageView.AllowNew = false;
            grdRateUsage.DataSource = new DevAge.ComponentModel.BoundDataView(usageView);

            // This is where we load all the data.  The auto-generated code did not load anything yet
            FMainDS.Merge(TRemote.MFinance.Common.WebConnectors.LoadDailyExchangeRateData());
        }

        /// <summary>
        /// not anymore RunOnceOnActivationManual, because that might not be run before the formhandler in a NUnitforms test
        /// </summary>
        private void RunBeforeActivation()
        {
            ((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbDetailFromCurrencyCode")).SelectedIndex = -1;
            ((TCmbAutoComplete)FFilterAndFindObject.FilterPanelControls.FindControlByName("cmbDetailToCurrencyCode")).SelectedIndex = -1;

            // Set the Tag for the checkbox since we don't want changes to the checkbox to look like we have to save the data
            this.chkHideOthers.Tag = MCommon.MCommonResourcestrings.StrCtrlSuppressChangeDetection;

            // Activate events we will use in manual code
            this.txtDetailRateOfExchange.TextChanged +=
                new EventHandler(txtDetailRateOfExchange_TextChanged);

            // These Leave events are all fired before validation updates the row
            this.cmbDetailFromCurrencyCode.Leave +=
                new System.EventHandler(this.CurrencyCodeComboBox_Leave);
            this.cmbDetailToCurrencyCode.Leave +=
                new System.EventHandler(this.CurrencyCodeComboBox_Leave);
            this.dtpDetailDateEffectiveFrom.Leave +=
                new EventHandler(dtpDetailDateEffectiveFrom_Leave);

            // GUI events
            this.btnInvertExchangeRate.Click += new System.EventHandler(this.InvertExchangeRate);
            this.btnRateUsage.Click += new EventHandler(ViewRateUsage);
            this.grdRateUsage.DoubleClickCell += new TDoubleClickCellEventHandler(ViewRateUsage);
            this.chkHideOthers.CheckedChanged += new EventHandler(chk_CheckedChanged);

            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);

            // Set a non-standard sort order (newest record first)
            DataView theView = FMainDS.ADailyExchangeRate.DefaultView;
            theView.Sort = SortByDateDescending;

            // Set the RowFilter - in MODAL mode it has already been set, but in MODELESS mode we need to set up to see all rows
            if (!blnIsInModalMode)
            {
                theView.RowFilter = "";

                // Have a last attempt at deciding what the base currency is...
                if (baseCurrencyOfLedger == null)
                {
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
            }

            // Having changed the sort order we need to put the correct details in the panel (assuming we have a row to display)
            if (theView.Count > 0)
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

            UpdateRecordNumberDisplay();
        }

        private void RunOnceOnActivationManual()
        {
            if (!this.blnIsInModalMode)
            {
                RunBeforeActivation();
            }
        }

        /// <summary>
        /// Standard method to process a OP Forms Message from another window
        /// </summary>
        /// <param name="AFormsMessage">The message</param>
        /// <returns>True if we handle the message</returns>
        public bool ProcessFormsMessage(TFormsMessage AFormsMessage)
        {
            bool MessageProcessed = false;

            // Is it for us??
            if (AFormsMessage.MessageClass == TFormsMessageClassEnum.mcGLOrGiftBatchSaved)
            {
                // We received this message because a GL or Gift Batch was saved that contained a Forex row!!
                // remember the current row
                int nCurrentRow = GetSelectedRowIndex();

                // re-load the data
                FMainDS.Clear();
                FMainDS.Merge(TRemote.MFinance.Common.WebConnectors.LoadDailyExchangeRateData());
                FMainDS.AcceptChanges();

                // select the same row as before
                SelectRowInGrid(nCurrentRow);

                MessageProcessed = true;
            }

            return MessageProcessed;
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
        /// <param name="LedgerNumber">The ledger number from which the base currency (currency to) will be extracted</param>
        /// <param name="dteEffective">Effective date of the actual acounting process.  The grid will show all entries on or before this date.</param>
        /// <param name="strCurrencyFrom">The actual foreign currency used for the transaction</param>
        /// <param name="ExchangeDefault">Default value for the exchange rate</param>
        /// <param name="SelectedExchangeRate">The selected value for the exchange rate</param>
        /// <param name="SelectedEffectiveDate">The selected value for the effective date</param>
        /// <param name="SelectedEffectiveTime">The selected value for the effective time</param>
        public DialogResult ShowDialog(Int32 LedgerNumber, DateTime dteEffective,
            string strCurrencyFrom,
            decimal ExchangeDefault,
            out decimal SelectedExchangeRate,
            out DateTime SelectedEffectiveDate,
            out int SelectedEffectiveTime)
        {
            // We can just call our alternate method, setting the start date to the beginning of time!
            return ShowDialog(LedgerNumber,
                DateTime.MinValue,
                dteEffective,
                strCurrencyFrom,
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
        /// <param name="LedgerNumber">The ledger number from which the base currency (currency to) will be extracted</param>
        /// <param name="dteStart">The start date for the date range</param>
        /// <param name="dteEnd">The end date for the date range.  The grid will show all entries between the start and end dates.</param>
        /// <param name="strCurrencyFrom">The actual foreign currency used for the transaction</param>
        /// <param name="ExchangeDefault">Default value for the exchange rate</param>
        /// <param name="SelectedExchangeRate">The selected value for the exchange rate</param>
        /// <param name="SelectedEffectiveDate">The selected value for the effective date</param>
        /// <param name="SelectedEffectiveTime">The selected value for the effective time</param>
        public DialogResult ShowDialog(Int32 LedgerNumber, DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyFrom,
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
            minModalEffectiveDate = dteStart;
            maxModalEffectiveDate = dteEnd;

            // Do not use local formats here!
            string filter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' and {2}='{3}' and {4}<#{5}#",
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                strCurrencyFrom,
                ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                baseCurrencyOfLedger,
                ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                dateEnd2.ToString("d", CultureInfo.InvariantCulture));

            if (dteStart > DateTime.MinValue)
            {
                filter += String.Format(CultureInfo.InvariantCulture, " and {0}>#{1}#",
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                    dteStart.ToString("d", CultureInfo.InvariantCulture));
            }

            DataView myDataView = FMainDS.ADailyExchangeRate.DefaultView;
            myDataView.RowFilter = filter;
            myDataView.Sort = SortByDateDescending;

            modalRateOfExchange = ExchangeDefault;
            modalCurrencyFrom = strCurrencyFrom;
            modalEffectiveDate = dteEnd;

            DefineModalSettings();

            RunBeforeActivation();

            DialogResult dlgResult = base.ShowDialog();

            SelectedExchangeRate = modalRateOfExchange;
            SelectedEffectiveDate = modalEffectiveDate;
            SelectedEffectiveTime = modalEffectiveTime;

            return dlgResult;
        }

        /// <summary>
        /// Get the most recent exchange rate value of the interval.  This method does not display a dialog
        /// </summary>
        /// <param name="LedgerNumber">The ledger number from which the base currency (currency to) will be extracted</param>
        /// <param name="dteStart">The start date for the date range</param>
        /// <param name="dteEnd">The end date for the date range.</param>
        /// <param name="strCurrencyFrom">The actual foreign currency used for the transaction</param>
        /// <returns>The most recent exchange rate in the specified date range</returns>
        public decimal GetLastExchangeValueOfInterval(Int32 LedgerNumber, DateTime dteStart,
            DateTime dteEnd,
            string strCurrencyFrom)
        {
            ALedgerRow ledger =
                ((ALedgerTable)TDataCache.TMFinance.GetCacheableFinanceTable(
                     TCacheableFinanceTablesEnum.LedgerDetails, LedgerNumber))[0];

            baseCurrencyOfLedger = ledger.BaseCurrency;
            DateTime dateEnd2 = dteEnd.AddDays(1.0);

            // Do not use local formats here!
            string filter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' and {2}='{3}' and {4}<#{5}# and {6}>#{7}#",
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                strCurrencyFrom,
                ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                baseCurrencyOfLedger,
                ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                dateEnd2.ToString("d", CultureInfo.InvariantCulture),
                ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                dteStart.ToString("d", CultureInfo.InvariantCulture));
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
            pnlModalButtons.Visible = true;

            // Import not allowed when MODAL
            mniImport.Enabled = false;
            tbbImport.Enabled = false;

            // Different Dialog Title text - and set the buttons
            this.Text = "Select an Exchange Rate";
            this.AcceptButton = btnClose;
            this.CancelButton = btnCancel;
            chkHideOthers.Visible = false;

            blnIsInModalMode = true;
            DialogResult = DialogResult.Cancel;     // assume it is cancelled for now

            // Redirect the standard close methods to the modal handler and modify the text
            mniClose.Click -= this.actClose;
            mniClose.Click += this.CloseDialog;
            btnClose.Click -= this.actClose;
            btnClose.Click += this.CloseDialog;
            mniClose.Text = "Accept";
            btnClose.Text = "Accept";
        }

        /// <summary>
        /// Called in MODAL mode when the user clicks the Accept/Close button ...
        /// Also called in all modes on grdDetails_DoubleClick
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void CloseDialog(object sender, EventArgs e)
        {
            // Don't let double click close us!
            if (!blnIsInModalMode)
            {
                return;
            }

            // If there have been changes we save them without asking, since that is part of the deal of clicking OK
            if (FPetraUtilsObject.HasChanges && !SaveChanges())
            {
                return;
            }

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
            if (btnCancel.DialogResult == DialogResult.Abort)
            {
                FPetraUtilsObject.DisableSaveButton();
            }

            // Although the user has clicked Cancel, we need to ask if we need to save any changes that have been made
            if (FPetraUtilsObject.CloseFormCheck())
            {
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

            UpdateExchangeRateLabels();
        }

        private void NewRowManual(ref ExchangeRateTDSADailyExchangeRateRow ARow)
        {
            // We just need to decide on the appropriate currency pair and then call the standard method to get a suggested rate and date
            if (FPreviouslySelectedDetailRow == null)
            {
                if (baseCurrencyOfLedger == null)
                {
                    ARow.FromCurrencyCode = "GBP";
                    ARow.ToCurrencyCode = "USD";
                }
                else
                {
                    if (modalCurrencyFrom != null)
                    {
                        ARow.FromCurrencyCode = modalCurrencyFrom;
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
                    }

                    ARow.ToCurrencyCode = baseCurrencyOfLedger;
                }
            }
            else
            {
                // Use the same settings as the highlighted row
                // Note that if we have been called modally the ToCurrencyCode will automatically be the baseLedgerCurrency
                if (modalCurrencyFrom != null)
                {
                    ARow.FromCurrencyCode = modalCurrencyFrom;
                }
                else
                {
                    ARow.FromCurrencyCode = cmbDetailFromCurrencyCode.GetSelectedString();
                }

                ARow.ToCurrencyCode = cmbDetailToCurrencyCode.GetSelectedString();
            }

            // Choose the effective date
            DateTime suggestedDate = GetSuggestedDate();

            Int32 suggestedTime;
            decimal suggestedRate;
            GetSuggestedTimeAndRateForCurrencyPair(ARow.FromCurrencyCode,
                ARow.ToCurrencyCode,
                suggestedDate,
                out suggestedTime,
                out suggestedRate);

            ARow.DateEffectiveFrom = suggestedDate;
            ARow.TimeEffectiveFrom = suggestedTime;
            ARow.RateOfExchange = suggestedRate;
        }

        private DateTime GetSuggestedDate()
        {
            // The suggested date is pretty much fixed - it is the time we fiddle with
            if (blnIsInModalMode && (modalEffectiveDate > DateTime.MinValue))
            {
                return DateTime.Parse(modalEffectiveDate.ToLongDateString());
            }
            else
            {
                //For Corporate Exchange Rate must be 1st of the month, for Daily Exchange Rate it must be now
                return DateTime.Parse(DateTime.Now.ToLongDateString());
            }
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            string rowFilter = FFilterAndFindObject.CurrentActiveFilter;

            ApplyFilterManual(ref rowFilter);

            FFilterAndFindObject.ApplyFilter();
            grdDetails.SelectRowWithoutFocus(grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1);
        }

        private void ApplyFilterManual(ref string AFilterString)
        {
            string extraFilter = AFilterString.Substring(FFilterAndFindObject.FilterPanelControls.BaseFilter.Length);

            if (extraFilter.StartsWith(" AND "))
            {
                extraFilter = extraFilter.Substring(5);
            }

            string showRatesFilter = String.Empty;
            RadioButton rbtShowUsed = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtUsedRates");
            RadioButton rbtShowUnused = (RadioButton)FFilterAndFindObject.FilterPanelControls.FindControlByName("rbtUnusedRates");

            if (rbtShowUsed.Checked)
            {
                showRatesFilter = String.Format("{0}>0 OR {1}>0",
                    ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                    ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName());
            }
            else if (rbtShowUnused.Checked)
            {
                showRatesFilter = String.Format("{0}=0 AND {1}=0",
                    ExchangeRateTDSADailyExchangeRateTable.GetJournalUsageDBName(),
                    ExchangeRateTDSADailyExchangeRateTable.GetGiftBatchUsageDBName());
            }

            if (chkHideOthers.Checked)
            {
                if (showRatesFilter.Length > 0)
                {
                    showRatesFilter += " AND ";
                }

                showRatesFilter += String.Format("{0}='{1}'",
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    cmbDetailToCurrencyCode.GetSelectedString());
            }

            FFilterAndFindObject.FilterPanelControls.SetBaseFilter(showRatesFilter, showRatesFilter.Length == 0);
            AFilterString = showRatesFilter + extraFilter;
        }

        private void dtpDetailDateEffectiveFrom_Leave(object sender, EventArgs e)
        {
            // Note that we use Leave because it is fired before control validation
            // Get a new time and rate for the date
            int suggestedTime;
            decimal suggestedRate;

            try
            {
                DateTime dt = dtpDetailDateEffectiveFrom.Date.Value;

                if (dt != FPreviouslySelectedDetailRow.DateEffectiveFrom)
                {
                    // The date in the control is different from the value in the table
                    GetSuggestedTimeAndRateForCurrencyPair(cmbDetailFromCurrencyCode.GetSelectedString(),
                        cmbDetailToCurrencyCode.GetSelectedString(), dt, out suggestedTime, out suggestedRate);
                    txtDetailTimeEffectiveFrom.Text =
                        new Ict.Common.TypeConverter.TShortTimeConverter().ConvertTo(suggestedTime, typeof(string)).ToString();
                    txtDetailRateOfExchange.NumberValueDecimal = suggestedRate;
                }
            }
            catch (InvalidOperationException)
            {
                // ooops.  The date is empty or badly formed
                txtDetailTimeEffectiveFrom.Text = new Ict.Common.TypeConverter.TShortTimeConverter().ConvertTo(-1, typeof(string)).ToString();
                txtDetailRateOfExchange.NumberValueDecimal = 0.0m;
            }
        }

        /// <summary>
        /// Updates the lblValueOneDirection and lblValueOtherDirection labels
        /// </summary>
        private void UpdateExchangeRateLabels(object Sender = null, EventArgs e = null)
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
            if ((strFrom != FPreviouslySelectedDetailRow.FromCurrencyCode) || (strTo != FPreviouslySelectedDetailRow.ToCurrencyCode))
            {
                // It must be a real change - so we should calculate a new effective date and propose an exchange rate
                // Start with the effective date
                DateTime suggestedDate = GetSuggestedDate();

                // Now do time and rate
                decimal suggestedRate;
                int suggestedTime;
                GetSuggestedTimeAndRateForCurrencyPair(strFrom, strTo, suggestedDate, out suggestedTime, out suggestedRate);
                dtpDetailDateEffectiveFrom.Date = suggestedDate;
                txtDetailTimeEffectiveFrom.Text =
                    new Ict.Common.TypeConverter.TShortTimeConverter().ConvertTo(suggestedTime, typeof(string)).ToString();
                txtDetailRateOfExchange.NumberValueDecimal = suggestedRate;
            }

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
            try
            {
                txtDetailRateOfExchange.NumberValueDecimal = Math.Round(1 / txtDetailRateOfExchange.NumberValueDecimal.Value, 10);
            }
            catch (Exception)
            {
            }

            UpdateExchangeRateLabels();
        }

        private void ViewRateUsage(object sender, EventArgs e)
        {
            // Which row is highlighted?
            int rowIndex = grdRateUsage.GetFirstHighlightedRowIndex();

            if (rowIndex < 0)
            {
                return;
            }

            DataRowView rowView = (DataRowView)grdRateUsage.Rows.IndexToDataSourceRow(rowIndex);
            ExchangeRateTDSADailyExchangeRateUsageRow row = (ExchangeRateTDSADailyExchangeRateUsageRow)rowView.Row;

            // We need an XML document node to pass to the method that opens screens
            XmlDocument xmlDoc = new XmlDocument();
            string xml = String.Empty;

            if (row.JournalNumber == 0)
            {
                // It is a gift
                xml = "<tasks><task Namespace=\"Ict.Petra.Client.MFinance.Gui.Gift\" ActionOpenScreen=\"TFrmGiftBatch\" ";
                xml += String.Format("LedgerNumber=\"{0}\" InitialBatchYear=\"{1}\" InitialBatchNumber=\"{2}\" /></tasks> ",
                    row.LedgerNumber, row.BatchYear, row.BatchNumber);
            }
            else
            {
                // Its GL
                xml = "<tasks><task Namespace=\"Ict.Petra.Client.MFinance.Gui.GL\" ActionOpenScreen=\"TFrmGLBatch\" ";
                xml += String.Format(
                    "LedgerNumber=\"{0}\" InitialBatchYear=\"{1}\" InitialBatchNumber=\"{2}\" InitialJournalNumber=\"{3}\" /></tasks> ",
                    row.LedgerNumber,
                    row.BatchYear,
                    row.BatchNumber,
                    row.JournalNumber);
            }

            xmlDoc.LoadXml(xml);
            XmlNode node = xmlDoc.SelectSingleNode("//task");

            // Open the screen with the properties we have defined in the node
            Cursor = Cursors.WaitCursor;
            TLstTasks.ExecuteAction(node, null);
            Cursor = Cursors.Default;
        }

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ExchangeRateTDSADailyExchangeRateRow ARow)
        {
            if (ARow == null)
            {
                txtDetailRateOfExchange.NumberValueDecimal = null;
                FMainDS.ADailyExchangeRateUsage.DefaultView.RowFilter = String.Format("{0}=0",
                    ExchangeRateTDSADailyExchangeRateUsageTable.GetLedgerNumberDBName());
                btnRateUsage.Enabled = false;
                grdRateUsage.TabStop = false;
                SetEnabledStates();
            }
            else
            {
                // Older databases may have times that require long format...  Otherwise the data gets 'modified' without us realising
                //  which leads to all sorts of unwanted warnings ...
                string strLong = new Ict.Common.TypeConverter.TLongTimeConverter().ConvertTo(ARow.TimeEffectiveFrom, typeof(string)).ToString();

                if (!strLong.EndsWith("00"))
                {
                    txtDetailTimeEffectiveFrom.Text = strLong;
                }

                if (ARow.FromCurrencyCode == ARow.ToCurrencyCode)
                {
                    ARow.RateOfExchange = 1.0m;
                }

                // Deal with where the rate has been used
                FIsRateUnused = (ARow.JournalUsage == 0) && (ARow.GiftBatchUsage == 0);

                string rowFilter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=#{5}# AND ({6}={7} OR {8}='GB')",
                    ExchangeRateTDSADailyExchangeRateUsageTable.GetFromCurrencyCodeDBName(),
                    ARow.FromCurrencyCode,
                    ExchangeRateTDSADailyExchangeRateUsageTable.GetToCurrencyCodeDBName(),
                    ARow.ToCurrencyCode,
                    ExchangeRateTDSADailyExchangeRateUsageTable.GetDateEffectiveFromDBName(),
                    ARow.DateEffectiveFrom.ToString("d", CultureInfo.InvariantCulture),
                    ExchangeRateTDSADailyExchangeRateUsageTable.GetTimeEffectiveFromDBName(),
                    ARow.TimeEffectiveFrom,
                    ExchangeRateTDSADailyExchangeRateUsageTable.GetTableSourceDBName());
                FMainDS.ADailyExchangeRateUsage.DefaultView.RowFilter = rowFilter;

                if (grdRateUsage.Rows.Count > 1)
                {
                    grdRateUsage.SelectRowWithoutFocus(1);
                    grdRateUsage.TabStop = true;
                }
                else
                {
                    grdRateUsage.TabStop = false;
                }

                btnRateUsage.Enabled = !FIsRateUnused;

                chkHideOthers.Enabled = true;

                SetEnabledStates();
            }

            btnClose.Enabled = ARow != null;
            UpdateExchangeRateLabels();
        }

        private void SetEnabledStates()
        {
            //Filter only applies to currency To/From fields, which are always disabled in Modal view
            // and so filter is not needed. Otherwise the user is able to use the filter to select different currencies
            //  other what is displayed in the To/From comboboxes
            chkToggleFilter.Enabled = !blnIsInModalMode;

            btnClose.Enabled = pnlDetails.Enabled;

            if (!pnlDetails.Enabled)
            {
                return;
            }

            // Enable or disable the combo boxes
            cmbDetailFromCurrencyCode.Enabled = cmbDetailFromCurrencyCode.Enabled && !blnIsInModalMode;
            cmbDetailToCurrencyCode.Enabled = cmbDetailToCurrencyCode.Enabled && !chkHideOthers.Checked && !blnIsInModalMode;

            // Set the Enabled states of txtRateOfExchange and the Invert and Delete buttons
            if (cmbDetailFromCurrencyCode.GetSelectedString() ==
                cmbDetailToCurrencyCode.GetSelectedString())
            {
                // Both currencies the same
                txtDetailRateOfExchange.NumberValueDecimal = 1.0m;
                txtDetailRateOfExchange.Enabled = false;
                txtDetailTimeEffectiveFrom.Enabled = false;
                btnInvertExchangeRate.Enabled = false;
                btnDelete.Enabled = true;
            }
            else
            {
                // Currencies differ
                txtDetailRateOfExchange.Enabled = FIsRateUnused;
                txtDetailTimeEffectiveFrom.Enabled = FIsRateUnused && !txtDetailTimeEffectiveFrom.ReadOnly;
                btnInvertExchangeRate.Enabled = FIsRateUnused;
                btnDelete.Enabled = FIsRateUnused;
            }
        }

        private void txtDetailRateOfExchange_TextChanged(object sender, EventArgs e)
        {
            if (txtDetailRateOfExchange.Text.Trim() != String.Empty)
            {
                UpdateExchangeRateLabels();
            }
        }

        private void Import(System.Object sender, EventArgs e)
        {
            if (ValidateAllData(true, true))
            {
                TVerificationResultCollection results = FPetraUtilsObject.VerificationResultCollection;

                int nRowsImported = TImportExchangeRates.ImportCurrencyExRates(FMainDS.ADailyExchangeRate, "Daily", results);

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
                    MessageBox.Show(String.Format(formatter,
                            Environment.NewLine,
                            results[0].ResultText,
                            nRowsImported,
                            MCommonResourcestrings.StrExchRateImportTryAgain,
                            results[0].ResultCode),
                        MCommonResourcestrings.StrExchRateImportTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);

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

        private void ValidateDataDetailsManual(ADailyExchangeRateRow ARow)
        {
            TVerificationResultCollection VerificationResultCollection = FPetraUtilsObject.VerificationResultCollection;

            TSharedFinanceValidation_GLSetup.ValidateDailyExchangeRate(this, ARow, ref VerificationResultCollection,
                FPetraUtilsObject.ValidationControlsDict, minModalEffectiveDate, maxModalEffectiveDate);

            // Now make an additional manual check that the rate is sensible
            TScreenVerificationResult verificationResult = null;

            if ((ARow.RowState == DataRowState.Added) || (ARow.RowState == DataRowState.Modified))
            {
                // We are going to check if the rate of exchange is sensible.  We need our own view because we don't know how the grid is currently sorted
                string filter =
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + ARow.FromCurrencyCode + "' and " +
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + ARow.ToCurrencyCode + "'";
                DataView myView = new DataView(FMainDS.ADailyExchangeRate, filter, SortByDateDescending, DataViewRowState.CurrentRows);

                // Find our current row
                int nThis = FindRowInDataView(myView, ARow.FromCurrencyCode, ARow.ToCurrencyCode, ARow.DateEffectiveFrom, ARow.TimeEffectiveFrom);
                ADailyExchangeRateRow drThis = null;
                ADailyExchangeRateRow drPrev = null;
                ADailyExchangeRateRow drNext = null;
                decimal ratio = 1.0m;

                if ((nThis >= 0) && (ARow.RateOfExchange != 0.0m))
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
                        string validationMessage = String.Format(
                            ErrorCodes.GetErrorInfo(PetraErrorCodes.ERR_EXCH_RATE_MAY_BE_INCORRECT).ErrorMessageText,
                            ARow.RateOfExchange,
                            ARow.FromCurrencyCode,
                            ARow.ToCurrencyCode,
                            dtpDetailDateEffectiveFrom.Text,
                            txtDetailTimeEffectiveFrom.Text,
                            ratio - 1.0m);

                        // So we have a new warning to raise on a row that has been added/edited
                        verificationResult = new TScreenVerificationResult(
                            this,
                            ARow.Table.Columns[ADailyExchangeRateTable.ColumnRateOfExchangeId],
                            validationMessage,
                            Catalog.GetString("Exchange Rate Alert"),
                            PetraErrorCodes.ERR_EXCH_RATE_MAY_BE_INCORRECT,
                            txtDetailRateOfExchange,
                            TResultSeverity.Resv_Noncritical);
                    }
                }
            }

            VerificationResultCollection.Auto_Add_Or_AddOrRemove(this, verificationResult,
                ARow.Table.Columns[ADailyExchangeRateTable.ColumnRateOfExchangeId]);
        }

        private void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
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
            DataView dv = new DataView(FMainDS.ADailyExchangeRate);

            for (int i = 0; i < lstInverses.Count; i++)
            {
                tInverseItem item = lstInverses[i];

                // Does the item exist already?
                dv.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4}=#{5}# AND {6}={7}",
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                    item.FromCurrencyCode,
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    item.ToCurrencyCode,
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                    item.DateEffective.ToString("d", CultureInfo.InvariantCulture),
                    ADailyExchangeRateTable.GetTimeEffectiveFromDBName(),
                    item.TimeEffective);

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

        /// <summary>
        /// This is the standard method that is used to suggest a rate and effective time for a new condition.
        /// The suggestions depend on the FromCurrency, ToCurrency and Effective date and is based on the other values in the table
        /// The method is called both when creating a new row and when modifying the currencies of an existing row
        /// The suggested time will be the next available time on or after 02:00 for the currencies and date.
        /// The suggested rate will be either the rate that applied immediately on or after the date/time,
        ///   or will be the corporate rate immediately on or after the date/time, or failing all that, 0.0
        /// </summary>
        /// <param name="FromCurrency">The FromCurrency</param>
        /// <param name="ToCurrency">The ToCurrency</param>
        /// <param name="EffectiveDate">The effective date for the currency pair</param>
        /// <param name="SuggestedTime">The suggested effective time for the currency pair</param>
        /// <param name="SuggestedRate">The suggested effective rate of exchange for the currency pair</param>
        private void GetSuggestedTimeAndRateForCurrencyPair(string FromCurrency,
            string ToCurrency,
            DateTime EffectiveDate,
            out Int32 SuggestedTime,
            out decimal SuggestedRate)
        {
            // Do the effective time.  We default to 2am.
            // I don't think that the current time is very useful, since the idea is that this rate should apply to the day as a whole
            // Use 2am so that if runs are done on a schedule during the night the new rate does not kick in too soon
            int tryEffectiveTime = 7200;

            // Ensure we don't create a duplicate record
            while (FMainDS.ADailyExchangeRate.Rows.Find(new object[] {
                           FromCurrency, ToCurrency,
                           EffectiveDate.ToString(), tryEffectiveTime.ToString()
                       }) != null)
            {
                tryEffectiveTime = tryEffectiveTime + 600;              // 10 minute increments

                if (tryEffectiveTime >= 86400)
                {
                    tryEffectiveTime = 60;                              // Do not pass midnight!
                }
            }

            SuggestedTime = tryEffectiveTime;

            // If we cannot come up with a rate, it will be 0.0 (which is not allowed so it will force the user to enter a better number)
            SuggestedRate = 0.0m;
            decimal tryCorporateRate;

            if (FromCurrency == ToCurrency)
            {
                // Always 1.0
                SuggestedRate = 1.0m;
            }
            else if (GetCorporateRate(FromCurrency, ToCurrency, EffectiveDate, out tryCorporateRate))
            {
                SuggestedRate = tryCorporateRate;
            }
            else
            {
                // Rate of exchange will be the latest value used, if there is one
                // Get the most recent value for this currency pair
                string rowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4} <= #{5}#",
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                    FromCurrency,
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    ToCurrency,
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                    EffectiveDate.ToString("d", CultureInfo.InvariantCulture));
                string sortBy = String.Format("{0} DESC, {1} DESC",
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(), ADailyExchangeRateTable.GetTimeEffectiveFromDBName());
                DataView dv = new DataView(FMainDS.ADailyExchangeRate, rowFilter, sortBy, DataViewRowState.CurrentRows);

                if (dv.Count > 0)
                {
                    // Use this rate
                    SuggestedRate = ((ADailyExchangeRateRow)dv[0].Row).RateOfExchange;
                }
            }
        }

        /// <summary>
        /// Gets the rate from the Corporate Rate table for a currency pair and date
        /// </summary>
        /// <param name="FromCurrency">The From Currency</param>
        /// <param name="ToCurrency">The To Currency</param>
        /// <param name="EffectiveDate">The effective date</param>
        /// <param name="SuggestedRate">The corresponding rate, if it exists.  0.0 otherwise</param>
        /// <returns>True if the rate exists</returns>
        private bool GetCorporateRate(string FromCurrency, string ToCurrency, DateTime EffectiveDate, out decimal SuggestedRate)
        {
            SuggestedRate = 0.0m;
            DataView dv = FMainDS.ACorporateExchangeRate.DefaultView;
            dv.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4} <= #{5}#",
                ACorporateExchangeRateTable.GetFromCurrencyCodeDBName(),
                FromCurrency,
                ACorporateExchangeRateTable.GetToCurrencyCodeDBName(),
                ToCurrency,
                ACorporateExchangeRateTable.GetDateEffectiveFromDBName(),
                EffectiveDate.ToString("d", CultureInfo.InvariantCulture));
            dv.Sort = String.Format("{0} DESC", ADailyExchangeRateTable.GetDateEffectiveFromDBName());

            if (dv.Count > 0)
            {
                SuggestedRate = ((ACorporateExchangeRateRow)dv[0].Row).RateOfExchange;
                return true;
            }

            return false;
        }

        /// <summary>
        /// I have had to write this method because I could not get the DataView.Find to work (with dates?)
        /// </summary>
        /// <param name="ADataView">The DataView to search</param>
        /// <param name="FromCurrency"></param>
        /// <param name="ToCurrency"></param>
        /// <param name="EffectiveDate"></param>
        /// <param name="EffectiveTime"></param>
        /// <returns>The integer row index, or -1 if not found</returns>
        private Int32 FindRowInDataView(DataView ADataView, String FromCurrency, String ToCurrency, DateTime EffectiveDate, Int32 EffectiveTime)
        {
            for (int n = 0; n < ADataView.Count; n++)
            {
                object[] itemArray = ADataView[n].Row.ItemArray;

                if (FromCurrency.Equals(itemArray[ADailyExchangeRateTable.ColumnFromCurrencyCodeId])
                    && ToCurrency.Equals(itemArray[ADailyExchangeRateTable.ColumnToCurrencyCodeId])
                    && EffectiveDate.Equals(itemArray[ADailyExchangeRateTable.ColumnDateEffectiveFromId])
                    && EffectiveTime.Equals(itemArray[ADailyExchangeRateTable.ColumnTimeEffectiveFromId]))
                {
                    return n;
                }
            }

            return -1;
        }
    }
}