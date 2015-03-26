//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2011 by OM International
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

using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Verification;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MFinance.Gift.Data.Access;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;

namespace Ict.Petra.Server.MFinance.Gift.WebConnectors
{
    /// <summary>
    /// setup the motivation groups and motivation details
    /// </summary>
    public class TTaxDeductibleWebConnector
    {
        /// <summary>
        /// Determines whether a partner is the recipient of unposted or posted gifts after a certain date
        /// with a different TaxDeductiblePct from the one given.
        /// </summary>
        /// <param name="APartnerKey"></param>
        /// <param name="AGiftTotals">Table containing the number of gifts to the partner with a different TaxDeductiblePct from the one given per ledger.</param>
        /// <param name="APct">What the Ta DeductiblePct should be.</param>
        /// <param name="ADateFrom">Date from which we check posted gifts</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool IsPartnerARecipient(Int64 APartnerKey, out DataTable AGiftTotals, decimal APct, DateTime ADateFrom)
        {
            DataTable Table = new DataTable();

            TDBTransaction Transaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                delegate
                {
                    string Query = "SELECT" +
                                   " a_gift_detail.a_ledger_number_i AS LedgerNumber," +
                                   " SUM(CASE WHEN a_gift_batch.a_batch_status_c = 'Unposted' " +
                                   " AND a_gift.a_date_entered_d >= '" + ADateFrom.ToString("yyyy-MM-dd") + "'" +
                                   " THEN 1 ELSE 0 END) AS Unposted," +
                                   " SUM(CASE WHEN a_gift_batch.a_batch_status_c = 'Posted' " +
                                   " AND a_gift_batch.a_gl_effective_date_d >= '" + ADateFrom.ToString("yyyy-MM-dd") + "'" +
                                   " THEN 1 ELSE 0 END) AS Posted" +

                                   " FROM a_gift_detail, a_gift_batch, a_gift" +

                                   " WHERE a_gift_detail.p_recipient_key_n = " + APartnerKey +
                                   " AND a_gift_detail.a_tax_deductible_pct_n <> " + APct +
                                   " AND a_gift_detail.a_modified_detail_l <> true" +
                                   " AND a_gift_detail.a_tax_deductible_l = true" +
                                   " AND a_gift_batch.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift_batch.a_batch_number_i = a_gift_detail.a_batch_number_i" +
                                   " AND a_gift_batch.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift.a_batch_number_i = a_gift_detail.a_batch_number_i" +
                                   " AND a_gift.a_gift_transaction_number_i = a_gift_detail.a_gift_transaction_number_i" +

                                   " GROUP BY a_gift_detail.a_ledger_number_i";

                    Table = DBAccess.GDBAccessObj.SelectDT(Query, "Gifts", Transaction);
                });

            AGiftTotals = Table;

            return Table != null && Table.Rows.Count > 0;
        }

        /// <summary>
        /// Updates the TaxDeductiblePct for unposted gifts to the given recipient
        /// </summary>
        /// <param name="ARecipientKey"></param>
        /// <param name="ANewPct"></param>
        /// <param name="ADateFrom"></param>
        [RequireModulePermission("FINANCE-1")]
        public static void UpdateUnpostedGiftsTaxDeductiblePct(Int64 ARecipientKey, decimal ANewPct, DateTime ADateFrom)
        {
            TDBTransaction Transaction = null;
            bool SubmissionOK = false;

            DBAccess.GDBAccessObj.BeginAutoTransaction(IsolationLevel.ReadCommitted,
                ref Transaction,
                ref SubmissionOK,
                delegate
                {
                    string Query = "SELECT a_gift_detail.*" +

                                   " FROM a_gift_detail, a_gift_batch, a_gift" +

                                   " WHERE a_gift_detail.p_recipient_key_n = " + ARecipientKey +
                                   " AND a_gift_detail.a_tax_deductible_pct_n <> " + ANewPct +
                                   " AND a_gift_detail.a_modified_detail_l <> true" +
                                   " AND a_gift_detail.a_tax_deductible_l = true" +
                                   " AND a_gift_batch.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift_batch.a_batch_number_i = a_gift_detail.a_batch_number_i" +
                                   " AND a_gift_batch.a_batch_status_c = 'Unposted'" +
                                   " AND a_gift.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift.a_batch_number_i = a_gift_detail.a_batch_number_i" +
                                   " AND a_gift.a_gift_transaction_number_i = a_gift_detail.a_gift_transaction_number_i" +
                                   " AND a_gift.a_date_entered_d >= '" + ADateFrom.ToString("yyyy-MM-dd") + "'";

                    AGiftDetailTable Table = new AGiftDetailTable();

                    DBAccess.GDBAccessObj.SelectDT(Table, Query, Transaction);

                    // update fields for each row
                    for (int i = 0; i < Table.Rows.Count; i++)
                    {
                        AGiftDetailRow Row = Table[i];

                        Row.TaxDeductiblePct = ANewPct;
                        TaxDeductibility.UpdateTaxDeductibiltyAmounts(ref Row);
                    }

                    AGiftDetailAccess.SubmitChanges(Table, Transaction);

                    SubmissionOK = true;
                });
        }

        /// <summary>
        /// Find all gifts that need their tax deductible percentage adjusted
        /// </summary>
        /// <param name="AGiftDS">Gift Batch containing all the data needed for tax deductible pct Adjustment</param>
        /// <param name="ARecipientKey"></param>
        /// <param name="ADateFrom"></param>
        /// <param name="ANewPct"></param>
        /// <param name="AMessages">Additional messages to display in a messagebox</param>
        /// <returns></returns>
        [RequireModulePermission("FINANCE-1")]
        public static bool GetGiftsForTaxDeductiblePctAdjustment(ref GiftBatchTDS AGiftDS,
            Int64 ARecipientKey,
            DateTime ADateFrom,
            decimal ANewPct,
            out TVerificationResultCollection AMessages)
        {
            TDBTransaction Transaction = null;
            GiftBatchTDS MainDS = new GiftBatchTDS();

            AMessages = new TVerificationResultCollection();

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(IsolationLevel.ReadCommitted, ref Transaction,
                delegate
                {
                    string Query = "SELECT a_gift_detail.*" +

                                   " FROM a_gift_detail, a_gift_batch" +

                                   " WHERE a_gift_detail.p_recipient_key_n = " + ARecipientKey +
                                   " AND a_gift_detail.a_tax_deductible_pct_n <> " + ANewPct +
                                   " AND a_gift_detail.a_modified_detail_l <> true" +
                                   " AND a_gift_detail.a_tax_deductible_l = true" +
                                   " AND a_gift_batch.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift_batch.a_batch_number_i = a_gift_detail.a_batch_number_i" +
                                   " AND a_gift_batch.a_ledger_number_i = a_gift_detail.a_ledger_number_i" +
                                   " AND a_gift_batch.a_batch_status_c = 'Posted' " +
                                   " AND a_gift_batch.a_gl_effective_date_d >= '" + ADateFrom.ToString("yyyy-MM-dd") + "'";

                    DBAccess.GDBAccessObj.Select(MainDS, Query, MainDS.AGiftDetail.TableName, Transaction);

                    // get additional data
                    foreach (GiftBatchTDSAGiftDetailRow Row in MainDS.AGiftDetail.Rows)
                    {
                        AGiftBatchAccess.LoadByPrimaryKey(MainDS, Row.LedgerNumber, Row.BatchNumber, Transaction);
                        AGiftRow GiftRow =
                            AGiftAccess.LoadByPrimaryKey(MainDS, Row.LedgerNumber, Row.BatchNumber, Row.GiftTransactionNumber, Transaction);

                        Row.DateEntered = GiftRow.DateEntered;
                        Row.DonorKey = GiftRow.DonorKey;
                        Row.DonorName = PPartnerAccess.LoadByPrimaryKey(Row.DonorKey, Transaction)[0].PartnerShortName;
                    }
                });

            AGiftDS = MainDS;

            return TAdjustmentWebConnector.CheckGiftsNotPreviouslyReversed(AGiftDS, out AMessages);
        }
    }
}