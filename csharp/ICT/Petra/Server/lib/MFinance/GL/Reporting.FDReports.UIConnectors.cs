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
using Ict.Petra.Server.MPartner.DataAggregates;
using Ict.Petra.Server.App.Core.Security;

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
                            " AND PUB_m_extract_master.m_extract_name_c = '" + AParameters["param_extract_name"].ToString() + "'" + // {param_extract_name}" +
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

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet GiftsOverMinimum(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            String StartDate = AParameters["param_start_date"].ToDate().ToString("yyyy-MM-dd");
            String EndDate = AParameters["param_end_date"].ToDate().ToString("yyyy-MM-dd");
            String MinimumAmount = AParameters["param_minimum_amount"].ToString();
            String CurrencyField = (AParameters["param_currency"].ToString().ToUpper() == "BASE" ? "a_gift_amount_n" : "a_gift_amount_intl_n");
            String DonorExclude = "";
            String MotivationQuery = "";

            TDBTransaction Transaction = null;
            DataTable Gifts = new DataTable();
            DataTable Donors = new DataTable();
            DataTable Contacts = new DataTable();
            DataSet Results = new DataSet();

            if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
            {
                foreach (String key in AParameters.Keys)
                {
                    TLogWriter.Log(key + " => " + AParameters[key].ToString());
                }
            }

            if (AParameters["param_exclude_anonymous_donors"].ToBool())
            {
                DonorExclude += "AND Donor.p_anonymous_donor_l = 0 ";
            }

            if (AParameters["param_exclude_no_solicitations"].ToBool())
            {
                DonorExclude += "AND Donor.p_no_solicitations_l = 0 ";
            }

            if (!AParameters["param_all_motivation_groups"].ToBool())
            {
                MotivationQuery += String.Format("AND a_gift_detail.a_motivation_group_code_c IN ({0}) ",
                    AParameters["param_motivation_group_quotes"]);
            }

            if (!AParameters["param_all_motivation_details"].ToBool())
            {
                MotivationQuery += String.Format("AND (a_gift_detail.a_motivation_group_code_c, a_gift_detail.a_motivation_detail_code_c) IN ({0}) ",
                    AParameters["param_motivation_group_detail_pairs"]);
            }

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"
                        WITH Details AS (
                            SELECT
                                a_gift.p_donor_key_n AS DonorKey
                                -- We need to join to Donor to check p_anonymous_donor_l and p_no_solicitations_l for the query, so we may as well pull Name etc.
                                -- at the same time, to avoid doing a second Donor query later. But we'll consolidate this duplicated data into another DataTable
                                -- to return it to the client.
                                , Donor.p_partner_short_name_c AS DonorName
                                , Donor.p_partner_class_c AS DonorClass
                                , Donor.p_receipt_letter_frequency_c AS ReceiptFrequency
                                , a_gift.a_date_entered_d AS GiftDate
                                , a_gift_detail.a_confidential_gift_flag_l AS Confidential
                                , a_gift_detail.a_motivation_group_code_c AS MotivationGroup
                                , a_gift_detail.a_motivation_detail_code_c AS MotivationDetail
                                , a_motivation_group.a_motivation_group_description_c AS MotivationGroupDescription
                                , a_motivation_detail.a_motivation_detail_desc_c AS MotivationDetailDescription
                                , a_gift_detail.p_recipient_key_n AS RecipientKey
                                , a_gift_detail."
                        +
                        CurrencyField + @" AS GiftAmount
                                , sum(a_gift_detail."                                                          + CurrencyField +
                        @") OVER (PARTITION BY a_gift.p_donor_key_n) AS TotalAmount
                            FROM
                                a_gift
                            INNER JOIN
                                p_partner AS Donor
                            ON
                                (Donor.p_partner_key_n = a_gift.p_donor_key_n)
                            INNER JOIN
                                a_gift_detail
                            USING
                                (a_ledger_number_i, a_batch_number_i, a_gift_transaction_number_i)
                            INNER JOIN
                                a_motivation_group
                            USING
                                (a_ledger_number_i, a_motivation_group_code_c)
                            INNER JOIN
                                a_motivation_detail
                            USING
                                (a_ledger_number_i, a_motivation_group_code_c, a_motivation_detail_code_c)
                            WHERE
                                a_gift.a_ledger_number_i = "
                        +
                        LedgerNumber + @"
                                AND a_gift.a_date_entered_d BETWEEN '"                                           + StartDate + "' AND '" +
                        EndDate +
                        @"'
                                -- I hope a_dont_report_l gets converted to a_report_l to avoid this horrible double negative:
                                AND a_motivation_detail.a_dont_report_l = False
                                "
                        +
                        MotivationQuery + DonorExclude +
                        @"
                                -- For OM Germany, exclude donors 99000000 and 27002909 (SHKI and anonymous UNBEKANNT)
                                AND NOT ((a_gift.a_ledger_number_i = 27 OR a_gift.a_ledger_number_i = 90 OR a_gift.a_ledger_number_i = 99)
                                    AND (a_gift.p_donor_key_n = 99000000 OR a_gift.p_donor_key_n = 27002909))
                        )
                        SELECT
                            Details.*
                            , Recipient.p_partner_short_name_c AS RecipientName
                        FROM
                            Details
                        INNER JOIN
                            p_partner AS Recipient
                        ON
                            (Recipient.p_partner_key_n = Details.RecipientKey)
                        WHERE
                            TotalAmount >= "
                        +
                        MinimumAmount +
                        @"
                        ORDER BY
                            Details.DonorName
                        ;
                    "                                                                                                                                    ;

                    Gifts = DbAdapter.RunQuery(Query, "GiftsOverMinimum", Transaction);

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        TLogWriter.Log("Query finished");
                    }

                    // Get the donors' addresses. Thought about using enum instead of const, but enums 1) have to be declared right at the top in the class declaration where
                    // it's easy to forget to add an item and 2) have to be cast to back int before use in array index, making the use uglier than constants.
                    const int DONOR_KEY = 0;
                    const int DONOR_ADDR = 5;
                    const int DONOR_POSTCODE = 6;
                    //const int DONOR_PHONE = 7;
                    //const int DONOR_EMAIL = 8;
                    Donors = Gifts.DefaultView.ToTable("Donors", true, "DonorKey", "DonorName", "DonorClass", "ReceiptFrequency", "TotalAmount");
                    Donors.Columns.Add("Address", typeof(String));
                    Donors.Columns.Add("PostalCode", typeof(String));
                    Donors.Columns.Add("Phone", typeof(String));
                    Donors.Columns.Add("Email", typeof(String));

                    // Having copied the distinct names and totals from Gifts to Donors, we no longer need to pass their duplicated data back to the client
                    foreach (String col in new String[] { "DonorName", "DonorClass", "ReceiptFrequency", "TotalAmount" })
                    {
                        Gifts.Columns.Remove(col);
                    }

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        TLogWriter.Log("Getting donor addresses");
                    }

                    DataTable DonorAddresses = TAddressTools.GetBestAddressForPartners(Donors, 0, Transaction);
                    DataRow[] AddressRows;
                    DataRow Addr;

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        TLogWriter.Log("Finished");
                    }

                    //String EmailAddress, PhoneNumber, FaxNumber;
                    List <String>DonorList = new List <string>();

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        TLogging.Log("Processing addresses");
                    }

                    foreach (DataRow Donor in Donors.Rows)
                    {
                        DonorList.Add(Donor[DONOR_KEY].ToString());

                        AddressRows = DonorAddresses.Select("p_partner_key_n = " + Donor[DONOR_KEY]);

                        if (AddressRows.Length > 0)
                        {
                            Addr = AddressRows[0];
                            Donor[DONOR_ADDR] = Calculations.DetermineLocationString(Addr["p_building_1_c"].ToString(),
                                Addr["p_building_2_c"].ToString(),
                                Addr["p_locality_c"].ToString(),
                                Addr["p_street_name_c"].ToString(),
                                Addr["p_address_3_c"].ToString(),
                                Addr["p_suburb_c"].ToString(),
                                Addr["p_city_c"].ToString(),
                                Addr["p_county_c"].ToString(),
                                Addr["p_postal_code_c"].ToString(),
                                Addr["p_country_code_c"].ToString(),
                                Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
                            Donor[DONOR_POSTCODE] = Addr["p_postal_code_c"];
                        }
                        else
                        {
                            Donor[DONOR_ADDR] = "";
                            Donor[DONOR_POSTCODE] = "";
                        }

                        // Phone and email were not required after all (https://tracker.openpetra.org/view.php?id=4955) and they seriously slow down the
                        // report. If they are reinstated, it should be as a single SQL query for all donors (similar to TAddressTools.GetBestAddressForPartners
                        // above), and not iterating hundreds of separate queries as it was here.
                        //TContactDetailsAggregate.GetPrimaryEmailAndPrimaryPhoneAndFax((Int64)Donor[DONOR_KEY], out PhoneNumber, out EmailAddress,
                        //    out FaxNumber);
                        //Donor[DONOR_PHONE] = PhoneNumber;
                        //Donor[DONOR_EMAIL] = EmailAddress;
                    }

                    if (DonorList.Count == 0)
                    {
                        DonorList.Add("null");
                    }

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        TLogging.Log(
                            "Addresses finished");
                    }

                    // Get the most recent contacts with each donor
                    Query =
                        @"
                        WITH Contacts AS (
                            SELECT
                                row_number() OVER (PARTITION BY p_partner_contact.p_partner_key_n ORDER BY s_contact_date_d DESC, s_contact_time_i DESC) AS RowID
                                , p_partner_contact.p_partner_key_n AS DonorKey
                                , p_contact_log.p_contactor_c AS Contactor
                                , p_contact_log.s_contact_date_d AS ContactDate
                                , p_contact_log.s_contact_time_i AS ContactTime
                                , p_contact_log.s_contact_time_i * '1 second'::interval AS Time
                                , p_contact_log.p_contact_code_c AS ContactCode
                                , p_contact_log.p_contact_comment_c AS Comment
                            FROM
                                p_partner_contact
                            INNER JOIN
                                p_contact_log
                            USING
                                (p_contact_log_id_i)
                            WHERE
                                p_partner_key_n in ("
                        +
                        String.Join(",",
                            DonorList) +
                        @")
                            ORDER BY
                                DonorKey,
                                RowID
                        )
                        SELECT
                            *
                        FROM
                            Contacts
                        WHERE
                            Contacts.RowID <= "
                        +
                        AParameters["param_max_contacts"] + @";
                    "                                                                ;

                    Contacts = DbAdapter.RunQuery(Query, "Contacts", Transaction);

                    if (TLogging.DebugLevel >= DBAccess.DB_DEBUGLEVEL_QUERY)
                    {
                        TLogWriter.Log("Query finished");
                    }

                    if (DbAdapter.IsCancelled)
                    {
                        Results = null;
                        return;
                    }
                }); // GetNewOrExistingAutoReadTransaction
            Results.Tables.Add(Gifts);
            Results.Tables.Add(Donors);
            Results.Tables.Add(Contacts);
            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        /// <param name="AParameters">Parameter dictionary</param>
        /// <param name="DbAdapter"></param>
        /// <returns></returns>
        [NoRemoting]
        public static DataSet TopDonorReport(Dictionary<string, TVariant> AParameters, TReportingDbAdapter DbAdapter)
        {
            DataSet ReturnDataSet = new DataSet();

            TDBTransaction Transaction = null;
            DataTable dt = new DataTable("TopDonorReport");

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
            IsolationLevel.ReadCommitted,
            TEnforceIsolationLevel.eilMinimum,
            ref Transaction,
            delegate
            {
                String MotivationQuery = "";
                if (!AParameters["param_all_motivation_groups"].ToBool())
                {
                    MotivationQuery += String.Format("AND detail.a_motivation_group_code_c IN ({0}) ",
                        AParameters["param_motivation_group_quotes"]);
                }

                if (!AParameters["param_all_motivation_details"].ToBool())
                {
                    MotivationQuery += String.Format("AND (detail.a_motivation_group_code_c, detail.a_motivation_detail_code_c) IN ({0}) ",
                        AParameters["param_motivation_group_detail_pairs"]);
                }

                String giftAmountColumn;
                if (AParameters["param_currency"].ToString() == "International")
                {
                    giftAmountColumn = "a_gift_amount_intl_n";
                }
                else
                {
                    giftAmountColumn = "a_gift_amount_n";
                }

                String recipientKey = "";
                if(AParameters["param_recipientkey"].ToString() != "0")
                {
                    recipientKey = " AND a_recipient_ledger_number_n = " + AParameters["param_recipientkey"].ToString();
                }
                

                String Query =
                    @"WITH GiftTotals AS (
	                    SELECT p_donor_key_n AS donorKey, p_partner_class_c AS partnerClass, p_partner_short_name_c AS donorName, 
		                SUM(detail." + giftAmountColumn + @") AS totalamount


                        FROM a_gift AS gift, a_gift_batch, a_gift_detail AS detail, p_partner";
                //Add extract parameter
                if (AParameters["param_extract"].ToBool())
                {
                     Query +=
                              ", m_extract," +
                              " m_extract_master" +
                              " WHERE" +
                             " p_donor_key_n = m_extract.p_partner_key_n" +
                             " AND m_extract.m_extract_id_i = m_extract_master.m_extract_id_i" +
                              " AND m_extract_master.m_extract_name_c = '" + AParameters["param_extract_name"].ToString() + "'" + 
                              " AND";
                }
                else
                {
                    Query += " WHERE";
                }

                Query += " detail.a_ledger_number_i = gift.a_ledger_number_i" +
                            recipientKey +
		                    @" AND detail.a_batch_number_i = gift.a_batch_number_i
		                    AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i
		                    AND gift.a_date_entered_d BETWEEN '" + AParameters["param_start_date"].ToDate().ToString("yyyy-MM-dd") + @"' AND '" +
                            AParameters["param_end_date"].ToDate().ToString("yyyy-MM-dd") + @"' AND gift.a_ledger_number_i = a_gift_batch.a_ledger_number_i
		                    AND a_gift_batch.a_ledger_number_i =" + AParameters["param_ledger_number_i"] +
                            @" AND a_gift_batch.a_batch_number_i = gift.a_batch_number_i
                            AND ( a_gift_batch.a_batch_status_c = 'Posted' OR a_gift_batch.a_batch_status_c = 'posted')
		                    AND p_partner.p_partner_key_n = gift.p_donor_key_n " + MotivationQuery +
                        @" GROUP BY gift.p_donor_key_n, p_partner.p_partner_class_c, p_partner.p_partner_short_name_c

	                    ORDER BY totalamount DESC
                    ), 
                        CumulativeTotals AS (
	                        SELECT *,
		                        SUM(totalamount) OVER (ORDER BY totalamount DESC) AS CumulativeTotal,
		                        SUM(totalamount) OVER () AS GrandTotal,
		                        ROUND(totalamount / (SUM(totalamount) OVER ()) * 100, 2) AS PercentTotal,
		                        ROUND((SUM(totalamount) OVER (ORDER BY totalamount DESC)) / (SUM(totalamount) OVER ()) * 100, 2) AS PercentCumulative
	                        FROM GiftTotals WHERE totalamount >= 0
                        )
                SELECT *
                FROM CumulativeTotals WHERE PercentCumulative";

                switch (AParameters["param_donor_type"].ToString())
                {
                    case "top":
                        Query += " < " + AParameters["param_percentage"];
                        break;
                    case "bottom":
                        Query += " > " + AParameters["param_to_percentage"];
                        break;
                    case "middle":
                        Query += " BETWEEN " + AParameters["param_to_percentage"] + " AND " + AParameters["param_percentage"];
                        break;
                    default:
                        Query += " IS NOT 0";
                        break;
                }

                //Add the first entry that is param_percentage or higher
                Query += String.Format(" UNION ALL SELECT * FROM CumulativeTotals WHERE PercentCumulative = (SELECT MIN(percentcumulative) FROM CumulativeTotals WHERE percentcumulative >= {0})", AParameters["param_percentage"]);

                dt = DbAdapter.RunQuery(Query, "TopDonorReport", Transaction);
            });

            DataTable DonorAddresses = new DataTable("DonorAddresses");
            foreach (DataRow Row in dt.Rows)
            {
                // get best address for donor
                Int64 DonorKey = (Int64)Row["DonorKey"];
                DataTable tempTable = TFinanceReportingWebConnector.GiftStatementDonorAddressesTable(DbAdapter, DonorKey);

                if (tempTable != null)
                {
                    DonorAddresses.Merge(tempTable);
                }

                if (DbAdapter.IsCancelled)
                {
                    return null;
                }
            }

            ReturnDataSet.Tables.Add(dt);
            ReturnDataSet.Tables.Add(DonorAddresses);
            return ReturnDataSet;

        }
    }
}