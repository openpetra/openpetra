//
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
using System.Data;
using System.Data.Odbc;
using System.Data.Common;
using System.Threading;
using System.Diagnostics;

using Ict.Common.DB.Exceptions;

namespace Ict.Common.DB
{
    /// <summary>
    /// A generic Class for managing all kinds of ADO.NET Database Transactions -
    /// to be used instead of concrete ADO.NET Transaction objects, eg. <see cref="OdbcTransaction" />
    /// or NpgsqlTransaction, etc. Effectively wraps ADO.NET Transaction objects.
    /// </summary>
    public class TDBTransaction : object, IDisposable
    {
        /// <summary>Holds the DbTransaction that we are wrapping inside this class.</summary>
        private DbTransaction FWrappedTransaction;
        private TDataBase FTDataBaseInstanceThatTransactionBelongsTo;
        private Thread FThreadThatTransactionWasStartedOn;
        private AppDomain FAppDomainThatTransactionWasStartedIn;
        private StackTrace FStackTraceAtPointOfTransactionStart;
        private System.Guid FTransactionIdentifier;
        private string FTransactionName;

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies a DB Transaction once it got created.
        /// It is used for internal 'sanity checks'. It also gets logged and hence can aid debugging (also useful for
        /// Unit Testing).</summary>
        public System.Guid TransactionIdentifier
        {
            get
            {
                return FTransactionIdentifier;
            }
        }

        /// <summary>
        /// Instance of <see cref="TDataBase"/> that this Transaction belongs to.
        /// <para>
        /// Can be used e.g. in Static Methods to get the <see cref="TDataBase"/> instance that a <see cref="TDBTransaction"/>
        /// object was instantiated on. This <see cref="TDataBase"/> instance can then be used to execute calls on the proper
        /// <see cref="TDataBase"/> instance without needing to have this instance readily in a given context
        /// (thinking of static Methods in the 'Typed DataStore' here)!
        /// </para>
        /// </summary>
        public TDataBase DataBaseObj
        {
            get
            {
                if (FTDataBaseInstanceThatTransactionBelongsTo == null)
                {
                    throw new Exception("DataBaseObj is null in Transaction");
                }
                return FTDataBaseInstanceThatTransactionBelongsTo;
            }
        }

        /// <summary>
        /// Name of the DB Transaction (optional). It gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        public string TransactionName
        {
            get
            {
                return FTransactionName;
            }
        }

        /// <summary>
        /// An identifier ('Globally Unique Identifier (GUID)') that uniquely identifies the DB Connection that this
        /// DB Transaction is running against. It is used for internal 'sanity checks'. It also gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</summary>
        public System.Guid ConnectionIdentifier
        {
            get
            {
                return FTDataBaseInstanceThatTransactionBelongsTo.ConnectionIdentifier;
            }
        }

        /// <summary>
        /// Database connection to which the Transaction belongs.
        /// </summary>
        public DbConnection Connection
        {
            get
            {
                return FWrappedTransaction.Connection;
            }
        }

        /// <summary>
        /// <see cref="IsolationLevel" /> of the Transaction.
        /// </summary>
        public System.Data.IsolationLevel IsolationLevel
        {
            get
            {
                return FWrappedTransaction.IsolationLevel;
            }
        }

        /// <summary>
        /// DbTransaction that is wrapped in an instance of this class, i.e. which the instance of this class represents.
        /// <para><em><b>WARNING:</b> Do not do anything
        /// with this Object other than inspecting it; the correct
        /// working of Transactions in the <see cref="TDataBase" />
        /// Object relies on the fact that <see cref="TDataBase" /> manages <em>everything</em> about
        /// Transactions!!!</em>
        /// </para>
        /// </summary>
        public DbTransaction WrappedTransaction
        {
            get
            {
                return FWrappedTransaction;
            }
        }

        /// <summary>
        /// True if the Transaction hasn't been Committed or Rolled Back, otherwise false.
        /// </summary>
        public bool Valid
        {
            get
            {
                if (FWrappedTransaction != null)
                {
                    return FWrappedTransaction.Connection != null;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Name of the AppDomain that the DB Transaction was started in. It gets logged and hence can aid debugging
        /// (also useful for Unit Testing).
        /// </summary>
        public string AppDomainNameThatTransactionWasStartedIn
        {
            get
            {
                return FAppDomainThatTransactionWasStartedIn.FriendlyName;
            }
        }

        /// <summary>
        /// AppDomain that the DB Transaction was started in.
        /// </summary>
        internal AppDomain AppDomainThatTransactionWasStartedIn
        {
            get
            {
                return FAppDomainThatTransactionWasStartedIn;
            }
        }

        /// <summary>
        /// Thread that the DB Transaction was started on. It is used for internal 'sanity checks'. It also gets logged and
        /// hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        internal Thread ThreadThatTransactionWasStartedOn
        {
            get
            {
                return FThreadThatTransactionWasStartedOn;
            }
        }

        /// <summary>
        /// StackTrace at the point of the starting of the DB Transaction. This is kept so it can get logged and hence it
        /// can aid debugging.
        /// </summary>
        internal StackTrace StackTraceAtPointOfTransactionStart
        {
            get
            {
                return FStackTraceAtPointOfTransactionStart;
            }
        }

        /// constructor for empty transaction.
        /// it can be filled with calling BeginTransaction.
        public TDBTransaction()
        {
        }

        /// <summary>
        /// Constructor for a <see cref="TDBTransaction" /> Object.
        /// </summary>
        /// <param name="ADataBase">The database connection that this transaction should be created from.</param>
        /// <param name="AIsolationLevel">The isolation level</param>
        /// <param name="ATransactionName">Name of the DB Transaction (optional). It gets logged and hence can aid
        /// debugging (also useful for Unit Testing).</param>
        public TDBTransaction(TDataBase ADataBase,
            IsolationLevel AIsolationLevel,
            string ATransactionName = "")
        {
            BeginTransaction(ADataBase,
                AIsolationLevel,
                ATransactionName);
        }

        /// to begin a transaction on an existing TDBTransaction object.
        /// this is useful for RunInTransaction to have access to the transaction object
        public void BeginTransaction(TDataBase ADataBase,
            IsolationLevel AIsolationLevel,
            string ATransactionName = "")
        {
            FTransactionIdentifier = System.Guid.NewGuid();
            FTransactionName = ATransactionName;

            FWrappedTransaction = ADataBase.BeginDbTransaction(AIsolationLevel);

            FTDataBaseInstanceThatTransactionBelongsTo = ADataBase;
            FAppDomainThatTransactionWasStartedIn = AppDomain.CurrentDomain;
            FThreadThatTransactionWasStartedOn = Thread.CurrentThread;
            FStackTraceAtPointOfTransactionStart = new StackTrace(true);
        }

        /// <summary>
        /// Returns a string containing the TransactionIdentifier. If a TransactionName was assigned when the DB Transaction
        /// was created it is included, too. Gets logged and hence can aid debugging (also useful for Unit Testing).
        /// </summary>
        /// <returns>String containing the TransactionIdentifier. If a TransactionName was assigned when the DB Transaction
        /// was created it is included, too.
        /// </returns>
        public string GetDBTransactionIdentifier()
        {
            return " (Trans.Identifier: " + FTransactionIdentifier + ")" +
                   (FTransactionName != String.Empty ? String.Format(" (Transaction Name: {0})", FTransactionName) : "");
        }

        /// commit the transaction
        public void Commit()
        {
            string TransactionIdentifier = null;
            bool TransactionValid = false;
            IsolationLevel TransactionIsolationLevel = IsolationLevel.Unspecified;

            // Attempt to commit the DB Transaction.
            try
            {
                // 'Sanity Check': Check that TheTransaction hasn't been committed or rolled back yet.
                if (!Valid)
                {
                    var Exc1 =
                        new EDBAttemptingToUseTransactionThatIsInvalidException(
                            "TDataBase.CommitTransaction called on DB Transaction that isn't valid",
                            ThreadingHelper.GetThreadIdentifier(ThreadThatTransactionWasStartedOn),
                            ThreadingHelper.GetCurrentThreadIdentifier());

                    TLogging.Log(Exc1.ToString());

                    throw Exc1;
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                {
                    // Gather information for logging
                    TransactionIdentifier = GetDBTransactionIdentifier();
                    TransactionValid = Valid;
                    TransactionIsolationLevel = IsolationLevel;
                }

                WrappedTransaction.Commit();

                // Commit was OK, now clean up.
                Dispose();

                TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_TRANSACTION, String.Format(
                        "DB Transaction{0} got rolled back.  Before that, its DB Transaction Properties were: Valid: {1}, " +
                        "IsolationLevel: {2} (it got started on Thread {3} in AppDomain '{4}').", TransactionIdentifier,
                        TransactionValid, TransactionIsolationLevel, ThreadThatTransactionWasStartedOn,
                        AppDomainNameThatTransactionWasStartedIn)
                    );
            }
            catch (Exception Exc)
            {
                // This catch block will handle any errors that may have occurred
                // on the server that would cause the rollback to fail, such as
                // a closed connection.
                //
                // MSDN says: "Try/Catch exception handling should always be used when rolling back a
                // transaction. A Rollback generates an InvalidOperationException if the connection is
                // terminated or if the transaction has already been rolled back on the server."
                TLogging.Log("Exception while attempting Transaction rollback: " + Exc.ToString());
            }
        }

        /// roll back the transaction
        public void Rollback()
        {
            string TransactionIdentifier = null;
            bool TransactionValid = false;
            IsolationLevel TransactionIsolationLevel = IsolationLevel.Unspecified;

            // Attempt to roll back the DB Transaction.
            try
            {
                // 'Sanity Check': Check that TheTransaction hasn't been committed or rolled back yet.
                if (!Valid)
                {
                    var Exc1 =
                        new EDBAttemptingToUseTransactionThatIsInvalidException(
                            "TDataBase.RollbackTransaction called on DB Transaction that isn't valid",
                            ThreadingHelper.GetThreadIdentifier(ThreadThatTransactionWasStartedOn),
                            ThreadingHelper.GetCurrentThreadIdentifier());

                    TLogging.Log(Exc1.ToString());

                    throw Exc1;
                }

                if (TLogging.DL >= DBAccess.DB_DEBUGLEVEL_TRANSACTION)
                {
                    // Gather information for logging
                    TransactionIdentifier = GetDBTransactionIdentifier();
                    TransactionValid = Valid;
                    TransactionIsolationLevel = IsolationLevel;
                }

                WrappedTransaction.Rollback();

                // Rollback was OK, now clean up.
                Dispose();

                TLogging.LogAtLevel(DBAccess.DB_DEBUGLEVEL_TRANSACTION, String.Format(
                        "DB Transaction{0} got rolled back.  Before that, its DB Transaction Properties were: Valid: {1}, " +
                        "IsolationLevel: {2} (it got started on Thread {3} in AppDomain '{4}').", TransactionIdentifier,
                        TransactionValid, TransactionIsolationLevel, ThreadThatTransactionWasStartedOn,
                        AppDomainNameThatTransactionWasStartedIn)
                    );
            }
            catch (Exception Exc)
            {
                // This catch block will handle any errors that may have occurred
                // on the server that would cause the rollback to fail, such as
                // a closed connection.
                //
                // MSDN says: "Try/Catch exception handling should always be used when rolling back a
                // transaction. A Rollback generates an InvalidOperationException if the connection is
                // terminated or if the transaction has already been rolled back on the server."
                TLogging.Log("Exception while attempting Transaction rollback: " + Exc.ToString());
            }
        }

        #region Dispose pattern

        /// <summary>
        /// Releases all resources used by the <see cref="TDBTransaction" />
        /// (these are really only resources held by the <see cref="WrappedTransaction" />
        /// and <see cref="Connection" />).
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the  <see cref="TDBTransaction" />
        /// (these are really only resources held by the <see cref="WrappedTransaction" />
        /// and optionally releases the managed resources of that object.
        /// </summary>
        /// <param name="ADisposing">True to release both managed and unmanaged resources;
        /// false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool ADisposing)
        {
            if (FTDataBaseInstanceThatTransactionBelongsTo != null)
            {
                FTDataBaseInstanceThatTransactionBelongsTo.ClearTransaction(this);
            }

            if (ADisposing)
            {
                if (FWrappedTransaction != null)
                {
                    FWrappedTransaction.Dispose();
                    FWrappedTransaction = null;
                }
            }
        }

        #endregion
    }
}
