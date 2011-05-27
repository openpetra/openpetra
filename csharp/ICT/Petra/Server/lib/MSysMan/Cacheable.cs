//
// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       berndr
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
using Ict.Common;
using Ict.Common.DB;
using Ict.Petra.Shared;
using Ict.Petra.Shared.MSysMan;
using Ict.Petra.Shared.RemotedExceptions;
using Ict.Petra.Server.App.ClientDomain;
using Ict.Petra.Server.MCommon;
using Ict.Petra.Server.MSysMan.Data.Access;


namespace Ict.Petra.Server.MSysMan.Application.Cacheable
{
    /// <summary>
    /// Returns DataTables for DB tables in the MSysMan namespace
    /// that can be cached on the Client side.
    ///
    /// Examples of such tables are tables that form entries of ComboBoxes or Lists
    /// and which would be retrieved numerous times from the Server as UI windows
    /// are opened.
    /// </summary>
    public class TSysManCacheable : TCacheableTablesLoader
    {
        /// time when this object was instantiated
        private DateTime FStartTime;

        /// <summary>
        /// constructor
        /// </summary>
        public TSysManCacheable() : base()
        {
#if DEBUGMODE
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + " created: Instance hash is " + this.GetHashCode().ToString());
            }
#endif
            FStartTime = DateTime.Now;
            FCacheableTablesManager = DomainManager.GCacheableTablesManager;
        }

#if DEBUGMODE
        /// <summary>
        /// destructor
        /// </summary>
        ~TSysManCacheable()
        {
            if (TLogging.DL >= 9)
            {
                Console.WriteLine(this.GetType().FullName + ": Getting collected after " + (new TimeSpan(
                                                                                                DateTime.Now.Ticks -
                                                                                                FStartTime.Ticks)).ToString() + " seconds.");
            }
        }
#endif



        /**
         * Returns a certain cachable DataTable that contains all columns and all
         * rows of a specified table.
         *
         * @comment Uses Ict.Petra.Shared.CacheableTablesManager to store the DataTable
         * once its contents got retrieved from the DB. It returns the cached
         * DataTable from it on subsequent calls, therefore making more no further DB
         * queries!
         *
         * @comment All DataTables are retrieved as Typed DataTables, but are passed
         * out as a normal DataTable. However, this DataTable can be cast by the
         * caller to the appropriate TypedDataTable to have access to the features of
         * a Typed DataTable!
         *
         * @param ACacheableTable Tells what cachable DataTable should be returned.
         * @param AHashCode Hash of the cacheable DataTable that the caller has. '' can
         * be specified to always get a DataTable back (see @return)
         * @param ARefreshFromDB Set to true to reload the cached DataTable from the
         * DB and through that refresh the Table in the Cache with what is now in the
         * DB (this would be done when it is known that the DB Table has changed).
         * The CacheableTablesManager will notify other Clients that they need to
         * retrieve this Cacheable DataTable anew from the PetraServer the next time
         * the Client accesses the Cacheable DataTable. Otherwise set to false.
         * @param AType The Type of the DataTable (useful in case it's a
         * Typed DataTable)
         * @return DataTable If the Hash that got passed in AHashCode doesn't fit the
         * Hash that the CacheableTablesManager has for this cacheable DataTable, the
         * specified DataTable is returned, otherwise nil.
         *
         */
        public DataTable GetStandardCacheableTable(TCacheableSysManTablesEnum ACacheableTable,
            String AHashCode,
            Boolean ARefreshFromDB,
            out System.Type AType)
        {
            TDBTransaction ReadTransaction;
            Boolean NewTransaction;
            String TableName;
            DataTable TmpTable;

            TableName = Enum.GetName(typeof(TCacheableSysManTablesEnum), ACacheableTable);
#if DEBUGMODE
            if (TLogging.DL >= 7)
            {
                Console.WriteLine(this.GetType().FullName + ".GetStandardCacheableTable called with ATableName='" + TableName + "'.");
            }
#endif

            if ((ARefreshFromDB) || ((!DomainManager.GCacheableTablesManager.IsTableCached(TableName))))
            {
                ReadTransaction = DBAccess.GDBAccessObj.GetNewOrExistingTransaction(
                    MCommonConstants.CACHEABLEDT_ISOLATIONLEVEL,
                    TEnforceIsolationLevel.eilMinimum,
                    out NewTransaction);

                try
                {
                    switch (ACacheableTable)
                    {
                        case TCacheableSysManTablesEnum.UserList:
                            TmpTable = SUserAccess.LoadAll(ReadTransaction);
                            DomainManager.GCacheableTablesManager.AddOrRefreshCachedTable(TableName, TmpTable, DomainManager.GClientID);
                            break;

                        default:
                            throw new ECachedDataTableNotImplementedException("Requested Cacheable DataTable '" +
                            Enum.GetName(typeof(TCacheableSysManTablesEnum), ACacheableTable) + "' is not available as a Standard Cacheable Table");
                    }
                }
                finally
                {
                    if (NewTransaction)
                    {
                        DBAccess.GDBAccessObj.CommitTransaction();
#if DEBUGMODE
                        if (TLogging.DL >= 7)
                        {
                            Console.WriteLine(this.GetType().FullName + ".GetStandardCacheableTable: commited own transaction.");
                        }
#endif
                    }
                }
            }

            // Return the DataTable from the Cache only if the Hash is not the same
            return ResultingCachedDataTable(TableName, AHashCode, out AType);
        }
    }
}