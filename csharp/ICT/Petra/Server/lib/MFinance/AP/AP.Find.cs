//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       timop
//
// Copyright 2004-2010 by OM International
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
using Ict.Petra.Server.MCommon;
using Ict.Petra.Shared.Interfaces.AsynchronousExecution;

#region ManualCode
using Ict.Petra.Server.MFinance;
using Ict.Petra.Shared.MFinance;
using Ict.Petra.Shared.MFinance.AP.Data;
using Ict.Petra.Server.MFinance.AP.Data.Access;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Shared.Interfaces.MFinance.AP.UIConnectors;
#endregion ManualCode

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

        /// <summary>Returns reference to the Asynchronous execution control object to the caller</summary>
        public IAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                return FAsyncExecProgress;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public TFindUIConnector() : base()
        {
        }

        #region ManualCode

        /// <summary>if true, then search for supplier; if false, then search for invoice</summary>
        private bool FSearchSupplierOrInvoice = false;

        /// <summary>if true, then search for by partner key; if false, then search by supplier name</summary>
        private bool FSearchByPartnerKeyOrSupplierName = false;

        /// <summary>
        /// Find a supplier or a list of suppliers matching the search criteria
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Find parameters</param>
        public void FindSupplier(DataTable ACriteriaData)
        {
            FSearchSupplierOrInvoice = true;
            PerformSearch(ACriteriaData);
        }

        /// <summary>
        /// Find a list of invoices matching the search criteria
        /// </summary>
        /// <param name="ACriteriaData">HashTable containing non-empty Find parameters</param>
        public void FindInvoices(DataTable ACriteriaData)
        {
            FSearchSupplierOrInvoice = false;
            PerformSearch(ACriteriaData);
        }

        #endregion

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

            // Pass the TAsynchronousExecutionProgress object to FPagedDataSetObject so that it
            // can update execution status
            FPagedDataSetObject.AsyncExecProgress = FAsyncExecProgress;

            // Register Event Handler for the StopAsyncronousExecution event
            FAsyncExecProgress.StopAsyncronousExecution += new System.EventHandler(this.StopSearch);

            DataRow CriteriaRow = PrepareDataRow(ACriteriaData);

            // Build WHERE criteria string based on ACriteriaData
            OdbcParameter[] ParametersArray;
            string WhereClause = BuildWhereClause(CriteriaRow, out ParametersArray);

            if (WhereClause.StartsWith(" AND") == true)
            {
                WhereClause = WhereClause.Substring(4);
            }

            string FieldList = BuildFieldList(CriteriaRow);
            string FromClause = BuildFromClause(CriteriaRow, ref WhereClause);
            string OrderByClause = BuildOrderByClause(CriteriaRow);

            Hashtable ColumnNameMapping = new Hashtable();
            FPagedDataSetObject.FindParameters = new TPagedDataSet.TAsyncFindParameters(
                FieldList,
                FromClause,
                WhereClause,
                OrderByClause,
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

#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetDataPagedResult called.");
            }
#endif
            ReturnValue = FPagedDataSetObject.GetData(APage, APageSize);
            ATotalPages = FPagedDataSetObject.TotalPages;
            ATotalRecords = FPagedDataSetObject.TotalRecords;

            // Thread.Sleep(500);    enable only for simulation of slow (modem) connection!

            #region ManualCode

            // TODO TAccountsPayableAggregate.ApplySecurity(ref ReturnValue);
            #endregion

            return ReturnValue;
        }

        /// <summary>
        /// prepares the datarow; can add new columns
        /// </summary>
        /// <param name="ACriteriaTable"></param>
        /// <returns></returns>
        private DataRow PrepareDataRow(DataTable ACriteriaTable)
        {
            #region ManualCode
            try
            {
                // try if this is a partner key
                Int64 SupplierPartnerKey = Convert.ToInt64(ACriteriaTable.Rows[0]["SupplierId"]);
                ACriteriaTable.Columns.Add("PartnerKey", typeof(Int64));
                ACriteriaTable.Rows[0]["PartnerKey"] = SupplierPartnerKey;
                FSearchByPartnerKeyOrSupplierName = true;
            }
            catch (Exception)
            {
                // this is just a partner short name
                FSearchByPartnerKeyOrSupplierName = false;
            }
            #endregion
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
            string WhereClause = "";
            ArrayList InternalParameters = new ArrayList();

            #region ManualCode

            if (FSearchByPartnerKeyOrSupplierName == true)
            {
                // search by partner key
                Int64 SupplierPartnerKey = Convert.ToInt64(ACriteriaRow["PartnerKey"]);
                WhereClause += String.Format(" AND PUB_{0}.{1} = ?", AApSupplierTable.GetTableDBName(), AApSupplierTable.GetPartnerKeyDBName());
                OdbcParameter Param = TTypedDataTable.CreateOdbcParameter(AApSupplierTable.TableId, AApSupplierTable.ColumnPartnerKeyId);
                Param.Value = SupplierPartnerKey;
                InternalParameters.Add(Param);
            }
            else
            {
                // search by supplier name
                WhereClause += String.Format(" AND {0} LIKE ?", PPartnerTable.GetPartnerShortNameDBName());
                OdbcParameter Param = TTypedDataTable.CreateOdbcParameter(PPartnerTable.TableId, PPartnerTable.ColumnPartnerShortNameId);

                // TODO: add LIKE % in the right place, defined by user
                Param.Value = ACriteriaRow["SupplierId"] + "%";
                InternalParameters.Add(Param);
            }

            #endregion

            // Convert ArrayList to a array of ODBCParameters
            // seem to need to declare a type first
            AParametersArray = ((OdbcParameter[])(InternalParameters.ToArray(typeof(OdbcParameter))));
            InternalParameters = null;             // ensure this is GC'd

            return WhereClause;
        }

        /// <summary>
        /// build the list of fields to be returned
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <returns></returns>
        private string BuildFieldList(DataRow ACriteriaRow)
        {
            #region ManualCode

            if (FSearchSupplierOrInvoice == false)
            {
                // TODO: FSearchSupplierOrInvoice: select invoices
                return "";
            }
            else
            {
                // TODO: add amount of outstanding invoices etc
                return "PUB_" + AApSupplierTable.GetTableDBName() + "." + AApSupplierTable.GetPartnerKeyDBName() + "," +
                       "PUB_" + PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerShortNameDBName();
            }

            #endregion
        }

        /// <summary>
        /// build the orderby clause
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <returns>the orderby clause</returns>
        private string BuildOrderByClause(DataRow ACriteriaRow)
        {
            #region ManualCode
            return PPartnerTable.GetPartnerShortNameDBName();
            #endregion
        }

        /// <summary>
        /// build the from clause
        /// </summary>
        /// <param name="ACriteriaRow"></param>
        /// <param name="AWhereClause">the where clause will be extended by the join conditions</param>
        /// <returns>the from clause</returns>
        private string BuildFromClause(DataRow ACriteriaRow, ref string AWhereClause)
        {
            #region ManualCode
            AWhereClause += " AND " + "PUB_" + AApSupplierTable.GetTableDBName() + "." + AApSupplierTable.GetPartnerKeyDBName() + " = " + "PUB_" +
                            PPartnerTable.GetTableDBName() + "." + PPartnerTable.GetPartnerKeyDBName();
            return "PUB_" + AApSupplierTable.GetTableDBName() + ", PUB_" + PPartnerTable.GetTableDBName();
            #endregion
        }
    }
}