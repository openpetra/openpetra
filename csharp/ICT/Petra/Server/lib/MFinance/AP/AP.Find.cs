//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//       Tim Ingham
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
    public class TFindUIConnector : TConfigurableMBRObject, IAPUIConnectorsFind
    {
        /// <summary>Paged query object</summary>
        TPagedDataSet FPagedDataSetObject;

        /// <summary>Asynchronous execution control object</summary>
        TAsynchronousExecutionProgress FAsyncExecProgress;

        /// <summary>Thread that is used for asynchronously executing the Find query</summary>
        Thread FFindThread;

        Int32 FLedgerNumber;

        /// <summary>Returns reference to the Asynchronous execution control object to the caller</summary>
        public IAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                return (IAsynchronousExecutionProgress)TCreateRemotableObject.CreateRemotableObject(
                    typeof(IAsynchronousExecutionProgress),
                    typeof(TAsynchronousExecutionProgressRemote),
                    FAsyncExecProgress);
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
        /// Retrieve all the information for the current Ledger
        /// </summary>
        /// <param name="ALedgerNumber"></param>
        /// <returns>ALedgerTable</returns>
        public ALedgerTable GetLedgerInfo(Int32 ALedgerNumber)
        {
            ALedgerTable Tbl = ALedgerAccess.LoadByPrimaryKey(ALedgerNumber, null);

            return Tbl;
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full
        /// query results are retrieved from the DB and stored internally in an object,
        /// data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        ///
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Find parameters</param>
        public void PerformSearch(DataTable ACriteriaData)
        {
            FAsyncExecProgress = new TAsynchronousExecutionProgress();

            FPagedDataSetObject = new TPagedDataSet(null);

/*
 *          if (FSearchTransactions)
 *          {
 *              FPagedDataSetObject = new TPagedDataSet(new AccountsPayableGUITDSTransactionListTable());
 *          }
 *          else if (FSearchSupplierOrInvoice)
 *          {
 *              FPagedDataSetObject = new TPagedDataSet(null);
 *          }
 *          else
 *          {
 *              FPagedDataSetObject = new TPagedDataSet(new AccountsPayableGUITDSInvoiceListTable());
 *          }
 */
            // Pass the TAsynchronousExecutionProgress object to FPagedDataSetObject so that it
            // can update execution status
            FPagedDataSetObject.AsyncExecProgress = FAsyncExecProgress;

            // Register Event Handler for the StopAsyncronousExecution event
            FAsyncExecProgress.StopAsyncronousExecution += new System.EventHandler(this.StopSearch);

            DataRow CriteriaRow = PrepareDataRow(ACriteriaData);
            FLedgerNumber = (Int32)CriteriaRow["LedgerNumber"];

            if (FSearchTransactions)
            {
                Int64 PartnerKey = Convert.ToInt64(CriteriaRow["PartnerKey"]);
                String SqlQuery = "SELECT DISTINCT " +
                                  "0 as ApDocumentId, " + 
                                  "PUB_a_ap_payment.a_payment_number_i as ApNum, " +
                                  "'' as InvNum, " +
                                  "true as CreditNote, " +
                                  "'Payment' as Type, " +
                                  "PUB_a_ap_payment.a_currency_code_c as Currency, " +
                                  "PUB_a_ap_payment.a_amount_n as Amount, " +
                                  "0 AS OutstandingAmount, " +
                                  "'' as Status, " +
                                  "0 as DiscountPercent, " +
                                  "0 as DiscountDays, " +
                                  "PUB_a_ap_payment.s_date_created_d as Date, " +
                                  "0 as DocumentId\n" +
                                  " FROM PUB_a_ap_payment LEFT JOIN PUB_a_ap_document_payment on PUB_a_ap_payment.a_payment_number_i = PUB_a_ap_document_payment.a_payment_number_i"
                                  +
                                  " LEFT JOIN PUB_a_ap_document on PUB_a_ap_document_payment.a_ap_document_id_i = PUB_a_ap_document.a_ap_document_id_i\n"
                                  +
                                  " WHERE PUB_a_ap_document_payment.a_ledger_number_i=" + FLedgerNumber +
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
                                  "a_date_issued_d as Date, " +
                                  "a_ap_document_id_i as DocumentId\n" +
                                  " FROM PUB_a_ap_document\n" +
                                  " WHERE a_ledger_number_i=" + FLedgerNumber +
                                  " AND p_partner_key_n=" + PartnerKey +
                                  " ORDER BY Date DESC"
                ;
                FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(SqlQuery);
                FPagedDataSetObject.FindParameters.FSearchName = "Transactions";
            }
            else
            {
                OdbcParameter[] ParametersArray;

                // Build WHERE criteria string based on ACriteriaData
                string WhereClause = BuildWhereClause(CriteriaRow, out ParametersArray);
                string FieldList = BuildFieldList(CriteriaRow);
                string FromClause = BuildFromClause(CriteriaRow, ref WhereClause);
                string OrderByClause = BuildOrderByClause(CriteriaRow);

                if (WhereClause.StartsWith(" AND") == true)
                {
                    WhereClause = WhereClause.Substring(4);
                }

                Hashtable ColumnNameMapping = new Hashtable();
                FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(
                    FieldList,
                    FromClause,
                    WhereClause,
                    OrderByClause,
                    ColumnNameMapping,
                    ParametersArray);

                if (FSearchSupplierOrInvoice)
                {
                    FPagedDataSetObject.FindParameters.FSearchName = "Suppliers";
                }
                else
                {
                    FPagedDataSetObject.FindParameters.FSearchName = "Invoices";
                }
            }

            //
            // Start the Find Thread
            //
            try
            {
                FFindThread = new Thread(new ThreadStart(FPagedDataSetObject.ExecuteQuery));
                FFindThread.Start();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Stops the query execution.
        ///
        /// Is intended to be called as an Event from FAsyncExecProgress.Cancel.
        ///
        /// @comment It might take some time until the executing query is cancelled by
        /// the DB, but this procedure returns immediately. The reason for this is that
        /// we consider the query cancellation as done since the application can
        /// 'forget' about the result of the cancellation process (but beware of
        /// executing another query while the other is stopping - this leads to ADO.NET
        /// errors that state that a ADO.NET command is still executing!).
        ///
        /// </summary>
        /// <param name="ASender">Object that requested the stopping (not evaluated)</param>
        /// <param name="AArgs">(not evaluated)
        /// </param>
        /// <returns>void</returns>
        public void StopSearch(object ASender, EventArgs AArgs)
        {
            Thread StopQueryThread;

            /* Start a separate Thread that should cancel the executing query
             * (Microsoft recommends doing it this way!) */
            ThreadStart ThreadStartDelegate = new ThreadStart(FPagedDataSetObject.StopQuery);

            StopQueryThread = new Thread(ThreadStartDelegate);
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

            AApDocumentPaymentTable PreviousPayments =
                AApDocumentPaymentAccess.LoadViaAApDocument(ApDocumentId, null);

            foreach (AApDocumentPaymentRow PrevPaymentRow in PreviousPayments.Rows)
            {
                PaidAmount += PrevPaymentRow.Amount;
            }

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
                Row["OutstandingAmount"] = Row["TotalAmount"];

                if (Convert.ToString(Row["DocumentStatus"]) == MFinanceConstants.AP_DOCUMENT_PAID)
                {
                    Row["OutstandingAmount"] = 0.0m;
                }

                if (Convert.ToString(Row["DocumentStatus"]) == MFinanceConstants.AP_DOCUMENT_PARTIALLY_PAID)
                {
                    Row["OutstandingAmount"] = Convert.ToDecimal(Row["TotalAmount"]) - GetPartPaidAmount(Convert.ToInt32(Row["ApDocumentId"]));
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
                            Row["DateDue"] = Convert.ToDateTime(Row["DateIssued"]).AddDays(Convert.ToDouble(Row["CreditTerms"]));
                        }

                        if (Row["DiscountDays"].GetType() != typeof(DBNull))
                        {
                            Row["DateDiscountUntil"] = Convert.ToDateTime(Row["DateIssued"]).AddDays(Convert.ToDouble(Row["DiscountDays"]));
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

            // Thread.Sleep(500);    enable only for simulation of slow (modem) connection!

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

        /// <summary>
        /// build the where clause
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <param name="AParametersArray"></param>
        /// <returns>the where clause</returns>
        private string BuildWhereClause(DataRow ACriteriaRow, out OdbcParameter[] AParametersArray)
        {
            ArrayList InternalParameters = new ArrayList();
            string WhereClause = "";

            if (FSearchSupplierOrInvoice) // Search by supplier name
            {
                if (((String)ACriteriaRow["SupplierId"]).Length > 0) // If the search box is empty, I'll not add this at all...
                {
                    WhereClause += String.Format(" AND {0} LIKE ?", PPartnerTable.GetPartnerShortNameDBName());
                    OdbcParameter Param = TTypedDataTable.CreateOdbcParameter(PPartnerTable.TableId, PPartnerTable.ColumnPartnerShortNameId);

                    // TODO: add LIKE % in the right place, defined by user
                    Param.Value = ACriteriaRow["SupplierId"] + "%";
                    InternalParameters.Add(Param);
                }
            }
            else // I'm looking for a list of outstanding invoices
            {
                // search by partner key
                Int64 SupplierPartnerKey = Convert.ToInt64(ACriteriaRow["PartnerKey"]);
                OdbcParameter Param;

                if (SupplierPartnerKey > 0)
                {
                    WhereClause += String.Format(" AND PUB_{0}.{1} = ?", AApSupplierTable.GetTableDBName(), AApSupplierTable.GetPartnerKeyDBName());
                    Param = TTypedDataTable.CreateOdbcParameter(AApSupplierTable.TableId, AApSupplierTable.ColumnPartnerKeyId);
                    Param.Value = SupplierPartnerKey;
                    InternalParameters.Add(Param);
                }

                WhereClause += String.Format(" AND {0}=?", AApDocumentTable.GetLedgerNumberDBName());
                Param = TTypedDataTable.CreateOdbcParameter(AApDocumentTable.TableId, AApDocumentTable.ColumnLedgerNumberId);
                Param.Value = (Int32)ACriteriaRow["LedgerNumber"];
                InternalParameters.Add(Param);

                WhereClause += String.Format(" AND {0} <> 'CANCELLED' AND {0} <> 'PAID'", AApDocumentTable.GetDocumentStatusDBName());
                decimal DaysPlus = (decimal)ACriteriaRow["DaysPlus"];

                if (DaysPlus >= 0)
                {
                    DateTime Deadline = DateTime.Now.AddDays((double)DaysPlus);
                    WhereClause += String.Format(" AND {0}+{1}<'{2}'",
                        AApDocumentTable.GetDateIssuedDBName(), AApDocumentTable.GetCreditTermsDBName(), Deadline.ToString("yyyyMMdd"));
                }
            }

            // Convert ArrayList to a array of ODBCParameters
            // seem to need to declare a type first
            AParametersArray = ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter))));
            InternalParameters = null;             // ensure this is GC'd

            return WhereClause;
        }

        /// <summary>
        /// Build the list of fields to be returned
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <returns></returns>
        private string BuildFieldList(DataRow ACriteriaRow)
        {
            if (!FSearchSupplierOrInvoice) // Find invoices
            {
                String DocTbl = "PUB_" + AApDocumentTable.GetTableDBName() + ".";

                return DocTbl + AApDocumentTable.GetApNumberDBName() + " AS ApNumber, " +
                       DocTbl + AApDocumentTable.GetDocumentCodeDBName() + " AS DocumentCode, " +
                       "PUB_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() + " AS PartnerShortName, " +
                       DocTbl + AApDocumentTable.GetCurrencyCodeDBName() + " AS CurrencyCode, " +
                       DocTbl + AApDocumentTable.GetTotalAmountDBName() + " AS TotalAmount, " +
                       DocTbl + AApDocumentTable.GetTotalAmountDBName() + " AS OutstandingAmount, " +
                       DocTbl + AApDocumentTable.GetDocumentStatusDBName() + " AS DocumentStatus, " +
                       DocTbl + AApDocumentTable.GetDateIssuedDBName() + " AS DateIssued, " +
                       DocTbl + AApDocumentTable.GetDateIssuedDBName() + " AS DateDue, " +
                       DocTbl + AApDocumentTable.GetDateIssuedDBName() + " AS DateDiscountUntil, " +
                       DocTbl + AApDocumentTable.GetCreditTermsDBName() + " AS CreditTerms, " +
                       DocTbl + AApDocumentTable.GetDiscountPercentageDBName() + " AS DiscountPercentage, " +
                       DocTbl + AApDocumentTable.GetDiscountDaysDBName() + " AS DiscountDays, " +
                       "\"none\" AS DiscountMsg, " +
                       "false AS Selected, " +
                       DocTbl + AApDocumentTable.GetCreditNoteFlagDBName() + " AS CreditNoteFlag, " +
                       DocTbl + AApDocumentTable.GetApDocumentIdDBName() + " AS DocumentId ";
            }
            else    // Find Suppliers
            {
                return "PUB_" + AApSupplierTable.GetTableDBName() + "." + AApSupplierTable.GetPartnerKeyDBName() + " AS PartnerKey, " +
                       "PUB_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName() + " AS PartnerShortName, " +
                       "PUB_" + AApSupplierTable.GetTableDBName() + "." + AApSupplierTable.GetCurrencyCodeDBName() + " AS CurrencyCode, " +
                       "PUB_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetStatusCodeDBName() + " AS StatusCode ";
            }
        }

        /// <summary>
        /// build the orderby clause
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <returns>the orderby clause</returns>
        private string BuildOrderByClause(DataRow ACriteriaRow)
        {
            if (FSearchSupplierOrInvoice) // Find Suppliers
            {
                return "PartnerShortName";
            }
            else // Find invoices
            {
                return "ApNumber DESC";
            }
        }

        /// <summary>
        /// Build the from clause
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <param name="AWhereClause">the where clause will be extended by the join conditions</param>
        /// <returns>the from clause</returns>
        private string BuildFromClause(DataRow ACriteriaRow, ref string AWhereClause)
        {
            AWhereClause += " AND " + "PUB_" + AApSupplierTable.GetTableDBName() + "." + AApSupplierTable.GetPartnerKeyDBName() + " = " + "PUB_" +
                            PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName();

            string FromClause = "PUB_" + AApSupplierTable.GetTableDBName() + ", PUB_" + PPartnerTable.GetTableDBName();

            if (!FSearchSupplierOrInvoice)
            {
                AWhereClause += " AND " + "PUB_" + AApDocumentTable.GetTableDBName() + "." + AApDocumentTable.GetPartnerKeyDBName() + " = " +
                                "PUB_" +
                                PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName();
                FromClause += (", PUB_" + AApDocumentTable.GetTableDBName());
            }

            return FromClause;
        }
    }
}