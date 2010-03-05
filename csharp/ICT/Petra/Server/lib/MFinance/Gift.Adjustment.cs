/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       timop
 *
 * Copyright 2004-2010 by OM International
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
using System.Data.Odbc;
using System.Collections.Specialized;
using System.Collections.Generic;
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
        public static Int32 FieldChangeAdjustment(Int32 ALedgerNumber,
            Int64 ARecipientKey,
            DateTime AStartDate,
            DateTime AEndDate,
            Int64 AOldField,
            DateTime ADateCorrection,
            bool AWithReceipt)
        {
            TDBTransaction Transaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.ReadCommitted);

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

            // load AGift and AGiftDetail
            GiftBatchTDS oldGiftDS = new GiftBatchTDS();

            SqlStmt = SqlStmt.Replace("SELECT PUB_a_gift_detail.*, PUB_a_gift.*", "SELECT PUB_a_gift_detail.*");
            DBAccess.GDBAccessObj.Select(oldGiftDS, SqlStmt, AGiftDetailTable.GetTableDBName(), Transaction, parameters.ToArray());

            parameters = new List <OdbcParameter>();
            param = new OdbcParameter("LedgerNumber", OdbcType.Int);
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

            SqlStmt = SqlStmt.Replace("SELECT PUB_a_gift_detail.*", "SELECT PUB_a_gift.*");
            DBAccess.GDBAccessObj.Select(oldGiftDS, SqlStmt, AGiftTable.GetTableDBName(), Transaction, parameters.ToArray());

            DBAccess.GDBAccessObj.RollbackTransaction();

            GiftBatchTDS GiftDS = TTransactionWebConnector.CreateAGiftBatch(ALedgerNumber, ADateCorrection);
            AGiftBatchRow giftbatchRow = GiftDS.AGiftBatch[0];
            giftbatchRow.BatchDescription = Catalog.GetString("Gift Adjustment (Field Change)");

            foreach (GiftBatchTDSAGiftDetailRow oldGiftDetail in oldGiftDS.AGiftDetail.Rows)
            {
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
                AGiftDetailRow detail = GiftDS.AGiftDetail.NewRowTyped();

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

            TSubmitChangesResult result = TTransactionWebConnector.SaveGiftBatchTDS(ref GiftDS, out VerificationResult);

            if (result == TSubmitChangesResult.scrOK)
            {
//              result = TTransactionWebConnector.SaveGiftBatchTDS(ref oldGiftDS, out VerificationResult);

                if (result == TSubmitChangesResult.scrOK)
                {
                    return giftbatchRow.BatchNumber;
                }
            }

            return -1;
        }
    }
}