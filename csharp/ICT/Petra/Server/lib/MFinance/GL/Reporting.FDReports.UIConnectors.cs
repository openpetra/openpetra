//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
//
// Copyright 2004-2012 by OM International
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
using System.Collections.Generic;
using System.Data;

using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MPartner;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPartner.Common;
using Ict.Petra.Server.MPartner.Partner.Data.Access;
using Ict.Petra.Server.MSysMan.Maintenance.SystemDefaults.WebConnectors;

namespace Ict.Petra.Server.MFinance.Reporting.WebConnectors
{
    ///<summary>
    /// This WebConnector provides data for the Financial Development reporting screens
    ///</summary>
    public partial class TFinanceReportingWebConnector
    {
        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable SYBUNTTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string Currency = AParameters["param_currency"].ToString().ToUpper() == "BASE" ? "a_gift_amount_n" : "a_gift_amount_intl_n";
            bool Extract = AParameters["param_extract"].ToBool();
            int MinimumAmount = AParameters["param_minimum_amount"].ToInt32();
            string GiftsInRange = AParameters["param_gifts_in_range"].ToString();
            string NoGiftsInRange = AParameters["param_nogifts_in_range"].ToString();

            // create new datatable
            DataTable Results = new DataTable();
            DataTable ReturnTable = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query = "SELECT" +
                                   " PUB_p_partner.p_partner_key_n AS PartnerKey," +
                                   " PUB_p_partner.p_partner_short_name_c AS PartnerName," +
                                   " PUB_p_partner.p_partner_class_c AS PartnerClass," +
                                   " gift.a_date_entered_d AS LastGiftDate," +
                                   " detail.a_motivation_group_code_c AS MotivationGroup," +
                                   " detail.a_motivation_detail_code_c AS MotivationDetail," +
                                   " detail." + Currency + " AS LastGiftAmount," +
                                   " detail.a_ledger_number_i AS Ledger," +
                                   " detail.a_batch_number_i AS Batch," +
                                   " detail.a_gift_transaction_number_i AS GiftTransaction," +
                                   " detail.a_detail_number_i AS Detail" +

                                   " FROM" +
                                   " PUB_a_gift as gift," +
                                   " PUB_a_gift_detail as detail," +
                                   " PUB_a_gift_batch," +
                                   " PUB_p_partner";

                    if (Extract)
                    {
                        Query +=
                            ", PUB_m_extract," +
                            " PUB_m_extract_master" +
                            " WHERE" +
                            " gift.p_donor_key_n = PUB_m_extract.p_partner_key_n" +
                            " AND PUB_m_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i" +
                            " AND PUB_m_extract_master.m_extract_name_c = {param_extract_name}" +
                            " AND";
                    }
                    else
                    {
                        Query += " WHERE";
                    }

                    Query +=
                        " detail.a_ledger_number_i = " + LedgerNumber +
                        " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                        " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i" +
                        " AND (SELECT SUM(detail2." + Currency + ") FROM PUB_a_gift_detail AS detail2" +
                        " WHERE detail2.a_ledger_number_i = detail.a_ledger_number_i" +
                        " AND detail2.a_batch_number_i = detail.a_batch_number_i" +
                        " AND detail2.a_gift_transaction_number_i = detail.a_gift_transaction_number_i)" + " >= " + MinimumAmount.ToString();

                    if (!string.IsNullOrEmpty(GiftsInRange))
                    {
                        string[] GiftsInRangeArray = GiftsInRange.Split(',');

                        foreach (string Range in GiftsInRangeArray)
                        {
                            Query += " AND (gift.a_date_entered_d BETWEEN '" + Range.Substring(0, 10) + "' AND '" + Range.Substring(13,
                                10) + "') AND";
                        }
                    }

                    Query += " gift.a_ledger_number_i = " + LedgerNumber +
                             " AND PUB_p_partner.p_partner_key_n = gift.p_donor_key_n" +
                             " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'" +
                             " AND PUB_a_gift_batch.a_batch_number_i = gift.a_batch_number_i" +
                             " AND PUB_a_gift_batch.a_ledger_number_i = " + LedgerNumber +

                             " AND NOT EXISTS (SELECT *" +
                             " FROM PUB_a_gift, PUB_a_gift_detail, PUB_a_gift_batch" +
                             " WHERE";

                    if (!string.IsNullOrEmpty(NoGiftsInRange))
                    {
                        string[] NoGiftsInRangeArray = NoGiftsInRange.Split(',');

                        foreach (string Range in NoGiftsInRangeArray)
                        {
                            Query += " (PUB_a_gift.a_date_entered_d BETWEEN '" + Range.Substring(0, 10) + "' AND '" + Range.Substring(13,
                                10) + "') AND";
                        }
                    }

                    Query += " PUB_a_gift.a_ledger_number_i = " + LedgerNumber +
                             " AND PUB_a_gift.p_donor_key_n = PUB_p_partner.p_partner_key_n" +
                             " AND PUB_a_gift_detail.a_ledger_number_i = " + LedgerNumber +
                             " AND PUB_a_gift_detail.a_batch_number_i = PUB_a_gift.a_batch_number_i" +
                             " AND PUB_a_gift_detail.a_gift_transaction_number_i = PUB_a_gift.a_gift_transaction_number_i" +

                             " AND (SELECT SUM(detail3." + Currency + ") FROM PUB_a_gift_detail AS detail3" +
                             " WHERE detail3.a_ledger_number_i = PUB_a_gift_detail.a_ledger_number_i" +
                             " AND detail3.a_batch_number_i = PUB_a_gift_detail.a_batch_number_i" +
                             " AND detail3.a_gift_transaction_number_i = PUB_a_gift_detail.a_gift_transaction_number_i)" + " >= " +
                             MinimumAmount.ToString() +

                             " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'" +
                             " AND PUB_a_gift_batch.a_batch_number_i = PUB_a_gift.a_batch_number_i" +
                             " AND PUB_a_gift_batch.a_ledger_number_i = " + LedgerNumber +
                             ")" +

                             " ORDER BY PartnerKey, LastGiftDate DESC";

                    Results = DbAdapter.RunQuery(Query, "Results", Transaction);

                    if (DbAdapter.IsCancelled)
                    {
                        Results = null;
                        return;
                    }

                    ReturnTable = Results.Clone();
                    ReturnTable.Columns.Add("Number", typeof(Int32));

                    int i = 0;

                    // Only keep the first gift for each donor (it will be the newest)
                    // Get the total number of gifts for a donor
                    while (i < Results.Rows.Count)
                    {
                        int NumberDetails = Results.Select("PartnerKey = '" + Results.Rows[i]["PartnerKey"] + "'").Length;
                        int NumberGifts = Results.Select("PartnerKey = '" + Results.Rows[i]["PartnerKey"] + "' AND Detail = '1'").Length;
                        decimal Amount = 0;

                        DataRow[] Gifts =
                            Results.Select("PartnerKey = '" + Results.Rows[i]["PartnerKey"] + "' AND Ledger = '" + Results.Rows[i]["Ledger"] +
                                "' AND Batch = '" + Results.Rows[i]["Batch"] + "' AND GiftTransaction = '" + Results.Rows[i]["GiftTransaction"] + "'");

                        foreach (DataRow Gift in Gifts)
                        {
                            Amount += Convert.ToInt32(Gift["LastGiftAmount"]);
                        }

                        DataRow NewRow = ReturnTable.NewRow();
                        NewRow["PartnerKey"] = Results.Rows[i]["PartnerKey"];
                        NewRow["PartnerName"] = Results.Rows[i]["PartnerName"];
                        NewRow["PartnerClass"] = Results.Rows[i]["PartnerClass"];
                        NewRow["LastGiftDate"] = Results.Rows[i]["LastGiftDate"];
                        NewRow["MotivationGroup"] = Results.Rows[i]["MotivationGroup"];
                        NewRow["MotivationDetail"] = Results.Rows[i]["MotivationDetail"];
                        NewRow["LastGiftAmount"] = Amount;
                        NewRow["Number"] = NumberGifts;
                        ReturnTable.Rows.Add(NewRow);

                        i += NumberDetails;
                    }
                });

            return ReturnTable;
        }
    }
}