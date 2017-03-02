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
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MFinance.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace Ict.Petra.Server.MFinance.Reporting.WebConnectors
{
    ///<summary>
    /// This WebConnector provides data for the Accounts Payable reporting screens
    ///</summary>
    public partial class TFinanceReportingWebConnector
    {
        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet APAgedSupplierList(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string DateSelection = AParameters["param_date_selection"].ToDate().ToString("yyyy-MM-dd");

            DataSet ReturnDataSet = new DataSet();
            // create new datatable
            DataTable APAgedSupplierListTable = new DataTable();
            DataTable documents = new DataTable();
            DataTable currencies = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"SELECT
                                        p_partner.p_partner_key_n,
	                                    a_ap_document.a_ap_document_id_i AS DocId,
	                                    a_ap_document.a_total_amount_n AS APAmount,
	                                    a_ap_document.a_ap_number_i AS APNumber,
	                                    a_ap_document.a_credit_note_flag_l AS IsCredit,
	                                    a_ap_document.a_date_issued_d AS IssueDate,
	                                    a_ap_document.a_credit_terms_i AS CreditTerms,
	                                    a_ap_document.a_document_code_c AS DocumentCode,
	                                    a_ap_document.a_reference_c AS Reference,
	                                    a_ap_document.a_discount_days_i AS DiscountDays,
	                                    a_ap_supplier.a_currency_code_c,
	                                    a_ap_document.a_document_status_c
                                    FROM a_ap_document
                                    JOIN a_ap_supplier ON a_ap_document.p_partner_key_n = a_ap_supplier.p_partner_key_n
                                    JOIN p_partner ON p_partner.p_partner_key_n = a_ap_document.p_partner_key_n
                                    WHERE a_ap_document.a_ledger_number_i = "
                        +
                        LedgerNumber + @"
                                    AND a_ap_document.a_date_entered_d <= '"                                            + DateSelection +
                        @"'
                                    AND a_ap_document.a_document_status_c <> 'CANCELLED'
                                    AND a_ap_document.a_document_status_c <> 'PAID'"                                                                                                                       ;

                    documents = DbAdapter.RunQuery(Query, "documents", Transaction);

                    List <string>partners = new List <string>();

                    foreach (DataRow dr in documents.Rows)
                    {
                        partners.Add(dr[0].ToString());
                    }

                    String partnerstring = String.Join(",",
                        partners);

                    if (partnerstring == String.Empty)
                    {
                        partnerstring = "0";
                    }

                    Query =
                        @"SELECT DISTINCT p_partner.p_partner_key_n, p_partner_short_name_c,a_currency_code_c FROM p_partner
                    JOIN a_ap_supplier ON p_partner.p_partner_key_n = a_ap_supplier.p_partner_key_n WHERE p_partner.p_partner_key_n IN("
                        +
                        partnerstring + ")";
                    APAgedSupplierListTable = DbAdapter.RunQuery(Query, "APAgedSupplierList", Transaction);
                });

            DataTable NewDocuments = documents.Copy();
            //adding new columns
            String[] newCol =
            {
                "OverdueP", "Overdue", "Due", "DueP", "DuePP"
            };

            foreach (String col in newCol)
            {
                NewDocuments.Columns.Add(col, typeof(decimal));
            }

            int counter = -1;

            foreach (DataRow row in documents.Rows)
            {
                counter++;

                DateTime Paramdate = AParameters["param_date_selection"].ToDate();
                DateTime DueDate = ((DateTime)row["issuedate"]).AddDays(Double.Parse(row["creditterms"].ToString()));

                //Overdue30+
                DateTime MyDate = Paramdate.AddDays(-30);

                if (DueDate <= MyDate)
                {
                    NewDocuments.Rows[counter]["OverdueP"] = row["apamount"];
                    continue;
                }

                //Overdue
                if ((DueDate >= MyDate) && (DueDate < Paramdate))
                {
                    NewDocuments.Rows[counter]["Overdue"] = row["apamount"];
                    continue;
                }

                //Due
                MyDate = Paramdate.AddDays(30);

                if ((DueDate >= Paramdate) && (DueDate < MyDate))
                {
                    NewDocuments.Rows[counter]["Due"] = row["apamount"];
                    continue;
                }

                //Due30p
                DateTime MyBigDate = MyDate.AddDays(30);

                if ((DueDate >= MyDate) && (DueDate < MyBigDate))
                {
                    NewDocuments.Rows[counter]["DueP"] = row["apamount"];
                    continue;
                }

                //Due60p
                if (DueDate >= MyBigDate)
                {
                    NewDocuments.Rows[counter]["DuePP"] = row["apamount"];
                    continue;
                }

                NewDocuments.Rows[counter]["apamount"] = "ERROR";
                counter++;
            }

            //get all the diffrent currencies into a table
            currencies = NewDocuments.DefaultView.ToTable("currencies", true, "a_currency_code_c");

            ReturnDataSet.Tables.Add(currencies);
            ReturnDataSet.Tables.Add(APAgedSupplierListTable);
            ReturnDataSet.Tables.Add(NewDocuments);
            return ReturnDataSet;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataTable APCurrentPayable(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string DateSelection = AParameters["param_payment_date"].ToDate().ToString("yyyy-MM-dd");

            // create new datatable
            DataTable APCurrentPayable = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"SELECT
                                        p_partner.p_partner_key_n,
                                        p_partner_short_name_c,
	                                    a_ap_document.a_ap_document_id_i AS DocId,
	                                    a_ap_document.a_total_amount_n AS APAmount,
	                                    a_ap_document.a_ap_number_i AS APNumber,
	                                    a_ap_document.a_credit_note_flag_l AS IsCredit,
	                                    a_ap_document.a_date_issued_d AS IssueDate,
	                                    a_ap_document.a_credit_terms_i AS CreditTerms,
	                                    a_ap_document.a_document_code_c AS DocumentCode,
	                                    a_ap_document.a_reference_c AS Reference,
	                                    a_ap_document.a_discount_days_i AS DiscountDays,
	                                    a_ap_supplier.a_currency_code_c,
	                                    a_ap_document.a_document_status_c,
                                        a_exchange_rate_to_base_n
                                    FROM a_ap_document
                                    JOIN a_ap_supplier ON a_ap_document.p_partner_key_n = a_ap_supplier.p_partner_key_n
                                    JOIN p_partner ON p_partner.p_partner_key_n = a_ap_document.p_partner_key_n
                                    WHERE a_ap_document.a_ledger_number_i = "
                        +
                        LedgerNumber + @"
                                    AND a_ap_document.a_date_entered_d <= '"                                            + DateSelection +
                        @"'
                                    AND a_ap_document.a_document_status_c <> 'CANCELLED'
                                    AND a_ap_document.a_document_status_c <> 'PAID'"                                                                                                                       ;

                    APCurrentPayable = DbAdapter.RunQuery(Query, "APCurrentPayable", Transaction);
                });

            return APCurrentPayable;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet APPaymentReport(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            string NumFrom = "";
            string NumTo = "";
            string DateFrom = "";
            string DateTo = "";

            if (AParameters["param_payment_date_from_i"].ToString() != String.Empty)
            {
                DateFrom = " AND a_payment_date_d >= '" + AParameters["param_payment_date_from_i"].ToDate().ToString("yyyy-MM-dd") + "'   ";
            }

            if (AParameters["param_payment_date_to_i"].ToString() != String.Empty)
            {
                DateTo = " AND a_payment_date_d <= '" + AParameters["param_payment_date_to_i"].ToDate().ToString("yyyy-MM-dd") + "'   ";
            }

            if (AParameters["param_payment_num_from_i"].ToString() != String.Empty)
            {
                NumFrom = " AND a_ap_document_payment.a_payment_number_i >= " + AParameters["param_payment_num_from_i"].ToInt() + " ";
            }

            if (AParameters["param_payment_num_to_i"].ToString() != String.Empty)
            {
                NumTo = " AND a_ap_document_payment.a_payment_number_i <= " + AParameters["param_payment_num_to_i"].ToInt() + " ";
            }

            DataSet ReturnDataSet = new DataSet();
            // create new datatable
            DataTable Payments = new DataTable();
            DataTable Suppliers = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"SELECT
	                                    a_ap_document.p_partner_key_n,
                                        a_ap_document_payment.a_payment_number_i,
	                                    a_ap_document.a_ap_number_i AS ApNumber,
	                                    a_ap_document.a_ap_document_id_i AS ApDocumentId,
	                                    a_ap_document.a_document_code_c AS DocCode,
	                                    a_ap_document.a_reference_c AS DocRef,
	                                    a_ap_document.a_credit_note_flag_l AS DocCreditNote,
	                                    a_ap_document.a_date_issued_d AS DocDate,
	                                    a_ap_document_payment.a_amount_n AS DocumentPaymentTotal,
                                        a_ap_document_detail.a_amount_n AS TotalAmountInvoice,
                                        a_payment_date_d,
                                        a_ap_payment.a_bank_account_c AS PaymentBAcc,
                                        a_account_code_c
                                    FROM a_ap_document
                                    JOIN a_ap_document_payment ON a_ap_document_payment.a_ap_document_id_i = a_ap_document.a_ap_document_id_i
                                    JOIN a_ap_document_detail ON a_ap_document_detail.a_ap_document_id_i = a_ap_document.a_ap_document_id_i
                                    LEFT JOIN a_ap_payment ON a_ap_payment.a_payment_number_i=a_ap_document_payment.a_payment_number_i AND a_ap_payment.a_ledger_number_i=a_ap_document_payment.a_ledger_number_i
                                    WHERE a_ap_document.a_ledger_number_i = "
                        +
                        LedgerNumber + @"
                                    AND a_ap_document_payment.a_ledger_number_i = "                                            + LedgerNumber +
                        "  " +
                        DateFrom + " " + DateTo + " " + NumFrom + " " + NumTo + " " +
                        @"

                                    ORDER BY a_ap_document.p_partner_key_n"                              ;

                    Payments = DbAdapter.RunQuery(Query, "Payments", Transaction);

                    List <string>partners = new List <string>();

                    foreach (DataRow dr in Payments.Rows)
                    {
                        partners.Add(dr[0].ToString());
                    }

                    String partnerstring = String.Join(",",
                        partners);

                    if (partnerstring == String.Empty)
                    {
                        partnerstring = "0";
                    }

                    Query =
                        @"SELECT DISTINCT p_partner.p_partner_key_n, p_partner_short_name_c,a_currency_code_c FROM p_partner
                    JOIN a_ap_supplier ON p_partner.p_partner_key_n = a_ap_supplier.p_partner_key_n WHERE p_partner.p_partner_key_n IN("
                        +
                        partnerstring + ")";
                    Suppliers = DbAdapter.RunQuery(Query, "Suppliers", Transaction);
                });

            ReturnDataSet.Tables.Add(Payments);
            ReturnDataSet.Tables.Add(Suppliers);
            return ReturnDataSet;
        }

        /// <summary>
        /// Returns a DataTable to the client for use in client-side reporting
        /// </summary>
        [NoRemoting]
        public static DataSet APAccountDetail(Dictionary <String, TVariant>AParameters, TReportingDbAdapter DbAdapter)
        {
            TDBTransaction Transaction = null;

            int LedgerNumber = AParameters["param_ledger_number_i"].ToInt32();
            TLedgerInfo LedgerInfo = new TLedgerInfo(LedgerNumber);
            string LedgerCurrency = LedgerInfo.BaseCurrency;
            string NumFrom = "";
            string NumTo = "";
            string DateFrom = "";
            string DateTo = "";

            if (AParameters["param_from_date"].ToString() != String.Empty)
            {
                DateFrom = " AND a_transaction.a_transaction_date_d >= '" + AParameters["param_from_date"].ToDate().ToString("yyyy-MM-dd") + "'   ";
            }

            if (AParameters["param_to_date"].ToString() != String.Empty)
            {
                DateTo = " AND a_transaction.a_transaction_date_d <= '" + AParameters["param_to_date"].ToDate().ToString("yyyy-MM-dd") + "'   ";
            }

            if (AParameters["param_account_from"].ToString() != String.Empty)
            {
                NumFrom = " AND a_transaction.a_account_code_c >= '" + AParameters["param_account_from"].ToString() + "' ";
            }

            if (AParameters["param_account_to"].ToString() != String.Empty)
            {
                NumTo = " AND a_transaction.a_account_code_c <= '" + AParameters["param_account_to"].ToString() + "' ";
            }

            DataSet ReturnDataSet = new DataSet();
            // create new datatable
            DataTable Accounts = new DataTable();
            DataTable Details = new DataTable();

            DbAdapter.FPrivateDatabaseObj.GetNewOrExistingAutoReadTransaction(
                IsolationLevel.ReadCommitted,
                TEnforceIsolationLevel.eilMinimum,
                ref Transaction,
                delegate
                {
                    String Query =
                        @"SELECT DISTINCT
	                                    a_transaction.a_account_code_c AS AccountCode,
                                        a_cost_centre.a_cost_centre_code_c,
	                                    a_cost_centre_name_c,
	                                    a_account_code_long_desc_c,
	                                    a_account_code_short_desc_c,
	                                    CASE WHEN a_foreign_currency_flag_l THEN a_foreign_currency_code_c ELSE '"
                        +
                        LedgerCurrency +
                        @"' END AS currency
                                    FROM a_transaction
                                    LEFT JOIN a_cost_centre ON a_cost_centre.a_cost_centre_code_c = a_transaction.a_cost_centre_code_c
                                    JOIN a_account ON a_transaction.a_ledger_number_i=a_account.a_ledger_number_i AND a_transaction.a_account_code_c=a_account.a_account_code_c
                                    WHERE
	                                    a_transaction.a_ledger_number_i = "
                        +
                        LedgerNumber +
                        @"
                                        AND a_narrative_c LIKE 'AP%'
	                                    AND a_transaction.a_transaction_status_l = true
	                                    AND NOT (a_transaction.a_system_generated_l = true
	                                    AND a_transaction.a_narrative_c LIKE 'Year end re-allocation%')
	                                    "
                        +
                        DateFrom + DateTo + NumFrom + NumTo;

                    Accounts = DbAdapter.RunQuery(Query,
                        "Accounts",
                        Transaction);

                    Query =
                        @"SELECT
	                            a_transaction.a_account_code_c AS AccountCode,
	                            a_transaction.a_transaction_date_d AS Date,
	                            a_transaction.a_amount_in_base_currency_n AS Amount,
	                            a_transaction.a_debit_credit_indicator_l,
	                            a_transaction.a_narrative_c AS Detail,
	                            a_transaction.a_reference_c AS ReferenceNumber,
	                            a_transaction.a_batch_number_i AS BatchNumber,
                                a_transaction.a_cost_centre_code_c AS CostCentreCode
                            FROM a_transaction
                            WHERE
	                            a_transaction.a_ledger_number_i = "
                        +
                        LedgerNumber +
                        @"
	                            AND a_transaction.a_transaction_status_l = true
	                            AND NOT (a_transaction.a_system_generated_l = true
	                            AND a_transaction.a_narrative_c LIKE 'Year end re-allocation%')
                                AND a_narrative_c LIKE 'AP%'"
                        +
                        DateFrom + DateTo + NumFrom + NumTo + "ORDER BY a_cost_centre_code_c,a_account_code_c";

                    Details = DbAdapter.RunQuery(Query, "Details", Transaction);

                    DataView tempView = Details.DefaultView;
                    tempView.RowFilter = "detail LIKE 'AP Payment:%' AND detail LIKE '% AP: %'";

                    List <string>invoicesList = new List <string>();

                    foreach (DataRow row in tempView.ToTable().Rows)
                    {
                        invoicesList.AddRange(row["detail"].ToString().Split(new char[] { ' ' },
                                4)[3].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    }

                    if (invoicesList.Count != 0)
                    {
                        DataTable Invoices =
                            DbAdapter.RunQuery(
                                "SELECT a_ap_number_i, a_document_code_c, a_document_code_c || ' (' || a_reference_c || ')' AS newref FROM a_ap_document WHERE a_ap_number_i IN("
                                +
                                String.Join(",", invoicesList) + ")", "Invoices", Transaction);
                        Invoices.PrimaryKey = new DataColumn[] { Invoices.Columns["a_ap_number_i"] };

                        for (int i = 0; i < Details.Rows.Count; i++)
                        {
                            if (Details.Rows[i]["detail"].ToString().Split(':')[0].ToString() == "AP Payment")
                            {
                                string detailString = Details.Rows[i]["detail"].ToString().Split(new char[] { ' ' }, 4)[3];

                                //If it contains a "-" it is a Payment for a supplier
                                if (!detailString.Contains("-"))
                                {
                                    string[] InvoiceNumbers = detailString.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                                    List <string>tempList = new List <string>();

                                    foreach (string num in InvoiceNumbers)
                                    {
                                        int IntNum;

                                        if (int.TryParse(num, out IntNum))
                                        {
                                            string Reference = Invoices.Rows.Find(IntNum)["newref"].ToString();

                                            if (Reference == String.Empty)
                                            {
                                                tempList.Add(Catalog.GetString("AP: ") + Invoices.Rows.Find(IntNum)["a_document_code_c"].ToString());
                                            }
                                            else
                                            {
                                                tempList.Add(Catalog.GetString("AP: ") + Reference);
                                            }
                                        }
                                    }

                                    Details.Rows[i]["referencenumber"] = String.Join("; ", tempList);
                                }
                            }
                        }
                    }
                });

            ReturnDataSet.Tables.Add(Accounts);
            ReturnDataSet.Tables.Add(Details);
            return ReturnDataSet;
        }
    }
}