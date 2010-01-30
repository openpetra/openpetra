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
using Mono.Unix;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Server.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.App.ClientDomain;

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
            NewRow.BatchYear = LedgerTable[0].CurrentFinancialYear;
            NewRow.BankAccountCode = DomainManager.GSystemDefaultsCache.GetStringDefault(
                SharedConstants.SYSDEFAULT_GIFTBANKACCOUNT + ALedgerNumber.ToString());

            if (NewRow.BankAccountCode.Length == 0)
            {
                // use the first bank account
                AAccountPropertyTable accountProperties = AAccountPropertyAccess.LoadViaALedger(ALedgerNumber, Transaction);
                accountProperties.DefaultView.RowFilter = AAccountPropertyTable.GetPropertyCodeDBName() + " = '" +
                                                          MFinanceConstants.ACCOUNT_PROPERTY_BANK_ACCOUNT + "' and " +
                                                          AAccountPropertyTable.GetPropertyValueDBName() + " = 'true'";

                if (accountProperties.DefaultView.Count > 0)
                {
                    NewRow.BankAccountCode = ((AAccountPropertyRow)accountProperties.DefaultView[0].Row).AccountCode;
                }

                // TODO? DomainManager.GSystemDefaultsCache.SetDefault(SharedConstants.SYSDEFAULT_GIFTBANKACCOUNT + ALedgerNumber.ToString(), NewRow.BankAccountCode);
            }

            NewRow.BankCostCentre = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(ALedgerNumber);
            NewRow.CurrencyCode = LedgerTable[0].BaseCurrency;
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

            DataView giftView = new DataView(MainDS.AGift);

            // fill the columns in the modified GiftDetail Table to show donorkey, dateentered etc in the grid
            foreach (GiftBatchTDSAGiftDetailRow giftDetail in MainDS.AGiftDetail.Rows)
            {
                // get the gift
                giftView.RowFilter = AGiftTable.GetGiftTransactionNumberDBName() + " = " + giftDetail.GiftTransactionNumber.ToString();

                AGiftRow giftRow = (AGiftRow)giftView[0].Row;

                StringCollection shortName = new StringCollection();
                shortName.Add(PPartnerTable.GetPartnerShortNameDBName());
                PPartnerTable partner = PPartnerAccess.LoadByPrimaryKey(giftRow.DonorKey, shortName, Transaction);

                giftDetail.DonorKey = giftRow.DonorKey;
                giftDetail.DonorName = partner[0].PartnerShortName;
                giftDetail.DateEntered = giftRow.DateEntered;
            }

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
        /// creates the GL batch needed for posting the gift batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AGiftDataset"></param>
        /// <returns></returns>
        private static GLBatchTDS CreateGLBatchAndTransactionsForPostingGifts(Int32 ALedgerNumber, ref GiftBatchTDS AGiftDataset)
        {
            // create one GL batch
            GLBatchTDS GLDataset = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.CreateABatch(ALedgerNumber);

            ABatchRow batch = GLDataset.ABatch[0];

            AGiftBatchRow giftbatch = AGiftDataset.AGiftBatch[0];

            batch.BatchDescription = Catalog.GetString("Gift Batch " + giftbatch.BatchNumber.ToString());
            batch.DateEffective = giftbatch.GlEffectiveDate;

            // TODO batchperiod depending on date effective; or fix that when posting?
            // batch.BatchPeriod =
            batch.BatchStatus = MFinanceConstants.BATCH_UNPOSTED;

            // one gift batch only has one currency, create only one journal
            AJournalRow journal = GLDataset.AJournal.NewRowTyped();
            journal.LedgerNumber = batch.LedgerNumber;
            journal.BatchNumber = batch.BatchNumber;
            journal.JournalNumber = 1;
            journal.DateEffective = batch.DateEffective;
            journal.JournalPeriod = giftbatch.BatchPeriod;
            journal.TransactionCurrency = giftbatch.CurrencyCode;
            journal.JournalDescription = batch.BatchDescription;
            journal.TransactionTypeCode = MFinanceConstants.TRANSACTION_GIFT;
            journal.SubSystemCode = MFinanceConstants.SUB_SYSTEM_GR;
            journal.DateOfEntry = DateTime.Now;

            // TODO journal.ExchangeRateToBase and journal.ExchangeRateTime
            journal.ExchangeRateToBase = 1.0;
            GLDataset.AJournal.Rows.Add(journal);

            Int32 TransactionCounter = 1;

            foreach (GiftBatchTDSAGiftDetailRow giftdetail in AGiftDataset.AGiftDetail.Rows)
            {
                ATransactionRow transaction = null;

                // at the moment, we create 2 transactions for each gift detail; no summarising of gift transactions etc

                transaction = GLDataset.ATransaction.NewRowTyped();
                transaction.LedgerNumber = journal.LedgerNumber;
                transaction.BatchNumber = journal.BatchNumber;
                transaction.JournalNumber = journal.JournalNumber;
                transaction.TransactionNumber = TransactionCounter++;
                transaction.TransactionAmount = giftdetail.GiftTransactionAmount;

                transaction.DebitCreditIndicator = (transaction.TransactionAmount > 0);

                if (transaction.TransactionAmount < 0)
                {
                    transaction.TransactionAmount *= -1;
                }

                // TODO: support foreign currencies
                transaction.AmountInBaseCurrency = transaction.TransactionAmount;

                transaction.AccountCode = giftbatch.BankAccountCode;
                transaction.CostCentreCode = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(
                    ALedgerNumber);
                transaction.Narrative = "Deposit from receipts - Gift Batch " + giftbatch.BatchNumber.ToString();
                transaction.Reference = "GB" + giftbatch.BatchNumber.ToString();

                // TODO transaction.DetailNumber

                GLDataset.ATransaction.Rows.Add(transaction);

                // at the moment: no summarising of gift details of same gift transaction etc
                // create one transaction for the gift destination account (income)
                ATransactionRow transactionGiftAccount = GLDataset.ATransaction.NewRowTyped();
                transactionGiftAccount.LedgerNumber = journal.LedgerNumber;
                transactionGiftAccount.BatchNumber = journal.BatchNumber;
                transactionGiftAccount.JournalNumber = journal.JournalNumber;
                transactionGiftAccount.TransactionNumber = TransactionCounter++;
                transactionGiftAccount.DebitCreditIndicator = !transaction.DebitCreditIndicator;
                transactionGiftAccount.TransactionAmount = transaction.TransactionAmount;
                transactionGiftAccount.AmountInBaseCurrency = transaction.AmountInBaseCurrency;
                transactionGiftAccount.AccountCode = giftdetail.AccountCode;
                transactionGiftAccount.CostCentreCode = giftdetail.CostCentreCode;
                transactionGiftAccount.Narrative = "GB - Gift Batch " + giftbatch.BatchNumber.ToString();
                transactionGiftAccount.Reference = "GB" + giftbatch.BatchNumber.ToString();

                // TODO transactionGiftAccount.DetailNumber

                GLDataset.ATransaction.Rows.Add(transactionGiftAccount);

                // TODO: for other currencies a post to a_ledger.a_forex_gains_losses_account_c ???
            }

            journal.LastTransactionNumber = TransactionCounter - 1;

            batch.LastJournal = 1;

            return GLDataset;
        }

        /// create GiftBatchTDS with the gift batch to post, and all gift transactions and details, and motivation details
        private static GiftBatchTDS LoadGiftBatchForPosting(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GiftBatchTDS MainDS = LoadTransactions(ALedgerNumber, ABatchNumber);

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            AGiftBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
            AMotivationDetailAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);

            DBAccess.GDBAccessObj.RollbackTransaction();

            return MainDS;
        }

        /// <summary>
        /// post a Gift Batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <param name="AVerifications"></param>
        public static bool PostGiftBatch(Int32 ALedgerNumber, Int32 ABatchNumber, out TVerificationResultCollection AVerifications)
        {
            AVerifications = null;
            bool ResultValue = false;

            GiftBatchTDS MainDS = LoadGiftBatchForPosting(ALedgerNumber, ABatchNumber);

            foreach (GiftBatchTDSAGiftDetailRow giftDetail in MainDS.AGiftDetail.Rows)
            {
                // find motivation detail
                AMotivationDetailRow motivationRow =
                    (AMotivationDetailRow)MainDS.AMotivationDetail.Rows.Find(new object[] { ALedgerNumber, giftDetail.MotivationGroupCode,
                                                                                            giftDetail.MotivationDetailCode });

                // TODO: make sure the correct costcentres and accounts are used (check pm_staff_data for commitment period, and motivation details, etc)
                // set custom column giftdetail.AccountCode motivation
                giftDetail.AccountCode = motivationRow.AccountCode;

                // TODO deal with different currencies; at the moment assuming base currency
                giftDetail.GiftAmount = giftDetail.GiftTransactionAmount;
            }

            // TODO if already posted, fail
            MainDS.AGiftBatch[0].BatchStatus = MFinanceConstants.BATCH_POSTED;

            TDBTransaction SubmitChangesTransaction = null;

            try
            {
                // create GL batch
                GLBatchTDS GLDataset = CreateGLBatchAndTransactionsForPostingGifts(ALedgerNumber, ref MainDS);

                ABatchRow batch = GLDataset.ABatch[0];

                // save the batch
                if (Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.SaveGLBatchTDS(ref GLDataset,
                        out AVerifications) == TSubmitChangesResult.scrOK)
                {
                    // post the batch
                    if (!Ict.Petra.Server.MFinance.GL.TGLPosting.PostGLBatch(ALedgerNumber, batch.BatchNumber,
                            out AVerifications))
                    {
                        // TODO: what if posting fails? do we have an orphaned GL batch lying around? can this be put into one single transaction? probably not
                        // TODO: we should cancel that GL batch
                    }
                    else
                    {
                        SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                        // store GiftBatch and GiftDetails to database
                        if (AGiftBatchAccess.SubmitChanges(MainDS.AGiftBatch, SubmitChangesTransaction,
                                out AVerifications))
                        {
                            if (AGiftAccess.SubmitChanges(MainDS.AGift, SubmitChangesTransaction,
                                    out AVerifications))
                            {
                                // save changed motivation details, costcentre etc to database
                                if (AGiftDetailAccess.SubmitChanges(MainDS.AGiftDetail, SubmitChangesTransaction, out AVerifications))
                                {
                                    ResultValue = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // we should not get here; how would the database get broken?
                // TODO do we need a bigger transaction around everything?

                TLogging.Log("after submitchanges: exception " + e.Message);

                if (SubmitChangesTransaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw new Exception(e.ToString() + " " + e.Message);
            }

            if (ResultValue)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }

            return ResultValue;
        }
    }
}