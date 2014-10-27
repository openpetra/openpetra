//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop
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
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Exceptions;
using Ict.Common.Verification;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Validation;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Account.Validation;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Server.MFinance.GL.WebConnectors;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.MFinance.Common.ServerLookups.WebConnectors;

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
            Int32 ProgressTrackerCounter = 0;       // Counts transactions per journal

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing GL Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            string ImportMessage = Catalog.GetString("Initialising");
            AMessages = new TVerificationResultCollection();
            GLBatchTDS MainDS = new GLBatchTDS();
            GLSetupTDS SetupDS = new GLSetupTDS();
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
            TDBTransaction Transaction = null;
            Int32 RowNumber = 0;
            ABatchRow NewBatch = null;
            AJournalRow NewJournal = null;
            int BatchPeriodNumber = -1;
            int BatchYearNr = -1;
            string LedgerBaseCurrency;
            bool ok = false;

            // Create some validation dictionaries
            TValidationControlsDict ValidationControlsDictBatch = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictJournal = new TValidationControlsDict();
            TValidationControlsDict ValidationControlsDictTransaction = new TValidationControlsDict();

            try
            {
                // This needs to be initialised because we will be calling the method
                TSharedFinanceValidationHelper.GetValidPostingDateRangeDelegate = @TFinanceServerLookups.GetCurrentPostingRangeDates;
                TSharedFinanceValidationHelper.GetValidPeriodDatesDelegate = @TAccountingPeriodsWebConnector.GetPeriodDates;

                // Get a new transaction
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                // If we did not succeed there is something wrong (a transaction is already dangling somewhere?)
                if (Transaction == null)
                {
                    throw new Exception(Catalog.GetString(
                            "Could not create a new import transaction because an existing transaction has not completed."));
                }

                // Load supplementary tables that we are going to need for validation
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(LedgerNumber, Transaction);
                AAnalysisTypeAccess.LoadAll(SetupDS, Transaction);
                AFreeformAnalysisAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                AAnalysisAttributeAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                ACostCentreAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                AAccountAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);

                if (LedgerTable.Rows.Count == 0)
                {
                    throw new Exception(String.Format(Catalog.GetString("Ledger {0} doesn't exist."), LedgerNumber));
                }

                LedgerBaseCurrency = LedgerTable[0].BaseCurrency;
                ACorporateExchangeRateTable CorporateExchangeTable = ACorporateExchangeRateAccess.LoadViaACurrencyToCurrencyCode(LedgerBaseCurrency, Transaction);
                ADailyExchangeRateTable DailyExchangeToTable = ADailyExchangeRateAccess.LoadViaACurrencyToCurrencyCode(LedgerBaseCurrency, Transaction);

                // Go round a loop reading the file line by line
                ImportMessage = Catalog.GetString("Parsing first line");
                FImportLine = sr.ReadLine();

                while (FImportLine != null)
                {
                    RowNumber++;

                    // skip empty lines and commented lines
                    if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                    {
                        int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count;

                        // Read the row analysisType - there is no 'validation' on this so we can make the call with null parameters
                        string RowType = ImportString(Catalog.GetString("row type"), null, null);

                        if (RowType == "B")
                        {
                            ImportMessage = Catalog.GetString("Parsing a batch row");

                            if (numberOfElements != 4)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                        Catalog.GetString("Wrong number of batch columns.  Expected 4 columns."), TResultSeverity.Resv_Critical));

                                FImportLine = sr.ReadLine();
                                continue;
                            }

                            if (NewBatch != null)   // update the totals of the batch that has just been imported
                            {
                                GLRoutines.UpdateTotalsOfBatch(ref MainDS, NewBatch);
                            }

                            NewBatch = MainDS.ABatch.NewRowTyped(true);
                            NewBatch.LedgerNumber = LedgerNumber;
                            LedgerTable[0].LastBatchNumber++;
                            NewBatch.BatchNumber = LedgerTable[0].LastBatchNumber;
                            NewBatch.BatchPeriod = LedgerTable[0].CurrentPeriod;
                            MainDS.ABatch.Rows.Add(NewBatch);
                            NewJournal = null;

                            NewBatch.BatchDescription = ImportString(Catalog.GetString("Batch description"),
                                MainDS.ABatch.ColumnBatchDescription, ValidationControlsDictBatch);

                            NewBatch.BatchControlTotal = ImportDecimal(Catalog.GetString("Batch hash value"),
                                MainDS.ABatch.ColumnBatchControlTotal, RowNumber, AMessages, ValidationControlsDictBatch);
                            NewBatch.DateEffective = ImportDate(Catalog.GetString("Batch effective date"),
                                MainDS.ABatch.ColumnDateEffective, RowNumber, AMessages, ValidationControlsDictBatch);

                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                string.Format(Catalog.GetString("Batch {0}"), NewBatch.BatchNumber),
                                10);

                            if (TFinancialYear.IsValidPostingPeriod(LedgerNumber,
                                    NewBatch.DateEffective,
                                    out BatchPeriodNumber,
                                    out BatchYearNr,
                                    Transaction))
                            {
                                NewBatch.BatchYear = BatchYearNr;
                                NewBatch.BatchPeriod = BatchPeriodNumber;
                            }
                            else
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine,
                                            RowNumber),
                                        String.Format(Catalog.GetString("The effective date [{0}] of the imported batch is not in an open period."),
                                            StringHelper.DateToLocalizedString(NewBatch.DateEffective)), TResultSeverity.Resv_Critical));
                            }

                            int messageCountBeforeValidate = AMessages.Count;

                            // Validate using the standard validation
                            ImportMessage = Catalog.GetString("Validating the batch data");
                            ABatchValidation.Validate(this, NewBatch, ref AMessages, ValidationControlsDictBatch);

                            // Now do the additional manual validation
                            ImportMessage = Catalog.GetString("Additional validation of the batch data");
                            TSharedFinanceValidation_GL.ValidateGLBatchManual(this, NewBatch, ref AMessages, ValidationControlsDictBatch);

                            for (int i = messageCountBeforeValidate; i < AMessages.Count; i++)
                            {
                                ((TVerificationResult)AMessages[i]).OverrideResultContext(String.Format(MCommonConstants.StrValidationErrorInLine, RowNumber));

                                if (AMessages[i] is TScreenVerificationResult)
                                {
                                    TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)AMessages[i]);
                                    AMessages.RemoveAt(i);
                                    AMessages.Insert(i, downgrade);
                                }
                            }

                            if ((NewBatch.BatchDescription == null)   // raise error if empty batch description is imported
                                || (NewBatch.BatchDescription == ""))
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, RowNumber),
                                        Catalog.GetString("The batch description must not be empty."), TResultSeverity.Resv_Critical));
                            }

                            if (TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
                            {
                                ImportMessage = Catalog.GetString("Saving GL batch:");

                                ABatchAccess.SubmitChanges(MainDS.ABatch, Transaction);
                                MainDS.ABatch.AcceptChanges();
                            }
                        }
                        else if (RowType == "J")
                        {
                            ImportMessage = Catalog.GetString("Parsing a journal row");

                            if (numberOfElements != 7)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                    Catalog.GetString("Wrong number of journal columns.  Expected 7 columns."), TResultSeverity.Resv_Critical));

                                FImportLine = sr.ReadLine();
                                continue;
                            }

                            if (NewBatch == null)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
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
                            NewBatch.LastJournal++;

                            ProgressTrackerCounter = 0;   // counts transactions per journal
                            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                string.Format(Catalog.GetString("Batch {0}, Journal {1}"),
                                    NewBatch.BatchNumber,
                                    NewJournal.JournalNumber),
                                15);

                            MainDS.AJournal.Rows.Add(NewJournal);

                            NewJournal.JournalDescription = ImportString(Catalog.GetString("Journal description"),
                                MainDS.AJournal.ColumnJournalDescription, ValidationControlsDictJournal);

                            NewJournal.SubSystemCode = ImportString(Catalog.GetString("Journal sub system code"),
                                MainDS.AJournal.ColumnSubSystemCode, ValidationControlsDictJournal);
                            NewJournal.TransactionTypeCode = ImportString(Catalog.GetString("Journal transaction type"),
                                MainDS.AJournal.ColumnTransactionCurrency, ValidationControlsDictJournal);
                            NewJournal.TransactionCurrency = ImportString(Catalog.GetString("Journal transaction currency"),
                                MainDS.AJournal.ColumnTransactionCurrency, ValidationControlsDictJournal);
                            NewJournal.ExchangeRateToBase = ImportDecimal(Catalog.GetString("Journal exchange rate"),
                                MainDS.AJournal.ColumnExchangeRateToBase, RowNumber, AMessages, ValidationControlsDictJournal);
                            NewJournal.DateEffective = ImportDate(Catalog.GetString("Journal effective date"),
                                MainDS.AJournal.ColumnDateEffective, RowNumber, AMessages, ValidationControlsDictJournal);

                            int messageCountBeforeValidate = AMessages.Count;

                            // Validate using the standard validation
                            ImportMessage = Catalog.GetString("Validating the journal data");
                            AJournalValidation.Validate(this, NewJournal, ref AMessages, ValidationControlsDictJournal);

                            // Now do the additional manual validation
                            ImportMessage = Catalog.GetString("Additional validation of the journal data");
                            TSharedFinanceValidation_GL.ValidateGLJournalManual(this, NewJournal, ref AMessages, ValidationControlsDictJournal,
                                CorporateExchangeTable, LedgerBaseCurrency);

                            for (int i = messageCountBeforeValidate; i < AMessages.Count; i++)
                            {
                                ((TVerificationResult)AMessages[i]).OverrideResultContext(String.Format(MCommonConstants.StrValidationErrorInLine, RowNumber));

                                if (AMessages[i] is TScreenVerificationResult)
                                {
                                    TVerificationResult downgrade = new TVerificationResult((TScreenVerificationResult)AMessages[i]);
                                    AMessages.RemoveAt(i);
                                    AMessages.Insert(i, downgrade);
                                }
                            }

                            // If this batch is in my base currency,
                            // the ExchangeRateToBase must be 1:
                            if ((NewJournal.TransactionCurrency == LedgerBaseCurrency)
                                && (NewJournal.ExchangeRateToBase != 1.0m))
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, RowNumber),
                                    Catalog.GetString("Journal in base currency must have exchange rate of 1.00."), TResultSeverity.Resv_Critical));
                            }

                            //
                            // The DateEffective might be different to that of the Batch,
                            // but it must be in the same accounting period.
                            Int32 journalYear;
                            Int32 journalPeriod;
                            DateTime journalDate = NewJournal.DateEffective;

                            TFinancialYear.GetLedgerDatePostingPeriod(LedgerNumber, ref journalDate,
                                out journalYear, out journalPeriod, Transaction, false);

                            if ((journalYear != BatchYearNr) || (journalPeriod != BatchPeriodNumber))
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, RowNumber),
                                    String.Format(Catalog.GetString(
                                        "The journal effective date {0} is not in the same period as the batch date {1}."),
                                        journalDate.ToShortDateString(), NewBatch.DateEffective.ToShortDateString()),
                                    TResultSeverity.Resv_Critical));
                            }

                            if (TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
                            {
                                // This row passes validation so we can do final actions if the batch is not in the ledger currency
                                if (NewJournal.TransactionCurrency != LedgerBaseCurrency)
                                {
                                    // Validation will have ensured that we have a corporate rate for the effective date
                                    // We need to know what that rate is...
                                    DateTime firstOfMonth = new DateTime(NewJournal.DateEffective.Year, NewJournal.DateEffective.Month, 1);
                                    ACorporateExchangeRateRow corporateRateRow = (ACorporateExchangeRateRow)CorporateExchangeTable.Rows.Find(
                                        new object[] { NewJournal.TransactionCurrency, LedgerBaseCurrency, firstOfMonth });
                                    decimal corporateRate = corporateRateRow.RateOfExchange;

                                    if (Math.Abs((NewJournal.ExchangeRateToBase - corporateRate) / corporateRate) > 0.20m)
                                    {
                                        AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, RowNumber),
                                            String.Format(Catalog.GetString("The exchange rate of {0} differs from the Corporate Rate of {1} for the month commencing {2} by more than 20 percent."),
                                                NewJournal.ExchangeRateToBase, corporateRate, StringHelper.DateToLocalizedString(firstOfMonth)),
                                            TResultSeverity.Resv_Noncritical));
                                    }

                                    // we need to create a daily exchange rate pair for the transaction date
                                    // start with To Ledger currency
                                    if (UpdateDailyExchangeRateTable(DailyExchangeToTable, NewJournal.TransactionCurrency, LedgerBaseCurrency, NewJournal.ExchangeRateToBase, NewJournal.DateEffective))
                                    {
                                        ADailyExchangeRateAccess.SubmitChanges(DailyExchangeToTable, Transaction);
                                        DailyExchangeToTable.AcceptChanges();

                                        AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportInformationForLine, RowNumber),
                                            String.Format(Catalog.GetString("Added exchange rate of {0} to Daily Exchange Rate table for {1}"),
                                                NewJournal.ExchangeRateToBase, StringHelper.DateToLocalizedString(NewJournal.DateEffective)),
                                            TResultSeverity.Resv_Info));
                                    }

                                    // Now the inverse for From Ledger currency
                                    ADailyExchangeRateTable DailyExchangeFromTable =
                                        ADailyExchangeRateAccess.LoadViaACurrencyFromCurrencyCode(NewJournal.TransactionCurrency, Transaction);
                                    decimal inverseRate = Math.Round(1 / NewJournal.ExchangeRateToBase, 10);

                                    if (UpdateDailyExchangeRateTable(DailyExchangeFromTable, LedgerBaseCurrency, NewJournal.TransactionCurrency,
                                        inverseRate, NewJournal.DateEffective))
                                    {
                                        ADailyExchangeRateAccess.SubmitChanges(DailyExchangeFromTable, Transaction);
                                    }
                                }

                                ImportMessage = Catalog.GetString("Saving the journal:");
                                AJournalAccess.SubmitChanges(MainDS.AJournal, Transaction);
                                MainDS.AJournal.AcceptChanges();
                            }
                        }
                        else if (RowType == "T")
                        {
                            ImportMessage = Catalog.GetString("Parsing a transaction row");

                            if (numberOfElements < 8)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                    Catalog.GetString("Wrong number of transaction columns.  Expected at least 8 columns."),
                                    TResultSeverity.Resv_Critical));

                                FImportLine = sr.ReadLine();
                                continue;
                            }

                            ImportGLTransactionsInner(LedgerNumber, RowNumber, ref MainDS, ref SetupDS, ref NewBatch, ref NewJournal,
                                ref ProgressTrackerCounter, ref Transaction, ref ImportMessage,
                                ref AMessages, ref ValidationControlsDictTransaction);
                        }
                        else
                        {
                            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                Catalog.GetString("Invalid Row Type. Perhaps using wrong CSV separator?"), TResultSeverity.Resv_Critical));
                        }
                    }  // if the CSV line qualifies

                    if (AMessages.Count > 100)
                    {
                        // This probably means that it is a big file and the user has made the same mistake many times over
                        break;
                    }

                    // Read the next line
                    FImportLine = sr.ReadLine();
                }  // while CSV lines

                // Finished reading the file - did we have critical errors?
                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Batch has critical errors"),
                        0);

                    // Record error count
                    AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                        String.Format(Catalog.GetString("{0} messages reported."), AMessages.Count), TResultSeverity.Resv_Info));

                    if (FImportLine == null)
                    {
                        // We did reach the end of the file
                        AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                            Catalog.GetString(
                                "Reached the end of file but errors occurred. When these errors are fixed the batch will import successfully."),
                            TResultSeverity.Resv_Info));
                    }
                    else
                    {
                        // We gave up before the end
                        AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                            Catalog.GetString(
                                "Stopped reading the file after generating more than 100 messages.  The file may contain more errors beyond the ones listed here."),
                            TResultSeverity.Resv_Info));
                    }

                    TLogging.Log("Return from here!");

                    // Do the 'finally' actions and return false
                    return false;
                }

                // Everything is ok, so we can do our finish actions

                ImportMessage = Catalog.GetString("Saving counter fields:");

                //Finally save all pending changes (last xxx number is updated)
                ABatchAccess.SubmitChanges(MainDS.ABatch, Transaction);
                ALedgerAccess.SubmitChanges(LedgerTable, Transaction);
                AJournalAccess.SubmitChanges(MainDS.AJournal, Transaction);

                MainDS.AcceptChanges();

                // Now we are done!!!
                DBAccess.GDBAccessObj.CommitTransaction();
                ok = true;
            }
            catch (Exception ex)
            {
                // Parse the exception text for possible references to database foreign keys
                // Make the message more friendly in that case
                string friendlyExceptionText = MakeFriendlyFKExceptions(ex);

                if (AMessages == null)
                {
                    AMessages = new TVerificationResultCollection();
                }

                if (RowNumber > 0)
                {
                    // At least we made a start
                    string msg = ImportMessage;

                    if (friendlyExceptionText.Length > 0)
                    {
                        msg += FNewLine + friendlyExceptionText;
                    }

                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                        msg, TResultSeverity.Resv_Critical));
                }
                else
                {
                    // We got an exception before we even started parsing the rows (getting a transaction?)
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                        friendlyExceptionText, TResultSeverity.Resv_Critical));
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Exception Occurred"),
                    0);

                ok = false;
            }
            finally
            {
                try
                {
                    sr.Close();
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured while closing the Import File:" + Environment.NewLine + Exc.ToString());

                    if (AMessages == null)
                    {
                        AMessages = new TVerificationResultCollection();
                    }

                    AMessages.Add(new TVerificationResult(Catalog.GetString("Import exception"),
                            Catalog.GetString("A problem was encountered while closing the Import File:"),
                            TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Exception Occurred"),
                        0);

                    TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                    throw;
                }

                if (ok)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Gift batch import successful"),
                        100);
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    if (AMessages == null)
                    {
                        AMessages = new TVerificationResultCollection();
                    }

                    AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                        Catalog.GetString("None of the data from the import was saved."),
                        TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Data could not be saved."),
                        0);
                }

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            } // end of 'finally'

            return ok;
        }

        /// <summary>
        /// Wrapper for importing GL Transactions. Called from client side
        /// </summary>
        /// <param name="ARequestParams"></param>
        /// <param name="AImportString"></param>
        /// <param name="AMainDS"></param>
        /// <param name="AMessages"></param>
        /// <returns></returns>
        public bool ImportGLTransactions(
            Hashtable ARequestParams,
            String AImportString,
            ref GLBatchTDS AMainDS,
            out TVerificationResultCollection AMessages)
        {
            Int32 ProgressTrackerCounter = 0;
            string ImportMessage = Catalog.GetString("Initialising");

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing GL Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            AMessages = new TVerificationResultCollection();
            GLBatchTDS MainDS = (GLBatchTDS)AMainDS.Copy();
            MainDS.Merge(AMainDS.ABatch);
            MainDS.Merge(AMainDS.AJournal);
            GLSetupTDS SetupDS = new GLSetupTDS();
            StringReader sr = new StringReader(AImportString);

            ABatchRow NewBatchRow = (ABatchRow)MainDS.ABatch[0];
            AJournalRow NewJournalRow = (AJournalRow)MainDS.AJournal[0];

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
            bool ok = false;

            // Create some validation dictionaries
            TValidationControlsDict ValidationControlsDictTransaction = new TValidationControlsDict();

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                // If we did not succeed there is something wrong (a transaction is already dangling somewhere?)
                if (Transaction == null)
                {
                    throw new Exception(Catalog.GetString(
                            "Could not create a new import transaction because an existing transaction has not completed."));
                }

                // Load supplementary tables that we are going to need for validation
                AAnalysisTypeAccess.LoadAll(SetupDS, Transaction);
                AFreeformAnalysisAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                AAnalysisAttributeAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                ACostCentreAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                AAccountAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);

                ImportMessage = Catalog.GetString("Parsing first line");

                // Go round a loop reading the file line by line
                FImportLine = sr.ReadLine();

                while (FImportLine != null)
                {
                    RowNumber++;

                    // skip empty lines and commented lines
                    if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                    {
                        int numberOfElements = StringHelper.GetCSVList(FImportLine, FDelimiter).Count;

                        // Read the row analysisType - there is no 'validation' on this so we can make the call with null parameters
                        string RowType = ImportString(Catalog.GetString("row type"), null, null);

                        if (RowType == "T")
                        {
                            if (numberOfElements < 8)
                            {
                                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                    Catalog.GetString("Wrong number of transaction columns.  Expected at least 8 columns."),
                                    TResultSeverity.Resv_Critical));

                                FImportLine = sr.ReadLine();
                                continue;
                            }

                            ImportGLTransactionsInner(LedgerNumber, RowNumber, ref MainDS, ref SetupDS, ref NewBatchRow, ref NewJournalRow,
                                ref ProgressTrackerCounter, ref Transaction, ref ImportMessage, ref AMessages, ref ValidationControlsDictTransaction);
                        }
                        else
                        {
                            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLine, RowNumber),
                                Catalog.GetString("Invalid Row Type. Perhaps using wrong CSV separator?"), TResultSeverity.Resv_Critical));
                        }
                    }  // if the CSV line qualifies

                    if (AMessages.Count > 100)
                    {
                        // This probably means that it is a big file and the user has made the same mistake many times over
                        break;
                    }

                    // Read the next line
                    FImportLine = sr.ReadLine();
                }  // while CSV lines

                // Finished reading the file - did we have critical errors?
                if (!TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Batch has critical errors"),
                        0);

                    if (FImportLine == null)
                    {
                        // We did reach the end of the file
                        AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                            Catalog.GetString(
                                "Reached the end of file but errors occurred. When these errors are fixed the batch will import successfully."),
                            TResultSeverity.Resv_Info));
                    }
                    else
                    {
                        // We gave up before the end
                        AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                            Catalog.GetString(
                                "Stopped reading the file after generating more than 100 messages.  The file may contian more errors beyond the ones listed here."),
                            TResultSeverity.Resv_Info));
                    }

                    // Do finally actions and then return false
                    return false;
                }

                // Everything is ok, so we can do our finish actions

                ImportMessage = Catalog.GetString("Saving counter fields:");

                if (NewBatchRow != null)   // update the totals of the batch that has just been imported
                {
                    //GLRoutines.UpdateTotalsOfBatch(ref MainDS, NewBatchRow);
                }

                //Finally reject and save accordingly
                MainDS.ABatch.RejectChanges();
                MainDS.AJournal.RejectChanges();
                ATransactionAccess.SubmitChanges(MainDS.ATransaction, Transaction);
                ATransAnalAttribAccess.SubmitChanges(MainDS.ATransAnalAttrib, Transaction);

                MainDS.AcceptChanges();

                // Now we are done!!!
                DBAccess.GDBAccessObj.CommitTransaction();
                ok = true;
            }
            catch (Exception ex)
            {
                // Parse the exception text for possible references to database foreign keys
                // Make the message more friendly in that case
                String friendlyExceptionText = MakeFriendlyFKExceptions(ex, "T");

                if (AMessages == null)
                {
                    AMessages = new TVerificationResultCollection();
                }

                if (RowNumber > 0)
                {
                    // At least we made a start
                    string msg = ImportMessage;

                    if (friendlyExceptionText.Length > 0)
                    {
                        msg += FNewLine + friendlyExceptionText;
                    }

                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine, RowNumber),
                        msg, TResultSeverity.Resv_Critical));
                }
                else
                {
                    // We got an exception before we even started parsing the rows (getting a transaction?)
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrExceptionWhileParsingLine,RowNumber),
                        friendlyExceptionText, TResultSeverity.Resv_Critical));
                }

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    Catalog.GetString("Exception Occurred"),
                    0);

                ok = false;
            }
            finally
            {
                try
                {
                    sr.Close();
                }
                catch (Exception Exc)
                {
                    TLogging.Log("An Exception occured while closing the Import File:" + Environment.NewLine + Exc.ToString());

                    if (AMessages == null)
                    {
                        AMessages = new TVerificationResultCollection();
                    }

                    AMessages.Add(new TVerificationResult(MCommonConstants.StrImportInformation,
                        Catalog.GetString("A problem was encountered while closing the Import File:"),
                        TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Exception Occurred"),
                        0);

                    TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                    throw;
                }

                if (ok)
                {
                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Gift batch import successful"),
                        100);
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    if (AMessages == null)
                    {
                        AMessages = new TVerificationResultCollection();
                    }

                    AMessages.Add(new TVerificationResult("Import information",
                            Catalog.GetString("None of the data from the import was saved."),
                            TResultSeverity.Resv_Critical));

                    TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                        Catalog.GetString("Data could not be saved."),
                        0);
                }

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());
            } // end of 'finally'

            return ok;
        }

        private void ImportGLTransactionsInner(Int32 ALedgerNumber, Int32 ARowNumber, ref GLBatchTDS AMainDS, ref GLSetupTDS ASetupDS,
            ref ABatchRow ANewBatchRow, ref AJournalRow ANewJournalRow,
            ref Int32 AProgressTrackerCounter, ref TDBTransaction ATransaction, ref string AImportMessage,
            ref TVerificationResultCollection AMessages, ref TValidationControlsDict AValidationControlsDictTransaction)
        {
            AImportMessage = Catalog.GetString("Parsing a transaction line.");
            string strIgnoreAnalysisTypeAndValue = Catalog.GetString(" The analysis type/value pair will be ignored.");

            GLBatchTDSATransactionRow NewTransaction = AMainDS.ATransaction.NewRowTyped(true);

            NewTransaction.LedgerNumber = ANewJournalRow.LedgerNumber;
            NewTransaction.BatchNumber = ANewJournalRow.BatchNumber;
            NewTransaction.JournalNumber = ANewJournalRow.JournalNumber;
            NewTransaction.TransactionNumber = ++ANewJournalRow.LastTransactionNumber;

            AMainDS.ATransaction.Rows.Add(NewTransaction);

            NewTransaction.CostCentreCode = ImportString(Catalog.GetString("Cost centre"),
                AMainDS.ATransaction.ColumnCostCentreCode, AValidationControlsDictTransaction);

            NewTransaction.AccountCode = ImportString(Catalog.GetString("Account code"),
                AMainDS.ATransaction.ColumnAccountCode, AValidationControlsDictTransaction);

            NewTransaction.Narrative = ImportString(Catalog.GetString("Narrative"),
                AMainDS.ATransaction.ColumnNarrative, AValidationControlsDictTransaction);

            NewTransaction.Reference = ImportString(Catalog.GetString("Reference"),
                AMainDS.ATransaction.ColumnReference, AValidationControlsDictTransaction);

            DateTime TransactionDate = ImportDate(Catalog.GetString("Transaction date"),
                AMainDS.ATransaction.ColumnTransactionDate, ARowNumber, AMessages, AValidationControlsDictTransaction);

            decimal DebitAmount = ImportDecimal(Catalog.GetString("Debit amount"),
                AMainDS.ATransaction.ColumnTransactionAmount, ARowNumber, AMessages, AValidationControlsDictTransaction);
            decimal CreditAmount = ImportDecimal(Catalog.GetString("Credit amount"),
                AMainDS.ATransaction.ColumnTransactionAmount, ARowNumber, AMessages, AValidationControlsDictTransaction);

            for (int i = 0; i < 10; i++)
            {
                String analysisType = ImportString(Catalog.GetString("Analysis Type") + "#" + i,
                    AMainDS.ATransAnalAttrib.ColumnAnalysisTypeCode, AValidationControlsDictTransaction);
                String analysisValue = ImportString(Catalog.GetString("Analysis Value") + "#" + i,
                    AMainDS.ATransAnalAttrib.ColumnAnalysisAttributeValue, AValidationControlsDictTransaction);

                bool gotType = (analysisType != null) && (analysisType.Length > 0);
                bool gotValue = (analysisValue != null) && (analysisValue.Length > 0);

                if (gotType && !gotValue)
                {
                    // All analysis analysisType errors are non-critical
                    AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationWarningInLine, ARowNumber),
                        Catalog.GetString("Transaction analysis attributes must have an attribute value.") + strIgnoreAnalysisTypeAndValue,
                        TResultSeverity.Resv_Noncritical));
                }

                // The analysis data is only imported if all corresponding values are there:
                // Errors are recorded on-the-fly but are marked as non-critical
                if (gotType && gotValue)
                {
                    DataRow atrow = ASetupDS.AAnalysisType.Rows.Find(new Object[] { analysisType });
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
                AValidationControlsDictTransaction);

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

            AImportMessage = Catalog.GetString("Additional import validation of the transaction data.");
            ACostCentreRow costcentre = (ACostCentreRow)ASetupDS.ACostCentre.Rows.Find(new object[] { ALedgerNumber, NewTransaction.CostCentreCode });

            // check if cost centre exists, and is a posting costcentre.
            // check if cost centre is active.
            if ((costcentre == null) || !costcentre.PostingCostCentreFlag)
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                    String.Format(Catalog.GetString("Invalid cost centre '{0}'."), NewTransaction.CostCentreCode), TResultSeverity.Resv_Critical));
            }
            else if (!costcentre.CostCentreActiveFlag)
            {
                // TODO: ask user if he wants to use an inactive cost centre???
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                    String.Format(Catalog.GetString("Inactive cost centre '{0}'."), NewTransaction.CostCentreCode), TResultSeverity.Resv_Critical));
            }

            AAccountRow account = (AAccountRow)ASetupDS.AAccount.Rows.Find(new object[] { ALedgerNumber, NewTransaction.AccountCode });

            // check if account exists, and is a posting account.
            // check if account is active
            if ((account == null) || !account.PostingStatus)
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                    String.Format(Catalog.GetString("Invalid account code '{0}'."), NewTransaction.AccountCode), TResultSeverity.Resv_Critical));
            }
            else if (!account.AccountActiveFlag)
            {
                // TODO: ask user if he wants to use an inactive account???
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrImportValidationErrorInLine, ARowNumber),
                    String.Format(Catalog.GetString("Inactive account code '{0}'."), NewTransaction.AccountCode), TResultSeverity.Resv_Critical));
            }

            // update the totals of the batch
            GLRoutines.UpdateTotalsOfBatch(ref AMainDS, ANewBatchRow);

            if (TVerificationHelper.IsNullOrOnlyNonCritical(AMessages))
            {
                AImportMessage = Catalog.GetString("Saving the transaction:");

                // TODO If this is a fund transfer to a foreign cost centre, check whether there are Key Ministries available for it.
                ATransactionAccess.SubmitChanges(AMainDS.ATransaction, ATransaction);
                AMainDS.ATransaction.AcceptChanges();

                AImportMessage = Catalog.GetString("Saving the attributes:");

                ATransAnalAttribAccess.SubmitChanges(AMainDS.ATransAnalAttrib, ATransaction);
                AMainDS.ATransAnalAttrib.AcceptChanges();
            }

            // Update progress tracker every 40 records
            if (++AProgressTrackerCounter % 40 == 0)
            {
                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    string.Format(Catalog.GetString("Batch {0}, Journal {1}: {2}"),
                        ANewBatchRow.BatchNumber,
                        ANewJournalRow.JournalNumber,
                        AProgressTrackerCounter),
                    ((AProgressTrackerCounter / 40) + 2) * 10 > 90 ? 90 : ((AProgressTrackerCounter / 40) + 2) * 10);
            }
        }

        private String MakeFriendlyFKExceptions(Exception ex, string AType = "B")
        {
            //note that this is only done for "user errors" not for program errors!
            String innerMessage = ex.InnerException.ToString();

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

        private String ImportString(String AColumnTitle,
            DataColumn ADataColumn,
            TValidationControlsDict AValidationColumnsDict,
            bool ATreatEmptyStringAsText = true)
        {
            if ((ADataColumn != null) && (AValidationColumnsDict != null) && !AValidationColumnsDict.ContainsKey(ADataColumn))
            {
                AValidationColumnsDict.Add(ADataColumn, new TValidationControlsData(null, AColumnTitle));
            }

            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);

            if ((sReturn.Length == 0) && !ATreatEmptyStringAsText)
            {
                return null;
            }

            return sReturn;
        }

        private Boolean ImportBoolean(String AColumnTitle, DataColumn ADataColumn, TValidationControlsDict AValidationColumnsDict)
        {
            if ((ADataColumn != null) && (AValidationColumnsDict != null) && !AValidationColumnsDict.ContainsKey(ADataColumn))
            {
                AValidationColumnsDict.Add(ADataColumn, new TValidationControlsData(null, AColumnTitle));
            }

            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return sReturn.ToLower().Equals("yes");
        }

        private Int64 ImportInt64(String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            TValidationControlsDict AValidationColumnsDict)
        {
            if ((ADataColumn != null) && (AValidationColumnsDict != null) && !AValidationColumnsDict.ContainsKey(ADataColumn))
            {
                AValidationColumnsDict.Add(ADataColumn, new TValidationControlsData(null, AColumnTitle));
            }

            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            Int64 retVal;

            if (Int64.TryParse(sReturn, out retVal))
            {
                return retVal;
            }

            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                String.Format(Catalog.GetString("Cannot convert '{0}' to a number. Will assume a value of -1."), sReturn),
                TResultSeverity.Resv_Critical));
            return -1;
        }

        private Int32 ImportInt32(String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            TValidationControlsDict AValidationColumnsDict)
        {
            if ((ADataColumn != null) && (AValidationColumnsDict != null) && !AValidationColumnsDict.ContainsKey(ADataColumn))
            {
                AValidationColumnsDict.Add(ADataColumn, new TValidationControlsData(null, AColumnTitle));
            }

            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            Int32 retVal;

            if (Int32.TryParse(sReturn, out retVal))
            {
                return retVal;
            }

            AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                String.Format(Catalog.GetString("Cannot convert '{0}' to a number. Will assume a value of -1."), sReturn),
                TResultSeverity.Resv_Critical));
            return -1;
        }

        private decimal ImportDecimal(String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            TValidationControlsDict AValidationColumnsDict)
        {
            if ((ADataColumn != null) && (AValidationColumnsDict != null) && !AValidationColumnsDict.ContainsKey(ADataColumn))
            {
                AValidationColumnsDict.Add(ADataColumn, new TValidationControlsData(null, AColumnTitle));
            }

            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            try
            {
                decimal dec = Convert.ToDecimal(sReturn, FCultureInfoNumberFormat);
                return dec;
            }
            catch
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                    String.Format(Catalog.GetString("Cannot convert '{0}' to a decimal number. Will assume a value of 1.00."), sReturn),
                    TResultSeverity.Resv_Critical));
                return 1.0m;
            }
        }

        private DateTime ImportDate(String AColumnTitle,
            DataColumn ADataColumn,
            int ARowNumber,
            TVerificationResultCollection AMessages,
            TValidationControlsDict AValidationColumnsDict)
        {
            if ((ADataColumn != null) && (AValidationColumnsDict != null) && !AValidationColumnsDict.ContainsKey(ADataColumn))
            {
                AValidationColumnsDict.Add(ADataColumn, new TValidationControlsData(null, AColumnTitle));
            }

            String sDate = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            DateTime dtReturn;

            try
            {
                dtReturn = Convert.ToDateTime(sDate, FCultureInfoDate);
            }
            catch (Exception)
            {
                AMessages.Add(new TVerificationResult(String.Format(MCommonConstants.StrParsingErrorInLineColumn, ARowNumber, AColumnTitle),
                    String.Format(Catalog.GetString("Cannot convert '{0}' to a date. Will assume a value of 'Today'."), sDate),
                    TResultSeverity.Resv_Critical));
                TLogging.Log("Problem parsing " + sDate + " with format " + FCultureInfoDate.DateTimeFormat.ShortDatePattern);
                return DateTime.Today;
            }

            return dtReturn;
        }
    }
}