//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
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
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;

using Ict.Common;
using Ict.Common.Data;
using Ict.Common.DB;
using Ict.Common.Remoting.Shared;
using Ict.Common.Remoting.Server;
using Ict.Common.Remoting.Client;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MPartner.Partner.Data;
using Ict.Petra.Server.MPartner.Partner.Data.Access;

namespace Ict.Petra.Server.MCommon
{
    /// <summary>
    /// Contains utility and helper functions that are Petra Server specific and are
    /// shared between several Petra modules.
    /// </summary>
    public class MCommonMain
    {
        #region Functions

        /// <summary>
        /// Retrieves the Partner ShortName, the PartnerClass and PartnerStatus.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey to identify the Partner.</param>
        /// <param name="APartnerShortName">Returns the ShortName.</param>
        /// <param name="APartnerClass">Returns the PartnerClass (FAMILY, ORGANISATION, etc).</param>
        /// <param name="APartnerStatus">Returns the PartnerStatus (eg. ACTIVE, DIED).</param>
        /// <returns>True if partner was found, otherwise false.</returns>
        public static Boolean RetrievePartnerShortName(Int64 APartnerKey,
            out String APartnerShortName,
            out TPartnerClass APartnerClass,
            out TStdPartnerStatusCode APartnerStatus)
        {
            bool NewTransaction = false;
            bool Result = false;
            TDBTransaction ReadTransaction;

            TPartnerClass tmpPartnerClass = new TPartnerClass();
            TStdPartnerStatusCode tmpPartnerStatus = new TStdPartnerStatusCode();
            string tmpPartnerShortName = "";

            if (APartnerKey != 0)
            {
                try
                {
                    ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                        TEnforceIsolationLevel.eilMinimum,
                        out NewTransaction);

                    Result = RetrievePartnerShortName(APartnerKey,
                        out tmpPartnerShortName,
                        out tmpPartnerClass,
                        out tmpPartnerStatus,
                        ReadTransaction);

                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "RetrievePartnerShortName: committed own transaction.");
                    }
                }
                catch (Exception)
                {
                    TLogging.Log(String.Format("Problem retrieveing partner short name for Partner {0}", APartnerKey));
                    TLogging.LogStackTrace(TLoggingType.ToLogfile);
                }
            }
            else
            {
                APartnerClass = new TPartnerClass();

                Result = true;                //partner key key 0 should be valid
            }

            APartnerShortName = tmpPartnerShortName;
            APartnerClass = tmpPartnerClass;
            APartnerStatus = tmpPartnerStatus;

            return Result;
        }

        /// <summary>
        /// Retrieves the Partner ShortName, the PartnerClass and PartnerStatus.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey to identify the Partner.</param>
        /// <param name="APartnerShortName">Returns the ShortName.</param>
        /// <param name="APartnerClass">Returns the PartnerClass (FAMILY, ORGANISATION, etc).</param>
        /// <param name="APartnerStatus">Returns the PartnerStatus (eg. ACTIVE, DIED).</param>
        /// <param name="ATransaction">Open DB Transaction.</param>
        /// <returns>True if partner was found, otherwise false.</returns>
        public static Boolean RetrievePartnerShortName(Int64 APartnerKey,
            out String APartnerShortName,
            out TPartnerClass APartnerClass,
            out TStdPartnerStatusCode APartnerStatus,
            TDBTransaction ATransaction)
        {
            Boolean ReturnValue;
            StringCollection RequiredColumns;
            PPartnerTable PartnerTable;

            // initialise out Arguments
            APartnerShortName = "";

            // Default. This is not really correct but the best compromise if PartnerKey is 0 or Partner isn't found since we have an enum here.
            APartnerClass = TPartnerClass.FAMILY;

            // Default. This is not really correct but the best compromise if PartnerKey is 0 or Partner isn't found since we have an enum here.
            APartnerStatus = TStdPartnerStatusCode.spscINACTIVE;

            if (APartnerKey != 0)
            {
                // only some fields are needed
                RequiredColumns = new StringCollection();
                RequiredColumns.Add(PPartnerTable.GetPartnerShortNameDBName());
                RequiredColumns.Add(PPartnerTable.GetPartnerClassDBName());
                RequiredColumns.Add(PPartnerTable.GetStatusCodeDBName());

                PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, RequiredColumns, ATransaction, null, 0, 0);

                if (PartnerTable.Rows.Count == 0)
                {
                    ReturnValue = false;
                }
                else
                {
                    // since we loaded by primary key there must just be one partner row
                    APartnerShortName = PartnerTable[0].PartnerShortName;
                    APartnerClass = SharedTypes.PartnerClassStringToEnum(PartnerTable[0].PartnerClass);

                    APartnerStatus = SharedTypes.StdPartnerStatusCodeStringToEnum(PartnerTable[0].StatusCode);
                    ReturnValue = true;
                }
            }
            else
            {
                // Return result as valid if Partner Key is 0.
                ReturnValue = true;
            }

            return ReturnValue;
        }

        /// <summary>
        /// Checks for the existance of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner to check for.</param>
        /// <param name="AMustNotBeMergedPartner">Set to true to check whether the Partner
        /// must not be a Merged Partner.</param>
        /// <returns>True if the Partner exists (taking AMustNotBeMergedPartner into consideration),
        /// otherwise False.</returns>
        public static bool CheckPartnerExists1(Int64 APartnerKey, bool AMustNotBeMergedPartner)
        {
            return CheckPartnerExists2(APartnerKey, AMustNotBeMergedPartner) != null;
        }

        /// <summary>
        /// Checks for the existance of a Partner.
        /// </summary>
        /// <param name="APartnerKey">PartnerKey of the Partner to check for.</param>
        /// <param name="AMustNotBeMergedPartner">Set to true to check whether the Partner
        /// must not be a Merged Partner.</param>
        /// <returns>An instance of PPartnerRow if the Partner exists (taking AMustNotBeMergedPartner into consideration),
        /// otherwise null.</returns>
        public static PPartnerRow CheckPartnerExists2(Int64 APartnerKey, bool AMustNotBeMergedPartner)
        {
            PPartnerRow ReturnValue = null;
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            PPartnerTable PartnerTable;

            if (APartnerKey != 0)
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(IsolationLevel.ReadCommitted,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    PartnerTable = PPartnerAccess.LoadByPrimaryKey(APartnerKey, ReadTransaction);
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel(7, "CheckPartnerExists: committed own transaction.");
                    }
                }

                if (PartnerTable.Rows.Count != 0)
                {
                    if (AMustNotBeMergedPartner)
                    {
                        if (SharedTypes.StdPartnerStatusCodeStringToEnum(
                                PartnerTable[0].StatusCode) != TStdPartnerStatusCode.spscMERGED)
                        {
                            ReturnValue = PartnerTable[0];
                        }
                    }
                    else
                    {
                        ReturnValue = PartnerTable[0];
                    }
                }
            }

            return ReturnValue;
        }

        #endregion
    }
    #region TPagedDataSet

    /// <summary>
    /// Universal class that can run any SQL SELECT query and return the resulting
    /// rows in 'pages' that contain a certain amount of rows.
    /// Especially useful for Find screens, but also for other screens where a lot of
    /// data is requested but might never be fully accessed.
    ///
    /// Execution of the query can happen asynchronously by executing ExecuteQuery
    /// in a separate Thread!
    ///
    /// </summary>
    public class TPagedDataSet : object
    {
        /// <summary>Asynchronous execution control object</summary>
        TAsynchronousExecutionProgress FAsyncExecProgress;

        /// <summary>An instance of TAsyncFindParameters containing parameters for the query execution</summary>
        TAsyncFindParameters FFindParameters;

        /// <summary>DataAdapter that is used to execute the query</summary>
        DbDataAdapter FDataAdapter;

        /// <summary>SQL command that will be used to execute the query</summary>
        String FSelectSQL;

        /// <summary>DataTable holding all DataRows that are returned by the query</summary>
        DataTable FTmpDataTable = new DataTable();

        /// <summary>DataTable holding DataRows for the current Page</summary>
        DataTable FPageDataTable;

        /// <summary>Last retrieved page</summary>
        System.Int32 FLastRetrievedPage = -1;

        /// <summary>Property value.</summary>
        System.Int32 FTotalRecords = -1;

        /// <summary>Property value.</summary>
        System.Int16 FTotalPages = -1;

        /// <summary>Pass in an instance of TAsyncFindParameters to set the parameters for the query execution</summary>
        public TAsyncFindParameters FindParameters
        {
            get
            {
                return FFindParameters;
            }

            set
            {
                FFindParameters = value;
            }
        }

        /// <summary>Returns reference to the Asynchronous execution control object to the caller</summary>
        public TAsynchronousExecutionProgress AsyncExecProgress
        {
            get
            {
                return FAsyncExecProgress;
            }

            set
            {
                FAsyncExecProgress = value;
            }
        }

        /// <summary>Returns the number of records that was returned by the query</summary>
        public System.Int32 TotalRecords
        {
            get
            {
                return FTotalRecords;
            }
        }

        /// <summary>Returns the number of Pages that were created</summary>
        public System.Int16 TotalPages
        {
            get
            {
                return FTotalPages;
            }
        }

        /// <summary>
        /// Runs a SQL SELECT query and returns the first 'page' containing a certain
        /// amount of rows.
        ///
        /// @see For an example of how to use this class: look at the
        /// Ict.Petra.Server.MPartner.Partner.UIConnectors.TPartnerFindUIConnector
        /// class.
        ///
        /// </summary>
        /// <param name="ATypedTable">if you want the result to be a typed datatable, you need to specify the type here, otherwise just pass new DataTable()</param>
        /// <returns>void</returns>
        public TPagedDataSet(DataTable ATypedTable) : base()
        {
            if (ATypedTable == null)
            {
                FTmpDataTable = new DataTable();
            }
            else
            {
                FTmpDataTable = ATypedTable.Clone();
            }

            TLogging.LogAtLevel(7, "TPagedDataSet created.");
        }

        /// <summary>
        /// destructor for debugging purposes only
        /// </summary>
        ~TPagedDataSet()
        {
            TLogging.LogAtLevel(7, "TPagedDataSet.FINALIZE called!");
        }

        /// <summary>
        /// Executes the query. Call this method in a separate Thread to execute the
        /// query asynchronously!
        /// </summary>
        /// <remarks>An instance of TAsyncFindParameters with set up Properties must
        /// exist before this procedure can get called!
        /// </remarks>
        /// <returns>void</returns>
        public void ExecuteQuery()
        {
            try
            {
                FAsyncExecProgress.ProgressInformation = "Executing Query...";
                FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Executing;

                // Create SQL statement and execute it to return all records
                ExecuteFullQuery();
            }
            catch (Exception exp)
            {
                TLogging.Log(this.GetType().FullName + ".ExecuteQuery:  Exception occured: " + exp.ToString());

                // Inform the caller that something has gone wrong...
                FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Stopped;

                /*
                 *     WE MUST 'SWALLOW' ANY EXCEPTION HERE, OTHERWISE THE WHOLE
                 *     PETRASERVER WILL GO DOWN!!! (THIS BEHAVIOUR IS NEW WITH .NET 2.0.)
                 *
                 * --> ANY EXCEPTION THAT WOULD LEAVE THIS METHOD WOULD BE SEEN AS AN   <--
                 * --> UNHANDLED EXCEPTION IN A THREAD, AND THE .NET/MONO RUNTIME       <--
                 * --> WOULD BRING DOWN THE WHOLE PETRASERVER PROCESS AS A CONSEQUENCE! <--
                 *
                 */
            }
        }

        private void ExecuteFullQuery()
        {
//            TDBTransaction ReadTransaction;
//            Boolean NewTransaction = false;
            if (FFindParameters.FParametersGivenSeparately)
            {
                string SQLOrderBy = "";
                string SQLWhereCriteria = "";

                if (FFindParameters.FPagedTableWhereCriteria != "")
                {
                    SQLWhereCriteria = "WHERE " + FFindParameters.FPagedTableWhereCriteria;
                }

                if (FFindParameters.FPagedTableOrderBy != "")
                {
                    SQLOrderBy = " ORDER BY " + FFindParameters.FPagedTableOrderBy;
                }

                FSelectSQL = "SELECT " + FFindParameters.FPagedTableColumns + " FROM " + FFindParameters.FPagedTable +
                             ' ' +
                             SQLWhereCriteria + SQLOrderBy;
            }
            else
            {
                FSelectSQL = FFindParameters.FSqlQuery;
            }

            FTmpDataTable.TableName = FFindParameters.FSearchName;
            try
            {
                // Fill temporary table with query results (all records)
                FDataAdapter = null;
                DBAccess.GDBAccessObj.PrepareNextCommand();
                DBAccess.GDBAccessObj.SetTimeoutForNextCommand(60);

                DBAccess.GDBAccessObj.SelectDT(FTmpDataTable, FSelectSQL,
                    null,
                    FFindParameters.FParametersArray, -1, -1);
            }
            finally
            {
            }

            try
            {
                FTotalRecords = FTmpDataTable.Rows.Count;
            }
            catch (System.InvalidOperationException)
            {
                // Note: these exceptions are thrown when a Query was cancelled. This works
                // only with MS.NET though, but not with mono (at least up to 1.1.13.2)
                FAsyncExecProgress.ProgressInformation = "Query cancelled!";
                FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Stopped;
                return;
            }
            catch (Exception)
            {
                FAsyncExecProgress.ProgressInformation = "Query cancelled!";
                FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Stopped;
                return;
            }

            // Check if query execution cancellation was requested  necessary only for mono (at least up to 1.1.13.2)
            if (FAsyncExecProgress.FCancelExecution)
            {
                TLogging.LogAtLevel(7, "Query got cancelled!");
                FAsyncExecProgress.ProgressInformation = "Query cancelled!";
                FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Stopped;
                return;
            }

            TLogging.LogAtLevel(7, "TPagedDataSet  FDataAdapter.Fill finished. FTotalRecords: " + FTotalRecords.ToString());
            FPageDataTable = FTmpDataTable.Clone();
            FPageDataTable.TableName = FFindParameters.FSearchName;
            FAsyncExecProgress.ProgressInformation = "Query executed.";
            FAsyncExecProgress.ProgressPercentage = 100;
            FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Finished;
        }

        /// <summary>
        /// Returns a DataTable for the requested 'Page'.
        /// </summary>
        /// <remarks><see cref="ExecuteQuery" /> (or <see cref="ExecuteFullQuery" />)
        /// needs to be called before to execute the SQL SELECT Statement to be able to return data.</remarks>
        /// <param name="APage">The 'Page' that should be returned</param>
        /// <param name="APageSize">Number of DataRows that a 'Page' should contain.</param>
        /// <returns>DataTable containing 'PageSize' (or less if the 'TotalRecords' are
        /// less than that value) DataRows.
        /// </returns>
        public DataTable GetData(Int16 APage, Int16 APageSize)
        {
            TLogging.LogAtLevel(7, "TPagedDataSet.GetData(" + APage.ToString() + ") called.");

            // wait until the query has been run in the other thread
            while (FTotalRecords == -1)
            {
                System.Threading.Thread.Sleep(500);
            }

            if (APage != FLastRetrievedPage)
            {
                FLastRetrievedPage = APage;

                if (APage == 0)
                {
                    FTotalPages = Convert.ToInt16(Math.Ceiling(((double)FTotalRecords) / ((double)APageSize)));

                    TLogging.LogAtLevel(7, "FTotalPages: " + FTotalPages.ToString());

                    if (FTotalRecords > 0)
                    {
                        // Build FPageDataTable
                        CopyRowsInPage(0, APageSize);
                    }
                }
                else
                {
                    // page > 0

                    if (FTotalRecords > 0)
                    {
                        if (APage <= FTotalPages)
                        {
                            FPageDataTable.Rows.Clear();

                            // Build FPageDataTable
                            CopyRowsInPage(APage, APageSize);
                        }
                        // FRecordsAffected > 0
                        else
                        {
                            throw new EPagedTableNoSuchPageException(
                                "Tried to retrieve page " + APage.ToString() + ", but there are only " + FTotalPages.ToString() +
                                " pages in the paged table!");
                        }
                    }
                    else
                    {
                        throw new EPagedTableNoRecordsException();
                    }
                }
            }
            // (APage > FLastRetrievedPage) or (APage = 0)
            else
            {
                if (APage >= 0)
                {
                    // return empty DataTable
                    FPageDataTable.Rows.Clear();
                }
                else
                {
                    throw new EPagedTableNoSuchPageException("Invalid page " + APage.ToString() + " was requested");
                }
            }

            FAsyncExecProgress.ProgressInformation = "Query executed.";
            FAsyncExecProgress.ProgressPercentage = 100;
            FAsyncExecProgress.ProgressState = TAsyncExecProgressState.Aeps_Finished;
            return FPageDataTable;
        }

        /// <summary>
        /// Returns all data that was found by executing the SQL SELECT Statement.
        /// </summary>
        /// <remarks><see cref="ExecuteQuery" /> (or <see cref="ExecuteFullQuery" />)
        /// needs to be called before to execute the SQL SELECT Statement to be able to return data.</remarks>
        /// <returns>DataTable holding all data that was found by executing the SQL
        /// SELECT Statement. This is an copy of the <see cref="TPagedDataSet" />'s
        /// internal DataTable, so the caller is free to do with it what it wants
        /// without affecting the internal DataTable of the <see cref="TPagedDataSet" />.</returns>
        public DataTable GetAllData()
        {
            return FTmpDataTable.Copy();
        }

        private void CopyRowsInPage(Int16 APage, Int16 APageSize)
        {
            Int32 RowInPage;
            Int32 MaxRowInPage;

            MaxRowInPage = (APageSize * APage) + APageSize;

            if (MaxRowInPage > FTotalRecords)
            {
                MaxRowInPage = FTotalRecords;
            }

            for (RowInPage = (APageSize * APage); RowInPage <= MaxRowInPage - 1; RowInPage += 1)
            {
                FPageDataTable.ImportRow(FTmpDataTable.Rows[RowInPage]);
            }
        }

/*
 *      /// <summary>
 *      /// Creates a mapping between the names of the fields in the DB and how they
 *      /// should be named in the resulting DataTable.
 *      ///
 *      /// </summary>
 *      /// <returns>void</returns>
 *      private void PerformColumnNameMapping()
 *      {
 *          DataTableMapping AliasNames;
 *          IDictionaryEnumerator ColumNameMappingEnumerator;
 *
 *          AliasNames = FDataAdapter.TableMappings.Add(FTmpDataTable.TableName, FTmpDataTable.TableName);
 *          ColumNameMappingEnumerator = FFindParameters.FColumNameMapping.GetEnumerator();
 *
 *          while (ColumNameMappingEnumerator.MoveNext())
 *          {
 *              AliasNames.ColumnMappings.Add(ColumNameMappingEnumerator.Key.ToString(), ColumNameMappingEnumerator.Value.ToString());
 *          }
 *      }
 */
        /// <summary>
        /// Cancels an asynchronously executing query. This might take some time;
        /// therefore always execute this procedure in a separate Thread!
        ///
        /// </summary>
        /// <returns>void</returns>
        public void StopQuery()
        {
            try
            {
                // Cancel the executing query.
                TLogging.LogAtLevel(7, "TPagedDataSet.StopQuery called...");
                FDataAdapter.SelectCommand.Cancel();
                TLogging.LogAtLevel(7, "TPagedDataSet.StopQuery finished.");
            }
            catch (Exception exp)
            {
                TLogging.Log(this.GetType().FullName + ".StopQuery:  Exception occured: " + exp.ToString());

                /*
                 *     WE MUST 'SWALLOW' ANY EXCEPTION HERE, OTHERWISE THE WHOLE
                 *     PETRASERVER WILL GO DOWN!!! (THIS BEHAVIOUR IS NEW WITH .NET 2.0.)
                 *
                 * --> ANY EXCEPTION THAT WOULD LEAVE THIS METHOD WOULD BE SEEN AS AN   <--
                 * --> UNHANDLED EXCEPTION IN A THREAD, AND THE .NET/MONO RUNTIME       <--
                 * --> WOULD BRING DOWN THE WHOLE PETRASERVER PROCESS AS A CONSEQUENCE! <--
                 *
                 */
            }
        }

        #region TPagedDataSet.TAsyncFindParameters

        /**
         * Nested Class for passing in of parameters.
         *
         * This is used because the main execution occurs in a Thread, and it's not
         * straightforward to pass in several typed parameters to a Thread Delegate.
         *
         */
        public class TAsyncFindParameters : object
        {
            /// Columns part of the SQL SELECT statement
            internal String FPagedTableColumns;

            /// Table part of the SQL SELECT statement
            internal String FPagedTable;

            /// Set this to the name of the search
            public String FSearchName;

            /// WHERE part of the SQL SELECT statement
            internal String FPagedTableWhereCriteria;

            /// ORDER BY part of the SQL SELECT statement
            internal String FPagedTableOrderBy;

            /// HashTable containing a mapping of source column names to result column names
            internal Hashtable FColumNameMapping;

            /// Array containing the OdbcParameters for the parameterised query
            internal OdbcParameter[] FParametersArray;

            internal bool FParametersGivenSeparately;

            internal String FSqlQuery;

            /// <summary>
            /// Initialises the fields.
            /// </summary>
            /// <param name="AColumns">Columns part of the SQL SELECT statement</param>
            /// <param name="ATable">Table part of the SQL SELECT statement</param>
            /// <param name="AWhereCriteria">WHERE part of the SQL SELECT statement</param>
            /// <param name="AOrderBy">ORDER BY part of the SQL SELECT statement</param>
            /// <param name="AColumNameMapping">HashTable containing a mapping of source column names to result column names</param>
            /// <param name="AParametersArray">Array containing the OdbcParameters for the parameterised query</param>
            public TAsyncFindParameters(
                String AColumns,
                String ATable,
                String AWhereCriteria,
                String AOrderBy,
                Hashtable AColumNameMapping,
                OdbcParameter[] AParametersArray) : base()
            {
                FPagedTableColumns = AColumns;
                FPagedTable = ATable;
                FPagedTableWhereCriteria = AWhereCriteria;
                FPagedTableOrderBy = AOrderBy;
                FColumNameMapping = AColumNameMapping;
                FParametersArray = AParametersArray;
                FParametersGivenSeparately = true;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="ASqlQuery">Completely formed SqlQuery</param>
            public TAsyncFindParameters(String ASqlQuery)
            {
                FSqlQuery = ASqlQuery;
                FPagedTable = "Find";
                FParametersGivenSeparately = false;
            }
        }
        #endregion
        #endregion
    }

    #region TAsynchronousExecutionProgress

    /// <summary>
    /// Universal class for providing progress information and results for
    /// asynchronous executions of any kind (eg. updating a ProgressBar on the Client
    /// side).
    ///
    /// This class will be instantiated by some object ('Instantiator') to be able to
    /// tell a 'Listener' object how much progress has been done on a certain task.
    /// For this the 'Instantiator' will create an instance of this class and have
    /// a Property that accesses this instance. The 'Listener' object can use the
    /// IAsynchronousExecutionProgress interface to read Properties and call the
    /// Cancel method. The 'Listener' object can be instantiated on the Client side
    /// as well as on the Server side.
    ///
    /// </summary>
    public class TAsynchronousExecutionProgress : TConfigurableMBRObject, IAsynchronousExecutionProgress
    {
        /// <summary>Property value.</summary>
        private String FProgressInformation;

        /// <summary>Property value.</summary>
        Int16 FProgressPercentage;

        /// <summary>Property value.</summary>
        System.Object FResult;

        /// <summary>Property value.</summary>
        TAsyncExecProgressState FProgressState;

        /// <summary>Set to true when the Cancel method is called (monitor this in the 'Instantiator' to know if to stop).</summary>
        internal Boolean FCancelExecution;

        /// <summary>Text that explains what is currently going on.</summary>
        public String ProgressInformation
        {
            get
            {
                return FProgressInformation;
            }

            set
            {
                FProgressInformation = value;
            }
        }

        /// <summary>A value between 0 and 100 that tells to which degree the progress is finished.</summary>
        public Int16 ProgressPercentage
        {
            get
            {
                return FProgressPercentage;
            }

            set
            {
                FProgressPercentage = value;
            }
        }

        /// <summary>Indicates the ProgressState. (Default: Aeps_ReadyToStart)</summary>
        public TAsyncExecProgressState ProgressState
        {
            get
            {
                return FProgressState;
            }

            set
            {
                FProgressState = value;
            }
        }
        /// <summary>Can be used by the 'Instantiator' to pass a result to the 'Listener'</summary>
        public object Result
        {
            get
            {
                return FResult;
            }

            set
            {
                FResult = value;
            }
        }

        /// <summary>Event that fires when the Cancel method is called (only the 'Instantiator' should subscribe to that).</summary>
        public event System.EventHandler StopAsyncronousExecution;

        /// <summary>
        /// constructor
        /// </summary>
        public TAsynchronousExecutionProgress() : base()
        {
            FProgressState = TAsyncExecProgressState.Aeps_ReadyToStart;
        }

        /// <summary>
        /// Returns ProgressState, ProgressPercentage and ProgressInformation properties
        /// in one call. This saves considerable bandwidth over calling these properties
        /// seperately!
        ///
        /// </summary>
        /// <param name="ProgressState">See ProgressState property</param>
        /// <param name="ProgressPercentage">See ProgressPercentage property</param>
        /// <param name="ProgressInformation">See ProgressInformation property
        /// </param>
        /// <returns>void</returns>
        public void ProgressCombinedInfo(out TAsyncExecProgressState ProgressState, out Int16 ProgressPercentage, out String ProgressInformation)
        {
            ProgressState = FProgressState;
            ProgressPercentage = FProgressPercentage;
            ProgressInformation = FProgressInformation;
        }

        /// <summary>
        /// Call this method from the 'Listener' object to signal the 'Instantiator' that the execution should be stopped
        /// </summary>
        /// <returns>void</returns>
        public void Cancel()
        {
            TLogging.LogAtLevel(6, "TAsynchronousExecutionProgress.Cancel called!");
            FCancelExecution = true;
            FProgressState = TAsyncExecProgressState.Aeps_Stopping;

            // Fire event
            if (StopAsyncronousExecution != null)
            {
                StopAsyncronousExecution(this, new System.EventArgs());
            }
        }
    }
    #endregion

    #region TDynamicSearchHelper

    /// <summary>
    /// Universal class to help assemble the ODBC parameters for a dynamic search
    /// </summary>
    public class TDynamicSearchHelper
    {
        private
        String FDBFieldName;
        DataRow FDataRow;
        String FCriteriaField;
        String FMatchField;
        String FSearchDelimiter = "";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ADBFieldName"></param>
        /// <param name="ADataRow"></param>
        /// <param name="ACriteriaField"></param>
        /// <param name="AMatchField"></param>
        public TDynamicSearchHelper(String ADBFieldName, DataRow ADataRow, String ACriteriaField, String AMatchField)
        {
            FDBFieldName = ADBFieldName;
            FDataRow = ADataRow;
            FCriteriaField = ACriteriaField;
            FMatchField = AMatchField;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <param name="ADBFieldName"></param>
        /// <param name="ADataRow"></param>
        /// <param name="ACriteriaField"></param>
        /// <param name="AMatchField"></param>
        /// <param name="AOdbcType"></param>
        /// <param name="AOdbcSize"></param>
        /// <param name="AWhereClause"></param>
        /// <param name="AIntParamArray"></param>
        public TDynamicSearchHelper(String ADBFieldName,
            DataRow ADataRow,
            String ACriteriaField,
            String AMatchField,
            OdbcType AOdbcType,
            Int32 AOdbcSize,
            ref String AWhereClause,
            ref ArrayList AIntParamArray)
        {
            object ParameterValue;
            OdbcParameter miParam;

            FDBFieldName = ADBFieldName;
            FDataRow = ADataRow;
            FCriteriaField = ACriteriaField;
            FMatchField = AMatchField;

            ParameterValue = GetParameterValue();

            if (ParameterValue.ToString() != String.Empty)
            {
                miParam = new OdbcParameter("", AOdbcType, AOdbcSize);
                miParam.Value = ParameterValue;

                AIntParamArray.Add(miParam);
                AWhereClause = AWhereClause + GetWhereString();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="AColumnNr"></param>
        /// <param name="ACriteriaDataRow"></param>
        /// <param name="ACriteriaField"></param>
        /// <param name="ACriteriaMatchField"></param>
        /// <param name="AWhereClause"></param>
        /// <param name="AIntParamArray"></param>
        public TDynamicSearchHelper(
            short ATableId,
            short AColumnNr,
            DataRow ACriteriaDataRow,
            String ACriteriaField,
            String ACriteriaMatchField,
            ref String AWhereClause,
            ref ArrayList AIntParamArray)
        {
            FDataRow = ACriteriaDataRow;
            FCriteriaField = ACriteriaField;
            FMatchField = ACriteriaMatchField;

            FDBFieldName = "PUB_" +
                           TTypedDataTable.GetTableNameSQL(ATableId) +
                           "." +
                           TTypedDataTable.GetColumnNameSQL(ATableId, AColumnNr);

            object ParameterValue = GetParameterValue();

            if (ParameterValue.ToString() != String.Empty)
            {
                OdbcParameter miParam = TTypedDataTable.CreateOdbcParameter(ATableId, AColumnNr);
                miParam.Value = ParameterValue;

                AIntParamArray.Add(miParam);
                AWhereClause = AWhereClause + GetWhereString();
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="ATableId"></param>
        /// <param name="AColumnNr"></param>
        /// <param name="ACriteriaDataRow"></param>
        /// <param name="ACriteriaField"></param>
        /// <param name="ACriteriaMatchField"></param>
        /// <param name="AWhereClause"></param>
        /// <param name="AIntParamArray"></param>
        /// <param name="ASearchDelimiter"></param>
        public TDynamicSearchHelper(
            short ATableId,
            short AColumnNr,
            DataRow ACriteriaDataRow,
            String ACriteriaField,
            String ACriteriaMatchField,
            ref String AWhereClause,
            ref ArrayList AIntParamArray,
            String ASearchDelimiter)
            : this(ATableId, AColumnNr, ACriteriaDataRow, ACriteriaField,
                   ACriteriaMatchField, ref AWhereClause, ref AIntParamArray)
        {
            FSearchDelimiter = ASearchDelimiter;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public object GetParameterValue()
        {
            String criteriavalue;
            object outcome;
            String matchvalue;
            double DoubleValue = -1;

            outcome = "";
            try
            {
                // not all fields have a MatchField defined yet
                matchvalue = this.FDataRow[FMatchField].ToString();
            }
            catch (Exception)
            {
                matchvalue = "BEGINS";
            }

            if (FMatchField == "")
            {
                matchvalue = "EXACT";
            }

            criteriavalue = FDataRow[FCriteriaField].ToString().Replace("*", String.Empty);

            if (criteriavalue.Length > 0)
            {
                if (matchvalue == "BEGINS")
                {
                    outcome = (object)(criteriavalue + '%');
                }

                if (matchvalue == "ENDS")
                {
                    outcome = (object)('%' + criteriavalue);
                }

                if (matchvalue == "CONTAINS")
                {
                    outcome = (object)('%' + criteriavalue + '%');
                }

                if (matchvalue == "EXACT")
                {
                    // Check if criteria value is a valid number
                    criteriavalue = FDataRow[FCriteriaField].ToString();
                    Double.TryParse(criteriavalue, out DoubleValue);

                    if (DoubleValue == 0)
                    {
                        // Criteria value isn't a valid number, so convert it to lowercase to make
                        // for case-insensitive searches
                        outcome = (object)(criteriavalue.ToLower());
                    }
                    else
                    {
                        outcome = (object)DoubleValue;
                    }
                }
            }

            return outcome;
        }

        /// <summary>
        /// todoComment
        /// </summary>
        /// <returns></returns>
        public String GetWhereString()
        {
            String matchvalue;
            String criteriavalue;
            String outcome = "";
            double DoubleValue = -1;

            if (this.FSearchDelimiter == "")
            {
                FSearchDelimiter = "AND";
            }

            try
            {
                // not all fields have a MatchField defined yet
                matchvalue = this.FDataRow[FMatchField].ToString().ToUpper();
            }
            catch (Exception)
            {
                matchvalue = "BEGINS";
            }

            if (FMatchField == "")
            {
                matchvalue = "EXACT";
            }

            if (matchvalue == "BEGINS")
            {
                outcome = ' ' + FSearchDelimiter + ' ' + FDBFieldName + " LIKE ?";
            }

            if (matchvalue == "ENDS")
            {
                outcome = ' ' + FSearchDelimiter + ' ' + FDBFieldName + " LIKE ?";
            }

            if (matchvalue == "CONTAINS")
            {
                outcome = ' ' + FSearchDelimiter + ' ' + FDBFieldName + " LIKE ?";
            }

            if (matchvalue == "EXACT")
            {
                // Check if criteria value is a valid number
                criteriavalue = FDataRow[FCriteriaField].ToString();
                Double.TryParse(criteriavalue, out DoubleValue);

                if (DoubleValue == 0)
                {
                    // Criteria value isn't a valid number, so convert it to lowercase to make
                    // for case-insensitive searches.
                    outcome = ' ' + FSearchDelimiter + " LOWER(" + FDBFieldName + ") = ?";
                }
                else
                {
                    // Criteria value is a number, therefore don't convert it to lowercase.
                    outcome = ' ' + FSearchDelimiter + " " + FDBFieldName + " = ?";
                }
            }

//            Console.WriteLine("WhereString: " + outcome);

            return outcome;
        }

        #endregion
    }
}