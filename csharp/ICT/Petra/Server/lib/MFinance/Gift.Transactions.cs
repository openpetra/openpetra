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
using Ict.Petra.Shared.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    ///<summary>
    /// This connector provides data for the finance Gift screens
    ///</summary>
    public class TTransactionWebConnector
    {
        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        public static GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            ALedgerTable LedgerTable;
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);

            AGiftBatchRow NewRow = MainDS.AGiftBatch.NewRowTyped(true);
            NewRow.LedgerNumber = ALedgerNumber;
            LedgerTable[0].LastGiftBatchNumber++;
            NewRow.BatchNumber = LedgerTable[0].LastGiftBatchNumber;
            NewRow.BatchPeriod = LedgerTable[0].CurrentPeriod;
            MainDS.AGiftBatch.Rows.Add(NewRow);

            TVerificationResultCollection VerificationResult;
            AGiftBatchAccess.SubmitChanges(MainDS.AGiftBatch, Transaction, out VerificationResult);
            ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out VerificationResult);

            MainDS.AGiftBatch.AcceptChanges();

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
        public static GiftBatchTDS LoadAGiftBatch(Int32 ALedgerNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
            AGiftBatchAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of gift transactions and details for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        public static GiftBatchTDS LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            AGiftAccess.LoadViaAGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

            // AGiftDetailAccess.LoadViaGiftBatch does not exist; but we can easily simulate it:
            AGiftDetailAccess.LoadViaForeignKey(AGiftDetailTable.TableId,
                AGiftBatchTable.TableId,
                MainDS,
                new string[2] { AGiftBatchTable.GetLedgerNumberDBName(), AGiftBatchTable.GetBatchNumberDBName() },
                new System.Object[2] { ALedgerNumber, ABatchNumber },
                null,
                Transaction,
                null,
                0,
                0);

            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// this will store all new and modified batches, gift transactions and details
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        public static TSubmitChangesResult SaveGiftBatchTDS(ref GiftBatchTDS AInspectDS,
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
                    if (!AGiftBatchAccess.SubmitChanges(AInspectDS.AGiftBatch, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    if (!AGiftAccess.SubmitChanges(AInspectDS.AGift, SubmitChangesTransaction,
                            out AVerificationResult))
                    {
                        SubmissionResult = TSubmitChangesResult.scrError;
                    }
                }

                if (SubmissionResult == TSubmitChangesResult.scrOK)
                {
                    if (!AGiftDetailAccess.SubmitChanges(AInspectDS.AGiftDetail, SubmitChangesTransaction,
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
                TLogging.Log("SaveGiftBatchTDS: exception " + e.Message);

                DBAccess.GDBAccessObj.RollbackTransaction();

                throw new Exception(e.ToString() + " " + e.Message);
            }

            return SubmissionResult;
        }

        /// <summary>
        /// post a Gift Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            // TODO return TGiftPosting.PostGiftBatch(ALedgerNumber, ABatchNumber, out AVerifications);
            AVerifications = null;
            return false;
        }
    }
}