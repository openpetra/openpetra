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
    /// This WebConnector provides data for the Gift reporting screens
    ///</summary>
    public partial class TFinanceReportingWebConnector
    {
        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable GiftBatchDetailTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            int BatchNumber = AParameters["param_batch_number_i"].ToInt32();

            // create new datatable
            DataTable Results = new DataTable();

            DbAdapter.FPrivateDatabaseObj.BeginAutoReadTransaction(
                ref Transaction,
                delegate
                {
                    String CurrentDate = DateTime.Today.ToString(
                        "yyyy-MM-dd");

                    string Query =
                        "SELECT DISTINCT a_gift_batch.a_batch_description_c, a_gift_batch.a_batch_status_c, a_gift_batch.a_gift_type_c, a_gift_batch.a_gl_effective_date_d, "
                        +
                        "a_gift_batch.a_bank_cost_centre_c, a_gift_batch.a_bank_account_code_c, a_gift_batch.a_currency_code_c, a_gift_batch.a_hash_total_n, a_gift_batch.a_batch_total_n, "
                        +
                        "a_gift_detail.a_gift_transaction_number_i, a_gift_detail.a_detail_number_i, a_gift_detail.a_confidential_gift_flag_l, " +
                        "a_gift_detail.p_recipient_key_n, a_gift_detail.a_gift_amount_n, a_gift_detail.a_gift_amount_intl_n, a_gift_detail.a_gift_transaction_amount_n, "
                        +
                        "a_gift_detail.a_motivation_group_code_c, a_gift_detail.a_motivation_detail_code_c, a_gift_detail.a_recipient_ledger_number_n, "
                        +
                        "a_gift_detail.a_gift_comment_one_c, a_gift_detail.a_gift_comment_two_c, a_gift_detail.a_gift_comment_three_c, a_gift_detail.a_tax_deductible_pct_n, "
                        +
                        "a_gift.p_donor_key_n AS DonorKey, a_gift.a_reference_c AS GiftReference, a_gift.a_method_of_giving_code_c, a_gift.a_method_of_payment_code_c, "
                        +
                        "a_gift.a_receipt_letter_code_c, a_gift.a_date_entered_d, a_gift.a_first_time_gift_l, a_gift.a_receipt_number_i, " +
                        "Donor.p_partner_class_c AS DonorClass, Donor.p_partner_short_name_c AS DonorShortName, Donor.p_receipt_letter_frequency_c, Donor.p_receipt_each_gift_l, "
                        +
                        "Recipient.p_partner_class_c AS RecipientClass, Recipient.p_partner_short_name_c AS RecipientShortName, " +
                        "a_gift_detail.p_mailing_code_c AS MailingCode, " +
                        "a_gift_detail.a_charge_flag_l AS ChargeFlag, "
                        +
                        // true if donor has a valid Ex-Worker special type
                        "CASE WHEN EXISTS (SELECT p_partner_type.* FROM p_partner_type WHERE " +
                        "p_partner_type.p_partner_key_n = a_gift.p_donor_key_n" +
                        " AND (p_partner_type.p_valid_from_d IS null OR p_partner_type.p_valid_from_d <= '" + CurrentDate + "')" +
                        " AND (p_partner_type.p_valid_until_d IS null OR p_partner_type.p_valid_until_d >= '" + CurrentDate + "')" +
                        " AND p_partner_type.p_type_code_c LIKE '" +
                        TSystemDefaults.GetStringDefault(SharedConstants.SYSDEFAULT_EXWORKERSPECIALTYPE, "EX-WORKER") + "%'" +
                        ") THEN True ELSE False END AS EXWORKER, " +

                        // true if the gift is restricted for the user
                        "CASE WHEN EXISTS (SELECT s_user_group.* FROM s_user_group " +
                        "WHERE a_gift.a_restricted_l IS true" +
                        " AND NOT EXISTS (SELECT s_group_gift.s_read_access_l FROM s_group_gift, s_user_group " +
                        "WHERE s_group_gift.s_read_access_l" +
                        " AND s_group_gift.a_ledger_number_i = " + LedgerNumber +
                        " AND s_group_gift.a_batch_number_i = " + BatchNumber +
                        " AND s_group_gift.a_gift_transaction_number_i = a_gift_detail.a_gift_transaction_number_i" +
                        " AND s_user_group.s_user_id_c = '" + UserInfo.GUserInfo.UserID + "'" +
                        " AND s_user_group.s_group_id_c = s_group_gift.s_group_id_c" +
                        " AND s_user_group.s_unit_key_n = s_group_gift.s_group_unit_key_n)" +
                        ") THEN False ELSE True END AS ReadAccess " +

                        "FROM a_gift_batch, a_gift_detail, a_gift, p_partner AS Donor, p_partner AS Recipient " +

                        "WHERE a_gift_batch.a_ledger_number_i = " + LedgerNumber + " AND a_gift_batch.a_batch_number_i = " + BatchNumber +
                        " AND a_gift.a_ledger_number_i = " + LedgerNumber + " AND a_gift.a_batch_number_i = " + BatchNumber +
                        " AND a_gift_detail.a_ledger_number_i = " + LedgerNumber + " AND a_gift_detail.a_batch_number_i = " +
                        BatchNumber +
                        " AND a_gift.a_gift_transaction_number_i = a_gift_detail.a_gift_transaction_number_i " +
                        " AND Donor.p_partner_key_n = a_gift.p_donor_key_n" +
                        " AND Recipient.p_partner_key_n = a_gift_detail.p_recipient_key_n";

                    Results = DbAdapter.RunQuery(Query, "Results", Transaction);
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable RecipientGiftStatementRecipientTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string RecipientSelection = AParameters["param_recipient"].ToString();
            string OrderBy = AParameters["param_order_by_name"].ToString();

/*
 *          string ReportType = string.Empty;
 *
 *          if (AParameters.ContainsKey("param_report_type"))
 *          {
 *              ReportType = AParameters["param_report_type"].ToString();
 *          }
 */
            DateTime CurrentDate = DateTime.Today;

            // create new datatable
            DataTable Results = new DataTable();

            DbAdapter.FPrivateDatabaseObj.BeginAutoReadTransaction(
                ref Transaction,
                delegate
                {
                    string Query = "SELECT DISTINCT" +
                                   " Recipient.p_partner_key_n AS RecipientKey," +
                                   " Recipient.p_partner_short_name_c AS RecipientName," +
                                   " Recipient.p_partner_class_c AS RecipientClass," +

                                   " CASE WHEN EXISTS (SELECT 1 FROM PUB_p_partner WHERE PUB_p_partner.p_partner_key_n = PUB_p_partner_gift_destination.p_field_key_n"
                                   +
                                   " OR PUB_p_partner.p_partner_key_n = um_unit_structure.um_child_unit_key_n)" +
                                   " THEN PUB_p_partner.p_partner_short_name_c " +
                                   " ELSE 'UNKNOWN'" +
                                   " END AS FieldName," +

                                   " CASE WHEN EXISTS (SELECT 1 FROM PUB_p_partner WHERE PUB_p_partner.p_partner_key_n = PUB_p_partner_gift_destination.p_field_key_n"
                                   +
                                   " OR PUB_p_partner.p_partner_key_n = um_unit_structure.um_child_unit_key_n)" +
                                   " THEN PUB_p_partner.p_partner_key_n " +
                                   " ELSE 0" +
                                   " END AS FieldKey" +

                                   " FROM" +
                                   " PUB_a_gift as gift, " +
                                   " PUB_a_gift_detail AS detail," +
                                   " PUB_a_gift_batch," +
                                   " PUB_p_partner AS Recipient" +

                                   " LEFT JOIN PUB_p_partner_gift_destination" +
                                   " ON Recipient.p_partner_class_c = 'FAMILY'" +
                                   " AND PUB_p_partner_gift_destination.p_partner_key_n = Recipient.p_partner_key_n" +
                                   " AND PUB_p_partner_gift_destination.p_date_effective_d <= '" + CurrentDate.ToString("yyyy-MM-dd") + "'" +
                                   " AND (PUB_p_partner_gift_destination.p_date_expires_d IS NULL" +
                                   " OR (PUB_p_partner_gift_destination.p_date_expires_d >= '" + CurrentDate.ToString(
                        "yyyy-MM-dd") + "'" +
                                   " AND PUB_p_partner_gift_destination.p_date_effective_d <> PUB_p_partner_gift_destination.p_date_expires_d))" +

                                   " LEFT JOIN um_unit_structure" +
                                   " ON Recipient.p_partner_class_c = 'UNIT'" +
                                   " AND um_unit_structure.um_child_unit_key_n = Recipient.p_partner_key_n" +

                                   " LEFT JOIN PUB_p_partner" +
                                   " ON (PUB_p_partner.p_partner_key_n = PUB_p_partner_gift_destination.p_field_key_n" +
                                   " AND EXISTS (SELECT * FROM PUB_p_partner_gift_destination WHERE PUB_p_partner_gift_destination.p_partner_key_n = Recipient.p_partner_key_n))"
                                   +
                                   " OR (PUB_p_partner.p_partner_key_n = um_unit_structure.um_parent_unit_key_n" +
                                   " AND EXISTS (SELECT * FROM um_unit_structure WHERE um_unit_structure.um_child_unit_key_n = Recipient.p_partner_key_n))";

                    if (RecipientSelection == "Extract")
                    {
                        Query += ", PUB_m_extract," +
                                 " PUB_m_extract_master";
                    }

                    Query += " WHERE";

                    if (RecipientSelection == "Extract")
                    {
                        Query += " detail.p_recipient_key_n =  PUB_m_extract.p_partner_key_n" +
                                 " AND PUB_m_extract.m_extract_id_i = PUB_m_extract_master.m_extract_id_i" +
                                 " AND PUB_m_extract_master.m_extract_name_c = '" + AParameters["param_extract_name"].ToString() + "'" +
                                 " AND";
                    }

                    Query += " detail.a_ledger_number_i = gift.a_ledger_number_i" +
                             " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                             " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i" +
                             " AND gift.a_date_entered_d BETWEEN '" + AParameters["param_from_date"].ToDate().ToString("yyyy-MM-dd") +
                             "' AND '" + AParameters["param_to_date"].ToDate().ToString("yyyy-MM-dd") + "'" +
                             " AND gift.a_ledger_number_i = " + LedgerNumber +

                             " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'" +
                             " AND PUB_a_gift_batch.a_batch_number_i = gift.a_batch_number_i" +
                             " AND PUB_a_gift_batch.a_ledger_number_i = " + LedgerNumber +

                             " AND Recipient.p_partner_key_n = detail.p_recipient_key_n";

                    if (RecipientSelection == "One Recipient")
                    {
                        Query += " AND detail.p_recipient_key_n = " + AParameters["param_recipientkey"].ToInt64();
                    }

                    if (OrderBy == "RecipientField")
                    {
                        Query += " ORDER BY FieldName, RecipientKey";
                    }
                    else if (OrderBy == "RecipientKey")
                    {
                        Query += " ORDER BY RecipientKey";
                    }
                    else if (OrderBy == "RecipientName")
                    {
                        Query += " ORDER BY RecipientName";
                    }

                    Results = DbAdapter.RunQuery(Query, "Recipients", Transaction);
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable RecipientGiftStatementTotalsTable(Dictionary <String, TVariant>AParameters,
            Int64 ARecipientKey,
            TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            int CurrentYear = AParameters["param_from_date"].ToDate().Year;
            string Currency = AParameters["param_currency"].ToString().ToUpper() == "BASE" ? "a_gift_amount_n" : "a_gift_amount_intl_n";

            // create new datatable
            DataTable Results = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    //TODO: Calendar vs Financial Date Handling - Check if this should use financial year start/end in all places below
                    string Query = "SELECT " +
                                   " GiftDetail.p_recipient_key_n AS RecipientKey," +

                                   " SUM (" +
                                   " CASE WHEN" +
                                   " Gift.a_date_entered_d >= '" + new DateTime(CurrentYear - 1, 1, 1).ToString("yyyy-MM-dd") + "'" +
                                   " AND Gift.a_date_entered_d <= '" + new DateTime(CurrentYear - 1, 12, 31).ToString("yyyy-MM-dd") + "'" +
                                   " THEN GiftDetail." + Currency +
                                   " ELSE 0" +
                                   " END) AS PreviousYearTotal," +

                                   " SUM (" +
                                   " CASE WHEN" +
                                   " Gift.a_date_entered_d >= '" + new DateTime(CurrentYear, 1, 1).ToString("yyyy-MM-dd") + "'" +
                                   " AND Gift.a_date_entered_d <= '" + AParameters["param_to_date"].ToDate().ToString("yyyy-MM-dd") + "'" +
                                   " THEN GiftDetail." + Currency +
                                   " ELSE 0" +
                                   " END) AS CurrentYearTotal" +

                                   " FROM" +
                                   " PUB_a_gift AS Gift, " +
                                   " PUB_a_gift_detail AS GiftDetail," +
                                   " PUB_a_gift_batch AS GiftBatch" +

                                   " WHERE" +

                                   " GiftDetail.a_ledger_number_i = " + LedgerNumber +
                                   " AND GiftDetail.p_recipient_key_n = " + ARecipientKey +
                                   " AND Gift.a_ledger_number_i = " + LedgerNumber +
                                   " AND Gift.a_batch_number_i = GiftDetail.a_batch_number_i" +
                                   " AND Gift.a_gift_transaction_number_i = GiftDetail.a_gift_transaction_number_i" +
                                   " AND ((Gift.a_date_entered_d >= '" + new DateTime(CurrentYear - 1, 1, 1).ToString("yyyy-MM-dd") + "'" +
                                   " AND Gift.a_date_entered_d <= '" + new DateTime(CurrentYear - 1, 12, 31).ToString("yyyy-MM-dd") + "')" +
                                   " OR (Gift.a_date_entered_d >= '" + new DateTime(CurrentYear, 1, 1).ToString("yyyy-MM-dd") + "'" +
                                   " AND Gift.a_date_entered_d <= '" + AParameters["param_to_date"].ToDate().ToString("yyyy-MM-dd") + "'))" +
                                   " AND GiftBatch.a_ledger_number_i = " + LedgerNumber +
                                   " AND GiftBatch.a_batch_number_i = Gift.a_batch_number_i" +
                                   " AND GiftBatch.a_batch_status_c = 'Posted'" +

                                   " GROUP BY GiftDetail.p_recipient_key_n";

                    Results = DbAdapter.RunQuery(Query, "RecipientTotals", Transaction);
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable RecipientGiftStatementDonorTable(Dictionary <String, TVariant>AParameters,
            Int64 ARecipientKey,
            TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string ReportType = AParameters["param_report_type"].ToString();
            string Currency = "";

            if ((ReportType == "List") || (ReportType == "Email"))
            {
                Currency = "a_gift_transaction_amount_n";
            }
            else
            {
                Currency = AParameters["param_currency"].ToString().ToUpper() == "BASE" ? "a_gift_amount_n" : "a_gift_amount_intl_n";
            }

            // create new datatable
            DataTable Results = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    string Query = "SELECT" +
                                   " gift.a_date_entered_d AS GiftDate," +
                                   " gift.p_donor_key_n AS DonorKey," +
                                   " CASE WHEN DonorPartner.p_partner_short_name_c NOT LIKE ''" +
                                   " THEN DonorPartner.p_partner_short_name_c" +
                                   " ELSE '" + Catalog.GetString("Unknown Donor") + "' END AS DonorName," +
                                   " DonorPartner.p_partner_class_c AS DonorClass," +
                                   " detail.p_recipient_key_n AS RecipientKey," +
                                   " detail.a_motivation_detail_code_c AS MotivationCode," +
                                   " detail.a_confidential_gift_flag_l AS Confidential," +
                                   " detail." + Currency + " AS GiftAmount," +
                                   " gift.a_receipt_number_i AS Receipt," +
                                   " PUB_a_gift_batch.a_currency_code_c AS GiftCurrency," +
                                   " RecipientLedgerPartner.p_partner_short_name_c AS GiftField," +

                                   " CASE WHEN" +
                                   " (UPPER(detail.a_comment_one_type_c) = 'RECIPIENT' OR UPPER(detail.a_comment_one_type_c) = 'BOTH')" +
                                   " AND '" + ReportType + "' = 'Complete'" +
                                   " THEN detail.a_gift_comment_one_c" +
                                   " ELSE ''" +
                                   " END AS CommentOne," +
                                   " CASE WHEN" +
                                   " UPPER(detail.a_comment_one_type_c) = 'RECIPIENT' OR UPPER(detail.a_comment_one_type_c) = 'BOTH'" +
                                   " AND '" + ReportType + "' = 'Complete'" +
                                   " THEN detail.a_gift_comment_two_c" +
                                   " ELSE ''" +
                                   " END AS CommentTwo," +
                                   " CASE WHEN" +
                                   " UPPER(detail.a_comment_one_type_c) = 'RECIPIENT' OR UPPER(detail.a_comment_one_type_c) = 'BOTH'" +
                                   " AND '" + ReportType + "' = 'Complete'" +
                                   " THEN detail.a_gift_comment_three_c" +
                                   " ELSE ''" +
                                   " END AS CommentThree" +

                                   " FROM" +
                                   " PUB_a_gift as gift," +
                                   " PUB_a_gift_detail as detail," +
                                   " PUB_a_gift_batch," +
                                   " PUB_p_partner AS DonorPartner," +
                                   " PUB_p_partner AS RecipientLedgerPartner" +

                                   " WHERE" +
                                   " detail.a_ledger_number_i = gift.a_ledger_number_i" +
                                   " AND detail.p_recipient_key_n = " + ARecipientKey +
                                   " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'" +
                                   " AND PUB_a_gift_batch.a_batch_number_i = gift.a_batch_number_i" +
                                   " AND PUB_a_gift_batch.a_ledger_number_i = " + LedgerNumber +
                                   " AND gift.a_date_entered_d BETWEEN '" + AParameters["param_from_date"].ToDate().ToString("yyyy-MM-dd") +
                                   "' AND '" + AParameters["param_to_date"].ToDate().ToString("yyyy-MM-dd") + "'" +
                                   " AND DonorPartner.p_partner_key_n = gift.p_donor_key_n" +
                                   " AND RecipientLedgerPartner.p_partner_key_n = detail.a_recipient_ledger_number_n" +
                                   " AND gift.a_ledger_number_i = " + LedgerNumber +
                                   " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                                   " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i";

                    if ((ReportType == "Complete") || (ReportType == "Gifts Only"))
                    {
                        Query += " ORDER BY gift.a_date_entered_d";
                    }
                    else if (ReportType == "Donors Only")
                    {
                        Query += " ORDER BY DonorPartner.p_partner_short_name_c";
                    }

                    Results = DbAdapter.RunQuery(Query, "Donors", Transaction);
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable RecipientGiftStatementDonorAddressesTable(Int64 ADonorKey, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            // create new datatable
            DataTable Results = new DataTable();

            Results.Columns.Add("DonorKey", typeof(Int64));

            DbAdapter.FPrivateDatabaseObj.BeginAutoReadTransaction(
                ref Transaction,
                delegate
                {
                    // get best address for the partner
                    PPartnerLocationTable PartnerLocationDT = PPartnerLocationAccess.LoadViaPPartner(ADonorKey, Transaction);
                    TLocationPK BestAddress = Calculations.DetermineBestAddress(PartnerLocationDT);

                    string QueryLocation = "SELECT" +
                                           " PUB_p_location.p_locality_c AS Locality," +
                                           " PUB_p_location.p_street_name_c," +
                                           " PUB_p_location.p_address_3_c," +
                                           " PUB_p_location.p_postal_code_c," +
                                           " PUB_p_location.p_city_c," +
                                           " PUB_p_location.p_county_c," +
                                           " PUB_p_location.p_country_code_c," +
                                           " PUB_p_country.p_address_order_i" +

                                           " FROM" +
                                           " PUB_p_location" +

                                           " LEFT JOIN PUB_p_country" +
                                           " ON PUB_p_country.p_country_code_c = PUB_p_location.p_country_code_c" +

                                           " WHERE" +
                                           " PUB_p_location.p_site_key_n = " + BestAddress.SiteKey +
                                           " AND PUB_p_location.p_location_key_i = " + BestAddress.LocationKey;

                    Results.Merge(DbAdapter.RunQuery(QueryLocation, "DonorAddresses", Transaction));

                    if (Results.Rows.Count == 0)
                    {
                        DataRow NewRow = Results.NewRow();
                        NewRow["Locality"] = "UNKNOWN";
                        Results.Rows.Add(NewRow);
                    }

                    Results.Rows[0]["DonorKey"] = ADonorKey;
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable RecipientTaxDeductPctTable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            // create new datatable
            DataTable Results = new DataTable();

            DbAdapter.FPrivateDatabaseObj.BeginAutoReadTransaction(ref Transaction,
                delegate
                {
                    DateTime CurrentDate = DateTime.Today;

                    string RecipientSelection = AParameters["param_recipient_selection"].ToString();

                    string Query =
                        "SELECT DISTINCT p_partner_tax_deductible_pct.p_partner_key_n, p_partner_tax_deductible_pct.p_date_valid_from_d, " +
                        "p_partner_tax_deductible_pct.p_percentage_tax_deductible_n, p_partner.p_partner_short_name_c, " +
                        "p_partner_gift_destination.p_field_key_n, um_unit_structure.um_parent_unit_key_n " +

                        "FROM p_partner_tax_deductible_pct " +

                        "LEFT JOIN p_partner " +
                        "ON p_partner.p_partner_key_n = p_partner_tax_deductible_pct.p_partner_key_n " +

                        "LEFT JOIN p_partner_gift_destination " +
                        "ON CASE WHEN p_partner.p_partner_class_c = 'FAMILY' " +
                        "THEN p_partner.p_partner_key_n = p_partner_gift_destination.p_partner_key_n " +
                        "AND p_partner_gift_destination.p_date_effective_d <= '" + CurrentDate + "' " +
                        "AND (p_partner_gift_destination.p_date_expires_d IS NULL " +
                        "OR (p_partner_gift_destination.p_date_expires_d >= '" + CurrentDate + "' " +
                        "AND p_partner_gift_destination.p_date_effective_d <> p_partner_gift_destination.p_date_expires_d)) END " +

                        "LEFT JOIN um_unit_structure " +
                        "ON CASE WHEN p_partner.p_partner_class_c = 'UNIT' " +
                        "THEN NOT EXISTS (SELECT * FROM p_partner_type " +
                        "WHERE p_partner_type.p_partner_key_n = p_partner_tax_deductible_pct.p_partner_key_n " +
                        "AND p_partner_type.p_type_code_c = 'LEDGER') " +
                        "AND um_unit_structure.um_child_unit_key_n = p_partner_tax_deductible_pct.p_partner_key_n " +
                        "AND um_unit_structure.um_child_unit_key_n <> um_unit_structure.um_parent_unit_key_n END";

                    if (RecipientSelection == "one_partner")
                    {
                        Query += " WHERE p_partner_tax_deductible_pct.p_partner_key_n = " + AParameters["param_recipient_key"].ToInt64();
                    }
                    else if (RecipientSelection == "Extract")
                    {
                        // recipient must be part of extract
                        Query += " WHERE EXISTS(SELECT * FROM m_extract, m_extract_master";

                        if (!AParameters["param_chkPrintAllExtract"].ToBool())
                        {
                            Query += ", a_gift_detail, a_gift_batch";
                        }

                        Query += " WHERE p_partner_tax_deductible_pct.p_partner_key_n = m_extract.p_partner_key_n " +
                                 "AND m_extract.m_extract_id_i = m_extract_master.m_extract_id_i " +
                                 "AND m_extract_master.m_extract_name_c = '" + AParameters["param_extract_name"] + "'";

                        if (!AParameters["param_chkPrintAllExtract"].ToBool())
                        {
                            // recipient must have a posted gift
                            Query += " AND a_gift_detail.a_ledger_number_i = " + AParameters["param_ledger_number_i"] +
                                     " AND a_gift_detail.p_recipient_key_n = m_extract.p_partner_key_n " +
                                     "AND a_gift_batch.a_ledger_number_i = " + AParameters["param_ledger_number_i"] +
                                     " AND a_gift_batch.a_batch_number_i = a_gift_detail.a_batch_number_i " +
                                     "AND a_gift_batch.a_batch_status_c = 'Posted'";
                        }

                        Query += ")";
                    }

                    Results = DbAdapter.RunQuery(Query, "Results", Transaction);
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable OneYearMonthGivingDonorTable(Dictionary <String, TVariant>AParameters,
            Int64 ARecipientKey,
            TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string Currency = AParameters["param_currency"].ToString().ToUpper() == "BASE" ? "a_gift_amount_n" : "a_gift_amount_intl_n";

            // create new datatable
            DataTable Results = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    //TODO: Calendar vs Financial Date Handling - Check if this should use financial year start/end in all places below
                    string Query = "SELECT DISTINCT" +
                                   " gift.p_donor_key_n AS DonorKey," +
                                   " DonorPartner.p_partner_short_name_c AS DonorName," +
                                   " DonorPartner.p_partner_class_c AS DonorClass," +
                                   " detail.p_recipient_key_n AS RecipientKey," +
                                   " SUM (detail." + Currency + ") AS GiftAmountTotal," +
                                   " COUNT (detail." + Currency + ") AS TotalCount," +
                                   " PUB_a_gift_batch.a_currency_code_c AS GiftCurrency," +

                                   " SUM (CASE WHEN UPPER(DonorPartner.p_partner_class_c) = 'CHURCH' THEN detail." + Currency +
                                   " ELSE 0 END) AS TotalChurches," +
                                   " SUM (CASE WHEN UPPER(DonorPartner.p_partner_class_c) = 'PERSON' OR " +
                                   " UPPER(DonorPartner.p_partner_class_c) = 'FAMILY' THEN detail." + Currency +
                                   " ELSE 0 END) AS TotalIndividuals," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-01-01'" +
                                   " AND '" + AParameters["param_year"] + "-01-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftJanuary," +

                                   " SUM (CASE WHEN gift.a_date_entered_d >= '" + AParameters["param_year"] + "-02-01'" +
                                   " AND gift.a_date_entered_d < '" + AParameters["param_year"] + "-03-01'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftFebruary," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-03-01'" +
                                   " AND '" + AParameters["param_year"] + "-03-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftMarch," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-04-01'" +
                                   " AND '" + AParameters["param_year"] + "-04-30'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftApril," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-05-01'" +
                                   " AND '" + AParameters["param_year"] + "-05-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftMay," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-06-01'" +
                                   " AND '" + AParameters["param_year"] + "-06-30'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftJune," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-07-01'" +
                                   " AND '" + AParameters["param_year"] + "-07-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftJuly," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-08-01'" +
                                   " AND '" + AParameters["param_year"] + "-08-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftAugust," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-09-01'" +
                                   " AND '" + AParameters["param_year"] + "-09-30'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftSeptember," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-10-01'" +
                                   " AND '" + AParameters["param_year"] + "-10-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftOctober," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-11-01'" +
                                   " AND '" + AParameters["param_year"] + "-11-30'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftNovember," +

                                   " SUM (CASE WHEN gift.a_date_entered_d BETWEEN '" + AParameters["param_year"] + "-12-01'" +
                                   " AND '" + AParameters["param_year"] + "-12-31'" +
                                   " THEN detail." + Currency +
                                   " ELSE 0 END) AS GiftDecember" +

                                   " FROM" +
                                   " PUB_a_gift as gift," +
                                   " PUB_a_gift_detail as detail," +
                                   " PUB_a_gift_batch," +
                                   " PUB_p_partner AS DonorPartner" +

                                   " WHERE" +
                                   " detail.a_ledger_number_i = gift.a_ledger_number_i" +
                                   " AND detail.p_recipient_key_n = " + ARecipientKey +
                                   " AND PUB_a_gift_batch.a_batch_status_c = 'Posted'" +
                                   " AND PUB_a_gift_batch.a_batch_number_i = gift.a_batch_number_i" +
                                   " AND PUB_a_gift_batch.a_ledger_number_i = " + LedgerNumber +
                                   " AND gift.a_date_entered_d BETWEEN '" + AParameters["param_from_date"].ToDate().ToString("yyyy-MM-dd") +
                                   "' AND '" + AParameters["param_to_date"].ToDate().ToString(
                        "yyyy-MM-dd") + "'" +
                                   " AND DonorPartner.p_partner_key_n = gift.p_donor_key_n" +
                                   " AND gift.a_ledger_number_i = " + LedgerNumber +
                                   " AND detail.a_batch_number_i = gift.a_batch_number_i" +
                                   " AND detail.a_gift_transaction_number_i = gift.a_gift_transaction_number_i" +
                                   " AND detail.a_modified_detail_l = false" +

                                   " GROUP BY DonorPartner.p_partner_key_n, gift.p_donor_key_n, detail.p_recipient_key_n, DonorPartner.p_partner_short_name_c, DonorPartner.p_partner_class_c, PUB_a_gift_batch.a_currency_code_c"
                                   +
                                   " ORDER BY gift.p_donor_key_n";

                    Results = DbAdapter.RunQuery(Query, "Donors", Transaction);
                });

            return Results;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// Derived from the existing method in MFinanceQueries\ReportFinance.cs
        /// </summary>
        [NoRemoting]
        public static DataTable MotivationResponse(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            DataTable ReturnTable = new DataTable();

            Int32 LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string ReportType = AParameters["param_report_type"].ToString();
            DateTime FromDate = AParameters["param_from_date"].ToDate();
            DateTime ToDate = AParameters["param_to_date"].ToDate();
            bool AllMotivationGroups = AParameters["param_all_motivation_groups"].ToBool();
            bool AllMotivationDetails = AParameters["param_all_motivation_details"].ToBool();
            bool SurpressDetailForGifts = AParameters["param_suppress_detail"].ToBool();
            string MailingCode = AParameters["param_mailing_code"].ToString();

            const string ANONYMOUS = "ANONYMOUS";

            string Query = string.Empty;

            if (ReportType == "Detailed") // Detailed report only
            {
                Query = "SELECT a_gift.a_receipt_number_i AS ReceiptNumber, a_gift.a_date_entered_d AS DateEntered," +
                        " a_gift_detail.a_gift_amount_n AS Amount," +
                        " a_gift_detail.a_gift_comment_one_c, a_gift_detail.a_gift_comment_two_c, a_gift_detail.a_gift_comment_three_c,";
            }
            else // Brief and Totals reports only
            {
                Query = "SELECT DISTINCT SUM (a_gift_detail.a_gift_amount_n) AS Amount," +
                        " COUNT(*) AS Quantity,";
            }

            Query += " a_gift.a_first_time_gift_l," +
                     " a_gift_detail.a_motivation_group_code_c, a_gift_detail.a_motivation_detail_code_c, a_gift_detail.a_confidential_gift_flag_l,"
                     +
                     " a_motivation_detail.a_motivation_detail_desc_c,";

            // all reports
            Query += " CASE WHEN a_gift_detail.a_confidential_gift_flag_l = 'false' THEN to_char(a_gift.p_donor_key_n, 'FM0000000000')" +
                     " ELSE '" + ANONYMOUS + "' END AS DonorKey," +
                     " CASE WHEN a_gift_detail.a_confidential_gift_flag_l = 'false' THEN p_partner.p_partner_short_name_c" +
                     " ELSE '' END AS DonorName" +

                     " FROM a_gift, a_gift_batch, a_gift_detail, a_motivation_detail, p_partner" +

                     " WHERE a_gift.a_ledger_number_i = " + LedgerNumber +
                     " AND a_gift.a_date_entered_d >= '" + FromDate.Date.ToString() + "'" +
                     " AND a_gift.a_date_entered_d <= '" + ToDate.Date.ToString() + "'" +
                     " AND a_gift_batch.a_ledger_number_i = " + LedgerNumber +
                     " AND a_gift_batch.a_batch_number_i  = a_gift.a_batch_number_i" +
                     " AND a_gift_batch.a_batch_status_c = 'Posted'" +
                     " AND a_gift_detail.a_ledger_number_i = " + LedgerNumber +
                     " AND a_gift_detail.a_batch_number_i = a_gift_batch.a_batch_number_i" +
                     " AND a_gift_detail.a_gift_transaction_number_i = a_gift.a_gift_transaction_number_i" +
                     " AND a_motivation_detail.a_ledger_number_i = " + LedgerNumber +
                     " AND a_motivation_detail.a_motivation_detail_code_c = a_gift_detail.a_motivation_detail_code_c" +
                     " AND a_motivation_detail.a_motivation_group_code_c = a_gift_detail.a_motivation_group_code_c" +
                     " AND p_partner.p_partner_key_n = a_gift.p_donor_key_n";

            if (!AllMotivationGroups)
            {
                string MotivationGroups = AParameters["param_motivation_group_quotes"].ToString();
                Query += " AND a_gift_detail.a_motivation_group_code_c IN (" + MotivationGroups + ")";
            }

            if (!AllMotivationDetails)
            {
                string MotivationGroupDetailPairs = AParameters["param_motivation_group_detail_pairs"].ToString();
                Query += " AND (a_gift_detail.a_motivation_group_code_c,a_gift_detail.a_motivation_detail_code_c) IN (" +
                         MotivationGroupDetailPairs + ")";
            }

            if (!string.IsNullOrEmpty(MailingCode))
            {
                Query += " AND a_gift_detail.p_mailing_code_c = '" + MailingCode + "'";
            }

            if (ReportType == "Detailed") // Detailed report only
            {
                Query += " ORDER BY a_gift_detail.a_motivation_group_code_c, a_gift_detail.a_motivation_detail_code_c, DateEntered";
            }
            else // Brief and Totals reports only
            {
                Query += " GROUP BY a_gift_detail.a_motivation_group_code_c, a_gift_detail.a_motivation_detail_code_c, DonorKey," +
                         " a_gift.a_first_time_gift_l, a_gift_detail.a_confidential_gift_flag_l, a_motivation_detail.a_motivation_detail_desc_c," +
                         " DonorName" +

                         " ORDER BY a_gift_detail.a_motivation_group_code_c, a_gift_detail.a_motivation_detail_code_c, DonorKey";
            }

            TDBTransaction Transaction = null;
            DBAccess.GDBAccessObj.GetNewOrExistingAutoReadTransaction(IsolationLevel.ReadCommitted, TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    ReturnTable = DbAdapter.RunQuery(Query, "MotivationResponse", Transaction);

                    if (ReturnTable != null)
                    {
                        // add columns if they do not exist (they need to exist even if empty)
                        if (!ReturnTable.Columns.Contains("a_gift_comment_one_c"))
                        {
                            ReturnTable.Columns.Add("a_gift_comment_one_c", typeof(string));
                        }

                        if (!ReturnTable.Columns.Contains("a_gift_comment_two_c"))
                        {
                            ReturnTable.Columns.Add("a_gift_comment_two_c", typeof(string));
                        }

                        if (!ReturnTable.Columns.Contains("a_gift_comment_three_c"))
                        {
                            ReturnTable.Columns.Add("a_gift_comment_three_c", typeof(string));
                        }

                        if (!ReturnTable.Columns.Contains("DateEntered"))
                        {
                            ReturnTable.Columns.Add("DateEntered", typeof(string));
                        }

                        if (!ReturnTable.Columns.Contains("ReceiptNumber"))
                        {
                            ReturnTable.Columns.Add("ReceiptNumber", typeof(string));
                        }

                        if (!ReturnTable.Columns.Contains("Quantity"))
                        {
                            ReturnTable.Columns.Add("Quantity", typeof(string));
                        }

                        ReturnTable.Columns.Add("DonorAddress", typeof(string));

                        if ((ReportType == "Detailed") || (ReportType == "Brief"))
                        {
                            List <string[]>DonorAddresses = new List <string[]>();

                            // get best address for the partner
                            foreach (DataRow Row in ReturnTable.Rows)
                            {
                                if ((Row["DonorKey"].ToString() != ANONYMOUS)
                                    && !(SurpressDetailForGifts
                                         && (Row["a_motivation_group_code_c"].ToString() == MFinanceConstants.MOTIVATION_GROUP_GIFT)))
                                {
                                    string[] DonorAddress = DonorAddresses.Find(x => (x[0] == Row["DonorKey"].ToString()));

                                    // if we already have the donor's address...
                                    if (DonorAddress != null)
                                    {
                                        Row["DonorAddress"] = DonorAddress[1];
                                    }
                                    else
                                    {
                                        PLocationTable DonorLocation;
                                        string CountryName;

                                        TAddressTools.GetBestAddress(Convert.ToInt64(Row["DonorKey"]), out DonorLocation, out CountryName,
                                            Transaction);

                                        if (DonorLocation.Rows.Count > 0)
                                        {
                                            Row["DonorAddress"] = Calculations.DetermineLocationString(DonorLocation[0],
                                                Calculations.TPartnerLocationFormatEnum.plfCommaSeparated);
                                            DonorAddresses.Add(new string[] { Row["DonorKey"].ToString(), Row["DonorAddress"].ToString() });
                                        }
                                    }
                                }
                            }
                        }
                    }
                });

            return ReturnTable;
        }
    }
}