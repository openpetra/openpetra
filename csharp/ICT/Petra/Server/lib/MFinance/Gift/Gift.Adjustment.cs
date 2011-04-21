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
using System.Data;
using System.Data.Odbc;
using System.Collections.Specialized;
using System.Collections.Generic;
using GNU.Gettext;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Shared.MFinance.GL.Data;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.App.Core.Security;

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
                    result = TTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);
                }
            }

            if (result == TSubmitChangesResult.scrOK)
            {
                result = TTransactionWebConnector.SaveGiftBatchTDS(ref oldGiftDS, out VerificationResult);

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
                GiftBatchTDS GiftDS = TTransactionWebConnector.CreateAGiftBatch(AOldGiftBatch.LedgerNumber, ADateCorrection);
                AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];
                giftbatchRow.BatchDescription = Catalog.GetString("Gift Adjustment (Field Change)");
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
    }
}