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
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Threading;

using Ict.Common;
using Ict.Common.Data;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Gift.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPartner.DataAggregates;

namespace Ict.Petra.Server.MFinance.Gift
{
    /// <summary>
    /// Base for the Gift Detail Find Screen.
    /// (Based on Partner.PartnerFind)
    /// </summary>
    public class TGiftDetailFind
    {
        /// <summary>Paged query object</summary>
        TPagedDataSet FPagedDataSetObject;

        /// <summary>Asynchronous execution control object</summary>
        TAsynchronousExecutionProgress FAsyncExecProgress;

        /// <summary>Thread that is used for asynchronously executing the Find query</summary>
        Thread FFindThread;

        /// <summary>Returns reference to the Asynchronous execution control object to the caller</summary>
        public TAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                return FAsyncExecProgress;
            }
        }

        /// <summary>
        /// Procedure to execute a Find query. Although the full
        /// query results are retrieved from the DB and stored internally in an object,
        /// data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        ///
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters</param>
        /// <returns>void</returns>
        public void PerformSearch(DataTable ACriteriaData)
        {
            String CustomWhereCriteria;
            Hashtable ColumnNameMapping;

            OdbcParameter[] ParametersArray;
            String FieldList;
            String FromClause;
            String WhereClause;
            System.Text.StringBuilder sb;
            TLogging.LogAtLevel(7, "TGiftDetailFind.PerformSearch called.");

            FAsyncExecProgress = new TAsynchronousExecutionProgress();
            FPagedDataSetObject = new TPagedDataSet(new GiftBatchTDSAGiftDetailTable());

            /* Pass the TAsynchronousExecutionProgress object to FPagedDataSetObject so that it
             * can update execution status */
            FPagedDataSetObject.AsyncExecProgress = FAsyncExecProgress;

            // Register Event Handler for the StopAsyncronousExecution event
            FAsyncExecProgress.StopAsyncronousExecution += new System.EventHandler(this.StopSearch);

            // Build WHERE criteria string based on AFindCriteria
            CustomWhereCriteria = BuildCustomWhereCriteria(ACriteriaData, out ParametersArray);

            //
            // Set up find parameters
            //
            ColumnNameMapping = new Hashtable();

            // Create Field List
            sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_batch_number_i", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_gift_transaction_number_i", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_detail_number_i", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_confidential_gift_flag_l", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_gift_amount_n", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift.a_receipt_number_i", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "DonorPartner.p_partner_short_name_c DonorPartnerShortName", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "RecipientPartner.p_partner_short_name_c RecipientPartnerShortName", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_motivation_group_code_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_motivation_detail_code_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift.a_date_entered_d", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_cost_centre_code_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_gift_comment_one_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_gift_comment_two_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail.a_gift_comment_three_c", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "PUB.a_gift_batch.a_batch_status_c", Environment.NewLine);

            // short
            FieldList = sb.ToString();

            // Create FROM From Clause
            sb = new System.Text.StringBuilder();
            System.Text.StringBuilder sbWhereClause = new System.Text.StringBuilder();

            sb.AppendFormat("{0},{1}", "PUB.a_gift_detail", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_gift_batch", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.p_partner DonorPartner", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "PUB.p_partner RecipientPartner", Environment.NewLine);
            sbWhereClause.AppendFormat("{0}{1}",
                "DonorPartner.p_partner_key_n = PUB.a_gift.p_donor_key_n " +
                "AND RecipientPartner.p_partner_key_n = PUB.a_gift_detail.p_recipient_key_n " +
                "AND PUB.a_gift.a_ledger_number_i = PUB.a_gift_detail.a_ledger_number_i " +
                "AND PUB.a_gift.a_batch_number_i = PUB.a_gift_detail.a_batch_number_i " +
                "AND PUB.a_gift.a_gift_transaction_number_i = PUB.a_gift_detail.a_gift_transaction_number_i " +
                "AND PUB.a_gift_batch.a_ledger_number_i = PUB.a_gift_detail.a_ledger_number_i " +
                "AND PUB.a_gift_batch.a_batch_number_i = PUB.a_gift_detail.a_batch_number_i", Environment.NewLine);

            FromClause = sb.ToString();
            WhereClause = CustomWhereCriteria;

            if (WhereClause.StartsWith(" AND") == true)
            {
                WhereClause = WhereClause.Substring(4);
            }

            if (sbWhereClause.ToString().Length > 0)
            {
                WhereClause += " AND " + sbWhereClause.ToString();
            }

            FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(FieldList,
                FromClause,
                WhereClause,
                "PUB.a_gift_detail.a_batch_number_i, PUB.a_gift_detail.a_gift_transaction_number_i, PUB.a_gift_detail.a_detail_number_i",
                ColumnNameMapping,
                ParametersArray);

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
            DataTable ReturnValue;

            TLogging.LogAtLevel(7, "TPartnerFind.GetDataPagedResult called.");
            ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);
            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;
            
            if (!ReturnValue.Columns.Contains("BatchPosted"))
            {
            	ReturnValue.Columns.Add(new DataColumn("BatchPosted", typeof(bool)));
            }
            
            foreach (DataRow Row in ReturnValue.Rows)
            {
            	if (Row["a_batch_status_c"].ToString() == MFinanceConstants.BATCH_POSTED)
            	{
            		Row["BatchPosted"] = true;
            	}
            	else
            	{
            		Row["BatchPosted"] = false;
            	}
            }

            if (ReturnValue != null)
            {
                TPPartnerAddressAggregate.ApplySecurity(ref ReturnValue);
            }

            return ReturnValue;
        }

        /// <summary>
        /// Used internally to build a SQL WHERE criteria from the AFindCriteria HashTable.
        ///
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Partner Find parameters</param>
        /// <param name="AParametersArray">An array holding 1..n instantiated OdbcParameters
        /// (including parameter Value)</param>
        /// <returns>SQL WHERE criteria
        /// </returns>
        private static String BuildCustomWhereCriteria(DataTable ACriteriaData, out OdbcParameter[] AParametersArray)
        {
            String CustomWhereCriteria = "";
            DataTable CriteriaDataTable;
            DataRow CriteriaRow;
            ArrayList InternalParameters;

            CriteriaDataTable = ACriteriaData;
            CriteriaRow = CriteriaDataTable.Rows[0];
            InternalParameters = new ArrayList();

            if (CriteriaRow["Ledger"].ToString().Length > 0)
            {
                // Searched DB Field: 'a_ledger_number_i'
                new TDynamicSearchHelper(AGiftDetailTable.TableId,
                    AGiftDetailTable.ColumnLedgerNumberId, CriteriaRow, "Ledger", "",
                    ref CustomWhereCriteria, ref InternalParameters);
            }

            // Searched DB Field: 'a_batch_number_i'
            if ((CriteriaRow["Batch"] != null) && (CriteriaRow["Batch"] != System.DBNull.Value))
            {
                // do manually otherwise 0 gets changed to a string and we get a crash
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    "PUB_" + TTypedDataTable.GetTableNameSQL(AGiftDetailTable.TableId) + "." + AGiftDetailTable.GetBatchNumberDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                miParam.Value = (object)(CriteriaRow["Batch"]);
                InternalParameters.Add(miParam);
            }

            // Searched DB Field: 'a_gift_transaction_number_i'
            if ((CriteriaRow["Transaction"] != null) && (CriteriaRow["Transaction"] != System.DBNull.Value))
            {
                // do manually otherwise 0 gets changed to a string and we get a crash
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    "PUB_" + TTypedDataTable.GetTableNameSQL(AGiftDetailTable.TableId) + "." + AGiftDetailTable.GetGiftTransactionNumberDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                miParam.Value = (object)(CriteriaRow["Transaction"]);
                InternalParameters.Add(miParam);
            }

            // Searched DB Field: 'a_receipt_number_i'
            if ((CriteriaRow["Receipt"] != null) && (CriteriaRow["Receipt"] != System.DBNull.Value))
            {
                // do manually otherwise 0 gets changed to a string and we get a crash
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    "PUB_" + TTypedDataTable.GetTableNameSQL(AGiftTable.TableId) + "." + AGiftTable.GetReceiptNumberDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                miParam.Value = (object)(CriteriaRow["Receipt"]);
                InternalParameters.Add(miParam);
            }

            // Searched DB Field: 'a_motivation_group_code_c'
            if (CriteriaRow["MotivationGroup"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(AGiftDetailTable.TableId,
                    AGiftDetailTable.ColumnMotivationGroupCodeId, CriteriaRow, "MotivationGroup", "",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_motivation_detail_code_c'
            if (CriteriaRow["MotivationDetail"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(AGiftDetailTable.TableId,
                    AGiftDetailTable.ColumnMotivationDetailCodeId, CriteriaRow, "MotivationDetail", "",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_gift_comment_one_c'
            if (CriteriaRow["Comment1"].ToString().Length > 0)
            {
                CriteriaRow.Table.Columns.Add(new DataColumn("Comment1Match"));
                CriteriaRow["Comment1Match"] = "CONTAINS";

                new TDynamicSearchHelper(AGiftDetailTable.TableId,
                    AGiftDetailTable.ColumnGiftCommentOneId, CriteriaRow, "Comment1", "Comment1Match",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_donor_key_n'
            if (((Int64)CriteriaRow["Donor"]) > 0)
            {
                new TDynamicSearchHelper(AGiftTable.TableId,
                    AGiftTable.ColumnDonorKeyId, CriteriaRow, "Donor", "",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'p_recipient_key_n'
            if (((Int64)CriteriaRow["Recipient"]) > 0)
            {
                new TDynamicSearchHelper(AGiftDetailTable.TableId,
                    AGiftDetailTable.ColumnRecipientKeyId, CriteriaRow, "Recipient", "",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_date_entered_d'
            if ((CriteriaRow["From"] != System.DBNull.Value) && (CriteriaRow["To"] != System.DBNull.Value)
                && (CriteriaRow["From"] == CriteriaRow["To"]))
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    AGiftTable.GetDateEnteredDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.DateTime, 10);
                miParam.Value = (object)(CriteriaRow["From"]);
                InternalParameters.Add(miParam);
            }
            else
            {
                if (CriteriaRow["From"] != System.DBNull.Value)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} >= ?", CustomWhereCriteria,
                        AGiftTable.GetDateEnteredDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.DateTime, 10);
                    miParam.Value = (object)(CriteriaRow["From"]);
                    InternalParameters.Add(miParam);
                }

                if (CriteriaRow["To"] != System.DBNull.Value)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} <= ?", CustomWhereCriteria,
                        AGiftTable.GetDateEnteredDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.DateTime, 10);
                    miParam.Value = (object)(CriteriaRow["To"]);
                    InternalParameters.Add(miParam);
                }
            }

            // Searched DB Field: 'a_gift_amount_n'
            if ((CriteriaRow["MinAmount"].ToString().Length > 0) && (CriteriaRow["MaxAmount"].ToString().Length > 0)
                && (CriteriaRow["MinAmount"] == CriteriaRow["MaxAmount"]))
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    AGiftDetailTable.GetGiftAmountDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                miParam.Value = (object)(CriteriaRow["MinAmount"]);
                InternalParameters.Add(miParam);
            }
            else
            {
                if (CriteriaRow["MinAmount"].ToString().Length > 0)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} >= ?", CustomWhereCriteria,
                        AGiftDetailTable.GetGiftAmountDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                    miParam.Value = (object)(CriteriaRow["MinAmount"]);
                    InternalParameters.Add(miParam);
                }

                if (CriteriaRow["MaxAmount"].ToString().Length > 0)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} <= ?", CustomWhereCriteria,
                        AGiftDetailTable.GetGiftAmountDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                    miParam.Value = (object)(CriteriaRow["MaxAmount"]);
                    InternalParameters.Add(miParam);
                }
            }

//           TLogging.LogAtLevel(7, "CustomWhereCriteria: " + CustomWhereCriteria);

            /* Convert ArrayList to a array of ODBCParameters
             * seem to need to declare a type first
             */
            AParametersArray = ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter))));
            InternalParameters = null;             // ensure this is GC'd

            return CustomWhereCriteria;
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
             * (Microsoft recommends doing it this way!)
             */
            TLogging.LogAtLevel(7, "TGiftDetailFindUIConnector.StopSearch: Starting StopQuery thread...");

            ThreadStart ThreadStartDelegate = new ThreadStart(FPagedDataSetObject.StopQuery);
            StopQueryThread = new Thread(ThreadStartDelegate);
            StopQueryThread.Start();

            /* It might take some time until the executing query is cancelled by the DB,
             * but we consider it as done since the application can 'forget' about the
             * result of the cancellation process (but beware of executing another query
             * while the other is stopping - this leads to ADO.NET errors that state that
             * a ADO.NET command is still executing!
             */

            TLogging.LogAtLevel(7, "TGiftDetailFindUIConnector.StopSearch: Query cancelled!");
        }
    }
}