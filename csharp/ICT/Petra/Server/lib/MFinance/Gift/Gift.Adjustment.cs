//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop,matthiash
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    ///<summary>
    /// This connector provides functions to adjust and reverse gifts
    ///</summary>
    public class TAdjustmentWebConnector
    {
        /// <summary>
        /// The field of a worker has changed, and we need to adjust previous gifts that have been posted to the wrong field.
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ARecipientKey"></param>
        /// <param name="AStartDate">start of period where we want to fix gifts</param>
        /// <param name="AEndDate">end of period where we want to fix gifts</param>
        /// <param name="AOldField">the wrong field</param>
        /// <param name="ADateCorrection">the date where we want to create the correction gift batch</param>
        /// <param name="AWithReceipt">if the gifts have already been receipted in the old year, and correction is in the new year. don't print receipts again in the new year.</param>
        /// <returns>the gift batch</returns>
        [RequireModulePermission("FINANCE-1")]
        public static Int32 FieldChangeAdjustment(Int32 ALedgerNumber,
            Int64 ARecipientKey,
            DateTime AStartDate,
            DateTime AEndDate,
            Int64 AOldField,
            DateTime ADateCorrection,
            bool AWithReceipt)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);
            GiftBatchTDS oldGiftDS = new GiftBatchTDS();

            try
            {
                // find all gifts that need reversing.
                // criteria:
                // posted gift batches only
                // no adjusted/reversed gifts
                // date of gift batch in specified date range
                // recipient field is the old field
                string SqlStmt = TDataBase.ReadSqlFile("Gift.GetGiftsToReverse.sql");

                List <OdbcParameter>parameters = new List <OdbcParameter>();
                OdbcParameter param = new OdbcParameter("LedgerNumber", OdbcType.Int);
                param.Value = ALedgerNumber;
                parameters.Add(param);
                param = new OdbcParameter("StartDate", OdbcType.Date);
                param.Value = AStartDate;
                parameters.Add(param);
                param = new OdbcParameter("EndDate", OdbcType.Date);
                param.Value = AEndDate;
                parameters.Add(param);
                param = new OdbcParameter("RecipientKey", OdbcType.BigInt);
                param.Value = ARecipientKey;
                parameters.Add(param);
                param = new OdbcParameter("OldField", OdbcType.BigInt);
                param.Value = AOldField;
                parameters.Add(param);

                DBAccess.GDBAccessObj.Select(oldGiftDS, SqlStmt, oldGiftDS.AGiftDetail.TableName, Transaction, parameters.ToArray());

                // load the gift and the gift batch records if they have not been loaded yet
                foreach (AGiftDetailRow giftdetail in oldGiftDS.AGiftDetail.Rows)
                {
                    oldGiftDS.AGift.DefaultView.RowFilter = String.Format("{0} = {1} and {2} = {3}",
                        AGiftTable.GetBatchNumberDBName(),
                        giftdetail.BatchNumber,
                        AGiftTable.GetGiftTransactionNumberDBName(),
                        giftdetail.GiftTransactionNumber);

                    if (oldGiftDS.AGift.DefaultView.Count == 0)
                    {
                        AGiftTable tempGiftTable =
                            AGiftAccess.LoadByPrimaryKey(giftdetail.LedgerNumber,
                                giftdetail.BatchNumber,
                                giftdetail.GiftTransactionNumber,
                                Transaction);
                        oldGiftDS.AGift.Merge(tempGiftTable);
                    }

                    oldGiftDS.AGiftBatch.DefaultView.RowFilter = String.Format("{0} = {1}",
                        AGiftTable.GetBatchNumberDBName(),
                        giftdetail.BatchNumber);

                    if (oldGiftDS.AGiftBatch.DefaultView.Count == 0)
                    {
                        AGiftBatchTable tempGiftBatchTable =
                            AGiftBatchAccess.LoadByPrimaryKey(giftdetail.LedgerNumber,
                                giftdetail.BatchNumber,
                                Transaction);
                        oldGiftDS.AGiftBatch.Merge(tempGiftBatchTable);
                    }
                }

                DBAccess.GDBAccessObj.RollbackTransaction();
            }
            catch (Exception)
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
                throw;
            }

            // we need to create a gift batch for each set of gifts with the same Currency, BankAccountCode, BankCostCentre, and Gift Type
            SortedList <string, GiftBatchTDS>NewGiftBatches = new SortedList <string, GiftBatchTDS>();

            foreach (GiftBatchTDSAGiftDetailRow oldGiftDetail in oldGiftDS.AGiftDetail.Rows)
            {
                // get the gift batch row for this detail
                oldGiftDS.AGiftBatch.DefaultView.RowFilter =
                    String.Format("{0} = {1}",
                        AGiftTable.GetBatchNumberDBName(), oldGiftDetail.BatchNumber);

                AGiftBatchRow oldGiftBatch = (AGiftBatchRow)oldGiftDS.AGiftBatch.DefaultView[0].Row;

                GiftBatchTDS GiftDS = CreateNewGiftBatch(NewGiftBatches, oldGiftBatch, ADateCorrection);

                AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];

                // get the gift row for this detail
                DataView v = oldGiftDS.AGift.DefaultView;
                v.RowFilter =
                    String.Format("{0} = {1} and {2} = {3}",
                        AGiftTable.GetBatchNumberDBName(), oldGiftDetail.BatchNumber,
                        AGiftTable.GetGiftTransactionNumberDBName(), oldGiftDetail.GiftTransactionNumber);

                AGiftRow oldGift = (AGiftRow)v[0].Row;

                AGiftRow gift = GiftDS.AGift.NewRowTyped();
                gift.LedgerNumber = giftbatchRow.LedgerNumber;
                gift.BatchNumber = giftbatchRow.BatchNumber;
                gift.GiftTransactionNumber = giftbatchRow.LastGiftNumber + 1;
                gift.DonorKey = oldGift.DonorKey;
                gift.DateEntered = ADateCorrection;
                giftbatchRow.LastGiftNumber++;
                GiftDS.AGift.Rows.Add(gift);

                if (!AWithReceipt)
                {
                    gift.ReceiptLetterCode = "NO*RECET";
                }

                // reverse the original gift
                GiftBatchTDSAGiftDetailRow detail = GiftDS.AGiftDetail.NewRowTyped();

                DataUtilities.CopyAllColumnValues(oldGiftDetail, detail);

                detail.LedgerNumber = gift.LedgerNumber;
                detail.BatchNumber = gift.BatchNumber;
                detail.GiftTransactionNumber = gift.GiftTransactionNumber;
                detail.DetailNumber = gift.LastDetailNumber + 1;
                detail.GiftAmount = detail.GiftAmount * -1;
                detail.GiftAmountIntl = detail.GiftAmountIntl * -1;
                detail.GiftTransactionAmount = detail.GiftTransactionAmount * -1;
                gift.LastDetailNumber++;

                GiftDS.AGiftDetail.Rows.Add(detail);

                // create the detail for the corrected gift to the new field
                detail = GiftDS.AGiftDetail.NewRowTyped();

                DataUtilities.CopyAllColumnValues(oldGiftDetail, detail);

                detail.LedgerNumber = gift.LedgerNumber;
                detail.BatchNumber = gift.BatchNumber;
                detail.GiftTransactionNumber = gift.GiftTransactionNumber;
                detail.DetailNumber = gift.LastDetailNumber + 1;
                detail.GiftCommentOne = String.Format(Catalog.GetString("posted on {0}"), oldGiftBatch.GlEffectiveDate.ToShortDateString());
                gift.LastDetailNumber++;

                // TODO: calculate costcentre code from current commitment; this currently is done only at time of posting
                // detail.RecipientLedgerNumber = oldGiftDetail.RecipientLedgerNumber;
                // detail.CostCentreCode = oldGiftDetail.CostCentreCode;

                GiftDS.AGiftDetail.Rows.Add(detail);

                // TODO: how to make sure that the gl transaction is marked as System generated? avoid display on HOSA?

                // mark original gift detail as modified
                oldGiftDetail.ModifiedDetail = true;
            }

            TVerificationResultCollection VerificationResult;

            TSubmitChangesResult result = TSubmitChangesResult.scrOK;

            for (Int32 batchCounter = 0; batchCounter < NewGiftBatches.Count; batchCounter++)
            {
                if (result == TSubmitChangesResult.scrOK)
                {
                    GiftBatchTDS GiftDS = NewGiftBatches.Values[batchCounter];
                    result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);
                }
            }

            if (result == TSubmitChangesResult.scrOK)
            {
                result = TGiftTransactionWebConnector.SaveGiftBatchTDS(ref oldGiftDS, out VerificationResult);

                if ((result == TSubmitChangesResult.scrOK) && (NewGiftBatches.Count > 0))
                {
                    return NewGiftBatches.Values[0].AGiftBatch[0].BatchNumber;
                }
            }

            return -1;
        }

        /// try to find a new gift batch that does already have the same properties
        private static GiftBatchTDS CreateNewGiftBatch(SortedList <string, GiftBatchTDS>ANewGiftBatches,
            AGiftBatchRow AOldGiftBatch,
            DateTime ADateCorrection)
        {
            string key = AOldGiftBatch.CurrencyCode + ";" +
                         AOldGiftBatch.BankAccountCode + ";" +
                         AOldGiftBatch.BankCostCentre + ";" +
                         AOldGiftBatch.GiftType;

            if (!ANewGiftBatches.ContainsKey(key))
            {
                GiftBatchTDS GiftDS = TGiftTransactionWebConnector.CreateAGiftBatch(AOldGiftBatch.LedgerNumber, ADateCorrection,
                    Catalog.GetString("Gift Adjustment (Field Change)"));
                AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];
                giftbatchRow.BankCostCentre = AOldGiftBatch.BankCostCentre;
                giftbatchRow.BankAccountCode = AOldGiftBatch.BankAccountCode;
                giftbatchRow.GiftType = AOldGiftBatch.GiftType;
                ANewGiftBatches.Add(key, GiftDS);

                return GiftDS;
            }
            else
            {
                return ANewGiftBatches[key];
            }
        }

        /// <summary>
        /// Revert or Adjust a Gift, revert a Gift Detail , revert a gift batch
        /// </summary>
        /// <param name="requestParams">Hashtable containing the given params </param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns>false if error</returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GiftRevertAdjust(Hashtable requestParams, out TVerificationResultCollection AMessages)
        {
            bool success = false;

            AMessages = new TVerificationResultCollection();

            Int32 ALedgerNumber = (Int32)requestParams["ALedgerNumber"];
            Boolean batchSelected = (Boolean)requestParams["NewBatchSelected"];
            Int32 ANewBatchNumber = 0;

            if (batchSelected)
            {
                ANewBatchNumber = (Int32)requestParams["NewBatchNumber"];
            }

            String Function = (String)requestParams["Function"];
            Int32 AGiftDetailNumber = (Int32)requestParams["GiftDetailNumber"];
            Int32 AGiftNumber = (Int32)requestParams["GiftNumber"];
            Int32 ABatchNumber = (Int32)requestParams["BatchNumber"];

            //decimal batchHashTotal = 0;
            decimal batchGiftTotal = 0;

            GiftBatchTDS MainDS = new GiftBatchTDS();
            TDBTransaction Transaction = null;
            DateTime ADateEffective;
            try
            {
                Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);
                ALedgerTable LedgerTable = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, Transaction);
                AGiftBatchRow giftBatch;

                if (!batchSelected)
                {
                    ADateEffective = (DateTime)requestParams["GlEffectiveDate"];
                    AGiftBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, Transaction);
                    AGiftBatchRow oldGiftBatch = MainDS.AGiftBatch[0];
                    TGiftBatchFunctions.CreateANewGiftBatchRow(ref MainDS, ref Transaction, ref LedgerTable, ALedgerNumber, ADateEffective);
                    giftBatch = MainDS.AGiftBatch[1];
                    giftBatch.BankAccountCode = oldGiftBatch.BankAccountCode;
                    giftBatch.BankCostCentre = oldGiftBatch.BankCostCentre;
                    giftBatch.CurrencyCode = oldGiftBatch.CurrencyCode;
                    giftBatch.ExchangeRateToBase = oldGiftBatch.ExchangeRateToBase;
                    giftBatch.MethodOfPaymentCode = oldGiftBatch.MethodOfPaymentCode;
                    //giftBatch.HashTotal = -oldGiftBatch.HashTotal;
                    //giftBatch.BatchTotal = -oldGiftBatch.BatchTotal;

                    if (giftBatch.MethodOfPaymentCode.Length == 0)
                    {
                        giftBatch.SetMethodOfPaymentCodeNull();
                    }

                    giftBatch.BankCostCentre = oldGiftBatch.BankCostCentre;
                    giftBatch.GiftType = oldGiftBatch.GiftType;

                    if (Function.Equals("AdjustGift"))
                    {
                        giftBatch.BatchDescription = Catalog.GetString("Gift Adjustment");
                    }
                    else
                    {
                        giftBatch.BatchDescription = Catalog.GetString("Reverse Gift");
                    }
                }
                else
                {
                    AGiftBatchAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ANewBatchNumber, Transaction);
                    giftBatch = MainDS.AGiftBatch[0];
                    ADateEffective = giftBatch.GlEffectiveDate;
					//If into an existing batch, then retrive the existing batch total
                    batchGiftTotal = giftBatch.BatchTotal;
                }

                if (Function.Equals("ReverseGiftBatch"))
                {
                    AGiftAccess.LoadViaAGiftBatch(MainDS, ALedgerNumber, ABatchNumber, Transaction);

                    foreach (AGiftRow gift in MainDS.AGift.Rows)
                    {
                        AGiftDetailAccess.LoadViaAGift(MainDS, ALedgerNumber, ABatchNumber, gift.GiftTransactionNumber, Transaction);
                    }
                }
                else
                {
                    AGiftAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AGiftNumber, Transaction);

                    if (Function.Equals("ReverseGiftDetail"))
                    {
                        AGiftDetailAccess.LoadByPrimaryKey(MainDS, ALedgerNumber, ABatchNumber, AGiftNumber, AGiftDetailNumber, Transaction);
                    }
                    else
                    {
                        AGiftDetailAccess.LoadViaAGift(MainDS, ALedgerNumber, ABatchNumber, AGiftNumber, Transaction);
                    }
                }

                int countGifts = MainDS.AGift.Rows.Count;
                int countGiftsDetail = MainDS.AGiftDetail.Rows.Count;

                //assuming new elements are added after these static borders

                int cycle = 0;

                do
                {
                    for (int i = 0; (i < countGifts); i++)
                    {
                        AGiftRow oldGift = MainDS.AGift[i];

                        if ((oldGift.BatchNumber == ABatchNumber) && (oldGift.LedgerNumber == ALedgerNumber)
                            && (Function.Equals("ReverseGiftBatch") || (oldGift.GiftTransactionNumber == AGiftNumber)))
                        {
                            AGiftRow gift = MainDS.AGift.NewRowTyped(true);
                            DataUtilities.CopyAllColumnValuesWithoutPK(oldGift, gift);
                            gift.LedgerNumber = giftBatch.LedgerNumber;
                            gift.BatchNumber = giftBatch.BatchNumber;
                            gift.DateEntered = ADateEffective;
                            gift.GiftTransactionNumber = giftBatch.LastGiftNumber + 1;
                            giftBatch.LastGiftNumber++;
                            gift.LastDetailNumber = 1;

                            /*
                             * gift.MethodOfGivingCode = oldGift.MethodOfGivingCode;
                             *
                             * if (gift.MethodOfGivingCode.Length == 0)
                             * {
                             *  gift.SetMethodOfGivingCodeNull();
                             * }
                             *
                             * gift.MethodOfPaymentCode = oldGift.MethodOfPaymentCode;
                             *
                             * if (gift.MethodOfPaymentCode.Length == 0)
                             * {
                             *  gift.SetMethodOfPaymentCodeNull();
                             * }
                             *
                             * gift.AdminCharge = oldGift.AdminCharge;
                             * gift.DonorKey = oldGift.DonorKey;
                             * gift.ReceiptLetterCode = oldGift.ReceiptLetterCode;
                             * gift.Reference = oldGift.Reference;
                             * gift.BankingDetailsKey = oldGift.BankingDetailsKey;
                             * gift.Restricted = oldGift.Restricted;
                             * gift.FirstTimeGift = oldGift.FirstTimeGift;
                             * gift.GiftStatus = oldGift.GiftStatus;         // unknown status is copied?
                             */
                            MainDS.AGift.Rows.Add(gift);

                            for (int j = 0; (j < countGiftsDetail); j++)
                            {
                                AGiftDetailRow oldGiftDetail = MainDS.AGiftDetail[j];

                                if ((oldGiftDetail.GiftTransactionNumber == oldGift.GiftTransactionNumber)
                                    && (oldGiftDetail.BatchNumber == ABatchNumber)
                                    && (oldGiftDetail.LedgerNumber == ALedgerNumber)
                                    && (!Function.Equals("ReverseGiftDetail") || (oldGiftDetail.DetailNumber == AGiftDetailNumber)))
                                {
                                    if ((cycle == 0) && oldGiftDetail.ModifiedDetail)
                                    {
                                        AMessages.Add(new TVerificationResult(
                                                String.Format(Catalog.GetString("Cannot reverse or adjust Gift {0} with Detail {1} in Batch {2}"),
                                                    oldGiftDetail.GiftTransactionNumber, oldGiftDetail.DetailNumber, oldGiftDetail.BatchNumber),
                                                String.Format(Catalog.GetString("It was already adjusted or reversed.")),
                                                TResultSeverity.Resv_Critical));
                                        DBAccess.GDBAccessObj.RollbackTransaction();
                                        return false;
                                    }

                                    AGiftDetailRow giftDetail = MainDS.AGiftDetail.NewRowTyped(true);
                                    DataUtilities.CopyAllColumnValuesWithoutPK(oldGiftDetail, giftDetail);
                                    giftDetail.DetailNumber = gift.LastDetailNumber++;
                                    giftDetail.LedgerNumber = gift.LedgerNumber;
                                    giftDetail.BatchNumber = giftBatch.BatchNumber;
                                    giftDetail.GiftTransactionNumber = gift.GiftTransactionNumber;

                                    decimal signum = (cycle == 0) ? -1 : 1;
                                    giftDetail.GiftTransactionAmount = signum * oldGiftDetail.GiftTransactionAmount;
                                    giftDetail.GiftAmount = signum * oldGiftDetail.GiftAmount;
                                    batchGiftTotal += giftDetail.GiftAmount;
                                    giftDetail.GiftAmountIntl = signum * oldGiftDetail.GiftAmountIntl;

                                    /*
                                     * giftDetail.RecipientLedgerNumber = oldGiftDetail.RecipientLedgerNumber;
                                     * giftDetail.MotivationGroupCode = oldGiftDetail.MotivationGroupCode;
                                     * giftDetail.MotivationDetailCode = oldGiftDetail.MotivationDetailCode;
                                     * giftDetail.ConfidentialGiftFlag = oldGiftDetail.ConfidentialGiftFlag;
                                     * giftDetail.TaxDeductable = oldGiftDetail.TaxDeductable;
                                     * giftDetail.RecipientKey = oldGiftDetail.RecipientKey;
                                     *
                                     * giftDetail.ChargeFlag = oldGiftDetail.ChargeFlag;
                                     * giftDetail.CostCentreCode = oldGiftDetail.CostCentreCode;
                                     *
                                     * if (giftDetail.CostCentreCode.Length == 0)
                                     * {
                                     *  giftDetail.SetCostCentreCodeNull();
                                     * }
                                     *
                                     * giftDetail.IchNumber = oldGiftDetail.IchNumber;
                                     *
                                     * giftDetail.MailingCode = oldGiftDetail.MailingCode;
                                     *
                                     * if (giftDetail.MailingCode.Length == 0)
                                     * {
                                     *  giftDetail.SetMailingCodeNull();
                                     * }
                                     */
                                    giftDetail.GiftCommentOne = (String)requestParams["ReversalCommentOne"];
                                    giftDetail.GiftCommentTwo = (String)requestParams["ReversalCommentTwo"];
                                    giftDetail.GiftCommentThree = (String)requestParams["ReversalCommentThree"];
                                    giftDetail.CommentOneType = (String)requestParams["ReversalCommentOneType"];
                                    giftDetail.CommentTwoType = (String)requestParams["ReversalCommentTwoType"];
                                    giftDetail.CommentThreeType = (String)requestParams["ReversalCommentThreeType"];

                                    // This is used to mark both as a Reverted giftDetails, except the adjusted (new) gift

                                    giftDetail.ModifiedDetail = (cycle == 0);
                                    oldGiftDetail.ModifiedDetail = (cycle == 0);
                                    MainDS.AGiftDetail.Rows.Add(giftDetail);
                                }
                            }
                        }
                    }

                    cycle++;
                } while ((cycle < 2) && Function.Equals("AdjustGift"));

                //When reversing into a new or existing batch, set batch total
                if (!Function.Equals("AdjustGift"))
                {
               		giftBatch.BatchTotal = batchGiftTotal;
                }

                // save everything at the end
                if (AGiftBatchAccess.SubmitChanges(MainDS.AGiftBatch, Transaction, out AMessages))
                {
                    if (ALedgerAccess.SubmitChanges(LedgerTable, Transaction, out AMessages))
                    {
                        if (AGiftAccess.SubmitChanges(MainDS.AGift, Transaction, out AMessages))
                        {
                            if (AGiftDetailAccess.SubmitChanges(MainDS.AGiftDetail, Transaction, out AMessages))
                            {
                                success = true;
                            }
                        }
                    }
                }

                if (success)
                {
                    MainDS.AGiftBatch.AcceptChanges();
                    DBAccess.GDBAccessObj.CommitTransaction();
                    return success;
                }
                else
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (Transaction != null)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();
                }

                throw new Exception(Catalog.GetString("Gift Reverse/Adjust failed."), ex);
            }
        }
    }
}