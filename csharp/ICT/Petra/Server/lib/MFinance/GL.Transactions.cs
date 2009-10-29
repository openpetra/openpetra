/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Data;
using System.Collections.Specialized;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.GL;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.GL.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance GL screens
    ///</summary>
    public class TTransactionWebConnector
    {
        /// <summary>
        /// retrieve the start and end dates of the current period of the ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDate"></param>
        /// <param name="AEndDate"></param>
        public static bool GetCurrentPeriodDates(Int32 ALedgerNumber, out DateTime AStartDate, out DateTime AEndDate)
        {
            ALedgerTable LedgerTable;
            AAccountingPeriodTable AccountingPeriodTable;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, LedgerTable[0].CurrentPeriod, Transaction);

            AStartDate = AccountingPeriodTable[0].PeriodStartDate;
            AEndDate = AccountingPeriodTable[0].PeriodEndDate;

            DBAccess.GDBAccessObj.CommitTransaction();

            return true;
        }

        /// <summary>
        /// Get the valid dates for posting;
        /// based on current period and number of forwarding periods
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AStartDateCurrentPeriod"></param>
        /// <param name="AEndDateLastForwardingPeriod"></param>
        public static bool GetCurrentPostingRangeDates(Int32 ALedgerNumber,
            out DateTime AStartDateCurrentPeriod,
            out DateTime AEndDateLastForwardingPeriod)
        {
            ALedgerTable LedgerTable;
            AAccountingPeriodTable AccountingPeriodTable;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
            AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber, LedgerTable[0].CurrentPeriod, Transaction);

            AStartDateCurrentPeriod = AccountingPeriodTable[0].PeriodStartDate;

            AccountingPeriodTable = AAccountingPeriodAccess.LoadByPrimaryKey(ALedgerNumber,
                LedgerTable[0].CurrentPeriod + LedgerTable[0].NumberFwdPostingPeriods,
                Transaction);
            AEndDateLastForwardingPeriod = AccountingPeriodTable[0].PeriodEndDate;

            DBAccess.GDBAccessObj.CommitTransaction();

            return true;
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static GLBatchTDS CreateABatch(Int32 ALedgerNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            ALedgerTable LedgerTable;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            ABatchRow NewRow = MainDS.ABatch.NewRowTyped(true);
            NewRow.LedgerNumber = ALedgerNumber;
            LedgerTable[0].LastBatchNumber++;
            NewRow.BatchNumber = LedgerTable[0].LastBatchNumber;
            NewRow.BatchPeriod = LedgerTable[0].CurrentPeriod;
            MainDS.ABatch.Rows.Add(NewRow);

            TVerificationResultCollection VerificationResult;
            ABatchAccess.SubmitChanges(MainDS.ABatch, Transaction, out VerificationResult);
            ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out VerificationResult);

            MainDS.ABatch.AcceptChanges();

            DBAccess.GDBAccessObj.CommitTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of batches for the given ledger
        /// also get the ledger for the base currency etc
        /// TODO: limit to period, limit to batch status, etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static GLBatchTDS LoadABatch(Int32 ALedgerNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
            ABatchAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of journals for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        public static GLBatchTDS LoadAJournal(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AJournalAccess.LoadViaABatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of transactions for the given ledger and batch and journal
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AJournalNumber"></param>
        /// <returns></returns>
        public static GLBatchTDS LoadATransaction(Int32 ALedgerNumber, Int32 ABatchNumber, Int32 AJournalNumber)
        {
            GLBatchTDS MainDS = new GLBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ATransactionAccess.LoadViaAJournal(MainDS, ALedgerNumber, ABatchNumber, AJournalNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// this will store all new and modified batches, journals, transactions
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveGLBatchTDS(ref GLBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TDBTransaction SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AVerificationResult = new TVerificationResultCollection();

            try
            {
                SubmissionResult = TSubmitChangesResult.scrOK;

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    if (!ABatchAccess.SubmitChanges(AInspectDS.ABatch, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    if (!AJournalAccess.SubmitChanges(AInspectDS.AJournal, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    if (!ATransactionAccess.SubmitChanges(AInspectDS.ATransaction, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            catch (Exception e)
            {
                TLogging.Log("SaveGLBatchTDS: exception " + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }

        /// <summary>
        /// post a GL Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGLBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            return TGLPosting.PostGLBatch(ALedgerNumber, ABatchNumber, out AVerifications);
        }

        /// <summary>
        /// return the name of the standard costcentre for the given ledger;
        /// this supports up to 4 digit ledgers
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static string GetStandardCostCentre(Int32 ALedgerNumber)
        {
            return String.Format("{0:##00}00", ALedgerNumber);
        }
    }
}