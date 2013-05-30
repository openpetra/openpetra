//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       matthiash, timop
//
// Copyright 2004-2013 by OM International
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
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Data.Access;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.MSysMan.Data;
using Ict.Petra.Server.MFinance.Common;
using Ict.Petra.Server.App.Core;

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
        String FImportMessage;
        String FImportLine;
        String FNewLine;

        /// <summary>
        /// Import GL Batches data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="importString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        public bool ImportGLBatches(
            Hashtable requestParams,
            String importString,
            out TVerificationResultCollection AMessages
            )
        {
            Int32 ProgressTrackerCounter = 0;

            TProgressTracker.InitProgressTracker(DomainManager.GClientID.ToString(),
                Catalog.GetString("Importing GL Batches"),
                100);

            TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                Catalog.GetString("Initialising"),
                5);

            AMessages = new TVerificationResultCollection();
            GLBatchTDS MainDS = new GLBatchTDS();
            GLSetupTDS SetupDS = new GLSetupTDS();
            StringReader sr = new StringReader(importString);

            FDelimiter = (String)requestParams["Delimiter"];
            Int32 LedgerNumber = (Int32)requestParams["ALedgerNumber"];
            FDateFormatString = (String)requestParams["DateFormatString"];
            String NumberFormat = (String)requestParams["NumberFormat"];
            FNewLine = (String)requestParams["NewLine"];

            FCultureInfoNumberFormat = new CultureInfo(NumberFormat.Equals("American") ? "en-US" : "de-DE");
            FCultureInfoDate = new CultureInfo("en-GB");
            FCultureInfoDate.DateTimeFormat.ShortDatePattern = FDateFormatString;

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            Int32 RowNumber = 0;

            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(LedgerNumber, Transaction);
                AAnalysisTypeAccess.LoadAll(SetupDS, Transaction);
                AFreeformAnalysisAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                AAnalysisAttributeAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                ACostCentreAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);
                AAccountAccess.LoadViaALedger(SetupDS, LedgerNumber, Transaction);

                ABatchRow NewBatch = null;
                AJournalRow NewJournal = null;
                int BatchPeriodNumber = -1;
                int BatchYearNr = -1;

                //AGiftRow gift = null;
                FImportMessage = Catalog.GetString("Parsing first line");
                bool ok = false;

                while ((FImportLine = sr.ReadLine()) != null)
                {
                    RowNumber++;

                    // skip empty lines and commented lines
                    if ((FImportLine.Trim().Length > 0) && !FImportLine.StartsWith("/*") && !FImportLine.StartsWith("#"))
                    {
                        string RowType = ImportString(Catalog.GetString("row type"));

                        if (RowType == "B")
                        {
                            if (NewBatch != null)
                            {
                                // update the totals of the batch that has just been imported
                                GLRoutines.UpdateTotalsOfBatch(ref MainDS, NewBatch);
                            }

                            NewBatch = MainDS.ABatch.NewRowTyped(true);
                            NewBatch.LedgerNumber = LedgerNumber;
                            LedgerTable[0].LastBatchNumber++;
                            NewBatch.BatchNumber = LedgerTable[0].LastBatchNumber;
                            NewBatch.BatchPeriod = LedgerTable[0].CurrentPeriod;
                            MainDS.ABatch.Rows.Add(NewBatch);
                            NewJournal = null;

                            NewBatch.BatchDescription = ImportString(Catalog.GetString("batch description"));

                            if ((NewBatch.BatchDescription == null)
                                || (NewBatch.BatchDescription == ""))
                            {
                                // raise error if empty batch description is imported
                                FImportMessage = Catalog.GetString("The batch description must not be empty");
                                throw new Exception();
                            }

                            NewBatch.BatchControlTotal = ImportDecimal(Catalog.GetString("batch hash value"));
                            NewBatch.DateEffective = ImportDate(Catalog.GetString("batch effective date"));

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
                                FImportMessage = Catalog.GetString("The effective date of the imported batch is not in an open period:") + " " +
                                                 StringHelper.DateToLocalizedString(NewBatch.DateEffective);
                                throw new Exception();
                            }

                            FImportMessage = Catalog.GetString("Saving GL batch:");

                            if (!ABatchAccess.SubmitChanges(MainDS.ABatch, Transaction, out AMessages))
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    Catalog.GetString("Database I/O failure"),
                                    0);

                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                                return false;
                            }

                            MainDS.ABatch.AcceptChanges();
                        }
                        else if (RowType == "J")
                        {
                            if (NewBatch == null)
                            {
                                FImportMessage = Catalog.GetString("Expected a Batch line, but found a Journal");
                                throw new Exception();
                            }

                            NewJournal = MainDS.AJournal.NewRowTyped(true);
                            NewJournal.LedgerNumber = NewBatch.LedgerNumber;
                            NewJournal.BatchNumber = NewBatch.BatchNumber;
                            NewJournal.JournalNumber = NewBatch.LastJournal + 1;
                            NewJournal.SubSystemCode = "GL";
                            NewJournal.TransactionTypeCode = "STD";
                            NewJournal.TransactionCurrency = "EUR";
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

                            NewJournal.JournalDescription = ImportString(Catalog.GetString("journal") + " - " + Catalog.GetString("description"));

                            if ((NewJournal.JournalDescription == null)
                                || (NewJournal.JournalDescription == ""))
                            {
                                // raise error if empty journal description is imported
                                FImportMessage = Catalog.GetString("The journal description must not be empty");
                                throw new Exception();
                            }

                            NewJournal.SubSystemCode = ImportString(Catalog.GetString("journal") + " - " + Catalog.GetString("sub system code"));
                            NewJournal.TransactionTypeCode = ImportString(Catalog.GetString("journal") + " - " + Catalog.GetString("transaction type"));
                            NewJournal.TransactionCurrency =
                                ImportString(Catalog.GetString("journal") + " - " + Catalog.GetString("transaction currency"));
                            NewJournal.ExchangeRateToBase = ImportDecimal(Catalog.GetString("journal") + " - " + Catalog.GetString("exchange rate"));
                            NewJournal.DateEffective = ImportDate(Catalog.GetString("journal") + " - " + Catalog.GetString("effective date"));

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
                                FImportMessage = String.Format(
                                    Catalog.GetString("The journal effective date {0} is not in the same period as the batch date {1}."),
                                        journalDate.ToShortDateString(), NewBatch.DateEffective.ToShortDateString());
                                throw new Exception();
                            }

                            FImportMessage = Catalog.GetString("Saving the journal:");

                            if (!AJournalAccess.SubmitChanges(MainDS.AJournal, Transaction, out AMessages))
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    Catalog.GetString("Database I/O failure"),
                                    0);

                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                                return false;
                            }

                            MainDS.AJournal.AcceptChanges();
                        }
                        else if (RowType == "T")
                        {
                            if (NewJournal == null)
                            {
                                FImportMessage = Catalog.GetString("Expected a Journal or Batch line, but found a Transaction");
                                throw new Exception();
                            }

                            GLBatchTDSATransactionRow NewTransaction = MainDS.ATransaction.NewRowTyped(true);
                            NewTransaction.LedgerNumber = NewJournal.LedgerNumber;
                            NewTransaction.BatchNumber = NewJournal.BatchNumber;
                            NewTransaction.JournalNumber = NewJournal.JournalNumber;
                            NewTransaction.TransactionNumber = NewJournal.LastTransactionNumber + 1;
                            NewJournal.LastTransactionNumber++;
                            MainDS.ATransaction.Rows.Add(NewTransaction);

                            NewTransaction.CostCentreCode = ImportString(Catalog.GetString("transaction") + " - " + Catalog.GetString("cost centre"));

                            ACostCentreRow costcentre = (ACostCentreRow)SetupDS.ACostCentre.Rows.Find(new object[] { LedgerNumber,
                                NewTransaction.CostCentreCode });

                            // check if cost centre exists, and is a posting costcentre.
                            // check if cost centre is active.
                            if ((costcentre == null) || !costcentre.PostingCostCentreFlag)
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Importing Transaction"),
                                        String.Format(Catalog.GetString("Invalid cost centre {0} in line {1}"),
                                            NewTransaction.CostCentreCode,
                                            RowNumber),
                                        TResultSeverity.Resv_Critical));
                            }
                            else if (!costcentre.CostCentreActiveFlag)
                            {
                                // TODO: ask user if he wants to use an inactive cost centre???
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Importing Transaction"),
                                        String.Format(Catalog.GetString("Inactive cost centre {0} in line {1}"),
                                            NewTransaction.CostCentreCode,
                                            RowNumber),
                                        TResultSeverity.Resv_Critical));
                            }

                            NewTransaction.AccountCode = ImportString(Catalog.GetString("transaction") + " - " + Catalog.GetString("account code"));

                            AAccountRow account = (AAccountRow)SetupDS.AAccount.Rows.Find(new object[] { LedgerNumber, NewTransaction.AccountCode });

                            // check if account exists, and is a posting account.
                            // check if account is active
                            if ((account == null) || !account.PostingStatus)
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Importing Transaction"),
                                        String.Format(Catalog.GetString("Invalid account code {0} in line {1}"),
                                            NewTransaction.AccountCode,
                                            RowNumber),
                                        TResultSeverity.Resv_Critical));
                            }
                            else if (!account.AccountActiveFlag)
                            {
                                // TODO: ask user if he wants to use an inactive account???
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Importing Transaction"),
                                        String.Format(Catalog.GetString("Inactive account {0} in line {1}"),
                                            NewTransaction.AccountCode,
                                            RowNumber),
                                        TResultSeverity.Resv_Critical));
                            }

                            NewTransaction.Narrative = ImportString(Catalog.GetString("transaction") + " - " + Catalog.GetString("narrative"));

                            NewTransaction.Reference = ImportString(Catalog.GetString("transaction") + " - " + Catalog.GetString("reference"));

                            NewTransaction.TransactionDate = ImportDate(Catalog.GetString("transaction") + " - " + Catalog.GetString("date"));


                            decimal DebitAmount = ImportDecimal(Catalog.GetString("transaction") + " - " + Catalog.GetString("debit amount"));
                            decimal CreditAmount = ImportDecimal(Catalog.GetString("transaction") + " - " + Catalog.GetString("credit amount"));

                            if ((DebitAmount == 0) && (CreditAmount == 0))
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Importing Transaction"),
                                    Catalog.GetString("Either the debit amount or the credit amount must be greater than 0."),
                                    TResultSeverity.Resv_Critical));
                            }

                            if ((DebitAmount != 0) && (CreditAmount != 0))
                            {
                                AMessages.Add(new TVerificationResult(Catalog.GetString("Importing Transaction"),
                                    Catalog.GetString("Transactions cannot have values for both debit and credit amounts."),
                                    TResultSeverity.Resv_Critical));
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

                            NewTransaction.AmountInBaseCurrency = GLRoutines.Divide(NewTransaction.TransactionAmount, NewJournal.ExchangeRateToBase);

                            for (int i = 0; i < 10; i++)
                            {
                                String type = ImportString(Catalog.GetString("Transaction") + " - " + Catalog.GetString("Analysis Type") + "#" + i);
                                String val = ImportString(Catalog.GetString("Transaction") + " - " + Catalog.GetString("Analysis Value") + "#" + i);

                                //these data is only be imported if all corresponding values are there
                                if ((type != null) && (type.Length > 0) && (val != null) && (val.Length > 0))
                                {
                                    DataRow atrow = SetupDS.AAnalysisType.Rows.Find(new Object[] { type });
                                    DataRow afrow = SetupDS.AFreeformAnalysis.Rows.Find(new Object[] { NewTransaction.LedgerNumber, type, val });
                                    DataRow anrow =
                                        SetupDS.AAnalysisAttribute.Rows.Find(new Object[] { NewTransaction.LedgerNumber,
                                                                                            type,
                                                                                            NewTransaction.AccountCode });

                                    if ((atrow != null) && (afrow != null) && (anrow != null))
                                    {
                                        ATransAnalAttribRow NewTransAnalAttrib = MainDS.ATransAnalAttrib.NewRowTyped(true);
                                        NewTransAnalAttrib.LedgerNumber = NewTransaction.LedgerNumber;
                                        NewTransAnalAttrib.BatchNumber = NewTransaction.BatchNumber;
                                        NewTransAnalAttrib.JournalNumber = NewTransaction.JournalNumber;
                                        NewTransAnalAttrib.TransactionNumber = NewTransaction.TransactionNumber;
                                        NewTransAnalAttrib.AnalysisTypeCode = type;
                                        NewTransAnalAttrib.AnalysisAttributeValue = val;
                                        NewTransAnalAttrib.AccountCode = NewTransaction.AccountCode;
                                        MainDS.ATransAnalAttrib.Rows.Add(NewTransAnalAttrib);
                                    }
                                }
                            }

                            if (AMessages.HasCriticalErrors)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    Catalog.GetString("Batch has critical errors"),
                                    0);

                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                                return false;
                            }

                            // update the totals of the last batch that has just been imported
                            GLRoutines.UpdateTotalsOfBatch(ref MainDS, NewBatch);

                            FImportMessage = Catalog.GetString("Saving the transaction:");

                            // TODO If this is a fund transfer to a foreign cost centre, check whether there are Key Ministries available for it.
                            if (!ATransactionAccess.SubmitChanges(MainDS.ATransaction, Transaction, out AMessages))
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    Catalog.GetString("Database I/O failure"),
                                    0);

                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                                return false;
                            }

                            MainDS.ATransaction.AcceptChanges();
                            FImportMessage = Catalog.GetString("Saving the attributes:");

                            if (!ATransAnalAttribAccess.SubmitChanges(MainDS.ATransAnalAttrib, Transaction, out AMessages))
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    Catalog.GetString("Database I/O failure"),
                                    0);

                                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                                return false;
                            }

                            MainDS.ATransAnalAttrib.AcceptChanges();

                            // Update progress tracker every 40 records
                            if (++ProgressTrackerCounter % 40 == 0)
                            {
                                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                                    string.Format(Catalog.GetString("Batch {0}, Journal {1}: {2}"),
                                        NewBatch.BatchNumber,
                                        NewJournal.JournalNumber,
                                        ProgressTrackerCounter),
                                    ((ProgressTrackerCounter / 40) + 2) * 10 > 90 ? 90 : ((ProgressTrackerCounter / 40) + 2) * 10);
                            }
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }

                FImportMessage = Catalog.GetString("Saving counter fields:");

                //Finally save all pending changes (last xxx number is updated)
                if (ABatchAccess.SubmitChanges(MainDS.ABatch, Transaction, out AMessages))
                {
                    if (ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out AMessages))
                    {
                        if (AJournalAccess.SubmitChanges(MainDS.AJournal, Transaction, out AMessages))
                        {
                            ok = true;
                        }
                    }
                }

                MainDS.AcceptChanges();

                if (ok)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    AMessages.Add(new TVerificationResult("Import",
                            Catalog.GetString("Data could not be saved."),
                            TResultSeverity.Resv_Critical));
                }
            }
            catch (Exception ex)
            {
                String speakingExceptionText = SpeakingExceptionMessage(ex);
                AMessages.Add(new TVerificationResult(Catalog.GetString("Import"),
                        String.Format(Catalog.GetString("There is a problem parsing the file in row {0}:"), RowNumber) +
                        FNewLine +
                        Catalog.GetString(FImportMessage) + FNewLine + speakingExceptionText,
                        TResultSeverity.Resv_Critical));
                DBAccess.GDBAccessObj.RollbackTransaction();

                TProgressTracker.SetCurrentState(DomainManager.GClientID.ToString(),
                    String.Format(Catalog.GetString("Problem parsing the file in row {0}"), RowNumber),
                    0);

                TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

                return false;
            }
            finally
            {
                try
                {
                    sr.Close();
                }
                catch
                {
                };

                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            TProgressTracker.FinishJob(DomainManager.GClientID.ToString());

            return true;
        }

        private String SpeakingExceptionMessage(Exception ex)
        {
            //note that this is only done for "user errors" not for program errors!
            String theExMessage = ex.Message;

            if (theExMessage.Contains("a_journal_fk3"))
            {
                return Catalog.GetString("Invalid sub system code or transaction type code");
            }

            if (theExMessage.Contains("a_journal_fk4"))
            {
                return Catalog.GetString("Invalid transaction currency");
            }

            if (theExMessage.Contains("a_transaction_fk3"))
            {
                return Catalog.GetString("Invalid cost centre");
            }

            if (theExMessage.Contains("a_transaction_fk2"))
            {
                return Catalog.GetString("Invalid account code");
            }

            TLogging.Log("Importing GL batch: " + ex.ToString());

            if (TLogging.DebugLevel > 0)
            {
                return ex.ToString();
            }

            return String.Empty;
        }

        private String ImportString(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);

            if (sReturn.Length == 0)
            {
                return null;
            }

            return sReturn;
        }

        private Boolean ImportBoolean(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return sReturn.ToLower().Equals("yes");
        }

        private Int64 ImportInt64(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return Convert.ToInt64(sReturn);
        }

        private Int32 ImportInt32(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            return Convert.ToInt32(sReturn);
        }

        private decimal ImportDecimal(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sReturn = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            decimal dec = sReturn.Trim().Length == 0 ? 0.0M : Convert.ToDecimal(sReturn, FCultureInfoNumberFormat);
            return dec;
        }

        private DateTime ImportDate(String message)
        {
            FImportMessage = String.Format(Catalog.GetString("Parsing the {0}:"), message);
            String sDate = StringHelper.GetNextCSV(ref FImportLine, FDelimiter);
            DateTime dtReturn = Convert.ToDateTime(sDate, FCultureInfoDate);
            return dtReturn;
        }
    }
}