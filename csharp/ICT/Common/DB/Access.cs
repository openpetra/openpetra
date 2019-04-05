// DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
//
// @Authors:
//       christiank, timop
//
// Copyright 2004-2019 by OM International
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
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.IO;
using System.Threading;

using Ict.Common;
using Ict.Common.Exceptions;
using Ict.Common.DB.Exceptions;
using Ict.Common.IO;

namespace Ict.Common.DB
{
    /// <summary>
    /// <see cref="IsolationLevel" /> that needs to be enforced when requesting a
    /// DB Transaction with Methods
    /// <see cref="M:DB.TDataBase.GetNewOrExistingTransaction(IsolationLevel, out Boolean)" /> and
    /// <see cref="M:DB.TDataBase.GetNewOrExistingTransaction(IsolationLevel, TEnforceIsolationLevel, out Boolean)" />.
    /// </summary>
    public enum TEnforceIsolationLevel
    {
        /// <summary>
        /// <see cref="IsolationLevel" /> of current Transaction must match the
        /// specified <see cref="IsolationLevel" /> <em>exactly</em>.
        /// </summary>
        eilExact,

        /// <summary>
        /// <see cref="IsolationLevel" /> of current Transaction must match or
        /// exceed the specified <see cref="IsolationLevel" />.
        /// </summary>
        eilMinimum
    }

    /// <summary>
    /// Contains some Constants and a Global Variable for use with Database Access.
    /// </summary>
    public class DBAccess
    {
        /// <summary>DebugLevel for logging of detailed DB Establishing/DB Disconnection information.</summary>
        public const Int32 DB_DEBUGLEVEL_DETAILED_CONN_INFO = 2;

        /// <summary>DebugLevel for logging the SQL code from DB queries.</summary>
        public const Int32 DB_DEBUGLEVEL_QUERY = 3;

        /// <summary>DebugLevel for logging information about DB Transactions.</summary>
        public const Int32 DB_DEBUGLEVEL_TRANSACTION = 10;

        /// <summary>DebugLevel for logging results from DB queries: is 6 (was 4 before).</summary>
        public const Int32 DB_DEBUGLEVEL_RESULT = 6;

        /// <summary>DebugLevel for tracing (very verbose log output): is 10 (was 4 before).</summary>
        public const Int32 DB_DEBUGLEVEL_TRACE = 10;

        /// <summary>DebugLevel for dumping stacktraces when Thread-safe access to the TDataBase Class is requested/released (extremely verbose log output): is 11</summary>
        public const Int32 DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES = 11;

        /// <summary>
        /// we create a new database object if this object is accessed.
        /// this static property is used for backwards compatibility
        /// </summary>
        public static TDataBase GDBAccessObj
        {
            get
            {
                TLogging.LogAtLevel(10, "Deprecated call to GDBAccessObj");
                return Connect("GDBAccessObj");
            }
        }

        /// <summary>
        /// Deprecated: use Transaction.DataBaseObj instead
        /// </summary>
        public static TDataBase GetDBAccessObj(TDBTransaction ATransaction)
        {
            TLogging.Log("GetDBAccessObj is deprecated, use Transaction.DataBaseObj instead");
            return ATransaction != null ? ATransaction.DataBaseObj : GDBAccessObj;
        }

        /// <summary>Returns the type of the RDBMS that is defined in the config file</summary>
        public static TDBType DBType
        {
            get
            {
                if (!TSrvSetting.Initialized)
                {
                    new TSrvSetting();
                }

                return TSrvSetting.RDMBSType;
            }
        }


        /// <summary>
        /// Gets the <see cref="TDataBase"/> instance that gets passed in with Argument <paramref name="ADataBase"/>, or
        /// a new database instance in case <paramref name="ADataBase"/>
        /// is null.
        /// <para>
        /// This is needed for some generated code to be able to work with a (custom) reference to a TDataBase instance, is
        /// helpful for static Methods (which might not have a means to get a reference to a custom <see cref="TDataBase" />
        /// instance easily), and can be neat for other scenarios, too.
        /// </para>
        /// </summary>
        /// <param name="ADataBase">An instantiated <see cref="TDataBase" /> object, or null.</param>
        /// <returns><see cref="TDataBase"/> instance that got passed in with Argument <paramref name="ADataBase"/>, or
        /// a new database instance in case <paramref name="ADataBase"/>
        /// is null.
        /// </returns>
        public static TDataBase GetDBAccessObj(TDataBase ADataBase)
        {
            return ADataBase ?? GDBAccessObj;
        }

        /// <summary>
        /// Creates a new <see cref="TDataBase"/> instance and opens a DB Connection on it.
        /// </summary>
        /// <param name="AConnectionName">Name of the DB Connection (optional). It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).</param>
        /// <param name="ADataBase">if this is not null, no new Connection is opened, but this connection is reused</param>
        /// <returns><see cref="TDataBase"/> instance with an open DB Connection.</returns>
        public static TDataBase Connect(string AConnectionName, TDataBase ADataBase = null)
        {
            if (ADataBase != null)
            {
                return ADataBase;
            }

            TDataBase DBAccessObj = new TDataBase();

            if (!TSrvSetting.Initialized)
            {
                new TSrvSetting();
            }

            DBAccessObj.EstablishDBConnection(TSrvSetting.RDMBSType,
                TSrvSetting.PostgreSQLServer,
                TSrvSetting.PostgreSQLServerPort,
                TSrvSetting.PostgreSQLDatabaseName,
                TSrvSetting.DBUsername,
                TSrvSetting.DBPassword,
                "",
                true,
                AConnectionName);

            return DBAccessObj;
        }

        /// <summary>
        /// Starts a DB Transaction on a new TDataBase instance and executes code that is passed in via a C# Delegate in
        /// <paramref name="AEncapsulatedDBAccessCode"/>. After that the DB Transaction gets committed 
        /// and the DB Connection gets closed.
        /// </summary>
        /// <param name="AIsolationLevel">Desired <see cref="IsolationLevel" />.</param>
        /// <param name="ATransaction">Transaction to be used in the encapsulated action code.</param>
        /// <param name="AContext">Context in which the Method runs (passed as Name to the newly established DB Connection
        /// and as Name to the DB Transaction, too.</param>
        /// <param name="AEncapsulatedDBAccessCode">C# Delegate that encapsulates C# code that should be run inside the
        /// automatic DB Transaction handling scope that this Method provides.</param>
        public static void RunInTransaction(IsolationLevel AIsolationLevel, ref TDBTransaction ATransaction, string AContext,
            Action AEncapsulatedDBAccessCode)
        {
            TDataBase DBConnectionObj = Connect(AContext);

            if (ATransaction == null)
            {
                ATransaction = new TDBTransaction();
            }

            ATransaction.BeginTransaction(DBConnectionObj, AIsolationLevel, AContext);

            try
            {
                DBConnectionObj.AutoTransaction(ref ATransaction, true, AEncapsulatedDBAccessCode);
            }
            finally
            {
                DBConnectionObj.CloseDBConnection();
            }
        }
    }
}
