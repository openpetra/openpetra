// auto generated with nant generateORM from ORM\CacheableServer.cs
// Do not modify this file manually!
//
{#GPLFILEHEADER}
using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.Odbc;
using Ict.Common.Data;
using Ict.Common;
using Ict.Common.DB;
using Ict.Common.Data.Exceptions;
using Ict.Common.Verification;
using Ict.Common.Exceptions;
using Ict.Common.Remoting.Server;
using Ict.Petra.Shared;
using Ict.Petra.Server.App.Core;
using Ict.Petra.Server.App.Core.Security;
using Ict.Petra.Server.MCommon;
{#USINGNAMESPACES}

namespace Ict.Petra.Server.{#SUBNAMESPACE}.Cacheable.WebConnectors
{
    /// <summary>
    /// WebConnector for Cacheable
    /// </summary>
    public class T{#SUBMODULE}CacheableWebConnector
    {
        /// <summary>holds reference to the CachePopulator object (only once instantiated)</summary>
        private static Ict.Petra.Server.{#SUBNAMESPACE}.Cacheable.{#CACHEABLECLASS} FCachePopulator = null;
        
        /// <summary>
        /// init static object
        /// </summary>
        private static void Init()
        {
            if (FCachePopulator == null)
            {
                FCachePopulator = new Ict.Petra.Server.{#SUBNAMESPACE}.Cacheable.{#CACHEABLECLASS}();
            }
        }

        /// <summary>
        /// Returns the desired cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTable">Used to select the desired DataTable</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable The desired DataTable
        /// </returns>
        private static DataTable GetCacheableTableInternal(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            Init();

            DataTable ReturnValue = FCachePopulator.GetCacheableTable(
                ACacheableTable, AHashCode, ARefreshFromDB, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }

{#IFDEF WITHLEDGER}
        /// <summary>
        /// Returns the desired cacheable DataTable.
        ///
        /// </summary>
        /// <param name="ACacheableTable">Used to select the desired DataTable</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="ALedgerNumber"></param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>)
        /// DataTable The desired DataTable
        /// </returns>
        private static DataTable GetCacheableTableInternal(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            System.Int32 ALedgerNumber,
            out System.Type AType)
        {
            Init();

            DataTable ReturnValue = FCachePopulator.GetCacheableTable(
                ACacheableTable, AHashCode, ARefreshFromDB, ALedgerNumber, out AType);

            if (ReturnValue != null)
            {
                if (Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable) != ReturnValue.TableName)
                {
                    throw new ECachedDataTableTableNameMismatchException(
                        "Warning: cached table name '" + ReturnValue.TableName + "' does not match enum '" +
                        Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable) + "'");
                }
            }

            return ReturnValue;
        }
{#ENDIF WITHLEDGER}
        
        /// GetCacheableTable
        [RequireModulePermission("NONE")]
        public static System.Data.DataTable GetCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       out System.Type AType)
        {
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, out AType);
        }

        /// RefreshCacheableTable
        [RequireModulePermission("NONE")]
        public static void RefreshCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable)
        {
            System.Type TmpType;
            GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
        }

        /// RefreshCacheableTable
        [RequireModulePermission("NONE")]
        public static void RefreshCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
                                          out System.Data.DataTable ADataTable)
        {
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, out TmpType);
        }

        /// SaveChangedStandardCacheableTable
        [RequireModulePermission("NONE")]
        public static TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            Init();
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, out AVerificationResult);
        }
        
{#IFDEF WITHLEDGER}
        /// GetCacheableTable
        [RequireModulePermission("NONE")]
        public static System.Data.DataTable GetCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
                                                       System.String AHashCode,
                                                       System.Int32 ALedgerNumber,
                                                       out System.Type AType)
        {
            return GetCacheableTableInternal(ACacheableTable, AHashCode, false, ALedgerNumber, out AType);
        }

        /// RefreshCacheableTable
        [RequireModulePermission("NONE")]
        public static void RefreshCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
                                          System.Int32 ALedgerNumber,
                                          out System.Data.DataTable ADataTable)
        {
            System.Type TmpType;
            ADataTable = GetCacheableTableInternal(ACacheableTable, "", true, ALedgerNumber, out TmpType);
        }

        /// SaveChangedStandardCacheableTable
        [RequireModulePermission("NONE")]
        public static TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
                                                                      ref TTypedDataTable ASubmitTable,
                                                                      System.Int32 ALedgerNumber,
                                                                      out TVerificationResultCollection AVerificationResult)
        {
            Init();
            return FCachePopulator.SaveChangedStandardCacheableTable(ACacheableTable, ref ASubmitTable, ALedgerNumber, out AVerificationResult);
        }
{#ENDIF WITHLEDGER}
    }
}

namespace {#NAMESPACE}
{
    /// <summary>
    /// Returns cacheable DataTables for DB tables in the {#SUBNAMESPACE} sub-namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows
    /// are opened.
    /// </summary>
    public partial class {#CACHEABLECLASS} : TCacheableTablesLoader
    {
        /// time when this object was instantiated
        private DateTime FStartTime;

        /// <summary>
        /// constructor
        /// </summary>
        public {#CACHEABLECLASS}() : base()
        {
            TLogging.LogAtLevel (9,"{#CACHEABLECLASS} created: Instance hash is " + this.GetHashCode().ToString());
            FStartTime = DateTime.Now;
            FCacheableTablesManager = TCacheableTablesManager.GCacheableTablesManager;
        }

        /// destructor
        ~{#CACHEABLECLASS}()
        {
            TLogging.LogAtLevel (9,"{#CACHEABLECLASS} Getting collected after " + 
                (new TimeSpan(DateTime.Now.Ticks - FStartTime.Ticks)).ToString() + " seconds.");
        }

        /// <summary>
        /// Returns a certain cachable DataTable that contains all columns and all
        /// rows of a specified table.
        ///
        /// @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its contents got retrieved from the DB. It returns the cached
        /// DataTable from it on subsequent calls, therefore making more no further DB
        /// queries!
        ///
        /// @comment All DataTables are retrieved as Typed DataTables, but are passed
        /// out as a normal DataTable. However, this DataTable can be cast by the
        /// caller to the appropriate TypedDataTable to have access to the features of
        /// a Typed DataTable!
        /// </summary>
        ///
        /// <param name="ACacheableTable">Tells what cacheable DataTable should be returned.</param>
        /// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can
        /// be specified to always get a DataTable back (see @return)</param>
        /// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
        /// DB and through that refresh the Table in the Cache with what is now in the
        /// DB (this would be done when it is known that the DB Table has changed).
        /// The CacheableTablesManager will notify other Clients that they need to
        /// retrieve this Cacheable DataTable anew from the PetraServer the next time
        /// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
        /// <param name="AType">The Type of the DataTable (useful in case it's a
        /// Typed DataTable)</param>
        /// <returns>
        /// DataTable If the Hash that got passed in AHashCode doesn't fit the
        /// Hash that the CacheableTablesManager has for this cacheable DataTable, the
        /// specified DataTable is returned, otherwise nil.
        /// </returns>
        public DataTable GetCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            String TableName = Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable);

            TLogging.LogAtLevel (9, "{#CACHEABLECLASS}.GetCacheableTable '{#SUBMODULE}' called.");

            if ((ARefreshFromDB) || ((!FCacheableTablesManager.IsTableCached(TableName))))
            {
                Boolean NewTransaction;
                TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);
                try
                {

                    switch(ACacheableTable)
                    {
                        {#LOADTABLESANDLISTS}
                    
                        default:
                            // Unknown Standard Cacheable DataTable
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                                TableName + "' is not available as a Standard Cacheable Table");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                        TLogging.LogAtLevel (9, "{#CACHEABLECLASS}.GetCacheableTable: commited own transaction.");
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
        }
        {#LEDGERGETCACHEABLE}

{#IFDEF SAVETABLE}
        /// <summary>
        /// Saves a specific Cachable DataTable. The whole DataTable needs to be submitted,
        /// not just changes to it!
        /// </summary>
        /// <remarks>
        /// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
        /// once its saved successfully to the DB, which in turn tells all other Clients
        /// that they need to reload this Cacheable DataTable the next time something in the
        /// Client accesses it.
        /// </remarks>
        /// <param name="ACacheableTable">Name of the Cacheable DataTable with changes.</param>
        /// <param name="ASubmitTable">Cacheable DataTable with changes. The whole DataTable needs
        /// to be submitted, not just changes to it!</param>
        /// <param name="AVerificationResult">Will be filled with any
        /// VerificationResults if errors occur.</param>
        /// <returns>Status of the operation.</returns>
        public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
            ref TTypedDataTable ASubmitTable,
            out TVerificationResultCollection AVerificationResult)
        {
            TDBTransaction SubmitChangesTransaction;
            TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
            TVerificationResultCollection SingleVerificationResultCollection;
            string CacheableDTName = Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable);

            // Console.WriteLine("Entering {#SUBMODULE}.SaveChangedStandardCacheableTable...");
            AVerificationResult = null;

            // TODO: check write permissions

            if (ASubmitTable != null)
            {
                AVerificationResult = new TVerificationResultCollection();
                SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

                try
                {
                    switch (ACacheableTable)
                    {
                        {#SAVETABLE}

                        default:

                            throw new Exception(
                            "{#CACHEABLECLASS}.SaveChangedStandardCacheableTable: unsupported Cacheable DataTable '" + CacheableDTName + "'");
                    }

                    if (SubmissionResult == TSubmitChangesResult.scrOK)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
                    }
                    else
                    {
                        DBAccess.GDBAccessObj.RollbackTransaction();
                    }
                }
                catch (EDBConcurrencyException)
                {
                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw;
                }
                catch (Exception e)
                {
                    TLogging.Log(
                        "{#CACHEABLECLASS}.SaveChangedStandardCacheableTable: after SubmitChanges call for Cacheable DataTable '" +
                        CacheableDTName +
                        "':  Exception " + e.ToString());

                    DBAccess.GDBAccessObj.RollbackTransaction();

                    throw;
                }
            }


            // If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
            // Cache and inform all other Clients that they need to reload this Cacheable DataTable
            // the next time something in the Client accesses it.
            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                Type TmpType;
                GetCacheableTable(ACacheableTable, String.Empty, true, out TmpType);
                AfterSaving(ACacheableTable);
            }

            if (AVerificationResult.Count > 0)
            {
                // Downgrade TScreenVerificationResults to TVerificationResults in order to allow 
                // Serialisation (needed for .NET Remoting).
                TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
            }

            return SubmissionResult;
        }

        partial void AfterSaving(TCacheable{#SUBMODULE}TablesEnum ACacheableTable);            

#region Data Validation

    {#DATAVALIDATION}    

#endregion Data Validation
{#ENDIF SAVETABLE}
        {#LEDGERSAVECACHEABLE}
        {#GETCALCULATEDLISTFROMDB}
    }
}

{##LEDGERGETCACHEABLE}

/// <summary>
/// Returns a certain cachable DataTable that contains all columns and rows
/// of a specified table that match a specified Ledger Number.
///
/// @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
/// once its contents got retrieved from the DB. It returns the cached
/// DataTable from it on subsequent calls, therefore making more no further DB
/// queries!
///
/// @comment All DataTables are retrieved as Typed DataTables, but are passed
/// out as a normal DataTable. However, this DataTable can be cast by the
/// caller to the appropriate TypedDataTable to have access to the features of
/// a Typed DataTable!
///
/// </summary>
/// <param name="ACacheableTable">Tells what cachable DataTable should be returned.</param>
/// <param name="AHashCode">Hash of the cacheable DataTable that the caller has. '' can be
/// specified to always get a DataTable back (see @return)</param>
/// <param name="ARefreshFromDB">Set to true to reload the cached DataTable from the
/// DB and through that refresh the Table in the Cache with what is now in the
/// DB (this would be done when it is known that the DB Table has changed).
/// The CacheableTablesManager will notify other Clients that they need to
/// retrieve this Cacheable DataTable anew from the PetraServer the next time
/// the Client accesses the Cacheable DataTable. Otherwise set to false.</param>
/// <param name="AType">The Type of the DataTable (useful in case it's a
/// Typed DataTable)</param>
/// <param name="ALedgerNumber">The LedgerNumber that the rows that should be stored in
/// the Cache need to match.</param>
/// <returns>)
/// DataTable If the Hash that got passed in AHashCode doesn't fit the
/// Hash that the CacheableTablesManager has for this cacheable DataTable, the
/// specified DataTable is returned, otherwise nil.
/// </returns>
public DataTable GetCacheableTable(TCacheable{#SUBMODULE}TablesEnum ACacheableTable,
    String AHashCode,
    Boolean ARefreshFromDB,
    System.Int32 ALedgerNumber,
    out System.Type AType)
{
    string TableName = Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum), ACacheableTable);

    if (TLogging.DL >= 7)
    {
        TLogging.Log ("{#CACHEABLECLASS}.GetCacheableTable '{#SUBMODULE}' called with ALedgerNumber=" + ALedgerNumber.ToString() + '.');

        if (FCacheableTablesManager.IsTableCached(TableName))
        {
            Console.WriteLine("Cached DataTable has currently " +
                FCacheableTablesManager.GetCachedDataTable(TableName, out AType).Rows.Count.ToString() + " rows in total.");
            Console.WriteLine("Cached DataTable has currently " +
                Convert.ToString(FCacheableTablesManager.GetCachedDataTable(TableName,
                        out AType).Select(
                        ALedgerTable.GetLedgerNumberDBName() + " = " +
                        ALedgerNumber.ToString()).Length) + " rows with ALedgerNumber=" + ALedgerNumber.ToString() + '.');
        }
    }

    if ((ARefreshFromDB) || ((!FCacheableTablesManager.IsTableCached(TableName)))
        || ((FCacheableTablesManager.IsTableCached(TableName))
            && (FCacheableTablesManager.GetCachedDataTable(TableName,
                    out AType).Select(ALedgerTable.GetLedgerNumberDBName() + " = " +
                    ALedgerNumber.ToString()).Length == 0)))
    {
        Boolean NewTransaction;
        TDBTransaction ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
            MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
            TEnforceIsolationLevel.eilMinimum,
            out NewTransaction);

        try
        {
            switch (ACacheableTable)
            {
                {#LOADTABLESANDLISTS}

                default:

                    // Unknown Standard Cacheable DataTable
                    throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                    Enum.GetName(typeof(TCacheable{#SUBMODULE}TablesEnum),
                        ACacheableTable) + "' is not available as a Standard Cacheable Table (with ALedgerNumber as an Argument)");
            }
        }
        finally
        {
            if (NewTransaction)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
                TLogging.LogAtLevel (7, "T{#SUBMODULE}CacheableWebConnector.GetCacheableTable: commited own transaction.");
            }
        }
    }

    DataView TmpView = new DataView(FCacheableTablesManager.GetCachedDataTable(TableName,
            out AType), ALedgerTable.GetLedgerNumberDBName() + " = " + ALedgerNumber.ToString(), "", DataViewRowState.CurrentRows);

    // Return the DataTable from the Cache only if the Hash is not the same
    return ResultingCachedDataTable(TableName, AHashCode, TmpView, out AType);
}

{##LEDGERSAVECACHEABLE}

/// <summary>
/// Saves a specific Cachable DataTable. The whole DataTable needs to be submitted,
/// not just changes to it!
/// </summary>
/// <remarks>
/// Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
/// once its saved successfully to the DB, which in turn tells all other Clients
/// that they need to reload this Cacheable DataTable the next time something in the
/// Client accesses it.
/// </remarks>
/// <param name="ACacheableTable">Name of the Cacheable DataTable with changes.</param>
/// <param name="ASubmitTable">Cacheable DataTable with changes. The whole DataTable needs
/// to be submitted, not just changes to it!</param>
/// <param name="ALedgerNumber">The LedgerNumber that the rows that should be stored in
/// the Cache need to match.</param>
/// <param name="AVerificationResult">Will be filled with any
/// VerificationResults if errors occur.</param>
/// <returns>Status of the operation.</returns>
public TSubmitChangesResult SaveChangedStandardCacheableTable(TCacheableFinanceTablesEnum ACacheableTable,
    ref TTypedDataTable ASubmitTable,
    int ALedgerNumber,
    out TVerificationResultCollection AVerificationResult)
{
    TDBTransaction SubmitChangesTransaction;
    TSubmitChangesResult SubmissionResult = TSubmitChangesResult.scrError;
    TVerificationResultCollection SingleVerificationResultCollection;
    string CacheableDTName = Enum.GetName(typeof(TCacheableFinanceTablesEnum), ACacheableTable);
    Type TmpType;

    // Console.WriteLine("Entering Finance.SaveChangedStandardCacheableTable...");
    AVerificationResult = null;

    // TODO: check write permissions

    if (ASubmitTable != null)
    {
        AVerificationResult = new TVerificationResultCollection();
        SubmitChangesTransaction = DBAccess.GDBAccessObj.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            switch (ACacheableTable)
            {
                {#SAVETABLE}

                default:

                    throw new Exception(
                    "TFinanceCacheable.SaveChangedStandardCacheableTable: unsupported Cacheable DataTable '" + CacheableDTName + "'");
            }

            if (SubmissionResult == TSubmitChangesResult.scrOK)
            {
                DBAccess.GDBAccessObj.CommitTransaction();
            }
            else
            {
                DBAccess.GDBAccessObj.RollbackTransaction();
            }
        }
        catch (EDBConcurrencyException)
        {
            DBAccess.GDBAccessObj.RollbackTransaction();

            throw;
        }
        catch (Exception e)
        {
            TLogging.Log(
                "TFinanceCacheable.SaveChangedStandardCacheableTable: after SubmitChanges call for Cacheable DataTable '" +
                CacheableDTName +
                "':  Exception " + e.ToString());

            DBAccess.GDBAccessObj.RollbackTransaction();

            throw;
        }
    }

    // If saving of the DataTable was successful, update the Cacheable DataTable in the Servers'
    // Cache and inform all other Clients that they need to reload this Cacheable DataTable
    // the next time something in the Client accesses it.
    if (SubmissionResult == TSubmitChangesResult.scrOK)
    {
        //FCacheableTablesManager.AddOrRefreshCachedTable(ATableName, ASubmitTable, DomainManager.GClientID);
        GetCacheableTable(ACacheableTable, String.Empty, true, ALedgerNumber, out TmpType);
        AfterSaving(ACacheableTable);
    }

    if (AVerificationResult.Count > 0)
    {
        // Downgrade TScreenVerificationResults to TVerificationResults in order to allow 
        // Serialisation (needed for .NET Remoting).
        TVerificationResultCollection.DowngradeScreenVerificationResults(AVerificationResult);
    }
    
    return SubmissionResult;
}

#region Data Validation

{#DATAVALIDATION}

#endregion Data Validation


{##LOADTABLE}
case TCacheable{#SUBMODULE}TablesEnum.{#ENUMNAME}:
{
    DataTable TmpTable = {#DATATABLENAME}Access.LoadAll(ReadTransaction);
    FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
    break;
}

{##LOADTABLEVIALEDGER}
case TCacheable{#SUBMODULE}TablesEnum.{#ENUMNAME}:
{
    DataTable TmpTable = {#DATATABLENAME}Access.LoadViaALedger(ALedgerNumber, ReadTransaction);
    FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
    break;
}

{##LOADCALCULATEDLIST}
case TCacheable{#SUBMODULE}TablesEnum.{#ENUMNAME}:
{
    DataTable TmpTable = Get{#CALCULATEDLISTNAME}Table(ReadTransaction, TableName);
    FCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
    break;
}

{##LOADCALCULATEDLISTFORLEDGER}
case TCacheable{#SUBMODULE}TablesEnum.{#ENUMNAME}:
{
    DataTable TmpTable = Get{#CALCULATEDLISTNAME}Table(ReadTransaction, ALedgerNumber, TableName);
    FCacheableTablesManager.AddOrMergeCachedTable(TableName, TmpTable, DomainManager.GClientID, (object)ALedgerNumber);
    break;
}

{##SAVETABLE}
case TCacheable{#SUBMODULE}TablesEnum.{#ENUMNAME}:
    if (ASubmitTable.Rows.Count > 0) 
    { 
        {#DATATABLENAME}Validation.Validate(ASubmitTable, ref AVerificationResult);
        Validate{#ENUMNAME}Manual(ref AVerificationResult, ASubmitTable);

        if (TVerificationHelper.IsNullOrOnlyNonCritical(AVerificationResult))
        {
            if ({#DATATABLENAME}Access.SubmitChanges(({#DATATABLENAME}Table)ASubmitTable, SubmitChangesTransaction,
                out SingleVerificationResultCollection))
            {
                SubmissionResult = TSubmitChangesResult.scrOK;
            }
            else
            {
                AVerificationResult.AddCollection(SingleVerificationResultCollection);
            }
        }
    }

    break;
    
{##DATAVALIDATION}
partial void Validate{#ENUMNAME}Manual(ref TVerificationResultCollection AVerificationResult, TTypedDataTable ASubmitTable);        

