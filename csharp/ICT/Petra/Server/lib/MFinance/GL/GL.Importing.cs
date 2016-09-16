//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop, alanP
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
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Common.Remoting.Server;

using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MCommon.Data.Access;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;
using Ict.Petra.Server.MFinance.GL.WebConnectors;

using Ict.Petra.Shared;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MCommon.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Account.Validation;
using Ict.Petra.Shared.MFinance.GL.Data;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Import a GL Batch
    /// </summary>
    public class TGLImporting
    {
        String FDelimiter;
        String FDateFormatString;
        CultureInfo FCultureInfoNumberFormat;
        CultureInfo FCultureInfoDate;
        String FImportLine;
        String FNewLine;

        private bool UpdateDailyExchangeRateTable(ADailyExchangeRateTable DailyExchangeTable, string AFromCurrencyCode, string AToCurrencyCode,
            decimal AExchangeRate, DateTime AEffectiveDate)
        {
            string SortByTimeDescending = ADailyExchangeRateTable.GetTimeEffectiveFromDBName() + " DESC";

            string filter = String.Format("{0}='{1}' AND {2}='{3}' AND {4}=#{5}#",
                ADailyExchangeRateTable.GetFromCurrencyCodeDBName(), AFromCurrencyCode,
                ADailyExchangeRateTable.GetToCurrencyCodeDBName(), AToCurrencyCode,
                ADailyExchangeRateTable.GetDateEffectiveFromDBName(),
                AEffectiveDate.ToString("d", CultureInfo.InvariantCulture));

            DataView dvTo = new DataView(DailyExchangeTable, filter, SortByTimeDescending, DataViewRowState.CurrentRows);
            bool foundMatch = false;

            for (int i = 0; i < dvTo.Count; i++)
            {
                if (((ADailyExchangeRateRow)dvTo[i].Row).RateOfExchange == AExchangeRate)
                {
                    foundMatch = true;
                    break;
                }
            }

            if (!foundMatch)
            {
                int newTime = 3600;

                if (dvTo.Count > 0)
                {
                    newTime = ((ADailyExchangeRateRow)dvTo[0].Row).TimeEffectiveFrom + 600;
                }

                ADailyExchangeRateRow newRow = DailyExchangeTable.NewRowTyped();
                newRow.DateEffectiveFrom = AEffectiveDate;
                newRow.FromCurrencyCode = AFromCurrencyCode;
                newRow.ToCurrencyCode = AToCurrencyCode;
                newRow.TimeEffectiveFrom = newTime;
                newRow.RateOfExchange = AExchangeRate;
                DailyExchangeTable.Rows.Add(newRow);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Import GL Batches data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="ARequestParams">Hashtable containing the given params </param>
        /// <param name="AImportString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        public bool ImportGLBatches(
            Hashtable ARequestParams,
            String AImportString,
            out TVerificationResultCollection AMessages
            )
        {
            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing GL Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            TVerificationResultCollection Messages = new TVerificationResultCollection();

            // fix for Mono issue with out parameter: https://bugzilla.xamarin.com/show_bug.cgi?id=28196
            AMessages = Messages;

            GLBatchTDS MainDS = new GLBatchTDS();
            GLSetupTDS SetupDS = new GLSetupTDS();
            SetupDS.CaseSensitive = true;
            StringReader sr = new StringReader(AImportString);

            // Parse the supplied parameters
            FDelimiter = (String)ARequestParams["Delimiter"];
            Int32 LedgerNumber = (Int32)ARequestParams["ALedgerNumber"];
            FDateFormatString = (String)ARequestParams["DateFormatString"];
            String NumberFormat = (String)ARequestParams["NumberFormat"];
            FNewLine = (String)ARequestParams["NewLine"];

            // Set culture from parameters
            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FCultureInfoDate = new CultureInfo("en-GB");
            FCultureInfoDate.DateTimeFormat.ShortDatePattern = FDateFormatString;

            // Initialise our working variables
            Int32 InitialTextLength = AImportString.Length;
            Int32 PercentDone = 10;
            Int32 PreviousPercentDone = 0;
            ABatchRow NewBatch = null;
            AJournalRow NewJournal = null;
            int BatchPeriodNumber = -1;
            int BatchYearNr = -1;
            String ImportMessage = "";
            Int32 RowNumber = 0;
            decimal intlRateFromBase = -1.0m;
            Boolean gotFirstBatch = false;
            Boolean CancelledByUser = false;

            // Create some validation dictionaries
            TValidationControlsDict ValidationControlsDictBatch = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictJournal = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictTransaction = new TValidationControlsDict();

            // This needs to be initialised because we will be calling the method
            TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TFinanceServerLookups.GetCurrentPostingRangeDates;
            TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;

            // Get a new transaction
            TDBTransaction transaction = null;
            Boolean submissionOK = false;
            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref transaction, ref submissionOK,
                delegate
                {
                    try
                    {
                        // Load supplementary tables that we are going to need for validation
                        ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(LedgerNumber, transaction);
                        ACurrencyTable CurrencyTable = ACurrencyAccess.LoadAll(transaction);

                        AAnalysisTypeAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);
                        AFreeformAnalysisAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);
                        AAnalysisAttributeAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);
                        ACostCentreAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);
                        AAccountAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);
                        ALedgerInitFlagAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);
                        ATransactionTypeAccess.LoadViaALedger(SetupDS, LedgerNumber, transaction);

                        if (LedgerTable.Rows.Count == 0)
                        {
                            return;
                        }

                        string LedgerBaseCurrency = LedgerTable[0].BaseCurrency;
                        string LedgerIntlCurrency = LedgerTable[0].IntlCurrency;
                        ACorporateExchangeRateTable CorporateExchangeRateTable =
                            ACorporateExchangeRateAccess.LoadViaACurrencyToCurrencyCode(LedgerIntlCurrency,
                                transaction);
                        ADailyExchangeRateTable DailyExchangeRateTable = ADailyExchangeRateAccess.LoadAll(transaction);

                        // Go round a loop reading the file line by line
                        ImportMessage = Catalog.GetString("Parsing first line");
                        FImportLine = sr.ReadLine();

                        Int32 TextProcessedLength = 0;

                        while (FImportLine != null)
                        {
                            RowNumber++;

                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            PercentDone = 10 + ((TextProcessedLength * 90) / InitialTextLength);

                            // skip empty lines and commented lines
                            if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                            {
                                int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count;
                                int preParseMessageCount = Messages.Count;

                                // Read the row analysisType - there is no 'validation' on this so we can make the call with null parameters
                                string RowType =
                                    TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("row type"), null, RowNumber, Messages,
                                        null).Trim();

                                if (RowType == "")  // don't object if there are "empty" lines
                                {
                                    // Skip to the next line
                                    FImportLine = sr.ReadLine();
                                    continue;
                                }

                                if (RowType == "B")
                                {
                                    ImportMessage = Catalog.GetString("Parsing a batch row");

                                    if (numberOfElements < 4)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString("Wrong number of batch columns.  Expected 4 columns."),
                                                TResultSeverity.Resv_Critical));

                                        FImportLine = sr.ReadLine();

                                        if (FImportLine != null)
                                        {
                                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                        }

                                        continue;
                                    }

                                    if (NewBatch != null)   // update the totals of the batch that has just been imported
                                    {
                                        GLRoutines.UpdateBatchTotals(ref MainDS, ref NewBatch);

                                        if (TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                                        {
                                            // Save the current batch and its components
                                            ImportMessage = Catalog.GetString("Saving batch");
                                            ABatchAccess.SubmitChanges(MainDS.ABatch, transaction);
                                            MainDS.ABatch.AcceptChanges();

                                            ImportMessage = Catalog.GetString("Saving gift");
                                            AJournalAccess.SubmitChanges(MainDS.AJournal, transaction);
                                            MainDS.AJournal.AcceptChanges();

                                            ImportMessage = Catalog.GetString("Saving giftdetails");
                                            ATransactionAccess.SubmitChanges(MainDS.ATransaction, transaction);
                                            MainDS.ATransaction.AcceptChanges();
                                        }
                                    }

                                    ImportMessage = Catalog.GetString("Starting new batch");
                                    gotFirstBatch = true;
                                    NewBatch = MainDS.ABatch.NewRowTyped(true);
                                    NewBatch.LedgerNumber = LedgerNumber;
                                    LedgerTable[0].LastBatchNumber++;
                                    NewBatch.BatchNumber = LedgerTable[0].LastBatchNumber;
                                    NewBatch.BatchPeriod = LedgerTable[0].CurrentPeriod;
                                    MainDS.ABatch.Rows.Add(NewBatch);
                                    NewJournal = null;
                                    intlRateFromBase = -1.0m;

                                    NewBatch.BatchDescription =
                                        TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Batch description"),
                                            MainDS.ABatch.ColumnBatchDescription, RowNumber, Messages, ValidationControlsDictBatch);

                                    NewBatch.BatchControlTotal =
                                        TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat,
                                            Catalog.GetString("Batch hash value"),
                                            MainDS.ABatch.ColumnBatchControlTotal, RowNumber, Messages, ValidationControlsDictBatch);
                                    NewBatch.DateEffective =
                                        TCommonImport.ImportDate(ref FImportLine, FDelimiter, FCultureInfoDate,
                                            Catalog.GetString("Batch effective date"),
                                            MainDS.ABatch.ColumnDateEffective, RowNumber, Messages, ValidationControlsDictBatch);

                                    if (Messages.Count == preParseMessageCount)
                                    {
                                        if (TFinancialYear.IsValidPostingPeriod(LedgerNumber,
                                                NewBatch.DateEffective,
                                                out BatchPeriodNumber,
                                                out BatchYearNr,
                                                transaction))
                                        {
                                            NewBatch.BatchYear = BatchYearNr;
                                            NewBatch.BatchPeriod = BatchPeriodNumber;
                                        }
                                        else
                                        {
                                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine,
                                                        RowNumber),
                                                    String.Format(Catalog.GetString(
                                                            "The effective date [{0}] of the imported batch is not in an open period."),
                                                        StringHelper.DateToLocalizedString(NewBatch.DateEffective)), TResultSeverity.Resv_Critical));
                                        }

                                        int messageCountBeforeValidate = Messages.Count;

                                        // Validate using the standard validation
                                        ImportMessage = Catalog.GetString("Validating the batch data");
                                        ABatchValidation.Validate(this, NewBatch, ref Messages, ValidationControlsDictBatch);

                                        // Now do the additional manual validation
                                        ImportMessage = Catalog.GetString("Additional validation of the batch data");
                                        TSharedFinanceValidation_GL.ValidateGLBatchManual(this, NewBatch, ref Messages, ValidationControlsDictBatch);

                                        for (int i = messageCountBeforeValidate; i < Messages.Count; i++)
                                        {
                                            ((TVerificationResult)Messages[i]).OverrideResultContext(String.Format(MCommonConstants.
                                                    StrValidationErrorInLine,
                                                    RowNumber));

                                            if (Messages[i] is TScreenVerificationResult)
                                            {
                                                TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)Messages[i]);
                                                Messages.RemoveAt(i);
                                                Messages.Insert(i, downgrade);
                                            }
                                        }

                                        if ((NewBatch.BatchDescription == null)   // raise error if empty batch description is imported
                                            || (NewBatch.BatchDescription == ""))
                                        {
                                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine,
                                                        RowNumber),
                                                    Catalog.GetString("The batch description must not be empty."), TResultSeverity.Resv_Critical));
                                        }
                                    }
                                }
                                else if (RowType == "J")
                                {
                                    ImportMessage = Catalog.GetString("Parsing a journal row");

                                    if (numberOfElements < 7)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString("Wrong number of journal columns.  Expected 7 columns."),
                                                TResultSeverity.Resv_Critical));

                                        FImportLine = sr.ReadLine();

                                        if (FImportLine != null)
                                        {
                                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                        }

                                        continue;
                                    }

                                    if (NewBatch == null)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString(
                                                    "Expected a Batch line, but found a Journal. Will create a dummy working batch for the current period."),
                                                TResultSeverity.Resv_Critical));

                                        // in order to carry on we will make a dummy batch and force the date to fit
                                        NewBatch = MainDS.ABatch.NewRowTyped(true);
                                        NewBatch.LedgerNumber = LedgerNumber;
                                        LedgerTable[0].LastBatchNumber++;
                                        NewBatch.BatchNumber = LedgerTable[0].LastBatchNumber;
                                        NewBatch.BatchPeriod = LedgerTable[0].CurrentPeriod;
                                        MainDS.ABatch.Rows.Add(NewBatch);
                                    }

                                    NewJournal = MainDS.AJournal.NewRowTyped(true);
                                    NewJournal.LedgerNumber = NewBatch.LedgerNumber;
                                    NewJournal.BatchNumber = NewBatch.BatchNumber;
                                    NewJournal.JournalNumber = NewBatch.LastJournal + 1;
                                    NewJournal.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GL;
                                    NewJournal.TransactionTypeCode = MFinanceConstants.STANDARD_JOURNAL;
                                    NewJournal.TransactionCurrency = LedgerBaseCurrency;
                                    NewJournal.ExchangeRateToBase = 1;
                                    NewJournal.DateEffective = NewBatch.DateEffective;
                                    NewJournal.JournalPeriod = NewBatch.BatchPeriod;
                                    intlRateFromBase = -1.0m;
                                    NewBatch.LastJournal++;

                                    MainDS.AJournal.Rows.Add(NewJournal);

                                    NewJournal.JournalDescription =
                                        TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Journal description"),
                                            MainDS.AJournal.ColumnJournalDescription, RowNumber, Messages, ValidationControlsDictJournal);

                                    NewJournal.SubSystemCode =
                                        TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Journal sub system code"),
                                            MainDS.AJournal.ColumnSubSystemCode, RowNumber, Messages, ValidationControlsDictJournal).ToUpper();
                                    NewJournal.TransactionTypeCode =
                                        TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Journal transaction type"),
                                            MainDS.AJournal.ColumnTransactionTypeCode, RowNumber, Messages, ValidationControlsDictJournal).ToUpper();
                                    NewJournal.TransactionCurrency =
                                        TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Journal transaction currency"),
                                            MainDS.AJournal.ColumnTransactionCurrency, RowNumber, Messages, ValidationControlsDictJournal).ToUpper();
                                    NewJournal.ExchangeRateToBase =
                                        TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat,
                                            Catalog.GetString("Journal exchange rate"),
                                            MainDS.AJournal.ColumnExchangeRateToBase, RowNumber, Messages, ValidationControlsDictJournal);
                                    NewJournal.DateEffective =
                                        TCommonImport.ImportDate(ref FImportLine, FDelimiter, FCultureInfoDate,
                                            Catalog.GetString("Journal effective date"),
                                            MainDS.AJournal.ColumnDateEffective, RowNumber, Messages, ValidationControlsDictJournal);

                                    if (Messages.Count == preParseMessageCount)
                                    {
                                        int messageCountBeforeValidate = Messages.Count;

                                        // Validate using the standard validation
                                        ImportMessage = Catalog.GetString("Validating the journal data");
                                        AJournalValidation.Validate(this, NewJournal, ref Messages, ValidationControlsDictJournal);

                                        // Now do the additional manual validation
                                        ImportMessage = Catalog.GetString("Additional validation of the journal data");
                                        TSharedFinanceValidation_GL.ValidateGLJournalManual(this, NewJournal, ref Messages,
                                            ValidationControlsDictJournal, SetupDS, CurrencyTable,
                                            CorporateExchangeRateTable, LedgerBaseCurrency, LedgerIntlCurrency);

                                        for (int i = messageCountBeforeValidate; i < Messages.Count; i++)
                                        {
                                            ((TVerificationResult)Messages[i]).OverrideResultContext(String.Format(MCommonConstants.
                                                    StrValidationErrorInLine,
                                                    RowNumber));

                                            if (Messages[i] is TScreenVerificationResult)
                                            {
                                                TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)Messages[i]);
                                                Messages.RemoveAt(i);
                                                Messages.Insert(i, downgrade);
                                            }
                                        }

                                        // If this batch is in my base currency,
                                        // the ExchangeRateToBase must be 1:
                                        if ((NewJournal.TransactionCurrency == LedgerBaseCurrency)
                                            && (NewJournal.ExchangeRateToBase != 1.0m))
                                        {
                                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine,
                                                        RowNumber),
                                                    Catalog.GetString("Journal in base currency must have exchange rate of 1.00."),
                                                    TResultSeverity.Resv_Critical));
                                        }

                                        //
                                        // The DateEffective might be different to that of the Batch,
                                        // but it must be in the same accounting period.
                                        Int32 journalYear;
                                        Int32 journalPeriod;
                                        DateTime journalDate = NewJournal.DateEffective;

                                        TFinancialYear.GetLedgerDatePostingPeriod(LedgerNumber, ref journalDate,
                                            out journalYear, out journalPeriod, transaction, false);

                                        if ((journalYear != BatchYearNr) || (journalPeriod != BatchPeriodNumber))
                                        {
                                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine,
                                                        RowNumber),
                                                    String.Format(Catalog.GetString(
                                                            "The journal effective date {0} is not in the same period as the batch date {1}."),
                                                        journalDate.ToShortDateString(), NewBatch.DateEffective.ToShortDateString()),
                                                    TResultSeverity.Resv_Critical));
                                        }

                                        if (TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                                        {
                                            // Get a Corporate Exchange Rate for international currency
                                            // Validation will have ensured that we have a corporate rate for intl currency
                                            // at least for the first day of the accounting period.
                                            // (There may possibly be others between then and the effective date)
                                            DateTime firstDayOfMonth;

                                            if (TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriod(LedgerNumber, NewJournal.DateEffective,
                                                    out firstDayOfMonth))
                                            {
                                                intlRateFromBase =
                                                    TExchangeRateTools.GetCorporateExchangeRate(LedgerBaseCurrency, LedgerIntlCurrency,
                                                        firstDayOfMonth,
                                                        NewJournal.DateEffective);

                                                if (intlRateFromBase <= 0.0m)
                                                {
                                                    // This should never happen (see above)
                                                    Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine,
                                                                RowNumber),
                                                            String.Format(
                                                                "There is no Corporate Exchange Rate for {0} to {1} applicable to the period {2} to {3}.  Please set up an appropriate rate and then import the data again.",
                                                                LedgerBaseCurrency,
                                                                LedgerIntlCurrency,
                                                                StringHelper.DateToLocalizedString(firstDayOfMonth),
                                                                StringHelper.DateToLocalizedString(NewJournal.DateEffective)),
                                                            TResultSeverity.Resv_Critical));
                                                }
                                            }
                                        }

                                        if (TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                                        {
                                            // This row passes validation so we can do final actions if the batch is not in the ledger currency
                                            if (NewJournal.TransactionCurrency != LedgerBaseCurrency)
                                            {
                                                // we need to create a daily exchange rate pair for the transaction date
                                                // start with To Ledger currency
                                                if (UpdateDailyExchangeRateTable(DailyExchangeRateTable, NewJournal.TransactionCurrency,
                                                        LedgerBaseCurrency,
                                                        NewJournal.ExchangeRateToBase, NewJournal.DateEffective))
                                                {
                                                    Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportInformationForLine,
                                                                RowNumber),
                                                            String.Format(Catalog.GetString(
                                                                    "An exchange rate of {0} for '{1}' to '{2}' on {3} will be added to the Daily Exchange Rate table after a successful import."),
                                                                NewJournal.ExchangeRateToBase,
                                                                NewJournal.TransactionCurrency,
                                                                LedgerBaseCurrency,
                                                                StringHelper.DateToLocalizedString(NewJournal.DateEffective)),
                                                            TResultSeverity.Resv_Info));
                                                }

                                                // Now the inverse for From Ledger currency
                                                decimal inverseRate = Math.Round(1 / NewJournal.ExchangeRateToBase, 10);

                                                UpdateDailyExchangeRateTable(DailyExchangeRateTable, LedgerBaseCurrency,
                                                    NewJournal.TransactionCurrency,
                                                    inverseRate, NewJournal.DateEffective);
                                            }
                                        }
                                    }
                                }
                                else if (RowType == "T")
                                {
                                    ImportMessage = Catalog.GetString("Parsing a transaction row");
                                    bool skipThisLine = false;

                                    if (numberOfElements < 8)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString("Wrong number of transaction columns.  Expected at least 8 columns."),
                                                TResultSeverity.Resv_Critical));
                                        skipThisLine = true;
                                    }

                                    if (NewJournal == null)
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                Catalog.GetString("Expected a Journal row but found a Transaction row."),
                                                TResultSeverity.Resv_Critical));
                                        skipThisLine = true;
                                    }

                                    if (skipThisLine)
                                    {
                                        FImportLine = sr.ReadLine();

                                        if (FImportLine != null)
                                        {
                                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                        }

                                        continue;
                                    }

                                    ImportGLTransactionsInner(LedgerNumber, RowNumber, ref MainDS, ref SetupDS, ref NewBatch, ref NewJournal,
                                        intlRateFromBase, ref transaction, ref ImportMessage, ref Messages,
                                        ref ValidationControlsDictTransaction);
                                }
                                else
                                {
                                    if ((NewBatch == null) && !gotFirstBatch)
                                    {
                                        string msg = Catalog.GetString(
                                            "Expecting a Row Type definition. Valid types are 'B', 'J' or 'T'. Maybe you are opening a 'Transactions' file.");
                                        msg +=
                                            Catalog.GetString(
                                                "  You need to be on the 'Transactions' Tab to import transaction-only data into an existing batch.");
                                        msg += Catalog.GetString("  Alternatively you may have selected the wrong Field Delimiter.");
                                        msg += Catalog.GetString("  Choose a delimiter that shows multiple columns in the preview window.");
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                msg,
                                                TResultSeverity.Resv_Critical));
                                        break;
                                    }
                                    else
                                    {
                                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                                String.Format(Catalog.GetString(
                                                        "'{0}' is not a valid Row Type. Valid types are 'B', 'J' or 'T'."),
                                                    RowType),
                                                TResultSeverity.Resv_Critical));
                                    }
                                }
                            }  // if the CSV line qualifies

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                CancelledByUser = true;
                                break;
                            }

                            if (Messages.Count > 100)
                            {
                                // This probably means that it is a big file and the user has made the same mistake many times over
                                break;
                            }

                            // Update progress tracker every few percent
                            if ((PercentDone - PreviousPercentDone) > 3)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    String.Format(Catalog.GetString("Importing row {0}"), RowNumber),
                                    (PercentDone > 98) ? 98 : PercentDone);
                                PreviousPercentDone = PercentDone;
                            }

                            // Read the next line
                            FImportLine = sr.ReadLine();

                            if (FImportLine != null)
                            {
                                TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            }
                        }  // while CSV lines

                        if (CancelledByUser)
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    "The import was cancelled by the user.", TResultSeverity.Resv_Info));
                            return;
                        }

                        // Finished reading the file - did we have critical errors?
                        if (!TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Batch has critical errors"),
                                0);

                            // Record error count
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));

                            if (FImportLine == null)
                            {
                                // We did reach the end of the file
                                Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                        Catalog.GetString(
                                            "Reached the end of file but errors occurred. When these errors are fixed the batch will import successfully."),
                                        TResultSeverity.Resv_Info));
                            }
                            else
                            {
                                // We gave up before the end
                                if (Messages.Count > 100)
                                {
                                    Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                            Catalog.GetString(
                                                "Stopped reading the file after generating more than 100 messages.  The file may contain more errors beyond the ones listed here."),
                                            TResultSeverity.Resv_Info));
                                }
                                else
                                {
                                    Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                            Catalog.GetString(
                                                "Stopped reading the file. Please check you are using a GL Batch Import file and have chosen the correct Field Delimiter."),
                                            TResultSeverity.Resv_Info));
                                }
                            }

                            // Do the 'finally' actions and return false
                            return;
                        }

                        // Everything is ok, so we can do our finish actions

                        // Save all pending changes (last xxx number is updated)
                        ImportMessage = Catalog.GetString("Saving daily exchange rate data");
                        ADailyExchangeRateAccess.SubmitChanges(DailyExchangeRateTable, transaction);
                        DailyExchangeRateTable.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving changes to Ledger table");
                        ALedgerAccess.SubmitChanges(LedgerTable, transaction);
                        LedgerTable.AcceptChanges();

                        // Update totals of final batch
                        ImportMessage = Catalog.GetString("Saving changes to batch totals");

                        if (NewBatch != null)
                        {
                            GLRoutines.UpdateBatchTotals(ref MainDS, ref NewBatch);
                        }

                        ImportMessage = Catalog.GetString("Saving changes to batch");
                        ABatchAccess.SubmitChanges(MainDS.ABatch, transaction);
                        MainDS.ABatch.AcceptChanges();
                        ImportMessage = Catalog.GetString("Saving changes to journal");
                        AJournalAccess.SubmitChanges(MainDS.AJournal, transaction);
                        MainDS.AJournal.AcceptChanges();
                        ImportMessage = Catalog.GetString("Saving changes to transactions");
                        ATransactionAccess.SubmitChanges(MainDS.ATransaction, transaction);
                        MainDS.ATransaction.AcceptChanges();

                        // Now we are done!!!
                        submissionOK = true;
                    } // try
                    catch (Exception ex)
                    {
                        // Parse the exception text for possible references to database foreign keys
                        // Make the message more friendly in that case
                        string friendlyExceptionText = MakeFriendlyFKExceptions(ex);

                        if (RowNumber > 0)
                        {
                            // At least we made a start
                            string msg = ImportMessage;

                            if (friendlyExceptionText.Length > 0)
                            {
                                msg += FNewLine + friendlyExceptionText;
                            }

                            if (ImportMessage.StartsWith(Catalog.GetString("Saving ")))
                            {
                                // Do not display any specific line number because these errors occur outside the parsing loop
                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileSavingBatch,
                                            NewBatch.BatchDescription),
                                        msg, TResultSeverity.Resv_Critical));
                            }
                            else
                            {
                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                        msg, TResultSeverity.Resv_Critical));
                            }
                        }
                        else
                        {
                            // We got an exception before we even started parsing the rows (getting a transaction?)
                            Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                    friendlyExceptionText, TResultSeverity.Resv_Critical));
                        }

                        TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                            Catalog.GetString("Exception Occurred"),
                            0);
                    } // catch
                    finally
                    {
                        sr.Close();

                        if (submissionOK)
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(), Catalog.GetString("Gift batch import successful"),
                                100);
                        }
                        else
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    Catalog.GetString("None of the data from the import was saved."),
                                    TResultSeverity.Resv_Critical));

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Data could not be saved."),
                                0);
                        }

                        TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
                    } // end of 'finally'
                }); // Begin Auto Transaction


            // Set our 'out' parameters
            AMessages = Messages;

            return submissionOK;
        } // Import GL Batches

        /// <summary>
        /// Wrapper for importing GL Transactions. Called from client side
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <param name="AMessages"></param>
        /// <returns></returns>
        public bool ImportGLTransactions(
            Hashtable ARequestParams,
            String AImportString,
            Int32 ALedgerNumber,
            Int32 ABatchNumber,
            Int32 AJournalNumber,
            out TVerificationResultCollection AMessages)
        {
            string ImportMessage = Catalog.GetString("Initialising");

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing GL Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            TVerificationResultCollection Messages = new TVerificationResultCollection();

            // fix for Mono issue with out parameter: https://bugzilla.xamarin.com/show_bug.cgi?id=28196
            AMessages = Messages;

            GLSetupTDS SetupDS = new GLSetupTDS();
            SetupDS.CaseSensitive = true;
            StringReader sr = new StringReader(AImportString);

            FDelimiter = (String)ARequestParams["Delimiter"];
            Int32 LedgerNumber = (Int32)ARequestParams["ALedgerNumber"];
            FDateFormatString = (String)ARequestParams["DateFormatString"];
            String NumberFormat = (String)ARequestParams["NumberFormat"];
            FNewLine = (String)ARequestParams["NewLine"];

            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FCultureInfoDate = new CultureInfo("en-GB");
            FCultureInfoDate.DateTimeFormat.ShortDatePattern = FDateFormatString;

            TDBTransaction Transaction = null;
            Int32 RowNumber = 0;
            Int32 InitialTextLength = AImportString.Length;
            Int32 TextProcessedLength = 0;
            Int32 PercentDone = 10;
            Int32 PreviousPercentDone = 0;
            bool submissionOK = false;
            Boolean CancelledByUser = false;

            // Create some validation dictionaries
            TValidationControlsDict ValidationControlsDictTransaction = new TValidationControlsDict();

            try
            {
                // This needs to be initialised because we will be calling the method
                TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TFinanceServerLookups.GetCurrentPostingRangeDates;
                TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;

                DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.Serializable, ref Transaction, ref submissionOK,
                    delegate
                    {
                        // Construct our DataSet - we use all the journals for the batch so we can update the batch totals.
                        GLBatchTDS MainDS = new GLBatchTDS();
                        ABatchTable BatchTable = ABatchAccess.LoadByPrimaryKey(ALedgerNumber, ABatchNumber, Transaction);
                        MainDS.ABatch.Merge(BatchTable);
                        AJournalTable JournalTable = AJournalAccess.LoadViaABatch(ALedgerNumber, ABatchNumber, Transaction);
                        MainDS.AJournal.Merge(JournalTable);
                        ATransactionTable TransactionTable = ATransactionAccess.LoadViaABatch(ALedgerNumber, ABatchNumber, Transaction);
                        MainDS.ATransaction.Merge(TransactionTable);
                        ATransAnalAttribTable TransAnalAttributeTable = ATransAnalAttribAccess.LoadViaAJournal(ALedgerNumber,
                            ABatchNumber,
                            AJournalNumber,
                            Transaction);
                        MainDS.ATransAnalAttrib.Merge(TransAnalAttributeTable);
                        MainDS.AcceptChanges();

                        ABatchRow NewBatchRow = (ABatchRow)MainDS.ABatch.Rows.Find(new object[] { ALedgerNumber, ABatchNumber });
                        AJournalRow NewJournalRow = (AJournalRow)MainDS.AJournal.Rows.Find(new object[] { ALedgerNumber, ABatchNumber, AJournalNumber });

                        // Load supplementary tables that we are going to need for validation
                        ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                        AAnalysisTypeAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                        AFreeformAnalysisAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                        AAnalysisAttributeAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                        ACostCentreAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                        AAccountAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                        ALedgerInitFlagAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);

                        DateTime firstDayOfMonth;
                        decimal intlRateFromBase = -1.0m;

                        if (LedgerTable.Rows.Count > 0)
                        {
                            string intlCurrency = LedgerTable[0].IntlCurrency;
                            string baseCurrency = LedgerTable[0].BaseCurrency;

                            if (TSharedFinanceValidationHelper.GetFirstDayOfAccountingPeriod(LedgerNumber, NewJournalRow.DateEffective,
                                    out firstDayOfMonth))
                            {
                                intlRateFromBase =
                                    TExchangeRateTools.GetCorporateExchangeRate(baseCurrency, intlCurrency, firstDayOfMonth,
                                        NewJournalRow.DateEffective);
                            }

                            if (intlRateFromBase <= 0.0m)
                            {
                                Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, 1),
                                        String.Format(
                                            "There is no Corporate Exchange Rate for {0} to {1} applicable to the period {2} to {3}.  Please set up an appropriate rate and then import the data again.",
                                            baseCurrency,
                                            intlCurrency,
                                            StringHelper.DateToLocalizedString(firstDayOfMonth),
                                            StringHelper.DateToLocalizedString(NewJournalRow.DateEffective)),
                                        TResultSeverity.Resv_Critical));
                            }
                        }

                        ImportMessage = Catalog.GetString("Parsing first line");

                        // Go round a loop reading the file line by line
                        FImportLine = sr.ReadLine();

                        while (FImportLine != null)
                        {
                            RowNumber++;

                            TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            PercentDone = 10 + ((TextProcessedLength * 90) / InitialTextLength);

                            // skip empty lines and commented lines
                            if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                            {
                                int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count + 1;

                                if (numberOfElements < 8)
                                {
                                    Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                            Catalog.GetString("Wrong number of transaction columns.  Expected at least 8 columns."),
                                            TResultSeverity.Resv_Critical));

                                    FImportLine = sr.ReadLine();

                                    if (FImportLine != null)
                                    {
                                        TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                                    }

                                    continue;
                                }

                                ImportGLTransactionsInner(LedgerNumber, RowNumber, ref MainDS, ref SetupDS, ref NewBatchRow, ref NewJournalRow,
                                    intlRateFromBase, ref Transaction, ref ImportMessage, ref Messages,
                                    ref ValidationControlsDictTransaction);
                            }  // if the CSV line qualifies

                            if (TProgressTracker.GetCurrentState(DomainManager.GClientID.ToString()).CancelJob == true)
                            {
                                CancelledByUser = true;
                                break;
                            }

                            if (Messages.Count > 100)
                            {
                                // This probably means that it is a big file and the user has made the same mistake many times over
                                break;
                            }

                            // Update progress tracker every few percent
                            if ((PercentDone - PreviousPercentDone) > 3)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    String.Format(Catalog.GetString("Importing row {0}"), RowNumber),
                                    (PercentDone > 98) ? 98 : PercentDone);
                                PreviousPercentDone = PercentDone;
                            }

                            // Read the next line
                            FImportLine = sr.ReadLine();

                            if (FImportLine != null)
                            {
                                TextProcessedLength += (FImportLine.Length + FNewLine.Length);
                            }
                        }  // while CSV lines

                        if (CancelledByUser)
                        {
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    String.Format(Catalog.GetString("{0} messages reported."), Messages.Count), TResultSeverity.Resv_Info));
                            Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                    "The import was cancelled by the user.", TResultSeverity.Resv_Info));
                            return;
                        }

                        // Finished reading the file - did we have critical errors?
                        if (!TVerificationHelper.IsNullOrOnlyNonCritical(Messages))
                        {
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                Catalog.GetString("Batch has critical errors"),
                                0);

                            if (FImportLine == null)
                            {
                                // We did reach the end of the file
                                Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                        Catalog.GetString(
                                            "Reached the end of file but errors occurred. When these errors are fixed the batch will import successfully."),
                                        TResultSeverity.Resv_Info));
                            }
                            else
                            {
                                // We gave up before the end
                                Messages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                                        Catalog.GetString(
                                            "Stopped reading the file after generating more than 100 messages.  The file may contian more errors beyond the ones listed here."),
                                        TResultSeverity.Resv_Info));
                            }

                            // Do finally actions and then return false
                            return;
                        }

                        // Everything is ok, so we can do our finish actions


                        //Finally submit all our changes
                        ImportMessage = Catalog.GetString("Saving transactions");
                        ATransactionAccess.SubmitChanges(MainDS.ATransaction, Transaction);
                        MainDS.ATransaction.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving analysis attributes");
                        ATransAnalAttribAccess.SubmitChanges(MainDS.ATransAnalAttrib, Transaction);
                        MainDS.ATransAnalAttrib.AcceptChanges();

                        // update the totals of the batch that has just been imported
                        ImportMessage = Catalog.GetString("Saving changes to totals");
                        GLRoutines.UpdateBatchTotals(ref MainDS, ref NewBatchRow);

                        ImportMessage = Catalog.GetString("Saving journal totals");
                        AJournalAccess.SubmitChanges(MainDS.AJournal, Transaction);
                        MainDS.AJournal.AcceptChanges();

                        ImportMessage = Catalog.GetString("Saving batch totals");
                        ABatchAccess.SubmitChanges(MainDS.ABatch, Transaction);
                        MainDS.ABatch.AcceptChanges();

                        // Now we are done!!!
                        submissionOK = true;
                    }); // Begin Auto Transaction
            } // try
            catch (Exception ex)
            {
                // Parse the exception text for possible references to database foreign keys
                // Make the message more friendly in that case
                String friendlyExceptionText = MakeFriendlyFKExceptions(ex, "T");

                if (RowNumber > 0)
                {
                    // At least we made a start
                    string msg = ImportMessage;

                    if (friendlyExceptionText.Length > 0)
                    {
                        msg += FNewLine + friendlyExceptionText;
                    }

                    if (ImportMessage.StartsWith(Catalog.GetString("Saving ")))
                    {
                        // Do not display any specific line number because these errors occur outside the parsing loop
                        Messages.Add(new TVerificationResult(MCommonConstants.StrExceptionWhileSavingTransactions,
                                msg, TResultSeverity.Resv_Critical));
                    }
                    else
                    {
                        Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                                msg, TResultSeverity.Resv_Critical));
                    }
                }
                else
                {
                    // We got an exception before we even started parsing the rows (getting a transaction?)
                    Messages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                            friendlyExceptionText, TResultSeverity.Resv_Critical));
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Exception Occurred"),
                    0);
            }
            finally
            {
                sr.Close();

                if (submissionOK)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Gift batch import successful"),
                        100);
                }
                else
                {
                    Messages.Add(new TVerificationResult("Import information",
                            Catalog.GetString("None of the data from the import was saved."),
                            TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Data could not be saved."),
                        0);
                }

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            } // end of 'finally'

            // Set our 'out' parameters
            AMessages = Messages;

            return submissionOK;
        } // Import GL Transactions

        private void ImportGLTransactionsInner(Int32 ALedgerNumber,
            Int32 ARowNumber,
            ref GLBatchTDS AMainDS,
            ref GLSetupTDS ASetupDS,
            ref ABatchRow ANewBatchRow,
            ref AJournalRow ANewJournalRow,
            decimal AIntlRateFromBase,
            ref TDBTransaction ATransaction,
            ref string AImportMessage,
            ref TVerificationResultCollection AMessages,
            ref TValidationControlsDict AValidationControlsDictTransaction)
        {
            AImportMessage = Catalog.GetString("Parsing a transaction line.");
            string strIgnoreAnalysisTypeAndValue = Catalog.GetString(" The analysis type/value pair will be ignored.");

            GLBatchTDSATransactionRow NewTransaction = AMainDS.ATransaction.NewRowTyped(true);

            NewTransaction.LedgerNumber = ANewJournalRow.LedgerNumber;
            NewTransaction.BatchNumber = ANewJournalRow.BatchNumber;
            NewTransaction.JournalNumber = ANewJournalRow.JournalNumber;
            NewTransaction.TransactionNumber = ++ANewJournalRow.LastTransactionNumber;

            AMainDS.ATransaction.Rows.Add(NewTransaction);

            int preParseMessageCount = AMessages.Count;
            int nonCriticalErrorCount = 0;

            string costCentreCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Cost centre"),
                AMainDS.ATransaction.ColumnCostCentreCode, ARowNumber, AMessages, AValidationControlsDictTransaction).ToUpper();

            string accountCode = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Account code"),
                AMainDS.ATransaction.ColumnAccountCode, ARowNumber, AMessages, AValidationControlsDictTransaction).ToUpper();

            // This might add a non-critical error
            int msgCount = AMessages.Count;
            TCommonImport.FixAccountCodes(ALedgerNumber, ARowNumber, ref accountCode, ASetupDS.AAccount,
                ref costCentreCode, ASetupDS.ACostCentre, AMessages);
            nonCriticalErrorCount = AMessages.Count - msgCount;

            NewTransaction.CostCentreCode = costCentreCode;
            NewTransaction.AccountCode = accountCode;

            NewTransaction.Narrative = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Narrative"),
                AMainDS.ATransaction.ColumnNarrative, ARowNumber, AMessages, AValidationControlsDictTransaction);

            NewTransaction.Reference = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Reference"),
                AMainDS.ATransaction.ColumnReference, ARowNumber, AMessages, AValidationControlsDictTransaction);

            DateTime TransactionDate = TCommonImport.ImportDate(ref FImportLine, FDelimiter, FCultureInfoDate, Catalog.GetString("Transaction date"),
                AMainDS.ATransaction.ColumnTransactionDate, ARowNumber, AMessages, AValidationControlsDictTransaction);

            decimal DebitAmount = TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat, Catalog.GetString("Debit amount"),
                AMainDS.ATransaction.ColumnTransactionAmount, ARowNumber, AMessages, AValidationControlsDictTransaction, "0");
            decimal CreditAmount =
                TCommonImport.ImportDecimal(ref FImportLine, FDelimiter, FCultureInfoNumberFormat, Catalog.GetString("Credit amount"),
                    AMainDS.ATransaction.ColumnTransactionAmount, ARowNumber, AMessages, AValidationControlsDictTransaction, "0");

            // The critical parsing is complete now
            bool hasParsingErrors = (AMessages.Count != (preParseMessageCount + nonCriticalErrorCount));

            for (int i = 0; i < 10; i++)
            {
                String analysisType = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Analysis Type") + "#" + i,
                    AMainDS.ATransAnalAttrib.ColumnAnalysisTypeCode, ARowNumber, AMessages, AValidationControlsDictTransaction).ToUpper();
                String analysisValue = TCommonImport.ImportString(ref FImportLine, FDelimiter, Catalog.GetString("Analysis Value") + "#" + i,
                    AMainDS.ATransAnalAttrib.ColumnAnalysisAttributeValue, ARowNumber, AMessages, AValidationControlsDictTransaction);

                bool gotType = (analysisType != null) && (analysisType.Length > 0);
                bool gotValue = (analysisValue != null) && (analysisValue.Length > 0);

                if (gotType && !gotValue)
                {
                    // All analysis analysisType errors are non-critical
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                            Catalog.GetString("Transaction analysis attributes must have an attribute value.") + strIgnoreAnalysisTypeAndValue,
                            TResultSeverity.Resv_Noncritical));
                }

                if (!hasParsingErrors)
                {
                    // The analysis data is only imported if all corresponding values are there:
                    // Errors are recorded on-the-fly but are marked as non-critical
                    if (gotType && gotValue)
                    {
                        DataRow atrow = ASetupDS.AAnalysisType.Rows.Find(new Object[] { NewTransaction.LedgerNumber, analysisType });
                        DataRow afrow = ASetupDS.AFreeformAnalysis.Rows.Find(new Object[] { NewTransaction.LedgerNumber, analysisType, analysisValue });
                        AAnalysisAttributeRow anrow = (AAnalysisAttributeRow)ASetupDS.AAnalysisAttribute.Rows.Find(
                            new Object[] { NewTransaction.LedgerNumber, analysisType, NewTransaction.AccountCode });

                        bool isActive = (anrow != null) && anrow.Active;

                        if ((atrow != null) && (afrow != null) && isActive)
                        {
                            ATransAnalAttribRow NewTransAnalAttrib = AMainDS.ATransAnalAttrib.NewRowTyped(true);
                            NewTransAnalAttrib.LedgerNumber = NewTransaction.LedgerNumber;
                            NewTransAnalAttrib.BatchNumber = NewTransaction.BatchNumber;
                            NewTransAnalAttrib.JournalNumber = NewTransaction.JournalNumber;
                            NewTransAnalAttrib.TransactionNumber = NewTransaction.TransactionNumber;
                            NewTransAnalAttrib.AnalysisTypeCode = analysisType;
                            NewTransAnalAttrib.AnalysisAttributeValue = analysisValue;
                            NewTransAnalAttrib.AccountCode = NewTransaction.AccountCode;
                            AMainDS.ATransAnalAttrib.Rows.Add(NewTransAnalAttrib);
                        }
                        else
                        {
                            // All analysis analysisType errors are non-critical
                            if (atrow == null)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                                        String.Format(Catalog.GetString("Unknown transaction analysis attribute '{0}'."),
                                            analysisType) + strIgnoreAnalysisTypeAndValue,
                                        TResultSeverity.Resv_Noncritical));
                            }
                            else if (afrow == null)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                                        String.Format(Catalog.GetString("Unknown transaction analysis value '{0}' for type '{1}'."),
                                            analysisValue, analysisType) + strIgnoreAnalysisTypeAndValue,
                                        TResultSeverity.Resv_Noncritical));
                            }
                            else if (!isActive)
                            {
                                if (anrow == null)
                                {
                                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                                            String.Format(Catalog.GetString(
                                                    "Transaction analysis type/value '{0}'/'{1}' is not associated with account '{2}'."),
                                                analysisType, analysisValue, NewTransaction.AccountCode) + strIgnoreAnalysisTypeAndValue,
                                            TResultSeverity.Resv_Noncritical));
                                }
                                else
                                {
                                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                                            String.Format(Catalog.GetString("Transaction analysis type/value '{0}'/'{1}' is no longer active."),
                                                analysisType, analysisValue) + strIgnoreAnalysisTypeAndValue,
                                            TResultSeverity.Resv_Noncritical));
                                }
                            }
                        }
                    }
                }
            }

            if (!hasParsingErrors)
            {
                NewTransaction.TransactionDate = TransactionDate;

                if ((DebitAmount == 0) && (CreditAmount == 0))
                {
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                            Catalog.GetString("Either the debit amount or the credit amount must be greater than 0."), TResultSeverity.Resv_Critical));
                }

                if ((DebitAmount < 0) || (CreditAmount < 0))
                {
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                            Catalog.GetString("Negative amount specified - debits and credits must be positive."), TResultSeverity.Resv_Critical));
                }

                if ((DebitAmount != 0) && (CreditAmount != 0))
                {
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                            Catalog.GetString("Transactions cannot have values for both debit and credit amounts."), TResultSeverity.Resv_Critical));
                }

                if (DebitAmount != 0)
                {
                    NewTransaction.DebitCreditIndicator = true;
                    NewTransaction.TransactionAmount = DebitAmount;
                }
                else
                {
                    NewTransaction.DebitCreditIndicator = false;
                    NewTransaction.TransactionAmount = CreditAmount;
                }

                NewTransaction.AmountInBaseCurrency = GLRoutines.Divide(NewTransaction.TransactionAmount, ANewJournalRow.ExchangeRateToBase);

                // If we know the international currency exchange rate we can set the value for the transaction amount
                if (AIntlRateFromBase > 0.0m)
                {
                    NewTransaction.AmountInIntlCurrency = GLRoutines.Divide(NewTransaction.AmountInBaseCurrency, AIntlRateFromBase, 2);
                }

                // Now we can start validation
                int messageCountBeforeValidate = AMessages.Count;

                // Do our standard gift batch validation checks on this row
                AImportMessage = Catalog.GetString("Validating the transaction data");
                ATransactionValidation.Validate(this, NewTransaction, ref AMessages, AValidationControlsDictTransaction);

                // And do the additional manual ones
                AImportMessage = Catalog.GetString("Additional validation of the transaction data");
                TSharedFinanceValidation_GL.ValidateGLDetailManual(this,
                    ANewBatchRow,
                    NewTransaction,
                    null,
                    ref AMessages,
                    AValidationControlsDictTransaction,
                    ASetupDS.ACostCentre,
                    ASetupDS.AAccount);

                for (int i = messageCountBeforeValidate; i < AMessages.Count; i++)
                {
                    ((TVerificationResult)AMessages[i]).OverrideResultContext(String.Format(MCommonConstants.StrValidationErrorInLine, ARowNumber));

                    if (AMessages[i] is TScreenVerificationResult)
                    {
                        TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)AMessages[i]);
                        AMessages.RemoveAt(i);
                        AMessages.Insert(i, downgrade);
                    }
                }

                if (NewTransaction.TransactionAmount <= 0.0m)
                {
                    // We will have a validation message that will duplicate one we already have and may not really make sense in the context
                    //  of separate credit and debit amounts
                    for (int i = messageCountBeforeValidate; i < AMessages.Count; i++)
                    {
                        TVerificationResult msg = (TVerificationResult)AMessages[i];

                        if (msg.ResultText.Contains(Catalog.GetString("Debit amount")) || msg.ResultText.Contains(Catalog.GetString("Credit amount")))
                        {
                            AMessages.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        private String MakeFriendlyFKExceptions(Exception ex, string AType = "B")
        {
            //note that this is only done for "user errors" not for program errors!
            String innerMessage;

            if (ex.InnerException == null)
            {
                innerMessage = ex.Message;
            }
            else
            {
                innerMessage = ex.InnerException.ToString();
            }

            string formatStr = Catalog.GetString("  Do you need to add this code to the '{0}' main setup screen?");

            if (innerMessage.Contains("a_journal_fk3"))
            {
                return Catalog.GetString("Unknown sub system code or transaction type code.  Please use one of the recognised codes.");
            }

            if (innerMessage.Contains("a_journal_fk4"))
            {
                return Catalog.GetString("Unknown transaction currency code.") + String.Format(formatStr, "Currencies");
            }

            if (innerMessage.Contains("a_journal_fk5"))
            {
                return Catalog.GetString("Unknown base currency code.") + String.Format(formatStr, "Currencies");
            }

            if (innerMessage.Contains("a_transaction_fk4"))
            {
                return Catalog.GetString("Unknown cost centre.") + String.Format(formatStr, "Manage Cost Centres");
            }

            if (innerMessage.Contains("a_transaction_fk3"))
            {
                return Catalog.GetString("Unknown account code.") + String.Format(formatStr, "Manage Accounts");
            }

            if (innerMessage.Contains("a_transaction_fk5"))
            {
                return Catalog.GetString("Unknown key ministry partner key (unit).");
            }

            if (AType == "B")
            {
                TLogging.Log("Importing GL batch: " + ex.ToString());
            }
            else
            {
                TLogging.Log("Importing GL transactions: " + ex.ToString());
            }

            return ex.Message;
        }
    }
}
