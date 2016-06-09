//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
//
// Copyright 2004-2015 by OM International
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
using System.Text;
using System.Threading;
using System.Collections;
using Ict.Petra.Shared;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data;
using Ict.Common.Verification;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Common.Session;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.MCommon;
using Ict.Petra.Shared.MFinance;
using Ict.Common.Conversion;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MFinance.Account.Data.Access;

namespace Ict.Petra.Server.MFinance.AP.UIConnectors
{
    ///<summary>
    /// This UIConnector provides data for the finance Accounts Payable screens
    ///
    /// UIConnector Objects are instantiated by the Client's User Interface via the
    /// Instantiator classes.
    /// They handle requests for data retrieval and saving of data (including data
    /// verification).
    ///
    /// Their role is to
    ///   - retrieve (and possibly aggregate) data using Business Objects,
    ///   - put this data into *one* DataSet that is passed to the Client and make
    ///     sure that no unnessary data is transferred to the Client,
    ///   - optionally provide functionality to retrieve additional, different data
    ///     if requested by the Client (for Client screens that load data initially
    ///     as well as later, eg. when a certain tab on the screen is clicked),
    ///   - save data using Business Objects.
    ///
    /// @Comment These Objects would usually not be instantiated by other Server
    ///          Objects, but only by the Client UI via the Instantiator classes.
    ///          However, Server Objects that derive from these objects and that
    ///          are also UIConnectors are feasible.
    ///</summary>
    public class TFindUIConnector : IAPUIConnectorsFind
    {
        /// <summary>Paged query object</summary>
        TPagedDataSet FPagedDataSetObject;

        /// <summary>Thread that is used for asynchronously executing the Find query</summary>
        Thread FFindThread;

        /// <summary>Get the current state of progress</summary>
        public TProgressState Progress
        {
            get
            {
                return FPagedDataSetObject.Progress;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TFindUIConnector() : base()
        {
        }

        /// <summary>
        /// If this is true, then search behaves a bit differently, with few filters on the server.
        /// Other filters may applied by the user on the client.
        /// </summary>
        private bool FSearchTransactions = false;

        /// <summary>if true, then search for supplier; if false, then search for invoice</summary>
        private bool FSearchSupplierOrInvoice = false;

        /// <summary>
        /// Find a supplier or a list of suppliers matching the search criteria
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Find parameters</param>
        public void FindSupplier(DataTable ACriteriaData)
        {
            FSearchTransactions = false;
            FSearchSupplierOrInvoice = true;
            PerformSearch(ACriteriaData);
        }

        /// <summary>
        /// Find a list of invoices matching the search criteria
        /// </summary>
        /// <param name="ACriteriaData">Optional HashTable containing "DaysPlus" for "due by" calculation</param>
        public void FindInvoices(DataTable ACriteriaData)
        {
            FSearchTransactions = false;
            FSearchSupplierOrInvoice = false;
            PerformSearch(ACriteriaData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <param name="ASupplierKey"></param>
        public void FindSupplierTransactions(Int32 ALedgerNumber, Int64 ASupplierKey)
        {
            FSearchTransactions = true;

            DataTable CriteriaTable = new DataTable();
            CriteriaTable.Columns.Add("PartnerKey", typeof(Int64));
            CriteriaTable.Columns.Add("LedgerNumber", typeof(Int32));

            DataRow Row = CriteriaTable.NewRow();
            Row["PartnerKey"] = ASupplierKey;
            Row["LedgerNumber"] = ALedgerNumber;
            CriteriaTable.Rows.Add(Row);

            PerformSearch(CriteriaTable);
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full query results are retrieved from the DB and stored
        /// internally in an object, data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Find parameters.</param>
        public void PerformSearch(DataTable ACriteriaData)
        {
            string PaymentNumberSQLPart;

            FPagedDataSetObject = new TPagedDataSet(null);

            DataRow CriteriaRow = PrepareDataRow(ACriteriaData);
            Int32 ledgerNumber = (Int32)CriteriaRow["LedgerNumber"];

            if (FSearchTransactions)
            {
                if (CommonTypes.ParseDBType(DBAccess.GDBAccessObj.DBType) == TDBType.SQLite)
                {
                    // Fix for SQLite: it does not support the 'to_char' Function
                    PaymentNumberSQLPart = "PUB_a_ap_payment.a_payment_number_i as InvNum, ";
                }
                else
                {
                    // whereas PostgreSQL does!
                    PaymentNumberSQLPart = "to_char(PUB_a_ap_payment.a_payment_number_i, '99999') as InvNum, ";
                }

                Int64 PartnerKey = Convert.ToInt64(CriteriaRow["PartnerKey"]);
                String SqlQuery = "SELECT DISTINCT " +
                                  "0 as ApDocumentId, " +
                                  "PUB_a_ap_payment.a_payment_number_i as ApNum, " +
                                  PaymentNumberSQLPart +
                                  "true as CreditNote, " +
                                  "'Payment' as Type, " +
                                  "PUB_a_ap_payment.a_currency_code_c as Currency, " +
                                  "PUB_a_ap_payment.a_amount_n as Amount, " +
                                  "0 AS OutstandingAmount, " +
                                  "'' as Status, " +
                                  "0 as DiscountPercent, " +
                                  "0 as DiscountDays, " +
                                  "PUB_a_ap_payment.s_date_created_d as Date " +
                                  " FROM PUB_a_ap_payment LEFT JOIN PUB_a_ap_document_payment on PUB_a_ap_payment.a_payment_number_i = PUB_a_ap_document_payment.a_payment_number_i"
                                  +
                                  " LEFT JOIN PUB_a_ap_document on PUB_a_ap_document_payment.a_ap_document_id_i = PUB_a_ap_document.a_ap_document_id_i\n"
                                  +
                                  " WHERE PUB_a_ap_document_payment.a_ledger_number_i=" + ledgerNumber +
                                  " AND p_partner_key_n=" + PartnerKey +
                                  "\n UNION\n" +
                                  " SELECT " +
                                  "a_ap_document_id_i as ApDocumentId, " +
                                  "a_ap_number_i as ApNum, " +
                                  "a_document_code_c as InvNum, " +
                                  "a_credit_note_flag_l as CreditNote, " +
                                  "'Invoice' as Type, " +
                                  "a_currency_code_c AS Currency, " +
                                  "a_total_amount_n as Amount, " +
                                  "a_total_amount_n AS OutstandingAmount, " +
                                  "a_document_status_c as Status, " +
                                  "a_discount_percentage_n as DiscountPercent, " +
                                  "a_discount_days_i as DiscountDays, " +
                                  "a_date_issued_d as Date " +
                                  "FROM PUB_a_ap_document " +
                                  "WHERE a_ledger_number_i=" + ledgerNumber + " " +
                                  "AND p_partner_key_n=" + PartnerKey + " " +
                                  "ORDER BY Date DESC";
                FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(SqlQuery);
                FPagedDataSetObject.FindParameters.FSearchName = "ATransactions";
            }
            else
            {
                if (!FSearchSupplierOrInvoice)
                {
                    String SqlQuery = "SELECT " +
                                      "PUB_a_ap_document.a_ap_number_i AS ApNumber, " +
                                      "PUB_a_ap_document.a_document_code_c AS DocumentCode, " +
                                      "PUB_p_partner.p_partner_short_name_c AS PartnerShortName, " +
                                      "PUB_a_ap_document.a_currency_code_c AS CurrencyCode, " +
                                      "PUB_a_ap_document.a_total_amount_n AS TotalAmount, " +
                                      "PUB_a_ap_document.a_total_amount_n AS OutstandingAmount, " +
                                      "PUB_a_ap_document.a_document_status_c AS DocumentStatus, " +
                                      "PUB_a_ap_document.a_date_issued_d AS DateIssued, " +
                                      "PUB_a_ap_document.a_date_issued_d AS DateDue, " +
                                      "PUB_a_ap_document.a_date_issued_d AS DateDiscountUntil, " +
                                      "PUB_a_ap_document.a_credit_terms_i AS CreditTerms, " +
                                      "PUB_a_ap_document.a_discount_percentage_n AS DiscountPercentage, " +
                                      "PUB_a_ap_document.a_discount_days_i AS DiscountDays, " +
                                      "'none' AS DiscountMsg, " +
                                      "false AS Selected, " +
                                      "PUB_a_ap_document.a_credit_note_flag_l AS CreditNoteFlag, " +
                                      "PUB_a_ap_document.a_ap_document_id_i AS ApDocumentId " +
                                      "FROM PUB_a_ap_document, PUB_a_ap_supplier, PUB_p_partner " +
                                      "WHERE PUB_a_ap_document.a_ledger_number_i=" + ledgerNumber + " " +
                                      "AND PUB_a_ap_document.a_document_status_c <> 'CANCELLED' " +
                                      "AND PUB_a_ap_document.a_document_status_c <> 'PAID' " +
                                      "AND PUB_a_ap_supplier.p_partner_key_n = PUB_p_partner.p_partner_key_n " +
                                      "AND PUB_a_ap_document.p_partner_key_n = PUB_p_partner.p_partner_key_n " +
                                      "ORDER BY PUB_a_ap_document.a_ap_number_i DESC";
                    FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(SqlQuery);
                    FPagedDataSetObject.FindParameters.FSearchName = "AInvoices";
                }
                else
                {
                    String SqlQuery = "SELECT " +
                                      "PUB_a_ap_supplier.p_partner_key_n AS PartnerKey, " +
                                      "PUB_p_partner.p_partner_short_name_c AS PartnerShortName, " +
                                      "PUB_a_ap_supplier.a_currency_code_c AS CurrencyCode, " +
                                      "PUB_p_partner.p_status_code_c AS StatusCode " +
                                      "FROM PUB_a_ap_supplier, PUB_p_partner " +
                                      "WHERE ";

                    if (((String)CriteriaRow["SupplierId"]).Length > 0)  // If the search box is empty, I'll not add this at all...
                    {
                        SqlQuery += String.Format("p_partner_short_name_c LIKE '{0}' AND ", (String)CriteriaRow["SupplierId"] + "%");
                    }

                    SqlQuery += "PUB_a_ap_supplier.p_partner_key_n = PUB_p_partner.p_partner_key_n " +
                                "ORDER BY PartnerShortName";

                    FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(SqlQuery);
                    FPagedDataSetObject.FindParameters.FSearchName = "ASuppliers";
                }
            }

            string session = TSession.GetSessionID();

            //
            // Start the Find Thread
            //
            try
            {
                ThreadStart myThreadStart = delegate {
                    FPagedDataSetObject.ExecuteQuery(session, "AP TFindUIConnector");
                };
                FFindThread = new Thread(myThreadStart);
                FFindThread.Name = "APFind" + Guid.NewGuid().ToString();
                FFindThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Stops the query execution.
        /// <remarks>It might take some time until the executing query is cancelled by the DB, but this procedure returns
        /// immediately. The reason for this is that we consider the query cancellation as done since the application can
        /// 'forget' about the result of the cancellation process (but beware of executing another query while the other is
        /// stopping - this leads to ADO.NET errors that state that a ADO.NET command is still executing!).
        /// </remarks>
        /// </summary>
        public void StopSearch()
        {
            Thread StopQueryThread;

            /* Start a separate Thread that should cancel the executing query
             * (Microsoft recommends doing it this way!) */
            StopQueryThread = new Thread(new ThreadStart(FPagedDataSetObject.StopQuery));
            StopQueryThread.Name = "APFindStopQuery" + Guid.NewGuid().ToString();
            StopQueryThread.Start();

            /* It might take some time until the executing query is cancelled by the DB,
             * but we consider it as done since the application can 'forget' about the
             * result of the cancellation process (but beware of executing another query
             * while the other is stopping - this leads to ADO.NET errors that state that
             * a ADO.NET command is still executing! */
        }

        /// <summary>
        /// Find out how much has already been paid off this invoice.
        /// (Also called from EditTransaction.)
        /// </summary>
        /// <param name="ApDocumentId"></param>
        /// <returns>Amount already paid</returns>
        [NoRemoting]
        public static Decimal GetPartPaidAmount(Int32 ApDocumentId)
        {
            Decimal PaidAmount = 0m;

            TDBTransaction ReadTransaction = null;

            DBAccess.GDBAccessObj.BeginAutoReadTransaction(ref ReadTransaction,
                delegate
                {
                    AApDocumentPaymentTable PreviousPayments =
                        AApDocumentPaymentAccess.LoadViaAApDocument(ApDocumentId, ReadTransaction);

                    foreach (AApDocumentPaymentRow PrevPaymentRow in PreviousPayments.Rows)
                    {
                        PaidAmount += PrevPaymentRow.Amount;
                    }
                }); // End of BeginAutoReadTransaction

            return PaidAmount;
        }

        private void SetOutstandingAmount(DataRow Row)
        {
            if (FSearchTransactions)
            {
                if (Row["Type"].ToString() == "Invoice")
                {
                    Row["OutstandingAmount"] = Row["Amount"];

                    if (Convert.ToString(Row["Status"]) == MFinanceConstants.AP_DOCUMENT_PAID)
                    {
                        Row["OutstandingAmount"] = 0.0m;
                    }

                    if (Convert.ToString(Row["Status"]) == MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID)
                    {
                        Row["OutstandingAmount"] = Convert.ToDecimal(Row["Amount"]) - GetPartPaidAmount(Convert.ToInt32(Row["ApDocumentId"]));
                    }
                }
            }
            else
            {
                // If any of the invoices are part-paid, I want to retrieve the outstanding amount.
                String docStatus = Convert.ToString(Row["DocumentStatus"]);
                Row["OutstandingAmount"] = Row["TotalAmount"];

                if (docStatus == MFinanceConstants.AP_DOCUMENT_PAID)
                {
                    Row["OutstandingAmount"] = 0.0m;
                }

                if (docStatus == MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID)
                {
                    Row["OutstandingAmount"] = Convert.ToDecimal(Row["TotalAmount"]) - GetPartPaidAmount(Convert.ToInt32(Row["ApDocumentID"]));
                }
            }
        }

        /// <summary>
        /// Returns the specified find results page.
        ///
        /// @comment Pages can be requested in any order!
        ///
        /// </summary>
        /// <param name="APage">Page to return</param>
        /// <param name="APageSize">Number of records to return per page</param>
        /// <param name="ATotalRecords">The amount of rows found by the SELECT statement</param>
        /// <param name="ATotalPages">The number of pages that will be needed on client-side to
        /// hold all rows found by the SELECT statement</param>
        /// <returns>DataTable containing the find result records for the specified page
        /// </returns>
        public DataTable GetDataPagedResult(System.Int16 APage, System.Int16 APageSize, out System.Int32 ATotalRecords, out System.Int16 ATotalPages)
        {
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetDataPagedResult called.");
            }

            DataTable ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);

            if (FSearchTransactions)
            {
                foreach (DataRow Row in ReturnValue.Rows)
                {
                    SetOutstandingAmount(Row);
                }
            }
            else
            {
                // searching for outstanding invoices on the main screen
                if (!FSearchSupplierOrInvoice)
                {
                    foreach (DataRow Row in ReturnValue.Rows)
                    {
                        // calculate DateDue and DateDiscountUntil
                        // add creditTerms to dateIssued to get DateDue

                        if (Row["CreditTerms"].GetType() != typeof(DBNull))
                        {
                            Row["DateDue"] = Convert.ToDateTime(Row["DateIssued"]).AddDays(Convert.ToInt16(Row["CreditTerms"]));
                        }

                        if (Row["DiscountDays"].GetType() != typeof(DBNull))
                        {
                            Row["DateDiscountUntil"] = Convert.ToDateTime(Row["DateIssued"]).AddDays(Convert.ToInt16(Row["DiscountDays"]));
                        }

                        SetOutstandingAmount(Row);

                        Row["DiscountMsg"] = "None";
                        Row["Selected"] = false;

                        if (Row["DiscountPercentage"].GetType() == typeof(DBNull))
                        {
                            Row["DiscountPercentage"] = 0;
                        }

                        if (Convert.ToDateTime(Row["DateDiscountUntil"]) > DateTime.Now)
                        {
                            Row["DiscountMsg"] =
                                String.Format("{0:n0}% until {1}",
                                    Row["DiscountPercentage"],
                                    TDate.DateTimeToLongDateString2(Convert.ToDateTime(Row["DateDiscountUntil"])));
                        }
                        else
                        {
                            Row["DiscountMsg"] = "Expired";
                        }
                    }
                }
            }

            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;

            // TODO TAccountsPayableAggregate.ApplySecurity(ref ReturnValue);
            return ReturnValue;
        }

        /// <summary>
        /// prepares the datarow; can add new columns
        /// </summary>
        /// <param name="ACriteriaTable"></param>
        /// <returns></returns>
        private DataRow PrepareDataRow(DataTable ACriteriaTable)
        {
            if (!ACriteriaTable.Columns.Contains("PartnerKey"))
            {
                ACriteriaTable.Columns.Add("PartnerKey", typeof(Int64));
                ACriteriaTable.Rows[0]["PartnerKey"] = 0;

                // try if this is a partner key

                if (ACriteriaTable.Columns.Contains("SupplierId"))
                {
                    Int64 SupplierPartnerKey;

                    if (Int64.TryParse(ACriteriaTable.Rows[0]["SupplierId"].ToString(), out SupplierPartnerKey))
                    {
                        ACriteriaTable.Rows[0]["PartnerKey"] = SupplierPartnerKey;
                    }
                }
            }

            return ACriteriaTable.Rows[0];
        }
    }
}
