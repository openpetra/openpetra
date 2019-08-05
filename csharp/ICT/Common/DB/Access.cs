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
    /// Contains some Constants and a Global Variable for use with Database Access.
    /// </summary>
    public class DBAccess
    {
        /// <summary>DebugLevel for logging of detailed DB Establishing/DB Disconnection information.</summary>
        public const Int32 DB_DEBUGLEVEL_DETAILED_CONN_INFO = 2;

        /// <summary>DebugLevel for logging the SQL code from DB queries.</summary>
        public const Int32 DB_DEBUGLEVEL_QUERY = 3;

        /// <summary>DebugLevel for logging information about DB Transactions.</summary>
        public const Int32 DB_DEBUGLEVEL_TRANSACTION = 2;

        /// <summary>DebugLevel for logging information about DB Transactions.</summary>
        public const Int32 DB_DEBUGLEVEL_TRANSACTION_DETAIL = 10;

        /// <summary>DebugLevel for logging results from DB queries: is 6 (was 4 before).</summary>
        public const Int32 DB_DEBUGLEVEL_RESULT = 6;

        /// <summary>DebugLevel for tracing (very verbose log output): is 10 (was 4 before).</summary>
        public const Int32 DB_DEBUGLEVEL_TRACE = 10;

        /// <summary>DebugLevel for dumping stacktraces when Thread-safe access to the TDataBase Class is requested/released (extremely verbose log output): is 11</summary>
        public const Int32 DB_DEBUGLEVEL_COORDINATED_DBACCESS_STACKTRACES = 11;

        /// <summary>Returns the type of the RDBMS that is defined in the config file</summary>
        public static TDBType DBType
        {
            get
            {
                return CommonTypes.ParseDBType(TAppSettingsManager.GetValue("Server.RDBMSType", "postgresql"));
            }
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

            DBAccessObj.EstablishDBConnection(AConnectionName);

            return DBAccessObj;
        }

        /// this is a direct way to create a serializable transaction on an anonymous database connection
        public static void WriteTransaction(ref TDBTransaction ATransaction,
            ref bool ASubmitOK, Action AEncapsulatedDBAccessCode)
        {
            if (!ATransaction.Valid)
            {
                TDataBase db = DBAccess.Connect("DBAccess.WriteTransaction");
                db.WriteTransaction(ref ATransaction, ref ASubmitOK, AEncapsulatedDBAccessCode);
                db.CloseDBConnection();
            }
            else
            {
                ATransaction.DataBaseObj.WriteTransaction(ref ATransaction, ref ASubmitOK, AEncapsulatedDBAccessCode);
            }
        }

        /// this is a direct way to create a read transaction on an anonymous database connection
        public static void ReadTransaction(ref TDBTransaction ATransaction,
            Action AEncapsulatedDBAccessCode)
        {
            if (!ATransaction.Valid)
            {
                TDataBase db = DBAccess.Connect("DBAccess.ReadTransaction");
                db.ReadTransaction(ref ATransaction, AEncapsulatedDBAccessCode);
                db.CloseDBConnection();
            }
            else
            {
                ATransaction.DataBaseObj.ReadTransaction(ref ATransaction, AEncapsulatedDBAccessCode);
            }
        }
    }
}
