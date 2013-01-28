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

        private string JournalRowFilter = "(" + AJournalTable.GetTransactionCurrencyDBName() + " = '{0}' OR " +
                                          AJournalTable.GetTransactionCurrencyDBName() + " = '{1}') AND " +
                                          AJournalTable.GetDateEffectiveDBName() + " >= #{2}# AND " +
                                          AJournalTable.GetExchangeRateToBaseDBName() + " = {4} AND " +
                                          AJournalTable.GetJournalStatusDBName() + " = '{5}'";
        private string JournalRowFilterRange = "(" + AJournalTable.GetTransactionCurrencyDBName() + " = '{0}' OR " +
                                               AJournalTable.GetTransactionCurrencyDBName() + " = '{1}') AND " +
                                               AJournalTable.GetDateEffectiveDBName() + " >= #{2}# AND " +
                                               AJournalTable.GetDateEffectiveDBName() + " < #{3}# AND " +
                                               AJournalTable.GetExchangeRateToBaseDBName() + " = {4} AND " +
                                               AJournalTable.GetJournalStatusDBName() + " = '{5}'";
        private string GiftBatchRowFilter = "(" + AGiftBatchTable.GetCurrencyCodeDBName() + " = '{0}' OR " +
                                            AGiftBatchTable.GetCurrencyCodeDBName() + " = '{1}') AND " +
                                            AGiftBatchTable.GetGlEffectiveDateDBName() + " >= #{2}# AND " +
                                            AGiftBatchTable.GetGlEffectiveDateDBName() + " < #{3}# AND " +
                                            AGiftBatchTable.GetExchangeRateToBaseDBName() + " = {4} AND " +
                                            AGiftBatchTable.GetBatchStatusDBName() + " = 'Unposted'";

        // Used for edit/delete
        private bool FCanEditCurrentRow = false;
        private bool FCanDeleteCurrentRow = false;
        private bool FIsCurrentRowStateAdded = false;
        private bool bCanEditDelete = false;
        ToolTip tooltipDeleteInfo = new ToolTip();

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

                return SerialisableDS.SaveChanges(GiftBatchTable, TableChanges, AGiftBatchTable.GetTableDBName());
            }
        }

        #endregion

        #region Additional DataSet for Corporate Exchange Rate

        private TCorporateDS FCorporateDS = new TCorporateDS();
        private class TCorporateDS : TTypedDataSet
        {
            private ACorporateExchangeRateTable TableACorporateExchangeRate;

            public ACorporateExchangeRateTable ACorporateExchangeRate
            {
                get
                {
                    return this.TableACorporateExchangeRate;
                }
            }

            protected override void InitTables()
            {
                this.Tables.Add(new ACorporateExchangeRateTable("ACorporateExchangeRate"));
            }

            protected override void InitTables(System.Data.DataSet ds)
            {
                if ((ds.Tables.IndexOf("ACorporateExchangeRate") != -1))
                {
                    this.Tables.Add(new ACorporateExchangeRateTable("ACorporateExchangeRate"));
                }
            }

            protected override void MapTables()
            {
                this.InitVars();
                base.MapTables();

                if ((this.TableACorporateExchangeRate != null))
                {
                    this.TableACorporateExchangeRate.InitVars();
                }
            }

            public override void InitVars()
            {
                this.DataSetName = "PrivateScreenTDS";
                this.TableACorporateExchangeRate = ((ACorporateExchangeRateTable)(this.Tables["ACorporateExchangeRate"]));
            }

            protected override void InitConstraints()
            {
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
            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0} = #{1}#",
                FMainDS.ADailyExchangeRate.ColumnDateEffectiveFrom,
                DateTime.MaxValue.ToString("d", CultureInfo.InvariantCulture));

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

            lblEnableEditDelete.BorderStyle = BorderStyle.FixedSingle;
            lblEnableEditDelete.BackColor = Color.LightGreen;
            lblEnableEditDelete.Visible = false;
            lblEnableEditDelete.Top = btnEnableEdit.Top;
            lblEnableEditDelete.Height = btnEnableEdit.Height;
            lblEnableEditDelete.TextAlign = ContentAlignment.MiddleCenter;

            // We may need the information from the corporate exchange rate table when we create new rates
            Ict.Common.Data.TTypedDataTable TypedTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(ACorporateExchangeRateTable.GetTableDBName(), null, out TypedTable);
            FCorporateDS.ACorporateExchangeRate.Merge(TypedTable);
        }

        private void RunOnceOnActivationManual()
        {
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
            this.btnInvertExchangeRate.Click +=
                new System.EventHandler(this.InvertExchangeRate);
            this.chkHideOthers.CheckedChanged += new EventHandler(chkHideOthers_CheckedChanged);

            FPetraUtilsObject.DataSavingStarted += new TDataSavingStartHandler(FPetraUtilsObject_DataSavingStarted);
            FPetraUtilsObject.DataSaved += new TDataSavedHandler(FPetraUtilsObject_DataSaved);

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
            maxModalEffectiveDate = dateEnd2;

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

        private void NewRowManual(ref ADailyExchangeRateRow ARow)
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

            // We will use this event to hide the tooltip(s)
            grdDetails.Leave += new EventHandler(grdDetails_Leave);

            this.Cursor = Cursors.WaitCursor;

            FJournalDS.JournalTable = new AJournalTable();
            Ict.Common.Data.TTypedDataTable TypedJournalTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(AJournalTable.GetTableDBName(), null, out TypedJournalTable);
            FJournalDS.JournalTable.Merge(TypedJournalTable);
            FJournalDS.JournalTable.DefaultView.Sort = AJournalTable.GetDateEffectiveDBName() + " ASC";

            FGiftBatchDS.GiftBatchTable = new AGiftBatchTable();
            Ict.Common.Data.TTypedDataTable TypedGiftBatchTable;
            TRemote.MCommon.DataReader.WebConnectors.GetData(AGiftBatchTable.GetTableDBName(), null, out TypedGiftBatchTable);
            FGiftBatchDS.GiftBatchTable.Merge(TypedGiftBatchTable);
            FGiftBatchDS.GiftBatchTable.DefaultView.Sort = AGiftBatchTable.GetGlEffectiveDateDBName() + " ASC";

            this.Cursor = Cursors.Default;

            CheckIfRateHasBeenUsed(
                FPreviouslySelectedDetailRow.FromCurrencyCode,
                FPreviouslySelectedDetailRow.ToCurrencyCode,
                FPreviouslySelectedDetailRow.DateEffectiveFrom,
                FPreviouslySelectedDetailRow.TimeEffectiveFrom,
                FPreviouslySelectedDetailRow.RateOfExchange,
                out FCanEditCurrentRow,
                out FCanDeleteCurrentRow);

            SetEnabledStates();

            grdDetails.Focus();
        }

        private void grdDetails_Leave(object sender, EventArgs e)
        {
            tooltipDeleteInfo.Hide(btnInvertExchangeRate);
        }

        private void chkHideOthers_CheckedChanged(object sender, EventArgs e)
        {
            string rowFilter = String.Empty;

            if (chkHideOthers.Checked)
            {
                rowFilter = String.Format("{0}='{1}'",
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    cmbDetailToCurrencyCode.GetSelectedString());
            }

            FMainDS.ADailyExchangeRate.DefaultView.RowFilter = rowFilter;
            SelectRowInGrid(grdDetails.DataSourceRowToIndex2(FPreviouslySelectedDetailRow) + 1);
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

        /// <summary>
        /// Standardroutine
        /// </summary>
        /// <param name="ARow"></param>
        private void ShowDetailsManual(ADailyExchangeRateRow ARow)
        {
            if (ARow == null)
            {
                txtDetailRateOfExchange.NumberValueDecimal = null;
                FIsCurrentRowStateAdded = false;
            }
            else
            {
                if (ARow.FromCurrencyCode == ARow.ToCurrencyCode)
                {
                    ARow.RateOfExchange = 1.0m;
                }

                if (lblEnableEditDelete.Visible)
                {
                    CheckIfRateHasBeenUsed(
                        ARow.FromCurrencyCode,
                        ARow.ToCurrencyCode,
                        ARow.DateEffectiveFrom,
                        ARow.TimeEffectiveFrom,
                        ARow.RateOfExchange,
                        out FCanEditCurrentRow,
                        out FCanDeleteCurrentRow);
                }

                FIsCurrentRowStateAdded = ARow.RowState == DataRowState.Added;
                SetEnabledStates();
            }

            UpdateExchangeRateLabels();
        }

        private void SetEnabledStates()
        {
            btnClose.Enabled = pnlDetails.Enabled;

            if (!pnlDetails.Enabled)
            {
                return;
            }

            // Enable or disable the combo boxes
            bool bEnable = (FIsCurrentRowStateAdded && !blnIsInModalMode);
            cmbDetailFromCurrencyCode.Enabled = bEnable;
            cmbDetailToCurrencyCode.Enabled = bEnable && !chkHideOthers.Checked;

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
                txtDetailRateOfExchange.Enabled = (FIsCurrentRowStateAdded || FCanEditCurrentRow);
                btnInvertExchangeRate.Enabled = (FIsCurrentRowStateAdded || FCanEditCurrentRow);
                btnDelete.Enabled = (FIsCurrentRowStateAdded || FCanDeleteCurrentRow);
            }

            //UpdateExchangeRateLabels();
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
            if (FJournalDS.HasMatchingUnpostedRate && (FJournalDS.MatchingRate != ARow.RateOfExchange))
            {
                // change all the relevant entries
                DataView dvJournal = FJournalDS.JournalTable.DefaultView;
                dvJournal.RowFilter = String.Format(CultureInfo.InvariantCulture, JournalRowFilter, ARow.FromCurrencyCode, ARow.ToCurrencyCode,
                    ARow.DateEffectiveFrom.ToString("d", CultureInfo.InvariantCulture), FJournalDS.MatchingRate, "Unposted");

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

            if (FGiftBatchDS.HasMatchingUnpostedRate && (FGiftBatchDS.MatchingRate != ARow.RateOfExchange))
            {
                // change all the relevant entries
                DataView dvGift = FGiftBatchDS.GiftBatchTable.DefaultView;
                dvGift.RowFilter = String.Format(CultureInfo.InvariantCulture, GiftBatchRowFilter, ARow.FromCurrencyCode, ARow.ToCurrencyCode,
                    ARow.DateEffectiveFrom.ToString("d", CultureInfo.InvariantCulture), FGiftBatchDS.MatchingRate);

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

        private void Import(System.Object sender, EventArgs e)
        {
            TImportExchangeRates.ImportCurrencyExRates(FMainDS.ADailyExchangeRate, "Daily");
            FPetraUtilsObject.SetChangedFlag();
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

        private void CheckIfRateHasBeenUsed(string FromCurrency,
            string ToCurrency,
            DateTime FromDate,
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
                tooltipDeleteInfo.Hide(btnInvertExchangeRate);
                return;
            }

            if ((FJournalDS.JournalTable == null) || (FGiftBatchDS.GiftBatchTable == null) || !bCanEditDelete)
            {
                // We have either been called before we have loaded the tables or edit/delete is not allowed anyway
                CanEdit = false;
                CanDelete = false;
                return;       // Assume it has been used
            }

            // Display a tooltip with our findings
            string tipText = String.Empty;

            // We need not only the current row info from the exchange rate table, but also the next row later in time, if it exists
            // Create a special view because we don't know how the grid is sorted
            string filter =
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName() + " = '" + FromCurrency + "' and " +
                ADailyExchangeRateTable.GetToCurrencyCodeDBName() + " = '" + ToCurrency + "'";
            DataView dvDailyRate = new DataView(FMainDS.ADailyExchangeRate, filter, SortByDateDescending, DataViewRowState.CurrentRows);

            // Find our current row and hence the previous one
            DateTime ToDate = DateTime.MaxValue;
            int nThis = FindRowInDataView(dvDailyRate, FromCurrency, ToCurrency, FromDate, FromTime);

            if (nThis > 0)
            {
                ToDate = ((ADailyExchangeRateRow)dvDailyRate[nThis - 1].Row).DateEffectiveFrom;
            }

            // Set up a row filter on the two external tables
            DataView dvGift = FGiftBatchDS.GiftBatchTable.DefaultView;
            dvGift.RowFilter =
                String.Format(CultureInfo.InvariantCulture, GiftBatchRowFilter, FromCurrency, ToCurrency,
                    FromDate.ToString("d", CultureInfo.InvariantCulture), ToDate.ToString("d", CultureInfo.InvariantCulture), rate);
            FGiftBatchDS.HasMatchingUnpostedRate = (dvGift.Count > 0);

            if (FGiftBatchDS.HasMatchingUnpostedRate)
            {
                List <int>listLedgers = new List <int>();
                FGiftBatchDS.MatchingRate = rate;

                for (int i = 0; i < dvGift.Count; i++)
                {
                    int ledgerNum = ((AGiftBatchRow)dvGift[i].Row).LedgerNumber;

                    if (!listLedgers.Contains(ledgerNum))
                    {
                        listLedgers.Add(ledgerNum);
                    }
                }

                if (dvGift.Count == 1)
                {
                    tipText +=
                        String.Format(Catalog.GetString("Used by 1 row dated {0} in {1} in the Gift Batch table"),
                            StringHelper.DateToLocalizedString((DateTime)dvGift[0][AGiftBatchTable.ColumnGlEffectiveDateId]),
                            GetLedgerListText(listLedgers));
                }
                else
                {
                    tipText +=
                        String.Format(Catalog.GetString("Used by {0} row(s) between {1} and {2} in {3} in the Gift Batch table"),
                            dvGift.Count,
                            StringHelper.DateToLocalizedString((DateTime)dvGift[0][AGiftBatchTable.ColumnGlEffectiveDateId]),
                            StringHelper.DateToLocalizedString((DateTime)dvGift[dvGift.Count - 1][AGiftBatchTable.ColumnGlEffectiveDateId]),
                            GetLedgerListText(listLedgers));
                }
            }

            DataView dvJournal = FJournalDS.JournalTable.DefaultView;
            dvJournal.RowFilter =
                String.Format(CultureInfo.InvariantCulture, JournalRowFilterRange, FromCurrency, ToCurrency,
                    FromDate.ToString("d", CultureInfo.InvariantCulture), ToDate.ToString("d", CultureInfo.InvariantCulture), rate, "Unposted");
            FJournalDS.HasMatchingUnpostedRate = (dvJournal.Count > 0);

            if (FJournalDS.HasMatchingUnpostedRate)
            {
                FJournalDS.MatchingRate = rate;

                if (tipText != String.Empty)
                {
                    tipText += Environment.NewLine;
                }

                List <int>listLedgers = new List <int>();
                FGiftBatchDS.MatchingRate = rate;

                for (int i = 0; i < dvGift.Count; i++)
                {
                    int ledgerNum = ((AJournalRow)dvJournal[i].Row).LedgerNumber;

                    if (!listLedgers.Contains(ledgerNum))
                    {
                        listLedgers.Add(ledgerNum);
                    }
                }

                if (dvJournal.Count == 1)
                {
                    tipText +=
                        String.Format(Catalog.GetString("Used by 1 unposted row dated {0} in {1} in the Journal table"),
                            StringHelper.DateToLocalizedString((DateTime)dvJournal[0][AJournalTable.ColumnDateEffectiveId]),
                            GetLedgerListText(listLedgers));
                }
                else
                {
                    tipText += String.Format(Catalog.GetString("Used by {0} unposted rows between {1} and {2} in {3} in the Journal table"),
                        dvJournal.Count,
                        StringHelper.DateToLocalizedString((DateTime)dvJournal[0][AJournalTable.ColumnDateEffectiveId]),
                        StringHelper.DateToLocalizedString((DateTime)dvJournal[dvJournal.Count - 1][AJournalTable.ColumnDateEffectiveId]),
                        GetLedgerListText(listLedgers));
                }
            }

            dvJournal.RowFilter =
                String.Format(CultureInfo.InvariantCulture, JournalRowFilterRange, FromCurrency, ToCurrency,
                    FromDate.ToString("d", CultureInfo.InvariantCulture), ToDate.ToString("d", CultureInfo.InvariantCulture), rate, "Posted");
            bool bHasPostedJournalEntries = (dvJournal.Count > 0);

            if (bHasPostedJournalEntries)
            {
                if (tipText != String.Empty)
                {
                    tipText += Environment.NewLine;
                }

                List <int>listLedgers = new List <int>();
                FGiftBatchDS.MatchingRate = rate;

                for (int i = 0; i < dvGift.Count; i++)
                {
                    int ledgerNum = ((AJournalRow)dvJournal[i].Row).LedgerNumber;

                    if (!listLedgers.Contains(ledgerNum))
                    {
                        listLedgers.Add(ledgerNum);
                    }
                }

                if (dvJournal.Count == 1)
                {
                    tipText +=
                        String.Format(Catalog.GetString("Used by 1 posted row in dated {0} {1} in the Journal table"),
                            StringHelper.DateToLocalizedString((DateTime)dvJournal[0][AJournalTable.ColumnDateEffectiveId]),
                            GetLedgerListText(listLedgers));
                }
                else
                {
                    tipText += String.Format(Catalog.GetString("Used by {0} posted rows between {1} and {2} in {3} in the Journal table"),
                        dvJournal.Count,
                        StringHelper.DateToLocalizedString((DateTime)dvJournal[0][AJournalTable.ColumnDateEffectiveId]),
                        StringHelper.DateToLocalizedString((DateTime)dvJournal[dvJournal.Count - 1][AJournalTable.ColumnDateEffectiveId]),
                        GetLedgerListText(listLedgers));
                }
            }

            if (tipText == String.Empty)
            {
                tooltipDeleteInfo.Hide(btnInvertExchangeRate);
            }
            else
            {
                tooltipDeleteInfo.Show(tipText, btnInvertExchangeRate);
            }

            // return true if the rate has been used
            CanEdit = !bHasPostedJournalEntries;
            CanDelete = !FJournalDS.HasMatchingUnpostedRate && !FGiftBatchDS.HasMatchingUnpostedRate && !bHasPostedJournalEntries;
            return;
        }

        private string GetLedgerListText(List <int>ALedgerList)
        {
            string ledgerText = String.Empty;

            for (int i = 0; i < ALedgerList.Count; i++)
            {
                if (i == 0)
                {
                    ledgerText = (ALedgerList.Count > 1) ? Catalog.GetString("Ledgers") : Catalog.GetString("Ledger");
                }
                else
                {
                    ledgerText += ",";
                }

                ledgerText += " #" + ALedgerList[i].ToString();
            }

            return ledgerText;
        }

        private void FPetraUtilsObject_DataSavingStarted(object Sender, EventArgs e)
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
                dv.RowFilter = String.Format(CultureInfo.InvariantCulture, "{0}='{1}' AND {2}='{3}' AND {4}=#{5}# AND {6}={7} AND {8}={9}",
                    ADailyExchangeRateTable.GetFromCurrencyCodeDBName(),
                    item.FromCurrencyCode,
                    ADailyExchangeRateTable.GetToCurrencyCodeDBName(),
                    item.ToCurrencyCode,
                    ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                    item.DateEffective.ToString("d", CultureInfo.InvariantCulture),
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

        private void FPetraUtilsObject_DataSaved(object Sender, TDataSavedEventArgs e)
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
            DataView dv = FCorporateDS.ACorporateExchangeRate.DefaultView;
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