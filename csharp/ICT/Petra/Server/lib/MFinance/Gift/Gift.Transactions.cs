//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;


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
        /// <param name="ADateEffective"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber, DateTime ADateEffective)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);


            TGiftBatchFunctions.CreateANewGiftBatchRow(ref MainDS, ref Transaction, ref LedgerTable, ALedgerNumber, ADateEffective);

            TVerificationResultCollection VerificationResult;
            bool success = false;

            if (AGiftBatchAccess.SubmitChanges(MainDS.AGiftBatch, Transaction, out VerificationResult))
            {
                if (ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out VerificationResult))
                {
                    success = true;
                }
            }

            if (success)
            {
                MainDS.AGiftBatch.AcceptChanges();
                DBAccess.GDBAccessObj.CommitTransaction();
                return MainDS;
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw new Exception("Error in CreateAGiftBatch");
            }
        }

        /// <summary>
        /// create a new batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS CreateAGiftBatch(Int32 ALedgerNumber)
        {
            return CreateAGiftBatch(ALedgerNumber, DateTime.Today);
        }

        /// <summary>
        /// create a new recurring batch with a consecutive batch number in the ledger,
        /// and immediately store the batch and the new number in the database
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static RecurringGiftBatchTDS CreateARecurringGiftBatch(Int32 ALedgerNumber)
        {
            RecurringGiftBatchTDS MainDS = new RecurringGiftBatchTDS();

            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

            ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);


            TGiftBatchFunctions.CreateANewRecurringGiftBatchRow(ref MainDS, ref Transaction, ref LedgerTable, ALedgerNumber);

            TVerificationResultCollection VerificationResult;
            bool success = false;

            if (ARecurringGiftBatchAccess.SubmitChanges(MainDS.ARecurringGiftBatch, Transaction, out VerificationResult))
            {
                if (ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out VerificationResult))
                {
                    success = true;
                }
            }

            if (success)
            {
                MainDS.ARecurringGiftBatch.AcceptChanges();
                DBAccess.GDBAccessObj.CommitTransaction();
                return MainDS;
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw new Exception("Error in CreateAGiftBatch");
            }
        }

        /// <summary>
        /// create a gift batch from a recurring gift batch
        /// including gift and gift detail
        /// </summary>
        /// <param name="requestParams ">HashTable with many parameters</param>
        /// <param name="AMessages ">Output structure for user error messages</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static Boolean SubmitRecurringGiftBatch(Hashtable requestParams, out TVerificationResultCollection AMessages)
        {
            Boolean success = false;

            AMessages = new TVerificationResultCollection();
            GiftBatchTDS GMainDS = new GiftBatchTDS();
            Int32 ALedgerNumber = (Int32)requestParams["ALedgerNumber"];
            Int32 ABatchNumber = (Int32)requestParams["ABatchNumber"];
            DateTime AEffectiveDate = (DateTime)requestParams["AEffectiveDate"];
            Decimal AExchangeRateToBase = (Decimal)requestParams["AExchangeRateToBase"];

            RecurringGiftBatchTDS RMainDS = LoadRecurringTransactions(ALedgerNumber, ABatchNumber);


            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                ARecurringGiftBatchAccess.LoadByPrimaryKey(RMainDS, ALedgerNumber, ABatchNumber, Transaction);

                // Assuming all relevant data is loaded in FMainDS
                foreach (ARecurringGiftBatchRow recBatch  in RMainDS.ARecurringGiftBatch.Rows)
                {
                    if ((recBatch.BatchNumber == ABatchNumber) && (recBatch.LedgerNumber == ALedgerNumber))
                    {
                        Decimal batchTotal = 0;
                        AGiftBatchRow batch = TGiftBatchFunctions.CreateANewGiftBatchRow(ref GMainDS,
                            ref Transaction,
                            ref LedgerTable,
                            ALedgerNumber,
                            AEffectiveDate);
                        batch.GlEffectiveDate = AEffectiveDate;
                        batch.BatchDescription = recBatch.BatchDescription;
                        batch.BankCostCentre = recBatch.BankCostCentre;
                        batch.BankAccountCode = recBatch.BankAccountCode;
                        batch.ExchangeRateToBase = AExchangeRateToBase;
                        batch.MethodOfPaymentCode = recBatch.MethodOfPaymentCode;
                        batch.GiftType = recBatch.GiftType;
                        //batch.HashTotal = recBatch.HashTotal; // Does this  make sense? Active Gifts are not
                        batch.CurrencyCode = recBatch.CurrencyCode;

                        foreach (ARecurringGiftRow recGift in RMainDS.ARecurringGift.Rows)
                        {
                            if ((recGift.BatchNumber == ABatchNumber) && (recGift.LedgerNumber == ALedgerNumber) && recGift.Active)
                            {
                                //Look if there is a detail which is in the donation period (else continue)
                                bool foundDetail = false;

                                foreach (ARecurringGiftDetailRow recGiftDetail in RMainDS.ARecurringGiftDetail.Rows)
                                {
                                    if ((recGiftDetail.GiftTransactionNumber == recGift.GiftTransactionNumber)
                                        && (recGiftDetail.BatchNumber == ABatchNumber) && (recGiftDetail.LedgerNumber == ALedgerNumber)
                                        && ((recGiftDetail.StartDonations == null) || (recGiftDetail.StartDonations <= DateTime.Today))
                                        && ((recGiftDetail.EndDonations == null) || (recGiftDetail.EndDonations >= DateTime.Today))
                                        )
                                    {
                                        foundDetail = true;
                                        break;
                                    }
                                }

                                if (!foundDetail)
                                {
                                    continue;
                                }

                                // make the gift from recGift
                                AGiftRow gift = GMainDS.AGift.NewRowTyped();
                                gift.LedgerNumber = batch.LedgerNumber;
                                gift.BatchNumber = batch.BatchNumber;
                                gift.GiftTransactionNumber = batch.LastGiftNumber + 1;
                                gift.DonorKey = recGift.DonorKey;
                                gift.MethodOfGivingCode = recGift.MethodOfGivingCode;

                                if (gift.MethodOfGivingCode.Length == 0)
                                {
                                    gift.SetMethodOfGivingCodeNull();
                                }

                                gift.MethodOfPaymentCode = recGift.MethodOfPaymentCode;

                                if (gift.MethodOfPaymentCode.Length == 0)
                                {
                                    gift.SetMethodOfPaymentCodeNull();
                                }

                                gift.Reference = recGift.Reference;
                                gift.ReceiptLetterCode = recGift.ReceiptLetterCode;


                                GMainDS.AGift.Rows.Add(gift);
                                batch.LastGiftNumber++;
                                //TODO (not here, but in the client or while posting) Check for Ex-OM Partner
                                //TODO (not here, but in the client or while posting) Check for expired key ministry (while Posting)

                                foreach (ARecurringGiftDetailRow recGiftDetail in RMainDS.ARecurringGiftDetail.Rows)
                                {
                                    if ((recGiftDetail.GiftTransactionNumber == recGift.GiftTransactionNumber)
                                        && (recGiftDetail.BatchNumber == ABatchNumber) && (recGiftDetail.LedgerNumber == ALedgerNumber)
                                        && ((recGiftDetail.StartDonations == null) || (recGiftDetail.StartDonations <= DateTime.Today))
                                        && ((recGiftDetail.EndDonations == null) || (recGiftDetail.EndDonations >= DateTime.Today))
                                        )
                                    {
                                        AGiftDetailRow detail = GMainDS.AGiftDetail.NewRowTyped();
                                        detail.LedgerNumber = gift.LedgerNumber;
                                        detail.BatchNumber = gift.BatchNumber;
                                        detail.GiftTransactionNumber = gift.GiftTransactionNumber;
                                        detail.DetailNumber = gift.LastDetailNumber + 1;
                                        gift.LastDetailNumber++;

                                        detail.GiftTransactionAmount = recGiftDetail.GiftAmount;
                                        batchTotal += recGiftDetail.GiftAmount;
                                        detail.RecipientKey = recGiftDetail.RecipientKey;
                                        //maybe that this is unused
                                        detail.RecipientLedgerNumber = recGiftDetail.RecipientLedgerNumber;
                                        detail.ChargeFlag = recGiftDetail.ChargeFlag;
                                        detail.ConfidentialGiftFlag = recGiftDetail.ConfidentialGiftFlag;
                                        detail.TaxDeductable = recGiftDetail.TaxDeductable;
                                        detail.MailingCode = recGiftDetail.MailingCode;

                                        if (detail.MailingCode.Length == 0)
                                        {
                                            detail.SetMailingCodeNull();
                                        }

                                        // TODO convert with exchange rate to get the amount in base currency
                                        // detail.GiftAmount=

                                        detail.MotivationGroupCode = recGiftDetail.MotivationGroupCode;
                                        detail.MotivationDetailCode = recGiftDetail.MotivationDetailCode;
                                        detail.GiftCommentOne = recGiftDetail.GiftCommentOne;
                                        detail.CommentOneType = recGiftDetail.CommentOneType;
                                        detail.GiftCommentTwo = recGiftDetail.GiftCommentTwo;
                                        detail.CommentTwoType = recGiftDetail.CommentTwoType;
                                        detail.GiftCommentThree = recGiftDetail.GiftCommentThree;
                                        detail.CommentThreeType = recGiftDetail.CommentThreeType;


                                        GMainDS.AGiftDetail.Rows.Add(detail);
                                    }
                                }

                                batch.BatchTotal = batchTotal;
                            }
                        }
                    }
                }

                if (AGiftBatchAccess.SubmitChanges(GMainDS.AGiftBatch, Transaction, out AMessages))
                {
                    if (ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out AMessages))
                    {
                        if (AGiftAccess.SubmitChanges(GMainDS.AGift, Transaction, out AMessages))
                        {
                            if (AGiftDetailAccess.SubmitChanges(GMainDS.AGiftDetail, Transaction, out AMessages))
                            {
                                success = true;
                            }
                        }
                    }
                }

                if (success)
                {
                    DBAccess.GDBAccessObj.CommitTransaction();
                    GMainDS.AcceptChanges();
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    GMainDS.RejectChanges();
                }
            }
            catch (Exception ex)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw new Exception("Error in SubmitRecurringGiftBatch", ex);
            }
            return success;
        }

        /// <summary>
        /// loads a list of batches for the given ledger
        /// also get the ledger for the base currency etc
        /// TODO: limit to period, limit to batch status, etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
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
        /// loads a list of recurring batches for the given ledger
        /// also get the ledger for the base currency etc
        /// TODO: limit to period, limit to batch status, etc
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static RecurringGiftBatchTDS LoadARecurringGiftBatch(Int32 ALedgerNumber)
        {
            RecurringGiftBatchTDS MainDS = new RecurringGiftBatchTDS();
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

            ALedgerAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, Transaction);
            ARecurringGiftBatchAccess.LoadViaALedger(MainDS, ALedgerNumber, Transaction);
            DBAccess.GDBAccessObj.RollbackTransaction();
            return MainDS;
        }

        /// <summary>
        /// loads a list of gift transactions and details for the given ledger and batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static GiftBatchTDS LoadTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

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
                    shortName.Add(PPartnerTable.GetPartnerClassDBName());
                    PPartnerTable partner = PPartnerAccess.LoadByPrimaryKey(giftRow.DonorKey, shortName, Transaction);

                    giftDetail.DonorKey = giftRow.DonorKey;
                    giftDetail.DonorName = partner[0].PartnerShortName;
                    giftDetail.DonorClass = partner[0].PartnerClass;
                    giftDetail.MethodOfGivingCode = giftRow.MethodOfGivingCode;
                    giftDetail.MethodOfPaymentCode = giftRow.MethodOfPaymentCode;
                    giftDetail.ReceiptNumber = giftRow.ReceiptNumber;
                    giftDetail.ReceiptPrinted = giftRow.ReceiptPrinted;
                    // This may be not very fast we can optimize later
                    Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable unitTable = null;


                    //do the same for the Recipient
                    partner.Clear();
                    Int64 fieldNumber;

                    LoadKeyMinistryInsideTrans(ref Transaction, ref unitTable, ref partner, giftDetail.RecipientKey, out fieldNumber);
                    giftDetail.RecipientField = fieldNumber;

                    //partner = PPartnerAccess.LoadByPrimaryKey(giftDetail.RecipientKey, shortName, Transaction);
                    if (partner.Count > 0)
                    {
                        giftDetail.RecipientDescription = partner[0].PartnerShortName;
                    }
                    else
                    {
                        giftDetail.RecipientDescription = "INVALID";
                    }

                    giftDetail.DateEntered = giftRow.DateEntered;
                }
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return MainDS;
        }

        /// <summary>
        /// loads a list of recurring gift transactions and details for the given ledger and recurring batch
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ABatchNumber"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static RecurringGiftBatchTDS LoadRecurringTransactions(Int32 ALedgerNumber, Int32 ABatchNumber)
        {
            RecurringGiftBatchTDS MainDS = new RecurringGiftBatchTDS();
            TDBTransaction Transaction = null;

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                ARecurringGiftAccess.LoadViaARecurringGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                // AGiftDetailAccess.LoadViaGiftBatch does not exist; but we can easily simulate it:
                ARecurringGiftDetailAccess.LoadViaForeignKey(ARecurringGiftDetailTable.TableId,
                    ARecurringGiftBatchTable.TableId,
                    MainDS,
                    new string[2] { ARecurringGiftBatchTable.GetLedgerNumberDBName(), ARecurringGiftBatchTable.GetBatchNumberDBName() },
                    new System.Object[2] { ALedgerNumber, ABatchNumber },
                    null,
                    Transaction,
                    null,
                    0,
                    0);

                DataView giftView = new DataView(MainDS.ARecurringGift);

                // fill the columns in the modified GiftDetail Table to show donorkey, dateentered etc in the grid
                foreach (RecurringGiftBatchTDSARecurringGiftDetailRow giftDetail in MainDS.ARecurringGiftDetail.Rows)
                {
                    // get the gift
                    giftView.RowFilter = ARecurringGiftTable.GetGiftTransactionNumberDBName() + " = " + giftDetail.GiftTransactionNumber.ToString();

                    ARecurringGiftRow giftRow = (ARecurringGiftRow)giftView[0].Row;

                    StringCollection shortName = new StringCollection();
                    shortName.Add(PPartnerTable.GetPartnerShortNameDBName());
                    shortName.Add(PPartnerTable.GetPartnerClassDBName());
                    PPartnerTable partner = PPartnerAccess.LoadByPrimaryKey(giftRow.DonorKey, shortName, Transaction);

                    giftDetail.DonorKey = giftRow.DonorKey;
                    giftDetail.DonorName = partner[0].PartnerShortName;
                    giftDetail.DonorClass = partner[0].PartnerClass;
                    giftDetail.MethodOfGivingCode = giftRow.MethodOfGivingCode;
                    giftDetail.MethodOfPaymentCode = giftRow.MethodOfPaymentCode;
                    // This may be not very fast we can optimize later
                    Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable unitTable = null;


                    //do the same for the Recipient
                    partner.Clear();
                    Int64 fieldNumber;

                    LoadKeyMinistryInsideTrans(ref Transaction, ref unitTable, ref partner, giftDetail.RecipientKey, out fieldNumber);
                    giftDetail.RecipientField = fieldNumber;

                    //partner = PPartnerAccess.LoadByPrimaryKey(giftDetail.RecipientKey, shortName, Transaction);
                    if (partner.Count > 0)
                    {
                        giftDetail.RecipientDescription = partner[0].PartnerShortName;
                    }
                    else
                    {
                        giftDetail.RecipientDescription = "INVALID";
                    }
                }
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return MainDS;
        }

        /// <summary>
        /// this will store all new and modified batches, gift transactions and details
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveGiftBatchTDS(ref GiftBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            SubmissionResult = GiftBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                // TODO: check that gifts are in consecutive numbers?
                // TODO: check that gift details are in consecutive numbers, no gift without gift details?
                // Problem: unchanged rows will not arrive here? check after committing, and update the gift batch again
                // TODO: calculate hash of saved batch or batch of saved gift
            }

            return SubmissionResult;
        }

        /// <summary>
        /// this will store all new and modified recurring batches, recurring gift transactions and recurring details
        /// </summary>
        /// <param name="AInspectDS"></param>
        /// <param name="AVerificationResult"></param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static TSubmitChangesResult SaveRecurringGiftBatchTDS(ref RecurringGiftBatchTDS AInspectDS,
            out TVerificationResultCollection AVerificationResult)
        {
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;

            SubmissionResult = RecurringGiftBatchTDSAccess.SubmitChanges(AInspectDS, out AVerificationResult);

            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                // TODO: check that gifts are in consecutive numbers?
                // TODO: check that gift details are in consecutive numbers, no gift without gift details?
                // Problem: unchanged rows will not arrive here? check after committing, and update the gift batch again
                // TODO: calculate hash of saved batch or batch of saved gift
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
            journal.LastTransactionNumber = 0;
            journal.DateOfEntry = DateTime.Now;

            // TODO journal.ExchangeRateToBase and journal.ExchangeRateTime
            journal.ExchangeRateToBase = 1.0M;
            GLDataset.AJournal.Rows.Add(journal);

            foreach (GiftBatchTDSAGiftDetailRow giftdetail in AGiftDataset.AGiftDetail.Rows)
            {
                ATransactionRow transaction = null;

                // do we have already a transaction for this costcentre&account?
                GLDataset.ATransaction.DefaultView.RowFilter = String.Format("{0}='{1}' and {2}='{3}'",
                    ATransactionTable.GetAccountCodeDBName(),
                    giftdetail.AccountCode,
                    ATransactionTable.GetCostCentreCodeDBName(),
                    giftdetail.CostCentreCode);

                if (GLDataset.ATransaction.DefaultView.Count == 0)
                {
                    transaction = GLDataset.ATransaction.NewRowTyped();
                    transaction.LedgerNumber = journal.LedgerNumber;
                    transaction.BatchNumber = journal.BatchNumber;
                    transaction.JournalNumber = journal.JournalNumber;
                    transaction.TransactionNumber = ++journal.LastTransactionNumber;
                    transaction.AccountCode = giftdetail.AccountCode;
                    transaction.CostCentreCode = giftdetail.CostCentreCode;
                    transaction.Narrative = "GB - Gift Batch " + giftbatch.BatchNumber.ToString();
                    transaction.Reference = "GB" + giftbatch.BatchNumber.ToString();
                    transaction.DebitCreditIndicator = false;
                    transaction.TransactionAmount = 0;
                    transaction.AmountInBaseCurrency = 0;
                    transaction.TransactionDate = giftbatch.GlEffectiveDate;

                    GLDataset.ATransaction.Rows.Add(transaction);
                }
                else
                {
                    transaction = (ATransactionRow)GLDataset.ATransaction.DefaultView[0].Row;
                }

                transaction.TransactionAmount += giftdetail.GiftTransactionAmount;
                transaction.AmountInBaseCurrency += giftdetail.GiftAmount;

                // TODO: for other currencies a post to a_ledger.a_forex_gains_losses_account_c ???

                // TODO: do the fee calculation, a_fees_payable, a_fees_receivable
            }

            ATransactionRow transactionForTotals = GLDataset.ATransaction.NewRowTyped();
            transactionForTotals.LedgerNumber = journal.LedgerNumber;
            transactionForTotals.BatchNumber = journal.BatchNumber;
            transactionForTotals.JournalNumber = journal.JournalNumber;
            transactionForTotals.TransactionNumber = ++journal.LastTransactionNumber;
            transactionForTotals.TransactionAmount = 0;
            transactionForTotals.TransactionDate = giftbatch.GlEffectiveDate;

            foreach (GiftBatchTDSAGiftDetailRow giftdetail in AGiftDataset.AGiftDetail.Rows)
            {
                transactionForTotals.TransactionAmount += giftdetail.GiftTransactionAmount;
            }

            transactionForTotals.DebitCreditIndicator = true;

            // TODO: support foreign currencies
            transactionForTotals.AmountInBaseCurrency = transactionForTotals.TransactionAmount;

            // TODO: account and costcentre based on linked costcentre, current commitment, and Motivation detail
            // if motivation cost centre is a summary cost centre, make sure the transaction costcentre is reporting to that summary cost centre
            // Careful: modify gift cost centre and account and recipient field only when the amount is positive.
            // adjustments and reversals must remain on the original value
            transactionForTotals.AccountCode = giftbatch.BankAccountCode;
            transactionForTotals.CostCentreCode = Ict.Petra.Server.MFinance.GL.WebConnectors.TTransactionWebConnector.GetStandardCostCentre(
                ALedgerNumber);
            transactionForTotals.Narrative = "Deposit from receipts - Gift Batch " + giftbatch.BatchNumber.ToString();
            transactionForTotals.Reference = "GB" + giftbatch.BatchNumber.ToString();

            GLDataset.ATransaction.Rows.Add(transactionForTotals);

            GLDataset.ATransaction.DefaultView.RowFilter = string.Empty;

            foreach (ATransactionRow transaction in GLDataset.ATransaction.Rows)
            {
                if (transaction.DebitCreditIndicator)
                {
                    journal.JournalDebitTotal += transaction.TransactionAmount;
                    batch.BatchDebitTotal += transaction.TransactionAmount;
                }
                else
                {
                    journal.JournalCreditTotal += transaction.TransactionAmount;
                    batch.BatchCreditTotal += transaction.TransactionAmount;
                }
            }

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
        [RequireModulePermission("FINANCE-2")]
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

        /// <summary>
        /// export all the Data of the batches array list to a String
        /// </summary>
        /// <param name="batches">Arraylist containing the batch numbers of the gift batches to export</param>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="exportString">Big parts of the export file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if not completed</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ExportAllGiftBatchData(ref ArrayList batches,
            Hashtable requestParams,
            out String exportString,
            out TVerificationResultCollection AMessages)
        {
            TGiftExporting exporting = new TGiftExporting();

            return exporting.ExportAllGiftBatchData(ref batches, requestParams, out exportString, out AMessages);
        }

        /// <summary>
        /// Import Gift batch data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediatelya
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="importString">The import file as a simple String</param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool ImportGiftBatches(
            Hashtable requestParams,
            String importString,
            out TVerificationResultCollection AMessages
            )
        {
            TGiftImporting importing = new TGiftImporting();

            return importing.ImportGiftBatches(requestParams, importString, out AMessages);
        }

        /// <summary>
        /// Load Partner Data
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="DonorKey">Partner Key </param>
        /// <returns>GLSetupDS with Partnertable for the partner Key</returns>
        [RequireModulePermission("FINANCE-1")]
        public static GLSetupTDS LoadPartnerData(long DonorKey)
        {
            GLSetupTDS PartnerDS = new GLSetupTDS();
            TDBTransaction Transaction = null;

            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

                PPartnerAccess.LoadByPrimaryKey(PartnerDS, DonorKey, Transaction);
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return PartnerDS;
        }

        /// <summary>
        /// Load key Ministry
        /// The data file contents from the client is sent as a string, imported in the database
        /// and committed immediately
        /// </summary>
        /// <param name="partnerKey">Partner Key </param>
        /// <param name="fieldNumber">Field Number </param>
        /// <returns>ArrayList for loading the key ministry combobox</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable LoadKeyMinistry(Int64 partnerKey, out Int64 fieldNumber)
        {
            TDBTransaction Transaction = null;

            Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable unitTable = null;
            PPartnerTable partnerTable = null;
            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);


                LoadKeyMinistryInsideTrans(ref Transaction, ref unitTable, ref partnerTable, partnerKey, out fieldNumber);
            }
            finally
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }
            }
            return unitTable;
        }

        // First check if partner is in class "unit"


        //TODO Get the field in p_person.p_om_field_key_n

        //TODO Get the field in p_family.p_om_field_key_n.


        private static void LoadKeyMinistryInsideTrans(ref TDBTransaction Transaction,
            ref Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable unitTable,
            ref PPartnerTable APartnerTable,
            Int64 partnerKey,
            out Int64 fieldNumber)
        {
            unitTable = new PUnitTable();
            fieldNumber = 0;

            if (partnerKey != 0)
            {
                APartnerTable = PPartnerAccess.LoadByPrimaryKey(partnerKey, Transaction);

                if (APartnerTable.Rows.Count == 1)
                {
                    PPartnerRow partnerRow = (PPartnerRow)APartnerTable.Rows[0];

                    switch (partnerRow.PartnerClass)
                    {
                        case MPartnerConstants.PARTNERCLASS_PERSON:
                            break;

                        case MPartnerConstants.PARTNERCLASS_FAMILY:
                            break;

                        case MPartnerConstants.PARTNERCLASS_BANK:
                            break;

                        case MPartnerConstants.PARTNERCLASS_VENUE:
                            break;

                        case MPartnerConstants.PARTNERCLASS_ORGANISATION:
                            break;

                        case MPartnerConstants.PARTNERCLASS_CHURCH:
                            break;

                        case MPartnerConstants.PARTNERCLASS_UNIT:
                            ProcessUnit(ref unitTable, ref Transaction, ref fieldNumber, partnerKey);
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        static void ProcessUnit(ref Ict.Petra.Shared.MPartner.Partner.Data.PUnitTable unitTable,
            ref TDBTransaction Transaction,
            ref Int64 fieldNumber,
            Int64 partnerKey)
        {
            // if the unittype is a key ministry we need to find the field

            PUnitTable put = PUnitAccess.LoadByPrimaryKey(partnerKey, Transaction);

            if (put.Rows.Count == 1)
            {
                PUnitRow unitRow = (PUnitRow)put.Rows[0];

                switch (unitRow.UnitTypeCode)
                {
                    case MPartnerConstants.UNIT_TYPE_KEYMIN:
                        fieldNumber = SearchField(partnerKey, ref Transaction);
                        LoadKeyMinistries(fieldNumber, ref unitTable, ref Transaction);
                        break;

                    case MPartnerConstants.UNIT_TYPE_FIELD:
                        fieldNumber = partnerKey;
                        LoadKeyMinistries(fieldNumber, ref unitTable, ref Transaction);
                        break;
                }
            }
        }

        private static Int64 SearchField(Int64 partnerKey, ref TDBTransaction Transaction)
        {
            PPartnerTypeTable ptt = PPartnerTypeAccess.LoadByPrimaryKey(partnerKey, MPartnerConstants.PARTNERTYPE_LEDGER, Transaction);

            if (ptt.Rows.Count == 1)
            {
                return partnerKey;
            }

            //This was taken from old Petra - perhaps we should better search for unit type = F in PUnit

            UmUnitStructureTable uust = UmUnitStructureAccess.LoadViaPUnitChildUnitKey(partnerKey, Transaction);

            if (uust.Rows.Count == 1)
            {
                if (uust[0].ParentUnitKey == uust[0].ChildUnitKey)
                {
                    return 0;
                }

                return SearchField(uust[0].ParentUnitKey, ref Transaction);
            }

            //TODO Warning on inactive Fund
            return partnerKey;
        }

        private static void LoadKeyMinistries(Int64 partnerKey, ref PUnitTable unitTable, ref TDBTransaction Transaction)
        {
            UmUnitStructureTable uust = UmUnitStructureAccess.LoadViaPUnitParentUnitKey(partnerKey, Transaction);

            foreach (UmUnitStructureRow uusr in uust.Rows)
            {
                PUnitTable put = PUnitAccess.LoadByPrimaryKey(uusr.ChildUnitKey, Transaction);

                if (put.Rows.Count == 1)
                {
                    PUnitRow unitRow = (PUnitRow)put.Rows[0];

                    if (unitRow.UnitTypeCode.Equals(MPartnerConstants.UNIT_TYPE_KEYMIN))
                    {
                        PPartnerTable myPPartnerTable =
                            PPartnerAccess.LoadByPrimaryKey(unitRow.PartnerKey, Transaction);

                        if (myPPartnerTable.Rows.Count == 1)
                        {
                            PPartnerRow partnerRow = (PPartnerRow)myPPartnerTable.Rows[0];

                            if (partnerRow.StatusCode.Equals(MPartnerConstants.PARTNERSTATUS_ACTIVE))
                            {
                                PUnitRow newRow = unitTable.NewRowTyped();
                                newRow.PartnerKey = unitRow.PartnerKey;
                                newRow.UnitName = unitRow.UnitName;
                                newRow.UnitTypeCode = unitRow.UnitTypeCode;
                                unitTable.Rows.Add(newRow);
                            }
                        }
                    }
                }
            }
        }
    }
}