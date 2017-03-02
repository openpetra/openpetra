//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       peters
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
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Threading;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.Session;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.Account.Data;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MPartner.DataAggregates;

namespace Ict.Petra.Server.MFinance.GL
{
    /// <summary>
    /// Base for the GL Transaction Find Screen.
    /// (Based on Partner.PartnerFind).
    /// Utilised by the 'TGLTransactionFindUIConnector' Class.
    /// </summary>
    public class TGLTransactionFind
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
        /// Procedure to execute a Find query. Although the full query results are retrieved from the DB and stored
        /// internally in an object, data will be returned in 'pages' of data, each page holding a defined number
        /// of records.
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Find parameters.</param>
        public void PerformSearch(DataTable ACriteriaData)
        {
            String CustomWhereCriteria;
            Hashtable ColumnNameMapping;

            OdbcParameter[] ParametersArray;
            String FieldList;
            String FromClause;
            String WhereClause;
            System.Text.StringBuilder sb;
            TLogging.LogAtLevel(7, "TGLTransactionFind.PerformSearch called.");

            FPagedDataSetObject = new TPagedDataSet(new ATransactionTable());

            // Build WHERE criteria string based on AFindCriteria
            CustomWhereCriteria = BuildCustomWhereCriteria(ACriteriaData, out ParametersArray);

            //
            // Set up find parameters
            //
            ColumnNameMapping = new Hashtable();

            // Create Field List
            sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0},{1}", "PUB_a_transaction.*", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_journal.a_journal_description_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_batch.a_batch_description_c", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB.a_batch.a_batch_period_i", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "PUB.a_batch.a_batch_year_i", Environment.NewLine);

            // short
            FieldList = sb.ToString();

            // Create FROM From Clause
            sb = new System.Text.StringBuilder();
            System.Text.StringBuilder sbWhereClause = new System.Text.StringBuilder();

            sb.AppendFormat("{0},{1}", "PUB_a_transaction", Environment.NewLine);
            sb.AppendFormat("{0},{1}", "PUB_a_journal", Environment.NewLine);
            sb.AppendFormat("{0}{1}", "PUB_a_batch", Environment.NewLine);

            sbWhereClause.AppendFormat("{0}{1}",
                "PUB_a_transaction.a_ledger_number_i = PUB_a_batch.a_ledger_number_i " +
                "AND PUB_a_transaction.a_batch_number_i = PUB_a_batch.a_batch_number_i " +
                "AND PUB_a_transaction.a_ledger_number_i = PUB_a_journal.a_ledger_number_i " +
                "AND PUB_a_transaction.a_batch_number_i = PUB_a_journal.a_batch_number_i " +
                "AND PUB_a_transaction.a_journal_number_i = PUB_a_journal.a_journal_number_i", Environment.NewLine);

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
                "PUB_a_transaction.a_batch_number_i, PUB_a_transaction.a_journal_number_i, PUB_a_transaction.a_transaction_number_i",
                ColumnNameMapping,
                ParametersArray);

            string session = TSession.GetSessionID();

            //
            // Start the Find Thread
            //
            try
            {
                ThreadStart myThreadStart = delegate {
                    FPagedDataSetObject.ExecuteQuery(session, "GL Transaction Find");
                };
                FFindThread = new Thread(myThreadStart);
                FFindThread.Name = "GLTransactionFind" + Guid.NewGuid().ToString();
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

            TLogging.LogAtLevel(7, "TGLTransactionFind.GetDataPagedResult called.");
            ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);
            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;

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
                new TDynamicSearchHelper(ATransactionTable.TableId,
                    ATransactionTable.ColumnLedgerNumberId, CriteriaRow, "Ledger", "",
                    ref CustomWhereCriteria, ref InternalParameters);
            }

            // Searched DB Field: 'a_batch_number_i'
            if ((CriteriaRow["BatchNumber"] != null) && (CriteriaRow["BatchNumber"] != System.DBNull.Value))
            {
                // do manually otherwise 0 gets changed to a string and we get a crash
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    "PUB_" + TTypedDataTable.GetTableNameSQL(ATransactionTable.TableId) + "." + ATransactionTable.GetBatchNumberDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                miParam.Value = (object)(CriteriaRow["BatchNumber"]);
                InternalParameters.Add(miParam);
            }

            // Searched DB Field: 'a_transaction_status_l'
            if (!string.IsNullOrEmpty(CriteriaRow["TransactionStatus"].ToString()))
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    "CAST (PUB_" + TTypedDataTable.GetTableNameSQL(
                        ATransactionTable.TableId) + "." + ATransactionTable.GetTransactionStatusDBName() + " as INT)");
                OdbcParameter miParam = new OdbcParameter("", DbType.Boolean);
                miParam.Value = (object)(CriteriaRow["TransactionStatus"]);
                InternalParameters.Add(miParam);
            }

            // Searched DB Field: 'a_batch_description_c'
            if (!string.IsNullOrEmpty(CriteriaRow["BatchDescription"].ToString()))
            {
                CriteriaRow.Table.Columns.Add(new DataColumn("BatchDescriptionMatch"));
                CriteriaRow["BatchDescriptionMatch"] = "CONTAINS";

                new TDynamicSearchHelper(ABatchTable.TableId,
                    ABatchTable.ColumnBatchDescriptionId, CriteriaRow, "BatchDescription", "BatchDescriptionMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_journal_description_c'
            if (CriteriaRow["JournalDescription"].ToString().Length > 0)
            {
                CriteriaRow.Table.Columns.Add(new DataColumn("JournalDescriptionMatch"));
                CriteriaRow["JournalDescriptionMatch"] = "CONTAINS";

                new TDynamicSearchHelper(AJournalTable.TableId,
                    AJournalTable.ColumnJournalDescriptionId, CriteriaRow, "JournalDescription", "JournalDescriptionMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_narrative_c'
            if (CriteriaRow["Narrative"].ToString().Length > 0)
            {
                CriteriaRow.Table.Columns.Add(new DataColumn("NarrativeMatch"));
                CriteriaRow["NarrativeMatch"] = "CONTAINS";

                new TDynamicSearchHelper(ATransactionTable.TableId,
                    ATransactionTable.ColumnNarrativeId, CriteriaRow, "Narrative", "NarrativeMatch",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_cost_centre_code_c'
            if (CriteriaRow["CostCentreCode"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(ATransactionTable.TableId,
                    ATransactionTable.ColumnCostCentreCodeId, CriteriaRow, "CostCentreCode", "",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_account_code_c'
            if (CriteriaRow["AccountCode"].ToString().Length > 0)
            {
                new TDynamicSearchHelper(ATransactionTable.TableId,
                    ATransactionTable.ColumnAccountCodeId, CriteriaRow, "AccountCode", "",
                    ref CustomWhereCriteria,
                    ref InternalParameters);
            }

            // Searched DB Field: 'a_transaction_date_d'
            if ((CriteriaRow["From"] != System.DBNull.Value) && (CriteriaRow["To"] != System.DBNull.Value)
                && (CriteriaRow["From"] == CriteriaRow["To"]))
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    ATransactionTable.GetTransactionDateDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.DateTime, 10);
                miParam.Value = (object)(CriteriaRow["From"]);
                InternalParameters.Add(miParam);
            }
            else
            {
                if (CriteriaRow["From"] != System.DBNull.Value)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} >= ?", CustomWhereCriteria,
                        ATransactionTable.GetTransactionDateDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.DateTime, 10);
                    miParam.Value = (object)(CriteriaRow["From"]);
                    InternalParameters.Add(miParam);
                }

                if (CriteriaRow["To"] != System.DBNull.Value)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} <= ?", CustomWhereCriteria,
                        ATransactionTable.GetTransactionDateDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.DateTime, 10);
                    miParam.Value = (object)(CriteriaRow["To"]);
                    InternalParameters.Add(miParam);
                }
            }

            // Searched DB Field: 'a_amount_in_base_currency_n'
            if ((CriteriaRow["MinAmount"].ToString().Length > 0) && (CriteriaRow["MaxAmount"].ToString().Length > 0)
                && (CriteriaRow["MinAmount"] == CriteriaRow["MaxAmount"]))
            {
                CustomWhereCriteria = String.Format("{0} AND {1} = ?", CustomWhereCriteria,
                    ATransactionTable.GetAmountInBaseCurrencyDBName());
                OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                miParam.Value = (object)(CriteriaRow["MinAmount"]);
                InternalParameters.Add(miParam);
            }
            else
            {
                if (CriteriaRow["MinAmount"].ToString().Length > 0)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} >= ?", CustomWhereCriteria,
                        ATransactionTable.GetAmountInBaseCurrencyDBName());
                    OdbcParameter miParam = new OdbcParameter("", OdbcType.Int, 10);
                    miParam.Value = (object)(CriteriaRow["MinAmount"]);
                    InternalParameters.Add(miParam);
                }

                if (CriteriaRow["MaxAmount"].ToString().Length > 0)
                {
                    CustomWhereCriteria = String.Format("{0} AND {1} <= ?", CustomWhereCriteria,
                        ATransactionTable.GetAmountInBaseCurrencyDBName());
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
             * (Microsoft recommends doing it this way!)
             */
            TLogging.LogAtLevel(7, "TGLTransactionFindUIConnector.StopSearch: Starting StopQuery thread...");

            StopQueryThread = new Thread(new ThreadStart(FPagedDataSetObject.StopQuery));
            StopQueryThread.Name = "GLTransactionFindStopQuery" + Guid.NewGuid().ToString();
            StopQueryThread.Start();

            /* It might take some time until the executing query is cancelled by the DB,
             * but we consider it as done since the application can 'forget' about the
             * result of the cancellation process (but beware of executing another query
             * while the other is stopping - this leads to ADO.NET errors that state that
             * a ADO.NET command is still executing!
             */

            TLogging.LogAtLevel(7, "TGLTransactionFindUIConnector.StopSearch: Query cancelled!");
        }
    }
}
